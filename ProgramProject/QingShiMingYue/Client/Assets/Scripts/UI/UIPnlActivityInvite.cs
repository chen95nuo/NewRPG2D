using System;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections;
using System.Collections.Generic;
using KodGames;

public class UIPnlActivityInvite : UIModule
{
	public UIScrollList rewardLists;
	public GameObjectPool rewardPool;

	public UIBox getRewardBox;
	public UIButton getRewardBtn;

	public UITextField textInput;
	public SpriteText inputLabel;
	public UIElemAssetIcon rewardIcon;

	public SpriteText decsLabel;
	public SpriteText codeLabel;

	private KodGames.ClientClass.Reward useCodeRewards;
	private string copyString;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		ClearData();

		//邀请码比较特殊，如果在这个页面关闭的话，强制返回到第一个休息室活动中
		SysUIEnv.Instance.GetUIModule<UIPnlActivityTab>().SetActiveButton(_UIType.UIPnlActivityInvite);
		ActivityManager.Instance.ActivityJumpUI = _UIType.UIPnlActivityInnTab;

		//Code rewards.
		this.useCodeRewards = userDatas[0] as KodGames.ClientClass.Reward;

		//My Invitation Code.
		copyString = userDatas[1] as string;
		this.codeLabel.Text = GameUtility.FormatUIString("UIPnlActivityInvite_Code", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(), copyString);

		//Invite desc.
		decsLabel.Text = userDatas[2] as string;

		//Invitation Code List.
		StartCoroutine("FillList", (userDatas[3] as List<KodGames.ClientClass.InviteReward>));

		//Code rewards and Inviter.
		ShowUI((bool)userDatas[4], userDatas[5] as string);

		//Invitation Code reward Icon.
		rewardIcon.SetData((int)(userDatas[6]));

		textInput.Text = string.Empty;

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		//手动清空list下面的所有数据【界面切来切去的时候，里面数据时对的，渲染出来的东西是错误的】
		for (int index = 0; index < rewardLists.Count; index++)
		{
			UIElemInviteItemRoot item = rewardLists.GetItem(index) as UIElemInviteItemRoot;
			if (item != null)
			{
				item.RewardItemData = null;
				item.UiItem = null;
			}
		}

		StopCoroutine("FillList");
		rewardLists.ClearList(false);
		rewardLists.ScrollListTo(0);
	}

	private void ShowUI(bool useCodeRewardHasPick, string codeOwnerName)
	{
		//Set Get Reward Button and Box.
		getRewardBox.Hide(!useCodeRewardHasPick);
		getRewardBtn.Hide(useCodeRewardHasPick);

		//Set Input label.
		textInput.gameObject.SetActive(!useCodeRewardHasPick);
		inputLabel.Text = !useCodeRewardHasPick ? string.Empty : GameUtility.FormatUIString("UIPnlActivityInvite_Friend", GameDefines.textColorBtnYellow.ToString(),
																						GameDefines.textColorWhite.ToString(), codeOwnerName);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(List<KodGames.ClientClass.InviteReward> rewardItems)
	{
		yield return null;

		for (int index = 0; index < rewardItems.Count; index++)
		{
			UIElemInviteItemRoot item = rewardPool.AllocateItem(false).GetComponent<UIElemInviteItemRoot>();

			item.SetData(rewardItems[index]);
			rewardLists.AddItem(item);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGiftItem(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGiftGoods), this.useCodeRewards, GameUtility.GetUIString("UIPnlActivityInvite_Gift_Title"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetGift(UIButton btn)
	{
		string formTxt = textInput.spriteText.Text.Trim();
		if (!formTxt.Equals(string.Empty) && !formTxt.Equals(""))
			RequestMgr.Inst.Request(new VerifyInviteCodeAndPickRewardReq(textInput.Text));
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityInvite_Reward_Lingqu"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCopy(UIButton btn)
	{
		Platform.Instance.CopyString(copyString);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFaceBook(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgShare));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIcon(UIButton btn)
	{
		UIElemAssetIcon item = btn.Data as UIElemAssetIcon;
		ClientServerCommon.Reward reward = item.Data as ClientServerCommon.Reward;
		GameUtility.ShowAssetInfoUI(reward);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetGood(UIButton btn)
	{
		var reward = btn.Data as KodGames.ClientClass.InviteReward;

		if (reward.PickState == _InviteCodeRewardPickState.Pickable)
			RequestMgr.Inst.Request(new PickInviteCodeRewardReq(reward.ID));
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityInvite_Reward_Not_Item"));
	}

	//领取邀请码奖励协议成功
	public void OnVerifyInviteCodeAndPickRewardSuccess(CostAndRewardAndSync costAndRewardAndSync, string codeOwnername)
	{
		ShowUI(true, codeOwnername);

		//Show Rewards.
		string message = GameUtility.GetUIString("UIPnlActivityInvite_Code_Reward_Message");

		List<KodGames.Pair<int, int>> rewards = SysLocalDataBase.ConvertIdCountList(costAndRewardAndSync.Reward);
		for (int index = 0; index <= rewards.Count - 1; index++)
		{
			if (index < rewards.Count - 1)
				message += GameUtility.FormatUIString("UIPnlActivityInvite_Code_Reward_Message_3", ItemInfoUtility.GetAssetName(rewards[index].first), rewards[index].second);
			else
				message += GameUtility.FormatUIString("UIPnlActivityInvite_Code_Reward_Message_4", ItemInfoUtility.GetAssetName(rewards[index].first), rewards[index].second);
		}

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(SysLocalDataBase.GetRewardDesc(costAndRewardAndSync.Reward, true, false, true), true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//领取每档奖励协议成功
	public void OnPickInviteCodeRewardSuccess(int itemId)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityInvite_Reward_Item_Succes"));

		for (int index = 0; index < rewardLists.Count; index++)
		{
			UIElemInviteItemRoot item = rewardLists.GetItem(index) as UIElemInviteItemRoot;

			if (item.RewardItemData != null)
				if (item.RewardItemData.ID == itemId)
				{
					item.SetButtonState(_InviteCodeRewardPickState.HasPicked);
					break;
				}
		}
	}
}
