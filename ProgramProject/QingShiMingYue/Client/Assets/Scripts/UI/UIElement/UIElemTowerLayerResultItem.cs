using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTowerLayerResultItem : MonoBehaviour
{
	public SpriteText layerLabel;
	public UIButton replayBtn;
	public UIBox SuccessBg;
	public UIBox FailedBg;
	public AutoSpriteControlBase bgBox;

	public void SetData(int startLayer, int endLayer, bool isWin)
	{
		layerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_ToLayer_Label"), startLayer.ToString(), endLayer.ToString());
		if(isWin)
		{
			replayBtn.gameObject.SetActive(false);
			FailedBg.Hide(true);
			SuccessBg.Hide(false);
		}
		else
		{
			replayBtn.gameObject.SetActive(true);
			FailedBg.Hide(false);
			SuccessBg.Hide(true);
		}
	}
}
