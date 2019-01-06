using UnityEngine;
using ClientServerCommon;
using System.Collections;
using System.Collections.Generic;
using KodGames.ExternalCall;

public class UIPnlCentralCityPlayerInfo : UIPnlPlayerInfoBase
{
	public GameObject assistantObj;
	public List<UIElemCentralCityTempItem> layoutItems;

	public UIBox activityNotify;
	public UIButton activityBtn;

	public UIChildLayoutControl layoutCoutrol;
	public UIButton signBtn;
	public GameObject signFlag;


	public GameObject chatMessageEffect;
	public GameObject emailMessageEffect;
	public GameObject assistantParticleRoot;

	//711
	public UIButton convertButton;
	public GameObject convertSpe;

	//f打开网页
	public UIButton webPageButton;

	private float delta = 0f;
	private GameObject newMessageParticle;
	private GameObject assistantParticle;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		chatMessageEffect.SetActive(false);
		emailMessageEffect.SetActive(false);

		// 注册活动按钮绿点的数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Activity, UpdateActivityNotify);
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Unknown, UpDataActivityConvertNotify);
		return true;
	}

	public override void Dispose()
	{
		base.Dispose();

		// 取消活动按钮绿点的数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Activity, UpdateActivityNotify);
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Unknown, UpDataActivityConvertNotify);
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		InitTempItems();

		// Activity.
		UpdateActivityNotify();
		UpDataActivityConvertNotify();

		// Assistant Animation.
		SetAssistantAnimation();

		// Daily Sign.
		UpdateSignbtnState();

		//webPage
		UpdateWebPageState();

		//Show fade bg
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().ShowFadeBg();

		return true;
	}

	public override void OnHide()
	{
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().ShowColorBg();

		base.OnHide();
	}

	public override void Overlay()
	{
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().ShowColorBg();
		base.Overlay();
	}

	public override void RemoveOverlay()
	{
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().ShowFadeBg();
		base.RemoveOverlay();
	}

	private void Update()
	{
		delta += Time.deltaTime;

		if (delta > 1)
		{
			delta = 0f;

			// Refresh Common UI.
			RefreshView();

			// Update Fixed Button.
			SetAssistantAnimation();
			UpdateSignbtnState();
			UpdataChatAndEmailState();

			// Update unFixed Button.
			UpdateTempButtons();
		}

		UpdateActivityBtn();
		UpDataActivityConvertBtn();
	}

	#region  Activity Notify
	public void UpdateActivityNotify()
	{
		if (this.IsShown && !this.IsOverlayed)
		{
			bool showNotify = false;

			foreach (var state in SysLocalDataBase.Inst.LocalPlayer.FunctionStates)
			{
				if (state.id == GreenPointType.RunActivityAccumulative || state.id == GreenPointType.ZentiaServerReward)
				{
					if (state.isOpen)
					{
						showNotify = state.isOpen;
						break;
					}
				}



			}

			activityNotify.Hide(!showNotify);
		}
	}

	public void UpdateActivityBtn()
	{
		List<int> activity = null;
		if (ActivityManager.Instance != null)
			activity = ActivityManager.Instance.GetActivityIdInRunActivity();


		if (activityBtn.IsHidden() != (!(activity != null && activity.Count >= 1)))
			activityBtn.Hide(!(activity != null && activity.Count >= 1));

		if (activityBtn.IsHidden() == true)
			activityNotify.Hide(true);
	}

	public void UpDataActivityConvertNotify()
	{
		if (this.IsShown && !this.IsOverlayed)
		{
			bool showNotify = false;

			foreach (var state in SysLocalDataBase.Inst.LocalPlayer.FunctionStates)
			{
				if (state.id != GreenPointType.SEVENELEVENGIFT)
					continue;

				if (state.isOpen && convertButton.IsHidden() == false && SysLocalDataBase.Inst.LocalPlayer.Function.ShowSevenElevenGift)
				{
					showNotify = true;
					break;
				}
			}

			convertSpe.SetActive(showNotify);
		}
	}

	public void UpDataActivityConvertBtn()
	{
		if (ConfigDatabase.DefaultCfg.GameConfig.isConvertExpain)
		{
			if (ActivityManager.Instance != null)
			{
				if (ActivityManager.Instance.GetActivity<ActivityConvert>() != null)
				{
					if ((convertButton.IsHidden() == ActivityManager.Instance.GetActivity<ActivityConvert>().IsActive))
						convertButton.Hide(!ActivityManager.Instance.GetActivity<ActivityConvert>().IsActive);
				}
				else
				{
					if (convertButton.IsHidden() == false)
						convertButton.Hide(true);
				}
			}
		}
		else
		{
			if (convertButton.IsHidden() == false)
				convertButton.Hide(true);
		}

	}
	#endregion

	#region Fixed Button
	private void ShowAssistGuidView(bool show)
	{
		int guidPlayerLevel = ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.assistantParticleCloseLevel;

		if (show)
		{
			if (assistantParticle == null)
			{
				assistantParticle = ResourceManager.Instance.InstantiateAsset<GameObject>(
					KodGames.PathUtility.Combine(GameDefines.uiEffectPath, guidPlayerLevel <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level ? GameDefines.assistantParticle : GameDefines.assistantParticleStrength));

				ObjectUtility.AttachToParentAndKeepLocalTrans(assistantParticleRoot, assistantParticle);
			}
			else
			{
				var animation = assistantParticle.GetComponentInChildren<Animation>();
				if (animation != null)
					animation.Play();
			}
		}
		else
		{
			if (assistantParticle != null)
				GameObject.Destroy(assistantParticle);
		}
	}

	private void SetAssistantAnimation()
	{
		var animation = assistantObj.GetComponentInChildren<Animation>();
		bool showAnima = SysLocalDataBase.Inst.LocalPlayer.TaskData.NewTaskAmount > 0;

		bool showQuest = false;
		//判断列表是不是空
		if (SysLocalDataBase.Inst.LocalPlayer.QuestData.Quests != null)
		{
			foreach (var quest in SysLocalDataBase.Inst.LocalPlayer.QuestData.Quests)
			{
				if (quest.Phase >= QuestConfig._PhaseType.Finished && quest.Phase < QuestConfig._PhaseType.FinishedAndGotReward)
				{
					//找到有一个完成的即可退出
					showQuest = true;
					break;
				}
			}
		}

		if (SysLocalDataBase.Inst.LocalPlayer.QuestData.QuestQuick != null)
			//列表为空但是系统数据不为零，说明有完成的
			showQuest = SysLocalDataBase.Inst.LocalPlayer.QuestData.QuestQuick.CanPickDailyQuestsCount > 0 ? true : showQuest;

		//取小助手和任务的和
		showAnima = showAnima || showQuest;

		ShowAssistGuidView(showAnima);

		if (!animation.IsPlaying("QiLin_Idle"))
			animation.Play("QiLin_Idle", PlayMode.StopSameLayer);
	}

	private void UpdateSignbtnState()
	{
		signFlag.SetActive(GameUtility.CheckFuncOpened(_OpenFunctionType.DailyReward, false, true) && !UIDlgDailyReward.SignInState(SysLocalDataBase.Inst.LoginInfo.NowDateTime.Day));
	}

	private void UpdateWebPageState()
	{
		webPageButton.Hide(!ConfigDatabase.DefaultCfg.GameConfig.isShowWebPage);
	}

	private void UpdataChatAndEmailState()
	{
		chatMessageEffect.SetActive(SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadPrivateChatMsgs || SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadWorldChatMsgCount > 0 || SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount > 0 || SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadGuildChatCount > 0);

		int emailNum = SysLocalDataBase.Inst.LocalPlayer.EmailData.GetNewEmailCount(_EmailDisplayType.System) + 
			SysLocalDataBase.Inst.LocalPlayer.EmailData.GetNewEmailCount(_EmailDisplayType.Friend) + 
			SysLocalDataBase.Inst.LocalPlayer.EmailData.GetNewEmailCount(_EmailDisplayType.Combat) +
			SysLocalDataBase.Inst.LocalPlayer.EmailData.GetNewEmailCount(_EmailDisplayType.Guild);
		emailMessageEffect.SetActive(emailNum > 0);
	}
	#endregion

	#region TempButton
	private void InitTempItems()
	{
		foreach (var item in layoutItems)
			item.Init();

		UpdateTempButtons();
	}

	private void UpdateTempButtons()
	{
		for (int index = 0; index < layoutItems.Count; index++)
			layoutItems[index].Update();

		if (layoutCoutrol.childLayoutControls.Length != layoutItems.Count)
		{
			Debug.LogError("UIPnlCentralCityInfo : LayoutItem's Count is not equal with layoutCoutrol 's count.");
			return;
		}

		for (int index = 0; index < layoutCoutrol.childLayoutControls.Length; index++)
			layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[index].gameObject, layoutItems[index].IsHidden);
	}

	#endregion

	#region Event
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDailyGuidClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlQuest);
	}



	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMessageClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlEmail);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPlayerDetailClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPlayerAttrTip));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnVipBtnClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlRechargeVip));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChatBtnClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChatTab));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnWebPageBtnClick(UIButton btn)
	{
		Application.OpenURL(GameUtility.GetUIString("UIPnlCentralCityPlayerInfo_WebPageUrl"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnEmailBtnClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEmail));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnActivityBtnClick(UIButton btn)
	{
		List<int> activity = ActivityManager.Instance.GetActivityIdInRunActivity();
		if (activity == null || activity.Count <= 0)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlRunAccumulativeTab_CentralCity"));
		else
		{
			//Select Activity UI.
			var activityCfg = ConfigDatabase.DefaultCfg.GameConfig.GetOperationActivityByType(ActivityManager.Instance.GetActivityInRunActivity(activity[0]).ActivityData.ActivityType);
			if (activityCfg != null)
			{
				int ui = _UIType.UnKonw;
				switch (activityCfg.activityType)
				{
					case _ActivityType.ACCUMULATEACTIVITY:
						ui = _UIType.UIPnlRunAccumulativeTab;
						break;
					case _ActivityType.ZENTIA:
						if (ActivityZentia.Instance.IsOpen)
							ui = _UIType.UIPnlEastSeaFindFairyMain;
						else
							ui = _UIType.UIPnlEastSeaCloseActivity;
						break;
				}

				if (ui != _UIType.UnKonw)
					SysUIEnv.Instance.ShowUIModule(ui, activity[0]);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSignBtnClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDailyReward));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAssistantClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAssistantTask));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGameMoney(UIButton btn)
	{
		/************************************************************************/
		//游戏币跳转规则：
		//***********进入秘境指定副本
		/************************************************************************/
		//根据配置文件获取到副本ID
		CampaignData.OpenCampaignView(ConfigDatabase.DefaultCfg.CampaignConfig.gameMoneyGetDungeonId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRealMoney(UIButton btn)
	{
		/************************************************************************/
		// Show UIDlgShopBuyTips
		//规则：直接跳到充值界面
		/************************************************************************/
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlRecharge);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickStamina(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAffordCost), IDSeg._SpecialId.Stamina);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnConvertexpain(UIButton btn)
	{
		RequestMgr.Inst.Request(new QuerySevenElevenGiftReq(DevicePlugin.GetGUID()));
	}
	#endregion
}
