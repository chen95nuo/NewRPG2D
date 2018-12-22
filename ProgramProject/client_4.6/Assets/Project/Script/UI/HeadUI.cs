using UnityEngine;
using System.Collections;

public class HeadUI : BWUIPanel,ProcessResponse {
	
	public static HeadUI mInstance;
	
	public UILabel uiName;
	public UISprite uiExp;
	public UISprite uiSprPower;
	public UILabel uiLevel;
	public UILabel uiGold;
	public UILabel uiCrystal;
	public UILabel uiPower;
	public UISprite headIcon;
	public UILabel battlePower;
	public UILabel vipLabel;
	
	public bool wairRequestPlayerInfo = false;
	
	// 1 player info 2,   3  获取购买界面信息， 4 购买水晶, 5 获得头像设置界面信息//
	private int requestType;
	private int errorCode;
	private bool receiveData;
	//上一次的金币数//
	private int preGold = -1;
	//上一次的战斗力//
	private int prePower;
	//购买物品的类型 1 金币， 2 体力//
	private int buyType;
	//请求类型,1 请求购买界面信息， 2 购买//
	private int jsonType;
	//花费类型 ，1 水晶， 2 金币//
	private int costType;
	//购买花费的水晶数//
	private int costCrystal;
	//要购买的金币或体力的个数//
	private int num;
	//当天剩余的购买次数//
	private int times;
	
	//充值界面的json//
	private RechargeUiResultJson rechargeRJ;
	
	//Vector3 expLocalScale;
	
	//距离下次恢复体力的剩余时间 单位s//
	private float time;
	//头像设置区域的json//
	HeadSetResultJson headSetRJ;
	

    public UILabel[] powers;
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init ();
	}
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			
			switch(requestType)
			{
			case 1:
			{
				wairRequestPlayerInfo = false;
				refreshPlayerInfo();
			}break;
			case 2:
				refreshPlayerInfo();
				break;
			case 3:
				if(errorCode == 0)
				{
					
					BuyTipManager.mInstance.SetData(buyType, costType , costCrystal, num, times);
				}
				else if(errorCode == 51)		//体力达到上限//
				{
					string str = TextsData.getData(270).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 79)		//购买次数达到上限//
				{
					string str = TextsData.getData(240).chinese;
					ToastWindow.mInstance.showText(str);
				}
				
				break;
			case 4:
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
				charge.show();
				break;
			case 5:
				if(errorCode == 0)
				{
					UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_HEADSETTING);
					HeadSettingManager headSetting = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_HEADSETTING, 
						"HeadSettingManager") as HeadSettingManager;
					headSetting.SetData(headSetRJ);
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ChangePlayerName))
					{
						GuideUI_ChangePlayerName.mInstance.showStep(1);
					}
				}
				break;
			}
		}
		
		time+=Time.deltaTime;
		if(PlayerInfo.getInstance().player.power<Constant.MaxPower && time>=Constant.AutoRestorePowerTime)
		{
			requestType=2;
			if(PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_RESTORE_POWER),this))
			{
				time=0;
			}
		}
	}
    public void PowerClick()
    {

        int times = ((120 - PlayerInfo.getInstance().player.power) * 300) / 60;
        if (this.transform.FindChild("Power").gameObject.activeSelf)
        {
            this.transform.FindChild("Power").gameObject.SetActive(false);
        }
        else
        {
            this.transform.FindChild("Power").gameObject.SetActive(true);
            powers[0].text = TextsData.getData(447).chinese + PlayerInfo.getInstance().player.power;
            powers[1].text = TextsData.getData(448).chinese;
            if (times >= 60)
                powers[2].text = TextsData.getData(449).chinese + (times / 60) + TextsData.getData(145).chinese + times % 60 + TextsData.getData(152).chinese;
            else
            {
                if (times <= 0)
                {
                    powers[2].text = TextsData.getData(473).chinese;
                }
                else
                    powers[2].text = TextsData.getData(449).chinese + (times % 60) + TextsData.getData(152).chinese;
            }
        }
    }
	public override void init ()
	{
		//base.init ();
		_MyObj.transform.localPosition = new Vector3(-40,203,0);
		_MyObj.GetComponent<UIPanel>().depth=20;
		//expLocalScale = uiExp.transform.localScale;
	}
	
	public override void show()
	{
		base.show();
		refreshPlayerInfo();
	}
	
	public override void hide()
	{
		base.hide();
	}
	
	public void setName(string name)
	{
		uiName.text=name;
	}
	public void setLevel(int level)
	{
		uiLevel.text=level+"";
	}
	public void setExp(int exp,int maxExp)
	{
		if(maxExp<=0)
		{
//			uiExp.width=214;
//			uiExp.transform.localScale = Vector3.one;
			uiExp.fillAmount = 1;
		}
		else
		{
//			uiExp.width=214*exp/maxExp;
			float s = (float)exp/((float)maxExp);
//			float x = expLocalScale.x * exp/maxExp;
//			uiExp.transform.localScale = new Vector3(x, expLocalScale.y, expLocalScale.z);
			uiExp.fillAmount = s;
//			Debug.Log("uiExp.fillAmoun ====== " + uiExp.fillAmount);
		}
		
	}
	public void setGold(int gold)
	{
		uiGold.text=gold+"";
	}
	
	public void setPreGold(int curGold)
	{
		if(preGold != curGold)
		{
			if(preGold > 0 && preGold < curGold)
			{
				//播放音效//
				MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COIN);
			}
			preGold = curGold;
		}
	}
	
	public void setCrystal(int crystal)
	{
		uiCrystal.text=crystal+"";
	}
	
	public void setPower(int power, int maxPower){
//		uiPower.text = power +"/"+ maxPower;
		uiSprPower.fillAmount = (float)power / (maxPower);
	}
	
	public void setHeadIcon(string head)
	{
		headIcon.spriteName=head;
		string atlasName = CardData.getAtlas(head);
		UIAtlas iconAtlas = LoadAtlasOrFont.LoadAtlasByName(atlasName);
		headIcon.atlas = iconAtlas;
	}
	
	public void setBattlePower(int power)
	{
		if(prePower==0)
		{
			prePower=power;
		}
		if(power!=prePower)
		{
			TickerUI.mInstance.show(prePower,power,1f);
		}
		battlePower.text=TextsData.getData(203).chinese+power;
		prePower=power;
	}
	
	//cuixl 添加vip判断//
	public void setVip(int vipLevel)
	{
		if(vipLevel > 0)
		{
			vipLabel.gameObject.SetActive(true);
			vipLabel.text = vipLevel.ToString();
		}
		else 
		{
			vipLabel.gameObject.SetActive(false);
		}
	}
	
	//cxl 获得体力的剩余时间//
	public int GetPowerRestoreTime()
	{
		int restoreTime = (int)(Constant.AutoRestorePowerTime - time);
		return restoreTime;
	}
	
	public void refreshPlayerInfo()
	{
		setName(PlayerInfo.getInstance().player.name);
		setLevel(PlayerInfo.getInstance().player.level);
		setGold(PlayerInfo.getInstance().player.gold);
		setCrystal(PlayerInfo.getInstance().player.crystal);
		setPower(PlayerInfo.getInstance().player.power, PlayerInfo.getInstance().player.sPower);
		setBattlePower(PlayerInfo.getInstance().player.battlePower);
		PlayerData pd=PlayerData.getData(PlayerInfo.getInstance().player.level+1);
		if(pd==null)
		{
//			Debug.Log("11111111111111111111111111111111111");
			setExp(0,0);
		}
		else
		{
//			Debug.Log("22222222222222222222222222222 ==== " + PlayerInfo.getInstance().player.curExp);
			setExp(PlayerInfo.getInstance().player.curExp,pd.exp);
		}
		setHeadIcon(PlayerInfo.getInstance().player.head);
		
		setPreGold(PlayerInfo.getInstance().player.gold);
		
		setVip(PlayerInfo.getInstance().player.vipLevel);
		
		//更新头像设置区域数据//
		refreshHeadSettingInfo();
	}
	
	//更新头像设置区域数据//
	public void refreshHeadSettingInfo()
	{
		//通知头像设置区域更新基础信息//
		HeadSettingManager setting = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_HEADSETTING, 
			"HeadSettingManager") as HeadSettingManager;
		if(setting != null)
		{
			setting.refreshBasicData();
		}
	}

    public void refreshPlayerGold()
    {
        setGold(PlayerInfo.getInstance().player.gold);
        setCrystal(PlayerInfo.getInstance().player.crystal);
        setPower(PlayerInfo.getInstance().player.power, PlayerInfo.getInstance().player.sPower);
    }
	
	public void requestPlayerInfo()
	{
		requestType = 1;
		wairRequestPlayerInfo = true;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PLAYER),this);
	}
	
	//头像设置//
	public void OnClickHeadBtn()
	{
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			if(main.gameObject.activeSelf && main.isCanClick == false)
				return;
			
		}
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
//		HeadSettingManager.mInstance.show();
		
		//获得头像设置界面信息//
		requestType = 5;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_HEADSET_SHOWDATA),this);
		
//		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_HEADSETTING);
//		HeadSettingManager headSetting = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_HEADSETTING, 
//			"HeadSettingManager") as HeadSettingManager;
//		headSetting.show ();
	}
	
	//购买按钮 id 0 购买金币， 1 购买体力, 2 购买水晶//
	public void OnClickBuyInfoBtn(int id)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		switch(id)
		{
		case 0:			//金币//
			requestType = 3;
			buyType = 1;
			jsonType = 1;
			costType = 1;
			PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType),this);
			break;
		case 1:			//体力//
			requestType = 3;
			buyType = 2;
			jsonType = 1;
			costType = 1;
			PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType),this);
			break;
		case 2:			//购买水晶//
			requestType = 4;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE),this);
			break;

		}
	}
	
	
	
	public void receiveResponse(string json)
	{
		Debug.Log("headUI  json : " + json);
		if(json==null)
		{
			//TODO
		}
		else
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
			{
				PlayerResultJson pj=JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode = pj.errorCode;
				
				if(pj!=null && pj.list!=null && pj.list.Count>0)
				{
					string[] str = pj.s;
					PlayerInfo.getInstance().SetUnLockData(str);
					PlayerInfo.getInstance().player = pj.list[0];
					receiveData=true;
				}
			}break;
			case 2:
				PowerResultJson prj=JsonMapper.ToObject<PowerResultJson>(json);
				errorCode = prj.errorCode;
				if(errorCode==0)
				{
					time=0;
					PlayerInfo.getInstance().player.power=prj.p;
					receiveData=true;
				}
				break;
			case 3:			//请求购买界面信息//
				BuyPowerOrGoldResultJson brj = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = brj.errorCode;
				this.costCrystal = brj.crystal;
				this.num = brj.num;
				this.times = brj.times;
				receiveData = true;
				break;
			case 4:			//购买水晶//
				RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
				errorCode = rechargej.errorCode;
				if(errorCode==0)
				{
					rechargeRJ = rechargej;
				}
				receiveData = true;
				break;
			case 5:			//获得头像设置界面信息//
				HeadSetResultJson hrj = JsonMapper.ToObject<HeadSetResultJson>(json);
				errorCode = hrj.errorCode;
				if(errorCode == 0)
				{
					headSetRJ = hrj;
				}
				receiveData = true;
				break;
			}
		}
	}
}
