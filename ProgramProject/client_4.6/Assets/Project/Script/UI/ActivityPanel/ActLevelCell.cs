using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActLevelCell : MonoBehaviour, ProcessResponse
{

    public NumberPic levelNum;
    public UISprite texButPrize;
    public UILabel labButPrize;
    public GameObject objGoCopy;

    public GameObject objPos;

    int levelPrizeId;   //1-26//
    int state;          //0可以领取 1已领取 2不可领取//
    int activityId;     //activity id from ActivityElement//
    ActLevelPanel levelPanel;//更新mark标志用//
    LevelGiftData levelData;

    int messType;
    bool receiveData = false;
    int errorCode = -1;

    LoginSevenDayResJson sevenResJson;
    static int rowItemMaxNum = 0;   //为了对齐Item,所有ActLevelCell用同一个值//

    const string ItemIconPath = "Prefabs/UI/UI-Shop/ItemIcon";
	
	GameObject rewardObj;

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

            if (errorCode == 0)
            {
                state = 1;
                setButPrizeColor();

                //talkingdata记录等级奖励//
                SendToTD();
				
				//记录获得卡牌//
				SendUnitSkillNeedCard();
				
                //保存金币水晶体力并刷新//
                saveLevelPrize();
                HeadUI.mInstance.refreshPlayerGold();

                //refresh icon mark//
                levelPanel.RefreshTopMark();

                //success tips
                //ToastWindow.mInstance.showText(TextsData.getData(652).chinese);
				rewardObj.SetActive(true);
				//Set cometype as "2" to show level reward infomation.
				rewardObj.GetComponent<RewardPanel>().InitRewardPanel(2,levelPrizeId);
            }
            else if (errorCode == 57)
            {
                ToastWindow.mInstance.showText(TextsData.getData(630).chinese);//等级不足不能领取//
            }
            else if (errorCode == 91)
            {
                ToastWindow.mInstance.showText(TextsData.getData(370).chinese);//礼包已经领取//
            }
        }
	}

    public void Init(int levelPrizeId, int state, int activityId, ActLevelPanel levelPanel,GameObject rObj)
    {
        this.levelPrizeId = levelPrizeId;
        this.state = state;
        this.activityId = activityId;
        this.levelPanel = levelPanel;
		this.rewardObj = rObj;

        levelData = LevelGiftData.getData(levelPrizeId);

        levelNum.GetComponent<NumberPic>().setNum(levelData.level);

        //init prize
        Object res = Resources.Load(ItemIconPath);
        int itemWidth = 90;
        int startPosX = itemWidth * (6 - getRowItemMaxNum()) / 2;
        for (int i = 0; i < levelData.goodsType.Count; i++)
        {
            GameObject obj = Instantiate(res) as GameObject;
            obj.transform.parent = objPos.transform;
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            obj.name = res.name + i;

            obj.transform.localPosition = new Vector3(startPosX + i * itemWidth, 0, 0);

            obj.GetComponent<ItemIcon>().Init(levelData.goodsType[i], levelData.goodsIds[i], levelData.goodsNum[i], ItemIcon.ItemUiType.NameDownNum);
        }

        setButPrizeColor();
		
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
        json.UIJsonForSevenPrize(STATE.UI_LEVEL_BUT, levelPrizeId, activityId);
        PlayerInfo.getInstance().sendRequest(json, this);
    }

    void OnBtnCopy(int param)
    {
        Main3dCameraControl.mInstance.SetBool(false);
        UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE);

        GameObject.Find("MainMenuPanel").GetComponent<MainMenuManager>().openMap();
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
                texButPrize.transform.gameObject.SetActive(false);
                objGoCopy.SetActive(true);
                break;
        }
    }

    int getRowItemMaxNum()
    {
        if (rowItemMaxNum == 0)
        {
            foreach (LevelGiftData data in LevelGiftData.getAllData().Values)
            {
                int temp = data.goodsType.Count;
                if (temp > rowItemMaxNum)
                    rowItemMaxNum = temp;
            } 
        }
        return rowItemMaxNum;
    }
	
	//talkingdata记录等级奖励//
	public void SendToTD()
	{
		if(!TalkingDataManager.isTDPC )
		{
			for(int i = 0;i< levelData.goodsType.Count; i++)
			{
				if(levelData.goodsType[i] == 8)		//钻石//
				{
					TDGAVirtualCurrency.OnReward(levelData.goodsNum[i], "levelgift");
				}
			}
		}
		
	}

	public void SendUnitSkillNeedCard()
	{
		//添加卡牌获得统计@zhangsai//
		for(int i = 0;i< levelData.goodsType.Count; i++)
		{
			if(levelData.goodsType[i] == 3)		//卡牌//
			{
				int id = levelData.goodsIds[i];
				if(!UniteSkillInfo.cardUnlockTable.ContainsKey(id))
				UniteSkillInfo.cardUnlockTable.Add(id,true);
			}
		}
	}

    void saveLevelPrize()
    {
        for (int i = 0; i < levelData.goodsType.Count; i++)
        {
            switch (levelData.goodsType[i])
            {
                case 6:
                    PlayerInfo.getInstance().player.gold += levelData.goodsNum[i];
                    break;
                case 8:
                    PlayerInfo.getInstance().player.crystal += levelData.goodsNum[i];
                    break;
                case 10:
                    PlayerInfo.getInstance().player.power += levelData.goodsNum[i];
                    break;
            }
        }
    }
}
