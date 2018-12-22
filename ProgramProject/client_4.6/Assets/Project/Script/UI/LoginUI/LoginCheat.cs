using UnityEngine;
using System.Collections;

public class LoginCheat : MonoBehaviour ,ProcessResponse{
	
	public GameObject cheatInput;
	private int clickNum;
	private bool receiveData;
	
	// Use this for initialization
	void Start () {
		cheatInput.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData=false;
			LoginUI_new.mInstance.show();
			cheatInput.SetActive(false);
		}
	}
	
	public void onClickCheatZone(int param)
	{
		clickNum++;
		if(clickNum>=10)
		{
			clickNum=0;
			cheatInput.SetActive(true);
		}
	}
	
	public void onClickCheatBtn()
	{
		string cheatKey=cheatInput.GetComponent<UIInput>().value;
		if(string.IsNullOrEmpty(cheatKey))
		{
			return;
		}
		RequestSender.serverIp=LoadingControl.ServerCenterIp;
		RequestSender.serverPort=LoadingControl.ServerCenterPort;
		RequestSender.getInstance().request(33,cheatKey,false,this);
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			PlayerInfo.getInstance().isShowConnectObj = false;
			ServerListJson slj=JsonMapper.ToObject<ServerListJson>(json);
			LoginUI_new.mInstance.servers=slj.list;
			receiveData=true;
		}
	}
}
