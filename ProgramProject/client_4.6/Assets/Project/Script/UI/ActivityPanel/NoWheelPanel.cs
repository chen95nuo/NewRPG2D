using UnityEngine;
using System.Collections;

public class NoWheelPanel : MonoBehaviour,ProcessResponse {
	
	public GameObject activityPanel;
	
	public UILabel nameLabel;
	public UILabel textLabel;
	public UILabel btnLabel;
	
	int cellID;
	int activeType;
	
	//充值界面的json//
	RechargeUiResultJson curVIPRJ;
	//阵容界面的json//
	CardGroupResultJson cardGroupRJ;
	//合成界面的json//
	ComposeResultJson composeRJ;
	//抽卡界面的json//
	LotResultJson lotRJ;
	
	bool receiveData = false;
    int errorCode = -1;
	 int requestType = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (receiveData)
        {
            receiveData = false;
			if(errorCode == -3)
				return;
			
			if (errorCode != 0)
            {
				if (errorCode == 96)
                {
                    string errorMsg = TextsData.getData(385).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
				return;
			}
			switch (requestType)
            {
                case 1:
                {
				activityPanel.GetComponent<ActivityPanel>().CloseActivityPanelOnly();
					//Charge logic
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
		        ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ,
		                "ChargePanel") as ChargePanel;
		        charge.curRechargeResult = curVIPRJ;
				//如果vipCost是0表示没有充值过，是第一次充值//
				if(curVIPRJ.vipCost == 0)
				{
					charge.firstCharge = 0;
				}
				else
					charge.firstCharge = curVIPRJ.vipCost;
		        charge.isShowType = 0;
		        charge.show();
				}
				break;
			case 2:
			{
				activityPanel.GetComponent<ActivityPanel>().CloseActivityPanelOnly();
				//cardgroup logic
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
				CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,"CombinationInterManager")as CombinationInterManager;
				combination.curCardGroup=cardGroupRJ .transformCardGroup();
				PlayerInfo.getInstance().curCardGroup = combination.curCardGroup;
				combination.SetData(1);
				
				cardGroupRJ = null;
				
				//删除主场景界面//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
			}
			break;
			case 3:
			{
				activityPanel.GetComponent<ActivityPanel>().CloseActivityPanelOnly();
				
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE);
				ComposePanel compose = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE, 
					"ComposePanel") as ComposePanel;
				
				
				if(compose != null)
				{
					compose.newItemIDList.Clear();
					compose.crj = composeRJ;
					compose.packItemInfoList = compose.crj.cs;

                    compose.mark = composeRJ.mark;

                    compose.GetTip();
					compose.setComposePage(ComposePanel.PageType.E_ComposePackPage);
					compose.setComposeType(ComposePanel.ComposeType.E_Equip);
					compose.show();
				}
				
				composeRJ = null;
				
				//删除主场景界面//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
			}
			break;
			case 4:
			{
				activityPanel.GetComponent<ActivityPanel>().CloseActivityPanelOnly();
				
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT);
				LotCardUI lotCard = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT, "LotCardUI")as LotCardUI;
				lotCard.lrj=lotRJ;
                lotCard.freeTimes = lotRJ.t;
				lotCard.show();
				
				lotRJ = null;
				
				//删除主场景界面//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
			}
				break;
			}
		}
	
	}
	
	public void InitNoWheelPanel(GameObject actPanel,int id,int activeType,int index)
	{
		switch(index)
		{
		case 0 :
			nameLabel.text = TextsData.getData(730).chinese;
			break;
		case 1:
			nameLabel.text = TextsData.getData(731).chinese;
			break;
		case 2:
			nameLabel.text = TextsData.getData(732).chinese;
			break;
		}
		this.cellID = id;
		this.activeType = activeType;
		this.activityPanel = actPanel;
		
		RunnerData rData = RunnerData.getData(cellID);
		int condition = rData.condition;
		int time = rData.time;
		textLabel.text = TextsData.getData(696).chinese.Replace("num1",condition.ToString()).Replace("num2",time.ToString());
		
		switch(activeType)
		{
		case 1:
			//去充值//
			btnLabel.text = TextsData.getData(481).chinese;
			//Debug.Log("TextsData.getData(481).chinese:"+TextsData.getData(481).chinese);
			break;
		case 2:
			//去阵容//
			btnLabel.text = TextsData.getData(707).chinese;
			break;
		case 3:
			//去合成//
			btnLabel.text = TextsData.getData(708).chinese;
			break;
		case 4:
			//btnLabel.text = TextsData.getData(481).chinese;
			break;
		case 5:
			//去抽卡//
			btnLabel.text = TextsData.getData(709).chinese;
			break;
		case 6:
			//btnLabel.text = TextsData.getData(481).chinese;
			break;
		case 7:
			//btnLabel.text = TextsData.getData(481).chinese;
			break;
		case 8:
			//btnLabel.text = TextsData.getData(481).chinese;
			break;
			
		}
	}
	
	void CloseNoWheelPanel(int param)
	{
		gameObject.SetActive(false);
		Resources.UnloadUnusedAssets();
	}
	
	void OnToBtn(int param)
	{
		Debug.Log("param:"+param);
		switch(activeType)
		{
		case 1:
			//去充值//
			requestType = 1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE),this);
			
//			requestType = 2;
//			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0),this);
			
//			requestType = 3;
//			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)ComposePanel.ComposeType.E_Equip),this);
//			
//			requestType = 4;
//			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_LOT),this);
			
			break;
		case 2:
			//去阵容//
			requestType = 2;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0),this);
			break;
		case 3:
			//去合成//
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)ComposePanel.ComposeType.E_Equip),this);
			break;
		case 4:
			break;
		case 5:
			//去抽卡//
			requestType = 4;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_LOT),this);
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		}
	}
	
	public void receiveResponse(string json)
    {
        if (json != null)
        {
            //关闭连接界面的动画//
            PlayerInfo.getInstance().isShowConnectObj = false;
            switch (requestType)
            {
                case 1:
                    {
                        RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
                        errorCode = rechargej.errorCode;
                        if (errorCode == 0)
                        {
                            curVIPRJ = rechargej;
                        }
                        receiveData = true;
                    }
                    break;
			case 2:
				{
					CardGroupResultJson cgrj = JsonMapper.ToObject<CardGroupResultJson>(json);
	                errorCode = cgrj.errorCode;
	                if (errorCode == 0)
	                {
	                    cardGroupRJ = cgrj;
	                }
	                receiveData = true;
				}
				break;
			case 3:
				{
					ComposeResultJson crj = JsonMapper.ToObject<ComposeResultJson>(json);
                    errorCode = crj.errorCode;
					if(errorCode == 0)
					{
						composeRJ = crj;
					}
                    receiveData = true;
				}
				break;
			case 4:
				{
					LotResultJson lrj = JsonMapper.ToObject<LotResultJson>(json);
					errorCode = lrj.errorCode;
					if(errorCode == 0)
					{
						lotRJ = lrj;
					}
                    receiveData = true;
				}
				break;
			}
		}
	}
}
