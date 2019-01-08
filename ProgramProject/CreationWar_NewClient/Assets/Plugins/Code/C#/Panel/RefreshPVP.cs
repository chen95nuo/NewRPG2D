using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefreshPVP : MonoBehaviour
{

    public UILabel lblPoint;
    public YuanPicManager yuanPicManager;
    public UILabel lblNull2;
    public UILabel lblNull4;
    public GameObject objPVP2;
    public GameObject objPVP4;
    public UILabel lblInPVP;

    public UILabel lblCPlayerChallengeNum;
    public UILabel lblCPlayerLevel;
    public UILabel lblCPlayerName;
    public UILabel lblCPlayerPower;
    public UILabel lblCPlayerPro;
    public UILabel lblCPlayerRanking;
    public UILabel lblCPlayerTitle;
    public UISprite picCHead;
	public UILabel lblCPlayerNote;

    public UILabel lblBPlayerdedication;
    public UILabel lblBPlayerMilitary;
    public UILabel lblBPlayerName;
    public UILabel lblBPlayerNowLegion;
    public UILabel lblBPlayerPost;
    public UILabel lblBPlayerRanking;
    public UISprite picBHead;
	public UILabel lblBPlayerNote;

    public UILabel lblListsPoint;
    public UILabel lblCeiling;
    public UILabel lblL2Fail;
    public UILabel lblL2Game;
    public UILabel lblL2ListsLevel;
    public UILabel lblL2ListsRanking;
    public UILabel lblL2Victory;
    public UILabel lblL2Winning;

    public UILabel lblL4Fail;
    public UILabel lblL4Game;
    public UILabel lblL4ListsLevel;
    public UILabel lblL4ListsRanking;
    public UILabel lblL4Victory;
    public UILabel lblL4Winning;

    public UIButton btnCreatePVP1;
    public UIButton btnRemovePVP1;
    public UIButton btnCreatePVP8;
    public UIButton btnRemovePVP8;

	public static int PVPRankNum = 0;

    private yuan.YuanMemoryDB.YuanTable ytCrop = new yuan.YuanMemoryDB.YuanTable("mCrop", "id");
    private yuan.YuanMemoryDB.YuanRow yrPVP2;
    private yuan.YuanMemoryDB.YuanRow yrPVP4;
    public enum PVPType
    {
        Battlefield,
        Colosseum,
        Lists,
    }

    public PVPType pvpType;
    // Use this for initialization
    void Start()
    {
        
    }
	void Awake()
	{
		yuanPicManager = PanelStatic.StaticYuanPicManger;
		//ytRanking=new yuan.YuanMemoryDB.YuanTable("ytPlayerRanking"+this.gameObject.name,"");
		ytRanking=new YuanRank("ytPlayerRanking"+this.gameObject.name);
	}

    void OnEnable()
    {
		InRoom.GetInRoomInstantiate().GetRankByMe(yuan.YuanPhoton.RankingType.Abattoir, BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, "ColosseumPointrank");
        if (btnCreatePVP1 != null)
        {
            if (BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText.Parse (0)>= 10)
            {
                btnCreatePVP1.isEnabled = true;
                btnRemovePVP1.isEnabled = true;
            }
            else
            {
               
                btnCreatePVP1.isEnabled = false;
                btnRemovePVP1.isEnabled = false;
            }
			if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.PVPSwitch)!="1")
			{
				btnCreatePVP1.gameObject.SetActiveRecursively (false);
				btnRemovePVP1.gameObject.SetActiveRecursively (false);
			}
        }
        if (btnCreatePVP8 != null)
        {
            if (BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText.Parse (0) >= 30)
            {
                btnCreatePVP8.isEnabled = true;
                btnRemovePVP8.isEnabled = true;
           
            }
            else
            {
                btnCreatePVP8.isEnabled = false;
                btnRemovePVP8.isEnabled = false;
            }
			if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.PVPSwitch)!="1")
			{
				btnCreatePVP8.gameObject.SetActiveRecursively (false);
				btnRemovePVP8.gameObject.SetActiveRecursively (false);
			}			
			
        }
        lblPoint.text = BtnGameManager.yt.Rows[0]["PVPPoint"].YuanColumnText;
        StartCoroutine(ReadInfo());
    }
	
	private YuanRank ytRanking;
	private string sqlMy=string.Empty;
    private IEnumerator ReadInfo()
    {
        switch (pvpType)
        {
            case PVPType.Colosseum:
                lblNull2.gameObject.active = false;
                lblNull4.gameObject.active = false;
				lblCPlayerChallengeNum.text =(10- int.Parse (BtnGameManager.yt.Rows[0]["PVPTimes"].YuanColumnText)).ToString(); 
                lblCPlayerLevel.text =InRoom.isUpdatePlayerLevel?InRoom.playerLevel: BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText;
                lblCPlayerName.text = BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText;
                lblCPlayerPower.text = BtnGameManager.yt.Rows[0]["ColosseumFighting"].YuanColumnText;
                lblCPlayerPro.text = RefreshList.GetPro(BtnGameManager.yt.Rows[0]["ProID"].YuanColumnText);
                //lblCPlayerRanking.text = BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText;
                lblCPlayerTitle.text = BtnGameManager.yt.Rows[0]["SelectTitle"].YuanColumnText;
				picCHead.atlas=yuanPicManager.picPlayer[BtnGameManager.yt.Rows[0]["ProID"].YuanColumnText.Parse (0) - 1].atlas;
                picCHead.spriteName = yuanPicManager.picPlayer[BtnGameManager.yt.Rows[0]["ProID"].YuanColumnText.Parse (0) - 1].spriteName;
                if (BtnGameManagerBack.isInPVPOne)
                {
					if(lblInPVP!=null)
					{
	                    lblInPVP.gameObject.SetActiveRecursively(true);
					}
                }
                else
                {
					if(lblInPVP!=null)
					{
	                    lblInPVP.gameObject.SetActiveRecursively(false);
					}
                }
				SetNoteString ("PVP1Info",lblCPlayerNote);
				lblCPlayerRanking.text = PVPRankNum.ToString ();
				//sqlMy=string.Format ("Select (Select count(*) from {3} where a.{0}<{0} or (a.{0}={0} and a.PlayerID<PlayerID)) as ROW_NUMBER,a.{0},a.{1},a.PlayerID from {3} a where PlayerID={2} order by ROW_NUMBER", "ColosseumPoint", "PlayerName", BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, "PlayerInfo");
			    //InRoom.GetInRoomInstantiate().GetYuanTable(sqlMy, "DarkSword2", ytRanking);
				InRoom.GetInRoomInstantiate ().GetRankOne (yuan.YuanPhoton.RankingType.Abattoir,BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,ytRanking);
				while(ytRanking.IsUpdate)
				{
					yield return new WaitForSeconds(0.1f);
				}
//				lblCPlayerRanking.text = ytRanking.myRank.ToString ();
			
                break;
            case PVPType.Battlefield:
                lblNull2.gameObject.active = false;
                lblNull4.gameObject.active = false;
                lblBPlayerdedication.text = BtnGameManager.yt.Rows[0]["PVPPoint"].YuanColumnText.Parse (0).ToString ();
                lblBPlayerMilitary.text = BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText;
                lblBPlayerName.text = BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText;
				picBHead.atlas = yuanPicManager.picPlayer[BtnGameManager.yt.Rows[0]["ProID"].YuanColumnText.Parse (0) - 1].atlas;
                picBHead.spriteName = yuanPicManager.picPlayer[BtnGameManager.yt.Rows[0]["ProID"].YuanColumnText.Parse (0) - 1].spriteName;
                // lblBPlayerNowLegion.text = BtnGameManager.yt.Rows[0][""].YuanColumnText;
                //lblBPlayerPost.text = BtnGameManager.yt.Rows[0][""].YuanColumnText;
                if (BtnGameManagerBack.isInLegion)
                {
					if(lblInPVP!=null)
				{
                    lblInPVP.gameObject.SetActiveRecursively(true);
				}
                }
                else
                {
					if(lblInPVP!=null)
				{
                    lblInPVP.gameObject.SetActiveRecursively(false);
				}
                }

				SetPVP8NoteString("PVP8Info",lblBPlayerNote);
				//sqlMy=string.Format ("Select (Select count(*) from {3} where a.{0}<{0} or (a.{0}={0} and a.PlayerID<PlayerID)) as ROW_NUMBER,a.{0},a.{1},a.PlayerID from {3} a where PlayerID={2} order by ROW_NUMBER", "BattlefieldDedication", "PlayerName", BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, "PlayerInfo");
			    //InRoom.GetInRoomInstantiate().GetYuanTable(sqlMy, "DarkSword2", ytRanking);
				InRoom.GetInRoomInstantiate ().GetRankOne (yuan.YuanPhoton.RankingType.Rank,BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,ytRanking);
				while(ytRanking.IsUpdate)
				{
					yield return new WaitForSeconds(0.1f);
				}
				lblBPlayerRanking.text = PVPRankNum.ToString ();
			
                break;
            case PVPType.Lists:
                
                lblNull2.gameObject.active = false;
                lblNull4.gameObject.active = false;
                lblListsPoint.text = BtnGameManager.yt.Rows[0]["ListsPoint"].YuanColumnText;
                lblCeiling.text = BtnGameManager.yt.Rows[0]["ListsCeiling"].YuanColumnText;
                if (BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText != "" || BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText != "")
                {
                    if (BtnGameManagerBack.isInPVP)
                    {
						if(lblInPVP!=null)
				{
                        lblInPVP.gameObject.SetActiveRecursively(true);
					}
                    }
                    else
                    {
						if(lblInPVP!=null)
						{
                        	lblInPVP.gameObject.SetActiveRecursively(false);
						}
                    }
                    //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select * from Corps where id ='{0}' or id='{1}'", BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText, BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText), "DarkSword2", ytCrop);
                    InRoom.GetInRoomInstantiate ().GetTableForID (BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText,yuan.YuanPhoton.TableType.Corps,ytCrop);
					while (ytCrop.IsUpdate)
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    //Debug.Log("-------------------------------yyyyyyyyyyyyyyyyyyyyyyyyy:" + yuan.YuanPhoton.CorpType.PVP2.ToString());
                    if (ytCrop.Rows.Count > 0)
                    {

                        yrPVP2 = ytCrop.SelectRowEqual("PlayerNumber", "2");
                        yrPVP4 = ytCrop.SelectRowEqual("PlayerNumber", "4");

                        if (yrPVP2 != null)
                        {

                            lblL2Fail.text = yrPVP2["FailNum"].YuanColumnText==""?"0":yrPVP2["FailNum"].YuanColumnText;
                            lblL2Game.text = (yrPVP2["FailNum"].YuanColumnText.Parse (0) + yrPVP2["VictoryNum"].YuanColumnText.Parse (0)).ToString();
                            lblL2Victory.text = yrPVP2["VictoryNum"].YuanColumnText==""?"0":yrPVP2["VictoryNum"].YuanColumnText;
							if( lblL2Game.text.Parse (0) ==0)
							{
								lblL2Winning.text="0%";
							}
							else
							{
								lblL2Winning.text = (yrPVP2["VictoryNum"].YuanColumnText.Parse (0) / lblL2Game.text.Parse (0) * 100).ToString() + "%";
							}
                            
                            lblL2ListsLevel.text = yrPVP2["SelfLevel"].YuanColumnText;
                            //lblL2ListsRanking.text = yrPVP2["Ranking"].YuanColumnText;
                            lblL2ListsRanking.text = BtnGameManager.yt[0]["Rank"].YuanColumnText;
                        }
                        else
                        {
                            lblNull2.gameObject.active = true;
                            objPVP2.SetActiveRecursively(false);
                        }

                        if (yrPVP4 != null)
                        {
                            lblL4Fail.text = yrPVP4["FailNum"].YuanColumnText==""?"0":yrPVP4["FailNum"].YuanColumnText;
                            lblL4Game.text = (yrPVP4["FailNum"].YuanColumnText.Parse (0) + yrPVP4["VictoryNum"].YuanColumnText.Parse (0)).ToString();
                            lblL4Victory.text = yrPVP4["VictoryNum"].YuanColumnText==""?"0":yrPVP4["VictoryNum"].YuanColumnText;
						
							if( lblL4Game.text.Parse (0) ==0)
							{
								lblL4Winning.text="0%";
							}
							else
							{
								 lblL4Winning.text = (yrPVP4["VictoryNum"].YuanColumnText.Parse (0) / lblL2Game.text.Parse (0) * 100).ToString() + "%";
							}						
                           
                            lblL4ListsLevel.text = yrPVP4["SelfLevel"].YuanColumnText;
                            //lblL4ListsRanking.text = yrPVP4["Ranking"].YuanColumnText;
                            lblL4ListsRanking.text = BtnGameManager.yt[0]["Rank"].YuanColumnText;
                        }
                        else
                        {
                            lblNull4.gameObject.active = true;
                            objPVP4.SetActiveRecursively(false);
                        }
                    }
                    else
                    {
                        lblNull2.gameObject.active = true;
                        lblNull4.gameObject.active = true;
                        this.gameObject.SetActiveRecursively(false);
                    }
                }
                else
                {
                    lblNull2.gameObject.active = true;
                    this.gameObject.SetActiveRecursively(false);
                }

                break;
        }
    }
	
	private System.Text.StringBuilder sbNote=new System.Text.StringBuilder();
	//private List<string> listNote=new List<string>();
	private string strDay=string.Empty;
	private string strTime=string.Empty;
	private string strOver=string.Empty;
	private System.TimeSpan tsNote;
	private void SetNoteString(string mName,UILabel mlbl)
	{
		string[] listText=BtnGameManager.yt[0][mName].YuanColumnText.Split (';');
		//listNote.CopyTo (listText);
		sbNote.Length=0;
		for(int i=0;i<listText.Length;i++)
		{
			if(!string.IsNullOrEmpty (listText[i]))
			{
				string[] tempList=listText[i].Split (',');
				if(tempList.Length==3)
				{
					tsNote=InRoom.GetInRoomInstantiate ().serverTime-System.DateTime.Parse (tempList[0]);
					if(tsNote.Days>2)
					{
						listText[i]="";
						continue;
					}
					else if(tsNote.Days==0)
					{
						strDay=StaticLoc.Loc.Get("info588");
					}
					else if(tsNote.Days==1)
					{
						strDay=StaticLoc.Loc.Get("info589");
					}
					else if(tsNote.Days==2)
					{
						strDay=StaticLoc.Loc.Get("info590");
					}
					
					if(tempList[1]=="0")
					{
						strOver=StaticLoc.Loc.Get("buttons186");
					}
					else if(tempList[1]=="1")
					{
						strOver=StaticLoc.Loc.Get("buttons187");
					}
					
					strTime=System.DateTime.Parse (tempList[0]).ToShortTimeString();
					sbNote.AppendFormat ("{0}{1} {2} {3} {4}\n",strDay,strTime,StaticLoc.Loc.Get("info591"),tempList[2],strOver);
				}
			}
		}
		mlbl.text=sbNote.ToString ();
		
		sbNote.Length=0;
		for(int j=0;j<listText.Length;j++)
		{
			if(listText[j]!="")
			{
				sbNote.AppendFormat ("{0};",listText[j]);
			}
		}
		BtnGameManager.yt[0][mName].YuanColumnText=sbNote.ToString ();
		
	}
	
	private void SetPVP8NoteString(string mName,UILabel mlbl)
	{
		string[] listText=BtnGameManager.yt[0][mName].YuanColumnText.Split (';');
		//listNote.CopyTo (listText);
		sbNote.Length=0;
		for(int i=0;i<listText.Length;i++)
		{
	//		Debug.Log("ran================================================="+listText[i]);
			if(!string.IsNullOrEmpty (listText[i]))
			{
				string[] tempList=listText[i].Split (',');
				if(tempList.Length==3)
				{
					tsNote=InRoom.GetInRoomInstantiate ().serverTime-System.DateTime.Parse (tempList[0]);
					if(tsNote.Days>2)
					{
						listText[i]="";
						continue;
					}
					else if(tsNote.Days==0)
					{
						strDay=StaticLoc.Loc.Get("info588");
					}
					else if(tsNote.Days==1)
					{
						strDay=StaticLoc.Loc.Get("info589");
					}
					else if(tsNote.Days==2)
					{
						strDay=StaticLoc.Loc.Get("info590");
					}
					
					if(((PVP8InfoType)tempList[1].Parse(0))==PVP8InfoType.Boss)
					{
						
						strOver=StaticLoc.Loc.Get("info595");
					}
					else if(((PVP8InfoType)tempList[1].Parse(0))==PVP8InfoType.Flag)
					{
						strOver=StaticLoc.Loc.Get("info594");
					}
					else if(((PVP8InfoType)tempList[1].Parse(0))==PVP8InfoType.Kill)
					{
						strOver=StaticLoc.Loc.Get("info592");
					}
					else if(((PVP8InfoType)tempList[1].Parse(0))==PVP8InfoType.Tower)
					{
						strOver=StaticLoc.Loc.Get("info593");
					}
					
					strTime=System.DateTime.Parse (tempList[0]).ToShortTimeString();
					sbNote.AppendFormat ("{0}{1} {2} {3} {4}{5}\n",strDay,strTime,strOver,StaticLoc.Loc.Get("tips058"),tempList[2],StaticLoc.Loc.Get("messages059"));
				}
			}
		}
		mlbl.text=sbNote.ToString ();
		
		sbNote.Length=0;
		for(int j=0;j<listText.Length;j++)
		{
			if(listText[j]!="")
			{
				sbNote.AppendFormat ("{0};",listText[j]);
			}
		}
		BtnGameManager.yt[0][mName].YuanColumnText=sbNote.ToString ();
		
	}


}

public enum PVP8InfoType:int
{
	Kill=0,
	Tower,
	Flag,
	Boss,
}
