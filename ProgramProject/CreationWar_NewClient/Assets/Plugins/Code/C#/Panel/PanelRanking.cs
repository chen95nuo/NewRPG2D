using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SortType
{
    asc,
    desc,
}

public class PanelRanking : MonoBehaviour {

    public string rankName = string.Empty;
    public string tableName;
    public string showRowName;
    public SortType sortType;
    public yuan.YuanPhoton.RankingType rankingType;
    //private yuan.YuanMemoryDB.YuanTable yt;
    //private yuan.YuanMemoryDB.YuanTable ytMyRank;
    //private yuan.YuanMemoryDB.YuanTable ytPVP4;
    private YuanRank yt;
    private YuanRank ytPVP4;
    public GameObject loading;
    public UILabel lblMyRank;
    public UILabel lblPVP4;
	
	private bool insYT=false;

    void Awake()
    {
		InsYT();
    }
	
	private void InsYT()
	{
		if(insYT==false)
		{
            //yt = new yuan.YuanMemoryDB.YuanTable(rankName + "rank", "id");
            //ytMyRank = new yuan.YuanMemoryDB.YuanTable("myrank" + rankName, "");
            //ytPVP4 = new yuan.YuanMemoryDB.YuanTable("pvp4" + rankName, "");
            yt = new YuanRank(rankName + "rank");
            ytPVP4 = new YuanRank("pvp4" + rankName);
        loading.SetActiveRecursively(false);
			insYT=true;
		}
	}

    public GameObject btnRanking;
    public UIGrid grid;
    private List<BtnRanking> listBtn = new List<BtnRanking>();



    public IEnumerator SetRanking()
    {

        InsYT();
        loading.SetActiveRecursively(true);
        lblPVP4.gameObject.SetActiveRecursively(false);
        //string sql = string.Format("Select * from GameRanking where Ranking in (Select top 20 Ranking from GameRanking where RankType='{0}' and ApplicationName='{1}' order by Ranking asc)", (byte)rankingType, InRoom.GetInRoomInstantiate().ServerApplication);
        //Debug.Log("sql:" + sql);
        string sqlMy = string.Empty;
        string sql = string.Empty;
        switch (rankingType)
        {
            case yuan.YuanPhoton.RankingType.Arena:
                {
//                    if (BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText == "")
//                    {
						lblMyRank.text = StaticLoc.Loc.Get("info349");

 //                   }
//                    else
//                    {


                        //if(sortType.ToString()=="desc")
                        //{
                        //    sqlMy=string.Format ("Select (Select count(*) from Corps where a.{0}<{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{1},a.id from Corps a where id={2} order by ROW_NUMBER", rankName, showRowName, BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText);
                        //}
                        //else if(sortType.ToString()=="asc")
                        //{
                        //    sqlMy=string.Format ("Select (Select count(*) from Corps where a.{0}>{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{1},a.id from Corps a where id={2} order by ROW_NUMBER", rankName, showRowName, BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText);
                        //}
                        // InRoom.GetInRoomInstantiate().GetYuanTable(sqlMy, "DarkSword2", ytMyRank);
                        //if(sortType.ToString()=="desc")
                        //{
                        //    sql=string.Format ("Select (Select count(*) from {1} where a.{0}<{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{2},a.id from {1} a order by ROW_NUMBER limit 30", rankName, tableName, showRowName);
                        //}
                        //else if(sortType.ToString()=="asc")
                        //{
                        //    sql=string.Format ("Select (Select count(*) from {1} where a.{0}>{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{2},a.id from {1} a order by ROW_NUMBER limit 30", rankName, tableName, showRowName);
                        //}
                        //InRoom.GetInRoomInstantiate().GetYuanTable(sql, "DarkSword2", yt);	
                        ;
                    }
			InRoom.GetInRoomInstantiate().GetRank(this.rankingType, BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, yt);
//                    if (BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText == "")
//                    {
//                        lblPVP4.text = StaticLoc.Loc.Get("info345");
//                        lblPVP4.gameObject.SetActiveRecursively(true);
//                    }
//                    else
//                    {

                        //if(sortType.ToString()=="desc")
                        //{
                        //    sqlMy=string.Format ("Select (Select count(*) from Corps where a.{0}<{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{1},a.id from Corps a where id={2} order by ROW_NUMBER", rankName, showRowName, BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText);
                        //}
                        //else if(sortType.ToString()=="asc")
                        //{
                        //    sqlMy=string.Format ("Select (Select count(*) from Corps where a.{0}>{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{1},a.id from Corps a where id={2} order by ROW_NUMBER", rankName, showRowName, BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText);
                        //}				
                        // InRoom.GetInRoomInstantiate().GetYuanTable(sqlMy, "DarkSword2", ytPVP4);
                        //if(sortType.ToString()=="desc")
                        //{
                        //    sql=string.Format ("Select (Select count(*) from {1} where a.{0}<{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{2},a.id from {1} a order by ROW_NUMBER limit 30", rankName, tableName, showRowName);
                        //}
                        //else if(sortType.ToString()=="asc")
                        //{
                        //    sql=string.Format ("Select (Select count(*) from {1} where a.{0}>{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{2},a.id from {1} a order by ROW_NUMBER limit 30", rankName, tableName, showRowName);
                        //}
                        //InRoom.GetInRoomInstantiate().GetYuanTable(sql, "DarkSword2", yt);	
//                        
 //                   }
//                    InRoom.GetInRoomInstantiate().GetRank(this.rankingType, BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText, ytPVP4);
 //               }
                break;
            case yuan.YuanPhoton.RankingType.Guild:
                {
                    if (BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText == "")
                    {
                        lblMyRank.text = StaticLoc.Loc.Get("info346");

                    }
                    else
                    {

                        //if(sortType.ToString()=="desc")
                        //{
                        //    sqlMy=string.Format ("Select (Select count(*) from GuildInfo where a.{0}<{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{1},a.id from GuildInfo a where id={2} order by ROW_NUMBER", rankName, showRowName, BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText);
                        //}
                        //else if(sortType.ToString()=="asc")
                        //{
                        //    sqlMy=string.Format ("Select (Select count(*) from GuildInfo where a.{0}>{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{1},a.id from GuildInfo a where id={2} order by ROW_NUMBER", rankName, showRowName, BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText);
                        //}					
                        // InRoom.GetInRoomInstantiate().GetYuanTable(sqlMy, "DarkSword2", ytMyRank);
                        //if(sortType.ToString()=="desc")
                        //{
                        //    sql=string.Format ("Select (Select count(*) from {1} where a.{0}<{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{2},a.id from {1} a order by ROW_NUMBER limit 30", rankName, tableName, showRowName);
                        //}
                        //else if(sortType.ToString()=="asc")
                        //{
                        //    sql=string.Format ("Select (Select count(*) from {1} where a.{0}>{0} or (a.{0}={0} and a.id<id)) as ROW_NUMBER,a.{0},a.{2},a.id from {1} a order by ROW_NUMBER limit 30", rankName, tableName, showRowName);
                        //}
                        //InRoom.GetInRoomInstantiate().GetYuanTable(sql, "DarkSword2", yt);		    
                        
                       
                    }
                    InRoom.GetInRoomInstantiate().GetRank(this.rankingType, BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText, yt);
                }
                break;
            default:
                {


                    //if(sortType.ToString()=="desc")
                    //{
                    //    sqlMy=string.Format ("Select (Select count(*) from {3} where a.{0}<{0} or (a.{0}={0} and a.PlayerID<PlayerID)) as ROW_NUMBER,a.{0},a.{1},a.PlayerID from {3} a where PlayerID={2} order by ROW_NUMBER", rankName, showRowName, BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, tableName);
                    //}
                    //else if(sortType.ToString()=="asc")
                    //{
                    //    sqlMy=string.Format ("Select (Select count(*) from {3} where a.{0}>{0} or (a.{0}={0} and a.PlayerID<PlayerID)) as ROW_NUMBER,a.{0},a.{1},a.PlayerID from {3} a where PlayerID={2} order by ROW_NUMBER", rankName, showRowName, BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, tableName);
                    //}
                    //InRoom.GetInRoomInstantiate().GetYuanTable(sqlMy, "DarkSword2", ytMyRank);
                    //if(sortType.ToString()=="desc")
                    //{
                    //    sql=string.Format ("Select (Select count(*) from {1} where a.{0}<{0} or (a.{0}={0} and a.PlayerID<PlayerID)) as ROW_NUMBER,a.{0},a.{2},a.PlayerID from {1} a order by ROW_NUMBER limit 30", rankName, tableName, showRowName);
                    //}
                    //else if(sortType.ToString()=="asc")
                    //{
                    //    sql=string.Format ("Select (Select count(*) from {1} where a.{0}>{0} or (a.{0}={0} and a.PlayerID<PlayerID)) as ROW_NUMBER,a.{0},a.{2},a.PlayerID from {1} a order by ROW_NUMBER limit 30", rankName, tableName, showRowName);
                    //}
                    //InRoom.GetInRoomInstantiate().GetYuanTable(sql, "DarkSword2", yt);	
                    InRoom.GetInRoomInstantiate().GetRank(this.rankingType, BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, yt);

                }
                break;

        }

        //sql = string.Format("Select top 30 * from (Select {0},{2},ROW_NUMBER()over(order by {0} {3})ROW_NUMBER from {1}) as temp ", rankName, tableName, showRowName, sortType.ToString());

        if (yt.IsUpdate || ytPVP4.IsUpdate)
        {
            yield return new WaitForSeconds(1);
        }


        loading.SetActiveRecursively(false);
        if (yt.dicMyRank != null)
        {
            if (lblMyRank != null)
            {
                switch (rankingType)
                {
                    case yuan.YuanPhoton.RankingType.Guild:
                        {
                            if (BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText == "")
                            {
                                lblMyRank.text = StaticLoc.Loc.Get("info346");

                            }
                            else
                            {
                                if (yt.myRank > 0)
                                {
                                    lblMyRank.text = StaticLoc.Loc.Get("info347") + yt.myRank;
                                }
                                else
                                {
                                    lblMyRank.text = StaticLoc.Loc.Get("info347");
                                }
                            }
                        }
                        break;
                    case yuan.YuanPhoton.RankingType.Arena:
                        {
//                            if (BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText == "")
//                            {
							lblMyRank.text = StaticLoc.Loc.Get("info349");

 //                           }
//                            else
//                            {
                                if (yt.myRank > 0)
                                {
								lblMyRank.text = StaticLoc.Loc.Get("info349") + yt.myRank;
                                }
                                else
                                {
							lblMyRank.text = StaticLoc.Loc.Get("info349")+StaticLoc.Loc.Get("info1159");
                                }
//                            }
                        }
                        break;
                    default:
                        {
                            if (yt.myRank > 0)
                            {
                                lblMyRank.text = StaticLoc.Loc.Get("info349") + yt.myRank;
                            }
                            else
                            {
						lblMyRank.text = StaticLoc.Loc.Get("info349")+StaticLoc.Loc.Get("info1159");
                            }
                        }
                        break;
                }


            }

            if (lblPVP4 != null)
            {
                if (ytPVP4.myRank > 0)
                {
                    lblPVP4.text = StaticLoc.Loc.Get("info350") + yt.myRank;
                }
            }

            foreach (BtnRanking btn in listBtn)
            {
                btn.gameObject.SetActiveRecursively(false);
            }
            //Debug.Log(this.name+"+++"+yt.Rows.Count);

            int num = 0;
            foreach (KeyValuePair<string, int> yr in yt.dicMyRank)
            {
                if (listBtn.Count > num)
                {
                    listBtn[num].gameObject.SetActiveRecursively(true);
                    SetBtn(listBtn[num], yr.Key, yr.Value, num + 1);
                }
                else
                {
                    GameObject obj = (GameObject)Instantiate(btnRanking);
                    BtnRanking tempBtn = obj.GetComponent<BtnRanking>();
                    tempBtn.transform.parent = grid.transform;
                    tempBtn.transform.localPosition = Vector3.zero;
                    tempBtn.transform.localScale = new Vector3(1, 1, 1);
                    SetBtn(tempBtn, yr.Key, yr.Value, num + 1);
                    listBtn.Add(tempBtn);

                }
                num++;
            }
        }
        grid.repositionNow = true;
    }

    private void SetBtn(BtnRanking btn,string mName,int mValue,int mNum)
    {
        
        //Debug.Log("qqqqqqqqqqqqqq+++" + yr.Count);
        btn.lblName.text = mName;
        btn.lblRank.text = mNum.ToString();
        //Debug.Log("----------------------" + yr[rankName].YuanColumnText);
        btn.lblInfo.text = mValue.ToString();
		switch(mNum)
		{
		case 1 : 
			btn.lblSprite.spriteName = "taskm";
			btn.LblNumber.spriteName = "sz_1";
			break;
		case 2 : 
			btn.lblSprite.spriteName = "taskt";
			btn.LblNumber.spriteName = "sz_2";
			break;
		case 3 : 
			btn.lblSprite.spriteName = "taskp";
			btn.LblNumber.spriteName = "sz_3";
			break;
		}
    }


}
