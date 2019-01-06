using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgWolfParticular : UIModule
{

	public UIScrollList descList;
	public GameObjectPool descPool;
	public SpriteText text;

	private int gainId;
	private int stargeId;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		gainId = IDSeg.InvalidId;
		stargeId = IDSeg.InvalidId;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		if (userDatas[1] != null)
		{
			gainId = (int)userDatas[1];
			stargeId = (int)userDatas[2];
		}

		if (((userDatas[0] as List<KodGames.ClientClass.WolfSelectedAddition>) == null || (userDatas[0] as List<KodGames.ClientClass.WolfSelectedAddition>).Count <= 0) && gainId == IDSeg.InvalidId)
			text.Text = GameUtility.GetUIString("UIDlgWolfParticular_Text");
		else
		{
			text.Text = "";
			StartCoroutine("FillData", (userDatas[0] as List<KodGames.ClientClass.WolfSelectedAddition>));
		}

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public void ClearData()
	{
		StopCoroutine("FillData");
		descList.ClearList(false);
		descList.ScrollPosition = 0f;
		gainId = IDSeg.InvalidId;
		stargeId = IDSeg.InvalidId;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public IEnumerator FillData(List<KodGames.ClientClass.WolfSelectedAddition> selectedAdditions)
	{
		yield return null;

		selectedAdditions.Sort((t1, t2) =>
		{
			int id1 = t1.StageId;
			int id2 = t2.StageId;

			return id1 - id2;
		});

		for (int index = 0; index < selectedAdditions.Count; index++)
		{
			UIElemParticularItem item = descPool.AllocateItem().GetComponent<UIElemParticularItem>();
			item.SetData(selectedAdditions[index].StageId, selectedAdditions[index].AdditionId);
			descList.AddItem(item.gameObject);
		}

		if (gainId != IDSeg.InvalidId && stargeId != IDSeg.InvalidId)
		{
			UIElemParticularItem item = descPool.AllocateItem().GetComponent<UIElemParticularItem>();
			item.SetData(stargeId, gainId);
			descList.AddItem(item.gameObject);
		}
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}
}
