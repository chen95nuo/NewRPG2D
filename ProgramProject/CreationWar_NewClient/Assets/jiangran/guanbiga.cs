using UnityEngine;
using System.Collections;

public class guanbiga : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Transform[] transforms = this.GetComponentsInChildren<Transform>();
		for(int i = 0; i<transforms.Length;i++){
			Transform t = transforms[i];
			t.gameObject.active = false;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
