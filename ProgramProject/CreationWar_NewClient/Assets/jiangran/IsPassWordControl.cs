using UnityEngine;
using System.Collections;

public class IsPassWordControl : MonoBehaviour {
	public UIToggle IsMy;
	public GameObject TagObj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void IsPassWord(){
		if(TagObj!=null){
		if(IsMy.value==true){
			TagObj.SetActive(true);
		}else{
			TagObj.SetActive(false);
		}
	}
	}
}
