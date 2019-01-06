using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class UIPnlWolfInfo : UIModule
{
	public GameObject completeRoot;
	public GameObject piXiuRoot;

	public UIBox FontFx1;
	public UIBox FontFx2;

	public SpriteText medalsLabel;
	public SpriteText gameMoneyLabel;
	public SpriteText realMoneyLabel;

	public SpriteText playerNameLabel;
	public UIElemAssetIcon assetIcon;

	public SpriteText alreadyPassLabel;
	public SpriteText thisMaxPassLabel;
	public SpriteText leftCount;

	//第二十关卡
	public UIButton stageRewardBtn;

	//重新开始按钮
	public UIButton resetBtn;

	private WolfSmokeData wolfSmokeData;
	private UIPnlWolfBattleResult.WolfBattleResultData wolfBattleResult;
	private bool isBattleReturn = false;

	private int gameMoney;
	private int medals;
	private bool isPlayerMove = false;

	private int passStageIndex = 0;
	public int PassStageIndex { get { return passStageIndex; } }
	private int historyPassStage = 0;
	public int HistoryPassStage { get { return historyPassStage; } }

	private int playerAvatarId = 0;
	private int enemyAvatarId = 0;
	private int playerBreakThrough = 0;
	private int enemyBreakThrough = 0;

	//private bool isComplete = false;

	private bool isShowShop = false;

	public List<string> avatarHpData_Guid;
	public List<double> avatarHpData_hpMaxHp;

	public class WolfSmokeData
	{
		private bool isJoin;
		public bool IsJoin { get { return isJoin; } }

		private KodGames.ClientClass.WolfInfo wolfInfo;
		public KodGames.ClientClass.WolfInfo WolfInfo { get { return wolfInfo; } }

		private KodGames.ClientClass.Player wolfPlayer;
		public KodGames.ClientClass.Player WolfPlayer { get { return wolfPlayer; } }

		private KodGames.ClientClass.Player enemyPlayer;
		public KodGames.ClientClass.Player EnemyPlayer { get { return enemyPlayer; } }

		private List<KodGames.ClientClass.WolfSmokeAddition> wolfSmokeAdditions;
		public List<KodGames.ClientClass.WolfSmokeAddition> WolfSmokeAdditions { get { return wolfSmokeAdditions; } }

		private List<KodGames.ClientClass.WolfSelectedAddition> wolfSelectedAdditions;
		public List<KodGames.ClientClass.WolfSelectedAddition> WolfSelectedAdditions { get { return wolfSelectedAdditions; } }

		private List<KodGames.ClientClass.WolfAvatar> wolfAvatars;
		public List<KodGames.ClientClass.WolfAvatar> WolfAvatars { get { return wolfAvatars; } }

		private int lastPositionId;
		public int LastPositionId { get { return lastPositionId; } }

		public WolfSmokeData(bool isJoin, KodGames.ClientClass.WolfInfo wolfInfo, KodGames.ClientClass.Player wolfPlayer,
			 List<KodGames.ClientClass.Location> locations, KodGames.ClientClass.Player enemyPlayer,
			 List<KodGames.ClientClass.WolfSmokeAddition> wolfSmokeAdditions,
			List<KodGames.ClientClass.WolfSelectedAddition> wolfSelectedAdditions, List<KodGames.ClientClass.WolfAvatar> wolfAvatars, int lastPositionId)
		{
			this.isJoin = isJoin;
			this.wolfInfo = wolfInfo;
			this.wolfPlayer = wolfPlayer;
			this.enemyPlayer = enemyPlayer;
			this.wolfSmokeAdditions = wolfSmokeAdditions;
			this.wolfSelectedAdditions = wolfSelectedAdditions;
			this.wolfAvatars = wolfAvatars;
			this.lastPositionId = lastPositionId;

			//如果locations不为空并且长度不为零
			if (locations != null && locations.Count > 0)
			{
				for (int index = 0; index < locations.Count; index++)
				{
					for (int i = 0; i < this.wolfPlayer.PositionData.Positions[0].AvatarLocations.Count; i++)
					{
						if (this.wolfPlayer.PositionData.Positions[0].AvatarLocations[i].Guid.Equals(locations[index].Guid))
						{
							this.wolfPlayer.PositionData.Positions[0].AvatarLocations[i].LocationId = locations[index].LocationId;
						}
					}
				}
			}
		}
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		return true;
	}

	public override void OnHide()
	{
		WolfSmokeSceneData.Instance.MainCamera.enabled = false;

		base.OnHide();
	}

	public override void Overlay()
	{
		base.Overlay();

		WolfSmokeSceneData.Instance.MainCamera.enabled = false;

		SetAssistantAnimation();
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		WolfSmokeSceneData.Instance.MainCamera.enabled = true;
		SetAssistantAnimation();
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		WolfSmokeSceneData.Instance.MainCamera.enabled = true;

		if (userDatas.Length > 0)
		{
			if (userDatas[0] is WolfSmokeData)
				this.wolfSmokeData = userDatas[0] as WolfSmokeData;

			if (userDatas.Length > 1)
			{
				if (userDatas[1] is UIPnlWolfBattleResult.WolfBattleResultData)
					this.wolfBattleResult = userDatas[1] as UIPnlWolfBattleResult.WolfBattleResultData;
				isBattleReturn = true;

				isShowShop = (bool)userDatas[2];
			}
			InitWolffPlayerInfo();
		}

		realMoneyLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.RealMoney);
		gameMoneyLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.GameMoney);
		medalsLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.Medals);

		InitHP_MaxHP();

		return true;
	}

	public void InitWolffPlayerInfo()
	{
		completeRoot.SetActive(false);
		stageRewardBtn.Hide(false);
		piXiuRoot.SetActive(true);

		string enemyName = wolfSmokeData.EnemyPlayer.Name;

		var playerPosition = wolfSmokeData.WolfPlayer.PositionData.Positions[0];
		playerPosition.AvatarLocations.Sort((a1, a2) =>
			{
				return a1.LocationId - a2.LocationId;
			});

		var enemyPosition = wolfSmokeData.EnemyPlayer.PositionData.Positions[0];
		enemyPosition.AvatarLocations.Sort((a1, a2) =>
		{
			return a1.LocationId - a2.LocationId;
		});
		enemyAvatarId = enemyPosition.AvatarLocations[0].ResourceId;
		enemyBreakThrough = wolfSmokeData.EnemyPlayer.SearchAvatar(enemyPosition.AvatarLocations[0].Guid).BreakthoughtLevel;
		playerAvatarId = playerPosition.AvatarLocations[0].ResourceId;
		playerBreakThrough = wolfSmokeData.WolfPlayer.SearchAvatar(playerPosition.AvatarLocations[0].Guid).BreakthoughtLevel;

		//获取自己角色的模型数据
		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(playerAvatarId);
		int assetID = avatarCfg.GetAvatarAssetId(playerBreakThrough);
		//获取敌人的模型数据
		AvatarConfig.Avatar enemyCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(enemyAvatarId);
		int enemyAssetID = enemyCfg.GetAvatarAssetId(enemyBreakThrough);

		int stageMaxCount = ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages.Count;

		//超过最大关卡之后，关卡ID为负
		if (wolfSmokeData.WolfInfo.StageId > 0)
			passStageIndex = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetIndexByStageId(wolfSmokeData.WolfInfo.StageId);
		else
			passStageIndex = stageMaxCount + 1; //用于超出关卡判定

		//创建关卡模型
		WolfSmokeSceneData.Instance.CreateWolfStage(passStageIndex, stageMaxCount);

		//战斗对话判断
		bool isShowDialogue = false;

		//最大通关数
		historyPassStage = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetIndexByStageId(wolfSmokeData.WolfInfo.MaxPassStageId);

		//剩余重置次数
		int leftTimes = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount)
			- wolfSmokeData.WolfInfo.AlreadyResetTimes;

		if (passStageIndex >= historyPassStage && historyPassStage != stageMaxCount)
			isShowDialogue = true;

		if (passStageIndex <= stageMaxCount)
		{
			if (isBattleReturn && wolfBattleResult.IsWinner())
			{
				isPlayerMove = true;
				WolfSmokeSceneData.Instance.CreateWolfRun(assetID, enemyAssetID, passStageIndex, wolfBattleResult, isShowDialogue, enemyName, wolfSmokeData.EnemyPlayer.Power);
			}
			else
			{
				if (isBattleReturn)
					WolfSmokeSceneData.Instance.CreateAvatar(assetID, enemyAssetID, passStageIndex, false, false, enemyName, wolfSmokeData.EnemyPlayer.Power);// 根据战斗返回显示动画			
				else
				{
					SysLocalDataBase.Inst.LocalPlayer.Medals = wolfSmokeData.WolfInfo.Medals;
					SysLocalDataBase.Inst.LocalPlayer.GameMoney = wolfSmokeData.WolfInfo.GameMoney;
					SysLocalDataBase.Inst.LocalPlayer.RealMoney = wolfSmokeData.WolfInfo.RealMoney;
					WolfSmokeSceneData.Instance.AvatarEnterScene(assetID, enemyAssetID, passStageIndex, isShowDialogue, false, enemyName, wolfSmokeData.EnemyPlayer.Power);
				}
			}
		}
		else
		{
			WolfSmokeSceneData.Instance.ReSetCamerTransform(ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages.Count);

			completeRoot.SetActive(true);
			piXiuRoot.SetActive(true);
			SetAssistantAnimation();
			//isComplete = true;
			stageRewardBtn.Hide(true);
			if (leftTimes > 0)
			{
				GameObject resetEff = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.resetBtnEffect));
				if (resetEff != null)
					ObjectUtility.AttachToParentAndResetLocalPosAndRotation(resetBtn.gameObject, resetEff);
			}
		}

		gameMoney = SysLocalDataBase.Inst.LocalPlayer.GameMoney;
		medals = SysLocalDataBase.Inst.LocalPlayer.Medals;

		playerNameLabel.Text = wolfSmokeData.WolfPlayer.Name;
		assetIcon.SetData(playerAvatarId);
		realMoneyLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.RealMoney);
		gameMoneyLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.GameMoney);
		medalsLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.Medals);

		alreadyPassLabel.Text = ItemInfoUtility.GetCNStageIndex(historyPassStage);
		thisMaxPassLabel.Text = ItemInfoUtility.GetCNStageIndex(passStageIndex - 1);
		leftCount.Text = leftTimes.ToString();

		if (isShowShop)
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfNormalShop));
	}

	public void ShowWolfDialogue()
	{
		if (wolfSmokeData == null)
			return;
		int dialogueId = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageById(wolfSmokeData.WolfInfo.StageId).Dialogue;
		SysUIEnv.Instance.GetUIModule<UITipAdviser>().ShowDialogueAvatar(dialogueId, wolfSmokeData.EnemyPlayer.Name, enemyAvatarId, null);
	}

	public void ResetPlayerMoving()
	{
		isPlayerMove = false;
	}

	public void UpDateEggsReward(KodGames.ClientClass.WolfEggs wolfEgg, int index)
	{
		if (wolfEgg.RewardId == IDSeg._SpecialId.GameMoney)
			gameMoney += wolfEgg.RewardCount;
		if (wolfEgg.RewardId == IDSeg._SpecialId.Medals)
			medals += wolfEgg.RewardCount;

		gameMoneyLabel.Text = ItemInfoUtility.GetItemCountStr(gameMoney);
		medalsLabel.Text = ItemInfoUtility.GetItemCountStr(medals);

		if (wolfBattleResult != null)
		{
			if (index == wolfBattleResult.WolfEggs.Count - 1)
			{
				SysLocalDataBase.Inst.ProcessCostRewardSync(wolfBattleResult.EggsCostAndRewardAndSync, "UpDateEggsReward");
				gameMoneyLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.GameMoney);
				medalsLabel.Text = ItemInfoUtility.GetItemCountStr(SysLocalDataBase.Inst.LocalPlayer.Medals);
			}
		}

		string message = string.Format(GameUtility.GetUIString("UIPnlWolfInfo_EggGetInfo_Label"), ItemInfoUtility.GetAssetName(wolfEgg.RewardId), wolfEgg.RewardCount);
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), message);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickShopBtn(UIButton btn)
	{
		if (isPlayerMove)
			return;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfNormalShop));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOpenBtn(UIButton btn)
	{
		if (isPlayerMove)
			return;
		WolfSmokeSceneData.Instance.PassPoint();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardBtn(UIButton btn)
	{
		if (isPlayerMove)
			return;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfCheckPoint), wolfSmokeData.WolfInfo.StageId, wolfSmokeData.WolfInfo.MaxPassStageId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackButtonClick(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>();
	}

	//点击重置按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRestart(UIButton btn)
	{
		if (isPlayerMove)
			return;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfStart), this.wolfSmokeData.WolfInfo.AlreadyResetTimes);
	}

	//由增益弹窗中跳转到我方阵容
	public void JumpToMyBattle(int gainId)
	{
		if (gainId != IDSeg.InvalidId)
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfMyBattle), this.wolfSmokeData.WolfPlayer, this.wolfSmokeData.WolfPlayer.PositionData, this.wolfSmokeData.WolfSelectedAdditions, this.avatarHpData_Guid, this.avatarHpData_hpMaxHp, gainId, this.wolfSmokeData.WolfInfo.StageId);
	}

	//进行战斗协议
	private bool WaitCombat = false;
	public void FightSever(int gainId)
	{
		int failyNumber = 0;
		failyNumber = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddCanFaildCount) -
						this.wolfSmokeData.WolfInfo.AlreadyFailedTimes;

		if (failyNumber > 0)
		{
			//发送战斗协议
			if (!WaitCombat)
			{
				WaitCombat = true;
				RequestMgr.Inst.Request(new QueryCombatWolfSmoke(gainId, this.wolfSmokeData.WolfPlayer.PositionData.Positions[0]));
			}
		}
		else
			//战斗失败次数为零			
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfFailyDeficiency), this.wolfSmokeData.WolfInfo);
	}

	//点击阵容按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickMyAvatar(UIButton btn)
	{
		if (isPlayerMove)
			return;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfMyBattle), this.wolfSmokeData.WolfPlayer, this.wolfSmokeData.WolfPlayer.PositionData, this.wolfSmokeData.WolfSelectedAdditions, this.avatarHpData_Guid, this.avatarHpData_hpMaxHp, null);
	}

	//己方阵容3D按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickAvatar(UIButton3D btn)
	{
		if (isPlayerMove)
			return;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfMyBattle), this.wolfSmokeData.WolfPlayer, this.wolfSmokeData.WolfPlayer.PositionData, this.wolfSmokeData.WolfSelectedAdditions, this.avatarHpData_Guid, this.avatarHpData_hpMaxHp, null);
	}
	//敌方参战3D按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickEnemy(UIButton3D btn)
	{
		if (isPlayerMove)
			return;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfEnemyExpedition), this.wolfSmokeData.EnemyPlayer, this.wolfSmokeData.EnemyPlayer.PositionData, this.wolfSmokeData.WolfInfo,this.wolfSmokeData.EnemyPlayer.Power, this.avatarHpData_Guid, this.avatarHpData_hpMaxHp);
	}

	//点击弹出规则
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickFourBtn(UIButton btn)
	{
		if (isPlayerMove)
			return;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfGuide), this.wolfSmokeData.WolfInfo.MaxPassStageId);
	}

	public void SetAssistantAnimation()
	{
		var animation = piXiuRoot.GetComponentInChildren<Animation>();
		if (animation != null)
		{
			//设置动画循环播放，防止别的地方停止该动画的播放状态
			animation.wrapMode = WrapMode.Loop;
			if (!animation.IsPlaying("QiLin_Idle"))
			{
				animation.Play("QiLin_Idle", PlayMode.StopSameLayer);
			}
		}
	}

	//点击元宝
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickRealMoney(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.RealMoney).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//点击铜币
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickGameMoney(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.GameMoney).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//点击军工
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickCupTure(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.Medals).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	#region calculate HP

	public void InitHP_MaxHP()
	{
		this.avatarHpData_Guid = new List<string>();
		this.avatarHpData_hpMaxHp = new List<double>();

		for (int i = 0; i < this.wolfSmokeData.WolfAvatars.Count; i++)
		{
			if (this.wolfSmokeData.WolfAvatars[i].ResourceId != IDSeg.InvalidId)
			{
				this.avatarHpData_Guid.Add(this.wolfSmokeData.WolfAvatars[i].Guid);
				this.avatarHpData_hpMaxHp.Add(this.wolfSmokeData.WolfAvatars[i].LeftHP > 1 ? 1 : this.wolfSmokeData.WolfAvatars[i].LeftHP);
			}
		}
	}

	#endregion
}
