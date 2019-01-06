using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlSelectOrganList : UIPnlItemInfoBase
{
	public UIScrollList organLists;
	public GameObjectPool actOrganPools;					
	public SpriteText selectedText;
	public SpriteText emptyText;

	private KodGames.ClientClass.Location organLocation;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		organLocation = userDatas[0] as KodGames.ClientClass.Location;

		Init();

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
		organLists.ClearList(false);
		organLists.ScrollPosition = 0f;
	}

	private void Init()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		
		List<KodGames.ClientClass.Beast> playerBeasts = SysLocalDataBase.Inst.LocalPlayer.Beasts;

		playerBeasts.Sort(DataCompare.CompareBeast);

		emptyText.Text = "";
		if(playerBeasts.Count == 0)
			emptyText.Text = GameUtility.GetUIString("UIPnlSelectOrganList_NoBeastTips_Label");

		for (int i = 0; i < playerBeasts.Count; i ++)
		{

			if (playerBeasts[i].Guid == this.organLocation.Guid)
				continue;

			UIListItemContainer uiContainer = actOrganPools.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemOrganBeast item = uiContainer.GetComponent<UIElemOrganBeast>();
			uiContainer.data = item;
			item.SetData(playerBeasts[i]);					
			organLists.AddItem(item.gameObject);								
		}
	}

	public void SeletOrganItem(KodGames.ClientClass.Beast organ)
	{
		RequestMgr.Inst.Request(new ChangeLocationReq(organ.Guid, organ.ResourceId, organLocation.Guid, organLocation.PositionId, organLocation.ShowLocationId, organLocation.Index));
	}

	//Click to return to UIPnlGuide.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}

	public void OnChangeBeastSuccess(KodGames.ClientClass.Location location)
	{
		HideSelf();
		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlAvatar>().OnChangeBeastSuccess(location);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOrganIcon(UIButton btn)
	{
		KodGames.ClientClass.Beast playerBeast = btn.Data as KodGames.ClientClass.Beast;

		UIPnlOrganSelectInfo.SelectDelegate selectDelegate = new UIPnlOrganSelectInfo.SelectDelegate(SeletOrganItem);

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrganSelectInfo), playerBeast, selectDelegate);
	}

	//点击更换机关兽
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectBtnClick(UIButton btn)
	{
		KodGames.ClientClass.Beast item = btn.data as KodGames.ClientClass.Beast;
		if (item == null)
			return;
		else
			SeletOrganItem(item);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItemDetailInfo(UIButton btn)
	{
		ItemInfoUtility.ShowLineUpBeastDesc(btn.Data as KodGames.ClientClass.Beast);
	}
}