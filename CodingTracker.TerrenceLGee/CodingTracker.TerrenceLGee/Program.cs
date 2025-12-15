using CodingTracker.TerrenceLGee.Data;
using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.Data.Repositories;
using CodingTracker.TerrenceLGee.Services;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using CodingTracker.TerrenceLGee.TrackerUi;
using CodingTracker.TerrenceLGee.TrackerUi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

try
{
    LoggingSetup();
    Startup();
}
catch (Exception ex)
{
    Console.WriteLine($"There was an unexpected error starting this program: {ex.Message}");
}

return;

void Startup()
{
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionStringValue = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Unable to retrieve connection string");

    var connectionString = new ConnectionString(connectionStringValue);

    var services = new ServiceCollection()
        .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
        .AddSingleton(connectionString)
        .AddScoped<IDatabaseInitializer, DatabaseInitializer>()
        .AddScoped<ICoderRepository, CoderRepository>()
        .AddScoped<ICodingGoalRepository, CodingGoalRepository>()
        .AddScoped<ICodingSessionRepository, CodingSessionRepository>()
        .AddScoped<ICodingReportRepository, CodingReportRepository>()
        .AddScoped<ICoderService, CoderService>()
        .AddScoped<ICodingGoalService, CodingGoalService>()
        .AddScoped<ICodingSessionService, CodingSessionService>()
        .AddScoped<ICodingReportService, CodingReportService>()
        .AddScoped<IViewInfoUi, ViewInfoUi>()
        .AddScoped<ICoderUi, CoderUi>()
        .AddScoped<ICodingGoalUi, CodingGoalUi>()
        .AddScoped<ICodingSessionUi, CodingSessionUi>()
        .AddScoped<ICodingReportUi, CodingReportUi>()
        .AddScoped<ICodingReportGenerator, CodingReportGenerator>();

    var serviceProvider = services.BuildServiceProvider();

    var database = serviceProvider.GetRequiredService<IDatabaseInitializer>();

    database.InitializeDatabase();

    var coderUi = serviceProvider.GetRequiredService<ICoderUi>();
    var codingGoalUi = serviceProvider.GetRequiredService<ICodingGoalUi>();
    var codingSessionUi = serviceProvider.GetRequiredService<ICodingSessionUi>();
    var viewInfoUi = serviceProvider.GetRequiredService<IViewInfoUi>();
    var codingReportUi = serviceProvider.GetRequiredService<ICodingReportUi>();

    var app = new CodingTrackerApp(
        coderUi,
        codingGoalUi,
        codingSessionUi,
        codingReportUi,
        viewInfoUi);
    
    app.Run();
}

void LoggingSetup()
{
    var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    Directory.CreateDirectory(loggingDirectory);
    var filePath = Path.Combine(loggingDirectory, "cdtkr-.txt");
    var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File(
            path: filePath,
            rollingInterval: RollingInterval.Day,
            outputTemplate: outputTemplate)
        .CreateLogger();
}