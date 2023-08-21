using Microsoft.EntityFrameworkCore;

public delegate Task<IEnumerable<Video>> GetVideosCapability(GetVideosCommand command);

public class GetVideosUseCase
{
    private readonly RiduttoreDbContext _dbContext;

    public GetVideosUseCase(RiduttoreDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<Video>> HandleAsync(GetVideosCommand command)
    {
        return await _dbContext.Videos.ToListAsync();
    }
}