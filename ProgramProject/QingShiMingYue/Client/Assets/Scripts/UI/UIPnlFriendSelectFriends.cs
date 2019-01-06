using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendSelectFriends : UIModule
{
	public UIScrollList friendList;
	public GameObjectPool friendRootPool;

	public SpriteText selectFirendText;
	public SpriteText friendsNumberText;

	private int positionId;//本次选择的阵容Id
	private List<KodGames.ClientClass.FriendInfo> friendInfos;

	private List<int> lastFriendIds;
	private bool positionIdAndLast;//判断是否与上次选择的阵容相同

	private int MaxHelpFriendCount
	{
		get { return ConfigDatabase.DefaultCfg.FriendCampaignConfig.MaxHelpFriendCount; }
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		positionId = (int)userDatas[0];
		lastFriendIds = userDatas[1] as List<int>;
		positionIdAndLast = (bool)userDatas[2];

		QueryFriendListReq();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		friendList.ClearList(false);
		friendList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(List<KodGames.ClientClass.FriendInfo> friendInfos)
	{
		yield return null;

		if (friendInfos != null && friendInfos.Count > 0)
		{
			friendInfos.Sort((a1, a2) =>
			{
				int id1 = FriendIdInLast(a1);
				int id2 = FriendIdInLast(a2);

				if (id1 != id2)
					return id2 - id1;

				int line1 = a1.IsOnLine ? 1 : 0;
				int line2 = a2.IsOnLine ? 1 : 0;

				if (line1 != line2)
					return line2 - line1;

				long time1 = SysLocalDataBase.Inst.LoginInfo.NowTime - a1.LastLoginTime;
				long time2 = SysLocalDataBase.Inst.LoginInfo.NowTime - a2.LastLoginTime;

				if (time1 > time2)
					return 1;
				else if (time1 < time2)
					return -1;

				return a2.Level - a1.Level;

			});

			for (int index = 0; index < friendInfos.Count; index++)
			{
				UIElemFriendsItemRoot item = friendRootPool.AllocateItem(false).GetComponent<UIElemFriendsItemRoot>();
				item.SetData(friendInfos[index], FriendIdInLast(friendInfos[index]) == 0 ? false : true);
				friendList.AddItem(item);
			}

			friendsNumberText.Text = string.Empty;
		}
		else
			friendsNumberText.Text = GameUtility.GetUIString("UIPnlFriendNumbers_Count");

		SetFriendLabel();
	}

	//判断一下好友是不是上次选的||看看好友是否是已经离线超过固定天数
	private int FriendIdInLast(KodGames.ClientClass.FriendInfo friendInfo)
	{
		int pRet = 0;
		if (lastFriendIds != null)
		{
			for (int index = 0; index < lastFriendIds.Count; index++)
			{
				long time = SysLocalDataBase.Inst.LoginInfo.NowTime - friendInfo.LastLoginTime;
				long day = time / 1000 / 60 / 60 / 24;
				long hour = time / 1000 / 60 / 60 % 24;
				int hourD = (int)(day * 24 + hour);

				if (lastFriendIds[index] == friendInfo.PlayerId && (hourD < ConfigDatabase.DefaultCfg.FriendCampaignConfig.FriendOffLineTime || friendInfo.IsOnLine))
				{
					pRet = 1;
					break;
				}
			}
		}

		return pRet;
	}

	//获取选中好友个数
	private List<int> GetSelectFriends()
	{
		//每次使用这个数据的时候，对已经选择的好友计数器重置
		var selectFriendIds = new List<int>();

		for (int index = 0; index < friendList.Count; index++)
		{
			UIElemFriendsItemRoot item = friendList.GetItem(index) as UIElemFriendsItemRoot;
			if (item.IsSelectBoxStage())
				selectFriendIds.Add(item.PlayerId);
		}

		return selectFriendIds;
	}

	private void SetFriendLabel()
	{
		selectFirendText.Text = GameUtility.FormatUIString(
									"UIPnlSelectFriends_Count",
									GameDefines.textColorBtnYellow.ToString(),
									GameDefines.textColorWhite.ToString(),
									GetSelectFriends().Count,
									MaxHelpFriendCount);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击阵容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickAvatarViewBtn(UIButton btn)
	{
		RequestMgr.Inst.Request(new QueryFriendPlayerInfoReq((int)btn.Data));
	}

	//点击选择好友
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickSelectFriendBtn(UIButton btn)
	{
		UIElemFriendsItemRoot item = btn.Data as UIElemFriendsItemRoot;

		var selectFriendList = GetSelectFriends();
		if (selectFriendList.Count < MaxHelpFriendCount || item.IsSelectBoxStage())
		{
			if (item.IsSelectBoxStage() || item.JudgeTimeWithDay())
				item.SelectBoxToggle();
			else
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlFriendSelectFriends_Lixian"));
		}
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlFriendSelectFriends_Count"));

		SetFriendLabel();
	}

	//点击参战
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickExpectBtn(UIButton btn)
	{
		var selectFriendIds = GetSelectFriends();

		//判断一下我这次选择的好友跟上次选择的好友是否一样
		bool hasChange = false;
		foreach (var selectFriendId in selectFriendIds)
			if (!lastFriendIds.Contains(selectFriendId))
			{
				hasChange = true;
				break;
			}

		if (!hasChange)
			foreach (var lastFriendId in lastFriendIds)
				if (!selectFriendIds.Contains(lastFriendId))
				{
					hasChange = true;
					break;
				}

		// 当前选择好友与上次好友是否有变化 或者 当前所选阵位与上次所选阵位是否有变化
		hasChange = hasChange || !positionIdAndLast;

		if (selectFriendIds.Count < MaxHelpFriendCount)
		{
			string message = GameUtility.FormatUIString("UIPnlFriendSelectFriends_NotFriendNumber_Neirong", MaxHelpFriendCount, selectFriendIds.Count);
			string title = GameUtility.GetUIString("UIPnlFriendSelectFriends_NotFriendNumber_Title");

			MainMenuItem continueBtn = new MainMenuItem();
			continueBtn.ControlText = GameUtility.GetUIString("UIPnlFriendSelectFriends_NotFriendNumber_Jixu");
			continueBtn.CallbackData = hasChange;
			continueBtn.Callback = (data) =>
			{
				if (hasChange)
					RequestMgr.Inst.Request(new QueryFriendCampaignHelpFriendInfoReq(positionId, selectFriendIds));
				else
					RequestMgr.Inst.Request(new JoinFriendCampaignReq(positionId, selectFriendIds));

				return true;
			};

			MainMenuItem cancelBtn = new MainMenuItem();
			cancelBtn.ControlText = GameUtility.GetUIString("UIPnlFriendSelectFriends_NotFriendNumber_Fanhui");

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(title, message, cancelBtn, continueBtn);

			SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
		}
		else
		{
			if (hasChange)
				RequestMgr.Inst.Request(new QueryFriendCampaignHelpFriendInfoReq(positionId, selectFriendIds));
			else
				RequestMgr.Inst.Request(new JoinFriendCampaignReq(positionId, selectFriendIds));
		}
	}

	private void QueryFriendListReq()
	{
		ClearData();
		RequestMgr.Inst.Request(new QueryFriendListReq());
	}

	public void OnQueryFriendListSuccess(List<KodGames.ClientClass.FriendInfo> friendInfos)
	{
		StartCoroutine("FillList", friendInfos);
	}

	//参战失败
	public void JoinFriendCampaignReqNotSuccess(string message)
	{
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		message = message + GameUtility.GetUIString("UIPnlFriendSelectFriends_IsNotFriend");

		showData.OnHideCallback = (data) =>
		{
			QueryFriendListReq();
			return true;
		};
		showData.SetData(message, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}
}
