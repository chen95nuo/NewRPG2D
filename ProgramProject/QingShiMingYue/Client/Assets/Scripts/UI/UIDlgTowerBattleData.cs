using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KodGames;
using ClientServerCommon;
public class UIDlgTowerBattleData : UIModule
{
	public UIScrollList battleInfoList;
	public GameObjectPool battleInfoPool;

	private KodGames.ClientClass.CombatResultAndReward battleData;

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
		if(userDatas[0] is KodGames.ClientClass.CombatResultAndReward)
			battleData = userDatas[0] as KodGames.ClientClass.CombatResultAndReward;

		init();
		return true;
	}
	public void ClearData()
	{
		StopCoroutine("FillList");

		battleInfoList.ClearList(false);
		battleInfoList.ScrollPosition = 0f;
	}


	private void init()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		BattleDataAnalyser analyser = new BattleDataAnalyser(battleData);

		for (int index = 0; index < analyser.BattleCount; index ++)
		{
			bool isWin = battleData.BattleRecords[index].TeamRecords[0].IsWinner;

			UIListItemContainer uiContainer = battleInfoPool.AllocateItem().GetComponent<UIListItemContainer>();			
			UIElemTowerBattleDataLayer item = uiContainer.GetComponent<UIElemTowerBattleDataLayer>();
			uiContainer.data = item;
			item.SetData(index, analyser, isWin);
			battleInfoList.AddItem(item.gameObject);
		}
		
		//当最后一场战斗为失败
		if (!battleData.BattleRecords[analyser.BattleCount - 1].TeamRecords[0].IsWinner)
			battleInfoList.ScrollToItem(analyser.BattleCount-1,0);

		//battleInfoList.ScrollListTo((float)(analyser.BattleCount -1));
	}


	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		HideSelf();
	}
}
