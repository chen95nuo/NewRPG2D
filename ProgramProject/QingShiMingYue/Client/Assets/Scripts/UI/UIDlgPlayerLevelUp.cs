using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgPlayerLevelUp : UIModule
{
	public SpriteText playerLevelLabel;
	public SpriteText playerLevelBenefitLabel;
	public UIChildLayoutControl rewardControl;
	public SpriteText rewardOneLabel;
	public SpriteText rewardTwoLable;
	public UIChildLayoutControl childLayout;
	public UIButton closeBtn;
	public UIButton viewFuncBtn;

	public AutoSpriteControlBase dlgBg;
	public UIBox contentBg;
	public UIBox rewardBtn;
	public GameObject botBtnBg;

	public UIChildLayoutControl contentLayout;
	public GameObject titleObject;
	public GameObject lvlBenefitObject;
	public GameObject rewardObject;
	public GameObject particleObject;
	public float showTime=1.0f;

	private const float BORDER_EXCEED_HEIGHT = 72f;
	private const float BUTTON_HEIGHT_VAL = -48f;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		dlgBg.gameObject.SetActive(false);

		GameObject particle= ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.levelUpParticle));

		ObjectUtility.AttachToParentAndResetLocalTrans(particleObject, particle);

		StartCoroutine("ShowPanel");

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator ShowPanel()
	{
		yield return new WaitForSeconds(showTime);

		dlgBg.gameObject.SetActive(true);

		SetData();
	}

	private void SetData()
	{
		int playerLevel = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;
		KodGames.ClientClass.Reward reward = SysLocalDataBase.Inst.LocalPlayer.PlayerLevelUpData.Crs.Reward;

		SysLocalDataBase.Inst.LocalPlayer.PlayerLevelUpData = null;
		LevelConfig.PlayerLevel playerLevelConfig = ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByLevel(playerLevel);

		// player level Label
		playerLevelLabel.Text = GameUtility.FormatUIString("UIPlayerLeveup_Level", playerLevel);

		//
		playerLevelBenefitLabel.Text = "";
		if (playerLevelConfig != null)
		{
			// player level up desc
			playerLevelBenefitLabel.Text = GameUtility.GetUIString(playerLevelConfig.levelUpDesc);

			// set the ViewFucButton Data
			viewFuncBtn.Data = playerLevelConfig.linkedUIType;
		}

		// Set Reward.
		var rewardDescs = GetRewardDesc(reward);
		rewardControl.HideChildObj(rewardOneLabel.gameObject, rewardDescs.Count < 1);
		rewardControl.HideChildObj(rewardTwoLable.gameObject, rewardDescs.Count < 2);

		if (rewardDescs.Count > 0)
			rewardOneLabel.Text = rewardDescs[0];

		if (rewardDescs.Count > 1)
			rewardTwoLable.Text = rewardDescs[1];

		// set the Func Button State
		HideOperatorButtons(playerLevelConfig == null || playerLevelConfig.linkedUIType == _UIType.UnKonw);

		contentLayout.HideChildObj(titleObject, false);
		contentLayout.HideChildObj(lvlBenefitObject, string.IsNullOrEmpty(playerLevelBenefitLabel.Text));
		contentLayout.HideChildObj(rewardObject, false);

		dlgBg.SetSize(dlgBg.width, contentBg.height + BORDER_EXCEED_HEIGHT);
		botBtnBg.transform.localPosition = GetPosByY(botBtnBg, BUTTON_HEIGHT_VAL - contentBg.height);
	}

	private Vector3 GetPosByY(GameObject go, float y)
	{
		return new Vector3(go.transform.localPosition.x, y, go.transform.localPosition.z);
	}

	public void HideOperatorButtons(bool hideViewButton)
	{
		childLayout.HideChildObj(closeBtn.gameObject, false);
		childLayout.HideChildObj(viewFuncBtn.gameObject, hideViewButton);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickViewFunc(UIButton btn)
	{
		int uiType = (int)btn.Data;
		if (SysLocalDataBase.Inst.LocalPlayer.UnDoneTutorials.Count <= 0
			|| SysTutorial.Instance.IsNoviceQuestFinished(ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.forceTutorialEndId))
		{
			if (SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState is GameState_CentralCity)
				GameUtility.JumpUIPanel(uiType);
			else
			{
				switch(uiType)
				{
					case _UIType.UI_WolfSmoke:
						SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_Callback(()=>
							{
								if (GameUtility.CheckFuncOpened(_OpenFunctionType.WolfSmoke, true, true))
									RequestMgr.Inst.Request(new QueryWolfSmoke());
							}));
						break;
					case _UIType.UI_Tower:
						SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Tower>();
						break;
					default:
						SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(uiType));
						break;
				}
			}
		}
		else if (SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState is GameState_CentralCity == false)
		{
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>();
		}

		// 单击前往时，清空界面显示队列。
		SysUIEnv.Instance.ClearShowEventsList();

		HideSelf();
	}

	private List<string> GetRewardDesc(KodGames.ClientClass.Reward reward)
	{
		List<string> rewardDescs = new List<string>();
		var rewardLists = SysLocalDataBase.ConvertIdCountList(reward, true);

		for (int index = 0; index < rewardLists.Count; index++)
		{
			Color assetNameColor = GameDefines.textColorBtnYellow;

			switch (IDSeg.ToAssetType(rewardLists[index].first))
			{
				case IDSeg._AssetType.Avatar:
				case IDSeg._AssetType.Equipment:
				case IDSeg._AssetType.CombatTurn:
					assetNameColor = ItemInfoUtility.GetAssetQualityColor(ItemInfoUtility.GetAssetQualityLevel(rewardLists[index].first));
					break;
			}

			rewardDescs.Add(GameUtility.FormatUIString("UIDlgPlayerLevelUp_Reawrd",
													   assetNameColor,
													   ItemInfoUtility.GetAssetName(rewardLists[index].first),
													   GameDefines.textColorWhite,
													   rewardLists[index].second));
		}

		return rewardDescs;
	}
}
