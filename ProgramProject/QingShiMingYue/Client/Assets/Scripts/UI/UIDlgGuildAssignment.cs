using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgGuildAssignment : UIModule
{
	public delegate void OnAssignmentSuccess();

	public UIScrollList membersList;
	public GameObjectPool memberPool;
	public GameObjectPool moreItemPool;
	public UIButton closeBtn;
	public UIButton backBtn;
	public UIButton assignmentBtn;
	public SpriteText noMember;
	public UIChildLayoutControl layout;
	public SpriteText explainText;

	private List<KodGames.ClientClass.GuildTransferMember> guildTransferMembers;
	private OnAssignmentSuccess onAssignmentSuccess;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas.Length > 0)
			onAssignmentSuccess = userDatas[0] as OnAssignmentSuccess;

		RequestMgr.Inst.Request(new GuildQueryTransferMemberReq());

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		membersList.ClearList(false);
		membersList.ScrollListTo(0f);

		this.onAssignmentSuccess = null;
	}

	public void RequsetQuerySuccess(List<KodGames.ClientClass.GuildTransferMember> guildTransferMembers)
	{
		explainText.Text = GameUtility.FormatUIString("UIPnlGuildAssignment_Explain", ConfigDatabase.DefaultCfg.GuildConfig.ChangeLeaderVipLimit);

		this.guildTransferMembers = guildTransferMembers;

		if (guildTransferMembers == null || guildTransferMembers.Count <= 0)
		{
			noMember.Hide(false);
			noMember.Text = GameUtility.GetUIString("UIPnlGuildAssignment_CountNull");
		}
		else
			noMember.Hide(true);

		layout.HideChildObj(layout.childLayoutControls[1].gameObject, !noMember.IsHidden());

		this.guildTransferMembers.Sort(DataCompare.CompareGuildTransferMember);

		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		int currentCount = GetLastListItemIndex();
		int index = currentCount;

		for (; index < currentCount + ConfigDatabase.DefaultCfg.GuildConfig.GuildCountPerPage && index < guildTransferMembers.Count; index++)
		{
			UIElemGuildAssignmentItem item = memberPool.AllocateItem().GetComponent<UIElemGuildAssignmentItem>();

			item.SetData(guildTransferMembers[index]);

			if (membersList.Count == 0 || membersList.GetItem(membersList.Count - 1).Data is UIElemGuildAssignmentItem)
				membersList.AddItem(item.container);
			else
				membersList.InsertItem(item.container, membersList.Count - 1);
		}

		if (membersList.Count > 0 && membersList.GetItem(membersList.Count - 1).Data == null)
			membersList.RemoveItem(membersList.Count - 1, false);

		if (GetLastListItemIndex() < guildTransferMembers.Count)
			membersList.AddItem(moreItemPool.AllocateItem());
	}

	private int GetLastListItemIndex()
	{
		if (membersList.Count <= 0)
			return 0;

		if (membersList.GetItem(membersList.Count - 1).Data is UIElemGuildAssignmentItem)
			return membersList.Count;
		else
			return membersList.Count - 1;
	}

	private int FindSelectPlayerId()
	{
		for (int index = 0; index < membersList.Count; index++)
		{
			UIElemGuildAssignmentItem item = membersList.GetItem(index).gameObject.GetComponent<UIElemGuildAssignmentItem>();
			if (!item.selectBox.IsHidden())
				return (int)item.selectBtn.Data;
		}
		return IDSeg.InvalidId;
	}

	private bool OnAssignmentResSuccess()
	{
		if (onAssignmentSuccess != null)
			onAssignmentSuccess();

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildAssignment_Success"));

		HideSelf();

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAssignment(UIButton btn)
	{
		if (FindSelectPlayerId() != IDSeg.InvalidId)
			RequestMgr.Inst.Request(new GuildTransferReq(FindSelectPlayerId(), OnAssignmentResSuccess));
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildAssignment_NoPlayer"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelect(UIButton btn)
	{
		for (int index = 0; index < membersList.Count; index++)
		{
			UIElemGuildAssignmentItem item = membersList.GetItem(index).gameObject.GetComponent<UIElemGuildAssignmentItem>();

			if (item == null)
				continue;

			if ((int)item.selectBtn.Data == (int)btn.Data)
				item.SetButtonState(true);
			else
				item.SetButtonState(false);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMoreItem(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList");
	}

}
