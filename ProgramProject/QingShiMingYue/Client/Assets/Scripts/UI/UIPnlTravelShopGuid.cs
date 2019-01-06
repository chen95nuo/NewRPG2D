using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTravelShopGuid : UIModule
{
	public List<UIButton> tabButtons;
	public UIScrollList travelGuidList;
	public GameObjectPool travelGuidPool;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		for (int index = 0; index < tabButtons.Count; index++)
			tabButtons[index].Data = _DungeonDifficulity.Common + index;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		ChangeTab((int)tabButtons[0].Data);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		travelGuidList.ClearList(false);
		travelGuidList.ScrollPosition = 0f;
	}

	private void ChangeTab(int dungeonDiff)
	{
		// Set TabButton State.
		for (int index = 0; index < tabButtons.Count; index++)
			tabButtons[index].controlIsEnabled = dungeonDiff != (int)tabButtons[index].Data;

		// Clear List 
		ClearData();

		StartCoroutine("FillList", dungeonDiff);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(int dungeonDiff)
	{
		yield return null;

		foreach (var travel in ConfigDatabase.DefaultCfg.CampaignConfig.travelTraders)
		{
			var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(travel.dungeonId);

			if (dungeonCfg.DungeonDifficulty != dungeonDiff)
				continue;

			var item = travelGuidPool.AllocateItem().GetComponent<UIElemTravelShopGuid>();
			item.SetData(travel);
			travelGuidList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDiffBtn(UIButton btn)
	{
		ChangeTab((int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoBtn(UIButton btn)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById((int)btn.Data);

		var errorMsg = CampaignData.CheckDungeonEnterErrorMsg(dungeonCfg.dungeonId, false);
		if (string.IsNullOrEmpty(errorMsg))
		{
			HideSelf();

			var zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(dungeonCfg.ZoneId);
			SetZoneStatusReq.OnResponseSuccessDel del = () =>
				{
					CampaignSceneData.Instance.ScrollView(
							CampaignData.GetZoneIndexInCfg(dungeonCfg.ZoneId),
							CampaignSceneData.Instance.CurrentIndex,
							false,
							() =>
							{
								SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().SetOnShowDel(() =>
								{
									if (SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonTravelDataByDungeonId(dungeonCfg.dungeonId) == null)
										RequestMgr.Inst.Request(new QueryTravelReq(dungeonCfg.dungeonId));
									else
										SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDungeonTravelShop), dungeonCfg.dungeonId);
									return true;
								});

								SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneMid), new UIPnlCampaignBase.CampaignRecrod(dungeonCfg.ZoneId, IDSeg.InvalidId, dungeonCfg.DungeonDifficulty, -1f, false));
							});
				};

			if (zoneRecord.Status < _ZoneStatus.ZoneProceed)
				RequestMgr.Inst.Request(new SetZoneStatusReq(dungeonCfg.ZoneId, _ZoneStatus.ZoneProceed, del));
			else
				del();
		}
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errorMsg);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}