using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlMainScene : UIModule
{
	public enum HighlightState
	{
		none = 0,
		beaconFire = 1,
		tavern = 1 << 1,
		retainer = 1 << 2,
		arena = 1 << 3,
		tower = 1 << 5,
		guild = 1 << 6,
		rank = 1 << 7,
	}

	private HighlightState highlightStates = HighlightState.none;
	private Dictionary<HighlightState, GameObject> highlightPfxs = new Dictionary<HighlightState, GameObject>();

	private Transform cameraRootTransfrom;
	private Transform CameraRootTransfrom
	{
		get
		{
			if (cameraRootTransfrom == null)
				cameraRootTransfrom = MainSceneData.Instance.cameraRoot.transform;

			return cameraRootTransfrom;
		}
	}

	private GameObject campaignGuid;

	public AutoSpriteControlBase tavernNameBtn;
	public AutoSpriteControlBase retainerNameBtn;
	public AutoSpriteControlBase arenaNameBtn;
	public AutoSpriteControlBase towerNameBtn;
	public AutoSpriteControlBase beaconfireNameBtn;
	public AutoSpriteControlBase guildNameBtn;
	public AutoSpriteControlBase tavernFreeBtn;
	public AutoSpriteControlBase adventureNameBtn;
	public AutoSpriteControlBase friendCombatNameBtn;
	public AutoSpriteControlBase illusionNameBtn;
	public AutoSpriteControlBase coreNameBtn;
	public AutoSpriteControlBase organNameBtn;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		tavernFreeBtn.Hide(true);

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.tavernNameButton.gameObject, tavernNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.tavernButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickTavernButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.retainerNameButton.gameObject, retainerNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.retainerButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickRetainerButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.arenaNameButton.gameObject, arenaNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.arenaButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickArenaButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.towerNameButton.gameObject, towerNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.towerButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickTowerButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.beaconfireNameButton.gameObject, beaconfireNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.beaconfireButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickBeaconfireButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.guildNameButton.gameObject, guildNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.guildButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickGuildButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.adventureButton.gameObject, adventureNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.adventureButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickAdventureButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.friendCombatSystemButton.gameObject, friendCombatNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.friendCombatSystemButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickFriendCombatButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.illusionButon.gameObject, illusionNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.illusionSceneButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickIllusionButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.coreButton.gameObject, coreNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.coreSceneButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickCoreButton";
		}

		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(MainSceneData.Instance.organButton.gameObject, organNameBtn.gameObject);
		foreach (var btn in MainSceneData.Instance.organSceneButtons)
		{
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "OnClickOrganButton";
		}

		HighlightBuildings(HighlightState.tavern, true);

		// 烽火狼烟关闭
		beaconfireNameBtn.Hide(false);

		//从千机楼退出时清空千机楼数据
		SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData = new KodGames.ClientClass.MelaleucaFloorData();

		return true;
	}

	public override void Dispose()
	{
		Platform.Instance.ShowToolBar(false);
		base.Dispose();
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		KodGames.Camera.main.enabled = true;

		MainSceneData.Instance.CameraCtrl.AddInputDelegate();

		ProcessBuildingHighlight();

		Platform.Instance.ShowToolBar(true);
		return true;
	}

	public override void OnHide()
	{
		KodGames.Camera.main.enabled = false;

		MainSceneData.Instance.CameraCtrl.RemoveInputDelegate();

		ShowCampaignGuidView(false);

		Platform.Instance.ShowToolBar(false);

		if (highlightPfxs != null)
		{
			foreach (var pfxObj in highlightPfxs.Values)
				Destroy(pfxObj);
			highlightPfxs.Clear();
		}
		base.OnHide();
	}

	public override void Overlay()
	{
		base.Overlay();

		MainSceneData.Instance.CameraCtrl.RemoveInputDelegate();

		KodGames.Camera.main.enabled = false;

		Platform.Instance.ShowToolBar(false);
	}

	public override void RemoveOverlay()
	{
		KodGames.Camera.main.enabled = true;

		MainSceneData.Instance.CameraCtrl.AddInputDelegate();

		Platform.Instance.ShowToolBar(true);

		base.RemoveOverlay();
	}

	private void Update()
	{
	}

	private void ShowCampaignGuidView(bool show)
	{
		int guidPlayerLevel = ConfigDatabase.DefaultCfg.TutorialConfig.campaignGuid.playerLevel;

		if (show && guidPlayerLevel >= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
		{
			if (campaignGuid == null)
			{
				campaignGuid = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.campaignGuidParticle));
				ObjectUtility.AttachToParentAndKeepLocalTrans(MainSceneData.Instance.beaconfireButtons[0].gameObject, campaignGuid);
			}
		}
		else
		{
			if (campaignGuid != null)
				GameObject.Destroy(campaignGuid);
		}
	}

	public void HighlightBuildings(HighlightState buildingsToHighlight, bool highlight)
	{
		if (highlight)
			highlightStates |= buildingsToHighlight;
		else
			highlightStates &= ~buildingsToHighlight;
		ProcessBuildingHighlight();
	}

	void ProcessBuildingHighlight()
	{
		HighlightOneBuilding(HighlightState.arena, (highlightStates & HighlightState.arena) != 0);
		HighlightOneBuilding(HighlightState.beaconFire, (highlightStates & HighlightState.beaconFire) != 0);
		HighlightOneBuilding(HighlightState.tower, (highlightStates & HighlightState.tower) != 0);
		HighlightOneBuilding(HighlightState.guild, (highlightStates & HighlightState.guild) != 0);
		HighlightOneBuilding(HighlightState.tavern, (highlightStates & HighlightState.tavern) != 0);
		HighlightOneBuilding(HighlightState.retainer, (highlightStates & HighlightState.retainer) != 0);
	}

	void HighlightOneBuilding(HighlightState buildingToHighlight, bool highlight)
	{
		if (highlight)
		{
			if (highlightPfxs.ContainsKey(buildingToHighlight))
			{
				if (highlightPfxs[buildingToHighlight] != null)
					return;
				else
					highlightPfxs.Remove(buildingToHighlight);
			}
			GameObject building = null;
			switch (buildingToHighlight)
			{
				case HighlightState.arena:
					if (MainSceneData.Instance.arenaButtons.Length > 0)
						building = MainSceneData.Instance.arenaButtons[0].gameObject;
					break;
				case HighlightState.beaconFire:
					if (MainSceneData.Instance.beaconfireButtons.Length > 0)
						building = MainSceneData.Instance.beaconfireButtons[0].gameObject;
					break;
				case HighlightState.tavern:
					if (MainSceneData.Instance.tavernButtons.Length > 0)
						building = MainSceneData.Instance.tavernButtons[0].gameObject;
					break;
				case HighlightState.retainer:
					if (MainSceneData.Instance.retainerButtons.Length > 0)
						building = MainSceneData.Instance.retainerButtons[0].gameObject;
					break;
				case HighlightState.guild:
					if (MainSceneData.Instance.guildButtons.Length > 0)
						building = MainSceneData.Instance.guildButtons[0].gameObject;
					break;
				case HighlightState.tower:
					if (MainSceneData.Instance.towerButtons.Length > 0)
						building = MainSceneData.Instance.towerButtons[0].gameObject;
					break;
			}
			if (building == null)
				return;
			GameObject fx = SysModuleManager.Instance.GetSysModule<ResourceManager>().InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, GameDefines.centralCityBuildingHighlightFx));
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(building, fx);
			if (buildingToHighlight == HighlightState.tower)
			{
				fx.transform.localPosition = new Vector3(0, 0, 25);
				fx.transform.localScale = new Vector3(5, 5, 11);
			}
			else if (buildingToHighlight == HighlightState.arena)
			{
				fx.transform.localPosition = new Vector3(-2.25f, -4.77f, -12f);
			}
			else if (buildingToHighlight == HighlightState.tavern)
			{
				fx.transform.localPosition = new Vector3(0, 0, -4);
			}
			else if (buildingToHighlight == HighlightState.guild)
			{
				fx.transform.localPosition = new Vector3(0, 0, 1.5f);
			}
			else
			{
				fx.transform.localScale = Vector3.one * 4;
			}
			highlightPfxs.Add(buildingToHighlight, fx);
		}
		else
		{
			if (highlightPfxs.ContainsKey(buildingToHighlight))
			{
				if (highlightPfxs[buildingToHighlight] != null)
					Destroy(highlightPfxs[buildingToHighlight].gameObject);
				highlightPfxs.Remove(buildingToHighlight);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRetainerButton(UIButton3D btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatarDiner);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTavernButton(UIButton3D btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlShopWine);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickArenaButton(UIButton3D btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlArena);
	}

	//烽火狼烟
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBeaconfireButton(UIButton3D btn)
	{
		GameUtility.JumpUIPanel(_UIType.UI_WolfSmoke);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGuildButton(UIButton3D btn)
	{
		GameUtility.JumpUIPanel(_UIType.UI_Guild);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTowerButton(UIButton3D btn)
	{
		GameUtility.JumpUIPanel(_UIType.UI_Tower);
	}

	//奇遇
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAdventureButton(UIButton3D btn)
	{
		//GameUtility.JumpUIPanel(_UIType.UIPnlAdventureMain);
		GameUtility.JumpUIPanel(_UIType.UI_Adventrue);
	}

	//好友战斗系统
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFriendCombatButton(UIButton3D btn)
	{
		GameUtility.JumpUIPanel(_UIType.UIPnlFriendStart);
		//RequestMgr.Inst.Request(new CombatFriendCampaignReq(SysLocalDataBase.Inst.LocalPlayer.PlayerId, SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions[0]));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIllusionButton(UIButton3D btn)
	{
		GameUtility.JumpUIPanel(_UIType.UIPnlIllusion);
	}

	//炼丹
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCoreButton(UIButton3D btn)
	{
		GameUtility.JumpUIPanel(_UIType.UIPnlDanMain);
	}

	//机关兽工坊
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOrganButton(UIButton3D btn)
	{
		GameUtility.JumpUIPanel(_UIType.UIPnlOrgansBeastTab);
	}	
}
