using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemQualityBtn : MonoBehaviour
{
	public AutoSpriteControlBase qualityBtn;
	public bool horizontalLayout = false;

	private float originalWidth = 0;
	private float originalHeight = 0;

	private void Awake()
	{
		originalWidth = qualityBtn.width;
		originalHeight = qualityBtn.height;
	}

	public void SetQuality(int assetId, bool horizontal)
	{
		horizontalLayout = horizontal;
		SetQuality(assetId);
	}

	public void SetQuality(int assetId)
	{
		// Get quality.
		int quality = ItemInfoUtility.GetAssetQualityLevel(assetId);

		SetQualityByQulityLv(quality, true);
	}

	public void SetQualityByQulityLv(int quality, bool copyIcon)
	{
		if (quality == 0)
			Hide(true);
		else
			Hide(false);

		// Set quality button icon if copyIcon is true.
		if (copyIcon)
		{
			UIElemTemplate uiElemTemplate = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIElemTemplate>();
			UIElemAvatarQualityTemplate avatarQualityTemplate = uiElemTemplate.avatarQualityTemplate;

			UIUtility.CopyIcon(qualityBtn, avatarQualityTemplate.avatarQualityBtn);
		}

		qualityBtn.keepBorderAspectRate = true;
		qualityBtn.xTextureTile = horizontalLayout;
		qualityBtn.yTextureTile = !horizontalLayout;

		if (horizontalLayout)
			qualityBtn.SetSize(originalHeight * quality, originalHeight);
		else
			qualityBtn.SetSize(originalWidth, originalWidth * quality);
	}

	public void Hide(bool hide)
	{
		qualityBtn.Hide(hide);
	}
}
