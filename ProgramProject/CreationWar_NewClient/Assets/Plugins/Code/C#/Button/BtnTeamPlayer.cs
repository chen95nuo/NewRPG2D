using UnityEngine;
using System.Collections;

public class BtnTeamPlayer : MonoBehaviour
{

    [HideInInspector]
    public string PlayerID;

	[HideInInspector]
	public string PlayerPro;

	[HideInInspector]
	public string PlayerName;

	[HideInInspector]
	public string PlayerLevel;
    
    [HideInInspector]
    public yuan.YuanMemoryDB.YuanTable yt;

}
