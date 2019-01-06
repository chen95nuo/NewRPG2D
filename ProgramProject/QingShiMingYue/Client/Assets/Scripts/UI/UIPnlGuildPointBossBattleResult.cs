using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointBossBattleResult : UIPnlBattleResultBase
{
	public SpriteText leftTitle;
	public SpriteText leftTips;
	public SpriteText myDamage;	
	public SpriteText downLabel;
	public SpriteText winBossName;

	public UIBox ResultType;

	public GameObject successObj;
	public GameObject failedObj;	

	public UIScrollList battleInfoList;
	public GameObjectPool battleInfoPool;

	public UIScrollList rewardList;
	public GameObjectPool rewardPool;
	public GameObjectPool titlePool;

	public UIBox ResultBg;

	//private float defaultWidth;
	private float defaultHeight = 245;
	private float height = 80;

	public class GuildBossBattleResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;
		public KodGames.ClientClass.CombatResultAndReward BattleData { get { return battleData; } }

		private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
		public KodGames.ClientClass.CostAndRewardAndSync CostAndRewardAndSync { get { return costAndRewardAndSync; } }

		private com.kodgames.corgi.protocol.Rank myRank;
		public com.kodgames.corgi.protocol.Rank MyRank { get { return myRank; } }		

		private com.kodgames.corgi.protocol.BossRank bossRank;
		public com.kodgames.corgi.protocol.BossRank BossRank { get { return bossRank; } }
		
		private List<com.kodgames.corgi.protocol.ShowReward> commonRewards;
		public List<com.kodgames.corgi.protocol.ShowReward> CommonRewards { get { return commonRewards; } }

		private List<com.kodgames.corgi.protocol.ShowReward> extraRewards;
		public List<com.kodgames.corgi.protocol.ShowReward> ExtraRewards { get { return extraRewards; } }

		private bool hasActivateGoods;
		public bool HasActivateGoods { get { return hasActivateGoods; } }

		private com.kodgames.corgi.protocol.Rank thisDate;
		public com.kodgames.corgi.protocol.Rank ThisDate { get { return thisDate; } }

		public GuildBossBattleResultData(KodGames.ClientClass.CombatResultAndReward battleData, 	KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync,
			com.kodgames.corgi.protocol.Rank myRank, com.kodgames.corgi.protocol.BossRank bossRank, List<com.kodgames.corgi.protocol.ShowReward> commonRewards,
			List<com.kodgames.corgi.protocol.ShowReward> extraRewards, bool hasActivateGoods, com.kodgames.corgi.protocol.Rank thisDate)
			: base(_UIType.UIPnlGuildPointBossBattleResult)
		{
			this.battleData = battleData;
			this.costAndRewardAndSync = costAndRewardAndSync;			
			this.myRank = myRank;
			this.bossRank = bossRank;
			this.commonRewards = commonRewards;
			this.extraRewards = extraRewards;
			this.hasActivateGoods = hasActivateGoods;
			this.thisDate = thisDate;
			this.CombatType = _CombatType.GuildBoss;
		}

		public override bool CanShowView()
		{
			return true;
		}

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

		public override bool CanShowLvlInformation()
		{
			return true;
		}

		public override string GetGoldRewardOrOtherStr()
		{
			return "";
		}

		public override string GetExpRewardOrOtherStr()
		{
			return "";
		}

		public override KodGames.ClientClass.Reward GetBattleReward()
		{
			return battleData.DungeonReward;
		}

		public override KodGames.ClientClass.Reward GetFirstPassReward()
		{
			return battleData.FirstpassReward;
		}

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
	}

	public override void InitViews()
	{
		base.InitViews();

		var bossBattleData= battleResultData as GuildBossBattleResultData;

		int myBattle = (int)bossBattleData.MyRank.damage;
		if (myBattle > 1000000)
			myDamage.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_LeftTipsLargeCount", myBattle / 10000, bossBattleData.MyRank.doubleValue.ToString("P"));
		else
			myDamage.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_LeftTips", myBattle, bossBattleData.MyRank.doubleValue.ToString("P"));
		
		ClearData();
		StartCoroutine("FillList");
		StartCoroutine("FillRewardList");

		bool isWin = bossBattleData.IsWinner();
		ResultType.Hide(isWin);
		successObj.SetActive(isWin);

		if(isWin)
		{
			leftTitle.Text = GameUtility.GetUIString("UIPnlGuildPointMonsterBattleResult_LeftTitle2");

			if (bossBattleData.MyRank.rankValue <= 10 && bossBattleData.MyRank.rankValue > 0)
				leftTips.Text = bossBattleData.MyRank.rankValue.ToString();
			else
				leftTips.Text = GameUtility.GetUIString("UIPnlGuildPointBossBattleResult_NotRank");

			downLabel.Text = "";

			winBossName.Text = bossBattleData.BossRank.name;
			ResultBg.SetSize(ResultBg.width, defaultHeight);
		}
		else
		{
			if (bossBattleData.MyRank.rankValue <= 10 && bossBattleData.MyRank.rankValue > 0)
				downLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_MyRank", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, bossBattleData.MyRank.rankValue.ToString());
			else
				downLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_NotRank", GameDefines.textColorBtnYellow, GameDefines.textColorWhite);

			leftTitle.Text = GameUtility.GetUIString("UIPnlGuildPointMonsterBattleResult_LeftTitle");
			int thisDamage = (int)bossBattleData.ThisDate.damage;
			if (thisDamage > 1000000)
				leftTips.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_LeftTipsLargeCount", thisDamage / 10000, bossBattleData.ThisDate.doubleValue.ToString("P"));
			else
				leftTips.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_LeftTips", thisDamage, bossBattleData.ThisDate.doubleValue.ToString("P"));	

			ResultBg.SetSize(ResultBg.width, defaultHeight + height);
			winBossName.Text = "";
		}		
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void FillData()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	private void ClearData()
	{
		StopCoroutine("FillList");

		battleInfoList.ClearList(false);
		battleInfoList.ScrollPosition = 0f;

		StopCoroutine("FillRewardList");

		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		var bossBattleData = battleResultData as GuildBossBattleResultData;

		if (SysLocalDataBase.CCRewardListToShowReward(bossBattleData.GetFirstPassReward()).Count > 0)
		{
			UIElemGuildPointRewardShowTitleItem item = titlePool.AllocateItem().GetComponent<UIElemGuildPointRewardShowTitleItem>();
			item.SetData(false);

			rewardList.AddItem(item.gameObject);
		}

		foreach (var reward in SysLocalDataBase.CCRewardListToShowReward(bossBattleData.GetFirstPassReward()))
		{
			UIElemDanExtraReward item = rewardPool.AllocateItem().GetComponent<UIElemDanExtraReward>();
			item.SetData(reward);

			rewardList.AddItem(item.gameObject);
		}

		if (SysLocalDataBase.CCRewardListToShowReward(bossBattleData.GetBattleReward()).Count > 0)
		{
			UIElemGuildPointRewardShowTitleItem item = titlePool.AllocateItem().GetComponent<UIElemGuildPointRewardShowTitleItem>();
			item.SetData(true);

			rewardList.AddItem(item.gameObject);
		}

		foreach (var reward in SysLocalDataBase.CCRewardListToShowReward(bossBattleData.GetBattleReward()))
		{
			UIElemDanExtraReward item = rewardPool.AllocateItem().GetComponent<UIElemDanExtraReward>();
			item.SetData(reward);

			rewardList.AddItem(item.gameObject);
		}

		rewardList.ScrollToItem(0, 0);		
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		var bossBattleData = battleResultData as GuildBossBattleResultData;

		for (int i = 0; i < bossBattleData.BossRank.ranks.Count; i++)
		{
			UIElemGuildBossBattleResultItem item = battleInfoPool.AllocateItem().GetComponent<UIElemGuildBossBattleResultItem>();
			item.SetData(bossBattleData.BossRank.ranks[i]);
			battleInfoList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCloseClick(UIButton btn)
	{
		GameUtility.JumpUIPanel(_UIType.UI_GuildPoint);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnReplayClick(UIButton btn)
	{
		HideSelf();

		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		battleState.ReplayBattle();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var showItem = assetIcon.Data as com.kodgames.corgi.protocol.ShowReward;
		GameUtility.ShowAssetInfoUI(showItem, _UILayer.Top);
	}
}
