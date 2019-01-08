
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendTeamPlayer : MonoBehaviour {

	public static FriendTeamPlayer FTP;
	public BtnTeamFriendPlayer[] ListTeamPlayer;
	public List<Dictionary<string,string>> ListPlayers ; 
	private string TeamHeadId;

	private bool isclick = false;
	void Awake()
	{
		FTP = this;
	}
	public void ClickMe(){
		if(isclick){
			StartCoroutine(click());
		}
	}
	IEnumerator click()
	{
		yield return new WaitForSeconds(0.1f);
		ShowFriendPlayer(ListPlayers,TeamHeadId);
	}
	public void ShowFriendPlayer(List<Dictionary<string,string>> mListBtn , string headID){
		ListPlayers = mListBtn;
		TeamHeadId = headID;
		foreach(BtnTeamFriendPlayer Lis in ListTeamPlayer){
			Lis.gameObject.SetActive(false);
		}

		for(int i = 0 ; i<ListPlayers.Count ; i++){
			if(ListPlayers[i]["playerProID"]=="1")
			{
				ListTeamPlayer[i].PlayerPro.spriteName = "head-zhanshi";

			}else if(ListPlayers[i]["playerProID"]=="2")
			{
				ListTeamPlayer[i].PlayerPro.spriteName = "head-youxia";
			}else if(ListPlayers[i]["playerProID"]=="3")
			{
				ListTeamPlayer[i].PlayerPro.spriteName = "head-fashi";
			}
			if(TeamHeadId==ListPlayers[i]["playerID"]){
				ListTeamPlayer[i].PlayerID.enabled = true;
			}else{
				ListTeamPlayer[i].PlayerID.enabled = false;
			}
			ListTeamPlayer[i].PlayerName.text = ListPlayers[i]["playerName"] ;
			ListTeamPlayer[i].PlayerLevel.text = ListPlayers[i]["playerLevel"] ;
			ListTeamPlayer[i].gameObject.SetActive(true);
		}
		isclick = true;
	}

}
