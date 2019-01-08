using UnityEngine;
using System.Collections;

public class RefreshGuildBuild : MonoBehaviour {


    public UILabel lblNull;
    public UILabel lblNowBuildPoint;
    public UILabel lblNowFunds;
    public UISlider sliderGuildLevel;
    public BtnDisable btnGuildLevelUp;
    public Warnings warnings;

    public UILabel lblGuildName;
    public UILabel lblChairmanName;
    public UILabel lblGuildFunds;
    public UILabel lblGuildLevel;
    public UILabel lblGuildMembership;
    public UILabel lblGuildRanking;

    public UILabel lblAllDedicationPoint;
    public UILabel lblHistoryDedicationRanking;
    public UILabel lblMilitary;
    public UILabel lblPost;

	public string rankName = string.Empty;
	//private YuanRank ytMe;

    public UILabel lblPoint;

	public static int GuildRankNum = 0; 
    public enum RefeshType
    {
       Build,
        Info
    }
    public RefeshType refreshType;
    private yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("mGuildID", "id");
    private YuanRank ytDedicationRanking = new YuanRank("DedicationRanking");
	
	void Start()
	{
		warnings=PanelStatic.StaticWarnings;
	//	ytMe = new YuanRank(rankName + "rank");
	}
	
    public void OnEnable()
    {
		InRoom.GetInRoomInstantiate().GetRankByMe(yuan.YuanPhoton.RankingType.Guild, BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText, "GuildRankingrank");
        //InRoom.GetInRoomInstantiate().GetTableForID(BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.TableType.PlayerInfo,BtnGameManager.yt);
        lblPoint.text = BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText;
        StartCoroutine(ReadInfo());
    }

    IEnumerator ReadInfo()
    {
//        lblNull.gameObject.active = false;
        if (btnGuildLevelUp != null)
        {
            btnGuildLevelUp.Disable = true;
        }
        if (BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText != "")
        {
            //InRoom.GetInRoomInstantiate().GetYuanTable("Select * from GuildInfo where id=" + BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText, "DarkSword2", yt);
			InRoom.GetInRoomInstantiate ().GetTableForID(BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText,yuan.YuanPhoton.TableType.GuildInfo,yt);
            while (yt.IsUpdate)
            {
                yield return new WaitForSeconds(0.5f);
            }
            
            if (yt.Rows.Count > 0)
            {
                switch (refreshType)
                {
                    case RefeshType.Build:
                        lblNowBuildPoint.text = yt.Rows[0]["GuildBuild"].YuanColumnText;
                        lblNowFunds.text = yt.Rows[0]["GuildFunds"].YuanColumnText;
                        SetLevelUpSlider(int.Parse(yt.Rows[0]["GuildLevel"].YuanColumnText), float.Parse(yt.Rows[0]["GuildBuild"].YuanColumnText));
                        break;
                    case RefeshType.Info:
                        lblGuildName.text = yt.Rows[0]["GuildName"].YuanColumnText;
                        lblChairmanName.text = yt.Rows[0]["GuildHeadID"].YuanColumnText.Split(',')[1];
                        lblGuildFunds.text = yt.Rows[0]["GuildFunds"].YuanColumnText;
                        lblGuildLevel.text = yt.Rows[0]["GuildLevel"].YuanColumnText;
                        lblGuildMembership.text = yt.Rows[0]["PlayerNumber"].YuanColumnText;
					//Debug.Log("-------------------------------"+ytMe.myRank.ToString());

					lblGuildRanking.text = GuildRankNum.ToString();

                        lblAllDedicationPoint.text = BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText;
                        lblPost.text = GetGuildPostType();
                        lblMilitary.text = BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText;
                        //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select * from GameRanking where PlayerID='{0}' and RankType='{1}'", BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, (int)yuan.YuanPhoton.RankingType.Guild), "DarkSword2", ytDedicationRanking);
                        
						InRoom.GetInRoomInstantiate ().GetRankOne (yuan.YuanPhoton.RankingType.Guild,BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText ,ytDedicationRanking);
						while (ytDedicationRanking.IsUpdate)
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                   		lblHistoryDedicationRanking.text = ytDedicationRanking.myRank.ToString();
						
                        break;
                }
            }
            else
            {
//                lblNull.gameObject.active = true;
                this.gameObject.SetActiveRecursively(false);
            }
        }
        else
        {
  //          lblNull.gameObject.active = true;
            this.gameObject.SetActiveRecursively(false);
        }
    }

    private string GetGuildPostType()
    {

//		if (int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==1)
//        {
//            return "会长";
//        }
//        else if (yt.Rows[0]["GuildDepHeadID"].YuanColumnText.Split(',').Length > 0 && yt.Rows[0]["GuildDepHeadID"].YuanColumnText.Split(',')[0] == BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText)
//        {
//            return "副会长";
//        }
//        else if (yt.Rows[0]["GuildOldManID"].YuanColumnText.Split(',').Length > 0 && yt.Rows[0]["GuildOldManID"].YuanColumnText.Split(',')[0] == BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText)
//        {
//            return "元老";
//        }
//        else if (yt.Rows[0]["GuildElite"].YuanColumnText.Split(',').Length > 0 && yt.Rows[0]["GuildElite"].YuanColumnText.Split(',')[0] == BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText)
//        {
//            return "精英";
//        }
//        else
//        {
//            return "普通会员";
//        }

		if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==1){
						return "会长";
//			lblPost.text = StaticLoc.Loc.Get("info950");
		}else if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==2){
						return "副会长";
//			lblPost.text = StaticLoc.Loc.Get("info947");
		}else if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==3){
						return "长老";
//			lblPost.text = StaticLoc.Loc.Get("info948");
		}else if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==4){
						return "精英";
//			lblPost.text = StaticLoc.Loc.Get("info705");
		}else if (int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==5){
						return "普通会员";
		}else{
						return "非公会会员";
//			lblPost.text = StaticLoc.Loc.Get("info949");
		}
    }

    /// <summary>
    /// 设置公会等级进度条
    /// </summary>
    /// <param name="mLevel"></param>
    /// <param name="mBuild"></param>
    public void SetLevelUpSlider(int mLevel,float mBuild)
    {
        float need = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicGuildLevel[mLevel][yuan.YuanPhoton.GuildLevelUp.Build];
        sliderGuildLevel.sliderValue = mBuild / need;
        if (GetGuildPostType() == "会长")
        {
            if (mBuild / need >= 1)
            {
                btnGuildLevelUp.Disable = false;
            }
            else
            {
                if (int.Parse(yt.Rows[0]["GuildLevel"].YuanColumnText) < YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicGuildLevel.Count)
                {
                    btnGuildLevelUp.Disable = true;
                }
            }
        }
    }

    public void GuildLevelUpClick()
    {
        if (btnGuildLevelUp.disable == false)
        {
         //   InRoom.GetInRoomInstantiate().GuildLevelUp(yt.Rows[0]["id"].YuanColumnText);
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info406"));  
        }
    }

}
