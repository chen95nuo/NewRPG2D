using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using System.Collections.Generic;

public class UIPnlGuildApplyInfo : UIModule
{
	public SpriteText guildNameLabel;
	public SpriteText guildDeclarationLabel;
	public UIScrollList guildMemberList;
	public GameObjectPool memberPool;
	public UIButton closeBtn;

	private int guildId;
	private List<FriendInfo> firends = new List<FriendInfo>();

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		if (userDatas != null)
		{
			guildId = (int)userDatas[0];
			if (userDatas.Length > 1)
				closeBtn.Data = userDatas[1];
		}


		RequestMgr.Inst.Request(new GuildViewSimpleReq(guildId, (guildInfoSimple) =>
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildApplyInfo)))
				SysUIEnv.Instance.GetUIModule<UIPnlGuildApplyInfo>().RequestQuerySuccess((KodGames.ClientClass.GuildInfoSimple)guildInfoSimple);
			return true;
		}));

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
		guildMemberList.ClearList(false);
		guildMemberList.ScrollListTo(0f);
	}

	private void SetData(KodGames.ClientClass.GuildInfoSimple guildInfoSimple)
	{
		guildNameLabel.Text = GameUtility.FormatUIString("UIDlgGuildTab_NameAndLV", GameDefines.textColorGuildInfo, guildInfoSimple.GuildName, GameDefines.textColorWhite, guildInfoSimple.GuildLevel);

		if (string.IsNullOrEmpty(guildInfoSimple.Declaration))
			guildDeclarationLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildDeclaration", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, GameUtility.GetUIString("UITextHolder_Declaration"));
		else
			guildDeclarationLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildDeclaration", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, guildInfoSimple.Declaration);

		guildInfoSimple.Members.Sort(DataCompare.CompaceGuildInfoRecord);

		RequestMgr.Inst.Request(new QueryFriendListReq((firends) =>
		{
			this.firends = firends;
			StartCoroutine("FillList", guildInfoSimple.Members);
			return true;
		}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(List<KodGames.ClientClass.GuildMemberInfo> playerRecords)
	{
		yield return null;

		for (int index = 0; index < playerRecords.Count; index++)
		{
			UIElemGuildApplyInfoItem item = memberPool.AllocateItem().GetComponent<UIElemGuildApplyInfoItem>();
			item.SetData(playerRecords[index], firends);
			guildMemberList.AddItem(item.gameObject);
		}
	}

	public void RequestQuerySuccess(KodGames.ClientClass.GuildInfoSimple guildInfoSimple)
	{
		SetData(guildInfoSimple);
	}

	public void RequestInviteFriendSuccess()
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgFriendMsg_Title_InviteFriendSuccess"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAddFriend(UIButton btn)
	{
		RequestMgr.Inst.Request(new InviteFriendReq((int)btn.Data, null));
	}
}
