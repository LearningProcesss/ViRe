using Hangfire;
using Microsoft.AspNetCore.SignalR;

public delegate Task<Video> CreateNewVideoCapability(CreateNewVideoCommand command);

public class CreateNewVideoUseCase
{
    private readonly RiduttoreDbContext _dbContext;
    private readonly NewGuid _newGuid;
    private readonly IHubContext<RiduttoreHub> _hub;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public CreateNewVideoUseCase(RiduttoreDbContext dbContext,
                                 NewGuid newGuid,
                                 IHubContext<RiduttoreHub> hub,
                                 IBackgroundJobClient backgroundJobClient)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _newGuid = newGuid ?? throw new ArgumentNullException(nameof(newGuid));
        _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        _backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
    }

    public async Task<Video> HandleAsync(CreateNewVideoCommand command)
    {
        Guid newEntityId = _newGuid();

        string normalizedFileName = command.File.FileName.Replace(" ", "_");

        string physicalNewDirPath = Path.Combine("vault",
                                                 newEntityId.ToString());

        Directory.CreateDirectory(physicalNewDirPath);

        string physicalFilePath = Path.Combine(physicalNewDirPath, normalizedFileName);

        string physicalProcessedFilePath = Path.Combine(physicalNewDirPath,
                                                        $"_{normalizedFileName}");

        string physicalThumbnailFilePath = Path.Combine(physicalNewDirPath,
                                                        $"{Path.GetFileNameWithoutExtension(normalizedFileName)}.jpg");

        using var stream = new FileStream(physicalFilePath, FileMode.Create);

        await command.File.CopyToAsync(stream);

        Video newEntity = new()
        {
            Id = newEntityId,
            Name = Path.GetFileNameWithoutExtension(normalizedFileName),
            Extension = Path.GetExtension(normalizedFileName),
            NativeVaultPath = $"/vault/{newEntityId}/{normalizedFileName}",
            NativePhysicalVaultPath = physicalFilePath,
            CreatedOn = DateTime.Now
            // ProcessedVaultPath = $"/vault/{newEntityId}/_{normalizedFileName}",
            // ProcessedPhysicalVaultPath = physicalProcessedFilePath,
            // ThumbnailVaultPath = $"/vault/{newEntityId}/{Path.GetFileNameWithoutExtension(normalizedFileName)}.jpg",
            // ThumbnailPhysicalVaultPath = physicalThumbnailFilePath
        };

        newEntity.NativeSizeReadable = newEntity.ReadableNativeVideoSize();

        await _dbContext.Videos.AddAsync(newEntity, default);

        await _dbContext.SaveChangesAsync(default);

        await _hub.Clients.All.SendAsync(RiduttoreHubEvents.VideoUploaded, new VideoUploadedEvent(_newGuid(), newEntity.ToApiModel()));

        string jobId = _backgroundJobClient.Enqueue<ITakeThumbnailJob>(job => job.Process(newEntity.Id));

        _backgroundJobClient.ContinueJobWith<IProcessVideoJob>(jobId, job => job.Process(newEntity.Id));

        return newEntity;
    }
}

public record CreateNewVideoCommand(IFormFile File);