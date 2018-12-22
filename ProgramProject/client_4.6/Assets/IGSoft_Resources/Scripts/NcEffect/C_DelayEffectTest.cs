using UnityEngine;
using System.Collections;

public class C_DelayEffectTest : NcEffectBehaviour {
	public float delayTime;
	private float frame = 0; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if(frame < delayTime){
//		}
		frame += Time.deltaTime;
		if(frame >= delayTime){
			//播放特效//
			SetActiveRecursively(gameObject, true);
		}
	}
}
