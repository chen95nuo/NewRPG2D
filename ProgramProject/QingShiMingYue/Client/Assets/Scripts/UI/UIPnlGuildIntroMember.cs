using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildIntroMember : UIModule
{
	public UIScrollList guildMemberList;
	public GameObjectPool guildPool;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlGuildIntroTab>().ChangeTabButtons(_UIType.UIPnlGuildIntroMember);

		QueryGuildMemberInfos();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillGuildMemberList");
		guildMemberList.ClearList(false);
		guildMemberList.ScrollPosition = 0f;
	}

	private void QueryGuildMemberInfos()
	{
		ClearData();

		RequestMgr.Inst.Request(new GuildQueryMemberReq(() =>
		{
			SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers.Sort(DataCompare.CompareGuildMember);
			StartCoroutine("FillGuildMemberList");
			return true;
		}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuildMemberList()
	{
		yield return null;

		foreach (var guildMember in SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers)
		{
			var container = guildPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemGuildIntroMemberItem>();

			container.Data = item;
			item.SetData(guildMember);

			guildMemberList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMemberOperaiton(UIButton btn)
	{
		RequestMgr.Inst.Request(new QueryFriendListReq((friendInfos) =>
			{
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGuildMemberOperation), btn.Data, friendInfos, new UIDlgGuildMemberOperation.FinishOperationDel(OnMemberOperationDel));
				return true;
			}));
	}

	private void OnMemberOperationDel(UIDlgGuildMemberOperation.OperatorType operationType, int operatorPlayerId)
	{
		switch (operationType)
		{
			case UIDlgGuildMemberOperation.OperatorType.KickOfGuild:
				QueryGuildMemberInfos();
				break;

			case UIDlgGuildMemberOperation.OperatorType.RoleChange:

				var guildMemberInfo = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GetGuildMemberByPlayerId(operatorPlayerId);
				if (guildMemberInfo != null)
				{
					for (int i = 0; i < guildMemberList.Count; i++)
					{
						var item = guildMemberList.GetItem(i).Data as UIElemGuildIntroMemberItem;
						if (item != null && item.PlayerId == operatorPlayerId)
						{
							item.SetData(guildMemberInfo);
							break;
						}
					}
				}

				break;
		}
	}
}