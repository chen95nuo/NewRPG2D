using UnityEngine;
using System.Collections;

public class AuctionSell : MonoBehaviour {
    public GameObject StorageIconTemplate6;
    public GameObject parentLeft;
    public UILabel lblAuctionCount;
    public UIInput price;
    public UIToggle hours24;
    public UIToggle hours48;
    private string itemID;

    public string ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }
    private int fixPrice;
    private int hours;

    void Start()
    {
        AuctionCompany.auctionCompany.auctionSell = this;
    }

    void OnEnable()
    {
        ClearItemBox();
        AuctionCompany.auctionCompany.ShowAuctionCount();
    }

    /// <summary>
    /// 卖出装备
    /// </summary>
    public void AuctionOut()
    {
        StorageIconTemplate6.SendMessage("GetItemID", this, SendMessageOptions.RequireReceiver);
        if (string.IsNullOrEmpty(itemID))
        {
            // 请选择需要拍卖的物品！
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info856"));
            return;
        }

        if (!int.TryParse(price.value, out fixPrice) || fixPrice <= 0 || fixPrice > 9999999)
        {
            // 价格超出范围，请重新输入！
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info853"));
            return;
        }

        if (hours24.value)
        {
            hours = 24;
        }
        else
        {
            hours = 48;
        }

        AuctionCompany.auctionCompany.SellOut(itemID, fixPrice, hours);       
    }

    /// <summary>
    /// 清空右边方框显示的物品图标（当卖出时需立即清空，或者切换UI时也需清空）
    /// </summary>
    public void ClearItemBox()
    {
        StorageIconTemplate6.SendMessage("invClear", SendMessageOptions.RequireReceiver);
        //整理背包
        
        PanelStatic.StaticBtnGameManager.invcl.SendMessage("ReInitItem", SendMessageOptions.DontRequireReceiver);
        parentLeft.SendMessage("ReInitItem", SendMessageOptions.RequireReceiver);
        parentLeft.SendMessage("SelectBag1", SendMessageOptions.RequireReceiver);

        itemID = "";
    }

    /// <summary>
    /// 购买拍卖位
    /// </summary>
    public void BuyAuctionSlot()
    {
        if(totalCount >= CommonDefine.AUCTION_MAX_SLOT)
        {
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info875"));// 提示，对不起，栏位已达上限，不能购买！
            return;
        }

        int price = totalCount * CommonDefine.AUCTION_SLOT_PRICE;

        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target = this.gameObject;
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "RequestBuySlot";
        PanelStatic.StaticWarnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("meg0154"), string.Format("{0}{1}{2}", StaticLoc.Loc.Get("meg0169"), price, StaticLoc.Loc.Get("meg0170")));
    }

    public void RequestBuySlot()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "";
        PanelStatic.StaticWarnings.warningAllEnterClose.Close();
        AuctionCompany.auctionCompany.BuyAuctionSlot(1);
    }

    private int totalCount = 0;
    /// <summary>
    /// 设置拍卖次数
    /// </summary>
    /// <param name="lastCout">剩余拍卖次数</param>
    /// <param name="count">总拍卖次数</param>
    public void SetAuctionCount(int lastCout, int count)
    {
        if (null != lblAuctionCount)
        {
            totalCount = count;
            lblAuctionCount.text = string.Format("{0}/{1}", lastCout, count);
        }
    }
}
