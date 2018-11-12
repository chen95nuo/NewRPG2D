using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMagicMessage : TTUIPage
{
    public static UIMagicMessage instance;

    public Text txt_Tip_1;

    public Text txt_LevelTip;
    public Text txt_DownTip;

    public Text txt_Name;
    public Text txt_Message;

    public Text txt_NeedMana;
    public Text txt_NeedTime;

    public Button btn_LevelUp;
    public Button btn_Close;

    public Image Icon;
    public Image IconBG;
    public Image Lock;
    public Material gray;
    public GameObject levelUp;
    public ContentSizeFitter mainFitter;

    public bool isCryNewMagic = false;

    public int currentMagic;

    private void Awake()
    {
        instance = this;
        btn_LevelUp.onClick.AddListener(ChickLevelUp);
        btn_Close.onClick.AddListener(ClosePage);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
    }

    private void ChickLevelUp()
    {
        Debug.Log("升级");
        if (isCryNewMagic)
        {
            MagicDataMgr.instance.CryNewMagic(currentMagic);
            UIMagicWorkShop.instance.UpdateWorkMagic();
        }
        else
        {
            MagicDataMgr.instance.MagicLevelUp(currentMagic);
            UIPanelManager.instance.ShowPage<UIMagicLevelUp>();
        }
        ClosePage();
    }

    /// <summary>
    /// 信息收取
    /// </summary>
    /// <param name="data">当前技能信息</param>
    /// <param name="isCryPage">是否显示升级窗口</param>
    /// <param name="st">下方信息提示框</param>
    public void UpdateInfo(MagicData data, string st = "", bool isCryPage = false, bool isLock = false, bool isCryNewMagic = false)
    {
        currentMagic = data.ItemId;
        this.isCryNewMagic = isCryNewMagic;
        txt_Name.text = data.magicName.ToString();
        string message = string.Format(LanguageDataMgr.instance.GetString("Info_" + data.magicName), "<color=#77ff58>", "</color>");
        txt_Message.text = message;
        string level = string.Format(LanguageDataMgr.instance.GetString("Tip_Level"));
        txt_LevelTip.text = level + data.level;
        //Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.magicName.ToString());
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon("技能1");

        ShowDownTip(st);
        ShowLevelUp(data, isCryPage);
        ShowLock(isLock);
        mainFitter.SetLayoutVertical();
    }

    public void ShowLock(bool isLock)
    {
        Lock.enabled = isLock;
        IconBG.material = isLock ? gray : null;
        Icon.material = isLock ? gray : null;
    }
    public void ShowDownTip(string st)
    {
        //是否显示下方提示框
        txt_DownTip.gameObject.SetActive(st == "" ? false : true);
        txt_DownTip.text = st;
    }
    public void ShowLevelUp(MagicData data, bool isLevelUp)
    {
        //要不要显示升级
        levelUp.SetActive(isLevelUp);
        if (isLevelUp)
        {
            txt_NeedMana.text = data.produceNeed.ToString();
            int time = isCryNewMagic ? data.produceTime : data.levelUpTime;
            string stime = SystemTime.instance.TimeNormalizedOfMin(time);
            txt_NeedTime.text = stime;
        }
        txt_Tip_1.text = isCryNewMagic ? "制作" : "升级";
    }
}
