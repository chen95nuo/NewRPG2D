using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class PackElement : ICloneable
{
	public int i;				//索引//
	public int eType;			//1角色卡,2主动技能,3被动技能,4装备,5材料//
	public int dataId;			//对应excel里的id//
	public string ct;			//创建时间//
	public int lv;				//强化等级//
	public int curExp;
	public int bn;				//突破次数//
	
	public int use;				//1在卡组,0不在//
	public int pile;			//数量//
	
	//==只有此对象是角色卡时以下才有效==//
	public int skId;//技能Id,//
	
	public int nw;				//新卡牌//
	public int bp;				//战斗力//
	public int bnType;		// is can break, 0 can not, 1 can//
	public int breakType;	//突破进度//
	public int newType;			//是否为新卡，0是，1不是//
	
	
	public int getSelfAtk()
	{
		return (int)Statics.getCardSelfMaxAtkForUI(dataId,lv,bn);
	}
	
	public int getSelfDef()
	{
		return (int)Statics.getCardSelfMaxDefForUI(dataId,lv,bn);
	}
	
	public int getSelfHp()
	{
		return (int)Statics.getCardSelfMaxHpForUI(dataId,lv,bn);
	}
	
	public int getTalent1()
	{
		CardData cd=CardData.getData(dataId);
		return cd.talent;
	}
	
	public int getTalent2()
	{
		if(bn==Constant.MaxBreakNum1)
		{
			CardData cd=CardData.getData(dataId);
			return cd.talent2;
		}
		return 0;
	}
	
  	object ICloneable.Clone() 
    { 
        return this.Clone(); 
    } 
	
	public PackElement Clone()
	{
		return (PackElement)this.MemberwiseClone();
	}
	
}

public class PackUI : MonoBehaviour,ProcessResponse,BWWarnUI{
	
//	public static PackUI mInstance;
	
	public PackResultJson prj;
	
	public List<PackElement> elements;
    public List<PackElement> notPackCells;
	
    public List<PackElementJson> cells;
	
	/**格子父类**/
	public GameObject packWindow;
	public GameObject packWindowGridParent;
	/**遮盖panel**/
	public GameObject coverPanel;
	/**全选**/
	public UIButton btnSelectAll;
	public GameObject popSelectAllSetting;
	public GameObject selAllSettingGridObj;
	/**全选设定按钮**/
	public UIButton btnSelectAllSetting;
	//全选设定的滚动条//
	public UIScrollBar selAllSettingSB;
	List<string> curSelSetting;
	List<string> tempSelSetting;
	/**出售**/
	public UIButton btnSale;
	
	public GameObject pack;
	public GameObject packNum;
	public GameObject packExtend;
	public PopCardDetailUI popCardDetail;
	public PopOtherDetailUI popOtherDetail;
	public PopShowBoxDataUI popShowBoxUI;
	
	private Transform _myTransform;
	
	private PackElement card;
	private PackElement skill;
	private List<PackElement> pSkillList;
	private List<PackElement> equips;
	
	private const string SelectedGbName="MarkSelect";
	private List<GameObject> grids;
	/**当前type**/
	private int curType;
	/**是否出售状态**/
	private bool isSaleStatic;
	/**待出售的物品集**/
	private List<PackElement> selectedCells=new List<PackElement>();

    bool isReachStar = false;
	/**是否已全选**/
	private bool selectedAll;
	
	private bool receivedData;
	/**1请求背包,2请求出售背包,3出售,4出售返回背包,7请求保存全选设置**/
	private int requestType;
	private int errorCode;
	
	private bool showGold;
	
	private int addGold;
	private int playerGold;

    private int fromIdIndex;

    private bool isBuyPackNum;
	
	/**背包网格,程序里需要反复实例化**/
	private GameObject packGridCell;
	//private GameObject popGridCell;
	
	//是否需要更新头像信息//
	private bool isNeedRefreshHead = false;
	
	//private int salePackFrequency = 0;
	
	public GameObject nextCard;
	public GameObject lastCard;
	private int selectCardParam = 0;
	private int selectCardIndex = -1;
    private PackElement selectCardElement = null;
	
	private int buyPackUseCrystal = 0;

    public UIScrollBar listValue;
	
	void Awake()
	{
		_myTransform = transform ;
		
		coverPanel.SetActive(false);
		popSelectAllSetting.SetActive(false);
	}
	 
	
	// Update is called once per frame
	void Update () {
		if(receivedData)
		{
			receivedData=false;
			switch(requestType)
			{
			case 1:
				if(errorCode == 0)
				{
					showPack(curType);
					//通知头像更新//
					if(isNeedRefreshHead)
					{
						isNeedRefreshHead = false;
						HeadUI.mInstance.requestPlayerInfo();
						//更新item的详细信息界面中的数据//
	
						PackElement pe=null;
						for(int m = 0;m < cells.Count;m ++)
						{
							PackElement temp = cells[m].pe;
							if(temp.i == selectCardIndex)
							{
								pe = temp;
							}
						}
	                    if (pe != null && selectCardElement != null && pe.eType == selectCardElement.eType && pe.dataId == selectCardElement.dataId)
	                    {
	                        ItemsData itemD = ItemsData.getData(pe.dataId);
	                        int saleNum = itemD.sell * pe.pile;
	                        if (popOtherDetail != null && popOtherDetail.gameObject.activeSelf)
	                        {
	                            popOtherDetail.RefreshItemNum(pe.pile, pe.eType, saleNum);
	                        }
	                    }
	                    else
	                    {
	                        popOtherDetail.fadeToHide();
	                    }
					}
				}
				
				
				break;
			case 2:
				if(errorCode == 0)
				{
					if(showGold)
					{
						showGold=false;
						/**显示一下获得的金币**/
						if(ToastWindow.mInstance!=null)
						{
							//获得金币//
							ToastWindow.mInstance.showText(TextsData.getData(19).chinese+addGold);
						}
						PlayerInfo.getInstance().player.gold=playerGold;
						/**更新UI头**/
						HeadUI.mInstance.refreshPlayerInfo();
						
	//					//播放增加金币音效//
	//					MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COIN);
					}
					clearSelectedAll();
					openSale();
					showPack(curType);
				}
				
				break;
			case 3:
				if(errorCode == 0)
				{
					Dictionary<string,object> dic = new Dictionary<string, object>();
					dic.Add("GetGold",addGold.ToString());
					TalkingDataManager.SendTalkingDataEvent("SaleCard",dic);
					requestType=2;
	                if (isSaleStatic)
	                {
	                    selectedCells.Clear();
	                }
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK_SALE,curType),this);
				}
				
				break;
			case 4:
				if(errorCode == 0)
				{
					closeSale();
					showPack(curType);
				}
				break;
			case 5:
				if(errorCode == 0)
				{
					popCardDetail.setContent(card,skill,pSkillList,equips);
					pack.SetActive(false);
				}
				
				break;
			case 6://==购买背包上限==//
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
//					ToastWindow.mInstance.showText(str);
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
				else if(errorCode == 0)
				{
					showPackNum();
					HeadUI.mInstance.refreshPlayerInfo();
					BagCostData bcd = BagCostData.getData(prj.buyTimes);
					if(!TalkingDataManager.isTDPC)
					{
						TDGAItem.OnPurchase("PackGrid-"+bcd.number1,1,buyPackUseCrystal);
					}
				}
				break;
			case 7:
				if(errorCode == 110)
				{
					//您没有选中任何全选的卡牌//
					ToastWindow.mInstance.showText(TextsData.getData(554).chinese);
				}
				else if(errorCode == 4)
				{
					//全选类型错误，请重新设定//
					 ToastWindow.mInstance.showText(TextsData.getData(556).chinese);
				}
				else if(errorCode == 0)
				{
					curSelSetting = tempSelSetting;
					popSelectAllSetting.SetActive(false);
				}
				break;
			}
		}
	}
	

	public void show()
	{
//		base.show();
		if(grids==null)
		{
			grids=new List<GameObject>();
		}
		curType=1;
		showPack(curType);
//		MainMenuManager.mInstance.hide();
		//隐藏主城//
		if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
		{
			UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		}
		//隐藏界面//
		if(popShowBoxUI!=null)
		{
			popShowBoxUI.hide();
		}
	}
	
	public void hide()
	{
//		base.hide();
		gc();
		
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_PACK);
	}
	
	public void gc()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(packWindowGridParent);
		grids.Clear();
        if (cells != null)
        {
            cells.Clear();
        }
        if (notPackCells != null)
        {
            notPackCells.Clear();
        }
		if (elements != null)
        {
            elements.Clear();
        }
		if (prj != null)
        {
            prj=null;
        }
		card=null;
		skill=null;
		pSkillList=null;
		equips=null;
		selectedCells.Clear();
		
		packGridCell=null;
		//popGridCell=null;
		Resources.UnloadUnusedAssets();
	}
	
	public void SendToGetItemData(bool isNeedRefresh)
	{
		isNeedRefreshHead = isNeedRefresh;
		curType = 4;
		requestType=1;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK,curType),this);
	}
	
	/**按钮参数:1角色卡,2主动技能卡,3装备卡,4材料卡,5后退,6出售/确定,7全选,8神力卡**/
	public void onClickBtn(int param)
	{
        listValue.value = 0;
		switch(param)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 8:
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
			if(curType==param)
			{
				return;
			}
			curType=param;
			if(!isSaleStatic)
			{
				
				requestType=1;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK,curType),this);
			}
			else
			{
				requestType=2;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK_SALE,curType),this);
			}
			break;
		case 5:
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			/**出售状态**/
			if(isSaleStatic)
			{
				requestType=4;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK,curType),this);
			}
			else
			{
//				MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
				if(main!= null)
				{
					main.SetData(STATE.ENTER_MAINMENU_BACK);
				}
				hide();
			}
			break;
		case 6:
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(isSaleStatic)
			{
				sendSale();
			}
			else
			{
				requestType=2;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK_SALE,curType),this);
			}
			break;
		case 7:
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(selectedAll)
			{
				clearSelectedAll();
			}
			else 
			{
				selectAll();
			}
			break;
		}
	}
	
	public void onClickSelectAllSetting()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		popSelectAllSetting.SetActive(true);
		List<string> allSelList = curSelSetting;
		Debug.Log("curSelSetting count:"+curSelSetting.Count);
		if(allSelList==null || (allSelList.Count == 1 && allSelList.Contains("")))
		{
			for(int i=0;i<7;i++)
			{
				if(i<3)
				{
					selAllSettingGridObj.transform.FindChild("Toggle"+i.ToString()).GetComponent<UIToggle>().value = true;
				}
				else
					selAllSettingGridObj.transform.FindChild("Toggle"+i.ToString()).GetComponent<UIToggle>().value = false;
			}
			
		}
		else
		{
			for(int i=0;i<7;i++)
			{
				if(allSelList.Contains(i.ToString()))
				{
					selAllSettingGridObj.transform.FindChild("Toggle"+i.ToString()).GetComponent<UIToggle>().value = true;
				}
				else
					selAllSettingGridObj.transform.FindChild("Toggle"+i.ToString()).GetComponent<UIToggle>().value = false;
			}
		}
		selAllSettingSB.value = 0;
	}
	
	public void OnCloseSelAllSetting()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		popSelectAllSetting.SetActive(false);
	}
	
	public void OnSelAllSettingOK()
	{
		Debug.Log("Select all setting OK!");
		bool isSelectSth = CheckToggleState();
		if(!isSelectSth)
		{
			//您没有选中任何全选的卡牌//
			string str = TextsData.getData(554).chinese;
			ToastWindow.mInstance.showText(str);
			return;
		}
		else
		{
			requestType = 7;
			string ids = GetSettingIds();
			Debug.Log("ids:"+ids);
			UIJson uiJson = new UIJson();
			uiJson.UIJsonForPackSelectAll(46,ids);
			PlayerInfo.getInstance().sendRequest(uiJson,this);
		}
	}
	
	bool CheckToggleState()
	{
		bool selectSth = false;
		for(int i=0;i<7;i++)
		{
			if(selAllSettingGridObj.transform.FindChild("Toggle"+i.ToString()).GetComponent<UIToggle>().value)
			{
				selectSth = true;
				return selectSth;
			}
		}
		return selectSth;
	}
	
	string GetSettingIds()
	{
		string idsStr = "";
		List<string> ids = new List<string>();
		for(int i=0;i<7;i++)
		{
			if(selAllSettingGridObj.transform.FindChild("Toggle"+i.ToString()).GetComponent<UIToggle>().value)
			{
				ids.Add(i.ToString());
			}
		}
		tempSelSetting = ids;
		if(ids.Count == 1)
		{
			idsStr = ids[0];
		}
		else
		{
			for(int i=0;i<ids.Count;i++)
			{
				if(i<ids.Count-1)
				{
					idsStr += ids[i] +"&";
				}
				else
				{
					idsStr += ids[i];
				}
			}
		}
		return idsStr;
	}
	
	/**选中返回:点一次添加,再点一次删除**/
	public void selectedCallBack(int index)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		if(index<0)
		{
			return;
		}
		selectCardParam = index;
		selectCardIndex = cells[index].pe.i;
        selectCardElement = cells[index].pe;
		if(!isSaleStatic)
		{
			if(index<0 || index>=cells.Count)
			{
				return;
			}
			PackElement pe=cells[index].pe;
			//string icon,string name,string level,string star,string attribute,string des,string sale
			
			if(index+1>cells.Count)
			{
				nextCard.SetActive(false);
			}
			else
			{
				nextCard.SetActive(true);
			}
			
			if(index-1<0)
			{
				lastCard.SetActive(false);
			}
			else
			{
				lastCard.SetActive(true);
			}
			
			switch(pe.eType)
			{
			case 1://角色卡//
				CardData cd=CardData.getData(pe.dataId);
				if(cd!=null)
				{
					if(pe.use==1)
					{
						//请求卡组此位置的所有物品//
						requestType=5;
						PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK_DETAIL,pe.i,pe.eType),this);
					}
					else
					{
						popCardDetail.setContent(pe,null,null,null);
						pack.SetActive(false);
					}
				}
				break;
			case 2://主动技能//
				SkillData sd=SkillData.getData(pe.dataId);
				if(sd!=null)
				{
					string skillAddData = Statics.getSkillValueForUIShow02(pe.dataId, pe.lv);
					popOtherDetail.setContentNew(pe.eType,GameHelper.E_CardType.E_Skill,pe.dataId,sd.name,pe.lv.ToString(),sd.star.ToString(),sd.getElementName(),sd.description,sd.sell.ToString(),skillAddData);
				}
				break;
			case 3://被动技能//
				PassiveSkillData pd=PassiveSkillData.getData(pe.dataId);
				if(pd!=null)
				{
					popOtherDetail.setContentNew(pe.eType,GameHelper.E_CardType.E_PassiveSkill,pe.dataId,pd.name,pd.level.ToString(),pd.star.ToString(),"",pd.describe,pd.sell.ToString());
				}
				break;
			case 4://装备//
				EquipData ed=EquipData.getData(pe.dataId);
				if(ed!=null)
				{
					int addValue = EquippropertyData.getValue(ed.type,pe.lv,ed.star);
					popOtherDetail.setContentNew(pe.eType,GameHelper.E_CardType.E_Equip,pe.dataId,ed.name,pe.lv.ToString(),ed.star.ToString(),Statics.getEquipValue(ed,pe.lv).ToString(),ed.description,ed.sell.ToString(),addValue.ToString());
				}
				break;
			case 5://材料//
				ItemsData itemD=ItemsData.getData(pe.dataId);
				if(itemD!=null)
				{
					popOtherDetail.setContentNew(pe.eType,GameHelper.E_CardType.E_Item,pe.dataId,itemD.name,pe.lv.ToString(),itemD.star.ToString(),pe.pile.ToString(),itemD.discription,(itemD.sell*pe.pile).ToString());
				}
				break;
			}
			
			return;
		}
		else if(isSaleStatic)
		{
			if(index < 0 || index >= notPackCells.Count)
				return;
			if(notPackCells.Count>0)
			{
				/**如果有物品icon,添加已选中图片**/
				Transform tf=grids[index].transform.FindChild(SelectedGbName);
				if(!tf.gameObject.activeSelf)
				{
					tf.gameObject.SetActive(true);
					checkSelectAll();
					/**记录下来相应物品**/
					
			            if (!selectedCells.Contains(notPackCells[index]))
						{
			                selectedCells.Add(notPackCells[index]);
						}
				}
				else
				{
					tf.gameObject.SetActive(false);
//					selectedAll=false;
//					btnSelectAll.transform.FindChild("Label").gameObject.GetComponent<UILabel>().text=TextsData.getData(11).chinese;
					checkSelectAll();
					/**清除相应物品**/
					selectedCells.Remove(notPackCells[index]);
				}
			}
		}
		
	}
	/**如果全选了,设置为全选(适用手动全选)**/
	private void checkSelectAll()
	{
		if(cells!=null)
		{
			bool mark=true;
            for (int i = 0; i < notPackCells.Count; i++)
			{
				PackElement pe = notPackCells[i];
				
				GameObject gb=grids[i];
				Transform tf=gb.transform.FindChild(SelectedGbName);
				
				
				switch(pe.eType)
				{
				case 1://角色卡//
					CardData cd=CardData.getData(pe.dataId);
					if(cd!=null)
					{
						//是否选择了金币卡//
						if(curSelSetting.Contains("0"))
						{
							//卡牌是金币卡//
							if(cd.race == 7)
							{
								if(tf==null || !tf.gameObject.activeSelf)
								{
									mark=false;
								}
							}
						}
						//是否选择了其他星级的//
						if(curSelSetting.Contains(cd.star.ToString()))
						{
							if(tf==null || !tf.gameObject.activeSelf)
							{
								mark=false;
							}
						}
					}
					break;
				case 2://主动技能//
					SkillData sd=SkillData.getData(pe.dataId);
					if(sd!=null)
					{
						if(curSelSetting.Contains(sd.star.ToString()))
						{
							if(tf==null || !tf.gameObject.activeSelf)
							{
								mark=false;
							}
						}
					}
					break;
				case 3://被动技能//
					PassiveSkillData pd=PassiveSkillData.getData(pe.dataId);
					if(pd!=null)
					{
						if(curSelSetting.Contains(pd.star.ToString()))
						{
							if(tf==null || !tf.gameObject.activeSelf)
							{
								mark=false;
							}
						}
					}
					break;
				case 4://装备//
					EquipData ed=EquipData.getData(pe.dataId);
					if(ed!=null)
					{
						if(curSelSetting.Contains(ed.star.ToString()))
						{
							if(tf==null || !tf.gameObject.activeSelf)
							{
								mark=false;
							}
						}
					}
					break;
				case 5://材料//
					ItemsData itemD=ItemsData.getData(pe.dataId);
					if(itemD!=null)
					{
						if(curSelSetting.Contains(itemD.star.ToString()))
						{
							if(tf==null || !tf.gameObject.activeSelf)
							{
								mark=false;
							}
						}
					}
					break;
				}
			}
			if(mark)
			{
				selectedAll=true;
				//取消全选//
				btnSelectAll.transform.FindChild("Label").gameObject.GetComponent<UILabel>().text=TextsData.getData(12).chinese;
			}
			else
			{
				selectedAll = false;
				//全选//
				btnSelectAll.transform.FindChild("Label").gameObject.GetComponent<UILabel>().text=TextsData.getData(11).chinese;
			}
		}
	}
	
	private void selectAll()
	{
		selectedAll=true;
		//取消全选//
		btnSelectAll.transform.FindChild("Label").gameObject.GetComponent<UILabel>().text=TextsData.getData(12).chinese;
		for(int i=0;i<grids.Count;i++)
		{
            if (i >= notPackCells.Count)
			{
				continue;
			}
			PackElement pe = notPackCells[i];
			
			switch(pe.eType)
			{
			case 1://角色卡//
				CardData cd=CardData.getData(pe.dataId);
				if(cd!=null)
				{
					//是否选择了金币卡//
					if(curSelSetting.Contains("0"))
					{
						//卡牌是金币卡//
						if(cd.race == 7)
						{
							if(!selectedCells.Contains(notPackCells[i]))
							{
				                selectedCells.Add(notPackCells[i]);
								grids[i].transform.FindChild(SelectedGbName).gameObject.SetActive(true);
							}
						}
					}
					//是否选择了其他星级的//
					if(curSelSetting.Contains(cd.star.ToString()))
					{
						if(!selectedCells.Contains(notPackCells[i]))
						{
			                selectedCells.Add(notPackCells[i]);
							grids[i].transform.FindChild(SelectedGbName).gameObject.SetActive(true);
						}
					}
				}
				break;
			case 2://主动技能//
				SkillData sd=SkillData.getData(pe.dataId);
				if(sd!=null)
				{
					if(curSelSetting.Contains(sd.star.ToString()))
					{
						if(!selectedCells.Contains(notPackCells[i]))
						{
			                selectedCells.Add(notPackCells[i]);
							grids[i].transform.FindChild(SelectedGbName).gameObject.SetActive(true);
						}
					}
				}
				break;
			case 3://被动技能//
				PassiveSkillData pd=PassiveSkillData.getData(pe.dataId);
				if(pd!=null)
				{
					if(curSelSetting.Contains(pd.star.ToString()))
					{
						if(!selectedCells.Contains(notPackCells[i]))
						{
			                selectedCells.Add(notPackCells[i]);
							grids[i].transform.FindChild(SelectedGbName).gameObject.SetActive(true);
						}
					}
				}
				break;
			case 4://装备//
				EquipData ed=EquipData.getData(pe.dataId);
				if(ed!=null)
				{
					if(curSelSetting.Contains(ed.star.ToString()))
					{
						if(!selectedCells.Contains(notPackCells[i]))
						{
			                selectedCells.Add(notPackCells[i]);
							grids[i].transform.FindChild(SelectedGbName).gameObject.SetActive(true);
						}
					}
				}
				break;
			case 5://材料//
				ItemsData itemD=ItemsData.getData(pe.dataId);
				if(itemD!=null)
				{
					if(curSelSetting.Contains(itemD.star.ToString()))
					{
						if(!selectedCells.Contains(notPackCells[i]))
						{
			                selectedCells.Add(notPackCells[i]);
							grids[i].transform.FindChild(SelectedGbName).gameObject.SetActive(true);
						}
					}
				}
				break;
			}
			/**记录下来相应物品**/
//			if(!selectedCells.Contains(notPackCells[i]))
//			{
//                selectedCells.Add(notPackCells[i]);
//			}
		}
	}
	
	 
	
	/**清除所有已选中图片**/
	private void clearSelectedAll()
	{
		selectedAll=false;
		//全选//
		btnSelectAll.transform.FindChild("Label").gameObject.GetComponent<UILabel>().text=TextsData.getData(11).chinese;
		for(int i=0;i<grids.Count;i++)
		{
			if(cells!=null)
			{
				if(i>=cells.Count)
				{
					continue;
				}
				grids[i].transform.FindChild(SelectedGbName).gameObject.SetActive(false);
				/**清除相应物品**/
	            selectedCells.Remove(cells[i].pe);
			}
		}
		for(int i=0;i<grids.Count;i++)
		{
			if(notPackCells!=null)
			{
				if(i>=notPackCells.Count)
				{
					continue;
				}
				grids[i].transform.FindChild(SelectedGbName).gameObject.SetActive(false);
				/**清除相应物品**/
	            selectedCells.Remove(notPackCells[i]);
			}
		}
	}
	/**打开出售**/
	private void openSale()
	{
		/**标记为出售状态**/
		isSaleStatic=true;
		/**显示全选和确定按钮**/
		changeSaleMark(true);
		/**显示网格**/
		showPackContent(curType);
	}
	/**关闭出售**/
	private void closeSale()
	{
		clearSelectedAll();
		isSaleStatic=false;
		changeSaleMark(false);
		/**显示网格**/
		showPackContent(curType);
	}
	private void changeSaleMark(bool isSale)
	{
		if(isSale)
		{
			btnSelectAll.gameObject.SetActive(true);
			btnSelectAllSetting.gameObject.SetActive(true);
			//btnSale.transform.FindChild("Background").gameObject.GetComponent<UISprite>().spriteName="btn_sure";
		}
		else
		{
			btnSelectAll.gameObject.SetActive(false);
			btnSelectAllSetting.gameObject.SetActive(false);
			//btnSale.transform.FindChild("Background").gameObject.GetComponent<UISprite>().spriteName="btn_sale";
		}
	}
	
	/**绘制格子,每个格子大小128*160**/  
	private void drawGrid(List<PackElementJson> cells)
	{
        //背包用//
		int length=10;
		if(cells.Count>10)
		{
			if((cells.Count-10)%5==0)
			{
				length=cells.Count;
			}
			else
			{
				length=10+((cells.Count-10)/5+1)*5;
			}
		}
		//隐藏多余的网格//
		for(int i=grids.Count-1;grids.Count>length && i>=0;i--)
		{
			if(i>=length)
			{
				grids[i].transform.FindChild("title_new").gameObject.SetActive(false);
				grids[i].SetActive(false);
			}
		}
		//grids.Clear();
		//GameObjectUtil.destroyGameObjectAllChildrens(packWindowGridParent);
		
		for(int i=0;i<length;i++)
		{
			//如果当前网格不够,克隆新的网格//
			if(i>=grids.Count)
			{
				if(packGridCell==null)
				{
					packGridCell=GameObjectUtil.LoadResourcesPrefabs("UI-pack/CardItem",3);
				}
				GameObject gb=Instantiate(packGridCell) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(gb,packWindowGridParent,true);
				
				gb.GetComponent<SimpleCardInfo1>().clear();
				gb.SetActive(true);
				grids.Add(gb);
			}
			else
			{
				grids[i].transform.FindChild("title_new").gameObject.SetActive(false);
				grids[i].GetComponent<SimpleCardInfo1>().clear();
				grids[i].SetActive(true);
			}
		}
	}

    /**绘制格子,每个格子大小128*160**/
    private void drawGrid(List<PackElement> cells)
    {
        //非背包用//
        int length = 10;
        if (cells.Count > 10)
        {
            if ((cells.Count - 10) % 5 == 0)
            {
                length = cells.Count;
            }
            else
            {
                length = 10 + ((cells.Count - 10) / 5 + 1) * 5;
            }
        }
        //隐藏多余的网格//
        for (int i = grids.Count - 1; grids.Count > length && i >= 0; i--)
        {
            if (i >= length)
            {
                grids[i].transform.FindChild("title_new").gameObject.SetActive(false);
                grids[i].SetActive(false);
            }
        }
        //grids.Clear();
        //GameObjectUtil.destroyGameObjectAllChildrens(packWindowGridParent);

        for (int i = 0; i < length; i++)
        {
            //如果当前网格不够,克隆新的网格//
            if (i >= grids.Count)
            {
                if (packGridCell == null)
                {
                    packGridCell = GameObjectUtil.LoadResourcesPrefabs("UI-pack/CardItem", 3);
                }
                GameObject gb = Instantiate(packGridCell) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(gb, packWindowGridParent,true);
                gb.GetComponent<SimpleCardInfo1>().clear();
                gb.SetActive(true);
                grids.Add(gb);
            }
            else
            {
                grids[i].transform.FindChild("title_new").gameObject.SetActive(false);
                grids[i].GetComponent<SimpleCardInfo1>().clear();
                grids[i].SetActive(true);
            }
        }
    }

	/**显示网格icon**/
	private void showGridMidgroud(List<PackElementJson> cells)
	{
        //背包用//
		if(cells==null)
		{
			return;
		}
		for(int i=0;i<cells.Count;i++)
		{
			SimpleCardInfo1 sci=grids[i].GetComponent<SimpleCardInfo1>();
			PackElement pe=cells[i].pe;
			//==newMark==//
			if(pe.newType==0)
			{
				grids[i].transform.FindChild("title_new").gameObject.SetActive(true);
			}
			else
			{
				grids[i].transform.FindChild("title_new").gameObject.SetActive(false);
			}
			int saleValue=0;
			switch(pe.eType)
			{
			case 1://角色卡//
                sci.cardUserLabel.gameObject.SetActive(false);
				sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Hero,pe);
				saleValue=CardData.getData(pe.dataId).sell*pe.lv;
				break;
			case 2://主动技能//
				sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Skill,pe);
				saleValue=SkillData.getData(pe.dataId).sell;
                int fromId = cells[i].type;
                sci.cardUserLabel.gameObject.SetActive(true);
                sci.setCardUserInfo(fromId);
				break;
			case 3://被动技能//
				sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_PassiveSkill,pe);
				saleValue=PassiveSkillData.getData(pe.dataId).sell;
                fromId = cells[i].type;
                sci.cardUserLabel.gameObject.SetActive(true);
                sci.setCardUserInfo(fromId);
				break;
			case 4://装备//
				sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Equip,pe);
				saleValue=EquipUpgradeData.getData(pe.lv).sell + EquipData.getData(pe.dataId).sell;
                fromId = cells[i].type;
                sci.cardUserLabel.gameObject.SetActive(true);
                sci.setCardUserInfo(fromId);
				break;
			case 5://材料//
                sci.cardUserLabel.gameObject.SetActive(false);
                //ItemsData itemD = ItemsData.getData(pe.dataId);
				sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Item,pe);
				saleValue=ItemsData.getData(pe.dataId).sell * pe.pile;
				break;
			}
			sci.bm.target=_myTransform.gameObject;
			sci.bm.param=i;
			sci.bm.functionName="selectedCallBack";
			if(isSaleStatic)
			{
				grids[i].GetComponent<SaleInfo>().setData(saleValue);
			}
			else
			{
				grids[i].GetComponent<SaleInfo>().hide();
			}
		}
	}

    private void showGridMidgroud(List<PackElement> cells)
    {
        //非背包用//
        if (cells == null)
        {
            return;
        }
        for (int i = 0; i < cells.Count; i++)
        {
            SimpleCardInfo1 sci = grids[i].GetComponent<SimpleCardInfo1>();
            PackElement pe = cells[i];
            //==newMark==//
            if (pe.newType == 0)
            {
                grids[i].transform.FindChild("title_new").gameObject.SetActive(true);
            }
            else
            {
                grids[i].transform.FindChild("title_new").gameObject.SetActive(false);
            }
            int saleValue = 0;
            switch (pe.eType)
            {
                case 1://角色卡//
                    sci.cardUserLabel.gameObject.SetActive(false);
					sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Hero,pe);
                    saleValue = CardData.getData(pe.dataId).sell * pe.lv;
					sci.cardUserLabel.gameObject.SetActive(false);
                    break;
                case 2://主动技能//
					sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Skill,pe);
                    saleValue = SkillData.getData(pe.dataId).sell;
                	sci.cardUserLabel.gameObject.SetActive(false);    
					break;
                case 3://被动技能//
					sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_PassiveSkill,pe);
                    saleValue = PassiveSkillData.getData(pe.dataId).sell;
                    sci.cardUserLabel.gameObject.SetActive(false);
					break;
                case 4://装备//
					sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Equip,pe);
                    saleValue = EquipUpgradeData.getData(pe.lv).sell + EquipData.getData(pe.dataId).sell;
					sci.cardUserLabel.gameObject.SetActive(false);
                    break;
                case 5://材料//
					sci.setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Item,pe);
					sci.cardUserLabel.gameObject.SetActive(false);
                    saleValue = ItemsData.getData(pe.dataId).sell*pe.pile;
                    break;
            }
			sci.bm.target=_myTransform.gameObject;
			sci.bm.param=i;
			sci.bm.functionName="selectedCallBack";
            if (isSaleStatic)
            {
                grids[i].GetComponent<SaleInfo>().setData(saleValue);
            }
            else
            {
                grids[i].GetComponent<SaleInfo>().hide();
            }
        }
    }

	/**清除网格icon**/
	private void clearGridMidgroud()
	{
		foreach(GameObject gb in grids)
		{
			gb.GetComponent<SimpleCardInfo1>().clear();
			//Transform tf=gb.transform.FindChild(MidGbName);
			//UISprite sprite=tf.gameObject.GetComponent<UISprite>();
			//sprite.spriteName=null;
		}
	}
	/**type:1角色卡,2技能卡,3装备卡,4材料卡,8被动技能卡**/
	private void showPack(int type)
	{
		showPackNum();
		showPackContent(type);
	}
	
	private void showPackNum()
	{
		if(isSaleStatic)
		{
			packNum.SetActive(false);
			packExtend.SetActive(false);
		}
		else
		{
			BagCostData bd=BagCostData.getData(prj.buyTimes);
			int total=0;
			if(bd!=null)
			{
				total=bd.number1;
			}
			packNum.SetActive(true);
			packExtend.SetActive(true);
			packNum.GetComponent<UILabel>().text=prj.pejs.Count+"/"+total;
		}
	}
	
	private void showPackContent(int type)
	{
		/**清除同类型网格的前景图**/
		clearGridMidgroud();
		/**获得当前条件下的cells**/
        if (!isSaleStatic)
        {
            //背包用//
            cells =prj.pejs;
            /**绘制网格**/
            drawGrid(cells);
            /**绘制元素图**/
            showGridMidgroud(cells);
        }
        else
        {
            //非背包用//
            notPackCells = PlayerInfo.getCells(type, 0, 0, 0, elements);
            /**绘制网格**/
            drawGrid(notPackCells);
            /**绘制元素图**/
            showGridMidgroud(notPackCells);
        }
		
		packWindowGridParent.GetComponent<UIGrid2>().repositionNow=true;
		packWindow.transform.FindChild("Scroll Bar-pack").gameObject.GetComponent<UIScrollBar>().value=0;
	}
	/**向服务器发送要出售的物品**/
	private void sendSale()
	{
		if(selectedCells==null || selectedCells.Count==0)
		{
			if(ToastWindow.mInstance!=null)
			{
				//请选择物品//
				string str = TextsData.getData(20).chinese;
				
				ToastWindow.mInstance.showText(str);
				//提示去充值//
//				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
			}
			return;
		}
		int saleValue=0;

		foreach(PackElement pe in selectedCells)
		{
			switch(curType)
			{
			case 1://==角色卡==//
				CardData cd=CardData.getData(pe.dataId);
				saleValue+=cd.sell*pe.lv;
                if (cd.star >= 3)
                    isReachStar = true;
                else
                    isReachStar = false;
				break;
			case 2://==主动技能卡==//
				SkillData sd=SkillData.getData(pe.dataId);
				saleValue+=sd.sell;
                if (sd.star >= 3)
                    isReachStar = true;
                else
                    isReachStar = false;
				break;
			case 3://==装备卡==//
				EquipUpgradeData ed=EquipUpgradeData.getData(pe.lv);
				EquipData eData = EquipData.getData(pe.dataId);
				saleValue+=ed.sell;
				saleValue+=eData.sell;

                EquipData edd = EquipData.getData(pe.dataId);
                if (edd.star >= 3)
                    isReachStar = true;
                else
                    isReachStar = false;
				break;
			case 4://==材料卡==//
				ItemsData id=ItemsData.getData(pe.dataId);
				saleValue+=id.sell*pe.pile;
                if (id.star >= 3)
                    isReachStar = true;
                else
                    isReachStar = false;
				break;
			case 8://==神力卡==//
				PassiveSkillData psd=PassiveSkillData.getData(pe.dataId);
				saleValue+=psd.sell;
                if (psd.star >= 3)
                    isReachStar = true;
                else
                    isReachStar = false;
				break;
			}
		}
        if (isReachStar)
        {
			//确定出售吗？/n可获得num金币//
            ToastWarnUI.mInstance.showWarn(TextsData.getData(202).chinese.Replace("num", saleValue + "").Replace("/n", "\n"), this, true);
        }
        else
            ToastWarnUI.mInstance.showWarn(TextsData.getData(202).chinese.Replace("num", saleValue + "").Replace("/n", "\n"), this);
	}
	
	public void onClickBuyPackageTimes()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		BagCostData bd=BagCostData.getData(prj.buyTimes+1);
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
		isBuyPackNum=true;
		ToastWarnUI.mInstance.showWarn(msg,this);
	}
	
	public void warnningSure()
	{
		if(isBuyPackNum)
		{
			requestType=6;
			PlayerInfo.getInstance().sendRequest(new BuyBagJson(),this);
		}
		else
		{
			List<int> list=new List<int>();
			switch(curType)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 8:
				foreach(PackElement pe in selectedCells)
				{
					list.Add(pe.i);
				}
				break;
			}
	        if (isReachStar)
	        {
				//出售的卡牌中包含3星或更高星级卡牌，是否继续？//
	            ToastWarnUI.mInstance.showWarn(TextsData.getData(377).chinese, this);
	            isReachStar = false;
	        }
	
	        else
	        {
	            requestType = 3;
	            PlayerInfo.getInstance().sendRequest(new SaleJson(curType, list), this);
	        }
		}
	}
	
	public void warnningCancel()
    {
        isBuyPackNum = false;
    }
	
		/*点击查看下一个物品*/
	public void onClickNextCard(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(param == 1)
		{
		    selectedCallBack(selectCardParam+1);
		}
		else if(param == -1)
		{
			selectedCallBack(selectCardParam-1);
		}
		
		if(selectCardParam+1>cells.Count)
		{
			nextCard.SetActive(false);
		}
		else
		{
			nextCard.SetActive(true);
		}
		
		if(selectCardParam-1<0)
		{
			lastCard.SetActive(false);
		}
		else
		{
			lastCard.SetActive(true);
		}
	}

	/**处理服务器返回**/
	public void receiveResponse(string json)
	{
        Debug.Log("json========="+json);
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
			case 4:
				prj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
                receivedData=true;
				break;
            case 2:
                PackResultJson pr=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = pr.errorCode;
				Debug.Log("chushou:"+json);
                elements = pr.list;
				curSelSetting = pr.allSelect;
				//如果玩家没有进行过设置，默认设置为前三个选择//
				if(curSelSetting == null || (curSelSetting.Count == 1 && curSelSetting.Contains("")))
				{
					if(curSelSetting == null)
					{
						curSelSetting = new List<string>();
					}
					else if(curSelSetting.Count == 1 && curSelSetting.Contains(""))
					{
						curSelSetting.Clear();
					}
					curSelSetting.Add("0");
					curSelSetting.Add("1");
					curSelSetting.Add("2");
				}
                receivedData=true;
                break;
			case 3:
				SaleResultJson srj=JsonMapper.ToObject<SaleResultJson>(json);
				errorCode = srj.errorCode;
				addGold=srj.addG;
				playerGold=srj.g;
				showGold=true;
				receivedData=true;
				break;
			case 5:
				prj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
				card=null;
				skill=null;
				pSkillList=null;
				equips=null;
				if(prj.list!=null)
				{
                    foreach (PackElement pe in prj.list)
					{
						switch(pe.eType)
						{
						case 1:
							card=pe;
							break;
						case 2:
                            skill = pe;
							break;
						case 4:
							if(equips==null)
							{
								equips=new List<PackElement>();
							}
                            equips.Add(pe);
							break;
						}
					}
				}
				
				if(prj.list2 != null)
				{
					if(pSkillList == null)
					{
						pSkillList = new List<PackElement>();
					}
					for(int i = 0; i < prj.list2.Count;++i)
					{
						pSkillList.Add(prj.list2[i]);
					}
				}
				
				if(card!=null)
				{
					receivedData=true;
				}
				break;
			case 6://==购买背包上限==//
				BuyResultJson brj=JsonMapper.ToObject<BuyResultJson>(json);
				errorCode=brj.errorCode;
				if(errorCode==0)
				{
					prj.buyTimes=brj.buyTimes;
					buyPackUseCrystal = PlayerInfo.getInstance().player.crystal - brj.crystal;
					PlayerInfo.getInstance().player.gold=brj.gold;
					PlayerInfo.getInstance().player.crystal=brj.crystal;
				}
				receivedData=true;
				break;
			case 7:
				Debug.Log("request7 json:"+json);
				PackResultJson tpr=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = tpr.errorCode;
				receivedData = true;
				break;
			}
		}
	}
	
}
