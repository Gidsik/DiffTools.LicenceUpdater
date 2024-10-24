using Gidsiks.TrialReseterService;
using Gidsiks.TrialReseterService.Resetters;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
	options.ServiceName = "DiffTools LicenceUpdater";
});

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

builder.Services.AddSingleton<DiffMergeLicenceResetter>();
builder.Services.AddSingleton<BeyondCompareTrialResetter>();
builder.Services.AddHostedService<TrialResetterService>();

var host = builder.Build();
host.Run();

