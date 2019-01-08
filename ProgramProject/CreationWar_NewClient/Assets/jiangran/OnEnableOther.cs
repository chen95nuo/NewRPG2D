using UnityEngine;
using System.Collections;

public class OnEnableOther : MonoBehaviour {
	private GameObject IsMe;
	public GameObject OtherObj;
	// Use this for initialization
	void Start () {
		IsMe = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		OtherObj.SetActive(false);
	}
}
