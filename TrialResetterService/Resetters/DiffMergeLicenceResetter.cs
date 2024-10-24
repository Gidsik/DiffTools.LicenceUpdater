using Microsoft.Win32;

namespace Gidsiks.TrialReseterService.Resetters;

public class DiffMergeLicenceResetter
{
	private static TimeSpan _rescanKeyTimeSpan = TimeSpan.FromMinutes(59); // 1 hour popup

	private RegistryKey? _licenceRegKey;
	private DateTimeOffset _lastRescanKeyTimeUtc;

	public DiffMergeLicenceResetter()
	{
		RescanRegKey();
	}

	private void RescanRegKey()
	{
		_lastRescanKeyTimeUtc = DateTimeOffset.UtcNow;
		_licenceRegKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
		_licenceRegKey = _licenceRegKey.OpenSubKey("Software\\SourceGear\\SourceGear DiffMerge\\License", true);
	}

	public void Update()
	{
		if (_licenceRegKey is null)
		{
			if (DateTimeOffset.UtcNow.Subtract(_lastRescanKeyTimeUtc) < _rescanKeyTimeSpan) return;
			RescanRegKey();
			return;
		}

		var newLicenceTime = DateTimeOffset.Now.ToUnixTimeSeconds();
		_licenceRegKey.SetValue("Check", unchecked((int)newLicenceTime), RegistryValueKind.DWord);
	}
}
