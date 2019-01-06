using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlBattleBar : UIModule
{
	public GameObject skillPos;
	public GameObject superSkillPosTop;
	public GameObject superSkillPosBottom;
	public GameObject continueEffect;
	public UIElemBattleStart battleStartUI;
	public UIElemSpeedUpButtons speedUpButton;
	public UIElemBattleTopBar topBar;
	public UIButton skipButton;

	public SpriteText topLevelText;//波次数

	public UIButton skillCameraAnimationButton;
	public SpriteText round;//回合数
	public UIButton resetCameraButton;
	public GameObject bottomBar;

	public void Start()
	{
		var battleState = SysGameStateMachine.Instance.CurrentState as GameState_Battle;
		int maxRound = battleState.BattleRecordPlayer.BattleRecord.MaxRecordCount;
		round.Text = GameUtility.FormatUIString("UIPnlBattleBar_RoundIndex", 1, maxRound);

		if (battleState.CombatResultAndReward != null)
		{
			//剧情战斗没有“重置视角”按钮
			resetCameraButton.CachedTransform.parent.gameObject.SetActive(!battleState.CombatResultAndReward.IsPlotBattle);
			//剧情战斗没有“加速”按钮
			speedUpButton.gameObject.SetActive(!battleState.CombatResultAndReward.IsPlotBattle);
		}
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		HideAll();

		return true;
	}

	public void HideAll()
	{
		HideContinueButton(true);
		HideBattleStart(true);
		HideSpeedupButton();
		HideSkipButton(true);
		HideTopBar(true);
		HideRound(true);
		HideResetCameraButton(true);
		HideBottomBar(true);
	}

	public void HideContinueButton(bool tf)
	{
		continueEffect.SetActive(!tf);
	}

	public void HideSkipButton(bool tf)
	{
		skipButton.Hide(tf);
	}

	public void HideResetCameraButton(bool tf)
	{
		resetCameraButton.Hide(tf);
	}

	public void HideRound(bool tf)
	{
		round.Hide(tf);
	}

	public void HideSpeedupButton()
	{
		speedUpButton.Hide();
	}

	public void ShowSpeedupButton()
	{
		speedUpButton.Show();
	}

	public void HideTopBar(bool tf)
	{
		topBar.Hide(tf);
	}

	public void HideBottomBar(bool tf)
	{
		bottomBar.SetActive(!tf);
	}

	public void SetTopBarData(KodGames.ClientClass.BattleRecord battleRecord)
	{
		topBar.SetData(battleRecord);
	}

	public void HideBattleStart(bool tf)
	{
		battleStartUI.gameObject.SetActive(!tf);
	}

	//波次数量
	public void UpdateLevelText(int maxLevelCount)
	{
		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;

		int currLevel = battleState.CurrentBattleIndex;
		//开场提示
		if (SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>().BattleType == _CombatType.Tower)
		{
			//千层楼战斗有独有的显示方式
			battleStartUI.SetTowerBattleIdxText(currLevel);
			battleStartUI.SetTowerBattleBGText(SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentLayer + currLevel);
		}
		else
			battleStartUI.battleIdxText.Text = currLevel.ToString() + " - " + battleState.CombatResultAndReward.CombatNumMax.ToString();

		//波次数
		//比武场战斗，不显示波数
		if (SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>().BattleType != _CombatType.Arena)
			//topLevelText.Text = currLevel.ToString() + " - " + battleState.CombatResultAndReward.CombatNumMax.ToString();
			topLevelText.Text = string.Format(GameUtility.GetUIString("UIPnlBattleBar_LevelText"), GameDefines.textColorBtnYellow.ToString(),
								GameDefines.textColorWhite.ToString(), currLevel, battleState.CombatResultAndReward.CombatNumMax);
		else
		{
			//比武场不显示波数
			topLevelText.Text = "";
		}
	}

	//回合数量
	public void UpdateRoundIndex(int roundIndex, int maxRecordCount)
	{
		//回合数
		round.Text = GameUtility.FormatUIString("UIPnlBattleBar_RoundIndex", roundIndex, maxRecordCount);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSkip(UIButton btn)
	{
		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		battleState.SkipBattle();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSkipCameraAnimation(UIButton btn)
	{
		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;

		if (!battleState.BattleRecordPlayer.CanSkipCameraAnimation || !battleState.BattleRecordPlayer.BattleScene.canSkipCameraAniamtion)
		{
			return;
		}

		battleState.BattleRecordPlayer.IsSkipCameraAnimation = true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickReturn(UIButton btn)
	{
		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		SysUIEnv.Instance.ShowUIModule(battleState.BattleResultData.BattleResultUIType, battleState.BattleResultData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickResetCamera(UIButton btn)
	{
		if (KodGames.Camera.main == null)
			return;

		float defaultAngle = ConfigDatabase.DefaultCfg.GameConfig.combatSetting.battleDefault.GetBattleDefaultItemByCombatType(SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>().BattleType).sceneCamAngleHDefault;
		KodGames.Camera.main.GetComponent<CameraController_Touch>().RotateTo(1, defaultAngle, 0, EZAnimation.EASING_TYPE.Default, EZAnimation.EASING_TYPE.Default, false);
	}
}
