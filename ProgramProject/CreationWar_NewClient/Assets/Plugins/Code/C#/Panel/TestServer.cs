using UnityEngine;
using System.Collections;

public class TestServer : MonoBehaviour {

    public UIInput txtServerIP;
    public UILabel lblNowServer;
    public UILabel lblNowServerStatus;
    public BtnManager btnManager;
    public MainMenuManage mmManage;
    public TableRead tableRead;

    void OnEnable()
		
    {   
		PlayerPrefs.SetString("TestServer","117.131.207.219");
        txtServerIP.text = PlayerPrefs.GetString("TestServer");
    }
	
	// Update is called once per frame
	void Update () {
        lblNowServer.text = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.ServerAddress;
       
        if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.PeerState==ExitGames.Client.Photon.PeerStateValue.Connected)
        {
            lblNowServerStatus.text = StaticLoc.Loc.Get("info351");
        }
        else
        {
            lblNowServerStatus.text = StaticLoc.Loc.Get("info352");
        }
        
	}

   

    IEnumerator Connect()
    {
      
        if (txtServerIP.text != "")
        {
            //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.Disconnect();
            //while (YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.PeerState != ExitGames.Client.Photon.PeerStateValue.Disconnected)
            //{
            //    yield return new WaitForSeconds(1);
            //}
            //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerAddress = txtServerIP.text + ":5059";
            //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().Connect();
            //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.Disconnect();
            //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer = new ExitGames.Client.Photon.PhotonPeer(YuanUnityPhoton.GetYuanUnityPhotonInstantiate());
            //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.Connect(txtServerIP.text+":5059", "YuanPhotonServerRoom");
            //InRoom.GetInRoomInstantiate().peer.Disconnect();
            //InRoom.GetInRoomInstantiate().peer = new ExitGames.Client.Photon.PhotonPeer(InRoom.GetInRoomInstantiate());

            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.Disconnect();
            YuanUnityPhoton.NewYuanUnityPhotonInstantiate().ServerAddress = txtServerIP.text + ":5059";
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().MMManage = this.mmManage;
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead = this.tableRead;
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().Connect();
            InRoom.GetInRoomInstantiate().peer.Disconnect();
            InRoom.NewInRoomInstantiate();
            //InRoom.GetInRoomInstantiate().Connect();

            ServerSettings.DefaultServerAddress = txtServerIP.text + ":5055";
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.connectionState != ConnectionState.Disconnected)
            {
                yield return new WaitForSeconds(0.5f);
            }
            PhotonNetwork.Connect(txtServerIP.text, 5055, "Master", "1.0");
            PlayerPrefs.SetString("TestServer", txtServerIP.text);
            PlayerPrefs.SetString("TestServerName", "YuanPhotonServerRoom");

        }
    }

    void Close()
    {
        yuan.YuanClass.SwitchListOnlyOne(btnManager.listMenu, 7, true, true);
    }
}
