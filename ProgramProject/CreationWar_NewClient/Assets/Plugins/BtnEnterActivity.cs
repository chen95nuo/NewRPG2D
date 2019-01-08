using UnityEngine;
using System.Collections;
using System.Text;

public enum BtnState
{
    JoinActivity,
    ContinueActivity,
    ActivityQueue,
    GetReward,
    CancelQueue,
    WasDone,
}
public class BtnEnterActivity : MonoBehaviour
{
    public static BtnEnterActivity btnEnterActivity;
    public static BtnState btnState = BtnState.JoinActivity;
    public static string activityID = "";
    public static int activityType = 0;
    private UILabel btnLabel;
    private UIButton btn;
    private string showTxt;//按钮显示文字

    void Awake()
    {
        btnEnterActivity = this;
        btnLabel = this.transform.GetComponentInChildren<UILabel>();
        btn = this.GetComponent<UIButton>();
    }
	
    public void SwitchBtnState(BtnState mBtnState)
    {
        switch (mBtnState)
        {
            case BtnState.JoinActivity:
                {
                    if (!btn.isEnabled)
                    {
                        btn.isEnabled = true;
                    }

                    PanelActivity.panelActivity.EnableItemCollider(true);

                    showTxt = StaticLoc.Loc.Get("info821");//参加活动
                    btnState = BtnState.JoinActivity;
                }
                break;
            case BtnState.ContinueActivity:
                {
                    //if (btn.isEnabled)
                    //{
                    //    btn.isEnabled = false;
                    //}

                    //if (string.Equals(showTxt, StaticLoc.Loc.Get("info821")))// 当按钮上显示参加活动
                    if (activityType == PanelActivity.ACTIVITY_TYPE_NORAML || activityType == PanelActivity.ACTIVITY_TYPE_TASK || activityType == PanelActivity.ACTIVITY_TYPE_BOSS)
                    {
                        if (btn.isEnabled)
                        {
                            btn.isEnabled = false;
                        }
                        showTxt = StaticLoc.Loc.Get("info861");// 进行中
                    }
                    //else if (string.Equals(showTxt, StaticLoc.Loc.Get("messages086")))// 当按钮上显示排队
                    else if (activityType == PanelActivity.ACTIVITY_TYPE_BATTLEFIELD)
                    {
                        PanelActivity.panelActivity.EnableItemCollider(false);
                        if (!btn.isEnabled)
                        {
                            btn.isEnabled = true;
                        }
                        showTxt = StaticLoc.Loc.Get("info862");// 排队中
                    }

                    //if (!btn.isEnabled)
                    //{
                    //    btn.isEnabled = true;
                    //}
                    //showTxt = StaticLoc.Loc.Get("info862");// 排队中

                    btnState = BtnState.ContinueActivity;
                }
                break;
            case BtnState.ActivityQueue:
                {
                    if (!btn.isEnabled)
                    {
                        btn.isEnabled = true;
                    }

                    PanelActivity.panelActivity.EnableItemCollider(true);

                    showTxt = StaticLoc.Loc.Get("messages086");//排队
                    btnState = BtnState.ActivityQueue;

                    if (!string.IsNullOrEmpty(activityID))
                    {
                        PanelActivity.panelActivity.SetItemCancelState(activityID, false);
                    }
                }
                break;
            case BtnState.GetReward:
                {
                    if (!btn.isEnabled)
                    {
                        btn.isEnabled = true;
                    }

                    PanelActivity.panelActivity.EnableItemCollider(true);

                    showTxt = StaticLoc.Loc.Get("info760");//领取奖励
                    btnState = BtnState.GetReward;
                }
                break;
            case BtnState.CancelQueue:
                {
                    if (!btn.isEnabled)
                    {
                        btn.isEnabled = true;
                    }

                    PanelActivity.panelActivity.EnableItemCollider(true);

                    showTxt = StaticLoc.Loc.Get("info1024");// 退出排队
                    btnState = BtnState.CancelQueue;

                    if (!string.IsNullOrEmpty(activityID))
	                {
                        PanelActivity.panelActivity.SetItemCancelState(activityID, true);
                    }
                }
                break;
            case BtnState.WasDone:
                {
                    if (btn.isEnabled)
                    {
                        btn.isEnabled = false;
                    }

                    showTxt = StaticLoc.Loc.Get("tips077");//已经完成
                }
                break;
        }

        btnLabel.text = showTxt;
	}

    public void BtnEnterClick()
    {
		try{
	        switch (btnState)
	        {
	            case BtnState.JoinActivity:
	                {
	                    if (!string.IsNullOrEmpty(activityID))
	                    {
	                        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().JoinActivity(activityID));
	                    }
	                    else
	                    {
	                        btn.isEnabled = false;
	                    }
	                    // PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("JoinActivity");
	                }
	                break;
	            case BtnState.ContinueActivity:
	                {
                        int activityType = int.Parse(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytActivity.SelectRowEqual("id", activityID)["ActivityType"].YuanColumnText);
                        if (activityType == PanelActivity.ACTIVITY_TYPE_NORAML || activityType == PanelActivity.ACTIVITY_TYPE_TASK || activityType == PanelActivity.ACTIVITY_TYPE_BOSS)
                        {
                            BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.JoinActivity);
                        }
                        else if (activityType == PanelActivity.ACTIVITY_TYPE_BATTLEFIELD)
                        {
                            BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.ActivityQueue);
                        }
	                }
	                break;
	            case BtnState.ActivityQueue:
	                {
	                    if (!string.IsNullOrEmpty(activityID))
	                    {
	                        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().JoinActivity(activityID));
	                        SwitchBtnState(BtnState.ContinueActivity);
	                    }
	//                    PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("ActivityQueue");
	                }
	                break;
	            case BtnState.GetReward:
	                {
	                    //PanelActivity.panelActivity.SetItem();//领取奖励
                        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().FinishActivity(activityID));

                        //PanelActivity.activityReward.Remove(activityID);
                        //string[] canFinishActivities = BtnGameManager.yt[0]["CanFinishActivity"].YuanColumnText.Split(';');
                        //StringBuilder sb = new StringBuilder();
                        //for (int i=0; i<canFinishActivities.Length;i++)
                        //{
                        //    string str = canFinishActivities[i];

                        //    if (string.Equals(activityID, str))
                        //    {
                        //        continue;
                        //    }

                        //    if (0 == i)
                        //    {
                        //        sb.Append(str);
                        //    }
                        //    else
                        //    {
                        //        sb.Append(";");
                        //        sb.Append(str);
                        //    }
                        //}
                        //BtnGameManager.yt[0]["CanFinishActivity"].YuanColumnText = sb.ToString();

                        //GameObject item = null;
                        //PanelActivity.panelActivity.ActivityItems.TryGetValue(activityID, out item);
                        //if (null != item)
                        //{
                        //    item.GetComponent<BtnActivity>().canGetReward = false;
                        //    PanelActivity.panelActivity.BtnActivityOnClick(item);
                        //}
						
                        //int num = 0;
                        //if(int.TryParse(BtnGameManager.yt[0]["NumActivityTimes"].YuanColumnText, out num) && num>= 4)
                        //{
                        //    btn.isEnabled = false;
                        //}
	                    
	                }
	                break;
                case BtnState.CancelQueue:
                    {
                        if (!string.IsNullOrEmpty(activityID))
                        {
                            string activityRefID = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytActivity.SelectRowEqual("id", activityID)["ActivityRefID"].YuanColumnText;// 战场ID
                            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().PVPCancel(activityRefID));
                        }   
                    }
                    break;
	        }
		}
		catch(UnityException e)
		{
			Debug.Log(e);
		}
    }

    public void RefreshActivity(string mID)
    {
        if(activityID.Equals(mID))
        {
            PanelActivity.activityReward.Remove(activityID);
            string[] canFinishActivities = BtnGameManager.yt[0]["CanFinishActivity"].YuanColumnText.Split(';');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < canFinishActivities.Length; i++)
            {
                string str = canFinishActivities[i];

                if (string.Equals(activityID, str))
                {
                    continue;
                }

                if (0 == i)
                {
                    sb.Append(str);
                }
                else
                {
                    sb.Append(";");
                    sb.Append(str);
                }
            }
            BtnGameManager.yt[0]["CanFinishActivity"].YuanColumnText = sb.ToString();

            GameObject item = null;
            PanelActivity.panelActivity.ActivityItems.TryGetValue(activityID, out item);
            if (null != item)
            {
                item.GetComponent<BtnActivity>().canGetReward = false;
                PanelActivity.panelActivity.BtnActivityOnClick(item);
            }
        }
    }
}
