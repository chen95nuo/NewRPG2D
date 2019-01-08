using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BossRanking : MonoBehaviour {

	public static BossRanking BR;

	public BtnBossRank[] ListTeamPlayer;
	public List<Dictionary<string,string>> ListPlayers ; 
//	public UILabel LblName1;
//	public UILabel LblName2;
//	public UILabel LblName3;
//
//	public UILabel LblDamage1;
//	public UILabel LblDamage2;
//	public UILabel LblDamage3;

	public UILabel LblMyRank;
	public UILabel LblMyDamage;


	public UILabel LblTime;
	public string ActiveID ;

	DateTime currentTime = new DateTime();
	// Use this for initialization
	void Start () {
		BR = this;
		currentTime = System.DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
		ShowTime();
	}
	public void ShowBoosRank(List<Dictionary<string,string>> plist , int MyRank , int MyDamage)
	{
//		foreach(Dictionary<string,string> item in plist)
//		{
//			LblName1.text =  item["name"];
//			LblDamage1.text = item["damage"];
//
//			LblName2.text =  item["name"];
//			LblDamage2.text = item["damage"];
//
//			LblName3.text =  item["name"];
//			LblDamage3.text = item["damage"];
//		}
		//			if(WorldBoosControl.worldBoosControl){
//				
//				WorldBoosControl.worldBoosControl.LblOneName.text =  item["name"];
//				WorldBoosControl.worldBoosControl.LblOneDamage.text = item["damage"];
//				
//				WorldBoosControl.worldBoosControl.LblTwoName.text =  item["name"];
//				WorldBoosControl.worldBoosControl.LblTwoDamage.text = item["damage"];
//				
//				WorldBoosControl.worldBoosControl.LblThreeName.text =  item["name"];
//				WorldBoosControl.worldBoosControl.LblThreeDamage.text = item["damage"];
//				
//			}
		

		for(int i = 0;i<plist.Count ; i++){
			ListTeamPlayer[i].LblName.text = plist[i]["name"] ;
			ListTeamPlayer[i].LblDamage.text = plist[i]["damage"] ;
		}

		LblMyRank.text = MyRank.ToString();
		LblMyDamage.text = MyDamage.ToString();
//		if(WorldBoosControl.worldBoosControl){
//			WorldBoosControl.worldBoosControl.LblMyRank.text = MyRank.ToString();
//			WorldBoosControl.worldBoosControl.MyDamage.text = MyDamage.ToString();
//		}
	
	}

	void ShowTime(){
		if(Application.loadedLevelName == "Map911")
		{
			ActiveID = "9";

			yuan.YuanMemoryDB.YuanRow yr = YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytActivity.SelectRowEqual ("id",ActiveID);
			string Tim =  yr["ActivityTimePeriods"].YuanColumnText ;
			
			
			
			string[] strt = Tim.Split('-');
			string ActivityEndTime = strt[1];
	//		string ActivityEndTime = "15:30";
			string timMe = InRoom.GetInRoomInstantiate().serverTime.ToString("d").Trim()+" "+ActivityEndTime + ":00" ;
			int LastTime = (int)((DateTime.Parse(timMe) - InRoom.GetInRoomInstantiate().serverTime).TotalSeconds);
			
			int hours = (int)(LastTime / (60 * 60));
			int minutes = LastTime % (60 * 60) / 60;
			int seconds = LastTime % (60 * 60) % 60;
			if(LastTime>=0){
			LblTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
			}

		}
		if(Application.loadedLevelName == "Map912")
		{
			ActiveID = "22";
			yuan.YuanMemoryDB.YuanRow yr = YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytActivity.SelectRowEqual ("id",ActiveID);
			string Tim =  yr["ActivityTimePeriods"].YuanColumnText ;
			
			
			
			string[] strt = Tim.Split('-');
			string ActivityEndTime = strt[1];
//			string ActivityEndTime = "15:30";
			string timMe = InRoom.GetInRoomInstantiate().serverTime.ToString("d").Trim()+" "+ActivityEndTime + ":00" ;
			int LastTime = (int)((DateTime.Parse(timMe) - InRoom.GetInRoomInstantiate().serverTime).TotalSeconds);
			
			int hours = (int)(LastTime / (60 * 60));
			int minutes = LastTime % (60 * 60) / 60;
			int seconds = LastTime % (60 * 60) % 60;
			if(LastTime>=0){
			LblTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
			}

		}else{
			return;
		}
	}
}
