using System;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using UnityEngine;
using com.kodgames.corgi.protocol;

public class UIDlgGuildPlayerList : UIModule
{
	public UIScrollList playerList;
	public GameObjectPool itemPool;


	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		StartCoroutine("SetData");
		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void SetData()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers != null && SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers.Count > 0)
		{
			playerList.ClearList(false);
			SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers.Sort(SortPlayerList);
			for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers.Count; i++)
			{
				var container = itemPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemGuildPlayerListItem item = container.gameObject.GetComponent<UIElemGuildPlayerListItem>();
				item.SetData(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers[i]);
				playerList.AddItem(container.gameObject);
			}
		}
		
	}

	private int SortPlayerList(KodGames.ClientClass.GuildMemberInfo info1, KodGames.ClientClass.GuildMemberInfo info2)
	{
		if (info1.Online && info2.Online)
			return 0;
		else if (info1.Online)
			return -1;
		else if (info2.Online)
			return 1;

		return 0;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOkBtn(UIButton btn)
	{
		HideSelf();
	}

}