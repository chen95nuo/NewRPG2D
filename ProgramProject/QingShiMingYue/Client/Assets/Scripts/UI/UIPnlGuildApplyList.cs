using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildApplyList : UIModule
{
	public UIScrollList guildApplyList;
	public GameObjectPool guildPool;
	public GameObjectPool moreItemPool;
	public UIBox vipCondition;
	public UITextField searchForm;
	public UIButton queryBtn;
	public UIButton foundBtn;
	public UIButton quickBtn;
	public UIButton closeBtn;

	private List<KodGames.ClientClass.GuildRecord> guildRecords;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// Init Search Label.
		searchForm.Text = string.Empty;

		RequestMgr.Inst.Request(new GuildQueryGuildListReq(null));

		return true;
	}

	public override void OnHide()
	{
		searchForm.Text = "";

		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("CreateGuildName"))
			SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.RemoveDynamicValue("CreateGuildName");

		ClearData();

		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		guildApplyList.ClearList(false);
		guildApplyList.ScrollListTo(0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		int currentCount = GetLastListItemIndex();
		int index = currentCount;

		for (; index < currentCount + ConfigDatabase.DefaultCfg.GuildConfig.GuildCountPerPage && index < guildRecords.Count; index++)
		{
			UIElemGuildApplyItem item = guildPool.AllocateItem().GetComponent<UIElemGuildApplyItem>();

			item.SetData(guildRecords[index], index + 1);

			if (guildApplyList.Count == 0 || guildApplyList.GetItem(guildApplyList.Count - 1).Data is UIElemGuildApplyItem)
			{
				guildApplyList.AddItem(item.container);
			}
			else
			{
				guildApplyList.InsertItem(item.container, guildApplyList.Count - 1);
			}
		}

		if (guildApplyList.Count > 0 && guildApplyList.GetItem(guildApplyList.Count - 1).Data == null)
			guildApplyList.RemoveItem(guildApplyList.Count - 1, false);

		if (GetLastListItemIndex() < guildRecords.Count)
		{
			guildApplyList.AddItem(moreItemPool.AllocateItem());
		}
	}

	private int GetLastListItemIndex()
	{
		if (guildApplyList.Count <= 0)
			return 0;

		if (guildApplyList.GetItem(guildApplyList.Count - 1).Data is UIElemGuildApplyItem)
			return guildApplyList.Count;
		else
			return guildApplyList.Count - 1;
	}

	public void OnRequestMsgListSuccess(List<KodGames.ClientClass.GuildRecord> guildRecords, string searchKey)
	{
		this.guildRecords = guildRecords;

		this.guildRecords.Sort(DataCompare.CompaceApplyGuildRecord);

		if (!string.IsNullOrEmpty(searchKey) && guildRecords.Count <= 0)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildApplyList_EmptyTip"));
		else
			ClearData();

		if (SysLocalDataBase.Inst.LocalPlayer.VipLevel >= ConfigDatabase.DefaultCfg.GuildConfig.ChangeLeaderVipLimit)
			vipCondition.Hide(true);
		else
			vipCondition.Text = GameUtility.FormatUIString("UIRankingTab_Txt_MyVipLevel", ConfigDatabase.DefaultCfg.GuildConfig.ChangeLeaderVipLimit);

		StartCoroutine("FillList");
	}

	public void OnRequestApplySuccess(KodGames.ClientClass.GuildRecord guildRecord)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildApplyList_ApplySuccess"));

		for (int index = 0; index < guildApplyList.Count; index++)
		{
			UIElemGuildApplyItem item = guildApplyList.GetItem(index).gameObject.GetComponent<UIElemGuildApplyItem>();

			if (item == null)
				return;

			if ((int)item.infoBtn.Data == guildRecord.GuildId)
			{
				item.SetData(guildRecord, (int)item.infoBtn.IndexData);
				return;
			}
		}
	}

	public void OnRequestApplyAutoSuccess(string applyText)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab, applyText);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickQuery(UIButton btn)
	{
		string formTxt = searchForm.spriteText.Text.Trim();

		string formPlaceHolder = string.Format("{0}{1}", searchForm.placeHolderColorTag, searchForm.placeHolder);

		if (!formTxt.Equals(formPlaceHolder) && !formTxt.Equals(""))
			RequestMgr.Inst.Request(new GuildQueryGuildListReq(formTxt));
		else
			RequestMgr.Inst.Request(new GuildQueryGuildListReq(null));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickInfo(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildApplyInfo, (int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickApply(UIButton btn)
	{
		RequestMgr.Inst.Request(new GuildApplyReq((int)btn.Data));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFound(UIButton btn)
	{
		if (SysLocalDataBase.Inst.LocalPlayer.VipLevel >= ConfigDatabase.DefaultCfg.GuildConfig.ChangeLeaderVipLimit)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGuildFound));
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlGuildApplyList_ConditionVip", ConfigDatabase.DefaultCfg.GuildConfig.ChangeLeaderVipLimit));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickQuickApply(UIButton btn)
	{
		RequestMgr.Inst.Request(new GuildQuickJoinReq());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMoreItem(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList");
	}
}
