using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemBattleStart : MonoBehaviour
{
	//千层楼方式：第X（汉字艺术字）战—— 挑战Y（数字）层
	public SpriteText towerBattleBgText;
	public SpriteText towerBattleIdxText;
	//普通方式：回合数-最大回合数
	public SpriteText battleIdxText;

	void Start()
	{
		if (SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>().BattleType == _CombatType.Tower)
		{
			//暂时不hide，battleIdxText有动画towerBattleBgText和towerBattleIdxText处于battleIdxText子节点
			battleIdxText.Text = "";
		}
		else
		{
			towerBattleBgText.Text = "";
			towerBattleIdxText.Text = "";
		}
	}

	public void SetTowerBattleIdxText(int battleIdx)
	{
		if (battleIdx < 1 || battleIdx > 8)
		{
			Debug.LogError("千机楼战斗 battleIdx超出范围，无法设置UI");
			return;
		}

		string idxKey = string.Format("UI_CN_BreakLevel_{0}", battleIdx);
		towerBattleIdxText.Text = GameUtility.FormatUIString("MelaleucaFloorBattleStartIdx_BattleIdxText", GameUtility.GetUIString(idxKey));
	}

	public void SetTowerBattleBGText(int floorCount)
	{
		towerBattleBgText.Text = GameUtility.FormatUIString("MelaleucaFloorBattleStartIdx_BGText", floorCount);
	}
}