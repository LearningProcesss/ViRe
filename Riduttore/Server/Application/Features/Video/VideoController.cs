using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly ILogger<VideosController> _logger;
    private readonly CreateNewVideoCapability _createNewVideoCapability;
    private readonly GetVideosCapability _getVideosCapability;
    private readonly GetVideoByIdCapability _getVideoByIdCapability;

    public VideosController(ILogger<VideosController> logger,
                           CreateNewVideoCapability createNewVideoCapability,
                           GetVideosCapability getVideosCapability,
                           GetVideoByIdCapability getVideoByIdCapability)
    {
        _logger = logger;
        _createNewVideoCapability = createNewVideoCapability ?? throw new ArgumentNullException(nameof(createNewVideoCapability));
        _getVideosCapability = getVideosCapability ?? throw new ArgumentNullException(nameof(getVideosCapability));
        _getVideoByIdCapability = getVideoByIdCapability ?? throw new ArgumentNullException(nameof(getVideoByIdCapability));
    }

    [HttpPost]
    [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Upload([FromForm(Name = "files")] IFormFile files)
    {
        Video video = await _createNewVideoCapability(new CreateNewVideoCommand(files));

        return CreatedAtAction(nameof(GetById), new { id = video.Id }, video.ToApiModel());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Video? video = await _getVideoByIdCapability(new GetVideoByIdCommand(id));
        
        return video is null ? NotFound() : Ok(video);
    }

    [HttpGet()]
    public async Task<IActionResult> GetVideos([FromQuery] int? page,
                                               [FromQuery] int? perPage,
                                               [FromQuery] string? sortBy,
                                               [FromQuery] string? sortDirection)
    {
        IEnumerable<Video> videos = await _getVideosCapability(new GetVideosCommand());
        
        return Ok(videos.Select(entity => entity.ToApiModel()));
    }
}
