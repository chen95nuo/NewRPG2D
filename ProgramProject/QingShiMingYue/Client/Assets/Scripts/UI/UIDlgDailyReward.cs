using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDailyReward : UIModule
{
	public UIScrollList calendarPageScrollList; //天数LIST
	public UIScrollList signInRewardList;//签到奖励LIST
	public GameObjectPool calendarDayPool;//天数池
	public GameObjectPool signInPool;//奖励池
	public UIButton signBtn;//签到
	public UIButton reSignInBtn;//补签
	public SpriteText reSignCostLabel;//签到消耗
	public UIElemAssetIcon reSignInCostIcon;//签到消耗Icon
	public SpriteText titleLabel;//每日签到
	public SpriteText reSignInFreeCountLabel; //Be left of reSignIn count免费补签次数
	public SpriteText signInCountLabel;  //Total SignIn count in this Month本月签到次数
	public static readonly int MaxColunm = 7;
	public float showRewardDelay = 0.6f;
	public UIButton closeBtn;
	public SpriteText remedyRewardLabel;

	private int signType;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		ClearData();
		RefreshView();
		StartCoroutine("FillList");

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public void ClearData()
	{
		StopCoroutine("FillList");
		calendarPageScrollList.ClearList(false);
		calendarPageScrollList.ScrollPosition = 0;

		signInRewardList.ClearList(false);
		signInRewardList.ScrollPosition = 0;
	}

	private void RefreshView()
	{

		DailySignInConfig dailyConfig = ConfigDatabase.DefaultCfg.DailySignInConfig;

		// If month changed , reset the signData.
		if (SysLocalDataBase.Inst.LoginInfo.NowDateTime.Month != SysLocalDataBase.Inst.LocalPlayer.SignData.GetMonth())
		{
			KodGames.ClientClass.SignData signData = SysLocalDataBase.Inst.LocalPlayer.SignData;

			signData.FreeSignCount = dailyConfig.freeRemedySignInTimes;
			signData.SignCount = 0;
			signData.RemedySignCount = 0;
			signData.SignDetails = 0;
			signData.ServerTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime.Year * 10000 + SysLocalDataBase.Inst.LoginInfo.NowDateTime.Month * 100 + SysLocalDataBase.Inst.LoginInfo.NowDateTime.Day;
		}

		// Set the label : title
		titleLabel.Text = GameUtility.FormatUIString("UIPnlDaily_Title", GameDefines.txColorOrange, ItemInfoUtility.GetLevelCN(SysLocalDataBase.Inst.LoginInfo.NowDateTime.Month));

		// Set the label : signInCountLabel
		signInCountLabel.Text = GameUtility.FormatUIString("UIPnlDaily_SignIn_Count", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, SysLocalDataBase.Inst.LocalPlayer.SignData.SignCount, GameDefines.textColorBtnYellow);

		// Set the label : reSignInFreeCountLabel
		//int reSignInMaxCount = dailyConfig.maxRemedySignInTimes;
		int reSignInFreeCount = SysLocalDataBase.Inst.LocalPlayer.SignData.FreeSignCount;

		// Set the cost signIn view.
		if (reSignInFreeCount > 0)
		{
			reSignCostLabel.Text = "";
			reSignInCostIcon.Hide(true);
			reSignInFreeCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDaily_ReSignIn_Count"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, reSignInFreeCount, GameDefines.textColorBtnYellow);
		}
		else
		{
			int reSignCostCount = dailyConfig.remedySignInCosts[0].count + (SysLocalDataBase.Inst.LocalPlayer.SignData.RemedySignCount - dailyConfig.freeRemedySignInTimes) * dailyConfig.remedySignInCostDelta;
			reSignCostLabel.Text = GameUtility.GetUIString("UIPnlDaily_ResignIn_CostLabel");
			reSignInCostIcon.SetData(dailyConfig.remedySignInCosts[0].id);
			reSignInCostIcon.Text = reSignCostCount.ToString();
			reSignInCostIcon.Hide(false);
			reSignInFreeCountLabel.Text = "";
		}

		// Set the ResignButton state.
		reSignInBtn.controlIsEnabled = CanResign();//&& (SysLocalDataBase.Inst.LocalPlayer.SignData.RemedySignCount < reSignInMaxCount);
		if (CanResign())
		{
			//remedyRewardLabel.Text = "";
			string str = string.Format(GameUtility.GetUIString("UIDlgDailyReward_remedyReward_01"), GameDefines.textColorBtnYellow.ToString());
			for (int index = 0; index < ConfigDatabase.DefaultCfg.DailySignInConfig.remedyRewards.Count; index++)
			{
				str += string.Format(GameUtility.GetUIString("UIDlgDailyReward_remedyReward_02"),
					GameDefines.textColorBtnYellow.ToString(), ItemInfoUtility.GetAssetName(ConfigDatabase.DefaultCfg.DailySignInConfig.remedyRewards[index].id),
					GameDefines.textColorWhite.ToString(), ConfigDatabase.DefaultCfg.DailySignInConfig.remedyRewards[index].count);

				if (index < ConfigDatabase.DefaultCfg.DailySignInConfig.remedyRewards.Count - 1)
					str += "\t";
			}
			remedyRewardLabel.Text = str;
		}
		else
		{
			remedyRewardLabel.Text = string.Empty;
		}


		// Set the label : signInBtn
		if (SignInState(SysLocalDataBase.Inst.LoginInfo.NowDateTime.Day))
		{
			signBtn.Text = GameUtility.GetUIString("UIPnlDaily_Quit");
		}
		else
		{
			signBtn.Text = GameUtility.GetUIString("UIPnlDaily_SignIn_Ok");
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public IEnumerator FillList()
	{
		yield return null;

		// Fill the Calendar List
		for (int index = 1; index < MaxColunm; index++)
		{
			UIListItemContainer container = calendarDayPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemDailyRewardCalendarDayItem item = container.GetComponent<UIElemDailyRewardCalendarDayItem>();
			container.Data = item;

			item.SetData(index);
			container.ScanChildren();
			calendarPageScrollList.AddItem(item.gameObject);
		}

		// Fill SignIn Special-Reward's data
		List<DailySignInConfig.StepReward> rewardsList = ConfigDatabase.DefaultCfg.DailySignInConfig.GetMonthRewardByMonth(SysLocalDataBase.Inst.LoginInfo.NowDateTime.Month).stepRewards;
		for (int index = 0; index < rewardsList.Count; index++)
		{
			UIListItemContainer container = signInPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIDlgDailyRewardSignInTemplatePool itemPool = container.GetComponent<UIDlgDailyRewardSignInTemplatePool>();

			container.Data = itemPool;

			DailySignInConfig.StepReward reward = rewardsList[index];

			for (int i = 0; i < itemPool.TemplatePool.Count; i++)
			{
				itemPool.TemplatePool[i].SetData(reward);
				index++;
				reward = index < rewardsList.Count ? rewardsList[index] : null;

				if (reward == null || i == 2)
				{
					reward = null;
					index--;
				}
			}

			signInRewardList.AddItem(container);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSpecialRewardIcon(UIButton btn)
	{
		UIElemDailyRewardSignInItem item = btn.Data as UIElemDailyRewardSignInItem;
		GameUtility.ShowAssetInfoUI(item.signInRewardIcon.AssetId, _UILayer.Top);
	}

	//点击补签
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickReSignIn(UIButton btn)
	{
		RequestMgr.Inst.Request(new SignInReq(1));
	}

	//点击签到
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSignIn(UIButton btn)
	{
		if (signBtn.Text.Equals(GameUtility.GetUIString("UIPnlDaily_Quit")))
			HideSelf();
		else
			RequestMgr.Inst.Request(new SignInReq(0));
	}

	public void OnResponseResignInFail()
	{
		RefreshView();
		StartCoroutine("FillList");
	}

	//签到回来
	public void OnResponeSignInSuccess(KodGames.ClientClass.Reward reward, KodGames.ClientClass.Reward specialReward, int signDay, int signType)
	{
		//Refresh the view 
		RefreshView();

		// Refresh the Calendar.
		for (int index = 0; index < calendarPageScrollList.Count; index++)
		{
			UIElemDailyRewardCalendarDayItem item = calendarPageScrollList.GetItem(index).Data as UIElemDailyRewardCalendarDayItem;
			item.SetSignIconPool(signDay);
		}

		// Refresh the SetpReward View.
		for (int index = 0; index < signInRewardList.Count; index++)
		{
			UIListItemContainer container = signInRewardList.GetItem(index) as UIListItemContainer;
			UIDlgDailyRewardSignInTemplatePool templatePool = container.GetComponent<UIDlgDailyRewardSignInTemplatePool>();
			foreach (UIElemDailyRewardSignInItem item in templatePool.TemplatePool)
				item.SetRewardFinishIconState();
		}

		List<KodGames.ClientClass.Reward> rewards = new List<KodGames.ClientClass.Reward>();
		rewards.Add(reward);
		rewards.Add(specialReward);

		this.signType = signType;
		StartCoroutine("ShowSignInReward", rewards);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator ShowSignInReward(List<KodGames.ClientClass.Reward> rewards)
	{
		yield return new WaitForSeconds(showRewardDelay);

		string rewardDesc = string.Empty;
		if (signType == 0)
			rewardDesc = GameUtility.GetUIString("UI_DlgDaily_Lable1");
		else
			rewardDesc = GameUtility.GetUIString("UI_DlgDaily_Lable3");

		//---------------------------------奖励内容 是否使用统一标题 是否不显示数量 行与行之间是否换行
		rewardDesc += SysLocalDataBase.GetRewardDesc(rewards[0], false, false, true);

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);

		if (rewards[1] != null && SysLocalDataBase.ConvertIdCountList(rewards[1]).Count > 0)
		{
			showData.OnHideCallback = ShowSignInSpecialReward;
			showData.onHideCallbackObj = rewards[1];
		}

		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	private bool ShowSignInSpecialReward(object obj)
	{
		string rewarDesc = GameUtility.FormatUIString("UI_DlgDaily_Lable2", SysLocalDataBase.Inst.LocalPlayer.SignData.SignCount);
		rewarDesc += SysLocalDataBase.GetRewardDesc(obj as KodGames.ClientClass.Reward, false, false, true);

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewarDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);

		return true;
	}

	private static bool CanResign()
	{
		int currentDay = SysLocalDataBase.Inst.LoginInfo.NowDateTime.Day;
		if (currentDay == 1)
		{
			return false;
		}

		if (SysLocalDataBase.Inst.LocalPlayer.SignData.SignDetails == 0)
		{
			return true;
		}

		for (int index = 1; index < currentDay; index++)
		{
			if (!SignInState(index))
			{
				return true;
			}
		}

		return false;
	}

	public static bool SignInState(int day)
	{
		int temp = 1 << (day - 1);

		int daySignIn = SysLocalDataBase.Inst.LocalPlayer.SignData.SignDetails & temp;

		return daySignIn != 0;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}
}
