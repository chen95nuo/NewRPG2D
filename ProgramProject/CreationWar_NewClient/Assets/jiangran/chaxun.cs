using UnityEngine;
using System.Collections;
[ExecuteInEditMode()]

public class chaxun : MonoBehaviour {

void Awake(){
		GameObject[] obj = FindObjectsOfType(typeof(GameObject))as GameObject[];
		
		for(var i = 0; i<obj.Length;i++){
			if(obj[i].collider == true){
				obj[i].AddComponent("Addbox");
			}
		}
	}
}
