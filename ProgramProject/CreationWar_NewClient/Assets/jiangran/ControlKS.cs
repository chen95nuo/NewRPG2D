using UnityEngine;
using System.Collections;

public class ControlKS : MonoBehaviour {
	private GameObject My;
	// Use this for initialization
	void Start () {
		My = this.gameObject;
		InvokeRepeating("ShowMy",0,1);
	}
	
	// Update is called once per frame
	void ShowMy () {
		if(Application.loadedLevelName == "Map311"||Application.loadedLevelName == "Map321"||Application.loadedLevelName == "Map421"||Application.loadedLevelName == "Map431"||Application.loadedLevelName == "Map911"){
			My.SetActive(false);
		}
	
	}
}
