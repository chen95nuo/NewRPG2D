using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendCampaginBattleResult : UIPnlBattleResultBase
{
	//胜利方面
	public UIBox upGoldBgBox, downGoldBgBox;
	public SpriteText upGoldCount, downGoldCount;
	public SpriteText upGoldName, downGoldName;
	public UIElemAssetIcon upGoldIcon, downGoldIcon;

	//****首次通关奖励
	public UIBox firstBgBox;
	public SpriteText firstText;
	public UIScrollList firstRewardList;
	public GameObjectPool fristRewardItemRootPool;

	//****通关奖励
	public UIBox passBgBox;
	public SpriteText passText;
	public UIScrollList passRewardList;
	public GameObjectPool passRewardItemRootPool;

	//失败方面
	public SpriteText lostText;
	public UIBox lostBgBox;
	public SpriteText lostTitleText;
	public List<UIElemFriendCombatTabItem> avatarItems;

	//下方按钮组控制
	public Transform buttonNodeRoot;
	public Transform buttonNodeRootFirstRewards;//有首通
	public Transform buttonNodeRootrewards;//没首通
	public Transform buttonNodeRootPass;//失败

	//普通奖励的List
	public Transform passRewardsBoxNodeRoot;
	public Transform passRewardsBoxNodeRootFirst;	//有首通
	public Transform passRewardsBoxNodeRootNotFirst;//没首通

	//首通奖励List
	public Transform firstRewardsBoxNodeRoot;
	public Transform firstRewardsBoxNodeRootFirst;//没有首通

	//上面两个获得奖励的位置
	public Transform getRewardsBoxNodeRoot;
	public Transform getRewardsBoxNodeRootFirst;	//有首通
	public Transform getRewardsBoxNodeRootNotFirst; //没首通
	public Transform getRewardsBoxNodeRootLoss;		//失败

	//上面图标的显示
	public Transform topBoxNodeRoot;
	public Transform topBoxNodeRootFirst;			//有首通
	public Transform topBoxNodeRootNotFirst;		//没首通
	public Transform topBoxNodeRootLoss;			//失败

	//失败时的位置
	public Transform lossBoxNodeRoot;
	public Transform lossBoxNodeRootLoss;

	public class FriendCampaginBattleResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;

		private int passLastId;
		public int PassLastId
		{
			get { return passLastId; }
		}

		private KodGames.ClientClass.Player enemyPlayer;
		public KodGames.ClientClass.Player EnemyPlayer
		{
			get { return enemyPlayer; }
		}

		private List<KodGames.ClientClass.HpInfo> enemyHpInfos;
		public List<KodGames.ClientClass.HpInfo> EnemyHpInfos
		{
			get { return enemyHpInfos; }
		}

		private com.kodgames.corgi.protocol.RobotInfo robotInfo;
		public com.kodgames.corgi.protocol.RobotInfo RobotInfo
		{
			get { return robotInfo; }
		}

		public FriendCampaginBattleResultData(int passLastId, KodGames.ClientClass.CombatResultAndReward battleData, KodGames.ClientClass.Player enemyPlayer,
											List<KodGames.ClientClass.HpInfo> enemyHpInfos, com.kodgames.corgi.protocol.RobotInfo robotInfo)
			: base(_UIType.UIPnlFriendCampaginBattleResult)
		{
			this.passLastId = passLastId;
			this.battleData = battleData;
			this.enemyPlayer = enemyPlayer;
			this.enemyHpInfos = enemyHpInfos;
			this.robotInfo = robotInfo;
			this.CombatType = _CombatType.FriendCampaign;
		}

		#region inherit

		public override bool CanShowView()
		{
			return true;
		}

		public override bool CanShowLvlInformation()
		{
			return true;
		}

		//判断是胜利还是失败
		public override bool IsWinner()
		{
			if (battleData != null)
				return battleData.BattleRecords[battleData.BattleRecords.Count - 1].TeamRecords[0].IsWinner;

			return false;
		}

		public override int GetAppraiseNumber()
		{
			return 0;
		}

		//获取通关奖励
		public override KodGames.ClientClass.Reward GetBattleReward()
		{
			return battleData.DungeonReward;
		}

		//获取首通奖励
		public override KodGames.ClientClass.Reward GetFirstPassReward()
		{
			return battleData.FirstpassReward;
		}

		//判断是否有首通
		public override bool HasFirstPassReward()
		{
			if (GetFirstPassReward() == null)
				return false;

			var dic = SysLocalDataBase.ConvertIdCountList(GetFirstPassReward());

			if (dic == null || dic.Count == 0)
				return false;

			return true;
		}

		public override bool CanShowFailGuid()
		{
			return true;
		}

		public override string GetGoldRewardOrOtherStr()
		{
			int goldRewardOrOtherCount = 0;

			string message = string.Empty;

			bool isWin = false;
			if (this.battleData != null)
				isWin = battleData.BattleRecords[battleData.BattleRecords.Count - 1].TeamRecords[0].IsWinner;

			if (isWin)
			{
				foreach (KodGames.ClientClass.Consumable consumable in this.battleData.DungeonReward.Consumable)
				{
					if (ClientServerCommon.IDSeg._SpecialId.GameMoney == consumable.Id)
						goldRewardOrOtherCount = consumable.Amount;
				}
				message = goldRewardOrOtherCount.ToString();
			}

			return message;
		}

		public int GetRewardGameMoney()
		{
			foreach (KodGames.ClientClass.Consumable consumable in this.battleData.DungeonReward.Consumable)
				if (ClientServerCommon.IDSeg._SpecialId.GameMoney == consumable.Id)
					return consumable.Amount;

			return 0;
		}

		#endregion
	}

	//由基类当中的 BattleResultData 强制转化成 FriendCampaginBattleResultData
	public FriendCampaginBattleResultData GetFriendCampaginBattleInfo()
	{
		var FriendCampaginBattleResultData = battleResultData as FriendCampaginBattleResultData;
		return FriendCampaginBattleResultData;
	}

	public override void InitViews()
	{
		base.InitViews();

		//是否胜利
		bool pRetPass = battleResultData.IsWinner();

		//首次通关
		bool firstReward = battleResultData.HasFirstPassReward();

		// 记录状态，对数据进行写入修改
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.InterruptCampaign = true;

		//不管胜利失败，先把界面上的节点全部置空
		ClearUpGold();
		ClearDownGold();
		ClearFirstRewars();
		ClearPassRewars();
		ClearLostPosition();

		if (pRetPass)
		{
			if (GetFriendCampaginBattleInfo().GetRewardGameMoney() != 0)
			{
				this.upGoldBgBox.Hide(false);
				this.upGoldName.Text = GameUtility.FormatUIString("UIPnlFriendCampaginBattleResult_IconText", ItemInfoUtility.GetAssetName(ClientServerCommon.IDSeg._SpecialId.GameMoney));
				this.upGoldIcon.Hide(false);
				this.upGoldCount.Text = battleResultData.GetGoldRewardOrOtherStr();
				this.upGoldIcon.SetData(ClientServerCommon.IDSeg._SpecialId.GameMoney);

				if (getRewardsBoxNodeRoot != null)
				{
					Animation ani = getRewardsBoxNodeRoot.GetComponent<Animation>();
					if (ani != null)
						ani.Play();
				}
			}

			//通关奖励
			passBgBox.Hide(false);
			passText.Text = GameUtility.GetUIString("UIPnlFriendCampaginBattleResult_PassText");
			foreach (var reward in SysLocalDataBase.ConvertIdCountList(battleResultData.GetBattleReward()))
			{
				UIElemFriendBattleRewardItemRoot item = passRewardItemRootPool.AllocateItem().GetComponent<UIElemFriendBattleRewardItemRoot>();
				item.SetData(reward.first, reward.second);
				passRewardList.AddItem(item);
			}

			if (firstReward)
			{
				//首通奖励
				firstBgBox.Hide(false);
				firstText.Text = GameUtility.GetUIString("UIPnlFriendCampaginBattleResult_FirstText");

				foreach (var kvp in SysLocalDataBase.ConvertIdCountList(battleResultData.GetFirstPassReward()))
				{
					UIElemFriendBattleRewardItemRoot item = fristRewardItemRootPool.AllocateItem().GetComponent<UIElemFriendBattleRewardItemRoot>();
					item.SetData(kvp.first, kvp.second);
					firstRewardList.AddItem(item);
				}

				//有首通
				buttonNodeRoot.localPosition = buttonNodeRootFirstRewards.localPosition;
				passRewardsBoxNodeRoot.localPosition = passRewardsBoxNodeRootFirst.localPosition;
				firstRewardsBoxNodeRoot.localPosition = firstRewardsBoxNodeRootFirst.localPosition;
				getRewardsBoxNodeRoot.localPosition = getRewardsBoxNodeRootFirst.localPosition;
				topBoxNodeRoot.localPosition = topBoxNodeRootFirst.localPosition;
			}
			else
			{
				//没首通
				buttonNodeRoot.localPosition = buttonNodeRootrewards.localPosition;
				passRewardsBoxNodeRoot.localPosition = passRewardsBoxNodeRootNotFirst.localPosition;
				getRewardsBoxNodeRoot.localPosition = getRewardsBoxNodeRootNotFirst.localPosition;
				topBoxNodeRoot.localPosition = topBoxNodeRootNotFirst.localPosition;
			}
		}
		else
		{
			//失败情况下
			buttonNodeRoot.localPosition = buttonNodeRootPass.localPosition;
			getRewardsBoxNodeRoot.localPosition = getRewardsBoxNodeRootLoss.localPosition;
			topBoxNodeRoot.localPosition = topBoxNodeRootLoss.localPosition;
			lossBoxNodeRoot.localPosition = lossBoxNodeRootLoss.localPosition;

			if (getRewardsBoxNodeRoot != null)
			{
				Animation ani = getRewardsBoxNodeRoot.GetComponent<Animation>();
				if (ani != null)
					ani.Play();
			}

			this.lostBgBox.Hide(false);
			this.lostText.Text = GameUtility.GetUIString("UIPnlFriendCampaginBattleResult_LostText");

			string str = GameUtility.GetUIString("UIPnlFriendCampaginBattleResult_LostTitleText");

			float addition = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetEnemyAdditionByStageId(ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetNextStageIdById(GetFriendCampaginBattleInfo().PassLastId));
			if (addition != 0)
				str += GameUtility.FormatUIString("UIPnlFriendCampaginBattleResult_LostTitleText_HP", (int)(addition * 100));

			this.lostTitleText.Text = str;

			//先把阵容开起来
			for (int index = 0; index < this.avatarItems.Count; index++)
			{
				this.avatarItems[index].avatarIcon.Hide(false);
				this.avatarItems[index].InitData(PlayerDataUtility.GetBattlePosByIndexPos(index));
				this.avatarItems[index].hpProgress.Hide(false);
			}

			if (GetFriendCampaginBattleInfo().RobotInfo.isRobot)
			{
				bool isRecruiteAvatar = true;

				for (int index = 0; index < this.avatarItems.Count; index++)
				{
					for (int i = 0; i < GetFriendCampaginBattleInfo().RobotInfo.robotNpcs.Count; i++)
					{
						if (GetFriendCampaginBattleInfo().RobotInfo.robotNpcs[i].battlePosition == this.avatarItems[index].Position)
						{
							this.avatarItems[index].SetData(GetFriendCampaginBattleInfo().RobotInfo.robotNpcs[i], isRecruiteAvatar,
														GetHpByLocationId(GetFriendCampaginBattleInfo().RobotInfo.robotNpcs[i].battlePosition));

							isRecruiteAvatar = false;
						}
					}
				}
			}
			else
			{
				var positionData = GetFriendCampaginBattleInfo().EnemyPlayer.PositionData;
				if (positionData != null)
				{
					for (int index = 0; index < this.avatarItems.Count; index++)
					{
						for (int j = 0; j < positionData.Positions[0].AvatarLocations.Count; j++)
						{
							if (positionData.Positions[0].AvatarLocations[j].LocationId == this.avatarItems[index].Position)
							{
								this.avatarItems[index].SetData(GetFriendCampaginBattleInfo().EnemyPlayer.SearchAvatar(positionData.Positions[0].AvatarLocations[j].Guid),
														positionData.Positions[0].AvatarLocations[j].LocationId == positionData.Positions[0].EmployLocationId,
														GetHpByLocationId(positionData.Positions[0].AvatarLocations[j].LocationId));
								break;
							}
						}
					}
				}
			}
		}
	}

	public double GetHpByLocationId(int location)
	{
		for (int index = 0; index < GetFriendCampaginBattleInfo().EnemyHpInfos.Count; index++)
		{
			if (GetFriendCampaginBattleInfo().EnemyHpInfos[index].LocationId == location)
				return GetFriendCampaginBattleInfo().EnemyHpInfos[index].LeftHP;
		}

		return 0;
	}

	#region Set View.

	private void ClearUpGold()
	{
		this.upGoldBgBox.Hide(true);
		this.upGoldCount.Text = string.Empty;
		this.upGoldIcon.Hide(true);
		this.upGoldName.Text = string.Empty;
	}

	private void ClearDownGold()
	{
		this.downGoldBgBox.Hide(true);
		this.downGoldCount.Text = string.Empty;
		this.downGoldIcon.Hide(true);
		this.downGoldName.Text = string.Empty;
	}

	private void ClearFirstRewars()
	{
		this.firstBgBox.Hide(true);
		this.firstText.Text = string.Empty;
		this.firstRewardList.ClearList(false);
		this.firstRewardList.ScrollListTo(0);
	}

	private void ClearPassRewars()
	{
		this.passBgBox.Hide(true);
		this.passText.Text = string.Empty;
		this.passRewardList.ClearList(false);
		this.passRewardList.ScrollListTo(0);
	}

	private void ClearLostPosition()
	{

		this.lostText.Text = string.Empty;
		this.lostBgBox.Hide(true);
		this.lostTitleText.Text = string.Empty;
		for (int index = 0; index < this.avatarItems.Count; index++)
		{
			this.avatarItems[index].ClearData();
		}
	}

	#endregion

	//点击关闭，跳转场景并且发送协议
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		if (!(SysGameStateMachine.Instance.CurrentState is GameState_CentralCity))
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_Callback(() =>
			{
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.Arena, true, true))
					RequestMgr.Inst.Request(new QueryFriendCampaignReq());
				else
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlFriendCombatTab_JuidGameScene"));
			}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnReplayClick(UIButton btn)
	{
		HideSelf();

		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		battleState.ReplayBattle();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId);
	}
}
