using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlRecruitShow : UIModule
{
	public SpriteText recruitDescLabel;
	public UIScrollList recruitTypeIconList;
	public GameObjectPool recruitTypePool;
	public UIScrollList avatarIconList;
	public GameObjectPool avatarIconPool;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas[0] is ClientServerCommon.TavernConfig.Tavern)
		{
			ClientServerCommon.TavernConfig.Tavern tavern = userDatas[0] as ClientServerCommon.TavernConfig.Tavern;
			FillData(tavern.Id);
		}
		return true;
	}

	private void FillData(int tavernId)
	{
		// Set Desc Label.
		int minQuality = 5;
		int maxQuality = 0;
		var showCards = ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(tavernId).ShowResourceIds;

		for (int i = 0; i < showCards.Count; i++)
		{
			int qualityLevel = ItemInfoUtility.GetAssetQualityLevel(showCards[i]);

			if (qualityLevel > maxQuality)
				maxQuality = qualityLevel;

			if (qualityLevel < minQuality)
				minQuality = qualityLevel;
		}

		recruitDescLabel.Text = ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(tavernId).RewardDesc;

		ClearData();
		StartCoroutine("FillCountryList", tavernId);
		StartCoroutine("FillAvatarList", tavernId);
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillCountryList");
		StopCoroutine("FillAvatarList");
		avatarIconList.ClearList(false);
		recruitTypeIconList.ClearList(false);
		avatarIconList.ScrollPosition = 0f;
		recruitTypeIconList.ScrollPosition = 0F;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillCountryList(int tavernId)
	{
		yield return null;
		var shopData = SysLocalDataBase.Inst.LocalPlayer.ShopData;
		List<TavernConfig.Tavern> taverns = ConfigDatabase.DefaultCfg.TavernConfig.Taverns;
		taverns.Sort(DataCompare.CompareTavern);

		foreach (var tavernCfg in ConfigDatabase.DefaultCfg.TavernConfig.Taverns)
		{
			var tavernData = shopData.GetTavernInfoById(tavernCfg.Id);
			if (tavernData == null || !tavernCfg.IsOpen)
				continue;

			UIListItemContainer uiContainer = recruitTypePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemTavernCountryItem item = uiContainer.GetComponent<UIElemTavernCountryItem>();
			uiContainer.data = item;
			item.SetData(tavernCfg);

			if (tavernCfg.Id == tavernId)
				item.SetSelectedStat(true);
			else
				item.SetSelectedStat(false);

			recruitTypeIconList.AddItem(item.gameObject);
		}

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillAvatarList(int tavernId)
	{
		yield return null;

		TavernConfig.Tavern tavernShow = ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(tavernId);

		List<int> avatarIds = new List<int>();
		int rewardRow = 4;

		for (int i = 0; i <= tavernShow.ShowResourceIds.Count / rewardRow; i++)
		{
			if (i == tavernShow.ShowResourceIds.Count &&
				tavernShow.ShowResourceIds.Count <= rewardRow * i)
				continue;

			for (int j = 0; j < rewardRow && i * rewardRow + j < tavernShow.ShowResourceIds.Count; j++)
				avatarIds.Add(tavernShow.ShowResourceIds[i * rewardRow + j]);

			UIListItemContainer uiContainer = avatarIconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemRecruitList item = uiContainer.GetComponent<UIElemRecruitList>();
			uiContainer.data = item;
			item.SetData(avatarIds);
			avatarIconList.AddItem(item.gameObject);

			avatarIds.Clear();
		}
	}

	public void SetLight(int tavernId)
	{
		for (int i = 0; i < recruitTypeIconList.Count; i++)
		{
			UIListItemContainer container = recruitTypeIconList.GetItem(i) as UIListItemContainer;
			UIElemTavernCountryItem countryElem = (UIElemTavernCountryItem)container.Data;
			var tavern = countryElem.tavernItem.Data as TavernConfig.Tavern;

			if (countryElem == null)
				continue;

			if (tavern.Id == tavernId)
			{
				countryElem.SetSelectedStat(true);
				Debug.Log("TAVERN.ID: " + tavern.Id.ToString());
			}
			else
				countryElem.SetSelectedStat(false);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRecruitTypeClick(UIButton btn)
	{
		//On Click Type Btn
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var tavernShow = assetIcon.Data as TavernConfig.Tavern;
		FillData(tavernShow.Id);


		//SetLight(tavern.Id);
		//StopCoroutine("FillAvatarList");
		//avatarIconList.ScrollPosition = 0f;
		//avatarIconList.ClearList(false);
		//StartCoroutine("FillAvatarList", tavernShow.Id);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAvatarIconClick(UIButton btn)
	{
		//On Click Avatar Icon

		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById((int)btn.Data);

		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIDlgAvatarInfo, avatarCfg, false, true, false, true, null);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		//Hide
		HideSelf();
	}
}
