public static class AddVideoFeatureExtensions
{
    public static IServiceCollection AddVideoFeature(this IServiceCollection services)
    {
        services.AddScoped<IProcessVideoJob, ProcessVideoJob>();
        services.AddScoped<ITakeThumbnailJob, TakeThumbnailJob>();
        
        services.AddScoped<CreateNewVideoUseCase>();
        services.AddScoped<CreateNewVideoCapability>(sp => sp.GetRequiredService<CreateNewVideoUseCase>().HandleAsync);

        services.AddScoped<GetVideosUseCase>();
        services.AddScoped<GetVideosCapability>(sp => sp.GetRequiredService<GetVideosUseCase>().HandleAsync);

        services.AddScoped<GetVideoByIdUseCase>();
        services.AddScoped<GetVideoByIdCapability>(sp => sp.GetRequiredService<GetVideoByIdUseCase>().HandleAsync);

        services.AddScoped<NewGuid>(sp => ApplicationNewGuid.NewGuid);

        return services;
    }
}