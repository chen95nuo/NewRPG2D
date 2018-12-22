using UnityEngine;
using System.Collections.Generic;

public class LotCardUI : MonoBehaviour,ProcessResponse{
	
	public UIAtlas lotAtals;
	public UIAtlas cardAtals;
	public GameObject starParent;
	public GameObject cardParent;
	public GameObject effectParent;
	public GameObject lot;
	public GameObject result;
	public GameObject label1Ctrl;
	public GameObject label10Ctrl;
	public UILabel friendNum;
	public UILabel securitiesNum;
	public UILabel Need_diamondNum;
	public GameObject crystalMark;
	public GameObject crystal10Mark;
	public GameObject diaosiMark;
	public GameObject friendMark;
	
	public GameObject white;
	public GameObject word;
	public GameObject newMark;
	
	private Transform _myTransform;
	private GameObject fazhenEffect;
	private GameObject nvws;
	private GameObject rotatEffect;
	private GameObject headEffect;
	private GameObject headEffect2;
	private float effectTime;
	//==抽卡cell==//
	private GameObject cardCell;
	
	private bool receiveData;
	private int requestType;
	public LotResultJson lrj;
	public LotResultJson lrjTemp;
	public NewUnitSkillResultJson nusrj;
	private string errorString;
	private int curClickLotId;
	private int whiteMark;
	private bool canClickBtn=true;
	private GameObject curCell;
	private bool haveWord;
	
	public GameObject btnAgainObj;
	
	public GameObject oneCardCtrl;
	public GameObject card3DNode;
	public UILabel card3DName;
	public UISprite card3DStarIcon;
	
	public GameObject pointObj;
	
	public List<GameObject> lotList;
	Vector3 initRotaCard = new Vector3(0,80,0);
	public int playRotateIconIndex = 0;	
	
	public GameObject oneCardEffect;
	
	int errorCode = 0;

    public int freeTimes;

    public UILabel freeLbael;

    public UILabel freeLbael1;

    public GameObject diamonds;
	
	void Awake()
	{
//		_MyObj=gameObject;
//		mInstance=this;
		_myTransform = transform ;
		if (PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_ZH)
        {
            if (UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
            {
                UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
            }
            //获取界面数据//
            requestType = 2;
            PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_LOT), this);
            //播放声音//
            string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MENU).music;
            MusicManager.playBgMusic(musicName);
        }
	}
	
	// Use this for initialization
	void Start () {
//		close();
	}
	

	public void show()
	{
//		base.show();
		showLot();
	}
	
	public void hide()
	{
//		base.hide();
		gc();
		
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT);
	}
	
	private void gc()
	{
		fazhenEffect=null;
		nvws=null;
		rotatEffect=null;
		headEffect=null;
		headEffect2=null;
		cardCell=null;
		lrj=null;
		lrjTemp=null;
		curCell=null;
		lotAtals = null;
		cardAtals = null;
		Resources.UnloadUnusedAssets();
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			
            switch (requestType)
            {
                case 1:
                    if (lrjTemp.errorCode != 0)
                    {
                        if (lrjTemp.errorCode == 53)		//背包空间不足//
                        {
                            errorString = TextsData.getData(78).chinese;
                            //UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_PACKAGE, errorString);
							UIJumpTipManager.mInstance.SetPackageTypeData(UIJumpTipManager.UI_JUMP_TYPE.UI_EXTENDPACKAGE, errorString);
                        }
                        else
                        {
							CardGetData cgd = CardGetData.getData(curClickLotId);
                            errorString = cgd.getCostName() + TextsData.getData(51).chinese;
							if(cgd.type != 2)		//type = 2表示水晶不足//
							{
								ToastWindow.mInstance.showText(errorString);
							}
							else 
							{
								//提示去充值//
								UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, errorString);
							}
                        }
                    }
                    else
                    {
                        lrj = lrjTemp;
						int useCrystal = PlayerInfo.getInstance().player.crystal - lrj.c;
                        PlayerInfo.getInstance().player.crystal = lrj.c;
                        showEffect();
                        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
                        {
                            GuideUI_GetCard.mInstance.hideAllStep();
                        }
                        else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard2))
                        {
                            GuideUI9_GetCard2.mInstance.hideAllStep();
                            GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_GetCard2);
                        }
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        Dictionary<string, object> dic2 = new Dictionary<string, object>();
                        dic.Add("PlayerId", PlayerPrefs.GetString("username"));
                        dic2.Add("PlayerId", PlayerPrefs.GetString("username"));
                        string eventName = "LotCardInfo";
                        if (lrj.list.Count == 1)
                        {
                            CardData cd = CardData.getData(lrj.list[0].dataId);
                            dic.Add("Card", cd.name);
                            TalkingDataManager.SendTalkingDataEvent(eventName, dic);
							if(!TalkingDataManager.isTDPC)//talkingdata统计抽卡消费钻石//
								TDGAItem.OnPurchase("LotCard",1,useCrystal);
                        }
                        else if (lrj.list.Count == 10)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                CardData cd = CardData.getData(lrj.list[i].dataId);
                                dic.Add("Card" + i, cd.name);
                            }
                            for (int i = 5; i < 10; i++)
                            {
                                CardData cd = CardData.getData(lrj.list[i].dataId);
                                dic2.Add("Card" + i, cd.name);
                            }
                            TalkingDataManager.SendTalkingDataEvent(eventName, dic);
                            TalkingDataManager.SendTalkingDataEvent(eventName, dic2);
							if(!TalkingDataManager.isTDPC)//talkingdata统计十连抽消费钻石//
								TDGAItem.OnPurchase("LotCardTen",1,useCrystal);
                        }
						
						//添加卡牌获得统计@zhangsai//
						if(lrj.list.Count == 1)
						{
							if(!UniteSkillInfo.cardUnlockTable.ContainsKey(lrj.list[0].dataId))
							{
								UniteSkillInfo.cardUnlockTable.Add(lrj.list[0].dataId,true);	
							}
						}
						else if(lrj.list.Count == 10)
						{
							for(int i =0;i<lrj.list.Count;i++)
							{
								if(!UniteSkillInfo.cardUnlockTable.ContainsKey(lrj.list[i].dataId))
								{
									UniteSkillInfo.cardUnlockTable.Add(lrj.list[i].dataId,true);	
								}
							}	
						}
					}
                    break;
                case 2:
                    if (PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_ZH)
                    {
                        PlayerInfo.getInstance().BattleOverBackType = 0;
                        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
                        {
                            GuideUI_GetCard.mInstance.hideAllStep();
                        }
                        else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard2))
                        {
                            GuideUI9_GetCard2.mInstance.hideAllStep();
                            GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_GetCard2);
                        }

                        show();
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
		
		if(effectTime>0)
		{
			effectTime-=Time.deltaTime;
			if(effectTime<=0)
			{
				TweenAlpha ta=white.GetComponent<TweenAlpha>();
				ta.from=0;
				ta.to=1f;
				ta.duration=1f;
				ta.tweenFactor=0;
				ta.enabled=true;
				
				canClickBtn=false;
			}
		}
	}
	
	void RotateLotIcon()
	{
		
		lotList[playRotateIconIndex].SetActive(true);
		//GameObjectUtil.playForwardUITweener(lotList[playRotateIconIndex].GetComponent<TweenAlpha>());
		GameObjectUtil.playForwardUITweener(lotList[playRotateIconIndex].GetComponent<TweenRotation>());
		playRotateIconIndex++;
	}

    void SetFreeTimes()
    {

        if (freeTimes < 0)
        {
            CancelInvoke("SetFreeTimes");

            freeLbael.gameObject.SetActive(false);
            freeLbael1.gameObject.SetActive(true);
            diamonds.gameObject.SetActive(false);
            freeLbael1.text = TextsData.getData(722).chinese;
        }
        else
        {
            freeLbael.gameObject.SetActive(true);
            freeLbael1.gameObject.SetActive(false);
            diamonds.gameObject.SetActive(true);
        }
        int h = freeTimes / 3600;
        int m = (freeTimes % 3600) / 60;
        int s = freeTimes % 60;
        freeLbael.text = (h > 9 ? h.ToString() : ("0" + h)) + TextsData.getData(718).chinese + (m > 9 ? m.ToString() : ("0" + m)) + TextsData.getData(719).chinese + (s > 9 ? s.ToString() : ("0" + s)) + TextsData.getData(720).chinese + TextsData.getData(721).chinese;
        freeTimes--;
    }
	private void showLot()
	{
        if (freeTimes > 0)
        {
            CancelInvoke("SetFreeTimes");
            InvokeRepeating("SetFreeTimes", 0, 1.0f);
            freeLbael.gameObject.SetActive(true);
            freeLbael1.gameObject.SetActive(false);
            diamonds.gameObject.SetActive(true);
        }
        else
        {
            freeLbael.gameObject.SetActive(false);
            freeLbael1.gameObject.SetActive(true);
            diamonds.gameObject.SetActive(false);
            freeLbael1.text = TextsData.getData(722).chinese;
        }
           

		playRotateIconIndex = 0;
		for(int i = 0;i < lotList.Count;++i)
		{
			lotList[i].SetActive(false);
			lotList[i].transform.localEulerAngles = initRotaCard;
			Invoke("RotateLotIcon",0.1f);
		}
		
		clearOneCardCtrl();
		lot.SetActive(true);
		result.SetActive(false);
		effectParent.SetActive(false);
		word.SetActive(false);
		newMark.SetActive(false);
		HeadUI.mInstance.show();
		HeadUI.mInstance.refreshPlayerInfo();
		friendNum.text=lrj.f.ToString();
		securitiesNum.text = lrj.d.ToString();
		for(int i=0;i<starParent.transform.childCount;i++)
		{
			if(i<lrj.n)
			{
				starParent.transform.GetChild(i).GetComponent<UISprite>().spriteName="light_on";
			}
			else
			{
				starParent.transform.GetChild(i).GetComponent<UISprite>().spriteName="light_off";
			}
		}
		
		crystalMark.SetActive(false);
		crystal10Mark.SetActive(false);
		diaosiMark.SetActive(false);
		friendMark.SetActive(false);
		//==唤神号角满足条件==//
		CardGetData cd=CardGetData.getData(4);
		if(lrj.d>=cd.cost)
		{
			diaosiMark.SetActive(true);
		}
		//==友情点满足条件==//
		cd=CardGetData.getData(1);
		if(lrj.f>cd.cost)
		{
			friendMark.SetActive(true);
		}
		
		if(GuideManager.getInstance().onlyShowLotCardPointer)
		{
			GuideManager.getInstance().onlyShowLotCardPointer = false;
			pointObj.SetActive(true);
		}
		else
		{
			pointObj.SetActive(false);
		}
	}

    private void showEffect()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGET_MODEL);
		lot.SetActive(false);
		result.SetActive(false);
		effectParent.SetActive(true);
		HeadUI.mInstance.hide();
		white.GetComponent<UISprite>().alpha=0;
		//播特效//
		if(fazhenEffect==null)
		{
			fazhenEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/haoyou_fazhen_02",1)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(fazhenEffect,effectParent);
			GameObjectUtil.setGameObjectLayer(fazhenEffect,effectParent.layer);
			fazhenEffect.transform.localRotation=Quaternion.Euler(new Vector3(30f,180f,0));
			fazhenEffect.transform.localPosition=new Vector3(0,-170f,80f);
		}
		
		if(nvws==null)
		{
			nvws=Instantiate(GameObjectUtil.LoadResourcesPrefabs("chouka",0)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(nvws,effectParent);
			GameObjectUtil.setGameObjectLayer(nvws,effectParent.layer);
			nvws.transform.localScale=new Vector3(180f,180f,180f);
			nvws.transform.localRotation=Quaternion.Euler(new Vector3(30f,180f,0));
			nvws.transform.localPosition=new Vector3(0,-170f,80f);
		}
		
		if(curClickLotId==2 || curClickLotId==3 || curClickLotId==4)
		{
			rotatEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_rotatcard_zi",1)) as GameObject;
		}
		else if(curClickLotId==1)
		{
			rotatEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_rotatcard_lan",1)) as GameObject;
		}
		GameObjectUtil.gameObjectAttachToParent(rotatEffect,effectParent);
		GameObjectUtil.setGameObjectLayer(rotatEffect,effectParent.layer);
		rotatEffect.transform.localPosition=new Vector3(0,-360f,0);
		rotatEffect.transform.localRotation=Quaternion.Euler(new Vector3(340f,0,0));
		rotatEffect.transform.localScale=new Vector3(100f,100f,100f);
		
		if(headEffect==null)
		{
			headEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/guang_chouka",1)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(headEffect,effectParent);
			GameObjectUtil.setGameObjectLayer(headEffect,effectParent.layer);
			headEffect.transform.localPosition=new Vector3(0,80f,0);
		}
		
		effectTime=2f;
		whiteMark=1;
	}
	
	void clearOneCardCtrl()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(card3DNode);
		GameObjectUtil.destroyGameObjectAllChildrens(oneCardEffect);
		card3DName.text = string.Empty;
		card3DStarIcon.spriteName = "";
		oneCardCtrl.SetActive(false);
		
	}
	
	private int showResult()
	{
		clearOneCardCtrl();
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_GETCARD);
		
		lot.SetActive(false);
		result.SetActive(true);
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
		{
			btnAgainObj.SetActive(false);

		}
		else
		{
			btnAgainObj.SetActive(true);
		}
		effectParent.SetActive(false);
		GameObjectUtil.destroyGameObjectAllChildrens(cardParent);
		
		//==按星级降序排序(取消排序)==//
		List<PackElement> pes=new List<PackElement>();
		for(int i=0;i<lrj.list.Count;i++)
		{
			PackElement pe=lrj.list[i];
			pes.Add(pe);
		}
		
		clearOneCardCtrl();
		for(int i=0;i<pes.Count;i++)
		{
			GameObject cell = null;
			PackElement pe=pes[i];
			if(pe == null)
				continue;
			CardData cd=CardData.getData(pe.dataId);
			if(cd == null)
				continue;
			if(pes.Count!=10)
			{
				oneCardCtrl.SetActive(true);

				GameObject cardModel = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel,STATE.PREFABS_TYPE_CARD))as GameObject;
				if(cardModel == null)
					continue;
				GameObjectUtil.gameObjectAttachToParent(cardModel,card3DNode,true);
				GameObjectUtil.setGameObjectLayer(cardModel,STATE.LAYER_ID_NGUI);
				cardModel.transform.localPosition = new Vector3(0,cd.modelposition,0);
				cardModel.transform.localScale = new Vector3(cd.modelsize,cd.modelsize,cd.modelsize);
				float rotaY = cd.modelrotation;
				cardModel.transform.localEulerAngles =new Vector3(0,rotaY,0);
				GameObjectUtil.hideCardEffect(cardModel);
				card3DName.text = "LV.1  " +cd.name;
				card3DStarIcon.spriteName = "card_side_s"+cd.star.ToString();
				cell = oneCardCtrl;
			}
			else
			{
				if(cardCell==null)
				{
					cardCell=GameObjectUtil.LoadResourcesPrefabs("UI-LotCards/CardItem",3);
				}
				cell=Instantiate(cardCell) as GameObject;
				cell.name=i+"";
				cell.GetComponent<SimpleCardInfo1>().setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Hero,pe);
				GameObjectUtil.gameObjectAttachToParent(cell,cardParent,true);
				if(pe.nw==1)
				{
					UISprite newS=NGUITools.AddChild<UISprite>(cell);
					newS.gameObject.name="new-mark";
					newS.atlas=lotAtals;
					newS.spriteName="card_title_new";
					newS.depth=29;
					newS.width=109;
					newS.height=37;
					newS.transform.localPosition=new Vector3(-34f,45f,0);
					newS.transform.localRotation=Quaternion.Euler(new Vector3(0,0,45f));
					newS.transform.localScale=new Vector3(0.8f,0.8f,0.8f);
				}
			}
			
			GameObject cardStarEffect=null;
			
			switch(cd.star)
			{
			case 3:
				break;
			case 4:
				cardStarEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_star_s04",1)) as GameObject;
				break;
			case 5:
				cardStarEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_star_s05",1)) as GameObject;
				break;
			case 6:
				cardStarEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_star_s06",1)) as GameObject;
				break;
			}
			if(cardStarEffect!=null)
			{
				if(pes.Count!=10)
				{
					GameObjectUtil.gameObjectAttachToParent(cardStarEffect,oneCardEffect);
				}
				else
				{
					GameObjectUtil.gameObjectAttachToParent(cardStarEffect,cell);
				}
				GameObjectUtil.setGameObjectLayer(cardStarEffect,cell.layer);
				if(pes.Count!=10)
				{
					cardStarEffect.transform.localPosition = new Vector3(cardStarEffect.transform.localPosition.x,cardStarEffect.transform.localPosition.y,-700);
				}
				cardStarEffect.transform.localScale=new Vector3(200f,200f,200f);
			}
			
			UIButtonMessage msg=cell.GetComponent<UIButtonMessage>();
			msg.target=_myTransform.gameObject;
			msg.functionName="onClickCard";
			msg.param=lrj.list.IndexOf(pe);
			
			//设置位置//
			if(pes.Count==10)
			{
				Vector3 pos=Vector3.zero;
				if(i<pes.Count/2)
				{
					pos=new Vector3((i-2)*140f,90f,0);
				}
				else
				{
					pos=new Vector3((i-2-pes.Count/2)*140f,-90f,0);
				}
				float during=0.3f;
				float delay=0.09f*i;
				
				cell.transform.localPosition=new Vector3(0,600f,0);
				cell.transform.localScale=new Vector3(2f,2f,2f);
				
				TweenPosition tp=cell.GetComponent<TweenPosition>();
				tp.from=new Vector3(0,600f,0);
				tp.to=pos;
				tp.duration=during;
				tp.delay=delay;
				tp.tweenFactor=0;
				tp.enabled=true;
				
				TweenScale ts=cell.GetComponent<TweenScale>();
				ts.from=new Vector3(2f,2f,2f);
				ts.to=new Vector3(0.8f,0.8f,0.8f);
				ts.duration=during;
				ts.delay=delay;
				ts.tweenFactor=0;
				ts.enabled=true;
			}
			else
			{
				haveWord=false;
				
				switch(cd.star)
				{
				case 3:
					haveWord=true;
					word.SetActive(true);
					word.GetComponent<TweenPosition>().enabled=false;
					word.GetComponent<UISprite>().spriteName="title_3";
					break;
				case 4:
					haveWord=true;
					word.SetActive(true);
					word.GetComponent<TweenPosition>().enabled=false;
					word.GetComponent<UISprite>().spriteName="title_4";
					break;
				case 5:
					haveWord=true;
					word.SetActive(true);
					word.GetComponent<TweenPosition>().enabled=false;
					word.GetComponent<UISprite>().spriteName="title_5";
					break;
				case 6:
					haveWord=true;
					word.SetActive(true);
					word.GetComponent<TweenPosition>().enabled=false;
					word.GetComponent<UISprite>().spriteName="title_6";
					break;
				}
				word.SetActive(false);
				if(pe.nw==1)
				{
					//cell.transform.FindChild("new-mark").gameObject.SetActive(false);
					curCell=cell;
				}
			}
		}
		
		if(pes.Count==1)
		{
			if(headEffect2==null)
			{
				headEffect2=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/guang_chouka",1)) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(headEffect2,result);
				GameObjectUtil.setGameObjectLayer(headEffect2,result.layer);
				headEffect2.transform.localPosition=new Vector3(0,80f,0);
			}
			label1Ctrl.SetActive(true);
			label10Ctrl.SetActive(false);
		}
		else if(pes.Count == 10)
		{
			label1Ctrl.SetActive(false);
			label10Ctrl.SetActive(true);
		}
		
		//向服务器请求判断是否有新解锁合体技//
		{
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
		}
		return pes.Count;
	}
	//==白背景变换完毕==//
	public void tweenOver()
	{
		if(whiteMark==1)
		{
			Destroy(rotatEffect);
			rotatEffect=null;
			int num=showResult();
			TweenAlpha ta=white.GetComponent<TweenAlpha>();
			ta.from=1f;
			ta.to=0;
			ta.duration=1f;
			ta.tweenFactor=0;
			ta.enabled=true;
			if(num==1)
			{
				if(haveWord)
				{
					//whiteMark=2;
					whiteMark=3;
				}
			}
			else
			{
				whiteMark=4;
			}
		}
		else if(whiteMark==2)
		{
			//==移动金属字==//
			word.SetActive(true);
			word.GetComponent<UISprite>().transform.localPosition=new Vector3(-480f,0,0);
			TweenPosition tp=word.GetComponent<TweenPosition>();
			tp.from=new Vector3(-480f,0,0);
			tp.to=new Vector3(480f,0,0);
			tp.tweenFactor=0;
			tp.duration=1f;
			tp.enabled=true;
		}
		else if(whiteMark==3)
		{
			moveOver();
		}
		else if(whiteMark==4)
		{
			canClickBtn=true;
		}
	}
	
	//==移动金属字完毕==//
	public void moveOver()
	{
		word.SetActive(false);
		
		if(curCell!=null)
		{
			newMark.SetActive(true);
			newMark.transform.localPosition=new Vector3(0f,225f,-720f);
			newMark.transform.localScale=new Vector3(5f,5f,1f);
			TweenScale ts=newMark.GetComponent<TweenScale>();
			ts.from=newMark.transform.localScale;
			ts.to=new Vector3(1.4f,1.4f,1f);
			ts.tweenFactor=0;
			ts.duration=0.3f;
			ts.enabled=true;
		}
		else
		{
			canClickBtn=true;
		}
	}
	
	public void waitForSetCanClickBtn()
	{
		canClickBtn=true;
	}
	
	//==移动new-mark完毕==//
	public void moveOver2()
	{
		curCell=null;
		Invoke("waitForSetCanClickBtn",0.2f);
		
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
		{
			UISceneDialogPanel.mInstance.showDialogID(3);
		}
	}
	
	public void onClickResultBtn(int param)
	{
		if(!canClickBtn)
		{
			return;
		}
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		switch(param)
		{
		case 1://再抽一次//
			onClickBtn(curClickLotId);
			newMark.SetActive(false);
			break;
		case 2://确定//
			clickOKBtn();
			break;
		}
	}
	
	public void clickOKBtn()
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		GameObjectUtil.destroyGameObjectAllChildrens(cardParent);
		showLot();
	}
	
	//==显示卡牌详细信息==//
	public void onClickCard(int param)
	{
		if(!canClickBtn)
		{
			return;
		}
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
		{
			GuideUI_GetCard.mInstance.hideAllStep();
		}
		
		PackElement pe=lrj.list[param];
		result.transform.FindChild("result-lot").gameObject.SetActive(false);
		PopCardDetailUI pop=result.transform.FindChild("result-cardDetail/pop-cardDetail").GetComponent<PopCardDetailUI>();
		pop.fromLot=true;
		pop.setContent(pe,null,null,null);
		if(headEffect2 != null)
		{
			headEffect2.SetActive(false);
		}
	}
	
	public void cardDetailBack()
	{
		result.transform.FindChild("result-lot").gameObject.SetActive(true);
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
		{
			GuideUI_GetCard.mInstance.showStep(2);
		}
		if(headEffect2 != null)
		{
			headEffect2.SetActive(true);
		}
	}
	
	public void onClickBtn(int param)
	{
		
		errorString=null;
		if(param>=1 && param<=4)
		{
			CardGetData cgd=CardGetData.getData(param);
			switch(cgd.type)
			{
			case 1://友情抽//
				Need_diamondNum.text = cgd.cost.ToString();
                result.transform.FindChild("result-lot/btn_again/Sprite_diamond").GetComponent<UISprite>().spriteName = "icon_02";
				if(lrj.f<cgd.cost)
				{
					errorString=cgd.getCostName()+TextsData.getData(51).chinese;
				}
				break;
			case 2://十连抽//
				Need_diamondNum.text = cgd.cost.ToString();
                result.transform.FindChild("result-lot/btn_again/Sprite_diamond").GetComponent<UISprite>().spriteName = "icon_01";
				if(param==2)
				{
					lotOneCard();
					return;
				}
				if(lrj.c<cgd.cost)
				{
					errorString=cgd.getCostName()+TextsData.getData(51).chinese;
				}
				break;
			case 3://屌丝抽卡//
				Need_diamondNum.text = cgd.cost.ToString();
                result.transform.FindChild("result-lot/btn_again/Sprite_diamond").GetComponent<UISprite>().spriteName = "maze_item";
				if(lrj.d<cgd.cost)
				{
					errorString=cgd.getCostName()+TextsData.getData(51).chinese;
				}
				break;
			}
			if(errorString!=null)
			{
//				ToastWindow.mInstance.showText(errorString);
				if(cgd.type != 2)		//type = 2表示水晶不足//
				{
					ToastWindow.mInstance.showText(errorString);
				}
				else 
				{
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, errorString);
				}
				return;
			}
//			//播放音效//
//			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGET_MODEL);
			curClickLotId=param;
			requestType=1;
			PlayerInfo.getInstance().sendRequest(new LotJson(param),this);
			
			string eventName = "LotCard";
			string lotType = "";
			Dictionary<string,object> dic = new Dictionary<string, object>();
			switch(param)
			{
			case 2:							//普通抽卡//
				lotType = "Normal";
				dic.Add("UseDiamond",cgd.cost.ToString());
				break;
			case 4:
				lotType = "Normal";
				break;
			case 3://十连抽//
				lotType = "TenLot";
				dic.Add("UseDiamond",cgd.cost.ToString());
				break;
			case 1://友情抽//
				lotType = "FriendLot";
				dic.Add("UseDiamond",cgd.cost.ToString());
				break;
			}
			dic.Add("lotType",lotType);
			dic.Add("PlayerId",PlayerPrefs.GetString("username"));
			TalkingDataManager.SendTalkingDataEvent(eventName,dic);
		}
	}
	
	public void lotOneCard()
	{
		CardGetData cgd=CardGetData.getData(2);
		if(lrj.c<cgd.cost&&freeTimes>0)
		{
			errorString=cgd.getCostName()+TextsData.getData(51).chinese;
		}
		if(errorString!=null)
		{
//			ToastWindow.mInstance.showText(errorString);
			if(cgd.type != 2)		//type = 2表示水晶不足//
			{
				ToastWindow.mInstance.showText(errorString);
			}
			else 
			{
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, errorString);
			}
			return;
		}
		curClickLotId=2;
		requestType=1;
		PlayerInfo.getInstance().sendRequest(new LotJson(2),this);
	}
	
	public void onClickBack()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		
		if(fazhenEffect!=null)
		{
			Destroy(fazhenEffect);
			fazhenEffect=null;
		}
		if(nvws!=null)
		{
			Destroy(nvws);
			{
				nvws=null;
			}
		}
		if(headEffect!=null)
		{
			Destroy(headEffect);
			{
				headEffect=null;
			}
		}
		if(headEffect2!=null)
		{
			Destroy(headEffect2);
			{
				headEffect2=null;
			}
		}
		lrj=null;
		lrjTemp=null;
//		MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.SetData(STATE.ENTER_MAINMENU_BACK);
		}
		hide();
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
				lrjTemp=JsonMapper.ToObject<LotResultJson>(json);
				errorCode = lrjTemp.errorCode;
                freeTimes = lrjTemp.t;
				receiveData=true;
				break;
            case 2:
                LotResultJson lrjlr = JsonMapper.ToObject<LotResultJson>(json);
				errorCode = lrjlr.errorCode;
                lrj = lrjlr;
                freeTimes = lrjlr.t;
                receiveData = true;
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
