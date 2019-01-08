using UnityEngine;
using System.Collections;

public class BtnEnterContor : MonoBehaviour {
	public GameObject BtnEnter;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnDisable(){
		BtnEnter.SetActive(false);
	}

	void OnEnable(){
		BtnEnter.SetActive(true);
	}
}
