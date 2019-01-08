using UnityEngine;
using System.Collections;

public class ShowMonsterHP : MonoBehaviour {
	public UISprite NowHp;
	public UILabel HpText;
	public UILabel MaxHp;
	public UILabel NumberText;
	public static ShowMonsterHP showMonsterHP;
	// Use this for initialization
	void Start () {
		showMonsterHP = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowHaloHp(int nowHp , int maxHp){
		float NHp = (float)nowHp;
//		float MHp = (float)maxHp;

//		NowHp.fillAmount = NHp/MHp;

		HpText.text = nowHp.ToString();
	}

	public void ShowNumber(int Str){
		NumberText.text = StaticLoc.Loc.Get("info1039")+Str.ToString()+StaticLoc.Loc.Get("info1040");
	}

	public void ShowMaxHp(int maxHp)
	{
		if(MaxHp){
		MaxHp.text = "/"+maxHp.ToString();
	}
	}
}
