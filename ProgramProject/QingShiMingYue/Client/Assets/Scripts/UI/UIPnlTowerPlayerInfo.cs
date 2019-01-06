using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;
using System.Collections;

public class UIPnlTowerPlayerInfo : UIModule
{

	public UIButton sweepButton;
	public UIBox lockbg;

	public SpriteText points;
	public SpriteText challengeTimes;

	public SpriteText staminaLabel;
	public SpriteText gameMoneyLabel;
	public SpriteText realMoneyLabel;

	public SpriteText doublePointLabel;
	public SpriteText fourPointLabel;
	public SpriteText eightPointLabel;

	public static int sweepBattleType;
	public static int sweepBattleCount;

	private int layerLevel = 0;
	public static int battleType = 0;
	private int challengeCount = 0;

	//扫荡功能开启层数限制
	private int limitLayer = 0;
	public int LimitLayer { get { return limitLayer; } }

	//是否在千机楼标识
	private bool isInTower = false;
	public bool IsInTower { get { return isInTower; } }

	//是否为扫荡战斗标识
	private bool isSweepBattle= false;
	public bool IsSweepBattle 
	{
		get { return isSweepBattle; }
		set { isSweepBattle = value; }
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		isInTower = false;
		int positionId = SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId;

		//设置角色模型
		var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(positionId);
		position.AvatarLocations.Sort((a1, a2) =>
			{
				return a1.ShowLocationId - a2.ShowLocationId;
			});

		if (position.AvatarLocations.Count <= 0)
		{
			Debug.LogError("No Avatar On Position.");
			return true;
		}

		var avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(position.AvatarLocations[0].Guid);

		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);
		int assetID = avatarCfg.GetAvatarAssetId(avatar.BreakthoughtLevel);

		TowerPlayerRole.Create(assetID, true, false, TowerSceneData.Instance.initPathNode, TowerSceneData.Instance.initDoorNode);
		SetPointRewardLabel();

		//扫荡功能开启层数为不同战斗方式最小层
		List<MelaleucaFloorConfig.Challenge> challenges = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.Challenges;

		for (int i = 0; i < challenges.Count; i++)
		{
			if (limitLayer == 0)
				limitLayer = challenges[i].OpenByLayer;
			if (limitLayer > challenges[i].OpenByLayer && challenges[i].OpenByLayer != 0)
				limitLayer = challenges[i].OpenByLayer;
		}

		RequestMgr.Inst.Request(new QueryMelaleucaFloorPlayerInfoReq());

		return true;
	}

	public void sweepOpen()
	{
		StartCoroutine(UnLockPlay(0.5f));
	}

	public void TowerConnect()
	{
		// Show message dlg and reconnect
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
		okCallback.Callback = (userData) =>
		{
			// Current state may be GameState_Upgrading, force enter it.
			SysGameStateMachine.Instance.EnterState<GameState_CentralCity>();
			return true;
		};

		string towerMessage = GameUtility.GetUIString("UIDlgMessage_TowerConnec");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(
			GameUtility.GetUIString("UIDlgMessage_TowerConnecTitle"),
			towerMessage,
			false,
			null,
			okCallback);

		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}

	public void OnPlayerInfoSuccess(int gotoLayer, bool isConnect, bool isSweepLock)
	{
		PlayTowerMusic();

		if (gotoLayer > 0)
			TowerSceneData.Instance.currentPlayerRole.MoveToNextDoorNode(gotoLayer);

		//重新连接时不刷新楼层面板
		if (!isConnect)
			TowerSceneData.Instance.ReFillFloorData(gotoLayer);

		gameMoneyLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.GameMoney);

		realMoneyLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.RealMoney);

		staminaLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.TrialStamp);

		points.Text = SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint.ToString();

		challengeCount = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.MelaleucaFloorChallengeCount) - SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.ChallengeFailsCount;

		challengeTimes.Text = challengeCount.ToString();

		if (isConnect)
			TowerConnect();

		if (isSweepLock)
		{
			sweepOpen();
		}
		else
		{
			//开启扫荡文字
			if (SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.MaxPassLayer >= limitLayer)
			{
				UIUtility.CopyIconTrans(sweepButton, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);
				lockbg.Hide(true);
			}
			else
			{
				lockbg.Hide(false);
				UIUtility.CopyIconTrans(sweepButton, UIElemTemplate.Inst.iconBorderTemplate.iconCardGray);
			}
		}

		//进入过千机楼并请求成功
		isInTower = true;

		//每天第一次进入千机楼
		if (SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.IsResetPlayerInfo && !isConnect)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgMessage_TowerAlreadyConnec"));
			SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.IsResetPlayerInfo = false;
		}
	}

	//人物模型是否移动
	private bool isMoving
	{
		get
		{
			if (TowerSceneData.Instance.currentPlayerRole != null && TowerSceneData.Instance.currentPlayerRole.IsMoving)
				return true;

			return false;
		}
	}

	//战N层，积分显示
	private void SetPointRewardLabel()
	{
		List<MelaleucaFloorConfig.Challenge> challenges = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.Challenges;
		for (int i = 0; i < challenges.Count; i++)
		{
			switch (challenges[i].Layers)
			{
				case 2: doublePointLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerAttackMode_GetPoints"),
					GameDefines.textColorBtnYellow, GameDefines.textColorWhite, challenges[i].Point);
					break;

				case 4: fourPointLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerAttackMode_GetPoints"),
					GameDefines.textColorBtnYellow, GameDefines.textColorWhite, challenges[i].Point);
					break;

				case 8: eightPointLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerAttackMode_GetPoints"),
					GameDefines.textColorBtnYellow, GameDefines.textColorWhite, challenges[i].Point);
					break;

				default: break;
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator UnLockPlay(float time)
	{

		yield return new WaitForSeconds(time);
		GameObject particle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_IconUnlock));
		if (particle != null)
			ObjectUtility.AttachToParentAndKeepLocalTrans(lockbg.gameObject, particle);

		yield return new WaitForSeconds(0.6f);

		string openMes = string.Format(GameUtility.GetUIString("UIPnlTowerPlayerInfo_SweepLimit_Label"), limitLayer);
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), openMes);

		UIUtility.CopyIconTrans(sweepButton, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);
		lockbg.Hide(true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackButtonClick(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnShopButtonClick(UIButton btn)
	{
		if (isMoving)
			return;

		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTowerNormalShop);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnLayerButtonClick(UIListButton3D btn)
	{
		if (isMoving)
			return;

		layerLevel = (int)btn.Data;
		// 请求单层NPC阵容信息
		RequestMgr.Inst.Request(new QueryMelaleucaFloorInfoReq(layerLevel));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRankButtonClick(UIButton btn)
	{
		if (isMoving)
			return;

		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTowerThisWeekRank);
	}

	//扫荡选择
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSweepButtonClick(UIButton btn)
	{
		if (isMoving)
			return;

		//扫荡条件限定
		string sweepMessage = "";
		if (SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.MaxPassLayer < limitLayer)
		{
			sweepMessage = string.Format(GameUtility.GetUIString("UIPnlTowerPlayerInfo_SweepLimit_Label"), limitLayer);
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), sweepMessage);
		}
		else
		{
			List<MainMenuItem> menuItems = new List<MainMenuItem>();

			string titleName = GameUtility.GetUIString("UIPnlTowerSweepBattle_SweepTypeChoose_Label");

			MainMenuItem SweepDoubleMenu = new MainMenuItem();
			SweepDoubleMenu.Callback = SweepDoubleCallback;
			SweepDoubleMenu.ControlText = GameUtility.GetUIString("UIPnlTowerSweepBattle_DoubleType_Label");
			menuItems.Add(SweepDoubleMenu);

			MainMenuItem SweepFourMenu = new MainMenuItem();
			SweepFourMenu.Callback = SweepFourCallback;
			SweepFourMenu.ControlText = GameUtility.GetUIString("UIPnlTowerSweepBattle_FourType_Label");
			menuItems.Add(SweepFourMenu);

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(titleName, true, true, menuItems.ToArray());
			SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
		}
	}

	private bool SweepDoubleCallback(object data)
	{
		sweepBattleType = 2;
		sweepBattleCount = 4;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Tower, true);
		return true;
	}

	private bool SweepFourCallback(object data)
	{
		sweepBattleType = 4;
		sweepBattleCount = 2;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Tower, true);
		return true;
	}

	private void PlayTowerMusic()
	{
		string levelName = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.towerSceneId).levelName;
		string music = ConfigDatabase.DefaultCfg.SceneConfig.GetBgMusicBySceneName(levelName);
		if (AudioManager.Instance.IsMusicPlaying(music) == false)
		{
			AudioManager.Instance.StopMusic();
			AudioManager.Instance.PlayMusic(music, true);
		}
	}

	public void QueryMelaleucaFloorInfoSuccess(List<com.kodgames.corgi.protocol.NpcInfo> npcInfos)
	{
		//发送当前层数
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTowerNpcLineUp), npcInfos, layerLevel);
	}

	public void QuerySweepBattleSuccess(UIPnlTowerSweepBattle.TowerSweepResultData sweepData)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTowerSweepBattle), sweepData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickExplainBtn(UIButton btn)
	{
		if (isMoving)
			return;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgTowerExplain));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTwoBtn(UIButton btn)
	{
		if (isMoving)
			return;

		battleType = 2;
		if (challengeCount > 0)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Tower, false);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlTowerPlayerInfo_challengeTimes_Failed"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFourBtn(UIButton btn)
	{
		if (isMoving)
			return;

		battleType = 4;
		if (challengeCount > 0)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Tower, false);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlTowerPlayerInfo_challengeTimes_Failed"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEightBtn(UIButton btn)
	{
		if (isMoving)
			return;

		battleType = 8;
		if (challengeCount > 0)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Tower, false);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlTowerPlayerInfo_challengeTimes_Failed"));
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

	//试炼印
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickStamina(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.TrialStamp).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}
}
