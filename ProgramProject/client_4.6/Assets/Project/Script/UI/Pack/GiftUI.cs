using UnityEngine;
using System.Collections;

public class GiftUI : MonoBehaviour, ProcessResponse
{
    public GiftRewardResultJson gtrj;

    public MainMenuManager main;

    public int id;

    public int onlineTime;


    public UIAtlas itemCircleAtals;

    public UIAtlas equipCircleAtals;

    public UIAtlas heroCircleAtlas;

    public UIAtlas skillCircleAtals;

    public UIAtlas pSkillCircleAtals;

    public UIAtlas Atals01;

    public UIAtlas Atals04;

    private bool receiveData;

    private bool isBeginCount = false;

    public int updateTime;

    public int errorCode;

    public int curTime_h;

    public int curTime_m;

    public int curTime_s;

    public int nextGift;

    public int nextOnline;

    public int times;
    // Use this for initialization


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (receiveData)
        {
            if (onlineTime == 0)
            {

                HeadUI.mInstance.requestPlayerInfo();
            }
            if (isBeginCount)
            {
                GiftsReach();
            }
            else
            {
                GiftsNoReach();
            }
            receiveData = false;
            if (errorCode == -3)
                return;
        }
        if (!isBeginCount)
        {
            CancelInvoke("Refresh");
            InvokeRepeating("Refresh", 0, 1.0f);
            isBeginCount = true;
        }
    }


    public void Refresh()
    {
        //GiftData gt = GiftData.getData(id);
        if ((curTime_s + curTime_m + curTime_h) == 0)
        {
            CancelInvoke("Refresh");
            this.transform.FindChild("time/Label").gameObject.SetActive(false);
        }
        else
        {
            if (onlineTime > 0)
            {
                onlineTime--;
                LineRefresh();
            }
            string cts, ctm, cth;
            if (curTime_s < 10)
                cts = "0" + curTime_s;
            else
                cts = curTime_s.ToString();
            if (curTime_m < 10)
                ctm = "0" + curTime_m;
            else
                ctm = curTime_m.ToString();
            if (curTime_h < 10)
                cth = "0" + curTime_h;
            else
                cth = curTime_h.ToString();
            this.transform.FindChild("time/Label").gameObject.SetActive(true);
            this.transform.FindChild("time/Label").GetComponent<UILabel>().text = cth + ":" + ctm + ":" + cts + TextsData.getData(444).chinese;
        }
    }

    public void SetReward(GameObject go, int i, string rewards)
    {
        int type = i;
        string N = rewards;
        int reward = 0, d = 0;
        string[] s = rewards.Split(',');
        if (rewards != null && i != 6 && i != 7)
        {
            string rw = "";
            string rd = "";
            if (s.Length > 1)
            {
                rw = s[0];
                rd = s[1];
                d = StringUtil.getInt(rd);
            }
            reward = StringUtil.getInt(rw);
        }
        UISprite sprite = go.transform.FindChild("Icon").GetComponent<UISprite>();
        sprite.spriteName = "";
        UILabel label = go.transform.FindChild("Text").GetComponent<UILabel>();
        label.text = "";

        SimpleCardInfo2 sc = go.transform.FindChild("CardSample1").GetComponent<SimpleCardInfo2>();
        switch (type)
        {
            case 1:
                //  go.transform.FindChild("CardSample1").gameObject.SetActive(true);
                //sprite.atlas = itemCircleAtals;
                ItemsData id = ItemsData.getData(reward);
                //sprite.spriteName = id.icon;
                sc.setSimpleCardInfo(reward, GameHelper.E_CardType.E_Item);
                label.text = id.name + " X " + d;
                break;
            case 2:
                // go.transform.FindChild("CardSample1").gameObject.SetActive(true);
                //sprite.atlas = equipCircleAtals;
                EquipData ed = EquipData.getData(reward);
                //sprite.spriteName = ed.icon;
                sc.setSimpleCardInfo(reward, GameHelper.E_CardType.E_Equip);
                label.text = ed.name + " X " + d;
                break;
            case 3:
                // go.transform.FindChild("CardSample1").gameObject.SetActive(true);
                //sprite.atlas = heroCircleAtlas;
                CardData cd = CardData.getData(reward);
                //sprite.spriteName = cd.icon;
                sc.setSimpleCardInfo(reward, GameHelper.E_CardType.E_Hero);
                label.text = cd.name + " X " + d;
                break;
            case 4:
                // go.transform.FindChild("CardSample1").gameObject.SetActive(true);
                //sprite.atlas = skillCircleAtals;
                SkillData sd = SkillData.getData(reward);
                //sprite.spriteName = sd.icon;
                sc.setSimpleCardInfo(reward, GameHelper.E_CardType.E_Skill);
                label.text = sd.name + " X " + d;
                break;
            case 5:
                // go.transform.FindChild("CardSample1").gameObject.SetActive(true);
                //sprite.atlas = pSkillCircleAtals;
                //PassiveSkillData psd = PassiveSkillData.getData(reward);
                //sprite.spriteName = psd.icon;
                sc.setSimpleCardInfo(reward, GameHelper.E_CardType.E_PassiveSkill);
                label.text = " X " + d;
                break;
            case 6:
                //sprite.atlas = pSkillCircleAtals;
                //PassiveSkillData psd = PassiveSkillData.getData(i);
                go.transform.FindChild("CardSample1").gameObject.SetActive(false);
                sprite.spriteName = "reward_gold";
                label.text = " X " + N;
                break;
            case 7:
                sprite.atlas = pSkillCircleAtals;
                //PassiveSkillData psd = PassiveSkillData.getData(i);
                //sprite.spriteName = psd.icon;
                go.transform.FindChild("CardSample1").gameObject.SetActive(false);
                label.text = " X " + N;
                break;
            case 8:
                //sprite.atlas = pSkillCircleAtals;
                //PassiveSkillData psd = PassiveSkillData.getData(i);
                go.transform.FindChild("CardSample1").gameObject.SetActive(false);
                sprite.gameObject.SetActive(true);
                sprite.spriteName = "reward_crystal";
                label.text = " X " + N;
                break;
            case 9:
                sprite.atlas = Atals01;
                //PassiveSkillData psd = PassiveSkillData.getData(i);
                sprite.spriteName = "rune";
                sprite.gameObject.SetActive(true);
                go.transform.FindChild("CardSample1").gameObject.SetActive(false);
                label.text = TextsData.getData(660).chinese + " X " + N;
                break;
            case 12:
                sprite.atlas = itemCircleAtals;

                //PassiveSkillData psd = PassiveSkillData.getData(i);
                sprite.gameObject.SetActive(true);
                sprite.spriteName = "homC2";
                go.transform.FindChild("CardSample1").gameObject.SetActive(false);
                label.text = TextsData.getData(714).chinese + " X " + N;
                break;
            case 13:
                sprite.atlas = Atals01;
                //PassiveSkillData psd = PassiveSkillData.getData(i);
                sprite.gameObject.SetActive(true);
                sprite.spriteName = "jingangxin";
                go.transform.FindChild("CardSample1").gameObject.SetActive(false);
                label.text = TextsData.getData(749).chinese + " X " + N;
                break;
        }
    }

    public void GiftsReach()
    {
        //能够领取//
        GiftData gt = GiftData.getData(id);
        this.transform.FindChild("TipReward0").gameObject.SetActive(true);
        SetReward(this.transform.FindChild("TipReward0").gameObject, gt.rewardtyp1, gt.reward1);
        if (gt.rewardtyp2 != 0)
        {
            this.transform.FindChild("TipReward1").gameObject.SetActive(true);
            SetReward(this.transform.FindChild("TipReward1").gameObject, gt.rewardtyp2, gt.reward2);
        }
        this.transform.FindChild("Status").GetComponent<UILabel>().text = TextsData.getData(403).chinese;

        if (gt.rewardtyp3 != 0)
        {
            this.transform.FindChild("TipReward2").gameObject.SetActive(true);
            SetReward(this.transform.FindChild("TipReward2").gameObject, gt.rewardtyp3, gt.reward3);
        }
        if (!TalkingDataManager.isTDPC)
        {
            if (gt.reward1 != null)
            {
                //				if(gt.rewardtyp1 == 6)//金币//
                //				TDGAVirtualCurrency.OnReward(StringUtil.getInt(gt.reward1),"Gift"+TextsData.getData(58).chinese);
                if (gt.rewardtyp1 == 8)//钻石//
                    TDGAVirtualCurrency.OnReward(StringUtil.getInt(gt.reward1), "Gift" + TextsData.getData(48).chinese);
            }
            if (gt.reward2 != null)
            {
                //				if(gt.rewardtyp2 == 6)//金币//
                //				TDGAVirtualCurrency.OnReward(StringUtil.getInt(gt.reward2),"Gift"+TextsData.getData(58).chinese);
                if (gt.rewardtyp2 == 8)//钻石//
                    TDGAVirtualCurrency.OnReward(StringUtil.getInt(gt.reward2), "Gift" + TextsData.getData(48).chinese);
            }
            if (gt.reward3 != null)
            {
                //				if(gt.rewardtyp3 == 6)//金币//
                //				TDGAVirtualCurrency.OnReward(StringUtil.getInt(gt.reward3),"Gift"+TextsData.getData(58).chinese);
                if (gt.rewardtyp3 == 8)//钻石//
                    TDGAVirtualCurrency.OnReward(StringUtil.getInt(gt.reward3), "Gift" + TextsData.getData(48).chinese);
            }
        }
    }
    public void GiftsNoReach()
    {
        //领取时间冷却//
        GiftData gtr = GiftData.getData(id);
        this.transform.FindChild("TipReward0").gameObject.SetActive(true);
        SetReward(this.transform.FindChild("TipReward0").gameObject, gtr.rewardtyp1, gtr.reward1);
        if (gtr.rewardtyp2 != 0)
        {
            this.transform.FindChild("TipReward1").gameObject.SetActive(true);
            SetReward(this.transform.FindChild("TipReward1").gameObject, gtr.rewardtyp2, gtr.reward2);
        }
        if (gtr.rewardtyp3 != 0)
        {
            this.transform.FindChild("TipReward2").gameObject.SetActive(true);
            SetReward(this.transform.FindChild("TipReward2").gameObject, gtr.rewardtyp3, gtr.reward3);
        }
        this.transform.FindChild("Status").GetComponent<UILabel>().text = TextsData.getData(404).chinese;
        this.transform.FindChild("time").gameObject.SetActive(true);
        isBeginCount = false;
    }
    public void show()
    {
        UIJson uijson = new UIJson();
        Main3dCameraControl.mInstance.SetBool(true);
        uijson.UIJsonForGift(STATE.UI_Gift, id);
        PlayerInfo.getInstance().sendRequest(uijson, this);
    }

    public void ConfirmGetClick()
    {

        if (receiveData)
        {
            main.SendToGetData();
        }
        HeadUI.mInstance.refreshPlayerInfo();
        Main3dCameraControl.mInstance.SetBool(false);
        UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GIFT);
    }

    public void LineRefresh()
    {
        curTime_s = onlineTime % 60;
        curTime_m = onlineTime / 60;
        if (curTime_s >= 3600)
        {
            curTime_m = 59;
            if (curTime_m > 60)
            {
                curTime_h = curTime_m / 60;
            }
        }
        if (curTime_s < 0)
        {
            curTime_s = 0;
        }
    }
    public void receiveResponse(string json)
    {
        if (json != null)
        {
            //关闭连接界面的动画//
            PlayerInfo.getInstance().isShowConnectObj = false;
            Debug.Log("gift result json:" + json);
            GiftRewardResultJson grj = JsonMapper.ToObject<GiftRewardResultJson>(json);
            errorCode = grj.errorCode;
            onlineTime = grj.linetime;
            nextOnline = grj.nextOnline;
            main.onlineTime = grj.linetime - 1;
            main.gtRJ = gtrj;
            main.LineRefresh();
            if (grj.nextGift != 0)
            {
                main.giftId = grj.nextGift;
                if (grj.linetime == 0)
                {
                    main.onlineTime = nextOnline;
                    main.nextGift = grj.nextGift;
                    main.OpenTimes();
                }
            }
            if (errorCode == 0)
            {
                gtrj = grj;
                nextGift = grj.nextGift;
            }
            if (errorCode == 99)
            {
                main.nextGift = grj.nextGift;
            }
            if (errorCode != 59)
            {
                receiveData = true;
            }
            if (grj.linetime == 0)
            {
                isBeginCount = true;
            }
            else
                isBeginCount = false;
            LineRefresh();

        }

    }
}
