using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OpenChest : MonoBehaviour {

	public static OpenChest my;
    public UILabel lblNum;
    private Warnings warnings;

    public OpenChestsBox[] boxes;

    private int[] openBoxesTime;

    public int[] OpenBoxesTime
    {
        get { return openBoxesTime; }
        set { openBoxesTime = value; }
    }

    private yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("","");
	void Awake()
	{
		my=this;

        isFirst = true;
	}

    void Start()
    {
		invCL = PanelStatic.StaticBtnGameManager.invcl;
		warnings=PanelStatic.StaticWarnings;
        yt.Rows = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytTablePacks.SelectRowsList("Name", "VIP");
        //Debug.Log("---------------------每日开启:" + yt.Rows.Count);
    }

    /// <summary>
    /// 是否是第一次打开宝箱面板,这个是为了防止切换界面时倒计时重置
    /// </summary>
    private bool isFirst = false;
    private int tempTime = 0;
    void OnEnable()
    {
        GetTimeFromSever();
        //ReadInfo();

        //ReceiveOpenBox();

        if(isFirst)
        {
//            Debug.Log("1在线宝箱--------------------------" + BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText);
            int onlineChestTime = int.Parse(BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText);
            DateTime logonTime;
            if(DateTime.TryParse(BtnGameManager.yt[0]["LogonTime"].YuanColumnText, out logonTime))
            {
                tempTime = ((int)(InRoom.GetInRoomInstantiate().serverTime - logonTime).TotalSeconds);
                onlineChestTime += tempTime;
            }
            
            BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText = onlineChestTime.ToString();
//            Debug.Log("2在线宝箱--------------------------" + BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText);

            isFirst = false;
        }
    }

    /// <summary>
    /// 从服务器获取开宝箱倒计时的初始时间
    /// </summary>
    public void GetTimeFromSever()
    {
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().UseMoney(yuan.YuanPhoton.UseMoneyType.OpenChest, 0, 0, "-1"));// -1表示获取开宝箱时间
    }

    void OnDestroy()
    {
        int onlineChestTime = int.Parse(BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText);
        onlineChestTime -= tempTime;
        BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText = onlineChestTime.ToString();
    }
	
    private int openNum = 0;
    public void ReadInfo()
    {
        //openNum = int.Parse(BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText) - int.Parse(BtnGameManager.yt.Rows[0]["NumOpenBox"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["NumOpenBox"].YuanColumnText) + 1;
        //Debug.Log("openNum:" + openNum);
        //lblNum.text = openNum.ToString();
    }

   private System.Random ran=new System.Random((int)System.DateTime.Now.Ticks);
   private int ranNum = 1;
   private int bloodNum = 0;
   private yuan.YuanMemoryDB.YuanRow yr;
   private string strInfo = string.Empty;
   private string[] listItem;
   private string[] listItemID;
   private yuan.YuanMemoryDB.YuanRow rowItemID;
   private string numItem = "0";
   private int numPos = 0;
   public void BtnChestClick(GameObject mObj)
   {
        int boxNum = (int)Enum.Parse(typeof(BoxNumber), mObj.name);
//        Debug.Log("wei-----------------BtnChestClick-----------" + boxNum);
        PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().UseMoney (yuan.YuanPhoton.UseMoneyType.OpenChest,0,0, boxNum.ToString()));
   }

    /// <summary>
    /// 更新所有宝箱状态和时间
    /// </summary>
   public void ReceiveOpenBox()
   {
       string openBoxInfo = BtnGameManager.yt[0]["OpenedChests"].YuanColumnText;
       int time = int.Parse(BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText);
       //Debug.Log(string.Format("wei*****************OpenedChests:{0},OnlineChestsTime:{1}", openBoxInfo, time));

       string[] openBoxes = openBoxInfo.Split(',');

       for (int i = 0; i < boxes.Length; i++)
       {
           boxes[i].RefreshInfo(time, openBoxes);
       }
   }

    public GameObject invCL;

    public static string GetString0(string mStr, int mNum)
    {
        while (mStr.Length < mNum)
        {
            mStr = "0" + mStr;
        }
        return mStr;
    }
}

