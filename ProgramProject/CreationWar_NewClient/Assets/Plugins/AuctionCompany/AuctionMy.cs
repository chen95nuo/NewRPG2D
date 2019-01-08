using UnityEngine;
using System.Collections;
using System;

public class AuctionMy : MonoBehaviour
{
    #region 公有变量
    public GameObject itemGrid;
    #endregion

    void Start()
    {
        AuctionCompany.auctionCompany.auctionMy = this;
    }

    void OnEnable()
    {
        EnableGrid(false);
    }

    /// <summary>
    /// 控制Grid的启用和关闭
    /// </summary>
    /// <param name="isEnable"></param>
    public void EnableGrid(bool isEnable)
    {
        if (itemGrid && itemGrid.activeSelf != isEnable)
        {
            itemGrid.SetActive(isEnable);
            itemGrid.GetComponent<UIGrid>().Reposition();
        }
    }

    /// <summary>
    /// 刷新搜索结果
    /// </summary>
    public void RefreshAuctionList()
    {
        AuctionCompany.auctionCompany.ShowAuctionInfo();
    }

    private LoopScrollView m_scrollView;
    private yuan.YuanMemoryDB.YuanTable aucTable;
    /// <summary>
    /// 初始化列表，显示拍卖信息
    /// </summary>
    /// <param name="tempTable"></param>
    public void ShowAuctionItems(yuan.YuanMemoryDB.YuanTable tempTable)
    {
        EnableGrid(true);

        m_scrollView = itemGrid.transform.GetComponent<LoopScrollView>();
        m_scrollView.Init(true);
        m_scrollView.UpdateListItem(tempTable.Rows.Count);
        m_scrollView.SetDelegate(UpdateItemInfo, ItemOnClick);

        aucTable = tempTable;

        for (int i = 0; i < itemGrid.transform.childCount; i++)
        {
            if (tempTable.Rows.Count != 0 && i <= tempTable.Rows.Count)
            {
                UpdateItemInfo(itemGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 拍卖到时间时，移除此条拍卖
    /// </summary>
    /// <param name="index"></param>
    public void RemoveItem(int index)
    {
        yuan.YuanMemoryDB.YuanTable yTable = AuctionCompany.auctionCompany.myAuctionTable;
        yuan.YuanMemoryDB.YuanRow yRow = yTable.SelectRowEqual("id", index.ToString());
        if (null == yRow) return;

        yTable.Rows.Remove(yRow);
        m_scrollView.UpdateListItem(AuctionCompany.auctionCompany.myAuctionTable.Rows.Count);
        aucTable = AuctionCompany.auctionCompany.myAuctionTable;
    }

    /// <summary>
    /// 更新Item上显示的信息
    /// </summary>
    /// <param name="go">被更新的对象</param>
    void UpdateItemInfo(GameObject go)
    {
        AuctionItem item = go.GetComponent<AuctionItem>();
        if (item.gameObject.activeSelf)
        {
            int index = int.Parse(item.gameObject.name);
            item.AuctionID = int.Parse(aucTable[index]["id"].YuanColumnText);
            item.SetItemIcon(aucTable[index]["ItemID"].YuanColumnText);
            item.SetPrice(aucTable[index]["AuctionPrice"].YuanColumnText);
            //int lastTime = int.Parse(aucTable[index]["AuctionTime"].YuanColumnText) - (int)((DateTime.Now - DateTime.Parse(aucTable[index]["StartTime"].YuanColumnText)).TotalSeconds);
            int lastTime = int.Parse(aucTable[index]["AuctionTime"].YuanColumnText) - (int)((InRoom.GetInRoomInstantiate().serverTime - DateTime.Parse(aucTable[index]["StartTime"].YuanColumnText)).TotalSeconds);
            item.RefreshTime(lastTime);
        }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="i"></param>
    void ItemOnClick(GameObject go, int i)
    {
        //Debug.Log("ItemOnClick" + go.name + "," + i);
    }
}
