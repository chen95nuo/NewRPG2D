using System;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using UnityEngine;

public class UIDlgEastSeaMessages : UIModule
{
	public UIScrollList pmdList;
	public GameObjectPool pmdPool;
	public GameObjectPool morePool;
	public SpriteText noMessageLable;

	private List<string> flowMessages;
	private const int MAX_MESSAGE_COUNT = 20;
	private int count = 1;
	private UIListItemContainer moreItem;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		flowMessages = userDatas[0] as List<string>;
		if (flowMessages == null || flowMessages.Count == 0)
		{
			noMessageLable.gameObject.SetActive(true);
		}
		else
		{
			noMessageLable.gameObject.SetActive(false);
			SetData();
		}
		return true;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	private int pointItem = 0;

	private void SetData()
	{
		if (moreItem != null)
		{
			pmdList.RemoveItem(moreItem, false, true, false);
			moreItem = null;
		}

		int maxCount = IsMore(count) ? MAX_MESSAGE_COUNT : flowMessages.Count - (count - 1) * MAX_MESSAGE_COUNT;
		
		for (int i = 0; i < maxCount; i++)
			AddItem(i + pointItem, pmdPool);

		if (IsMore(count))
		{
			moreItem = morePool.AllocateItem().GetComponent<UIListItemContainer>();
			pmdList.AddItem(moreItem.gameObject);
		}

		pointItem += maxCount;
		count++;
	}

	private bool IsMore(int count)
	{
		if (flowMessages != null && flowMessages.Count > MAX_MESSAGE_COUNT * count)
			return true;
		else
			return false;
	}
	
	private void AddItem(int index, GameObjectPool pool)
	{
		var container = pool.AllocateItem().GetComponent<UIListItemContainer>();
		var item = container.GetComponent<UIElemEastSeaPMDItem>();
		if (item != null)
			item.SetData(flowMessages[index]);
		pmdList.AddItem(container);
	}

	private void SetPointItem(int index)
	{
		pmdList.ScrollToItem(index, 0);
	}

	private void ClearList()
	{
		pmdList.ClearList(false);
		pmdList.ScrollPosition = 0f;
		pointItem = 0;
		count = 1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMoreBtn(UIButton btn)
	{
		SetData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		OnHide();
	}
}