using UnityEngine;
using System.Collections;

public class AuctionItem : MonoBehaviour {
 
    public UISprite itemIcon;//装备图标
    public UISprite itemBox;//图标底框
    public UILabel lblNum;  //物品数量
    public UILabel lblPrice; //拍卖价格
    public UILabel lblTime; //拍卖剩余时间

    private float startTime;
    private int totalTime;
    private int currentTime;
    private int lastTime;
    private int hours;
    private int minutes;
    private int seconds;
    private bool isCountDown = false;

    private int auctionID;   //此拍卖品的id号（表的主键）
    private string itemID;  // 装备ID

    private UIToggle toggle;

    void Awake()
    { 
        toggle = this.GetComponent<UIToggle>();
    }

    public int AuctionID
    {
      get { return auctionID; }
      set { auctionID = value; }
    }

    public void SetItemIcon(string mItemID)
    {
        if (string.IsNullOrEmpty(mItemID))
        {
            return;
        }
        itemID = mItemID;

        object[] parms = new object[4];
        parms[0] = mItemID;
        parms[1] = itemIcon;
        parms[2] = itemBox;
        parms[3] = lblNum;

        PanelStatic.StaticBtnGameManager.invcl.SendMessage("SetItemIconAsID", parms, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 刷新时间
    /// </summary>
    /// <param name="time"></param>
    public void RefreshTime(int time)
    {
        totalTime = time;
        startTime = Time.time;
        isCountDown = true;
    }

    /// <summary>
    /// 设置显示的金钱
    /// </summary>
    /// <param name="price"></param>
    public void SetPrice(string price)
    {
        lblPrice.text = price;
    }

    public void ShowItemInfo()
    {
//        Debug.Log("wei-----------------show item info------itemID" + itemID);
        if (null == PanelStatic.StaticIteminfo)
        {
            return;
        }

        PanelStatic.StaticIteminfo.SetActiveRecursively(true);
        PanelStatic.StaticIteminfo.transform.localPosition = new Vector3(-0.2875011f, 100.1449f, -5.680656f);
        PanelStatic.StaticIteminfo.SendMessage("SetItemID", itemID, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 当toggle值改变时，同时改变“购买”列表中的拍卖品选中的id
    /// </summary>
    public void ChangeAutionBuyID()
    {
        if (toggle.value)
        {
            AuctionCompany.auctionCompany.auctionBuy.AuctionID = this.auctionID;
        }
    }

    /// <summary>
    /// 当toggle值改变时，同时改变“我的”列表中的拍卖品选中的id，目前用不上，只是留好接口
    /// </summary>
    public void ChangeAutionMyID()
    {
        if (toggle.value)
        {
            // 设置Auction ID
        }
    }

    void Update()
    {
        if(isCountDown)
        {
            currentTime = (int)(Time.time - startTime);
            lastTime = (int)(totalTime - currentTime);
            hours = (int)(lastTime / (60 * 60));
            minutes = lastTime % (60 * 60) / 60;
            seconds = lastTime % (60 * 60) % 60;
            lblTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

            if (lastTime <= 0)
            {
                isCountDown = false;

                AuctionCompany.auctionCompany.auctionBuy.RemoveItem(auctionID);
                AuctionCompany.auctionCompany.auctionMy.RemoveItem(auctionID);
            }
        }  
    }
}
