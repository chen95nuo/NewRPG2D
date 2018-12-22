using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChargePanel : MonoBehaviour ,ProcessResponse {
	
//	public static ChargePanel mInstance = null;
	
	public GameObject chargeMainPanel;
	public GameObject payClipPanel;
	public GameObject payGroup;
	GameObject payItemPrefab = null;
	
	public GameObject SpecialPowerPanel;
	public GameObject descClipPanel;
	public GameObject preBtn,nextBtn;
	
	public UISprite mainVIPCrystalPB,specialVIPCrystalPB;
	
	public UILabel tVIPValueLabel;
	public UILabel specialDescLabel;
	public UILabel preBtnLabel,nextBtnLabel;
	
	public GameObject firstChargePanel;
	
	public int firstCharge;
	
	public GameObject giftItemPanel;
	public UILabel gVIPValueLabel;
	public UILabel oldGiftValue;
	public UILabel newGiftValue;
	public GameObject giftBtn;
	public UIButtonMessage buyGiftBtn;
	
	public GameObject buyGiftResultPanel;
	public GameObject toChargeTipPanel;
	public GameObject helpUnit;
	
	public GameObject giftItemUIGrid;
	
	GameObject giftInfoPrefab = null;
	
	//private Transform _myTransform;
	
	private bool isMazeComeIn = false;
	
	public enum VIPGiftType
	{
		E_Null = 0,
		E_Item = 1,
		E_Equip = 2,
		E_Card = 3,
		E_Skill = 4,
		E_PassiveSkill = 5,
		E_Gold = 6,
		E_Exp = 7,
		E_Crystal = 8,
		E_Friend = 9,
	}
	
	string expSpriteName = "reward_exp";
	string crystalSpriteName = "reward_crystal";
	string goldSpriteName = "reward_gold";
	string friendshipSpriteName = "icon_02";
	
	public UIAtlas otherAtlas;
	public UIAtlas friendshipAtlas;
	
	int curVIPValue;
	int tempVIPValue;
	List<string> specialList;
	
	//1请求刷新充值界面,2请求刷新玩家数据，3请求订单数据，4请求购买vip礼包//
	int requestType;
	int errorCode;
	bool receiveData;
	//==订单信息==//
	private PayOrderResultJson porj;
	
	public RechargeUiResultJson curRechargeResult;
	
	public BuyPowerOrGoldResultJson bpgrj;

    public int isShowType = 0;
	
    public bool isShow;
	
	void Awake()
	{
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		//_myTransform = transform ;
		init();
        isShow = false;
//		hide();
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
				if(errorCode == 0)
				{
					requestType=2;
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PLAYER),this);
				}
				break;
			case 2:
				if(errorCode == 0)
				{
					HeadUI.mInstance.refreshPlayerInfo();
					InitPayClipPanel();
					if(!TalkingDataManager.isTDPC)
					{
						//统计月卡消费//
						int price = RechargeData.getData(1).cost;
						TDGAItem.OnPurchase("MonthAward",1,price);
					}
				}
				break;
			case 3:
				if(errorCode==0)
				{
					if(SDKManager.getInstance().isSDKCPYYUsing())
					{
						if(Application.platform==RuntimePlatform.Android)
						{
							
							//porj.consumValue="1";//
							
							SDK_CPYY_manager.sdk_call_pay(porj.consumValue,porj.order+"",porj.extra);
						}
					} 
					else if(SDKManager.getInstance().isSDKCoolpadUsing())
					{
						if(Application.platform==RuntimePlatform.Android)
						{
							PayExtraJson pej=JsonMapper.ToObject<PayExtraJson>(porj.extra);
							int price=StringUtil.getInt(porj.consumValue);
							SDK_CoolPadManager.sdk_startPay(pej.rechargeId+"",(price*100)+"",porj.order+"",porj.extra);
						}
					}
					else if(SDKManager.getInstance().isSDK91Using() || SDKManager.getInstance().isSDKKYUsing())
					{
						PayExtraJson pej=JsonMapper.ToObject<PayExtraJson>(porj.extra);
						int chargeId = pej.rechargeId;
						string chargeName = RechargeData.getData(chargeId).name;
						RechargeNow(porj.order + "", chargeId+"", chargeName, porj.consumValue);//
					}
					else if(SDKManager.getInstance().isSDKBDDKUsing())
					{
						if(Application.platform==RuntimePlatform.Android)
						{
							// 金额单位：元， porj.order 订单号, porj.extra额外信息//
							int amount = StringUtil.getInt(porj.consumValue);
							SDK_BDDK_Manager.sdk_call_pay(amount+"", porj.order+"", porj.extra);
						}
					}
				}
				break;
			case 4:
				if(errorCode == 0)
				{
					int useCrystal = PlayerInfo.getInstance().player.crystal - bpgrj.crystal;
					if(!TalkingDataManager.isTDPC)
					{
						//统计vip礼包消费//
						TDGAItem.OnPurchase("VipPackage",1,useCrystal);
					}
					PlayerInfo.getInstance().player.crystal = bpgrj.crystal;
					HeadUI.mInstance.refreshPlayerInfo();
					curRechargeResult.giftIds = bpgrj.ids;
					buyGiftResultPanel.SetActive(true);
					string msg = TextsData.getData(545).chinese;
					buyGiftResultPanel.transform.FindChild("Label").GetComponent<UILabel>().text = msg;
				}
				//金币或者水晶不足//
				else if(errorCode == 19)
				{
					string errorMsg = TextsData.getData(544).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				//vip等级不足//
				else if(errorCode == 70)
				{
					string errorMsg = TextsData.getData(243).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				//该vip等级礼包已经购买过//
				else if(errorCode == 104)
				{
					string errorMsg = TextsData.getData(542).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				break;
			}
		}
	}
	
	void InitPayClipPanel()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(payGroup);
		curVIPValue = curRechargeResult.vipLv;
		tempVIPValue = curVIPValue;
		
		chargeMainPanel.transform.FindChild("VIPLabel").GetComponent<UILabel>().text = curRechargeResult.vipLv.ToString();
		chargeMainPanel.transform.FindChild("VIPValue").GetComponent<UILabel>().text = curRechargeResult.vipCost.ToString()+"/"+curRechargeResult.sCost.ToString();
		mainVIPCrystalPB.fillAmount = (float)curRechargeResult.vipCost/curRechargeResult.sCost;
		if(curVIPValue == VipData.vipList.Count-1)
		{
			chargeMainPanel.transform.FindChild("VIPNextGroup").gameObject.SetActive(false);
		}
		else
		{
			chargeMainPanel.transform.FindChild("VIPNextGroup").gameObject.SetActive(true);
			chargeMainPanel.transform.FindChild("VIPNextGroup/ChargeCount").GetComponent<UILabel>().text = (curRechargeResult.sCost - curRechargeResult.vipCost).ToString();
			chargeMainPanel.transform.FindChild("VIPNextGroup/NextVIPLevel").GetComponent<UILabel>().text = (curRechargeResult.vipLv+1).ToString();
		}
		chargeMainPanel.transform.FindChild("crystalLabel").GetComponent<UILabel>().text = PlayerInfo.getInstance().player.crystal.ToString();
		
		List<RechargeData> tReDataList = new List<RechargeData>();
		//得到type=2的项(30天月卡),显示在第一位//
		for(int i=0;i<RechargeData.dataList.Count;i++)
		{
			RechargeData trData = RechargeData.dataList[i];
			if(trData.type == 2)
			{
				tReDataList.Add(trData);
			}
		}
		//得到type=1的项,并且倒序加到list里面去//
		for(int i=RechargeData.dataList.Count-1;i>=0;i--)
		{
			RechargeData trData = RechargeData.dataList[i];
			if(trData.type == 1)
			{
				tReDataList.Add(trData);
			}
		}
		for(int i=0;i<tReDataList.Count;i++)
		{
			RechargeData tReData = tReDataList[i];
			if(payItemPrefab == null)
			{
				payItemPrefab = Resources.Load("Prefabs/UI/ChargePanel/PayItem") as GameObject;
			}
			GameObject pItem = Instantiate(payItemPrefab) as GameObject;
			pItem.name = "pItem"+i.ToString();
			pItem.GetComponent<UIDragPanelContents>().draggablePanel = payClipPanel.GetComponent<UIDraggablePanel>();
			//Handle the special situation
			if(tReData.type == 2)
			{
				//If player pay the daily card
				if(curRechargeResult.vipMonthType == 1)
				{
					string desc = TextsData.getData(383).chinese;
					string ldesc = desc.Replace("num",curRechargeResult.vipMonthDay.ToString());
					pItem.transform.FindChild("CZDesc").GetComponent<UILabel>().text = ldesc;
					pItem.transform.FindChild("CZButton/Label").GetComponent<UILabel>().text = TextsData.getData(389).chinese;
				}
				else
				{
					pItem.transform.FindChild("CZDesc").GetComponent<UILabel>().text = tReData.description;
				}
				pItem.transform.FindChild("DoubleIcon").gameObject.SetActive(false);
			}
			else
			{
				if(curRechargeResult.ids.Contains(tReData.id.ToString()))
				{
					pItem.transform.FindChild("CZDesc").GetComponent<UILabel>().text = tReData.description;
					pItem.transform.FindChild("DoubleIcon").gameObject.SetActive(true);
				}
				else
				{
					pItem.transform.FindChild("CZDesc").GetComponent<UILabel>().text = "";
					pItem.transform.FindChild("DoubleIcon").gameObject.SetActive(false);
				}
			}
			
			pItem.transform.FindChild("itemIcon").GetComponent<UISprite>().spriteName = tReData.icon;
			pItem.transform.FindChild("CZName").GetComponent<UILabel>().text = tReData.name;
			pItem.transform.FindChild("CZValue").GetComponent<UILabel>().text = tReData.cost.ToString()+"RMB";
			
			pItem.transform.FindChild("CZButton").GetComponent<UIButtonMessage>().target = gameObject;
			pItem.transform.FindChild("CZButton").GetComponent<UIButtonMessage>().param = tReData.id;
			GameObjectUtil.gameObjectAttachToParent(pItem,payGroup);
		}
		
		payGroup.GetComponent<UIGrid>().repositionNow = true;
		ResetScrollBar(payClipPanel.GetComponent<UIDraggablePanel>().verticalScrollBar);
	}
	
	void InitialSpecialHeadContent()
	{
		SpecialPowerPanel.transform.FindChild("VIPLabel").GetComponent<UILabel>().text = curRechargeResult.vipLv.ToString();
		SpecialPowerPanel.transform.FindChild("VIPValue").GetComponent<UILabel>().text = curRechargeResult.vipCost.ToString()+"/"+curRechargeResult.sCost.ToString();
		specialVIPCrystalPB.fillAmount = (float)curRechargeResult.vipCost/curRechargeResult.sCost;
		if(curVIPValue == VipData.vipList.Count-1)
		{
			SpecialPowerPanel.transform.FindChild("VIPNextGroup").gameObject.SetActive(false);
		}
		else
		{
			SpecialPowerPanel.transform.FindChild("VIPNextGroup").gameObject.SetActive(true);
			SpecialPowerPanel.transform.FindChild("VIPNextGroup/ChargeCount").GetComponent<UILabel>().text = (curRechargeResult.sCost - curRechargeResult.vipCost).ToString();
			SpecialPowerPanel.transform.FindChild("VIPNextGroup/NextVIPLevel").GetComponent<UILabel>().text = (curRechargeResult.vipLv+1).ToString();
		}
		giftItemPanel.SetActive(false);
		buyGiftResultPanel.SetActive(false);
		toChargeTipPanel.SetActive(false);
		helpUnit.SetActive(false);
	}
	
	void UpdateSpecialPanel()
	{
		tVIPValueLabel.text = tempVIPValue.ToString();
		VipData tVIPData = VipData.getData(tempVIPValue);
		
		//Update left region description.
		specialDescLabel.text = tVIPData.description;
		//Update right region content
		gVIPValueLabel.text = tempVIPValue.ToString();
		oldGiftValue.text = tVIPData.fakeprice.ToString();
		newGiftValue.text = tVIPData.realprice.ToString();
		giftBtn.GetComponent<UIButtonMessage>().param = tVIPData.giftid;
		UISprite giftBtnBG = giftBtn.transform.FindChild("Background").GetComponent<UISprite>();
		if(tempVIPValue <= 3)
		{
			giftBtnBG.spriteName = "lv1";
			giftBtnBG.width = 74;
			giftBtnBG.height = 88;
		}
		else if(tempVIPValue <= 6)
		{
			giftBtnBG.spriteName = "lv2";
			giftBtnBG.width = 82;
			giftBtnBG.height = 71;
		}
		else if(tempVIPValue <= 9)
		{
			giftBtnBG.spriteName = "lv3";
			giftBtnBG.width = 105;
			giftBtnBG.height = 79;
		}
		else
		{
			giftBtnBG.spriteName = "lv4";
			giftBtnBG.width = 87;
			giftBtnBG.height = 92;
		}
//		giftBtnBG.MakePixelPerfect();
//		giftBtnBG.width.Update();
		buyGiftBtn.param = tempVIPValue;
		
		if(tempVIPValue==1)
		{
			preBtn.SetActive(false);
			nextBtn.SetActive(true);
			nextBtnLabel.text = (tempVIPValue+1).ToString();
			return;
		}
		else if(tempVIPValue== VipData.vipList.Count-1)
		{
			preBtn.SetActive(true);
			nextBtn.SetActive(false);
			preBtnLabel.text = (tempVIPValue-1).ToString();
			return;
		}
		else
		{
			preBtn.SetActive(true);
			nextBtn.SetActive(true);
		}
		preBtnLabel.text = (tempVIPValue-1).ToString();
		nextBtnLabel.text = (tempVIPValue+1).ToString();
	}
	
	void OnChargeBtn(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		RechargeData rd=RechargeData.getData(param);
		if(rd==null)
		{
			return;
		}
		if(SDKManager.getInstance().isSDKGCUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				//==小米渠道要求每次支付前都提交玩家信息==//
				if(TalkingDataManager.channelId.Equals("46"))
				{
					PlayerElement pe=PlayerInfo.getInstance().player;
					ExtendData ed=new ExtendData(pe.id+"",pe.name,pe.level+"",PlayerPrefs.GetInt("lastServerId"),PlayerPrefs.GetString("lastServerName"),pe.crystal+"","",pe.vipLevel+"");
					SDK_GCStubManager.sdk_submitExtendData("loginGameRole",JsonMapper.ToJson(ed));
				}
				PayExtraJson pej=new PayExtraJson(PlayerPrefs.GetString("username"),PlayerInfo.getInstance().player.id,rd.id,"android",TextsData.getData(730).chinese);
				int serverId=PlayerPrefs.GetInt("lastServerId");
				string extraStr=JsonMapper.ToJson(pej);
				SDK_GCStubManager.sdk_startPayment(rd.cost,rd.name,serverId+"",extraStr);
			}
		}
		else if(SDKManager.getInstance().isSDKCPYYUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				PayExtraJson pej=new PayExtraJson(PlayerPrefs.GetString("username"),PlayerInfo.getInstance().player.id,rd.id,"android",TextsData.getData(730).chinese);
				string extraStr=JsonMapper.ToJson(pej);
				requestType=3;
				PlayerInfo.getInstance().sendRequest(new PayOrderJson(rd.cost+"",extraStr),this);
			}
		}
		else if(SDKManager.getInstance().isSDKCoolpadUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				PayExtraJson pej=new PayExtraJson(PlayerPrefs.GetString("username"),PlayerInfo.getInstance().player.id,rd.id,"android",TextsData.getData(730).chinese);
				string extraStr=JsonMapper.ToJson(pej);
				requestType=3;
				PlayerInfo.getInstance().sendRequest(new PayOrderJson(rd.cost+"",extraStr),this);
			}
		}
		else if(SDKManager.getInstance().isSDK91Using() || SDKManager.getInstance().isSDKKYUsing())
		{
			ReqRechargeOrder(rd.id, rd.cost);
		}
		else if(SDKManager.getInstance().isSDKBDDKUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				PayExtraJson pej=new PayExtraJson(PlayerPrefs.GetString("username"),PlayerInfo.getInstance().player.id,rd.id,"android",TextsData.getData(730).chinese);
				string extraStr=JsonMapper.ToJson(pej);
				requestType=3;
				PlayerInfo.getInstance().sendRequest(new PayOrderJson(rd.cost+"",extraStr),this);
			}
		}
	}
	
	void ReqRechargeOrder(int rechargeId, int rechargeNum)
    {
		Debug.Log("---------- ReqRechargeOrder  rechargeId:"+rechargeId+" rechargeNum:"+rechargeNum);
#if UNITY_EDITOR
#elif (PLAT_91 || PLAT_KY)
        PayExtraJson json = new PayExtraJson(PlayerPrefs.GetString("username"), PlayerInfo.getInstance().player.id, rechargeId, Constant.OS_IOS);
        string extra = JsonMapper.ToJson(json);
        requestType = 3;
        PlayerInfo.getInstance().sendRequest(new PayOrderJson(rechargeNum + "", extra), this);
#endif
    }
	
    void RechargeNow(string orderId, string productId, string productName, string consumeNum)
    {
		Debug.Log("---------- RechargeNow  orderId:"+orderId+" productId:"+productId+" productName:"+productName+" consumeNum:"+consumeNum);
#if UNITY_EDITOR
#elif PLAT_91
        SDKPlatform91.SdkCallback(gameObject.name, "RechargeCallback");
        SDKPlatform91.SdkPayAsyn(orderId, productId, productName, int.Parse(consumeNum), 1, "desc");
#elif PLAT_KY
		SDKPlatform91.SdkCallback(gameObject.name, "RechargeCallbackKy");
		SDKPlatform91.SdkPayKy(orderId,productName,consumeNum+".00");
#endif
    }

    void RechargeCallback(string param)
    {
		Debug.Log("---------- RechargeCallback  param:"+param);
        Dictionary<string, string> dic = SDKPlatform91.string2Dic(param);
        if (dic.ContainsKey("code"))
        {
            int code = int.Parse(dic["code"]);
            if (code == 1)
            {
                ToastWindow.mInstance.showText(TextsData.getData(399).chinese);
                UpdateChargePanelRequest();
            }
			else if (code == 0)
            {
				//param is wrong
                ToastWindow.mInstance.showText(TextsData.getData(616).chinese);
            }
            else if (code == -12)
            {
				//user cancel
                ToastWindow.mInstance.showText(TextsData.getData(612).chinese);
            }
            else if (code == -7)
            {
				//network error
                ToastWindow.mInstance.showText(TextsData.getData(613).chinese);
            }
            else if (code == -10)
            {
				//server error
                ToastWindow.mInstance.showText(TextsData.getData(614).chinese);
            }
			else if (code == -4004)
            {
				//order serial have submit
                ToastWindow.mInstance.showText(TextsData.getData(527).chinese);
				UpdateChargePanelRequest();
            }
			else
			{
				//other error
				ToastWindow.mInstance.showText(TextsData.getData(615).chinese);
			}
        }
    }
	
	void RechargeCallbackKy(string param)
	{
		Debug.Log("---------- RechargeCallbackKy  param:"+param);
		
		Dictionary<string, string> dic = SDKPlatform91.string2Dic(param);
        if (dic.ContainsKey("code"))
        {
            int code = int.Parse(dic["code"]);
			if(code==1)
			{
				//用户银联支付成功//
				ToastWindow.mInstance.showText(TextsData.getData(399).chinese);
                UpdateChargePanelRequest();
			}
			else if (code == 0)
            {
                //close pay page//
				ToastWindow.mInstance.showText(TextsData.getData(527).chinese);
				UpdateChargePanelRequest();
            }
			else if (code == -1)
            {
                //用户银联支付失败//
				ToastWindow.mInstance.showText(TextsData.getData(615).chinese);
            }
            else if (code == -2)
            {
                //用户取消银联支付//
				ToastWindow.mInstance.showText(TextsData.getData(612).chinese);
            }
            else if (code == -3)
            {
                //银联没有返回结果//
				ToastWindow.mInstance.showText(TextsData.getData(615).chinese);
            }
			else
			{
				ToastWindow.mInstance.showText(TextsData.getData(615).chinese);	
			}
		}
	}
	
	public void UpdateChargePanelRequest()
	{
		requestType = 1;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE),this);
	}
	
	void OnSpecialBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		chargeMainPanel.SetActive(false);
		SpecialPowerPanel.SetActive(true);
		firstChargePanel.SetActive(false);
		InitialSpecialHeadContent();
		//Default display level1
		if(curVIPValue == 0)
		{
			tempVIPValue = 1;
		}
		else
		{
			tempVIPValue = curVIPValue;
		}
		
		UpdateSpecialPanel();
		ResetScrollBar(descClipPanel.GetComponent<UIDraggablePanel>().verticalScrollBar);
	}
	
	void OnBackToMainBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		SpecialPowerPanel.SetActive(false);
		chargeMainPanel.SetActive(true);
		firstChargePanel.SetActive(false);
		ResetScrollBar(payClipPanel.GetComponent<UIDraggablePanel>().verticalScrollBar);
	}
	
	void OnPreBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(tempVIPValue>1)
		{
			tempVIPValue--;
		}
		else
		{
			return;
		}
		
		UpdateSpecialPanel();
	}
	
	void OnNextBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(tempVIPValue == VipData.vipList.Count-1)
		{
			return;
		}
		else
		{
			tempVIPValue++;
		}
		UpdateSpecialPanel();
	}
	
	void OnGiftBtn(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		giftItemPanel.SetActive(true);
		UpdateGiftItemPanel(param);
	}
	
	void UpdateGiftItemPanel(int giftId)
	{
		giftItemPanel.transform.FindChild("TitleSprite/tVIPLabel").GetComponent<UILabel>().text = tempVIPValue.ToString();
		GetVipGiftDataItemList(giftId);
	}
	
	void GetVipGiftDataItemList(int giftId)
	{
		//VipGiftData tvgData = VipGiftData.getData(giftId);
		GameObject clipPanel = giftItemPanel.transform.FindChild("ClipPanel").gameObject;
		
		List<string> gItemList = VipGiftData.getItemList(giftId);
		
		for(int i=0;i<gItemList.Count;i++)
		{
			//Debug.Log("item"+i.ToString()+":"+gItemList[i]);
			if(giftInfoPrefab == null)
			{
				giftInfoPrefab = Resources.Load("Prefabs/UI/ChargePanel/GiftItem") as GameObject;
			}
			GameObject gItem = Instantiate(giftInfoPrefab) as GameObject;
			
			gItem.GetComponent<RechargeItemInfo>().helpUnitPanel = helpUnit;
			UIButtonMessage[] iconMsgs = gItem.GetComponents<UIButtonMessage>();
			UIButtonMessage iconMsg=iconMsgs[0];
			iconMsg.target = gItem;
			iconMsg.functionName="showHelperUnit";
			
			UIButtonMessage iconMsg2=iconMsgs[1];
			iconMsg2.target = gItem;
			iconMsg2.functionName="hiddleHelperUnit";
			
			UISprite rewardIconBG = gItem.transform.FindChild("IconBG").GetComponent<UISprite>();
			rewardIconBG.gameObject.SetActive(false);
			UILabel rewardLabel = gItem.transform.FindChild("Text").GetComponent<UILabel>();
			
			rewardLabel.text = string.Empty;
			SimpleCardInfo2 cardInfo = gItem.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
			cardInfo.clear();
			cardInfo.gameObject.SetActive(false);
			
			string[] ss = gItemList[i].Split('-');
			int rewardType = StringUtil.getInt(ss[0]);
			string rewardText = StringUtil.getString(ss[1]);
			
			int rewardID = 0;
			
			switch(rewardType)
			{
				case (int)VIPGiftType.E_Item:
				{
					string[] tempS = rewardText.Split(',');
					int itemID = StringUtil.getInt(tempS[0]);
					rewardID = itemID;
					int num = StringUtil.getInt(tempS[1]);
					ItemsData itemData = ItemsData.getData(itemID);
					if(itemData == null)
					{
						gItem.SetActive(false);
						continue;
					}
					cardInfo.gameObject.SetActive(true);
					cardInfo.setSimpleCardInfo(itemID,GameHelper.E_CardType.E_Item);

					rewardLabel.text = " x " + num.ToString();
					
				}break;
				case (int)VIPGiftType.E_Equip:
				{
					string[] tempS = rewardText.Split(',');
					int equipID = StringUtil.getInt(tempS[0]);
				rewardID = equipID;
					int num = StringUtil.getInt(tempS[1]);
					
					EquipData ed = EquipData.getData(equipID);
					if(ed == null)
					{
						gItem.SetActive(false);
						continue;
					}
					cardInfo.gameObject.SetActive(true);
					cardInfo.setSimpleCardInfo(equipID,GameHelper.E_CardType.E_Equip);	

					rewardLabel.text = " x " + num.ToString();
					
				}break;
				case (int)VIPGiftType.E_Card:
				{
					string[] tempS = rewardText.Split(',');
					int heroID = StringUtil.getInt(tempS[0]);
				rewardID = heroID;
					int num = StringUtil.getInt(tempS[1]);
					
					CardData cd = CardData.getData(heroID);
					if(cd == null)
					{
						gItem.SetActive(false);
						continue;
					}
					cardInfo.gameObject.SetActive(true);
					cardInfo.setSimpleCardInfo(heroID,GameHelper.E_CardType.E_Hero);

					rewardLabel.text = " x " + num.ToString();
					
				}break;
				case (int)VIPGiftType.E_Skill:
				{
					string[] tempS = rewardText.Split(',');
					int skillID = StringUtil.getInt(tempS[0]);
				rewardID = skillID;
					int num = StringUtil.getInt(tempS[1]);
					
					SkillData sd = SkillData.getData(skillID);
					if(sd == null)
					{
						gItem.SetActive(false);
						continue;
					}
					cardInfo.gameObject.SetActive(true);
					cardInfo.setSimpleCardInfo(skillID,GameHelper.E_CardType.E_Skill);

					rewardLabel.text = " x " + num.ToString();
					
				}break;
				case (int)VIPGiftType.E_PassiveSkill:
				{
					string[] tempS = rewardText.Split(',');
					int passiveSkillID = StringUtil.getInt(tempS[0]);
				rewardID = passiveSkillID;
					int num = StringUtil.getInt(tempS[1]);
					
					PassiveSkillData psd = PassiveSkillData.getData(passiveSkillID);
					if(psd == null)
					{
						gItem.SetActive(false);
						continue;
					}
					cardInfo.gameObject.SetActive(true);
					cardInfo.setSimpleCardInfo(passiveSkillID,GameHelper.E_CardType.E_PassiveSkill);
					
					rewardLabel.text = " x " + num.ToString();
					
				}break;
				case (int)VIPGiftType.E_Gold:
				{
					rewardIconBG.gameObject.SetActive(true);
					rewardIconBG.atlas = otherAtlas;
					rewardIconBG.spriteName = goldSpriteName;
					rewardLabel.text = "x " + rewardText;
					
				}break;
				case (int)VIPGiftType.E_Exp:
				{
					rewardIconBG.gameObject.SetActive(true);
					rewardIconBG.atlas = otherAtlas;
					rewardIconBG.spriteName = expSpriteName;
					rewardLabel.text = "x " + rewardText;
				}break;
				case (int)VIPGiftType.E_Crystal:
				{
					rewardIconBG.gameObject.SetActive(true);
					rewardIconBG.atlas = otherAtlas;
					rewardIconBG.spriteName = crystalSpriteName;
					rewardLabel.text = "x " + rewardText;
				}break;
				case (int)VIPGiftType.E_Friend:
				{
					rewardIconBG.gameObject.SetActive(true);
					rewardIconBG.atlas = friendshipAtlas;
					rewardIconBG.spriteName = friendshipSpriteName;
					rewardLabel.text = "x " + rewardText;
				}break;
			}
			
			if(gItem.GetComponent<RechargeItemInfo>())
			{
				gItem.GetComponent<RechargeItemInfo>().itemType = rewardType;
				gItem.GetComponent<RechargeItemInfo>().itemId = rewardID;
			}
			
			GameObjectUtil.gameObjectAttachToParent(gItem,giftItemUIGrid);
		}
		giftItemUIGrid.GetComponent<UIGrid>().repositionNow = true;
		
		if(gItemList.Count == 1)
		{
			giftItemUIGrid.transform.localPosition = new Vector3(0,0,0);
		}
		else if(gItemList.Count == 2)
		{
			giftItemUIGrid.transform.localPosition = new Vector3(-50,0,0);
		}
		else if(gItemList.Count == 3)
		{
			giftItemUIGrid.transform.localPosition = new Vector3(-100,0,0);
		}
		else
		{
			giftItemUIGrid.transform.localPosition = new Vector3(-150,0,0);
		}
		ResetScrollBar(clipPanel.GetComponent<UIDraggablePanel>().horizontalScrollBar);
	}
	
	void OnBuyGiftBtn(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		List<string> curGiftIds = curRechargeResult.giftIds;
		
		//如果当前vip等级不够来买该礼包//
		if(curVIPValue<tempVIPValue)
		{
			toChargeTipPanel.SetActive(true);
		}
		//如果vip等级够了，但是不在服务器可以购买礼包的列表中，说明已经买过该礼包了//
		else if(!curGiftIds.Contains(param.ToString()))
		{
			buyGiftResultPanel.SetActive(true);
			string msg = TextsData.getData(542).chinese;
			buyGiftResultPanel.transform.FindChild("Label").GetComponent<UILabel>().text = msg;
		}
		//vip等级够了，也在服务器可以购买礼包列表中，向服务器请求购买//
		else
		{
			requestType = 4;
			PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson("",7,2,1,param),this);
		}
	}
	
	void OnGiftOKBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		giftItemPanel.SetActive(false);
		GameObjectUtil.destroyGameObjectAllChildrens(giftItemUIGrid);
	}
	
	void OnBuyResultOKBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		buyGiftResultPanel.SetActive(false);
	}
	
	//去充值//
	void ToChargeBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        Main3dCameraControl.mInstance.SetBool(true);
		chargeMainPanel.SetActive(true);
		SpecialPowerPanel.SetActive(false);
		firstChargePanel.SetActive(false);
		InitPayClipPanel();
		ResetScrollBar(payClipPanel.GetComponent<UIDraggablePanel>().verticalScrollBar);
		gameObject.transform.localPosition = new Vector3(0,0,-720);
	}
	
	//取消充值//
	void CancleChargeBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		toChargeTipPanel.SetActive(false);
	}
	
	void OnMainCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		hide();
	}
	
	void OnFirstChargeCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		firstChargePanel.SetActive(false);
		chargeMainPanel.SetActive(true);
		SpecialPowerPanel.SetActive(false);
	}
	
	void OnSpecialCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		hide();
	}
	
	void ResetScrollBar(UIScrollBar tScrollBar)
	{
		tScrollBar.value = 0;
	}
	
	public void receiveResponse(string json)
	{
		Debug.Log("json:"+json);
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
				errorCode = rechargej.errorCode;
				if(errorCode==0)
				{
					curRechargeResult = rechargej;
				}
				receiveData = true;
				break;
			case 2:
				PlayerResultJson prj=JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode=prj.errorCode;
				if(errorCode==0)
				{
					PlayerInfo.getInstance().player=prj.list[0];
				}
				receiveData=true;
				break;
			case 3:
				PayOrderResultJson porj=JsonMapper.ToObject<PayOrderResultJson>(json);
				errorCode=porj.errorCode;
				if(errorCode==0)
				{
					this.porj=porj;
				}
				receiveData=true;
				break;
			case 4:
				BuyPowerOrGoldResultJson bpogrj = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = bpogrj.errorCode;
				if(errorCode == 0)
				{
					this.bpgrj = bpogrj;
				}
				receiveData = true;
				break;
			}
		}
	}
	
	public void init()
	{
//		base.init();
		ResetScrollBar(payClipPanel.GetComponent<UIDraggablePanel>().verticalScrollBar);
	}
	
	public void show()
	{
        if (isShow)
        {
            CornucopiaPanel cp = this.transform.parent.FindChild("ActivityPanel(Clone)/Down/CornucopiaPanel(Clone)").GetComponent<CornucopiaPanel>();
            cp.SetEc(false);
        }
//		base.show();
        Main3dCameraControl.mInstance.SetBool(true);
		chargeMainPanel.SetActive(true);
		SpecialPowerPanel.SetActive(false);
		if(firstCharge == 0)
		{
			firstChargePanel.SetActive(true);
		}
		else
		{
			firstChargePanel.SetActive(false);
		}
		InitPayClipPanel();
		ResetScrollBar(payClipPanel.GetComponent<UIDraggablePanel>().verticalScrollBar);
		gameObject.transform.localPosition = new Vector3(0,0,-720);
	}
	
	public void hide()
	{
//		base.hide();
        if (isShow)
        {
            CornucopiaPanel cp = this.transform.parent.FindChild("ActivityPanel(Clone)/Down/CornucopiaPanel(Clone)").GetComponent<CornucopiaPanel>();
            cp.SetEc(true);
            isShow = false;
        }
        if (isShowType == 0)
        {
            Main3dCameraControl.mInstance.SetBool(false);
        }
		
		if(isMazeComeIn)
		{
			NewMazeUIManager maze = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE, "NewMazeUIManager") as NewMazeUIManager;
			maze.OnClickUseMedicine();
		}
		
		gc();
		
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
	}
	
	//判断是否是从迷宫跳转的//
	public void SetIsMazeComeIn(bool isMaze)
	{
		isMazeComeIn = isMaze;
	}
	
	public void gc()
	{
		//==释放资源==//
		Resources.UnloadUnusedAssets();
	}
}
