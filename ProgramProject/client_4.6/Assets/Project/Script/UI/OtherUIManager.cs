using UnityEngine;
using System.Collections;

public class OtherUIManager : BWUIPanel ,ProcessResponse{
	public static OtherUIManager mInstance;
	
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
//	public UILabel QualityLabel;
	
	void Awake(){
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
	}
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:
//				FriendUI.mInstance.show();
//				MainMenuManager.mInstance.hide();
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				hide();
				break;
			}
		}
	}
	
	public void OnClickBtn_Other(int param){
		switch(param){
		case 0:						//帮助//
		case 1:						//联系我们//
		case 2:						//激活码兑换//		
		case 3:						//设置选项//
//			UIToastTipManager.mInstance.show();
			string str = TextsData.getData(142).chinese;
			ToastWindow.mInstance.showText(str);
			break;
		case 4://好友//
			requestType=1;
			PlayerInfo.getInstance().sendRequest(new FriendJson(0,0),this);
			break;
		}
	}
	
	public void OnBackBtn(){
//		MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager")as MainMenuManager;
		if(main!= null)
		{
			main.SetData(STATE.ENTER_MAINMENU_BACK);
		}
		hide();
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				FriendResultJson frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				if(errorCode == 0)
				{
//					FriendUI.mInstance.frj=frj;
				}
				receiveData=true;
				break;
			}
		}
	}
	
//	int ii = 0;
//	
//	public void ChangeQuality(){
//		string[] names = QualitySettings.names;
//		if (ii < names.Length) {
//			QualitySettings.SetQualityLevel(ii, true);
//			ii++;
//			if (ii >= names.Length){
//				ii = 0;
//			}
//        }
//		QualityLabel.text = names[ii];
//	}
	
}
