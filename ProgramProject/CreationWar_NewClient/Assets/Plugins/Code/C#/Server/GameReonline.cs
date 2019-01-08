using UnityEngine;
using System.Collections;

public class GameReonline : MonoBehaviour {

	public static Vector3 playerPostion;
	public static string mapID=string.Empty;

	// Use this for initialization
	void Start () {
			my=this;
		isEnable=true;
			 if (InRoom.GetInRoomInstantiate().ServerConnected == false&&isConnecting==false)
	        {
				isStart=true;
	            isConnecting = true;
	            time = 0;
	            reTime = 0;
	            InvokeRepeating("Connect", 0, 1);
                lblConnectInfo.gameObject.active = true;
                lblConnectInfo.text = StaticLoc.Loc.Get("info354");
	        }
	}
	
	public static GameReonline my;
    public SendManager sendManager;
    public BtnGameManagerBack btnGameManagerBack;
    public GameObject uiCon;
    public UILabel lblConnectInfo;

    private bool isConnecting = false;
    private int time = 0;
    private int reTime = 0;
	private static bool isStart=false;
	public static bool isEnable=true;

	// Update is called once per frame
	float ptime= 0;
	void Update () {
		if(Time.time > ptime + 10)
		{
			if(isEnable)
			{
		        if (InRoom.GetInRoomInstantiate().ServerConnected == false&&isConnecting==false)
		        {
		            isConnecting = true;
		            time = 0;
		            reTime = 0;
		            InvokeRepeating("Connect", 3, 0.05f);
	                lblConnectInfo.gameObject.active = true;
	                lblConnectInfo.text = StaticLoc.Loc.Get("info354");
		        }
			}
		}
//        Debug.Log("房间名称："+ PhotonNetwork.room.name);
	}
	
    void Connect()
    {
		if(isEnable)
		{
	        if (InRoom.GetInRoomInstantiate().ServerConnected)
	        {
	            InRoom.GetInRoomInstantiate().SendID(BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText, BtnGameManager.yt.Rows[0]["ProID"].YuanColumnText, BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText, true, PlayerPrefs.GetString("Language", "CH"), SystemInfo.deviceUniqueIdentifier, PlayerUtil.mapInstanceID, playerPostion.x, playerPostion.y, playerPostion.z);//,BtnGameManagerBack.teaminstensid);
	            isConnecting = false;
	            lblConnectInfo.text = "";
	            lblConnectInfo.gameObject.active = false;
	            CancelInvoke("Connect");
	        
	        }
			//Debug.Log ("------------------:"+time%5);
	        if ((time % 100 )== 0||time==0)
	        {
				ZealmConnector.closeConnection();
	            //InRoom.GetInRoomInstantiate().peer.Disconnect();
				//while(InRoom.GetInRoomInstantiate ().peer.PeerState!=ExitGames.Client.Photon.PeerStateValue.Disconnected)
				//{
				//	yield return new WaitForSeconds(0.1f);
				//}	
				try
				{			
		            PhotonHandler.ShowLog("GameReonline");
					InRoom.NewInRoomInstantiate().SetAddress(PlayerPrefs.GetString("InAppServerIP"));
		            InRoom.GetInRoomInstantiate().ServerApplication = PlayerPrefs.GetString("InAppServer");
		            InRoom.GetInRoomInstantiate().btnGameManagerBack = this.btnGameManagerBack;
		            InRoom.GetInRoomInstantiate().SM = this.sendManager;
					InRoom.GetInRoomInstantiate ().Connect ();
		            reTime++;
				}
				catch(System.Exception ex)
				{
					Debug.LogError (ex.ToString ());
				}
	        }
	        if (reTime >= 5)
	        {
				if(isStart)
				{
		            //uiCon.SendMessage("UIDisconnect", SendMessageOptions.DontRequireReceiver);
					lblConnectInfo.text = StaticLoc.Loc.Get("info356");
				}
				else
				{
					lblConnectInfo.text = StaticLoc.Loc.Get("info357");
				}
				CancelInvoke("Connect");
				
				isStart=false;
				//PanelStatic.StaticWarnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info649"));
				PanelStatic.StaticBtnGameManager.OffLine();
	        }
	        time++;
		}
    }
}
