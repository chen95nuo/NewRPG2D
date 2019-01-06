using UnityEngine;
using System;
using ClientServerCommon;

public class UIElemCountryIcon : MonoBehaviour
{
	//public UIElemAssetIcon BgIcon;
	public UIElemAssetIcon countryIcon;

	//private ClientServerCommon.TavernConfig.Tavern tavern;

	//private UIListItemContainer container;
	//public UIListItemContainer Container { get { return container; } }

	public void SetData(ClientServerCommon.TavernConfig.Tavern tavern)
	{
	//	this.tavern = tavern;
		
		//Set CountryBg Icon
		//BgIcon.SetData();

		//Set AvatarIcon
		countryIcon.SetData(tavern.Id);

	}
}
