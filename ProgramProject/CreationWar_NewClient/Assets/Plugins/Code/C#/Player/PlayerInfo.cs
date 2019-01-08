using UnityEngine;
using System.Collections;
public enum TeamType
{
    PVP2=0,
    PVP4,
    Legion,
    Team,
}

public class PlayerInfo : MonoBehaviour {

    public UISprite picPlayer;
    public UILabel lblPlayerName;
    public UILabel lblPlayerLevel;
    public UILabel lblPlayerPro;
    public UILabel lblPrestige;
    public UILabel lblRanking;
    public UILabel lblGuild;
    public UILabel lblLocation;
    public BtnTalkSomebody btnTalk;
    public yuan.YuanMemoryDB.YuanRow yr;
    [HideInInspector]
    public TeamType teamType;

    void Start()
    {
		warnings=PanelStatic.StaticWarnings;
        picPlayer = this.transform.FindChild("PicPlayer").GetComponent<UISprite>();
        lblPlayerName = this.transform.FindChild("lblPlayerName").GetComponent<UILabel>();
        lblPlayerLevel = this.transform.FindChild("lblPlayerLevel").GetComponent<UILabel>();
        lblPlayerPro = this.transform.FindChild("lblPlayerPro").GetComponent<UILabel>();
        lblPrestige = this.transform.FindChild("lblPrestige").GetComponent<UILabel>();
        lblRanking = this.transform.FindChild("lblRanking").GetComponent<UILabel>();
        lblGuild = this.transform.FindChild("lblGuild").GetComponent<UILabel>();
        lblLocation = this.transform.FindChild("lblLocation").GetComponent<UILabel>();
        teamType = TeamType.Team;
    }

  //  void Update()
  //  {
  //      Debug.Log("-----------------teamType:" + teamType);
  //  }

	private yuan.YuanMemoryDB.YuanRow yrLoaction;
    /// <summary>
    /// 更新玩家信息
    /// </summary>
    public void RefreshPlayerInfo()
    {
        if (yr != null&&yr.ContainsKey ("PlayerName"))
        {
            lblPlayerName.text = yr["PlayerName"].YuanColumnText.Trim();
            lblPlayerLevel.text = yr["PlayerLevel"].YuanColumnText.Trim();
            lblPlayerPro.text = RefreshList.GetPro(yr["ProID"].YuanColumnText.Trim());
            lblPrestige.text = yr["Prestige"].YuanColumnText.Trim();
            lblRanking.text = yr["VSRanking"].YuanColumnText.Trim();
			if(yr["GuildName"].YuanColumnText.Trim()=="")
			{
				lblGuild.text = StaticLoc.Loc.Get("info689");
			}else{
			lblGuild.text = yr["GuildName"].YuanColumnText.Trim();
			}
			yrLoaction=YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytMapLevel.SelectRowEqual ("MapID",yr["Place"].YuanColumnText.Trim());
            lblLocation.text = yrLoaction["MapName"].YuanColumnText.Trim();
            if (btnTalk != null)
            {
                btnTalk.playerID = yr["PlayerID"].YuanColumnText.Trim();
            }
        }
    }
	
	public static bool canInviteGoPVE=false;
	public static string mapName=string.Empty;
	public float timePVE=0;
	
	private int FriendTimebtnOne = 0;
	private int FriendTimebtnTwo = 0;
	

	public enum RobotType
	{
		One,
		Two,
	}
	
	public RobotType robotType=RobotType.One;
	
	public void SetBobotPlayerOne(GameObject mObj)
	{
		BtnPlayerForTeam tempPlayer=mObj.GetComponent<BtnPlayerForTeam>();
		PanelStatic.StaticBtnGameManager.objsBobotPlayer[0]=tempPlayer.lblPlayerName.text;
		PanelStatic.StaticBtnGameManager.objsBobotPlayer[1]=tempPlayer.lblPlayerLevel.text;
		PanelStatic.StaticBtnGameManager.objsBobotPlayer[2]=tempPlayer.strPro;
		robotType=RobotType.One;
//		if(FriendTimebtnTwo==0){
//		objsBobotPlayer[0]=tempPlayer.lblPlayerName.text;
//		objsBobotPlayer[1]=tempPlayer.lblPlayerLevel.text;
//		objsBobotPlayer[2]=tempPlayer.strPro;
//			FriendTimebtnTwo = 1;
//		}
		
	}


	public void SetBobotPlayerTwo(GameObject mObj)
	{
		BtnPlayerForTeam tempPlayer=mObj.GetComponent<BtnPlayerForTeam>();
		PanelStatic.StaticBtnGameManager.objsBobotPlayer[0]=tempPlayer.lblPlayerName.text;
		PanelStatic.StaticBtnGameManager.objsBobotPlayer[1]=tempPlayer.lblPlayerLevel.text;
		PanelStatic.StaticBtnGameManager.objsBobotPlayer[2]=tempPlayer.strPro;
		robotType=RobotType.Two;
//		if(FriendTimebtnTwo==0){
//		objsBobotPlayer[0]=tempPlayer.lblPlayerName.text;
//		objsBobotPlayer[1]=tempPlayer.lblPlayerLevel.text;
//		objsBobotPlayer[2]=tempPlayer.strPro;
//			FriendTimebtnTwo = 1;
//		}
		
	}



	public static string[] objsBobotPlayerOne=new string[3];
	public  static string[] objsBobotPlayerTwe = new string[3];

	private static string porfes;
	private  static string myLevel;
	public  static void PlayerShowInfo(){
		porfes = BtnGameManager.GetPro( BtnGameManager.yt[0]["ProID"].YuanColumnText);
		myLevel = BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText;
		int ThisMyLevel = int.Parse(BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText);
		int numPro = int.Parse(BtnGameManager.yt[0]["ProID"].YuanColumnText.Trim());
		if(porfes == StaticLoc.Loc.Get("buttons581")){
			string strPro="1";
			objsBobotPlayerOne[0]=StaticLoc.Loc.Get("buttons576");
			if(ThisMyLevel-2<0){
				objsBobotPlayerOne[1]=(1).ToString();
			}else{
			objsBobotPlayerOne[1]=(ThisMyLevel-2).ToString();
			}
			objsBobotPlayerOne[2]=strPro;



			string strProOne="3";
			objsBobotPlayerTwe[0]= StaticLoc.Loc.Get("buttons582");
			objsBobotPlayerTwe[1]=(ThisMyLevel+2).ToString();
			objsBobotPlayerTwe[2]=strProOne;
			
		}
		
		if(porfes == StaticLoc.Loc.Get("buttons576")){
			string  strPro="2";
			objsBobotPlayerOne[0]=StaticLoc.Loc.Get("buttons581");
			if(ThisMyLevel-2<0){
				objsBobotPlayerOne[1]=(1).ToString();
			}else{
				objsBobotPlayerOne[1]=(ThisMyLevel-2).ToString();
			}
			objsBobotPlayerOne[2]=strPro;

			string strProOne="3";
			objsBobotPlayerTwe[0]=StaticLoc.Loc.Get("buttons572");
			objsBobotPlayerTwe[1]=(ThisMyLevel+2).ToString();
			objsBobotPlayerTwe[2]=strProOne;
		}

		
		if(porfes == StaticLoc.Loc.Get("buttons582")){
			string  strPro="1";
			objsBobotPlayerOne[0]=StaticLoc.Loc.Get("buttons576");
			if(ThisMyLevel-2<0){
				objsBobotPlayerOne[1]=(1).ToString();
			}else{
				objsBobotPlayerOne[1]=(ThisMyLevel-2).ToString();
			}
			objsBobotPlayerOne[2]=strPro;

			string strProOne="2";
			objsBobotPlayerTwe[0]=StaticLoc.Loc.Get("buttons571");
			objsBobotPlayerTwe[1]=(ThisMyLevel+2).ToString();
			objsBobotPlayerTwe[2]=strProOne;

			
		}

		PanelStatic.StaticBtnGameManagerBack.invCL.SendMessage("CreateBotPlayer",objsBobotPlayerOne, SendMessageOptions.DontRequireReceiver);
//		PanelStatic.StaticBtnGameManagerBack.invCL.SendMessage("CreateBotPlayer",objsBobotPlayerTwe, SendMessageOptions.DontRequireReceiver);
	}
	
	public void BtnInviteGoPVEClick()
	{ 
		
		 if (yr != null&&Application.loadedLevelName!="Map200"&&yr.ContainsKey ("PlayerID"))
        {
			
			if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.InviteGoPVESwitch)!="1")
			{
				warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info645"));
				return;
			}
			if(Time.time-timePVE>=5)
			{
				timePVE=Time.time;
				if(PhotonNetwork.room!=null&&PhotonNetwork.room.playerCount>=4)
				{
					warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info505"));
				}
				else if(yr.ContainsKey ("PlayerID"))
				{
					InRoom.GetInRoomInstantiate ().InviteGoPVE (yr["PlayerID"].YuanColumnText,mapName,Application.loadedLevelName.Substring (3,1)=="1"?true:false);
				}
			}
			else
			{
				warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info506"));
			}
		}
		else
		{
			

			switch(robotType)
			{
				case RobotType.One:
				{
					if(FriendTimebtnOne==0)
					{
					warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"),string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info818"),"50",StaticLoc.Loc.Get("info297")));
					warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
					warnings.warningAllEnterClose.btnEnter.functionName = "CostMoney";
					
					warnings.warningAllEnterClose.btnExit.target = this.gameObject;
					warnings.warningAllEnterClose.btnExit.functionName = "CloseInvi";

						FriendTimebtnOne=1;
					}
					else
					{
						return;
					}
				}
				break;
				case RobotType.Two:
				{
					if(FriendTimebtnTwo==0)
					{
					warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"),string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info818"),"50",StaticLoc.Loc.Get("info297")));
					warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
					warnings.warningAllEnterClose.btnEnter.functionName = "CostMoney";
					
					warnings.warningAllEnterClose.btnExit.target = this.gameObject;
					warnings.warningAllEnterClose.btnExit.functionName = "CloseInvi";

						FriendTimebtnTwo=1;
					}
					else
					{
						return;
					}
				}
				break;				
			}
			//执行机器人方法
//			PanelStatic.StaticBtnGameManager.invcl.SendMessage("CreateBotPlayer", objsBobotPlayer, SendMessageOptions.DontRequireReceiver);	



//			warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"),string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info818"),"50",StaticLoc.Loc.Get("info297")));
//			warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
//			warnings.warningAllEnterClose.btnEnter.functionName = "CostMoney";
//			
//			warnings.warningAllEnterClose.btnExit.target = this.gameObject;
//			warnings.warningAllEnterClose.btnExit.functionName = "CloseInvi";

		}
	}
	
	public void CostMoney(){
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().UseMoney (yuan.YuanPhoton.UseMoneyType.InviteGoPVE,0,0,null));
//		YuanBackInfo back=new YuanBackInfo("playerRobote"+this.gameObject.name);
//			InRoom.GetInRoomInstantiate ().ClientMoney ("0",System.Convert.ToString (-50,16),back);
//			warnings.warningAllEnterClose.Close();
//			while(true)
//			{
//				if(!back.isUpate)
//				{
//					break;
//				}
//				yield return new WaitForSeconds(0.1f);
//			}
//			
//			if(back.opereationResponse.ReturnCode==(short)yuan.YuanPhoton.ReturnCode.Yes)
//			{
//				//执行机器人方法
//				PanelStatic.StaticBtnGameManager.invcl.SendMessage("CreateBotPlayer", objsBobotPlayer, SendMessageOptions.DontRequireReceiver);
//			}
		warnings.warningAllEnterClose.Close();
	}
	
	public void CloseInvi(){
		warnings.warningAllEnterClose.Close();
	}
	
	
	
	public void BtnPVP1()
	{
		if (yr != null&&Application.loadedLevelName!="Map200")
        {
			if(Application.loadedLevelName.Substring (3,1)=="1")
			{
				if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.InvitePVP1Switch)!="1")
				{
					warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info645"));
					return;
				}				
				if(Time.time-timePVE>=5)
				{
					timePVE=Time.time;
					InRoom.GetInRoomInstantiate ().PVP1Invite (yr["PlayerID"].YuanColumnText);
					
				}
				else
				{
					warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info506"));
				}
			}
			else
			{
				warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info508"));
			}
		}		
	}

    public void BtnExitClick()
    {
        if (yr != null && yr.ContainsKey("PlayerID") && yr["PlayerID"].YuanColumnText.Trim() == BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim())
        {
            switch (teamType)
            {
                case TeamType.PVP2:
                    {
                        InRoom.GetInRoomInstantiate().CorpRemove(BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText.Trim(), yr["PlayerID"].YuanColumnText);
                    }
                    break;
                case TeamType.PVP4:
                    {
                        InRoom.GetInRoomInstantiate().CorpRemove(BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText.Trim(), yr["PlayerID"].YuanColumnText);
                    }
                    break;
                case TeamType.Legion:
                    {
                        InRoom.GetInRoomInstantiate().LegionRemove(BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim());
                    }
                    break;
                case TeamType.Team:
                    {
                        InRoom.GetInRoomInstantiate().TeamRemove(BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim());
                    }
                    break;
            }
        }
    }

    public void BtnRemoveClick()
    {
        if (yr != null && yr.ContainsKey("PlayerID") && yr["PlayerID"].YuanColumnText.Trim() != BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim())
        {
            switch (teamType)
            {
                case TeamType.PVP2:
                    {
                        InRoom.GetInRoomInstantiate().CorpRemove(BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText.Trim(), yr["PlayerID"].YuanColumnText);
                    }
                    break;
                case TeamType.PVP4:
                    {
                        InRoom.GetInRoomInstantiate().CorpRemove(BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText.Trim(), yr["PlayerID"].YuanColumnText);
                    }
                    break;
                case TeamType.Legion:
                    {
                        InRoom.GetInRoomInstantiate().LegionRemove(yr["PlayerID"].YuanColumnText);
                    }
                    break;
                case TeamType.Team:
                    {
                        InRoom.GetInRoomInstantiate().TeamRemove(yr["PlayerID"].YuanColumnText);
                    }
                    break;
            }
        }
    }

    public Warnings warnings;
    public void BtnPlayerUp()
    {
        if (yr != null && yr.ContainsKey("PlayerID") && yr["PlayerID"].YuanColumnText.Trim() != BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim())
        {
            switch (teamType)
            {
                case TeamType.PVP2:
                    {
                        warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info407"));
                        warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
                        warnings.warningAllEnterClose.btnEnter.functionName = "PlayerUp";
                    }
                    break;
                case TeamType.PVP4:
                    {
                        warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info408"));
                        warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
                        warnings.warningAllEnterClose.btnEnter.functionName = "PlayerUp";
                    }
                    break;
                case TeamType.Legion:
                    {
                        warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info359"),StaticLoc.Loc.Get("info409") );
                        warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
                        warnings.warningAllEnterClose.btnEnter.functionName = "PlayerUp";
                    }
                    break;
                case TeamType.Team:
                    {
                        warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info410"));
                        warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
                        warnings.warningAllEnterClose.btnEnter.functionName = "PlayerUp";
                    }
                    break;
            }
        }
      
    }

    public void PlayerUp()
    {
        warnings.warningAllEnterClose.Close();
        if (yr != null && yr.ContainsKey("PlayerID") && yr["PlayerID"].YuanColumnText.Trim() != BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim())
        {
            switch (teamType)
            {
                case TeamType.PVP2:
                    {
                        InRoom.GetInRoomInstantiate().PVPPlayerUp(yr["PlayerID"].YuanColumnText.Trim(), yuan.YuanPhoton.CorpType.PVP2);
                    }
                    break;
                case TeamType.PVP4:
                    {
                        InRoom.GetInRoomInstantiate().PVPPlayerUp(yr["PlayerID"].YuanColumnText.Trim(), yuan.YuanPhoton.CorpType.PVP4);
                    }
                    break;
                case TeamType.Legion:
                    {
                        InRoom.GetInRoomInstantiate().LegionPlayerUp(yr["PlayerID"].YuanColumnText);
                    }
                    break;
                case TeamType.Team:
                    {
                        InRoom.GetInRoomInstantiate().TeamPlayerUp(yr["PlayerID"].YuanColumnText);
                    }
                    break;
            }
        }
    }

    public GameObject btnSend;
    private string[] someOne = new string[2];
    public void BtnTalk()
    {
        if(int.Parse(BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText) < 5)
        {
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info846"));
            return;
        }

        if (yr != null && yr.ContainsKey("PlayerID") && yr["PlayerID"].YuanColumnText.Trim() != BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim())
        {
            someOne[0] = yr["PlayerID"].YuanColumnText.Trim();
            someOne[1] = yr["PlayerName"].YuanColumnText.Trim();
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage ("OpenLiaoTianOne",someOne,SendMessageOptions.DontRequireReceiver);
			//PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage ("ShowLiaoTian",SendMessageOptions.DontRequireReceiver);
			//PanelStatic.StaticBtnGameManager.btnSend.active=true;
            //PanelStatic.StaticBtnGameManager.btnSend.SendMessage("ShowOne", someOne, SendMessageOptions.DontRequireReceiver);
//			Debug.Log ("----------------");
        }
    }

    public UIInput txtName;
    private yuan.YuanMemoryDB.YuanTable ytPVP2PlayerName = new yuan.YuanMemoryDB.YuanTable("ytPVP2PlayerName", "");
    private yuan.YuanMemoryDB.YuanTable ytPVP4PlayerName = new yuan.YuanMemoryDB.YuanTable("ytPVP4PlayerName", "");
    private yuan.YuanMemoryDB.YuanTable ytLegionPlayerName = new yuan.YuanMemoryDB.YuanTable("ytLegionPlayerName", "");
    private yuan.YuanMemoryDB.YuanTable ytTeamPlayerName = new yuan.YuanMemoryDB.YuanTable("ytTeamPlayerName", "");
    public IEnumerator BtnInviteClick() 
    {
        if (txtName.text != "")
        {
            if (txtName.text.Trim() != BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText)
            {
                switch (teamType)
                {
                    case TeamType.PVP2:
                        {
                            //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select PlayerID from PlayerInfo where PlayerName='{0}'", txtName.text.Trim()), "DarkSword2", ytPVP2PlayerName);
							InRoom.GetInRoomInstantiate ().GetPlayerForName(txtName.text.Trim(),ytPVP2PlayerName);
                            while (ytPVP2PlayerName.IsUpdate)
                            {
                                yield return new WaitForSeconds(0.1f);
                            }
                            if (ytPVP2PlayerName != null && ytPVP2PlayerName.Rows.Count > 0)
                            {
                                InRoom.GetInRoomInstantiate().GorpsInviteAdd(yuan.YuanPhoton.CorpType.PVP2, ytPVP2PlayerName.Rows[0]["PlayerID"].YuanColumnText);
                            }
                            else
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info411"));
                            }


                        }
                        break;
                    case TeamType.PVP4:
                        {
                            //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select PlayerID from PlayerInfo where PlayerName='{0}'", txtName.text.Trim()), "DarkSword2", ytPVP4PlayerName);
                            InRoom.GetInRoomInstantiate ().GetPlayerForName(txtName.text.Trim(),ytPVP4PlayerName);
							while (ytPVP4PlayerName.IsUpdate)
                            {
                                yield return new WaitForSeconds(0.1f);
                            }
                            if (ytPVP4PlayerName != null && ytPVP4PlayerName.Rows.Count > 0)
                            {
                                InRoom.GetInRoomInstantiate().GorpsInviteAdd(yuan.YuanPhoton.CorpType.PVP4, ytPVP4PlayerName.Rows[0]["PlayerID"].YuanColumnText);
                            }
                            else
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info411"));
                            }

                        }
                        break;
                    case TeamType.Legion:
                        {
                            //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select PlayerID from PlayerInfo where PlayerName='{0}'", txtName.text.Trim()), "DarkSword2", ytLegionPlayerName);
							InRoom.GetInRoomInstantiate ().GetPlayerForName(txtName.text.Trim(),ytLegionPlayerName);
                            while (ytLegionPlayerName.IsUpdate)
                            {
                                yield return new WaitForSeconds(0.1f);
                            }
                            if (ytLegionPlayerName != null && ytLegionPlayerName.Rows.Count > 0)
                            {
                                InRoom.GetInRoomInstantiate().LegionInviteAdd(ytLegionPlayerName.Rows[0]["PlayerID"].YuanColumnText);
                            }
                            else
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info411"));
                            }

                        }
                        break;
                    case TeamType.Team:
                        {
                            //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select PlayerID from PlayerInfo where PlayerName='{0}'", txtName.text.Trim()), "DarkSword2", ytTeamPlayerName);
							InRoom.GetInRoomInstantiate ().GetPlayerForName(txtName.text.Trim(),ytTeamPlayerName);
                            while (ytTeamPlayerName.IsUpdate)
                            {
                                yield return new WaitForSeconds(0.1f);
                            }
                            if (ytTeamPlayerName != null && ytTeamPlayerName.Rows.Count > 0)
                            {
                                InRoom.GetInRoomInstantiate().TeamInviteAdd(ytTeamPlayerName.Rows[0]["PlayerID"].YuanColumnText);
                            }
                            else
                            {
                                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info411"));
                            }

                        }
                        break;
                }
            }
            else
            {
                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info518"));
            }
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"),StaticLoc.Loc.Get("info519") );
        }
    }
	

	
    public void GuildTeamInv()
    {
        if (yr != null && yr.ContainsKey("PlayerID") && yr["PlayerID"].YuanColumnText.Trim() != BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim())
        {
            InRoom.GetInRoomInstantiate().TeamInviteAdd(yr["PlayerID"].YuanColumnText);
        }
    }

    private yuan.YuanMemoryDB.YuanTable ytGuildPlayer = new yuan.YuanMemoryDB.YuanTable("GuildPlayer", "");
    public IEnumerator GuildInviteAdd()
    {
        if (txtName.text.Trim() != "")
        {
            if (txtName.text.Trim() != BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText.Trim() && BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText != "")
            {
                string[] tablename = new string[1];
                tablename[0] = txtName.text.Trim();
                string[] tablerow = new string[1];
                tablerow[0] = "PlayerID";
             //   tablerow[1] = "PlayerName";

                InRoom.GetInRoomInstantiate().GetTablesSomeForNames(tablename, tablerow,yuan.YuanPhoton.TableType.PlayerInfo, ytGuildPlayer);
                //GetTablesSomeForNames(string[] tableNames,string[] tableRows,TableType mTableType,YuanTable table)
                //InRoom.GetInRoomInstantiate ().GetPlayerForName(txtName.text.Trim(),ytGuildPlayer);
                while (ytGuildPlayer.IsUpdate)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                if (ytGuildPlayer != null && ytGuildPlayer.Rows.Count > 0)
                {
                    InRoom.GetInRoomInstantiate().GuildInviteAdd(ytGuildPlayer.Rows[0]["PlayerID"].YuanColumnText, BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText);
                }
                else
                {
                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info411"));
                }
            }
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info520"));
        }
    }


    public void GuildPlayerUp()
    {
        if (yr != null && yr.ContainsKey("PlayerID"))
        {
            InRoom.GetInRoomInstantiate().GuildPlayerPurview(yr["PlayerID"].YuanColumnText, true);
        }
    }


    public void GuildPlayerDown()
    {
        if (yr != null && yr.ContainsKey("PlayerID"))
        {
            InRoom.GetInRoomInstantiate().GuildPlayerPurview(yr["PlayerID"].YuanColumnText, false);
        }
    }

    public void GuildPlayerRemove()
    {
        if (yr != null && yr.ContainsKey("PlayerID") && BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText != "")
        {
            InRoom.GetInRoomInstantiate().GuildRemove(yr["PlayerID"].YuanColumnText, BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText);
        }
    }

    public void GuildPlayerStopTalk()
    {
        if (yr != null && yr.ContainsKey("PlayerID") && BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText != "")
        {
            InRoom.GetInRoomInstantiate().GuildStopTalk(yr["PlayerID"].YuanColumnText, BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText);
        }
    }
	
	public void BlackFirend()
	{
        if (yr != null && yr.ContainsKey("PlayerID"))
        {
            PanelStatic.StaticBtnGameManager.BlackFirend (yr["PlayerID"].YuanColumnText);
        }		
	}
}
