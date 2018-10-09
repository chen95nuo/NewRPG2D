using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIScreenRole : TTUIPage
{
    public static UIScreenRole instance;

    public Button[] btnGroup;
    public Transform gridPoint;
    //public Button btn_All;
    //public Button btn_Atk;
    //public Button btn_Gold;
    //public Button btn_Food;
    //public Button btn_Mana;
    //public Button btn_Wood;
    //public Button btn_Iron;
    private List<HallRoleData> screenRole = new List<HallRoleData>();
    private List<UIScreenRoleGrid> RoleGrid = new List<UIScreenRoleGrid>();

    private void Awake()
    {
        instance = this;
        //btn_All.onClick.AddListener(ChickAll);
        //btn_Atk.onClick.AddListener(ChickAtk);
        //btn_Gold.onClick.AddListener(ChickGold);
        //btn_Food.onClick.AddListener(ChickFood);
        //btn_Mana.onClick.AddListener(ChickMana);
        //btn_Wood.onClick.AddListener(ChickWood);
        //btn_Iron.onClick.AddListener(ChickIron);
        btnGroup[0].onClick.AddListener(ChickAll);
        btnGroup[1].onClick.AddListener(ChickAtk);
        btnGroup[2].onClick.AddListener(ChickGold);
        btnGroup[3].onClick.AddListener(ChickFood);
        btnGroup[4].onClick.AddListener(ChickMana);
        btnGroup[5].onClick.AddListener(ChickWood);
        btnGroup[6].onClick.AddListener(ChickIron);
    }

    private void UpdateInfo()
    {
        int index = ChickPlayerInfo.instance.ChickAtrNumber();
        for (int i = 0; i < btnGroup.Length; i++)
        {
            if (i < index)
            {
                btnGroup[i].gameObject.SetActive(true);
            }
            else
            {
                btnGroup[i].gameObject.SetActive(false);
            }
        }
    }

    private void ChickAll()
    {
        screenRole = HallRoleMgr.instance.ScreenRole();
        ShowAllRole(screenRole, RoleAttribute.Max);
    }
    private void ChickAtk()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Fight);
        ShowAllRole(screenRole, RoleAttribute.Fight);
    }
    private void ChickGold()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Gold);
        ShowAllRole(screenRole, RoleAttribute.Gold);
    }
    private void ChickFood()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Food);
        ShowAllRole(screenRole, RoleAttribute.Food);
    }
    private void ChickMana()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Mana);
        ShowAllRole(screenRole, RoleAttribute.Mana);
    }
    private void ChickWood()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Wood);
        ShowAllRole(screenRole, RoleAttribute.Wood);
    }
    private void ChickIron()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Iron);
        ShowAllRole(screenRole, RoleAttribute.Iron);
    }

    private void ShowAllRole(List<HallRoleData> AllRole, RoleAttribute needAtr)
    {
        ChickGrid(AllRole.Count);
        for (int i = 0; i < AllRole.Count; i++)
        {
            RoleGrid[i].gameObject.SetActive(true);
            RoleGrid[i].UpdateInfo(AllRole[i], needAtr);
        }
        for (int i = AllRole.Count; i < RoleGrid.Count; i++)
        {
            RoleGrid[i].gameObject.SetActive(false);
        }
    }

    private void ChickGrid(int count)
    {
        if (RoleGrid.Count < count)
        {
            for (int i = RoleGrid.Count; i < count; i++)
            {
                GameObject go = Resources.Load("UIPrefab/UIScreenRoleGrid") as GameObject;
                go = Instantiate(go, gridPoint) as GameObject;
                UIScreenRoleGrid data = go.GetComponent<UIScreenRoleGrid>();
                RoleGrid.Add(data);
            }
        }
    }

}
