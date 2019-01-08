using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zealm;
public class BtnGameManagerBack : MonoBehaviour {
    public BtnGameManager btnGameManager;
	
    public UILabel lblTeamInfo;
    public Warnings warnings;
    public UILabel lblPlayerBindWarning;

    public static bool isInPVPOne=false;
    public static bool isInPVP = false;
    public static bool isInLegion = false;

    public static bool pvpStart = false;
    public static bool pvpReady = false;
	
	public static bool isOhterLogin=false;


    public static int teaminstensid = 0;
	
	public static BtnGameManagerBack my;
	void Awake()
	{
		my=this;
		if(isOhterLogin)
		{
			OtherLogin ();
		}
		InRoom.GetInRoomInstantiate().btnGameManagerBack = this;
        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().btnGameManagerBack = this;
	}
	
	public void OtherLogin()
	{   
		BtnManager.isOhterLogin=true;
		if(!string.IsNullOrEmpty(BtnManager.strOtherLogin))
		{
			BtnManager.strOtherLogin=StaticLoc.Loc.Get("info481");
		}
		
		if(UICL)
		{
			UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
		}
		//Application.LoadLevel (0);
	}
	
	private System.Timers.Timer timer;
	
	// Use this for initialization
	void Start () {
//		if(Application.loadedLevelName!="Loading 1"&&Application.loadedLevelName!="Loading 1")
        InvokeRepeating("GetInfo", 3, ReadDicBenefitsInfo.RefreshTime);
		InRoom.GetInRoomInstantiate ().GetClientPrams();
		//InvokeRepeating("RemoveGC",2,2);
	}
//	void OnLevelWasLoaded(int level) {
//		Awake();
//		Start();
//	}
	public bool isTimeOut=false;
	void Update()
	{
		if(isOhterLogin)
		{
			OtherLogin ();
		}
		if(isTimeOut)
		{
			isTimeOut=false;
			TimeOutInfo();
		}
	}
	
	public void TimeOutInfo()
	{
		 warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info726") );
	}

	
	void RemoveGC()
	{
		System.GC.Collect ();
	}
	
	void GetInfo()
	{
		try
		{
			//NGUIDebug.Log ("------------------------11111");
			if (operationResponse.Count>0&&Application.loadedLevelName!="Map200")
	        {
					if(Application.loadedLevelName=="Loading"||Application.loadedLevelName=="Loading 1")
					{
						if((OpCode)operationResponse[0].OperationCode==OpCode.OtherLogin)
						{
							isOhterLogin=true;						
						}
						if((OpCode)operationResponse[0].OperationCode!=OpCode.InRoom)
						{
							operationResponse.RemoveAt (0);
							return;		
						}
					}

//					Debug.Log ("------------------"+operationResponse[0]);
				
		            if (operationResponse[0] != null)
		            {
                        //Profiler.BeginSample ("+++++Yuan");
		                this.OnOperationResponse(operationResponse[0]);
                        //Profiler.EndSample ();
		            }
					else
					{
						operationResponse.RemoveAt (0);
					}
	        }
		}
		catch(System.Exception ex)
		{
			//operationResponse.RemoveAt (0);
			Debug.LogWarning (ex.ToString ());
		}
	}

    void OnApplicationQuit()
    {

		InRoom.YuanDispose ();
		YuanUnityPhoton.YuanDispose ();
    }

    [HideInInspector]
    public bool isBlackFirend = false;

    [HideInInspector]
    public bool isFirend = false;

    [HideInInspector]
    public bool isResponse = false;

    public static List<Zealm.OperationResponse> operationResponse = new List<Zealm.OperationResponse>();

    public void OnOperationResponse(Zealm.OperationResponse mResponse)
    {
		try
		{
			//Debug.Log ("------------------------------"+((yuan.YuanPhoton.OperationCode)mResponse.OperationCode).ToString ());
			operationResponse.Remove (mResponse);
            
		if(Application.loadedLevelName!="Map200")	
			{
                
				if(mResponse.OperationCode==28)
				{
					SetInfo10(mResponse);
				}
				else if(mResponse.OperationCode>=0&&mResponse.OperationCode<=32)
				{
					//11
					SetInfo1(mResponse);
				}
				else if(mResponse.OperationCode>=33&&mResponse.OperationCode<=46)
				{
					//10
					SetInfo2 (mResponse);
				}
				else if(mResponse.OperationCode>=50&&mResponse.OperationCode<=64)
				{
					//10
					SetInfo3(mResponse);
				}
				else if(mResponse.OperationCode>=65&&mResponse.OperationCode<=76)
				{
					//10
					SetInfo4 (mResponse);
				}
				else if(mResponse.OperationCode>=77&&mResponse.OperationCode<=88)
				{
					//10
					SetInfo5 (mResponse);
				}
				else if(mResponse.OperationCode>=89&&mResponse.OperationCode<=104)
				{
					//10
					SetInfo6(mResponse);
				}
				else if(mResponse.OperationCode>=105&&mResponse.OperationCode<=121)
				{
					//10
					SetInfo7(mResponse);
				}
				else if(mResponse.OperationCode>=122&&mResponse.OperationCode<=155)
				{
					//12
					SetInfo8(mResponse);
				}
				else if(mResponse.OperationCode>=156&&mResponse.OperationCode<=174)
				{
					//12
					SetInfo9(mResponse);
				}
				else if(mResponse.OperationCode>=175&&mResponse.OperationCode<=2250)
				{
					//8
					SetInfo11 (mResponse);
				}
        }
			
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning(ex.ToString ());
		}
    }

    public void SetInfo1(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((OpCode)mResponse.OperationCode)
			{
                case OpCode.YuanDBUpdate:
                {
                    string tableName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableName];
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                string errorText = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ErrorParameterCode.ErrorText];
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info440") +errorText.Substring(0,50));
                            }
                            break;
                    }

                }
                break;
                case OpCode.BindUserID:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                lblPlayerBindWarning.text = StaticLoc.Loc.Get(mResponse.DebugMessage);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                lblPlayerBindWarning.text = StaticLoc.Loc.Get(mResponse.DebugMessage);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                            {
                                lblPlayerBindWarning.text = StaticLoc.Loc.Get(mResponse.DebugMessage);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GetPlayerID:
                {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string playerID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.UserID];
                                    if (isBlackFirend)
                                    {
                                        btnGameManager.BlackFirend(playerID);
                                        isBlackFirend = false;
                                    }
                                    else if (isFirend)
                                    {
                                        btnGameManager.AddFirend(playerID);
                                        isFirend = false;
                                    }

                                   
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                                {
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info441") );
                                    isBlackFirend = false;
									isFirend=false;
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                    isBlackFirend = false;
									isFirend=false;
                                }
                                break;
                        
                    }
                }
                break;
                case OpCode.TeamCreate:
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    UICL.SendMessage("TeamTest", SendMessageOptions.DontRequireReceiver);
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info442") );
                                    BtnGameManager.isTeamCreat = false;
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                                {
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info443") );
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:
                                {
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info443") );
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                    BtnGameManager.isTeamCreat = false;
                                }
                                break;
                        }
                    }
                break;
                case OpCode.GetTeam:
                {
                    switch (mResponse.ReturnCode)
                    {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                   StartCoroutine( btnGameManager.GetTeamOK(mResponse.Parameters));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                }
                                break;
                    }
                }
                break;
                case OpCode.GetMyTeam:
                {
					
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                
                                //                                MyTeamInfo myTeamInfo = btnGameManager.gridTeam.GetComponent<MyTeamInfo>();
                                string teamheadid = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamHeadID];
                                //                                myTeamInfo.teamHeadID = (string)mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.TeamHeadID];
                                //                                myTeamInfo.teamID = (string)mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.TeamID];
                                //                                myTeamInfo.teamInfo = (string)mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.TeamInfo];
                                //                                myTeamInfo.teamMemver = (string)mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.TeamMemver];
                                //                                myTeamInfo.teamName = (string)mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.TeamName];
                                //string tableName = (string)mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.TeamID];
                                //Debug.Log("teamid == " + teamheadid);
								//Debug.Log(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                UICL.SendMessage("ShowTeamID", StaticLoc.Loc.Get(mResponse.DebugMessage)==null?"":StaticLoc.Loc.Get(mResponse.DebugMessage), SendMessageOptions.DontRequireReceiver);
                                UICL.SendMessage("ShowTeamHeadID", teamheadid, SendMessageOptions.DontRequireReceiver);
//                                Debug.Log("----------------------返回队伍ID:" + StaticLoc.Loc.Get(mResponse.DebugMessage));

                                captainID = teamheadid;
                                string teamMembersInfo = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamMemver];
                                string[] teamMembers = teamMembersInfo.Split(';');
                                teamMembersID.Clear();
                                for (int i = 0; i < teamMembers.Length; i++)
                                {
                                    teamMembersID.Add(teamMembers[i]);
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
								
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
					return;
                }
                break;
                case OpCode.GetMyTeamPlayers://×Ô¶¯Ë¢ÐÂ¶ÓÎéÁÐ±í
                {
					
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                yuan.YuanMemoryDB.YuanTable responseTable = new yuan.YuanMemoryDB.YuanTable("", "");
                                responseTable.CopyToDictionary(mResponse.Parameters);
								UICL.SendMessage("ShowTeam" , responseTable , SendMessageOptions.DontRequireReceiver);

                                teamMembersID.Clear();
                                if (responseTable.Count > 0)
                                {
                                    foreach (yuan.YuanMemoryDB.YuanRow row in responseTable.Rows)
                                    {
                                        teamMembersID.Add(row["PlayerID"].YuanColumnText);
                                    }
                                }
                                else
                                {
                                    captainID = "";
                                    teamMembersID.Clear();
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.GetServer:
                            {
                                string teamID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID];
                                UICL.SendMessage("ShowTeamID", teamID, SendMessageOptions.DontRequireReceiver);

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.TeamAdd:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info445"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info446"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info446"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info447"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info360"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info361"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GropsCreate:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string gorpsID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID];
                                BtnGameManager.yt[0]["GorpsID"].YuanColumnText = gorpsID;
							
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info362"));
                                RefreshMemberList();
				}
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info363"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info448"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
								btnGameManager.CloseLoading ();
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info449"));
                                SwitchToStore();
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info364"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GetGrops:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                if (btnGameManager.gridCrops.gameObject.active)
                                {
                                    yuan.YuanMemoryDB.YuanTable yt=new yuan.YuanMemoryDB.YuanTable("","");
									yuan.YuanMemoryDB.YuanTable ytTemp=new yuan.YuanMemoryDB.YuanTable("","");
                                    yt.CopyToDictionary(mResponse.Parameters);
                                    if (btnGameManager.tablePVP2 != null)
                                    {
										ytTemp.Rows=yt.SelectRowsListEqual ("PlayerNumber","2");
                                        btnGameManager.tablePVP2.SetFrist(ytTemp, btnGameManager.GetCropsOK, 4, btnGameManager.gridCrops, btnGameManager.listCropsBtns, "BtnCropsClick", btnGameManager.SetCropsBtn, "2");
                                    }
                                    //btnGameManager.GetCropsOK(mResponse.Parameters, btnGameManager.gridCrops, btnGameManager.listCropsBtns, "BtnCropsClick", btnGameManager.SetCropsBtn,"2");
                                }
                                if(btnGameManager.gridLegion.gameObject.active)
                                {
                                    yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("", "");
								yuan.YuanMemoryDB.YuanTable ytTemp=new yuan.YuanMemoryDB.YuanTable("","");
                                    yt.CopyToDictionary(mResponse.Parameters);
                                    if (btnGameManager.tablePVP4 != null)
                                    {
										ytTemp.Rows=yt.SelectRowsListEqual ("PlayerNumber","4");
                                        btnGameManager.tablePVP4.SetFrist(ytTemp, btnGameManager.GetCropsOK, 4, btnGameManager.gridLegion, btnGameManager.listPVP4Btns, "BtnPVP4Click", btnGameManager.SetCropsBtn, "4");
                                    }
                                    //btnGameManager.GetCropsOK(mResponse.Parameters, btnGameManager.gridLegion, btnGameManager.listPVP4Btns, "BtnPVP4Click", btnGameManager.SetCropsBtn, "4");
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;

                    }
                }
                break;
                case OpCode.GropsAdd:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);								
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info365"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info366"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info321"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info368"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info369"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;				
				
			}
		
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo2(Zealm.OperationResponse mResponse)
	{
		try	
		{
            switch ((OpCode)mResponse.OperationCode)
			{
                case OpCode.GropsRemove:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info370"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info371"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GetTransactionInfo:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								TransactionParameters tp = new TransactionParameters();
                                tp.equepmentID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.TransactionInfo.ItemID];
                                tp.blood = ((int)mResponse.Parameters[(short)yuan.YuanPhoton.TransactionInfo.BloodStone]).ToString();
                                tp.gold = ((int)mResponse.Parameters[(short)yuan.YuanPhoton.TransactionInfo.Golds]).ToString();
                                tp.isReady = (bool)mResponse.Parameters[(short)yuan.YuanPhoton.TransactionInfo.IsReady];
                                tp.isTransaction = (bool)mResponse.Parameters[(short)yuan.YuanPhoton.TransactionInfo.IsTransaction];
								tc.SendMessage("SetItemRight" , tp ,SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                    }
                }
                break;
                case OpCode.SendTransactionID:
                {
					//print ("1111");
                    switch (mResponse.ReturnCode)
                    {
                          case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string transactionID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TransactionID];
							//	//print(transactionID);
								UICL.SendMessage("ShowJiaoYi" , transactionID , SendMessageOptions.DontRequireReceiver);
//								InRoom.GetInRoomInstantiate().SendTransactionInfo(transactionID, );
                           }
                            break;
                    }
                }
                break;
                case OpCode.TransactionClose:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								tc.SendMessage("TweenClose" , SendMessageOptions.DontRequireReceiver);
                                //提示信息，交易被取消！
                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("tips053"));
                            }
                            break;
                    }
                }
                break;
                case OpCode.LegionTempCreate:
                case OpCode.LegionDBCreate:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string legionID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID];

                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info372"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasNickName:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info373"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"),  StaticLoc.Loc.Get("info374"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info375"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                    }
                }
                break;
                case OpCode.LegionTempAdd:
                case OpCode.LegionDBAdd:
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info376"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"),  StaticLoc.Loc.Get("info377") );
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info378")  );
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                    }
                }
                break;
                case OpCode.GetLegion://»ñµÃ¾üÍÅÁÐ±í
                {
                    btnGameManager.GetLegion(mResponse.Parameters);
                }
                break;
                case OpCode.GuildCreate:
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info379"));
					 			invCL.SendMessage("ReInitItem", SendMessageOptions.DontRequireReceiver);
                                string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
                                RefershYT(strKey, strValue);
                                //BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText = (string)mResponse.Parameters[(short)ParameterType.GuildID];
                                //BtnGameManager.yt.Rows[0]["GuildName"].YuanColumnText = (string)mResponse.Parameters[(short)ParameterType.GuildName];
                                //BtnGameManager.yt.Rows[0]["Money"].YuanColumnText = (string)mResponse.Parameters[(short)ParameterType.MoneyNumb];
								btnGameManager.BtnClose();
								GuildInformation.guildInfoMation.ShowBack();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info380"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
				PanelStatic.StaticBtnGameManager.CloseLoading();	
                }
                break;
                case OpCode.GuildAdd:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                RefreshList.refreshList.RefreshGuildPlayer();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info381"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show("",StaticLoc.Loc.Get(mResponse.DebugMessage) );                         
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasID: 
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info972"));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GetGuildAll:
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("", "");
                                yt.CopyToDictionary(mResponse.Parameters);
                                if (btnGameManager.tableGuild)
                                {
                                    btnGameManager.tableGuild.SetFrist(yt, btnGameManager.GetCropsOK, 6, btnGameManager.gridGuild, btnGameManager.listGuildBtns, "BtnGuildClick", btnGameManager.SetGuildBtn, "");
						btnGameManager.tableGuild.SortGuild ();
							//btnGameManager.tableGuild.Sort(SortType.desc, "GuildLevel","id");
                                }

                                //btnGameManager.GetCropsOK(mResponse.Parameters, btnGameManager.gridGuild, btnGameManager.listGuildBtns, "BtnGuildClick", btnGameManager.SetGuildBtn,"");
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;				
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo3(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((OpCode)mResponse.OperationCode)
			{
                case OpCode.GetFavorableItem://»ñµÃ´òÕÛÉÌÆ·
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                               
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GetItems://»ñµÃµÀ¾ßÉÌÆ·
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GetEquipment://»ñµÃ×°±¸ÉÌÆ·
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.BuyItem://¹ºÂòÉÌÆ·
                {
                    switch (mResponse.ReturnCode)
                    {
						
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info515"));
                                
								string itemID = (string)mResponse.Parameters[(short)ParameterType.ItemID];
								string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
								string[] strValue= (string[])mResponse.Parameters[(short)ParameterType.TableSql];
								RefershYT (strKey,strValue);
								
								
								UICL.SendMessage("CategoryTipsAsID" , itemID , SendMessageOptions.DontRequireReceiver);
								btnGameManager.CloseLoading ();
					int mGold= (int)mResponse.Parameters[(short)ParameterType.Gold];
					int mBlood= (int)mResponse.Parameters[(short)ParameterType.BloodStone];
					int[] mMoney = new int[2];
					mMoney[0] = mGold;
					mMoney[1] = mBlood;
					//商城购买统计
                    //if(itemID.Substring (0,1)=="J")
                    //{
                    //    //TD_info.setUserPurchase(string.Format("{0};{1};{2}",
                    //                                          YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBlueprint.SelectRowEqual("id",itemID.Substring
                    //                                                                              (1,itemID.Length-1))["BlueprintName"].YuanColumnText,
                    //                                          "1",
                    //                                          Mathf.Abs(mBlood) ));
                    //}else if(itemID.Substring (0,1)=="7")
                    //{
                    //    //TD_info.setUserPurchase(string.Format("{0};{1};{2}",
                    //                                          YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerPet.SelectRowEqual("ItemID",itemID.Split(',')[0])["Name"].YuanColumnText,
                    //                                          "1",
                    //                                          Mathf.Abs(mBlood)));
                    //}else
                    //{

                    //    //TD_info.setUserPurchase(string.Format("{0};{1};{2}",
                    //                                          YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytGameItem.SelectRowEqual("ItemID",itemID.Split(',')[0])["Name"].YuanColumnText,
                    //                                          "1",
                    //                                          Mathf.Abs(mBlood)));
                    //}



					UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info444"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info448"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
								btnGameManager.CloseLoading ();
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info449"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info450"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								btnGameManager.CloseLoading ();
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GuildBuild://公会建设
                {

                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
//                                Debug.Log("正常：---------------------");
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info451"));
                                string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
								RefershYT (strKey,strValue);
                                btnGameManager.CloseLoading();
                            //    btnGameManager.PanelGuildBuildRefresh();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoNum:
                            {
//                                Debug.Log("剩余次数：---------------------");
                                //本日金币建设次数已满,直接付成10/10次
                                btnGameManager.CloseLoading();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoTime: 
                            {
                                //CD5分钟没到
                                btnGameManager.CloseLoading();
                                long min = (long)mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                                //根据我传来的时间，写个假的倒计时即可。
//                                Debug.Log("剩余时间：---------------------"+min);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info452"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
								btnGameManager.CloseLoading ();
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info453"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
					            btnGameManager.CloseLoading ();
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GuildFund://公会捐献
                {

                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info454"));
                                string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
								RefershYT (strKey,strValue);
							//	btnGameManager.BtnClose();
                                GuildAddMoney.my.ShowTextUI();
                                btnGameManager.CloseLoading();
                        //        btnGameManager.PanelGuildBuildRefresh();
                                
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info455"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
								btnGameManager.CloseLoading ();
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info456"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								btnGameManager.CloseLoading ();
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.MailSend://¹ºÂòÉÌÆ·
                {

                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                if (mResponse.Parameters.ContainsKey((short)yuan.YuanPhoton.ParameterType.TableKey))
								{
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
		                                RefershYT(strKey, strValue);
								}
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info457") );
                                //invCL.SendMessage("UpDateGold1", SendMessageOptions.DontRequireReceiver);
								//invCL.SendMessage("ReInitItem",SendMessageOptions.DontRequireReceiver);
								if(null!=MailInfo.mailInfo)
								{
									MailInfo.mailInfo.ClearBage ();
								}
                                 //需要刷新人物信息
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info458"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info459"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info460"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info752"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info751"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
						case (short)yuan.YuanPhoton.ReturnCode.NoMarrowGold:
						{
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info894"),  StaticLoc.Loc.Get("info894"));
						}
						break;
                        case (short)yuan.YuanPhoton.ReturnCode.OtherLogin:
						{
                            // 邮件光圈闪，提醒有邮件到了
                            AllSpriteControl.allSC.ShowMailSprite();    	
						}
						break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                    btnGameManager.CloseLoading();
                }
                break;
                case OpCode.MailDelete:
                {

                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info461"));
							
							    if(GetMail.my!=null)
							    {
							        StartCoroutine( GetMail.my.GetMailInfo());
							    }
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                case OpCode.GetMailTool://¹ºÂòÉÌÆ·
                {
				try
				{
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								//string itemID = (string)mResponse.Parameters[(byte) yuan.YuanPhoton.ParameterType.ItemID];
								//string blood = (string)mResponse.Parameters[(byte) yuan.YuanPhoton.ParameterType.BloodStone];
								//string gold = (string)mResponse.Parameters[(byte) yuan.YuanPhoton.ParameterType.Gold];
								//invCL.SendMessage("YAddBlood", int.Parse(blood), SendMessageOptions.DontRequireReceiver);
								//invCL.SendMessage("YAddGold", int.Parse(gold), SendMessageOptions.DontRequireReceiver);
								//if(itemID!="")
								//{	
								//	if(itemID.Substring (0,2)=="88")
								//	{
								//		PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewDaojuItemAsID", itemID, SendMessageOptions.DontRequireReceiver);
								//	}
								//	else if(itemID.Substring (0,2)=="72")
								//	{
								//		PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewRideItemAsID", itemID, SendMessageOptions.DontRequireReceiver);
								//	}
								//	else if(itemID.Substring (0,2)=="70")
								//	{
								//		PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagDigestItemAsID",  itemID, SendMessageOptions.DontRequireReceiver);
								//	}
								//	else if(itemID.Substring (0,2)=="71")
								//	{
								//		PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagSoulItemAsID",  itemID, SendMessageOptions.DontRequireReceiver);
								//	}				
								//	else
								//	{
								//		PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagItemAsID", itemID, SendMessageOptions.DontRequireReceiver);
								//	} 
								//}
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);	
						warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0050"));
                                //invCL.SendMessage("UpDateGold1", SendMessageOptions.DontRequireReceiver);
								//invCL.SendMessage("ReInitItem",SendMessageOptions.DontRequireReceiver);
								
								MailInfo.mailInfo.getMail.OpenMail ();
								MailInfo.mailInfo.ClearGetBage ();;
                                 //需要刷新人物信息
					
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info463") );
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
								btnGameManager.CloseLoading ();
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info464"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info465"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								btnGameManager.CloseLoading ();
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
				}
				catch(System.Exception ex)
				{
					Debug.Log (ex.ToString ());
				}
				finally
				{
					btnGameManager.CloseLoading ();
				}
                }
                break;
                case OpCode.GetDailyBenefits:
                {
				try
				{
					switch (mResponse.ReturnCode)
					{
						
					case (short)yuan.YuanPhoton.ReturnCode.Yes:
					{
						warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info382"));
                        string[] strKey = (string[])mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.TableKey];
                        string[] strValue = (string[])mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.TableSql];
                        RefershYT(strKey, strValue);                            

                        int dailyBenefits = (int)mResponse.Parameters[(short)yuan.YuanPhoton.BenefitsType.DailyBenefits];
                        int canDailyBenefits = (int)mResponse.Parameters[(short)yuan.YuanPhoton.BenefitsType.CanDailyBenefits];
						
					    PanelDailyBenefits.panelDailyBenefits.SetBtnState(canDailyBenefits, dailyBenefits);
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.NoSlot:
					{
						//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info382"));
						//提示信息，背包已满！
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("tips030"));
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.Nothing:
					{
						//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info382"));
						//提示信息，没有可领取的奖励！
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info895"));

                        int dailyBenefits = int.Parse(BtnGameManager.yt.Rows[0]["DailyBenefits"].YuanColumnText);
                        int canDailyBenefits = int.Parse(BtnGameManager.yt.Rows[0]["CanDailyBenefits"].YuanColumnText);

                        PanelDailyBenefits.panelDailyBenefits.SetBtnState(canDailyBenefits, dailyBenefits);
					}
						break;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
					btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}

                }
                break;				
			}			
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo4(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((yuan.YuanPhoton.OperationCode)mResponse.OperationCode)
			{
			case yuan.YuanPhoton.OperationCode.PVPCreate:
			{
				switch (mResponse.ReturnCode)
				{
					case (short)yuan.YuanPhoton.ReturnCode.Yes:
					{
					Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" + System.DateTime.Now.ToString());
                        int battlefieldType = (int)mResponse.Parameters[(short)0];

                        battlefieldID = battlefieldType;
                        //todo define
                        //单人PVP
                        if (battlefieldType == 1)
                        {
                            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SetDuelType", 2, SendMessageOptions.DontRequireReceiver);

                            //if (null != PanelPVPBattlefield.pvpBattlefield)
                            //{
                            //    PanelPVPBattlefield.pvpBattlefield.SetSingleBtnState(MatchBtnState.CantMatch);
                            //    PanelPVPBattlefield.pvpBattlefield.SetTeamBtnState(MatchBtnState.CantMatch);
                            //    captainID = "";
                            //    teamMembersID.Clear();
                            //}
                        }
                        //活动PVP
                        else if (battlefieldType == 2)
                        {
                            if (null != BtnEnterActivity.btnEnterActivity) BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.CancelQueue);
                        }

                        if (null != btnGameManager)
                        {
                            btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                        }
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.No:
					{
						warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(mResponse.DebugMessage));

                        if (null != PanelPVPBattlefield.pvpBattlefield && 1 == battlefieldID)
                        {
                            PanelPVPBattlefield.pvpBattlefield.SetSingleBtnState(MatchBtnState.SingleMatch);
                        }

                        if (null != btnGameManager)
                        {
                            btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                        }
					}
						break;
				}
			}
				break;    

            case yuan.YuanPhoton.OperationCode.TeamInviteAdd:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info383"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info383"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info384"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info385"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.IsMine:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info518"));
                            }
                            break;
                    }
                }
                break;            
            case yuan.YuanPhoton.OperationCode.GetPlayerTeamID:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string teamID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID];
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.TeamPlayerUp:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info386"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
              case yuan.YuanPhoton.OperationCode.GuildPlayerPurview:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {	 
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info387"));
								RefreshList.refreshList.RefreshMe();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.IsMine: 
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
                                RefershYT(strKey, strValue);
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info969"));
                                GuildInformation.guildInfoMation.ShowBack();
								RefreshList.refreshList.RefreshMe();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                               case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.SetHonor://获得成就
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string honorID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.HonorID];
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);	
                                warnings.warningAllEnterHonor.Show(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytObjective.SelectRowEqual("id", honorID)["ObjectiveName"].YuanColumnText, YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytObjective.SelectRowEqual("id", honorID)["ObjectiveInfo"].YuanColumnText);
                            }
                            break;
                    }
                }
                break;					
            case yuan.YuanPhoton.OperationCode.SendPVPInfo://·¢ËÍPVPÐÅÏ¢
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								
//                                string itemID = (string)mResponse.Parameters[(byte)yuan.YuanPhoton.TransactionInfo.ItemID];
//                                bool isReady = (bool)mResponse.Parameters[(byte)yuan.YuanPhoton.TransactionInfo.IsReady];
//                                bool isStart = (bool)mResponse.Parameters[(byte)yuan.YuanPhoton.TransactionInfo.IsTransaction];

					

					string myTeamID = (string)mResponse.Parameters[(short)0];
//					print (myTeamID);
                   	string otherTeamID = (string)mResponse.Parameters[(short)1];
//					print (otherTeamID);
					string myTeamHeadID = (string)mResponse.Parameters[(short)2];
//					print (myTeamHeadID);
			
					string teamInfo = (string)mResponse.Parameters[(short)3];
//					print (teamInfo);
					int teamPlayerMaxNum = (int)mResponse.Parameters[(short)4];
//					print (teamPlayerMaxNum);
					string BattlefieldID = (string)mResponse.Parameters[(short)5];
//					print (BattlefieldID);
                    int instanceID = (int)mResponse.Parameters[(short)6];
					string BattlefieldStarTime = (string)mResponse.Parameters[(short)7];
//                    Debug.Log("@@@@@@@instanceID = " + instanceID);

//					string BattlefieldOpenDoorTime = (string)mResponse.Parameters[(short)7];
//					print (BattlefieldOpenDoorTime + " == BattlefieldOpenDoorTime");
//					string BattlefieldEndTime = (string)mResponse.Parameters[(short)8];
//					print (BattlefieldEndTime + " == BattlefieldEndTime");
//                                pvpReady = isReady;
//                                pvpStart = isStart;
//                                print(isReady + " --  " + isStart + " -- itemID == " + itemID);
                                string[] str = new string[12];
				
//								if(teamName.IndexOf("雪地") != (-1)){
//									str[6] = "xuedi";
//								}else
//								if(teamName.IndexOf("城区") != (-1)){
//									str[6] = "chengqu";									
//								}else{
//									str[6] = teamName;		
//								}
//                                str[0] = itemID;
                                str[1] = myTeamID;
                                str[2] = otherTeamID;
                                str[3] = teamInfo.ToString();
                                str[4] = teamPlayerMaxNum.ToString();
                                str[5] = myTeamHeadID;
								str[7] = BattlefieldID;
//								str[8] = BattlefieldOpenDoorTime;
//								str[9] = BattlefieldEndTime;
								str[10] = instanceID.ToString();
								str[11] = BattlefieldStarTime;
								UICL.SendMessage("PVPGO", str, SendMessageOptions.DontRequireReceiver);
//								if(teamPlayerMaxNum == 8){
//	                                if (isReady&& !isStart)
//	                                {
//                                        strGO=str;
//	                                    UICL.SendMessage("PVPGO", str, SendMessageOptions.DontRequireReceiver);
//	                                    lblTeamInfo.gameObject.SetActiveRecursively(false);
//	                                    if (teamPlayerMaxNum == 1)
//	                                    {
//	                                        isInPVPOne = false;
//	                                    }
//	                                    else if (teamPlayerMaxNum == 8)
//	                                    {
//	                                        isInLegion = false;
//	                                    }
//	                                    else
//	                                    {
//	                                        isInPVP = false;
//	                                    }
//	                                }
//									if(isReady && isStart && UICL != null){
//
//                                        if (Application.loadedLevelName.Substring(3, 1) == "3" || Application.loadedLevelName.Substring(3, 1) == "4")
//                                        {
//                                            
//                                            UICL.SendMessage("OpenDoor", str, SendMessageOptions.DontRequireReceiver);
//                                        }
//                                        else
//                                        {
//                                            UICL.SendMessage("EnterOpen", str, SendMessageOptions.DontRequireReceiver);
//                                        }
//									}
//
//								}else{
//	                                if (isReady && !isStart)
//	                                {
//	                                    UICL.SendMessage("WanChengPeiDui", str[4], SendMessageOptions.DontRequireReceiver);
//	                                }
//	                                if (isReady && isStart)
//	                                {
//	                                    UICL.SendMessage("PVPGO", str, SendMessageOptions.DontRequireReceiver);
//	                                    lblTeamInfo.gameObject.SetActiveRecursively(false);
//	                                    if (teamPlayerMaxNum == 1)
//	                                    {
//	                                        isInPVPOne = false;
//	                                    }
//	                                    else if (teamPlayerMaxNum == 8)
//	                                    {
//	                                        isInLegion = false;
//	                                    }
//	                                    else
//	                                    {
//	                                        isInPVP = false;
//	                                    }
//	                                }
//								}

                                if (null != PanelPVPBattlefield.pvpBattlefield)
                                {
                                    PanelPVPBattlefield.pvpBattlefield.SetSingleBtnState(MatchBtnState.CantMatch);
                                    PanelPVPBattlefield.pvpBattlefield.SetTeamBtnState(MatchBtnState.CantMatch);
                                    captainID = "";
                                    teamMembersID.Clear();
                                }
                            }
                            break;
                    }
                }
                break;
          
            case yuan.YuanPhoton.OperationCode.PVPDissolve://pvp解散111
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                UICL.SendMessage("PVPTeamJieSanDone" , SendMessageOptions.DontRequireReceiver);
								PVPCL.SendMessage ("SetDuelType",1,SendMessageOptions.DontRequireReceiver);
                                lblTeamInfo.gameObject.SetActiveRecursively(false);
                                isInPVPOne = false;
                                isInPVP = false;
                                isInLegion = false;


                                if (null != PanelPVPBattlefield.pvpBattlefield)
                                {
                                    PanelPVPBattlefield.pvpBattlefield.SetSingleBtnState(MatchBtnState.SingleMatch);
                                    PanelPVPBattlefield.pvpBattlefield.SetTeamBtnState(MatchBtnState.CantMatch);
                                }
                            }
                            break;
                    }
                }
                break;	
            case yuan.YuanPhoton.OperationCode.SendTeamTeamInfo://临时队伍请求消息
                {
                    int time = 0;
                    if (mResponse.Parameters.ContainsKey((short)yuan.YuanPhoton.ParameterType.Time))
                    {
                        time = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.Time];//剩余时间
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.SendTeamTeamLeave://临时队伍有人离开
                {
                    lblTeamInfo.gameObject.SetActiveRecursively(false);
                }
                break;				
			}			
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo5(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((yuan.YuanPhoton.OperationCode)mResponse.OperationCode)
			{
            case yuan.YuanPhoton.OperationCode.TempTeamOK://临时队伍组队成功
                {
//				Debug.Log ("1111111----------------------");
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
//								Debug.Log ("-----------11111111----------"+mResponse.Parameters.Count);
                                string teamName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamName];
                                BtnGameManager.IsInTempTeam = false;
                                int teamID = int.Parse((string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID]);
                                string mapID = string.Empty;
                               string levelID = string.Empty;
                                 int  levelL = 0;
                                int tempLevel = 0;
                                string[] mapabout = teamName.Split(',');
                              //  levelID = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapID", levelID)["MapID"].YuanColumnText;
                                mapID = levelID + levelL;	
								object[] objs;
								objs = new object[3];
								objs[0] = teamID;
                                objs[1] = mapabout[0];//levelID;
                                objs[2] = int.Parse(mapabout[1]);//levelL;					
								UICL.SendMessage("GoLinShiFuBen" , objs , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                 lblTeamInfo.gameObject.SetActiveRecursively(false);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.IsMine:
                            {
                                string teamName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamName];
                                BtnGameManager.IsInTempTeam = false;
                                int teamID = int.Parse((string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID]);
                                //要进入的地图的instensid
                                teaminstensid = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];

                                string mapID = string.Empty;
                                string levelID = string.Empty;
                                int levelL = 0;
                                int tempLevel = 0;
                                string[] mapabout = teamName.Split(',');


                                mapID = levelID + levelL;
                                object[] objs;
                                objs = new object[4];
                                objs[0] = teamID;
                                objs[1] = mapabout[0];//levelID;
                                objs[2] = int.Parse(mapabout[1]);//levelL;
                                objs[3] = teaminstensid;
                                UICL.SendMessage("CZGoLinShiFuBen", objs, SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.TempTeamWait://临时队伍开始等待
                {
					UICL.SendMessage("ShowFuBenQR" , SendMessageOptions.DontRequireReceiver);
                }
                break;
            case yuan.YuanPhoton.OperationCode.AddTempTeam://临时队伍排队
                {
                    switch (mResponse.ReturnCode)
                    {
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
						{
								string teamID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID];
								BtnGameManager.IsInTempTeam = true;
								warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info466"));
								lblTeamInfo.text = StaticLoc.Loc.Get("info319")+"";
								lblTeamInfo.gameObject.SetActiveRecursively(true);
								MainTW.SendMessage("SetPaiDuiAsID", teamID, SendMessageOptions.DontRequireReceiver);		
								UICL.SendMessage("AddTempTeamTeamPlayerYes" , SendMessageOptions.DontRequireReceiver);

								btnGameManager.objTeam.SendMessage("ShowTeamLayer",false,SendMessageOptions.DontRequireReceiver);
							//TODO:加入队伍成功，提示玩家等待队长同意进入副本
						}
						break;
						case (short)yuan.YuanPhoton.ReturnCode.YesDo:
						{
							string teamID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID];
							BtnGameManager.IsInTempTeam = true;
							warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info466"));
							lblTeamInfo.text = StaticLoc.Loc.Get("info319")+"";
							lblTeamInfo.gameObject.SetActiveRecursively(true);
							MainTW.SendMessage("SetPaiDuiAsID", teamID, SendMessageOptions.DontRequireReceiver);
							
								int mapInstensID = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamName];//地图实例ID
								string mapID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamInfo];//地图MapID
								object[] objs = new object[2];
								objs[0] = mapInstensID;
								objs[1] = mapID;
								UICL.SendMessage("AddTempTeamYesDo" , objs , SendMessageOptions.DontRequireReceiver);
							//TODO:队长已经在副本中，直接让玩家进入地图
						}
					break;
                        case (short)yuan.YuanPhoton.ReturnCode.HeadYes:
                            {
                                string teamID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamID];
                                BtnGameManager.IsInTempTeam = true;
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info466"));
                                lblTeamInfo.text = StaticLoc.Loc.Get("info319")+"";
                                lblTeamInfo.gameObject.SetActiveRecursively(true);
                                MainTW.SendMessage("SetPaiDuiAsID", teamID, SendMessageOptions.DontRequireReceiver);

								string mapID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamInfo];
								UICL.SendMessage("DungeonTeamHeadYes" , mapID , SendMessageOptions.DontRequireReceiver);
								//TODO:创建队伍成功，队长可以选择进入副本或者等待其他队员再进入
                            }
                            break;
							case (short)yuan.YuanPhoton.ReturnCode.TempTeamHeadGoMap:
							{
									int mapInstensID = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamName];//地图实例ID
									string mapID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamInfo];//地图MapID
									object[] objs = new object[2];
									objs[0] = mapInstensID;
									objs[1] = mapID;
									UICL.SendMessage("AddTempTeamYesDo" , objs , SendMessageOptions.DontRequireReceiver);
					//					queren
								//TODO:队长已经进入副本，提示队员是否跟随加入
							}
							break;
							case (short)yuan.YuanPhoton.ReturnCode.TempTeamNewPlayerAdd:
							{
									string mapID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamInfo];//地图MapID
									UICL.SendMessage("TempTeamNewPlayerAdd" , mapID , SendMessageOptions.DontRequireReceiver);
									//TODO:队伍有新人加入，提示队长是否要进入副本
							}
							break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info467"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info468"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.IsMine:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0020"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info477"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoTime:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0106"));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.RemoveTempTeam://退出排队
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                BtnGameManager.IsInTempTeam = false;
                                btnGameManager.OutTempTeam();
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info469") );
                                lblTeamInfo.gameObject.SetActiveRecursively(false);
								UICL.SendMessage("TempTeamHeadRemove", true, SendMessageOptions.DontRequireReceiver);
							}
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info470"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.GetTempTeam://获取临时列表
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                               btnGameManager.GetTempTeamOK(mResponse.Parameters);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;			
            case yuan.YuanPhoton.OperationCode.RemoveLegion:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info389"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info390") );
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                               // warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info391") );
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                    }
                }
                break;			
            case yuan.YuanPhoton.OperationCode.TeamRemove:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info392"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info393"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info394"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info698"));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                BtnGameManager.isLegionTempCreate = false;
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.PVPInviteAdd:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info395"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info397"));
                            }
                            break;		
                        case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info447"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Create:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info396"));
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info697"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;


            case yuan.YuanPhoton.OperationCode.LegionInviteAdd:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info397"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasTeam:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info397"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info398"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
             case yuan.YuanPhoton.OperationCode.PVPPlayerUp:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info399"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;				
			}			
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo6(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((yuan.YuanPhoton.OperationCode)mResponse.OperationCode)
			{
                case yuan.YuanPhoton.OperationCode.LegionPlayerUp:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info400"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.GuildInviteAdd:
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show("",StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.GuildRemove:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                //刷新人员列表
                                RefreshList.refreshList.RefreshGuildPlayer();
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
                                RefershYT(strKey, strValue); 

								warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.GuildStopTalk:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
								warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;	
            case yuan.YuanPhoton.OperationCode.MailOtherGet://¹ºÂòÉÌÆ·
                {

                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
					warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0050"));
                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);	
                                invCL.SendMessage("UpDateGold1", SendMessageOptions.DontRequireReceiver);
                               //需要刷新人物信息
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.AddLegionQueue://军团加入队列
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info401"));
                                lblTeamInfo.text = StaticLoc.Loc.Get("info320")+"";
                                lblTeamInfo.gameObject.SetActiveRecursively(true);
                                isInLegion = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;		
            case yuan.YuanPhoton.OperationCode.SetTitle://获得称号
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string honorID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.HonorID];
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);	
                                warnings.warningAllEnterHonor.Show(StaticLoc.Loc.Get("info516"), honorID);
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.GetPVETeamList://ÐÂ½¨PVPÐ¡¶Ó
                {
                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                btnGameManager.GetOnePVPList(mResponse.Parameters);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;				
            case yuan.YuanPhoton.OperationCode.LegionOneAdd://战场排队
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info471"));
								PVPCL.SendMessage ("SetBattlefieldType",2,SendMessageOptions.DontRequireReceiver);
                                lblTeamInfo.text =StaticLoc.Loc.Get("info320")+ "";
                                lblTeamInfo.gameObject.SetActiveRecursively(true);
                                isInLegion = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.LegionOneTeamAdd://加入指定ID战场
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), "您已成功加入战场");
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info471"));
                                lblTeamInfo.text = StaticLoc.Loc.Get("info320")+"";
                                lblTeamInfo.gameObject.SetActiveRecursively(true);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;				
			}			
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo7(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((yuan.YuanPhoton.OperationCode)mResponse.OperationCode)
			{
            case yuan.YuanPhoton.OperationCode.LegionOneRemove://退出战场
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info472") );
								PVPCL.SendMessage ("SetBattlefieldType",1,SendMessageOptions.DontRequireReceiver);
                                lblTeamInfo.gameObject.SetActiveRecursively(false);
                                isInLegion = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.LegionOneList://获取战场列表
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                btnGameManager.GetLegionOneList(mResponse.Parameters);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;		
            case yuan.YuanPhoton.OperationCode.GuildLevelUp://公会升级
                {

                    switch (mResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info473"));
                                GuildInformation.guildInfoMation.ShowBack();
                                btnGameManager.PanelGuildBuildRefresh();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
								warnings.warningAllTime.Show("",StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case(short) yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                int GuildGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.Gold];
                                int GuildValue = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.NumStart];
                                warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1185") + GuildGold + StaticLoc.Loc.Get("info1186") + GuildValue + StaticLoc.Loc.Get("info1187"));
                                warnings.warningAllEnterClose.btnEnter.target = btnGameManager.gameObject;
                                warnings.warningAllEnterClose.btnEnter.functionName = "YesGuildLevelUP";
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.LegionOneClose://战场关闭
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;	
            case yuan.YuanPhoton.OperationCode.InRoom://加入房间
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Create://创建房间
                            {
                                string mRoomName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.RoomName];
								lding.SendMessage("PVPCreate", SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Join://加入房间
                            {
                                string mRoomName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.RoomName];
								lding.SendMessage("PVPJoin", SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case yuan.YuanPhoton.OperationCode.LeaveRoom://离开房间
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                               
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;		
            case yuan.YuanPhoton.OperationCode.ActivityPVPRemove://退出战场
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info474"));
								PVPCL.SendMessage ("SetBattlefieldType",1,SendMessageOptions.DontRequireReceiver);
                                lblTeamInfo.gameObject.SetActiveRecursively(false);
                                isInLegion = false;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
			 case yuan.YuanPhoton.OperationCode.GetBlood91://购买91代币
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);  
								invCL.SendMessage("UpDateGold1", SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
				case yuan.YuanPhoton.OperationCode.OtherLogin:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                               btnGameManager.OtherLogin ();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
				case yuan.YuanPhoton.OperationCode.RedemptionCode:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

                                string mPageId = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
                               btnGameManager.redemptionCode.GetPage (mPageId);
                            }
                            break;
					case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                               warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info475"));
                            }
                            break;
					case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                               warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info746"));
                            }
                            break;
                    case (short)yuan.YuanPhoton.ReturnCode.IsDone:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1046"));
                            }
                            break;
                    case (short)yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1047"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;				
			}			
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo8(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((OpCode)mResponse.OperationCode)
			{
			 case OpCode.PVP1Invite://邀请玩家决斗
                {
                    switch (mResponse.ReturnCode)
                    {
                 		case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
//								Debug.Log ("---------------------");
                                string mItemID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];//决斗ID
                                int mTeamInfo = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamInfo];
//						Debug.Log ("---------------------"+mTeamInfo);
                                string[] str = new string[9];
								str[6] = "";		
                                str[0] = mItemID;
                                str[1] = mTeamInfo.ToString();
                                str[2] = "";					
                                str[3] = mTeamInfo.ToString();
                                str[4] = "1";					
                                str[5] = "";
	                            UICL.SendMessage("PVPGO", str, SendMessageOptions.DontRequireReceiver);
							}
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
								yuan.YuanMemoryDB.YuanTable ytPlayer=new yuan.YuanMemoryDB.YuanTable("","");
								ytPlayer.CopyToDictionary (mResponse.Parameters);
								UICL.SendMessage("YaoQingPVP" , ytPlayer , SendMessageOptions.DontRequireReceiver);
//                                warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),"邀请失败，您邀请的玩家不在线");
                            }
                            break;
					  case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info476"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
			case OpCode.InviteGoPVE://邀请玩家进入自己的副本
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								string mRoomName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.RoomName];//RoomID
								int instensID=(int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
								object[] objs = new object[2];
								objs[0] = mRoomName;
								objs[1] = instensID;
								
								UICL.SendMessage("YaoQingGO" , objs , SendMessageOptions.DontRequireReceiver);
							}
					break;
	                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllTime.Show ("",StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;				
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
								yuan.YuanMemoryDB.YuanTable ytPlayer=new yuan.YuanMemoryDB.YuanTable("","");
								ytPlayer.CopyToDictionary (mResponse.Parameters);
								UICL.SendMessage("YaoQingFuben" , ytPlayer , SendMessageOptions.DontRequireReceiver);
//                                warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),"邀请失败，您邀请的玩家不在线");
                            }
                            break;
					    case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                            {
                                warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info477"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
			case OpCode.OtherBindDevice://角色被其他设备绑定，强制下线
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								GameReonline.isEnable=false;
								InRoom.GetInRoomInstantiate ().peer.Disconnect ();
								BtnManager.isOhterLogin=true;
								BtnManager.strOtherLogin=StaticLoc.Loc.Get("info561");
								PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
                            }
                            break;
					    case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								GameReonline.isEnable=false;
								InRoom.GetInRoomInstantiate ().peer.Disconnect ();
								BtnManager.isOhterLogin=true;
								BtnManager.strOtherLogin=StaticLoc.Loc.Get("info562");
								PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                    }
                }
                break;	
			case OpCode.TeamHeadIn://队伍队长消息
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								string mTeamInfo = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamInfo];
                                int mTeamMapInsId = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
								object[] objs = new object[2];
								objs[0] = mTeamInfo;
								objs[1] = mTeamMapInsId;
								UICL.SendMessage("DuiYuanGoFB" , objs , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
					    case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								Debug.LogWarning (StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;	
			case OpCode.BindDevice:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								Song.SendMessage("BangDingOK" , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
					    case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								Debug.LogWarning (StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
			case OpCode.RefershTable:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
									yuan.YuanMemoryDB.YuanTable tempYt=new yuan.YuanMemoryDB.YuanTable("PlayerInfo","PlayerID");
									tempYt.CopyToDictionary (mResponse.Parameters);
									BtnGameManager.yt.Rows=tempYt.Rows;
									invCL.SendMessage("ReInitItem", SendMessageOptions.DontRequireReceiver);

					
					#region 七日奖励零点刷新
					int dailyBenefits = 0;
					int canDailyBenefits = 0;
					if (int.TryParse(tempYt.Rows[0]["DailyBenefits"].YuanColumnText, out dailyBenefits) && int.TryParse(tempYt.Rows[0]["CanDailyBenefits"].YuanColumnText, out canDailyBenefits))
					{
						PanelDailyBenefits.panelDailyBenefits.SetBtnState(canDailyBenefits, dailyBenefits);
					}
					#endregion
					
					#region 在线宝箱零点刷新
					int numOpenBox = int.Parse(tempYt.Rows[0]["NumOpenBox"].YuanColumnText); // 已开宝箱数量
					int onlineChestsTime = int.Parse(tempYt.Rows[0]["OnlineChestsTime"].YuanColumnText); // 在线宝箱当天累计时间
					#endregion
                            }
                            break;
                    }
                }
                break;		
			case OpCode.RefershTableSome:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
								string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
                            }
                            break;
                    }
                }
                break;	
			case OpCode.ClientBuyItem:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string itemID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								UICL.SendMessage("CategoryTipsAsID" , itemID , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info444"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info448"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
								btnGameManager.CloseLoading ();
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info449"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info450"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								btnGameManager.CloseLoading ();
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
                }
                break;		
			case OpCode.ClientGetItemSome:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string itemID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								UICL.SendMessage("CategoryTipsAsID" , itemID , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info444"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info448"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
								btnGameManager.CloseLoading ();
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info449"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {								
								btnGameManager.CloseLoading ();
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info450"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								btnGameManager.CloseLoading ();
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
                }
                break;					
            case OpCode.ClientMoney:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);

                                int mGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.Gold];
                                int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];
								SetMoneyMessage (mGold,mBlood);
                            }
                            break;
							case (short) yuan.YuanPhoton.ReturnCode.NoGold:
							{
								warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info491"));
							}
							break;
							case (short) yuan.YuanPhoton.ReturnCode.NoBloodStone:
							{
								//warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
							}
							break;	
                    }
					PanelStatic.StaticBtnGameManager.CloseLoading ();
                }
                break;	    
			case OpCode.EquepmentBuild:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
						            RefershYT(strKey, strValue);
                                    //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info694"));
						            UICL.SendMessage("ReturnEquepmentBuild", true, SendMessageOptions.DontRequireReceiver);

						            int mGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.Gold];
                                    int mBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.BloodStone];
                                    int marrowIron = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowIron];
                                    int marrowGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowGold];

                                    SetMoneyMessage(mGold, mBlood, marrowIron, marrowGold);
                                    int[] mMoney = new int[2];
                                    mMoney[0] = mGold;
                                    mMoney[1] = mBlood;
                                    UICL.SendMessage("CharBarTextMoney", mMoney, SendMessageOptions.DontRequireReceiver);
									// 统计装备强化血石消耗			
						//TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo035"),"1", Mathf.Abs(mBlood)));
									if (null != EquipEnhance.instance)
                                    {
                                    EquipEnhance.instance.ShowEquipEnhanceItem(true);
									UICL.SendMessage("BuildSuccessful", mMoney, SendMessageOptions.DontRequireReceiver);
                                    }
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
                                    RefershYT(strKey, strValue);
                                    UICL.SendMessage("ReturnEquepmentBuild", true, SendMessageOptions.DontRequireReceiver);
                                    int mGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.Gold];
                                    int mBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.BloodStone];
                                    int marrowIron = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowIron];
                                    int marrowGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowGold];
                                    SetMoneyMessage(mGold, mBlood, marrowIron, marrowGold);
                                    int[] mMoney = new int[2];
                                    mMoney[0] = mGold;
                                    mMoney[1] = mBlood;
                                    UICL.SendMessage("CharBarTextMoney", mMoney, SendMessageOptions.DontRequireReceiver);
                                    //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips009"));
                                    if (null != EquipEnhance.instance)
                                    {
                                        EquipEnhance.instance.ShowEquipEnhanceItem(false);
                                    }
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                                {
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                                {
                                    //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                    SwitchToStore();
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoMarrowIron:
                                {
                                    //提示，没有足够的精铁粉末！
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info880"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoMarrowGold:
                                {
                                    //提示，没有足够的精金粉末！
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info881"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                                {
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info695"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                                {
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0130"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    finally
                    {
                        btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    }
                }
                break;	
			case OpCode.EquepmentHole:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info696"));
								UICL.SendMessage("ReturnEquepmentHole" , SendMessageOptions.DontRequireReceiver);

                                int mGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.Gold];
                                int mBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.BloodStone];
								SetMoneyMessage (mGold,mBlood);
					int[] mMoney = new int[2];
					mMoney[0] = mGold;
					mMoney[1] = mBlood;
					UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;	
			case OpCode.EquepmentMosaic:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info711"));
								UICL.SendMessage("ReturnEquepmentHole" , SendMessageOptions.DontRequireReceiver);

                                int mGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.Gold];
                                int mBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.BloodStone];
								SetMoneyMessage (mGold,mBlood);
					int[] mMoney = new int[2];
					mMoney[0] = mGold;
					mMoney[1] = mBlood;
					UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info710"));
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;			
			case OpCode.Training:
                {
//					Debug.Log ("-----------------------Training");
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                int[] ids = (int[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info715"));
					            UICL.SendMessage("ReturnTraining" , ids , SendMessageOptions.DontRequireReceiver);

                                int mGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.Gold];
                                int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];
								            SetMoneyMessage (mGold,mBlood);
					            int[] mMoney = new int[2];
					            mMoney[0] = mGold;
					            mMoney[1] = mBlood;
					            UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);

					            //TD训练血石消耗
					            //TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo033"),"1", Mathf.Abs(mBlood)));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info714"));
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.IsDone:
                            {
                                int playerBloodStone = (int)mResponse.Parameters[(short)ParameterType.BloodStone];
                                int playerTrain = (int)mResponse.Parameters[(short)ParameterType.ItemID];
                                int canBlood = (int)mResponse.Parameters[(short)ParameterType.UseBlood];
                            //    warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0114") + playerBloodStone + StaticLoc.Loc.Get("meg0115") + playerTrain + StaticLoc.Loc.Get("meg0116") + canBlood + StaticLoc.Loc.Get("meg0117"));
                                warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0114") + playerBloodStone + StaticLoc.Loc.Get("meg0115") + playerTrain + StaticLoc.Loc.Get("meg0116") + canBlood + StaticLoc.Loc.Get("meg0117"));
				      			warnings.warningAllEnterClose.btnEnter.target = btnGameManager.gameObject;
                                warnings.warningAllEnterClose.btnEnter.functionName = "QuickTraining";
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoNum:
                            {
                                int playerBloodStone = (int)mResponse.Parameters[(short)ParameterType.BloodStone];
                                int playerTrain = (int)mResponse.Parameters[(short)ParameterType.ItemID];
                                int canBlood = (int)mResponse.Parameters[(short)ParameterType.UseBlood];
                               // warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0114") + playerBloodStone + StaticLoc.Loc.Get("meg0115") + playerTrain + StaticLoc.Loc.Get("meg0116") + canBlood + StaticLoc.Loc.Get("meg0117"));
                                TrainSwitchToStore(StaticLoc.Loc.Get("meg0114") + playerBloodStone + StaticLoc.Loc.Get("meg0115") + playerTrain + StaticLoc.Loc.Get("meg0116") + canBlood + StaticLoc.Loc.Get("meg0117"));
                            }
                            break;
                    }
					btnGameManager.CloseLoading ();
                }
                break;					
			case OpCode.TrainingSave:
                {
//					Debug.Log ("-----------------------TrainingSave");
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info716"));
								UICL.SendMessage("ReturnTrainingSave" , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info714"));
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                UICL.SendMessage("ReturnTrainingSave" , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;								
			}			
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo9(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((OpCode)mResponse.OperationCode)
			{
                case OpCode.EquepmentProduce:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info712"));
								UICL.SendMessage("ReturnEquepmentProdece" , SendMessageOptions.DontRequireReceiver);

                                int mGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.Gold];
                                int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];
								SetMoneyMessage (mGold,mBlood);
					int[] mMoney = new int[2];
					mMoney[0] = mGold;
					mMoney[1] = mBlood;
					UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
					//统计装备制造血石消耗
					//TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo034"),"1", Mathf.Abs(mBlood)));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info713"));
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;
                case OpCode.GetStoreList:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.ItemID];

								UICL.SendMessage("retrunGetStoreList9" , strKey , SendMessageOptions.DontRequireReceiver);
//					Debug.Log("999999");
					short type = (short)mResponse.Parameters[(short)ParameterType.StoreType];
					yuan.YuanPhoton.StoreType storeType = (yuan.YuanPhoton.StoreType)type;
								if(storeType==yuan.YuanPhoton.StoreType.Blacksmith)
								{
//						Debug.Log("wen hao 33333");
									string[] strRandom = (string[])mResponse.Parameters[(short)ParameterType.NumRandom];
						UICL.SendMessage("returnGetStoreList3" , strRandom , SendMessageOptions.DontRequireReceiver);
								}

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips049"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;
                case OpCode.BuyStoreClient:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
					
								string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
								string[] strValue= (string[])mResponse.Parameters[(short)ParameterType.TableSql];
//					for(int i=0; i<strValue.Length; i++){
//						Debug.Log(strKey[i] + " ------key------- " + strValue[i] + " =================== " + i);
//					}
								RefershYT (strKey,strValue);
								short type = (short)mResponse.Parameters[(short)ParameterType.StoreType];

								//string[] itemID = (string[])mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.ItemID];
								yuan.YuanPhoton.StoreType storeType = (yuan.YuanPhoton.StoreType)type;
																
					int mGold = (int)mResponse.Parameters[(short)ParameterType.Gold];
					int mBlood = (int)mResponse.Parameters[(short)ParameterType.BloodStone];
								SetMoneyMessage (mGold,mBlood);
                    int HeroStone = (int)mResponse.Parameters[(short)ParameterType.HeroStone];
                                SetMoneyMessage(mGold, mBlood);
								SetHeroBadge(0,HeroStone);
                    
					int[] mMoney = new int[2];
					mMoney[0] = mGold;
					mMoney[1] = mBlood;
					UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);


					int[] mStone = new int[2];
					mStone[0] = HeroStone;
					mStone[1] = 0;
					UICL.SendMessage("CharBarTextStone" , mStone , SendMessageOptions.DontRequireReceiver);

                    string id = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.DeviceID];
								UICL.SendMessage("CategoryTipsAsID" , id , SendMessageOptions.DontRequireReceiver);
					//统计玩家购买装备（NPC购买）
					//TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo036"),"1", Mathf.Abs(mBlood)));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info717"));
                            }
                            break;		
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips049"));
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NOHeroStone:
                            {
								warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1062"));
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NOConquerStone:
                            {
								warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1063"));
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NOStoreID:
                            {
								warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1068"));
								Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
					btnGameManager.CloseLoading ();
                }
                break;
                case OpCode.GetRandomItem:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								//string[] itemID = (string[])mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.ItemID];

                                int mGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.Gold];
                                int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];	
								SetMoneyMessage (mGold,mBlood);

                                string id = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.DeviceID];
								UICL.SendMessage("CategoryTipsAsID" , id , SendMessageOptions.DontRequireReceiver);
								//
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info717"));
                            }
                            break;		
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips049"));
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;
                case OpCode.AddExperience:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
//								Debug.Log ("-----------------------addExp");
								string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)ParameterType.TableSql];
								RefershYT (strKey,strValue);
                                bool isLevelUp = (bool)mResponse.Parameters[(short)ParameterType.PlayerLevel];
                                string beforeLevel = (string)mResponse.Parameters[(short)ParameterType.NumStart];
                                string nowLevel = (string)mResponse.Parameters[(short)ParameterType.NumEnd];
                                int addExp = (int)mResponse.Parameters[(short)ParameterType.ItemID];
								UICL.SendMessage("retrunReturnAddExperienceF" , addExp , SendMessageOptions.DontRequireReceiver);
								string[] strs;
								strs = new string[2];
								strs[0] = beforeLevel;
								strs[1] = nowLevel;
//                                Debug.Log("--------------------PlayerExp:" + BtnGameManager.yt[0]["Exp"].YuanColumnText);
								if(isLevelUp)
									UICL.SendMessage("retrunReturnUpDateLevelF" , strs , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
                }
                break;
                case OpCode.PlayerLook:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                int[] info = (int[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TeamInfo];
                                string sender = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.MailSender];
								PhotonView tempView=null;
								PlayerSendInfo pInfo;
								pInfo.x=info[0];
								pInfo.y=info[1];
								pInfo.z=info[2];
								pInfo.rotation=info[3];
								pInfo.animStuat=info[4];
								pInfo.animSeed=info[5];
//								Debug.Log (string.Format ("{0},{1},{2},{3},{4}",sender,pInfo.x,pInfo.y,pInfo.z,pInfo.rotation));
								if(InRoom.listLookPlayer.TryGetValue (sender,out tempView))
								{
									tempView.SendMessage ("RetrunPlayerStatus",pInfo,SendMessageOptions.DontRequireReceiver);
								}
                            }
                            break;				
                    }
                }
                break;
                case OpCode.DoneCard:
                {
//				Debug.Log("翻牌++++++++++++++++++++++++++++++++++++++11111111");
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {

					//string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableKey];
					string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
					string[] strValue= (string[])mResponse.Parameters[(short)ParameterType.TableSql];
								RefershYT (strKey,strValue);
								//string[] itemID = (string[])mResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.ItemID];
																
					int mGold = (int)mResponse.Parameters[(short)ParameterType.Gold];
					int mBlood = (int)mResponse.Parameters[(short)ParameterType.BloodStone];
								SetMoneyMessage (mGold,mBlood);

					int[] mMoney = new int[2];
					mMoney[0] = mGold;
					mMoney[1] = mBlood;
					UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
					
					string[] ids= (string[])mResponse.Parameters[(short)ParameterType.ItemID];
					int xingxing= (int)mResponse.Parameters[(short)ParameterType.MyRank];
					int zongFeng= (int)mResponse.Parameters[(short)ParameterType.PageNum];
					int addPreatige= (int)mResponse.Parameters[(short)ParameterType.HoleID];
					int addExp= (int)mResponse.Parameters[(short)ParameterType.GemID];
					int firstRewardBlood= (int)mResponse.Parameters[(short)ParameterType.HonorID];



                    //英雄徽章
                    int horeStone = (int)mResponse.Parameters[(short)ParameterType.MoneyNumb];
					//组队经验
					int teamExp = (int)mResponse.Parameters[(short)1000];
					//组队金钱
					int teamgold = (int)mResponse.Parameters[(short)1001];



					object[] objs = new object[11];
					objs[0] = mGold;
					objs[1] = mBlood;
					objs[2] = ids;
					objs[3] = xingxing;
					objs[4] = zongFeng;
					objs[5] = addPreatige;
					objs[6] = addExp;
					objs[7] = firstRewardBlood;
					objs[8] = horeStone;
					objs[9] = teamgold;
					objs[10] = teamExp;

//					Debug.Log("ran````````````````````````````````````````````````````"+teamgold);
//					Debug.Log("ran```````````````````````````````====================="+teamExp);
					UICL.SendMessage("retrunDoneCard" , objs , SendMessageOptions.DontRequireReceiver);
//					Debug.Log("翻牌++++++++++++++++++++++++++++++++++++++");
                            }
                            break;			
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;

                case OpCode.OpenCard:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
				
					
					string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
					string[] strValue= (string[])mResponse.Parameters[(short)ParameterType.TableSql];
								RefershYT (strKey,strValue);
																
					int mGold=(int)mResponse.Parameters[(short)555];
					int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];
								SetMoneyMessage (mGold,mBlood);
					
								string id= (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.DeviceID];
								UICL.SendMessage("CategoryTipsAsID" , id , SendMessageOptions.DontRequireReceiver);

								UICL.SendMessage("retrunOpenCard" , id , SendMessageOptions.DontRequireReceiver);
						//统计购买副本翻牌血石消耗
					//TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo032"),"1", Mathf.Abs(mBlood)));
							}
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info717"));
                            }
                            break;		
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
//					Debug.Log ("-----------------------------------------");
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips049"));
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info491"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                            {
                                //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info490"));
                                SwitchToStore();
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;
                case OpCode.DoneCardPVP:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
//					Debug.Log("111111123123123123");

                                string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)ParameterType.TableSql];
                                RefershYT(strKey, strValue);
                                int PVPStone = (int)mResponse.Parameters[(short)ParameterType.MoneyNumb];
								int money = (int)mResponse.Parameters[(short)ParameterType.Gold];
                                int power = (int)mResponse.Parameters[(short)100];// 体力，单人pvp时不加体力为0，影魔加体力；所以要加判断

								string[] id= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
								object[] obj= new object[4];
								obj[0] = id;
								obj[1] = PVPStone;
                                obj[2] = money;
                                obj[3] = power;

                                UICL.SendMessage("retrunPVPCard" , obj , SendMessageOptions.DontRequireReceiver);
                                
								PVPTimeControl.PvpT.IsContinue = true;
							}
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;
                case OpCode.IsSaveDate:
                {
				
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
							}
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
								PanelStatic.StaticBtnGameManager.NoSaveData ();
							}
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
								PanelStatic.StaticBtnGameManager.NoSaveData ();
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;						
                    }
					btnGameManager.CloseLoading ();
                }
                break;
                case OpCode.ActivityGetInfo:
                {
				    AnlaysActivityGetInfo (mResponse);
			    }
                break;
                    case OpCode.ActivityGetReward:
                {			
				    AnlaysActivityGetReward (mResponse);
			    }	
			    break;
                case OpCode.ActivityFirstCharge:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Create:// 接收到下发的奖励信息
                                {
                                    string[] rewards = (string[])mResponse.Parameters[(short)ParameterType.ItemIDS];
                                    
                                    //foreach (string str in rewards)
                                    //{
                                    //    Debug.Log("!!!!!!!!!!!!---------ActivityFirstChargeRewards:" + str);
                                    //}

                                    FristTimeReward.rewards = rewards;

                                    if (null != FristTimeReward.fristTimeReward)
                                    {
                                        FristTimeReward.fristTimeReward.FristReward(rewards);
                                    }
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.IsDone: // 可以领取首充奖励
                                {
                                    if (null != FristTimeReward.fristTimeReward)
                                    {
                                        FristTimeReward.fristTimeReward.EnableGetRewardBtn(true);
                                    }
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Yes: // 领取奖励成功
                                {
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1001"));

                                    if (null != FristRewardControl.fristReward)
                                    {
                                        FristRewardControl.fristReward.ShowVIP();
                                    }
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No: // 奖励已领取
                                {
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1002"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoSlot: // 背包已满
                                {
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("tips030"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Nothing: // 正版审核，vip按钮不显示
                                {
                                    int showVIP = (int)mResponse.Parameters[(short)ParameterType.ActivityState];

                                    PlayerPrefs.SetInt("ShowVIP", showVIP);// 0表示vip不显示，1表示显示
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    finally
                    {
                        btnGameManager.CloseLoading();
                    }
                }
                break;
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo10(Zealm.OperationResponse mResponse)
	{
		try	
		{
			switch((OpCode)mResponse.OperationCode)
			{
				case OpCode.Request://ÇëÇó
                {
                    short requstType = (short)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.RequestType];
                    switch (requstType)
                    {
                        case (short)yuan.YuanPhoton.RequstType.TeamAdd:
                            {
								if(PlayerPrefs.GetInt ("RefusalTeam")==0)
					            {
                                    SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info699"), StaticLoc.Loc.Get("info700"), "GetTeamRequsetYes", "GetTeamRequsetNo");
					            }
					            else
					            {
						            InRoom.GetInRoomInstantiate().ReturnRequest(yuan.YuanPhoton.ReturnCode.No, mResponse.Parameters);
					            }
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnTeamAdd:
                            {
                                SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info701"), StaticLoc.Loc.Get("info700"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.GropsAdd:
                            {
                                SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info699"), StaticLoc.Loc.Get("info702"), "GetTeamRequsetYes", "GetTeamRequsetNo");
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnCropsAdd:
                            {
                                SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info701"), StaticLoc.Loc.Get("info702"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.LegionDBAdd:
                            {
                                SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info699"), StaticLoc.Loc.Get("info703"), "GetTeamRequsetYes", "GetTeamRequsetNo");
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.LegionTempAdd:
                            {
                                SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info699"), StaticLoc.Loc.Get("buttons336"), "GetTeamRequsetYes", "GetTeamRequsetNo");
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnLegionDBAdd:
                        case (short)yuan.YuanPhoton.RequstType.ReturnLegionTempAdd:
                            {
                                SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info701"), StaticLoc.Loc.Get("info702"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.GuildAdd:
                            {
                                SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info699"), StaticLoc.Loc.Get("buttons093"), "GetTeamRequsetYes", "GetTeamRequsetNo");
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnGuildAdd:
                            {
								SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info970"), StaticLoc.Loc.Get("buttons093"));
								invCL.SendMessage("ReInitItem", SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.TeamInviteAdd:
                            {
                                if (PlayerPrefs.GetInt("RefusalTeam") == 0)
                                {
                                    SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info704"), StaticLoc.Loc.Get("info700"), "GetTeamRequsetYes", "GetTeamRequsetNo");
                                }
                                else
                                {
                                    InRoom.GetInRoomInstantiate().ReturnRequest(yuan.YuanPhoton.ReturnCode.No, mResponse.Parameters);
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnTeamInviteAdd:
                            {
					          //  if(PlayerPrefs.GetInt ("RefusalTeam")==0)
					          //  {
                                    SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info705"), StaticLoc.Loc.Get("info700"));  
					           // }
					          //  else
					         //   {
						      //      InRoom.GetInRoomInstantiate().ReturnRequest(yuan.YuanPhoton.ReturnCode.No, mResponse.Parameters);
					         //   }
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.PVPInviteAdd:
                            {
                                SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info704"), StaticLoc.Loc.Get("info702"), "GetTeamRequsetYes", "GetTeamRequsetNo");
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnPVPInviteAdd:
                            {
                                SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info705"), StaticLoc.Loc.Get("info702"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.LegionInviteAdd:
                            {
                                SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info704"), StaticLoc.Loc.Get("info706"), "GetTeamRequsetYes", "GetTeamRequsetNo");
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnLegionInviteAdd:
                            {
                                SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info705"), StaticLoc.Loc.Get("info699"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.GuildInviteAdd:
                            {
					SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info704"), StaticLoc.Loc.Get("info692"), "GetTeamRequsetYes", "GetTeamRequsetNo");
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnGuildInviteAdd:
                            {
                                SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info705"), StaticLoc.Loc.Get("buttons093"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.PVP1Invite:
                            {
					            if(PlayerPrefs.GetInt ("RefusalTeam")==0)
					            {
                                    SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info707"), StaticLoc.Loc.Get("info709"), "GetTeamRequsetYes", "GetTeamRequsetNo");
					            }
					            else
					            {
						            InRoom.GetInRoomInstantiate().ReturnRequest(yuan.YuanPhoton.ReturnCode.No, mResponse.Parameters);
					            }
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnPVP1Invite:
                            {
                                SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info705"), StaticLoc.Loc.Get("info709"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.InviteGoPVE:
                            {
					            if(PlayerPrefs.GetInt ("RefusalTeam")==0)
					            {
                                    SetPlayerRequest(mResponse.Parameters, StaticLoc.Loc.Get("info708"), StaticLoc.Loc.Get("buttons096"), "GetTeamRequsetYes", "GetTeamRequsetNo");
					            }
					            else
					            {
						            InRoom.GetInRoomInstantiate().ReturnRequest(yuan.YuanPhoton.ReturnCode.No, mResponse.Parameters);
					            }
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnInviteGoPVE:
                            {
                                SetPlayerReturn(mResponse.Parameters, StaticLoc.Loc.Get("info705"), StaticLoc.Loc.Get("buttons096"));
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.TransactionRequest:
                            {
                                string playerID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.UserID];
                                string playerNickName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.UserNickName];
                                //warnings.warningAllEnterClose.Show("ÌáÊ¾", string.Format("[ffff00]{0}[-]ÇëÇó½»Ò×£¬ÊÇ·ñÔÊÐí£¿", playerNickName));
                                //warnings.warningAllEnterClose.btnEnter.target = btnGameManager.gameObject;
                                //warnings.warningAllEnterClose.btnEnter.functionName = "GetTeamRequsetYes";
                                //warnings.warningAllEnterClose.btnExit.target = btnGameManager.gameObject;
                                //warnings.warningAllEnterClose.btnExit.functionName = "GetTeamRequsetNo";
                                //btnGameManager.dicTeamParameter = mResponse.Parameters;
								TransactionParameters tp = new TransactionParameters();
								tp.playerID = playerID;
								tp.playerName = playerNickName;
                                tp.requstType = (yuan.YuanPhoton.RequstType)requstType;
								
						        if(PlayerPrefs.GetInt ("RefusalDeal")==0)
								{	
						            UICL.SendMessage("getPlayerParameters" , tp , SendMessageOptions.DontRequireReceiver);
								}
								else
								{
							        SongSendTransactionRequest(yuan.YuanPhoton.ReturnCode.No,yuan.YuanPhoton.RequstType.TransactionRequest ,playerID , "DarkSword2" , "PlayerInfo" );
								}
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.ReturnTransactionRequest:
                            {
                                string playerID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.UserID];
                                string playerNickName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.UserNickName];
                                short returnCode = (short)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.RetureRequestType];
                                switch (returnCode)
                                {
                                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                        {
//                                            warnings.warningAllEnter.Show("ÌáÊ¾", string.Format("[ffff00]{0}[-]Í¬ÒâÄúµÄ½»Ò×ÇëÇó", playerNickName));
                                        }
                                        break;
                                    case (short)yuan.YuanPhoton.ReturnCode.No:
                                        {
											UICL.SendMessage("beijujue" , playerNickName , SendMessageOptions.DontRequireReceiver);
//                                          warnings.warningAllEnter.Show("ÌáÊ¾", string.Format("[ffff00]{0}[-]¾Ü¾øÄúµÄ½»Ò×ÇëÇó", playerNickName));
                                        }
                                        break;
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.RequstType.TransactionOK:
                            {
//								Debug.LogWarning ("TransactionOK-----------------");
								invCL.SendMessage("ReInitItem",SendMessageOptions.DontRequireReceiver);
								tc.SendMessage("TweenClose" , SendMessageOptions.DontRequireReceiver);
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info517"));
							}
							break;
                    }
                }
                break;
			}			
		}
		
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

    public void SetInfo11(Zealm.OperationResponse mResponse)
	{
		try
		{
			switch((OpCode)mResponse.OperationCode)
			{
			case OpCode.GetLevelPack:
			{
				try
				{
					switch(mResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
						{
							string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
							string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
							RefershYT (strKey,strValue);
						// 提示领取成功
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1238"));
						AllLeveReward.All.BtnShowState();
						}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.HasID:
					{
						//提示已领取过该等级礼包
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1239"));
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.NoSlot:
					{
						// 提示没有足够的包裹位
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info819"));
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.Nothing:
					{
						//提示没有该等级的礼包
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1240"));
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.NoNum:
					{
						//提示当前等级没有达到
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1241"));
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.No:
					{
						//提示活动已关闭
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1242"));
					}
						break;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
					btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}
			}
				break;
			case OpCode.GetLevelPackInfo:
			{
				try
				{
					switch(mResponse.ReturnCode)
					{
					case (short)yuan.YuanPhoton.ReturnCode.Yes:
					{
						Dictionary<int, object> info =( (Dictionary<object, object>)mResponse.Parameters[(short)Zealm.ParameterType.ItemID]).DicObjTo<int,object>();
						AllLeveReward.All.ShowLeveItem(info);
						//TODO:姜然  int是等级，object是string，string类型的字典，强转以后用
//						<LevelNum>5</LevelNum>
//							<gold>10000</gold>
//								<blood>50</blood>
//								<MarrowIron>0</MarrowIron>
//								<MarrowGold>0</MarrowGold>
//								<soul>0</soul>
//								<Item>8815,08;</Item>
					}
						break;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
					btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}
			}
				break;
                case OpCode.DynamicActivity:
                    {
                        try
                        {
                            switch(mResponse.ReturnCode)
                            {
                                case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                    {
                                        Dictionary<object, object> tmpData = (Dictionary<object, object>)mResponse.Parameters[(short)Zealm.ParameterType.ActivityState];
                                        Dictionary<string, object> dicData = tmpData.DicObjTo<string, object>();

                                        Dictionary<string, object> dynamicActivity = new Dictionary<string, object>();
                                        foreach (KeyValuePair<string, object> item in dicData)
                                        {
                                            dynamicActivity.Add(item.Key, item.Value);
                                        }

                                        //取dynamicActivity用

                                        if (null != ActivityNotice.instance)
                                        {
                                            ActivityNotice.instance.GetActivityInfo(dynamicActivity);
                                        }

                                        if (null != DynamicActivity.instance)
                                        {
                                            DynamicActivity.instance.GetActivityInfo(dynamicActivity);
                                        }
                                    }
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError(ex.ToString());
                        }
                        finally
                        {
                            btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                        }
                    }
                        break;
				case OpCode.TempTeamDissolve://
				{
					switch (mResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
						{
							//TODO:小队解散
						}
						break;
					}
				}
				break;
				case OpCode.TempTeamPlayerChange://
				{
					switch (mResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
						{
							string headID = (string)mResponse.Parameters[(short)Zealm.ParameterType.TeamHeadID];
							string teamInfo = (string)mResponse.Parameters[(short)Zealm.ParameterType.TeamInfo];
							string[] teamPlayer=teamInfo.Split (';');

							List<Dictionary<string,string>> players=new List<Dictionary<string, string>>();
							foreach(string item in teamPlayer)
							{
							if(item!=""){
								string[] strInfo=item.Split(',');
								Dictionary<string,string> dicInfo=new Dictionary<string, string>();
								dicInfo.Add ("playerID",strInfo[0]);//ID
								dicInfo.Add ("playerProID",strInfo[1]);//职业
								dicInfo.Add ("playerName",strInfo[2]);//名称
								dicInfo.Add ("playerLevel",strInfo[3]);//等级
								players.Add (dicInfo);
								}
						}
//							btnGameManager.objTeam.SendMessage("ShowFriendPlayer",players,SendMessageOptions.DontRequireReceiver);
							FriendTeamPlayer.FTP.ShowFriendPlayer(players,headID);
							//players里是小队里所有玩家的信息，headID是队长的ID,可以循环这个list来取小队成员
					
						}
						break;
					}
				}
				break;
			case OpCode.TempTeamPlayerRemove:
			{
				try
				{
					switch (mResponse.ReturnCode)
					{
					case (short)yuan.YuanPhoton.ReturnCode.Yes:
					{
						UICL.SendMessage("TempTeamPlayerRemove", true, SendMessageOptions.DontRequireReceiver);
						//TODO:退队成功提示
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.No:
					{
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1144"));
						//TODO:当前不在临时小队提示
					}
						break;
					}
				}
				catch (System.Exception ex)
					{
						Debug.LogError(ex.ToString());
					}
					finally
					{
						btnGameManager.CloseLoading();
					}
				}
				break;
			case OpCode.EquipmentResolveAll:
			{
				try
				{
					switch (mResponse.ReturnCode)
					{
					case (short)yuan.YuanPhoton.ReturnCode.Yes:
					{
						UICL.SendMessage("ReturnEquipmentResolve", true, SendMessageOptions.DontRequireReceiver);
						
						int produceIron = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowIron];
						int produceGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowGold];
						
						//提示信息，拆分成功，生成XX精铁粉末,XX精金粉末！
						PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2},{3}{4}", StaticLoc.Loc.Get("info896"), produceIron, StaticLoc.Loc.Get("info897"), produceGold, StaticLoc.Loc.Get("info898")));
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.No:
					{
						//提示信息，装备拆分失败！
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info877"));
						UICL.SendMessage("ReturnEquipmentResolve", false, SendMessageOptions.DontRequireReceiver);
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.NoGold:
					{
						//提示信息，金币不够！
						PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info491"));
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
					{
						//提示信息，血石不够！
						//PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info490"));
						SwitchToStore();
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.Error:
					{
						Debug.LogError(mResponse.DebugMessage);
					}
						break;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
					btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}
			}
				break;
			case OpCode.ShowTraining:
			{
				try{
					int gold=(int)mResponse.Parameters[(short)1];//金币训练所扣除的钱数
					int yongshi=(int)mResponse.Parameters[(short)2];//爵士训练所扣除的实际值
					int vipLower=(int)mResponse.Parameters[(short)3];//vip低级训练所扣除的实际值
					int vip2High=(int)mResponse.Parameters[(short)4];//vip高级训练所扣除的实际值
					int yijian=(int)mResponse.Parameters[(short)5];//一键训练所扣除的实际值
					int yijian1=(int)mResponse.Parameters[(short)6];//一键训练所扣除的点数
					int vipBegin1=(int)mResponse.Parameters[(short)7];//vip需要开启的训练模式
					int vipBegin2=(int)mResponse.Parameters[(short)8];//vip需要开启的训练模式

                    if(null != TrainingViplevel.instance)
                    {
                        TrainingViplevel.instance.SetVipLabel(vipBegin1, vipBegin2);
                        TrainingViplevel.instance.SetBtnInfo(gold, yongshi, vipLower, vip2High);
                    }
				}catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
					btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}

			}
				break;
		
			case OpCode.DeductStrength:
			{
				try
				{ 
					switch(mResponse.ReturnCode)
					{
					case (short)yuan.YuanPhoton.ReturnCode.Yes://添加扣除体力成功
					{
						string[] strKey = (string[])mResponse.Parameters[(short)6];
						string[] strValue= (string[])mResponse.Parameters[(short)7];
						int tili=(int)mResponse.Parameters[(short)100];
						if(tili==null){
							tili=0;
						}

						yuan.YuanPhoton.CostPowerType useMoneyType=(yuan.YuanPhoton.CostPowerType)(short)mResponse.Parameters[(short)111];
						if((int)useMoneyType==10&&tili!=0){
							PanelStatic.StaticWarnings.warningAllTime.Show("",StaticLoc.Loc.Get("info1174")+(System.Math.Abs(tili)).ToString()+StaticLoc.Loc.Get("meg0119"));
						}else{
						if(tili>0){
								EquipEnhance.instance.ShowMyItem("",StaticLoc.Loc.Get("info1219")+(System.Math.Abs(tili)).ToString()+StaticLoc.Loc.Get("meg0119"));
						}else if(tili<0){
								PanelStatic.StaticWarnings.warningAllTime.Show("",StaticLoc.Loc.Get("info1220")+(System.Math.Abs(tili)).ToString()+StaticLoc.Loc.Get("meg0119"));
						}
						}
						RefershYT (strKey,strValue);


						UICL.SendMessage("ReturnCostPower" , useMoneyType , SendMessageOptions.DontRequireReceiver);

					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.No://添加扣除体力失败
					{
						int tili=(int)mResponse.Parameters[(short)100];
						yuan.YuanPhoton.CostPowerType useMoneyType=(yuan.YuanPhoton.CostPowerType)(short)mResponse.Parameters[(short)111];
						UICL.SendMessage("ReturnTipsNoPower" , SendMessageOptions.DontRequireReceiver);
					}
						break;


					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
					if(!Application.loadedLevelName.Equals("Login-1") && !Application.loadedLevelName.Equals("Login-2"))
						btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}
			}
				break;
			
			case OpCode.buyIron:
                {
				try
                { 
				    switch(mResponse.ReturnCode)
				    {
				    case (short)yuan.YuanPhoton.ReturnCode.Yes://购买精铁成功
				    {
						string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
						string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
						RefershYT (strKey,strValue);
						UICL.SendMessage("ReturnEquepmentBuildRefresh", true, SendMessageOptions.DontRequireReceiver);

						//精铁的总数用于刷新客户端yt
                        int bloodStone = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];
					    int irons = (int)mResponse.Parameters[(short)113];

						EquipEnhance.instance.ShowMyItem("", string.Format("{0}{1}{2}{3}{4}", StaticLoc.Loc.Get("messages052"), bloodStone, StaticLoc.Loc.Get("meg0168"), irons, StaticLoc.Loc.Get("info897")));// 花费XX血石，获得了XX精铁
				    }
					    break;
				    case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
				    {
					    //血石不够；
                        SwitchToStore();
				    }
					    break;
				    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
                finally
                {
                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                }
			}
				break;
			case OpCode.buyGold:
                {
				try
                {
				    switch(mResponse.ReturnCode)
				    {
				    case (short)yuan.YuanPhoton.ReturnCode.Yes://购买精金成功
				    {
						string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
						string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
						RefershYT (strKey,strValue);
					    // 精金的总数用于刷新客户端yt
						UICL.SendMessage("ReturnEquepmentBuildRefresh", true, SendMessageOptions.DontRequireReceiver);

                        int bloodStone = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];
					    int golds = (int)mResponse.Parameters[(short)114];

						EquipEnhance.instance.ShowMyItem("", string.Format("{0}{1}{2}{3}{4}", StaticLoc.Loc.Get("messages052"), bloodStone, StaticLoc.Loc.Get("meg0168"), golds, StaticLoc.Loc.Get("info898")));// 花费XX血石，获得了XX精金
				    }
					    break;
				    case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
				    {
					    //血石不够；
                        SwitchToStore();
				    }
					    break;
				    }
				}
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
                finally
                {
                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                }
			}
				break;
			case OpCode.HuntingMap:
			{
				try
				{
					switch(mResponse.ReturnCode)
					{
					case (short)yuan.YuanPhoton.ReturnCode.Yes://进入狩猎模式成功
					{
						string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
						string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
						RefershYT (strKey,strValue);
						int bloodStone = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];//花费的血石
						string mapid = (string)mResponse.Parameters[(short)5];//副本id
						int level = (int)mResponse.Parameters[(short)6];//副本难度
						int moshi = (int)mResponse.Parameters[(short)7] ; //副本Type
						//PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2}{3}{4}", StaticLoc.Loc.Get("messages052"), bloodStone, StaticLoc.Loc.Get("meg0168"), StaticLoc.Loc.Get("info898")));// 花费XX血石，获得了XX精金
						object[] objs = new object[3];
						objs[0] = mapid;
						objs[1] = level;
						objs[2] = moshi; 
						UICL.SendMessage("ClickRightMapGo" ,objs, SendMessageOptions.DontRequireReceiver);
						//狩猎模式血石消耗统计
						//TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo037"),"1", Mathf.Abs(bloodStone)));
					}
						break;
					case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
					{
						//血石不够；
						SwitchToStore();
					}
						break;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
					btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}
			}
				break;
			case OpCode.EquipmentSellAll:
			{
				try
				{
					switch (mResponse.ReturnCode)
					{
					case (short)yuan.YuanPhoton.ReturnCode.Yes:
					{
						string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
							string[] strValue = (string[])mResponse.Parameters[(short)ParameterType.TableSql];
							RefershYT(strKey, strValue);

							int gold = (int)mResponse.Parameters[(short)Zealm.ParameterType.Gold];
							SetMoneyMessage(gold, 0, 0, 0);

							int[] mMoney = new int[2];
							mMoney[0] = gold;
							mMoney[1] = 0;
							UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
							UICL.SendMessage("ReturnSellAll" , SendMessageOptions.DontRequireReceiver);

						}
						break;
						case (short)yuan.YuanPhoton.ReturnCode.Error:
						{
							Debug.LogError(mResponse.DebugMessage);
						}
						break;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
					btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}
			}
				break;
                case OpCode.NumberRechargeDay:
                    {
                        try
                        {
                            switch (mResponse.ReturnCode)
                            {
                                
                                case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)ParameterType.TableSql];
                                    RefershYT(strKey, strValue);

                                    if (null != ContinuTopUp.continuTopUp)
                                    {
                                        ContinuTopUp.continuTopUp.OnEnable();
                                    }

                                    warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1074"));
                                }
                                break;
                                case (short)yuan.YuanPhoton.ReturnCode.No:
                                    {
                                        warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips049"));
                                    }
                                    break;
                                case (short)yuan.YuanPhoton.ReturnCode.Error:
                                    {
                                        Debug.LogError(mResponse.DebugMessage);
                                    }
                                break;
                                case (short)yuan.YuanPhoton.ReturnCode.HasID:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)ParameterType.TableSql];
                                    RefershYT(strKey, strValue);

                                    if (null != ContinuTopUp.continuTopUp)
                                    {
                                        ContinuTopUp.continuTopUp.OnEnable();
                                    }
                                }
                                break;
                            }
                                
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogWarning(ex.ToString());
                        }
                        finally
                        {
                            btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                        }
                    }
                    break;
				case OpCode.GrowthWelfareInfo:
				{
					switch (mResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
						{
							PanelStatic.StaticBtnGameManager.CloseLoading();
							int servingBlood = (int)mResponse.Parameters[(short)0];//自己所充值的总数
							//Dictionary<int,int> dicInfo=((Dictionary<object,object>)mResponse.Parameters[(short)1]).DicObjTo<int,int>();//配置的公共参数（Key是等级,Value为血石数量）
							int GrowthWelfareLevel = (int)mResponse.Parameters[(short)2];//基础等级
							int GrowthWelfareBlood = (int)mResponse.Parameters[(short)3];//基础血石
							RewardWelfare.rewardWelfare.ShowWelfare(servingBlood,GrowthWelfareLevel,servingBlood);
						}
						break;
					}
				}
				break;
				case OpCode.GetGrowthWelfare:
				{
					switch (mResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
						{
							PanelStatic.StaticBtnGameManager.CloseLoading();
							int getBlood = (int)mResponse.Parameters[(short)ParameterType.ItemID];//获得的血石数
							string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
							string[] strValue = (string[])mResponse.Parameters[(short)ParameterType.TableSql];
							RefershYT(strKey, strValue);
							SetMoneyMessage (0,getBlood);
						}
						break;
						case (short)yuan.YuanPhoton.ReturnCode.HasID:
						{
						PanelStatic.StaticBtnGameManager.CloseLoading();
							//已领过奖励
						warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1116"));
						}
						break;
						case (short)yuan.YuanPhoton.ReturnCode.No:
						{
						PanelStatic.StaticBtnGameManager.CloseLoading();
							//还没达到活动资格
						warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1117"));
						}
						break;
						case (short)yuan.YuanPhoton.ReturnCode.Nothing:
						{
						PanelStatic.StaticBtnGameManager.CloseLoading();
							//还没达到活动等级
						warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1118"));
						}
						break;
						}
				}
				break;

                case OpCode.PVPInviteIsNo:
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.IsMine:
                                {
                                    warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0109"));

                                    if (null != PanelPVPBattlefield.pvpBattlefield && PanelPVPBattlefield.pvpBattlefield.isSingleMatching)
                                    {
                                        PanelPVPBattlefield.pvpBattlefield.SetSingleBtnState(MatchBtnState.SingleMatch);
                                        captainID = "";
                                        teamMembersID.Clear();
                                    }

                                    if (null != PanelPVPBattlefield.pvpBattlefield)
                                    {
                                        PanelPVPBattlefield.pvpBattlefield.isReady = false;
                                        captainID = "";
                                        teamMembersID.Clear();
                                        PanelPVPBattlefield.pvpBattlefield.OnDisable();
                                        PanelPVPBattlefield.pvpBattlefield.OnEnable();
                                    }
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:
                                {
                                    UICL.SendMessage("closeNewWanChengPeiDui" , SendMessageOptions.DontRequireReceiver);
                                    warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0108"));

                                    if (null != PanelPVPBattlefield.pvpBattlefield && PanelPVPBattlefield.pvpBattlefield.isSingleMatching)
                                    {
                                        PanelPVPBattlefield.pvpBattlefield.SetSingleBtnState(MatchBtnState.SingleMatch);
                                        captainID = "";
                                        teamMembersID.Clear();
                                    }

                                    if (null != PanelPVPBattlefield.pvpBattlefield)
                                    {
                                        PanelPVPBattlefield.pvpBattlefield.isReady = false;
                                        captainID = "";
                                        teamMembersID.Clear();
                                        PanelPVPBattlefield.pvpBattlefield.OnDisable();
                                        PanelPVPBattlefield.pvpBattlefield.OnEnable();
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case OpCode.PVPisFall:
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)ParameterType.TableSql];
                                    RefershYT(strKey, strValue);
                                    int pvpStone = (int)mResponse.Parameters[(short)ParameterType.ItemID];

									UICL.SendMessage("ReturnPVPisFall" , pvpStone , SendMessageOptions.DontRequireReceiver);
                                }
                                break;
                        }
                    }
                    break;
                case OpCode.IsTeamHead:
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                { 
                                    //在这做处理即可
									UICL.SendMessage("TeamHeadNextLoad" , SendMessageOptions.DontRequireReceiver);
                                }
                                break;
                        }
                    }
                    break;
                case OpCode.GuildDismiss:
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
                                    RefershYT(strKey, strValue);
                                  //  InRoom.GetInRoomInstantiate().GetGuildAll();
                                    warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0087"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.IsMine:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
                                    RefershYT(strKey, strValue);
                                  //  InRoom.GetInRoomInstantiate().GetGuildAll();
                                    warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0085"));
                                }
                                break;

                        }
                    }
                    break;
                case OpCode.GuildHeadMiss:
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
                                    RefershYT(strKey, strValue);
                                    warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0086"));
                                }
                                break;
                        }
                    }
                    break;
			case OpCode.GetClientParms:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                 BtnGameManager.numSeviceMianCity= (float)mResponse.Parameters[(short)yuan.YuanPhoton.ClientParmsType.ServiceMianCtiy];
								BtnGameManager.numSeviceDuplicate= (float)mResponse.Parameters[(short)yuan.YuanPhoton.ClientParmsType.ServiceDuplicate];
                                BtnGameManager.numSevicePVP = (float)mResponse.Parameters[(short)yuan.YuanPhoton.ClientParmsType.ServicePVP];
                                Dictionary<string, int> ClientParms = ((Dictionary<object, object>)mResponse.Parameters[(short)yuan.YuanPhoton.ClientParmsType.ClientParms]).DicObjTo<string, int>();
					
								foreach(KeyValuePair<string,int> item in ClientParms)
								{
									BtnGameManager.dicClientParms[item.Key]=item.Value;
								}
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
			case OpCode.GetFirstPacks:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string itemID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								UICL.SendMessage("CategoryTipsAsID" , itemID , SendMessageOptions.DontRequireReceiver);       
                            }
                            break;
					
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;				
			case OpCode.UseItem:
                {
					try
					{
	                    switch (mResponse.ReturnCode)
	                    {
	                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                            {

                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
									RefershYT (strKey,strValue);
                                    string itemID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
									if(itemID.Substring(0,3) == "884"){
										int power = (int)mResponse.Parameters[(short)111];
										object[] objs = new object[2];
										objs[0] = itemID;
										objs[1] = power;
										invCL.SendMessage("returnPowerSolution" , objs , SendMessageOptions.DontRequireReceiver);
									}else{
										invCL.SendMessage("returnUseItemAsID" , itemID , SendMessageOptions.DontRequireReceiver);
									}
	                            }
	                            break;
	                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
	                            {
									warnings.warningAllEnter.Show(StaticLoc.Loc.Get("buttons620"), StaticLoc.Loc.Get("info770"));
	                            }
	                            break;	
								case (short)yuan.YuanPhoton.ReturnCode.NoSlot:
								{
									//TODO:姜然,提示vip等级异常
								warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1207"));
									
								}
									break;
								case (short)yuan.YuanPhoton.ReturnCode.NoNum:
								{
									//TODO:姜然,提示体力药次数已达到上限
								warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1208"));

								}
						break;
	                        case (short)yuan.YuanPhoton.ReturnCode.Error:
	                            {
	                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
	                            }
	                            break;
	                    }
					}
					catch(System.Exception ex)
					{
						Debug.LogError (ex.ToString ());
					}
					finally
					{
						btnGameManager.CloseLoading();
					}
                }
                break;
			case OpCode.TaskCompleted:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
								RefershYT (strKey,strValue);
                                string itemID = (string)mResponse.Parameters[(short)Zealm.ParameterType.ItemID];
					            if(UICL)
								UICL.SendMessage("returnTaskComplet" , itemID , SendMessageOptions.DontRequireReceiver);

                                int mGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.Gold];
                                int mBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.BloodStone];
								int[] mMoney = new int[2];
								mMoney[0] = mGold;
								mMoney[1] = mBlood;
								UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
					            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("buttons620"), StaticLoc.Loc.Get("info771"));
                            }
                            break;					
				        case (short)yuan.YuanPhoton.ReturnCode.Error:
				            {
					            Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
				            }
					        break;
				        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
				            {
					            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info819"));
//					            Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
				            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                string itemID = (string)mResponse.Parameters[(short)Zealm.ParameterType.ItemID];
                                if (UICL)
                                    UICL.SendMessage("returnTaskComplet", itemID, SendMessageOptions.DontRequireReceiver);
                     //           Debug.Log("重复完成任务===========================================================");
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.GetServer:
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
                                RefershYT(strKey, strValue);
                                string itemID = (string)mResponse.Parameters[(short)Zealm.ParameterType.ItemID];
                                if (UICL)
                                    UICL.SendMessage("returnActivitiesTaskComplet", itemID, SendMessageOptions.DontRequireReceiver);
                            }
                            break;
				    }
					btnGameManager.CloseLoading ();
                }
                break;
			case OpCode.GETRANKMONEY:{//充值第一名的详细信息
				switch(mResponse.ReturnCode){
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{			

					//first 是服务器传过来的拼接参数玩家姓名+“，”+等级+“，”+公会姓名+“，”+职业+”,“+称号
					string first = (string)mResponse.Parameters[(short)10];

                    if(ShowRichStatueInfo.instance)
                    {
                        ShowRichStatueInfo.instance.ShowRoleInfo(first);
                    }
				}
					break;
				}
			}
				break;
			case OpCode.GETRANKForceValue:{//战力值第一名的详细信息
				switch(mResponse.ReturnCode){
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					//first 是服务器传过来的拼接参数玩家姓名+“，”+等级+“，”+公会姓名+“，”+职业+”,“+称号
					string first = (string)mResponse.Parameters[(short)4];

                    if (ShowPKStatueInfo.instance)
                    {
                        ShowPKStatueInfo.instance.ShowRoleInfo(first);
                    }
				}
					break;
				}
			}
				break;
			case OpCode.GETRANKPVP:{//决斗场第一名的详细信息
				switch(mResponse.ReturnCode){
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					//first 是服务器传过来的拼接参数玩家姓名+“，”+等级+“，”+公会姓名+“，”+职业+”,“+称号
					string first = (string)mResponse.Parameters[(short)3];

                    if (ShowPVPStatueInfo.instance)
                    {
                        ShowPVPStatueInfo.instance.ShowRoleInfo(first);
                    }
				}
					break;
				}
			}
				break;
			case OpCode.correctLanwei:{
				switch(mResponse.ReturnCode){
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					int money=(int)mResponse.Parameters[(short)300];

					//购买栏位成功的提示
					warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1100"));
					int myRandom = Random.Range(10000,99999);
					//购买栏位仅仅统计成功
					string orderID = System.DateTime.Now.Ticks.ToString()+myRandom.ToString();
					//TD_info.setPayRequest(string.Format("{0};{1};{2};{3}", orderID,	money, "0",TableRead.strPageName));
					//TD_info.paySuccess(orderID);
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					//购买栏位失败的提示
					warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1101"));
				}
					break;
				}

			}
				break;
			case OpCode.payCard:
            {
                try
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                // string activeTime = (string)mResponse.Parameters[(short)Zealm.ParameterType.Time];
                                string remainTimes = (string)mResponse.Parameters[(short)36];
								string  ispaycard=(string)mResponse.Parameters[(short)37];
								BtnGameManager.yt[0]["isReceiveCardreward"].YuanColumnText = remainTimes;
								BtnGameManager.yt[0]["isPaycard"].YuanColumnText = ispaycard;//修改yt的值，是否购买原石；

                                // 提示购买成功
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0122"));

                                // 把购买月卡按钮状态置为领取福利
                                if (null != MonthCard.monthCard)
                                {
                                    MonthCard.monthCard.SetBtnState(CardBtnState.CanReward);// 显示领取福利
                                    MonthCard.monthCard.SetLabelTex(true, null);// 显示激活
                                    MonthCard.monthCard.SetTimesTxt(remainTimes); // 显示次数
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
								string remainTimes = (string)mResponse.Parameters[(short)36];
                                // 提示：生产次数已用完，请重新购买！
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0123"));

                                // TODO: 把领取福利按钮置为购买月卡
                                if (null != MonthCard.monthCard)
                                {
									MonthCard.monthCard.SetTimesTxt(remainTimes); // 显示次数
                                    MonthCard.monthCard.SetBtnState(CardBtnState.BuyCard);// 显示购买原石
                                    MonthCard.monthCard.SetLabelTex(false, null); // 显示未激活
                                }

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.isReceiveCardreward:
                            {
								

                                string activeTime = (string)mResponse.Parameters[(short)Zealm.ParameterType.Time];
                                string remainTimes = (string)mResponse.Parameters[(short)36];

                                BtnGameManager.yt[0]["PaycardTime"].YuanColumnText = activeTime;
                                BtnGameManager.yt[0]["isReceiveCardreward"].YuanColumnText = remainTimes;
                                // 提示：已炼制成功！
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0124"));

                                // 按钮状态变为不可点状态，用户每次上线检测isReceiveCardreward字段是否为2，若为2则可以生产，若为1则正在cd
                                if (null != MonthCard.monthCard)
                                {
                                    MonthCard.monthCard.SetBtnState(CardBtnState.CantReward);// CD
                                    MonthCard.monthCard.SetLabelTex(true, activeTime); // 显示CD时间
                                    MonthCard.monthCard.SetTimesTxt(remainTimes); // 显示次数
                                }

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:// CD时间未到
                            {
                                string activeTime = (string)mResponse.Parameters[(short)Zealm.ParameterType.Time];
                                string remainTimes = (string)mResponse.Parameters[(short)36];

                                BtnGameManager.yt[0]["PaycardTime"].YuanColumnText = activeTime;
                                BtnGameManager.yt[0]["isReceiveCardreward"].YuanColumnText = remainTimes;
                                // 提示：炼制冷却时间未到！
                                warnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0131"));

                                // 按钮状态变为不可点状态，用户每次上线检测isReceiveCardreward字段是否为2，若为2则可以生产，若为1则正在cd
                                if (null != MonthCard.monthCard)
                                {
                                    MonthCard.monthCard.SetBtnState(CardBtnState.CantReward);// CD
                                    MonthCard.monthCard.SetLabelTex(true, activeTime); // 显示CD时间
                                    MonthCard.monthCard.SetTimesTxt(remainTimes); // 显示次数
                                }

                            }
                            break;
					case (short)100:
						int xueshi=(int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];
						string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
						string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
						RefershYT (strKey,strValue);
						warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1103")+xueshi.ToString());
						
						break;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
                finally
                {
                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                }
			}
				break;
			case OpCode.Payinformation:{
				switch(mResponse.ReturnCode){
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{

					string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
					string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
					int xueshi=(int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.BloodStone];
					string money= (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.MoneyType]; // use for TD
					RefershYT (strKey,strValue);
					//购买血石成功的提示添加多少血石提供给玩家 
					warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1103")+xueshi.ToString());

					int myRandom = Random.Range(10000,99999);
					//TD 统计充值   仅仅统计充值成功
					string orderID = System.DateTime.Now.Ticks.ToString()+myRandom.ToString();
					//TD_info.setPayRequest(string.Format("{0};{1};{2};{3}", orderID, money, xueshi,TableRead.strPageName));
					//TD_info.paySuccess(orderID);
	
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					//购买血石失败的提示
					warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1102"));
				}
					break;
				}
				
			}
				break;
			case OpCode.PayGameRole:
			{
		//		Debug.Log("显示价格-----------------------------------");
				Dictionary<string, string> dicGetResult1 ;
				Dictionary<string, string> dicGetResult2 ;
				Dictionary<string, string> dicGetResult3 ;
				Dictionary<string, string> dicGetResult4 ;
//				Dictionary<string, string> dicGetResult5 = new Dictionary<string, string>();
				Dictionary<string, object> dicTempInfos = null;
				//从服务器传过来五种计费方式，是以五个字典的形式，前四种方式是在游戏商城中，第五种计费方式是购买栏位；
				//字典中所有字段：“propNum”，“propTag”，“propPrice”，“propName”；
				Dictionary<object,object> a1=(Dictionary<object, object>)mResponse.Parameters[(short)yuan.YuanPhoton.PayinfoParamsBack.propOne];
				Dictionary<object,object> a2=(Dictionary<object, object>)mResponse.Parameters[(short)yuan.YuanPhoton.PayinfoParamsBack.propTwo];
				Dictionary<object,object> a3=(Dictionary<object, object>)mResponse.Parameters[(short)yuan.YuanPhoton.PayinfoParamsBack.propThree];
				Dictionary<object,object> a4=(Dictionary<object, object>)mResponse.Parameters[(short)yuan.YuanPhoton.PayinfoParamsBack.propFour];
				string a5= (string)mResponse.Parameters[(short)5]; // 基础兑换比为1:10
				string a6= (string)mResponse.Parameters[(short)6]; // 额外赠送50血石\n首购可升级为VIP1
                string a7 = (string)mResponse.Parameters[(short)7]; // 额外赠送800血石\n首购可升级为VIP3
                string a8 = (string)mResponse.Parameters[(short)8]; // 额外赠送1800血石\n首购可升级为VIP4
				string a9= (string)mResponse.Parameters[(short)9]; // 当日即可炼制600血石每隔3日再炼制300血石，一共可炼制10次!
				string a10= (string)mResponse.Parameters[(short)10]; // 首次600血石+300血石*10次  一次购买！共可获得
				string a11= (string)mResponse.Parameters[(short)11]; // 3600

                if (SetRechargePanelInfo.instance)
                {
                    SetRechargePanelInfo.instance.SetInfo(a5, a6, a7, a8, a9, a10, a11);
                }

				dicGetResult1 = a1.DicObjTo<string,string>();
		//		Debug.Log("显示价格1-----------------------------------");
				dicGetResult2 = a2.DicObjTo<string,string>();
				dicGetResult3 = a3.DicObjTo<string,string>();
				dicGetResult4 = a4.DicObjTo<string,string>();
//				dicGetResult5 = (Dictionary<string, string>)mResponse.Parameters[(byte)yuan.YuanPhoton.PayinfoParamsBack.propFive];

				string TAG1=dicGetResult1["propNum"];
				string TAG2=dicGetResult2["propNum"];
				string TAG3=dicGetResult3["propNum"];
				string TAG4=dicGetResult4["propNum"];
//				string TAG5=dicGetResult5["propNum"];

				object[] objs = new object[4];
				objs[0] = dicGetResult1;
				objs[1] = dicGetResult2;
				objs[2] = dicGetResult3;
				objs[3] = dicGetResult4;

				UICL.SendMessage("ReturnPayGameRole" , objs , SendMessageOptions.DontRequireReceiver);

				//Debug.Log(TAG1+","+TAG2+","+TAG3+","+TAG4);
				//Debug.Log(dicGetResult1["propName"]+","+dicGetResult2["propName"]+","+dicGetResult3["propName"]+","+dicGetResult4["propName"]);
				btnGameManager.CloseLoading ();
			}
				break;

			case OpCode.PaycardBuy:
			{
				if (mResponse.ReturnCode == (short)yuan.YuanPhoton.ReturnCode.No)
				{
					warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1013"));
					//PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1013"));
					return;
				}
				Dictionary<string, string> dicGetResult6  = new Dictionary<string, string>();
				Dictionary<object,object> a6=(Dictionary<object, object>)mResponse.Parameters[(short)yuan.YuanPhoton.PayinfoParamsBack.propSix];
				dicGetResult6 = a6.DicObjTo<string,string>();
				Dictionary<string, object> dicTempInfos = null;
				Debug.Log(dicGetResult6["propName"]+","+dicGetResult6["propNum"]+","+dicGetResult6["propTag"]+","+dicGetResult6["propPrice"]);
				int myRandom = Random.Range(10000,99999);
#if UNITY_ANDROID
#if SDK_UC
				string peerId = "1";
				SDKManager.zzsdk_pay(string.Format("{0},{1},{2};{3};{4};{5};{6};{7}",
			                                   PlayerPrefs.GetString("NumTitleS1", "Empty"),
			                                   "0",
			                                   peerId,
			                                   BtnManager.passID,
			                                   "0",
			                                   "1",
			                                   dicGetResult6["propTag"],
			                                   dicGetResult6["propPrice"]
			                                   ));
#else
				string peerId = "1";
				SDKManager.zzsdk_pay(string.Format("{0},{1},{2};{3};{4};{5};{6}",
				                                   PlayerPrefs.GetString("NumTitleS1", "Empty"),
				                                   "0",
				                                   peerId,
				                                   dicGetResult6["propPrice"],
				                                   dicGetResult6["propTag"],
				                                   dicGetResult6["propName"],
				                                   PlayerPrefs.GetString("lblServerNameS1", "Empty")
				                                   ));
#endif
#elif UNITY_IOS
#if SDK_AZ


#elif SDK_ZSYIOS
				StoreKitBinding.requestProductData("cszz.065");
				StoreKitBinding.purchaseProduct("cszz.065",1);	
#elif SDK_HM
				HMSdkControl.HMSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(), 
				                      dicGetResult6["propPrice"], 
				                      dicGetResult6["propNum"],
				                      string.Format("{0},{1},{2},{3}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				              BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				              dicGetResult6["propNum"]));	
#elif SDK_TONGBU
				TBSdkControl.TBSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      dicGetResult6["propPrice"],
				                      string.Format("{0},{1},{2},{3},{4}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				              BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				              dicGetResult6["propNum"],
				              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
						
#elif SDK_JYIOS
				// TODO: API：异步支付（订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串）
				SdkConector.NdUniPayAsyn( System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                         dicGetResult6["propNum"],
				                         dicGetResult6["propName"],
				                         dicGetResult6["propPrice"],
				                         "1",
				                         string.Format("{0},{1},{2},{3}", PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				              BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				              dicGetResult6["propNum"]));
#elif SDK_ITOOLS
				ItoolsSdkControl.ItoolSDKpay( dicGetResult6["propName"],
				                             dicGetResult6["propPrice"],
				                             string.Format("{0},{1},{2},{3},{4}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				              BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				              dicGetResult6["propNum"],
				              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
#elif SDK_KUAIYONG
				KYSdkControl.KYSDKpay( string.Format("{0},{1},{2},{3},{4}",	
				                                     PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                                     BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				                                     BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				                                     dicGetResult6["propNum"],
				                                     System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                      dicGetResult6["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                      dicGetResult6["propName"]);
#elif SDK_XY
				XYSDKControl.XYSDKpay(dicGetResult6["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                      string.Format("{0},{1},{2},{3},{4}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				              BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				              dicGetResult6["propNum"],
				              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
#elif SDK_I4
				ASSDKControl.ASSDKpay(dicGetResult6["propPrice"],
				                      System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      dicGetResult6["propName"],
				                      string.Format("{0},{1},{2},{3},{4}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				              BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				              dicGetResult6["propNum"],
				              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
#elif SDK_ZSY
				ZSYSDKControl.ZSYSDKpay(string.Format("{0},{1},{2},{3},{4}",	
				                                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                                      BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				                                      BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				                                      dicGetResult6["propNum"],
				                                      System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                        dicGetResult6["propPrice"],
				                        dicGetResult6["propName"]);
#elif SDK_PP
				PPSdkControl.PPSdkpay(dicGetResult6["propPrice"],
				                      System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      dicGetResult6["propName"],
				                      string.Format("{0},{1},{2},{3}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,
				              BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
				              dicGetResult6["propNum"]),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
#endif
#endif

			}

				break;
				
			case OpCode.payLanwei:
			{
				if (mResponse.ReturnCode == (short)yuan.YuanPhoton.ReturnCode.No)
                {
                    warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1013"));
                    //PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1013"));
                    return;
                }
				Dictionary<string, string> dicGetResult5  = new Dictionary<string, string>();
				Dictionary<object,object> a5=(Dictionary<object, object>)mResponse.Parameters[(short)yuan.YuanPhoton.PayinfoParamsBack.propFive];
				dicGetResult5 = a5.DicObjTo<string,string>();
				Dictionary<string, object> dicTempInfos = null;
				int myRandom = Random.Range(10000,99999);
				//从服务器传过来五种计费方式，是以五个字典的形式，前四种方式是在游戏商城中，第五种计费方式是购买栏位；
				//字典中所有字段：“propNum”，“propTag”，“propPrice”，“propName”；

//                dicGetResult5 = (Dictionary<string, string>)mResponse.Parameters[(short)yuan.YuanPhoton.PayinfoParamsBack.propFive];
//				Debug.Log(dicGetResult5["propName"]+","+dicGetResult5["propNum"]+","+dicGetResult5["propTag"]+","+dicGetResult5["propPrice"]);
//                string peerId = (string)mResponse.Parameters[(short)yuan.YuanPhoton.PayinfoParamsBack.peerID];
//                Debug.Log(string.Format("wei----------------propNum:{0},propTag:{1},propPrice:{2},propName:{3}", dicGetResult5["propNum"], dicGetResult5["propTag"], dicGetResult5["propPrice"], dicGetResult5["propName"]));
#if UNITY_ANDROID
#if SDK_UC
                string peerId = "1";
                SDKManager.zzsdk_pay(string.Format("{0},{1},{2};{3};{4};{5};{6};{7}",
                    PlayerPrefs.GetString("NumTitleS1", "Empty"),
                    "0",
                    peerId,
                    BtnManager.passID,
                    "0",
                    "1",
                    dicGetResult5["propTag"],
                    dicGetResult5["propPrice"]
                ));
#else
				string peerId = "1";
                SDKManager.zzsdk_pay(string.Format("{0},{1},{2};{3};{4};{5};{6}",
                    PlayerPrefs.GetString("NumTitleS1", "Empty"),
                    "0",
                    peerId,
                    dicGetResult5["propPrice"],
                    dicGetResult5["propTag"],
                    dicGetResult5["propName"],
                    PlayerPrefs.GetString("lblServerNameS1", "Empty")
                ));
#endif
#elif UNITY_IOS
#if SDK_AZ

#elif SDK_HM
				HMSdkControl.HMSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),  dicGetResult5["propPrice"], dicGetResult5["propNum"],
				                      string.Format("{0},{1},{2},{3}",	
									              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              						"0",
									              BtnManager.passID,
				              dicGetResult5["propNum"]));	
				#elif SDK_TONGBU
				TBSdkControl.TBSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      dicGetResult5["propPrice"],
				                      string.Format("{0},{1},{2},{3},{4}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              "0",
				              BtnManager.passID,
				              System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				              dicGetResult5["propNum"]));
#elif SDK_JYIOS
				// TODO: API：异步支付（订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串）
				SdkConector.NdUniPayAsyn( System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                        dicGetResult5["propNum"],
				                         dicGetResult5["propName"],
				                         dicGetResult5["propPrice"],
				                         "1",
				                         string.Format("{0},{1},{2},{3}", PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              				"0",
				              			BtnManager.passID,
				              dicGetResult5["propNum"]));
#elif SDK_ITOOLS
				ItoolsSdkControl.ItoolSDKpay(dicGetResult5["propName"],
				                             dicGetResult5["propPrice"],
				                             string.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				            				  "0",
								              BtnManager.passID,
				              dicGetResult5["propNum"],
				              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
#elif SDK_KUAIYONG
				KYSdkControl.KYSDKpay( string.Format("{0},{1},{2},{3},{4}",	
				                                     PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                                    "0",
				                                     BtnManager.passID,
				                                     dicGetResult5["propNum"],
				                                     System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                      dicGetResult5["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                      dicGetResult5["propName"]);
#elif SDK_XY
				XYSDKControl.XYSDKpay(dicGetResult5["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                      string.Format("{0},{1},{2},{3},{4}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              "0",
				              BtnManager.passID,
				              dicGetResult5["propNum"],
				              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
#elif SDK_I4
				ASSDKControl.ASSDKpay(dicGetResult5["propPrice"],
				                      System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      dicGetResult5["propName"],
				                      string.Format("{0},{1},{2},{3},{4}",	
						              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
						              "0",
						              BtnManager.passID,
				              dicGetResult5["propNum"],
						              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
#elif SDK_ZSY
				ZSYSDKControl.ZSYSDKpay(  string.Format("{0},{1},{2},{3},{4}",	
				                                        PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                                        "0",
				                                        BtnManager.passID,
				                                        dicGetResult5["propNum"],
				                                        System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                        dicGetResult5["propPrice"],
				                        dicGetResult5["propName"]);
#elif SDK_PP
				PPSdkControl.PPSdkpay(dicGetResult5["propPrice"],
				              System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      dicGetResult5["propName"],
				                      string.Format("{0},{1},{2},{3}",	
				              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				              "0",
				              BtnManager.passID,
				              dicGetResult5["propNum"]),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
#endif
#endif
			
					
			}
				break;

            case OpCode.AuctionCompany:
                {
                    AuctionCompanyType act = (AuctionCompanyType)mResponse.Parameters[(short)AuctionCompanyParams.AuctionCompanyType];
                    switch (act)
                    {
                        case AuctionCompanyType.FixedPriceAuction:
                            {
                                try
                                {
                                    switch (mResponse.ReturnCode)
                                    {
                                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                            {
                                                int fee = (int)mResponse.Parameters[(short)AuctionCompanyParams.FixedPrice]; // 扣除的管理费
                                                //提示信息，拍卖品上传成功，消耗XX金币！
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info854"), fee, StaticLoc.Loc.Get("info335")));
                                                AuctionCompany.auctionCompany.auctionSell.ClearItemBox();
                                                //Debug.Log("===============================拍卖品上传成功========================");

                                                int auctionCount = (int)mResponse.Parameters[(short)AuctionCompanyParams.AuctionCount];
                                                int usedAuctionCount = (int)mResponse.Parameters[(short)AuctionCompanyParams.UsedAuctionCount];

                                                AuctionCompany.auctionCompany.auctionSell.SetAuctionCount(usedAuctionCount, auctionCount);
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.No:
                                            {
                                                //提示信息，拍卖品上传失败
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info855"));
                                                //Debug.Log("===============================拍卖品上传失败========================");
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                                            {
                                                //提示信息，对不起，该物品不允许拍卖！
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info874"));
                                                // Debug.Log("===============================物品不允许拍卖========================");
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                                            {
                                                //提示信息，对不起，您的金币不够，无法拍卖!
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info864"));
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                                            {
                                                //提示信息，对不起，您的血石不够，无法拍卖!
                                                //PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info865"));
                                                SwitchToStore();
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                                            {
                                                //提示信息，对不起，拍卖数据异常，不允许拍卖!
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info939"));
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.NoSlot:
                                            {
                                                //提示信息，对不起，您的拍卖栏位已用完，需购买拍卖栏位，才能继续拍卖!
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info872"));
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                                            {
                                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                            }
                                            break;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex.ToString());
                                }
                                finally
                                {
                                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                                }

                            }
                            break;
                        case AuctionCompanyType.AuctionSearch:
                            {
                                try
                                {
                                    switch (mResponse.ReturnCode)
                                    {
                                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                            {
												Dictionary<short, object> auctionRows = ((Dictionary<object, object>)mResponse.Parameters[(short)AuctionCompanyParams.AuctionID]).DicObjTo<short, object>();

                                                AuctionCompany.auctionCompany.SetAuctionTable(auctionRows);
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.No:
                                            {
                                                AuctionCompany.auctionCompany.auctionBuy.EnableGrid(false);
                                                //提示信息，目前没有拍卖信息
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info857"));

                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                                            {
                                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                            }
                                            break;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex.ToString());
                                }
                                finally
                                {
                                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                                }
                            }
                            break;
                        case AuctionCompanyType.BuyAuctions:
                            {
                                try
                                {
                                    switch (mResponse.ReturnCode)
                                    {
                                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                            {
                                                //提示信息，拍卖品购买成功,消耗XX血石！
                                                int costBlood = (int)mResponse.Parameters[(short)AuctionCompanyParams.FixedPrice];
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info859"), costBlood, StaticLoc.Loc.Get("info297")));
                                                AuctionCompany.auctionCompany.auctionBuy.RefreshAuctionList();
												
												//统计拍卖行消耗血石
							//TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo030"),"1", Mathf.Abs(costBlood)));
																	
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.No:
                                            {
                                                //提示信息，拍卖已结束！
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info858"));
                                                AuctionCompany.auctionCompany.auctionBuy.RefreshAuctionList();
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                                            {
                                                //提示信息，没有足够血石
                                                //PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("tips060"));
                                                SwitchToStore();
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                                            {
                                                //提示信息，您的包裹已满，不能进行交易。
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("tips035"));
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                                            {
                                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                            }
                                            break;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex.ToString());
                                }
                                finally
                                {
                                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                                }
                            }
                            break;
                        case AuctionCompanyType.PlayerAuctionInfo:
                            {
                                try
                                {
                                    switch (mResponse.ReturnCode)
                                    {
                                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                            {
                                                Dictionary<short, object> auctionRows = ((Dictionary<object, object>)mResponse.Parameters[(short)AuctionCompanyParams.AuctionID]).DicObjTo<short, object>();

                                                AuctionCompany.auctionCompany.SetMyAuctionTable(auctionRows);
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.No:
                                            {
                                                AuctionCompany.auctionCompany.auctionMy.EnableGrid(false);
                                                //提示信息，目前没有拍卖信息
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info857"));
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                                            {
                                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                            }
                                            break;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex.ToString());
                                }
                                finally
                                {
                                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                                }
                            }
                            break;
                        case AuctionCompanyType.BuyAuctionSlot:
                            {
                                try
                                {
                                    switch (mResponse.ReturnCode)
                                    {
                                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                            {
                                                int auctionCount = (int)mResponse.Parameters[(short)AuctionCompanyParams.AuctionCount];
                                                int usedAuctionCount = (int)mResponse.Parameters[(short)AuctionCompanyParams.UsedAuctionCount];
                                                int costBlood = (int)mResponse.Parameters[(short)AuctionCompanyParams.FixedPrice];

                                                AuctionCompany.auctionCompany.auctionSell.SetAuctionCount(usedAuctionCount, auctionCount);
												//统计购买拍卖栏位血石消耗
							//TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo031"),"1", Mathf.Abs(costBlood)));
												if (costBlood > 0)
                                                {
                                                    //提示消耗的血石数量！
                                                    PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info919"), costBlood, StaticLoc.Loc.Get("info297")));
                                                }
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.No:
                                            {
                                                //提示信息，购买失败！
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info873"));
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                                            {
                                                //提示信息，对不起，拍卖栏位已达上限，不能购买！
                                                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info875"));
                                                // Debug.Log("===============================拍卖栏位已达上限，不能购买========================");
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                                            {
                                                //提示信息，没有足够血石
                                                //PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("tips060"));
                                                SwitchToStore();
                                            }
                                            break;
                                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                                            {
                                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                                            }
                                            break;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex.ToString());
                                }
                                finally
                                {
                                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                                }
                            }
                            break;
                    }
                }
                break;
            case OpCode.OnlineChests:
                {
                    //Debug.Log("wei----------OnlineChests-------------------------------------------------");
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string numOpenBox = (string)mResponse.Parameters[(short)yuan.YuanPhoton.OnlineChestsParams.OpenedChests]; // 已开宝箱数量
                                    int onlineChestsTime = (int)mResponse.Parameters[(short)yuan.YuanPhoton.OnlineChestsParams.OnlineChestsTime]; // 在线宝箱当天累计时间
                                    BtnGameManager.yt[0]["OpenedChests"].YuanColumnText = numOpenBox;
                                    BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText = onlineChestsTime.ToString();
                                    //Debug.Log(string.Format("wei----------OnlineChests-------------numOpenBox:{0},onlineChestsTime{1}", numOpenBox, onlineChestsTime));
                                    //OpenChest.my.ReceiveOpenBox(onlineChestsTime, numOpenBox);
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    //finally
                    //{
                    //    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    //}

                }
                break;
            case OpCode.EquipmentResolve:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    UICL.SendMessage("ReturnEquipmentResolve", true, SendMessageOptions.DontRequireReceiver);

                                    int produceIron = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowIron];
                                    int produceGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowGold];

                                    //提示信息，拆分成功，生成XX精铁粉末,XX精金粉末！
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2},{3}{4}", StaticLoc.Loc.Get("info896"), produceIron, StaticLoc.Loc.Get("info897"), produceGold, StaticLoc.Loc.Get("info898")));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:
                                {
                                    //提示信息，装备拆分失败！
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info877"));
                                    UICL.SendMessage("ReturnEquipmentResolve", false, SendMessageOptions.DontRequireReceiver);
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                                {
                                    //提示信息，金币不够！
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info491"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                                {
                                    //提示信息，血石不够！
                                    //PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info490"));
                                    SwitchToStore();
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(mResponse.DebugMessage);
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    finally
                    {
                        btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    }
                }
                break;
			case OpCode.Task:
				{
					AnlaysTask (mResponse);
				}
				break;
			case OpCode.Activity:
				{
					AnlaysActivity (mResponse);
				}
				break;				
			case OpCode.Firends:
				{
					AnlaysFirends (mResponse);
				}
				break;			
			case OpCode.UseMoney:
				{
					AnlaysUseMoney(mResponse);
				}
				break;
			case OpCode.ShowAllMONEY:{
				ShowAllMONEY(mResponse);	
							}
				break;
			case OpCode.ShowStrength:
			{
				ShowStrength(mResponse);
			}
				break;
            case OpCode.ZhuanZhi:
                {
                    try
                    {
                        int bloodstone = (int)mResponse.Parameters[(short)1];

                        if (ResetProfession.instance)
                        {
                            ResetProfession.instance.SetInfo(bloodstone);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    finally
                    {
                        btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    }
                }
                break;
			case OpCode.ChangeBottle:
				{
					AnlaysChangeBottle(mResponse);
				}
				break;
			case OpCode.BattlefieldReady:
                {
				Debug.Log(mResponse.ReturnCode);
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								
								UICL.SendMessage("NewWanChengPeiDui" , SendMessageOptions.DontRequireReceiver);
                                //to show ready ui

                                if (null != PanelPVPBattlefield.pvpBattlefield)
                                {
                                    PanelPVPBattlefield.pvpBattlefield.isReady = true;
                                    PanelPVPBattlefield.pvpBattlefield.singleMatchBtn.isEnabled = false;
                                    PanelPVPBattlefield.pvpBattlefield.teamMatchBtn.isEnabled = false;
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;

			case OpCode.BattlefieldScoreInfo:
			{
//				KDebug.Log("Sync Onuo boss HP BattlefieldScoreInfo：ReturnCode =  " + mResponse.ReturnCode);
//				Debug.Log(mResponse.ReturnCode);
				switch (mResponse.ReturnCode)
				{
					case (short)yuan.YuanPhoton.ReturnCode.Yes:
					{
						
	//					UICL.SendMessage("NewWanChengPeiDui" , SendMessageOptions.DontRequireReceiver);
                        int redScore = (int)mResponse.Parameters[(short)0];
                        int blueScore = (int)mResponse.Parameters[(short)1];
                        int redFlagCount = (int)mResponse.Parameters[(short)2];
                        int blueFlagCount = (int)mResponse.Parameters[(short)3];
						List<string> redFlagList = new List<string>();
						List<string> blueFlagList = new List<string>();
						for(int i = 4; i< redFlagCount + 4; i++)
						{
                            redFlagList.Add((string)mResponse.Parameters[(short)i]);
						}
						for(int i = 9; i< blueFlagCount + 9; i++)
						{
                            blueFlagList.Add((string)mResponse.Parameters[(short)i]);
						}
						
						object[] objs = new object[4];
						objs[0] = redScore;
						objs[1] = blueScore;
						objs[2] = redFlagList;
						objs[3] = blueFlagList;
						UICL.SendMessage("ReturnBattlefieldScoreInfo" , objs , SendMessageOptions.DontRequireReceiver);
					//to show ready ui
					}
					break;
					case (short)yuan.YuanPhoton.ReturnCode.Error:
					{
						Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
					}
					break;
				}
			}
				break;

			case OpCode.BattlefieldBossInfo:
                {
//					KDebug.Log(	"Sync Onuo boss HP BattlefieldBossInfo：ReturnCode =  " + mResponse.ReturnCode	);
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                int redBossHP = (int)mResponse.Parameters[(short)0];
                                int blueBossHP = (int)mResponse.Parameters[(short)1];
								int[] hps = new int[2];
								hps[0] = redBossHP;
								hps[1] = blueBossHP;
								UICL.SendMessage("ReturnPVPBossHP" , hps , SendMessageOptions.DontRequireReceiver);
								MonsterHP.monsterHP.ShowMyMonsterHp(redBossHP,200000);
								MonsterHP.monsterHP.ShowOtherMonsterHp(blueBossHP,200000);
                                //to show ready ui
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
			case OpCode.CanFinishActivity:
				{
					//todo try catch
					switch (mResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
						{
                            string activityID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ActivityCanFinish];

                            PanelActivity.panelActivity.AddActivityReward(activityID);
						}
							break;
						case (short)yuan.YuanPhoton.ReturnCode.Error:
						{
							Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
						}
							break;
						}
					}
					break;
			case OpCode.JoinActivity:
			{
				try{
					switch (mResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
						{
                            BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.CancelQueue);
						}
						break;
						case (short)yuan.YuanPhoton.ReturnCode.No:
						{
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(mResponse.DebugMessage));
						}
						break;
						case (short)yuan.YuanPhoton.ReturnCode.HasID:
                        {
                            string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                            string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
                            RefershYT(strKey, strValue);
                            BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.ContinueActivity);
                            taskActivityState = 1;
                        }
                        break;
                        case (short)yuan.YuanPhoton.ReturnCode.IsDone:// 任务活动可领取奖励
                        {
                            string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                            string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
                            RefershYT(strKey, strValue);
                            BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.GetReward);
                            taskActivityState = 2;
                        }
                        break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasRegister:// 任务活动奖励领取成功
                        {
                            string[] strKey = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                            string[] strValue = (string[])mResponse.Parameters[(short)Zealm.ParameterType.TableSql];
                            RefershYT(strKey, strValue);
                            BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.JoinActivity);
                            taskActivityState = 0;
                        }
                        break;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				finally
				{
                    if (null != PanelActivity.panelActivity) PanelActivity.panelActivity.EnableItemCollider(true);// 左边按钮变为可点状态
					btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
				}
				
			}
				break;
			case OpCode.ActivityJoinSuccess:
			{
				//todo try catch
				switch (mResponse.ReturnCode)
				{
					case (short)yuan.YuanPhoton.ReturnCode.Yes:
					{
                                int activityType = (int)mResponse.Parameters[(short)0];
								string mapID = "";
                                if(activityType == CommonDefine.ACTIVITY_TYPE_BOSS)
                                {
                                    mapID = (string)mResponse.Parameters[(short)1];
									UICL.SendMessage("ReturnActivityJoinSuccess" , mapID , SendMessageOptions.DontRequireReceiver);
                                }
                                else if(activityType == CommonDefine.ACTIVITY_TYPE_NORAML)
                                {
                                    mapID = (string)mResponse.Parameters[(short)1];
                                    int mapInstanceID = (int)mResponse.Parameters[(short)2];
									object[] objs = new object[2];
									objs[0] = mapID;
									objs[1] = mapInstanceID;
									UICL.SendMessage("ReturnActivityJoinSuccess" , objs , SendMessageOptions.DontRequireReceiver);
                                }
                                BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.ContinueActivity);

								
                                //todo enter map   
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case OpCode.FinishActivity:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string activityID = (string)mResponse.Parameters[(short)0];
                                    BtnEnterActivity.btnEnterActivity.RefreshActivity(activityID);
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0178"));// 提示：领取奖励成功！
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:
                                {
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0179"));// 提示：领取奖励失败！
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    finally
                    {
                        btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    }

                }
                break;
                case OpCode.ActivityBossHPMax:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                int bossMaxHp = (int)mResponse.Parameters[(short)1];
								UICL.SendMessage("SetActivityBossHPMax" , bossMaxHp , SendMessageOptions.DontRequireReceiver);
								ShowMonsterHP.showMonsterHP.ShowMaxHp(bossMaxHp);
						}
                            break;
                    }     
                }
                break;
			case OpCode.ActivityBoosHP:
                {
                    //todo try catch
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                List<Dictionary<string, string>> plist = new List<Dictionary<string, string>>();
                                int leftBossHp = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ActivityBossHP];
                                int myDamage = (int)mResponse.Parameters[(short)99];
                                int myRank = (int)mResponse.Parameters[(short)100];
                                int playerCount = (int)mResponse.Parameters[(short)101];
                                for (int i = 0; i < playerCount; i++)
                                {
                                    plist.Add(((Dictionary<object, object>)mResponse.Parameters[(short)i]).DicObjTo<string, string>());
									
								}
								
								BossRanking.BR.ShowBoosRank(plist,myRank,myDamage);
								UICL.SendMessage("ReturnActivityBoosHP" , leftBossHp , SendMessageOptions.DontRequireReceiver);
								ShowMonsterHP.showMonsterHP.ShowHaloHp(leftBossHp,0);
								
                                //todo update client info
                            }
                           break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;

			case OpCode.ActivityBossResult:
                {
                    //todo try catch
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                int playerCount = (int)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.PlayerCount];

                                Dictionary<string, string> playerInfo = new Dictionary<string, string>();
							    List<Dictionary<string, string>> plist = new List<Dictionary<string, string>>();

                                for (int i = 0; i < playerCount; i++)
                                {
                                    plist.Add(((Dictionary<object, object>)mResponse.Parameters[(short)i]).DicObjTo<string,string>());
                                }
                                int myDamge = (int)mResponse.Parameters[(short)99];
                                int myRank = (int)mResponse.Parameters[(short)100];
								UICL.SendMessage("ReturnActivityBossResult" , SendMessageOptions.DontRequireReceiver);
								UICL.SendMessage("ReturnActivityBoosHP" , -1 , SendMessageOptions.DontRequireReceiver);
								ShowMonsterHP.showMonsterHP.ShowHaloHp(0,0);
								WorldBoosControl.worldBoosControl.Show(plist,myRank.ToString(),myDamge.ToString());
                                //玩家信息1 :plist[0]["name"] -> playerName , plist[0]["damage"] ->playerDamage
                                //todo update ui
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
                
			case OpCode.BattlefieldResult:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string teamRedScore = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.RedTeamScore];
                                    string teamBlueScore = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.BlueTeamScore];

                                    string RedTeamFlag = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.RedTeamFlag];
                                    string BlueTeamFlag = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.BlueTeamFlag];
                                    string PlayerCount = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.PlayerCount];
                                    string teamLabel = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.winTeamLabel];

                                    Dictionary<string, string> playerInfo = new Dictionary<string, string>();
                                    List<Dictionary<string, string>> plist = new List<Dictionary<string, string>>();
                                    Dictionary<string, string>[] dics;
                                    dics = new Dictionary<string, string>[int.Parse(PlayerCount)];
                                    int i = 0;
                                    for (i = 0; i < int.Parse(PlayerCount); i++)
                                    {
                                        dics[i] = ((Dictionary<object, object>)mResponse.Parameters[(short)i]).DicObjTo<string,string>();            
                                        plist.Add(dics[i]);
                                    }

                                    Debug.Log(plist.Count);
                                    for (i = 0; i < plist.Count; i++)
                                    {
                                        Debug.Log(plist[i]["name"]);
                                    }
                                    object[] objs = new object[7];
                                    objs[0] = teamRedScore;
                                    objs[1] = teamBlueScore;
                                    objs[2] = RedTeamFlag;
                                    objs[3] = BlueTeamFlag;
                                    objs[4] = PlayerCount;
                                    objs[5] = dics;
                                    objs[6] = teamLabel;
                                    UICL.SendMessage("ReturnShowPVPWin", objs, SendMessageOptions.DontRequireReceiver);
                       //             Debug.Log("Winner is " + teamLabel + " !!!!");
                                }
                                break;
                        }

                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
		//		KDebug.Log(	"PVP PVP Onuo boss BattlefieldResult：ReturnCode =  " + mResponse.ReturnCode	);
                    
                }
                break;

			case OpCode.BattlefieldResultBoss:
                {
		//			KDebug.Log(	"PVP PVP Onuo boss BattlefieldResultBoss：ReturnCode =  " + mResponse.ReturnCode	);
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                     
                                string PlayerCount = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.PlayerCount];
                                string teamLabel = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.winTeamLabel];

                                Dictionary<string, string> playerInfo = new Dictionary<string, string>();
                                List<Dictionary<string, string>> plist = new List<Dictionary<string, string>>();
                                Dictionary<string, string>[] dics;
                                dics = new Dictionary<string, string>[int.Parse(PlayerCount)];
                                int i = 0;
                                for (i = 0; i < int.Parse(PlayerCount); i++)
                                {
                                    dics[i] = ((Dictionary<object, object>)mResponse.Parameters[(short)i]).DicObjTo<string, string>();
                                    plist.Add(dics[i]);
                                }

                                Debug.Log(plist.Count);
                                for (i = 0; i < plist.Count; i++)
                                {
                                    Debug.Log(plist[i]["name"]);
                                }
								object[] objs = new object[7];
								objs[0] = "0";
								objs[1] = "0";
								objs[2] = "0";
								objs[3] = "0";
								objs[4] = PlayerCount;
								objs[5] = dics;
								objs[6] = teamLabel;
								UICL.SendMessage("ReturnShowPVPWin", objs, SendMessageOptions.DontRequireReceiver);
								Debug.Log("Winner is " + teamLabel + " !!!!");
                            }
                            break;
                    }
                }
                break;

			case OpCode.BattlefieldInfo:
                {
//                    Debug.Log(mResponse.ReturnCode);
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                           {
							string teamRedScore = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.RedTeamScore];
                            string teamBlueScore = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.BlueTeamScore];

                            string RedTeamFlag = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.RedTeamFlag];
                            string BlueTeamFlag = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.BlueTeamFlag];
                            string PlayerCount = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.PlayerCount];
				
							Dictionary<string, string> playerInfo = new Dictionary<string, string>();
							List<Dictionary<string, string>> plist = new List<Dictionary<string, string>>();
					        Dictionary<string, string>[] dics;
					        dics = new Dictionary<string, string>[int.Parse(PlayerCount)];
							int i = 0;
							for (i = 0; i < int.Parse(PlayerCount); i++)
							{
                                dics[i] = ((Dictionary<object, object>)mResponse.Parameters[(short)i]).DicObjTo<string, string>();
                                plist.Add(dics[i]);
							}
			
//							Debug.Log(plist.Count);
//							for(i=0; i<plist.Count; i++){
//								Debug.Log(plist[i]["name"]);
//							}
							object[] objs = new object[6];
							objs[0] = teamRedScore ;
							objs[1] = teamBlueScore;
							objs[2] = RedTeamFlag;
							objs[3] = BlueTeamFlag;
							objs[4] = PlayerCount;
							objs[5] = dics;
							UICL.SendMessage("RetrunShowPVPInfo" , objs , SendMessageOptions.DontRequireReceiver);
//							playerInfo.Add("id", player.myID);
//							playerInfo.Add("name", player.myNickName);
//							playerInfo.Add("kill", player.BattlefieldKillCount.ToString());
//							playerInfo.Add("die", player.BattlefieldDieCount.ToString());
//							playerInfo.Add("teamLabel", player.myPVPTeam.BattlefieldLabel);
//							playerInfo.Add("score",player.PVPScore.ToString());
//							playerInfo.Add("pro",player.myTable[0]["ProID"].YuanColumnText);				
						}
						break;
						case (short)yuan.YuanPhoton.ReturnCode.Error:
					    {
						    Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
					    }
					    break;
                    }
                }
                break;

            case OpCode.BattlefieldTimeOut:
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                int battlefieldType = (int)mResponse.Parameters[(short)0];
                                //单人PVP
                                if (battlefieldType == 1)
                                {
                                    PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SetDuelType", 1, SendMessageOptions.DontRequireReceiver);
                                }
                                //活动PVP
                                else if (battlefieldType == 2)
                                {
                                    if (null != BtnEnterActivity.btnEnterActivity) BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.ActivityQueue);
                                }

								UICL.SendMessage("BattlefieldTimeOut" , SendMessageOptions.DontRequireReceiver);
						//		Debug.Log("BattlefieldTimeOut");

                                if (null != PanelPVPBattlefield.pvpBattlefield && PanelPVPBattlefield.pvpBattlefield.isSingleMatching)
                                {
                                    PanelPVPBattlefield.pvpBattlefield.SetSingleBtnState(MatchBtnState.SingleMatch);
                                    captainID = "";
                                    teamMembersID.Clear();
                                }

                                if (null != PanelPVPBattlefield.pvpBattlefield && PanelPVPBattlefield.pvpBattlefield.isTeamMatching)
                                {
                                    PanelPVPBattlefield.pvpBattlefield.SetTeamBtnState(MatchBtnState.CantMatch);
                                    captainID = "";
                                    teamMembersID.Clear();
                                }
                            }
                            break;
                    }
                }
            break;
            case OpCode.PVPCancel:
            {
                try
                {
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                int battlefieldType = (int)mResponse.Parameters[(short)0];

                                //单人PVP
                                if (battlefieldType == 1)
                                {
                                    PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SetDuelType", 1, SendMessageOptions.DontRequireReceiver);
                                }
                                //活动PVP
                                else if (battlefieldType == 2)
                                {
                                    if (null != BtnEnterActivity.btnEnterActivity) BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.ActivityQueue);
                                    // 提示：您已成功退出排队
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info469"));
                                }
                                if (null != PanelPVPBattlefield.pvpBattlefield)
                                {
                                    PanelPVPBattlefield.pvpBattlefield.SetSingleBtnState(MatchBtnState.SingleMatch);
                                    PanelPVPBattlefield.pvpBattlefield.SetTeamBtnState(MatchBtnState.CantMatch);
                                    captainID = "";
                                    teamMembersID.Clear();
                                }

                                battlefieldID = 0;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SetDuelType", 1, SendMessageOptions.DontRequireReceiver);
                                if (null != BtnEnterActivity.btnEnterActivity) 
                                    BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.ActivityQueue);
                                // 提示：您已成功退出排队
                              //  PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info469"));
                            }
                            break;

                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
                finally
                {
                    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                }
            }
            break;

            case OpCode.BattlefieldNotifyExit:
                {
                    Debug.Log(mResponse.ReturnCode);
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                string battlefieldLabel = (string)mResponse.Parameters[(short)0];
								UICL.SendMessage("BattlefieldNotifyExit" , battlefieldLabel , SendMessageOptions.DontRequireReceiver);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
//            case OpCode.DefenceBattleStartCommit:
//			{KDebug.Log("Received   Start  !!");
//				switch (mResponse.ReturnCode)
//                    {
//                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
//                            {
//                                long diffTime = (long)mResponse.Parameters[(short)0];
////					KDebug.Log("Received   Start  !!");
////								TMonsterControl	TMC	=	FindObjectOfType<TMonsterControl>();
////								TMC.gameObject.SendMessage("OnStart",diffTime);
//                                //to do ...
//                            }
//                            break;
//                        case (short)yuan.YuanPhoton.ReturnCode.Error:
//                            {
//                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
//                            }
//                            break;
//                    }
//                }
//                break;
//
//            case OpCode.BallCommitDamage:
//                {
//                    Debug.Log(mResponse.ReturnCode);
//                    switch (mResponse.ReturnCode)
//                    {
//                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
//                            {
//                                int damage = (int)mResponse.Parameters[(short)0];
//                            }
//                            break;
//                        case (short)yuan.YuanPhoton.ReturnCode.Error:
//                            {
//                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
//                            }
//                            break;
//                    }
//                }
//                break;

			case OpCode.BattlefieldInfoBoss:
                {
		//		KDebug.Log( "BattlefieldInfoBoss Get 欧诺城 boss BattlefieldInfoBoss： ReturnCode = " + mResponse.ReturnCode );
                    switch (mResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                int teamRedScore = (int)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.RedTeamBossHP];
                                int teamBlueScore = (int)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.BlueTeamBossHP];

                                string PlayerCount = (string)mResponse.Parameters[(short)yuan.YuanPhoton.BattlefiledType.PlayerCount];

                                Dictionary<string, string> playerInfo = new Dictionary<string, string>();
                                List<Dictionary<string, string>> plist = new List<Dictionary<string, string>>();
                                Dictionary<string, string>[] dics;
                                dics = new Dictionary<string, string>[int.Parse(PlayerCount)];
                                int i = 0;
                                for (i = 0; i < int.Parse(PlayerCount); i++)
                                {
                                    dics[i] = (Dictionary<string, string>)mResponse.Parameters[(short)i];
                                    plist.Add((Dictionary<string, string>)mResponse.Parameters[(short)i]);
                                }

                                Debug.Log(plist.Count);
                                for (i = 0; i < plist.Count; i++)
                                {
                                    Debug.Log(plist[i]["name"]);
                                }
//                                /*object[] objs = new object[6];
//                                objs[0] = teamRedScore;
//                                objs[1] = teamBlueScore;
//                                objs[2] = RedTeamFlag;
//                                objs[3] = BlueTeamFlag;
//                                objs[4] = PlayerCount;
//                                objs[5] = dics;
//                                UICL.SendMessage("RetrunShowPVPInfo", objs, SendMessageOptions.DontRequireReceiver);*/
//                                //							playerInfo.Add("id", player.myID);
//                                //							playerInfo.Add("name", player.myNickName);
//                                //							playerInfo.Add("kill", player.BattlefieldKillCount.ToString());
//                                //							playerInfo.Add("die", player.BattlefieldDieCount.ToString());
//                                //							playerInfo.Add("teamLabel", player.myPVPTeam.BattlefieldLabel);
//                                //							playerInfo.Add("score",player.PVPScore.ToString());
//                                //							playerInfo.Add("pro",player.myTable[0]["ProID"].YuanColumnText);				
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
                    }
                }
                break;
            case OpCode.TransactionRequest:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {

                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:
                                {
                                    //提示：玩家离线，交易取消！
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info965"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                                {
                                    //提示：玩家正在交易，交易请求取消！
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info966"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(mResponse.DebugMessage);
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    //finally
                    //{
                    //    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    //}
                }
                break;
            case OpCode.Gamble_Info:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    int cardBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.cardBlood];
                                    int lotteryBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.lotteryBlood];
                                    PanelGamble.panelGamble.ReturnBloodInfo(cardBlood, lotteryBlood);
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(mResponse.DebugMessage);
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    //finally
                    //{
                    //    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    //}
                }
                break;
            case OpCode.Gamble_Card:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    int costBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.BloodStone];
                                    int cardBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.cardBlood];
                                    int lotteryBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.lotteryBlood];
                                    string[] itemIds = (string[])mResponse.Parameters[(short)126];
                                    if (costBlood > 0)
                                    {
                                        // 提示消耗血石
                                        PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info1022"), costBlood, StaticLoc.Loc.Get("messages053")));

                                        //统计九宫格血石消耗
							            //TD_info.setUserPurchase(string.Format("{0};{1};{2}", StaticLoc.Loc.Get("tdinfo038"), "1", Mathf.Abs(costBlood)));
                                    }

                                    PanelGamble.panelGamble.RunGetItems(itemIds);
                                    PanelGamble.panelGamble.ReturnBloodInfo(cardBlood, lotteryBlood);
								}
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                                {
                                    // 血石不够，切换到商城充值
                                    SwitchToStore();
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(mResponse.DebugMessage);
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    finally
                    {
                        btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    }
                }
                break;
            case OpCode.Gamble_Lottery:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string itemID = (string)mResponse.Parameters[(short)Zealm.ParameterType.ItemID];
                                    int costBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.BloodStone];
                                    int cardBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.cardBlood];
                                    int lotteryBlood = (int)mResponse.Parameters[(short)Zealm.ParameterType.lotteryBlood];

                                    if(costBlood > 0)
                                    { 
                                        // 提示消耗血石
                                        PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info1021"), costBlood, StaticLoc.Loc.Get("messages053")));
                                    
									    //统计九宫格血石消耗
							//TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo038"),"1", Mathf.Abs(costBlood)));
                                    }

                                    PanelGamble.panelGamble.StartLottery(itemID);
                                    PanelGamble.panelGamble.ReturnBloodInfo(cardBlood, lotteryBlood);
								}
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                                {
                                    PanelGamble.canStartEnter = true;
                                    // 血石不够，切换到商城充值
                                    SwitchToStore();
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoSlot:
                                {
                                    PanelGamble.canStartEnter = true;
                                    // 提示背包已满
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info497"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.HasID:// 处理超时后，客户端没有接收到返回，再次抽奖就刷新客户端，且不消耗血石
                                {
                                    PanelGamble.canStartEnter = true;

                                    int index = (int)mResponse.Parameters[(short)Zealm.ParameterType.SolutionNum];
                                    PanelGamble.panelGamble.RefreshSudoku(index);
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.IsDone:
                                {
                                    //PanelGamble.canStartEnter = true;

                                    // 提示：本轮抽奖次数已用完，需重新翻牌才能继续抽奖！
                                    PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1023"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    PanelGamble.canStartEnter = true;
                                    Debug.LogError(mResponse.DebugMessage);
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        PanelGamble.canStartEnter = true;
                        Debug.LogError(ex.ToString());
                    }
                    finally
                    {
                        btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    }
                }
                break;
            //case OpCode.ModifyMarrow:
            //    {
            //        try
            //        {
            //            switch (mResponse.ReturnCode)
            //            {
            //                case (short)yuan.YuanPhoton.ReturnCode.Yes:
            //                    {
            //                        int marrowIron = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowIron];
            //                        int marrowGold = (int)mResponse.Parameters[(short)Zealm.ParameterType.MarrowGold];

            //                        // 提示消耗或者获得精铁粉末提示字符串
            //                        string msgIron = "";
            //                        if (marrowIron > 0)
            //                        {
            //                            msgIron = string.Format("{0}{1}", StaticLoc.Loc.Get("info1043"), marrowIron);
            //                        }
            //                        else if (marrowIron < 0)
            //                        {
            //                            msgIron = string.Format("{0}{1}", StaticLoc.Loc.Get("info882"), marrowIron);
            //                        }
            //                        else
            //                        {
            //                            msgIron = "";
            //                        }

            //                        // 提示消耗或者获得精金粉末提示字符串
            //                        string msgGold = "";
            //                        if (marrowGold > 0)
            //                        {
            //                            msgGold = string.Format("{0}{1}", StaticLoc.Loc.Get("info1044"), marrowGold);
            //                        }
            //                        else if (marrowGold < 0)
            //                        {
            //                            msgGold = string.Format("{0}{1}", StaticLoc.Loc.Get("info883"), marrowGold);
            //                        }
            //                        else
            //                        {
            //                            msgGold = "";
            //                        }

            //                        string msg = string.Format("{0}{1}{2}", msgIron, string.IsNullOrEmpty(msgIron) || string.IsNullOrEmpty(msgGold) ? "" : ",", msgGold);

            //                        PanelStatic.StaticWarnings.warningAllTime.Show("", msg); 
            //                    }
            //                    break;
            //                case (short)yuan.YuanPhoton.ReturnCode.NoMarrowIron:
            //                    {
            //                        PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info880")); 
            //                    }
            //                    break;
            //                case (short)yuan.YuanPhoton.ReturnCode.NoMarrowGold:
            //                    {
            //                        PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info881"));
            //                    }
            //                    break;
            //                case (short)yuan.YuanPhoton.ReturnCode.Error:
            //                    {
            //                        Debug.LogError(mResponse.DebugMessage);
            //                    }
            //                    break;
            //            }
            //        }
            //        catch (System.Exception ex)
            //        {
            //            Debug.LogError(ex.ToString());
            //        }
                    //finally
                    //{
                    //    btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    //}
                //}
                //break;
            case OpCode.IphonePay:
                {
                    try
                    {
                        switch (mResponse.ReturnCode)
                        {
                            case (short)yuan.YuanPhoton.ReturnCode.Yes:// 购买成功
                                {
                                    string productInfo = (string)mResponse.Parameters[(short)110];
									StoreKitManager.RemoveOnePay(productInfo);

									int bloodStone = (int)mResponse.Parameters[(short)111];
									PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0107") + bloodStone);
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:// 提示：订单无效！
                                {
                                    string productInfo = (string)mResponse.Parameters[(short)110];
									StoreKitManager.RemoveOnePay(productInfo);

									PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1059"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.HasID:// 该订单号已使用过了
                                {
                                    string productInfo = (string)mResponse.Parameters[(short)110];
									StoreKitManager.RemoveOnePay(productInfo);
								}
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Error:
                                {
                                    Debug.LogError(mResponse.DebugMessage);
                                }
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                    finally
                    {
                        //btnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
                    }
                }
                break;
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
	}

	private void AnlaysChangeBottle(Zealm.OperationResponse mResponse)
	{
		try
		{
			switch (mResponse.ReturnCode)
			{	
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.TableKey];
					string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.TableSql];
					RefershYT (strKey,strValue);
				}
				break;
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
		finally
		{
			btnGameManager.CloseLoading ();
		}
	}

	private void ShowStrength(Zealm.OperationResponse mResponse){
		try
		{
			switch (mResponse.ReturnCode)
			{	
			case (short)yuan.YuanPhoton.ReturnCode.Yes:
			{
				yuan.YuanPhoton.CostPowerType useMoneyType=(yuan.YuanPhoton.CostPowerType)(short)mResponse.Parameters[(short)0];
				
				int tili = (int)mResponse.Parameters[(short)1];

				object[] objs = new object[3];
				objs[0] = useMoneyType;
				objs[1] = tili;
				objs[2] = true;
				UICL.SendMessage("ReturnTipsPower" , objs , SendMessageOptions.DontRequireReceiver);
			}
				break;
			case (short)yuan.YuanPhoton.ReturnCode.No:
			{
				yuan.YuanPhoton.CostPowerType useMoneyType=(yuan.YuanPhoton.CostPowerType)(short)mResponse.Parameters[(short)0];
				
				int tili = (int)mResponse.Parameters[(short)1];
				
				object[] objs = new object[3];
				objs[0] = useMoneyType;
				objs[1] = tili;
				objs[2] = false;
				UICL.SendMessage("ReturnTipsPower" , objs , SendMessageOptions.DontRequireReceiver);
				//体力不足
			}
				break;
			}


		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
		finally
		{
			btnGameManager.CloseLoading ();
		}
		
	}

	private void ShowAllMONEY(Zealm.OperationResponse mResponse){
		try
		{
			yuan.YuanPhoton.UseMoneyType useMoneyType=(yuan.YuanPhoton.UseMoneyType)(short)mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.UseMoneyType];
			int mGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.GoldNum];
			int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.BloodNum];
			int mMarrowIron = mResponse.Parameters.ContainsKey((short)ParameterType.MarrowIron) ? (int)mResponse.Parameters[(short)ParameterType.MarrowIron] : 0;
			int mMarrowGold = mResponse.Parameters.ContainsKey((short)ParameterType.MarrowGold) ? (int)mResponse.Parameters[(short)ParameterType.MarrowGold] : 0;

			object[] objs = new object[5];
			objs[0] = useMoneyType;
			objs[1] = mGold;
			objs[2] = mBlood;
			objs[3] = mMarrowIron;
			objs[4] = mMarrowGold;
			UICL.SendMessage("ReturnUseTips" , objs , SendMessageOptions.DontRequireReceiver);

		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
		finally
		{
			btnGameManager.CloseLoading ();
		}

	}
		


	/// <summary>
	/// 解析客户端使用金钱相关
	/// </summary>
	/// <param name="mResponse">M response.</param>
    private void AnlaysUseMoney(Zealm.OperationResponse mResponse)
	{
		try
		{
			yuan.YuanPhoton.UseMoneyType useMoneyType=(yuan.YuanPhoton.UseMoneyType)(short)mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.UseMoneyType];
			switch (mResponse.ReturnCode)
			{
				
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
						
				    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.TableKey];
				    string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.TableSql];
					RefershYT (strKey,strValue);
					
					int mGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.GoldNum];
				    int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.BloodNum];
                    int mMarrowIron = mResponse.Parameters.ContainsKey((short)ParameterType.MarrowIron) ? (int)mResponse.Parameters[(short)ParameterType.MarrowIron] : 0;
                    int mMarrowGold = mResponse.Parameters.ContainsKey((short)ParameterType.MarrowGold) ? (int)mResponse.Parameters[(short)ParameterType.MarrowGold] : 0;
					// SetMoneyMessage (mGold,mBlood);
                    SetMoneyMessage(mGold, mBlood, mMarrowIron, mMarrowGold);
				UICL.SendMessage("ReturnUseMoney" , useMoneyType , SendMessageOptions.DontRequireReceiver);

				//Debug.Log(YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytPlayer.Rows[0]["Soul"].YuanColumnText + "===============-----===============");
				object[] objs = new object[2];
					objs[0] = mBlood;
					objs[1] = useMoneyType;
					UICL.SendMessage("setTDPurchase" , objs , SendMessageOptions.DontRequireReceiver);

					switch(useMoneyType)
					{
						//处理相应使用金钱成功的地方
						case yuan.YuanPhoton.UseMoneyType.OpenChest:
						{
							if(OpenChest.my!=null)
							{
							        OpenChest.my.ReadInfo ();
						            string[] itemIDs = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.ItemID];

                                    //if (null != itemIDs)
                                    //{
                                    //    foreach (string str in itemIDs)
                                    //    {
                                    //        Debug.Log("OpenBox-----------------------------------" + str);
                                    //    }
                                    //}
								    warnings.OpenBoxBar(mGold.ToString (), mBlood.ToString (), itemIDs);    // 弹出领宝箱界面
									
								    OpenChest.my.ReceiveOpenBox();
								}
							}
							break;
							case yuan.YuanPhoton.UseMoneyType.MakeGold:
							{
								if(PanelMakeGold.my!=null)
								{
									PanelMakeGold.my.ShowGoldBlood ();
								}
							}
							break;
							case yuan.YuanPhoton.UseMoneyType.InviteGoPVE:
							{
								PanelStatic.StaticBtnGameManager.RobotPlayer ();
							}
							break;
							case yuan.YuanPhoton.UseMoneyType.ButtonBuildSoulAndDigest:
							{
								//收到扣除了多少灵魂
								int mSoul = (int)mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyType.SoulAndDigestGold];
								if(mSoul != 0)
									UICL.SendMessage("AnlaysUseSoul" , mSoul , SendMessageOptions.DontRequireReceiver);
							}
								break;
						}
				}
				break;			
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					switch((short)useMoneyType)
					{
				case (short)yuan.YuanPhoton.UseMoneyType.OpenChest:
						{
							// 提示，对不起，开宝箱时间未到，请耐心等待！
							//warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info913"));
							PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info913"));
						}
						break;
				case (short)yuan.YuanPhoton.UseMoneyType.BenefitsSalaries:
						{
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info414") );
						}
						break;
				case (short)yuan.YuanPhoton.UseMoneyType.BenefitsRank:
						{
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info416"));
						}
						break;
				case (short)yuan.YuanPhoton.UseMoneyType.BenefitsGuild:
						{
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info418"));
						}
						break;
				case (short)yuan.YuanPhoton.UseMoneyType.PlayerInvite:
						{
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"),StaticLoc.Loc.Get("info404"));
						}
						break;
				case (short)yuan.YuanPhoton.UseMoneyType.PlayerInvitees:
						{
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info405"));
						}
						break;
					}
				}
				break;
				case (short)yuan.YuanPhoton.ReturnCode.Nothing:
				{
				switch( (short)useMoneyType)
					{
						
				case (short)yuan.YuanPhoton.UseMoneyType.OpenChest:
						{
							// 提示，对不起，该宝箱已打开！
							// warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info403"));
							PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info914"));
						}
						break;
				case (short)yuan.YuanPhoton.UseMoneyType.PlayerInvitees:
				case (short)yuan.YuanPhoton.UseMoneyType.PlayerInvite:
						{
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info820"));
						}
						break;
					}
				}
				break;
                case (short)yuan.YuanPhoton.ReturnCode.HasID:// 这条协议只是用来打开在线宝箱面板时，接收开宝箱时间
                {
                    switch ((short)useMoneyType)
                    {

                        case (short)yuan.YuanPhoton.UseMoneyType.OpenChest:
                            {
                                int[] openBoxesTime = (int[])mResponse.Parameters[(short)yuan.YuanPhoton.UseMoneyParams.ItemID];
                                OpenChest.my.OpenBoxesTime = openBoxesTime;
                            }
                            break;
                    }
                }
                break;
				case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
				{
					warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info819"));
				}
				break;
				case (short)yuan.YuanPhoton.ReturnCode.NoGold:
				{
					//金币不足
//					warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips011"));
					warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips011"));
				}
				break;
				case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
				{
					//血石不足
//					warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips060"));
					//warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips060"));
                    SwitchToStore();
				}
				break;
                case (short)yuan.YuanPhoton.ReturnCode.NoMarrowIron:
                {
                    //提示，没有足够的精铁粉末！
                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info880"));
                }
                break;
                case (short)yuan.YuanPhoton.ReturnCode.NoMarrowGold:
                {
                    //提示，没有足够的精金结晶！
                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info881"));
                }
                break;
				case (short)yuan.YuanPhoton.ReturnCode.NoSlot:
				{
				//提示VIP等级没到，请充值
				PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1207"));
				}
				break;
				case (short)yuan.YuanPhoton.ReturnCode.NoNum:
				{
				//提示炼金次数已达到上限
				PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1206"));
				}
				break;
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
				}
				break;	
			}		
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
		finally
		{
			btnGameManager.CloseLoading ();
		}
	}

	/// <summary>
	/// 解析好友相关
	/// </summary>
	/// <param name='mResponse'>
	/// M response.
	/// </param>
    private void AnlaysFirends(Zealm.OperationResponse mResponse)
	{
		yuan.YuanPhoton.FirendsType firendsType=(yuan.YuanPhoton.FirendsType)(short)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.FirendsType];
		switch(firendsType)
		{
			case yuan.YuanPhoton.FirendsType.FirendsAddInvitForName://请求加好友从玩家名
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Nothing:
	                        {
								//没有此名称的玩家
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info411"));
							}
	                        break;			
	                }				
			}
			break;			
			case yuan.YuanPhoton.FirendsType.FirendsAddInvit://请求加好友
			{
			       switch (mResponse.ReturnCode)
	                {
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
								//发送请求消息成功
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info800"));
							}
	                        break;				
	                    case (short)yuan.YuanPhoton.ReturnCode.No:
	                        {
								//请求失败，对方不在线
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info801"));
							}
	                        break;					
	                    case (short)yuan.YuanPhoton.ReturnCode.HasID:
	                        {
								//对方已是自己好友，不要重复加好友
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info802"));
							}
	                        break;							
	                    case (short)yuan.YuanPhoton.ReturnCode.Nothing:
	                        {
								//没有此id的玩家
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info803"));
							}
	                        break;			
	                    case (short)yuan.YuanPhoton.ReturnCode.IsMine:
	                        {
								//不允许加自己为好友
							warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info804"));
							}
	                        break;					
	                }				
			}
			break;		
			case yuan.YuanPhoton.FirendsType.GetFirendsAddInvitInfo://接受请求加好友信息
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
                                string firendID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.FirendID];//请求方ID
                                string firendName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.FirendName];//请求方名称
								//这里要处理请求，并返回请求InRoom.GetInRoomInstantiate().RetrunFirendsAddInvit()
								warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"),string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info805"), firendName,StaticLoc.Loc.Get("info806")));
								btnGameManager.myFriendId = firendID;
								btnGameManager.myFriendName = firendName;
								
								warnings.warningAllEnterClose.btnEnter.target = btnGameManager.gameObject;
								warnings.warningAllEnterClose.btnEnter.functionName = "AddFirendYes";

								warnings.warningAllEnterClose.btnExit.target = btnGameManager.gameObject;
								warnings.warningAllEnterClose.btnExit.functionName = "AddFriendNo";
							}
	                        break;			
	                }				
			}
			break;

			case yuan.YuanPhoton.FirendsType.RetrunFirendsAddInvit://返回加好友请求
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.TableSql];
								RefershYT (strKey,strValue);

                                string firendID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.FirendID];//请求方ID
                                string firendName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.FirendName];//请求方名称
                                yuan.YuanPhoton.ReturnCode returnType = (yuan.YuanPhoton.ReturnCode)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.RetrunType];//响应结果
								//这里要处理消息，如果为同意就提示加好友成功，不同意可以不提示
								if(returnType==yuan.YuanPhoton.ReturnCode.Yes){
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info807"), firendName,StaticLoc.Loc.Get("info808")));
				}
								
							}
	                        break;			
	                }				
			}
			break;			
			case yuan.YuanPhoton.FirendsType.GetRetrunFirendsAddInvitInfo://接受返回请求加好友信息
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.TableSql];
								RefershYT (strKey,strValue);

                                string firendID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.FirendID];//请求方ID
                                string firendName = (string)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.FirendName];//请求方名称
                                yuan.YuanPhoton.ReturnCode returnType = (yuan.YuanPhoton.ReturnCode)((short)mResponse.Parameters[(short)yuan.YuanPhoton.FirendsParams.RetrunType]);//响应结果
								//这里要处理消息，如果为同意就提示加好友成功，不同意为对方拒绝加好友
								if(returnType==yuan.YuanPhoton.ReturnCode.Yes){
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info807"), firendName,StaticLoc.Loc.Get("info808")));
				}
				
								if(returnType==yuan.YuanPhoton.ReturnCode.No){
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}", firendName,StaticLoc.Loc.Get("info809")));
					
				}
								
							}
	                        break;			
	                }				
			}
			break;						
		}
		
		if(mResponse.ReturnCode==(short)yuan.YuanPhoton.ReturnCode.Error)
		{
			Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
		}
		btnGameManager.CloseLoading ();
	}	
	
	/// <summary>
	/// 解析活动相关
	/// </summary>
	/// <param name='mResponse'>
	/// M response.
	/// </param>
    private void AnlaysActivity(Zealm.OperationResponse mResponse)
	{
        try
        {
			short activityType = (short)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.ActivityType];
            switch (activityType)
            {
                case (short)yuan.YuanPhoton.ActivityType.JockPotLottery://奖池抽奖
                    {
                        switch (mResponse.ReturnCode)
                        {

                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.TableSql];
                                    RefershYT(strKey, strValue);

                                    int poolType = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.PoolType];//奖池类型（0为金币，1为血石）
                                    int numBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.BloodNum];//获得中奖的金币或血石数量
                                    if (poolType == 1)
                                    {
                                        warnings.warningAllEnter.ShowLucky(numBlood);
										//抽奖得到的血石统计
										////TD_info.setGiftCurrency(string.Format("0;1",numBlood,StaticLoc.Loc.Get("tdinfo039")));
									}
                                    if (poolType == 0)
                                    {
                                        warnings.warningAllEnter.ShowLucky(numBlood);
                                    }

					int mGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.ChangeGold];
					int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.ChangeBlood];
					SetMoneyMessage (mGold,mBlood);
					int[] mMoney = new int[2];
					mMoney[0] = mGold;
					mMoney[1] = mBlood;
					UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
					// 统计宝藏相关血石消耗			
					////TD_info.setUserPurchase(string.Format("{0};{1};{2}",StaticLoc.Loc.Get("tdinfo039"),"1", Mathf.Abs(mBlood)));
				}

                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoGold:
                                {
                                    //金币不足
                                    //								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips011"));
                                    warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips011"));

                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.NoBloodStone:
                                {
                                    //血石不足
                                    //								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips060"));
                                    //warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips060"));
                                    SwitchToStore();
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.TableSql];
                                    RefershYT(strKey, strValue);
                                    PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().JockPotShowInfo());
                                    //没有中奖
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info783"));
                                    int mGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.ChangeGold];
                                    int mBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.ChangeBlood];
					SetMoneyMessage (mGold,mBlood);
					int[] mMoney = new int[2];
					mMoney[0] = mGold;
					mMoney[1] = mBlood;
					UICL.SendMessage("CharBarTextMoney" , mMoney , SendMessageOptions.DontRequireReceiver);
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:
                                {
                                    //当前此功能未开放
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info778"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.IsDone:
                                {
                                    //此活动已结束
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info784"));
                                }
                                break;
                        }
                    }
                    break;
                case (short)yuan.YuanPhoton.ActivityType.JockPotShowInfo://获取奖池信息
                    {
                        switch (mResponse.ReturnCode)
                        {

                            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                                {
                                    string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.TableKey];
                                    string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.TableSql];
                                    RefershYT(strKey, strValue);
                                    int poolBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.PoolBlood];//当前奖池中所有的血石数
                                    int needBlood = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.NeedBlood];//抽奖一次所需的血石
                                    int poolGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.PoolGold];//当前奖池中所有的金币数
                                    int needGold = (int)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityParams.NeedGold];//抽奖一次所需的金币

                                    ShowLuckey.show.ShowBonuses(poolBlood, needBlood, poolGold, needGold);
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.No:
                                {
                                    //当前此功能未开放
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info778"));
                                }
                                break;
                            case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                                {
                                    //此活动已结束
                                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info784"));
                                }
                                break;
                        }
                    }
                    break;
            }

            if (mResponse.ReturnCode == (short)yuan.YuanPhoton.ReturnCode.Error)
            {
                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
        finally
        {
            btnGameManager.CloseLoading();
        }
	}	
	
	
	/// <summary>
	/// 解析任务相关
	/// </summary>
	/// <param name='mResponse'>
	/// M response.
	/// </param>
    private void AnlaysTask(Zealm.OperationResponse mResponse)
	{
		yuan.YuanPhoton.TaskType taskType=(yuan.YuanPhoton.TaskType)((short)mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TaskType]);
		switch(taskType)
		{
			case yuan.YuanPhoton.TaskType.TaskAcceptedAsID://接受任务
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableSql];
								RefershYT (strKey,strValue);
                                string taskID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TaskID];
								UICL.SendMessage("returnAddNewTask" , taskID , SendMessageOptions.DontRequireReceiver);
                     //           Debug.Log("接收任务++++++++++++++++++++++++++++++++++++++");
							}
	                        break;
						case (short)yuan.YuanPhoton.ReturnCode.No:
	                        {
								//已经接受过此任务，不可重复接受
                                string taskID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TaskID];
                           //     UICL.SendMessage("returnAddNewTask", taskID, SendMessageOptions.DontRequireReceiver);
                       //         Debug.Log("重复接收任务++++++++++++++++++++++++++++++++++++++");
	                        }
	                        break;
						case (short)yuan.YuanPhoton.ReturnCode.Nothing:
	                        {
								//无此任务id
	                        }
	                        break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableSql];
                                RefershYT(strKey, strValue);
                                string taskID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TaskID];
                                UICL.SendMessage("returnAddActivitiesTask", taskID, SendMessageOptions.DontRequireReceiver);
                            }
                            break;
	                }				
			}
			break;
			case yuan.YuanPhoton.TaskType.TaskAddNumsAsID://达成任务所需条目
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
								string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableKey];
								string[] strValue= (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableSql];
								RefershYT (strKey,strValue);   
								string taskID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TaskID];
								UICL.SendMessage("returnTaskAddNumsAsID" , taskID , SendMessageOptions.DontRequireReceiver);
							}		
	                        break;
						case (short)yuan.YuanPhoton.ReturnCode.HasID:
	                        {
								//任务已完成
								string taskID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TaskID];
	                        }
	                        break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
                            }
                            break;
	                }				
			}
			break;			
			case yuan.YuanPhoton.TaskType.TaskGiveUpAsID://放弃任务
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TableSql];
								RefershYT (strKey,strValue);
                                string taskID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.TaskParams.TaskID];
								UICL.SendMessage("returnTaskGiveUpAsID" , taskID , SendMessageOptions.DontRequireReceiver);
							}
	                        break;				
	                }				
			}
			break;					
			
		}
		
		if(mResponse.ReturnCode==(short)yuan.YuanPhoton.ReturnCode.Error)
		{
			Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
		}
		btnGameManager.CloseLoading ();
	}
	
	/// <summary>
	/// 解析活动内容相关
	/// </summary>
	/// <param name='mResponse'>
	/// M response.
	/// </param>
	private void AnlaysActivityGetInfo(Zealm.OperationResponse mResponse)
	{
        yuan.YuanPhoton.ActivityType activityType = (yuan.YuanPhoton.ActivityType)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ActivityType];
		switch(activityType)
		{
			case yuan.YuanPhoton.ActivityType.ActivityLevel:
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
                                string activityInfo = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityType.ActivityInfo];
                                string activityTime = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityType.ActivityTime];
//			                    string chargeDays = (string)mResponse.Parameters[(byte)yuan.YuanPhoton.ActivityType.ChargeDays];
                                Dictionary<int, string> activityReward = (Dictionary<int, string>)mResponse.Parameters[(short)yuan.YuanPhoton.ActivityType.ActivityReward];
								ActivityControl.activityControl.ShowActivity(activityInfo,activityTime,activityReward);
	                        }
	                        break;
						case (short)yuan.YuanPhoton.ReturnCode.No:
	                        {
								//本活动已结束
//								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info761"));
								ActivityControl.activityControl.ShowActivity(StaticLoc.Loc.Get("info761"),null,null);
	                        }
	                        break;
						case (short)yuan.YuanPhoton.ReturnCode.Nothing:
	                        {
								//本活动尚未开启
//								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info762"));
								ActivityControl.activityControl.ShowActivity(StaticLoc.Loc.Get("info762"),null,null);
	                        }
	                        break;				
	                    case (short)yuan.YuanPhoton.ReturnCode.Error:
	                        {
	                            PanelStatic.StaticBtnGameManager.NoSaveData ();
	                            Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
	                        }
	                        break;						
	                }
			}
			break;
		}
		btnGameManager.CloseLoading ();
	}
	
	/// <summary>
	/// 解析活动奖励相关
	/// </summary>
	/// <param name='mResponse'>
	/// M response.
	/// </param>
    private void AnlaysActivityGetReward(Zealm.OperationResponse mResponse)
	{
        yuan.YuanPhoton.ActivityType activityType = (yuan.YuanPhoton.ActivityType)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ActivityType];
		switch(activityType)
		{
			case yuan.YuanPhoton.ActivityType.ActivityLevel:
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.Yes:
	                        {
                                string itemID = (string)mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
                                string[] strKey = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
                                string[] strValue = (string[])mResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								RefershYT (strKey,strValue);
								UICL.SendMessage("CategoryTipsAsID" , itemID , SendMessageOptions.DontRequireReceiver);
	                        }
	                        break;
						case (short)yuan.YuanPhoton.ReturnCode.Create:
	                        {
								//您的等级没有达到最低要求，不能领取奖励
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info763"));
	                        }
	                        break;				
						case (short)yuan.YuanPhoton.ReturnCode.No:
	                        {
								//本活动还未到截止日期
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("inf0764"));
	                        }
	                        break;
                        case (short)yuan.YuanPhoton.ReturnCode.NoInventory:
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips009"));
                            }
                            break;				
						case (short)yuan.YuanPhoton.ReturnCode.Nothing:
	                        {
								//本活动尚未开启
								warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info762"));
	                        }
	                        break;				
	                    case (short)yuan.YuanPhoton.ReturnCode.Error:
	                        {
	                            PanelStatic.StaticBtnGameManager.NoSaveData ();
	                            Debug.LogError(StaticLoc.Loc.Get(mResponse.DebugMessage));
	                        }
	                        break;						
	                }
			}
			break;
		}
		btnGameManager.CloseLoading ();		
	}
	
	
	/// <summary>
	/// Refershs the Y.
	/// </summary>
	/// <param name='strKeys'>
	/// String keys.
	/// </param>
	/// <param name='strValues'>
	/// String values.
	/// </param>
	private void RefershYT(string[] strKey,string[] strValue)
	{
			for(int i=0;i<strKey.Length;i++)
			{
				if(!string.IsNullOrEmpty (strKey[i])&&BtnGameManager.yt.Rows[0].ContainsKey (strKey[i]))
				{
					BtnGameManager.yt.Rows[0][strKey[i]].YuanColumnText=strValue[i];
				}
			}	
			
			
			invCL.SendMessage("ReInitItem", SendMessageOptions.DontRequireReceiver);
	}

    /// <summary>
    /// ÉèÖÃÍæ¼Ò»Øµ÷
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="teamType"></param>
    /// <param name="requestFunctionYes"></param>
    /// <param name="requestFunctionNo"></param>
    private void SetPlayerRequest( Dictionary<short,object> parameters,string text,string teamType,string requestFunctionYes,string requestFunctionNo)
    {
        string playerID = (string)parameters[(short)yuan.YuanPhoton.ParameterType.UserID];
        string playerNickName = (string)parameters[(short)yuan.YuanPhoton.ParameterType.UserNickName];
        warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), string.Format("[ffff00]{0}[-]{2}{1}{3}", playerNickName, teamType, text , StaticLoc.Loc.Get("info478")));
        warnings.warningAllEnterClose.btnEnter.target = btnGameManager.gameObject;
        warnings.warningAllEnterClose.btnEnter.functionName = requestFunctionYes;
        warnings.warningAllEnterClose.btnExit.target = btnGameManager.gameObject;
        warnings.warningAllEnterClose.btnExit.functionName = requestFunctionNo;
        btnGameManager.dicTeamParameter = parameters;
    }

    private void SetPlayerReturn(Dictionary<short, object> parameters,string text,string teamType)
    {
        string playerID = (string)parameters[(short)yuan.YuanPhoton.ParameterType.UserID];
        string playerNickName = (string)parameters[(short)yuan.YuanPhoton.ParameterType.UserNickName];
        short returnCode = (short)parameters[(short)yuan.YuanPhoton.ParameterType.RetureRequestType];
        switch (returnCode)
        {
            case (short)yuan.YuanPhoton.ReturnCode.Yes:
                {
                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("[ffff00]{0}[-]{1}{3}{2}", playerNickName, StaticLoc.Loc.Get("info479") , teamType, text));
                    RefreshMemberList();
                }
                break;
            case (short)yuan.YuanPhoton.ReturnCode.No:
                {
                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("[ffff00]{0}[-]{1}{3}{2}", playerNickName, StaticLoc.Loc.Get("info480") , teamType, text));
                }
                break;
            case (short)yuan.YuanPhoton.ReturnCode.IsMine:
                {
                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("[ffff00]{0}[-]{1}{3}{2},{4}", playerNickName, StaticLoc.Loc.Get("info479"), teamType, text, StaticLoc.Loc.Get("meg0091")));
                }
                break;
        }
    }

    
    private IEnumerator RefreshTable()
    {
        InRoom.GetInRoomInstantiate().UpdateYuanTable("DarkSword2", BtnGameManager.yt,SystemInfo.deviceUniqueIdentifier);
        while (BtnGameManager.yt.IsUpdate)
        {
            yield return new WaitForSeconds(0.5f);
        }
        InRoom.GetInRoomInstantiate().GetTableForID(BtnGameManager.yt[0]["PlayerID"].YuanColumnText,yuan.YuanPhoton.TableType.PlayerInfo,BtnGameManager.yt);
    }

	public void SongSendTransactionRequest(yuan.YuanPhoton.ReturnCode code , yuan.YuanPhoton.RequstType codeType , string playerID , string database , string table){
        Dictionary<short, object> dicTemp = new Dictionary<short, object>();
        dicTemp.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
        //dicTemp.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, database);
        dicTemp.Add((short)yuan.YuanPhoton.ParameterType.RequestType, (short)codeType);
        //dicTemp.Add((short)yuan.YuanPhoton.ParameterType.ServerName, InRoom.GetInRoomInstantiate().ServerApplication);
        //dicTemp.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
		InRoom.GetInRoomInstantiate().ReturnRequest(
			code , 
			dicTemp);
	}

    public void PVPGONO(object sender,object parms)
    {
//        Debug.Log("取消进入战场");
        warnings.warningAllEnterClose.btnExit.functionName = "";
        btnGameManager.BtnRemoveLegionOne();
    }
	
	public System.Text.StringBuilder sbMenoyMessage=new System.Text.StringBuilder();
	/// <summary>
	/// 消费消息
	/// </summary>
	/// <param name='mGold'>
	/// M gold.
	/// </param>
	/// <param name='mBlood'>
	/// M blood.
	/// </param>
	public void SetMoneyMessage(int mGold,int mBlood)
	{
		sbMenoyMessage.Length=0;
		if(mGold>0)
		{
			sbMenoyMessage.AppendFormat ("{0}{1}{2}",StaticLoc.Loc.Get("messages150"),mGold.ToString (),StaticLoc.Loc.Get("info335"));
			EquipEnhance.instance.ShowMyItem("",sbMenoyMessage.ToString ());
		}
		else if(mGold<0)
		{
			sbMenoyMessage.AppendFormat ("{0}{1}{2}",StaticLoc.Loc.Get("messages052"),Mathf.Abs (mGold),StaticLoc.Loc.Get("info335"));
			if(sbMenoyMessage.Length!=0)
			{
				warnings.warningAllTime.Show ("",sbMenoyMessage.ToString ());
			}
		}
		if(mBlood>0)
		{
			sbMenoyMessage.AppendFormat ("{0}{1}{2}",StaticLoc.Loc.Get("messages150"),mBlood.ToString (),StaticLoc.Loc.Get("messages053"));
			EquipEnhance.instance.ShowMyItem("",sbMenoyMessage.ToString ());
		}
		else if(mBlood<0)
		{
			sbMenoyMessage.AppendFormat ("{0}{1}{2}",StaticLoc.Loc.Get("messages052"),Mathf.Abs (mBlood),StaticLoc.Loc.Get("messages053"));
			if(sbMenoyMessage.Length!=0)
			{
				warnings.warningAllTime.Show ("",sbMenoyMessage.ToString ());
			}
		}
		

	}

	//英雄徽章和征服徽章的消费提示 mGold = 征服徽章数量   mBlood = 英雄徽章数量
	public void SetHeroBadge(int mGold,int mBlood)
	{
		sbMenoyMessage.Length=0;
		if(mGold>0)
		{
			sbMenoyMessage.AppendFormat ("{0}{1}{2}",StaticLoc.Loc.Get("messages150"),mGold.ToString (),StaticLoc.Loc.Get("messages156"));
			EquipEnhance.instance.ShowMyItem("",sbMenoyMessage.ToString ());
		}
		else if(mGold<0)
		{
			sbMenoyMessage.AppendFormat ("{0}{1}{2}",StaticLoc.Loc.Get("messages052"),Mathf.Abs (mGold),StaticLoc.Loc.Get("messages156"));
			if(sbMenoyMessage.Length!=0)
			{
				warnings.warningAllTime.Show ("",sbMenoyMessage.ToString ());
			}
		}
		if(mBlood>0)
		{
			sbMenoyMessage.AppendFormat ("{0}{1}{2}",StaticLoc.Loc.Get("messages150"),mBlood.ToString (),StaticLoc.Loc.Get("messages157"));
			EquipEnhance.instance.ShowMyItem("",sbMenoyMessage.ToString ());
		}
		else if(mBlood<0)
		{
			sbMenoyMessage.AppendFormat ("{0}{1}{2}",StaticLoc.Loc.Get("messages052"),Mathf.Abs (mBlood),StaticLoc.Loc.Get("messages157"));
			if(sbMenoyMessage.Length!=0)
			{
				warnings.warningAllTime.Show ("",sbMenoyMessage.ToString ());
			}
		}
		

	}

	/// <summary>
	/// 消费消息
	/// </summary>
	/// <param name='mGold'>
	/// M gold.
	/// </param>
	/// <param name='mBlood'>
	/// M blood.
	/// </param>
	/// <param name='marrowIron'>
	///  精铁粉末
	/// </param>
	/// <param name='marrowGold'>
	/// 精金粉末
	/// </param>
	public void SetMoneyMessage(int mGold,int mBlood, int marrowIron, int marrowGold)
	{
		sbMenoyMessage.Length = 0;
		if (mGold > 0)
		{
			sbMenoyMessage.AppendFormat("{0}{1}{2}", StaticLoc.Loc.Get("messages150"), mGold.ToString(), StaticLoc.Loc.Get("info335"));
			EquipEnhance.instance.ShowMyItem("",sbMenoyMessage.ToString ());
		}
		else if (mGold < 0)
		{
			sbMenoyMessage.AppendFormat("{0}{1}{2}", StaticLoc.Loc.Get("messages052"), Mathf.Abs(mGold), StaticLoc.Loc.Get("info335"));
			if (sbMenoyMessage.Length != 0)
			{
				warnings.warningAllTime.Show("", sbMenoyMessage.ToString());
			}
		}
		if (mBlood > 0)
		{
			sbMenoyMessage.AppendFormat("{0}{1}{2}", StaticLoc.Loc.Get("messages150"), mBlood.ToString(), StaticLoc.Loc.Get("messages053"));
			EquipEnhance.instance.ShowMyItem("",sbMenoyMessage.ToString ());
		}
		else if (mBlood < 0)
		{
			sbMenoyMessage.AppendFormat("{0}{1}{2}", StaticLoc.Loc.Get("messages052"), Mathf.Abs(mBlood), StaticLoc.Loc.Get("messages053"));
			if (sbMenoyMessage.Length != 0)
			{
				warnings.warningAllTime.Show("", sbMenoyMessage.ToString());
			}
		}
		if (marrowIron > 0)
		{
			sbMenoyMessage.AppendFormat("{0}{1}{2}", StaticLoc.Loc.Get("messages150"), marrowIron.ToString(), StaticLoc.Loc.Get("info897"));
			EquipEnhance.instance.ShowMyItem("",sbMenoyMessage.ToString ());
		}
		else if (marrowIron < 0)
		{
			sbMenoyMessage.AppendFormat("{0}{1}{2}", StaticLoc.Loc.Get("messages052"), Mathf.Abs(marrowIron), StaticLoc.Loc.Get("info897"));
			if (sbMenoyMessage.Length != 0)
			{
				warnings.warningAllTime.Show("", sbMenoyMessage.ToString());
			}
		}
		if (marrowGold > 0)
		{
			sbMenoyMessage.AppendFormat("{0}{1}{2}", StaticLoc.Loc.Get("messages150"), marrowGold.ToString(), StaticLoc.Loc.Get("info898"));
			EquipEnhance.instance.ShowMyItem("",sbMenoyMessage.ToString ());
		}
		else if (marrowGold < 0)
		{
			sbMenoyMessage.AppendFormat("{0}{1}{2}", StaticLoc.Loc.Get("messages052"), Mathf.Abs(marrowGold), StaticLoc.Loc.Get("info898"));
			if (sbMenoyMessage.Length != 0)
			{
				warnings.warningAllTime.Show("", sbMenoyMessage.ToString());
			}
		}
		

	}

    private string[] strGO;
    public void PVPGO()
    {
        UICL.SendMessage("PVPGO", strGO, SendMessageOptions.DontRequireReceiver);
    }
	public GameObject opc;
	public GameObject tc;
	public GameObject invCL;
	public GameObject UICL;
    public GameObject MainTW;
	public GameObject PVPCL;
	public GameObject lding;
	public GameObject Song;

    //---------------用于PVP战场的变量------------------------
    //public bool isInTeam = false;// 当前是否在队伍中
    public string captainID = "";// 队长ID
    public List<string> teamMembersID = new List<string>(); // 所有组员ID
    //---------------用于PVP战场的变量------------------------

    public int taskActivityState = 0;// 任务活动状态，0表示参加活动，1表示活动进行中，2表示可领取奖励

    public static int battlefieldID = 0;// 正在排队的战场，0表示没有排队，1表示单人PVP排队中，2表示战场排队中

    /// <summary>
    /// 刷新组队、2V2和4v4成员列表
    /// </summary>
    private void RefreshMemberList()
    {
        if (btnGameManager.gridsRefreshTeam != null)
        {
            btnGameManager.gridsRefreshTeam.SendMessage("RecevieMsgRefresh", SendMessageOptions.DontRequireReceiver);
        }
        btnGameManager.gridsRefresh2V2.SendMessage("RecevieMsgRefresh", SendMessageOptions.DontRequireReceiver);
        btnGameManager.gridsRefresh4V4.SendMessage("RecevieMsgRefresh", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 血石不足时提示充值，点充值可切换到商城
    /// </summary>
    public void SwitchToStore()
    {
//        if (isTown())
//        {
            warnings.warningAllEnterClose.btnEnter.target = PanelStatic.StaticBtnGameManagerBack.UICL;
            warnings.warningAllEnterClose.btnEnter.functionName = "StoreMoveOn";
            warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info1018"));
//        }
//        else
//        {
//            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("tips060"));
//        }
    }

    /// <summary>
    /// 训练专用血石不足时提示充值，点充值可切换到商城
    /// </summary>
    public void TrainSwitchToStore(string str)
    {
        warnings.warningAllEnterClose.btnEnter.target = PanelStatic.StaticBtnGameManagerBack.UICL;
        warnings.warningAllEnterClose.btnEnter.functionName = "StoreMoveOn";
        warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info359"), str + ","+StaticLoc.Loc.Get("info1180"));
    }


    // 判断是否在主城
    public static bool isTown()
    {
        return Application.loadedLevelName.Substring(3, 1).Equals("1");// 主城地图名一般都是“Map1**”
    }


}

