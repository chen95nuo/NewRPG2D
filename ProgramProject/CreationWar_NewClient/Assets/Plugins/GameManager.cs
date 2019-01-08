using System.Collections;
using UnityEngine;

public class GameManager : Photon.MonoBehaviour
{

	
	public GameObject reLian;
	
	public void Start(){
		cantTan = false;
		PhotonNetwork.isMessageQueueRunning = true;
//		tishi.SetActiveRecursively(false);
	}

//	void OnLevelWasLoaded(int level) {
//		if (level != 15 && level != 16)
//			Start();
//	}	

	public GameObject respawn;
	public static Vector3 rePosition = new Vector3(0,0,0);
	public static string reLevel;
	public static string rePlayerName;

//    public void OnGUI()
//    {
//        if (GUILayout.Button("Return to Lobby"))
//        {
//            PhotonNetwork.LeaveRoom();
//        }
//    }
	
	public GameObject uicl;
//	public GameObject tishi;
	public static bool cantTan = false;
    public void OnLeftRoom()
    {
		return;
		if(cantTan)
			return;
        Debug.Log("OnLeftRoom (local)");
		if(Application.loadedLevelName.Substring(0,4) != "Load"){
			reLian.SendMessage("SetOffLineActiveAsBool" , true, SendMessageOptions.DontRequireReceiver);
//			tishi.SetActiveRecursively(true);
//			PhotonNetwork.offlineMode = true;
//			PhotonNetwork.offlineMode_inRoom = true;
		}
    }

    public void OnDisconnectedFromPhoton()
    {
		return;
		if(cantTan)
			return;
		Debug.Log("OnDisconnectedFromPhoton");
		if(Application.loadedLevelName.Substring(0,4) != "Load"){
			reLian.SendMessage("SetOffLineActiveAsBool" , true, SendMessageOptions.DontRequireReceiver);
//			tishi.SetActiveRecursively(true);
//			PhotonNetwork.offlineMode = true;
//			PhotonNetwork.offlineMode_inRoom = true;		
		}
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected: " + player);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {   
		bool bol;

		if(!player.isLocal && PhotonNetwork.isMasterClient)
			bol = InRoom.GetInRoomInstantiate().SetPhotonMasterClient(player);
		if(player.isLocal)
		 PhotonNetwork.DestroyPlayerObjects(player);	
    }

	public void OnJoinedRoom()
    { 
//		PhotonNetwork.DoClearYuanList();
		Debug.Log("Joinedroomagain");
	}


    public void OnConnectedToPhoton()
    {
        Debug.Log("OnConnectedToPhoton");
    }

    public void OnFailedToConnectToPhoton()
    {
        Debug.Log("OnFailedToConnectToPhoton");
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate " + info.sender);
    }
}
