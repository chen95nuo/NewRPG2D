using UnityEngine;
using System.Collections;

public class UIElemWolfGuideItem : MonoBehaviour
{

	public SpriteText guideTitleLabel;
	public UIButton guideInfoBtn;

	public void SetData(ClientServerCommon.MainType guideItem)
	{
		guideInfoBtn.Data = guideItem;
		guideTitleLabel.Text = guideItem.name;
	}
}
