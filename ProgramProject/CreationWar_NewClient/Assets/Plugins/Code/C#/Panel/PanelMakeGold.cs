using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelMakeGold : MonoBehaviour {

	public static PanelMakeGold my;

	void Awake()
	{
		my=this;
	}

    public Warnings warnings;
    public GameObject invCL;
    public UILabel lblMakeGoldTime;
    public UILabel lblMakeGoldScale;
    public UILabel lblMakeGoldAddtion;
    public UILabel lblGold;
    public UILabel lblBlood;


    public UILabel lblMakeGold2;
    public UILabel lblMakeGold5;
    public UILabel lblMakeGold10;

    public BtnEvent btnMakeGold2;
    public BtnEvent btnMakeGold5;
    public BtnEvent btnMakeGold10;

	public UIButton btnMGold1;
	public UIButton btnMGold2;
	public UIButton btnMGold3;
	public UILabel LblVip2;
	public UILabel LblVip3;

	public UILabel LblNumMe;

    [System.Serializable]
    public class MakeGoldTimes
    {
        public MakeGoldTimes(System.DateTime mStartTime, System.DateTime mEndTime)
        {
            this.startTime = mStartTime;
            this.endTime = mEndTime;
        }

        public System.DateTime startTime;
        public System.DateTime endTime;
    }

    public List<MakeGoldTimes> listTime = new List<MakeGoldTimes>();
    private bool canAddtion;
    private float goldScale;
    private float bloodScale;
    void Start()
    {
        warnings=PanelStatic.StaticWarnings;
		invCL=PanelStatic.StaticBtnGameManager.invcl;
        string[] strTime = InRoom.GetInRoomInstantiate().GetMakeGoldTime().Trim().Split(';');
        string[] myTime;
        foreach (string item in strTime)
        {
            if (item != "")
            {
                myTime = item.Split(',');
                MakeGoldTimes tempTime = new MakeGoldTimes(System.DateTime.Parse(myTime[0]), System.DateTime.Parse(myTime[1]));
                listTime.Add(tempTime);
                lblMakeGoldTime.text += myTime[0] + "-" + myTime[1]+"\n";
            }
        }
        lblMakeGoldScale.text = InRoom.GetInRoomInstantiate().GetMakeGoldScale();
        bloodScale = float.Parse(InRoom.GetInRoomInstantiate().GetMakeGoldScale().Split(':')[0]);
        goldScale = float.Parse(InRoom.GetInRoomInstantiate().GetMakeGoldScale().Split(':')[1]);
        lblMakeGoldAddtion.text = InRoom.GetInRoomInstantiate().GetMakeGoldAddtion().ToString() + "%";
        btnMakeGold2.SetEvent(this.OnBtnMakeGold2Click);
        btnMakeGold5.SetEvent(this.OnBtnMakeGold5Click);
        btnMakeGold10.SetEvent(this.OnBtnMakeGold10Click);
    }

    void OnEnable()
    {
       
        InvokeRepeating("AnlasyAddtion", 0, 1);
        ShowGoldBlood();
		ShowNewNum();
    }

    void OnDisable()
    {
        CancelInvoke("AnlasyAddtion");
    }

    /// <summary>
    /// 炼金
    /// </summary>
    /// <param name="mNum">转化的血石数</param>
    public void MakeGold(int mNum)
    {
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().UseMoney (yuan.YuanPhoton.UseMoneyType.MakeGold,0,mNum,null));
//        if (int.Parse(BtnGameManager.yt[0]["Bloodstone"].YuanColumnText) >= mNum)
//        {
//
//            //invCL.SendMessage("YAddBlood", -mNum, SendMessageOptions.DontRequireReceiver);
//            //invCL.SendMessage("YAddGold", CanMakeGold(mNum), SendMessageOptions.DontRequireReceiver);
//			YuanBackInfo yuanBack=new YuanBackInfo("MakeGold");
//			StartCoroutine( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate ().ClientMoney (CanMakeGold(mNum).To16String (),(-mNum).To16String (),yuanBack)));
//			
//			while(yuanBack.isUpate)
//			{
//				yield return new WaitForSeconds(0.1f);
//			}
//			switch(yuanBack.opereationResponse.ReturnCode)
//			{
//				case (short) yuan.YuanPhoton.ReturnCode.NoGold:
//				{
//					warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info491"));
//				}
//				break;
//				case (short) yuan.YuanPhoton.ReturnCode.NoBloodStone:
//				{
//					warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info490"));
//				}
//				break;				
//			}
//            ShowGoldBlood();
//        }
//        else
//        {
//            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info498"));
//        }
    }

    /// <summary>
    /// 检测是否在加成时间段内
    /// </summary>
    public void AnlasyAddtion()
    {

		yuan.YuanMemoryDB.YuanRow yrZeor = YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytPlayerService.SelectRowEqual ("VIPType","0");
		lblMakeGold2.text = yrZeor["blood"].YuanColumnText+":"+yrZeor["gold"].YuanColumnText;

		yuan.YuanMemoryDB.YuanRow yrOne = YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytPlayerService.SelectRowEqual ("VIPType","1");
		lblMakeGold5.text = yrOne["blood"].YuanColumnText+":"+ yrOne["gold"].YuanColumnText;

		yuan.YuanMemoryDB.YuanRow yrFive = YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytPlayerService.SelectRowEqual ("VIPType","5");
		lblMakeGold10.text = yrFive["blood"].YuanColumnText+":"+ yrFive["gold"].YuanColumnText;
        System.DateTime nowTime =System.DateTime.Parse(InRoom.GetInRoomInstantiate().serverTime.TimeOfDay.ToString());
        foreach (MakeGoldTimes item in listTime)
        {
            if (nowTime >= item.startTime && nowTime <= item.endTime)
            {
                canAddtion = true;
                return;
            }
        }
        canAddtion = false;
    }

    /// <summary>
    /// 可以转化成多少金币
    /// </summary>
    /// <param name="mNum">转化的血石数</param>
    /// <returns></returns>
    public int CanMakeGold(int mNum)
    {
        if (canAddtion)
        {

			return (int)((float)mNum / bloodScale * goldScale * (((float)InRoom.GetInRoomInstantiate().GetMakeGoldAddtion()/ 100f + 1f) + (int)(mNum / 4f) / 10f));

        }
		else {
			return (int)(((float)mNum / bloodScale * goldScale) * (((int)(mNum / 4f) / 10f)+1f));
		}
    }

    /// <summary>
    /// 刷新金钱显示
    /// </summary>
    public void ShowGoldBlood()
    {
        lblGold.text = BtnGameManager.yt[0]["Money"].YuanColumnText;
        lblBlood.text = BtnGameManager.yt[0]["Bloodstone"].YuanColumnText;
		ShowNewNum();
    }

	public  void ShowNewNum()
	{
		if(int.Parse(BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText)>=1)
		{
			btnMGold2.isEnabled = true ; 
			LblVip2.text = StaticLoc.Loc.Get("buttons062");
		}else
		{
			btnMGold2.isEnabled = false ; 
			LblVip2.text = StaticLoc.Loc.Get("info1204");
		}

		if(int.Parse(BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText)>=5)
		{
			btnMGold2.isEnabled = true ; 
			btnMGold3.isEnabled = true ;
			LblVip3.text = StaticLoc.Loc.Get("buttons062");

		}else
		{
			btnMGold3.isEnabled = false ; 
			LblVip3.text = StaticLoc.Loc.Get("info1205");
		}


		yuan.YuanMemoryDB.YuanRow yr = YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytPlayerService.SelectRowEqual ("VIPType",BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText);
		
//		yr["blood"].YuanColumnText

//		Debug.Log("ran==========================================="+BtnGameManager.yt.Rows[0]["useMakeGold"].YuanColumnText+"=================================="+yr["canMakeGold"].YuanColumnText);
		string NumNow = (int.Parse(yr["canMakeGold"].YuanColumnText)-int.Parse(BtnGameManager.yt.Rows[0]["useMakeGold"].YuanColumnText)).ToString();
		LblNumMe.text =  string.Format("{0}{1}{2}" , NumNow , "/" , yr["canMakeGold"].YuanColumnText) ;

//		BtnGameManager.yt.Rows[0]["canMakeGold"].YuanColumnText


	}

    public void OnBtnMakeGold2Click(object sender, object parm)
    {
        MakeGold(0);
    }

    public void OnBtnMakeGold5Click(object sender, object parm)
    {
        MakeGold(1);
    }
    public void OnBtnMakeGold10Click(object sender, object parm)
    {
       MakeGold(5);
    }
}
