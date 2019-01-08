using UnityEngine;
using System.Collections;

public class TD_SSS : MonoBehaviour {
	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}
	
	void OnApplicationQuit(){
	
#if UNITY_IPHONE

		if(TD_info.isStart){
			TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo005"), StaticLoc.Loc.Get("tdinfo024"));
			TD_info.isStart = false;
		}
		if(TD_info.isNotice){
			TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo001"), StaticLoc.Loc.Get("tdinfo024"));
			TD_info.isNotice = false;
		}
		if(TD_info.isLogin){
			TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo007"), StaticLoc.Loc.Get("tdinfo024"));
			TD_info.isLogin = false;
		}
		if(TD_info.isCreatRole){
			TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo009"), StaticLoc.Loc.Get("tdinfo024"));
			TD_info.isCreatRole = false;
		}
		if(TD_info.isTraining){
			TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo014"), StaticLoc.Loc.Get("tdinfo024"));
			TD_info.isTraining = false;
		}
		TalkingDataGA.OnEnd ();
#endif
	}	
}
