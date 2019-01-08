using UnityEngine;
using System.Collections;

public class ShowCardItemTime : MonoBehaviour {
	public UILabel LblCardGow ; 
	public UILabel LblCardDouble;

	public UISprite SprCardGow;
	public UISprite SprCardDouble;
	// Use this for initialization
	void Start () {
		InvokeRepeating("ShowLabText",0,1f);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void ShowLabText(){
	 	if(BtnGameManager.yt.Rows[0]["GOWCard"].YuanColumnText!="0"){
			int tim = int.Parse(BtnGameManager.yt.Rows[0]["GOWCard"].YuanColumnText) ;
			int minutes = tim % (60 * 60) / 60;
			int seconds = tim % (60 * 60) % 60;
			LblCardGow.text = StaticLoc.Loc.Get("info1232") + string.Format("{0:00}:{1:00}",minutes,seconds);
			SprCardGow.enabled = true;
		}else{
			LblCardGow.text = "";
			SprCardGow.enabled = false;
		}
		if(BtnGameManager.yt.Rows[0]["DoubleCard"].YuanColumnText!="0"){
			int tim = int.Parse(BtnGameManager.yt.Rows[0]["DoubleCard"].YuanColumnText) ;
			int minutes = tim % (60 * 60) / 60;
			int seconds = tim % (60 * 60) % 60;
			LblCardDouble.text = StaticLoc.Loc.Get("info1233") + string.Format("{0:00}:{1:00}",minutes,seconds);
			SprCardDouble.enabled = true;
		}else{
			LblCardDouble.text = "";
			SprCardDouble.enabled = false;
		}
		
	}


}
