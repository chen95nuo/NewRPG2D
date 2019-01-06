using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointMonsterBattleResult : UIPnlBattleResultBase
{
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;
	public SpriteText nameLabel;
	public SpriteText tipsLabel;
	public SpriteText FalidLabel;	
	public GameObject winObj;
	public UIBox monsterType;

	public class GuildPointBattleResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;

		private KodGames.ClientClass.StageInfo stageInfo;
		public KodGames.ClientClass.StageInfo StageInfo
		{
			get {return stageInfo;}
		}

		public GuildPointBattleResultData(KodGames.ClientClass.StageInfo stageInfo, KodGames.ClientClass.CombatResultAndReward battleData)
			: base(_UIType.UIPnlGuildPointMonsterBattleResult)
		{
			this.stageInfo = stageInfo;
			this.battleData = battleData;
			this.CombatType = _CombatType.GuildMonster;
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
		var monsterBattleResultData = battleResultData as GuildPointBattleResultData;
		bool isWin = monsterBattleResultData.IsWinner();
			
		winObj.SetActive(isWin);

		if(isWin)
		{
			monsterType.Hide(false);
			nameLabel.Text = monsterBattleResultData.StageInfo.Player.Name;
			if (monsterBattleResultData.StageInfo.EventType == GuildStageConfig._EventType.Enemy)
				monsterType.SetState(0);
			else monsterType.SetState(1);

			tipsLabel.Text = GameUtility.GetUIString("UIPnlGuildPointMonsterBattleResult_Success_Label");
			FalidLabel.Text = "";

			ClearData();
			StartCoroutine("FillRewardList");
		}
		else
		{
			nameLabel.Text = "";
			monsterType.Hide(true);
			tipsLabel.Text = "";
			FalidLabel.Text = GameUtility.GetUIString("UIPnlGuildPointMonsterBattleResult_Faild_Label");
		}

		base.InitViews();
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas)) return false;
		return true;
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

		foreach (var reward in SysLocalDataBase.CCRewardListToShowReward(battleResultData.GetBattleReward()))
		{
			UIElemDanExtraReward item = rewardPool.AllocateItem().GetComponent<UIElemDanExtraReward>();
			item.SetData(reward);

			rewardList.AddItem(item.gameObject);
		}

		//foreach (var kvp in SysLocalDataBase.ConvertIdCountList())
		//{
		//        UIElemBattleResultDungeonItem item = rewardPool.AllocateItem().GetComponent<UIElemBattleResultDungeonItem>();
		//        item.SetData(kvp.first, kvp.second);

		//        rewardList.AddItem(item.gameObject);
		//}

		rewardList.ScrollToItem(0, 0);		
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		GameUtility.JumpUIPanel(_UIType.UI_GuildPoint);
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
