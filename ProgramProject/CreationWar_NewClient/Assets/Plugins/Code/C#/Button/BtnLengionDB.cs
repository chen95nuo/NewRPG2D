using UnityEngine;
using System.Collections;
using yuan.YuanPhoton;

public class BtnLengionDB : MonoBehaviour {
    public GetPic getPic;
    public UILabel lblName;
    public UILabel lblInfo;

    //[HideInInspector]
    public LegionType legionType = LegionType.DBLegion;
    [HideInInspector]
    public string teamID = string.Empty;
    [HideInInspector]
    public string teamPicID = string.Empty;
    [HideInInspector]
    public string teamName = string.Empty;
    [HideInInspector]
    public string teamLevel = string.Empty;
    [HideInInspector]
    public string teamRanking = string.Empty;
    [HideInInspector]
    public string teamInfo = string.Empty;
    [HideInInspector]
    public string teamMemver = string.Empty;
    [HideInInspector]
    public string teamHeadID = string.Empty;
    [HideInInspector]
    public string teamDepHeadID = string.Empty;


    public void SetMyInfo()
    {
        if(legionType==LegionType.DBLegion)
        {
            this.getPic.PicID = this.teamPicID;
        }
        this.lblName.text = this.teamName;
        this.lblInfo.text = this.teamInfo;
    }

}
