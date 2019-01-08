using UnityEngine;
using System.Collections;

public class ShowMyBodyNum : MonoBehaviour {
	public UILabel LblNumMe ; 
	public UISprite SprMe;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ShowNum();
	}

	void OnEnable(){

	}

	void ShowNum()
	{
		if(SprMe.spriteName=="884"&&SprMe.spriteName!="")
		{
			LblNumMe.enabled = true;
		}else{
			LblNumMe.enabled = false;
		}
		
		yuan.YuanMemoryDB.YuanRow yr = YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytPlayerService.SelectRowEqual ("VIPType",BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText);
		if(int.Parse(yr["canEatPower"].YuanColumnText)==-1)
		{
			LblNumMe.text = "";
		}else{
			string NumNow = (int.Parse(yr["canEatPower"].YuanColumnText)-int.Parse(BtnGameManager.yt.Rows[0]["useEatPower"].YuanColumnText)).ToString();
			LblNumMe.text =  string.Format("{0}{1}{2}{3}" , StaticLoc.Loc.Get("info1203") ,NumNow , "/" , yr["canEatPower"].YuanColumnText);
		}
		

	}
}
