using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgGuildChangeRole : UIModule
{
	public delegate void OnChangeRoleSuccess();

	public UIScrollList roleList;
	public GameObjectPool rolePool;

	private KodGames.ClientClass.GuildMemberInfo memberInfo;
	private OnChangeRoleSuccess onChangeRoleSuccess;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.memberInfo = userDatas[0] as KodGames.ClientClass.GuildMemberInfo;

		if (userDatas.Length > 1)
			onChangeRoleSuccess = userDatas[1] as OnChangeRoleSuccess;
		else
			onChangeRoleSuccess = null;

		RequestMgr.Inst.Request(new GuildQueryMemberReq(() =>
		{
			StartCoroutine("FillRoleList");
			return true;
		}));

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		StopCoroutine("FillRoleList");
		roleList.ClearList(false);
		roleList.ScrollPosition = 0f;

		memberInfo = null;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRoleList()
	{
		yield return null;

		foreach (var roleCfg in ConfigDatabase.DefaultCfg.GuildConfig.Roles)
		{
			if (roleCfg.Id == memberInfo.RoleId ||
				roleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeLeader) ||
				roleCfg.CanOffered == false)
				continue;

			var memberRoleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(memberInfo.RoleId);

			if (roleCfg.Id > memberRoleCfg.Id)
				if (!memberRoleCfg.CanDownRole)
					continue;

			var container = rolePool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemGuildChangeRoleItem>();

			container.Data = item;
			item.SetData(roleCfg);

			roleList.AddItem(container);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelectItem(UIButton btn)
	{
		for (int index = 0; index < roleList.Count; index++)
		{
			UIElemGuildChangeRoleItem item = roleList.GetItem(index).gameObject.GetComponent<UIElemGuildChangeRoleItem>();

			if (item == null)
				continue;

			if ((int)item.RoleId == (int)btn.Data)
				item.ChangeSelectStatus(true);
			else
				item.ChangeSelectStatus(false);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOkBtn(UIButton btn)
	{
		UIElemGuildChangeRoleItem selectItem = null;
		for (int index = 0; index < roleList.Count; index++)
		{
			var item = roleList.GetItem(index).Data as UIElemGuildChangeRoleItem;
			if (item.IsSelected)
			{
				selectItem = item;
				break;
			}
		}

		if (selectItem == null)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgGuildChangeRole_NoRoleSelect"));
		else
			RequestMgr.Inst.Request(new GuildSetPlayerRoleReq(memberInfo.PlayerId, selectItem.RoleId, () =>
				{
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildIntroReview_ReviewOneMember"));
					HideSelf();

					if (onChangeRoleSuccess != null)
						onChangeRoleSuccess();

					return true;
				}));

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCancelBtn(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}
