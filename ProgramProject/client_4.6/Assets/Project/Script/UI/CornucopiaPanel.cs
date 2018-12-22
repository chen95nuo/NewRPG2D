using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CornucopiaPanel : MonoBehaviour, ProcessResponse
{
    public UILabel labelTimes;   //倒计时//

    public UILabel labelNotices;   //通知//

    public UILabel labelNotice;   //通知//

    public UILabel labelAffiche;   //通知//

    public UILabel needLabels;

    public UILabel[] labels;

    public UISlider progress;


    public GameObject rewardPanel;

    public GameObject effect;

    public GameObject[] Rewards;

    public GameObject[] skillItems;

    public GameObject[] effects;


    float sliderValue;

    const int UI_CORNUCOPIA = 50;   //聚宝操作//


    bool isReceiveData;
     
    int requestType;

    int id;

    int time;

    public int errorCode;

    int diamond = 0;

    int curTime_h = 1;

    int curTime_m = 1;

    int curTime_s = 1;

    int crestalPay = 0;

    int cost1;

    int cost2;

    int cost3;
    bool isRefresh;

    Hashtable types = new Hashtable();

    bool isHide = true;
    static bool isShow;
    // Use this for initialization
    void Start()
    {
        labelAffiche.text = TextsData.getData(627).chinese;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReceiveData)
        {
            isReceiveData = false;			
            if (isRefresh)
            {
                CancelInvoke("RefreshTimes");
            }
            Refresh();
			
			if(errorCode == -3)
				return;
			
            if (errorCode == 19)
            {
                UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, TextsData.getData(646).chinese, true);
                return;
            }
            else if (errorCode == 112)
            {
                return;
            }
            else if (errorCode == 113)
            {
                ToastWindow.mInstance.showText(TextsData.getData(628).chinese); //活动已结束//
                return;
            }
            else if (errorCode == 114)
            {
                ToastWindow.mInstance.showText(TextsData.getData(629).chinese); //活动未开始//
                return;
            }
            else if (errorCode == 115)
            {

                UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, TextsData.getData(645).chinese + "\n" + TextsData.getData(651).chinese + (CornucopiaData.GetData(id + 1).cost - crestalPay), true); //聚宝盆活动期间充值钻石数量不足，不能参与聚宝活动//
                return;
            }
            else if (errorCode == 0)
            {
                //添加talkingdata,点击聚宝//
                if (!TalkingDataManager.isTDPC)
                {
                    TDGAItem.OnPurchase("cornucopia", 1, diamond);
                }
            }
        }
        effect.SetActive(isShow);
        if (UIJumpTipManager.mInstance.gameObject.activeSelf || ToastWindow.mInstance.gameObject.activeSelf || rewardPanel.activeSelf || !isHide)
        {
            isShow = false;
        }
        else
        {
            isShow = true;
        }
    }


    void AccumulationBtnClick(int param)
    {
        switch (param)
        {
            case 0:
                CornucopiaData t = CornucopiaData.GetData(id + 1);
                diamond = t.cost;
                isShow = false;
                PlayerInfo.getInstance().sendRequest(new UIJson(UI_CORNUCOPIA, 6), this);
                break;  //聚宝//
            case 1:
                rewardPanel.SetActive(true); SetReward(); labelNotice.text = TextsData.getData(625).chinese;
                isShow = false;
                break;  //查看规则//
        }
    }

    public void SetId(int id, int time)
    {
        this.id = id;
        this.time = time;
        Refresh();
    }
    void ItemClick(int param)
    {
        if (RewardsDatasControl.mInstance.gameObject.activeSelf)
        {
            for (int i = 0; i < skillItems.Length; i++)
            {
                if (id - i > 0)
                    effects[i].SetActive(true);
                else
                    effects[i].SetActive(false);
            }
            RewardsDatasControl.mInstance.hide();
        }
        else
        {
            ShowReward(param);
            for (int i = 0; i < skillItems.Length; i++)
            {
                if (param == skillItems[i].GetComponent<UIButtonMessage_Press>().param)
                {
                    effects[i].SetActive(false);
                }
                else
                {
                    if (id - i > 0)
                        effects[i].SetActive(true);
                    else
                        effects[i].SetActive(false);
                }
            }
        }
    }

    public void SetEc(bool isshow)
    {
        isHide = isshow;
    }
    public bool IsShow()
    {
        if (id < 1)
        {
            return false;
        }
        else
            return true;
    }
    public void Refresh()
    {
        types.Clear();
        isShow = IsShow();

        //int labelsNum1 = 0;
        //int labelsNum2 = 0;
        //int labelsNum3 = 0;
        for (int i = 0; i < skillItems.Length; i++)
        {
            CornucopiaData data = CornucopiaData.GetData(i + 1);
            string str = data.dayawardstype_itemId[7];
            string[] ss = str.Split('-');

            string[] ss1 = ss[1].Split(',');
            int type = StringUtil.getInt(ss[0]);

            int rewardId = StringUtil.getInt(ss1[0]);
            //int num = StringUtil.getInt(ss1[1]);
            SimpleCardInfo2 sc = skillItems[i].GetComponent<SimpleCardInfo2>();
            types.Add(rewardId, type);
            skillItems[i].GetComponent<UIButtonMessage_Press>().param = rewardId;

            labels[i].text = "x " + data.cost;
            UILabel label = sc.transform.FindChild("Label").GetComponent<UILabel>();
            switch (type)
            {
                case 1:
                    sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Item);
                    label.text = GetName(rewardId, GameHelper.E_CardType.E_Item);
                    break;
                case 2:
                    sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Equip);
                    label.text = GetName(rewardId, GameHelper.E_CardType.E_Equip);
                    break;
                case 3:
                    sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Hero);
                    label.text = GetName(rewardId, GameHelper.E_CardType.E_Hero);
                    break;
                case 4:
                    sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Skill);
                    label.text = GetName(rewardId, GameHelper.E_CardType.E_Skill);
                    break;
                case 5:
                    sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_PassiveSkill);
                    label.text = GetName(rewardId, GameHelper.E_CardType.E_PassiveSkill);
                    break;
            }


            if (id - i > 0)
                effects[i].SetActive(true);
            else
                effects[i].SetActive(false);

        }
        isRefresh = true;
        float a = 0;
        switch (id)
        {
            case 0:
                a = 0f;
                break;
            case 1:
                a = 0.1f;
                break;
            case 2:
                a = 0.5f;
                break;
            case 3:
                a = 1f;
                break;
        }
        progress.value = a;
        if (time == -1)
        {
            labelTimes.text = TextsData.getData(631).chinese;
        }
        else if (time == 0)
        {
            labelTimes.text = TextsData.getData(631).chinese;
        }
        else
        {
            CancelInvoke("RefreshTimes");
            InvokeRepeating("RefreshTimes", 0, 1.0f);
        }
        this.transform.FindChild("AccumulationBtn/Background").GetComponent<UISprite>().color = id == 3 || errorCode == 113 || errorCode == 114 ? Color.gray : Color.white;
        this.transform.FindChild("AccumulationBtn").GetComponent<UIButtonMessage>().enabled = id == 3 ? false : true;

        HeadUI.mInstance.refreshPlayerInfo();
    }

    public string GetName(int CardId, GameHelper.E_CardType type)
    {
        string name = "";
        switch (type)
        {
            case GameHelper.E_CardType.E_Hero:
                {
                    CardData cd = CardData.getData(CardId);
                    if (cd == null)
                        return "";
                    name = cd.name;
                } break;
            case GameHelper.E_CardType.E_Equip:
                {
                    EquipData ed = EquipData.getData(CardId);
                    if (ed == null)
                        return "";
                    name = ed.name;
                } break;
            case GameHelper.E_CardType.E_Item:
                {
                    ItemsData itemData = ItemsData.getData(CardId);
                    if (itemData == null)
                        return "";
                    name = itemData.name;
                } break;
            case GameHelper.E_CardType.E_Skill:
                {
                    SkillData sd = SkillData.getData(CardId);
                    if (sd == null)
                        return "";
                    name = sd.name;
                } break;
            case GameHelper.E_CardType.E_PassiveSkill:
                {
                    PassiveSkillData psd = PassiveSkillData.getData(CardId);
                    if (psd == null)
                        return "";
                    name = psd.name;
                } break;
        }
        return name;
    }
    void RefreshTimes()
    {
        if ((curTime_s + curTime_m + curTime_h) == 0)
        {
            CancelInvoke("RefreshTimes");
            labelTimes.text = TextsData.getData(628).chinese;
        }
        else
        {
            curTime_h = time / 3600;
            curTime_m = (time % 3600) / 60;
            curTime_s = time % 60;
            time--;
            labelTimes.text = TextsData.getData(626).chinese + (curTime_h < 10 ? "0" + curTime_h :  curTime_h.ToString()) + "时" + (curTime_m < 10 ? "0" + curTime_m : curTime_m.ToString()) + "分" + (curTime_s < 10 ? "0" + curTime_s : curTime_s.ToString()) + "秒";
        }
    }
    void SetReward()
    {
		Object Item = Resources.Load("Prefabs/UI/ActivityPanel/CardInfoItem");
        for (int i = 0; i < Rewards.Length; i++)
        {
            int k = 1;
            CornucopiaData data = CornucopiaData.GetData(i + 1);
            for (int c = data.dayawardstype_itemId.Count - 1; c >= 0; c--)
            {
                k++;
                string str = data.dayawardstype_itemId[c];
                string[] ss = str.Split('-');

                string[] ss1 = ss[1].Split(',');
                int type = StringUtil.getInt(ss[0]);

                int rewardId = 0;
                int num = 0;

                string text = i == 0 ? TextsData.getData(635 + k).chinese : "";
                if (type > 5)
                {
                    num = StringUtil.getInt(ss[1]);
                }
                else
                {
                    rewardId = StringUtil.getInt(ss1[0]);
                    num = StringUtil.getInt(ss1[1]);
                }


                GameObject RewardItem = (GameObject)Instantiate(Item);
                GameObjectUtil.gameObjectAttachToParent(RewardItem, Rewards[i]);
                RewardItem.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                //UIDraggablePanel draggablePanel = RewardItem.GetComponent<UIDraggablePanel>();
                SimpleCardInfo2 sc = RewardItem.GetComponent<SimpleCardInfo2>();
                GetItemNum gn = RewardItem.GetComponent<GetItemNum>();
                sc.clear();
                switch (type)
                {
                    case 1:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Item);
                        break;
                    case 2:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Equip);
                        break;
                    case 3:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Hero);
                        break;
                    case 4:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_Skill);
                        break;
                    case 5:
                        sc.setSimpleCardInfo(rewardId, GameHelper.E_CardType.E_PassiveSkill);
                        break;
                    case 6:
                        GetDiamondReward(RewardItem, 1);
                        break;
                    case 8:
                        GetDiamondReward(RewardItem, 0);
                        break;
                }
                gn.GetNum(num, text);
            }
			

        }
        for (int i = 0; i < Rewards.Length; i++)
        {
            Rewards[i].GetComponent<UIGrid>().repositionNow = true;
        }

        Item = null;
		Resources.UnloadUnusedAssets();
    }
    void GetDiamondReward(GameObject obj, int i)
    {
        obj.transform.FindChild("Child/HeroIcon").gameObject.SetActive(false);
        obj.transform.FindChild("Child/OtherIcon").gameObject.SetActive(false);
        obj.transform.FindChild("Child/Frame").gameObject.SetActive(false);

        obj.transform.FindChild("Child/BG").gameObject.SetActive(false);

        if (i == 0)
            obj.transform.FindChild("Child/DiamondIcon").gameObject.SetActive(true);
        else
            obj.transform.FindChild("Child/GoldIcon").gameObject.SetActive(true);
    }
    void RewardPanelCloseClick(int param)
    {
        rewardPanel.SetActive(false);

        for (int i = 0; i < Rewards.Length; i++)
        {
            GameObjectUtil.destroyGameObjectAllChildrens(Rewards[i]);
        }
    }

    //显示物品信息//
    public void ShowReward(int param)
    {

        //string iconName = "";
        string frameName = "";
        string name = "";
        string des = "";
        int sell = 0;
        int formID = (int)types[param];
        int level = 0;
        int type = 0;
        //int star = 0;

        GameHelper.E_CardType cardType = GameHelper.E_CardType.E_Null;


        switch (formID)
        {
            case 1:				//items//
                ItemsData item = ItemsData.getData(param);
                if (item == null)
                    return;
                cardType = GameHelper.E_CardType.E_Item;
                formID = param;
                //star = item.star;
                //iconName = item.icon;
                name = item.name;
                des = item.discription;
                sell = item.sell;
                break;
            case 2:				//equip//
                EquipData ed = EquipData.getData(param);
                if (ed == null)
                    return;
                cardType = GameHelper.E_CardType.E_Equip;
                formID = param;
                //star = ed.star;
                //iconName = ed.icon;
                name = ed.name;
                des = ed.description;
                sell = ed.sell;
                level = 1;
                break;
            case 3:				//card//
                CardData cd = CardData.getData(param);
                if (cd == null)
                    return;
                cardType = GameHelper.E_CardType.E_Hero;
                formID = param;
                //star = cd.star;
                //iconName = cd.icon;
                name = cd.name;
                des = cd.description;
                sell = cd.sell;
                break;
            case 4:				//skill//
                SkillData sd = SkillData.getData(param);
                if (sd == null)
                    return;
                cardType = GameHelper.E_CardType.E_Skill;
                formID = param;
                //star = sd.star;
                //iconName = sd.icon;
                name = sd.name;
                if (sd.exptype == 2)
                {
                    des = sd.description;
                }
                else
                {
                    des = Statics.getSkillValueForUIShow(sd.index, 1);
                }

                sell = sd.sell;
                break;
            case 5:				//passiveSkill//
                PassiveSkillData psd = PassiveSkillData.getData(param);
                if (psd == null)
                    return;
                cardType = GameHelper.E_CardType.E_PassiveSkill;
                formID = param;
                //star = psd.star;
                //iconName = psd.icon;
                name = psd.name;
                des = psd.describe;
                sell = psd.sell;
                break;
        }

        RewardsDatasControl.mInstance.SetData(formID, cardType, name, frameName, des, level, sell, type);
        for (int i = 0; i < skillItems.Length; i++)
        {
            if (param == skillItems[i].GetComponent<UIButtonMessage_Press>().param)
            {
                Vector3 vt = skillItems[i].transform.position;
                RewardsDatasControl.mInstance.transform.position = new Vector3(vt.x, vt.y + 0.4f, 0f);
                RewardsDatasControl.mInstance.transform.localScale = new Vector3(0.76f, 0.76f, 0.76f);
            }
        }

    }

    public void receiveResponse(string json)
    {
        if (json != null)
        {
            //关闭连接界面的动画//
            PlayerInfo.getInstance().isShowConnectObj = false;
            isReceiveData = true;
            CornucopiaResultJson irj = JsonMapper.ToObject<CornucopiaResultJson>(json);
            errorCode = irj.errorCode;

            this.crestalPay = irj.crestalPay;
            this.time = irj.time;
            if (errorCode == 0)
            {
                this.id = irj.id;
            }
        }
    }
}
