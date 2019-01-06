using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlOrgansBeastTab : UIModule
{
	public UIScrollList organLists;
	public GameObjectPool actOrganPools;
	public GameObjectPool titlePoos;
	public GameObjectPool unActOrganPools;		
	//public SpriteText notActivityLabel;
	public SpriteText selectedText;	

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlOrganTab>().ChangeTabButtons(_UIType.UIPnlOrgansBeastTab);

		Init();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();
		
		Init();
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

	public void OnActiveBeastSuccess()
	{
		Debug.Log("ActiveBeastSuccess");
		Init();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		List<BeastConfig.BaseInfo> beastInfos = ConfigDatabase.DefaultCfg.BeastConfig.BaseInfos;
		List<KodGames.ClientClass.Beast> playerBeasts = SysLocalDataBase.Inst.LocalPlayer.Beasts;
		List<BeastConfig.BaseInfo> unActBeasts = new List<BeastConfig.BaseInfo>();

		foreach (var beastInfo in beastInfos)
		{
			bool isActive = false;

			for (int i = 0; i < playerBeasts.Count; i ++)
			{
				if(playerBeasts[i].ResourceId == beastInfo.Id)
					isActive = true;
			}

			//添加未激活机关兽，筛除不显示机关兽
			if (!isActive && beastInfo.IsShow)
				unActBeasts.Add(beastInfo);
		}

		playerBeasts.Sort(DataCompare.CompareBeast);

		foreach (var beast in playerBeasts)
		{
			UIListItemContainer uiContainer = actOrganPools.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemOrganBeast item = uiContainer.GetComponent<UIElemOrganBeast>();
			uiContainer.data = item;
			item.SetData(beast);
			organLists.AddItem(item.gameObject);	
		}

		if(unActBeasts.Count > 0)
		{
			UIElemOrganTitleLabel titleitem = titlePoos.AllocateItem().GetComponent<UIElemOrganTitleLabel>();
			titleitem.SetData(GameUtility.GetUIString("UIPnlOrgansBeastTab_DescribePool_Context"));
			organLists.AddItem(titleitem.gameObject);
		}
		
		unActBeasts.Sort(DataCompare.CompareBeast);
		foreach (var beast in unActBeasts)
		{
			UIListItemContainer uiContainer = unActOrganPools.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemUnActOrganBeast item = uiContainer.GetComponent<UIElemUnActOrganBeast>();
			uiContainer.data = item;
			item.SetData(beast);
			organLists.AddItem(item.gameObject);	
		}	
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetSkill(UIButton btn)
	{
		
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItemDetailInfo(UIButton btn)
	{		
		ItemInfoUtility.ShowLineUpBeastDesc(btn.Data as KodGames.ClientClass.Beast);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOrganIcon(UIButton btn)
	{
		KodGames.ClientClass.Beast playerBeast = btn.Data as KodGames.ClientClass.Beast;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrganInfo), playerBeast);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickActiveOrganIcon(UIButton btn)
	{
		KodGames.ClientClass.Beast playerBeast = btn.Data as KodGames.ClientClass.Beast;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrganGrowPage), playerBeast);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickUnActiveOrganIcon(UIButton btn)
	{		
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrganInfo), (int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOrganActiveBtn(UIButton btn)
	{	
		RequestMgr.Inst.Request(new ActiveBeastReq((int)btn.Data));
	}
}