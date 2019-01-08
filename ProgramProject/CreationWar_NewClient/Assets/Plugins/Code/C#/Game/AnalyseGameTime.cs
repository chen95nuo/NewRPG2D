using UnityEngine;
using System.Collections;
using System.Net;

public class AnalyseGameTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		yServerSpan=new yuan.YuanTimeSpan();
		timeBefor=Time.time;
		yServerSpan.TimeStart (InRoom.GetInRoomInstantiate ().serverTime);
		InvokeRepeating("Analyse",30,30);
	}
	
	private float timeBefor=0;
	private yuan.YuanTimeSpan yServerSpan;
	private float numTimeSpan=0;
	private void Analyse()
	{
		numTimeSpan=Mathf.Abs ((Time.time-timeBefor)-yServerSpan.TimeEndtoFloat(InRoom.GetInRoomInstantiate ().serverTime));
		Debug.LogWarning (numTimeSpan);
		if(numTimeSpan>=3)
		{
			Logout ();
		}
		timeBefor=Time.time;
		yServerSpan.TimeStart (InRoom.GetInRoomInstantiate ().serverTime);
	}
	
	private void Logout()
	{
		BtnManager.isOhterLogin=true;
		BtnManager.strOtherLogin=StaticLoc.Loc.Get("info550");
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SongLoadLevel",0,SendMessageOptions.DontRequireReceiver);
	}
}
