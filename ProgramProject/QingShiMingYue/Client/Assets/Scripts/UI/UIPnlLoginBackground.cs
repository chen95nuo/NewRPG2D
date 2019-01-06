    //#define ENABLE_LOGINBACKGROUNDICONCHANGE

using UnityEngine;
using System.Collections;
using System;
using ClientServerCommon;
using KodGames.ExternalCall;
using System.IO;

public class UIPnlLoginBackground : UIModule
{
	public UIProgressBar loadingBar;
	public SpriteText loadingMessage;
	public Animation brightnesAnim;
	public SpriteText versionLabel;

	public AutoSpriteControlBase backBg;

#if ENABLE_LOGINBACKGROUNDICONCHANGE
	private string iconPath = "ExcludeCard/LOGINBG.png";
#endif

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SetBackGroundIcon();

		versionLabel.Text = GameUtility.FormatUIString("VersionNum", BundlePlugin.GetMainBundleVersion(), KodConfigPlugin.GetRevision());
		loadingBar.Hide(true);
		loadingBar.Value = 0f;
		loadingBar.Text = "";
		loadingMessage.Text = "";

		PlayAnimation(false);

		return true;
	}

	public void SetBackGroundIcon()
	{
#if ENABLE_LOGINBACKGROUNDICONCHANGE
		StopCoroutine("LoadBackGround");
		StartCoroutine("LoadBackGround");
#endif
	}

#if ENABLE_LOGINBACKGROUNDICONCHANGE
	public override void OnHide()
	{
		base.OnHide();
		StopCoroutine("LoadBackGround");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator LoadBackGround()
	{
		yield return null;

		// Hide Start.
		backBg.Hide(true);

		WWW www = null;
		var fileInLocal = new FileInfo(ResourceManager.Instance.GetLocalFilePath(KodGames.PathUtility.GetAssetNameKeepInBuddle(iconPath)));

		if (fileInLocal.Exists)
			www = new WWW(ResourceManager.Instance.GetLocalFileUrl(KodGames.PathUtility.GetAssetNameKeepInBuddle(iconPath)));
		else
		{
			var filePath = Path.Combine(Application.streamingAssetsPath, iconPath);
			if (filePath.Contains("://"))
				www = new WWW(filePath);
			else
				www = new WWW("file://" + filePath);
		}

		yield return www;

		if (www != null && string.IsNullOrEmpty(www.error))
		{
			var cardMat = new Material(Shader.Find("Kod/UI/Transparent Color"));
			cardMat.mainTexture = www.textureNonReadable;

			//Set Card UV.
			UIUtility.SetIconInUVUnit(backBg, cardMat, new Rect(0, 0, 1, 1));
			cardMat = null;
		}

		www = null;

		backBg.Hide(false);
	}
#endif

	public void PlayAnimation(bool play)
	{
		if (brightnesAnim == null)
			return;

		if (play)
		{
			if (!brightnesAnim.isPlaying)
				brightnesAnim.Play();
		}
		else
		{
			if (brightnesAnim.isPlaying)
				brightnesAnim.Stop();
		}
	}

	public void ShowLoadingBar(bool show)
	{
		loadingBar.Hide(!show);
		loadingMessage.Hide(!show);
	}

	public void Step(float progress, string message)
	{
		loadingBar.Value = progress;
		loadingMessage.Text = message;
	}
}
