using UnityEngine;
using System.Collections;

public class LableLink : MonoBehaviour {
	
	//[HideInInspector]
	public string id="";
	[HideInInspector]
	public BoxCollider clickCollider;
	//[HideInInspector]
	public UIPanel infoBar;
	public UISprite myPic;
	public bool isEnable;
	// Use this for initialization
	void Awake()
	{
		clickCollider=GetComponent<BoxCollider>();
		if(clickCollider==null)
		{
		clickCollider= gameObject.AddComponent<BoxCollider>();
		}
		clickCollider.size=Vector3.zero;		
	}
	
	void Start () {

	}
	

	
	void OnClick()
	{
		if(id!=""&&infoBar!=null)
		{
			Debug.Log ("LableLink!!!!!!!"+id);
			infoBar.enabled=true;
			infoBar.gameObject.SetActiveRecursively (true);
			infoBar.transform.localPosition=new Vector3(-0.2875011f,100.1449f,-5.680656f);
			//infoBar.transform.Translate (infoBar.transform.up);
			infoBar.SendMessage("SetItemID",id,SendMessageOptions.DontRequireReceiver);
		}
	}
}
