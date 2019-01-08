/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class ResetProfession : MonoBehaviour 
{
    public static ResetProfession instance;
    public UILabel lblBloodstone;
    public GameObject target;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        //PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GetResetProfessionCost());
    }

	// Use this for initialization
	void Start () 
	{
	
	}

    public int costBloodstone = 0;
    public void SetInfo(int bloodstone)
    {
        costBloodstone = bloodstone;
        lblBloodstone.text = string.Format("{0}{1}{2}", StaticLoc.Loc.Get("meg0207"), bloodstone, StaticLoc.Loc.Get("info297"));
    }
	
    public void ShowPanel()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target = this.gameObject;
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "OpenPanelAndSave";
        PanelStatic.StaticWarnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("meg0154"), string.Format("{0}{1}{2},{3}", StaticLoc.Loc.Get("meg0207"), costBloodstone, StaticLoc.Loc.Get("info297"), StaticLoc.Loc.Get("meg0208")));
    }

    public void OpenPanelAndSave()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "";
        PanelStatic.StaticWarnings.warningAllEnterClose.Close();
        target.SendMessage("SaveBranch", SendMessageOptions.DontRequireReceiver);
    }
}
