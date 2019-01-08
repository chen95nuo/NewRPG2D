using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldBoosControl : MonoBehaviour {
	public static WorldBoosControl worldBoosControl;

	public BtnBossRank[] btnBoosRank;

	public UILabel LblMyRank;
	public UILabel LblMyDamage;
	// Use this for initialization
	void Awake(){
		worldBoosControl = this ;
	}

	public void Show( List<Dictionary<string , string>> pList,string MyRank,string Damage){
		for(int i = 0; i<pList.Count ; i++)
		{
			btnBoosRank[i].LblName.text = pList[i]["name"] ;
			btnBoosRank[i].LblDamage.text = pList[i]["damage"];
		}

		LblMyRank.text = MyRank;
		LblMyDamage.text = Damage;
	}
}
