/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class BuyMaterials : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}

    //购买精铁
    public void AddMeIron()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target = this.gameObject;
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "RequestBuyIronMaterial";
        PanelStatic.StaticWarnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("meg0154"), StaticLoc.Loc.Get("meg0166"));
    }

    //购买精金
    public void AddMeGold()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target = this.gameObject;
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "RequestBuyGoldMaterial";
        PanelStatic.StaticWarnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("meg0154"), StaticLoc.Loc.Get("meg0167"));
    }

    public void RequestBuyIronMaterial()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "";
        PanelStatic.StaticWarnings.warningAllEnterClose.Close();

        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().BuyIronMaterial());
    }

    public void RequestBuyGoldMaterial()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "";
        PanelStatic.StaticWarnings.warningAllEnterClose.Close();

        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().BuyGoldMaterial());
    }
}
