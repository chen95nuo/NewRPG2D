using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlAdventureBuyReward : UIModule
{
	//AvatarNormalRecruit
	public GameObject avatarNormalRecruitObject;

	public UIElemAssetIcon costIcon;
	public SpriteText playerCostText;
	public UIElemAssetIcon costIconBtn;
	public SpriteText costTextBtn;
	public UIElemAvatarCard buyIcon;

	public UIBox delaySign;
	//buyList
	public GameObject buyList;

	public UIScrollList rewardList;
	public GameObjectPool gameObjectPool;

	private Cost cost;
	private List<Reward> rewards = new List<Reward>();
	private MarvellousAdventureConfig.BuyEvent buyEvent;
	private int MAX_COLUMN_NUM = 3;

	private int coinNum = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;
		buyEvent = userDatas[0] as MarvellousAdventureConfig.BuyEvent;
		cost = buyEvent.Costs;
		rewards = buyEvent.Rewards;

		costIcon.SetData(cost.id);
		costIconBtn.SetData(cost.id);

		UpdateShowCoinUI();


		if (rewards.Count == 1)
		{
			SetShowBuyObject(true);

			if (IDSeg.ToAssetType(rewards[0].id) == IDSeg._AssetType.Avatar)
			{
				buyIcon.SetData(rewards[0].id, false, false, null);
				buyIcon.GetComponent<UIButton>().Data = rewards[0].id;
			}
			else
				SetBuyObjectsData();
		}
		else if (rewards.Count > 1)
		{
			SetBuyObjectsData();
		}
		return true;
	}

	private void Update()
	{
		if (AdventureSceneData.Instance.HadDelayReward > 0)
			delaySign.Hide(false);
		else delaySign.Hide(true);
	}

	public void UpdateShowCoinUI()
	{
		if (cost == null) return;
		if (cost.id == ClientServerCommon.IDSeg._SpecialId.GameMoney)
			coinNum = SysLocalDataBase.Inst.LocalPlayer.GameMoney;
		else if (cost.id == ClientServerCommon.IDSeg._SpecialId.RealMoney)
			coinNum = SysLocalDataBase.Inst.LocalPlayer.RealMoney;
		playerCostText.Data = coinNum;
		playerCostText.Text = ItemInfoUtility.GetItemCountStr((int)playerCostText.Data);
		costTextBtn.Data = cost.count;
		costTextBtn.Text = GameUtility.FormatUIString("UIPnlAdventureBuReward_Label_Cost", ItemInfoUtility.GetItemCountStr((int)costTextBtn.Data));
	}

	private void SetBuyObjectsData()
	{
		SetShowBuyObject(false);
		SetListData(rewards, true);
	}

	private void SetShowBuyObject(bool isShow)
	{
		avatarNormalRecruitObject.SetActive(isShow);
		buyList.SetActive(!isShow);
	}

	private void SetListData(List<Reward> rewards, bool showCountInName)
	{
		rewardList.ClearList(false);
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
			UIElemAdventureBuyRewardItem giftItem = gameObjectPool.AllocateItem().GetComponent<UIElemAdventureBuyRewardItem>();
			giftItem.SetData(colunmRewards, showCountInName);
			rewardList.AddItem(giftItem.gameObject);
		}
		rewardList.ScrollToItem(0, 0);
	}

	public override void OnHide()
	{
		SysUIEnv.Instance.HideUIModule<UIPnlAdventureScene>();
		if (SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().IsShown)
			SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().OnHide();
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBuy(UIButton btn)
	{
		if (coinNum - cost.count >= 0)
		{
			RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._DoubleSelectType.Yes, -1, null));
			HideSelf();
		}
		else
		{
			// 提示用户重新登录
		/*	MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIPnlAdventureBuReward_Label_InBuyShopping");

			okCallback.Callback = (ButtonBuy) =>
			{
				// 强制重新登录(包括在GameState_Upgrading的状态)
				//SysGameStateMachine.Instance.EnterState<GameState_Upgrading>(null, true);
				//brokenDlgShown = false;
				SysUIEnv.Instance.ShowUIModule<UIPnlRecharge>();
				return true;
			};

			MainMenuItem cancelCallback = new MainMenuItem();
			cancelCallback.ControlText = GameUtility.GetUIString("UIPnlAdventureBuReward_Label_Cancel");
			cancelCallback.Callback = (ButtonCancel) =>
			{
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().OnHide();
				return true;
			};

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(
				GameUtility.GetUIString("UIDlgMessage_Title_RealMoney_NotEnough"),
				GameUtility.FormatUIString("UIPnlAdventureBuReward_Label_CoinInsufficient", GameDefines.textColorTipsInBlack, GameDefines.textColorMessWhite, coinNum, GameDefines.textColorTipsInBlack, GameDefines.textColorMessWhite, cost.count - coinNum),
				true,
				null,
				cancelCallback,
				okCallback
				);

			// 强制显示到最高层
			SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
			SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
		 */
			SysUIEnv.Instance.ShowUIModule<UIDlgAdventureMessage>(GameUtility.FormatUIString("UIPnlAdventureBuReward_Label_CoinInsufficient", GameDefines.textColorTipsInBlack, GameDefines.textColorMessWhite, coinNum, GameDefines.textColorTipsInBlack, GameDefines.textColorMessWhite, cost.count - coinNum));

		}

		//进行下一步
		//RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._DoubleSelectType.Yes,null));
		//HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnNoBuy(UIButton btn)
	{
		RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._DoubleSelectType.No, -1, null));
		OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlAdventureMain));
		OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		GameUtility.ShowAssetInfoUI((int)btn.Data, _UILayer.Top);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkAvatarIcon(UIButton btn)
	{
		GameUtility.ShowAssetInfoUI((int)btn.Data, _UILayer.Top);
	}
	
}