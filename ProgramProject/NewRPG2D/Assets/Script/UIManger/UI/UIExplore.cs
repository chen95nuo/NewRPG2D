using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIExplore : MonoBehaviour
{
    public UIExpMapGrid mapGrid;
    public UIExpMenuGrid[] menuGrids;
    public Button addMenu;
    public ScrollRect sr;

    private List<UIExpMapGrid> mapGrids = new List<UIExpMapGrid>();
    private UIExpMapGrid currentMap;
    private UIExpMenuGrid currentMenu;

    private SpriteAtlas mapSprite;

    private void Awake()
    {
        Init();

    }
    private void Init()
    {
        mapSprite = Resources.Load<SpriteAtlas>("UISpriteAtlas/ExploreMap");

        ChickMenu();
        ChickMap();
    }
    private void ChickMenu()
    {
        ExpeditionData menus = PlayerExpeditionData.Instance.items;
        for (int i = 0; i < menuGrids.Length; i++)
        {
            if (i < menus.ExpeditionTeam.Count)
            {
                menuGrids[i].UpdateMenu(menus.ExpeditionTeam[i]);
            }
            else
            {
                menuGrids[i].UpdateMenu();
            }
        }
        menuGrids[currentMenu].btn_menu.interactable = false;
    }
    private void ChickMap()
    {
        List<int> maps = PlayerExpeditionData.Instance.items.UnLockMap;
        int number = 0;
        if (maps.Count <= 2)
        {
            number = 3;
            GridsControl(number);
        }
        else
        {
            GridsControl(maps.Count + 1);
        }
        for (int i = 0; i < maps.Count; i++)
        {
            ExploreData data = GameExploreData.Instance.GetItem(maps[i]);
            Sprite sp = mapSprite.GetSprite(data.SpriteId);
            mapGrids[i].UpdateMapGrid(data, sp);
        }
        if (number > 0)
        {
            for (int i = maps.Count; i < number; i++)
            {
                mapGrids[i].UpdateMapGrid();
            }
        }
        else
        {
            mapGrids[maps.Count].UpdateMapGrid();
        }
        currentMap = mapGrids[0];
        currentMap.ChickMap(true);
    }
    private void GridsControl(int number)
    {
        mapGrid.gameObject.SetActive(false);
        Transform ts = mapGrid.transform.parent.transform;
        if (mapGrids.Count < number)
        {
            for (int i = mapGrids.Count; i < number; i++)
            {
                GameObject go = Instantiate(mapGrid.gameObject, ts) as GameObject;
                go.SetActive(true);
                UIExpMapGrid gr = go.GetComponent<UIExpMapGrid>();
                mapGrids.Add(gr);
            }
        }
        else
        {
            for (int i = 0; i < mapGrids.Count; i++)
            {
                if (i < number)
                {
                    mapGrids[i].gameObject.SetActive(true);
                }
                else
                {
                    mapGrids[i].gameObject.SetActive(false);
                }
            }
        }

    }
    public void UpdateMap(UIExpMapGrid mapData)
    {
        if (currentMap == null || currentMap != mapData)
        {
            currentMap.ChickMap(false);
            mapData.ChickMap(true);
            currentMap = mapData;
        }
        else return;
    }
}

[System.Serializable]
public class ExRolesBtn
{
    [SerializeField]
    public Button roleBtn;
    [SerializeField]
    public Text tipTop;
    [SerializeField]
    public UIBagGrid role;
    [SerializeField]
    public Text mainTip;
    [SerializeField]
    public Image addRole;

}