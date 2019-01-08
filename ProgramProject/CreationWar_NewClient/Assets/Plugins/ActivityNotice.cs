/// <summary>
/// 内部活动公告
/// </summary>

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ActivityNotice : MonoBehaviour 
{
    public static ActivityNotice instance;
    public UILabel lblActivityTitle;
    public UILabel lblActivityTime;
    public UILabel lblActivityContent;
    public UILabel lblPackContent;
    public UILabel lblTips;
    public UILabel lblTimeCount;
    public UILabel pages;
    public GameObject activityTexture;
    public GameObject iconObj;
    public UIButton btnGoForward;
    private Material mat;

	void Awake () 
	{
        instance = this;
        mat = activityTexture.GetComponent<MeshRenderer>().material;
	}

    void OnEnable()
    {
        StartCoroutine(HideIcon());

        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().AskDynamicActivity());
    }

    IEnumerator HideIcon()
    {
        while (!iconObj.activeSelf)
        {
            yield return null;
        }
        
        iconObj.SetActive(false);
    }

    private List<Dictionary<string, string>> noticeInfo = new List<Dictionary<string, string>>();
    private Dictionary<int, Texture> textures = new Dictionary<int, Texture>();
    void Start()
    {
        pages.text = "0/0";
        InvokeRepeating("UpdateTimeLabel", 0, 1.0f);
    }

    public void GetActivityInfo(Dictionary<string, object> activity)
    {
        noticeInfo.Clear();
        foreach (object obj in activity.Values)
        {
            Dictionary<string, object> mActivity = ((Dictionary<object, object>)obj).DicObjTo<string, object>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("label", (string)mActivity["label"]);
            dic.Add("onoff", (string)mActivity["onoff"]);
            dic.Add("name", (string)mActivity["name"]);
            dic.Add("startime", (string)mActivity["startime"]);
            dic.Add("endtime", (string)mActivity["endtime"]);
            dic.Add("activitycontent", (string)mActivity["activitycontent"]);
            dic.Add("packagecontents", (string)mActivity["packagecontents"]);
            dic.Add("warning", (string)mActivity["warning"]);
            dic.Add("bigicon", (string)mActivity["bigicon"]);
            noticeInfo.Add(dic);
        }

        for (int i = 0; i < noticeInfo.Count; i++)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(DownloadTexture(noticeInfo[i]["bigicon"], i));
            }
        }
        maxPage = noticeInfo.Count;
        pages.text = string.Format("{0}/{1}", Mathf.Clamp((currPage + 1), 1, maxPage), maxPage);
        ShowNoticeInfo(currPage);
    }

    private string currLabel = "";// 当前活动唯一标签
    private DateTime startTime;
    private DateTime endTime;
    private int totalTime;
    private bool isCountDown;
    void ShowNoticeInfo(int mPage)
    {
        currLabel = noticeInfo[mPage]["label"];
        if (noticeInfo[mPage]["onoff"].Equals("1"))
        {
            btnGoForward.isEnabled = true;
        }
        else
        {
            btnGoForward.isEnabled = false;
        }
        lblActivityTitle.text = noticeInfo[mPage]["name"];
        lblActivityTime.text = string.Format("{0}-{1}", noticeInfo[mPage]["startime"], noticeInfo[mPage]["endtime"]);
        lblActivityContent.text = noticeInfo[mPage]["activitycontent"];
        lblPackContent.text = noticeInfo[mPage]["packagecontents"];
        lblTips.text = noticeInfo[mPage]["warning"];

        
        //startTime = DateTime.Parse(noticeInfo[mPage]["startime"]);
        //endTime = DateTime.Parse(noticeInfo[mPage]["endtime"]);

        if (DateTime.TryParse(noticeInfo[mPage]["startime"], out startTime) && DateTime.TryParse(noticeInfo[mPage]["endtime"], out endTime))
        {
            if (InRoom.GetInRoomInstantiate().serverTime > endTime)
            {
                isCountDown = false;
                lblTimeCount.text = StaticLoc.Loc.Get("meg0186");
            }
            else if (InRoom.GetInRoomInstantiate().serverTime < startTime)
            {
                isCountDown = false;
                lblTimeCount.text = StaticLoc.Loc.Get("meg0187");
            }
            else
            {
                isCountDown = true;
                totalTime = (int)(endTime - InRoom.GetInRoomInstantiate().serverTime).TotalSeconds;
            }
        }
        else
        {
            isCountDown = false;
            lblTimeCount.text = "";
        }

        if (gameObject.activeSelf)
        {
            StartCoroutine(WaitShowTexture(mPage));
        }
    }

    IEnumerator WaitShowTexture(int mPage)
    {
        Texture tex = null;
        while (!textures.TryGetValue(mPage, out tex))
        {
            yield return null;
        }

        if (mPage == currPage)
        {
            if (!iconObj.activeSelf)
            {
                iconObj.SetActive(true);
            }
            mat.mainTexture = tex;
        }
    }

    IEnumerator DownloadTexture(string url, int index)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            textures[index] = null;
        }
        else
        {
            textures[index] = www.texture;
        }
    }

    private int day;
    private int hour;
    private int minute;
    void UpdateTimeLabel()
    {
        if (isCountDown)
        {
            totalTime--;
            day = (int)(totalTime / (60 * 60 * 24));
            hour = (int)(totalTime % (60 * 60 * 24) / (60 * 60));
            minute = totalTime % (60 * 60) / 60;

            if (totalTime < 60)
            {
                minute = 1;
            }

            lblTimeCount.text = string.Format("{0}{1}{2}{3}{4}{5}", day, StaticLoc.Loc.Get("meg0188"), hour, StaticLoc.Loc.Get("meg0189"), minute, StaticLoc.Loc.Get("meg0190"));
        }
    }

    /// <summary>
    /// 立即前往
    /// -----------说明---------------------
    /// 成长福利	    跳转成长福利面板
    /// 连续充值奖励	跳转连续充值面板
    /// 冲级返还	    无前往按钮
    /// 每日充值排行	跳转充值面板
    /// 消费活动	    跳转商城面板
    /// 充值返还	    跳转充值面板
    /// ------------------------------------
    /// </summary>
    public void GoForward()
    {
        if (currLabel.Equals("Leveling"))// 冲级活动
        {
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show0", SendMessageOptions.DontRequireReceiver);// 关闭公告活动板
            // 提示：活动已开放，赶快去升级吧！
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0211"));
        }
        else if (currLabel.Equals("Recharge"))// 充值排行（充值我第一）
        {
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show0", SendMessageOptions.DontRequireReceiver);
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StoreMoveOn", SendMessageOptions.DontRequireReceiver);// 打开充值
        }
        else if (currLabel.Equals("Consumption"))// 消费活动
        {
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show0", SendMessageOptions.DontRequireReceiver);
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StoreOpenMoveOn", SendMessageOptions.DontRequireReceiver);// 打开商城
        }
        else if (currLabel.Equals("Returnmoney"))// 充值返还
        {
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show0", SendMessageOptions.DontRequireReceiver);
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StoreMoveOn", SendMessageOptions.DontRequireReceiver);// 打开充值
        }
        else
        {
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("Show9", SendMessageOptions.DontRequireReceiver);
            DynamicActivity.instance.OpenTargetPanel(currLabel);  
        }
    }

    //IEnumerator OpenActivityPanel()
    //{
    //    PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("Show9", SendMessageOptions.DontRequireReceiver);

    //    yield return new WaitForSeconds(1.0f);
    //    DynamicActivity.instance.OpenTargetPanel(currLabel);  
    //}

    private int maxPage = 0;
    private int currPage = 0;
    public void PageDownClick()
    {
        currPage = Mathf.Clamp((++currPage), 0, maxPage - 1);
        pages.text = string.Format("{0}/{1}", (currPage + 1), maxPage);

        ShowNoticeInfo(currPage);
    }

    public void PageUpClick()
    {
        currPage = Mathf.Clamp((--currPage), 0, maxPage - 1);
        pages.text = string.Format("{0}/{1}", (currPage + 1), maxPage);
        ShowNoticeInfo(currPage);
    }
}
