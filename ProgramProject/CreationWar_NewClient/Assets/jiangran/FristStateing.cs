using UnityEngine;
using System.Collections;

public class FristStateing : MonoBehaviour {
	public UIToggle My;
	// Use this for initialization
	void Start () {
	}

	void OnEnable(){
		FristControl();
	}

	void FristControl(){
		My.Set(false);
	}
	
}
