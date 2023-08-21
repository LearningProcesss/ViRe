using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public interface ITakeThumbnailJob
{
    void Process(Guid videoId);
}

public interface IProcessVideoJob
{
    void Process(Guid videoId);
}

public interface IDeleteProcessedVideoJob
{
    void Process(string inputFilePath);
}

public sealed class TakeThumbnailJob : ITakeThumbnailJob
{
    private readonly RiduttoreDbContext _dbContext;
    private readonly IHubContext<RiduttoreHub> _hub;

    public TakeThumbnailJob(RiduttoreDbContext dbContext,
                            IHubContext<RiduttoreHub> hub)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _hub = hub ?? throw new ArgumentNullException(nameof(hub));
    }

    public void Process(Guid videoId)
    {
        Video video = _dbContext.Videos.Single(entity => entity.Id == videoId);

        var videoDump = ObjectDumper.Dump(video);

        Console.WriteLine($"TakeThumbnailJob pre {videoDump}");

        string physicalNewDirPath = Path.Combine("vault",
                                                 video.Id.ToString());

        string thumbnailPhysicalVaultPath = Path.Combine(physicalNewDirPath,
                                                        $"{video.Name}.jpg");

        string command = $"ffmpeg -ss 00:00:00 -i {video.NativePhysicalVaultPath} -frames:v 1 -q:v 2 {thumbnailPhysicalVaultPath}".Replace("\"", "\\\"");

        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        process.WaitForExit();

        video.ThumbnailPhysicalVaultPath = thumbnailPhysicalVaultPath;

        video.ThumbnailVaultPath = $"/vault/{video.Id}/{video.Name}.jpg";

        // Task.Run(async () => await _dbContext.SaveChangesAsync());

        _dbContext.SaveChanges();

        videoDump = ObjectDumper.Dump(video);

        Console.WriteLine($"TakeThumbnailJob post {videoDump}");

        Task.Run(async () => await _hub.Clients.All.SendAsync(RiduttoreHubEvents.ThumbnailCreated.ToString(),
                                                               new ThumbnailCreatedEvent(Guid.NewGuid(), video.ToApiModel())));
    }
}

public sealed class ProcessVideoJob : IProcessVideoJob
{
    private readonly RiduttoreDbContext _dbContext;
    private readonly IHubContext<RiduttoreHub> _hub;

    public ProcessVideoJob(RiduttoreDbContext dbContext,
                           IHubContext<RiduttoreHub> hub)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _hub = hub ?? throw new ArgumentNullException(nameof(hub));
    }

    public void Process(Guid videoId)
    {
        Video video = _dbContext.Videos.Single(entity => entity.Id == videoId);

        var videoDump = ObjectDumper.Dump(video);

        Console.WriteLine($"ProcessVideoJob pre {videoDump}");

        string physicalNewDirPath = Path.Combine("vault",
                                                 video.Id.ToString());

        string processedPhysicalVaultPath = Path.Combine(physicalNewDirPath,
                                                        $"_{video.Name}{video.Extension}");

        string command = $"ffmpeg -i {video.NativePhysicalVaultPath} -vcodec libx264 -crf 28 {processedPhysicalVaultPath}".Replace("\"", "\\\"");

        Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        process.WaitForExit();

        video.ProcessedPhysicalVaultPath = processedPhysicalVaultPath;

        video.ProcessedVaultPath = $"/vault/{video.Id}/_{video.Name}{video.Extension}";

        video.ProcessedSizeReadable = video.ReadableProcessedVideoSize();

        video.Command = command;

        _dbContext.SaveChanges();

        videoDump = ObjectDumper.Dump(video);

        Console.WriteLine($"ProcessVideoJob post {videoDump}");

        Task.Run(async () => await _hub.Clients.All.SendAsync(RiduttoreHubEvents.VideoConverted,
                                                               new VideoConvertedEvent(Guid.NewGuid(), video.ToApiModel())));
    }
}