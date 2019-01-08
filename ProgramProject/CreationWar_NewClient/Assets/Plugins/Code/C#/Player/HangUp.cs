using UnityEngine;
using System.Collections;
 

public class HangUp : MonoBehaviour {

    public UILabel lblTime;
    public GameObject panel;
	public UIPanel panelUI;
    public Warnings warnings;
    public GameObject inv;
    private  bool isHangUp;
    private  bool IsHangUp
    {
        get { return isHangUp; }
        set {
            isHangUp = value;
            StartHangUp(isHangUp);
			isPlayerHangUp=isHangUp;
			ServerRequest.requestHangUp(isHangUp);
        }
    }

	private static bool isPlayerHangUp;

	public static bool IsPlayerHangUp
	{
		get
		{
			return isPlayerHangUp;
		}
	}

	private bool isEnable=true;

	public bool IsEnable
	{
		get
		{
			return isEnable;
		}
		set
		{
			isEnable=value;
			if(isEnable)
			{
				BeginHangUp ();
			}
			else
			{
				if(IsHangUp)
				{
					IsHangUp=false;
				}
			}
		}
	}

    public static bool isCanGet = false;
    public static string logoinTime;

    IEnumerator Start()
    {
//        InvokeRepeating("SetPower", 1, 1);
		warnings=PanelStatic.StaticWarnings;
		//panel.SetActive(false);
		panelUI.enabled=false;
        timeHangUp = new System.TimeSpan();
        if (InRoom.GetInRoomInstantiate().ServerConnected&&Application.loadedLevelName!="Map200")
        {
			BeginHangUp ();

            while (BtnGameManager.yt == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (BtnGameManager.yt[0]["HangUpTime"].YuanColumnText != "")
            {
                System.TimeSpan timeSpan = InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["HangUpTime"].YuanColumnText);
                if (isCanGet && timeSpan.TotalSeconds > 30)
                {
                    isCanGet = false;
                    //GetHangUp((int)timeSpan.TotalSeconds);
                }
            }
        }
    }
	
	

    void Update()
    {
        if ((Input.touchCount>0||Input.GetMouseButtonDown (0)) && InRoom.GetInRoomInstantiate().ServerConnected && Application.loadedLevelName != "Map200")
        {
			if(IsEnable)
			{
				BeginHangUp ();
			}
        }
    }

	void BeginHangUp()
	{
		if(isHangUp)
		{
			IsHangUp = false;
		}
		else
		{
			StartHangUp(isHangUp);
		}
		timeBefor = 0;
		CancelInvoke("HangUpBefor");
		InvokeRepeating("HangUpBefor", 1, 1);
	}

    private int timeBefor = 0;
    void HangUpBefor()
    {
        timeBefor++;
		//Debug.Log("----------------------------------timeBefor:"+timeBefor);
        if (timeBefor >= 60)
        {
            IsHangUp = true;
            CancelInvoke("HangUpBefor");
        }
    }
	
	private static int getPowerTime=0;
	void SetPower()
	{
		if(InRoom.GetInRoomInstantiate().ServerConnected)
		{
			getPowerTime++;
			if(getPowerTime%3600==0)
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("YAddPower", 10, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

    private System.TimeSpan timeHangUp;
    private int numHangUp;
    void StartHangUp(bool mEnable)
    {
        if (mEnable)
        {
            numHangUp = 0;
            //exp = (int)(60 * (float)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.HangUpExp]);
            exp = (int)(60 * (float)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.HangUpExp]);
            CancelInvoke("StartHangUpTime");
            InvokeRepeating("StartHangUpTime", 1, 1);
        }
        else if(!mEnable&&numHangUp!=0)
        {
            GetExp(numHangUp);
            numHangUp = 0;
            exp = 0;
            CancelInvoke("StartHangUpTime");
        }
		panel.SetActive (mEnable);
        //panel.SetActiveRecursively(mEnable);
		panelUI.enabled = mEnable;
    }

    int playerHangTime;
    string strPlayerMaxHangTime;
    int PlayerMaxHangTime;
    void StartHangUpTime()
    {
		try
		{
	        if (InRoom.GetInRoomInstantiate().ServerConnected&&BtnGameManager.yt!=null)
	        {
	            playerHangTime=BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText==""?0: int.Parse(BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText);
	            strPlayerMaxHangTime = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerService.SelectRowEqual("VIPType", BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText)["HangUpMaxTime"].YuanColumnText;
	            PlayerMaxHangTime=int.Parse(strPlayerMaxHangTime==""?"0":strPlayerMaxHangTime)*60;
	            if (playerHangTime < PlayerMaxHangTime*60)
	            {
	                numHangUp++;
	                timeHangUp = System.TimeSpan.FromSeconds(numHangUp);
	                lblTime.text = timeHangUp.ToString();
	                if (numHangUp % 60 == 0)
	                {
	                    //exp += (int)(60 * (float)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.HangUpExp]);
                        exp += (int)(60 * (float)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.HangUpExp]);
	                    //inv.SendMessage("AddExp", 60 * (float)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.HangUpExp], SendMessageOptions.DontRequireReceiver);
						InRoom.GetInRoomInstantiate().HangUpAddExp();
	                    BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText) + 1).ToString();
	                }
	            }
	            else
	            {
	                lblTime.text = StaticLoc.Loc.Get("info353");
	            }
			}
	     }
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
    }

    int exp = 0;
    void GetExp(int mTime)
    {

//        warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}",StaticLoc.Loc.Get("info503") , exp , StaticLoc.Loc.Get("info504") ));
    }


    public void GetHangUp(int mSeconds)
    {
		if(mSeconds<=0)
		{
			return;
		}
//        Debug.Log("--------------------------上机挂机时间:" + mSeconds);
        int mHangTime=BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText==""?0: int.Parse(BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText);
        int maxHangTime = int.Parse(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerService.SelectRowEqual("VIPType", BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText)["HangUpMaxTime"].YuanColumnText) * 60*60;
        if (mHangTime < maxHangTime)
        {
            int getHangTime = ((maxHangTime - mHangTime) > mSeconds) ? mSeconds : (maxHangTime - mHangTime);

			
            //float getExp=(float)getHangTime * (float)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.HangUpExp];
            float getExp = (float)getHangTime * (float)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.HangUpExp];
			
			if(getExp>0)
			{
            inv.SendMessage("AddExp", (int)getExp, SendMessageOptions.DontRequireReceiver);
            BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["HangUpMaxTime"].YuanColumnText) + getHangTime).ToString();
				EquipEnhance.instance.ShowMyItem(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info503") , (int)getExp , StaticLoc.Loc.Get("info504") ));
			}
        }
    }
	
	public void GetHangUpPower(int mSeconds)
	{

		if(mSeconds<=0)
		{
			return;
		}
		double getPower = (double)mSeconds / 3600d * 10d;


		//进入游戏时，服务器加在线体力。
//		InRoom.GetInRoomInstantiate ().Coststrength(yuan.YuanPhoton.CostPowerType.LoginPower , (int)mSeconds , 0 , "");
		//		PanelStatic.StaticBtnGameManager.invcl.SendMessage("YAddPower", (int)getPower, SendMessageOptions.DontRequireReceiver);

//		if((int)getPower>0)
//		{
//			PanelStatic.StaticWarnings.warningAllTime.Show ("",StaticLoc.Loc.Get ("info1174")+(int)getPower+StaticLoc.Loc.Get ("messages110"));
//		}

		getPowerTime=0;


       StartCoroutine(SetRevengePower());
	}
	
	public static bool isRevenge=false;
    private IEnumerator SetRevengePower()
    {
        string[] strPlayer = BtnGameManager.yt[0]["pvp1BeInfo"].YuanColumnText.Split(';');
		isRevenge=false;
		int  minNum=0;
        for (int i = 0; i < strPlayer.Length; i++)
        {
            if (strPlayer[i] != "")
            {
                string[] tempStr = strPlayer[i].Split(',');
				if(tempStr[2]!="2")
				{
					isRevenge=true;
				}
                if (tempStr[2] == "0")
                {
                    if (tempStr[1] == "0")
                    {
                        //PanelStatic.StaticBtnGameManager.invcl.SendMessage("YAddPower", 10, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
//						Debug.Log ("---------------------------SetRevengePower:-10");
						minNum=minNum+5;
                        //PanelStatic.StaticBtnGameManager.invcl.SendMessage("YAddPower", -5, SendMessageOptions.DontRequireReceiver);
                    }
                    tempStr[2] = "1";
                    strPlayer[i] = string.Format("{0},{1},{2},{3}", tempStr[0], tempStr[1], tempStr[2], tempStr[3]);
                }
            }
        }

		if(minNum>0)
		{
			yield return new WaitForSeconds(3);
			//PanelStatic.StaticWarnings.warningAllTime.Show ("",StaticLoc.Loc.Get ("info1173")+minNum+StaticLoc.Loc.Get ("messages110"));
		}

		//if(isRevenge&&int.Parse(BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText)>=10)
		//{
		//	PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage ("OpenOfflinePlayer",SendMessageOptions.DontRequireReceiver);
		//}
        //BtnGameManager.yt[0]["pvp1BeInfo"].YuanColumnText = "";
        System.Text.StringBuilder strInfo = new System.Text.StringBuilder();
        for (int i = 0; i < strPlayer.Length; i++)
        {
            if (strPlayer[i] != "")
            {
                strInfo.AppendFormat("{0};", strPlayer[i]);
            }
        }
        BtnGameManager.yt[0]["pvp1BeInfo"].YuanColumnText = strInfo.ToString();
    }




}
