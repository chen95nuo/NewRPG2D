using UnityEngine;
using System.Collections;

public class ShowVipControl : MonoBehaviour {
	public GameObject ObjVip;
	public static ShowVipControl My;
	// Use this for initialization
	void Start () {
		My = this;
	}

	void Update(){
		ShowMainVip();
	}

	void ShowMainVip(){
		int showVIP = PlayerPrefs.GetInt("ShowVIP", 1);
		if(showVIP==1){
			ObjVip.SetActive(true);
		}else{
			ObjVip.SetActive(false);
		}
	}
}
