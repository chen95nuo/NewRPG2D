using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlCampaignActivityScene : UIPnlCampaignBase
{
	public List<AutoSpriteControlBase> campaignOpenTime;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		// Init Campaign Data.
		var campaignSceneData = CampaignSceneData.Instance;
		var secretZones = ConfigDatabase.DefaultCfg.CampaignConfig.secretZones;
		campaignSceneData.InitData(this, "OnClickZone");

		for (int index = 0; index < secretZones.Count; index++)
		{
			// Set 3D Button's Invoke Method.
			CampaignSceneData.Instance.campaignInfos[index].SetCampaignData(secretZones[index]);

			// Attach 3D Button ' Name Obj.
			ObjectUtility.AttachToParentAndResetLocalTrans(campaignSceneData.campaignInfos[index].campaignNameRoot, campaignNameBtns[index].gameObject);

			// Set the Campaign Star.
			SetCampaignStar(ConfigDatabase.DefaultCfg.CampaignConfig.secretZones[index].zoneId);

			// Hide OpenTime.
			campaignOpenTime[index].Hide(true);
		}

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatar));
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlHandBook)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlHandBook));

		CampaignSceneData.Instance.MainCamera.enabled = true;

		if (userDatas != null && userDatas.Length > 0)
		{
			var record = userDatas[0] as UIPnlCampaignBase.CampaignRecrod;

			CampaignSceneData.Instance.ScrollView(
							(int)userDatas[1],
							0,
							false,
							null);

			if (record != null && record.ShowDungeonMapUI())
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneMid), record);
		}

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

	private void Update()
	{
		for (int index = 0; index < campaignNameBtns.Count; index++)
		{
			CampaignConfig.Zone zone = ConfigDatabase.DefaultCfg.CampaignConfig.secretZones[index];

			bool hasLock = !CampaignData.IsZoneTimeOpen(zone.zoneId);

			// Set the Lock.
			CampaignSceneData.Instance.SetCampaignLocks(index, hasLock, false);

			if (hasLock && campaignOpenTime[index].IsHidden())
			{
				campaignOpenTime[index].Hide(false);

				foreach (var matrial in campaignOpenTime[index].spriteText.renderer.materials)
				{
					matrial.shader = Shader.Find("Kod/Environment/Alpha-VertexColor-Unlit");
				}

				campaignOpenTime[index].Text = zone.openDesc;
			}
			else if (hasLock == false && campaignOpenTime[index].IsHidden() == false)
				campaignOpenTime[index].Hide(true);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickZone(UIButton3D btn)
	{
		var zoneCfg = btn.data as CampaignConfig.Zone;

		var zoneErrorMsg = CampaignData.CheckZoneEnterErrorMsg(zoneCfg.zoneId);

		if (string.IsNullOrEmpty(zoneErrorMsg))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneMid), new UIPnlCampaignBase.CampaignRecrod(zoneCfg.zoneId, 0f, false));
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), zoneErrorMsg);
	}
}
