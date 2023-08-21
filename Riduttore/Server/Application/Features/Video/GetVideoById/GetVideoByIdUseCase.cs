using Microsoft.EntityFrameworkCore;

public delegate Task<Video?> GetVideoByIdCapability(GetVideoByIdCommand command);

public class GetVideoByIdUseCase
{
    private readonly RiduttoreDbContext _dbContext;

    public GetVideoByIdUseCase(RiduttoreDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Video?> HandleAsync(GetVideoByIdCommand command)
    {
        return await _dbContext.Videos.FirstOrDefaultAsync(video => video.Id == command.Id);
    }
}