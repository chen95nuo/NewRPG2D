using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;
using KodGames.ClientClass;

public class UIPnlActivityQinInfo : UIModule
{
	public SpriteText continueAnswerCount;
	public SpriteText recoverTime;
	public SpriteText answerCount;
	public SpriteText questionText;
	public UIElemAssetIcon finallyRewardIcon;
	public UIElemAssetIcon rewardIconFirst;
	public UIElemAssetIcon rewardIconSecond;
	public UIButton answerBtn;
	public UIButton getRewardBtn;
	public UIButton getRewardBtn_Enable;
	public UIBox successSprite;
	public GameObject questionObj;
	public GameObject piXiuObj;
	public UIButton rewardIconFristBtn;
	public UIButton rewardIconSecondBtn;
	public GameObject particlePoint;

	public List<UIElemSelectItem> selectAnswers;
	public List<SpriteText> answerTexts;
	public List<UIButton> selectBtns;

	private KodGames.ClientClass.QinInfo myQinInfo;
	private System.DateTime nextUpdateTime;

	public GameObject UIHideRoot;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlActivityTab>().SetActiveButton(_UIType.UIPnlActivityQinInfo);

		//防止在初始化完成前界面异常
		UIHideRoot.SetActive(false);

		RequestMgr.Inst.Request(new QueryQinInfoReq());

		return true;
	}

	public override void OnHide()
	{
		if(particlePoint.transform.childCount>0)
			ObjectUtility.DestroyChildObjects(particlePoint);

		base.OnHide();
	}

	private void Update()
	{
		if (SysLocalDataBase.Inst.LoginInfo.NowDateTime < nextUpdateTime)
			return;

		UpdateLeftTime();

		nextUpdateTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime.AddSeconds(1f);

	}

	private void SetData()
	{
		UIHideRoot.SetActive(true);

		continueAnswerCount.Text = GameUtility.FormatUIString("UIPnlActivityQinInfo_ContinueAnswerCout", myQinInfo.ContinueAnswerCount, ConfigDatabase.DefaultCfg.QinInfoConfig.MaxContinueAnswerCount);

		SetButtnState();

		finallyRewardIcon.SetData(ConfigDatabase.DefaultCfg.QinInfoConfig.ContinueRewards.ShowIcon);

		if (myQinInfo.QuestionId > 0)
		{
			QinInfoConfig.Question info = ConfigDatabase.DefaultCfg.QinInfoConfig.GetQuestionById(myQinInfo.QuestionId);
			questionText.Text = info.Content;
			SetAnswerText(info);
			SetRewardIcon(myQinInfo);
		}

		for (int i = 0; i < selectAnswers.Count; i++)
			selectAnswers[i].SetState(false);
	}

	private void SetRewardIcon(KodGames.ClientClass.QinInfo qinInfo)
	{
		QinInfoConfig.Question info = ConfigDatabase.DefaultCfg.QinInfoConfig.GetQuestionById(qinInfo.QuestionId);

		rewardIconSecond.gameObject.SetActive(true);

		if (info.Rewards.Count > 0)
		{
			rewardIconFirst.SetData(info.Rewards[0].id, info.Rewards[0].count);
			rewardIconFristBtn.data = info.Rewards[0].id;
		}

		if (info.Rewards.Count > 1)
		{
			rewardIconSecond.SetData(info.Rewards[1].id, info.Rewards[1].count);
			rewardIconFristBtn.data = info.Rewards[1].id;
		}
		else
			rewardIconSecond.gameObject.SetActive(false);

	}

	private void SetAnswerText(QinInfoConfig.Question question)
	{
		for (int i = 0; i < selectBtns.Count; i++)
			selectBtns[i].gameObject.SetActive(true);

		int index = 0;

		for (; index < question.Answers.Count; index++)
			answerTexts[index].Text = question.Answers[index].Content;

		for (int i = 0; i < index; i++)
			selectBtns[i].Data = selectAnswers[i];

		if (index >= answerTexts.Count)
			return;

		for (; index < answerTexts.Count; index++)
			selectBtns[index].gameObject.SetActive(false);
	}

	private void UpdateAnimation()
	{
		Animation animation = piXiuObj.GetComponent<Animation>();

		ObjectUtility.DestroyChildObjects(particlePoint);

		animation.Play("QiLin_Idle", PlayMode.StopAll);
	}

	private void UpdateLeftTime()
	{
		if (!SysLocalDataBase.Inst.LocalPlayer.QinInfoAnswerCount.IsPointFull())
		{
			float nextTime = SysLocalDataBase.Inst.LocalPlayer.QinInfoAnswerCount.GetNextGenerationLeftTime(SysLocalDataBase.Inst.LoginInfo.NowTime);

			if (nextTime >= 0)
				if (!GameUtility.Time2String((long)nextTime).Equals(recoverTime.Text))
					recoverTime.Text = GameUtility.Time2String((long)nextTime);
		}
		else
			recoverTime.Text = GameUtility.GetUIString("UIPnlActivityQinInfo_NoTime");

		answerCount.Text = GameUtility.FormatUIString("UIPnlActivityQinInfo_AnswerCout", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, SysLocalDataBase.Inst.LocalPlayer.QinInfoAnswerCount.Point.Value, SysLocalDataBase.Inst.LocalPlayer.QinInfoAnswerCount.MaxPoint);
	}

	private void SetButtnState()
	{

		if (myQinInfo.ContinueAnswerCount >= ConfigDatabase.DefaultCfg.QinInfoConfig.MaxContinueAnswerCount || myQinInfo.QuestionId <= 0)
		{
			successSprite.Hide(false);
			questionObj.SetActive(false);
			getRewardBtn.Hide(false);
			getRewardBtn_Enable.Hide(true);
		}
		else
		{
			questionObj.SetActive(true);
			successSprite.Hide(true);
			getRewardBtn.Hide(true);
			getRewardBtn_Enable.Hide(false);
		}

		if (myQinInfo.QuestionId > 0)
			for (int i = 0; i < selectAnswers.Count; i++)
				selectAnswers[i].InitState(ConfigDatabase.DefaultCfg.QinInfoConfig.GetQuestionById(myQinInfo.QuestionId).Answers[i].AnswerNum, null);

		answerBtn.controlIsEnabled = false;

	}


	#region ResponseCallBack

	public void OnResponseQueryQinInfoSuccess(KodGames.ClientClass.QinInfo qinInfo)
	{
		this.myQinInfo = qinInfo;
		SetData();
	}

	public void OnResponseAnswerSuccess(KodGames.ClientClass.QinInfo qinInfo, bool isRight)
	{

		this.myQinInfo = qinInfo;
		SetData();

		Animation animation = piXiuObj.GetComponent<Animation>();
		AnimationEventHandler ani = piXiuObj.GetComponent<AnimationEventHandler>();
		ani.userEventDelegate = null;
		ani.userData = null;

		animation.Stop();

		ObjectUtility.DestroyChildObjects(particlePoint);

		if (ani != null)
			ani.userEventDelegate = (name, data) =>
			{
				if (name == "PlayAnimation")
					UpdateAnimation();
			};

		if (isRight)
		{
			animation.Play("QiLin_happy", PlayMode.StopAll);

			GameObject particleObj = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFx_QinInfo_Right));
			ObjectUtility.AttachToParentAndResetLocalTrans(particlePoint, particleObj);
		}
		else
		{
			animation.Play("QiLin_yun", PlayMode.StopAll);

			GameObject particleObj = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFx_QinInfo_Wrong));
			ObjectUtility.AttachToParentAndResetLocalTrans(particlePoint, particleObj);
		}
	}

	public void OnResponseGetRewardSuccess(KodGames.ClientClass.QinInfo qinInfo, KodGames.ClientClass.Reward fixReward, KodGames.ClientClass.Reward randomReward)
	{
		this.myQinInfo = qinInfo;
		SetData();

		UIDlgShopGiftPreview.ShowData showdata = new UIDlgShopGiftPreview.ShowData();

		var rewardData = new UIDlgShopGiftPreview.RewardData();
		rewardData.rewards = SysLocalDataBase.CCRewardToCSCReward(fixReward);
		rewardData.title = GameUtility.GetUIString("UIPnlActivityQinInfo_MessageTitle");
		showdata.rewardDatas.Add(rewardData);

		List<ClientServerCommon.Reward> rewards = SysLocalDataBase.CCRewardToCSCReward(randomReward);

		if (rewards != null && rewards.Count > 0)
		{
			rewardData = new UIDlgShopGiftPreview.RewardData();
			rewardData.rewards = rewards;
			rewardData.title = GameUtility.GetUIString("UIPnlActivityQinInfo_MessageRandom");
			showdata.rewardDatas.Add(rewardData);
		}

		showdata.title = GameUtility.GetUIString("UIPnlActivityQinInfo_MessageTitle");

		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgShopGiftPreview), _UILayer.Top, showdata);
	}

	#endregion

	#region Click

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectClick(UIButton btn)
	{
		for (int i = 0; i < selectAnswers.Count; i++)
		{
			UIElemSelectItem item = btn.Data as UIElemSelectItem;
			if ((int)selectAnswers[i].Data == (int)(item.Data))
				selectAnswers[i].SetState(true);
			else
				selectAnswers[i].SetState(false);
		}

		answerBtn.controlIsEnabled = true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRankClick(UIButton btn)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("FUNC_CLOSE"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetRewardClick(UIButton btn)
	{
		RequestMgr.Inst.Request(new GetQinInfoContinueRewardReq());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAnswerClick(UIButton btn)
	{

		if (SysLocalDataBase.Inst.LocalPlayer.QinInfoAnswerCount.Point.Value <= 0)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityQinInfo_NotCount"));
			return;
		}

		int selectNum = -1;
		for (int i = 0; i < selectAnswers.Count; i++)
		{
			if (selectAnswers[i].IsSelected)
			{
				selectNum = (int)selectAnswers[i].Data;
				break;
			}
		}

		RequestMgr.Inst.Request(new AnswerQinInfoReq(myQinInfo.QuestionId, selectNum));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnShowRewardClick(UIButton btn)
	{
		UIDlgShopGiftPreview.ShowData showdata = new UIDlgShopGiftPreview.ShowData();

		var rewardData = new UIDlgShopGiftPreview.RewardData();
		rewardData.rewards = ConfigDatabase.DefaultCfg.QinInfoConfig.ContinueRewards.FixDisplayRewards;
		rewardData.title = GameUtility.GetUIString("UIPnlShop_FixedReward");
		showdata.rewardDatas.Add(rewardData);

		List<ClientServerCommon.Reward> rewards = ConfigDatabase.DefaultCfg.QinInfoConfig.ContinueRewards.RandomDisplayRewards;

		if (rewards != null && rewards.Count > 0)
		{
			rewardData = new UIDlgShopGiftPreview.RewardData();
			rewardData.rewards = rewards;
			rewardData.title = GameUtility.GetUIString("UIPnlPVERodomReward_Title_Random");
			showdata.rewardDatas.Add(rewardData);
		}

		showdata.title = GameUtility.GetUIString("UIDlgShopGiftPreview_Title");

		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgShopGiftPreview), _UILayer.Top, showdata);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRewardClick(UIButton btn)
	{
		GameUtility.ShowAssetInfoUI((int)btn.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnEnableRewardClick(UIButton btn)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlActivityQinInfo_GetRewardFalse",
			ConfigDatabase.DefaultCfg.QinInfoConfig.MaxContinueAnswerCount - myQinInfo.ContinueAnswerCount));
	}
	#endregion
}
