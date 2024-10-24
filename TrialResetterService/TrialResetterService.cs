using Gidsiks.TrialReseterService.Resetters;

namespace Gidsiks.TrialReseterService;


public class TrialResetterService : BackgroundService
{
	private readonly ILogger<TrialResetterService> _logger;

	private readonly DiffMergeLicenceResetter _diffMergeLU;
	private readonly BeyondCompareTrialResetter _BCompareLU;

	private readonly TimeSpan _updatePeriod;

	public TrialResetterService(ILogger<TrialResetterService> logger,
						   DiffMergeLicenceResetter diffMergeLU,
						   BeyondCompareTrialResetter bCompareLU)
	{
		_logger = logger;
		_diffMergeLU=diffMergeLU;
		_BCompareLU=bCompareLU;

		_updatePeriod = TimeSpan.FromMinutes(30);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				_diffMergeLU.Update();
				_BCompareLU.Update();

				await Task.Delay(_updatePeriod, stoppingToken);
			}
		}
		catch (OperationCanceledException) { }
		catch (Exception ex) 
		{
			_logger.LogError(ex, "{Message}", ex.Message);
			Environment.Exit(1);
		}
	}
}
