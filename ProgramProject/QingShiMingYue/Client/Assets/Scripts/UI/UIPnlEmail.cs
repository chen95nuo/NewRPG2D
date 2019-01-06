using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlEmail : UIModule
{
	public UIScrollList scrollList;
	public GameObjectPool friendRequestPool;
	public List<UIButton> tabBtns;
	public List<AutoSpriteControlBase> emailCounts;
	public SpriteText emptyTip;

	private UIElemEmailItem currentEmail;
	private int currentTab;
	private long lastTime;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabBtns[0].Data = _EmailDisplayType.System;
		tabBtns[1].Data = _EmailDisplayType.Friend;
		tabBtns[2].Data = _EmailDisplayType.Combat;
		tabBtns[3].Data = _EmailDisplayType.Guild;
		//初始化时间为当前时间，这样如果第一次不发送协议，使用本地数据填充可以全部置为旧邮件
		lastTime = -1;

		return true;
	}

	/// <summary>
	/// If has UserDatas , you Should pass showTab(int) and
	/// fouceRequest(bool) to UserDatas.
	/// </summary>
	/// <param name="layer"></param>
	/// <param name="userDatas">showTab,fouceRequest</param>
	/// <returns></returns>
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		bool fouceRequest = false;

		if (userDatas != null && userDatas.Length > 0)
		{
			currentTab = (int)userDatas[0];
			fouceRequest = (bool)userDatas[1];
		}
		else
			currentTab = _EmailDisplayType.System;

		ChangeTabState(fouceRequest);

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public void ClearData()
	{
		StopCoroutine("FillData");

		for (int index = 0; index < scrollList.Count; index++)
			(scrollList.GetItem(index).Data as UIElemEmailItem).ReleaseAttachmentItems();

		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0f;
		emptyTip.Text = "";

		//清空数据的时候，控制时间，把“新”字去掉
		//切换页标签的时候，也会把“新”给去掉【防止使用本地数据渲染的时候无限显示“新”】
		lastTime = -1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public IEnumerator FillData(int displayType)
	{
		yield return null;
		// Grab email data
		KodGames.ClientClass.EmailData emailData = SysLocalDataBase.Inst.LocalPlayer.EmailData;

		// Fill list
		var emails = emailData.GetEmails(displayType);
		if (emails != null)
		{
			//时间排序后添加的在上面		
			emails.Sort((a1, a2) =>
			{
				if (a2.SendTime >= a1.SendTime)
					return 0;
				else
					return -1;
			});

			foreach (var email in emails)
			{
				UIElemEmailItem item = friendRequestPool.AllocateItem().GetComponent<UIElemEmailItem>();

				item.SetData(email, lastTime);
				item.container.ScanChildren();
				scrollList.AddItem(item.container);
			}
		}

		if (scrollList.Count <= 0 && !emptyTip.Text.Equals(GameUtility.GetUIString("UIEmptyList_Email")))
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Email");
		else if (scrollList.Count > 0 && !emptyTip.Text.Equals(""))
			emptyTip.Text = "";
	}

	//对新邮件进行标记
	public void ChangeNewMailButtonState()
	{
		KodGames.ClientClass.EmailData emailData = SysLocalDataBase.Inst.LocalPlayer.EmailData;

		if (emailData.GetNewEmailCount(_EmailDisplayType.System) > 0 && currentTab != _EmailDisplayType.System)
		{
			emailCounts[0].Hide(false);
			emailCounts[0].Text = emailData.GetNewEmailCount(_EmailDisplayType.System).ToString();
		}
		else
		{
			emailCounts[0].Hide(true);
			emailCounts[0].Text = "";
		}

		if (emailData.GetNewEmailCount(_EmailDisplayType.Friend) > 0 && currentTab != _EmailDisplayType.Friend)
		{
			emailCounts[1].Hide(false);
			emailCounts[1].Text = emailData.GetNewEmailCount(_EmailDisplayType.Friend).ToString();
		}
		else
		{
			emailCounts[1].Hide(true);
			emailCounts[1].Text = "";
		}

		if (emailData.GetNewEmailCount(_EmailDisplayType.Combat) > 0 && currentTab != _EmailDisplayType.Combat)
		{
			emailCounts[2].Hide(false);
			emailCounts[2].Text = emailData.GetNewEmailCount(_EmailDisplayType.Combat).ToString();
		}
		else
		{
			emailCounts[2].Hide(true);
			emailCounts[2].Text = "";
		}

		if (emailData.GetNewEmailCount(_EmailDisplayType.Guild) > 0 && currentTab != _EmailDisplayType.Guild)
		{
			emailCounts[3].Hide(false);
			emailCounts[3].Text = emailData.GetNewEmailCount(_EmailDisplayType.Guild).ToString();
		}
		else
		{
			emailCounts[3].Hide(true);
			emailCounts[3].Text = "";
		}
	}

	//切换标签，判断是否向服务器发送消息
	private void ChangeTabState(bool fouceRequest)
	{
		// Update new email count
		ChangeNewMailButtonState();

		// Set tab button state
		foreach (UIButton btn in tabBtns)
			btn.controlIsEnabled = ((int)btn.data) != currentTab;

		// Clear UI
		ClearData();

		if (fouceRequest)
		{
			RequestMgr.Inst.Request(new QueryEmailListsReq(currentTab));
		}
		else
		{
			// Grab email data
			KodGames.ClientClass.EmailData emailData = SysLocalDataBase.Inst.LocalPlayer.EmailData;

			if (emailData.GetNewEmailCount(currentTab) != 0 || emailData.GetEmails(currentTab) == null)
			{
				// Has new emails, query them
				RequestMgr.Inst.Request(new QueryEmailListsReq(currentTab));
			}
			else
			{
				// No new email, show UI directly use FillData
				StartCoroutine("FillData", currentTab);
			}
		}
	}

	//切换标签后消息返回界面进行渲染
	public void QueryEmailListSuccess(int displayType, long lastTime)
	{
		this.lastTime = lastTime;
		StartCoroutine("FillData", displayType);
	}

	//切换标签
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChangeTab(UIButton btn)
	{
		currentTab = (int)btn.Data;
		ChangeTabState(false);
	}

	/// <summary>
	/// 1.Take the attachment 2.Agree to add new friend
	/// </summary>
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOperatorEmail(UIButton btn)
	{
		UIElemEmailItem item = (UIElemEmailItem)btn.data;
		currentEmail = item;
		//对邮件进行操作后，不管是否成功，默认是旧的邮件
		item.SetNewMessageBox(true);

		if (item.email.EmailType == _MailType.CombatRob)
		{
			//去往战斗界面【功能已经消失，暂定】
			//SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlRobSkill);
			//List<long> delEmals = new List<long>();
			//delEmals.Add(item.email.EmailId);
			//RequestMgr.Inst.Request(new DeleteEmailReq(delEmals));
		}
		else if (item.email.EmailType == _MailType.CombatArena)
		{
			//去往比武场
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlArena);
		}
		//Add friend request : show the dialog for confirm whether agree or refuse
		else if (item.email.EmailType == _MailType.AddFriendRequrst)
		{

			int limitLevel = ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Friend);
			if (limitLevel > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			{
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlEmail_Friend_Not_Open", limitLevel));
			}
			else
			{
				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				string title = GameUtility.GetUIString("UIPnlEmail_Title_AddFriendReq");
				string message = string.Format(GameUtility.GetUIString("UIPnlEmail_Message_AddFriendReq"), currentEmail.email.SenderName);

				MainMenuItem okCallback = new MainMenuItem();
				okCallback.Callback = AgreeAddFriendReq;
				okCallback.ControlText = GameUtility.GetUIString("UIPnlEmail_Agree_AddFriendReq");

				MainMenuItem cancelCallback = new MainMenuItem();
				cancelCallback.Callback = RefuseAddFriendReq;
				cancelCallback.ControlText = GameUtility.GetUIString("UIPnlEmail_Refuse_AddFriendReq");
				showData.SetData(title, message, cancelCallback, okCallback);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
			}
		}

		else if (item.email.AttachmentRewards != null && item.email.GetAttachmentRewardsCount() > 0)
		{
			// Get the attachment
			RequestMgr.Inst.Request(new GetAttachmentsReq(item.email.EmailId));
		}

	}

	//收取附件消息回来后处理函数
	public void OnGetAttachmentSuccess(KodGames.ClientClass.Reward reward)
	{
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(SysLocalDataBase.GetRewardDesc(reward, true, false, true), true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);

		//附件被领取，设置状态
		currentEmail.SetAttachmentAchievedState(true);
	}

	//同意添加好友
	private bool AgreeAddFriendReq(object obj)
	{
		int activePlayerId = currentEmail.email.SenderPlayerId;
		long passiveEmailId = currentEmail.email.EmailId;

		RequestMgr.Inst.Request(new AnswerFriendReq(activePlayerId, passiveEmailId, true));
		return true;
	}

	//拒绝添加好友
	private bool RefuseAddFriendReq(object obj)
	{
		int activePlayerId = currentEmail.email.SenderPlayerId;
		long passiveEmailId = currentEmail.email.EmailId;

		RequestMgr.Inst.Request(new AnswerFriendReq(activePlayerId, passiveEmailId, false));
		return true;
	}

	//更改好友申请状态
	public bool OnAnswerFriendReqSuccess(bool isYes)
	{
		// Agree friend request
		if (isYes)
			currentEmail.email.StatusDidPick = 1;
		// Refuse friend request
		else
			currentEmail.email.StatusDidPick = 2;

		currentEmail.SetEmailFriendRequestState();

		return true;
	}

	private void ShowMessage(string title, string message)
	{
		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();

		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
		showData.SetData(title, message, okCallback);

		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		Reward reward = assetIcon.Data as Reward;
		GameUtility.ShowAssetInfoUI(reward, _UILayer.Top);
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkGoShopping(UIButton btn)
	{
		GameUtility.JumpUIPanel(_UIType.UIPnlGuildPublicShop);
	}
}
