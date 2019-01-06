using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIDlgShopGiftPreview : UIModule
{
	public class ShowData
	{
		public List<RewardData> rewardDatas = new List<RewardData>();
		public string title;
		public string message;
		public string btnText;
		public MonoBehaviour btnScript;
		public string btnMethodInvoke;
		public bool showCountInName;
	}

	public class RewardData
	{
		public string title;
		public List<Reward> rewards = new List<Reward>();


		public RewardData() { }

		public RewardData(List<Reward> rewards) : this(string.Empty, rewards) { }

		public RewardData(string title, List<Reward> rewards)
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
	public UIButton clostBtn;
	public UIBox giftListBg;
	public UIBox bgBtn;
	private const int MAX_COLUMN_NUM = 3;
	private float bgOffset = 0f;
	
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;
		
		ShowDialog(userDatas[0] as ShowData);

		return true;
	}

	private void ShowDialog(ShowData showData)
	{
		// Set title
		if (string.IsNullOrEmpty(showData.title))
			titleLabel.Text = GameUtility.GetUIString("UIDlgShopGiftPreview_Title");
		else
			titleLabel.Text = showData.title; ;

		// Set Message
		if (string.IsNullOrEmpty(showData.message))
		{
			bgOffset = 0;
			messageLabel.Text = string.Empty;
		}
		else
		{
			messageLabel.Text = showData.message;
			bgOffset = messageLabel.GetLineHeight() * messageLabel.GetDisplayLineCount();
		}
		// Set button.
		if (string.IsNullOrEmpty(showData.btnText))
			okBtn.Text = GameUtility.GetUIString("UICtrl_Btn_OK");
		else
			okBtn.Text = showData.btnText;

		if (showData.btnScript == null)
			okBtn.scriptWithMethodToInvoke = this;
		else
			okBtn.scriptWithMethodToInvoke = showData.btnScript;

		if (string.IsNullOrEmpty(showData.btnMethodInvoke))
			okBtn.methodToInvoke = "OnClickClose";
		else
			okBtn.methodToInvoke = showData.btnMethodInvoke;

		SetOffsetBg(bgOffset);
		
		//Set RewardData List
		StartCoroutine("FillList", showData);
	}

	private void SetOffsetBg(float offsetValue)
	{
		Vector3 offset = new Vector3(0, offsetValue, 0);
		if (giftListBg.transform.localPosition.y >= 0)
			giftListBg.transform.localPosition += offset;
		else
			giftListBg.transform.localPosition -= offset;

		bgBtn.SetSize(bgBtn.width,bgBtn.height+=offsetValue);

		if (okBtn.transform.localPosition.y >= 0)
			okBtn.transform.localPosition += offset;
		else
			okBtn.transform.localPosition -= offset;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	private void ClearList()
	{
		StopCoroutine("FillList");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0;
		SetOffsetBg(0 - bgOffset);
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
		OnHide();
	}

	//µã»÷Í¼±ê
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.Data as Reward, _UILayer.Top);
	}
}
