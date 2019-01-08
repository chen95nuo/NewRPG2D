using UnityEngine;
using System.Collections;

public class GuildPanelChange : MonoBehaviour {
	public GameObject ObjCreatGuid;
	public GameObject ObjCompleteGuild;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ShowGuild();
	}
	void ShowGuild(){
		if (string.IsNullOrEmpty(BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText))
		{
			ObjCreatGuid.SetActive(true);
			ObjCompleteGuild.SetActive(false);
		}else{
			ObjCreatGuid.SetActive(false);
			ObjCompleteGuild.SetActive(true);
		}
	}	
}
