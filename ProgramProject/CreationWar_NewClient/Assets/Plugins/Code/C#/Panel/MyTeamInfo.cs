using UnityEngine;
using System.Collections;

public class MyTeamInfo : MonoBehaviour {


    public string teamID = string.Empty;
    public string teamName = string.Empty;
    public string teamInfo = string.Empty;
    public string teamHeadID = string.Empty;
    public string teamMemver = string.Empty;

    public GameObject objPlayerSet;

    void OnEnable()
    {
        //if (teamHeadID!=string.Empty && BtnGameManager.yt[0]["PlayerID"].YuanColumnText.Trim() == teamHeadID)
        //{
        //    objPlayerSet.SetActiveRecursively(true);
        //}
        //else
        //{
        //    objPlayerSet.SetActiveRecursively(false);
        //}
    }
}
