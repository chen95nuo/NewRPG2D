using UnityEngine;
using System.Collections;

public class ContorlText : MonoBehaviour {
	private TweenPosition tweenP;
	void Update(){
		tweenP = gameObject.transform.GetComponent<TweenPosition>();
		tweenP.from.y = -450;
		tweenP.to.y = 500;
	}   

}
