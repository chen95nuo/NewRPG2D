using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AuctionBuy : MonoBehaviour
{
    #region 公有变量
    public UIInput minLvl;
    public UIInput maxLvl;

    public GameObject qualityOptions;
    public GameObject equipOptions;
    public GameObject matOptions;

    public GameObject itemGrid;

    //public UIButton refreshBtn;
    //public UIButton buyBtn;
    #endregion

    #region 私有变量
    private int mMinLvl;
    private int mMaxLvl;
    private List<int> quality = new List<int>();
    private List<int> equip = new List<int>();
    private List<int> material = new List<int>();

    private int auctionID = -1; // 被点击的item上的拍卖品的拍卖id（表的主键）
    public int AuctionID
    {
        get { return auctionID; }
        set { auctionID = value; /*Debug.Log("auction id------------------------" + value);*/ }
    }
    #endregion

    #region 枚举变量
    enum Quality
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = 0,
        /// <summary>
        /// 白色
        /// </summary>
        White,
        /// <summary>
        /// 绿色
        /// </summary>
        Green,
        /// <summary>
        /// 蓝色
        /// </summary>
        Blue,
        /// <summary>
        /// 紫色
        /// </summary>
        Purple,
        /// <summary>
        /// 橙色
        /// </summary>
        Orange,
    }

    enum Equip
    {
        All = 0,               //默认值
        Helmet = 1,			    //头盔
        Breastplate = 2,		//胸甲
        Spaulders = 3,		    //护肩
        Gauntlets = 4,		    //护手
        Leggings = 5,			//护腿
        Rear = 6,				//背甲
        Belt = 7,				//腰带
        Collar = 8,			    //项链
        Ring = 9,				//戒指
        Weapon1 = 10,			//武器
        Weapon2 = 11,			//武器
        Consumables = 12,		//消耗品
        Formula = 13,			//配方
        Packs = 14			    //包裹
    }

    enum Mat
    {
        All = 0,            //所有
        Gem = 1, 			//宝石
        Fish = 2, 			//鱼
        Cooking = 3, 		//烤鱼
        Ore = 4, 			//矿石
        Stone = 5, 			//石头
        Material1 = 6, 		//副本产出1
        Material2 = 7, 		//副本产出2
        Material3 = 8, 		//副本产出3
        Key = 9,			//钥匙
    }
    #endregion

    /// <summary>
    /// 初始化本类变量
    /// </summary>
    void Awake()
    {
        if(null == qualityOptions)
        {
            qualityOptions = GameObject.Find("QualityOptions");
        }

        if (null == equipOptions)
        {
            equipOptions = GameObject.Find("EquipOptions");
        }

        if (null == matOptions)
        {
            matOptions = GameObject.Find("MatOptions");
        }
    }

	void Start () 
    {
        AuctionCompany.auctionCompany.auctionBuy = this;

        //mMinLvl = int.Parse(minLvl.value);
        //mMaxLvl = int.Parse(maxLvl.value);
        minLvl.value = "0";
        maxLvl.value = "99";
        mMinLvl = 0;
        mMaxLvl = 99;

        GetQualityOptions();
        GetEquipOptions();
        GetMatOptions();
	}

    void OnEnable()
    {
        EnableGrid(false);

        if (null != matOptions && matOptions.transform.parent.gameObject.activeSelf)
        {
            matOptions.transform.parent.gameObject.SetActive(false);
        }

        RefreshAuctionList();        
    }

    public void ResetAuctionID()
    {
       auctionID = -1;
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
    /// 获得等级范围
    /// </summary>
    private void GetLvl()
    {
        if (!int.TryParse(minLvl.value, out mMinLvl))
        {
            minLvl.value = "0";
            mMinLvl = 0;
        }

        if (!int.TryParse(maxLvl.value, out mMaxLvl))
        {
            maxLvl.value = "99";
            mMaxLvl = 99;
        }

//		Debug.Log(string.Format("===================mMinLvl::{0},mMaxLvl::{1}==========",mMinLvl, mMaxLvl));
    }

    /// <summary>
    /// 获取品质选项的参数
    /// </summary>
    private void GetQualityOptions()
    {
        quality.Clear();

        UIToggle[] toggles = qualityOptions.GetComponentsInChildren<UIToggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].value)
            {
                Quality qualityType = (Quality)Enum.Parse(typeof(Quality), toggles[i].gameObject.name);
                if (qualityType == Quality.All || qualityType == Quality.Orange)
                {
                    continue;
                }
                else if (qualityType != Quality.White)
                {
                    // 这里是套装的颜色，和普通装备颜色值相差4，搜索某种颜色的装备时，实际上是包含普通装备和套装两种装备
                    quality.Add(((int)Enum.Parse(typeof(Quality), toggles[i].gameObject.name) + 4));
                }

                quality.Add((int)Enum.Parse(typeof(Quality), toggles[i].gameObject.name));
//                Debug.Log("quality:" + i + "==========" + toggles[i].gameObject.name);
            } 
        }

        if (quality.Count == 0)
        {
            quality.Add(-1);
        }
    }

    /// <summary>
    /// 获取装备选项的参数
    /// </summary>
    private void GetEquipOptions()
    {
        equip.Clear();

        if (!equipOptions.activeSelf)
        {
            equip.Add(-1);
            return;
        }

        UIToggle[] toggles = equipOptions.GetComponentsInChildren<UIToggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].value)
            {
                Equip equipType = (Equip)Enum.Parse(typeof(Equip), toggles[i].gameObject.name);
                if (equipType == Equip.All)
                {
                    continue;
                }

                equip.Add((int)equipType);

//                Debug.Log("equip:" + i + "==========" + equipType.ToString());

                if (equipType == Equip.Weapon1)
                {
                    equip.Add((int)Equip.Weapon2);
//                    Debug.Log("equip:" + i + "==========" + Equip.Weapon2.ToString());
                }
            } 
        }

        if (equip.Count == 0)
        {
            equip.Add(-1);
        }
    }

    /// <summary>
    /// 获取材料选项的参数
    /// </summary>
    private void GetMatOptions()
    {
        material.Clear();

        if (!matOptions.activeSelf)
        {
            material.Add(-1);
            return;
        }

        UIToggle[] toggles = matOptions.GetComponentsInChildren<UIToggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].value)
            {
                Mat matType = (Mat)Enum.Parse(typeof(Mat), toggles[i].gameObject.name);
                if (matType == Mat.All)
                {
                    continue;
                }
                material.Add((int)matType);
//                Debug.Log("material:" + i + "==========" + matType.ToString());

                //if (matType == Mat.Material1)
                //{
                //    material.Add((int)Mat.Material2);
                //    Debug.Log("material:" + i + "==========" + Mat.Material2.ToString());
                //    material.Add((int)Mat.Material3);
                //    Debug.Log("material:" + i + "==========" + Mat.Material3.ToString());
                //}

                if (matType == Mat.Fish)
                {
                    material.Add((int)Mat.Cooking);
//                    Debug.Log("material:" + i + "==========" + Mat.Cooking.ToString());
                    material.Add((int)Mat.Ore);
//                    Debug.Log("material:" + i + "==========" + Mat.Ore.ToString());
                    material.Add((int)Mat.Stone);
//                    Debug.Log("material:" + i + "==========" + Mat.Stone.ToString());
                }
            }
        }

        if (material.Count == 0)
        {
            material.Add(-1);
        }
    }

    /// <summary>
    /// 刷新搜索结果
    /// </summary>
    public void RefreshAuctionList()
    {
        //Debug.Log("lvldown:" + minLvl.value + "======lvlup:" + maxLvl.value);

        GetLvl();

        if (mMinLvl < 0 || mMaxLvl < 0 || mMinLvl > mMaxLvl)
        {
            // 等级范围输入错误，请重新输入！
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info867"));
            return;
        }

        GetQualityOptions();
        GetEquipOptions();
        GetMatOptions();

        //Debug.Log(string.Format("1=================minlvl:{0}，maxLvl:{0}", mMinLvl, mMaxLvl));
        //Debug.Log(string.Format("2=================quality:::{0},equip:::{1},mat:::{2}", quality.Count, equip.Count, material.Count));
        ResetAuctionID();
        AuctionCompany.auctionCompany.RefreshAuctionInfo(mMinLvl, mMaxLvl, quality.ToArray(), equip.ToArray(), material.ToArray()); 
    }

    private LoopScrollView m_scrollView;
    private yuan.YuanMemoryDB.YuanTable aucTable;
    /// <summary>
    /// 初始化列表，显示拍卖信息
    /// </summary>
    /// <param name="tempTable"></param>
    public void ShowAuctionItems(yuan.YuanMemoryDB.YuanTable tempTable)
    {
        //if (itemGrid && !itemGrid.activeSelf)
        //{
        //    itemGrid.SetActive(true);
        //}

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
        yuan.YuanMemoryDB.YuanTable yTable = AuctionCompany.auctionCompany.auctionTable;
        yuan.YuanMemoryDB.YuanRow yRow = yTable.SelectRowEqual("id", index.ToString());
        if (null == yRow) return;

        yTable.Rows.Remove(yRow);
        m_scrollView.UpdateListItem(AuctionCompany.auctionCompany.auctionTable.Rows.Count);
        aucTable = AuctionCompany.auctionCompany.auctionTable;
    }

    /// <summary>
    /// 更新Item上显示的信息
    /// </summary>
    /// <param name="go">被更新的对象</param>
    void UpdateItemInfo(GameObject go)
    {
        AuctionItem item = go.GetComponent<AuctionItem>();
        if(item.gameObject.activeSelf)
        {
            int index = int.Parse(item.gameObject.name);
            item.AuctionID = int.Parse(aucTable[index]["id"].YuanColumnText);
            item.SetItemIcon(aucTable[index]["ItemID"].YuanColumnText);
            item.SetPrice(aucTable[index]["AuctionPrice"].YuanColumnText);
            //int lastTime = int.Parse(aucTable[index]["AuctionTime"].YuanColumnText) - (int)((DateTime.Now - DateTime.Parse(aucTable[index]["StartTime"].YuanColumnText)).TotalSeconds);
            int lastTime = int.Parse(aucTable[index]["AuctionTime"].YuanColumnText) - (int)((InRoom.GetInRoomInstantiate().serverTime - DateTime.Parse(aucTable[index]["StartTime"].YuanColumnText)).TotalSeconds);
            item.RefreshTime(lastTime);
            item.ChangeAutionBuyID();
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

        AuctionItem ai = go.GetComponent<AuctionItem>();
        auctionID = ai.AuctionID;
    }

    public void BuyAuction()
    {
        // auctionID是服务器传给客户端的记录的id号，一个id对应一条记录，这里需要取得选中条目的对应的id号
        if (auctionID < 0)
        {
            // 请选择要拍卖的物品！
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info860"));
            return;
        }

        AuctionCompany.auctionCompany.BuyAuction(auctionID);
    }
}
