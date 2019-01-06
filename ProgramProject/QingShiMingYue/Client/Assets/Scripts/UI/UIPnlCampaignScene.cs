using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlCampaignScene : UIPnlCampaignBase
{
	private CampaignData campaignData;
	private UIPnlCampaignBase.CampaignRecrod record;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		// Init Campaign Data.
		campaignData = CampaignData.GetCampaignData();
		CampaignSceneData.Instance.InitData(this, "OnClickZone");

		for (int index = 0; index < CampaignSceneData.Instance.campaignInfos.Count; index++)
		{
			// Set the Campaign Scene object's invoke method and data.
			CampaignSceneData.Instance.campaignInfos[index].SetCampaignData(ConfigDatabase.DefaultCfg.CampaignConfig.zones[index]);

			// Set the Campaign Name Button.
			ObjectUtility.AttachToParentAndResetLocalTrans(CampaignSceneData.Instance.campaignInfos[index].campaignNameRoot, campaignNameBtns[index].gameObject);

			// Set the Campaign Star.
			SetCampaignStar(ConfigDatabase.DefaultCfg.CampaignConfig.zones[index].zoneId);
		}

		return true;
	}

	// 0: zoneId,1: 3d position 2: map position
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatar));
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlHandBook)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlHandBook));

		CampaignSceneData.Instance.MainCamera.enabled = true;

		// Set the 3dButtons of the campaign
		SetCampaignZoneLockView();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		CampaignSceneData.Instance.MainCamera.enabled = false;
	}

	public override void Overlay()
	{
		base.Overlay();

		CampaignSceneData.Instance.MainCamera.enabled = false;
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		CampaignSceneData.Instance.MainCamera.enabled = true;
	}

	public void FillData(params object[] userDatas)
	{
		// Set CampaignRecord.
		if (userDatas != null && userDatas.Length > 0)
			record = userDatas[0] as CampaignRecrod;

		// If not back from battle , Set the record data by last battle data.
		if (record == null && SysLocalDataBase.Inst.LocalPlayer.CampaignData.InterruptCampaign)
			record = new CampaignRecrod(SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleZoneId, -1f, false);

		if (CheckCampaignUnLock() == false)
		{
			// Not Has zone should play unlock animation , scroll the ball position.
			// If has interrupt show the mapInfo after scroll the ball.
			if (record != null && record.ShowDungeonMapUI())
			{
				var zoneRecrod = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(record.jumpZoneId);

				if (zoneRecrod.Status == _ZoneStatus.PlotDialogue)
				{
					var tempRecord = new CampaignRecrod();
					tempRecord.ShallowCopy(record);

					RequestMgr.Inst.Request(new SetZoneStatusReq(record.jumpZoneId, _ZoneStatus.ZoneProceed, () =>
					{
						SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneMid), tempRecord);
					}));
				}
				else
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneMid), record);
			}
		}

		record = null;
	}

	public bool CheckCampaignUnLock()
	{
		campaignData = CampaignData.GetCampaignData();
		if (campaignData.shouldUnlockIndex < 0)
			return false;

		SetZoneStatusReq.OnResponseSuccessDel del = null;
		int changedZoneState = _ZoneStatus.PlotDialogue;
		CampaignConfig.Zone zone = CampaignSceneData.Instance.campaignInfos[campaignData.shouldUnlockIndex].campaignInvokeButtons[0].Data as CampaignConfig.Zone;

		if (record != null && (record.jumpDungeonId != 0 || record.dungeonDiff != _DungeonDifficulity.Unknow))
		{
			var tempRecord = new CampaignRecrod();
			tempRecord.ShallowCopy(record);

			changedZoneState = _ZoneStatus.ZoneProceed;

			del = () =>
			{
				int campaignIndex = GetCampaignButtonIndexByZoneID(zone.zoneId);
				CampaignSceneData.Instance.SetCampaignLocks(campaignIndex, false, false);

				// 显示跳转到的副本的章节
				CampaignSceneData.Instance.ScrollView(
							CampaignData.GetZoneIndexInCfg(ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(tempRecord.jumpDungeonId).ZoneId),
							0,
							false,
							null);

				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneMid), tempRecord);
			};
		}
		// Has zone should play unlock animation , send the request for change the zone state.
		RequestMgr.Inst.Request(new SetZoneStatusReq(zone.zoneId, changedZoneState, del));

		return true;
	}

	// Reset the 3dButton's data and lockButton's active.
	private void SetCampaignZoneLockView()
	{
		for (int index = 0; index < CampaignSceneData.Instance.campaignInfos.Count; index++)
		{
			bool hasLock = campaignData.zoneLockStates[index];
			CampaignSceneData.Instance.SetCampaignLocks(index, hasLock, false);
			CampaignSceneData.Instance.SetCampaigNewParticle(index);
		}
	}

	private int GetCampaignButtonIndexByZoneID(int zoneID)
	{
		for (int index = 0; index < CampaignSceneData.Instance.campaignInfos.Count; index++)
		{
			if ((CampaignSceneData.Instance.campaignInfos[index].Data as CampaignConfig.Zone).zoneId == zoneID)
				return index;
		}

		return -1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickZone(UIButton3D btn)
	{
		CampaignConfig.Zone zone = btn.data as CampaignConfig.Zone;

		var zoneErrorMsg = CampaignData.CheckZoneEnterErrorMsg(zone.zoneId);
		if (!string.IsNullOrEmpty(zoneErrorMsg))
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), zoneErrorMsg);
			return;
		}

		var currentRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(zone.zoneId);
		switch (currentRecord.Status)
		{
			case _ZoneStatus.UnlockAnimation:
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UICampaign_Close"));
				break;

			case _ZoneStatus.PlotDialogue:

				if (zone.dialogueId == IDSeg.InvalidId)
				{
					RequestMgr.Inst.Request(new SetZoneStatusReq(zone.zoneId, _ZoneStatus.ZoneProceed));
				}
				else
				{
					SysUIEnv.Instance.GetUIModule<UITipAdviser>().ShowDialogue(zone.dialogueId,
						() =>
						{
							RequestMgr.Inst.Request(new SetZoneStatusReq(zone.zoneId, _ZoneStatus.ZoneProceed));
						});
				}
				break;

			default:
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneMid), new UIPnlCampaignBase.CampaignRecrod(zone.zoneId, -1f, false));
				break;
		}
	}

	public void OnResponseSetZoneStatus(int zoneId, int preZoneStatus)
	{
		int campaignIndex = GetCampaignButtonIndexByZoneID(zoneId);

		if (campaignIndex < 0)
			return;

		switch (preZoneStatus)
		{
			case _ZoneStatus.UnlockAnimation:

				CampaignSceneData.Instance.LockScroll();

				CampaignSceneData.Instance.ScrollView(
					campaignIndex,
					campaignIndex - 1 < 0 ? 0 : campaignIndex - 1,
					true,
					null);

				CampaignSceneData.Instance.SetCampaignLocks(campaignIndex, false, true, () => { CampaignSceneData.Instance.UnLockScroll(); });

				// Reset UnLockIndex .
				campaignData.shouldUnlockIndex = -1;
				break;

			case _ZoneStatus.PlotDialogue:
				CampaignSceneData.Instance.SetCampaigNewParticle(campaignIndex);

				CampaignRecrod record = new CampaignRecrod(zoneId, -1f, false);
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneMid), record);
				break;
		}
	}
}
