using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInfoContrl : MonoBehaviour {
	private Transform _myTransform;
	
	private int curCardId;
	private int curEquipSkillId;
	private int cardBreakNum;			//卡牌突破次数//
	//private int power;					//卡牌战力//
	private Color TalentNameColor;
	private Color TalentDetailsColor;
	private string curActiveSkillAddData;
	private int atkNum;//攻击力数值//
	private int defNum;//防御力数值//
	private int hpNum;//生命力数值//
	private int otherAtkNum;//额外增加的攻击力//
	private int otherDefNum;//额外增加的防御力//
	private int otherHpNum;//额外增加的生命值//
	
//	private Color NeedCardColor;
	//public UILabel battlePower;
	public UILabel Race;
	public UISprite RaceSpr;
	public UILabel SkillName;
	public UILabel SkillDes;
	public UISprite SkillSprite;
	public UILabel TalentName1;
	public UILabel TalentName2;
	public UILabel TalentName3;
	public UILabel TalentDes1;
	public UILabel TalentDes2;
	public UILabel TalentDes3;
	public UISprite TalentSprite1;
	public UISprite TalentSprite2;
	public UISprite TalentSprite3;
	public UILabel CardDetails;
	public UILabel ATKBG;
	public UILabel DEFBG;
	public UILabel HPBG;
	
	//第一个合体技的prefab//
	public GameObject skillBoxPrefab01;
	//其他prefab//
	public GameObject skillBoxPrefab02;
	
	//skillBox的父节点//
	public GameObject uniteSkillBox;
	//显示卡牌获得途径的panel//
//	public GameObject getWayPanel;
//	public GameObject getWayDragParent;
	//合体技获得信息的prefab//
//	public GameObject getWayItemPrefab;
//	public GameObject getWayDragPanel;
//	public GameObject scrollBar;
	
	public bool isKoExchange;
	
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
//	private DropGuideResultJson drj;
	
	private bool isSaveColor = false;
	
	float startX = -66;
	float startY = -63;
	float offY1 = -155;
	float offY2 = -185;
	
	//float getWayItemStartY = 180f;
	//float getWayItemOffY = 160f;
	//描述//
	public Transform Description;
//	public Card_UniteSkillItem[] skillBox;
	private Hashtable uniteSkillCards = new Hashtable();

    public UIGrid breakGrid;

    public UISprite icon;

    public string itemPath = "Prefabs/UI/EmbattlePanel/BreakLabelItem";

    public UIScrollBar units;
	
	void Awake(){
		_myTransform = transform;
		init();
	}

	// Use this for initialization
	void Start () {
		//NeedCardColor = skillBox[1].needCardNames[0].color;
	}
	
	public void init()
	{
		if(!isSaveColor)
		{
			TalentNameColor = TalentName2.color; 
			TalentDetailsColor = TalentDes2.color;
			isSaveColor = true;
		}
		ATKBG.text = "";
		DEFBG.text = "";
		HPBG.text = "";
//		getWayPanel.SetActive(false);
		
		TalentName1.transform.localPosition= new Vector3(-60,-30,0);
		TalentName2.transform.localPosition= new Vector3(-60,-100,0);
		TalentName3.transform.localPosition= new Vector3(-60,-170,0);
		TalentDes1.transform.localPosition= new Vector3(-60,-70,0);
		TalentDes2.transform.localPosition= new Vector3(-60,-140,0);
		TalentDes3.transform.localPosition= new Vector3(-60,-210,0);
		TalentSprite1.transform.localPosition= new Vector3(-75,-30,0);
		TalentSprite2.transform.localPosition= new Vector3(-75,-100,0);
		TalentSprite3.transform.localPosition = new Vector3(-75,-170,0);
		Description.transform.localPosition = new Vector3(-50,-65,0);
	}
	
	public void initCardInfoData(){
        GameObjectUtil.destroyGameObjectAllChildrens(breakGrid.gameObject);
		GetComponent<SpreadManager>().initScrollBar();
		if(curCardId > 0)
		{
			//获取卡组界面的实例//
//			CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
				
			GameObjectUtil.destroyGameObjectAllChildrens(uniteSkillBox);
			
			CardData cd = CardData.getData(curCardId);
			SkillData sd = SkillData.getData(curEquipSkillId);
			TalentData talent = TalentData.getData(cd.talent);
			TalentData talent2 = TalentData.getData(cd.talent2);
			TalentData talent3 = TalentData.getData(cd.talent3);
            TalentData talent4 = TalentData.getData(cd.talent4);
			
			//战力//
			if(!isKoExchange)
			{
				//battlePower.text=TextsData.getData(203).chinese+power;
			}
			else
			{
				//battlePower.gameObject.SetActive(false);
				isKoExchange = false;
			}
			//显示攻防血//
			if(otherAtkNum == 0)
			{
				ATKBG.text = atkNum.ToString();
			}
			else
			{
				ATKBG.text = (atkNum - otherAtkNum)+"(+"+otherAtkNum+")";
			}
			if(otherDefNum == 0)
			{
				DEFBG.text = defNum.ToString();
			}
			else
			{
				DEFBG.text = (defNum - otherDefNum)+"(+"+otherDefNum+")";
			}
			if(otherHpNum == 0)
			{
				HPBG.text = hpNum.ToString();
			}
			else
			{
				HPBG.text = (hpNum - otherHpNum)+"(+"+otherHpNum+")";
			}
			//种族//
			int raceId = cd.race;
			//string s0 = TextsData.getData(168).chinese;
			string s2 = "";
			if(raceId < 3)
			{
				s2 = TextsData.getData(4 + raceId).chinese;
			}
			else if(raceId == 3)
			{
				s2 = TextsData.getData(8).chinese;
			}
			else if(raceId == 4)
			{
				s2 = TextsData.getData(7).chinese;
			}
			Race.text = s2;
			
			switch(raceId)
			{
			case 1:
				RaceSpr.spriteName = "race_1";
				break;
			case 2:
				RaceSpr.spriteName = "race_2";
				break;
			case 3:
				RaceSpr.spriteName = "race_3";
				break;
			case 4:
				RaceSpr.spriteName = "race_4";
				break;
			case 6:
			case 7:
				RaceSpr.spriteName = "";
				break;
			}
			
			
//			Race.text =  s2;
			//技能//
			SkillName.text = sd.name;
			SkillDes.text = sd.description + curActiveSkillAddData;
			SkillSprite.spriteName = sd.icon2;

            Object Breakitem = Resources.Load("Prefabs/UI/EmbattlePanel/BreakLabelItem");
			//天赋//
			if(talent!=null)
			{
                GameObject itemBreak = Instantiate(Breakitem) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(itemBreak, breakGrid.gameObject);
                itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = talent.name;
                itemBreak.transform.FindChild("Label").GetComponent<UILabel>().text = talent.description;
				TalentName1.text = talent.name;
				TalentDes1.text = talent.description;
				TalentSprite1.spriteName = talent.icon;
			}
			else
			{
				TalentName1.text = string.Empty;
				TalentDes1.text = string.Empty;
				TalentSprite1.spriteName = string.Empty;
			}
			
			if(talent2 != null)
			{
                GameObject itemBreak = Instantiate(Breakitem) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(itemBreak, breakGrid.gameObject);
                
                itemBreak.transform.FindChild("Label").GetComponent<UILabel>().text = talent.description;
				TalentDes2.text = talent2.description;
				if(cardBreakNum < Constant.MaxBreakNum1)
				{
					string tp = TextsData.getData(562).chinese;
					TalentName2.text = talent2.name + tp;
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = talent2.name + tp;
					TalentName2.color = Color.gray;
					TalentDes2.color = Color.gray;
					TalentSprite2.spriteName = talent2.icon;
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().color = Color.gray;
                    itemBreak.transform.FindChild("Label").GetComponent<UILabel>().color = Color.gray;
				}
				else
				{
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = talent2.name;
					TalentName2.text = talent2.name;
					TalentName2.color = TalentNameColor;
					TalentDes2.color = TalentDetailsColor;
					TalentSprite2.spriteName = talent2.icon;
				}
			}
			else
			{
				TalentName2.text = string.Empty;
				TalentDes2.text = string.Empty;
				TalentSprite2.spriteName = string.Empty;
				Vector3 talent3N = TalentName3.transform.localPosition;
				Vector3 talent3D = TalentDes3.transform.localPosition;
				Vector3 talent3S = TalentSprite3.transform.localPosition;
				TalentName3.transform.localPosition = new Vector3(talent3N.x,talent3N.y+70,talent3N.z);
				TalentDes3.transform.localPosition = new Vector3(talent3D.x,talent3D.y+70,talent3D.z);
				TalentSprite3.transform.localPosition = new Vector3(talent3S.x,talent3S.y+70,talent3S.z);
				//Description.localPosition = new Vector3(Description.localPosition.x,Description.localPosition.y +70,Description.localPosition.z);
			}
			
			if(talent3 != null)
			{
                GameObject itemBreak = Instantiate(Breakitem) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(itemBreak, breakGrid.gameObject);
                itemBreak.transform.FindChild("Label").GetComponent<UILabel>().text = talent.description;
               
				TalentDes3.text = talent3.description;
				if(cardBreakNum < Constant.MaxBreakNum2)
				{
					string tp = TextsData.getData(192).chinese;
					TalentName3.text = talent3.name +tp;
					TalentName3.color = Color.gray;
					TalentDes3.color = Color.gray;
					TalentSprite3.spriteName = talent3.icon;
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = talent3.name + tp;
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().color = Color.gray;
                    itemBreak.transform.FindChild("Label").GetComponent<UILabel>().color = Color.gray;
				}
				else
				{
					TalentName3.text = talent3.name;
					TalentName3.color = TalentNameColor;
					TalentDes3.color = TalentDetailsColor;
					TalentSprite3.spriteName = talent3.icon;
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = talent3.name;
				}
			}
			else
			{
				TalentName3.text = string.Empty;
				TalentDes3.text = string.Empty;
				TalentSprite3.spriteName = string.Empty;
				//Description.localPosition = new Vector3(Description.localPosition.x,Description.localPosition.y +70,Description.localPosition.z);
			}
            if (talent4 != null)
            {
                GameObject itemBreak = Instantiate(Breakitem) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(itemBreak, breakGrid.gameObject);
                itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = talent.name;
                itemBreak.transform.FindChild("Label").GetComponent<UILabel>().text = talent.description;
            }


            if (cd.star ==5 )
            {
                GameObject itemBreak = Instantiate(Breakitem) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(itemBreak, breakGrid.gameObject);

                if (cardBreakNum < 6)
                {
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = cd.PPSname + TextsData.getData(727).chinese;
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().color = Color.gray;
                    itemBreak.transform.FindChild("Label").GetComponent<UILabel>().color = Color.gray;
                }
                else
                {
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = cd.PPSname;
                }
               
                itemBreak.transform.FindChild("Label").GetComponent<UILabel>().text = cd.PPSdescription;
            }

            else if (cd.star == 6)
            {
                GameObject itemBreak = Instantiate(Breakitem) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(itemBreak, breakGrid.gameObject);

                if (cardBreakNum < 6)
                {
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = cd.PPSname + TextsData.getData(727).chinese;
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().color = Color.gray;
                    itemBreak.transform.FindChild("Label").GetComponent<UILabel>().color = Color.gray;
                }
                else
                {
                    itemBreak.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = cd.PPSname;
                }
                itemBreak.transform.FindChild("Label").GetComponent<UILabel>().text = cd.PPSdescription;

                GameObject itemBreak1 = Instantiate(Breakitem) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(itemBreak1, breakGrid.gameObject);
                itemBreak1.transform.FindChild("BreakLabel").GetComponent<UILabel>().text = TextsData.getData(729).chinese + TextsData.getData(728).chinese;
                itemBreak1.transform.FindChild("Label").GetComponent<UILabel>().text = "";
                itemBreak1.transform.FindChild("BreakLabel").GetComponent<UILabel>().color = Color.gray;
                itemBreak1.transform.FindChild("Label").GetComponent<UILabel>().color = Color.gray;
            }
			//卡牌描述//
			CardDetails.text = cd.description;
			
			//合体技//
			List<int> uniteSkillIds = UnitSkillData.getUnitSkillIds(curCardId);
			if(uniteSkillIds.Count ==0)
			{
				GameObject emptySkillBoxPrefab = Resources.Load("Prefabs/UI/EmbattlePanel/EmptySkillBox") as GameObject;
				if(emptySkillBoxPrefab == null)
					return;
				GameObject emptySkillBoxObj = GameObject.Instantiate(emptySkillBoxPrefab) as GameObject;
				if(emptySkillBoxObj == null)
					return;
				GameObjectUtil.gameObjectAttachToParent(emptySkillBoxObj, uniteSkillBox);
                emptySkillBoxObj.transform.localPosition = new Vector3(0, -50f, 0);
			}
			else
			{
				uniteSkillCards.Clear();
				float nextOffY = 0;
				float y = 0;
				for(int i = 0;i < uniteSkillIds.Count;i++)
				{
					UnitSkillData usd = UnitSkillData.getData(uniteSkillIds[i]);
					if(usd == null)
						continue;
                    if (usd.number >= 100)
                    {
                        if (uniteSkillIds.Count == 1)
                        {
                            GameObject emptySkillBoxPrefab = Resources.Load("Prefabs/UI/EmbattlePanel/EmptySkillBox") as GameObject;
                            if (emptySkillBoxPrefab == null)
                                return;
                            GameObject emptySkillBoxObj = GameObject.Instantiate(emptySkillBoxPrefab) as GameObject;
                            if (emptySkillBoxObj == null)
                                return;
                            GameObjectUtil.gameObjectAttachToParent(emptySkillBoxObj, uniteSkillBox);
                            emptySkillBoxObj.transform.localPosition = new Vector3(0, -50f, 0);
                        }
                        continue;
                    }
					//修改需要卡牌名字//
					List<int> ids = new List<int>();
					int id = usd.card1;
					if(id > 0)
					{
						ids.Add(id);
					}
					id = usd.card2;
					if(id > 0)
					{
						ids.Add(id);
					}
					id = usd.card3;
					if(id > 0)
					{
						ids.Add(id);
					}
					id = usd.card4;
					if(id > 0)
					{
						ids.Add(id);
					}
					id = usd.card5;
					if(id > 0)
					{
						ids.Add(id);
					}
					id = usd.card6;
					if(id > 0)
					{
						ids.Add(id);
					}
					uniteSkillCards.Add(i, ids); 
					
					GameObject skillBox = null;
					if(ids.Count <= 0)
					{
						skillBox = Instantiate(skillBoxPrefab01) as GameObject;
						GameObjectUtil.gameObjectAttachToParent(skillBox, uniteSkillBox);
						
						if(i == 0)
						{
							y += startY;
							skillBox.transform.localPosition = new Vector3(startX, y, 0);
						}
						else if(i > 0)
						{
							y += nextOffY;
							skillBox.transform.localPosition = new Vector3(startX, y, 0);
						}
						nextOffY = offY1 ;
					}
					else 
					{
						skillBox = Instantiate(skillBoxPrefab02) as GameObject;
						GameObjectUtil.gameObjectAttachToParent(skillBox, uniteSkillBox);
						if(i == 0)
						{
							y += startY;
							skillBox.transform.localPosition = new Vector3(startX, y, 0);
						}
						else if(i > 0) 
						{
	//						float y = startY + offY1 + (i-1)*offY2;
							y += nextOffY;
							skillBox.transform.localPosition = new Vector3(startX, y, 0);
						}
						nextOffY = offY2 ;
					}
					
					Card_UniteSkillItem item = skillBox.GetComponent<Card_UniteSkillItem>();
					item.IconNull.SetActive(false);
					item.Icon.gameObject.SetActive(true);
					item.Icon.spriteName = usd.icon;
					item.ConditionsObj.SetActive(true);
					
					item.Name.text = usd.name;
					item.Des.text = usd.description;
					
					
					string s1 = TextsData.getData(188).chinese;
					string s3 = TextsData.getData(189).chinese;
					item.NeedCardsTip.SetActive(true);
					item.NeedCardsTip.GetComponent<UILabel>().text = s1 + usd.cardnum + s3;
					if(item.getWayUbm != null)
					{
						
						item.getWayUbm.target = _myTransform.gameObject;
						item.getWayUbm.param = i;
						item.getWayUbm.functionName = "OnClickGetWayBtn" ;
					}
					
					
					//修改需要卡牌名字//
					if(ids.Count > 0)
					{
						item.NeedCardNames.text = string.Empty;
						for(int m = 0; m < ids.Count;m++)
						{
							item.NeedCardNames.gameObject.SetActive(true);
							
							//名字的格式是[0ffff0]lala[-],此时将其拆分//
							string cardName = CardData.getData(ids[m]).name;
							string[] str = cardName.Split(']');
							string[] ss = str[1].Split('[');
							string name = ss[0];
							
							//判断当前的卡组中是否装备了改卡牌//
							if(Statics.IsCardInCardGroup(PlayerInfo.getInstance().curCardGroup, ids[m]))	
							{
								s1 = "、[00FF00]";
								if(m == 0)
								{
									s1 = "[00FF00]";
								}
								item.NeedCardNames.text += s1 + name +"[-]";
							}
							else 
							{
								s1 = "、[7E7E7E]";
								if(m == 0)
								{
									s1 = "[7E7E7E]";
								}
								item.NeedCardNames.text += s1 + name +"[-]";
							}
						}
					}
						
					
				}
			}
			

		}
		else
		{
			//string s0 = TextsData.getData(168).chinese;
			//battlePower.text=string.Empty;
			Race.text = string.Empty;
			SkillName.text = string.Empty;
			SkillDes.text = string.Empty;
			
			TalentName1.text = string.Empty;
			TalentDes1.text = string.Empty;
			
			TalentName2.text = string.Empty;
			TalentDes2.text = string.Empty;
			
			CardDetails.text = string.Empty;
			
			GameObjectUtil.destroyGameObjectAllChildrens(uniteSkillBox);
			
		}
        breakGrid.repositionNow = true;
	}

    //PackElement card;
	//cardId 当前卡牌的id, skillId 当前卡牌装备的技能的id, cbn卡牌突破次数//
	public void SetData(int cardId, int skillId, int cbn,int power,int atk,int def,int hp,int otherAtk,int otherDef,int otherHp, string skillAddData = "",PackElement cards = null){
		init();
		this.curCardId = cardId;
		this.curEquipSkillId = skillId;
		this.cardBreakNum = cbn;
		//this.power=power;
		this.atkNum = atk;
		this.defNum = def;
		this.hpNum = hp;
		this.otherAtkNum = otherAtk;
		this.otherDefNum = otherDef;
		this.otherHpNum = otherHp;
        //this.card = cards;
		curActiveSkillAddData = skillAddData;
		initCardInfoData();
	}
	
	public void ShowGetWayData(int index)
	{
//		getWayPanel.SetActive(true);
		
		List<int> cardIds = (List<int>)uniteSkillCards[index];
		//有几个卡牌就绘制几个item//
//		for(int i = 0; cardIds!=null && i < cardIds.Count;i ++)
//		{
//			CardData cd = CardData.getData(cardIds[i]);
//			GameObject item = Instantiate(getWayItemPrefab) as GameObject;
//			GameObjectUtil.gameObjectAttachToParent(item, getWayDragParent);
//			float y = getWayItemStartY - i * getWayItemOffY;
//			item.transform.localPosition = new Vector3(0, y , 0);
//			
//			GetWayItem gwi = item.GetComponent<GetWayItem>();
//			gwi.sci2.setSimpleCardInfo(cardIds[i], GameHelper.E_CardType.E_Hero);
//			gwi.Name.text = cd.name;
//			
//			//修改卡牌出处//
//			string getWay = cd.waytoget;
//			string[] str = getWay.Split(',');
//			string showData = "";
//			for(int m = 0;m < str.Length; m++)
//			{
//				int id = StringUtil.getInt(str[m]);
//				string ss = "";
////				id = 6;
//				switch(id)
//				{
//				case 0:
//					ss = TextsData.getData(517).chinese;
//					gwi.Btn_Go.SetActive(false);
//					break;
//				case 1:
//					ss = TextsData.getData(510).chinese;
//					gwi.Btn_Go.SetActive(false);
//					break;
//				case 2:
//					ss = TextsData.getData(511).chinese;
//					gwi.Btn_Go.SetActive(false);
//					break;
//				case 3:
//					ss = TextsData.getData(512).chinese;
//					gwi.Btn_Go.SetActive(false);
//					break;
//				case 4:
//					ss = TextsData.getData(513).chinese;
//					gwi.Btn_Go.SetActive(false);
//					break;
//				case 5:
//					ss = TextsData.getData(514).chinese;
//					gwi.Btn_Go.SetActive(false);
//					break;
//				case 6:			//关卡掉咯//
////					cardIds[i] = 63001;
//					gwi.Btn_Go.SetActive(true);
//					UIButtonMessage ubm = gwi.Btn_Go.GetComponent<UIButtonMessage>();
//					ubm.target = _myTransform.gameObject;
//					ubm.functionName = "OnClickCardDropBtn";
//					ubm.param = cardIds[i];
//					
//					ss = TextsData.getData(555).chinese;
//					break;
//				}
//				showData += ss + "\r\n";
//			}
//			gwi.Des.text = showData;
//		}
		//显示合体技获得途径界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GETWAYPANEL);
		GetWayPanelManager getWay = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GETWAYPANEL, "GetWayPanelManager")as GetWayPanelManager;
		//2 表示从详细信息界面进入//
		getWay.SetData(index, cardIds, 2);	
		
	}
	
	//合体技中卡牌的获得途径的响应 index 可组成的合体技的index//
	public void OnClickGetWayBtn(int index)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		ShowGetWayData(index);
	}
	
	
	public void OnClickGetWayCloseBtn()
	{
//		//播放音效//
//		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
//		GameObjectUtil.destroyGameObjectAllChildrens(getWayDragParent);
////		getWayPanel.SetActive(false);
//		CleanScrollData();
	}
	
	public void OnClickCardDropBtn(int cardId)
	{
		//ComposeData data = ComposeData.getData(cardId,1);//参数1是英雄卡的类型//
		//string str = data.material_num[0];
		//string[] ss = str.Split('-');
		//int itemId = StringUtil.getInt( ss[0]);
//		requestType = 1;
//		itemId = 10101;
//		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_DROP_GUIDE,itemId),this);
	}
	
	public void CleanScrollData()
	{
//		scrollBar.GetComponent<UIScrollBar>().value = 0;
//		getWayDragPanel.transform.localPosition = Vector3.zero;
//		GameObjectUtil.destroyGameObjectAllChildrens(getWayDragParent);
	}
	

	public void CleanData()
	{
		TalentName2.color = TalentNameColor;
		TalentDes2.color = TalentDetailsColor;
	}
	
	
}
