using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIPnlGuildMessage : UIModule
{
	public UIScrollList messageList;
	public GameObjectPool objectPool;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlGuildMessageTab>().ChangeTabButtons(_UIType.UIPnlGuildMessage);

		RequestMgr.Inst.Request(new GuildQueryNewsReq());

		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.NewsLeft = 0;

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
		messageList.ClearList(false);
		messageList.ScrollListTo(0f);
	}

	public void RequestQueryNewsSuccess(List<KodGames.ClientClass.GuildNews> guildNews)
	{
		guildNews.Sort(DataCompare);
		StartCoroutine("FillList", guildNews);
	}

	public void AddOneNews(KodGames.ClientClass.GuildNews guildnew)
	{
		UIListItemContainer container = objectPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemGuildMessageItem item = container.gameObject.GetComponent<UIElemGuildMessageItem>();
		item.SetData(guildnew);
		container.Data = item;

		messageList.InsertItem(container, 0);
		messageList.ScrollToItem(0, 0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(List<KodGames.ClientClass.GuildNews> guildNews)
	{
		yield return null;

		for (int index = 0; index < guildNews.Count; index++)
		{
			UIElemGuildMessageItem item = objectPool.AllocateItem().GetComponent<UIElemGuildMessageItem>();
			item.SetData(guildNews[index]);
			messageList.AddItem(item.gameObject);
		}
	}

	private int DataCompare(KodGames.ClientClass.GuildNews n1, KodGames.ClientClass.GuildNews n2)
	{
		return (int)n2.Time - (int)n1.Time;
	}
}
