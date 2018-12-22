using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour, ProcessResponse, BWWarnUI{

    public GameObject dragPanel;    //draggable panel
    public GameObject cellParent;   //UIGrid

    public GameObject leftShop;     //shop content
    public GameObject leftBlack;    //refresh
	public GameObject leftPvpShop;	//pvp shop
    public UILabel labelTime;       //last refresh time
    public UILabel labelRefNum;     //gold crystal num
	public UILabel labelPvpRefNum;
	public UILabel labelHonornum;
    public UISprite texRefIcon;     //gold crystal icon

    public UILabel labelVip;        //vip level
    public UILabel labelGold;       //player gold
    public UILabel labelCrystal;    //player crystal

    public GameObject objDetail;    //shop detail panel
    public GameObject objBuySucc;   //shop buy success
    public GameObject objBoxShow;   //shop box show

    public GameObject objShopCount; //shop count GameObject
    public UILabel labelShopName;   //buy count panel name
    public UILabel labelShopNum;    //num
    public UISprite texCosttype;    //cost type
    public UILabel labelCost;       //cost
	public UILabel pvpRefTip;

    public UISprite[] butShop = new UISprite[2];

    List<GameObject> shopIcons = new List<GameObject>();

    private ShopResultJson shopResJson;
    public ShopResultJson ShopResJson
    {
        get { return shopResJson; }
        set { shopResJson = value; }
    }
    private BuyPowerOrGoldResultJson buyResJson;

    private int mode;           //1商店   2黑市   4pvp商城  5刷新pvp商城//
    private MessType messType;  //发送消息类型
    private enum MessType
    {
        ShopUi = 1,
        BlackUi,
        BlackRefresh,
        ShopBuy,
        BlackBuy,
        BlackBuyPos,
		PvpShop,
		PvpShopBuy,
		PvpShopRefresh,
    };
    bool receiveData = false;
    int detailId;
	//cxl--黑市开格子消耗钻石数//
	int blackOpenBoxCost;

    Dictionary<int, int> ses1 = new Dictionary<int, int>(); //id,num    shopResJson.ses1
    Dictionary<int, int> ses2 = new Dictionary<int, int>(); //id,num    shopResJson.ses2
    int time;
    float timeTemp;
    bool bAutoReqFlag;  //自动请求时为true//
    int refreshNum;
	int pvpRefreshNum;
	int honorNum;
    int nowBlackNum;    //当前黑市位置数//

    int buyNum;         //num
    const int BuyNumMax = int.MaxValue;

    Vector3 dragPanelPos;
    Vector4 dragPanelClip;

    string shopCellPrefabPath = "Prefabs/UI/UI-Shop/shopcell";
	
	int errorCode = 0;

	// Use this for initialization
	void Awake () {
		pvpRefTip.text = TextsData.getData(695).chinese;
	}
	
	// Update is called once per frame
	void Update () {

        updateMess();

        updateTimeLabel();
	}

    public void show(int mode)
    {
        Main3dCameraControl.mInstance.SetBool(true);
        
        initShopUi(mode);
        resolveShopJson();
        initShopPrefab();
    }

    public void receiveResponse(string json)
    {
        if (json == null) return;
		Debug.Log("ShopUI========"+json);
        receiveData = true;

        PlayerInfo.getInstance().isShowConnectObj = false;
        switch (messType)
        {
            case MessType.ShopUi:
            case MessType.BlackUi:
            case MessType.BlackRefresh:
			case MessType.PvpShop:
			case MessType.PvpShopRefresh:
                shopResJson = JsonMapper.ToObject<ShopResultJson>(json);
				errorCode = shopResJson.errorCode;
                break;
            case MessType.ShopBuy:
            case MessType.BlackBuy:
            case MessType.BlackBuyPos:
			case MessType.PvpShopBuy:
                buyResJson = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = buyResJson.errorCode;
                break;
        }
    }

    void OnBtnClose()
    {
        Main3dCameraControl.mInstance.SetBool(false);
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);

        UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SHOP);
		
        //通知背包刷新界面//
		PackUI pack = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_PACK, "PackUI" )as PackUI;
		if(pack != null)
		{
			pack.SendToGetItemData(true);
		}
    }

    void OnBtnPage(int param)
    {
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);

        if (param == 1 || param == 2||param == 4)
        {
            if (param == mode) return;
			int index = 0;
			if(param == 1 || param == 2)
			{
				index = param - 1;
			}
			else if(param == 4)
			{
				index = param - 2;
			}
            setButColorGray(butShop,index);
        }

        BtnPageNow(param);
    }

    void BtnPageNow(int param)
    {
        switch (param)
        {
            case 1:
                messType = MessType.ShopUi;
                mode = 1;
                break;
            case 2:
                messType = MessType.BlackUi;
                mode = 2;
                break;
            case 3:
                messType = MessType.BlackRefresh;
                break;
			case 4:
				mode = 4;
				messType = MessType.PvpShop;
				break;
			case 5:			
				messType = MessType.PvpShopRefresh;
				break;
        }
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SHOP, param), this);
    }

    void OnBtnBlackShow(int param)
    {
        objBoxShow.GetComponent<ShopBoxShow>().show();
    }

    void OnBtnItem(int param)
    {
        //Debug.Log("--- OnBtnItem param:" + param);
        detailId = param;
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (mode == 2 && detailId == 0) //black add pos
        {
            BlackShopboxData dd = BlackShopboxData.getData(nowBlackNum + 1);
            ToastWarnUI.mInstance.showWarn(TextsData.getData(567).chinese + dd.cost + TextsData.getData(dd.costtype == 1 ? 568 : 569).chinese, this, false);
			blackOpenBoxCost = dd.cost;
        }
        else
        {
            objDetail.GetComponent<ShopDetail>().Init(mode, detailId);
        }
    }

    void OnBtnBuy()
    {
        if (mode == 1)
        {
            initShopCount();
        }
        else if (mode == 2)
        {
            messType = MessType.BlackBuy;

            BuyPowerOrGoldJson json = new BuyPowerOrGoldJson();
            json.PackageShopJson(8, 2, 2, detailId);
            PlayerInfo.getInstance().sendRequest(json, this);
        }
		else if (mode == 4)
		{
			messType = MessType.PvpShopBuy;
			
            BuyPowerOrGoldJson json = new BuyPowerOrGoldJson();
            json.PackageShopJson(8, 2, 3, detailId);
            PlayerInfo.getInstance().sendRequest(json, this);
		}
    }

    void OnBtnCloseDetail()
    {
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        objDetail.SetActive(false);
    }

    void OnBtnCount(int param)
    {
        switch (param)
        {
            case 1: buyNum -= 1;break;
            case 2: buyNum -= 10; break;
            case 3: buyNum += 1; break;
            case 4: buyNum += 10; break;
        }
        refreshShopCountLabel();
    }

    void OnBtnCountBuy()
    {
        messType = MessType.ShopBuy;

        BuyPowerOrGoldJson json = new BuyPowerOrGoldJson();
        json.PackageShopJson(8, 2, 1, detailId, buyNum);
        PlayerInfo.getInstance().sendRequest(json, this);
    }

    void OnBtnCloseCount()
    {
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        objShopCount.SetActive(false);
    }

    public void warnningSure()
    {
		//黑市开箱子//
        if (mode == 2 && detailId == 0)
        {
            BuyPowerOrGoldJson json = new BuyPowerOrGoldJson();
            messType = MessType.BlackBuyPos;
            json.PackageShopJson(9, 2, 0, 0);
            PlayerInfo.getInstance().sendRequest(json, this);
        }
    }

    public void warnningCancel() { }

    void OnApplicationPause(bool flag)
    {
        if (!flag && mode == 2)
            BtnPageNow(mode);
    }

    void initShopUi(int mode)
    {
        this.mode = mode;
		switch(mode)
		{
		case 2:
			messType = MessType.BlackUi;
	        setButColorGray(butShop, mode - 1);
			break;
		case 4:
			messType = MessType.PvpShop;
	        setButColorGray(butShop, mode - 2);
			break;
		}
		labelVip.text = PlayerInfo.getInstance().player.vipLevel.ToString();
		updateLeftPic();
		refreshGoldCrystal();
		saveDragPanelPos();
    }

    void saveDragPanelPos()
    {
        dragPanelPos = dragPanel.transform.localPosition;
        dragPanelClip = dragPanel.GetComponent<UIPanel>().clipRange;
    }

    void resetDragPanelPos()
    {
        dragPanel.GetComponent<SpringPanel>().enabled = false;
        dragPanel.transform.localPosition = dragPanelPos;
        dragPanel.GetComponent<UIPanel>().clipRange = dragPanelClip;
    }

    void enableDragContents(bool flag = false)
    {
        foreach (GameObject obj in shopIcons)
        {
            obj.GetComponent<UIDragPanelContents>().enabled = flag;
        }
    }

    void setButColorGray(UISprite[] sprs, int selId)
    {
        for (int i = 0; i < sprs.Length; i++)
        {
            if (i == selId)
                butShop[i].color = Color.white;
            else
                butShop[i].color = Color.gray;
        }
    }

    void initShopCount()
    {
        objDetail.SetActive(false);
        objShopCount.SetActive(true);

        buyNum = 1;

        ShopData data = ShopData.getData(detailId);
        labelShopName.text = data.name;
        labelShopNum.text = buyNum.ToString();
        texCosttype.spriteName = data.costtype1 == 2 ? "gold" : "crystal2";
        labelCost.text = data.cost.ToString();
    }

    void refreshShopCountLabel()
    {
        if (buyNum < 1)
            buyNum = 1;
        else if (buyNum > lastShopNum(detailId))
            buyNum = lastShopNum(detailId);

        ShopData data = ShopData.getData(detailId);
        labelShopNum.text = buyNum.ToString();
        labelCost.text = (data.cost * buyNum).ToString();
    }

    void refreshGoldCrystal()
    {
        if (PlayerInfo.getInstance().player.gold < 0)
            PlayerInfo.getInstance().player.gold = 0;
        if (PlayerInfo.getInstance().player.crystal < 0)
            PlayerInfo.getInstance().player.crystal = 0;
        labelGold.text = PlayerInfo.getInstance().player.gold.ToString();
        labelCrystal.text = PlayerInfo.getInstance().player.crystal.ToString();

        //background gold refresh
        HeadUI.mInstance.refreshPlayerGold();
    }

    void resolveShopJson()
    {
        ses1.Clear();
        ses2.Clear();

        if (shopResJson.ses1 != null)
        {
            foreach (ShopElement e in shopResJson.ses1)
            {
                ses1.Add(e.id, e.num);
            }
        }
        if (shopResJson.ses2 != null)
        {
            foreach (ShopElement e in shopResJson.ses2)
            {
                ses2.Add(e.id, e.num);
            }
        }

        if (messType == MessType.BlackUi || messType == MessType.BlackRefresh)
        {
            //black market time
            setTimeNum(shopResJson.refreshTime);

            //update next refresh resume
            refreshNum = shopResJson.refresh;
            int temp = refreshNum >= BlackRefreshData.getTotalNum() ? BlackRefreshData.getTotalNum() : (refreshNum + 1);
            BlackRefreshData blackRefresh = BlackRefreshData.getData(temp);
            if (blackRefresh.costtype == 1)
                texRefIcon.spriteName = "crystal1";
            else if (blackRefresh.costtype == 2)
                texRefIcon.spriteName = "gold";
            labelRefNum.text = blackRefresh.costnumber.ToString();

            //update player gold/crystal
            if (messType == MessType.BlackRefresh)
            {
                int temp2 = refreshNum >= BlackRefreshData.getTotalNum() ? BlackRefreshData.getTotalNum() : refreshNum;
                BlackRefreshData blackRefresh2 = BlackRefreshData.getData(temp2);
                if (blackRefresh2.costtype == 1)
                    PlayerInfo.getInstance().player.crystal -= blackRefresh2.costnumber;
                else if (blackRefresh2.costtype == 2)
                    PlayerInfo.getInstance().player.gold -= blackRefresh2.costnumber;
                refreshGoldCrystal();
            }
			
            //auto request reset
            bAutoReqFlag = false;

            nowBlackNum = ses1.Count;
        }
		else if(messType == MessType.PvpShop||messType == MessType.PvpShopRefresh)
		{
			honorNum = shopResJson.pvpHonor;
			labelHonornum.text = honorNum.ToString();
			pvpRefreshNum = shopResJson.refresh;
			int temp = pvpRefreshNum >= BlackRefreshData.getTotalNum() ? BlackRefreshData.getTotalNum() : (pvpRefreshNum + 1);
			BlackRefreshData blackRefresh = BlackRefreshData.getData(temp);
			labelPvpRefNum.text = blackRefresh.costnumber.ToString();
			if(messType == MessType.PvpShopRefresh)
			{
				int temp2 = pvpRefreshNum >= BlackRefreshData.getTotalNum() ? BlackRefreshData.getTotalNum() : pvpRefreshNum;
                BlackRefreshData blackRefresh2 = BlackRefreshData.getData(temp2);
                if (blackRefresh2.costtype == 1)
                    PlayerInfo.getInstance().player.crystal -= blackRefresh2.costnumber;
                else if (blackRefresh2.costtype == 2)
                    PlayerInfo.getInstance().player.gold -= blackRefresh2.costnumber;
                refreshGoldCrystal();
			}
		}
    }

    void initShopPrefab()
    {
        foreach (GameObject obj in shopIcons)
        {
            obj.transform.parent = null;
            Destroy(obj);
        }
        shopIcons.Clear();

        Object res = Resources.Load(shopCellPrefabPath);
        if (mode == 1)
        {
            int count = 0;
            foreach (KeyValuePair<int, ShopData> temp in ShopData.getAllData())
            {
                GameObject obj = instantiateObj(res, cellParent, res.name + (++count + 1000));
                shopIcons.Add(obj);

                ShopCell cell = obj.GetComponent<ShopCell>();
                cell.Init(temp.Value);
                initCallback(obj, temp.Key);

                //if ((ses1.ContainsKey(temp.Key) && ses1[temp.Key] >= temp.Value.dailynumber) || (ses2.ContainsKey(temp.Key) && ses2[temp.Key] >= temp.Value.totalnumber)) cell.SellOut(true);
                if (lastShopNum(temp.Key) <= 0)
                    cell.SellOut(true);
            }
        }
        else if (mode == 2)
        {
            int count = 0;
            foreach (KeyValuePair<int, int> temp in ses1)
            {
                GameObject obj = instantiateObj(res, cellParent, res.name + (++count + 1000));
                shopIcons.Add(obj);

                ShopCell cell = obj.GetComponent<ShopCell>();
                BlackMarketData market = BlackMarketData.getData(temp.Key);
                cell.Init(market);
                initCallback(obj, temp.Key);

                if (temp.Value > 0)
                    cell.SellOut(true);
            }
            if (ses1.Count < BlackShopboxData.getTotalNum())
            {
                GameObject obj = instantiateObj(res, cellParent, res.name + (++count + 1000));
                shopIcons.Add(obj);

                ShopCell cell = obj.GetComponent<ShopCell>();
                cell.Init(count);
                initCallback(obj, 0);
            }

            enableDragContents();
        }
		else if(mode == 4)
		{
			int count = 0;
			foreach(KeyValuePair<int,int> temp in ses1)
			{
				GameObject obj = instantiateObj(res, cellParent, res.name + (++count + 1000));
				shopIcons.Add(obj);
				ShopCell cell = obj.GetComponent<ShopCell>();
				ShopPvpData spd = ShopPvpData.getData(temp.Key);
				cell.Init(spd);
				initCallback(obj, temp.Key);
				
                if (temp.Value > 0)
                    cell.SellOut(true);				
			}
		}

        resetDragPanelPos();
        cellParent.GetComponent<UIGrid>().repositionNow = true;
    }

    GameObject instantiateObj(Object res, GameObject parent, string name)
    {
        GameObject obj = Instantiate(res) as GameObject;
        obj.transform.parent = parent.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.name = name;
        return obj;
    }

    void initCallback(GameObject obj, int param)
    {
        UIButtonMessage mess = obj.GetComponent<UIButtonMessage>();
        mess.target = gameObject;
        mess.functionName = "OnBtnItem";
        mess.param = param;
    }

    int lastShopNum(int id)
    {
        ShopData data = ShopData.getData(id);
        int num1 = ses1.ContainsKey(id) ? data.dailynumber - ses1[id] : BuyNumMax;
        int num2 = ses2.ContainsKey(id) ? data.totalnumber - ses2[id] : BuyNumMax;
        return num1 > num2 ? num2 : num1;
    }

    void setTimeNum(int t)
    {
        time = t;
        timeTemp = 0;

        showLabelTime();
    }

    void updateMess()
    {
        if (!receiveData) return;
        receiveData = false;
		if(errorCode == -3)
			return;

        if (messType == MessType.ShopUi || messType == MessType.BlackUi || messType == MessType.BlackRefresh||messType == MessType.PvpShop||messType == MessType.PvpShopRefresh)
        {
            if (shopResJson.errorCode == 0)
            {
                updateLeftPic();
                
                resolveShopJson();
                initShopPrefab();
				
				if(messType == MessType.BlackRefresh)
				{
            		int temp = refreshNum >= BlackRefreshData.getTotalNum() ? BlackRefreshData.getTotalNum() : (refreshNum + 1);
            		BlackRefreshData blackRefresh = BlackRefreshData.getData(temp);
					if(!TalkingDataManager.isTDPC)
					{
						int useCost = blackRefresh.costnumber;
						TDGAItem.OnPurchase("refreshblackmarket",1,useCost);
					}
				}
            }
            else
            {
                showWrongMess(shopResJson.errorCode);
            }
        }
        else if (messType == MessType.ShopBuy)
        {
            objShopCount.SetActive(false);
            if (buyResJson.errorCode == 0)
            {
                //refresh gold&crystal
                ShopData data = ShopData.getData(detailId);
                if (data.costtype1 == 1)
                {
                    PlayerInfo.getInstance().player.crystal -= data.cost * buyNum;
                    if (!TalkingDataManager.isTDPC)
                    {
                        TDGAItem.OnPurchase("shop-" + data.name, buyNum, data.cost);
                    }
                }
                else if (data.costtype1 == 2)
                    PlayerInfo.getInstance().player.gold -= data.cost * buyNum;
                refreshGoldCrystal();

                //refresh icon
                int num = lastShopNum(detailId);
                if (num < BuyNumMax)
                {
                    if (ses1.ContainsKey(detailId)) ses1[detailId] += buyNum;
                    if (ses2.ContainsKey(detailId)) ses2[detailId] += buyNum;
                }
                if (lastShopNum(detailId) <= 0)
                    shopIcons[detailId - 1].GetComponent<ShopCell>().SellOut(true);

                //save goods

                //show success tips
                showSuccessBuy();
				
				//添加卡牌获得统计@zhangsai//
				if(data.goodstype == 3)
				{
					if(!UniteSkillInfo.cardUnlockTable.ContainsKey(data.itemId))
						UniteSkillInfo.cardUnlockTable.Add(data.itemId,true);
				}
            }
            else
            {
                showWrongMess(buyResJson.errorCode);
            }
        }
        else if (messType == MessType.BlackBuy)
        {
            objDetail.SetActive(false);
            if (buyResJson.errorCode == 0)
            {
                //refresh gold&crystal
                BlackMarketData data = BlackMarketData.getData(detailId);
                if (data.costtype == 1)
                {
                    PlayerInfo.getInstance().player.crystal -= data.cost;
                    if (!TalkingDataManager.isTDPC)
                    {
                        TDGAItem.OnPurchase("BlackShop-" + data.name, 1, data.cost);
                    }
                }
                else if (data.costtype == 2)
					if(data.goodstype == 8&&!TalkingDataManager.isTDPC)//金币买钻石，统计系统赠与//
					{
						TDGAVirtualCurrency.OnReward(data.number,"BlackShop");
					}
                    PlayerInfo.getInstance().player.gold -= data.cost;
                if (data.goodstype == 8)    //buy crystal//
                    PlayerInfo.getInstance().player.crystal += data.number;
                refreshGoldCrystal();

                //refresh icon
                int pos = 0;
                foreach (KeyValuePair<int, int> v in ses1)
                {
                    if (v.Key == detailId)
                    {
                        shopIcons[pos].GetComponent<ShopCell>().SellOut(true);
                        break;
                    }
                    pos++;
                }

                //save goods

                //show success tips
                showSuccessBuy();
				
				//添加卡牌获得统计@zhangsai//
				if(data.goodstype == 3)
				{
					if(!UniteSkillInfo.cardUnlockTable.ContainsKey(data.itemId))
						UniteSkillInfo.cardUnlockTable.Add(data.itemId,true);
				}
            }
            else
            {
                showWrongMess(buyResJson.errorCode);
            }
        }
        else if (messType == MessType.BlackBuyPos)
        {
            objDetail.SetActive(false);
            if (buyResJson.errorCode == 0)
            {
                //refresh gold&crystal
                BlackShopboxData data = BlackShopboxData.getData(++nowBlackNum);
                if (data.costtype == 1)
                    PlayerInfo.getInstance().player.crystal -= data.cost;
                else if (data.costtype == 2)
                    PlayerInfo.getInstance().player.gold -= data.cost;
                refreshGoldCrystal();

                //refresh icon
                Object res = Resources.Load(shopCellPrefabPath);
                GameObject obj = instantiateObj(res, cellParent, res.name + (nowBlackNum + 1000));
                ShopCell cell = obj.GetComponent<ShopCell>();
                cell.Init(BlackMarketData.getData(buyResJson.md));
                initCallback(obj, buyResJson.md);

                GameObject lastObj = shopIcons[shopIcons.Count - 1];
                shopIcons.Remove(lastObj);

                shopIcons.Add(obj);
                if (nowBlackNum == BlackShopboxData.getTotalNum())
                {
                    lastObj.transform.parent = null;
                    Destroy(lastObj);
                }
                else
                {
                    lastObj.name = res.name + (nowBlackNum + 1 + 1000);
                    lastObj.GetComponent<ShopCell>().Init(nowBlackNum + 1);
                    shopIcons.Add(lastObj);
                }

                cellParent.GetComponent<UIGrid>().repositionNow = true;
				
				//向talkingdata发数据,黑市开格子//
				if(!TalkingDataManager.isTDPC)
				{
					TDGAItem.OnPurchase("blackmarketbox" , 1, blackOpenBoxCost);
				}
            }
            else
            {
                showWrongMess(buyResJson.errorCode);
            }
        }
		
		else if(messType == MessType.PvpShopBuy)
		{
			objDetail.SetActive(false);
			if (buyResJson.errorCode == 0)
            {
                //refresh gold&crystal
                ShopPvpData data = ShopPvpData.getData(detailId);
                refreshGoldCrystal();

                //refresh icon
                int pos = 0;
                foreach (KeyValuePair<int, int> v in ses1)
                {
                    if (v.Key == detailId)
                    {
                        shopIcons[pos].GetComponent<ShopCell>().SellOut(true);
                        break;
                    }
                    pos++;
                }
				labelHonornum.text = shopResJson.pvpHonor.ToString();
				
				if (data.costtype == 3)
					honorNum -=data.cost;
				labelHonornum.text = honorNum.ToString();
				
                //show success tips
                showSuccessBuy();
				
				//添加卡牌获得统计@zhangsai//
				if(data.goodstype == 3)
				{
					if(!UniteSkillInfo.cardUnlockTable.ContainsKey(data.itemId))
						UniteSkillInfo.cardUnlockTable.Add(data.itemId,true);
				}
            }
            else
            {
                showWrongMess(buyResJson.errorCode);
            }
		}
    }

    void updateLeftPic()
    {
        if (messType == MessType.ShopUi)
        {
            leftShop.SetActive(true);
            leftBlack.SetActive(false);
			leftPvpShop.SetActive(false);
        }
        else if (messType == MessType.BlackUi)
        {
            leftShop.SetActive(false);
            leftBlack.SetActive(true);
			leftPvpShop.SetActive(false);
        }
		else if(messType == MessType.PvpShop)
		{
            leftShop.SetActive(false);
            leftBlack.SetActive(false);
			leftPvpShop.SetActive(true);
		}
    }

    void updateTimeLabel()
    {
        if (mode != 2) return;

        if (time > 0)
        {
            timeTemp += Time.deltaTime;
            if (timeTemp > 1)
            {
                timeTemp = 0;
                time -= 1;

                showLabelTime();
            }
        }
        else
        {
            if (!bAutoReqFlag)
            {
                bAutoReqFlag = !bAutoReqFlag;
                BtnPageNow(2);
            }
        }
    }

    void showLabelTime()
    {
        labelTime.text = getTimeString(time);
    }

    string getTimeString(int t)
    {
        int h = t / 3600;
        int m = (t % 3600) / 60;
        int s = t % 60;
        return (h > 9 ? h.ToString() : ("0" + h)) + ":" + (m > 9 ? m.ToString() : ("0" + m)) + ":" + (s > 9 ? s.ToString() : ("0" + s));
    }

    void showWrongMess(int errorCode)
    {
        //errorCode 19水晶不足 21购买物品不存在 70vip等级不足 79购买次数达到上限 81时间已到无需购买 89金币不足//
        string str = "";
        if (errorCode == 19 || errorCode == 70)
        {
            switch (errorCode)
            {
                case 19: str = TextsData.getData(544).chinese; break;
                case 70: str = TextsData.getData(243).chinese; break;
            }
            UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
        }
        else
        {
            switch (errorCode)
            {
                case 21: str = TextsData.getData(577).chinese; break;
                case 79: str = TextsData.getData(240).chinese; break;
                case 81: str = TextsData.getData(578).chinese; break;
                case 89: str = TextsData.getData(46).chinese; break;
				case 125: str = TextsData.getData(694).chinese;break;
            }
            ToastWindow.mInstance.showText(str);
        }
    }

    void showSuccessBuy()
    {
        objBuySucc.GetComponent<ShopBuySuccess>().show(mode, detailId, buyNum);
    }

    public static void updateIconPic(int goodsType, int itemId, UISprite texFrame, UISprite texCard, UISprite texOther, GameObject objPiece, GameObject goodsType1to5, UISprite goodsType6to8,
        UILabel labelName = null, UILabel labelDesc = null)
    {
        UIAtlas atlas = null;
        int starNum = 0;
        string iconName = "";
        string name = "";
        string desc = "";
        bool bPiece = false;
        bool bCard = false;
        switch (goodsType)
        {
            case 1: //item
                {
                    ItemsData temp = ItemsData.getData(itemId);
                    starNum = temp.star;
                    if (temp.fragment == 0)
                    {
                        iconName = temp.icon;
                        atlas = LoadAtlasOrFont.LoadAtlasByName("ItemCircularIcon");
                    }
                    else
                    {
                        bPiece = true;
                        switch (temp.goodztype)
                        {
                            case 1: iconName = SkillData.getData(temp.goodsid).icon; atlas = LoadAtlasOrFont.LoadAtlasByName("SkillCircularIcom"); break;
                            case 2: iconName = ItemsData.getData(temp.goodsid).icon; atlas = LoadAtlasOrFont.LoadAtlasByName("ItemCircularIcon"); break;
                            case 3: iconName = EquipData.getData(temp.goodsid).icon; atlas = LoadAtlasOrFont.LoadAtlasByName("EquipCircularIcon"); break;
                            case 4:
                                CardData data = CardData.getData(temp.goodsid);
                                iconName = data.icon;
                                atlas = LoadAtlasOrFont.LoadAtlasByName(data.atlas);
                                bCard = true;
                                break;
                            case 5: iconName = PassiveSkillData.getData(temp.goodsid).icon; atlas = LoadAtlasOrFont.LoadAtlasByName("PassSkillCircularIcon"); break;
                        }
                    }
                    name = temp.name;
                    desc = temp.discription;
                }
                break;
            case 2: //equip
                {
                    atlas = LoadAtlasOrFont.LoadAtlasByName("EquipCircularIcon");
                    EquipData temp = EquipData.getData(itemId);
                    starNum = temp.star;
                    iconName = temp.icon;
                    name = temp.name;
                    desc = temp.description + EquippropertyData.getData(temp.type, 1).starNumbers[starNum - 1];
                }
                break;
            case 3: //card
                {
                    CardData temp = CardData.getData(itemId);
                    starNum = temp.star;
                    iconName = temp.icon;
                    atlas = LoadAtlasOrFont.LoadAtlasByName(temp.atlas);
                    name = temp.name;
                    desc = temp.description;
                    bCard = true;
                }
                break;
            case 4: //skill
                {
                    atlas = LoadAtlasOrFont.LoadAtlasByName("SkillCircularIcom");
                    SkillData temp = SkillData.getData(itemId);
                    starNum = temp.star;
                    iconName = temp.icon;
                    name = temp.name;
                    desc = temp.description + (temp.exptype == 1 ? SkillPropertyData.getProperty(temp.type, 1, starNum).ToString() : "");
                }
                break;
            case 5: //passive skill
                {
                    atlas = LoadAtlasOrFont.LoadAtlasByName("PassSkillCircularIcon");
                    PassiveSkillData temp = PassiveSkillData.getData(itemId);
                    starNum = temp.star;
                    iconName = temp.icon;
                    name = temp.name;
                    desc = temp.describe;
                }
                break;
            case 6:
                atlas = LoadAtlasOrFont.LoadAtlasByName("AchievementAtlas");
                iconName = "jb";
                break;
            case 7:
                atlas = LoadAtlasOrFont.LoadAtlasByName("AchievementAtlas");
                iconName = "exp";
                break;
            case 8:
                atlas = LoadAtlasOrFont.LoadAtlasByName("AchievementAtlas");
                iconName = "zs";
                break;
			case 9:
				atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas01");
				iconName = "jingangxin";
				break;
        }
        if (goodsType <= 5)
        {
            goodsType1to5.SetActive(true);
            goodsType6to8.transform.gameObject.SetActive(false);

            texFrame.spriteName = "head_star_" + starNum;
            if (bCard)
            {
                texCard.transform.gameObject.SetActive(true);
                texOther.transform.gameObject.SetActive(false);
                texCard.atlas = atlas;
                texCard.spriteName = iconName;
            }
            else
            {
                texCard.transform.gameObject.SetActive(false);
                texOther.transform.gameObject.SetActive(true);
                texOther.atlas = atlas;
                texOther.spriteName = iconName;
            }

            if (bPiece)
                objPiece.SetActive(true);
            else
                objPiece.SetActive(false);
        }
        else
        {
            goodsType1to5.SetActive(false);
            goodsType6to8.transform.gameObject.SetActive(true);
            goodsType6to8.atlas = atlas;
            goodsType6to8.spriteName = iconName;
        }
        if (labelName != null)
            labelName.text = name;
        if (labelDesc != null)
            labelDesc.text = desc;
    }
}
