public class Video
{   
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Extension { get; set; }
    public string? NativeVaultPath { get; set; }
    public string? NativePhysicalVaultPath { get; set; }
    public string? NativeSizeReadable { get; set; }
    public string? ProcessedVaultPath { get; set; }
    public string? ProcessedPhysicalVaultPath { get; set; }
    public string? ProcessedSizeReadable { get; set; }
    public string? ThumbnailVaultPath { get; set; }
    public string? ThumbnailPhysicalVaultPath { get; set; }
    public string? Command { get; set; }
    public DateTime CreatedOn { get; set; }
}

public static class VideoExtensions
{
    public static string ReadableProcessedVideoSize(this Video video)
    {
        long byteCount = new FileInfo(video.ProcessedPhysicalVaultPath!).Length;

        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        if (byteCount == 0)
            return "0" + suf[0];

        long bytes = Math.Abs(byteCount);
        
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }
    
    public static string ReadableNativeVideoSize(this Video video)
    {
        long byteCount = new FileInfo(video.NativePhysicalVaultPath!).Length;

        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        if (byteCount == 0)
            return "0" + suf[0];

        long bytes = Math.Abs(byteCount);
        
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }

    public static VideoApiModel ToApiModel(this Video video)
    {
        return new VideoApiModel(video.Id,
                                 video.Name,
                                 video.Extension,
                                 video.NativeVaultPath,
                                 video.NativePhysicalVaultPath,
                                 video.NativeSizeReadable,
                                 video.ProcessedVaultPath,
                                 video.ProcessedPhysicalVaultPath,
                                 video.ProcessedSizeReadable,
                                 video.ThumbnailVaultPath,
                                 video.ThumbnailPhysicalVaultPath,
                                 video.Command,
                                 video.CreatedOn);
    }
}