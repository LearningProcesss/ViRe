using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(configuration =>
{
    // configuration.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddHangfire(configuration =>
{
    configuration.UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseColouredConsoleLogProvider()
                 .UseSQLiteStorage(builder.Configuration.GetConnectionString("HangfireDefaultConnection"));
});

builder.Services.AddHangfireServer();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddDbContext<RiduttoreDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("RiduttoreSqliteConnection")));

builder.Services.AddVideoFeature();

builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

var app = builder.Build();

app.InitializeVault()
   .MigrateDatabase();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "vault")), RequestPath = "/vault"
});
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//             Path.Combine(Directory.GetCurrentDirectory(), "vault")),
//     RequestPath = "/vault"
// });

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<RiduttoreHub>("/riduttorehub");
app.MapFallbackToFile("index.html");

app.Run();
