using UnityEngine;
using System.Collections;

public class BtnDelayClick : MonoBehaviour {
	private BoxCollider btnEnter;
	private float duration = 10.0f;
	private float lastTime = 0;
	
	void Start () {
		if(null == btnEnter)
		{
			btnEnter = GetComponent<BoxCollider>();
		}
		lastTime = Time.time;
	}
	
	void Update () {
		if(Time.time - lastTime > duration && !btnEnter.enabled)
		{
			btnEnter.enabled = true;
		}
	}
	
	void OnClick () 
	{
		if(btnEnter.enabled)
		{
			btnEnter.enabled = false;
			lastTime = Time.time;
		}
	}
}
