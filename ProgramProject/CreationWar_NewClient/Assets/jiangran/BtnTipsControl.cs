using UnityEngine;
using System.Collections;

public class BtnTipsControl : MonoBehaviour {
	public GameObject ObjTips ; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPress (bool isDown)
	{
		if(ObjTips){
			if(isDown)
			{
				ObjTips.SetActive(true);
				//infoBar.transform.Translate (infoBar.transform.up);
				//Debug.Log("-------------------" + ItemID);
				ObjTips.transform.localPosition=new Vector3(0f,0f,0f);
			}
			else
			{
				ObjTips.transform.position=new Vector3(0,-999999,0);
				
				ObjTips.gameObject.SetActive (false);
			}
			
		}
	}
	
	void OnDrag (Vector2 delta)
	{
		if(ObjTips&&ObjTips.gameObject.activeSelf&&Mathf.Abs ( delta.y)>10)
		{
			ObjTips.transform.position=new Vector3(0,-999999,0);
			
			ObjTips.gameObject.SetActive (false);
		}
	}
}
