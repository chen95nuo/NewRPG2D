using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemGuideItem : MonoBehaviour
{
	public SpriteText guideTitleLabel;
	public UIButton guideInfoBtn;

	public void SetData(ClientServerCommon.GuideConfig.MainType guideItem)
	{
		guideTitleLabel.Text = guideItem.name;
		guideInfoBtn.Data = guideItem;
	}

}
