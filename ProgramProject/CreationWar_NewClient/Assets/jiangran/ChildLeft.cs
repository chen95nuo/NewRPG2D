using UnityEngine;
using System.Collections;

public class ChildLeft : MonoBehaviour {
	public GameObject obj;
	// Use this for initialization
	void Start () {
		InvokeRepeating("ShowNull",0,1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){

	}
	void ShowNull(){
		if(obj.transform.childCount==0){
			gameObject.SetActive(true);
		}else{
			gameObject.SetActive(false);
		}
	}
}
