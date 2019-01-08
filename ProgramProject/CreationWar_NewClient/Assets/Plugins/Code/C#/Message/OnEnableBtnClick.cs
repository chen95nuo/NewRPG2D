using UnityEngine;
using System.Collections;

public class OnEnableBtnClick : MonoBehaviour {
	
	public GameObject objBtn;
	
	public void OnEnable()
	{
		objBtn.SendMessage ("OnClick",SendMessageOptions.DontRequireReceiver);
	}
	
}
