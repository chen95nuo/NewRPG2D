using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class socketconnect : MonoBehaviour {


	Socket clientSocket;
	// Use this for initialization
	void Start () {
        Connect();
	}
	
	// Update is called once per frame
	void Update () {
        /*
        ZMNetData nd = new ZMNetData(OpCode.ZM_MSG_TEST);
        nd.writeInt(123);
        nd.writeString("abcdefg");
        nd.writeLong(88888);
        nd.writeDouble(1.1);
        nd.writeUnsignedShot(55);
        ZealmConnector.sendRequest(nd);
         * */
     //   NetDataManager.update();
	}

    void Connect()
    {
        ZealmConnector.createConnection("192.168.1.121");  
    }
   
    void OnGUI()
    {
        if(GUI.Button(new Rect(100,20,700,500),"dddd"))
        {

        
        }
    }
}


