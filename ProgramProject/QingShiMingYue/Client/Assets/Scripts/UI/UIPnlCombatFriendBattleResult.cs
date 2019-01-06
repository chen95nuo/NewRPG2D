using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlCombatFriendBattleResult : UIPnlBattleResultBase
{
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;
	public SpriteText faildLabel;

	//结算面板
	//胜利

	//失败
	public class CombatFriendBattleResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;

		private int friendId;
		public int FriendId { get { return friendId; } }

		public CombatFriendBattleResultData(KodGames.ClientClass.CombatResultAndReward battleData, int friendId)
			: base(_UIType.UIPnlCombatFriendBattleResult)
		{
			this.battleData = battleData;
			this.friendId = friendId;
			this.CombatType = _CombatType.CombatFriend;			
		}

		public override bool CanShowView()
		{
			return true;
		}

		public override bool CanShowLvlInformation()
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

		// Record InterrupteCampaing.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.InterruptCampaign = true;

		bool isWin = battleResultData.IsWinner();

		if (isWin)
		{
			//战斗胜利
			faildLabel.Text = "";
			ClearData();
			StartCoroutine("FillRewardList");
		}
		else
		{
			//战斗失败
			faildLabel.Text = GameUtility.GetUIString("UIPnlFriendTab_Label_CombatFriend_Faild_Label");
		}		
		rewardInfoPanel.SetActive(false);
	}

	private void ClearData()
	{
		StopCoroutine("FillRewardList");
		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		foreach (var kvp in SysLocalDataBase.ConvertIdCountList(battleResultData.GetBattleReward()))
		{
			UIElemBattleResultDungeonItem item = rewardPool.AllocateItem().GetComponent<UIElemBattleResultDungeonItem>();
			item.SetData(kvp.first, kvp.second);

			rewardList.AddItem(item.gameObject);
		}
		rewardList.ScrollToItem(0, 0);			
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlFriendTab));
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

	//点击查看阵容按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickLineUp(UIButton btn)
	{
		CombatFriendBattleResultData combatData = battleResultData as CombatFriendBattleResultData;
		RequestMgr.Inst.Request(new QueryFriendPlayerInfoReq(combatData.FriendId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected override IEnumerator ShowRewardPanel()
	{
		Animation ani = null;
		if(resultInfoPanel != null)
			ani = resultInfoPanel.GetComponent<Animation>();
		if (ani != null)
			ani.Play();

		yield return new WaitForSeconds(0.5f);

		if (rewardInfoPanel != null && battleResultData.IsWinner() && SysLocalDataBase.ConvertIdCountList(battleResultData.GetBattleReward()).Count > 0)
			rewardInfoPanel.SetActive(true);	
	}
}
