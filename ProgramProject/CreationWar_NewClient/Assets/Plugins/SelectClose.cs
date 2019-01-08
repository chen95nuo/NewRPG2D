using UnityEngine;
using System.Collections;

public class SelectClose : MonoBehaviour {
	public GameObject Obj;


	public static SelectClose select;

	void Awake()
	{
		select = this;
	}
//	void OnEnable(){
//		OnSelect(true);
//	}
//	void OnSelect (bool isSelected)
//	{
//		if(isSelected){
//			Obj.SetActive(true);
//		}else{
//			Obj.SetActive(false);
//		}
//		
//	}

	public void SetBool(){
		isSelect = true;
	}

	bool isSelect = false;
	void OnPress(bool press){
		isSelect = true;
	}

	void Update(){
		if(Input.GetMouseButtonUp(0)){
			if(!isSelect){
				 Obj.SetActive(false);
			}
			if(isSelect){
				isSelect = false;
			}
		}
	}
}
