using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIElemShopMysteryItem1 : MonoBehaviour
{
	public UIBox bg;
	public UIBox yiGoumai;
	public UIButton buyBtn;
	public UIElemAssetIcon goodIcon;
	public SpriteText goodName;
	public SpriteText goodNumber;
	public UIElemAssetIcon costIcon;
	public SpriteText costNumber;

	public SpriteText staticLabel;

	public void InitData()
	{
		bg.Hide(true);
		yiGoumai.Hide(true);
		buyBtn.Hide(true);

		goodName.Text = string.Empty;
		goodNumber.Text = string.Empty;
		costNumber.Text = string.Empty;

		goodIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
		costIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
		goodIcon.Hide(true);
		costIcon.Hide(true);

		staticLabel.Hide(true);
	}

	public void SetData(com.kodgames.corgi.protocol.MysteryerGood goodInfo)
	{
		bg.Hide(false);
		staticLabel.Hide(false);

		//Set Btn.
		SelectBuyBtnToggle(goodInfo.buyOrNot);
		buyBtn.Data = goodInfo.goodsId;
		(buyBtn as AutoSpriteControlBase).IndexData = goodInfo.place;

		//Set Icon.
		goodIcon.Hide(false);
		costIcon.Hide(false);
		goodIcon.SetData(goodInfo.reward.id);
		costIcon.SetData(goodInfo.cost.id);

		//Set Label.
		goodName.Text = ItemInfoUtility.GetAssetName(goodInfo.reward.id);
		goodNumber.Text = GameUtility.FormatUIString("UIPnlShopMystery_GoodCount", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(), goodInfo.reward.amount);
		costNumber.Hide(false);
		costNumber.Text = goodInfo.cost.count.ToString();
	}

	public void SelectBuyBtnToggle(bool toggle)
	{
		buyBtn.Hide(toggle);
		yiGoumai.Hide(!toggle);
	}
}
