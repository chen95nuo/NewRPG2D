using UnityEngine;
using System.Collections;

public class PositionControl : MonoBehaviour {
	public TweenPosition TP;
	public TweenRotation TP1;
	private bool IsClick = true;
	public UILabel LabelText1;
	public UILabel LabelText2;

	private float a = 0;
	private float b = 5;

	public GameObject Obj1;
	public GameObject Obj2;
	
	// Use this for initialization
	void Start () {
		a = Time.time;
		InvokeRepeating("ShowLabText",0,1f);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time-a>b){
			ClickOn();
		}
	}

	void ClickOn()
	{
		IsClick = true;
		TP.Play(true);
		TP1.Play(true);
	}

	void ClickOff()
	{
		IsClick = false;
		TP.Play(false);
		TP1.Play(false);
		a = Time.time;
	}
	public void BtnClick()
	{
		if(IsClick){
			ClickOff();
		}else{
			ClickOn();
		}
	}

	void ShowLabText(){
		if(BtnGameManager.yt.Rows[0]["GOWCard"].YuanColumnText=="0"&&BtnGameManager.yt.Rows[0]["DoubleCard"].YuanColumnText=="0"){
			LabelText2.text = StaticLoc.Loc.Get("info1135");
			Obj1.SetActive(false);
			Obj2.SetActive(true);
		}else
		if(BtnGameManager.yt.Rows[0]["GOWCard"].YuanColumnText!="0"&&BtnGameManager.yt.Rows[0]["DoubleCard"].YuanColumnText=="0"){
			Obj1.SetActive(true);
			Obj1.transform.localPosition = new Vector3(54,Obj1.transform.localPosition.y,Obj1.transform.localPosition.z);
			Obj2.SetActive(false);
			int tim = int.Parse(BtnGameManager.yt.Rows[0]["GOWCard"].YuanColumnText) ;
			
			int minutes = tim % (60 * 60) / 60;
			int seconds = tim % (60 * 60) % 60;
			LabelText1.text = string.Format("{0:00}:{1:00}",minutes,seconds);
		}else
		if(BtnGameManager.yt.Rows[0]["GOWCard"].YuanColumnText=="0"&&BtnGameManager.yt.Rows[0]["DoubleCard"].YuanColumnText!="0"){
			Obj1.SetActive(false);
			Obj2.SetActive(true);
			int tim = int.Parse(BtnGameManager.yt.Rows[0]["DoubleCard"].YuanColumnText) ;
			int minutes = tim % (60 * 60) / 60;
			int seconds = tim % (60 * 60) % 60;
			LabelText2.text = string.Format("{0:00}:{1:00}",minutes,seconds);
		}else
			if(BtnGameManager.yt.Rows[0]["GOWCard"].YuanColumnText!="0"&&BtnGameManager.yt.Rows[0]["DoubleCard"].YuanColumnText!="0")
		{
			Obj1.SetActive(true);
			Obj1.transform.localPosition = new Vector3(-70,Obj1.transform.localPosition.y,Obj1.transform.localPosition.z);
			Obj2.SetActive(true);
			int tim = int.Parse(BtnGameManager.yt.Rows[0]["GOWCard"].YuanColumnText) ;
			
			int minutes = tim % (60 * 60) / 60;
			int seconds = tim % (60 * 60) % 60;
			LabelText1.text = string.Format("{0:00}:{1:00}",minutes,seconds);

			int tim1 = int.Parse(BtnGameManager.yt.Rows[0]["DoubleCard"].YuanColumnText) ;
			int minutes1 = tim1 % (60 * 60) / 60;
			int seconds1 = tim1 % (60 * 60) % 60;
			LabelText2.text = string.Format("{0:00}:{1:00}",minutes1,seconds1);
		}

	}
}
