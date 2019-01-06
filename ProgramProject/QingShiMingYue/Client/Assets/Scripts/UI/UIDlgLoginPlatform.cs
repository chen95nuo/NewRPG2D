using System;
using System.Collections.Generic;

public class UIDlgLoginPlatform : UIModule
{

	public void ShowLoginView()
	{
		var platform = Platform.Instance as PlatformWithLoginView;
		platform.ShowPlatformLoginView(true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnLoginClick(UIButton btn)
	{
		ShowLoginView();
	}
}