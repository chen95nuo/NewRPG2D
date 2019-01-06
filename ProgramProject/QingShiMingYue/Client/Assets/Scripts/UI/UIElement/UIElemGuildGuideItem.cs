using UnityEngine;
using System.Collections;

public class UIElemGuildGuideItem : MonoBehaviour
{
	public SpriteText guideTitleLabel;
	public UIButton guideInfoBtn;

	public void SetData(ClientServerCommon.MainType guideItem)
	{
		guideInfoBtn.Data = guideItem;
		guideTitleLabel.Text = guideItem.name;
	}
}
