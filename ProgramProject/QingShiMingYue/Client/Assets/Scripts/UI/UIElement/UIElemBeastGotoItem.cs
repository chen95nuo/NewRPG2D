using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemBeastGotoItem : MonoBehaviour
{
	public AutoSpriteControlBase getwayBg;
	public SpriteText getWayDesc;
	public SpriteText getWayTipLabel;
	public UIButton gotoBtn;

	public void SetData(GetWay getway)
	{
		gotoBtn.Hide(false);

		if (getway.type == _UIType.UI_ActivityDungeon || getway.type == _UIType.UI_Dungeon)
		{
			var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(getway.data);
			getWayDesc.Text = GameUtility.FormatUIString("UIDlgItemGetWay_GetWay_Label", GameDefines.textColorBtnYellow.ToString(), _UIType.GetDisplayNameByType(getway.type, ConfigDatabase.DefaultCfg),
																						GameDefines.textColorWhite.ToString(), GameUtility.FormatUIString(
																						"UIDlgItemGetWay_GoTo_Label",
																						ItemInfoUtility.GetAssetName(dungeonCfg.ZoneId),
																						_DungeonDifficulity.GetDisplayNameByType(dungeonCfg.DungeonDifficulty, ConfigDatabase.DefaultCfg),
																						ItemInfoUtility.GetAssetName(dungeonCfg.dungeonId)));
			getWayTipLabel.Text = "";
		}
		else
		{
			getWayTipLabel.Text = getway.desc;
			getWayDesc.Text = "";
		}
		
		gotoBtn.Data = getway;
	}

	public void SetData(int resourceId)
	{
		gotoBtn.Hide(true);
		getWayDesc.Text = "";

		switch (IDSeg.ToAssetType(resourceId))
		{
			case IDSeg._AssetType.Avatar:
				getWayTipLabel.Text = GameUtility.GetUIString("UIDlgItemGetWay_GoTo_AvatarTips");
				break;

			case IDSeg._AssetType.Equipment:
				getWayTipLabel.Text = GameUtility.GetUIString("UIDlgItemGetWay_GoTo_EquipTips");
				break;

			case IDSeg._AssetType.CombatTurn:
				getWayTipLabel.Text = GameUtility.GetUIString("UIDlgItemGetWay_GoTo_SkillTips");
				break;

			case IDSeg._AssetType.Beast:
				getWayTipLabel.Text = GameUtility.GetUIString("UIDlgItemGetWay_GoTo_BeastTips");
				break;
		}
	}
}
