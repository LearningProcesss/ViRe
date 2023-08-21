using Refit;

public interface IVideoApiClient
{
    [Get("/api/Videos?page={page}")]
    Task<IEnumerable<VideoApiModel>> GetVideos(int page);

    [Get("/api/Videos/{id}")]
    Task<VideoApiModel> GetVideoById(string id);

    [Multipart]
    [Post("/api/Videos")]
    Task<VideoApiModel> Upload([AliasAs("files")] StreamPart streamPart);
}