using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.Effect;

public class UIPnlBattleResultBase : UIModule
{
	public enum AppraiseType
	{
		One_Appraise,
		Two_Appraise,
		Three_Appraise,
	}

	public abstract class BattleResultData
	{
		protected int battleResultUIType;
		public int BattleResultUIType { get { return battleResultUIType; } }

		private int combatType;
		public int CombatType
		{
			get { return combatType; }
			set { combatType = value; }
		}

		private int rating = 0;
		public int Rating
		{
			get { return rating; }
			set { rating = value; }
		}

		public BattleResultData(int battleResultUIType)
		{
			this.battleResultUIType = battleResultUIType;
		}

		public virtual bool CanShowView() { return false; }
		public virtual bool IsWinner() { return false; }
		public virtual int GetAppraiseNumber() { return -1; }
		public virtual bool CanShowLvlInformation() { return false; }
		public virtual string GetGoldRewardOrOtherStr() { return string.Empty; }
		public virtual string GetExpRewardOrOtherStr() { return string.Empty; }
		public virtual KodGames.ClientClass.Reward GetBattleReward() { return null; }
		public virtual KodGames.ClientClass.Reward GetFirstPassReward() { return null; }
		//是否有首次通关奖励
		public virtual bool HasFirstPassReward() { return false; }
		public virtual bool CanShowFailGuid() { return false; }

		public virtual bool CanSkipBattle()
		{
			var setting = ConfigDatabase.DefaultCfg.GameConfig.GetCombatSkipSetting(this.combatType, this.rating);

			bool canSkip = false;
			if (setting != null && setting.canSkip)
				canSkip = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level >= setting.playerLevel && SysLocalDataBase.Inst.LocalPlayer.VipLevel >= setting.vipLevel;

			return canSkip;
		}

		public virtual float GetSkipBattleTime()
		{
			var setting = ConfigDatabase.DefaultCfg.GameConfig.GetCombatSkipSetting(this.combatType, this.rating);
			return setting != null ? setting.delayTime / 1000f : 0;
		}
	}

	public UIBox btn_BattleResult;
	public List<AutoSpriteControlBase> btn_Appraises;

	public SpriteText topRewardGoldOrOtherText;
	public SpriteText topRewardExpOrOtherText;

	public GameObject topLvlObject;
	public SpriteText topLvlText;
	public UIProgressBar topLvlProgress;
	public SpriteText expText;

	public AutoSpriteControlBase guidOne;
	public AutoSpriteControlBase guidTwo;
	public AutoSpriteControlBase guidThree;
	public AutoSpriteControlBase guidFour;

	public GameObject resultInfoPanel;
	public GameObject rewardInfoPanel;
	public UIChildLayoutControl starLayout;
	public GameObject loseInfoPanel;
	public GameObject backBtn;
	public GameObject backGround;
	public float showTime;

	protected BattleResultData battleResultData;
	private FXController fxController;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		if (guidOne != null)
		{
			guidOne.Data = _UIType.UIPnlAvatar;
			guidTwo.Data = _UIType.UIPnlAvatar;
			guidThree.Data = _UIType.UIPnlAvatar;
			guidFour.Data = _UIType.UIPnlGuide;
		}

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		battleResultData = userDatas[0] as BattleResultData;

		if (battleResultData.CanShowView())
			InitViews();

		// Set CombatFail View.
		SetCombatFailView();

		// Set the combat Music.
		PlayCombatResultMusic();

		return true;
	}

	public override void OnHide()
	{
		AudioManager.Instance.StopMusic();

		base.OnHide();
	}

	public virtual void DoOnClose()
	{
	}

	public virtual void InitViews()
	{
		if (backBtn != null)
		{
			Animation animation = backBtn.GetComponent<Animation>();
			animation.Play();
		}

		if (resultInfoPanel != null)
			resultInfoPanel.SetActive(false);
		if (loseInfoPanel != null)
			loseInfoPanel.SetActive(false);

		if (loseInfoPanel != null)
			loseInfoPanel.SetActive(false);

		if (btn_BattleResult != null)
			btn_BattleResult.SetToggleState(battleResultData.IsWinner() ? 0 : 1);

		int starCount = battleResultData.GetAppraiseNumber();

		if (starLayout != null && starLayout.childLayoutControls.Length > 0)
		{
			for (int i = 0; i < starCount; i++)
				starLayout.HideChildObj(starLayout.childLayoutControls[i].gameObject, false);

			for (int i = starCount; i < starLayout.childLayoutControls.Length; i++)
				starLayout.HideChildObj(starLayout.childLayoutControls[i].gameObject, true);

			for (int i = 0; i < starCount; i++)
				starLayout.childLayoutControls[i].gameObject.SetActive(false);
		}

		if (backBtn != null)
		{
			AnimationEventHandler ani = backBtn.GetComponent<AnimationEventHandler>();

			if (ani != null)
				ani.userEventDelegate = (name, data) =>
				{
					if (name == "PlayBattleResult")
						PlayBattleResult();
				};
		}

		// Top reward : gold and exp(some times exp is badge)
		if (topRewardGoldOrOtherText != null)
			topRewardGoldOrOtherText.Text = battleResultData.GetGoldRewardOrOtherStr();
		if (topRewardExpOrOtherText != null)
			topRewardExpOrOtherText.Text = battleResultData.GetExpRewardOrOtherStr();

		// Level
		if (topLvlObject != null)
		{
			bool canShowLvlInformation = battleResultData.CanShowLvlInformation();
			topLvlObject.SetActive(canShowLvlInformation);
			if (canShowLvlInformation)
			{
				int level = System.Math.Min(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level, ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel);
				topLvlText.Text = string.Format(GameUtility.GetUIString("UIPnlPVEBattleResult_Lable_PlayerLevel"), level);
				topLvlProgress.Value = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience / (float)ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(level).playerExp;

				int actualLevel = Mathf.Min(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level, ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel);
				if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience > ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp)
					expText.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob"), ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp, ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp);
				else
					expText.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob"), SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience, ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp);
			}
		}
	}

	private void SetCombatFailView()
	{
		bool showCombatFail = battleResultData.CanShowFailGuid() && !battleResultData.IsWinner();
		if (guidOne != null)
		{
			guidOne.Hide(!showCombatFail);
			guidTwo.Hide(!showCombatFail);
			guidThree.Hide(!showCombatFail);
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignBattleResult))
			{
				guidFour.Hide(true);
			}
			else
				guidFour.Hide(!showCombatFail);
		}
	}

	private void PlayCombatResultMusic()
	{
		bool isWin = battleResultData.IsWinner();
		string music = isWin ? ConfigDatabase.DefaultCfg.GameConfig.combatMusic.combatWin : ConfigDatabase.DefaultCfg.GameConfig.combatMusic.combatFail;

		if (AudioManager.Instance.IsMusicPlaying(music) == false)
		{
			AudioManager.Instance.StopMusic();
			AudioManager.Instance.PlayMusic(music, false);
		}
	}

	private void PlayBattleResult()
	{
		if (battleResultData.IsWinner())
		{
			fxController = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.battleResultWin)).GetComponent<FXController>();
			ObjectUtility.AttachToParentAndResetLocalTrans(btn_BattleResult.gameObject, fxController.gameObject);
		}
		else
		{
			fxController = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.battleResultLose)).GetComponent<FXController>();
			ObjectUtility.AttachToParentAndResetLocalTrans(btn_BattleResult.gameObject, fxController.gameObject);
		}
		StartCoroutine("PlayStarAnimation");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator PlayStarAnimation()
	{
		yield return new WaitForSeconds(0.8f);

		int starCount = battleResultData.GetAppraiseNumber();

		if (starCount < 1 || starLayout == null || starLayout.childLayoutControls.Length <= 0)
		{
			if (resultInfoPanel != null)
				resultInfoPanel.SetActive(true);

			StartCoroutine("ShowRewardPanel");

		}
		else
		{
			StartCoroutine("SetStarParticle", 0);
		}
	}

	//更改此函数时注意修改UIPnlCampaignBattleResult中的重写函数。
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected virtual IEnumerator ShowRewardPanel()
	{
		if (resultInfoPanel != null)
		{
			Animation ani = resultInfoPanel.GetComponent<Animation>();
			if (ani != null)
				ani.Play();
		}


		yield return new WaitForSeconds(0.5f);

		if (rewardInfoPanel != null && battleResultData.IsWinner())
			rewardInfoPanel.SetActive(true);
		else
		{
			if (loseInfoPanel != null)
				loseInfoPanel.SetActive(true);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator SetStarParticle(int index)
	{
		yield return new WaitForSeconds(showTime);

		int starCount = battleResultData.GetAppraiseNumber();

		if (index >= starCount)
		{
			resultInfoPanel.SetActive(true);
			StartCoroutine("ShowRewardPanel");
		}
		else
		{
			starLayout.childLayoutControls[index].gameObject.SetActive(true);

			Animation ani = starLayout.childLayoutControls[index].gameObject.GetComponentInChildren<Animation>();

			if (ani != null)
				ani.Play();
			else
				Debug.LogError("StarAnimation not found !");

			AnimationEventHandler eventHandler = starLayout.childLayoutControls[index].gameObject.GetComponentInChildren<AnimationEventHandler>();

			if (eventHandler == null)
				Debug.LogError("BattleAppraise lose AnimationEventHandler");
			else
				eventHandler.userEventDelegate = (name, data1) =>
				{
					if (name.Equals("PlayStarParticle"))
					{
						FXController starController = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.battleStarParticle)).GetComponent<FXController>();
						ObjectUtility.AttachToParentAndResetLocalTrans(starLayout.childLayoutControls[index].gameObject, starController.gameObject);

						CameraShaker.Shake(backGround, 10.0f, 0.3f, 0.01f);

						eventHandler.userEventDelegate = null;
					}
				};

			StartCoroutine("SetStarParticle", index + 1);
		}

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBattleFailGuid(AutoSpriteControlBase controllBase)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignBattleResult))
		{
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.InterruptCampaign = false;

			var campaignBattleResultData = battleResultData as UIPnlCampaignBattleResult.CampaignBattleResultData; ;

			campaignBattleResultData.EnterUIType = (int)controllBase.Data;

			if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(campaignBattleResultData.ZoneId))
				SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_ActivityDungeon>(battleResultData);
			else
				SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Dungeon>(battleResultData);
		}
		else
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI((int)controllBase.Data));
	}
}
