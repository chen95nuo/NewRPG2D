using UnityEngine;
using System.Collections;

public class ActionSpacing : MonoBehaviour {

	// Use this for initialization
	public static ActionSpacing actionS;
	void Start () {
		actionS = this;
	}
	
	// Update is called once per frame
	float ptime = 0;
	public Transform cube;
	void Update () {
		if(Time.time > ptime){
			if(cube && cube.position.y != 3000){
				cube.localPosition = new Vector3(0 , 3000 , 0);
			}
		}else{
			if(cube && cube.position.y != 0){
				cube.localPosition = new Vector3(0 , 0 , 0);
			}
		}
	}

	public float ActionSpacingTime = 0.4f;
	public void PTimePlus(){
		ptime = Time.time + ActionSpacingTime;
	}

}
