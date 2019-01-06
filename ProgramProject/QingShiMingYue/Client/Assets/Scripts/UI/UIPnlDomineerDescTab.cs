using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ClientServerCommon;

public class UIPnlDomineerDescTab : UIPnlItemInfoBase
{
	private enum PageTab
	{
		Page_Hezong = 2,
		Page_Lianheng = 1,
	}

	public UIScrollList pageScrollList;
	public GameObjectPool pagePool;
	public List<UIButton> tabBtns;

	public UIScrollList descScrollList;
	public SpriteText domineerName;
	public SpriteText domineerDesc;

	private int memoryTabSelectIndex;
	private int memoryHeZongSelectindex;
	private int memoryLianHengSelectindex;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// Set the tabBtn's data.

		memoryTabSelectIndex = (int)PageTab.Page_Hezong;
		tabBtns[0].data = PageTab.Page_Hezong;
		tabBtns[1].data = PageTab.Page_Lianheng;


		memoryHeZongSelectindex = GetCardList(PageTab.Page_Hezong)[0];
		memoryLianHengSelectindex = GetCardList(PageTab.Page_Lianheng)[0];

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;


		switch (memoryTabSelectIndex)
		{
			case (int)PageTab.Page_Hezong:
				SelectDomineer(memoryHeZongSelectindex);
				break;
			case (int)PageTab.Page_Lianheng:
				SelectDomineer(memoryLianHengSelectindex);
				break;
		}

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillCardPage");
		pageScrollList.ClearList(false);
		pageScrollList.ScrollPosition = 0;
	}

	private void SelectDomineer(int domineerId)
	{
		int type = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById(domineerId).Type;
		if (type == DomineerConfig._DomineerType.HeZong)
		{
			memoryHeZongSelectindex = domineerId;
			SelectTab(PageTab.Page_Hezong);
		}
		else if (type == DomineerConfig._DomineerType.LianHeng)
		{
			memoryLianHengSelectindex = domineerId;
			SelectTab(PageTab.Page_Lianheng);
		}
	}

	private void SetDescUI(int domineerId)
	{
		string name = ItemInfoUtility.GetAssetName(domineerId);
		StringBuilder skillLevelDesc = new StringBuilder();
		DomineerConfig.Domineer domineerConfig = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById(domineerId);

		skillLevelDesc.Append(GameUtility.FormatUIString("UIPnlDomineerDescTab_CardNameLabel", GameDefines.textColorBlack, ItemInfoUtility.GetAssetDesc(domineerId)));
		for (int i = 0; i < domineerConfig.DomineerLevels.Count; i++)
		{
			DomineerConfig.DomineerLevel levelConfig = domineerConfig.DomineerLevels[i];
			skillLevelDesc.AppendLine();
			skillLevelDesc.Append(GameUtility.FormatUIString(
									"UIPnlDomineerDescTab_CardDescLabel",
									GameDefines.textColorLightRed,
									name,
									levelConfig.Level,
									GameDefines.textColorBlack,
									levelConfig.Desc));
		}

		domineerName.Text = name;
		domineerDesc.Text = skillLevelDesc.ToString();

		//重新设置布局
		descScrollList.RepositionItems();
		descScrollList.ScrollPosition = 0f;
	}

	private void SetSelectCard(int cardId)
	{
		for (int i = 0; i < pageScrollList.Count; i++)
		{
			UIElemDomineerPictureItem item = pageScrollList.GetItem(i).gameObject.GetComponent<UIElemDomineerPictureItem>();
			if (item.SetSelect(cardId))
			{
				SetDescUI(cardId);
			}
			else
			{
				item.HideAllIcon();
			}
		}
	}

	private void SelectTab(PageTab currentPage)
	{
		memoryTabSelectIndex = (int)currentPage;

		foreach (UIButton btn in tabBtns)
		{
			PageTab pageTab = (PageTab)btn.data;
			if (pageTab == currentPage)
				btn.controlIsEnabled = false;
			else
				btn.controlIsEnabled = true;
		}

		ClearData();
		StartCoroutine("FillCardPage", currentPage);
	}

	private List<int> GetCardList(PageTab page)
	{
		List<int> cardPictures = new List<int>();

		for (int i = 0; i < ConfigDatabase.DefaultCfg.DomineerConfig.Domineers.Count; i++)
		{
			DomineerConfig.Domineer domineerConfig = ConfigDatabase.DefaultCfg.DomineerConfig.Domineers[i];

			if (page == PageTab.Page_Hezong && domineerConfig.Type == DomineerConfig._DomineerType.HeZong)
			{
				cardPictures.Add(domineerConfig.DomineerId);
			}
			else if (page == PageTab.Page_Lianheng && domineerConfig.Type == DomineerConfig._DomineerType.LianHeng)
			{
				cardPictures.Add(domineerConfig.DomineerId);
			}
		}

		cardPictures.Sort(DataCompare.CompareSkill);

		return cardPictures;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillCardPage(PageTab currentPage)
	{
		yield return null;
		List<int> cards = GetCardList(currentPage);
		int maxColumn = 4;
		List<int> cardsInRow = new List<int>();

		for (int i = 0; i < cards.Count; i++)
		{
			int domineerId = cards[i];
			cardsInRow.Add(domineerId);

			if (cardsInRow.Count == maxColumn || i == cards.Count - 1)
			{
				UIElemDomineerPictureItem item = pagePool.AllocateItem().GetComponent<UIElemDomineerPictureItem>();
				item.SetData(cardsInRow);
				item.container.gameObject.SetActive(true);
				pageScrollList.AddItem(item.container);
				cardsInRow.Clear();
			}
		}

		switch ((int)currentPage)
		{
			case DomineerConfig._DomineerType.HeZong:
				SetSelectCard(memoryHeZongSelectindex);
				break;
			case DomineerConfig._DomineerType.LianHeng:
				SetSelectCard(memoryLianHengSelectindex);
				break;
		}
	}

	// Invoke when the card's icon has been clicked : show the card information
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnIconClick(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		int cardId = (int)assetIcon.Data;

		switch (ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById(cardId).Type)
		{
			case DomineerConfig._DomineerType.LianHeng:
				memoryLianHengSelectindex = cardId;
				break;
			case DomineerConfig._DomineerType.HeZong:
				memoryHeZongSelectindex = cardId;
				break;
		}
		SetSelectCard(cardId);
	}

	// Invoke when the tab has been click.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabClose(UIButton btn)
	{
		HideSelf();
	}

	// Invoke when the tab has been click.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCardTabClick(UIButton btn)
	{
		// Show the data related to the click tab
		SelectTab((PageTab)btn.data);
	}
}
