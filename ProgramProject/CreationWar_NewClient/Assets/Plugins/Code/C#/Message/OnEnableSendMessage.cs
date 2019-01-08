using UnityEngine;
using System.Collections;

public class OnEnableSendMessage : MonoBehaviour {


    public GameObject targer;
    public string function;
	
	void Awake()
	{
		targer=PanelStatic.StaticBtnGameManager.gameObject;
	}
	
    void OnEnable()
    {
        targer.SendMessage(function, SendMessageOptions.DontRequireReceiver);
    }
}
