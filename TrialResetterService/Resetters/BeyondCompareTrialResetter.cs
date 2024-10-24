using Microsoft.Win32;

namespace Gidsiks.TrialReseterService.Resetters;

public class BeyondCompareTrialResetter
{ 
	private static TimeSpan _rescanKeyTimeSpan = TimeSpan.FromDays(29); // 30 day trial time
	
	private RegistryKey? _licenceRegKey;
	private DateTimeOffset _lastRescanKeyTimeUtc;

	public BeyondCompareTrialResetter()
	{
		RescanRegKey();
	}

	private void RescanRegKey()
	{
		_lastRescanKeyTimeUtc = DateTimeOffset.UtcNow;
		_licenceRegKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
		_licenceRegKey = _licenceRegKey.OpenSubKey("Software\\Scooter Software\\Beyond Compare 5", true);
	}

	public void Update()
	{
		if (_licenceRegKey is null)
		{
			if (DateTimeOffset.UtcNow.Subtract(_lastRescanKeyTimeUtc) < _rescanKeyTimeSpan) return;
			RescanRegKey();
			return;
		}
		
		if (_licenceRegKey.GetValueNames().Contains("CacheID"))
			_licenceRegKey.DeleteValue("CacheID");
	}
}
