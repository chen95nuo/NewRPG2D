using UnityEngine;
using System.Collections;

public class MonsterHP : MonoBehaviour {
	public UISprite NowHp;
	public UILabel HpText;

	public UISprite NowHp1;
	public UILabel HpText1;
	public static MonsterHP monsterHP;
	// Use this for initialization
	void Start () {
		monsterHP = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowMyMonsterHp(int nowHp , int maxHp){
		float NHp = (float)nowHp;
		float MHp = (float)maxHp;

		NowHp.fillAmount = NHp/MHp;

		HpText.text = nowHp.ToString();
	}
	public void ShowOtherMonsterHp(int nowHp , int maxHp){
		float NHp = (float)nowHp;
		float MHp = (float)maxHp;
		
		NowHp1.fillAmount = NHp/MHp;
		
		HpText1.text = nowHp.ToString();
	}
}
