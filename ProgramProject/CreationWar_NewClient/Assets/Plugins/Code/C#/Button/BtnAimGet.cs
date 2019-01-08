using UnityEngine;
using System.Collections;

public class BtnAimGet : MonoBehaviour {

    public UISprite pic;
    public UILabel lblInfo;
    public PanelEverydayAim panelAim;
    public UILabel activityValue;// 显示活跃度
     [HideInInspector]
    public string strInfo;
     [HideInInspector]
    public int numGet;
     [HideInInspector]
    public GameObject invMaker;
    public GameObject invCL;
     [HideInInspector]
    public Warnings warnings;
     [HideInInspector]
    public yuan.YuanPhoton.GetType getType;

    private int targetValue = 0;// 目标活跃度值，达到相应的值才能领取对应奖励

	void Start()
	{
		warnings=PanelStatic.StaticWarnings;
		invCL = PanelStatic.StaticBtnGameManager.invcl;
//		panelAim = new PanelEverydayAim ();
	}

    public void SetInfo()
    {
        string[] str = strInfo.Split('|');
        targetValue = int.Parse(str[1]);
        switch (getType)
        {
            case yuan.YuanPhoton.GetType.BloodStrone:
                pic.spriteName = "UIP_Bloodstone";
                //lblInfo.text = strInfo;
                lblInfo.text = str[0];
                activityValue.text = str[1] + StaticLoc.Loc.Get("info332");
                break;
            case yuan.YuanPhoton.GetType.Gold:
                pic.spriteName = "UIP_Gold";
                //lblInfo.text = strInfo;
                lblInfo.text = str[0];
                activityValue.text = str[1] + StaticLoc.Loc.Get("info332");
                break;
            case yuan.YuanPhoton.GetType.HP:
                pic.spriteName = "UIM_Activity_Values";
                //lblInfo.text = strInfo;
                lblInfo.text = str[0];
                activityValue.text = str[1] + StaticLoc.Loc.Get("info332");
                break;
            case yuan.YuanPhoton.GetType.Item:
                activityValue.text = str[1] + StaticLoc.Loc.Get("info332");
                //string[] strTemp = strInfo.Split(',');
                string[] strTemp = str[0].Split(',');
                lblInfo.text = int.Parse(strTemp[1].Trim()).ToString();
                object[] parms = new object[2];
                parms[0] = strInfo;
                parms[1] = pic;
                invMaker.SendMessage("SpriteName", parms, SendMessageOptions.DontRequireReceiver);
                break;
        }
    }

    void OnClick()
    {
        if (numGet == 0)
        {
            if (int.Parse(panelAim.lblActivyValue.text) >= targetValue)
            {
				if (BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(0, 1) == "1")
                {
					//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info412"));
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info412"));
                    panelAim.RefreshBtnState();
                    return;
                }
                else
                {
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1074"));
                }
            }
            else
            {
                panelAim.RefreshBtnState();
                return;
            }
        }
        else if (numGet == 1)
        {
            if (int.Parse(panelAim.lblActivyValue.text) >= targetValue)
            {
				if (BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(1, 1) == "1")
                {
					//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info412"));
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info412"));
                    panelAim.RefreshBtnState();
					return;
				}
                else
                {
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1074"));
                }
            }
            else
            {
                panelAim.RefreshBtnState();
                return;
            }
        }
        else if (numGet == 2)
        {
            if (int.Parse(panelAim.lblActivyValue.text) >= targetValue)
            {
				if (BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(2, 1) == "1")
                {
					//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info412"));
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info412"));
                    panelAim.RefreshBtnState();
					return;
                }
                else
                {
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1074"));
                }
            }
            else
            {
                panelAim.RefreshBtnState();
                return;
            }
        }
        else if (numGet == 3)
        {
            if (int.Parse(panelAim.lblActivyValue.text) >= targetValue)
            {
				if (BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(3, 1) == "1")
                {
					//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info412"));
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info412"));
                    panelAim.RefreshBtnState();
					return;
				}
                else
                {
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1074"));
                }
            }
            else
            {
                panelAim.RefreshBtnState();
                return;
            } 
        }

        switch (getType)
        {
            case yuan.YuanPhoton.GetType.BloodStrone:
                invCL.SendMessage("YAddBlood", int.Parse(lblInfo.text), SendMessageOptions.DontRequireReceiver);
                
                break;
            case yuan.YuanPhoton.GetType.Gold:
                invCL.SendMessage("YAddGold", int.Parse(lblInfo.text), SendMessageOptions.DontRequireReceiver);
               
                break;
            case yuan.YuanPhoton.GetType.HP:
				PanelStatic.StaticBtnGameManager.RunOpenLoading(()=>InRoom.GetInRoomInstantiate ().Coststrength(yuan.YuanPhoton.CostPowerType.EveryDayAim , 0 , 0 , ""));
//                invCL.SendMessage("YAddPower", int.Parse(lblInfo.text), SendMessageOptions.DontRequireReceiver);         
                break;
            case yuan.YuanPhoton.GetType.Item:
                //invCL.SendMessage("AddBagItemAsID", strInfo, SendMessageOptions.DontRequireReceiver);
 				if(strInfo.Substring (0,2)=="88")
				{
					PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewDaojuItemAsID", strInfo, SendMessageOptions.DontRequireReceiver);
				}
				else if(strInfo.Substring (0,2)=="72")
				{
					PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewRideItemAsID", strInfo, SendMessageOptions.DontRequireReceiver);
				}
				else if(strInfo.Substring (0,2)=="70")
				{
					PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagDigestItemAsID",  strInfo, SendMessageOptions.DontRequireReceiver);
				}
				else if(strInfo.Substring (0,2)=="71")
				{
					PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagSoulItemAsID",  strInfo, SendMessageOptions.DontRequireReceiver);
				}				
				else
				{
					PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagItemAsID", strInfo, SendMessageOptions.DontRequireReceiver);
				}               
                break;
        }

        if (numGet == 0)
        {
                BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText = "1" + BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(1, 3);
           
        }
        else if (numGet == 1)
        {
            BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText = BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(0, 1) + "1" + BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(2, 2);
        }
        else if (numGet == 2)
        {
            BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText = BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(0, 2) + "1" + BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(3, 1);
        }
        else if (numGet == 3)
        {
            BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText = BtnGameManager.yt.Rows[0]["AimGet"].YuanColumnText.Substring(0, 3) + "1";
        }

        panelAim.RefreshBtnState();
    }
}
