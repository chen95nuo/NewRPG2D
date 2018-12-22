using UnityEngine;
using System.Collections;

public class RuleCell : MonoBehaviour,ProcessResponse {
	
	public GameObject itemIcon;
	public UILabel descLabel;
	public UISprite rewardBtn;
	public UISprite rewardSprite;
	public UISprite npSprite;
	public UISprite labelSprite;
	
	int cellID;
	
	bool receiveData = false;
    int errorCode = -1;
	 int requestType = 0;
	
	//RunRewardResultJson rrrj;
	
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
				//已经领取过该奖励//
				if (errorCode == 91)
                {
                    string errorMsg = TextsData.getData(703).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
				//当前转动转盘次数不足，不能领取//
				if (errorCode == 128)
                {
                    string errorMsg = TextsData.getData(702).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
				return;
			}
			switch (requestType)
            {
                case 1:
				{
					Debug.Log("Get reward success!");
					UpdateRuleCellState();
				}
                break;
			}
		}
	
	}
	
	public void Init(int id,int state,int num)
	{
		this.cellID = id;
		Debug.Log("rule cell id:"+cellID);
		RunData tempData = RunData.getData(cellID);
		itemIcon.GetComponent<ItemIcon>().Init(tempData.rewardtype,tempData.rewardId,tempData.rewardNum,ItemIcon.ItemUiType.NameDownNum);
		//防止服务器出的数据错误，小于0就设置为0//
		if(num < 0)
		{
			descLabel.text = 0 + "";
		}
		else
		{
			descLabel.text = num.ToString();
		}
		
		//descLabel.text = tempData.turntime.ToString();
		//descLabel.text = TextsData.getData(698).chinese.Replace("num",tempData.turntime.ToString());
		if(state == -1)
		{
			rewardSprite.spriteName = "lingqu";
			rewardSprite.MakePixelPerfect();
			rewardBtn.spriteName = "fengche0001";
			rewardBtn.color = Color.gray;
			rewardBtn.GetComponent<UIButtonMessage>().target = null;
		}
		else
		{
			if(state == 0)
			{
				rewardBtn.color = Color.white;
				//rewardLabel.text = TextsData.getData(701).chinese;
				rewardSprite.spriteName = "weilingqu";
				rewardSprite.MakePixelPerfect();
				rewardBtn.GetComponent<UIButtonMessage>().target = gameObject;
			}
			else
			{
				rewardSprite.spriteName = "yilingqu";
				rewardSprite.MakePixelPerfect();
				//rewardLabel.text = TextsData.getData(397).chinese;
				rewardBtn.alpha = 0;
				rewardBtn.GetComponent<UIButtonMessage>().target = null;
				rewardSprite.transform.localPosition = new Vector3(-140,0,0);
				descLabel.text = "";
				labelSprite.gameObject.SetActive(false);
				npSprite.gameObject.SetActive(false);
				itemIcon.SetActive(false);
			}
			
			rewardBtn.GetComponent<UIButtonMessage>().param = cellID;
		}
	}
	
	void OnRewardBtn(int param)
	{
		requestType = 1;
		UIJson uijson = new UIJson();
		uijson.UIJsonForWheelReward(STATE.UI_ACT_WHEEL_REWARD,param);
		PlayerInfo.getInstance().sendRequest(uijson, this);
	}
	
	void UpdateRuleCellState()
	{
		rewardBtn.alpha = 0;
		rewardSprite.spriteName = "yilingqu";
		rewardSprite.MakePixelPerfect();
		//rewardLabel.text = TextsData.getData(397).chinese;
		rewardBtn.GetComponent<UIButtonMessage>().target = null;
		rewardSprite.transform.localPosition = new Vector3(-140,0,0);
		descLabel.text = "";
		labelSprite.gameObject.SetActive(false);
		npSprite.gameObject.SetActive(false);
		itemIcon.SetActive(false);
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
						RunRewardResultJson rJson = JsonMapper.ToObject<RunRewardResultJson>(json);
						Debug.Log("RunRewardResultJson:"+json);
                        errorCode = rJson.errorCode;
                        if (errorCode == 0)
                        {
                           // rrrj = rJson;
                        }
                        receiveData = true;
                    }
                    break;
			}
		}
	}
}
