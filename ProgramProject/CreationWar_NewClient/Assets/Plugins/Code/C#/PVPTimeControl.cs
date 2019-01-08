using UnityEngine;
using System.Collections;

public class PVPTimeControl : MonoBehaviour {

	public bool IsContinue = false ;
	private bool IsNow = true ;
	public UILabel LblTime;
	int Number = 91 ;

	public static PVPTimeControl PvpT;
	void Awake()
	{
		PvpT = this;
	}
	// Update is called once per frame
	void Update () {
		if(Application.loadedLevelName == "Map311"||Application.loadedLevelName == "Map321"){
			LblTime.gameObject.SetActive(true);
		}else{
			LblTime.gameObject.SetActive(false);
		}
	}

	void Start(){
		if(Application.loadedLevelName == "Map311"||Application.loadedLevelName == "Map321"){
			LblTime.gameObject.SetActive(true);
			InvokeRepeating("ShowTime",0,1f);
		}else{
			LblTime.gameObject.SetActive(false);
		}
	}

	public void TimeStop(){
		IsContinue = true;
	}

	void ShowTime()
	{
		if(!IsContinue){
		if(Number>0){
			Number -= 1 ;
			LblTime.text = Number.ToString();
		}else{
				BtnGameManagerBack.my.UICL.SendMessage("DuelFaild",SendMessageOptions.DontRequireReceiver);
				IsContinue = true;
		}
		}else{
			LblTime.text = StaticLoc.Loc.Get("info1223");
		}
	}
}
