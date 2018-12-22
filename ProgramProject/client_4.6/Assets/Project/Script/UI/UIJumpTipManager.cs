using UnityEngine;
using System.Collections;

public class UIJumpTipManager : BWUIPanel, ProcessResponse,BWWarnUI {
	
	/**
	 * 带跳转功能的提示界面
	 */
	
	
	//当前发送购买信息的ui的类型//
	public enum UI_JUMP_TYPE:int
	{
		UI_PACKAGE = 			1,			//去背包//
		UI_SHOP = 				2,			//去商场//
		UI_CHARGE = 			3,			//去充值//
		UI_EXTENDPACKAGE=4,			//扩充背包//
	}
	
	
	public static UIJumpTipManager mInstance;
	public UILabel tipLabel;
	public UILabel jumpBtnLabel;
	
	//当前要跳转的界面的类型，1 背包， 2//
	private UI_JUMP_TYPE curType;
	
	//1 请求背包数据 2, 请求充值界面信息, 3 请求请求商城界面信息//
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
	//背包界面的json//
	private PackResultJson packRJ;
	//充值界面的json//
	private RechargeUiResultJson rechargeRJ;
	 //商店json
    private ShopResultJson shopRJ;
	//扩充背包json//
	private PackResultJson packExtendRJ;
	
	private bool isSpriteWorld;
	//当前被动技能的剩余背包空间//
	private int curBagSizePs;	
	
	public GameObject childNode;
	
	void Awake(){
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
	}
	
	public override void init ()
	{
//		base.init ();
		_MyObj.transform.localPosition = new Vector3(0,0,-720);
		if(tipLabel == null){
			tipLabel = _MyObj.transform.FindChild("TipLabel").GetComponent<UILabel>();
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			
			switch(requestType)
			{
			case 1:
//				PackUI.mInstance.show();
				
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_PACK);
				PackUI pack = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_PACK, "PackUI")as PackUI;
				pack.prj = packRJ;
				pack.show();
				packRJ = null;
				
				hide();
//				LotCardUI.mInstance.hide();
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT);
//				MissionUI.mInstance.hide();
//				MissionUI2.mInstance.hide();
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
//				ActiveWroldUIManager.mInstance.hide();
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY);
//				ActiveWroldSelManager.mInstance.hide();
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COPYSELECT);
//				WarpSpaceUIManager.mInstance.hide();
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE);
//				MazeUIManager.mInstance.hide();
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE);
//				SpriteWroldUIManager.mInstance.hide();
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD);
//				ComposePanel.mInstance.hide();
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE);
				//扫荡//
				UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SWEEP);
				break;
			case 2:			//充值//
				if(errorCode == 0)
				{
					
					UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
					ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ, 
						"ChargePanel") as ChargePanel;
					charge.curRechargeResult = rechargeRJ;
					//如果vipCost是0表示没有充值过，是第一次充值//
					if(rechargeRJ.vipCost == 0)
					{
						charge.firstCharge = 0;
					}
					else
						charge.firstCharge = rechargeRJ.vipCost;

                    charge.isShowType = 1;
                    charge.isShow = isShow;
					charge.show();
					
					hide();
				}
				break;
			case 3:
				if (errorCode == 0)
                {
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SHOP);
                    ShopUI shop = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SHOP, "ShopUI") as ShopUI;
                    shop.ShopResJson = shopRJ;
                    shopRJ = null;

                    shop.show(2);
					hide ();
                }
				break;
			case 4:
				if(errorCode==0)
				{
					TipExtendPackage();
					hide();
				}
				break;
			case 5:
				if(errorCode==98)
				{
					//背包数量已达上限！//
					ToastWindow.mInstance.showText(TextsData.getData(446).chinese);
					return;
				}
				else if(errorCode==71)
				{
					//钻石不足//
					string str = TextsData.getData(49).chinese+TextsData.getData(51).chinese;
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
					return;
				}
				else if(errorCode==89)
				{
					//金币不足//
					ToastWindow.mInstance.showText(TextsData.getData(46).chinese);
					return;
				}
				else
				{
					if(isSpriteWorld)
					{
						SpriteWroldUIManager spriteWorld = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD, 
								"SpriteWroldUIManager")as SpriteWroldUIManager;
							spriteWorld.SetUnUsedBackGrid(curBagSizePs);
						spriteWorld.ChangePackTip();
						isSpriteWorld = false;
					}
					HeadUI.mInstance.refreshPlayerInfo();
				}
				break;
			}
		}
	}
    bool isShow;
	public void SetData(UI_JUMP_TYPE type, string tipStr,bool isshow = false){
        this.isShow = isshow;
		show();
		tipLabel.text = tipStr;
		curType = type;
		switch(curType)
		{
		case UI_JUMP_TYPE.UI_PACKAGE:
			jumpBtnLabel.text = TextsData.getData(361).chinese;
			break;
		case UI_JUMP_TYPE.UI_SHOP :
			jumpBtnLabel.text = TextsData.getData(535).chinese;
			break;
		case UI_JUMP_TYPE.UI_CHARGE :
			jumpBtnLabel.text = TextsData.getData(481).chinese;
			break;
		}
	}
	
	public void SetPackageTypeData(UI_JUMP_TYPE type, string tipStr)
	{
		//this.isShow = isShow;
		show();
		tipLabel.text = tipStr;
		curType = type;
		switch(curType)
		{
		case UI_JUMP_TYPE.UI_EXTENDPACKAGE:
			jumpBtnLabel.text = TextsData.getData(671).chinese;
			break;
		}
	}
	
	public void SetSpriteWorldBool(bool isTSpriteWorld)
	{
		this.isSpriteWorld = isTSpriteWorld;
	}
	
	void TipExtendPackage()
	{
		BagCostData bd=BagCostData.getData(packExtendRJ.buyTimes+1);
		if(bd==null)
		{
			//背包数量已达上限！//
			ToastWindow.mInstance.showText(TextsData.getData(446).chinese);
			return;
		}
		string msg="";
		if(bd.type==1)
		{
			//您是否花费钻石num//
			msg=TextsData.getData(391).chinese.Replace("num",bd.cost+"");
		}
		else
		{
			//您是否花费金币num//
			msg=TextsData.getData(445).chinese.Replace("num",bd.cost+"");
		}
		//扩充卡牌上限至num？//
		msg+="\n"+TextsData.getData(392).chinese.Replace("num",bd.number1+"");
		ToastWarnUI.mInstance.showWarn(msg,this);
	}
	
	public void warnningSure()
	{
		//请求扩充背包//
		requestType = 5;
		PlayerInfo.getInstance().sendRequest(new BuyBagJson(),this);
	}
	
	public void warnningCancel()
    {}
	
	public override void show ()
	{
		//base.show ();
		childNode.SetActive(true);
        tweenScale(new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1), new Vector3(1, 1, 1));
	}
	
	public override void hide ()
	{
		//base.hide ();
		childNode.SetActive(false);
	}
	
	public void OnClickBtn_Toast(){
		//hide();
        tweenAlpha(1, PANEL_ALPHA_SIZE, baseHide);
	}

    void baseHide()
    {
        gameObject.transform.localScale = new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1);
        gameObject.GetComponent<UIPanel>().alpha = 1;
        hide();
    }
	
	public void OnClickJumpBtn()
	{
		switch(curType)
		{
		case UI_JUMP_TYPE.UI_PACKAGE:
			//请求背包数据//
			requestType = 1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK,1),this);
			break;
		case UI_JUMP_TYPE.UI_SHOP:			//去商场//
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SHOP, 2), this);
			break;
		case UI_JUMP_TYPE.UI_CHARGE:		//去充值//
			requestType = 2;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE),this);
			break;
		case UI_JUMP_TYPE.UI_EXTENDPACKAGE:		//先请求背包，在请求扩充背包//
			requestType=4;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK,1),this);
			break;
		}
	}
	
	public void receiveResponse (string json)
	{
		if(json != null)
		{
			Debug.Log("UIJumpTipManager : json ================ " + json);
			
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			
			switch(requestType )
			{
			case 1:
			{	//请求背包数据//
				PackResultJson prj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
				if(errorCode == 0)
				{
					packRJ = prj;
				}
				receiveData = true;
			}break;
			case 2:
			{
				RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
				errorCode = rechargej.errorCode;
				if(errorCode==0)
				{
					rechargeRJ = rechargej;
				}
				receiveData = true;
			}break;
			case 3 :
			{
				ShopResultJson temp = JsonMapper.ToObject<ShopResultJson>(json);
				errorCode = temp.errorCode;
				if (errorCode == 0)
				{
					shopRJ = temp;
				}
				receiveData = true;
			}break;
			case 4:
			{
				PackResultJson perj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = perj.errorCode;
				if(errorCode == 0)
				{
					packExtendRJ = perj;
				}
				receiveData = true;
			}break;
			case 5:
			{
				BuyResultJson brj=JsonMapper.ToObject<BuyResultJson>(json);
				errorCode=brj.errorCode;
				if(errorCode==0)
				{
					PlayerInfo.getInstance().player.gold=brj.gold;
					PlayerInfo.getInstance().player.crystal=brj.crystal;
					curBagSizePs = brj.bagSizePs;
				}
				receiveData = true;
			}break;
			}
		}
	}
}
