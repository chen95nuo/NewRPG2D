using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAdventureCombatResult : UIPnlBattleResultBase
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

		public CombatFriendBattleResultData(KodGames.ClientClass.CombatResultAndReward battleData)
			: base(_UIType.UIPnlAdventureCombatResult)
		{
			this.battleData = battleData;
			this.CombatType = _CombatType.Adventure;
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
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas)) return false;
		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		//SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlFriendTab));
		//GameUtility.JumpUIPanel(_UIType.UI_Adventrue, UIPnlAdventureScene.combatMarvellouseProto);
		SysGameStateMachine.Instance.EnterState<GameState_Adventure>();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnReplayClick(UIButton btn)
	{
		HideSelf();

		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		battleState.ReplayBattle();
	}

}
