using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDanXiang : UIModule
{
	public SpriteText pageText;
	public UIScrollList magessageList;
	public GameObjectPool poolGameObject;

	private List<KodGames.ClientClass.DanAttributeGroup> danAttriList = new List<KodGames.ClientClass.DanAttributeGroup>();
	private int pageNum = 1;
	private int maxPageNum = 0;
	private int level = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (userDatas != null && userDatas.Length > 0)
		{
			danAttriList = userDatas[0] as List<KodGames.ClientClass.DanAttributeGroup>;
			level = (int)userDatas[1];
			maxPageNum = danAttriList.Count;
		}
		SetData();
		return true;
	}



	private void SetData()
	{
		ClearData();
		//List<KodGames.ClientClass.DanAttribute> listAttributes=danAttriList[pageNum].DanAttributes;
		pageText.Text = string.Format("{0}/{1}", pageNum, maxPageNum);
		List<KodGames.ClientClass.PropertyModifierSet> propertyModifiterList = danAttriList[pageNum-1].DanAttributes[0].PropertyModifierSets;
		foreach (var property in propertyModifiterList)
		{
			UIListItemContainer container = poolGameObject.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemDanCultureXiantItem item = container.gameObject.GetComponent<UIElemDanCultureXiantItem>();
			string context = string.Format("{0}+{1}", danAttriList[pageNum - 1].AttributeDesc, GameUtility.FormatUIString("UIPnlDanInfo_UpAttr_Label", System.Math.Round((property.Modifiers[0].AttributeValue * 100), 3)));
			item.SetData(property.Level, context,level==property.Level);
			container.Data = item;
			magessageList.AddItem(container);
		}
	}

	private void ClearData()
	{
		magessageList.ClearList(false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPrePageBtn(UIButton btn)
	{
		
		pageNum = pageNum - 1 <= 0 ? 1 : pageNum - 1;
		SetData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickNextPageBtn(UIButton btn)
	{
		pageNum = pageNum + 1 >= maxPageNum ? maxPageNum : pageNum + 1;
		SetData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCloseBtn(UIButton btn)
	{
		ClearData();
		HideSelf();
	}
}
