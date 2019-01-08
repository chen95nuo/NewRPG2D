using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefreshList : MonoBehaviour {


    public string rowName;
    public string rowNameList;
    public ListType listType = ListType.Firends;
    public GameObject btnPlayerForTeam;
    public YuanPicManager yuanPicManager;
    public GameObject loading;
    public PlayerInfo playerInfo;
    public UIInput txtMailAddressee;
    public PanelListTabel listTable;
    public int listMaxNum = 4;  //  列表最大容量
	private UIGrid myGrid;
    //public GameObject YuanBtnManager;

    private yuan.YuanMemoryDB.YuanTable ytPlayerList ;
    private yuan.YuanMemoryDB.YuanTable ytPlayerLegionList;
    private bool isLegionStart = false;
    private TeamType teamType;

    public static RefreshList refreshList;
	string[] strRows=new string[]{"PlayerID","PlayerName","ProID","PlayerLevel","Prestige","VSRanking","GuildID","Place","GuildPosition","GuildName"};
    void Awake()
    {
        ytPlayerList = new yuan.YuanMemoryDB.YuanTable(rowNameList, "PlayerID");
        ytPlayerLegionList = new yuan.YuanMemoryDB.YuanTable("LegionList", "");
        ytIDs = new yuan.YuanMemoryDB.YuanTable(rowNameList + "IDs", "");
    }
	// Use this for initialization
	void Start () {
        yuanPicManager = PanelStatic.StaticYuanPicManger;
        loading.SetActiveRecursively(false);
        myGrid=GetComponent<UIGrid>();
        refreshList = this;
	}

    public enum ListType
    {
        Firends,
        Team,
        Guild,
        PVP2,
        PVP4,
    }
	
	public GameObject lblNull;
//	void OnDisable()
//	{
//		lblNull.gameObject.active=false;
//	}

    void OnEnable()
    {
        //Debug.Log("00000000000000000000000000000000000000000000");
		StartCoroutine (YuanOnEnable ());
    }
	public void RefreshMe()
	{
		StartCoroutine (YuanOnEnable ());
	}

    void RecevieMsgRefresh()
    {
        StopCoroutine("YuanOnEnable");
        if (listType == ListType.Guild)
        {
            StopCoroutine("RefreshGuild");
        }
        else if (listType == ListType.PVP2 && BtnGameManager.yt != null && BtnGameManager.yt.Rows.Count > 0)
        {
            StopCoroutine("RefreshPVP2");
        }
        else if (listType == ListType.PVP4 && BtnGameManager.yt != null && BtnGameManager.yt.Rows.Count > 0)
        {
            StopCoroutine("RefreshPVP4");
        }
        //Debug.Log("11111111111111111111111111111111111111111111111111111111111");
        StartCoroutine(YuanOnEnable());
    }
	
	public IEnumerator YuanOnEnable()
	{
        foreach (BtnPlayerForTeam item in listBtn)
        {
            item.gameObject.SetActiveRecursively(false);
        }
		yield return new WaitForSeconds(1);
		 lblNull.gameObject.active = false;
        if ((listType==ListType.Firends)&& BtnGameManager.yt != null&&BtnGameManager.yt.Rows.Count>0)
        {
			if(BtnGameManager.yt.Rows[0][rowName].YuanColumnText.Trim()!="")
			{
            	Refresh(BtnGameManager.yt.Rows[0][rowName].YuanColumnText.Trim().Split(';'));
			}
			else
			{
				
			
				ytPlayerList.Clear ();
				 foreach (BtnPlayerForTeam item in listBtn)
		        {
		            item.gameObject.SetActiveRecursively(false);
		        }
				//RefreshPlayerList(ytPlayerList);
				
			}
        }
        else if (listType == ListType.Team && BtnGameManager.yt != null && BtnGameManager.yt.Rows.Count > 0)
        {
            RefreshTeam();
        }
        else if (listType == ListType.Guild && BtnGameManager.yt != null && BtnGameManager.yt.Rows.Count > 0)
        {
            StartCoroutine(RefreshGuild());
        }
        else if (listType == ListType.PVP2 && BtnGameManager.yt != null && BtnGameManager.yt.Rows.Count > 0)
        {
            StartCoroutine(RefreshPVP2());
        }
        else if (listType == ListType.PVP4 && BtnGameManager.yt != null && BtnGameManager.yt.Rows.Count > 0)
        {
            StartCoroutine(RefreshPVP4());
        }
	}
	
	// Update is called once per frame

    void Update()
    {
        if (ytPlayerList != null &&
            ytPlayerList.IsUpdate && 
            loading != null && 
            !loading.active)
        {
            loading.SetActiveRecursively(true);
        }
        else if (ytPlayerList != null && 
            !ytPlayerList.IsUpdate &&
            loading != null &&
            loading.active)
        {
            loading.SetActiveRecursively(false);
          
            if (listTable != null)
            {
                listTable.SetFrist(ytPlayerList, RefreshPlayerList, listMaxNum);
            }
			else
			{
				RefreshPlayerList(ytPlayerList);
			}
            //RefreshPlayerList(ytPlayerList);
        }
        else if (ytPlayerLegionList != null &&
           !ytPlayerLegionList.IsUpdate &&
           isLegionStart)
        {
			if(ytPlayerList==null||ytPlayerList.Rows.Count==0)
			{
				 isLegionStart = false;
				if(listTable!=null)
				{
                    listTable.SetFrist(ytPlayerLegionList, RefreshPlayerList, listMaxNum);
				}
				else
				{
					RefreshPlayerList(ytPlayerLegionList);
				}
			}
        }
    }

    public void Refresh(string[] strPlayerID)
    {
        if (strPlayerID.Length > 0)
        {
            //InRoom.GetInRoomInstantiate().GetPlayerList(strPlayerID, ytPlayerList, "DarkSword2", "PlayerInfo");
			InRoom.GetInRoomInstantiate ().GetTablesSomeForIDs (strPlayerID,strRows,yuan.YuanPhoton.TableType.PlayerInfo,ytPlayerList);
        }
    }
	
	 public void RefreshTeam()
    {
        if (ytPlayerList.Count > 0)
        {
            teamType = TeamType.Team;
        }
        else if(ytPlayerLegionList.Count>0)
        {
            teamType = TeamType.Legion;
        }
		else
		{
			teamType = TeamType.Team;
		}
		
		
        playerInfo.teamType = teamType;
		//Debug.Log ("RefreshTeam");
        InRoom.GetInRoomInstantiate().GetMyTeams(ytPlayerList, "DarkSword2", "PlayerInfo");
       InRoom.GetInRoomInstantiate().GetMyLegion(ytPlayerLegionList, "DarkSword2", "PlayerInfo");
        isLegionStart = true;
    }

	public void RefreshGuildPlayer(){
		StartCoroutine(RefreshGuild());
	}

     private yuan.YuanMemoryDB.YuanTable ytIDs;
    public IEnumerator RefreshGuild()
    {
		string mGuildID=BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText;
        if (mGuildID != "")
        {
			//Debug.Log ("------------------------------"+string.Format("select * from GuildInfo where id='{0}'", mGuildID));
            ytIDs.TableName = rowNameList + "IDs";
            //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("select * from GuildInfo where id='{0}'", mGuildID), "DarkSword2", ytIDs);
        	InRoom.GetInRoomInstantiate ().GetTableForID (mGuildID,yuan.YuanPhoton.TableType.GuildInfo,ytIDs);
			
	        while (ytIDs.IsUpdate)
	        {
	           yield return new WaitForSeconds(0.1f);
	        }
	        if(ytIDs!=null&&ytIDs.Rows.Count>0)
	        {
				lblNull.gameObject.active=false;
	            SetPlayerFun();
				string[] tempPlayerID = ytIDs.Rows[0]["MemverID"].YuanColumnText.ToString().Trim().Split (';');
				string[] rowName=new string[]{"PlayerID","PlayerName","ProID","PlayerLevel","Prestige","VSRanking","GuildID","Place","GuildPosition","GuildName"};
				InRoom.GetInRoomInstantiate().GetTablesSomeForIDs(tempPlayerID,rowName,yuan.YuanPhoton.TableType.PlayerInfo,ytPlayerList);
	           // InRoom.GetInRoomInstantiate().GetPlayerList(tempPlayerID, ytPlayerList, "DarkSword2", "PlayerInfo");
	        }
		}
		else
		{
			lblNull.gameObject.active=true;

            InActiveChild();

            //this.transform.parent.gameObject.SetActiveRecursively (false);
		}

    }

    public IEnumerator RefreshPVP2()
    {
        //Debug.Log("222222222222222222222222222222222222222222");
        teamType = TeamType.PVP2;
        playerInfo.teamType = teamType;
        string mID = BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText;
        if (mID != "")
        {
            ytIDs.TableName = rowNameList + "IDs";
            //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("select * from Corps where id='{0}'", mID), "DarkSword2", ytIDs);
			InRoom.GetInRoomInstantiate ().GetTableForID(mID,yuan.YuanPhoton.TableType.Corps,ytIDs);

            while (ytIDs.IsUpdate)
            {
                yield return new WaitForSeconds(0.1f);
            }
            //Debug.Log("33333333333333333333333333333333333333333333333");
            if (ytIDs != null && ytIDs.Rows.Count > 0)
            {
                lblNull.gameObject.active = false;
                //SetPlayerFun();
                string[] tempPlayerID = ytIDs.Rows[0]["MemverID"].YuanColumnText.ToString().Trim().Split(';');
                InRoom.GetInRoomInstantiate().GetPlayerList(tempPlayerID, ytPlayerList, "DarkSword2", "PlayerInfo");
            }
        }
        else
        {
            lblNull.gameObject.active = true;

            InActiveChild();

            //this.transform.parent.gameObject.SetActiveRecursively(false);
        }
    }

    public IEnumerator RefreshPVP4()
    {
        teamType = TeamType.PVP4;
        playerInfo.teamType = teamType;
        string mID = BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText;
        if (mID != "")
        {
            ytIDs.TableName = rowNameList + "IDs";
            //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("select * from Corps where id='{0}'", mID), "DarkSword2", ytIDs);
			InRoom.GetInRoomInstantiate ().GetTableForID (mID,yuan.YuanPhoton.TableType.Corps,ytIDs);

            while (ytIDs.IsUpdate)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (ytIDs != null && ytIDs.Rows.Count > 0)
            {
                lblNull.gameObject.active = false;
                //SetPlayerFun();
                string[] tempPlayerID = ytIDs.Rows[0]["MemverID"].YuanColumnText.ToString().Trim().Split(';');
                InRoom.GetInRoomInstantiate().GetPlayerList(tempPlayerID, ytPlayerList, "DarkSword2", "PlayerInfo");
            }
        }
        else
        {
            lblNull.gameObject.active = true;

            InActiveChild();

            //this.transform.parent.gameObject.SetActiveRecursively(false);
        }
    }



    private List<BtnPlayerForTeam> listBtn = new List<BtnPlayerForTeam>();
    private void RefreshPlayerList(yuan.YuanMemoryDB.YuanTable mYt)
    {
		
        foreach (BtnPlayerForTeam item in listBtn)
        {
            item.gameObject.SetActiveRecursively(false);
        }
        if (mYt.Rows.Count > 0)
		{
			lblNull.gameObject.active=false;
		}
		else
		{
			lblNull.gameObject.active=true;
            //if (listType != ListType.Firends&&listType!=ListType.Team)
            //{
//                this.transform.parent.gameObject.SetActiveRecursively(false);

            InActiveChild();
        
            //}
            if (listType == ListType.Team)
            {
                if (ytPlayerLegionList.Count > 0 || ytPlayerList.Count > 0)
                {
                    lblNull.gameObject.active = false;
                    if (ytPlayerList.Count > 0)
                    {
                        teamType = TeamType.Team;
                    }
                    else if(ytPlayerLegionList.Count>0)
                    {
                        teamType = TeamType.Legion;
                    }
					else
					{
						 teamType = TeamType.Team;
					}
					
                    playerInfo.teamType = teamType;
                }
                else
                {
                    //Debug.Log("+++++++++++++++++++++");
//                    this.transform.parent.gameObject.SetActiveRecursively(false);

                    InActiveChild();

                    foreach (BtnPlayerForTeam item in listBtn)
                    {
                        item.gameObject.SetActiveRecursively(false);
                    }
                }
            }
		}
       
        int num = 0;
        foreach (yuan.YuanMemoryDB.YuanRow item in mYt.Rows)
        {
            UIToggle ckb;
            if (num < listBtn.Count)
            {
                int numPro=int.Parse(item["ProID"].YuanColumnText.Trim())-1;
                listBtn[num].picPlayer.atlas = yuanPicManager.picPlayer[numPro].atlas;
                listBtn[num].picPlayer.spriteName = yuanPicManager.picPlayer[numPro].spriteName;
                listBtn[num].lblPlayerName.text = item["PlayerName"].YuanColumnText.Trim();
                listBtn[num].lblPlayerLevel.text = item["PlayerLevel"].YuanColumnText.Trim();
                listBtn[num].lblPlayerPro.text = GetPro(item["ProID"].YuanColumnText.Trim());

				if(listType == ListType.Guild){

				if(int.Parse(item["GuildPosition"].YuanColumnText.Trim())==1){
				listBtn[num].GuildPosition.text = StaticLoc.Loc.Get("info950");

				}else if(int.Parse(item["GuildPosition"].YuanColumnText.Trim())==2){
				listBtn[num].GuildPosition.text = StaticLoc.Loc.Get("info947");

				}else if(int.Parse(item["GuildPosition"].YuanColumnText.Trim())==3){
				listBtn[num].GuildPosition.text = StaticLoc.Loc.Get("info948");

				}else if(int.Parse(item["GuildPosition"].YuanColumnText.Trim())==4){
				listBtn[num].GuildPosition.text = StaticLoc.Loc.Get("buttons705");

				}else if (int.Parse(item["GuildPosition"].YuanColumnText.Trim())==5){
				listBtn[num].GuildPosition.text = StaticLoc.Loc.Get("info964");

				}else if (int.Parse(item["GuildPosition"].YuanColumnText.Trim())==0){
				listBtn[num].GuildPosition.text = StaticLoc.Loc.Get("info949");
			
				}
				}

                listBtn[num].yr = item;
				
				ckb= listBtn[num].GetComponent<UIToggle>();
				ckb.isChecked=false;

                listBtn[num].gameObject.SetActiveRecursively(true);
            }
            else
            {
                BtnPlayerForTeam btnforTeamTemp = ((GameObject)Instantiate(btnPlayerForTeam)).GetComponent<BtnPlayerForTeam>();
                btnforTeamTemp.GetComponent<UIToggle>().group = 5;
                UIButtonMessage btnMessage = btnforTeamTemp.GetComponent<UIButtonMessage>();
                btnMessage.target = this.gameObject;
                if (txtMailAddressee != null)
                {
                    btnMessage.functionName = "SetMailAddress";
                }
                else
                {
                    btnMessage.functionName = "SetPlayerInfo";
                }
                btnforTeamTemp.yr = item;
                btnforTeamTemp.playerInfo = this.playerInfo;
                btnforTeamTemp.transform.parent = this.transform;
                btnforTeamTemp.transform.localPosition = Vector3.zero;
                btnforTeamTemp.transform.localScale = new Vector3(1, 1, 1);
                int numPro = int.Parse(item["ProID"].YuanColumnText.Trim())-1;
                btnforTeamTemp.picPlayer.atlas = yuanPicManager.picPlayer[numPro].atlas;
                btnforTeamTemp.picPlayer.spriteName = yuanPicManager.picPlayer[numPro].spriteName;
                btnforTeamTemp.lblPlayerName.text = item["PlayerName"].YuanColumnText.Trim();
                btnforTeamTemp.lblPlayerLevel.text = item["PlayerLevel"].YuanColumnText.Trim();
                btnforTeamTemp.lblPlayerPro.text = GetPro(item["ProID"].YuanColumnText.Trim());


				if (listType == ListType.Guild){
				if(int.Parse(item["GuildPosition"].YuanColumnText.Trim())==1){
				btnforTeamTemp.GuildPosition.text = StaticLoc.Loc.Get("info950");
					
				}else if(int.Parse(item["GuildPosition"].YuanColumnText.Trim())==2){
				btnforTeamTemp.GuildPosition.text = StaticLoc.Loc.Get("info947");
					
				}else if(int.Parse(item["GuildPosition"].YuanColumnText.Trim())==3){
				btnforTeamTemp.GuildPosition.text = StaticLoc.Loc.Get("info948");
					
				}else if(int.Parse(item["GuildPosition"].YuanColumnText.Trim())==4){
				btnforTeamTemp.GuildPosition.text = StaticLoc.Loc.Get("buttons705");
					
				}else if (int.Parse(item["GuildPosition"].YuanColumnText.Trim())==5){
				btnforTeamTemp.GuildPosition.text = StaticLoc.Loc.Get("info964");
					
				}else if (int.Parse(item["GuildPosition"].YuanColumnText.Trim())==0){
                    btnforTeamTemp.GuildPosition.text = StaticLoc.Loc.Get("info949");
					
				}
				}
				
				 ckb=btnforTeamTemp.GetComponent<UIToggle>();
				ckb.group=5;

                UIPanel tempPanel = btnforTeamTemp.GetComponent<UIPanel>();
                if (tempPanel != null)
                {
                    Destroy(tempPanel);
                }
                listBtn.Add(btnforTeamTemp);
            }
            if (num == 0)
            {
                ckb.isChecked=true;
            }
            num++;
        }
        myGrid.repositionNow = true;
    }

    void InActiveChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActiveRecursively(false);
        }
    }

    public void SetPlayerInfo(GameObject obj)
    {
        BtnPlayerForTeam btnPlayerTemp = obj.GetComponent<BtnPlayerForTeam>();
        playerInfo.yr = btnPlayerTemp.yr;
        playerInfo.picPlayer.atlas = btnPlayerTemp.picPlayer.atlas;
        playerInfo.picPlayer.spriteName = btnPlayerTemp.picPlayer.spriteName;
        playerInfo.RefreshPlayerInfo();
    }

    public void SetMailAddress(GameObject obj)
    {
        BtnPlayerForTeam btnPlayerTemp = obj.GetComponent<BtnPlayerForTeam>();
        txtMailAddressee.text = btnPlayerTemp.lblPlayerName.text;
        //this.transform.parent.gameObject.SetActiveRecursively(false);
    }

    public GameObject[] listPlayerFunBtn;
    /// <summary>
    /// 设置玩家权限
    /// </summary>
    public void SetPlayerFun()
    {   
        string id = BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText;
        if (ytIDs.Rows[0]["GuildHeadID"].YuanColumnText.IndexOf(id) != -1)
        {
            yuan.YuanClass.SwitchList(listPlayerFunBtn, true, true);
        }
        else if (ytIDs.Rows[0]["GuildDepHeadID"].YuanColumnText.IndexOf(id) != -1)
        {
            yuan.YuanClass.SwitchList(listPlayerFunBtn, true, true);
        }
        else if (ytIDs.Rows[0]["GuildOldManID"].YuanColumnText.IndexOf(id) != -1)
        {
            yuan.YuanClass.SwitchListOnlyThis(listPlayerFunBtn, true, true, 1, 3, 5);
        }
        else if (ytIDs.Rows[0]["GuildElite"].YuanColumnText.IndexOf(id) != -1)
        {
            yuan.YuanClass.SwitchListOnlyThis(listPlayerFunBtn, true, true, 1);
        }
    }

    /// <summary>
    /// 获取职业
    /// </summary>
    /// <param name="proID"></param>
    /// <returns></returns>
    public static string GetPro(string proID)
    {
        string strPro = "";
        switch (proID)
        {
            case "1":
                {
                    strPro = StaticLoc.Loc.Get("buttons820");
                }
                break;
            case "2":
                {
                    strPro =StaticLoc.Loc.Get("buttons821");
                }
                break;
            case "3":
                {
                    strPro = StaticLoc.Loc.Get("buttons822");
                }
                break;

        }
        return strPro;
    }
}
