using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemWolfPointReward : MonoBehaviour
{
	public UIBox alreadyBox;
	public SpriteText rewardText;

	public void SetData(bool isFirstPass)
	{
			rewardText.Text = GameUtility.GetUIString("UIPnlWolfInfo_FirstReward_Label");
			alreadyBox.gameObject.SetActive(!isFirstPass);
	}
}
