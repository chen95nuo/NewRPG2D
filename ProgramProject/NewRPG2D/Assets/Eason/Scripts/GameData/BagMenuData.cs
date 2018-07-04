using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class BagMenuData
{
    private static BagMenuData instance;
    public static BagMenuData Instance
    {
        get
        {
            if (instance == null)
            {
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Json/BagMenuData.txt", Encoding.UTF8);
                try
                {
                    string json = sr.ReadToEnd();

                    instance = JsonUtility.FromJson<BagMenuData>(json);

                    if (instance.menu == null)
                    {
                        instance.menu = new List<MenuData>();
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                sr.Close();

                // instance = new BagMenuData();
            }
            return instance;
        }
    }
    public List<MenuData> menu; // 所有菜单

    /// <summary>
    /// 查找菜单数据
    /// </summary>
    /// <param name="menuNumb">菜单层级</param>
    /// <param name="id">菜单序号</param>
    /// <returns></returns>
    public MenuData GetMenu(int menuNumb, int id)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (menu[i].MenuNumb == menuNumb && menu[i].Id == id)
            {
                return menu[i];
            }
        }
        return null;
    }
}
