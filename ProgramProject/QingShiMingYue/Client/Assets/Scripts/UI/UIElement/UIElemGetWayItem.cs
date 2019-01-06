using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIElemGetWayItem : MonoBehaviour
{
	public UIElemAssetIcon itemIcon;
	public SpriteText itemNameLabel;
	public SpriteText itemQualityLabel;
	public SpriteText itemSuiteLabel;
	public GameObjectPool suiteItemPool;
	public AutoSpriteControlBase backBg;

	private int resourceId;
	private List<GetWay> getways;
	private float originalHeight;
	private List<UIElemGotoItem> gotoItems = new List<UIElemGotoItem>();

	public void Awake()
	{
		originalHeight = backBg.height;
	}

	public void SetData(int resourceId, List<GetWay> getways)
	{
		this.getways = getways;
		this.resourceId = resourceId;

		itemIcon.SetData(resourceId);
		itemNameLabel.Text = ItemInfoUtility.GetAssetName(resourceId);
		itemQualityLabel.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(resourceId);
		itemSuiteLabel.Text = string.Empty;

		FillData();
	}

	public void SetData(int resourceId, List<GetWay> getways, SuiteConfig.AssembleSetting assemble)
	{
		List<string> formatParams = new List<string>();
		this.getways = getways;
		this.resourceId = resourceId;

		for (int i = 0; i < assemble.Parts.Count; i++)
		{
			var part = assemble.Parts[i];
			for (int j = 0; j < part.Requiremets.Count; j++)
			{
				var require = part.Requiremets[j];
				string assetName = ItemInfoUtility.GetAssetName(require.Value);
				if (resourceId == require.Value)
					assetName = GameDefines.txColorGreen + assetName + GameDefines.txColorWhite;
				formatParams.Add(assetName);
			}
		}

		string suiteDesc = string.Empty;
		if (assemble.Type == SuiteConfig._Type.EquipmentSuite)
			suiteDesc = getDecs(resourceId);
		else
			suiteDesc = GameUtility.FormatStringOnlyWithParams(assemble.Assembles[0].AssembleEffectDesc, formatParams.ToArray());

		if (assemble.Id == 0)
		{
			suiteDesc = string.Empty;
			for (int i = 0; i < formatParams.Count; i++)
				suiteDesc += formatParams[i];
		}

		itemIcon.SetData(resourceId);
		itemQualityLabel.Text = string.Empty;
		itemNameLabel.Text = ItemInfoUtility.GetAssetName(resourceId) + "—" + assemble.Name;
		itemSuiteLabel.Text = suiteDesc;

		FillData();
	}

	public string getDecs(int assetId)
	{
		switch (IDSeg.ToAssetType(assetId))
		{
			case IDSeg._AssetType.Equipment:
				return ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(assetId).activeableAssembleDesc;

			case IDSeg._AssetType.CombatTurn:
				return ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(assetId).activeableAssembleDesc;
			default:
				return string.Empty;
		}
	}

	public void ReleaseGotoItem()
	{
		for (int index = 0; index < gotoItems.Count; index++)
			suiteItemPool.ReleaseItem(gotoItems[index].gameObject);

		gotoItems.Clear();
		backBg.SetSize(backBg.width, originalHeight);
	}

	public void FillData()
	{
		float lineSpacing = 5f;
		float itemSpacing = 14f;

		if (getways == null || getways.Count <= 0)
		{
			UIElemGotoItem item = suiteItemPool.AllocateItem().GetComponent<UIElemGotoItem>();
			item.SetData(resourceId);
			gotoItems.Add(item);

			item.transform.parent = backBg.transform;
			item.transform.localPosition = new Vector3(0, (-1) * (backBg.height - lineSpacing), 0);
			backBg.height = backBg.height + item.getwayBg.height + itemSpacing;
		}
		else
		{
			for (int i = 0; i < getways.Count; i++)
			{
				UIElemGotoItem item = suiteItemPool.AllocateItem().GetComponent<UIElemGotoItem>();
				item.SetData(getways[i]);
				gotoItems.Add(item);

				item.transform.parent = backBg.transform;
				item.transform.localPosition = new Vector3(0, (-1) * (backBg.height - lineSpacing), 0);
				backBg.height = backBg.height + item.getwayBg.height + itemSpacing;
			}
		}
	}
}
