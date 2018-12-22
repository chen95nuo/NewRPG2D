using UnityEngine;
using System.Collections;

public class ShowBuyTipControl :MonoBehaviour, ProcessResponse {

	public static ShowBuyTipControl mInstance;
	
	/************************请求购买购买start***************************/
	//购买物品的类型 1 金币， 2 体力， 3 扫荡券， 4 挑战次数, 5 购买冷却时间, 6 竞技场挑战次数购买//
	private int buyType;
	//请求类型,1 请求购买界面信息， 2 购买//
	//private int jsonType;
	//花费类型 ，1 水晶， 2 金币//
	private int costType;
	//购买花费的水晶数//
	private int costCrystal;
	//要购买的金币或体力的个数//
	private int num;
	//当天剩余的购买次数//
	private int times;
	//当前连续扫荡个数//
	private int curSweepNum;
	//当前购买的界面的类型//
	private BuyTipManager.UI_TYPE curUIType;
	//购买挑战次数时的missionId//
	private int missionId;
	//当前购买的冷却时间的类型, 1 pk, 2 maze, 3 event(活动副本)//
	private int curCDType;
	//副本id//
	private int curCopyID;
	/************************请求购买购买end***************************/
	
	//1， 请求购买界面信息//
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
	
	void Awake()
	{
		mInstance = this;
	}
	
	
	void Start()
	{
		
	}
	
	void Update()
	{
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:					//请求购买界面信息//
				if(errorCode == 0)
				{
					BuyTipManager.mInstance.SetData(buyType, costType , costCrystal, num, times, curSweepNum, curUIType, missionId, curCDType, curCopyID);
				}
                else if (errorCode == 81)
                {
                    ToastWindow.mInstance.showText(TextsData.getData(690).chinese);
                }
				else if(errorCode == 70)			//vip登记等级不足//
				{
					string str = TextsData.getData(618).chinese;
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else if(errorCode == 84)			//挑战次数还未用尽//
				{
//					string str = TextsData.getData(617).chinese;
//					//提示去充值//
//					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else if(errorCode == 79)		//购买次数达到上限,根据购买的物品不同，显示的提示内容也不同//
				{
					string str = TextsData.getData(240).chinese;
					switch(buyType)
					{
					case 1:			//金币 508//
						str = TextsData.getData(508).chinese;
						break;
					case 2:			//体力 525//
						str = TextsData.getData(525).chinese;
						break;
					case 4:			//挑战次数 526//
					case 6:			//竞技场挑战次数//
						str = TextsData.getData(526).chinese;
						break;
					}
					
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				break;
			}
		}
	}
	
	
	//请求购买界面信息//
	public void SendToGetUIData(int jsonT, int t, int costT, int sweepT = 0, int missionId = 0, 
		BuyTipManager.UI_TYPE ui_type = BuyTipManager.UI_TYPE.UI_HEAD, int cdType = 0, int copyId = 0)
	{
		//this.jsonType = jsonT;
		this.buyType = t;
		this.costType = costT;
		this.curSweepNum = sweepT;
		this.missionId = missionId;
		this.curUIType = ui_type;
		this.curCDType = cdType;
		this.curCopyID = copyId;
		requestType = 1;
		PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonT, t, costT, sweepT, missionId, cdType, copyId),this);
	}
	
	
	public void receiveResponse (string json)
	{
		Debug.Log("ShowBuyTipControl : json ====================== " + json);
		if(json != null)
		{
			receiveData = true;
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:					//请求购买界面信息//
				BuyPowerOrGoldResultJson brj = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = brj.errorCode;
				if(errorCode == 0)
				{
					this.costCrystal = brj.crystal;
					this.num = brj.num;
					this.times = brj.times;	
				}
				
				receiveData=true;
				break;
			}
		}
		
	}
}
