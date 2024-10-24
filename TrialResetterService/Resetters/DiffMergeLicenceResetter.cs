using Microsoft.Win32;

namespace Gidsiks.TrialReseterService.Resetters;

public class DiffMergeLicenceResetter
{
	private static TimeSpan _rescanKeyTimeSpan = TimeSpan.FromMinutes(59); // 1 hour popup

	private List<RegistryKey> _licenceRegKeys = [];
	private DateTimeOffset _lastRescanKeyTimeUtc;

	public DiffMergeLicenceResetter()
	{
		RescanRegKey();
	}

	private void RescanRegKey()
	{
		_lastRescanKeyTimeUtc = DateTimeOffset.UtcNow;

		_licenceRegKeys.Clear();

		foreach (var sid in Registry.Users.GetSubKeyNames())
		{
			var key = Registry.Users.OpenSubKey($"{sid}\\Software\\SourceGear\\SourceGear DiffMerge\\License", true);
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

		var newLicenceTime = DateTimeOffset.Now.ToUnixTimeSeconds();
		foreach (var key in _licenceRegKeys)
		{
			key.SetValue("Check", unchecked((int)newLicenceTime), RegistryValueKind.DWord);
		}
	}
}
