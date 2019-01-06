using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlCampaignSceneMid : UIModule
{
	public UIScrollList scrollList;
	public UIElementDungeonItem dungeonItem;
	public SpriteText zondName;

	public UIBox avatarLineUpNotify;
	public UIBox handBookNotify;

	private System.Func<bool> onShowFunc;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// 注册阵容绿点的数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Avatar, UpdateAvatarLineUpNotify);
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Equipment, UpdateAvatarLineUpNotify);
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.CombatTurn, UpdateAvatarLineUpNotify);

		// 注册图鉴绿点的数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Item, UpdateHandBookNotify);

		return true;
	}

	public override void Dispose()
	{
		base.Dispose();

		// 取消注册阵容绿点的数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Avatar, UpdateAvatarLineUpNotify);
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Equipment, UpdateAvatarLineUpNotify);
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.CombatTurn, UpdateAvatarLineUpNotify);

		// 取消注册活动绿点的数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Item, UpdateHandBookNotify);
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		UIPnlCampaignBase.CampaignRecrod config = userDatas[0] as UIPnlCampaignBase.CampaignRecrod;


		zondName.Text = GameUtility.FormatUIString(
								SysGameStateMachine.Instance.CurrentState is GameState_Dungeon ? "UIPnlCampaignSceneMid_DungeonName" : "UIPnlCampaignSceneMid_ActivityDungeonName",
								GameDefines.textColorBtnYellow,
								GameDefines.textColorWhite,
								ItemInfoUtility.GetAssetName(config.jumpZoneId));

		// Set the Current Panel 's view
		dungeonItem.SetData(config, scrollList);

		// Set TravelShopGuid Button.
		SysUIEnv.Instance.GetUIModule<UIPnlCampaignCityPlayerInfo>().ShowTravelShopButton(false);

		// Show Del.
		if (onShowFunc != null)
			onShowFunc();

		UpdateHandBookNotify();
		UpdateAvatarLineUpNotify();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		// Reset the scrollList's position
		scrollList.ScrollPosition = 0f;

		// Clear Data.
		dungeonItem.ClearData();

		this.onShowFunc = null;

		// Set TravelShopGuid Button.
		SysUIEnv.Instance.GetUIModule<UIPnlCampaignCityPlayerInfo>().ShowTravelShopButton(true);
	}

	public override void Overlay()
	{
		base.Overlay();

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlCampaignSceneBottom)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlCampaignSceneBottom));

		// Set Camera.
		CampaignSceneData.Instance.MainCamera.enabled = false;
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		// Show Campaign Bottom info.
		dungeonItem.ShowCampaignBottomInfo();

		// Set Camera.
		CampaignSceneData.Instance.MainCamera.enabled = true;

		UpdateHandBookNotify();

		UpdateAvatarLineUpNotify();

		dungeonItem.OnResponseGetDungeonRewardSuccess();
	}

	public void SetOnShowDel(System.Func<bool> func)
	{
		this.onShowFunc = func;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatar(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatar);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickHandBook(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlHandBook);
	}

	public void UpdateHandBookNotify()
	{
		if (this.IsShown && !this.IsOverlayed)
			handBookNotify.Hide(!ItemInfoUtility.HaveMergeIllustration());
	}

	public void UpdateAvatarLineUpNotify()
	{
		if (this.IsShown && !this.IsOverlayed)
			avatarLineUpNotify.Hide(!UIPnlAvatar.CheckUIAvatarNotify());
	}
}