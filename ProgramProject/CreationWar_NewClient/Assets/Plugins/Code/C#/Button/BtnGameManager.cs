using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnGameManager : MonoBehaviour {

    [HideInInspector]
    public static yuan.YuanMemoryDB.YuanTable yt;
	
	public static float rpcSendTime=0.1f;
	public static int roomPlayerNum=25;
	
	public GameObject objLoading;
    private GameObject objCreateTeam;
    private GameObject objCreateLegion;
    private GameObject objCreateGuild;
    private GameObject objCreateCorps;

    private GameObject objTeamPanel;
    [HideInInspector]
    public GameObject gridsRefreshTeam;
    [HideInInspector]
    public GameObject gridsRefresh2V2;
    [HideInInspector]
    public GameObject gridsRefresh4V4;

	public Transform tranCreatePath;
	public Transform tranBindPath; 

    public Warnings warnings;
    public BtnGameManagerBack btnGameManagerBack;

    public YuanPicManager yuanPicManager;
	public RedemptionCode redemptionCode;
	
	public static int numPubSkillCD=1;//技能公共CD
	public static float numSeviceMianCity=0.33f;//主城同步频率
	public static float numSeviceDuplicate=0.2f;//副本同步频率
	public static float numSevicePVP=0.1f;//PVP同步频率
	
	/// <summary>
	/// 客户端预备参数
	/// 现有参数名:
	/// ClientParms1
	/// ClientParms2
	/// ClientParms3
	/// ClientParms4
	/// </summary>
	public static Dictionary<string,int> dicClientParms=new Dictionary<string, int>(){
		{"ClientParms1",100},
		{"ClientParms2",100},
		{"ClientParms3",100},
		{"ClientParms4",100},
	};
	
	public UILabel lblTeamInfo;
//	void OnLevelWasLoaded(int level) {
//		if(warnings.warningAllEnter.gameObject.active)
//		{
//			warnings.warningAllEnter.Close ();
//		}
//		
//		if(!string.IsNullOrEmpty(lblTeamInfo.text))
//		{
//			lblTeamInfo.text = "";
//		}
//    }


	public void OnApplicationPause(bool pauseStatus) 
	{
		InRoom.GetInRoomInstantiate ().InHomeQueue (pauseStatus);
	}
	
	private bool isStartTimeOut=false;
	public IEnumerator OpenLoading(System.Action mAction)
	{
		if(Application.loadedLevelName!="Map200")
		{
			isStartTimeOut=true;
			objLoading.SetActiveRecursively (true);
			int outTime=0;
			if(null!=mAction)
			{
				mAction();
			}
			while(true)
			{
				yield return new WaitForSeconds(1);
				outTime++;
				if(outTime>=15||!InRoom.GetInRoomInstantiate ().ServerConnected||!isStartTimeOut)
				{
					break;
				}
			}
			
	        
			if(isStartTimeOut)
			{
	            if (mAction != null&&mAction.Target!=null)
	            {
	//                NGUIDebug.Log("==timeout :" + mAction.Target == null ? mAction.Method.Name : mAction.Target.ToString() + "\n");
	            }
	            
				warnings.warningAllTimeOut.ShowTimeOut (StaticLoc.Loc.Get ("info358"),StaticLoc.Loc.Get ("info720"),()=>RunOpenLoading(mAction));
				objLoading.SetActiveRecursively (false);
				isStartTimeOut=false;

	            PanelGamble.canStartEnter = true;
			}
		}
	}
	
	public void RunOpenLoading(System.Action mAction)
	{
		StartCoroutine (OpenLoading(mAction));
	}
	
	public void CloseLoading()
	{
		isStartTimeOut=false;
		objLoading.SetActiveRecursively (false);
	}
	
	public void CloseCreate()
	{
		tranCreatePath.gameObject.SetActiveRecursively (false);
	}

    /// <summary>
    /// 社交新建队伍
    /// </summary>
    /// <param name="obj"></param>
    private IEnumerator CreateTeam(GameObject obj)
    {
		if(objCreateTeam==null)
		{
			GameObject tempObj =	(GameObject)Resources.Load ("Anchor - CreatTeam");
			tempObj=(GameObject)Instantiate (tempObj);
			tempObj.transform.parent=tranCreatePath;
						tempObj.transform.localPosition=Vector3.zero;
			tempObj.transform.localScale=Vector3.one;
			yield return new WaitForSeconds(0.5f);
		}
        CreateWindow(objCreateTeam, objTeamPanel);
    }

    /// <summary>
    /// 社交新建军团
    /// </summary>
    /// <param name="obj"></param>
    private IEnumerator CreateLegion(GameObject obj)
    {
			if(objCreateLegion==null)
		{
			GameObject tempObj =	(GameObject)Resources.Load ("Anchor - CreatLegion");
				tempObj=(GameObject)Instantiate (tempObj);
			tempObj.transform.parent=tranCreatePath;
						tempObj.transform.localPosition=Vector3.zero;
			tempObj.transform.localScale=Vector3.one;
			yield return new WaitForSeconds(0.5f);
		}
        CreateWindow(objCreateLegion, objTeamPanel);
    }

    /// <summary>
    /// 社交新建工会
    /// </summary>
    /// <param name="obj"></param>
	private GameObject ObjGuild;
    private IEnumerator CreateGuild(GameObject obj)
    {
		if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.GuildSwitch)!="1")
		{
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info645"));
		}		
		else
		{
			if(!tranCreatePath.gameObject.activeSelf)
			{
				tranCreatePath.gameObject.SetActive(true);
			}

			if(objCreateGuild==null)
			{
				GameObject tempObj =	(GameObject)Resources.Load ("Anchor - CreatGuild");
					tempObj=(GameObject)Instantiate (tempObj);
				tempObj.transform.parent=tranCreatePath;
							tempObj.transform.localPosition=Vector3.zero;
				tempObj.transform.localScale=Vector3.one;
				ObjGuild = tempObj;
				yield return new WaitForSeconds(0.5f);
			}
	        CreateWindow(objCreateGuild, objTeamPanel);
		}
    }

    /// <summary>
    /// 社交新建战队
    /// </summary>
    /// <param name="obj"></param>
    private IEnumerator CreateCorps(GameObject obj)
    {
		if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.PVP2Switch)!="1")
		{
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info645"));

		}
		else
		{
			if(objCreateCorps==null)
			{
				GameObject tempObj =	(GameObject)Resources.Load ("Anchor - CreatCorps");
				tempObj=(GameObject)Instantiate (tempObj);
				tempObj.transform.parent=tranCreatePath;
				tempObj.transform.localPosition=Vector3.zero;
				tempObj.transform.localScale=Vector3.one;
				yield return new WaitForSeconds(0.5f);
			}
	        strCkbGropsType0.value = true;
	        CreateWindow(objCreateCorps, objTeamPanel);			
		}
    }

    /// <summary>
    /// 社交新建战队PVP4
    /// </summary>
    /// <param name="obj"></param>
    private IEnumerator CreatePVP4(GameObject obj)
    {
		if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.PVP4Switch)!="1")
		{
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info645"));

		}
		else
		{
			if(objCreateCorps==null)
			{
				GameObject tempObj =	(GameObject)Resources.Load ("Anchor - CreatCorps");
				tempObj=(GameObject)Instantiate (tempObj);
				tempObj.transform.parent=tranCreatePath;
				tempObj.transform.localPosition=Vector3.zero;
				tempObj.transform.localScale=Vector3.one;
				yield return new WaitForSeconds(0.5f);
			}		
	        strCkbGropsType1.value = true;
	        CreateWindow(objCreateCorps, objTeamPanel);
		}
    }

    /// <summary>
    /// 社交新建窗口
    /// </summary>
    /// <param name="objOpen">关闭后打开的面板</param>
    /// <param name="objClose">关闭按钮</param>
    private void CreateWindow(GameObject objOpen,GameObject objClose)
    {
//		objClose.SetActiveRecursively(false);
        objOpen.SetActiveRecursively(true);
      
        UIButtonMessage tempMessage = objOpen.transform.FindChild("btnClose").GetComponent<UIButtonMessage>();
        tempMessage.target = this.gameObject;
        tempMessage.functionName = "BtnClose";
        objCloseClose = objOpen;
        objCloseOpen = objClose;
    }

    private GameObject objCloseOpen;
    private GameObject objCloseClose;
    /// <summary>
    /// 社交新建的关闭按钮
    /// </summary>
    public void BtnClose()
    {
//		Debug.Log ("-======================================0000000000000000000000000");
        objCloseClose.SetActiveRecursively(false);
		ObjGuild.SetActive(false);
//        objCloseOpen.SetActiveRecursively(true);
//        objCloseOpen.GetComponent<CkbToPanel>().CbkClick();
    }

    private UIInput txtBlackFirend;
    /// <summary>
    /// 添加黑名单按钮
    /// </summary>
    private void BlackBtnClick()
    {
        if (txtBlackFirend.text.Trim() != "")
        {
            if (!btnGameManagerBack.isBlackFirend)
            {
                InRoom.GetInRoomInstantiate().BlackFirend(txtBlackFirend.text.Trim(), "DarkSword2", "PlayerInfo");
            }
		
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info419"));
        }
    }

    private UIInput txtFirend;
    /// <summary>
    /// 添加好友按钮
    /// </summary>
    private void AddFriendBtnClick()
    {
        if (txtFirend.text.Trim() != "")
        {
            if (!btnGameManagerBack.isFirend)
            {
                //InRoom.GetInRoomInstantiate().AddFirend(txtFirend.text.Trim(), "DarkSword2", "PlayerInfo");
				InRoom.GetInRoomInstantiate ().FirendsAddInvitForName (txtFirend.text.Trim());
            }
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info420"));
        }
    }

  

    /// <summary>
    /// 黑名单
    /// </summary>
    /// <param name="playerID">角色ID</param>
    public void BlackFirend(string playerID)
    {
//		Debug.Log ("------------------"+playerID);
		if(!string.IsNullOrEmpty(playerID))
		{
	        string strMy = yt[0]["BlackFriendsId"].YuanColumnText;
	        if (strMy.IndexOf(playerID.Trim()+";") != -1)
	        {
	            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info421"));
	        }
	        else
	        {
	            yt[0]["BlackFriendsId"].YuanColumnText += playerID + ";";
	            warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info422"));
	        }
	//		Debug.Log ("--------------------------BlackFriendsId:"+yt[0]["BlackFriendsId"].YuanColumnText);
	        yt[0]["FriendsId"].YuanColumnText = yt[0]["FriendsId"].YuanColumnText.Replace(playerID + ";", "");
			StartCoroutine(refreshBlackFirend.YuanOnEnable());
			
			StartCoroutine(refreshPlayerFirend.YuanOnEnable());
		}
    }
	
	

    /// <summary>
    /// 加好友
    /// </summary>
    /// <param name="playerID">角色ID</param>
    public void AddFirend(string playerID)
    {
		if(!string.IsNullOrEmpty (playerID))
		{
	        string strMy = yt[0]["FriendsId"].YuanColumnText;
			if(playerID==yt[0]["PlayerID"].YuanColumnText)
			{
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info423"));
				return;
			}
//			Debug.Log (string.Format ("--------{0}---------{1}",playerID,strMy));
	        if (strMy.IndexOf(playerID.Trim()+";") != -1)
	        {
	            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info424") );
	        }
	        else
	        {
	            yt[0]["FriendsId"].YuanColumnText += playerID + ";";
	            warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info425"));
				StartCoroutine (refreshPlayerFirend.YuanOnEnable());
	        }
	
	        yt[0]["BlackFriendsId"].YuanColumnText = yt[0]["BlackFriendsId"].YuanColumnText.Replace(playerID + ";", "");
		}
    }

    private PlayerInfo playerInfoFirends;
    /// <summary>
    /// 移除好友按钮
    /// </summary>
    public void BtnRemoveFirend()
    {
        if (playerInfoFirends != null && playerInfoFirends.yr.ContainsKey("PlayerID"))
        {
            removePlayerID = playerInfoFirends.yr["PlayerID"].YuanColumnText.Trim();
            if (yt[0]["FriendsId"].YuanColumnText.IndexOf(removePlayerID + ";") != -1)
            {
                warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info426"));
             //   removePlayerID = playerInfoFirends.yr["PlayerID"].YuanColumnText.Trim();
                removeRowName = "FriendsId";
                warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
                warnings.warningAllEnterClose.btnEnter.functionName = "RemoveFirends";
            }
        }
    }

    private PlayerInfo playerInfoBlack;
    /// <summary>
    /// 移除黑名单按钮
    /// </summary>
    public void BtnRemoveBlack()
    {
        if (playerInfoBlack != null && playerInfoBlack.yr.ContainsKey("PlayerID"))
        {
            removePlayerID = playerInfoBlack.yr["PlayerID"].YuanColumnText.Trim();
            if (yt[0]["BlackFriendsId"].YuanColumnText.IndexOf(removePlayerID + ";") != -1)
            {
                warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info427"));
             //   removePlayerID = playerInfoBlack.yr["PlayerID"].YuanColumnText.Trim();
                removeRowName = "BlackFriendsId";
                warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
                warnings.warningAllEnterClose.btnEnter.functionName = "RemoveFirends";
            }
        }
    }


    private string removePlayerID;
    private string removeRowName;
	private RefreshList refreshPlayerFirend;
	private RefreshList refreshBlackFirend;
    /// <summary>
    /// 移除好友
    /// </summary>
    public void RemoveFirends()
    {
        if (yt[0][removeRowName].YuanColumnText.IndexOf(removePlayerID+";") != -1)
        {
            yt[0][removeRowName].YuanColumnText = yt[0][removeRowName].YuanColumnText.Replace(removePlayerID + ";", "");
            warnings.warningAllEnterClose.Close();
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info600"));
            if (removeRowName == "FriendsId")
            {
                StartCoroutine(refreshPlayerFirend.YuanOnEnable());
            }
            else if (removeRowName == "BlackFriendsId")
            {
                StartCoroutine(refreshBlackFirend.YuanOnEnable());
            }
        }
    }

    private UIGrid gridSelectDuplicate;
    private UIGrid gridSelectLevel;
    /// <summary>
    /// 是否正在创建队伍
    /// </summary>
    public static bool isTeamCreat = false;
    /// <summary>
    /// 创建队伍
    /// </summary>
    public void TeamCreat()
    {
        if (!isTeamCreat)
        {
            string teamName = string.Empty;
            UIToggle[] tempSelectDuplicate = gridSelectDuplicate.GetComponentsInChildren<UIToggle>();
            UIToggle[] tempSelectLevel = gridSelectLevel.GetComponentsInChildren<UIToggle>();
            foreach (UIToggle item in tempSelectDuplicate)
            {
                if (item.value)
                {
                    teamName = item.GetComponentInChildren<UILabel>().text.Trim();
                    break;
                }
            }
            foreach (UIToggle item in tempSelectLevel)
            {
                if (item.value)
                {
                    teamName += " " + item.GetComponentInChildren<UILabel>().text.Trim();
                }
            }
            InRoom.GetInRoomInstantiate().TeamCreate(teamName);
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info428"));
        }
    }

    public static bool isLegionTempCreate = false;
    private UIInput txtLegionCreate;
    private GetPic gpLegionCreate; 
    /// <summary>
    /// 创建军团
    /// </summary>
    public void LegionTempCreate()
    {
        if (!isLegionTempCreate)
        {
            if (txtLegionCreate.text.Trim() != "")
            {
                if (cbxLegionTemp.value)
                {
                    InRoom.GetInRoomInstantiate().LegionTempCreate(txtLegionCreate.text.Trim(),gpLegionCreate.PicID);
                    BtnClose();
                }
                else if (cbxLegionForever.value)
                {
                    InRoom.GetInRoomInstantiate().LegionDBCreate(txtLegionCreate.text.Trim(), gpLegionCreate.PicID, "DarkSword2", "Legion");
                    BtnClose();
                }
            }
            else
            {
                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info429"));
            }
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info430"));
        }
    }

    /// <summary>
    /// 获取队伍列表
    /// </summary>
    public void GetTeam()
    {
        InRoom.GetInRoomInstantiate().GetTeams("123");
    }

    /// <summary>
    /// 获取临时队伍列表
    /// </summary>
    public void GetTempTeam()
    {
        InRoom.GetInRoomInstantiate().GetTempTeams(selectTempListName);
        //InRoom.GetInRoomInstantiate().GetTempTeams("");
    }

    private UIGrid gridTeam;
    public GameObject btnTeamForCreate;
    private List<BtnTeamForCreate> listBtnTeamForCreate = new List<BtnTeamForCreate>();
    /// <summary>
    /// 接收到队伍列表回调
    /// </summary>
    /// <param name="parameters"></param>
    public IEnumerator GetTeamOK(Dictionary<short, object> parameters)
    {
        //BtnTeamForCreate[] tempListBtn = gridTeam.transform.GetComponentsInChildren<BtnTeamForCreate>();
        foreach (BtnTeamForCreate item in listBtnTeamForCreate)
        {
            item.gameObject.SetActiveRecursively(false);
        }
        Dictionary<short, string> dicTempTeam;
        string teamID = string.Empty;
        string teamHeadID = string.Empty;
        string teamName = string.Empty;
        string teamInfo = string.Empty;
        string teamMemver = string.Empty;
        string[] strTeamMemver;
        

        int num = 0;
        foreach (KeyValuePair<short, object> itemTeamList in parameters)
        {

            dicTempTeam = itemTeamList.Value as Dictionary<short, string>;
            teamID = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamID];
            teamHeadID = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamHeadID];
            teamName = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamName];
            teamInfo = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamInfo];
            teamMemver = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamMemver];
            strTeamMemver = teamMemver.Split(';');

            if (num < listBtnTeamForCreate.Count)
            {
                SetTeamPlayerBtn(listBtnTeamForCreate[num],teamID,teamHeadID,teamName,teamInfo,strTeamMemver);
                listBtnTeamForCreate[num].gameObject.SetActiveRecursively(true);
            }
            else
            {
                GameObject objBtn = (GameObject)Instantiate(btnTeamForCreate);
                objBtn.transform.parent = gridTeam.transform;
                objBtn.transform.localScale = new Vector3(1, 1, 1);
                UIButtonMessage btnMessage = objBtn.GetComponent<UIButtonMessage>();
                btnMessage.target = this.gameObject;
                btnMessage.functionName = "BtnTeamForCreateClick";
                BtnTeamForCreate tempBtn = objBtn.GetComponent<BtnTeamForCreate>();
//                tempBtn.GetComponent<UIToggle>().radioButtonRoot = gridTeam.transform;
                listBtnTeamForCreate.Add(tempBtn);
                SetTeamPlayerBtn(tempBtn, teamID, teamHeadID,teamName, teamInfo, strTeamMemver);
            }

            num++;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        gridTeam.repositionNow = true;
    }

    /// <summary>
    /// 获取临时列表成功
    /// </summary>
    /// <param name="parameters"></param>
    public void GetTempTeamOK(Dictionary<short, object> parameters)
    {
        if (tableTempTeam!=null&&gridTeam!=null&&gridTeam.gameObject.active)
        {
            //StartCoroutine(RefreshMainTempTeam(parameters));
            tableTempTeam.SetFrist(parameters, RefreshMainTempTeam, 4);
        }
        if (gridTaskTempTeam!=null&&gridTaskTempTeam.gameObject.active)
        {
            //tableTempTeam.SetFrist (parameters,RefreshTaskTempTeam,4);
            RefreshTaskTempTeam(parameters);
        }
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("RefreshTempTeamButton" , SendMessageOptions.DontRequireReceiver);
    }

    private UIGrid gridTaskTempTeam;
    [HideInInspector]
    public string selectTempListName = string.Empty;
	public string selectTempName = string.Empty;
    public List<BtnTeamForCreate> listBtnTeamForTask = new List<BtnTeamForCreate>();
    /// <summary>
    /// 刷新副本版中的临时队伍列表
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public void RefreshTaskTempTeam(Dictionary<short, object> parameters)
    {
        //BtnTeamForCreate[] tempListBtn = gridTeam.transform.GetComponentsInChildren<BtnTeamForCreate>();
        foreach (BtnTeamForCreate item in listBtnTeamForTask)
        {
            item.gameObject.SetActiveRecursively(false);
        }
        Dictionary<short, string> dicTempTeam;
        string teamID = string.Empty;
        string teamHeadID = string.Empty;
        string teamName = string.Empty;
        string teamInfo = string.Empty;
        string teamMemver = string.Empty;
        string[] strTeamMemver;
        string teamInstensID = string.Empty;
        string teamPass = string.Empty;


        //Debug.Log("临时列表的成员个数:" + parameters.Count);
        int num = 0;
        foreach (KeyValuePair<short, object> itemTeamList in parameters)
      //  foreach(Dictionary<short, object> itemTeamList in parameters)
        {
            if (num <= 30)
            {
                //dicTempTeam = itemTeamList.Value as Dictionary<short, string>;
                Dictionary<object, object> test;
                test = itemTeamList.Value as Dictionary<object, object>;
                dicTempTeam = test.DicObjTo<short,string>();
                teamID = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamID];
                teamHeadID = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamHeadID];
                teamName = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamName];
                teamInfo = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamInfo];
                teamMemver = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamMemver];
                //小队地图instenID
                teamInstensID = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.ItemID];

                teamPass = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.UserPwd];//队伍密码

                strTeamMemver = teamMemver.Split(';');


                if (teamName != selectTempListName)
                {
                    continue;
                }

                if (num < listBtnTeamForTask.Count)
                {
					SetTeamPlayerBtn(listBtnTeamForTask[num], teamID, teamHeadID, teamName, teamInfo, strTeamMemver,teamPass,teamInstensID);
                    listBtnTeamForTask[num].gameObject.SetActiveRecursively(true);
                }
                else
                {
                    GameObject objBtn = (GameObject)Instantiate(btnTeamForCreate);
                    objBtn.transform.parent = gridTaskTempTeam.transform;
                    objBtn.transform.localScale = new Vector3(1, 1, 1);
                    objBtn.transform.localPosition = Vector3.zero;
                    UIButtonMessage btnMessage = objBtn.GetComponent<UIButtonMessage>();
                    btnMessage.target = this.gameObject;
                    //btnMessage.functionName = "BtnTeamForCreateClick";
                    BtnTeamForCreate tempBtn = objBtn.GetComponent<BtnTeamForCreate>();
//                    tempBtn.GetComponent<UIToggle>().radioButtonRoot = gridTaskTempTeam.transform;
                    listBtnTeamForTask.Add(tempBtn);
					SetTeamPlayerBtn(tempBtn, teamID, teamHeadID, teamName, teamInfo, strTeamMemver,teamPass,teamInstensID);
				}
				
				num++;
            }
        }
        gridTaskTempTeam.repositionNow = true;
		MTW.SendMessage("SetPaiDuiAsID2" , SendMessageOptions.DontRequireReceiver);
    }


	public WarningAll IsHavePassWard;
    /// <summary>
    /// 副本板临时小队加入
    /// </summary>
    public void BtnTaskTempTeamAdd()
    {
        foreach (BtnTeamForCreate item in listBtnTeamForTask)
        {
            if (item.myCheckbox.value)
            {
                if (item.gameObject.active)
                {
					if(item.teamPassWard==""){
                        InRoom.GetInRoomInstantiate().AddTempTeam(selectTempListName, item.teamID);
					}else{
						IsHavePassWard.gameObject.SetActive(true);
						IsHavePassWard.btnEnter.target = this.gameObject;
						IsHavePassWard.btnEnter.functionName = "AddIsHavePassWard";
					}
                  //  InRoom.GetInRoomInstantiate().AddTempTeam("123", item.teamID);
                }
                else
                {
                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info431"));
                }
                return;
            }
        }
        warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info432"));
    }

	public void AddIsHavePassWard(){
		foreach (BtnTeamForCreate item in listBtnTeamForTask)
		{
			if (item.myCheckbox.value)
			{
				if (item.gameObject.active)
				{
                    InRoom.GetInRoomInstantiate().AddTempTeam(selectTempListName, item.teamID, txtAddTempTeamPwd.text.Trim());
	}
	}
		}
	}

    public UIInput txtTempTeamPwd;

	public UIToggle IsChecked;

	public GameObject objTeam;
    /// <summary>
    /// 副本板临时小队创建
    /// </summary>
    public void BtnTaskTempTeamCreate()
    {
		objTeam.SendMessage("ShowTeamLayer",false,SendMessageOptions.DontRequireReceiver);
        if (selectTempListName != "" && !IsChecked.value)
        {
            InRoom.GetInRoomInstantiate().AddTempTeam(selectTempListName, "");
        }

        if (selectTempListName != "" && IsChecked.value)
		{
            InRoom.GetInRoomInstantiate().AddTempTeam(selectTempListName, "", txtTempTeamPwd.text.Trim());
		}
    }

    /// <summary>
    /// 刷新主菜单中的副本小队列表
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public void RefreshMainTempTeam(Dictionary<short, object> parameters)
    {
        //BtnTeamForCreate[] tempListBtn = gridTeam.transform.GetComponentsInChildren<BtnTeamForCreate>();
        foreach (BtnTeamForCreate item in listBtnTeamForCreate)
        {
            item.gameObject.SetActiveRecursively(false);
        }
        Dictionary<short, string> dicTempTeam;
        string teamID = string.Empty;
        string teamHeadID = string.Empty;
        string teamName = string.Empty;
        string teamInfo = string.Empty;
        string teamMemver = string.Empty;
        string[] strTeamMemver;

        string levelID = string.Empty;
        int levelL = 0;
        int tempLevel = 0;
        string[] strMyPlace = BtnGameManager.yt[0]["GetPlace"].YuanColumnText.Split(';');

        //Debug.Log("临时列表的成员个数:" + parameters.Count);
        int num = 0;
        foreach (KeyValuePair<short, object> itemTeamList in parameters)
        {
            if (num <= 30)
            {
                dicTempTeam = itemTeamList.Value as Dictionary<short, string>;
                teamID = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamID];
                teamHeadID = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamHeadID];
                teamName = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamName];
                teamInfo = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamInfo];
                teamMemver = (string)dicTempTeam[(short)yuan.YuanPhoton.ParameterType.TeamMemver];
                strTeamMemver = teamMemver.Split(';');

                //Debug.Log("--------------------------------队伍名称:" + teamName);
                //Debug.Log("--------------------------------队伍成员:" + teamMemver);

                levelID = string.Empty;
                levelL = 0;
                tempLevel = 0;
                tempLevel = teamName.IndexOf("关卡");
                if (teamName.IndexOf("精英关卡") != -1)
                {
                    levelL = 5;
                }
                else if (tempLevel != -1)
                {

                    levelL = int.Parse(teamName.Substring(tempLevel + 2, teamName.Length - tempLevel - 2));
                }

                tempLevel = teamName.IndexOf(" ");
                if (tempLevel != -1)
                {
                    levelID = teamName.Substring(0, tempLevel);
                    levelID = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapName", levelID)["MapID"].YuanColumnText;
                }

                tempLevel = 0;
                foreach (string itemPlace in strMyPlace)
                {
                    if (itemPlace != "")
                    {
                        if (itemPlace.Substring(0, 3) == levelID)
                        {
                            if (int.Parse(itemPlace.Substring(3, 1)) >= levelL)
                            {
                                tempLevel = 1;
                            }
                            continue;
                        }
                    }
                }
                if (tempLevel == 0)
                {
                    break;
                }

                if (num < listBtnTeamForCreate.Count)
                {
                    SetTeamPlayerBtn(listBtnTeamForCreate[num], teamID, teamHeadID, teamName, teamInfo, strTeamMemver);
                    listBtnTeamForCreate[num].gameObject.SetActiveRecursively(true);
                }
                else
                {
                    GameObject objBtn = (GameObject)Instantiate(btnTeamForCreate);
                    objBtn.transform.parent = gridTeam.transform;
                    objBtn.transform.localScale = new Vector3(1, 1, 1);
                    objBtn.transform.localPosition = Vector3.zero;
                    UIButtonMessage btnMessage = objBtn.GetComponent<UIButtonMessage>();
                    btnMessage.target = this.gameObject;
                    btnMessage.functionName = "BtnTeamForCreateClick";
                    BtnTeamForCreate tempBtn = objBtn.GetComponent<BtnTeamForCreate>();
//                    tempBtn.GetComponent<UIToggle>().radioButtonRoot = gridTeam.transform;
                    listBtnTeamForCreate.Add(tempBtn);
                    SetTeamPlayerBtn(tempBtn, teamID, teamHeadID, teamName, teamInfo, strTeamMemver);
                }

                num++;
            }
        }
        gridTeam.repositionNow = true;
    }

    /// <summary>
    /// 设置队伍列表的按钮状态
    /// </summary>
    /// <param name="btnPlayer">按钮</param>
    /// <param name="mTeamID">队伍ID</param>
    /// <param name="mTeamName">队伍名称</param>
    /// <param name="mTeamInfo">队伍信息</param>
    /// <param name="mStrTeamMemcer">队伍成员</param>
	public void SetTeamPlayerBtn(BtnTeamForCreate btnPlayer,string mTeamID,string mTeamHeadID,string mTeamName,string mTeamInfo,string[] mStrTeamMemcer,string mTeamPassWard="",string mInsTensID="")
	{

        btnPlayer.teamID = mTeamID;
        btnPlayer.teamHeadID = mTeamHeadID;


        
        string[] czteam = mTeamName.Split(',');
        string mapname = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapID", czteam[0])["MapName"].YuanColumnText;

        if (czteam[1] == "1")
        {
            btnPlayer.lblName.text = mapname + " " + StaticLoc.Loc.Get("info997");
        }
        else if (czteam[1] == "2")
        {
			btnPlayer.lblName.text = mapname + " " +  StaticLoc.Loc.Get("info998");
        }
        else if (czteam[1] == "3")
        {
			btnPlayer.lblName.text = mapname + " " +  StaticLoc.Loc.Get("info999");
        }
        else if (czteam[1] == "5")
        {
			btnPlayer.lblName.text = mapname + " " +  StaticLoc.Loc.Get("info1000");
        }
        //btnPlayer.lblName.text = mTeamName;
        btnPlayer.lblInfo.text = mTeamInfo;
		btnPlayer.teamPassWard = mTeamPassWard;
		btnPlayer.teamInstensId = mInsTensID;
        int numPlayer = 0;
        string[] strPlayer;
		List<BtnTeamPlayer> Btp  = new List<BtnTeamPlayer>();
        foreach (string player in mStrTeamMemcer)
        {
            if (player != "")
            {
                strPlayer = player.Split(',');
                BtnTeamPlayer tempBtnPlayer = btnPlayer.listPlayer[numPlayer].GetComponent<BtnTeamPlayer>();
                tempBtnPlayer.PlayerID = strPlayer[0];
//				if(strPlayer.Length>2)//TempTeamPlayers
//				{
//					tempBtnPlayer.PlayerPro=strPlayer[1];
//					tempBtnPlayer.PlayerName=strPlayer[2];
//					tempBtnPlayer.PlayerLevel=strPlayer[3];
//				}
//				Btp.Add(tempBtnPlayer);
//				objTeam.SendMessage("ShowFriendPlayer",Btp,SendMessageOptions.DontRequireReceiver);
                tempBtnPlayer.yt = null;
                UISprite tempPic = btnPlayer.listPlayer[numPlayer].transform.FindChild("pic").GetComponent<UISprite>();
                tempPic.atlas = yuanPicManager.picPlayer[int.Parse(strPlayer[1])-1].atlas;
                tempPic.spriteName = yuanPicManager.picPlayer[int.Parse(strPlayer[1])-1].spriteName;
                btnPlayer.listPlayer[numPlayer].target = this.gameObject;
                btnPlayer.listPlayer[numPlayer].functionName = "TeamPlayerBtnClick";
                
                numPlayer++;
            }
        }
    }

    private CaptainInfo captainInfo;
    /// <summary>
    /// 队伍列表队员按钮
    /// </summary>
    /// <param name="obj"></param>
    public IEnumerator TeamPlayerBtnClick(GameObject obj)
    {
        BtnTeamPlayer btnPlayer = obj.GetComponent<BtnTeamPlayer>();
		if(btnPlayer!=null)
		{
	        if (btnPlayer.yt == null)
	        {
	            btnPlayer.yt = new yuan.YuanMemoryDB.YuanTable("player" + btnPlayer.PlayerID, "");
	        }
			if(btnPlayer.yt.Count==0)
			{
				InRoom.GetInRoomInstantiate().GetPlayerList(new string[1] { btnPlayer.PlayerID }, btnPlayer.yt, "DarkSword2", "PlayerInfo");
			}
	        while (true)
	        {
	            if (btnPlayer.yt != null && btnPlayer.yt.Count > 0)
	            {
	                break;
	            }
	            yield return new WaitForSeconds(1);
	        }
			
			if(captainInfo!=null)
			{
		        captainInfo.yr = btnPlayer.yt[0];
		        captainInfo.RefreshPlayerInfo();
			}
		}
    }

    /// <summary>
    /// 队伍按钮单击
    /// </summary>
    /// <param name="obj"></param>
    public void BtnTeamForCreateClick(GameObject obj)
    {
        captainInfo.teamID = obj.GetComponent<BtnTeamForCreate>().teamID;
    }


    /// <summary>
    /// 加入队伍
    /// </summary>
    public void TeamAdd()
    {
        if (captainInfo.teamID != string.Empty)
        {
            InRoom.GetInRoomInstantiate().TeamAdd(captainInfo.teamID);
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info433"));
        }
            
    }

    /// <summary>
    /// 同意加入队伍
    /// </summary>
    public void GetTeamRequsetYes()
    {
        getTeamReturnCode = yuan.YuanPhoton.ReturnCode.Yes;
        GetTeamRequset();
        warnings.warningAllEnterClose.gameObject.SetActiveRecursively(false);
    }

    /// <summary>
    /// 拒绝加入队伍
    /// </summary>
    public void GetTeamRequsetNo()
    {
        getTeamReturnCode = yuan.YuanPhoton.ReturnCode.No;
        GetTeamRequset();
    }


    //[HideInInspector]
    //public yuan.YuanPhoton.RequstType getTeamRequstType;
    [HideInInspector]
    private yuan.YuanPhoton.ReturnCode getTeamReturnCode;
    public Dictionary<short, object> dicTeamParameter;
    /// <summary>
    /// 回应加入副本请求
    /// </summary>
    public void GetTeamRequset()
    {
        InRoom.GetInRoomInstantiate().ReturnRequest(getTeamReturnCode, dicTeamParameter);
    }


    

    private UIInput txtGropsCreate;
    private GetPic gpGropsGreate;
    private UIToggle strCkbGropsType0;
	private UIToggle strCkbGropsType1;
    /// <summary>
    /// 战队创建按钮按下
    /// </summary>
    public void BtnGropsCreateClick()
    {
        if (txtGropsCreate.text.Trim() != "")
        {
            if (int.Parse(BtnGameManager.yt.Rows[0]["Money"].YuanColumnText.Trim()) >= 500)
            {
                string playerNum = string.Empty;
                if (strCkbGropsType0.value)
                {
                    playerNum = "2";
                }
                else if (strCkbGropsType1.value)
                {
                    playerNum = "4";
                }
//                Debug.Log("---------------------");
                InRoom.GetInRoomInstantiate().GorpsCreate(txtGropsCreate.text.Trim(), playerNum, gpGropsGreate.PicID,"DarkSword2", "Corps");
                BtnClose();
            }
            else
            {
                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info434"));
            }
            
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info435"));
        }
    }

    /// <summary>
    /// 刷新战队列表
    /// </summary>
    public void RefreshGropsList()
    {
        InRoom.GetInRoomInstantiate().GetCorps("1", "DarkSword2", "Corps");
    }

    /// <summary>
    /// 刷新军团列表
    /// </summary>
    public void RefreshLegionList()
    {
        InRoom.GetInRoomInstantiate().GetLegion("123");
    }

    public UIGrid gridCrops;
    public UIGrid gridGuild;
    public GameObject btnArena;
	private PanelListTabel tableTempTeam;
    public PanelListTabel tablePVP2;
    public PanelListTabel tablePVP4;
    public PanelListTabel tableGuild;
    [HideInInspector]
    public List<BtnArena> listCropsBtns = new List<BtnArena>();
    [HideInInspector]
    public List<BtnArena> listPVP4Btns = new List<BtnArena>();
    [HideInInspector]
    public List<BtnArena> listGuildBtns = new List<BtnArena>();
    /// <summary>
    /// 获得战队回调
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="mGrid"></param>
    public void GetCropsOK(yuan.YuanMemoryDB.YuanTable mYt,UIGrid mGrid,List<BtnArena> mListBtns,string btnFunction,DegSetBtn degSetBtn,string mName)
    {

        
        int num = 0;
        
        foreach (BtnArena item in mListBtns)
        {
            item.gameObject.SetActiveRecursively(false);
			UIToggle ckb=item.GetComponent<UIToggle>();
			ckb.value=false;
        }

        foreach (yuan.YuanMemoryDB.YuanRow item in mYt.Rows)
        {
            

                if (mName != "")
                {
                    if (item["PlayerNumber"].YuanColumnText != mName)
                    {
                        continue;
                    }
                }
                if (num < mListBtns.Count)
                {
                    degSetBtn(mListBtns[num], item);
                    mListBtns[num].gameObject.SetActiveRecursively(mGrid.gameObject.active);
                }
                else
                {
                    GameObject tempObjBtnArena = (GameObject)Instantiate(btnArena);
                    tempObjBtnArena.transform.parent = mGrid.transform;

                    UIButtonMessage message = tempObjBtnArena.GetComponent<UIButtonMessage>();
                    message.target = this.gameObject;
                    message.functionName = btnFunction;

                    BtnArena tempBtnArena = tempObjBtnArena.GetComponent<BtnArena>();
                    tempBtnArena.getPic.yuanPicManager = this.yuanPicManager;
                    tempBtnArena.transform.localPosition = Vector3.zero;
					
					UIToggle ckb=tempObjBtnArena.GetComponent<UIToggle>();
					ckb.group=1;
					
                    degSetBtn(tempBtnArena, item);
                    tempBtnArena.gameObject.SetActiveRecursively(mGrid.gameObject.active);
                    mListBtns.Add(tempBtnArena);
                }
                num++;
            
        }
        while (num+1 < listCropsBtns.Count)
        {
            SetCropsBtnEmpty(listCropsBtns[num]);
            num++;
        }

        mGrid.repositionNow = true;
    }



    private UIInput txtSearchCropsName;
   /// <summary>
   /// 搜索战队按钮按下
   /// </summary>
    public void SearchCropsBtnClick()
    {
        //if (txtSearchCropsName.text.Trim() != "")
        //{
            SearchBtn(listCropsBtns, txtSearchCropsName.text.Trim(), true);
            gridCrops.repositionNow = true;
        //}
        //else
        //{
        //    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), "请先输入要搜索的内容");
        //}
    }

    private UIInput txtSearchLegionName;
    /// <summary>
    /// 搜索军团按钮按下
    /// </summary>
    public void SerchLegionBtnClick()
    {
        SearchBtn(listBtnLengion, txtSearchLegionName.text.Trim(), true);
    }

    private UIInput txtSearchGuildName;
    /// <summary>
    /// 搜索公会按钮按下
    /// </summary>
    public void SerchGuildBtnClick()
    {
		if(!string.IsNullOrEmpty(txtSearchGuildName.text)){
        SearchBtn(listGuildBtns, txtSearchGuildName.text.Trim(), true);
		}else{
			warnings.warningAllTime.Show("",StaticLoc.Loc.Get("info986"));
		}
    }

    /// <summary>
    /// 队伍按钮按下
    /// </summary>
    public void BtnCropsClick(GameObject mObj)
    {
        BtnArena tempBtn = mObj.GetComponent<BtnArena>();
        SetCropsTeamInfo(tempBtn, teamInfoForCrops);
    }

    /// <summary>
    /// pvp4按钮按下
    /// </summary>
    /// <param name="mObj"></param>
    public void BtnPVP4Click(GameObject mObj)
    {
        BtnArena tempBtn = mObj.GetComponent<BtnArena>();
        SetCropsTeamInfo(tempBtn, legionTeamInfo);
    }

   /// <summary>
   /// 公会按钮按下
   /// </summary>
   /// <param name="obj"></param>
    public void BtnGuildClick(GameObject obj)
    {
        BtnArena tempBtn = obj.GetComponent<BtnArena>();
        //SetCropsTeamInfo(tempBtn, guildTeamInfo);
		guildTeamInfo.yr=tempBtn.yr;
		guildTeamInfo.RefreshInfoGuild();
    }


    private CropsTeamInfo teamInfoForCrops;
    /// <summary>
    /// 队伍类按钮按下
    /// </summary>
    /// <param name="mBtn"></param>
    /// <param name="mTeamInfo"></param>
    private  void SetCropsTeamInfo(BtnArena mBtn,CropsTeamInfo mTeamInfo)
    {
        mTeamInfo.yr = mBtn.yr;
        mTeamInfo.RefreshInfo();
    }

    public delegate void DegSetBtn(BtnArena mBtn, yuan.YuanMemoryDB.YuanRow item);

    /// <summary>
    /// 设置战队类型的按钮
    /// </summary>
    /// <param name="mBtn">按钮</param>
    /// <param name="item"></param>
    /// <param name="mGrid"></param>
    public void SetCropsBtn(BtnArena mBtn,yuan.YuanMemoryDB.YuanRow item)
    {
        mBtn.yr = item;
        mBtn.getPic.yuanPicManager = this.yuanPicManager;
        mBtn.getPic.PicID = item["PicID"].YuanColumnText.Trim();
        mBtn.lblName.text = item["Name"].YuanColumnText.Trim();
        mBtn.lblInfo.text = item["SelfLevel"].YuanColumnText.Trim();
        mBtn.transform.localScale = new Vector3(1, 1, 1);
    }

   /// <summary>
   /// 设置公会类型按钮
   /// </summary>
   /// <param name="mBtn"></param>
   /// <param name="item"></param>
   /// <param name="mGrid"></param>
    public void SetGuildBtn(BtnArena mBtn, yuan.YuanMemoryDB.YuanRow item)
    {
        mBtn.yr = item;
        mBtn.getPic.yuanPicManager = this.yuanPicManager;
        mBtn.getPic.PicID = item["PicID"].YuanColumnText.Trim();
        mBtn.lblName.text = item["GuildName"].YuanColumnText.Trim();
        mBtn.lblInfo.text = item["GuildLevel"].YuanColumnText.Trim();
        mBtn.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// 清空战队类型按钮的信息
    /// </summary>
    /// <param name="mBtn"></param>
    private void SetCropsBtnEmpty(BtnArena mBtn)
    {
        mBtn.yr = null;
        mBtn.getPic.PicID = "0,0,0";
        mBtn.lblName.text = string.Empty;
        mBtn.lblInfo.text = string.Empty;
    }

    /// <summary>
    /// 搜索战队类按钮
    /// </summary>
    /// <param name="mListBtn">按钮列表</param>
    /// <param name="mText">搜索的字符串</param>
    /// <param name="setEnable">是否关闭未搜到的按钮</param>
    /// <returns></returns>
    private List<BtnArena> SearchBtn(List<BtnArena> mListBtn,string mText,bool setEnable)
    {
        List<BtnArena> tempListBtn = new List<BtnArena>();
        if (mText != "")
        {
            foreach (BtnArena item in mListBtn)
            {
                if (item.lblName.text != "" && item.lblName.text.IndexOf(mText) != -1)
                {
                    tempListBtn.Add(item);
                }
                else if (setEnable)
                {
                    item.gameObject.SetActiveRecursively(false);
                }
            }
        }
        else
        {
            Vector3 tempScale = new Vector3(1, 1, 1);
            foreach (BtnArena item in mListBtn)
            {
                if (item.lblName.text != "")
                {
                    item.gameObject.SetActiveRecursively(true);
                    item.transform.localScale = tempScale;
                }
            }
        }
        return tempListBtn;
    }

    /// <summary>
    /// 搜索军团类按钮
    /// </summary>
    /// <param name="mListBtn">按钮列表</param>
    /// <param name="mText">搜索的字符串</param>
    /// <param name="setEnable">是否关闭未搜到的按钮</param>
    /// <returns></returns>
    private List<BtnLengionDB> SearchBtn(List<BtnLengionDB> mListBtn, string mText, bool setEnable)
    {
        List<BtnLengionDB> tempListBtn = new List<BtnLengionDB>();
        if (mText != "")
        {
            foreach (BtnLengionDB item in mListBtn)
            {
                if (item.lblName.text != "" && item.lblName.text.IndexOf(mText) != -1)
                {
                    tempListBtn.Add(item);
                }
                else if (setEnable)
                {
                    item.gameObject.SetActiveRecursively(false);
                }
            }
        }
        else
        {
            Vector3 tempScale = new Vector3(1, 1, 1);
            foreach (BtnLengionDB item in mListBtn)
            {
                if (item.lblName.text != "")
                {
                    item.gameObject.SetActiveRecursively(true);
                    item.transform.localScale = tempScale;
                }
            }
        }
        return tempListBtn;
    }

    /// <summary>
    /// 加入战队按钮按下
    /// </summary>
    public void CropsAddBtnClick()
    {
		if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.PVP2Switch)!="1")
		{
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info645"));
			return;
		}			
        if (teamInfoForCrops.yr != null)
        {
            InRoom.GetInRoomInstantiate().GorpsAdd(teamInfoForCrops.id, "DarkSword2", "Corps");
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info436") );
        }
    }

    public void PVP4AddBtnClick()
    {
		if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.PVP4Switch)!="1")
		{
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info645"));
			return;
		}		
        if (legionTeamInfo != null)
        {
            InRoom.GetInRoomInstantiate().GorpsAdd(legionTeamInfo.id, "DarkSword2", "Corps");
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info436"));
        }
    }

    private UIToggle cbxLegionTemp;
    private UIToggle cbxLegionForever;
    private GameObject objsLegionPicPanel;
    /// <summary>
    /// 军团创建面板，类型选择按钮
    /// </summary>
    public void SelectLegionType()
    {
        if (cbxLegionTemp.value)
        {
            //yuan.YuanClass.SwitchList(objsLegionPicPanel, false, true);
			objsLegionPicPanel.gameObject.SetActiveRecursively (false);
        }
        else if (cbxLegionForever.value)
        {
            //yuan.YuanClass.SwitchList(objsLegionPicPanel, true, true);
			objsLegionPicPanel.gameObject.SetActiveRecursively (false);
        }

    }

    
    private List<BtnLengionDB> listBtnLengion = new List<BtnLengionDB>();
    public UIGrid gridLegion;
    public GameObject objBtnLegion;
    /// <summary>
    /// 设置军团列表
    /// </summary>
    /// <param name="dicLegion"></param>
    public void GetLegion(Dictionary<short,object> dicLegion)
    {
        foreach (BtnLengionDB item in listBtnLengion)
        {
            item.gameObject.SetActiveRecursively(false);
        }
        int num = 0;
        Dictionary<byte,string> dicTemp;
        foreach (KeyValuePair<short, object> item in dicLegion)
        {
            dicTemp=(Dictionary<byte,string>)item.Value;
            yuan.YuanPhoton.LegionType legionType = (yuan.YuanPhoton.LegionType)int.Parse(dicTemp[(byte)yuan.YuanPhoton.ParameterType.LegionType]);
            string legionID = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.TeamID];
            string legionName = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.TeamName];
            string legionInfo = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.TeamInfo];
            string legionHeadID = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.TeamHeadID];
            string legionMemver = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.TeamMemver];
            switch (legionType)
            {
                case yuan.YuanPhoton.LegionType.TempLegion:
                    {
                        string picID = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.PicID];
                        if(num<listBtnLengion.Count)
                        {
                            SetLegionTempBtn(listBtnLengion[num], legionID, legionName, legionInfo, legionHeadID, legionMemver,picID);

                        }
                        else
                        {
                            BtnLengionDB btnLegion = ((GameObject)Instantiate(objBtnLegion)).GetComponent<BtnLengionDB>();
                            btnLegion.getPic.yuanPicManager = this.yuanPicManager;
                            btnLegion.transform.parent = gridLegion.transform;
                            btnLegion.transform.localScale = new Vector3(1, 1, 1);
                            btnLegion.transform.localPosition = Vector3.zero;
                            UIButtonMessage btnMessage = btnLegion.GetComponent<UIButtonMessage>();
                            btnMessage.target = this.gameObject;
                            btnMessage.functionName = "LegionBtnClick";
                            //SetLegionTempBtn(listBtnLengion[num], legionID, legionName, legionInfo, legionHeadID, legionMemver);
                            SetLegionTempBtn(btnLegion, legionID, legionName, legionInfo, legionHeadID, legionMemver,picID);
                            listBtnLengion.Add(btnLegion);
                        }
                    }
                    break;
                case yuan.YuanPhoton.LegionType.DBLegion:
                    {
                        string legionPicID = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.PicID];
                        string legionLevel = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.TeamLevel];
                        string legionRanking = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.TeamRanking];
                        string legionDepHeadID = (string)dicTemp[(byte)yuan.YuanPhoton.ParameterType.TeamDepHeadID];
                        if (num < listBtnLengion.Count)
                        {
                            SetLegionDBBtn(listBtnLengion[num], legionID, legionPicID, legionName, legionLevel, legionRanking, legionInfo, legionMemver, legionHeadID, legionDepHeadID);
                        }
                        else
                        {
                            BtnLengionDB btnLegion = ((GameObject)Instantiate(objBtnLegion)).GetComponent<BtnLengionDB>();
                            btnLegion.transform.parent = gridLegion.transform;
                            btnLegion.transform.localScale = new Vector3(1, 1, 1);
                            btnLegion.transform.localPosition = Vector3.zero;
                            UIButtonMessage btnMessage = btnLegion.GetComponent<UIButtonMessage>();
                            btnMessage.target = this.gameObject;
                            btnMessage.functionName = "LegionBtnClick";
                            //SetLegionDBBtn(listBtnLengion[num], legionID, legionPicID, legionName, legionLevel, legionRanking, legionInfo, legionMemver, legionHeadID, legionDepHeadID);
                            SetLegionDBBtn(btnLegion, legionID, legionPicID, legionName, legionLevel, legionRanking, legionInfo, legionMemver, legionHeadID, legionDepHeadID);
                            listBtnLengion.Add(btnLegion);
                        }
                       
                    }
                    break;
            }
            num++;
        }
        gridLegion.repositionNow=true;
    }

    private CropsTeamInfo legionTeamInfo;
    private void LegionBtnClick(GameObject obj)
    {
        BtnLengionDB btnLegion = obj.GetComponent<BtnLengionDB>();
        legionTeamInfo.getPic.PicID = btnLegion.getPic.PicID;
        legionTeamInfo.id = btnLegion.teamID;
        legionTeamInfo.legionType = btnLegion.legionType;
        legionTeamInfo.lblHeaderName.text = btnLegion.teamName;
        legionTeamInfo.lblLevel.text = btnLegion.teamLevel;
        legionTeamInfo.lblName.text = btnLegion.teamHeadID;
        legionTeamInfo.lblRanking.text = btnLegion.teamRanking;

    }

   /// <summary>
   /// 设置自由军团按钮·
   /// </summary>
   /// <param name="btnLegion"></param>
   /// <param name="mTeamID"></param>
   /// <param name="mTeamName"></param>
   /// <param name="mTeamInfo"></param>
   /// <param name="mTeamHeadID"></param>
   /// <param name="mTeamMemver"></param>
    private void SetLegionTempBtn(BtnLengionDB btnLegion,string mTeamID,string mTeamName,string mTeamInfo,string mTeamHeadID,string mTeamMemver,string mPicID)
    {
        btnLegion.legionType = yuan.YuanPhoton.LegionType.TempLegion;
        btnLegion.teamID = mTeamID;
        btnLegion.teamName = mTeamName;
        btnLegion.teamInfo = mTeamInfo;
        btnLegion.teamHeadID = mTeamHeadID;
        btnLegion.teamMemver = mTeamMemver;
        btnLegion.getPic.PicID = mPicID;
        btnLegion.gameObject.SetActiveRecursively(true);
        //btnLegion.teamPicID = mTeamPicID;
        btnLegion.SetMyInfo();
        btnLegion.transform.localScale = new Vector3(1, 1, 1);

    }

    /// <summary>
    /// 设置铁血军团按钮
    /// </summary>
    /// <param name="btnLegion"></param>
    /// <param name="mTeamID"></param>
    /// <param name="mPicID"></param>
    /// <param name="mTeamName"></param>
    /// <param name="mTeamLevel"></param>
    /// <param name="mTeamRanking"></param>
    /// <param name="mTeamInfo"></param>
    /// <param name="mTeamMemver"></param>
    /// <param name="mTeamHeadID"></param>
    /// <param name="mTeamDepHeadID"></param>
    private void SetLegionDBBtn(BtnLengionDB btnLegion,string mTeamID,string mPicID,string mTeamName,string mTeamLevel,string mTeamRanking,string mTeamInfo,string mTeamMemver,string mTeamHeadID,string mTeamDepHeadID)
    {
        btnLegion.legionType = yuan.YuanPhoton.LegionType.DBLegion;
        btnLegion.teamID = mTeamID;
        btnLegion.getPic.yuanPicManager = this.yuanPicManager;
        btnLegion.getPic.PicID = mPicID;
        btnLegion.teamName = mTeamName;
        btnLegion.teamLevel = mTeamLevel;
        btnLegion.teamRanking = mTeamRanking;
        btnLegion.teamInfo = mTeamInfo;
        btnLegion.teamHeadID = mTeamHeadID;
        btnLegion.teamMemver = mTeamMemver;
        btnLegion.teamDepHeadID = mTeamDepHeadID;
        btnLegion.SetMyInfo();
        btnLegion.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// 加入军团按钮按下
    /// </summary>
    public void LegionAddBtnClick()
    {
        if (legionTeamInfo.id != "")
        {
            if (legionTeamInfo.legionType==yuan.YuanPhoton.LegionType.DBLegion)
            {
                InRoom.GetInRoomInstantiate().LegionDBAdd(legionTeamInfo.id);
            }
            else if (legionTeamInfo.legionType == yuan.YuanPhoton.LegionType.TempLegion)
            {
                InRoom.GetInRoomInstantiate().LegionTempAdd(legionTeamInfo.id);
            }
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info437") );
        }
    }

    private GetPic gpGuildCreate;
    private UIInput txtGuildCreateName;
   /// <summary>
   /// 创建公会按钮按下
   /// </summary>
    public void BtnGuildCreateClick()
    {
//		bool random = GetRandomName.HasShieldedWord(txtGuildCreateName.text.Trim());
		if(GetRandomName.HasShieldedWord(txtGuildCreateName.text.Trim())==false){
        if (txtGuildCreateName.text.Trim() != "")
        {
			PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate().GuildCreate(txtGuildCreateName.text.Trim(), gpGuildCreate.PicID));
//			BtnClose();
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info438") );
        }
		}else{
			warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info527") );
		}
    }


    private CropsTeamInfo guildTeamInfo;
   /// <summary>
   /// 加入公会按钮按下
   /// </summary>
    public void BtnGuildAddClick()
    {
		if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.GuildSwitch)!="1")
		{
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info645"));
			return;
		}		
        if (guildTeamInfo.id != "")
        {
            InRoom.GetInRoomInstantiate().GuildAdd(guildTeamInfo.id);
        }

    }
    /// <summary>
    /// 公会升级确认发送内容
    /// </summary>
    public void YesGuildLevelUP()
    {
        InRoom.GetInRoomInstantiate().GuildLevelUp(BtnGameManager.yt[0]["GuildID"].YuanColumnText, true);
        warnings.warningAllEnterClose.Close();
    }
   
   /// <summary>
   /// 公会列表刷新
   /// </summary>
    public void BtnGuildRefresh()
    {
        InRoom.GetInRoomInstantiate().GetGuildAll();
    }

    private RefreshGuildBuild refreshGuildBuild;
    /// <summary>
    /// 公会建设面板刷新
    /// </summary>
    public void PanelGuildBuildRefresh()
    {
        refreshGuildBuild.OnEnable();
    }

    /// <summary>
    /// 公会金币建设
    /// </summary>
    public void BtnGoldBuildClick()
    {
        if (yt.Rows[0]["GuildID"].YuanColumnText != "")
        {
            InRoom.GetInRoomInstantiate().GuildBuild(yt.Rows[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.MoneyType.Gold);
        }
    }

    /// <summary>
    /// 公会血石建设
    /// </summary>
    public void BtnBloodStoneBuildClick()
    {
        if (yt.Rows[0]["GuildID"].YuanColumnText != "")
        {
            InRoom.GetInRoomInstantiate().GuildBuild(yt.Rows[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.MoneyType.BloodStone);
        }
    }

    /// <summary>
    /// 公会金币捐献
    /// </summary>
    public void BtnGoldDonateClick()
    {
        if (yt.Rows[0]["GuildID"].YuanColumnText != "")
        {
 //           InRoom.GetInRoomInstantiate().GuildFunds(yt.Rows[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.MoneyType.Gold);
        }
    }

    /// <summary>
    /// 公会血石捐献
    /// </summary>
    public void BtnBloodStoneDonateClick()
    {
        if (yt.Rows[0]["GuildID"].YuanColumnText != "")
        {
//			StartCoroutine (OpenLoading (()=> InRoom.GetInRoomInstantiate().GuildFunds(yt.Rows[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.MoneyType.BloodStone)));
			
        }
    }

    /// <summary>
    /// 加入临时队伍
    /// </summary>
	public UIInput txtAddTempTeamPwd;
    public void BtnTempTeamAdd()
    {
        string teamName = string.Empty;
        UIToggle[] tempSelectDuplicate = gridSelectDuplicate.GetComponentsInChildren<UIToggle>();
        UIToggle[] tempSelectLevel = gridSelectLevel.GetComponentsInChildren<UIToggle>();
        foreach (UIToggle item in tempSelectDuplicate)
        {
            if (item.value)
            {
                teamName = item.GetComponentInChildren<UILabel>().text.Trim();
                break;
            }
        }
        foreach (UIToggle item in tempSelectLevel)
        {
            if (item.value)
            {
                teamName += " " + item.GetComponentInChildren<UILabel>().text.Trim();
                break;

            }
        }
        InRoom.GetInRoomInstantiate().AddTempTeam(teamName,"");
        BtnClose();
    }

    /// <summary>
    /// 加入已有队伍
    /// </summary>
    public void BtnTempTeamHasAdd()
    {
        if (captainInfo.teamID != string.Empty)
        {
           // InRoom.GetInRoomInstantiate().AddTempTeam(selectTempListName, captainInfo.teamID);
            InRoom.GetInRoomInstantiate().AddTempTeam("123", captainInfo.teamID);
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info439") );
        }
    }

    /// <summary>
    /// 退出排队
    /// </summary>
    public void RemoveTempTeam()
    {
//        InRoom.GetInRoomInstantiate().RemoveTempTeam();
    }

    
    private static bool isInTempTeam = false;
    /// <summary>
    /// 是否正在排队
    /// </summary>
    public static bool IsInTempTeam
    {
        get { return BtnGameManager.isInTempTeam; }
        set 
        { 
            BtnGameManager.isInTempTeam = value; 
        }
    }

    private GameObject setTempTeam;
    /// <summary>
    /// 退出排队
    /// </summary>
    public void OutTempTeam()
    {
		try
		{
			if(null!=setTempTeam)
			{
	        	setTempTeam.SetActiveRecursively(false);
			}
	        this.GetTempTeam();
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
    }
	

	
    //private string strAdress = "192.168.1.101:5059";
    /// <summary>
    /// 退出登录
    /// </summary>
    public void GoMainMenu()
    {
        warnings.warningAllEnterClose.btnEnter.target=this.gameObject;
		warnings.warningAllEnterClose.btnEnter.functionName="GoMainMenuOK";
		warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info609"));
    }


	
	public void GoMainMenuOK()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
//        PhotonNetwork.LeaveRoom();
        MainMenuManage.gameLoginType = MainMenuManage.GameLoginType.MainMenu;
        if (!YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerConnected)
        {
            YuanUnityPhoton.NewYuanUnityPhotonInstantiate().ServerAddress = PlayerPrefs.GetString("TestServer");
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerApplication = PlayerPrefs.GetString("TestServerName");
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().Connect();
        }
		if(Application.platform!=RuntimePlatform.Android&&Application.platform!=RuntimePlatform.IPhonePlayer)
		{
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
			warnings.warningAllEnterClose.Close ();			
		}
#if UNITY_ANDROID || UNITY_IOS
		LogoutSDK();
#endif
		//Application.LoadLevel(0);
    }
	
	public void LogoutSDK()
	{
#if UNITY_ANDROID
#if SDK_AZ	
		Exit.ExitSDK();
#elif SDK_CMGE	
		Exit.ExitSDK();
#elif SDK_DOWN	
		Exit.ExitSDK();
#elif SDK_DUOKU	
		SDKManager.LogoutSDK();
#elif SDK_HUAWEI	
		Exit.ExitSDK();
#elif SDK_ITOOLS	
		Exit.ExitSDK();
#elif SDK_JYIOS	
		Exit.ExitSDK();
#elif SDK_LENOVO
		Exit.ExitSDK();
#elif SDK_MI
		Exit.ExitSDK();
#elif SDK_MUZI
		Exit.ExitSDK();
#elif SDK_OPPO
		Exit.ExitSDK();
#elif SDK_PEASECOD
		SDKManager.LogoutSDK();
#elif SDK_PP
		Exit.ExitSDK();
#elif SDK_QH
		SDKManager.LogoutSDK();
#elif SDK_TONGBU
		Exit.ExitSDK();
#elif SDK_UC
		SDKManager.LogoutSDK();
#elif SDK_VIVO
		Exit.ExitSDK();
#else
		Exit.ExitSDK();
#endif
//		SDKManager.LogoutSDK();
			//Exit.ExitSDK();
		//Misdk_login.loginout ();//小米注销
//		UCGameSdk.logout ();//UC注销
		//DlBillingAndroid.Instance.Logout();//当乐注销
#elif UNITY_IOS
#if SDK_AZ
#elif SDK_JYIOS
		SdkConector.NdLogout(1);
#elif SDK_ITOOLS	
		ItoolsSdkControl.ItoolSDKLogout();
#elif SDK_KUAIYONG	
		KYSdkControl.KYSDKLogout ();
#elif SDK_XY
		XYSDKControl.XYSDKLogout ();
#elif SDK_I4
		ASSDKControl.ASSDKLogout ();
#elif SDK_ZSY
		ZSYSDKControl.ZSYSDKLogout ();
#elif SDK_ZSYIOS
		ZSYSDKControl.ZSYSDKLogout ();
#elif SDK_PP
		PPSdkControl.PPSdkLogout ();
#elif SDK_TONGBU
		TBSdkControl.TBSdkLogout ();
#elif SDK_HM
		HMSdkControl.HMSdkLogout ();
#endif

#endif

	}
	
	
	/// <summary>
	/// 退回角色选择列表
    /// </summary>
    public void GoPlayerList()
    {
        warnings.warningAllEnterClose.btnEnter.target=this.gameObject;
		warnings.warningAllEnterClose.btnEnter.functionName="GoPlayerListOK";
		warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info610"));
    }
	
    public void GoPlayerListOK()
    {
		ObjectAccessor.clearAOIObject();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
//        PhotonNetwork.LeaveRoom();
        MainMenuManage.gameLoginType = MainMenuManage.GameLoginType.PlayerList;
		//InRoom.GetInRoomInstantiate ().peer.Disconnect ();
        //ZealmConnector.closeConnection();
       //if (!YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerConnected)
       //{
       //    YuanUnityPhoton.NewYuanUnityPhotonInstantiate().ServerAddress = PlayerPrefs.GetString("TestServer") + ":5059";
       //    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerApplication = PlayerPrefs.GetString("TestServerName");
       //    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().Connect();
       //}
		//while(InRoom.GetInRoomInstantiate ().peer.PeerState!=ExitGames.Client.Photon.PeerStateValue.Disconnected)
		//{
		//	yield return new WaitForSeconds(0.1f);
		//}	
		try
		{
	        //InRoom.NewInRoomInstantiate().ServerAddress = PlayerPrefs.GetString("InAppServerIP");
	        //InRoom.GetInRoomInstantiate().ServerApplication = PlayerPrefs.GetString("InAppServer");
	        //InRoom.GetInRoomInstantiate().Connect();
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",1,SendMessageOptions.DontRequireReceiver);
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
		warnings.warningAllEnterClose.Close ();
        //Application.LoadLevel(1);
    }	

	private GameObject objBind;
	public IEnumerator OpenBindPalyer()
	{
		if(objBind==null)
		{
			GameObject obj= (GameObject)Resources.Load ("Anchor - PlayerBind");
				obj=(GameObject)Instantiate (obj);
			obj.transform.parent=tranBindPath;
			obj.transform.localPosition=new Vector3(-0.11866f,100.098f,-3.569458f);
			obj.transform.localScale=new Vector3(0.0025f,0.0025f,0.0025f);
			objBind=obj;
			yield return new WaitForSeconds(0.5f);
		}
		objBind.SetActiveRecursively (true);
	}
	
    private UIInput txtPlayerBindName;
    private UIInput txtPlayerBindPwd;
    private UILabel lblPlayerBindWarning;
    /// <summary>
    /// 绑定账号
    /// </summary>
    public void BindPlayer()
    {

        if (BtnGameManager.yt.Rows[0]["isRegister"].YuanColumnText.Trim() == "1")
        {
            lblPlayerBindWarning.text = StaticLoc.Loc.Get("info311");
        }
        else
        {
            if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerConnected)
            {
                if (txtPlayerBindName.text.Trim() != "" && txtPlayerBindPwd.text.Trim() != "")
                {
//                    UIToggle[] ckbTemp=gridServerBind.GetComponentsInChildren<UIToggle>();
//                    foreach(UIToggle ckb in ckbTemp)
//                    {
//                        if(ckb.value)
//                        {
//                    BtnServer btnServer=ckb.GetComponent<BtnServer>();
                    InRoom.GetInRoomInstantiate ().BindUserID(txtPlayerBindName.text.Trim(), txtPlayerBindPwd.text.Trim(), "123456", "ZealmPass", "UserInfo");
//                        }
//                    }
                }
                else
                {
                    lblPlayerBindWarning.text = StaticLoc.Loc.Get("info309");
                }
            }
        }
    }

	/// <summary>
	/// 私聊
	/// </summary>
	/// <param name="mID">M I.</param>
	public void RunShowOne(object mID)
	{
		StartCoroutine (ShowOne(mID));
	}

    public GameObject mainUI = null;
    /// <summary>
    /// 私聊
    /// </summary>
    /// <param name="mID"></param>
    private IEnumerator ShowOne(object mID)
    {
        if (mainUI == null)
        {
			GameObject getUI = GameObject.Find("/Song(Clone)/SongGUI/UI - All Grounds/Anchor - MainUI/Anchor - MainUI(Clone)");
            if (getUI == null)
            {
                Object objMainUI = Resources.Load("Anchor - MainUI");
                mainUI = (GameObject)Instantiate(objMainUI);
				GameObject mAnchor = GameObject.Find("/Song(Clone)/SongGUI/UI - All Grounds/Anchor - MainUI");
				PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage ("SetMainUI",mAnchor,SendMessageOptions.DontRequireReceiver);
                mainUI.transform.parent = mAnchor.transform;
                mainUI.transform.localPosition = Vector3.one;
                while (true)
                {
                    if (btnSend != null)
                    {
                        break; 
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }

        }
		while(eventShowOne==null)
		{
			yield return new WaitForEndOfFrame();
		}
        //btnSend.SendMessage("ShowOne",mID,SendMessageOptions.DontRequireReceiver);
        this.ShowOne((string[])mID);
    }

	public GameObject btnSend;
    private BtnEvent beCkbConsumerTip;
    private BtnEvent beCkbHideHelmet;
    private BtnEvent beCkbRefusalDeal;
	private BtnEvent beCkbHideOtherPlayers;	//屏蔽其他玩家
    private BtnEvent beCkbRefusalTeam;
	private BtnEvent beCkbRefusalPVP1;
	private BtnEvent beCkbSwichShock;
	private BtnEvent beCkbSwichBattery;
    private BtnEvent beCkbSwichSound;
    private BtnEvent beCkbSwichMusic;
    private BtnEvent beCmbSelectTitle;
    private BtnEvent beCkbGodFov;
    private BtnEvent beCkbFreeFov;
    private BtnEvent beCkbAutoAttack; // 开启自动攻击

    private BtnEvent beCkbShowNickNamePlayers;
    private BtnEvent beCkbShowNickNameSelf;
    private BtnEvent beCkbShowPet;
	private BtnEvent beCkbPlayerSelectEnamy;

    private UISlider sliderPlayerNum;
    private UISlider sliderCameraHeight;
	public static bool isPlayerSelectEnamy;
    IEnumerator Start()
    {
       // Application.RegisterLogCallbackThreaded(ProcessExceptionReport);
		StartCoroutine (GetPacks ());
        InvokeRepeating("NetSignal", 0, 10);
		if(btnSend!=null){
		btnSend.SetActiveRecursively (true);
		yield return new WaitForSeconds(0.1f);
		btnSend.SetActiveRecursively (false);
		}
		if(PlayerPrefs.GetInt ("SwichBattery",0)==1)
		{
			Application.targetFrameRate=25;
		}
		else
		{
			if(Application.platform==RuntimePlatform.IPhonePlayer)
			{
				Application.targetFrameRate=30;
			}
			else
			{
				Application.targetFrameRate=30;
			}

		}
		if(PlayerPrefs.GetInt ("isPlayerSelectEnamy",1)==1)
		{
			isPlayerSelectEnamy=true;
		}
		else
		{
			isPlayerSelectEnamy=false;
		}
		
		if(PlayerPrefs.GetInt ("MusicSwich",1) == 0)
		{
			listMusic.volume = 0;
            listMusic.enabled = false;
		}
		else
		{
			listMusic.volume = 1;
            listMusic.enabled = true;
		}
		
		if(PlayerPrefs.GetInt ("SoundSwich",1) == 0)
		{
			AudioListener.volume = 0;
		}
		else
		{
			AudioListener.volume = 1;
		}
		
		
//		while(BtnGameManager.yt==null)
//		{
//			yield return new WaitForSeconds(0.1f);
//		}
		while(!BtnGameManager.yt.IsUpdate)
		{
			yield return new WaitForSeconds(0.1f);
		}
        BtnGameManager.yt.Rows[0]["AimLogin"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["AimLogin"].YuanColumnText) + 1).ToString();
		

    }

    private void ProcessExceptionReport(string condition, string stackTrace, LogType type)
    {
//        NGUIDebug.Log(string.Format("{0},{1}", condition, stackTrace));
    }
	
	private IEnumerator GetPacks()
	{
		yield return new WaitForSeconds(2);
		string strName=string.Empty;
		yuan.YuanMemoryDB.YuanRow tempYr;
		string[] strPack=yt.Rows[0]["GetPacks"].YuanColumnText.Split (';');
		for(int i=0;i<strPack.Length;i++)
		{
			if(strPack[i]!=""&&strPack[i].Length>=4)
			{
				
				tempYr= YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytGameItem.SelectRowEqual ("ItemID",strPack[i].Substring(0,4));

				if(tempYr!=null)
				{
					strName=string.Format ("{0} {1}",strName,tempYr["Name"].YuanColumnText);
					PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagItemAsID", strPack[i], SendMessageOptions.DontRequireReceiver);

				}
			}
		}
		if(!string.IsNullOrEmpty (strName))
		{
			warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info413")+strName);
		}
		yt.Rows[0]["GetPacks"].YuanColumnText="";
	}
	
	private ExitGames.Client.Photon.TrafficStatsGameLevel trafficStats;

	
	/// <summary>
	/// 注册游戏设置面板
	/// </summary>
	/// <returns></returns>
	public IEnumerator ReOptionPanle()
	{
		yield return new WaitForSeconds(1);
		UIPopupList playerTitle = beCmbSelectTitle.GetComponent<UIPopupList>();
        beCkbConsumerTip.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbConsumerTipClick(beCkbConsumerTip.gameObject,null)));
        beCkbHideHelmet.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbCkbHideHelmetClick(beCkbHideHelmet.gameObject, null)));
        beCkbRefusalDeal.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbRefusalDealClick(beCkbRefusalDeal.gameObject, null)));
        beCkbHideOtherPlayers.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => HideOtherPlayersClick(beCkbHideOtherPlayers.gameObject, null)));
        beCkbRefusalTeam.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbRefusalTeamClick(beCkbRefusalTeam.gameObject, null)));
        beCkbRefusalPVP1.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbRefusalPVP1Click(beCkbRefusalPVP1.gameObject, null)));
        beCkbSwichShock.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbSwichShockClick(beCkbSwichShock.gameObject, null)));
        beCkbSwichBattery.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbSwichBatteryClick(beCkbSwichBattery.gameObject, null)));
        beCkbSwichMusic.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbMusicSwichClick(beCkbSwichMusic.gameObject, null)));
        beCkbSwichSound.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbSoundSwichClick(beCkbSwichSound.gameObject, null)));
        beCkbAutoAttack.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbAutoAttack(beCkbAutoAttack.gameObject, null)));

		playerTitle.onChange.Add(new EventDelegate(()=>CmbSelectPlayerTitle(beCmbSelectTitle.gameObject, null)));
        //beCkbGodFov.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbGodFov(beCkbGodFov.gameObject, null)));
        //beCkbFreeFov.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => CkbFreeFov(beCkbFreeFov.gameObject, null)));
        beCkbShowNickNamePlayers.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => OnCkbShowNickNamePlayers(beCkbShowNickNamePlayers.gameObject, null)));
        beCkbShowNickNameSelf.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => OnCkbShowNickNameSelf(beCkbShowNickNameSelf.gameObject, null)));
        beCkbShowPet.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => OnCkbShowPet(beCkbShowPet.gameObject, null)));
        beCkbPlayerSelectEnamy.GetComponent<UIToggle>().onChange.Add(new EventDelegate(() => OnPlayerSelectEnamy(beCkbPlayerSelectEnamy.gameObject, null)));
        //beCkbConsumerTip.BtnClickEvent += CkbConsumerTipClick;
        //beCkbHideHelmet.BtnClickEvent += CkbCkbHideHelmetClick;
        //beCkbRefusalDeal.BtnClickEvent += CkbRefusalDealClick;
		//beCkbHideOtherPlayers.BtnClickEvent += HideOtherPlayersClick;
        //beCkbRefusalTeam.BtnClickEvent += CkbRefusalTeamClick;
        //beCkbRefusalPVP1.BtnClickEvent+=CkbRefusalPVP1Click;
		//beCkbSwichShock.BtnClickEvent+=CkbSwichShockClick;
		//beCkbSwichBattery.BtnClickEvent+=CkbSwichBatteryClick;
        //beCkbSwichMusic.BtnClickEvent += CkbMusicSwichClick;
        //beCkbSwichSound.BtnClickEvent += CkbSoundSwichClick;
       // beCmbSelectTitle.BtnClickEvent += CmbSelectPlayerTitle;
        beCkbGodFov.BtnClickEvent += CkbGodFov;
        beCkbFreeFov.BtnClickEvent += CkbFreeFov;
        //beCkbShowNickNamePlayers.BtnClickEvent += OnCkbShowNickNamePlayers;
        //beCkbShowNickNameSelf.BtnClickEvent += OnCkbShowNickNameSelf;
        //beCkbShowPet.BtnClickEvent += OnCkbShowPet;
        //beCkbPlayerSelectEnamy.BtnClickEvent+=OnPlayerSelectEnamy;
//        Debug.Log("----------------");

       //*sliderPlayerNum.eventReceiver = this.gameObject;
        //*sliderPlayerNum.functionName = "OnSliderPlayerNum";

        if (PlayerPrefs.GetInt("SetMusic") == 0)
        {
            PlayerPrefs.SetInt("SetMusic", 1);
            PlayerPrefs.SetInt("MusicSwich", 1);
            PlayerPrefs.SetInt("SoundSwich", 1);
            PlayerPrefs.SetInt("ShowNickNamePlayers", 1);
            PlayerPrefs.SetInt("ShowNickNameSelf", 1);
            PlayerPrefs.SetInt("ShowPet", 1);
        }
		

        if (PlayerPrefs.GetInt("LockFovYuan") == 0)
        {
            PlayerPrefs.SetInt("LockFovYuan", 1);
            //PlayerPrefs.SetInt("LockFov", 1);
        }

        beCkbShowNickNamePlayers.GetComponent<UIToggle>().value = (PlayerPrefs.GetInt("ShowNickNamePlayers",1) == 1 ? true : false);
        OnCkbShowNickNamePlayers(beCkbShowNickNamePlayers.gameObject, null);

        beCkbShowNickNameSelf.GetComponent<UIToggle>().value = (PlayerPrefs.GetInt("ShowNickNameSelf",1) == 1 ? true : false);
        OnCkbShowNickNameSelf(beCkbShowNickNameSelf.gameObject, null);

        beCkbShowPet.GetComponent<UIToggle>().value = (PlayerPrefs.GetInt("ShowPet",1) == 1 ? true : false);
        OnCkbShowPet(beCkbShowPet.gameObject, null);

        beCkbConsumerTip.GetComponent<UIToggle>().value=(PlayerPrefs.GetInt ("ConsumerTip",1)==1?true:false);
		CkbConsumerTipClick (beCkbConsumerTip.gameObject,null);
		
		beCkbHideHelmet.GetComponent<UIToggle>().value=(BtnGameManager.yt.Rows[0]["HideHelmet"].YuanColumnText=="1"?true:false);
		CkbCkbHideHelmetClick(beCkbHideHelmet.gameObject,null);
		
	   	beCkbRefusalDeal.GetComponent<UIToggle>().value=(PlayerPrefs.GetInt ("RefusalDeal",0)==1?true:false);
		CkbRefusalDealClick(beCkbRefusalDeal.gameObject,null);
		
		beCkbHideOtherPlayers.GetComponent<UIToggle>().value=(PlayerPrefs.GetInt ("HideOtherPlayers",0)==1?true:false);
		HideOtherPlayersClick(beCkbHideOtherPlayers.gameObject,null);
		
		
		beCkbRefusalTeam.GetComponent<UIToggle>().value=(PlayerPrefs.GetInt ("RefusalTeam",0)==1?true:false);
		CkbRefusalTeamClick(beCkbRefusalTeam.gameObject,null);
		
		beCkbRefusalPVP1.GetComponent<UIToggle>().value=(PlayerPrefs.GetInt ("RefusalPVP1",0)==1?true:false);
		CkbRefusalPVP1Click(beCkbRefusalPVP1.gameObject,null);
		
		beCkbSwichShock .GetComponent<UIToggle>().value=(PlayerPrefs.GetInt ("SwichShock",0)==1?true:false);
		CkbSwichShockClick(beCkbSwichShock.gameObject,null);
		
		beCkbSwichBattery.GetComponent<UIToggle>().value=(PlayerPrefs.GetInt ("SwichBattery",0)==1?true:false);
		CkbSwichBatteryClick(beCkbSwichBattery.gameObject,null);
		
		 beCkbPlayerSelectEnamy.GetComponent<UIToggle>().value=(PlayerPrefs.GetInt ("isPlayerSelectEnamy",1)==1?true:false);
		OnPlayerSelectEnamy(beCkbPlayerSelectEnamy.gameObject,null);
		
        if (PlayerPrefs.GetInt("LockFov",1) == 0)
        {
            beCkbGodFov.GetComponent<UIToggle>().value = true;
            CkbGodFov(beCkbGodFov.gameObject, null);
        }
        else
        {
            beCkbFreeFov.GetComponent<UIToggle>().value = true;
            CkbFreeFov(beCkbFreeFov.gameObject, null);
        }
        if (PlayerPrefs.GetInt("MusicSwich") == 0)
        {
            beCkbSwichMusic.GetComponent<UIToggle>().value = false;
            CkbMusicSwichClick(beCkbSwichMusic.gameObject, null);
        }
        else
        {
            beCkbSwichMusic.GetComponent<UIToggle>().value = true;
            CkbMusicSwichClick(beCkbSwichMusic.gameObject, null);
        }

        if (PlayerPrefs.GetInt("SoundSwich") == 0)
        {
            beCkbSwichSound.GetComponent<UIToggle>().value = false;
            CkbSoundSwichClick(beCkbSwichSound.gameObject, null);
        }
        else
        {
            beCkbSwichSound.GetComponent<UIToggle>().value = true;
            CkbSoundSwichClick(beCkbSwichSound.gameObject, null);
        }

        if (PlayerPrefs.GetInt("AutoAttack", 0) == 0)
        {
            beCkbAutoAttack.GetComponent<UIToggle>().value = false;
            CkbAutoAttack(beCkbAutoAttack.gameObject, null);
        }
        else
        {
            beCkbAutoAttack.GetComponent<UIToggle>().value = true;
            CkbAutoAttack(beCkbAutoAttack.gameObject, null);
        }

		GetPlayerTitle(playerTitle);
		playerTitle.selection = BtnGameManager.yt[0]["SelectTitle"].YuanColumnText;

		sliderPlayerNum.onChange.Add(new EventDelegate(()=>this.OnSliderPlayerNum()));
        sliderPlayerNum.value = ((float)(PlayerPrefs.GetInt("PlayerNum") - 5) / 15);

        sliderCameraHeight.onChange.Add(new EventDelegate(() => this.OnCameraHeight()));
        //sliderCameraHeight.value = PlayerPrefs.GetInt("CameraHeight", 40) / 80f;
        sliderCameraHeight.value = (PlayerPrefs.GetInt("CameraHeight", 25) - 8) / 17f;
	}
	

    public UISprite picNetSianal;
    private int numPing;
    private int yuanPing;
    /// <summary>
    /// 网络信号
    /// </summary>
    public void NetSignal()
    {


		StartCoroutine(GetPingNum());

		if(numPing==-1||numPing>600)
		{
			picNetSianal.spriteName = "ping1";
		}
		else if(numPing>=300&&numPing<600)
		{
			picNetSianal.spriteName = "ping2";
		}
		else if(numPing <300)
		{
			picNetSianal.spriteName = "ping3";
		}
//        ping=PhotonNetwork.GetPing();
//        yuanPing = InRoom.GetInRoomInstantiate().peer.RoundTripTime;
//
//        if (ping >= 300 || yuanPing >= 300)
//        {
//            picNetSianal.spriteName = "ping1";
//        }
//        else if ((ping >= 100 && ping < 300) || (yuanPing>=100&&yuanPing<300))
//        {
//            picNetSianal.spriteName = "ping2";
//        }
//        else if (ping < 100||yuanPing<100)
//        {
//            picNetSianal.spriteName = "ping3";
//        }
     
    }

	public IEnumerator GetPingNum()
	{
		Ping ping;
		ping =new Ping(InRoom.GetInRoomInstantiate().GetSvrAddress().Split(':')[0]);
		while(!ping.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		numPing=ping.time;
		ping.DestroyPing();
		//Debug.Log ("----------------------------Ping:"+numPing);
	}

    /// <summary>
    /// 选择上帝视角事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbGodFov(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        ckb.value = true;

		beCkbFreeFov.GetComponent<UIToggle>().value=false;
        PlayerPrefs.SetInt("LockFov", 0);
        Camera.mainCamera.SendMessage("GodFOV", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 选择自由视角事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbFreeFov(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        ckb.value = true;
		beCkbGodFov.GetComponent<UIToggle>().value=false;
        PlayerPrefs.SetInt("LockFov", 1);
        Camera.mainCamera.SendMessage("FreeFOV", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 选择显示玩家名称事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void OnCkbShowNickNamePlayers(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("ShowNickNamePlayers", ckb.value ? 1 : 0);
    }

    /// <summary>
    /// 选择显示自己名称事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void OnCkbShowNickNameSelf(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("ShowNickNameSelf", ckb.value ? 1 : 0);
    }

    /// <summary>
    /// 选择显示宠物事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void OnCkbShowPet(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("ShowPet", ckb.value ? 1 : 0);
    }

    private float playerNum;
    /// <summary>
    /// 显示玩家人数
    /// </summary>
    /// 
	private UILabel lblPlayerNum;
	public void OnSliderPlayerNum()
    {
        playerNum = sliderPlayerNum.sliderValue * 15 + 5f;
		PlayerPrefs.SetInt("PlayerNum",(int)playerNum);
		lblPlayerNum.text=((int)playerNum).ToString ();
        //Minmap.SendMessage("ChangeRemoteNum", (int)playerNum, SendMessageOptions.DontRequireReceiver);
    }

    private float cameraHeight;
    /// <summary>
    /// 设置摄像机高度
    /// </summary>
    public void OnCameraHeight()
    {
        // cameraHeight = sliderCameraHeight.value * 80;
        cameraHeight = sliderCameraHeight.value * 17 + 8;

        PlayerPrefs.SetInt("CameraHeight", (int)cameraHeight);
    }

    /// <summary>
    /// 选择玩家称号事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CmbSelectPlayerTitle(object sender, object parm)
    {
        UIPopupList cmb = ((GameObject)sender).GetComponent<UIPopupList>();
		selectTitle=cmb;
        GetPlayerTitle(cmb);
		OnPlayerTitleSelection();
//        cmb.eventReceiver = this.gameObject;
//        cmb.functionName = "OnPlayerTitleSelection";
    }
	
	/// <summary>
	/// 选择手动选怪怪物
	/// </summary>
	/// <param name='sender'>
	/// Sender.
	/// </param>
	/// <param name='parm'>
	/// Parm.
	/// </param>
	 public void OnPlayerSelectEnamy(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("isPlayerSelectEnamy", ckb.value ? 1 : 0);
		isPlayerSelectEnamy=ckb.value;
    }

    /// <summary>
    /// 获得玩家称号列表
    /// </summary>
    /// <param name="mCmb"></param>
    public void GetPlayerTitle(UIPopupList mCmb)
    {
        
        string[] tempStr = BtnGameManager.yt[0]["PlayerTitle"].YuanColumnText.Split(';');
        mCmb.items.Clear();
        foreach (string item in tempStr)
        {
            if (item != "")
            {
                mCmb.items.Add(item);
            }
        }
		if(BtnGameManager.yt[0]["SelectTitle"].YuanColumnText==""&&mCmb.items.Count>0)
		{
			BtnGameManager.yt[0]["SelectTitle"].YuanColumnText=mCmb.items[0];
			mCmb.selection=mCmb.items[0];
		}
    }
	
	UIPopupList selectTitle;
    /// <summary>
    /// 玩家称号选择事件
    /// </summary>
    /// <param name="mSelect"></param>
    public void OnPlayerTitleSelection()
    {
        BtnGameManager.yt[0]["SelectTitle"].YuanColumnText = selectTitle.selection;
		invcl.SendMessage ("reTitle",SendMessageOptions.DontRequireReceiver);
    }


    public AudioSource listMusic;
    public AudioListener audioListener;
    /// <summary>
    /// 音乐开关事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbMusicSwichClick(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("MusicSwich", ckb.value ? 1 : 0);
        if (!ckb.value)
        {
            listMusic.volume = 0;
            listMusic.enabled = false;
        }
        else
        {
			if(0 == PlayerPrefs.GetInt("SoundSwich"))
			{
				ckb.value = false;
				return;
			}
            listMusic.volume = 1;
            listMusic.enabled = true;
        }
  
    }

	public List<UIPlaySound> listSoundBtn = new List<UIPlaySound>();
    public List<AudioSource> listSound = new List<AudioSource>();
    /// <summary>
    /// 音效开关事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbSoundSwichClick(object sender, object parm)
    {
        UIToggle ckb=((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("SoundSwich", ckb.value ? 1 : 0);
        if (!ckb.value) 
        {
            AudioListener.volume = 0;
            beCkbSwichMusic.GetComponent<UIToggle>().value = false;
            CkbMusicSwichClick(beCkbSwichMusic.gameObject, null);

        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    /// <summary>
    /// 消费提示事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbConsumerTipClick(object sender, object parm)
    {
		
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("ConsumerTip", ckb.value ? 1 : 0);
    }

    /// <summary>
    /// 开启自动攻击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbAutoAttack(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("AutoAttack", ckb.value ? 1 : 0);
    }

    public GameObject invcl;
    /// <summary>
    /// 隐藏头盔事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbCkbHideHelmetClick(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        BtnGameManager.yt.Rows[0]["HideHelmet"].YuanColumnText = ckb.value ? "1" : "0";
        invcl.SendMessage("lookTou", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 拒绝交易事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbRefusalDealClick(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("RefusalDeal", ckb.value ? 1 : 0);
    }
	
	/// <summary>
    /// 屏蔽其他玩家
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void HideOtherPlayersClick(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("HideOtherPlayers", ckb.value ? 1 : 0);
    }

    /// <summary>
    /// 拒绝组队事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbRefusalTeamClick(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("RefusalTeam", ckb.value ? 1 : 0);
    }
	
	public void CkbRefusalPVP1Click(object sender,object parm)
	{
		        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("RefusalPVP1", ckb.value ? 1 : 0);
	}
	
	public void CkbSwichShockClick(object sender,object parm)
	{
		UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("SwichShock", ckb.value ? 1 : 0);
	}
	
	public void CkbSwichBatteryClick(object sender,object parm)
	{
		UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("SwichBattery", ckb.value ? 1 : 0);
		if(ckb.value)
		{
			Application.targetFrameRate=25;
		}
		else
		{
			if(Application.platform==RuntimePlatform.IPhonePlayer)
			{
				Application.targetFrameRate=30;
			}
			else
			{
				Application.targetFrameRate=30;
			}

		}

	}

    /// <summary>
    /// 显示较少玩家事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parm"></param>
    public void CkbShowSmallPlayerClick(object sender, object parm)
    {
        UIToggle ckb = ((GameObject)sender).GetComponent<UIToggle>();
        PlayerPrefs.SetInt("ShowSmallPlayer", ckb.value ? 1 : 0);
    }

    public delegate void DelShowOne(object sender,object pram);
	public DelShowOne eventShowOne;
	public UIPanel talkpanel;
    /// <summary>
    /// 弹出私聊面板
    /// </summary>
    /// <param name="mID"></param>
    /// 
    public void ShowOne(string[] mID)
    {
        if (eventShowOne != null)
        { 
			//talkpanel.enabled = true;
            eventShowOne(this, mID);
			
        }
    }


    public BtnPlayerForTeam btnPlayerForOnePVP;
    private UIGrid gridOnePVP;
    private List<BtnPlayerForTeam> listOnePVP = new List<BtnPlayerForTeam>();
    /// <summary>
    /// 获取单人PVP列表
    /// </summary>
    /// <param name="mParm"></param>
    public void GetOnePVPList(Dictionary<short, object> mParm)
    {
		//Debug.Log ("---------------------------------------:mParm.Count):"+mParm.Count);
        foreach (BtnPlayerForTeam team in listOnePVP)
        {
            team.gameObject.SetActiveRecursively(false);
        }

        int num = 0;
        foreach (object item in mParm.Values)
        {
            Dictionary<byte,object> myItem=( Dictionary<byte,object>)item;
            if (listOnePVP.Count > num)
            {
                listOnePVP[num].playerID = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.TeamID];
                listOnePVP[num].lblPlayerName.text = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.UserNickName];
                listOnePVP[num].lblPlayerPro.text = RefreshList.GetPro((string)myItem[(byte)yuan.YuanPhoton.ParameterType.UserPro]);
                listOnePVP[num].lblPlayerLevel.text ="Lv"+ (int)myItem[(byte)yuan.YuanPhoton.ParameterType.TeamLevel];
				 listOnePVP[num].picPlayer.atlas = yuanPicManager.picPlayer[int.Parse((string)myItem[(byte)yuan.YuanPhoton.ParameterType.UserPro])-1].atlas;
                listOnePVP[num].picPlayer.spriteName = yuanPicManager.picPlayer[int.Parse((string)myItem[(byte)yuan.YuanPhoton.ParameterType.UserPro])-1].spriteName;
               
                listOnePVP[num].gameObject.SetActiveRecursively(true);
            }
            else
            {
                BtnPlayerForTeam tempPlayer =(BtnPlayerForTeam)Instantiate(btnPlayerForOnePVP);
                tempPlayer.GetComponent<UIToggle>().group = 1;
                tempPlayer.transform.parent = gridOnePVP.transform ;
                tempPlayer.transform.localPosition = Vector3.zero;
                tempPlayer.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
				tempPlayer.btnAddMessage.target=this.gameObject;
				tempPlayer.btnAddMessage.functionName="BtnAddOnePVP";
                tempPlayer.playerID = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.TeamID];
                tempPlayer.lblPlayerName.text = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.UserNickName];
                tempPlayer.lblPlayerPro.text = RefreshList.GetPro((string)myItem[(byte)yuan.YuanPhoton.ParameterType.UserPro]);
				tempPlayer.picPlayer.atlas = yuanPicManager.picPlayer[int.Parse((string)myItem[(byte)yuan.YuanPhoton.ParameterType.UserPro])-1].atlas;
                tempPlayer.picPlayer.spriteName = yuanPicManager.picPlayer[int.Parse((string)myItem[(byte)yuan.YuanPhoton.ParameterType.UserPro])-1].spriteName;
                tempPlayer.lblPlayerLevel.text = "Lv" + (int)myItem[(byte)yuan.YuanPhoton.ParameterType.TeamLevel];
                listOnePVP.Add(tempPlayer);
            }
            num++;
            gridOnePVP.repositionNow = true;
        }
    }

    /// <summary>
    /// 加入已选择单人PVP
    /// </summary>
    /// <param name="mObj"></param>
    public void BtnAddOnePVP(GameObject mObj)
    {
//        foreach (BtnPlayerForTeam team in listOnePVP)
//        {
//            if (team.myCheckbox.value)
//            {
				BtnPlayerForTeam team=mObj.transform.parent.GetComponent<BtnPlayerForTeam>();
                InRoom.GetInRoomInstantiate().PVPTeamCreate(team.playerID, 1);
		
//				return;
//            }
//        }
 //       warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), "请先选择一个队伍");
    }


    /// <summary>
    /// 加入单人PVP
    /// </summary>
    public void BtnAddPVP()
    {
        InRoom.GetInRoomInstantiate().PVPTeamCreate("", 1);
    }

    /// <summary>
    /// 退出PVP队列
    /// </summary>
    public void BtnPVPRemove()
    {
        InRoom.GetInRoomInstantiate().PVPTeamDissolve();
    }
    
    /// <summary>
    /// 获取战场列表
    /// </summary>
    /// <param name="mParm"></param>
    public void GetLegionOneList(Dictionary<short, object> mParm)
    {
        //Debug.Log("---------------------------------------:mParm.Count):" + mParm.Count);
		
		if(gridLegionOneCity.gameObject.active)
		{
			SetCityLegionOneList (mParm);
		}
		
//		if(gridLegionOneOut.gameObject.active)
//		{
//			SetOutLegionOneList(mParm);
//		}
    }
	
	private UIGrid gridLegionOneCity;
	private List<BtnPlayerForTeam> listLegionOneCity = new List<BtnPlayerForTeam>();
	public void SetCityLegionOneList(Dictionary<short, object> mParm)
	{
	      foreach (BtnPlayerForTeam team in listLegionOneCity)
        {
            team.gameObject.SetActiveRecursively(false);
        }

        int num = 0;
		string strLegionOneType=string.Empty;
        foreach (object item in mParm.Values)
        {
			
            Dictionary<byte, object> myItem = (Dictionary<byte, object>)item;
			strLegionOneType=(string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneType];
            if (listLegionOneCity.Count > num)
            {
                listLegionOneCity[num].playerID = ((int)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneID]).ToString();
                listLegionOneCity[num].yr["map"].YuanColumnText = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneMap];
                listLegionOneCity[num].lblPlayerName.text = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneMapName];
                if (listLegionOneCity[num].lblPlayerName.text.IndexOf(StaticLoc.Loc.Get("info312")+ "") != -1)
                {
                    listLegionOneCity[num].picPlayer.spriteName = "22";
                }
                else if (listLegionOneCity[num].lblPlayerName.text.IndexOf(StaticLoc.Loc.Get("info313")+"") != -1)
                {
                    listLegionOneCity[num].picPlayer.spriteName = "64";
                }

                listLegionOneCity[num].lblPlayerPro.text = "";
                listLegionOneCity[num].lblPlayerLevel.text = StaticLoc.Loc.Get("info314")+""+(int)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOnePlayerNum] + "人";
                listLegionOneCity[num].gameObject.SetActiveRecursively(true);
                listLegionOneCity[num].picPlayer.gameObject.SetActiveRecursively(false);

            }
            else
            {
                BtnPlayerForTeam tempPlayer = (BtnPlayerForTeam)Instantiate(btnPlayerForOnePVP);
                tempPlayer.GetComponent<UIToggle>().group = 1;
                tempPlayer.btnAddMessage.target = this.gameObject;
                tempPlayer.btnAddMessage.functionName = "BtnAddLegionOneTeam";
                tempPlayer.transform.parent = gridLegionOneCity.transform;
                tempPlayer.transform.localPosition = Vector3.zero;
                tempPlayer.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                tempPlayer.playerID =((int)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneID]).ToString();
                tempPlayer.yr = new yuan.YuanMemoryDB.YuanRow();
                tempPlayer.yr.Add("map", (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneMap]);
                tempPlayer.lblPlayerName.text = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneMapName];
                if (tempPlayer.lblPlayerName.text.IndexOf(StaticLoc.Loc.Get("info312")+"") != -1)
                {
                    tempPlayer.picPlayer.spriteName = "22";
                }
                else if (tempPlayer.lblPlayerName.text.IndexOf(StaticLoc.Loc.Get("info313")+"") != -1)
                {
                    tempPlayer.picPlayer.spriteName = "64";
                }
                tempPlayer.lblPlayerPro.text = "";
                tempPlayer.picPlayer.gameObject.SetActiveRecursively(true);
                tempPlayer.lblPlayerLevel.text = StaticLoc.Loc.Get("info314")+"" + (int)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOnePlayerNum] + "人";
                listLegionOneCity.Add(tempPlayer);
            }
            num++;
            gridLegionOneCity.repositionNow = true;
        }
	}
	
	private UIGrid gridLegionOneOut;
	private List<BtnPlayerForTeam> listLegionOneOut = new List<BtnPlayerForTeam>();
	public void SetOutLegionOneList(Dictionary<byte, object> mParm)
	{
		foreach (BtnPlayerForTeam team in listLegionOneOut)
        {
            team.gameObject.SetActiveRecursively(false);
        }

        int num = 0;
		string strLegionOneType=string.Empty;
        foreach (object item in mParm.Values)
        {
			
            Dictionary<byte, object> myItem = (Dictionary<byte, object>)item;
			strLegionOneType=(string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneType];
            if (listLegionOneOut.Count > num)
            {
                listLegionOneOut[num].playerID = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneID];
                listLegionOneOut[num].yr["map"].YuanColumnText = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneMap];
                listLegionOneOut[num].lblPlayerName.text = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneMap] + listLegionOneOut[num].playerID;
                listLegionOneOut[num].lblPlayerPro.text = "";
                listLegionOneOut[num].lblPlayerLevel.text = StaticLoc.Loc.Get("info314")+""+(int)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOnePlayerNum] + "人";
                listLegionOneOut[num].gameObject.SetActiveRecursively(true);
                listLegionOneOut[num].picPlayer.gameObject.SetActiveRecursively(false);
            }
            else
            {
                BtnPlayerForTeam tempPlayer = (BtnPlayerForTeam)Instantiate(btnPlayerForOnePVP);
                tempPlayer.GetComponent<UIToggle>().group = 1;
                tempPlayer.transform.parent = gridLegionOneOut.transform;
                tempPlayer.transform.localPosition = Vector3.zero;
                tempPlayer.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
				
				tempPlayer.btnAddMessage.target=this.gameObject;
				tempPlayer.btnAddMessage.functionName="BtnAddLegionOneTeam";
                tempPlayer.playerID = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneID];
                tempPlayer.yr = new yuan.YuanMemoryDB.YuanRow();
                tempPlayer.yr.Add("map", (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneMap]);
                tempPlayer.lblPlayerName.text = (string)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOneMap] + listLegionOneOut[num].playerID;
                tempPlayer.lblPlayerPro.text = "";
                tempPlayer.picPlayer.gameObject.SetActiveRecursively(true);
                tempPlayer.lblPlayerLevel.text = StaticLoc.Loc.Get("info314")+"" + (int)myItem[(byte)yuan.YuanPhoton.ParameterType.LegionOnePlayerNum] + "人";
                listOnePVP.Add(tempPlayer);
            }
            num++;
            gridLegionOneOut.repositionNow = true;
        }
	}

    /// <summary>
    /// 加入已选择战场
    /// </summary>
    /// <param name="mObj"></param>
    public void BtnAddLegionOneTeam(GameObject mObj)
    {
//        foreach (BtnPlayerForTeam team in listLegionOne)
//        {
//            if (team.myCheckbox.value)
//            {
				BtnPlayerForTeam team=mObj.transform.parent.GetComponent<BtnPlayerForTeam>();
                InRoom.GetInRoomInstantiate().LegionOneTeamAdd(int.Parse(team.playerID), team.yr["map"].YuanColumnText);
//                return;
//            }
//        }
//        warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), "请先选择一个队伍");
    }

    /// <summary>
    /// 加入战场
    /// </summary>
    public void BtnAddLegionOneMapCity()
    {
        InRoom.GetInRoomInstantiate().LegionOneAdd(InRoom.isUpdatePlayerLevel?InRoom.playerLevel : yt.Rows[0]["PlayerLevel"].YuanColumnText);
    }

    /// <summary>
    /// 加入战场
    /// </summary>
    public void BtnAddLegionOneMapOut()
    {
        InRoom.GetInRoomInstantiate().LegionOneAdd("野外");
    }

    private System.Random ranLegionOne = new System.Random((int)System.DateTime.Now.Ticks);
    private int numLegionOne;
    /// <summary>
    /// 随机加入战场
    /// </summary>
    public void BtnRandomAddLegionOne()
    {
        //numLegionOne = ranLegionOne.Next(0, 2);
        //if (numLegionOne == 0)
        //{
            BtnAddLegionOneMapCity();
        //}
        //else if(numLegionOne==1)
        //{
        //    BtnAddLegionOneMapOut();
        //}
    }

    /// <summary>
    /// 退出战场
    /// </summary>
    public void BtnRemoveLegionOne()
    {
        InRoom.GetInRoomInstantiate().LegionOneRemove();
    }
	
	public void OtherLogin()
	{
		GameReonline.isEnable=false;
		LogoutSDK();
		BtnManager.isOhterLogin=true;
		BtnManager.strOtherLogin=StaticLoc.Loc.Get("info481");
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
		//Application.LoadLevel (0);
	}
	
	public void OffLine()
	{
		GameReonline.isEnable=false;
		LogoutSDK();
		BtnManager.isOhterLogin=true;
		BtnManager.strOtherLogin=StaticLoc.Loc.Get("info322");
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
		//Application.LoadLevel (0);
	}
	
	public void NoSaveData()
	{
		GameReonline.isEnable=false;
		LogoutSDK();
		BtnManager.isOhterLogin=true;
		BtnManager.strOtherLogin=StaticLoc.Loc.Get("info728");
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
		//Application.LoadLevel (0);
	}

	public void Logout(string mInfo)
	{
		GameReonline.isEnable=false;
		LogoutSDK();
		BtnManager.isOhterLogin=true;
		BtnManager.strOtherLogin=StaticLoc.Loc.Get(mInfo);
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
		//Application.LoadLevel (0);
	}

	public void PlayerOffline()
	{
		GameReonline.isEnable=false;
		LogoutSDK();
		BtnManager.isOhterLogin=true;
		BtnManager.strOtherLogin=StaticLoc.Loc.Get("info1045");
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
		//Application.LoadLevel (0);
	}
	
	public void SendTalkSomeBody(string[] listBody)
	{
		GameReonline.isEnable=false;
		btnSend.gameObject.SetActiveRecursively (true);
		btnSend.SendMessage ("SendTalkSomeBody",listBody,SendMessageOptions.DontRequireReceiver);
	}
	
	private PanelListTabel listTablePVP1;
	private List<BtnPlayerForTeam> listPVP1=new List<BtnPlayerForTeam>();
	yuan.YuanMemoryDB.YuanTable ytPVP1=new yuan.YuanMemoryDB.YuanTable("listPVP1BtnGameManager","");
	yuan.YuanMemoryDB.YuanTable ytPVP1Firend=new yuan.YuanMemoryDB.YuanTable("listPVP1FirendBtnGameManager","");
	/// <summary>
	/// 获取可挑战玩家列表
	/// </summary>
	/// <returns>
	/// The PV p1 list.
	/// </returns>
	public IEnumerator GetPVP1List()
	{
		//string strSql=string.Format ("Select top 10 * from (select PlayerID,PlayerName,ProID,PlayerLevel,ROW_NUMBER()over(order by ColosseumPoint desc)ROW_NUMBER from PlayerInfo) as temp");
		//string strSql=string.Format ("select(select count(*) from PlayerInfo where a.ColosseumPoint<ColosseumPoint or (a.ColosseumPoint=ColosseumPoint and a.CreateTime<CreateTime)) as ROW_NUMBER,a.PlayerID,a.PlayerName,a.ProID,a.PlayerLevel from PlayerInfo a order by ROW_NUMBER limit 10");
		//InRoom.GetInRoomInstantiate ().GetYuanTable (strSql,"DarkSword2",ytPVP1);	
		InRoom.GetInRoomInstantiate ().GetRankTopYT (yuan.YuanPhoton.RankingType.Abattoir,ytPVP1);
		string[] listFirend=BtnGameManager.yt[0]["FriendsId"].YuanColumnText.Split (';');
		
		if(listFirend.Length>0)
		{
			InRoom.GetInRoomInstantiate ().GetPlayerList(listFirend,ytPVP1Firend,"DarkSword2","PlayerInfo");
			//System.Text.StringBuilder strSqlFirend=new System.Text.StringBuilder();
			//strSqlFirend.Append("Select PlayerID,PlayerName,ProID,PlayerLevel from PlayerInfo where ");
			//for(int i=0;i<listFirend.Length;i++)
			//{
			//	
			//	if( i!=0&&listFirend[i]!="")
			//	{
			//		strSqlFirend.AppendFormat (" or PlayerID={0}",listFirend[i]);
			//	}
			//	else if(listFirend[i]!="")
			//	{
			//		strSqlFirend.AppendFormat ("PlayerID={0}",listFirend[i]);
			//	}
			//}
			//InRoom.GetInRoomInstantiate ().GetYuanTable (strSqlFirend.ToString (),"DarkSword2",ytPVP1Firend);		
		}
		while(ytPVP1.IsUpdate||ytPVP1Firend.IsUpdate)
		{
			yield return new WaitForSeconds(0.1f);
		}
		ytPVP1.Rows.AddRange (ytPVP1Firend.Rows);
		listTablePVP1.SetFrist (ytPVP1,SetPVP1List,4);
	}
	
	public void SetPVP1List(yuan.YuanMemoryDB.YuanTable mYt)
	{
		for(int i=0;i<listPVP1.Count;i++)
		{
			listPVP1[i].gameObject.SetActiveRecursively (false);
		}		
        for (int i=0;i<mYt.Rows.Count;i++)
        {
            if (listPVP1.Count > i)
            {
                listPVP1[i].playerID = mYt.Rows[i]["PlayerID"].YuanColumnText;
                listPVP1[i].lblPlayerName.text =mYt.Rows[i]["PlayerName"].YuanColumnText;
                listPVP1[i].lblPlayerPro.text = RefreshList.GetPro(mYt.Rows[i]["ProID"].YuanColumnText);
                listPVP1[i].lblPlayerLevel.text ="Lv"+mYt.Rows[i]["PlayerLevel"].YuanColumnText;
				 listPVP1[i].picPlayer.atlas = yuanPicManager.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText)-1].atlas;
                listPVP1[i].picPlayer.spriteName = yuanPicManager.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText)-1].spriteName;
               if(gridOnePVP.gameObject.active)
				{
                listPVP1[i].gameObject.SetActiveRecursively(true);
				}
            }
            else
            {
                BtnPlayerForTeam tempPlayer =(BtnPlayerForTeam)Instantiate(btnPlayerForOnePVP);
                tempPlayer.GetComponent<UIToggle>().group = 1;
                tempPlayer.transform.parent = gridOnePVP.transform ;
                tempPlayer.transform.localPosition = Vector3.zero;
                tempPlayer.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
				tempPlayer.btnAddMessage.target=this.gameObject;
				tempPlayer.btnAddMessage.functionName="BtnPVP1";
				tempPlayer.btnAddLable.text=StaticLoc.Loc.Get("info315")+"";
                tempPlayer.playerID = mYt.Rows[i]["PlayerID"].YuanColumnText;
                tempPlayer.lblPlayerName.text = mYt.Rows[i]["PlayerName"].YuanColumnText;
                tempPlayer.lblPlayerPro.text =  RefreshList.GetPro(mYt.Rows[i]["ProID"].YuanColumnText);
				tempPlayer.picPlayer.atlas =  yuanPicManager.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText)-1].atlas;
                tempPlayer.picPlayer.spriteName = yuanPicManager.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText)-1].spriteName;
                tempPlayer.lblPlayerLevel.text = "Lv"+mYt.Rows[i]["PlayerLevel"].YuanColumnText;
                listPVP1.Add(tempPlayer);
				if(!gridOnePVP.gameObject.active)
				{
					tempPlayer.gameObject.SetActiveRecursively (false);
				}
            }
            gridOnePVP.repositionNow = true;
        }
	}
	
	 /// <summary>
    /// 挑战列表中玩家
    /// </summary>
    /// <param name="mObj"></param>
    public void BtnPVP1(GameObject mObj)
    {
				BtnPlayerForTeam team=mObj.transform.parent.GetComponent<BtnPlayerForTeam>();
                InRoom.GetInRoomInstantiate().PVP1Invite(team.playerID);
    
	}
	
	
	/// <summary>
	/// 根据proid获得职业名称
	/// </summary>
	/// <returns>
	/// proID
	/// </returns>
	/// <param name='proID'>
	/// Pro I.
	/// </param>
	public static string GetPro(string proID)
    {
        string pro = string.Empty;
        switch (proID)
        {
            case "1":
                {
                    pro =StaticLoc.Loc.Get("buttons576") ;
                }
                break;
            case "2":
                {
                    pro = StaticLoc.Loc.Get("buttons581");
                }
                break;
            case "3":
                {
                    pro = StaticLoc.Loc.Get("buttons582");
                }
                break;
        }
        return pro;
    }
	
	public List<string> Keys=new List<string>();
	private string Val=string.Empty;
	private string OldVal=string.Empty;
	public Localization Loc;
	public void SetUILable(UILabel mlbl)
	{
		OldVal=mlbl.text;
		mlbl.text="";
		for(int i=-0;i<Keys.Count;i++)
		{
			Val=StaticLoc.Loc.Get (Keys[i]);
			mlbl.text=string.Format ("{0}{1}",mlbl.text,Val);
		}
	}
	
	public string myFriendId;
	public string myFriendName;
	public void AddFirendYes()
	{
		InRoom.GetInRoomInstantiate().RetrunFirendsAddInvit(myFriendId,myFriendName,yuan.YuanPhoton.ReturnCode.Yes);
		warnings.warningAllEnterClose.Close();
	}
	public void AddFriendNo(){
		InRoom.GetInRoomInstantiate().RetrunFirendsAddInvit(myFriendId,myFriendName,yuan.YuanPhoton.ReturnCode.No);
		warnings.warningAllEnterClose.Close();
	}

	public object[] objsBobotPlayer=new object[3];
	public object[] objsBobotPlayerT = new object[3];
	/// <summary>
	/// 执行机器人方法
	/// </summary>
	public void RobotPlayer()
	{
		invcl.SendMessage("CreateBotPlayer", objsBobotPlayer, SendMessageOptions.DontRequireReceiver);
	}

	public void PlayerShowInfo(){
		invcl.SendMessage("CreateBotPlayer", objsBobotPlayer, SendMessageOptions.DontRequireReceiver);
//		invcl.SendMessage("CreateBotPlayer", objsBobotPlayerT, SendMessageOptions.DontRequireReceiver);
	}
	/// <summary>
	/// 执行影魔刷新机器人方法
	/// </summary>
	public void ShowRobotControl(){
		PanelOfflinePlayer.panelOfflinePlayer.ShowShaDow();
	}

	/// <summary>
	/// 执行挑战影魔机器人方法
	/// </summary>
	public void RobotPVPControl(){
		PanelOfflinePlayer.panelOfflinePlayer.RobotytShaDow();
	}

	public Transform pvpButtonTran;
	public Transform GetPVPButton(){
		return pvpButtonTran;
	}

	public void SaveTraining(){
		InRoom.GetInRoomInstantiate ().TrainingSave(1);
		warnings.warningAllEnterClose.Close();
	}
	
    public void QuickTraining()
    {
        InRoom.GetInRoomInstantiate().Training(2,1,1);
        warnings.warningAllEnterClose.Close();
    }

	public GameObject MTW;
	public GameObject Minmap;
    public GameObject InvMake;
	public UILabel lblStoreGold;
	public UILabel lblStoreBlood;
	public UILabel lblStoreGold1;
	public UILabel lblStoreBlood1;
	public UILabel lblStoreGold2;
	public UILabel lblStoreBlood2;
	public UILabel lblStoreGold3;
	public UILabel lblStoreBlood3;	

}
