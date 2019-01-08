using UnityEngine;
using System.Collections;

public class QHcallback : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
		/// <summary>
	/// Gets the qh code.
	/// 切换帐户回调，当接受到 code==qh 时执行切换帐号的逻辑
	/// </summary>
	/// <param name='code'>
	/// Code.
	/// </param>
	void getQhCode(string code){
		if(code == "qh")
		GoMainMenuOKSDK();
		//info = code;
	}
	
	
	
	public void GoMainMenuOKSDK()
    {

        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
        MainMenuManage.gameLoginType = MainMenuManage.GameLoginType.MainMenu;
        if (!YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerConnected)
        {
            YuanUnityPhoton.NewYuanUnityPhotonInstantiate().ServerAddress = PlayerPrefs.GetString("TestServer")+":5059";
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerApplication = PlayerPrefs.GetString("TestServerName");
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().Connect();
        }
		SendMessage("SongLoadOut" , SendMessageOptions.DontRequireReceiver);
    }

}
