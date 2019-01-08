using UnityEngine;
using System.Collections;

public class SelectServerActivity : MonoBehaviour {

	public GameObject Selectserver;
	
	void OnClick(){
		if(null!=Selectserver){
	   Selectserver.transform.localScale =new Vector3(1,1,1);
	}
	}
}
