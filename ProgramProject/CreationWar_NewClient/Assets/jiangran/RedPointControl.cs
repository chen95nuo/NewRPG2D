using UnityEngine;
using System.Collections;

public class RedPointControl : MonoBehaviour {
	public UISprite Tag ; 
	public UISprite Me ;
	// Use this for initialization
	void Start () {
		InvokeRepeating("ShowRedPoint",0,1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ShowRedPoint()
	{
		if(Me){
		if(Tag.enabled&&Tag){
			Me.enabled = true ; 
		}else{
			Me.enabled = false ; 
		}
	}
	}
}
