using UnityEngine;
using System.Collections;

public class OneClickMe : MonoBehaviour {

	public UIPanel Mypanel; 
	
	// Update is called once per frame
	void OnEnable()
	{
		this.gameObject.transform.localPosition = Vector3.zero;
		Mypanel.clipOffset = new Vector2(0f,0f);
	}
}
