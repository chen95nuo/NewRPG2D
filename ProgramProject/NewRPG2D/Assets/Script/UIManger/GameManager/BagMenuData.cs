using System.Collections.Generic;
using UnityEngine;

public class BagMenuData
{
    private static BagMenuData instance;
    public static BagMenuData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.BagMenuData];
                instance = JsonUtility.FromJson<BagMenuData>(json);
                if (instance.menu == null)
                {
                    instance.menu = new List<MenuData>();
                }
            }

            return instance;
        }
    }
    public List<MenuData> menu; // 所有菜单

    /// <summary>
    /// 查找菜单数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public MenuData GetMenu(string name)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (menu[i].Name == name)
            {
                return menu[i];
            }
        }
        return null;
    }
    public MenuData GetMenu(int parentNumber, int id)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (menu[i].ParentNumber == parentNumber && menu[i].Id == id)
            {
                return menu[i];
            }
        }
        return null;
    }

    public MenuData GetMenu(string name, int parentNumber)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (menu[i].ParentNumber == parentNumber && menu[i].Name == name)
            {
                return menu[i];
            }
        }
        return null;
    }
}
