using Microsoft.Win32;

namespace Gidsiks.TrialReseterService.Resetters;

public class BeyondCompareTrialResetter
{ 
	private static TimeSpan _rescanKeyTimeSpan = TimeSpan.FromDays(29); // 30 day trial time

	private List<RegistryKey> _licenceRegKeys = [];
	private DateTimeOffset _lastRescanKeyTimeUtc;

	public BeyondCompareTrialResetter()
	{
		RescanRegKey();
	}

	private void RescanRegKey()
	{
		_lastRescanKeyTimeUtc = DateTimeOffset.UtcNow;

		_licenceRegKeys.Clear();

		foreach (var sid in Registry.Users.GetSubKeyNames())
		{
			var key = Registry.Users.OpenSubKey($"{sid}\\Software\\Scooter Software\\Beyond Compare 5", true);
			if (key is not null)
			{
				_licenceRegKeys.Add(key);
			}
		}
	}

	public void Update()
	{
		if (DateTimeOffset.UtcNow.Subtract(_lastRescanKeyTimeUtc) >= _rescanKeyTimeSpan)
		{
			RescanRegKey();
		}

		foreach (var key in _licenceRegKeys)
		{
			if (key.GetValueNames().Contains("CacheID"))
				key.DeleteValue("CacheID");
		}
	}
}
