using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using System;

public class UIDlgMonthCardView : UIModule
{
	public class ShowData
	{
		public List<RewardData> rewardDatas = new List<RewardData>();
		public bool showCountInName;
	}

	public class RewardData
	{
		public string title;
		public List<ClientServerCommon.Reward> rewards = new List<ClientServerCommon.Reward>();

		public RewardData() { }

		public RewardData(List<ClientServerCommon.Reward> rewards) : this(string.Empty, rewards) { }

		public RewardData(string title, List<ClientServerCommon.Reward> rewards)
		{
			this.title = title;
			this.rewards = rewards;
		}
	}

	public GameObjectPool gameObjectPool;
	public GameObjectPool titlePool;
	public UIScrollList scrollList;
	public SpriteText titleLabel;
	public SpriteText messageLabel;
	public UIButton okBtn;
	public UIButton closeBtn;
	public UIButton getRewardBtn;
	public UIBox rewardPicked;
	public UIChildLayoutControl btnLayout;

	private const int MAX_COLUMN_NUM = 3;
	private int rewardType;
	private com.kodgames.corgi.protocol.OneMonthCardInfo monthCardInfo;
	private MonthCardConfig.MonthCard monthCardCfg;

	private ShowData showData;
	private System.Action OnInteractiveWithServer;

	private void CallOnInteractiveWithServer()
	{
		if (OnInteractiveWithServer != null)
			OnInteractiveWithServer();
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		monthCardCfg = userDatas[0] as MonthCardConfig.MonthCard;
		monthCardInfo = userDatas[1] as com.kodgames.corgi.protocol.OneMonthCardInfo;
		rewardType = (int)userDatas[2];

		if (userDatas.Length > 3)
			OnInteractiveWithServer = userDatas[3] as System.Action;
		else
			OnInteractiveWithServer = null;

		ShowDialog();

		return true;
	}

	public override void OnHide()
	{
		ClearList();

		rewardType = -1;
		monthCardCfg = null;
		monthCardInfo = null;
		showData = null;

		base.OnHide();
	}

	private void ClearList()
	{
		StopCoroutine("FillList");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0;
	}

	private void ShowDialog()
	{
		ClearList();

		titleLabel.Text = SetTitleText();

		// Set Message
		messageLabel.Text = SetMessageText();

		SetButtonState();

		GetRewardSetByType();

		StartCoroutine("FillList", showData);
	}

	private string SetTitleText()
	{
		switch (rewardType)
		{
			case MonthCardRewardType.BuyReward:
				return GameUtility.GetUIString("UIPnlActivityMonthCardInfo_BuyRewardTitle");

			case MonthCardRewardType.DailyReward:
				return GameUtility.GetUIString("UIPnlActivityMonthCardInfo_DailyRewardTitle");

			case MonthCardRewardType.TenTimesReward:
				return GameUtility.GetUIString("UIPnlActivityMonthCardInfo_TenRewardTitle");

			default:
				return "";
		}
	}

	private string SetMessageText()
	{
		string text = "";

		switch (rewardType)
		{
			case MonthCardRewardType.BuyReward:
				if (monthCardInfo.buyRewardCount >= 0)
					text = GameUtility.FormatUIString("UIPnlActivityMonthCardInfo_BuyRewardCount", monthCardInfo.buyRewardCount);
				break;

			case MonthCardRewardType.DailyReward:
				if (monthCardInfo.pickCounter >= 10)
					text = GameUtility.GetUIString("UIPnlActivityMonthCardInfo_ToGetTenReward");
				break;
		}
		return text;
	}

	private ShowData SetRewardDatas(List<Reward> fixRewards, List<Reward> randomRewards)
	{
		showData = new ShowData();

		var rewardData1= new RewardData();

		if (fixRewards != null && fixRewards.Count > 0)
		{
			rewardData1.rewards = fixRewards;
			rewardData1.title = GameUtility.GetUIString("UIPnlShop_FixedReward");
			showData.rewardDatas.Add(rewardData1);
		}

		var rewardData2 = new RewardData();

		if (randomRewards != null && randomRewards.Count > 0)
		{
			rewardData2.rewards = randomRewards;
			rewardData2.title = GameUtility.GetUIString("UIPnlPVERodomReward_Title_Random");
			showData.rewardDatas.Add(rewardData2);
		}

		return showData;
	}

	private void GetRewardSetByType()

	{
		switch (rewardType)
		{
			case MonthCardRewardType.BuyReward:
				showData = SetRewardDatas(monthCardCfg.buyRewardAndIcon.fixRewardShow, monthCardCfg.buyRewardAndIcon.randomRewardShow);
				break;
			case MonthCardRewardType.DailyReward:
				showData = SetRewardDatas(monthCardCfg.dailyRewardAndIcon.fixRewardShow, monthCardCfg.dailyRewardAndIcon.randomRewardShow);
				break;
			case MonthCardRewardType.TenTimesReward:
				showData = SetRewardDatas(monthCardCfg.tenRewardAndIcon.fixRewardShow, monthCardCfg.tenRewardAndIcon.randomRewardShow);
				break;
			default:
				Debug.LogError("MonthCardRewardType Error");
				break;
		}
	}

	private void SetButtonState()
	{
		for (int i = 0; i < btnLayout.childLayoutControls.Length; i++)
			btnLayout.HideChildObj(btnLayout.childLayoutControls[i].gameObject, false);

		rewardPicked.Hide(true);

		switch (rewardType)
		{
			case MonthCardRewardType.BuyReward:
				if (monthCardInfo.buyRewardCount <= 0)
					btnLayout.HideChildObj(btnLayout.childLayoutControls[1].gameObject, true);
				getRewardBtn.spriteText.Text = GameUtility.GetUIString("UIElemMonthCardItem_Get");
				break;

			case MonthCardRewardType.DailyReward:
				SetDailyButtonState();
				break;

			case MonthCardRewardType.TenTimesReward:
				if (monthCardInfo.pickCounter < 10)
					btnLayout.HideChildObj(btnLayout.childLayoutControls[1].gameObject, true);

				getRewardBtn.spriteText.Text = GameUtility.GetUIString("UIElemMonthCardItem_Get");
				break;

			default:
				break;
		}
	}

	private void SetDailyButtonState()
	{
		//十次奖励次数已满
		if (monthCardInfo.pickCounter >= 10)
		{
			getRewardBtn.spriteText.Text = GameUtility.GetUIString("UIPnlActivityMonthCardInfo_TenRewardTitle");
			return;
		}

		if (monthCardInfo.lastPickTime == 0 && monthCardInfo.remainDates > 0)
			return;

		if (!monthCardInfo.isCouldPickDailyReward)
		{
			btnLayout.HideChildObj(btnLayout.childLayoutControls[0].gameObject, true);
			btnLayout.HideChildObj(btnLayout.childLayoutControls[1].gameObject, true);
			//如果是新建的号，没有体验次数，没有购买次数，全部隐藏（隐藏“已领取”）
			if (monthCardInfo.lastPickTime == 0)
				rewardPicked.Hide(true);
			else
				rewardPicked.Hide(false);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(ShowData showData)
	{
		yield return null;

		foreach (var rewardData in showData.rewardDatas)
		{
			if (rewardData == null || rewardData.rewards.Count <= 0)
				continue;

			if (!string.IsNullOrEmpty(rewardData.title))
			{
				UIElemShopGiftViewItem viewItem = titlePool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
				viewItem.SetData(rewardData.title);
				scrollList.AddItem(viewItem.gameObject);
			}

			SetListData(rewardData.rewards, showData.showCountInName);
		}
	}

	private void SetListData(List<Reward> rewards, bool showCountInName)
	{
		int rows = rewards.Count / MAX_COLUMN_NUM;
		int leftItems = rewards.Count % MAX_COLUMN_NUM;
		rows = (leftItems == 0) ? rows - 1 : rows;

		List<Reward> colunmRewards = new List<Reward>();
		for (int index = 0; index <= rows; index++)
		{
			// Get item in this column
			colunmRewards.Clear();

			int maxColumn = (index == rows) ? ((leftItems == 0) ? MAX_COLUMN_NUM : leftItems) : MAX_COLUMN_NUM;
			for (int columnIndex = 0; columnIndex < maxColumn; columnIndex++)
				colunmRewards.Add(rewards[index * MAX_COLUMN_NUM + columnIndex]);

			// Add list item
			UIElemShopGiftItem giftItem = gameObjectPool.AllocateItem().GetComponent<UIElemShopGiftItem>();
			giftItem.SetData(colunmRewards, showCountInName);
			scrollList.AddItem(giftItem.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId, _UILayer.Top);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetRewardClick(UIButton btn)
	{
		if (rewardType == MonthCardRewardType.DailyReward && monthCardInfo.pickCounter >= 10)
		{
			RequestMgr.Inst.Request(new MonthCardPickRewardReq(monthCardCfg.id, MonthCardRewardType.TenTimesReward));
			CallOnInteractiveWithServer();
			return;
		}

		RequestMgr.Inst.Request(new MonthCardPickRewardReq(monthCardCfg.id, rewardType));
		CallOnInteractiveWithServer();
	}
}
