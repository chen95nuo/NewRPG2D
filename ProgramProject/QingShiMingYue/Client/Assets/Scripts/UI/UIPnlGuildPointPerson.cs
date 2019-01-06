using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointPerson : UIModule
{
	public UIScrollList itemList;
	public GameObjectPool itemPool;
	public UIButton[] tabBtns;
	public SpriteText noDataLable;

	private int type = ClientServerCommon.GuildStageConfig._MsgType.Person;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabBtns[0].Data = ClientServerCommon.GuildStageConfig._MsgType.Person;
		tabBtns[1].Data = ClientServerCommon.GuildStageConfig._MsgType.Guild;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (userDatas != null && userDatas.Length > 0)
			type = (int)userDatas[0];
		SetTab(type);
		RequestMgr.Inst.Request(new GuildStageQueryMsgReq(type));
		return true;
	}

	public void OnGuildStageQueryMsgResSuccess(List<com.kodgames.corgi.protocol.GuildStageMsg> msgs)
	{
		SetTab(type);
		SetData(msgs);
	}

	private void SetData(List<com.kodgames.corgi.protocol.GuildStageMsg> msgs)
	{
		itemList.ClearList(false);
		if (msgs != null)
			msgs.Sort(CompareTimeGuildStageMsg);
		for (int i = 0; i < msgs.Count; i++)
		{
			UIListItemContainer container = itemPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuildPointPerson item = container.gameObject.GetComponent<UIElemGuildPointPerson>();
			item.SetData(msgs[i]);
			container.Data = item;
			itemList.AddItem(container);
		}
		if (msgs == null || msgs.Count == 0)
			noDataLable.Text = GameUtility.GetUIString("UIPnlGuildPointPerson_NoDataLable");
		else
			noDataLable.Text = string.Empty;
	}

	private int CompareTimeGuildStageMsg(com.kodgames.corgi.protocol.GuildStageMsg l1, com.kodgames.corgi.protocol.GuildStageMsg l2)
	{
		if (l1 == null)
		{
			if (l2 == null)
				return -1;
			else
				return 0;
		}
		else
		{
			if (l2 == null)
				return -1;
			else
			{
				if (l1.time >= l2.time)
					return -1;
				else
					return 0;
			}
		}
	}

	private void SetTab(int type)
	{
		tabBtns[0].SetControlState(type == ClientServerCommon.GuildStageConfig._MsgType.Person ? UIBtnCamera.CONTROL_STATE.DISABLED : UIBtnCamera.CONTROL_STATE.NORMAL);
		tabBtns[1].SetControlState(type == ClientServerCommon.GuildStageConfig._MsgType.Guild ? UIBtnCamera.CONTROL_STATE.DISABLED : UIBtnCamera.CONTROL_STATE.NORMAL);
	}

	private void ClearData()
	{
		itemList.ClearList(false);
		type = ClientServerCommon.GuildStageConfig._MsgType.Person;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTabChange(UIButton btn)
	{
		type = (int)btn.Data;
		SetTab(type);
		RequestMgr.Inst.Request(new GuildStageQueryMsgReq((type)));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBack(UIButton btn)
	{
		HideSelf();
	}


}
