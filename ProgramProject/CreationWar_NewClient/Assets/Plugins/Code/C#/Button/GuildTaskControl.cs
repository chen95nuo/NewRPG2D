using UnityEngine;
using System.Collections;

public class GuildTaskControl : MonoBehaviour {

		public GameObject ObjGuildTask;
		
		void Update(){
			ShowMainVip();
		}
		
		void ShowMainVip(){
			int showVIP = PlayerPrefs.GetInt("ShowVIP", 1);
			if(showVIP==1){
			ObjGuildTask.SetActive(true);
			}else{
			ObjGuildTask.SetActive(false);
			}
		}
	}
