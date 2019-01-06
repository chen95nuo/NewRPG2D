using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIPnlCampaignCityPlayerInfo : UIPnlPlayerInfoBase
{
	public UIElemAssetIcon travelShopGuidBtn;

	private float delta = 0f;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// Set Travel Shop Icon.
		travelShopGuidBtn.SetData(ConfigDatabase.DefaultCfg.CampaignConfig.travelTraderIconOpenId);

		ShowTravelShopButton(true);

		return true;
	}

	private void Update()
	{
		delta += Time.deltaTime;
		if (delta > 1.0f)
		{
			delta = 0f;
			RefreshView();
		}
	}

	public void ShowTravelShopButton(bool show)
	{
		bool canShow = show && SysGameStateMachine.Instance.CurrentState is GameState_Dungeon;
		travelShopGuidBtn.Hide(!canShow);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTravelGuidBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTravelShopGuid));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPlayerDetailClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPlayerAttrTip));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDailyCampaign(UIButton btn)
	{
		CampaignData.EnterActivityDungeon();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickNormalCampaign(UIButton btn)
	{
		CampaignData.EnterNormalDungeon();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignSceneMid))
		{
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlCampaignSceneMid));
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlCampaignSceneBottom));

			// Check For UnLock Campaign Zone , Just For Normal Campaign.
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlCampaignScene)))
				SysUIEnv.Instance.GetUIModule<UIPnlCampaignScene>().CheckCampaignUnLock();
		}
		else
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGoCentralCity(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>();
	}

	//铜币
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGameMoney(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.GameMoney).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//元宝
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRealMoney(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.RealMoney).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//体力
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickStamina(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.Stamina).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}


}