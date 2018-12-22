using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SignUI : MonoBehaviour ,ProcessResponse{
	
//	public static SignUI mInstance;
	
	public UIAtlas heroCircleAtlas;
	public UIAtlas skillCircleAtals;
	public UIAtlas pSkillCircleAtals;
	public UIAtlas equipCircleAtals;
	public UIAtlas itemCircleAtals;
	public UIAtlas runeAtlas;
	public UIAtlas interface01Atlas;
	public UIAtlas interface04Atlas;
	public UIAtlas interface02Atlas;
	
	public UILabel titleSignTimes;
	public GameObject cellParent;
	
	
	public SignResultJson srj;
	private GameObject signCell;
	//private GameObject effect;
	
	private int requestType;
	private bool receiveData;
	private int errorCode;
	private int curDaylyIndex;
	private Transform _myTransform;
	
	public NewUnitSkillResultJson nusrj;
	
	void Awake()
	{
//		_MyObj=gameObject;
//		mInstance=this;
		_myTransform = transform;
	}
	
	// Use this for initialization
	void Start () {
//		base.init();
//		close();
		if(heroCircleAtlas == null)
		{
			
			heroCircleAtlas = LoadAtlasOrFont.LoadAtlasByName("headIcon");
		}
		if(skillCircleAtals == null)
		{
			
			skillCircleAtals = LoadAtlasOrFont.LoadAtlasByName("SkillCircularIcom");
		}
		if(pSkillCircleAtals == null)
		{
			
			pSkillCircleAtals = LoadAtlasOrFont.LoadAtlasByName("PassSkillCircularIcon");
		}
		if(equipCircleAtals == null)
		{
			
			equipCircleAtals = LoadAtlasOrFont.LoadAtlasByName("EquipCircularIcon");
		}
		if(itemCircleAtals == null)
		{
			
			itemCircleAtals = LoadAtlasOrFont.LoadAtlasByName("ItemCircularIcon");
		}
		if(runeAtlas == null)
		{
			
			runeAtlas = LoadAtlasOrFont.LoadAtlasByName("rune");
		}
		if(interface01Atlas == null)
		{
			
			interface01Atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas01");
		}
		
		if(interface04Atlas == null)
		{
			interface04Atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas04");
		}
		
		if(interface02Atlas == null)
		{
			interface02Atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas02");
		}
		
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
				else 	if(errorCode==66)
				{
					ToastWindow.mInstance.showText(TextsData.getData(205).chinese);
				}
				break;
			case 2:
				if(errorCode == 0)
				{
					Dictionary<string,object> dic = new Dictionary<string, object>();
					string username = PlayerPrefs.GetString("username");
					dic.Add("UserId",username);
					DaylyData dd = DaylyData.getDatas()[curDaylyIndex];
					switch(dd.type)
					{
					case 1://卡牌//
						dic.Add("RewardCard",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						break;
					case 2://装备//
						dic.Add("RewardEquip",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						break;
					case 3://主动技能//
						dic.Add("RewardKill",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						break;
					case 4://被动技能//
						dic.Add("RewardPassiveSkills",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						break;
					case 5://道具//
						dic.Add("RewardItem",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						break;
					case 6://符文值//
						dic.Add("RewardRune",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						break;
					case 7://钻石//
						dic.Add("RewardCrystal",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						if(!TalkingDataManager.isTDPC)
						{
							TDGAVirtualCurrency.OnReward(dd.number,TextsData.getData(216).chinese+"  "+TextsData.getData(49).chinese);					
						}
						break;
					case 8: // gold heart
						dic.Add("RewardGoldHeart",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						break;
					case 9: // gold
						dic.Add("RewardGold",dd.cardID.ToString());
						dic.Add("RewardNum",dd.number.ToString());
						break;
					}
					TalkingDataManager.SendTalkingDataEvent("Sign",dic);
					DaylyData curDayly=DaylyData.getDatas()[curDaylyIndex];
                    if (PlayerInfo.getInstance().player.vipLevel != 0&&dd.viptype != 0 && PlayerInfo.getInstance().player.vipLevel >= dd.viplevel)
                        ToastWindow.mInstance.showText(TextsData.getData(216).chinese + "\n" + curDayly.getRewardInfo(true, dd.number2));
                    else
                        ToastWindow.mInstance.showText(TextsData.getData(216).chinese + "\n" + curDayly.getRewardInfo(false, 0));
	
					show();
					HeadUI.mInstance.refreshPlayerInfo();
					
					//添加卡牌获得统计@zhangsai//
					if(dd.type == 1)
					{
						if(!UniteSkillInfo.cardUnlockTable.ContainsKey(dd.cardID))
							UniteSkillInfo.cardUnlockTable.Add(dd.cardID,true);
					}
					//向服务器请求判断是否有新解锁合体技//
					{
						requestType = 3;
						PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
					}
				}
				
				break;
			case 3:	
				if(nusrj.errorCode == 0)
				{
					if(!nusrj.unitskills.Equals(""))
					{
						UniteSkillUnlockManager.mInstance.SetDataAndShow(nusrj.unitskills,gameObject);	
					}
				}
				break;
			}
		}
	}
	
	public void show ()
	{
//		base.show ();
        Main3dCameraControl.mInstance.SetBool(true);
		titleSignTimes.text=TextsData.getData(206).chinese.Replace("num",srj.times+"");
		drawCells();
	}
	
	//==绘制奖励==//
	private void drawCells()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(cellParent);
		List<DaylyData> list=DaylyData.getDatas();
		for(int i=0;i<list.Count;i++)
		{
			if(i>=srj.days)
			{
				continue;
			}
			
			if(signCell==null)
			{
				signCell=GameObjectUtil.LoadResourcesPrefabs("UI-sign/sign-cell",3);
			}
			GameObject cell=Instantiate(signCell) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(cell,cellParent);
			
			UISpriteAnimation cellAnim = cell.GetComponent<UISpriteAnimation>();
			cellAnim.enabled = false;
			
			
			//设置数据//
			if(i>srj.times)
			{
				cell.transform.FindChild("selected").gameObject.SetActive(false);
				cell.transform.FindChild("cover").gameObject.SetActive(false);
			}
			else if(i==srj.times)
			{
				cell.transform.FindChild("selected").gameObject.SetActive(false);
				cell.transform.FindChild("cover").gameObject.SetActive(false);
				//特效//
				if(srj.mark==0)
				{

					cellAnim.enabled = true;
				}
			}
			else
			{
				cell.transform.FindChild("selected").gameObject.SetActive(true);
				cell.transform.FindChild("cover").gameObject.SetActive(true);
				cell.transform.FindChild("CardInfo/Child/HeroIcon").GetComponent<UISprite>().color = new Color(0.3f,0.3f,0.3f,1);
			}
			
			GameObject iconObj =cell.transform.FindChild("Obj").gameObject;
			UISprite iconSprite = iconObj.transform.FindChild("Icon").GetComponent<UISprite>();
			iconSprite.gameObject.SetActive(false);
			SimpleCardInfo2 cardInfo = cell.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
			cardInfo.transform.localScale = new Vector3(0.75f,0.75f,0.75f);
			//cardInfo.transform.localPosition = new Vector3(4,7,0);
			cardInfo.clear();
			cardInfo.gameObject.SetActive(false);
			
			
			DaylyData dd=list[i];
			switch(dd.type)
			{
			case 1://卡牌//
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(dd.cardID,GameHelper.E_CardType.E_Hero);
				cardInfo.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
				//cardInfo.transform.localPosition = new Vector3(0,7,0);
				break;
			case 2://装备//
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(dd.cardID,GameHelper.E_CardType.E_Equip);
				break;
			case 3://主动技能//
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(dd.cardID,GameHelper.E_CardType.E_Skill);
				break;
			case 4://被动技能//
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(dd.cardID,GameHelper.E_CardType.E_PassiveSkill);
				break;
			case 5://道具//
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(dd.cardID,GameHelper.E_CardType.E_Item);
				break;
			case 6://符文值//
				iconSprite.gameObject.SetActive(true);
				iconSprite.atlas=runeAtlas;
				iconSprite.spriteName="rune";
				break;
			case 7://钻石//
				iconSprite.gameObject.SetActive(true);
				iconSprite.atlas=interface02Atlas;
				iconSprite.spriteName="crystal4";
				iconSprite.width=100;
				iconSprite.height=100;
				
				iconSprite.transform.FindChild("frame").gameObject.SetActive(false);
				break;
			case 8://gold heart
				iconSprite.gameObject.SetActive(true);
				iconSprite.atlas=interface01Atlas;
				iconSprite.spriteName="jingangxin";
				iconSprite.width=100;
				iconSprite.height=100;
				
				iconSprite.transform.FindChild("frame").gameObject.SetActive(false);
				break;
			case 9://gold//
				iconSprite.gameObject.SetActive(true);
				iconSprite.atlas=interface04Atlas;
				iconSprite.spriteName="gold";
				iconSprite.width=100;
				iconSprite.height=100;
				
				iconSprite.transform.FindChild("frame").gameObject.SetActive(false);
				break;
			}
            //vip
			GameObject vipObj = iconObj.transform.FindChild("vip").gameObject;
            if (dd.viptype != 0)
			{
				vipObj.SetActive(true);
				UILabel vipLabel = vipObj.GetComponent<UILabel>();
				vipLabel.text = dd.viplevel.ToString();
			}
			else 
			{
				vipObj.SetActive(false);
			}
			


			iconObj.transform.FindChild("number").GetComponent<UILabel>().text="x"+dd.number;
			
			UIButtonMessage msg=cell.GetComponent<UIButtonMessage>();
//			msg.target=_MyObj;
			msg.target = _myTransform.gameObject;
			msg.functionName="onClickSign";
			msg.param=i;
            
		}
		cellParent.GetComponent<UIGrid>().repositionNow=true;
	}
	
	public void hide ()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(cellParent);
        Main3dCameraControl.mInstance.SetBool(false);
		_myTransform.gameObject.SetActive(false);
//		base.hide ();
		TalkMainToGetData();
		gc();
	}
	//cxl---通知主界面发请求//
	public void TalkMainToGetData()
	{
		//通知主城界面获取消息//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
		if(main!= null)
		{
			if(main.gameObject.activeSelf)
			{
				main.SendToGetData();
			}
		}
		
	}
	
	private void gc()
	{
		srj=null;
		signCell=null;
		//effect=null;
		heroCircleAtlas = null;
		skillCircleAtals = null;
		pSkillCircleAtals = null;
		equipCircleAtals = null;
		itemCircleAtals = null;
		runeAtlas = null;
		interface01Atlas = null;
		interface04Atlas = null;
		
		
		GameObjectUtil.destroyGameObjectAllChildrens(cellParent);
		Resources.UnloadUnusedAssets();
	}
	
	public void onClickSign(int param)
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
        List<DaylyData> list = DaylyData.getDatas();
        DaylyData dd = list[param];
		//==今日已签到==//
		if(srj.mark==1)
		{
			if(param<srj.times)
            {
                if (dd.viptype != 0 && PlayerInfo.getInstance().player.vipLevel < dd.viplevel)
                {
                    ToastWindow.mInstance.showText(TextsData.getData(205).chinese + " " + TextsData.getData(748).chinese.Replace("num", dd.viplevel.ToString()));
                }
                else
                {
                    if (PlayerInfo.getInstance().player.vipLevel != 0)
                        ToastWindow.mInstance.showText(TextsData.getData(217).chinese + "\n" + DaylyData.getDatas()[param].getRewardInfo(true, dd.number2));
                    else
                        ToastWindow.mInstance.showText(TextsData.getData(217).chinese + "\n" + DaylyData.getDatas()[param].getRewardInfo(true, 1));
                }
				return;
			}
			else 
			{
                if (PlayerInfo.getInstance().player.vipLevel != 0)
                {
                    if (dd.viptype != 0 && PlayerInfo.getInstance().player.vipLevel >= dd.viplevel)
                    {
                        ToastWindow.mInstance.showText(TextsData.getData(218).chinese.Replace("num", (param + 1) + "") + "\n" + DaylyData.getDatas()[param].getRewardInfo(true, dd.number2));
                    }
                    else
                    {
                        ToastWindow.mInstance.showText(TextsData.getData(218).chinese.Replace("num", (param + 1) + "") + "\n" + DaylyData.getDatas()[param].getRewardInfo(false, 1));
                    }
                }
                else
                {
                    ToastWindow.mInstance.showText(TextsData.getData(218).chinese.Replace("num", (param + 1) + "") + "\n" + DaylyData.getDatas()[param].getRewardInfo(false, 1)); ;
                }
				return;
			}
		}
		//==今日未签到==//
		else
		{
			if(param<srj.times)
			{
                if (PlayerInfo.getInstance().player.vipLevel > 0)
                {
                    ToastWindow.mInstance.showText(TextsData.getData(217).chinese + "\n" + DaylyData.getDatas()[param].getRewardInfo(true, dd.number2));
                }
                else
                    ToastWindow.mInstance.showText(TextsData.getData(217).chinese + "\n" + DaylyData.getDatas()[param].getRewardInfo(true, 1));
				return;
			}
			else if(param>srj.times)
			{
                if (PlayerInfo.getInstance().player.vipLevel != 0&&dd.viptype != 0 && PlayerInfo.getInstance().player.vipLevel >= dd.viplevel)
                {
                    ToastWindow.mInstance.showText(TextsData.getData(218).chinese.Replace("num", (param + 1) + "") + "\n" + DaylyData.getDatas()[param].getRewardInfo(true, dd.number2));
                }
                else
                {
                    ToastWindow.mInstance.showText(TextsData.getData(218).chinese.Replace("num", (param + 1) + "") + "\n" + DaylyData.getDatas()[param].getRewardInfo(false, 1));
                }
				return;
			}
			else
			{
                if (dd.viplevel != 0)
                {
                  // PlayerInfoss
                } 
                else
                {
                    ToastWindow.mInstance.showText(TextsData.getData(748).chinese.Replace("num",dd.viplevel.ToString()));
                }
				curDaylyIndex=param;
				requestType=1;
				PlayerInfo.getInstance().sendRequest(new SignJson(),this);
			}
		}
	}
	
	public void OnClickCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		hide();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SIGN);
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
				SignResultJson srj=JsonMapper.ToObject<SignResultJson>(json);
				errorCode=srj.errorCode;
				if(errorCode==0)
				{
					this.srj=srj;
				}
				receiveData=true;
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
				nusrj = JsonMapper.ToObject<NewUnitSkillResultJson>(json);
				errorCode = nusrj.errorCode;
				receiveData = true;
				break;
			}
		}
	}
	
}
