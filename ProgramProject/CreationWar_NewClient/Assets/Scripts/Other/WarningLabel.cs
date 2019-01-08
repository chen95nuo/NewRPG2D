using UnityEngine;
using System.Collections;

public class WarningLabel : MonoBehaviour {
	private UILabel label;
	private float durationTime = 0;
	// Use this for initialization
	void Start () {
		label = GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!string.IsNullOrEmpty(label.text))
		{
			durationTime += Time.deltaTime;
			if(durationTime > 1.0f)
			{
				label.text = "";
				durationTime = 0;
			}
		}
	}
}
