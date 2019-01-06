using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDanMaterial : MonoBehaviour
{
	public UIElemAssetIcon danIcons;
	public SpriteText danAttrLabel;	

	public void SetData(KodGames.ClientClass.Consumable comsumable)
	{
		danIcons.SetData(comsumable.Id, comsumable.Amount);
		danIcons.Data = comsumable;		
		danAttrLabel.Text = GameUtility.FormatUIString("UIPnlDanMaterial_Label_Desc",GameDefines.textColorOrange, GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetDesc(comsumable.Id));
	}
}
