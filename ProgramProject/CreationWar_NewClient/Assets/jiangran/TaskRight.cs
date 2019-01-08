using UnityEngine;
using System.Collections;

public class TaskRight : MonoBehaviour {
	public GameObject obj;
	void OnEnable(){
		if(obj.transform.childCount==0){
			gameObject.SetActive(false);
		}else{
			gameObject.SetActive(true);
		}
	}
}
