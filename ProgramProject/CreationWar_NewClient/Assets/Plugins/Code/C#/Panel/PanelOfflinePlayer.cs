using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelOfflinePlayer : MonoBehaviour {

    public UIGrid myGrid;
    public BtnPlayerForTeam btnPlayerForOnePVP;
    public PanelListTabel listTabel;
    public UIToggle ckbRanking;
	public UIToggle ckbRevenge;
	public int maxListNum = 4;

	public static PanelOfflinePlayer panelOfflinePlayer;

	public yuan.YuanMemoryDB.YuanRow objRobotRow;
	public yuan.YuanMemoryDB.YuanRow objRobotRow1;
	public yuan.YuanMemoryDB.YuanRow objRobotRow2;
	// Use this for initialization
	void Start () {
		panelOfflinePlayer = this;
		ShowRobotytList();
	}
	void ShowRobotytList(){
		robotyt = new yuan.YuanMemoryDB.YuanTable("","");

		objRobotRow  = BtnGameManager.yt[0].CopyTo();
		objRobotRow["ProID"].YuanColumnText="1";
		objRobotRow["PlayerLevel"].YuanColumnText="2";
		objRobotRow["PlayerName"].YuanColumnText=StaticLoc.Loc.Get("info1071");
		objRobotRow["PlayerID"].YuanColumnText="-10";


		objRobotRow1 = BtnGameManager.yt[0].CopyTo();
		objRobotRow1["ProID"].YuanColumnText="2";
		objRobotRow1["PlayerLevel"].YuanColumnText="2";
		objRobotRow1["PlayerName"].YuanColumnText=StaticLoc.Loc.Get("info1072");
		objRobotRow1["PlayerID"].YuanColumnText="-10";

		objRobotRow2 = BtnGameManager.yt[0].CopyTo();
		objRobotRow2["ProID"].YuanColumnText="3";
		objRobotRow2["PlayerLevel"].YuanColumnText="2";
		objRobotRow2["PlayerName"].YuanColumnText=StaticLoc.Loc.Get("info1073");
		objRobotRow2["PlayerID"].YuanColumnText="-10";
		objRobotRow2["EquipItem"].YuanColumnText = "3101143624100000003000000;;;;;;6201322311121000000000000,01;;;;;;";
		objRobotRow2["Skill"].YuanColumnText = "30;20;20;20;20;20;10;10;00;20;20;20;10;10;00;20;20;20;10;10;20;10;10;";
		objRobotRow2["SkillsPostion"].YuanColumnText = "0,,;0,,;24,14,ProID_314;12,03,ProID_303;";

//		robotyt.Add (objRobotRow);
//		robotyt.Add (objRobotRow1);
		robotyt.Add (objRobotRow2);
	}

	 public void ShowShaDow(){
		SetPVP1List(robotyt);
	}

	public void RobotytShaDow(){
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("YaoQingPVPRobotytShaDow",robotyt, SendMessageOptions.DontRequireReceiver);
	}


    void OnEnable()
    {
        StartCoroutine(YuanEnable());
    }

    private IEnumerator YuanEnable()
    {
        yield return new WaitForSeconds(0.1f);
		if(HangUp.isRevenge)
		{
			if(ckbRevenge!=null)
			{
			ckbRevenge.isChecked=true;
       		 StartCoroutine(BtnRevengeClick());		
			}
		}
		else
		{
			if(ckbRanking!=null)
			{
	        ckbRanking.isChecked = true;
			StartCoroutine( BtnRankingClick ());
			}
		}
    }

    private yuan.YuanMemoryDB.YuanTable ytRanking = new yuan.YuanMemoryDB.YuanTable("offlineplayerRanking","");
    public IEnumerator BtnRankingClick()
    {
        //string strSql = string.Format("Select top 20 * from (select PlayerID,PlayerName,ProID,PlayerLevel,ROW_NUMBER()over(order by ColosseumPoint desc)ROW_NUMBER from PlayerInfo) as temp where PlayerID != {0}",BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText);
		//string strSql=string.Format ("select(select count(*) from PlayerInfo where a.ColosseumPoint<ColosseumPoint or (a.ColosseumPoint=ColosseumPoint and a.CreateTime<CreateTime)) as ROW_NUMBER,a.PlayerID,a.PlayerName,a.ProID,a.PlayerLevel from PlayerInfo a where PlayerID != {0} order by ROW_NUMBER limit 20",BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText);
       //StartCoroutine(BtnClick(ytRanking, strSql, "决  斗"));
		InRoom.GetInRoomInstantiate ().GetRankTopYT (yuan.YuanPhoton.RankingType.Abattoir,ytRanking);
//		Debug.Log ("----------------------");
        for (int i = 0; i < listPVP1.Count; i++)
        {
            listPVP1[i].gameObject.SetActiveRecursively(false);
        }
		listTabel.SetZore ();
        while (ytRanking.IsUpdate)
        {
            yield return new WaitForSeconds(0.1f);
        }

        listTabel.SetFrist(ytRanking, SetPVP1List, maxListNum);		
    }

    private yuan.YuanMemoryDB.YuanTable ytFirends = new yuan.YuanMemoryDB.YuanTable("offlineplayerFirends", "");
    public void BtnFirendsClick()
    {
		
        for (int i = 0; i < listPVP1.Count; i++)
        {
            listPVP1[i].gameObject.SetActiveRecursively(false);
        }
        string[] strFirend = BtnGameManager.yt[0]["FriendsId"].YuanColumnText.Split(';');
		List<string> listFirend=new List<string>();
		for(int i=0;i<strFirend.Length;i++)
		{
			if(strFirend[i]!="")
			{
				listFirend.Add (strFirend[i]);
			}
		}
        if (listFirend.Count > 0)
        {
            //System.Text.StringBuilder strSqlFirend = new System.Text.StringBuilder();
            //strSqlFirend.Append("Select PlayerID,PlayerName,ProID,PlayerLevel from PlayerInfo where ");
            //for (int i = 0; i < listFirend.Count; i++)
            //{
			//
            //    if (i != 0 && listFirend[i] != "")
            //    {
            //        strSqlFirend.AppendFormat(" or PlayerID={0}", listFirend[i]);
            //    }
            //    else if (listFirend[i] != "")
            //    {
            //        strSqlFirend.AppendFormat("PlayerID={0}", listFirend[i]);
            //    }
            //}
            StartCoroutine(BtnClick(ytFirends, strFirend, "决  斗"));
        }
    }

    private yuan.YuanMemoryDB.YuanTable ytRandom = new yuan.YuanMemoryDB.YuanTable("offlineplayerRandom", "");
    public IEnumerator BtnRamdomClick()
    {
		string strSql=string.Empty;
        //strSql = string.Format("select top 20 PlayerID,PlayerName,ProID,PlayerLevel from PlayerInfo where PlayerID !={0} and PlayerLevel>10 order by newid()",BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText);
		//strSql=string.Format ("select PlayerID,PlayerName,ProID,PlayerLevel from PlayerInfo where PlayerID !={0} and PlayerLevel>10 order by rand() limit 20",BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText);
        //StartCoroutine(BtnClick(ytRandom, strSql, "决  斗"));
		InRoom.GetInRoomInstantiate ().GetRandomPlayer (20,10,ytRandom);
		 for (int i = 0; i < listPVP1.Count; i++)
        {
            listPVP1[i].gameObject.SetActiveRecursively(false);
        }
		listTabel.SetZore ();
        while (ytRandom.IsUpdate)
        {
            yield return new WaitForSeconds(0.1f);
        }

        listTabel.SetFrist(ytRandom, SetPVP1List, maxListNum);
    }

    private yuan.YuanMemoryDB.YuanTable ytRevenge = new yuan.YuanMemoryDB.YuanTable("offlineplayerRevenge", "");
	string[] strRows=new string[]{"PlayerID","PlayerName","ProID","PlayerLevel"};
    public IEnumerator BtnRevengeClick()
    {
        for (int i = 0; i < listPVP1.Count; i++)
        {
            listPVP1[i].gameObject.SetActiveRecursively(false);
        }
		listTabel.SetZore();
        string[] strPlayer = BtnGameManager.yt[0]["pvp1BeInfo"].YuanColumnText.Split(';');
        if (BtnGameManager.yt[0]["pvp1BeInfo"].YuanColumnText != "" && strPlayer.Length > 0)
        {
            List<string> listPlayer = new List<string>();
			List<string> listPlayerMemery=new List<string>();
            listPlayer.CopyTo(strPlayer);
            for (int i = 0; i < strPlayer.Length; i++)
            {
                string[] tempPlayer = strPlayer[i].Split(',');
                if (strPlayer[i] != "" && tempPlayer[2] != "2")
                {
					if(!listPlayer.Contains (tempPlayer[0]))
					{
                    	listPlayer.Add(tempPlayer[0]);
						listPlayerMemery.Add (strPlayer[i]);
					}
                }
            }
            for (int i = 0; i < strPlayer.Length; i++)
            {
                string[] tempPlayer = strPlayer[i].Split(',');
                if (strPlayer[i] != "" && !listPlayer.Contains(strPlayer[i]))
                {
					if(!listPlayer.Contains (tempPlayer[0]))
					{
                    	listPlayer.Add(tempPlayer[0]);
						listPlayerMemery.Add (strPlayer[i]);
					}
                }
            }
            if (listPlayer.Count > 0)
            {
                //System.Text.StringBuilder strSqlFirend = new System.Text.StringBuilder();
                //strSqlFirend.Append("Select PlayerID,PlayerName,ProID,PlayerLevel from PlayerInfo where ");
                //for (int i = 0; i < listPlayer.Count; i++)
                //{
				//
                //    if (i != 0 && listPlayer[i] != "")
                //    {
                //        strSqlFirend.AppendFormat(" or PlayerID={0}", listPlayer[i].Split(',')[0]);
                //    }
                //    else if (listPlayer[i] != "")
                //    {
                //        strSqlFirend.AppendFormat("PlayerID={0}", listPlayer[i].Split(',')[0]);
                //    }
                //}
				
				//InRoom.GetInRoomInstantiate ().GetPlayerList(listPlayer.ToArray (),ytRevenge,"DarkSword2","PlayerInfo");
				InRoom.GetInRoomInstantiate ().GetTablesSomeForIDs (listPlayer.ToArray (),strRows,yuan.YuanPhoton.TableType.PlayerInfo,ytRevenge);
                while (ytRevenge.IsUpdate)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                List<yuan.YuanMemoryDB.YuanRow> listRow = new List<yuan.YuanMemoryDB.YuanRow>();
                //try
                //{
                    for (int i = 0; i < listPlayerMemery.Count; i++)
                    {
                        yuan.YuanMemoryDB.YuanRow tempRow = ytRevenge.SelectRowEqual("PlayerID", listPlayerMemery[i].Split(',')[0]);
                       
                        if (tempRow != null)
                        {
                            tempRow = tempRow.CopyTo();
							tempRow.Add ("pvp1BeInfo",listPlayerMemery[i]);
							//tempRow["pvp1BeInfo"].YuanColumnText=listPlayerMemery[i];
                            //tempRow.Add("pvp1BeInfo", listPlayer[i]);
                            listRow.Add(tempRow);
                        }
                    }
                    ytRevenge.Rows = listRow;
                    listTabel.SetFrist(ytRevenge, SetPVP1RevengeList, maxListNum);
                //}
                //catch (System.Exception ex)
                //{
                //    Debug.LogWarning(ex.ToString());
                //}
            }

        }
    }

    private List<BtnPlayerForTeam> listPVP1 = new List<BtnPlayerForTeam>();
    public IEnumerator BtnClick(yuan.YuanMemoryDB.YuanTable mYt, string[] mSql,params string[] mBtnName)
    {
        for (int i = 0; i < listPVP1.Count; i++)
        {
            listPVP1[i].gameObject.SetActiveRecursively(false);
        }
		listTabel.SetZore ();
		//InRoom.GetInRoomInstantiate().GetPlayerList(mSql,mYt,"DarkSword2","PlayerInfo");
		InRoom.GetInRoomInstantiate ().GetTablesSomeForIDs (mSql,strRows,yuan.YuanPhoton.TableType.PlayerInfo,mYt);
        while (mYt.IsUpdate)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //try
        //{
        //    for (int i = 0; i < mYt.Rows.Count; i++)
        //    {
        //        //if(mYt.Rows)
        //        if()
        //    }
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogWarning(ex.ToString());
        //}

        listTabel.SetFrist(mYt, SetPVP1List, maxListNum);

    }
	


    public void SetPVP1List(yuan.YuanMemoryDB.YuanTable mYt)
    {
		for(int i=0;i<listPVP1.Count;i++)
		{
			listPVP1[i].gameObject.SetActiveRecursively (false);
		}
        for (int i = 0; i < mYt.Rows.Count; i++)
        {
            if (listPVP1.Count > i)
            {
                listPVP1[i].playerID = mYt.Rows[i]["PlayerID"].YuanColumnText;
                listPVP1[i].lblPlayerName.text = mYt.Rows[i]["PlayerName"].YuanColumnText;
                
                listPVP1[i].lblPlayerLevel.text = "Lv" + mYt.Rows[i]["PlayerLevel"].YuanColumnText;
                listPVP1[i].picPlayer.atlas = PanelStatic.StaticYuanPicManger.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText) - 1].atlas;
                listPVP1[i].picPlayer.spriteName = PanelStatic.StaticYuanPicManger.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText) - 1].spriteName;
                listPVP1[i].btnAddLable.text = StaticLoc.Loc.Get("info338")+"";
                listPVP1[i].lblPlayerPro.text = RefreshList.GetPro(mYt.Rows[i]["ProID"].YuanColumnText);
                
                if (myGrid.gameObject.active)
                {
                    listPVP1[i].gameObject.SetActiveRecursively(true);
                    listPVP1[i].picNew.gameObject.active = false;
                }
            }
            else
            {
                BtnPlayerForTeam tempPlayer = (BtnPlayerForTeam)Instantiate(btnPlayerForOnePVP);
                tempPlayer.GetComponent<UIToggle>().group = 7;
                tempPlayer.transform.parent = myGrid.transform;
                tempPlayer.transform.localPosition = Vector3.zero;
                tempPlayer.transform.localScale = new Vector3(1, 1, 1);
                tempPlayer.btnAddMessage.target = this.gameObject;
                tempPlayer.btnAddMessage.functionName = "BtnPVP1";
                tempPlayer.btnAddLable.text = StaticLoc.Loc.Get("info338")+"";
                tempPlayer.lblPlayerPro.text = RefreshList.GetPro(mYt.Rows[i]["ProID"].YuanColumnText);
                tempPlayer.picNew.gameObject.active = false;

                tempPlayer.playerID = mYt.Rows[i]["PlayerID"].YuanColumnText;
				if(mYt.Rows[i]["PlayerID"].YuanColumnText=="-10"){
					PanelStatic.StaticBtnGameManager.pvpButtonTran = tempPlayer.btnAddMessage.gameObject.transform;
					PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SetCourseStepObject" , tempPlayer.btnAddMessage.gameObject.transform , SendMessageOptions.DontRequireReceiver);
					tempPlayer.picNew.gameObject.active = false;
					tempPlayer.btnAddMessage.functionName = "RobotytShaDow";
				}
                tempPlayer.lblPlayerName.text = mYt.Rows[i]["PlayerName"].YuanColumnText;
                
                tempPlayer.picPlayer.atlas = PanelStatic.StaticYuanPicManger.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText) - 1].atlas;
                tempPlayer.picPlayer.spriteName = PanelStatic.StaticYuanPicManger.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText) - 1].spriteName;
                tempPlayer.lblPlayerLevel.text = "Lv" + mYt.Rows[i]["PlayerLevel"].YuanColumnText;
                listPVP1.Add(tempPlayer);
                if (!myGrid.gameObject.active)
                {
                    tempPlayer.gameObject.SetActiveRecursively(false);
                }
            }
            myGrid.repositionNow = true;
        }
    }

	public yuan.YuanMemoryDB.YuanTable robotyt;

    public void SetPVP1RevengeList(yuan.YuanMemoryDB.YuanTable mYt)
    {
		for(int i=0;i<listPVP1.Count;i++)
		{
			listPVP1[i].gameObject.SetActiveRecursively (false);
		}
        for (int i = 0; i < mYt.Rows.Count; i++)
        {
            if (listPVP1.Count > i)
            {
                listPVP1[i].playerID = mYt.Rows[i]["PlayerID"].YuanColumnText;
                listPVP1[i].lblPlayerName.text = mYt.Rows[i]["PlayerName"].YuanColumnText;
				listPVP1[i].yr=mYt.Rows[i];
                listPVP1[i].lblPlayerLevel.text = "Lv" + mYt.Rows[i]["PlayerLevel"].YuanColumnText;
                listPVP1[i].picPlayer.atlas = PanelStatic.StaticYuanPicManger.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText) - 1].atlas;
                listPVP1[i].picPlayer.spriteName = PanelStatic.StaticYuanPicManger.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText) - 1].spriteName;

                string[] bePVPInfo = mYt.Rows[i]["pvp1BeInfo"].YuanColumnText.Split(',');


                if (bePVPInfo[1].Trim() == "0")
                {
                    listPVP1[i].lblPlayerPro.text = string.Format("{0}[00ff00]{1}", System.DateTime.Parse(bePVPInfo[3]).ToShortTimeString() , StaticLoc.Loc.Get("info339"));
                }
                else
                {
                    listPVP1[i].lblPlayerPro.text = string.Format("{0}[ff0000]{1}", System.DateTime.Parse(bePVPInfo[3]).ToShortTimeString() , StaticLoc.Loc.Get("info339"));
                }

                
                //listPVP1[i].lblPlayerPro.text = RefreshList.GetPro(mYt.Rows[i]["ProID"].YuanColumnText);

                if (myGrid.gameObject.active)
                {
                    listPVP1[i].gameObject.SetActiveRecursively(true);
                    if (bePVPInfo[2].Trim() == "2")
                    {
                        listPVP1[i].btnAddLable.text = StaticLoc.Loc.Get("info341");
                        listPVP1[i].picNew.gameObject.active = false;
                    }
                    else
                    {
                        listPVP1[i].btnAddLable.text = StaticLoc.Loc.Get("info342");
                        listPVP1[i].picNew.gameObject.active = true;
                    }
                }
                else
                {
                    listPVP1[i].gameObject.SetActiveRecursively(false);
                }
            }
            else
            {
                BtnPlayerForTeam tempPlayer = (BtnPlayerForTeam)Instantiate(btnPlayerForOnePVP);
                tempPlayer.GetComponent<UIToggle>().group = 7;
                tempPlayer.transform.parent = myGrid.transform;
                tempPlayer.transform.localPosition = Vector3.zero;
                tempPlayer.transform.localScale = new Vector3(1, 1, 1);
                tempPlayer.btnAddMessage.target = this.gameObject;
                tempPlayer.btnAddMessage.functionName = "BtnPVP1";
                tempPlayer.btnAddLable.text = StaticLoc.Loc.Get("info338");
                tempPlayer.lblPlayerPro.text = RefreshList.GetPro(mYt.Rows[i]["ProID"].YuanColumnText);
                tempPlayer.playerID = mYt.Rows[i]["PlayerID"].YuanColumnText;
                tempPlayer.lblPlayerName.text = mYt.Rows[i]["PlayerName"].YuanColumnText;
                tempPlayer.picPlayer.atlas = PanelStatic.StaticYuanPicManger.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText) - 1].atlas;
                tempPlayer.picPlayer.spriteName = PanelStatic.StaticYuanPicManger.picPlayer[int.Parse(mYt.Rows[i]["ProID"].YuanColumnText) - 1].spriteName;
                tempPlayer.lblPlayerLevel.text = "Lv" + mYt.Rows[i]["PlayerLevel"].YuanColumnText;
				tempPlayer.yr=mYt.Rows[i];

                string[] bePVPInfo = mYt.Rows[i]["pvp1BeInfo"].YuanColumnText.Split(',');
                if (bePVPInfo[2].Trim() == "2")
                {
                    tempPlayer.btnAddLable.text = StaticLoc.Loc.Get("info343");
                    tempPlayer.picNew.gameObject.active = false;
                }
                else
                {
                    tempPlayer.btnAddLable.text = StaticLoc.Loc.Get("info342");
                    tempPlayer.picNew.gameObject.active = true;

                }

                if (bePVPInfo[1].Trim() == "0")
                {
                    tempPlayer.lblPlayerPro.text = string.Format("{0}[00ff00]{1}", System.DateTime.Parse(bePVPInfo[3]).ToShortTimeString() , StaticLoc.Loc.Get("info339"));
                }
                else
                {
                    tempPlayer.lblPlayerPro.text = string.Format("{0}[ff0000]{1}", System.DateTime.Parse(bePVPInfo[3]).ToShortTimeString() , StaticLoc.Loc.Get("info340"));
                }


                listPVP1.Add(tempPlayer);
                if (!myGrid.gameObject.active)
                {
                    tempPlayer.gameObject.SetActiveRecursively(false);
                }
            }
            myGrid.repositionNow = true;
        }
    }

    /// <summary>
    /// 挑战列表中玩家
    /// </summary>
    /// <param name="mObj"></param>
    public void BtnPVP1(GameObject mObj)
    {

        BtnPlayerForTeam team = mObj.transform.parent.GetComponent<BtnPlayerForTeam>();
        InRoom.GetInRoomInstantiate().PVP1Invite(team.playerID);
		if(team.yr.ContainsKey ("pvp1BeInfo"))
		{
                string[] bePVPInfo = team.yr["pvp1BeInfo"].YuanColumnText.Split(',');
                if (bePVPInfo[2].Trim() != "2")
                {
                    BtnGameManager.yt.Rows[0]["pvp1BeInfo"].YuanColumnText=BtnGameManager.yt.Rows[0]["pvp1BeInfo"].YuanColumnText.Replace (team.yr["pvp1BeInfo"].YuanColumnText,string.Format ("{0},{1},{2},{3}",bePVPInfo[0],bePVPInfo[1],"2",bePVPInfo[3]));
                }			
		}
    }
	
	
	
	public void show0(){
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show0" , SendMessageOptions.DontRequireReceiver);
	}
}
