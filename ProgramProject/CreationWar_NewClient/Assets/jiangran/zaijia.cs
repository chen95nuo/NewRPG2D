using UnityEngine;
using System.Collections;

public class zaijia : MonoBehaviour {
	public GameObject g;
	private float a = 0;
	private bool b = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Application.loadedLevelName!="Map111"&&!b){
			a += Time.deltaTime;
			g.SendMessage("Awake");
			g.SendMessage("Start");
		}
		if(a>5){
			b = true;
		}
	}
}
