using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopOtherDetailUI : MonoBehaviour, ProcessResponse {
	
	public UISprite icon;
	public UILabel nameLabel;
	public UILabel level;
	public UILabel star;
	public UILabel attribute;
	public UILabel des;
	public UILabel sale;
	public SimpleCardInfo2 cardInfo;
	public GameObject Btn_Use;
	public GameObject Btn_UseMore;
	public GameObject Btn_Show;
	
	public PopOpenResultUI popOpenResultUI;
	public PopShowBoxDataUI popShowBoxUI;
	
	
	private int curItemId;
	
	//为item时表示数量//
	private int curHaveNum;
	private bool shouldHide;
	//private Vector3 tweenFrom=new Vector3(500,0,0);
	//private Vector3 tweenTo=Vector3.zero;
	
	//1 是用物品//
	private int requestType;
	private int errorCode;
	private bool receiveData = false;
	private List<GameBoxElement> goodsList;
	
	void Awake()
	{
		
	}
	
	// Use this for initialization
	void Start () {
		hide();
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
			case 1 :				//使用物品//
				if(errorCode == 0)
				{
					if(goodsList.Count > 0)
					{
						ItemsData item = ItemsData.getData(curItemId);
						int type = item.type;
						popOpenResultUI.SetData(type, goodsList);
						if(type == 2)//消耗品//
						{
							for(int i =0;i<goodsList.Count;i++)
							{
//								if(goodsList[i].goodsType == 6)//金币//
//								{
//									if(!TalkingDataManager.isTDPC)
//									{
//										TDGAVirtualCurrency.OnReward(goodsList[i].num,"Usableitem-"+TextsData.getData(58).chinese);
//									}
//								}
								if(goodsList[i].goodsType == 8)//钻石//
								{
									if(!TalkingDataManager.isTDPC)
									{
										TDGAVirtualCurrency.OnReward(goodsList[i].num,"Usableitem-"+TextsData.getData(48).chinese);
									}
								}
//								else if(goodsList[i].goodsType == 9)//符文//
//								{
//									if(!TalkingDataManager.isTDPC)
//									{
//										TDGAVirtualCurrency.OnReward(goodsList[i].num,"Usableitem-"+TextsData.getData(221).chinese);
//									}
//								}
								
								//添加卡牌获得统计@zhangsai//
								if(goodsList[i].goodsType == 3)
								{
									if(!UniteSkillInfo.cardUnlockTable.ContainsKey(goodsList[i].goodsId))
										UniteSkillInfo.cardUnlockTable.Add(goodsList[i].goodsId,true);
								}
							}
						}
						else if(type == 5||type == 6)//宝箱类//
						{
							for(int i =0;i<goodsList.Count;i++)
							{
//								if(goodsList[i].goodsType == 6)//金币//
//								{
//									if(!TalkingDataManager.isTDPC)
//									{
//										TDGAVirtualCurrency.OnReward(goodsList[i].num,"Chest-"+TextsData.getData(58).chinese);
//									}
//								}
								if(goodsList[i].goodsType == 8)//钻石//
								{
									if(!TalkingDataManager.isTDPC)
									{
										TDGAVirtualCurrency.OnReward(goodsList[i].num,"gameboxaward");
									}
								}
//								else if(goodsList[i].goodsType == 9)//符文//
//								{
//									if(!TalkingDataManager.isTDPC)
//									{
//										TDGAVirtualCurrency.OnReward(goodsList[i].num,"Chest-"+TextsData.getData(221).chinese);
//									}
//								}
								
								//添加卡牌获得统计@zhangsai//
								if(goodsList[i].goodsType == 3)
								{
									if(!UniteSkillInfo.cardUnlockTable.ContainsKey(goodsList[i].goodsId))
										UniteSkillInfo.cardUnlockTable.Add(goodsList[i].goodsId,true);
								}
							}
						}
					}
					else 			//没有得到物品//
					{
						
					}
					//修改背包信息//
					PackUI pack = UISceneStateControl.mInstace.
						GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_PACK, "PackUI")as PackUI;
					pack.SendToGetItemData(true);
//					hide();
				}
				else if(errorCode == 107)		//钥匙不足数量不足//
				{
					string str = TextsData.getData(536).chinese;
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_SHOP, str);
				}
				else if(errorCode == 106)		//宝箱不足//
				{
					string str = TextsData.getData(537).chinese;
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_SHOP, str);
					
				}
				else if(errorCode == 105)		//该物品无法使用//
				{
					
				}
				else if(errorCode == 108)		//是用次数超过最多使用次数//
				{
					string str = TextsData.getData(534).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 20)		//物品不足//
				{
					string str = TextsData.getData(534).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 57)		//玩家等级不足//
				{
					UsableItemData usItem = UsableItemData.getData(curItemId);
					string s1 = TextsData.getData(549).chinese;
					string s2 = TextsData.getData(550).chinese;
					string str = s1 + usItem.uselevel + s2;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 70)		//vip等级不足//
				{
					UsableItemData usItem = UsableItemData.getData(curItemId);
					string s1 = TextsData.getData(551).chinese;
					string s2 = TextsData.getData(552).chinese;
					string str = s1 + usItem.viprequest + s2;
//					ToastWindow.mInstance.showText(str);
					 UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else if(errorCode == 53)		//背包空间不足//
				{
					string str = TextsData.getData(78).chinese;
					ToastWindow.mInstance.showText(str);
				}
				break;
			}
		}
	}
	
	public void show()
	{
		gameObject.SetActive(true);
		GameObjectUtil.playForwardUITweener(gameObject.GetComponent<TweenPosition>());
		
	}

	public void hide()
	{
		gameObject.SetActive(false);
	}
	/// <summary>
	/// Sets the content.
	/// </summary>
	/// <param name='icon'>
	/// Icon
	/// </param>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='level'>
	/// Level.
	/// </param>
	/// <param name='star'>
	/// Star.
	/// </param>
	/// <param name='attribute'>
	/// Attribute.
	/// </param>
	/// <param name='des'>
	/// DES.
	/// </param>
	/// <param name='sale'>
	/// Sale.
	/// </param>
	public void setContent(int eType,string icon,string name,string level,string star,string attribute,string des,string sale,UIAtlas atals,string addValue="")
	{
		switch(eType)
		{
		case 2:
		case 3:
			this.level.text=TextsData.getData(26).chinese+level;
			this.star.text=TextsData.getData(27).chinese+star;
			this.attribute.text=TextsData.getData(28).chinese+attribute;
			break;
		case 4:
			this.level.text=TextsData.getData(30).chinese+level;
			this.star.text=TextsData.getData(31).chinese+star;
			this.attribute.text=TextsData.getData(32).chinese+attribute;
			break;
		case 5:
			this.level.text="";
			this.star.text=TextsData.getData(33).chinese+star;
			this.attribute.text=TextsData.getData(34).chinese+attribute;
			break;
		}





		this.icon.atlas=atals;
		this.icon.spriteName=icon;
		this.nameLabel.text=name;
		this.des.text=des+addValue;
		this.sale.text=TextsData.getData(29).chinese+sale;
		show();
	}
	//eType : 1角色卡,2主动技能,3被动技能,4装备,5材料//
	public void setContentNew(int eType,GameHelper.E_CardType cardType,int formID,string name,string level,string star,
		string attribute,string des,string sale,string addValue = "")
	{
        this.level.gameObject.SetActive(true);
        this.star.gameObject.SetActive(true);
        this.sale.gameObject.SetActive(true);
        this.attribute.gameObject.SetActive(true);
		
		switch(eType)
		{
		case 2:
		case 3:
			this.level.text=TextsData.getData(26).chinese+level;
			this.level.transform.localPosition = this.star.transform.localPosition;
//			this.star.text=TextsData.getData(27).chinese+star;
//			this.attribute.text=TextsData.getData(28).chinese+attribute;
			this.star.text="";
			this.attribute.text="";
			Btn_Use.SetActive(false);
			Btn_UseMore.SetActive(false);
			Btn_Show.SetActive(false);
			break;
		case 4:
			this.level.text=TextsData.getData(30).chinese+level;
			this.level.transform.localPosition = this.star.transform.localPosition;
//			this.star.text=TextsData.getData(31).chinese+star;
//			this.attribute.text=TextsData.getData(32).chinese+attribute;
			this.star.text="";
			this.attribute.text="";
			Btn_Use.SetActive(false);
			Btn_UseMore.SetActive(false);
			Btn_Show.SetActive(false);
			break;
		case 5:
			this.curItemId = formID;
			this.curHaveNum = StringUtil.getInt( attribute);
			this.level.text="";
//			this.star.text=TextsData.getData(33).chinese+star;
			this.star.text="";
			this.attribute.text=TextsData.getData(34).chinese+attribute;		//为材料时表示已有数量//;
			this.attribute.transform.localPosition = this.star.transform.localPosition;
			ItemsData item = ItemsData.getData(formID);

			if(item.use > 0)
			{
				Btn_Use.SetActive(true);
			}
			else 
			{
				Btn_Use.SetActive(false);
			}
			if(item.usetimes > 0)
			{
				Btn_UseMore.SetActive(true);
				UILabel label = Btn_UseMore.transform.FindChild("Label").GetComponent<UILabel>();
				string str = TextsData.getData(530).chinese;
				string ss = str.Replace("n", item.usetimes.ToString());
				label.text = ss;
				UIButtonMessage ubm = Btn_UseMore.GetComponent<UIButtonMessage>();
				ubm.target = gameObject;
				ubm.functionName = "OnClickUsedBtn";
				ubm.param = item.usetimes;
				GameObject black = Btn_UseMore.transform.FindChild("Black").gameObject;
				if(curHaveNum < item.usetimes)
				{
					black.SetActive(true);
					ubm.target = null;
					ubm.functionName = "";
					ubm.param = 0;
				}
				else 
				{
					black.SetActive(false);
				}
			}
			else 
			{
				Btn_UseMore.SetActive(false);
			}
			
			//如果是宝箱的话才显示//
			if(Btn_Show!=null)
			{
				if(item.type == 5)
				{
					Btn_Show.SetActive(true);
				}
				else 
				{
					Btn_Show.SetActive(false);
				}
			}
			break;
		}
		//this.icon.atlas=atals;
		//this.icon.spriteName=icon;
		cardInfo.setSimpleCardInfo(formID,cardType);
		this.nameLabel.text=name;
		this.des.text=des+addValue;
		this.sale.text=TextsData.getData(29).chinese+sale;
		show();
	}

    public void showPssSkill(string name, string description,string iconName,string atlas)
    {
        //this.level.text = TextsData.getData(26).chinese + level;
        //this.level.transform.localPosition = this.star.transform.localPosition;
        ////			this.star.text=TextsData.getData(27).chinese+star;
        ////			this.attribute.text=TextsData.getData(28).chinese+attribute;
        //this.star.text = "";

        this.nameLabel.text = name;
        this.attribute.text = description;
        this.des.text = description;
        Btn_Use.SetActive(false);
        Btn_UseMore.SetActive(false);
        Btn_Show.SetActive(false);
        this.level.gameObject.SetActive(false);
        this.star.gameObject.SetActive(false);
        this.sale.gameObject.SetActive(false);
        this.attribute.gameObject.SetActive(false);
        cardInfo.setPssSkillIcon(iconName, atlas);
        show();
    }
	//材料详细信息界面打开的情况下，关闭商城则需要修改材料的个数//
	//eType : 1角色卡,2主动技能,3被动技能,4装备,5材料//
	public void RefreshItemNum(int num, int eType, int sale)
	{
		if(eType == 5)
		{
			this.attribute.text=TextsData.getData(34).chinese+num;		//为材料时表示已有数量//;
			this.sale.text=TextsData.getData(29).chinese+sale;
		}
	} 
	
	//useTimes 使用次数//
	public void OnClickUsedBtn(int useTimes)
	{
		if(useTimes > 0)
		{
			ItemsData item = ItemsData.getData(curItemId);
//			if(useTimes > item.usetimes)			//是用物品数量不足//
//			{
//				string str = TextsData.getData(534).chinese;
//				ToastWindow.mInstance.showText(str);
//				return;
//			}
			
			if(item.type == 2)		//非宝箱类的消耗品
			{
				UsableItemData usItem = UsableItemData.getData(curItemId);
				if(usItem.cost > curHaveNum)		//单次使用物品数量不足//
				{
					string s1 = TextsData.getData(548).chinese;
					string s2 = TextsData.getData(94).chinese;
					string str = s1 + usItem.cost + s2;
					ToastWindow.mInstance.showText(str);
				}
				else if(PlayerInfo.getInstance().player.level < usItem.uselevel)		//玩家等级不足//
				{
					string s1 = TextsData.getData(549).chinese;
					string s2 = TextsData.getData(550).chinese;
					string str = s1 + usItem.uselevel + s2;
					ToastWindow.mInstance.showText(str);
				}	
				else if(PlayerInfo.getInstance().player.vipLevel < usItem.viprequest)		//vip等级不足//
				{
					string s1 = TextsData.getData(551).chinese;
					string s2 = TextsData.getData(552).chinese;
					string str = s1 + usItem.viprequest + s2;
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else 
				{
					requestType = 1;
					PlayerInfo.getInstance().sendRequest(new GameBoxJson(curItemId, useTimes),this);
				}
			}
			else if(item.type == 5 || item.type == 6 || item.type == 7)		//宝箱类的消耗品//
			{
				requestType = 1;
				PlayerInfo.getInstance().sendRequest(new GameBoxJson(curItemId, useTimes),this);
			}
			
		}
	}
	
	public void OnClickShowBtn()
	{
		if(popShowBoxUI!=null)
		{
			ItemsData item = ItemsData.getData(curItemId);
			List<GameBoxData> showList = GameBoxData.getListData(curItemId);
			popShowBoxUI.SetData(showList, item.id);
		}
		
	}
	
	public void fadeToHide()
	{
		shouldHide=true;
		GameObjectUtil.playReverseUITweener(gameObject.GetComponent<TweenPosition>());
	}
	
	public void tweenOver()
	{
		if(shouldHide)
		{
			shouldHide=false;
			hide();
		}
	}
	
	public void receiveResponse (string json)
	{
		if(json != null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				GameBoxResultJson gbrj = JsonMapper.ToObject<GameBoxResultJson>(json);
				errorCode = gbrj.errorCode;
				if(errorCode == 0)
				{
					goodsList = gbrj.ges;
				}
				receiveData = true;
				break;
			}
		}
	}
	
}
