using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemTavernCountryItem : MonoBehaviour
{
	public UIElemAssetIcon tavernItem;
	public AutoSpriteControlBase selectedLight;

	public void SetData(TavernConfig.Tavern tavern)
	{
		tavernItem.SetData(tavern.CountyIconId);
		tavernItem.Data = tavern;
	}

	public void SetSelectedStat(bool selected)
	{
		if (tavernItem.Data == null)
			selectedLight.Hide(true);
		else
			selectedLight.Hide(!selected);
	}
}
