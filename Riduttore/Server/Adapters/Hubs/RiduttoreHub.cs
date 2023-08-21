using Microsoft.AspNetCore.SignalR;

public class RiduttoreHub : Hub
{
    public RiduttoreHub()
    {

    }

    public async Task VideoUploadedMessage(VideoUploadedEvent @event)
    {
        await Clients.All.SendAsync(RiduttoreHubEvents.VideoUploaded, @event);
    }

    public async Task ThumbnailCreatedMessage(ThumbnailCreatedEvent @event)
    {
        await Clients.All.SendAsync(RiduttoreHubEvents.ThumbnailCreated, @event);
    }

    public async Task VideoConvertedMessage(VideoConvertedEvent @event)
    {
        await Clients.All.SendAsync(RiduttoreHubEvents.VideoConverted, @event);
    }
}