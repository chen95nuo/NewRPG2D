using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityPanel : MonoBehaviour, ProcessResponse
{
    public GameObject tipCtrl;
    public GameObject jhmRewardPanel;
    public GameObject helpUnitPanel;

    public GameObject topPanel;
    public GameObject topUIGrid;
    public GameObject pageContentPos;
    GameObject pageContent;
    List<ActTopIcon> topUIIcon = new List<ActTopIcon>();
    Vector4 topPanelClip;  //draggable panel clip

    public List<ActivityElement> aes;
    public List<TaskElement> tes;

    //Transform _myTransform;
    //GameObject leftItemPrefab = null;
    //GameObject rightItemPrefab = null;
    GameObject jhmItemPrefab = null;
    GameObject noticePrefab = null;
    GameObject noticeBtnIconPrefab = null;
    GameObject equipReItemPrefab = null;
    GameObject rechargeItemPrefab = null;
    GameObject rechargeCellPrefab = null;
    GameObject cardRewardPrefab = null;
    UIInput jhmInput;

    bool needRefreshPlayer;
    GiftCodeResultJson gcrj;
    RechargeUiResultJson rechargeRJ;

    ActivityInfoResultJson gmaij;
    ExchangeResultJson exrj;

    LoginSevenDayResJson sevenResJson;


    CornucopiaPanel cornucopiaPanel;
    GameObject cornucopiaResPrefab = null;
    CornucopiaResultJson cornucopiaResultJson;


    /*12获取激活码信息,	13获取充值界面, 14获取公告展示界面, 15获取公告/物品兑换界面
     * 16请求活动兑换,18 获取玩家vip等级信息,19 请求购买vip礼包,21获取公告列表，22获取当前公告内容
     * 23,限时神将
     * */
    int requestType = 0;
    bool receiveData = false;
    int errorCode = -1;

    int butNowSel = -1;     //当前选择的活动按钮位置0-n//
    int butNowEleId = -1;   //当前选择的活动对应的活动id//

    const string ActTopIconPath = "Prefabs/UI/ActivityPanel/actTopIcon";
    const string ActSevenPanel = "Prefabs/UI/ActivityPanel/ActSevenPanel";
    const string ActLevelPanel = "Prefabs/UI/ActivityPanel/ActLevelPanel";
    const string ActWheelPanelPath = "Prefabs/UI/ActivityPanel/ActWheelPanel";
    const string LotCardPanelPath = "Prefabs/UI/ActivityPanel/LotActivityPanel";

    GameObject actTopIconPrefab = null;
    GameObject sevenPrizeResPrefab = null;
    GameObject levelPrizeResPrefab = null;
    GameObject wheelResPrefab = null;
    GameObject LotCardPanelPrefab = null;

    public UIAtlas friendshipAtlas;
    string friendshipSpriteName = "icon_02";

    GameObject giftGroupPrefab = null;
    const string giftGroupPrefabPath = "Prefabs/UI/ActivityPanel/GiftGroup";

    RechargeUiResultJson curVIPRJ;
    GameObject vipLeftBtnPrefab = null;
    const string vipLeftBtnPath = "Prefabs/UI/ActivityPanel/VIPLeftBtn";
    GameObject giftItemPrefab = null;
    const string giftItemPath = "Prefabs/UI/ActivityPanel/GiftItem";
    List<UISprite> vipBtnList;

    const string CornucopiaPath = "Prefabs/UI/ActivityPanel/CornucopiaPanel";

    int curVIPValue;
    int tempVIPValue;
    Vector4 dragPanelClip;  //draggable panel clip
    Vector4 rDragPanelClip;

    GameObject giftBottomTopObj;
    GameObject giftBottomLeftObj;
    GameObject giftBottomRightObj;
    GameObject giftBottomSpecialObj;
    GameObject giftLeftClipPanel;
    GameObject giftRightClipPanel;
    UIScrollBar giftLeftScrollbar;

    GameObject gPreBtn, gNextBtn;
    UILabel tVIPValueLabel;
    UILabel specialDescLabel;
    UILabel preBtnLabel, nextBtnLabel;

    ActivityGResultJson agrj = new ActivityGResultJson();

    GameObject noticeItem;
    Vector4 noticeClipPanelVec;
    int curNoticeId = 1;
    //int curNoticeType = -1;
    bool isGetRechargeData;
    ActivityInfoResultJson naij;
    LotEventRankResultJson lotjson;
    List<UISprite> noticeBtnList;

    GameObject rechargeClipPanel;

    //	float lastRechargeSBPos;
    //兑换界面滚动条上一次的位置//
    Vector3 lastRechargeClipPos;

    BuyPowerOrGoldResultJson bpgrj;

    public enum ActRewardType
    {
        E_Null = 0,
        E_Item = 1,
        E_Equip = 2,
        E_Card = 3,
        E_Skill = 4,
        E_PassiveSkill = 5,
        E_Gold = 6,
        E_Exp = 7,
        E_Crystal = 8,
        E_Rune = 9,
        E_Power = 10,
    }

    public enum VipGiftType
    {
        E_Null = 0,
        E_Item = 1,
        E_Equip = 2,
        E_Card = 3,
        E_Skill = 4,
        E_PassiveSkill = 5,
        E_Gold = 6,
        E_Exp = 7,
        E_Crystal = 8,
        E_Friend = 9,
    }

    public UIAtlas otherAtlas;
    //string iconFrameName = "head_star_";
    string expSpriteName = "reward_exp";
    string crystalSpriteName = "reward_crystal";
    string goldSpriteName = "reward_gold";
    // string runeSpriteName = "rune";
    //string powerSpriteName = "power";

    int curActType = 1;

    // ActivityInfoExchangeElement tempActInfoEElement;
    int curRechargeId;

    RunnerUIResultJson rUIResultJson;

    void Awake()
    {
        //_myTransform = transform;
        init();

        //		hide();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (receiveData)
        {
            receiveData = false;
            if (errorCode == -3)
                return;


            if (errorCode != 0)
            {
                //金币或者水晶不足//
                if (errorCode == 19)
                {
                    string errorMsg = TextsData.getData(544).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //vip等级不足//
                else if (errorCode == 70)
                {
                    string errorMsg = TextsData.getData(243).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //背包空间不足，请整理后继续//
                else if (errorCode == 53)
                {
                    string errorMsg = TextsData.getData(78).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //尚未解锁，请提升军团等级//
                else if (errorCode == 56)
                {
                    string errorMsg = TextsData.getData(384).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //任务尚未完成，无法领取奖励！//
                else if (errorCode == 90)
                {
                    string errorMsg = TextsData.getData(376).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //您已领取该奖励//
                else if (errorCode == 91)
                {
                    string errorMsg = TextsData.getData(217).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //激活码不对//
                else if (errorCode == 92)
                {
                    string errorMsg = TextsData.getData(368).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //激活码已经使用过//
                else if (errorCode == 93)
                {
                    string errorMsg = TextsData.getData(369).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //您已经领过这个礼包了//
                else if (errorCode == 94)
                {
                    string errorMsg = TextsData.getData(370).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //服务器出错//
                else if (errorCode == 96)
                {
                    string errorMsg = TextsData.getData(385).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //此兑换活动已过期，请重新查看活动内容！//
                else if (errorCode == 100)
                {
                    string errorMsg = TextsData.getData(506).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //已经兑换过了！//
                else if (errorCode == 101)
                {
                    string errorMsg = TextsData.getData(507).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //此兑换活动已过期，请重新查看活动内容！//
                else if (errorCode == 102)
                {
                    string errorMsg = TextsData.getData(506).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //物品不足//
                else if (errorCode == 103)
                {
                    string errorMsg = TextsData.getData(505).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                //该vip等级礼包已经购买过//
                else if (errorCode == 104)
                {
                    string errorMsg = TextsData.getData(542).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
                return;
            }
            switch (requestType)
            {
                case 1:
                    {
                        if (sevenResJson != null)
                        {
                            initSevenPrizePanel();
                        }
                    }
                    break;
                case 2:
                    {
                        if (sevenResJson != null)
                        {
                            initLevelPrizePanel();
                        }
                    }
                    break;
                case 3:
                    {
                        if (rUIResultJson != null)
                        {
                            initWheelPanel();
                        }
                    }
                    break;
                case 12:				//激活码兑换奖励//
                    {
                        if (gcrj != null)
                        {
                            jhmRewardPanel.SetActive(true);
                            InitJHMRewardPanel(gcrj);
                            HeadUI.mInstance.requestPlayerInfo();
                            if (!TalkingDataManager.isTDPC)
                            {
                                //                                if (gcrj.gold != 0)//金币//
                                //                                    TDGAVirtualCurrency.OnReward(gcrj.gold, "CDKEY" + TextsData.getData(59).chinese);
                                if (gcrj.crystal != 0)//钻石//
                                    TDGAVirtualCurrency.OnReward(gcrj.crystal, "keycode");
                                //                                if (gcrj.runeNum != 0)//符文//
                                //                                    TDGAVirtualCurrency.OnReward(gcrj.runeNum, "CDKEY" + TextsData.getData(221).chinese);
                                //                                if (gcrj.power != 0)//体力//
                                //                                    TDGAVirtualCurrency.OnReward(gcrj.power, "CDKEY" + TextsData.getData(158).chinese);
                            }

                            //添加卡牌获得统计@zhangsai//
                            if (!gcrj.card.Equals(""))
                            {
                                string[] ss = gcrj.card.Split(',');
                                for (int i = 0; i < ss.Length; i++)
                                {
                                    string sss = ss[i];
                                    int id = StringUtil.getInt(sss.Split('-')[0]);
                                    if (!UniteSkillInfo.cardUnlockTable.ContainsKey(id))
                                        UniteSkillInfo.cardUnlockTable.Add(id, true);
                                }
                            }
                        }
                    }
                    break;
                case 13:
                    {
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
                        ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ,
                                "ChargePanel") as ChargePanel;
                        charge.curRechargeResult = rechargeRJ;
                        charge.isShowType = 1;
                        charge.show();
                        CloseActivityPanelOnly();
                    }
                    break;
                case 14:
                    {
                        if (needRefreshPlayer)
                        {
                            needRefreshPlayer = false;
                            HeadUI.mInstance.requestPlayerInfo();
                        }
                        if (curActType == 4)
                        {
                            //  ActivityInfoExchangeElement aiee = GetActInfoExElement(curRechargeId);
                            /*
                                    if (!TalkingDataManager.isTDPC)
                                    {
                                        //如果是需要钻石来兑换，记录td类型5//
                                        if (aiee.needType == (int)ActRewardType.E_Crystal)
                                        {
                                            //统计活动钻石消耗//
                                            TDGAItem.OnPurchase("Activities", 1, aiee.needNum);
                                        }
                                        else//如果不是记录td类型4//目标资源类型id:1材料，2装备，3英雄卡，4主动技能，5被动技能，6金币，7钻石，8符文，9体力//
                                        {
                                            switch (aiee.exchangeType)
                                            {
        //                                        case 6:
        //                                            TDGAVirtualCurrency.OnReward(aiee.exchangeNum, "Activities-" + TextsData.getData(58).chinese);
        //                                            break;
                                                case 7:
                                                    TDGAVirtualCurrency.OnReward(aiee.exchangeNum, "Activities-" + TextsData.getData(48).chinese);
                                                    break;
        //                                        case 8:
        //                                            TDGAVirtualCurrency.OnReward(aiee.exchangeNum, "Activities-" + TextsData.getData(221).chinese);
        //                                            break;
                                            }
                                        }
                                    }
                                    */
                        }

                    }
                    break;
                case 15:
                    {
                        if (!isGetRechargeData)
                        {
                            InitNoticePanel(curNoticeId, 7);
                        }
                        else
                        {
                            InitRechargePanel();
                        }
                    }
                    break;
                case 16:
                    {
                        tipCtrl.SetActive(true);
                        tipCtrl.transform.FindChild("OkBtn").GetComponent<UIButtonMessage>().param = 1;
                        tipCtrl.transform.FindChild("ActName").GetComponent<UILabel>().text = TextsData.getData(85).chinese;
                        GameObject rObj0 = tipCtrl.transform.FindChild("Rward0").gameObject;
                        tipCtrl.transform.FindChild("Rward1").gameObject.SetActive(false);
                        InitRechargeIcon(GetActInfoExElement(curRechargeId), null, rObj0, 1);
                        //HeadUI.mInstance.requestPlayerInfo();
                        needRefreshPlayer = true;
                        //				requestType = 14;
                        //				PlayerInfo.getInstance().sendRequest(new ActivityInfoJson(curActId,curActType),this);
                        requestType = 17;
                        PlayerInfo.getInstance().sendRequest(new ActivityJson(), this);
                        //向talkingdata发数据,//
                        sendToTD();
                        //记录获得卡牌//
                        SendUnitSkillNeedCard();
                    }
                    break;
                case 17:
                    {
                        if (needRefreshPlayer)
                        {
                            needRefreshPlayer = false;
                            HeadUI.mInstance.requestPlayerInfo();
                        }
                        InitActivityTopPanel();
                        setButSel(butNowSel);
                        //						isGetRechargeData = true;
                        //                        requestType = 15;
                        //						PlayerInfo.getInstance().sendRequest(new ActivityInfoJson(butNowEleId,0, 4), this);
                    }
                    break;
                case 18:
                    {
                        InitGiftGroup();
                    }
                    break;
                case 19:
                    {
                        PlayerInfo.getInstance().player.crystal = bpgrj.crystal;
                        HeadUI.mInstance.refreshPlayerInfo();
                        curVIPRJ.giftIds = bpgrj.ids;
                        InitGiftRightRegion();
                        string msg = TextsData.getData(545).chinese;
                        ToastWindow.mInstance.showText(msg);
                        //添加卡牌获得统计@zhangsai//
                        List<string> str = VipGiftData.getItemList(tempVIPValue);
                        for (int i = 0; i < str.Count; i++)
                        {
                            string[] ss = str[i].Split('-');
                            int type = StringUtil.getInt(ss[0]);
                            int id = StringUtil.getInt(ss[1].Split(',')[0]);
                            if (type == 3)      //卡牌//
                            {
                                if (!UniteSkillInfo.cardUnlockTable.ContainsKey(id))
                                    UniteSkillInfo.cardUnlockTable.Add(id, true);
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        InitCornucopia();
                    }
                    break;
                case 21:
                    {
                        if (agrj.age.Count == 0)
                        {
                            return;
                        }
                        curNoticeId = agrj.age[0].aid;
                        isGetRechargeData = false;
                        requestType = 15;
                        PlayerInfo.getInstance().sendRequest(new ActivityInfoJson(butNowEleId, curNoticeId, 7), this);
                    }
                    break;
                case 22:
                    {
                        InitNoticeContentPanel();
                        SetBtnState(noticeBtnList, curNoticeId);
                    }
                    break;
                case 23:
                    {
                        initLotCardPanel();
                    }
                    break;
            }
        }
    }

    //cxl--向talkingdata发数据//
    public void sendToTD()
    {
        if (!TalkingDataManager.isTDPC && errorCode == 0)
        {
            int useCost = 0;
            ActivityInfoExchangeElement aieElement = null;
            for (int i = 0; i < gmaij.exActs.Count; i++)
            {
                ActivityInfoExchangeElement temp = gmaij.exActs[i];
                if (temp.id == curRechargeId)
                {
                    aieElement = temp;
                }

            }
            if (aieElement != null)
            {
                for (int j = 0; j < aieElement.ade.Count; j++)
                {
                    ActivityDElement element = aieElement.ade[j];
                    if (element.needType == (int)ActRewardType.E_Crystal)
                    {
                        useCost = element.needNum;
                    }
                }
            }

            //向talkingdata发数据，兑换消耗钻石数//
            string name = "activity-" + curRechargeId;
            TDGAItem.OnPurchase(name, 1, useCost);

            //兑换获得钻石数//
            int curRechargeGetNum = 0;
            if (aieElement.exchangeType == (int)ActRewardType.E_Crystal)
            {
                curRechargeGetNum = aieElement.exchangeNum;
            }
            TDGAVirtualCurrency.OnReward(curRechargeGetNum, "activityaward");
        }
    }

    public void SendUnitSkillNeedCard()
    {
        //添加卡牌获得统计@zhangsai//
        if (errorCode == 0)
        {
            //int useCost = 0;
            ActivityInfoExchangeElement aieElement = null;
            for (int i = 0; i < gmaij.exActs.Count; i++)
            {
                ActivityInfoExchangeElement temp = gmaij.exActs[i];
                if (temp.id == curRechargeId)
                {
                    aieElement = temp;
                }

            }
            if (aieElement != null)
            {
                if (aieElement.exchangeType == 3)
                {
                    if (!UniteSkillInfo.cardUnlockTable.ContainsKey(aieElement.exchangeId))
                        UniteSkillInfo.cardUnlockTable.Add(aieElement.exchangeId, true);
                }
            }
        }
    }

    void InitJHMRewardPanel(GiftCodeResultJson gcResult)
    {
        jhmRewardPanel.transform.FindChild("scroll-bar").GetComponent<UIScrollBar>().value = 0;

        GameObject panel = jhmRewardPanel.transform.FindChild("panel").gameObject;
        panel.transform.localPosition = new Vector3(0, 5f, 0);
        //panel.GetComponent<UIPanel>().clipRange = new Vector4(0, 0, 300f, 150f);

        float y = 0;
        float yOffset = 65f;

        if (setValue(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/gold").gameObject, gcResult.gold, y))
        {
            y -= yOffset;
        }
        if (setValue(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/crystal").gameObject, gcResult.crystal, y))
        {
            y -= yOffset;
        }
        if (setValue(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/rune").gameObject, gcResult.runeNum, y))
        {
            y -= yOffset;
        }
        if (setValue(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/power").gameObject, gcResult.power, y))
        {
            y -= yOffset;
        }
        y = setReward(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/card").gameObject, 1, gcResult.card, y, 70f);
        y = setReward(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/equip").gameObject, 2, gcResult.equip, y, 70f);
        y = setReward(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/mskill").gameObject, 3, gcResult.skill, y, 70f);
        y = setReward(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/oskill").gameObject, 4, gcResult.pSkill, y, 70f);
        y = setReward(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/item").gameObject, 5, gcResult.item, y, 70f);
    }

    private bool setValue(GameObject valueGo, int number, float y)
    {
        valueGo.SetActive(false);
        if (number > 0)
        {
            valueGo.SetActive(true);
            valueGo.transform.FindChild("Label").GetComponent<UILabel>().text = "x" + number;
            Vector3 pos = valueGo.transform.localPosition;
            valueGo.transform.localPosition = new Vector3(pos.x, y, pos.z);
            return true;
        }
        return false;
    }

    //type:1-card,2-equip,3-mskill,4-oskill,5-item(prop)
    private float setReward(GameObject rewardGoParent, int type, string reward, float y, float yOffset)
    {
        if (type == 4)
        {
            Debug.Log(123);
        }

        rewardGoParent.SetActive(false);
        if (!string.IsNullOrEmpty(reward))
        {
            rewardGoParent.SetActive(true);
            Vector3 basePos = rewardGoParent.transform.localPosition;
            string[] ss = reward.Split(',');
            Debug.Log("reward:" + reward);
            for (int k = 0; k < ss.Length; k++)
            {
                string[] rs = ss[k].Split('-');
                int rewardId = StringUtil.getInt(rs[0]);
                int number = StringUtil.getInt(rs[1]);

                if (cardRewardPrefab == null)
                {
                    cardRewardPrefab = GameObjectUtil.LoadResourcesPrefabs("ActivityPanel/Card-reward", 3) as GameObject;
                }
                GameObject rewardGo = Instantiate(cardRewardPrefab) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(rewardGo, rewardGoParent);
                rewardGo.transform.FindChild("Label").GetComponent<UILabel>().text = "x" + number;
                rewardGo.transform.FindChild("Label").localPosition = new Vector3(80f, -10f, 0);
                rewardGo.transform.FindChild("Label").localScale = new Vector3(1.7f, 1.7f, 1f);
                SimpleCardInfo2 sc = rewardGo.GetComponent<SimpleCardInfo2>();
                sc.clear();
                switch (type)
                {
                    case 1:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Hero);
                        break;
                    case 2:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Equip);
                        break;
                    case 3:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Skill);
                        break;
                    case 4:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_PassiveSkill);
                        break;
                    case 5:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Item);
                        break;
                }
                rewardGo.transform.localPosition = new Vector3(3f, y - k * yOffset, basePos.z);
                rewardGo.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            }
            return y - ss.Length * yOffset;
        }
        return y;
    }

    /*
	 * type----0,表示兑换的源物品
	 * 		 ----1,表示兑换的目标物品
	 */
    void InitRechargeIcon(ActivityInfoExchangeElement aiee, ActivityDElement ade, GameObject rItem, int type)
    {
        int rId;
        int rType;
        int rNum;

        if (type == 0)
        {
            rId = ade.needId;
            rType = ade.needType;
            rNum = ade.needNum;
        }
        else
        {
            rId = aiee.exchangeId;
            rType = aiee.exchangeType;
            rNum = aiee.exchangeNum;
        }
        if (rItem != null)
        {
            GameObject rewardObj = rItem;
            UISprite rewardIconBG = rewardObj.transform.FindChild("IconBG").GetComponent<UISprite>();
            rewardIconBG.gameObject.SetActive(false);
            UILabel rewardLabel = rewardObj.transform.FindChild("Text").GetComponent<UILabel>();
            UILabel rewardName = null;
            if (rewardObj.transform.FindChild("Name").GetComponent<UILabel>())
            {
                rewardName = rewardObj.transform.FindChild("Name").GetComponent<UILabel>();
            }

            rewardLabel.text = string.Empty;
            SimpleCardInfo2 cardInfo = rewardObj.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
            cardInfo.clear();
            cardInfo.gameObject.SetActive(false);
            if (rewardObj.GetComponent<RechargeItemInfo>())
            {
                rewardObj.GetComponent<RechargeItemInfo>().itemType = rType;
                rewardObj.GetComponent<RechargeItemInfo>().itemId = rId;
            }
            switch (rType)
            {
                case (int)ActRewardType.E_Item:
                    {
                        int itemID = rId;
                        int num = rNum;
                        ItemsData itemData = ItemsData.getData(itemID);
                        if (itemData == null)
                        {
                            rewardObj.SetActive(false);
                        }
                        else
                        {
                            cardInfo.gameObject.SetActive(true);
                            cardInfo.setSimpleCardInfo(itemID, GameHelper.E_CardType.E_Item);

                            rewardLabel.text = "x " + num.ToString();
                            if (rewardName != null)
                            {
                                rewardName.text = itemData.name;
                            }
                        }
                    }
                    break;
                case (int)ActRewardType.E_Equip:
                    {
                        int equipID = rId;
                        int num = rNum;

                        EquipData ed = EquipData.getData(equipID);
                        if (ed == null)
                        {
                            rewardObj.SetActive(false);
                        }
                        else
                        {
                            cardInfo.gameObject.SetActive(true);
                            cardInfo.setSimpleCardInfo(equipID, GameHelper.E_CardType.E_Equip);

                            rewardLabel.text = "x " + num.ToString();
                            if (rewardName != null)
                            {
                                rewardName.text = ed.name;
                            }
                        }
                    }
                    break;
                case (int)ActRewardType.E_Card:
                    {
                        int heroID = rId;
                        int num = rNum;

                        CardData cd = CardData.getData(heroID);
                        if (cd == null)
                        {
                            rewardObj.SetActive(false);
                        }
                        else
                        {
                            cardInfo.gameObject.SetActive(true);
                            cardInfo.setSimpleCardInfo(heroID, GameHelper.E_CardType.E_Hero);

                            rewardLabel.text = "x " + num.ToString();
                            if (rewardName != null)
                            {
                                rewardName.text = cd.name;
                            }
                        }
                    }
                    break;
                case (int)ActRewardType.E_Skill:
                    {
                        int skillID = rId;
                        int num = rNum;

                        SkillData sd = SkillData.getData(skillID);
                        if (sd == null)
                        {
                            rewardObj.SetActive(false);
                        }
                        else
                        {
                            cardInfo.gameObject.SetActive(true);
                            cardInfo.setSimpleCardInfo(skillID, GameHelper.E_CardType.E_Skill);

                            rewardLabel.text = "x " + num.ToString();
                            if (rewardName != null)
                            {
                                rewardName.text = sd.name;
                            }
                        }
                    }
                    break;
                case (int)ActRewardType.E_PassiveSkill:
                    {
                        int passiveSkillID = rId;
                        int num = rNum;

                        PassiveSkillData psd = PassiveSkillData.getData(passiveSkillID);
                        if (psd == null)
                        {
                            rewardObj.SetActive(false);
                        }
                        else
                        {
                            cardInfo.gameObject.SetActive(true);
                            cardInfo.setSimpleCardInfo(passiveSkillID, GameHelper.E_CardType.E_PassiveSkill);

                            rewardLabel.text = "x " + num.ToString();
                            if (rewardName != null)
                            {
                                rewardName.text = psd.name;
                            }
                        }
                    }
                    break;
                case (int)ActRewardType.E_Gold:
                    {
                        rewardIconBG.gameObject.SetActive(true);
                        rewardIconBG.atlas = otherAtlas;
                        rewardIconBG.spriteName = goldSpriteName;
                        rewardLabel.text = "x " + rNum.ToString();
                        if (rewardName != null)
                        {
                            rewardName.gameObject.SetActive(false);
                            if (type == 1)
                            {
                                float posX = rewardName.transform.localPosition.x;
                                float posY = rewardLabel.transform.localPosition.y;
                                float posZ = rewardLabel.transform.localPosition.z;
                                rewardLabel.transform.localPosition = new Vector3(posX, posY, posZ);
                            }
                        }

                    }
                    break;
                case (int)ActRewardType.E_Exp:
                    {
                        rewardIconBG.gameObject.SetActive(true);
                        rewardIconBG.atlas = otherAtlas;
                        rewardIconBG.spriteName = expSpriteName;
                        rewardLabel.text = "x " + rNum.ToString();
                        if (rewardName != null)
                        {
                            rewardName.gameObject.SetActive(false);
                            if (type == 1)
                            {
                                float posX = rewardName.transform.localPosition.x;
                                float posY = rewardLabel.transform.localPosition.y;
                                float posZ = rewardLabel.transform.localPosition.z;
                                rewardLabel.transform.localPosition = new Vector3(posX, posY, posZ);
                            }
                        }
                    }
                    break;
                case (int)ActRewardType.E_Crystal:
                    {
                        rewardIconBG.gameObject.SetActive(true);
                        rewardIconBG.atlas = otherAtlas;
                        rewardIconBG.spriteName = crystalSpriteName;
                        rewardLabel.text = "x " + rNum.ToString();
                        if (rewardName != null)
                        {
                            rewardName.gameObject.SetActive(false);
                            if (type == 1)
                            {
                                float posX = rewardName.transform.localPosition.x;
                                float posY = rewardLabel.transform.localPosition.y;
                                float posZ = rewardLabel.transform.localPosition.z;
                                rewardLabel.transform.localPosition = new Vector3(posX, posY, posZ);
                            }
                        }
                    }
                    break;
                case (int)ActRewardType.E_Rune:
                    {
                        cardInfo.gameObject.SetActive(true);
                        cardInfo.setSpecialIconInfo(GameHelper.E_CardType.E_Rune);

                        rewardLabel.text = "x " + rNum.ToString();
                        if (rewardName != null)
                        {
                            rewardName.gameObject.SetActive(false);
                            if (type == 1)
                            {
                                float posX = rewardName.transform.localPosition.x;
                                float posY = rewardLabel.transform.localPosition.y;
                                float posZ = rewardLabel.transform.localPosition.z;
                                rewardLabel.transform.localPosition = new Vector3(posX, posY, posZ);
                            }
                        }
                    }
                    break;
                case (int)ActRewardType.E_Power:
                    {
                        cardInfo.gameObject.SetActive(true);
                        cardInfo.setSpecialIconInfo(GameHelper.E_CardType.E_Power);

                        rewardLabel.text = "x " + rNum.ToString();
                        if (rewardName != null)
                        {
                            rewardName.gameObject.SetActive(false);
                            if (type == 1)
                            {
                                float posX = rewardName.transform.localPosition.x;
                                float posY = rewardLabel.transform.localPosition.y;
                                float posZ = rewardLabel.transform.localPosition.z;
                                rewardLabel.transform.localPosition = new Vector3(posX, posY, posZ);
                            }
                        }
                    }
                    break;
            }
        }
    }

    void OnRechargeBtn(int param)
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (rechargeClipPanel)
        {
            rDragPanelClip = rechargeClipPanel.GetComponent<UIPanel>().clipRange;
            lastRechargeClipPos = rechargeClipPanel.transform.localPosition;
            //			lastRechargeSBPos = rechargeClipPanel.GetComponent<UIDraggablePanel>().verticalScrollBar.value;
        }
        requestType = 16;
        curRechargeId = param;
        PlayerInfo.getInstance().sendRequest(new ExchangeJson(param), this);
    }

    void UpdateActInfoResult()
    {
        if (exrj != null)
        {
            int id = exrj.id;
            Debug.Log("exrj id:" + id);
            //int num = exrj.curNeedNum;
            int sell = exrj.sell;
            //ActivityInfoExchangeElement tElement = GetActInfoExElement(id);
            for (int i = 0; i < gmaij.exActs.Count; i++)
            {
                Debug.Log("gmaij.exActs id:" + gmaij.exActs[i].id);
                if (gmaij.exActs[i].id == id)
                {
                    //                    gmaij.exActs[i].curNeedNum = num;
                    gmaij.exActs[i].sell = sell;
                    int tSole = gmaij.exActs[i].pSole;
                    Debug.Log("tSole:" + tSole);
                    //如果玩家的兑换次数达到上限//
                    if ((tSole++) >= gmaij.exActs[i].sole)
                    {
                        gmaij.exActs[i].pSole = gmaij.exActs[i].sole;
                        Debug.Log("gmaij.exActs[i].pSole 1:" + gmaij.exActs[i].pSole);
                    }
                    else
                    {
                        gmaij.exActs[i].pSole = tSole++;
                        Debug.Log("gmaij.exActs[i].pSole 2:" + gmaij.exActs[i].pSole);
                    }
                }
            }
        }
    }

    ActivityInfoExchangeElement GetActInfoExElement(int aieeId)
    {
        ActivityInfoExchangeElement aieElement = null;
        for (int i = 0; i < gmaij.exActs.Count; i++)
        {
            ActivityInfoExchangeElement tElement = gmaij.exActs[i];
            if (tElement.id == aieeId)
            {
                aieElement = tElement;
            }
        }
        return aieElement;
    }

    ActivityElement GetActElementInfo(int aeId)
    {
        ActivityElement tarActElement = null;
        for (int i = 0; i < aes.Count; i++)
        {
            ActivityElement aeItem = aes[i];
            if (aeItem.id == aeId)
            {
                tarActElement = aeItem;
            }
        }
        return tarActElement;
    }

    public void TalkMainToGetData()
    {
        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
        GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU); ;
        if (obj != null)
        {
            MainMenuManager main = obj.GetComponent<MainMenuManager>();
            if (main != null && obj.activeSelf)
            {
                main.SendToGetData();


            }
        }
    }

    void OnJHMConfirmBtn(int param)
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (string.IsNullOrEmpty(jhmInput.value))
        {
            return;
        }
        requestType = 12;
        PlayerInfo.getInstance().sendRequest(new GiftCodeJson(jhmInput.value), this);
    }

    void CloseJHMRewardPanel()
    {
        GameObjectUtil.destroyGameObjectAllChildrens(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/card").gameObject);
        GameObjectUtil.destroyGameObjectAllChildrens(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/item").gameObject);
        GameObjectUtil.destroyGameObjectAllChildrens(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/equip").gameObject);
        GameObjectUtil.destroyGameObjectAllChildrens(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/mskill").gameObject);
        GameObjectUtil.destroyGameObjectAllChildrens(jhmRewardPanel.transform.FindChild("panel/pop-parent/pop-info/oskill").gameObject);
        jhmRewardPanel.SetActive(false);
    }

    //param:0每日任务的领奖励，1 兑换活动结果//
    void CloseTipWnd(int param)
    {
        tipCtrl.SetActive(false);
        switch (param)
        {
            case 0:
                //InitActivityLeftPanel();
                //InitActivityTopPanel();
                break;
            case 1:
                isGetRechargeData = true;
                requestType = 15;
                PlayerInfo.getInstance().sendRequest(new ActivityInfoJson(butNowEleId, 0, 4), this);
                //UpdateActInfoResult();
                //InitRechargePanel();
                break;
        }
    }

    public void CloseActivityPanel()
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
        GameObjectUtil.destroyGameObjectAllChildrens(pageContentPos);
        hide();

        HeadUI.mInstance.refreshPlayerInfo();
    }

    public void CloseActivityPanelOnly()
    {
        Main3dCameraControl.mInstance.SetBool(false);
        GameObjectUtil.destroyGameObjectAllChildrens(pageContentPos);
        //		base.hide();
        gc();
        UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE);
    }

    public void init()
    {
        //		base.init();
    }

    void ResetScrollBar(UIScrollBar tScrollBar)
    {
        tScrollBar.value = 0;
    }

    public void receiveResponse(string json)
    {
        if (json != null)
        {
            //关闭连接界面的动画//
            PlayerInfo.getInstance().isShowConnectObj = false;
            switch (requestType)
            {
                case 1:
                    {
                        LoginSevenDayResJson res = JsonMapper.ToObject<LoginSevenDayResJson>(json);
                        errorCode = res.errorCode;
                        if (errorCode == 0)
                        {
                            sevenResJson = res;
                        }
                        receiveData = true;
                    }
                    break;
                case 2:
                    {
                        LoginSevenDayResJson res = JsonMapper.ToObject<LoginSevenDayResJson>(json);
                        errorCode = res.errorCode;
                        if (errorCode == 0)
                        {
                            sevenResJson = res;
                        }
                        receiveData = true;
                    }
                    break;
                case 3:
                    {
                        RunnerUIResultJson rTempJson = JsonMapper.ToObject<RunnerUIResultJson>(json);
                        Debug.Log("RunnerUIResultJson:" + json);
                        errorCode = rTempJson.errorCode;
                        if (errorCode == 0)
                        {
                            rUIResultJson = rTempJson;
                        }
                        receiveData = true;
                    }
                    break;
                case 12:
                    {
                        GiftCodeResultJson gcrj = JsonMapper.ToObject<GiftCodeResultJson>(json);
                        errorCode = gcrj.errorCode;
                        if (errorCode == 0)
                        {
                            this.gcrj = gcrj;
                        }
                        receiveData = true;
                    }
                    break;
                case 13:
                    {
                        RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
                        errorCode = rechargej.errorCode;
                        if (errorCode == 0)
                        {
                            rechargeRJ = rechargej;
                        }
                        receiveData = true;
                    }
                    break;
                case 14:
                    {
                        ActivityInfoResultJson airj = JsonMapper.ToObject<ActivityInfoResultJson>(json);
                        errorCode = airj.errorCode;
                        if (errorCode == 0)
                        {
                            gmaij = airj;
                        }
                        receiveData = true;
                    }
                    break;
                case 15:
                    {
                        ActivityInfoResultJson airj = JsonMapper.ToObject<ActivityInfoResultJson>(json);
                        errorCode = airj.errorCode;
                        if (errorCode == 0)
                        {
                            gmaij = airj;
                        }
                        receiveData = true;
                    }
                    break;
                case 16:
                    {
                        ExchangeResultJson erj = JsonMapper.ToObject<ExchangeResultJson>(json);
                        errorCode = erj.errorCode;
                        if (errorCode == 0)
                        {
                            exrj = erj;
                        }
                        receiveData = true;
                    }
                    break;
                case 17:
                    {
                        ActivityResultJson arj = JsonMapper.ToObject<ActivityResultJson>(json);
                        errorCode = arj.errorCode;
                        if (errorCode == 0)
                        {
                            aes = arj.acts;
                        }
                        receiveData = true;
                    }
                    break;
                case 18:
                    {
                        RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
                        errorCode = rechargej.errorCode;
                        if (errorCode == 0)
                        {
                            curVIPRJ = rechargej;
                        }
                        receiveData = true;
                    }
                    break;
                case 19:
                    BuyPowerOrGoldResultJson bpogrj = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
                    errorCode = bpogrj.errorCode;
                    if (errorCode == 0)
                    {
                        this.bpgrj = bpogrj;
                    }
                    receiveData = true;
                    break;
                case 20:
                    {
                        CornucopiaResultJson irj = JsonMapper.ToObject<CornucopiaResultJson>(json);
                        errorCode = irj.errorCode;
                        if (errorCode == 0)
                        {
                            cornucopiaResultJson = irj;
                        }
                        receiveData = true;
                    }
                    break;
                case 21:
                    {
                        ActivityGResultJson actgrj = JsonMapper.ToObject<ActivityGResultJson>(json);
                        errorCode = actgrj.errorCode;
                        if (errorCode == 0)
                        {
                            agrj.age = new List<ActivityGElement>();
                            for (int i = 0; i < actgrj.age.Count; i++)
                            {
                                if (actgrj.age[i].aid == 24) { continue; }
                                agrj.age.Add(actgrj.age[i]);
                            }
                           
                        }
                        receiveData = true;
                    }
                    break;
                case 22:
                    {
                        ActivityInfoResultJson airj = JsonMapper.ToObject<ActivityInfoResultJson>(json);
                        errorCode = airj.errorCode;
                        if (errorCode == 0)
                        {
                            naij = airj;
                        }
                        receiveData = true;
                    }
                    break;
                case 23:
                    {
                        LotEventRankResultJson lerrj = JsonMapper.ToObject<LotEventRankResultJson>(json);
                        errorCode = lerrj.errorCode;
                        if (errorCode == 0)
                        {
                            lotjson = lerrj;
                        }
                        receiveData = true;
                    }
                    break;
            }
        }
    }

    public void show(int pagePos = 0)
    {
        Main3dCameraControl.mInstance.SetBool(true);
        tipCtrl.SetActive(false);
        jhmRewardPanel.SetActive(false);
        helpUnitPanel.SetActive(false);

        //set clip pos
        setDragPanelPos(pagePos);

        //init top draggable panel
        InitActivityTopPanel();

        //set select page
        BtnTopIcon(pagePos);
    }

    public void hide()
    {
        HeadUI.mInstance.refreshPlayerInfo();
        Main3dCameraControl.mInstance.SetBool(false);
        TalkMainToGetData();
        //		base.hide();
        gc();

        UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE);
    }

    public void gc()
    {
        //leftItemPrefab = null;
        //rightItemPrefab = null;
        jhmItemPrefab = null;
        cardRewardPrefab = null;
        actTopIconPrefab = null;
        sevenPrizeResPrefab = null;
        levelPrizeResPrefab = null;
        cornucopiaResPrefab = null;
        aes = null;
        tes = null;
        otherAtlas = null;
        gcrj = null;
        rechargeRJ = null;
        gmaij = null;
        exrj = null;
        sevenResJson = null;
        cornucopiaPanel = null;
        cornucopiaResultJson = null;
        agrj = null;
        naij = null;
        bpgrj = null;

        //==释放资源==//
        Resources.UnloadUnusedAssets();
    }

    //modify - 20140901
    void InitActivityTopPanel()
    {
        topUIIcon.Clear();
        GameObjectUtil.destroyGameObjectAllChildrens(topUIGrid);

        if (actTopIconPrefab == null)
        {
            actTopIconPrefab = Resources.Load(ActTopIconPath) as GameObject;
        }

        for (int i = 0; i < aes.Count; i++)
        {
            ActivityElement ae = aes[i];
            GameObject obj = Instantiate(actTopIconPrefab) as GameObject;
            obj.transform.parent = topUIGrid.transform;
            obj.transform.localScale = Vector3.one;
            obj.name = actTopIconPrefab.name + (i + 1000);

            initCallback(obj, i);

            ActTopIcon actTop = obj.GetComponent<ActTopIcon>();
            actTop.Init(ae);

            topUIIcon.Add(actTop);
        }
        topUIGrid.GetComponent<UIGrid>().repositionNow = true;
    }

    void initCallback(GameObject obj, int param)
    {
        UIButtonMessage mess = obj.GetComponent<UIButtonMessage>();
        mess.target = gameObject;
        mess.functionName = "OnBtnTopIcon";
        mess.param = param;
    }

    void OnBtnTopIcon(int param)
    {
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (param == butNowSel) return;

        BtnTopIcon(param);
    }

    void BtnTopIcon(int selId)
    {
        //Debug.Log("--------------------- selId:" + selId);
        butNowSel = selId;

        setButSel(butNowSel);

        GameObjectUtil.destroyGameObjectAllChildrens(pageContentPos);
        ActivityElement ae = aes[butNowSel];
        butNowEleId = ae.id;

        //ae.type 1-激活码 2-7天奖励 3-等级奖励 4-精彩兑换 5-vip礼包 6-聚宝盆 7-游戏公告  8-大风车 9-限时神将//
        switch (ae.type)
        {
            case 1:
                InitJHMPanel();
                break;
            case 2:
                requestType = 1;
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SEVEN_PRIZE), this);
                break;
            case 3:
                requestType = 2;
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_LEVEL_PRIZE), this);
                break;
            case 4:
                GetRechargeData();
                break;
            case 5:
                GetGiftGroupData();
                break;
            case 6:
                Getcornucopia();
                break;
            case 7:
                GetNoticeData(ae.id, ae.type);
                break;
            case 8:
                //initWheelPanel();
                requestType = 3;
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_ACT_WHEEL), this);
                break;
            case 9:
                GetLotActivity();
                break;
        }

    }

    void setButSel(int pos)
    {
        for (int i = 0; i < topUIIcon.Count; i++)
        {
            if (i == pos)
                topUIIcon[i].setSel(true);
            else
                topUIIcon[i].setSel(false);
        }
    }

    void setDragPanelPos(int pos)
    {
        topPanelClip = topPanel.GetComponent<UIPanel>().clipRange;

        float cellWidth = topUIGrid.GetComponent<UIGrid>().cellWidth;

        int temp = pos;
        if (temp > aes.Count - 5)
            temp = aes.Count - 5;

        topPanel.transform.localPosition -= new Vector3(temp * cellWidth, 0, 0);
        topPanelClip.x += temp * cellWidth;
        topPanel.GetComponent<UIPanel>().clipRange = topPanelClip;
    }

    void initSevenPrizePanel()
    {
        //Destroy(pageContent);
        if (sevenPrizeResPrefab == null)
        {
            sevenPrizeResPrefab = Resources.Load(ActSevenPanel) as GameObject;
        }

        pageContent = Instantiate(sevenPrizeResPrefab) as GameObject;
        pageContent.transform.parent = pageContentPos.transform;
        pageContent.transform.localScale = Vector3.one;
        pageContent.transform.localPosition = Vector3.zero;

        ActTopIcon temp = null;
        for (int i = 0; i < aes.Count; i++)
        {
            if (aes[i].type == 2)   //2表示七天,见BtnTopIcon//
                temp = topUIIcon[i];
        }
        pageContent.GetComponent<ActSevenPanel>().Init(sevenResJson, butNowEleId, temp);
    }

    void initLevelPrizePanel()
    {
        //Destroy(pageContent);
        if (levelPrizeResPrefab == null)
        {
            levelPrizeResPrefab = Resources.Load(ActLevelPanel) as GameObject;
        }

        pageContent = Instantiate(levelPrizeResPrefab) as GameObject;
        pageContent.transform.parent = pageContentPos.transform;
        pageContent.transform.localScale = Vector3.one;
        pageContent.transform.localPosition = Vector3.zero;

        ActTopIcon temp = null;
        for (int i = 0; i < aes.Count; i++)
        {
            if (aes[i].type == 3)   //3表示等级奖励,见BtnTopIcon//
                temp = topUIIcon[i];
        }
        pageContent.GetComponent<ActLevelPanel>().Init(sevenResJson, butNowEleId, temp);
    }

    void initWheelPanel()
    {
        if (wheelResPrefab == null)
        {
            wheelResPrefab = Resources.Load(ActWheelPanelPath) as GameObject;
        }

        pageContent = Instantiate(wheelResPrefab) as GameObject;
        pageContent.transform.parent = pageContentPos.transform;
        pageContent.transform.localScale = Vector3.one;
        pageContent.transform.localPosition = Vector3.zero;
        ActTopIcon temp = null;
        for (int i = 0; i < aes.Count; i++)
        {
            if (aes[i].type == 8)   //8表示大风车,见BtnTopIcon//
                temp = topUIIcon[i];
        }
        pageContent.GetComponent<ActWheelPanel>().Init(gameObject, rUIResultJson, butNowEleId, temp);
    }

    void initLotCardPanel()
    {
        if (LotCardPanelPrefab == null)
        {
            LotCardPanelPrefab = Resources.Load(LotCardPanelPath) as GameObject;
        }
        pageContent = Instantiate(LotCardPanelPrefab) as GameObject;
        pageContent.transform.parent = pageContentPos.transform;
        pageContent.transform.localScale = Vector3.one;
        pageContent.transform.localPosition = Vector3.zero;
        pageContent.GetComponent<LotActivityPanel>().Init(lotjson);
    }

    void InitJHMPanel()
    {
        GameObjectUtil.destroyGameObjectAllChildrens(pageContentPos);
        if (jhmItemPrefab == null)
        {
            jhmItemPrefab = Resources.Load("Prefabs/UI/ActivityPanel/jhmChargeItem") as GameObject;
        }
        GameObject jhmItem = Instantiate(jhmItemPrefab) as GameObject;
        jhmInput = jhmItem.transform.FindChild("Input").GetComponent<UIInput>();
        jhmInput.value = "";
        jhmItem.transform.FindChild("ConfirmButton").GetComponent<UIButtonMessage>().target = gameObject;
        jhmItem.transform.FindChild("ConfirmButton").GetComponent<UIButtonMessage>().param = 11;
        GameObjectUtil.gameObjectAttachToParent(jhmItem, pageContentPos);
    }

    void GetGiftGroupData()
    {
        requestType = 18;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE), this);
    }

    void GetNoticeData(int nId, int nType)
    {
        isGetRechargeData = false;
        requestType = 21;
        PlayerInfo.getInstance().sendRequest(new ActivityGJson(nId, nType), this);
    }

    void GetRechargeData()
    {
        isGetRechargeData = true;
        requestType = 15;
        PlayerInfo.getInstance().sendRequest(new ActivityInfoJson(butNowEleId, 0, 4), this);
    }

    void InitNoticePanel(int nId, int nType)
    {
        GameObjectUtil.destroyGameObjectAllChildrens(pageContentPos);
        if (agrj != null && gmaij != null)
        {
            if (noticePrefab == null)
            {
                noticePrefab = Resources.Load("Prefabs/UI/ActivityPanel/NoticeItem") as GameObject;
            }
            noticeItem = Instantiate(noticePrefab) as GameObject;
            GameObject nLeftGrid = noticeItem.transform.FindChild("LeftClipPanel/UIGrid").gameObject;
            GameObjectUtil.gameObjectAttachToParent(noticeItem, pageContentPos);
            if (noticeBtnList == null)
            {
                noticeBtnList = new List<UISprite>();
            }
            else
            {
                noticeBtnList.Clear();
            }
            for (int i = 0; i < agrj.age.Count; i++)
            {
                ActivityGElement agElement = agrj.age[i];
                if (noticeBtnIconPrefab == null)
                {
                    noticeBtnIconPrefab = Resources.Load("Prefabs/UI/ActivityPanel/NoticeBtnIcon") as GameObject;
                }
                GameObject noticeBtnItem = Instantiate(noticeBtnIconPrefab) as GameObject;
                noticeBtnItem.name = "NoticeBtnIcon" + (i + 1000).ToString();
                UIButtonMessage nBtnMsg = noticeBtnItem.GetComponent<UIButtonMessage>();
                nBtnMsg.target = gameObject;
                nBtnMsg.functionName = "OnNoticeBtnIcon";
                nBtnMsg.param = agElement.aid;

                noticeBtnList.Add(noticeBtnItem.GetComponent<UISprite>());

                UILabel nName = noticeBtnItem.transform.FindChild("Label").GetComponent<UILabel>();
                nName.text = agElement.name;

                GameObject hotObj = noticeBtnItem.transform.FindChild("Hot").gameObject;
                if (agElement.hot == 0)
                {
                    hotObj.SetActive(false);
                }
                else
                {
                    hotObj.SetActive(true);
                }

                GameObjectUtil.gameObjectAttachToParent(noticeBtnItem, nLeftGrid);
            }

            nLeftGrid.GetComponent<UIGrid>().repositionNow = true;
            SetBtnState(noticeBtnList, curNoticeId);

            GameObject nRightClip = noticeItem.transform.FindChild("RightClipPanel").gameObject;
            noticeClipPanelVec = nRightClip.GetComponent<UIPanel>().clipRange;
            nRightClip.transform.localPosition = Vector3.zero;
            if (nRightClip.GetComponent<SpringPanel>())
            {
                nRightClip.GetComponent<SpringPanel>().enabled = false;
            }
            UILabel nLabel = noticeItem.transform.FindChild("RightClipPanel/Label").GetComponent<UILabel>();
            nLabel.text = gmaij.content;
            SetLabelColliderHeight(nRightClip, nLabel);
        }
    }

    void InitRechargePanel()
    {
        GameObjectUtil.destroyGameObjectAllChildrens(pageContentPos);
        if (equipReItemPrefab == null)
        {
            equipReItemPrefab = Resources.Load("Prefabs/UI/ActivityPanel/EqiupRechargeItem") as GameObject;
        }
        GameObject equipRechargeItem = Instantiate(equipReItemPrefab) as GameObject;
        GameObjectUtil.gameObjectAttachToParent(equipRechargeItem, pageContentPos);

        equipRechargeItem.transform.FindChild("NoticeClipPanel/Label").GetComponent<UILabel>().text = gmaij.content;
        rechargeClipPanel = equipRechargeItem.transform.FindChild("ERechargePanel").gameObject;
        GameObject echargeUIGrid = equipRechargeItem.transform.FindChild("ERechargePanel/UIGrid").gameObject;
        //设置滚动条的位置到上一次的位置//
        Vector3 rClipVec = rechargeClipPanel.transform.localPosition;
        if (lastRechargeClipPos != Vector3.zero && rClipVec != lastRechargeClipPos)
        {
            rechargeClipPanel.transform.localPosition = lastRechargeClipPos;
            rechargeClipPanel.GetComponent<UIPanel>().clipRange = rDragPanelClip;
        }
        //		if(lastRechargeSBPos != 0)
        //		{
        //			Invoke("SetRechargeClipPanelPos",0.1f);
        //		}
        for (int i = 0; i < gmaij.exActs.Count; i++)
        {
            if (rechargeItemPrefab == null)
            {
                rechargeItemPrefab = Resources.Load("Prefabs/UI/ActivityPanel/rechargeItem") as GameObject;
            }
            GameObject rechargeItem = Instantiate(rechargeItemPrefab) as GameObject;
            GameObjectUtil.gameObjectAttachToParent(rechargeItem, echargeUIGrid);
            ActivityInfoExchangeElement aieElement = gmaij.exActs[i];
            rechargeItem.transform.FindChild("Desc").GetComponent<UILabel>().text = aieElement.exchangeContext;
            UILabel pSoleLabel = rechargeItem.transform.FindChild("Num").GetComponent<UILabel>();
            if (aieElement.sole == 0)
            {
                pSoleLabel.text = TextsData.getData(635).chinese;
            }
            else
            {
                string str = TextsData.getData(634).chinese;
                string temp1 = str.Replace("num1", (aieElement.sole - aieElement.pSole).ToString());
                string temp2 = temp1.Replace("num2", aieElement.sole.ToString());
                pSoleLabel.text = temp2;
            }

            GameObject leftRegion = rechargeItem.transform.FindChild("LeftRegion").gameObject;
            GameObject rightRegion = rechargeItem.transform.FindChild("RightRegion").gameObject;
            List<GameObject> sourceObjList = new List<GameObject>();

            for (int k = 0; k < aieElement.ade.Count; k++)
            {
                if (rechargeCellPrefab == null)
                {
                    rechargeCellPrefab = Resources.Load("Prefabs/UI/ActivityPanel/rechargeCell") as GameObject;
                }
                GameObject rechargeCell = Instantiate(rechargeCellPrefab) as GameObject;
                rechargeCell.name = "Rward" + (k + 1).ToString();
                GameObjectUtil.gameObjectAttachToParent(rechargeCell, leftRegion);
                sourceObjList.Add(rechargeCell);
            }

            if (sourceObjList.Count == 1)
            {
                sourceObjList[0].transform.localPosition = new Vector3(-225, -13, 0);
                rightRegion.transform.localPosition = new Vector3(-85, -125, 0);
            }
            else if (sourceObjList.Count == 2)
            {
                sourceObjList[0].transform.localPosition = new Vector3(-270, -13, 0);
                sourceObjList[1].transform.localPosition = new Vector3(-128, -13, 0);
                rightRegion.transform.localPosition = new Vector3(-85, -125, 0);
            }
            else if (sourceObjList.Count == 3)
            {
                sourceObjList[0].transform.localPosition = new Vector3(-322, -13, 0);
                sourceObjList[1].transform.localPosition = new Vector3(-200, -13, 0);
                sourceObjList[2].transform.localPosition = new Vector3(-76, -13, 0);
                rightRegion.transform.localPosition = new Vector3(-85, -125, 0);
            }
            for (int m = 0; m < sourceObjList.Count; m++)
            {
                GameObject obj = sourceObjList[m];
                obj.GetComponent<RechargeItemInfo>().tempActInfoEElement = aieElement;
                obj.GetComponent<RechargeItemInfo>().helpUnitPanel = helpUnitPanel;
                UIButtonMessage[] iconMsgs = obj.GetComponents<UIButtonMessage>();
                UIButtonMessage iconMsg = iconMsgs[0];
                iconMsg.target = obj;
                iconMsg.functionName = "showHelperUnit";

                UIButtonMessage iconMsg2 = iconMsgs[1];
                iconMsg2.target = obj;
                iconMsg2.functionName = "hiddleHelperUnit";

                InitRechargeIcon(null, aieElement.ade[m], obj, 0);
            }

            //改变兑换的图标状态//
            UIButton rechargeUIBtn = rechargeItem.transform.FindChild("RightRegion/Button").GetComponent<UIButton>();
            if (aieElement.sell == 0)
            {
                rechargeUIBtn.isEnabled = false;
                rechargeUIBtn.UpdateColor(false, true);
                //btnLabel.text = TextsData.getData(415).chinese;
            }
            else if (aieElement.sell == 2)
            {
                rechargeUIBtn.isEnabled = false;
                rechargeUIBtn.UpdateColor(false, true);
                //btnLabel.text = TextsData.getData(504).chinese;
            }
            else if (aieElement.sell == 3)
            {
                rechargeUIBtn.isEnabled = false;
                rechargeUIBtn.UpdateColor(false, true);
                //btnLabel.text = TextsData.getData(505).chinese;
            }
            else if (aieElement.sell == 1)
            {
                rechargeUIBtn.isEnabled = true;
                rechargeUIBtn.UpdateColor(true, true);

                UIButtonMessage rechargeBtn = rechargeItem.transform.FindChild("RightRegion/Button").GetComponent<UIButtonMessage>();

                rechargeBtn.target = gameObject;
                rechargeBtn.param = aieElement.id;
                rechargeBtn.functionName = "OnRechargeBtn";

            }
            //设置目标物体的信息//
            GameObject targetObj = rechargeItem.transform.FindChild("RightRegion/Rward0").gameObject;
            targetObj.GetComponent<RechargeItemInfo>().tempActInfoEElement = aieElement;
            targetObj.GetComponent<RechargeItemInfo>().helpUnitPanel = helpUnitPanel;
            UIButtonMessage[] iMsgs = targetObj.GetComponents<UIButtonMessage>();
            UIButtonMessage iMsg = iMsgs[0];
            iMsg.target = targetObj;
            iMsg.functionName = "showHelperUnit";

            UIButtonMessage iMsg2 = iMsgs[1];
            iMsg2.target = targetObj;
            iMsg2.functionName = "hiddleHelperUnit";

            InitRechargeIcon(aieElement, null, targetObj, 1);

        }
        echargeUIGrid.GetComponent<UIGrid>().repositionNow = true;

    }

    void UpdateRechargeMark()
    {
        for (int i = 0; i < aes.Count; i++)
        {
            if (aes[i].type == 4)
            {

            }
        }
    }

    void setDragPanelPos(GameObject clipPanel, GameObject grid, int total, int index, int pageNum)
    {
        dragPanelClip = clipPanel.GetComponent<UIPanel>().clipRange;

        float cellHeight = grid.GetComponent<UIGrid>().cellHeight;
        int temp = 0;
        if (index > total - pageNum)
            temp = total - pageNum;
        else
            temp = index - 1;

        clipPanel.transform.localPosition += new Vector3(0, temp * cellHeight, 0);
        dragPanelClip.y -= temp * cellHeight;
        clipPanel.GetComponent<UIPanel>().clipRange = dragPanelClip;
    }

    void setReDragPanelPos(GameObject clipPanel, GameObject grid, int total, int index, int pageNum)
    {

        rDragPanelClip = clipPanel.GetComponent<UIPanel>().clipRange;

        float cellHeight = grid.GetComponent<UIGrid>().cellHeight;
        int temp = 0;
        if (index > total - pageNum)
            temp = total - pageNum;
        else
            temp = index - 1;

        clipPanel.transform.localPosition += new Vector3(0, temp * cellHeight, 0);
        rDragPanelClip.y -= temp * cellHeight;
        clipPanel.GetComponent<UIPanel>().clipRange = rDragPanelClip;
    }

    //	void SetRechargeClipPanelPos()
    //	{
    //		rechargeClipPanel.GetComponent<UIDraggablePanel>().verticalScrollBar.value = lastRechargeSBPos;
    //	}

    void InitGiftGroup()
    {
        if (giftGroupPrefab == null)
        {
            giftGroupPrefab = Resources.Load(giftGroupPrefabPath) as GameObject;
        }
        GameObject giftGroup = Instantiate(giftGroupPrefab) as GameObject;

        GameObjectUtil.gameObjectAttachToParent(giftGroup, pageContentPos);

        giftBottomTopObj = giftGroup.transform.FindChild("GiftBottomTopPanel").gameObject;
        giftBottomLeftObj = giftGroup.transform.FindChild("GiftBottomLeftPanel").gameObject;
        giftBottomRightObj = giftGroup.transform.FindChild("GiftBottomRightPanel").gameObject;
        giftBottomSpecialObj = giftGroup.transform.FindChild("SpecialPowerPanel").gameObject;
        giftBottomSpecialObj.SetActive(false);

        giftLeftClipPanel = giftBottomLeftObj.transform.FindChild("ClipLeftPanel").gameObject;
        giftRightClipPanel = giftBottomRightObj.transform.FindChild("ClipRightPanel").gameObject;

        giftLeftScrollbar = giftBottomLeftObj.transform.FindChild("ScrollBar").GetComponent<UIScrollBar>();

        curVIPValue = curVIPRJ.vipLv;
        tempVIPValue = curVIPValue;
        if (curVIPValue == 0)
        {
            tempVIPValue = 1;
        }
        else
        {
            tempVIPValue = curVIPValue;
        }
        InitGiftTopRegion();
        InitGiftLeftRegion();
        InitGiftRightRegion();
        //等待0.1秒之后再去请求更新scrollbar的位置，如果不等待，可能会无法更新scrollbar的位置//
        //Invoke("SetLeftScrollbarPos",0.1f);
    }

    void Getcornucopia()
    {
        requestType = 20;
        PlayerInfo.getInstance().sendRequest(new UIJson(49, 6), this);
    }

    void GetLotActivity()
    {
        requestType = 23;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_ACT_LOTCARD), this);
    }

    void InitCornucopia()
    {
        if (cornucopiaResPrefab == null)
        {
            cornucopiaResPrefab = Resources.Load(CornucopiaPath) as GameObject;
        }

        pageContent = Instantiate(cornucopiaResPrefab) as GameObject;
        pageContent.transform.parent = pageContentPos.transform;
        pageContent.transform.localScale = Vector3.one;
        pageContent.transform.localPosition = Vector3.zero;

        cornucopiaPanel = pageContent.GetComponent<CornucopiaPanel>();
        cornucopiaPanel.errorCode = errorCode;
        cornucopiaPanel.Refresh();

        cornucopiaPanel.SetId(cornucopiaResultJson.id, cornucopiaResultJson.time);
    }

    void InitGiftTopRegion()
    {
        if (giftBottomTopObj == null)
        {
            return;
        }
        //Init gift top region
        UILabel curVIP = giftBottomTopObj.transform.FindChild("CurrentVIP/VIPValue").GetComponent<UILabel>();
        curVIP.text = curVIPRJ.vipLv.ToString();

        UISprite vipCrystalPB = giftBottomTopObj.transform.FindChild("pbVIPCrystal").GetComponent<UISprite>();
        vipCrystalPB.fillAmount = (float)curVIPRJ.vipCost / curVIPRJ.sCost;

        UIButtonMessage toVIPSpecialBtn = giftBottomTopObj.transform.FindChild("ToVIPSpecialBtn").GetComponent<UIButtonMessage>();
        toVIPSpecialBtn.target = gameObject;
        toVIPSpecialBtn.functionName = "OnToVIPSpecialBtn";
    }

    void InitGiftLeftRegion()
    {
        if (giftBottomLeftObj == null)
        {
            return;
        }
        //Init gift left region
        GameObject leftGrid = giftLeftClipPanel.transform.FindChild("Grid").gameObject;
        setDragPanelPos(giftLeftClipPanel, leftGrid, VipData.vipList.Count, tempVIPValue, 5);

        GameObjectUtil.destroyGameObjectAllChildrens(leftGrid);
        if (vipBtnList == null)
        {
            vipBtnList = new List<UISprite>();
        }
        else
        {
            vipBtnList.Clear();
        }

        for (int i = 0; i < VipData.vipList.Count; i++)
        {
            //不显示VIP0//
            if (VipData.vipList[i] != 0)
            {
                if (vipLeftBtnPrefab == null)
                {
                    vipLeftBtnPrefab = Resources.Load(vipLeftBtnPath) as GameObject;
                }
                GameObject vItem = Instantiate(vipLeftBtnPrefab) as GameObject;
                if (i < 10)
                {
                    vItem.name = "VIPGiftBtn0" + i.ToString();
                }
                else
                {
                    vItem.name = "VIPGiftBtn" + i.ToString();
                }

                vipBtnList.Add(vItem.GetComponent<UISprite>());
                vItem.GetComponent<UIButtonMessage>().target = gameObject;
                vItem.GetComponent<UIButtonMessage>().functionName = "OnVIPLeftBtn";
                vItem.GetComponent<UIButtonMessage>().param = VipData.vipList[i];
                vItem.GetComponent<UIDragPanelContents>().draggablePanel = giftLeftClipPanel.GetComponent<UIDraggablePanel>();
                vItem.transform.FindChild("VIPValue").GetComponent<UILabel>().text = VipData.vipList[i].ToString();
                GameObjectUtil.gameObjectAttachToParent(vItem, leftGrid);
            }
        }
        leftGrid.GetComponent<UIGrid>().repositionNow = true;
        //SetVIPBtnState(tempVIPValue);
        SetBtnState(vipBtnList, tempVIPValue);
    }

    void InitGiftRightRegion()
    {
        if (giftBottomRightObj == null)
        {
            return;
        }
        //Init gift right region
        GameObject rightGrid = giftRightClipPanel.transform.FindChild("Grid").gameObject;
        GameObjectUtil.destroyGameObjectAllChildrens(rightGrid);

        //VipGiftData tvgData = VipGiftData.getData(tempVIPValue);
        List<string> gItemList = VipGiftData.getItemList(tempVIPValue);
        for (int i = 0; i < gItemList.Count; i++)
        {
            if (giftItemPrefab == null)
            {
                giftItemPrefab = Resources.Load(giftItemPath) as GameObject;
            }
            GameObject gItem = Instantiate(giftItemPrefab) as GameObject;
            if (i < 9)
            {
                gItem.name = "gItem0" + (i + 1).ToString();
            }
            else
            {
                gItem.name = "gItem" + (i + 1).ToString();
            }
            gItem.GetComponent<RechargeItemInfo>().helpUnitPanel = helpUnitPanel;
            gItem.GetComponent<UIDragPanelContents>().draggablePanel = giftRightClipPanel.GetComponent<UIDraggablePanel>();
            UIButtonMessage[] iconMsgs = gItem.GetComponents<UIButtonMessage>();
            UIButtonMessage iconMsg = iconMsgs[0];
            iconMsg.target = gItem;
            iconMsg.functionName = "showHelperUnit";

            UIButtonMessage iconMsg2 = iconMsgs[1];
            iconMsg2.target = gItem;
            iconMsg2.functionName = "hiddleHelperUnit";

            UISprite rewardIconBG = gItem.transform.FindChild("IconBG").GetComponent<UISprite>();
            rewardIconBG.gameObject.SetActive(false);
            UILabel rewardLabel = gItem.transform.FindChild("Text").GetComponent<UILabel>();
            UILabel rewardName = gItem.transform.FindChild("Name").GetComponent<UILabel>();

            rewardLabel.text = string.Empty;
            SimpleCardInfo2 cardInfo = gItem.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
            cardInfo.clear();
            cardInfo.gameObject.SetActive(false);

            string[] ss = gItemList[i].Split('-');
            int rewardType = StringUtil.getInt(ss[0]);
            string rewardText = StringUtil.getString(ss[1]);

            int rewardID = 0;

            switch (rewardType)
            {
                case (int)VipGiftType.E_Item:
                    {
                        string[] tempS = rewardText.Split(',');
                        int itemID = StringUtil.getInt(tempS[0]);
                        rewardID = itemID;
                        int num = StringUtil.getInt(tempS[1]);
                        ItemsData itemData = ItemsData.getData(itemID);
                        if (itemData == null)
                        {
                            gItem.SetActive(false);
                            continue;
                        }
                        cardInfo.gameObject.SetActive(true);
                        cardInfo.setSimpleCardInfo(itemID, GameHelper.E_CardType.E_Item);

                        rewardLabel.text = " x " + num.ToString();
                        rewardName.text = itemData.name;

                    }
                    break;
                case (int)VipGiftType.E_Equip:
                    {
                        string[] tempS = rewardText.Split(',');
                        int equipID = StringUtil.getInt(tempS[0]);
                        rewardID = equipID;
                        int num = StringUtil.getInt(tempS[1]);

                        EquipData ed = EquipData.getData(equipID);
                        if (ed == null)
                        {
                            gItem.SetActive(false);
                            continue;
                        }
                        cardInfo.gameObject.SetActive(true);
                        cardInfo.setSimpleCardInfo(equipID, GameHelper.E_CardType.E_Equip);

                        rewardLabel.text = " x " + num.ToString();
                        rewardName.text = ed.name;

                    }
                    break;
                case (int)VipGiftType.E_Card:
                    {
                        string[] tempS = rewardText.Split(',');
                        int heroID = StringUtil.getInt(tempS[0]);
                        rewardID = heroID;
                        int num = StringUtil.getInt(tempS[1]);

                        CardData cd = CardData.getData(heroID);
                        if (cd == null)
                        {
                            gItem.SetActive(false);
                            continue;
                        }
                        cardInfo.gameObject.SetActive(true);
                        cardInfo.setSimpleCardInfo(heroID, GameHelper.E_CardType.E_Hero);

                        rewardLabel.text = " x " + num.ToString();
                        rewardName.text = cd.name;

                    }
                    break;
                case (int)VipGiftType.E_Skill:
                    {
                        string[] tempS = rewardText.Split(',');
                        int skillID = StringUtil.getInt(tempS[0]);
                        rewardID = skillID;
                        int num = StringUtil.getInt(tempS[1]);

                        SkillData sd = SkillData.getData(skillID);
                        if (sd == null)
                        {
                            gItem.SetActive(false);
                            continue;
                        }
                        cardInfo.gameObject.SetActive(true);
                        cardInfo.setSimpleCardInfo(skillID, GameHelper.E_CardType.E_Skill);

                        rewardLabel.text = " x " + num.ToString();
                        rewardName.text = sd.name;

                    }
                    break;
                case (int)VipGiftType.E_PassiveSkill:
                    {
                        string[] tempS = rewardText.Split(',');
                        int passiveSkillID = StringUtil.getInt(tempS[0]);
                        rewardID = passiveSkillID;
                        int num = StringUtil.getInt(tempS[1]);

                        PassiveSkillData psd = PassiveSkillData.getData(passiveSkillID);
                        if (psd == null)
                        {
                            gItem.SetActive(false);
                            continue;
                        }
                        cardInfo.gameObject.SetActive(true);
                        cardInfo.setSimpleCardInfo(passiveSkillID, GameHelper.E_CardType.E_PassiveSkill);

                        rewardLabel.text = " x " + num.ToString();
                        rewardName.text = psd.name;

                    }
                    break;
                case (int)VipGiftType.E_Gold:
                    {
                        rewardIconBG.gameObject.SetActive(true);
                        rewardIconBG.atlas = otherAtlas;
                        rewardIconBG.spriteName = goldSpriteName;
                        rewardLabel.text = "x " + rewardText;
                        rewardName.text = "";

                    }
                    break;
                case (int)VipGiftType.E_Exp:
                    {
                        rewardIconBG.gameObject.SetActive(true);
                        rewardIconBG.atlas = otherAtlas;
                        rewardIconBG.spriteName = expSpriteName;
                        rewardLabel.text = "x " + rewardText;
                        rewardName.text = "";
                    }
                    break;
                case (int)VipGiftType.E_Crystal:
                    {
                        rewardIconBG.gameObject.SetActive(true);
                        rewardIconBG.atlas = otherAtlas;
                        rewardIconBG.spriteName = crystalSpriteName;
                        rewardLabel.text = "x " + rewardText;
                        rewardName.text = "";
                    }
                    break;
                case (int)VipGiftType.E_Friend:
                    {
                        rewardIconBG.gameObject.SetActive(true);
                        rewardIconBG.atlas = friendshipAtlas;
                        rewardIconBG.spriteName = friendshipSpriteName;
                        rewardLabel.text = "x " + rewardText;
                        rewardName.text = "";
                    }
                    break;
            }

            if (gItem.GetComponent<RechargeItemInfo>())
            {
                gItem.GetComponent<RechargeItemInfo>().itemType = rewardType;
                gItem.GetComponent<RechargeItemInfo>().itemId = rewardID;
            }

            GameObjectUtil.gameObjectAttachToParent(gItem, rightGrid);
        }
        rightGrid.GetComponent<UIGrid>().repositionNow = true;

        if (gItemList.Count == 1)
        {
            rightGrid.transform.localPosition = new Vector3(70, 9, 0);
        }
        else if (gItemList.Count == 2)
        {
            rightGrid.transform.localPosition = new Vector3(0, 9, 0);
        }
        else if (gItemList.Count == 3)
        {
            rightGrid.transform.localPosition = new Vector3(-100, 9, 0);
        }
        else
        {
            rightGrid.transform.localPosition = new Vector3(-210, 9, 0);
        }

        ResetScrollBar(giftRightClipPanel.GetComponent<UIDraggablePanel>().horizontalScrollBar);

        UILabel oldValue = giftBottomRightObj.transform.FindChild("OldValue/Value").GetComponent<UILabel>();
        oldValue.text = VipData.getData(tempVIPValue).fakeprice.ToString();
        UILabel newValue = giftBottomRightObj.transform.FindChild("NewValue/Value").GetComponent<UILabel>();
        newValue.text = VipData.getData(tempVIPValue).realprice.ToString();

        UIButtonMessage toChargeBtn = giftBottomRightObj.transform.FindChild("ToChargeBtn").GetComponent<UIButtonMessage>();
        toChargeBtn.target = gameObject;
        toChargeBtn.functionName = "OnToChargeBtn";

        UISprite toBuyGiftBtn = giftBottomRightObj.transform.FindChild("ToBuyBtn").GetComponent<UISprite>();
        UILabel toBuyGiftLabel = giftBottomRightObj.transform.FindChild("ToBuyBtn/Label").GetComponent<UILabel>();
        UIButtonMessage toBuyGiftBtnMsg = giftBottomRightObj.transform.FindChild("ToBuyBtn").GetComponent<UIButtonMessage>();
        if (tempVIPValue <= curVIPRJ.vipLv && !curVIPRJ.giftIds.Contains(tempVIPValue.ToString()))
        {
            toBuyGiftBtn.color = Color.gray;
            toBuyGiftLabel.text = TextsData.getData(540).chinese;
            toBuyGiftBtnMsg.target = null;
        }
        else
        {
            toBuyGiftBtn.color = Color.white;
            toBuyGiftLabel.text = TextsData.getData(539).chinese;
            toBuyGiftBtnMsg.target = gameObject;
            toBuyGiftBtnMsg.functionName = "OnToBuyGiftBtn";
            toBuyGiftBtnMsg.param = VipData.getData(tempVIPValue).giftid;
        }

    }

    void OnVIPLeftBtn(int param)
    {
        tempVIPValue = param;
        //SetVIPBtnState(param);
        SetBtnState(vipBtnList, tempVIPValue);
        InitGiftRightRegion();
    }

    void SetVIPBtnState(int lv)
    {
        for (int i = 0; i < vipBtnList.Count; i++)
        {
            int vLevel = vipBtnList[i].GetComponent<UIButtonMessage>().param;
            if (vLevel == lv)
            {
                vipBtnList[i].color = Color.gray;
            }
            else
            {
                vipBtnList[i].color = Color.white;
            }
        }
    }

    void SetBtnState(List<UISprite> sList, int num)
    {
        for (int i = 0; i < sList.Count; i++)
        {
            int temp = sList[i].GetComponent<UIButtonMessage>().param;
            if (temp == num)
            {
                sList[i].color = Color.gray;
            }
            else
            {
                sList[i].color = Color.white;
            }
        }
    }

    void SetLeftScrollbarPos()
    {
        if (curVIPValue <= VipData.vipList.Count - 5)
        {
            int tVIPLv;
            if (curVIPValue == 0)
            {
                tVIPLv = 1;
            }
            else
                tVIPLv = curVIPValue;

            giftLeftScrollbar.value = (float)(0.15f * (tVIPLv - 1) - 0.05f);
            giftLeftScrollbar.ForceUpdate();
        }
        else
            giftLeftScrollbar.value = 1;
    }

    void OnToChargeBtn()
    {
        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
        ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ,
                "ChargePanel") as ChargePanel;
        charge.curRechargeResult = curVIPRJ;
        //如果vipCost是0表示没有充值过，是第一次充值//
        if (curVIPRJ.vipCost == 0)
        {
            charge.firstCharge = 0;
        }
        else
            charge.firstCharge = curVIPRJ.vipCost;
        charge.isShowType = 1;
        charge.show();
    }

    void OnToBuyGiftBtn(int param)
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);

        List<string> curGiftIds = curVIPRJ.giftIds;

        //如果当前vip等级不够来买该礼包//
        if (curVIPValue < tempVIPValue)
        {
            string str = TextsData.getData(243).chinese;
            UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
        }
        //如果vip等级够了，但是不在服务器可以购买礼包的列表中，说明已经买过该礼包了//
        else if (!curGiftIds.Contains(param.ToString()))
        {
            string str = TextsData.getData(542).chinese;
            ToastWindow.mInstance.showText(str);
        }
        //vip等级够了，也在服务器可以购买礼包列表中，向服务器请求购买//
        else
        {
            requestType = 19;
            PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson("", 7, 2, 1, param), this);
        }
    }

    void OnToVIPSpecialBtn()
    {
        giftBottomSpecialObj.SetActive(true);
        InitGiftSpecialRegion();
        UpdateSpecialPanel();
    }

    void InitGiftSpecialRegion()
    {
        UIButtonMessage chargeBtn = giftBottomSpecialObj.transform.FindChild("BackToMainBtn").GetComponent<UIButtonMessage>();
        chargeBtn.target = gameObject;
        chargeBtn.functionName = "OnToChargeBtn";

        giftBottomSpecialObj.transform.FindChild("VIPLabel").GetComponent<UILabel>().text = curVIPRJ.vipLv.ToString();
        giftBottomSpecialObj.transform.FindChild("VIPValue").GetComponent<UILabel>().text = curVIPRJ.vipCost.ToString() + "/" + curVIPRJ.sCost.ToString();
        UISprite specialVIPCrystalPB = giftBottomSpecialObj.transform.FindChild("pbVIPCrystal").GetComponent<UISprite>();
        specialVIPCrystalPB.fillAmount = (float)curVIPRJ.vipCost / curVIPRJ.sCost;
        if (curVIPValue == VipData.vipList.Count - 1)
        {
            giftBottomSpecialObj.transform.FindChild("VIPNextGroup").gameObject.SetActive(false);
        }
        else
        {
            giftBottomSpecialObj.transform.FindChild("VIPNextGroup").gameObject.SetActive(true);
            giftBottomSpecialObj.transform.FindChild("VIPNextGroup/ChargeCount").GetComponent<UILabel>().text = (curVIPRJ.sCost - curVIPRJ.vipCost).ToString();
            giftBottomSpecialObj.transform.FindChild("VIPNextGroup/NextVIPLevel").GetComponent<UILabel>().text = (curVIPRJ.vipLv + 1).ToString();
        }

        gPreBtn = giftBottomSpecialObj.transform.FindChild("PreButton").gameObject;
        gNextBtn = giftBottomSpecialObj.transform.FindChild("NextButton").gameObject;
        UIButtonMessage gPreBtnMsg = gPreBtn.GetComponent<UIButtonMessage>();
        gPreBtnMsg.target = gameObject;
        gPreBtnMsg.functionName = "OnPreBtn";
        UIButtonMessage gNextBtnMsg = gNextBtn.GetComponent<UIButtonMessage>();
        gNextBtnMsg.target = gameObject;
        gNextBtnMsg.functionName = "OnNextBtn";

        tVIPValueLabel = giftBottomSpecialObj.transform.FindChild("tVIPLabel").GetComponent<UILabel>();
        specialDescLabel = giftBottomSpecialObj.transform.FindChild("DescClipPanel/UIGrid/Description").GetComponent<UILabel>();
        preBtnLabel = giftBottomSpecialObj.transform.FindChild("PreButton/Label").GetComponent<UILabel>();
        nextBtnLabel = giftBottomSpecialObj.transform.FindChild("NextButton/Label").GetComponent<UILabel>();

        GameObject closeSpecialObj = giftBottomSpecialObj.transform.FindChild("SpecialCloseButton").gameObject;
        UIButtonMessage closeSpecialObjMsg = closeSpecialObj.GetComponent<UIButtonMessage>();
        closeSpecialObjMsg.target = gameObject;
        closeSpecialObjMsg.functionName = "OnSpecialCloseBtn";
    }

    void OnPreBtn()
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (tempVIPValue > 1)
        {
            tempVIPValue--;
        }
        else
        {
            return;
        }

        UpdateSpecialPanel();
    }

    void OnNextBtn()
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (tempVIPValue == VipData.vipList.Count - 1)
        {
            return;
        }
        else
        {
            tempVIPValue++;
        }
        UpdateSpecialPanel();
    }

    void UpdateSpecialPanel()
    {
        tVIPValueLabel.text = tempVIPValue.ToString();
        VipData tVIPData = VipData.getData(tempVIPValue);

        //Update left region description.
        specialDescLabel.text = tVIPData.description;

        if (tempVIPValue == 1)
        {
            gPreBtn.SetActive(false);
            gNextBtn.SetActive(true);
            nextBtnLabel.text = (tempVIPValue + 1).ToString();
            return;
        }
        else if (tempVIPValue == VipData.vipList.Count - 1)
        {
            gPreBtn.SetActive(true);
            gNextBtn.SetActive(false);
            preBtnLabel.text = (tempVIPValue - 1).ToString();
            return;
        }
        else
        {
            gPreBtn.SetActive(true);
            gNextBtn.SetActive(true);
        }
        preBtnLabel.text = (tempVIPValue - 1).ToString();
        nextBtnLabel.text = (tempVIPValue + 1).ToString();
    }

    void OnSpecialCloseBtn()
    {
        giftBottomSpecialObj.SetActive(false);
    }

    void OnNoticeBtnIcon(int id)
    {
        curNoticeId = id;
        requestType = 22;
        PlayerInfo.getInstance().sendRequest(new ActivityInfoJson(butNowEleId, curNoticeId, 7), this);
    }

    public void cardDetailBack()
    {
        transform.FindChild("Down/LotActivityPanel(Clone)/result/result-lot").gameObject.SetActive(true);
    }

    void InitNoticeContentPanel()
    {
        if (noticeItem != null && naij != null)
        {
            GameObject nRightClip = noticeItem.transform.FindChild("RightClipPanel").gameObject;
            nRightClip.GetComponent<UIPanel>().clipRange = noticeClipPanelVec;
            nRightClip.transform.localPosition = Vector3.zero;
            if (nRightClip.GetComponent<SpringPanel>())
            {
                nRightClip.GetComponent<SpringPanel>().enabled = false;
            }
            UILabel nLabel = noticeItem.transform.FindChild("RightClipPanel/Label").GetComponent<UILabel>();
            nLabel.text = naij.content;
            SetLabelColliderHeight(nRightClip, nLabel);
        }
    }

    //根据文本的高度来更新文本的BoxCollider//
    void SetLabelColliderHeight(GameObject clipPanel, UILabel label)
    {
        //float centerPosY = clipPanel.GetComponent<UIPanel>().clipRange.y;
        //int clipHeight = (int)clipPanel.GetComponent<UIPanel>().clipRange.w;

        int labelHeight = label.height;

        float cy = (float)labelHeight / 2;
        label.GetComponent<BoxCollider>().center = new Vector3(label.GetComponent<BoxCollider>().center.x, -cy, 0);
        label.GetComponent<BoxCollider>().size = new Vector3(label.GetComponent<BoxCollider>().size.x, labelHeight, 0);
    }

}
