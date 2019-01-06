using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildChat : UIModule
{
	public UIScrollList chatList;
	public GameObjectPool chatPool;
	public UITextField searchForm;
	public UIButton sendBtn;
	public SpriteText chatCountLabel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlGuildMessageTab>().ChangeTabButtons(_UIType.UIPnlGuildChat);

		SysUIEnv.Instance.GetUIModule<UIPnlGuildMessageTab>().SetMsgLeftState(false);

		RequestMgr.Inst.Request(new GuildQueryMsgReq());

		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.MsgLeft = 0;

		searchForm.Text = "";

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public void RequestQueryMsgSuccess(List<KodGames.ClientClass.GuildMsg> guildMsgs)
	{
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.MsgLeft = 0;

		var miniInfo = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo;

		chatCountLabel.Text = GameUtility.FormatUIString("UIDlgGuildChat_Count", ConfigDatabase.DefaultCfg.GuildConfig.MsgCountLimitPerDay - miniInfo.GuildMsgCount, ConfigDatabase.DefaultCfg.GuildConfig.MsgDayLimit);

		ClearData();

		StartCoroutine("FillList", guildMsgs);
	}

	public void OnNewMsgSuccess(KodGames.ClientClass.GuildMsg guildMsg)
	{
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.MsgLeft = 0;

		InsertOneMsg(guildMsg);
	}

	private void InsertOneMsg(KodGames.ClientClass.GuildMsg msg)
	{
		UIListItemContainer container = chatPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemGuildChatItem item = container.gameObject.GetComponent<UIElemGuildChatItem>();
		item.SetData(msg);
		chatList.AddItem(container);
		chatList.ScrollToItem(chatList.Count - 1, 0f);
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		chatList.ClearList(false);
		chatList.ScrollListTo(0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(List<KodGames.ClientClass.GuildMsg> guildMsgs)
	{
		yield return null;

		for (int index = 0; index < guildMsgs.Count; index++)
		{
			UIElemGuildChatItem item = chatPool.AllocateItem().GetComponent<UIElemGuildChatItem>();
			item.SetData(guildMsgs[index]);
			chatList.AddItem(item.gameObject);
		}

		chatList.ScrollToItem(chatList.Count - 1, 0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSend(UIButton btn)
	{
		string formTxt = searchForm.spriteText.Text.Trim();
		string formPlaceHolder = string.Format("{0}{1}", searchForm.placeHolderColorTag, searchForm.placeHolder);

		searchForm.Text = string.Empty;

		if (formTxt.Equals("") || formTxt.Equals(formPlaceHolder))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgGuildChat_Tips"));
		else
			RequestMgr.Inst.Request(new GuildAddMsgReq(formTxt));
	}

}
