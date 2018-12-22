using UnityEngine;
using System.Collections;

public class ActWheelCell : MonoBehaviour,ProcessResponse {
	
	public GameObject activityPanel;
	
	public UISprite icon;
	public UISprite nameSprite;
	public UISprite numSprite;
	//public UILabel textLabel;
	public UILabel numLabel;
	
	int cellID;
	int curActiveType;
	//当前的档位//
	int curActiveRun;
	//当前档位的索引//
	int curIndex;
	
	GameObject wheelPanel;
	//GameObject noWheelPanel;
	
	//当前类型可转动次数//
	int rotNum;
	
	bool receiveData = false;
    int errorCode = -1;
	 int requestType = 0;
	
	RunnerResultJson runnerResultJson;
	
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
				//活动已结束//
				if (errorCode == 113)
                {
                    string errorMsg = TextsData.getData(628).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
				//活动未开始//
				if (errorCode == 114)
                {
                    string errorMsg = TextsData.getData(629).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
//				//没有抽奖机会//
//				if (errorCode == 127)
//                {
//                    string errorMsg = TextsData.getData(699).chinese;
//                    ToastWindow.mInstance.showText(errorMsg);
//                }
				return;
			}
			switch (requestType)
            {
                case 1:
				{
					wheelPanel.SetActive(true);
					wheelPanel.GetComponent<WheelPanel>().InitWheelPanel(activityPanel,runnerResultJson,cellID,rotNum,curActiveType,curActiveRun,curIndex);
				}
                break;
			}
            
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
						RunnerResultJson rJson = JsonMapper.ToObject<RunnerResultJson>(json);
				Debug.Log("RunnerResultJson:"+json);
                        errorCode = rJson.errorCode;
                        if (errorCode == 0)
                        {
                            runnerResultJson = rJson;
                        }
                        receiveData = true;
                    }
                    break;
			}
		}
	}
	
	public void Init(int id,int activeType,GameObject actPanel,GameObject wheelObj,GameObject noWheelObj,int rotNum,int index)
	{
		this.cellID = id;
		this.curActiveType = activeType;
		this.activityPanel = actPanel;
		this.wheelPanel = wheelObj;
		//this.noWheelPanel = noWheelObj;
		this.rotNum = rotNum;
		this.curIndex = index;
		numLabel.text = rotNum.ToString()+TextsData.getData(269).chinese;
		
		RunnerData rData = RunnerData.getData(cellID);
		this.curActiveRun = rData.condition;
		icon.spriteName = rData.icon;
		//textLabel.text = rData.text;
		switch(index)
		{
		case 0:
			gameObject.GetComponent<UISprite>().spriteName = "fengche021";
			nameSprite.spriteName = "fengche04";
			numSprite.spriteName = "fengche011";
			break;
		case 1:
			gameObject.GetComponent<UISprite>().spriteName = "fengche022";
			nameSprite.spriteName = "fengche05";
			numSprite.spriteName = "fengche012";
			break;
		case 2:
			gameObject.GetComponent<UISprite>().spriteName = "fengche023";
			nameSprite.spriteName = "fengche06";
			numSprite.spriteName = "fengche013";
			break;
		}
	}
	
	void OnClickCell()
	{
		requestType = 1;
		//Debug.Log("cellid:"+cellID);
		//请求上次转动获得物品情况和是否领取标识//
		UIJson uijson = new UIJson();
		uijson.UIJsonForActWheel(STATE.UI_ACT_GEAR_WHEEL,cellID,1);
		PlayerInfo.getInstance().sendRequest(uijson, this);
		
//		if(rotNum > 0)
//		{
//			
//			
//		}
//		else
//		{
//			noWheelPanel.SetActive(true);
//			noWheelPanel.GetComponent<NoWheelPanel>().InitNoWheelPanel(activityPanel,cellID,numLabel.text,curActiveType,curIndex);
//		}
	}
}
