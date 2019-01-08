using UnityEngine;
using System.Collections;
using yuan.YuanPhoton;

public class CropsTeamInfo : MonoBehaviour {

    public GetPic getPic;
    public UILabel lblName;
    public UILabel lblHeaderName;
    public UILabel lblLevel;
    public UILabel lblRanking;
    public UILabel lblFuns;
	public UILabel lblDeclaration;
    public LegionType legionType = LegionType.DBLegion;

    [HideInInspector]
    public string id;
    [HideInInspector]
    public string headerID;
    [HideInInspector]
    public yuan.YuanMemoryDB.YuanRow yr;

    /// <summary>
    /// Ë¢ÐÂÐÅÏ¢
    /// </summary>
    public void RefreshInfo()
    {
		try
		{
	        getPic.PicID = yr["PicID"].YuanColumnText.Trim();
			
	        lblName.text = yr["Name"].YuanColumnText.Trim();
	        lblHeaderName.text = "[s]"+yr["HeadName"].YuanColumnText.Trim()+"[/s]";
	        lblLevel.text = yr["SelfLevel"].YuanColumnText.Trim();
	        lblRanking.text = yr["Ranking"].YuanColumnText.Trim();
			lblDeclaration.text = yr["GuildDeclaration"].YuanColumnText.Trim();
	        if (lblFuns != null)
	        {
				lblFuns.text = (yr["MemverID"].YuanColumnText.Trim().Split(';').Length-1).ToString();
	        }
	
	        id = yr["id"].YuanColumnText.Trim();
	        headerID = yr["HeadID"].YuanColumnText.Trim();
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
    }
	
	private string[] strGuildHead=new string[2];
	public void RefreshInfoGuild()
	{
		getPic.PicID = yr["PicID"].YuanColumnText.Trim();
		
        lblName.text = yr["GuildName"].YuanColumnText.Trim();
		strGuildHead=yr["GuildHeadID"].YuanColumnText.Trim().Split(',');
		lblHeaderName.text = "[s]"+strGuildHead[1]+"[/s]";
        lblLevel.text = yr["GuildLevel"].YuanColumnText.Trim();
		headerID = strGuildHead[0];
        lblRanking.text = yr["GuildRanking"].YuanColumnText.Trim();
		lblDeclaration.text = yr["GuildDeclaration"].YuanColumnText.Trim();
        if (lblFuns != null)
        {
	//		lblFuns.text = (yr["Count"].YuanColumnText.Trim().Split(';').Length).ToString();
			lblFuns.text = (int.Parse(yr["Count"].YuanColumnText.Trim())-1).ToString();
        }

        id = yr["id"].YuanColumnText.Trim();
        
	}


}
