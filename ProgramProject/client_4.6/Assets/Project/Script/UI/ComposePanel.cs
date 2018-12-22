using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ComposePackInfo
{
	public SimpleCardInfo1 info;
	public int targetID;
	public GameObject canComposeObj;
	public bool islock;
	public void clear()
	{
		targetID = -1;
		islock = true;
		info.clear();
		canComposeObj.SetActive(false);
	}
}

public class ComposePanel : MonoBehaviour ,ProcessResponse
{	
	public enum PageType
	{
		E_ComposePackPage = 0,
		E_ComposePage = 1,
		E_ComposeSuccessPage = 2,
	}
	
	public enum ComposeType
	{
		E_Null = 0,
		E_Hero = 1,
		E_Equip = 2,
		E_Item = 3,
		E_Skill = 4,
	}
	
//	public static ComposePanel mInstance;
	
	// ******compose ctrl******
	public GameObject composeCtrl;
	public List<SimpleCardInfo2> itemList;
	public List<UILabel> itemNumList;
	public UILabel composeCostLabel;
	
	// **********************************************************
	
	// ******compose pack ctrl******
	public GameObject composePackCtrl;
	public UIToggle heroCardToggle;
	public UIToggle equipToggle;
	public UIToggle itemToggle;
	public UIToggle skillToggle;
	// pack grid list
	public GameObject packGridListCtrl;
	public GameObject packCardPanelCtrl;
	public UIScrollBar packWindowScrollBar;
    //GameObject intensifySuccessEndEffectPrefab = null;
	
	public List<string> packItemInfoList = new List<string>(); // param string : id(formID) - num(0 can compose,1 can not compose)
	public List<ComposePackInfo> packItemCtrlList = new List<ComposePackInfo>();
	
	public List<PackElement> composeItemList = new List<PackElement>();
	// **********************************************************
	
	// ******compose result ctrl******
	public GameObject composeResultCtrl;
	public GameObject tipCtrl;
	public GameObject equipResultCtrl;
	// **********************************************************
	
	public GameObject cardShowCtrl;

	// 2D card
	public GameObject card2DCtrl;
	public SimpleCardInfo1 card2DInfo;
	public UILabel card2DDesLabel;
	
	public UIAtlas equipAtlas;
	public UIAtlas itemAtlas;
	
	// 3D card
	public GameObject card3DCtrl;
	public GameObject card3DNode;
	public UILabel card3DName;
	public UISprite card3DStarIcon;
	
	
	PageType mPageType;
	ComposeType mComposeType;
	ComposeType mTempComposeType;
	int targetID = -1;
	
	bool receiveData = false;
	int requestType = 0;
	
	// compose card prefab
	string packCardPrefabPath = "Prefabs/UI/ComposePanel/ComposePackCard";
	GameObject composePackCardPrefab;
	
	//bool composeIsSuccess = false;
	
	public ComposeResultJson crj;
	
	public GameObject effectCtrl;
	public List<GameObject> effectPosList;
	public List<Vector3>	effectStartPosList;
	
	int needItemCount = 0;
	public GameObject whiteFadeInEffect;
	public GameObject whiteFadeOutEffect;
	
	public UILabel dropGuideItemName;
	public GameObject dropGuide;
	public GameObject dropHead;
	public GameObject dropParent;
	public PopOtherDetailUI popOtherDetail;
	private GameObject dropGuideCell;
	private DropGuideResultJson drj;
	private int curGotoMissionId;
	private Transform _myTransform;
	
	bool isCanCompose_item = false;
	bool isCanCompose_money = false;
	
	int errorCode = 0;
	
	
	//卡组界面的json//
	CardGroupResultJson cardGroupRJ;
	//推图界面的json//
	MapResultJson mapRJ;
	
	public List<int> newItemIDList = new List<int>();

    public List<string> mark = new List<string>();

    public GameObject[] Tips;
	
	public int lastSelectComposeNeedItemId = -1;
	
	public NewUnitSkillResultJson nusrj;

    public UIScrollBar listValue;
	// Use this for initialization
	void Awake()
	{
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		_myTransform = transform;
		init();
//		close ();
	}

	void Start ()
	{
		
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
				if(crj.errorCode == 0)
				{
					newItemIDList.Clear();
					packItemInfoList = crj.cs;

                    mark = crj.mark;
					if(crj.mark != null)
					{
                       

                        GetTip();
					}
					
					mComposeType = mTempComposeType;
					showPage(PageType.E_ComposePackPage);
				}
				
			}break;
			case 2:
			{
				showPage(PageType.E_ComposePage);
			}break;
			case 3:
			{
				handleComposeJson();
			}break;
			case 4:
			{
				showDropGuide();
//				DropGuidePanel.mInstance.SetData(drj, 1);
			}break;
			case 5:
			{
				gotoMission();
			}break;
			case 6:
			{
				switch(errorCode)
				{
				case 0:
				{
//					CombinationInterManager.mInstance.SetData(0,1);
					UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
					CombinationInterManager combination=UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,"CombinationInterManager")as CombinationInterManager;
					combination.curCardGroup= cardGroupRJ.transformCardGroup();
					combination.SetData(1);
					
					hide();
					cardGroupRJ = null;
				}break;
				}
			}break;
			case 7:	
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
    public void GetTip()
    {
        for (int i = 0; mark != null && i < mark.Count; i++)
        {
            string str = mark[i];
            string[] ss = str.Split('-');
            //int id = StringUtil.getInt(ss[0]);
            int marks = StringUtil.getInt(ss[1]);

            if (marks == 1)
            {
                Tips[i].SetActive(true);

            }
            else
            {
                Tips[i].SetActive(false);
            }
       
        }
    }
	void handleComposeJson()
	{
		switch(crj.errorCode)
		{
		case 0:
		{
			if(crj.t == 0)
			{
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Compose))
				{
					GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Compose);
					
				}
				//composeIsSuccess = true;
				showSuccessEffect();
				//播放音效//
				MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_HC_SURE);
			}
			else if(crj.t == -1)
			{
				//composeIsSuccess = false;
				//播放音效//
				MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_FAIL);
			}
			showPage(PageType.E_ComposePage);
			
			HeadUI.mInstance.requestPlayerInfo();
			
			//添加卡牌获得统计@zhangsai//
			if(!UniteSkillInfo.cardUnlockTable.ContainsKey(targetID))
				UniteSkillInfo.cardUnlockTable.Add(targetID,true);
			//向服务器请求判断是否有新解锁合体技//
			if(PlayerInfo.getInstance().player.level>3)
			{
				requestType = 7;
				//PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
			}
		}break;
		case 19:
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_FAIL);
			//所需Gold不足//
//			ToastWindow.mInstance.showText(TextsData.getData(336).chinese);
			int buyType = 1;
			int jsonType = 1;
			int costType = 1;
			ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_COMPOSE);
		}break;
		case 20:
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_FAIL);
			//所需合成材料不足//
			ToastWindow.mInstance.showText(TextsData.getData(337).chinese);
		}break;
		case 21:
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_FAIL);
			//合成物品不存在//
			ToastWindow.mInstance.showText(TextsData.getData(54).chinese);
		}break;
		case 22:
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_FAIL);
			//合成物品未解锁//
			ToastWindow.mInstance.showText(TextsData.getData(55).chinese);
		}break;
		case 53:			//背包空间不足//
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_FAIL);
			
//			ToastWindow.mInstance.showText(TextsData.getData(324).chinese);
			string str = TextsData.getData(324).chinese;
			UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_PACKAGE, str);
		}break;
		}
	}
	
	public void init ()
	{
//		base.init();
		effectStartPosList = new List<Vector3>();
		for(int i = 0; i < effectPosList.Count;++i)
		{
			effectStartPosList.Add(effectPosList[i].transform.localPosition);
		}
		
		//初始化图集//
		equipAtlas = LoadAtlasOrFont.LoadAtlasByName("EquipIcon");
		itemAtlas = LoadAtlasOrFont.LoadAtlasByName("ItemIcon");
	}
	
	public void show()
	{
		if(composePackCardPrefab == null)
		{
			composePackCardPrefab = Resources.Load(packCardPrefabPath) as GameObject;
		}
//		base.show();
		showPage(mPageType);
		dropGuide.SetActive(false);
	}
	
	public void hide()
	{
//		base.hide();
		_myTransform.gameObject.SetActive(false);
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE);
	}
	
	private void gc()
	{
		lastSelectComposeNeedItemId = -1;
		
		if(packItemInfoList != null)
		{
			packItemInfoList.Clear();
		}
		
		if(packItemCtrlList != null)
		{
			packItemCtrlList.Clear();
		}
		
		if(composeItemList !=null)
		{
			composeItemList.Clear();
		}
		
		if(newItemIDList != null)
		{
			newItemIDList.Clear();
		}
		
		composePackCardPrefab=null;
		dropGuideCell=null;
		equipAtlas = null;
		itemAtlas = null;
		GameObjectUtil.destroyGameObjectAllChildrens(dropParent);
		GameObjectUtil.destroyGameObjectAllChildrens(packGridListCtrl);
		drj=null;
		cardGroupRJ = null;
		Resources.UnloadUnusedAssets();
	}
	
	public void onClickComposePackBtn(int param)
	{
        listValue.value = 0;
		switch(param)
		{
		case 1:
		{
			// back btn
			
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			
			hide();
//			MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
			UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
			GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
			if(obj!=null)
			{
				MainMenuManager main = obj.GetComponent<MainMenuManager>();
				main.SetData(STATE.ENTER_MAINMENU_BACK);
			}

		}break;
		case 2://==equip==//
		{
			
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			
			// equip card btn
			if(mComposeType == ComposeType.E_Equip)
				return;
			mTempComposeType = ComposeType.E_Equip;
			requestType = 1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)ComposeType.E_Equip),this);
		}break;
		case 3://==hero==//
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			// hero card btn
			if(mComposeType == ComposeType.E_Hero)
				return;
			mTempComposeType = ComposeType.E_Hero;
			requestType = 1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)ComposeType.E_Hero),this);
		}break;
		case 4://==item==//
		{
			
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			
			// item card btn
			if(mComposeType == ComposeType.E_Item)
				return;
			mTempComposeType = ComposeType.E_Item;
			requestType = 1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)ComposeType.E_Item),this);
		}break;
		case 5://==skill==//
			if(mComposeType == ComposeType.E_Skill)
				return;
			mTempComposeType = ComposeType.E_Skill;
			requestType = 1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)ComposeType.E_Skill),this);
			break;
		}
	}
	
	public void onSelectPackCardItem(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		
		if(param == -1)
			return;
		targetID = param;
		bool islock = true;
		for(int i = 0; i < packItemCtrlList.Count;++i)
		{
			if(packItemCtrlList[i].targetID == targetID)
			{
				islock = packItemCtrlList[i].islock;
				break;
			}
		}
		if(islock)
		{
			ComposeData composeData = ComposeData.getData(targetID,(int)mComposeType);
			string lockStr = "";
			if(composeData.style == 1)
			{
				lockStr = TextsData.getData(338).chinese.Replace("num",composeData.unlockmission.ToString());
				ToastWindow.mInstance.showText(lockStr);
			}
			else
			{
				// to do
			}
			return;
		}
		requestType = 2;
		UIJson uiJson = new UIJson();
		uiJson.UIJsonForCompose(STATE.UI_Compose2,targetID,(int)mComposeType);
		PlayerInfo.getInstance().sendRequest(uiJson,this);
	}
	
	public void onClickComposeCtrlBtn(int param)
	{
		switch(param)
		{
		case 1:
		{
            for (int i = 0; i < itemList.Count; ++i)
            {
                itemList[i].gameObject.SetActive(true);
                
            }
            cardShowCtrl.SetActive(true);
			// back btn
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			requestType = 1;
			mTempComposeType = mComposeType;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)mComposeType),this);
		}break;
		case 2:
		{
			doCompose();
		}break;
		}
	}
	public void doCompose()
	{
		
		if(!isCanCompose_item)
		{
			//所需合成材料不足//
			ToastWindow.mInstance.showText(TextsData.getData(337).chinese);
			return;
		}
		
		ComposeData composeData = ComposeData.getData(targetID,(int)mComposeType);
		if(PlayerInfo.getInstance().player.gold >=composeData.cost)
		{
			isCanCompose_money = true;
		}
		else
		{
			isCanCompose_money = false;
		}
		
		if(!isCanCompose_money)
		{
			//所需Gold不足//
//			ToastWindow.mInstance.showText(TextsData.getData(336).chinese);
			int buyType = 1;
			int jsonType = 1;
			int costType = 1;
			ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_COMPOSE);
			return;
		}
		
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		// compose btn
		requestType = 3;
		PlayerInfo.getInstance().sendRequest(new ComposeJson((int)mComposeType,targetID),this);
	}
	
	public void onClickTargetCard()
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		
		// click compose ctrl target card
		if(mPageType != PageType.E_ComposePage)
		{
			return;
		}
		requestType = 1;
		mTempComposeType = mComposeType;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)mComposeType),this);




	}
	
	public void onClickComposeResultWnd()
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		showPage(PageType.E_ComposePage);

        ShowMate();
	}
	
	public void drawComposePackList()
	{
		if(packItemCtrlList.Count < 10)
		{
			for(int i = 0 ; i < 10;++i)
			{
				GameObject obj = GameObject.Instantiate(composePackCardPrefab) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(obj,packGridListCtrl,true);
				ComposePackInfo info = new ComposePackInfo();
				info.info = obj.GetComponent<SimpleCardInfo1>();
				info.canComposeObj = GameObjectUtil.findGameObjectByName(obj,"CanComposeMark");
				info.info.bm.target = _myTransform.gameObject;
				info.info.bm.functionName = "onSelectPackCardItem";
				info.clear();
				packItemCtrlList.Add(info);
			}	
		}
		
		int hasItemLineCount = (packItemInfoList.Count-1)/5+1;
		int totalGridLineCout= (packItemCtrlList.Count-1)/5+1;

		if(hasItemLineCount <= totalGridLineCout)
		{
			for(int i = 0; i < packItemCtrlList.Count;++i)
			{
				ComposePackInfo info = packItemCtrlList[i];
				int formID = -1;
				bool canCompose = false;
				if(i < packItemInfoList.Count)
				{
					string cardInfo = packItemInfoList[i];
					getCardInfo(cardInfo,ref formID,ref canCompose,ref info.islock);
				}
				setPackCardDisplay(info,formID,canCompose,info.islock);
				info.info.bm.param = formID;
				info.targetID = formID;
				if(formID == -1 )
				{
					if( i > (hasItemLineCount*5 -1) && i > 9)
					{
						info.info.gameObject.SetActive(false);
					}
					else
					{
						info.info.gameObject.SetActive(true);
					}
				}
				else
				{
					info.info.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			for(int i = 0; i < packItemCtrlList.Count;++i)
			{
				ComposePackInfo info = packItemCtrlList[i];
				int formID = -1;
				bool canCompose = false;
				if(i < packItemInfoList.Count)
				{
					string cardInfo = packItemInfoList[i];
					getCardInfo(cardInfo,ref formID,ref canCompose,ref info.islock);
				}
				setPackCardDisplay(info,formID,canCompose,info.islock);
				info.info.bm.param = formID;
				info.targetID = formID;
			}
			for(int i = totalGridLineCout*5; i< hasItemLineCount*5;++i)
			{
				GameObject obj = GameObject.Instantiate(composePackCardPrefab) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(obj,packGridListCtrl,true);
				
				ComposePackInfo info = new ComposePackInfo();
				info.info = obj.GetComponent<SimpleCardInfo1>();
				info.canComposeObj = GameObjectUtil.findGameObjectByName(obj,"CanComposeMark");
				info.info.bm.target = _myTransform.gameObject;
				info.info.bm.functionName = "onSelectPackCardItem";
				int formID = -1;
				bool canCompose = false;
				if(i < packItemInfoList.Count)
				{
					string cardInfo = packItemInfoList[i];
					getCardInfo(cardInfo,ref formID,ref canCompose,ref info.islock);
				}
				setPackCardDisplay(info,formID,canCompose,info.islock);
				info.info.bm.param = formID;
				info.targetID = formID;
				packItemCtrlList.Add(info);
			}
		}
	}
	
	public void getCardInfo(string cardInfo,ref int formID,ref bool canCompose,ref bool islock)
	{
		if(cardInfo == string.Empty)
			return;
		string[] ss = cardInfo.Split('-');
		formID = StringUtil.getInt(ss[0]);
		int num = StringUtil.getInt(ss[1]);
		int num2 = StringUtil.getInt(ss[2]);
		if(num == 1)
		{
			canCompose = true;
		}
		else if(num == 0)
		{
			canCompose = false;
		}
		if(num2 == 1)
		{
			islock = false;
		}
		else
		{
			islock = true;
		}
	}

    public bool isNewItem(int formID)
    {
        for (int i = 0; i < newItemIDList.Count; ++i)
        {
            if (formID == newItemIDList[i])
            {
                return true;
            }
        }
        return false;
    }
	
	public void setPackCardDisplay(ComposePackInfo info,int formID,bool canCompose,bool islock)
	{
		if(formID == -1)
		{
			info.clear();
			return;
		}
		else
		{
            //if(isNewItem(formID))
            //{
            //    info.info.showTopNewObj();
            //}
            //else
            //{
            //    info.info.hideTopNewObj();
            //}
			
			if(canCompose)
			{
				info.canComposeObj.SetActive(true);	
			}
			else
			{
				info.canComposeObj.SetActive(false);
			}
			
			if(islock)
			{
				ComposeData composeData = ComposeData.getData(formID,(int)mComposeType);
				string lockStr = "";
				if(composeData.style == 1)
				{
					lockStr = TextsData.getData(338).chinese.Replace("num",composeData.unlockmission.ToString());
					info.info.showLockText(lockStr);
				}
				else
				{
					info.info.clearLockText();
				}
			}
		
			switch(mComposeType)
			{
			case ComposeType.E_Hero:
			{
				info.info.setSimpleCardInfo(formID,GameHelper.E_CardType.E_Hero);
			}break;
			case ComposeType.E_Equip:
			{
				info.info.setSimpleCardInfo(formID,GameHelper.E_CardType.E_Equip);
			}break;
			case ComposeType.E_Item:
			{
				info.info.setSimpleCardInfo(formID,GameHelper.E_CardType.E_Item);
			}break;
			case ComposeType.E_Skill:
				info.info.setSimpleCardInfo(formID,GameHelper.E_CardType.E_Skill);	
			break;
			}
		}
	}

	public void showPage(PageType pt)
	{
		mPageType = pt;
		GameObjectUtil.destroyGameObjectAllChildrens(card3DNode);
		switch(mPageType)
		{
		case PageType.E_ComposePackPage:
		{
			cardShowCtrl.SetActive(false);
			effectCtrl.SetActive(false);
			composePackCtrl.SetActive(true);
			composeCtrl.SetActive(false);
			composeResultCtrl.SetActive(false);
			showPackPageContents();
		}break;
		case PageType.E_ComposePage:
		{
			cardShowCtrl.SetActive(true);
			ComposeData composeData = ComposeData.getData(targetID,(int)mComposeType);
			if(composeData == null)
				return;
			composePackCtrl.SetActive(false);
			composeCtrl.SetActive(true);
			composeResultCtrl.SetActive(false);
            showComposePageContents();
		}break;
		case PageType.E_ComposeSuccessPage:
		{
			cardShowCtrl.SetActive(true);
			composePackCtrl.SetActive(false);
			composeCtrl.SetActive(false);
			composeResultCtrl.SetActive(true);
			if(mComposeType == ComposeType.E_Equip)
			{
				tipCtrl.SetActive(false);
				equipResultCtrl.SetActive(true);
			}
			else
			{
				tipCtrl.SetActive(true);
				equipResultCtrl.SetActive(false);
			}
			showComposeResultContents();
		}break;
		}
	}
	
	public void showPackPageCtrls()
	{
		switch(mComposeType)
		{
		case ComposeType.E_Hero:
		{
			heroCardToggle.value = true;
			equipToggle.value = false;
			itemToggle.value = false;
			skillToggle.value = false;
		}break;
		case ComposeType.E_Equip:
		{
			heroCardToggle.value = false;
			equipToggle.value = true;
			itemToggle.value = false;
			skillToggle.value = false;
		}break;
		case ComposeType.E_Item:
		{
			heroCardToggle.value = false;
			equipToggle.value = false;
			itemToggle.value = true;
			skillToggle.value = false;
		}break;
		case ComposeType.E_Skill:
		{
			heroCardToggle.value = false;
			equipToggle.value = false;
			itemToggle.value = false;
			skillToggle.value = true;
		}break;
		}
	}
	
	public void showPackPageContents()
	{
		showPackPageCtrls();
		for(int i = 0; i < packItemCtrlList.Count;++i)
		{
			ComposePackInfo info = packItemCtrlList[i];
			info.clear();
		}
		packGridListCtrl.GetComponent<UIGrid2>().repositionNow = true;
		packWindowScrollBar.value = 0;
		packCardPanelCtrl.transform.localPosition = Vector3.zero;
		packCardPanelCtrl.GetComponent<UIPanel>().clipRange = new Vector4(0,0,720,390);
		drawComposePackList();
        showCardShowCtrls();
	}
	
	public void showCardShowCtrls()
	{
		switch(mComposeType)
		{
		case ComposeType.E_Hero:
		{
			card2DCtrl.SetActive(false);
			card3DCtrl.SetActive(true);
		}break;
		case ComposeType.E_Equip:
		case ComposeType.E_Item:
		{
			card2DCtrl.SetActive(true);
			card3DCtrl.SetActive(false);
		}break;
        case ComposeType.E_Skill:
        {
            card2DCtrl.SetActive(true);
            card3DCtrl.SetActive(false);  
        }break;
		}
	}
	
	public void showComposePageContents()
	{
		showCardShowCtrls();
		
		for(int i = 0; i < itemList.Count;++i)
		{
			itemList[i].clear();
			UIButtonMessage msg=itemList[i].gameObject.GetComponent<UIButtonMessage>();
			msg.target=null;
		}
		for(int i = 0; i < itemNumList.Count;++i)
		{
			itemNumList[i].text = string.Empty;
		}
		isCanCompose_item = true;
		isCanCompose_money = true;
		ComposeData composeData = ComposeData.getData(targetID,(int)mComposeType);
		needItemCount = composeData.material_num.Count;
		if(composeData == null)
		{
			return;
		}
		
		composeCostLabel.text = composeData.cost.ToString();
		if(PlayerInfo.getInstance().player.gold >=composeData.cost)
		{
			isCanCompose_money = true;
		}
		else
		{
			isCanCompose_money = false;
		}
		
		for(int i = 0; i < needItemCount;++i)
		{
			string materialStr = composeData.material_num[i];
			string[] ss = materialStr.Split('-');
			int formID = StringUtil.getInt(ss[0]);
			int count = StringUtil.getInt(ss[1]);
            itemList[i].setSimpleCardInfo(formID, GameHelper.E_CardType.E_Item);
			PackElement tempPE = null;
			for(int j = 0; j < composeItemList.Count;++j)
			{
				if(composeItemList[j].dataId == formID)
				{
					tempPE = composeItemList[j];
					break;
				}
			}
			if(tempPE != null)
			{
				if(tempPE.pile < count)
				{
					isCanCompose_item = false;
					itemNumList[i].text = "[ff0000]"+tempPE.pile +"/" + count.ToString() + "[-]";
				}
				else
				{
					itemNumList[i].text = "[00ff00]"+tempPE.pile +"/" + count.ToString() + "[-]";
				}
			}
			else
			{
				isCanCompose_item = false;
				itemNumList[i].text = "[ff0000]0/"+ count.ToString() + "[-]";
			}
			//==点击弹出掉落指引==//
			UIButtonMessage msg=itemList[i].gameObject.GetComponent<UIButtonMessage>();
			msg.target=_myTransform.gameObject;
			if(mTempComposeType == ComposeType.E_Equip || mTempComposeType == ComposeType.E_Null)
			{
				msg.functionName="onClickDropGuide";
			}
			else if(mTempComposeType == ComposeType.E_Hero)
			{
				msg.functionName="onClickHero";
			}
            else if (mTempComposeType == ComposeType.E_Skill)
            {
                msg.functionName = "onClickSkill";
            }
			msg.param=formID;
			//itemList[i].gameObject.transform.FindChild("select").gameObject.SetActive(true);
		}

		showCardShowContents();
	}
	
	public void showComposeResultContents()
	{
		showCardShowCtrls();
		showCardShowContents();
	}
	
	public void showCardShowContents()
	{
		switch(mComposeType)
		{
		case ComposeType.E_Hero:
		{
			CardData cd = CardData.getData(targetID);
			if(cd == null)
				return;
			GameObject cardModel = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel,STATE.PREFABS_TYPE_CARD))as GameObject;
			if(cardModel == null)
				return;
			GameObjectUtil.gameObjectAttachToParent(cardModel,card3DNode,true);
			GameObjectUtil.setGameObjectLayer(cardModel,STATE.LAYER_ID_NGUI);
			cardModel.transform.localPosition = new Vector3(0,cd.modelposition,0);
			cardModel.transform.localScale = new Vector3(cd.modelsize,cd.modelsize,cd.modelsize);
			float rotaY = cd.modelrotation;
			cardModel.transform.localEulerAngles =new Vector3(0,rotaY,0);
			GameObjectUtil.hideCardEffect(cardModel);
			card3DName.text = "LV.1  " +cd.name;
			card3DStarIcon.spriteName = "card_side_s"+cd.star.ToString();
			
		}break;
			
		case ComposeType.E_Equip:
		{
			EquipData ed = EquipData.getData(targetID);
			if(ed == null)
				return;
			card2DInfo.setSimpleCardInfo(targetID,GameHelper.E_CardType.E_Equip);
			card2DDesLabel.text = GameHelper.getEquipAttrDescText(targetID);
		}break;
		case ComposeType.E_Item:
		{
			ItemsData itemData = ItemsData.getData(targetID);
			if(itemData == null)
				return;
			card2DInfo.setSimpleCardInfo(targetID,GameHelper.E_CardType.E_Item);
			card2DDesLabel.text = itemData.discription;
		}break;

        case ComposeType.E_Skill:
            {
                SkillData itemData = SkillData.getData(targetID);
                if (itemData == null)
                    return;
                card2DInfo.setSimpleCardInfo(targetID, GameHelper.E_CardType.E_Skill); 
              card2DDesLabel.text = itemData.description + GetNum(targetID);

            } break;
		}
	}


    public int GetNum(int id)
    {
        SkillData ed = SkillData.getData(id);


        int c = SkillPropertyData.getProperty(ed.type,ed.level,ed.star);

        return c;
    }
	public void setComposePage(PageType pt)
	{
		mPageType = pt;
	}
	
	public void setComposeType(ComposeType ct)
	{
		mComposeType = ct;
	}
	
	public void showSuccessEffect()
	{
        //string path = "Prefabs/Effects/UIEffect/flyingspark";
        //GameObject loadPrefab = Resources.Load(path) as GameObject;
		
        //effectCtrl.SetActive(true);
        //whiteFadeInEffect.SetActive(false);
        //whiteFadeOutEffect.SetActive(false);
        //for(int i = 0; i < needItemCount;++i)
        //{
        //    GameObject effectPosObj = effectPosList[i];
        //    GameObject effectObj = GameObject.Instantiate(loadPrefab) as GameObject;
        //    GameObjectUtil.gameObjectAttachToParent(effectObj,effectPosObj);
        //    effectPosObj.transform.localPosition = effectStartPosList[i];
        //    effectPosObj.GetComponent<TweenPosition>().enabled = true;
        //    effectPosObj.GetComponent<TweenPosition>().tweenFactor = 0;
        //    effectPosObj.GetComponent<TweenPosition>().PlayForward();
        //}
        string path = "Prefabs/Effects/UIEffect/compose01";
        GameObject loadPrefab =  Instantiate( Resources.Load(path)) as GameObject;

        GameObjectUtil.gameObjectAttachToParent(loadPrefab, UISceneEffectNodeManager.mInstance.intensifySuccessFlyEffectNode);
        GameObjectUtil.setGameObjectLayer(loadPrefab, STATE.LAYER_ID_UIEFFECT);
          for(int i = 0; i < itemList.Count;++i)
          {
              itemList[i].gameObject.SetActive(false);
          }

          Destroy(loadPrefab, 2.5f);
          InvokeRepeating("GetAc",0, 0);
        
	}

    void ShowMate()
    {
        for (int i = 0; i < itemList.Count; ++i)
        {
            itemList[i].gameObject.SetActive(true);
        }
    }
    void GetAct()
    {
        CancelInvoke("GetAct");
        CancelInvoke("GetAc");
        cardShowCtrl.SetActive(true);
        composeResultCtrl.SetActive(true);

        if (mComposeType == ComposeType.E_Equip)
        {
            tipCtrl.SetActive(false);
            equipResultCtrl.SetActive(true);
        }
        else
        {
            tipCtrl.SetActive(true);
            equipResultCtrl.SetActive(false);
        }
    }
    void GetAc()
    {
        cardShowCtrl.SetActive(false);
        if (!cardShowCtrl.activeSelf)
        {
            composePackCtrl.SetActive(false);
            composeCtrl.SetActive(false);
          
            Invoke("GetAct", 0.85f);
            
        }
      
    }
	public void effectFinish()
	{
		for(int i =0;i < effectPosList.Count;++i)
		{
			GameObjectUtil.destroyGameObjectAllChildrens(effectPosList[i]);
		}
		showWhiteFadeInEffect();
	}
	
	public void showWhiteFadeInEffect()
	{
		whiteFadeInEffect.SetActive(true);
		whiteFadeInEffect.GetComponent<UISprite>().alpha = 0;
		whiteFadeInEffect.GetComponent<TweenAlpha>().enabled = true;
		whiteFadeInEffect.GetComponent<TweenAlpha>().tweenFactor = 0;
		whiteFadeInEffect.GetComponent<TweenAlpha>().PlayForward();
		
	}
	
	public void showWhiteFadeInEnd()
	{
		whiteFadeInEffect.GetComponent<UISprite>().alpha = 0;
		whiteFadeInEffect.GetComponent<TweenAlpha>().tweenFactor = 0;
		whiteFadeInEffect.SetActive(false);
		
				
		showWhiteFadeOutEffect();
	}
	
	public void showWhiteFadeOutEffect()
	{
		whiteFadeOutEffect.SetActive(true);
		whiteFadeOutEffect.GetComponent<UISprite>().alpha = 1;
		whiteFadeOutEffect.GetComponent<TweenAlpha>().enabled = true;
		whiteFadeOutEffect.GetComponent<TweenAlpha>().tweenFactor = 0;
		whiteFadeOutEffect.GetComponent<TweenAlpha>().PlayForward();
		
		showPage(PageType.E_ComposeSuccessPage);
	}
	
	public void showWhiteFadeOutEnd()
	{
		whiteFadeOutEffect.GetComponent<UISprite>().alpha = 1;
		whiteFadeOutEffect.GetComponent<TweenAlpha>().tweenFactor = 0;
		whiteFadeOutEffect.SetActive(false);
		
	
		effectCtrl.SetActive(false);
	}
	
	//==掉落指引==//
	public void onClickDropGuide(int itemId)
	{
		lastSelectComposeNeedItemId = itemId;
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		requestDropGuide();
		
	}
	
	public void requestDropGuide()
	{
		requestType=4;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_DROP_GUIDE,lastSelectComposeNeedItemId),this);
	}
    public void onClickHero(int heroID)
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        lastSelectComposeNeedItemId = heroID;
        requestDropGuide();
    }
    public void onClickSkill(int itemId)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (itemId == 0)
            return;
        ItemsData itemD = ItemsData.getData(itemId);
        int count = 0;
        for (int i = 0; i < composeItemList.Count; ++i)
        {
            if (composeItemList[i].dataId == itemId)
            {
                count = composeItemList[i].pile;
            }
        }
        popOtherDetail.setContentNew(5, GameHelper.E_CardType.E_Item, itemId, itemD.name, 1 + "", itemD.star.ToString(), count.ToString(), itemD.discription, itemD.sell.ToString());
	}
    public void GetIn(int itemId)
    {
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (itemId == 0)
            return;
        ItemsData itemD = ItemsData.getData(itemId);
        int count = 0;
        for (int i = 0; i < composeItemList.Count; ++i)
        {
            if (composeItemList[i].dataId == itemId)
            {
                count = composeItemList[i].pile;
            }
        }
        popOtherDetail.setContentNew(5, GameHelper.E_CardType.E_Item, itemId, itemD.name, 1 + "", itemD.star.ToString(), count.ToString(), itemD.discription, itemD.sell.ToString());
    }
	public void showDropGuide()
	{
        ItemsData itemData = ItemsData.getData(drj.id);
        if (drj.ds.Count == 0)
        {
            GetIn(drj.id);
            return;
        }
		dropGuideItemName.text = itemData.name;
		dropGuide.SetActive(true);
		dropGuide.transform.FindChild("scroll-bar").GetComponent<UIScrollBar>().value=0;
              
		GameObjectUtil.destroyGameObjectAllChildrens(dropParent);
		
		dropHead.transform.FindChild("ItemInfo").GetComponent<SimpleCardInfo2>().clear();
        dropHead.transform.FindChild("ItemInfo").GetComponent<SimpleCardInfo2>().setSimpleCardInfo(drj.id,GameHelper.E_CardType.E_Item);
		for(int i=0;drj.ds!=null && i<drj.ds.Count;i++)
		{
			if(dropGuideCell==null)
			{

				dropGuideCell=GameObjectUtil.LoadResourcesPrefabs("ComposePanel/DropCell",3);
			}
			GameObject cell=Instantiate(dropGuideCell) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(cell,dropParent);
			GameObjectUtil.setGameObjectLayer(cell,dropParent.layer);
			//==关卡id-是否解锁-已完成次数==//
			string s=drj.ds[i];
			string[] ss=s.Split('-');
			int missionId=StringUtil.getInt(ss[0]);
			int unlock=StringUtil.getInt(ss[1]);
			int times=StringUtil.getInt(ss[2]);
			
			MissionData md=MissionData.getData(missionId);
			
			UIButtonMessage msg=cell.GetComponent<UIButtonMessage>();
			if(unlock==1)
			{
				msg.target=_myTransform.gameObject;
				msg.functionName="onClickDropGuideCell";
				msg.param=md.id;
				if(md.missiontype==1)
				{
					cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName="map_base_01";
				}
				else
				{
					cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName="map_base_03";
				}
				cell.transform.FindChild("times").GetComponent<UILabel>().text="("+(md.times - times)+"/"+md.times+")";
			}
			else
			{
				msg.target=null;
				if(md.missiontype==1)
				{
					cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName="map_base_02";
				}
				else
				{
					cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName="map_base_04";
				}
				cell.transform.FindChild("times").GetComponent<UILabel>().text=TextsData.getData(320).chinese;
			}
			cell.transform.FindChild("zoneName").GetComponent<UILabel>().text=md.zonename;
			cell.transform.FindChild("missionName").GetComponent<UILabel>().text=md.name;
		}
        if (dropParent.transform.childCount <= 3)
        {
            dropGuide.transform.FindChild("panel").GetComponent<UIDraggablePanel>().disableDragIfFits = true;
        }
        else
        {
            dropGuide.transform.FindChild("panel").GetComponent<UIDraggablePanel>().disableDragIfFits = false;
            
        }
		dropParent.GetComponent<UIGrid>().repositionNow=true;
	}
	
	public void onClickDropGuideCell(int param)
	{
		curGotoMissionId=param;
		requestType=5;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
	}
	
	private void gotoMission()
	{
//		base.hide();
		//隐藏合成界面但不删除数据//
		UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE);
		
		//打开推图界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI2")as MissionUI2;
//		MissionUI2.mInstance.showFromCompose(curGotoMissionId);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI")as MissionUI;
		mission.mrj=mapRJ;
		mission2.showFromCompose(curGotoMissionId);
		
		mission.showFromCompose(curGotoMissionId);
//		MissionUI.mInstance.showFromCompose(curGotoMissionId);
	}
	
	public void closeDropGuide()
	{
		dropGuide.SetActive(false);
	}
	
	public void baseShow()
	{
//		base.show();
		UISceneStateControl.mInstace.ShowObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE);
		requestDropGuide();
	}
	
	public void onClickGoTeamBtn()
	{
		requestType = 6;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0),this);
	}
	
	/**处理返回信息**/
	public void receiveResponse(string json)
	{
		Debug.Log(json);
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
				crj = JsonMapper.ToObject<ComposeResultJson>(json);
				errorCode = crj.errorCode;
                mark = crj.mark;
				receiveData = true;
			}break;
			case 2:
			{
				crj = JsonMapper.ToObject<ComposeResultJson>(json);
				errorCode = crj.errorCode;
				if(errorCode == 0)
				{
                    mark = crj.mark;
					composeItemList = crj.pes;
				}
				receiveData = true;
			}break;
			case 3:
			{
				crj = JsonMapper.ToObject<ComposeResultJson>(json);
				errorCode = crj.errorCode;
                mark = crj.mark;
                composeItemList = crj.pes;
				receiveData = true;
			}break;
			case 4:
			{
				DropGuideResultJson drj=JsonMapper.ToObject<DropGuideResultJson>(json);
				errorCode = drj.errorCode;
				this.drj=drj;
				receiveData = true;
			}break;
			case 5:
			{
				MapResultJson mrj=JsonMapper.ToObject<MapResultJson>(json);
				errorCode = mrj.errorCode;
//				MissionUI.mInstance.mrj=mrj;
				mapRJ = mrj;
				
				receiveData = true;
			}break;
			case 6:
			{
				CardGroupResultJson cgrj=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj.errorCode;
				if(errorCode == 0)
				{
//					CombinationInterManager.mInstance.curCardGroup=cgrj.transformCardGroup();
//					CombinationInterManager.mInstance.curPage=0;
//					CombinationInterManager.mInstance.isUsed=true;
					cardGroupRJ = cgrj;
				}
				receiveData=true;
			}break;
			case 7:
				nusrj = JsonMapper.ToObject<NewUnitSkillResultJson>(json);
				errorCode = nusrj.errorCode;
				receiveData = true;
				break;
			}
			
		}
	}
}
