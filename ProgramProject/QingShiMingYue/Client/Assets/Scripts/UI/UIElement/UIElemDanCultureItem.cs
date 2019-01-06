using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemDanCultureItem : MonoBehaviour
{
	public UIElemAssetIcon danIcon;
	public SpriteText numText;

	public void SetData(Cost cost, int multipleNum)
	{
		danIcon.SetData(cost.id);
		danIcon.GetComponent<UIButton>().Data = cost;
		int count=ItemInfoUtility.GetGameItemCount(cost.id);
		if(count >= cost.count * multipleNum){
			numText.Text = string.Format("{0}{1}/{2}",  GameDefines.textColorBtnYellow , count, cost.count * multipleNum);
			danIcon.assetNameLabel.SetColor(GameDefines.textColorBtnYellow);
		}else{
			numText.Text = string.Format("{0}{1}/{2}",  GameDefines.textColorRed, count, cost.count * multipleNum);
			danIcon.assetNameLabel.SetColor(GameDefines.textColorRed);
		}
		
	}

}
