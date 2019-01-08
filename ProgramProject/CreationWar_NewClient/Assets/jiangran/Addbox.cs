using UnityEngine;
using System.Collections;

public class Addbox : MonoBehaviour {
	void OnDisable(){
		if(gameObject.collider){
			gameObject.collider.enabled = false;
		}
	}
	
	void OnEnable(){
		if(gameObject.collider){
			gameObject.collider.enabled = true;
		}
	}
}
