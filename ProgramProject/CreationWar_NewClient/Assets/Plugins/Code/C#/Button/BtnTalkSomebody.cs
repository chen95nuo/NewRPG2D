using UnityEngine;
using System.Collections;

public class BtnTalkSomebody : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    [HideInInspector]
    public string[] listSomeBody = new string[1];
    [HideInInspector]
    public string playerID = string.Empty;
    public GameObject btnSend;
    void OnClick()
    {
        listSomeBody[0] = playerID;
        //btnSend.SendMessage("SendTalkSomeBody", listSomeBody, SendMessageOptions.DontRequireReceiver);
    	PanelStatic.StaticBtnGameManager.SendTalkSomeBody (listSomeBody);
    }
    
}
