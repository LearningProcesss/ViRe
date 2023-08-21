public partial class RiduttoreHubEvents
{
    public const string VideoUploaded = "VideoUploadedMessage";
    public const string ThumbnailCreated = "ThumbnailCreatedMessage";
    public const string VideoConverted = "VideoConvertedMessage";
}

public sealed record VideoUploadedEvent(Guid EventId, VideoApiModel Video);
public sealed record ThumbnailCreatedEvent(Guid EventId, VideoApiModel Video);
public sealed record VideoConvertedEvent(Guid EventId, VideoApiModel Video);