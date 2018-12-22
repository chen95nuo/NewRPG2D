using UnityEngine;
using System.Collections;

public class ActSevenCell : MonoBehaviour, ProcessResponse {

    public UISprite texDayNum;
    public UISprite texButPrize;
    public UILabel labButPrize;

    public GameObject objPos;

    int dayNum;         //1-7//
    int state;          //0可以领取 1已领取 2不可领取//
    int activityId;     //activity id from ActivityElement//
    ActSevenPanel sevenPanel;//更新mark标志用//
    SevenDaysData dayData;

    int messType;
    bool receiveData = false;
    int errorCode = -1;

    LoginSevenDayResJson sevenResJson;
    static int rowItemMaxNum = 0;   //为了对齐Item,所有ActSevenCell用同一个值//

    const string ItemIconPath = "Prefabs/UI/UI-Shop/ItemIcon";
	
	GameObject rewardObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (receiveData)
        {
            receiveData = false;
			if(errorCode == -3)
				return;
			
			
            if (errorCode == 0)
            {
                state = 1;
                setButPrizeColor();

				//talkingdata记录开服7日奖励//
				SendToTD();
				//记录获得卡牌//
				SendUnitSkillNeedCard();
                //保存金币水晶体力并刷新//
                saveLevelPrize();
                HeadUI.mInstance.refreshPlayerGold();

                //refresh icon mark//
                sevenPanel.RefreshTopMark();

                //success tips
                //ToastWindow.mInstance.showText(TextsData.getData(652).chinese);
				rewardObj.SetActive(true);
				//Set cometype as "1" to show seven days infomation.
				rewardObj.GetComponent<RewardPanel>().InitRewardPanel(1,dayNum);
            }
            else if (errorCode == 94)
            {
                ToastWindow.mInstance.showText(TextsData.getData(370).chinese);//礼包已经领取//
            }
            else if (errorCode == 95)
            {
                ToastWindow.mInstance.showText(TextsData.getData(620).chinese);//礼包暂不能领取//
            }
            else if (errorCode == 113)
            {
                ToastWindow.mInstance.showText(TextsData.getData(620).chinese);//活动已结束//
            }
            else if (errorCode == 114)
            {
                ToastWindow.mInstance.showText(TextsData.getData(620).chinese);//活动未开始//
            }
        }
    }

    public void Init(int dayNum, int state, int activityId, ActSevenPanel sevenPanel,GameObject rObj)
    {
        this.dayNum = dayNum;
        this.state = state;
        this.activityId = activityId;
        this.sevenPanel = sevenPanel;
		this.rewardObj = rObj;

        dayData = SevenDaysData.getData(dayNum);
        
        string sName = texDayNum.spriteName;
        texDayNum.spriteName = sName.Substring(0, sName.Length - 1) + dayData.days;
        texDayNum.MakePixelPerfect();

        //init prize
        Object res = Resources.Load(ItemIconPath);
        int itemWidth = 80;
        int startPosX = itemWidth * (4 - getRowItemMaxNum()) / 2;
        for (int i = 0; i < dayData.goodsType.Count; i++)
        {
            if (i >= 4) break;  //策划表格给了6个值,但是美术出图是4个位置,后策划规定只要4个值,表未改//

            GameObject obj = Instantiate(res) as GameObject;
            obj.transform.parent = objPos.transform;
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            obj.name = res.name + i;

            obj.transform.localPosition = new Vector3(startPosX + i * itemWidth, 0, 0);

            obj.GetComponent<ItemIcon>().Init(dayData.goodsType[i], dayData.goodsIds[i], dayData.goodsNum[i], ItemIcon.ItemUiType.NameDownNum);
        }

        setButPrizeColor();
		
		res = null;
		Resources.UnloadUnusedAssets();
    }

    public void receiveResponse(string json)
    {
        if (json == null) return;

        receiveData = true;
        PlayerInfo.getInstance().isShowConnectObj = false;

        if (messType == 1)
        {
            LoginSevenDayResJson res = JsonMapper.ToObject<LoginSevenDayResJson>(json);
            sevenResJson = res;
            errorCode = sevenResJson.errorCode;
        }
    }

    public int State
    {
        get { return state; }
    }

    void OnBtnPrize(int param)
    {
        messType = 1;

        UIJson json = new UIJson();
        json.UIJsonForSevenPrize(STATE.UI_SEVEN_BUT, dayNum, activityId);
        PlayerInfo.getInstance().sendRequest(json, this);
    }

    void setButPrizeColor()
    {
        switch (state)
        {
            case 1:
                labButPrize.text = TextsData.getData(397).chinese;
                texButPrize.color = Color.grey; 
                break;
            case 2:
                texButPrize.color = Color.grey;
                break;
        }
    }

    int getRowItemMaxNum()
    {
        if (rowItemMaxNum == 0)
        {
            foreach (SevenDaysData data in SevenDaysData.getAllData().Values)
            {
                int temp = data.goodsType.Count;
                if (temp > rowItemMaxNum)
                    rowItemMaxNum = temp;
            }
        }
        return rowItemMaxNum;
    }
	
	//talkingdata记录开服7日奖励//
	public void SendToTD()
	{
		if(!TalkingDataManager.isTDPC )
		{
			for(int i = 0;i< dayData.goodsType.Count; i++)
			{
				if(dayData.goodsType[i] == 8)		//钻石//
				{
					TDGAVirtualCurrency.OnReward(dayData.goodsNum[i], "sevendayaward");
				}
			}
		}
		
	}
	
	public void SendUnitSkillNeedCard()
	{
		//添加卡牌获得统计@zhangsai//
		for(int i = 0;i< dayData.goodsType.Count; i++)
		{
			if(dayData.goodsType[i] == 3)		//卡牌//
			{
				int id = dayData.goodsIds[i];
				if(!UniteSkillInfo.cardUnlockTable.ContainsKey(id))
				UniteSkillInfo.cardUnlockTable.Add(id,true);
			}
		}
	}
	

    void saveLevelPrize()
    {
        for (int i = 0; i < dayData.goodsType.Count; i++)
        {
            switch (dayData.goodsType[i])
            {
                case 6:
                    PlayerInfo.getInstance().player.gold += dayData.goodsNum[i];
                    break;
                case 8:
                    PlayerInfo.getInstance().player.crystal += dayData.goodsNum[i];
                    break;
                case 10:
                    PlayerInfo.getInstance().player.power += dayData.goodsNum[i];
                    break;
            }
        }
    }
}
