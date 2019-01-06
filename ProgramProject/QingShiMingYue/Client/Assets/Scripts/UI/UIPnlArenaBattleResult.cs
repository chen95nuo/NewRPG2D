using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlArenaBattleResult : UIPnlBattleResultBase
{
	public SpriteText badgeLabel;
	public SpriteText rewardTitle;
	public SpriteText rewardMessage;

	public class ArenaBattleResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;
		public KodGames.ClientClass.CombatResultAndReward BattleData { get { return battleData; } }

		private int selfRank;
		public int SelfRank { get { return selfRank; } }

		public ArenaBattleResultData(KodGames.ClientClass.CombatResultAndReward battleData, int selfRank)
			: base(_UIType.UIPnlArenaBattleResult)
		{
			this.battleData = battleData;
			this.selfRank = selfRank;
			this.CombatType = _CombatType.Arena;
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
			return battleData.StarCompleteIndexs.Count;
		}

		public override bool CanShowLvlInformation()
		{
			return true;
		}

		public override string GetGoldRewardOrOtherStr()
		{
			int count = 0;
			string name = "";

			if (this.battleData.Rewards == null) return "";

			if (this.battleData.Rewards.Count <= 0 || this.battleData.Rewards[0].Consumable == null || this.battleData.Rewards[0].Consumable.Count <= 0)
				return "";

			if (this.battleData.Rewards[0].Consumable[0] != null)
			{
				name = ItemInfoUtility.GetAssetName(battleData.Rewards[0].Consumable[0].Id);
				count = battleData.Rewards[0].Consumable[0].Amount;

				return GameUtility.FormatUIString("UIPnlBattleResult_Reward_Label", GameDefines.textColorBtnYellow, name, GameDefines.textColorWhite, count);
			}
			else
				return "";


		}

		public override string GetExpRewardOrOtherStr()
		{
			int count = 0;
			string name = "";

			if (this.battleData.Rewards == null) return "";

			if (this.battleData.Rewards.Count <= 0 || this.battleData.Rewards[0].Consumable == null || this.battleData.Rewards[0].Consumable.Count <= 1)
				return "";

			if (this.battleData.Rewards[0].Consumable[1] != null)
			{
				name = ItemInfoUtility.GetAssetName(battleData.Rewards[0].Consumable[1].Id);
				count = battleData.Rewards[0].Consumable[1].Amount;

				return GameUtility.FormatUIString("UIPnlBattleResult_Reward_Label", GameDefines.textColorBtnYellow, name, GameDefines.textColorWhite, count);
			}
			else
				return "";

		}

		public string GetBadgeStr()
		{
			int badgeCount = 0;
			if (this.battleData.Rewards[0].Consumable != null)
			{
				var consumable = ItemInfoUtility.FindConsumableById(battleData.Rewards[0].Consumable, IDSeg._SpecialId.Badge);
				if (consumable != null)
					badgeCount = consumable.Amount;
			}

			return string.Format(GameUtility.GetUIString("UIPnlBattleResult_Badge_Label"), badgeCount);
		}

		public override KodGames.ClientClass.Reward GetBattleReward()
		{
			return battleData.Rewards[0];
		}

		public override bool CanShowFailGuid()
		{
			return true;
		}
	}

	public override void InitViews()
	{
		base.InitViews();

		var arenaBattleResultData = battleResultData as ArenaBattleResultData;

		bool isWin = arenaBattleResultData.IsWinner();

		// Set the badge label.
		//badgeLabel.Text = arenaBattleResultData.GetBadgeStr();

		if (isWin)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.ArenaData.SelfRank != arenaBattleResultData.SelfRank)
			{
				rewardTitle.Text = "";
				rewardMessage.Text = GameUtility.FormatUIString("UIPnlBattleResult_Arena_WinRewardMessage", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, arenaBattleResultData.SelfRank);
			}
			else
			{
				rewardTitle.Text = "";
				rewardMessage.Text = GameUtility.FormatUIString("UIPnlBattleResult_Arena_FailRewardRankMessage", GameDefines.textColorBtnYellow);
			}
		}
		else
		{
			rewardTitle.Text = GameUtility.GetUIString("UIPnlBattleResult_Arena_FailRewardMessage");
			rewardMessage.Text = GameUtility.FormatUIString("UIPnlBattleResult_Arena_FailRewardRankMessage", GameDefines.textColorBtnYellow);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		//HideSelf();

		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlArena));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnReplayClick(UIButton btn)
	{
		HideSelf();

		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		battleState.ReplayBattle();
	}
}