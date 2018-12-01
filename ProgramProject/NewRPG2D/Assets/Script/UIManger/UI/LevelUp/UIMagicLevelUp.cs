using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using System;

public class UIMagicLevelUp : TTUIPage
{
    public static UIMagicLevelUp instance;
    public UIMagicGrid[] grids;
    public UIMagicGrid levelUpGrid;

    public Text txt_DownTip;
    public Text txt_SkilName;
    public Text txt_SkilInfoTip;
    public Text txt_Level;
    public Text txt_TimeTip;
    public Text txt_BtnTip;
    public Text txt_NeedDiamonds;

    public Button btn_Close;
    public Button btn_Back;
    public Button btn_SpeedUp;
    public GameObject LevelUp;

    public int currentLevelID = 0;

    private void Awake()
    {
        instance = this;

        HallEventManager.instance.AddListener<MagicLevelUpHelper>(HallEventDefineEnum.MagicLevelUp, LevelUpCallBack);
        btn_SpeedUp.onClick.AddListener(ChickSpeedUp);
        btn_Close.onClick.AddListener(ClosePage);
        btn_Back.onClick.AddListener(ClosePage);
    }

    private void ChickSpeedUp()
    {

    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<MagicLevelUpHelper>(HallEventDefineEnum.MagicLevelUp, LevelUpCallBack);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        RoomMgr room = mData as RoomMgr;
        UpdateInfo(room.currentBuildData);

        MagicLevelUpHelper helper = MagicDataMgr.instance.GetLevelUpData;
        if (helper != null)
        {
            LevelUp.SetActive(true);
            txt_DownTip.gameObject.SetActive(false);
            LevelUpCallBack(helper);
        }
        else
        {
            LevelUp.SetActive(false);
            txt_DownTip.gameObject.SetActive(true);
        }
    }

    public void UpdateInfo(LocalBuildingData room)
    {
        int workShopLevel = CheckPlayerInfo.instance.GetBuildingLevel(BuildRoomName.MagicWorkShop);
        for (int i = 0; i < (int)MagicName.Max; i++)
        {
            MagicData data = MagicDataMgr.instance.GetMagicLevel((MagicName)i);
            data = MagicDataMgr.instance.GetMagic(data.magicName, data.level + 1);
            Debug.Log(data.magicName.ToString());
            if (data.needWorkLevel > workShopLevel)
            {
                //如果技能要求等级超过房间等级 锁定 需要房间升级
                grids[i].UpdateLevelUpInfo(data, MagicGridType.NeedLevel);
            }
            else if (data.needLevel > workShopLevel)
            {
                //技能解锁但是下一等级没有解锁
                grids[i].UpdateLevelUpInfo(data, MagicGridType.NeedLevelUp);
            }
            else
            {
                //可升级
                grids[i].UpdateLevelUpInfo(data);
            }
        }
    }

    public void ChickLevelUpInfo()
    {
        MagicLevelUpHelper magicData = MagicDataMgr.instance.GetLevelUpData;
        MagicData nextMagic = MagicDataMgr.instance.GetXmlDataByItemId<MagicData>(magicData.id);
        MagicData nowMagic = MagicDataMgr.instance.GetMagic(nextMagic.magicName, nextMagic.level - 1);
        levelUpGrid.UpdateInfo(nowMagic);
        txt_Level.text = nowMagic.level.ToString();
        txt_SkilName.text = LanguageDataMgr.instance.GetString(nowMagic.magicName.ToString());
        string st = LanguageDataMgr.instance.GetString("Info_" + nowMagic.magicName.ToString());
        string number = nowMagic.param + "(~<color=#aae035>" + nextMagic.param + "</color>)";
        txt_SkilInfoTip.text = string.Format(st, number);
        int time = magicData.time;
        string timeTip = "制作时间 " + SystemTime.instance.TimeNormalized(time);
        txt_TimeTip.text = timeTip;
        txt_NeedDiamonds.text = GameHelper.instance.GetNeedDiamondsOfTime(time).ToString();
        currentLevelID = magicData.id;
    }
    public void LevelUpCallBack(MagicLevelUpHelper magicData)
    {
        if (magicData.id != currentLevelID)
        {
            ChickLevelUpInfo();
        }
        int time = magicData.time;
        string timeTip = "制作时间 " + SystemTime.instance.TimeNormalized(time);
        txt_TimeTip.text = timeTip;
        txt_NeedDiamonds.text = GameHelper.instance.GetNeedDiamondsOfTime(time).ToString();
    }
}
