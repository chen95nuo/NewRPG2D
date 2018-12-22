using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ListCardInfo
{
    public PackElement data;
    public SimpleCardInfo1 sInfo;
    public GameObject selectObj;
    public GameObject usedObj;

    public UIButton btn;
    public UISprite icon;

    public UILabel label;
    public void clear()
    {
        data = null;
        selectObj.SetActive(false);
        usedObj.SetActive(false);
        sInfo.clear();
    }
}

public class IntensifyPanel : MonoBehaviour, ProcessResponse,BWWarnUI
{
//    public static IntensifyPanel mInstance;

    public enum PAGETYPE : int
    {
        PAGE_PACK = 0,
        PAGE_INTENSIFY = 1,
    }

    public enum INTENSIFYTYPE : int
    {
        TYPE_NULL = 0,
        TYPE_HERO = 1,
        TYPE_SKILL = 2,
        TYPE_PASSIVESKILL = 3,
        TYPE_EQUIP = 4,
    }

    public enum INTENSIFYSORTTYPE : int
    {
        SORT_ALL = 0,
        SORT_RACE = 1,
    }

    //public Font font;

    //****** intensify ctrls******
    public GameObject intensifyCtrl;

    int maxConsumeCardCount = 8;
    int maxBreakCount = 5;

    // intensify value 
    public UILabel useCardNumText;
   
	public GameObject costValueCtrl;
    public UILabel costValueText;
    //public UILabel canEarnValueText;
	//public GameObject needBreakCtrl;
    //public GameObject maxLevelCtrl;
    //public GameObject canEarnExpCtrl;

    public GameObject consumeListCtrl;
    public GameObject consumeListPanelCtrl;
    public GameObject consumeListGrid;
    public UIScrollBar consumeListScrollBar;

    // intensify 2d obj
    public GameObject cardLeftCtrl;
	public UILabel card2DNameLabel;
 	public UILabel levelValueText;
	public SimpleCardInfo2 card2DInfo;
	public GameObject c2DHeroDescCtrl;
	public UILabel c2DHeroATKLabel;
	public UILabel c2DHeroDEFLabel;
	public UILabel c2DHeroHPLabel;
	public GameObject c2DOtherDescCtrl;
	public UILabel c2DOtherDescLabel;
	
    public GameObject equipRightCtrl;
	public SimpleCardInfo1 equipCardInfo;
	public UILabel equipDescLabel;
	public UILabel equipIntensifyCount;
	public GameObject upInfoObj;
	public UILabel upInfoText;
	public int lastEquipLevel;
	

    //public ExpManager view2DExp;

    public NewExpManager intensifyExp;

    public UILabel equipIntensifyResultText;
    public UISprite equipIntensifyResultSprite;
    public UISprite equipIntensifyResultSpriteCrit;

    // sort ctrl
    public GameObject sortBtnCtrl;
    public GameObject sortAllCtrl;
    public GameObject sortRaceCtrl;

    // **********************************************************

    //****** intensify bag ctrls******
    public GameObject intensifyBagCtrl;
    // toggle
    public GameObject toggleCtrl;
    public UIToggle heroCardToggle;
    public UIToggle skillToggle;
    public UIToggle passiveToggle;
    public UIToggle equipToggle;

    // pack grid list
    public GameObject packGridListCtrl;
    // intensfiy card prefab
    string packCardPrefabPath = "Prefabs/UI/IntensifyPanel/IntensifyPackCard";
    GameObject intensifyPackCardPerfab;

    public GameObject packWindowScrollPanel;
    public UIScrollBar packWindowScrollBar;

    // **********************************************************

    PAGETYPE mPageType;
    INTENSIFYTYPE mIntensifyType;
    INTENSIFYSORTTYPE mSortType = INTENSIFYSORTTYPE.SORT_ALL;

    public List<PackElement> sortItemList = new List<PackElement>();

    //全部卡牌列表//
    public List<PackElement> allItemList = new List<PackElement>();
    // pack grid gameobejct list
    List<ListCardInfo> packGridList = new List<ListCardInfo>();
    List<ListCardInfo> consumeGirdList = new List<ListCardInfo>();
    List<PackElement> consumeDataList = new List<PackElement>();
    public PackElement targetData = null;

    public bool notSetExp = false;
    public int oldExp = 0;
    public int oldLevel = 0;
    int moneyCost = 0;

	int curLevel;
	
    /**筛选前的元素**/
    public List<PackElement> allCells = new List<PackElement>();
	public List<PackElementJson> allFromIdList = new List<PackElementJson>();
    /**1选取吞噬者,2选取被吞噬者,3吞噬, 10 获取冥想界面信息，返回冥想界面**/
    private int requestType;

    private bool receiveData;
    private INTENSIFYTYPE tempIntensifyType;

    private bool isMaxLevelAndBreakNum = false;
    private bool mustSelectBreakCard = false;
    private bool equipLevelMax = false;

    bool needShowResurlt = false;
    int intensifyResult = -1;

    PackElement intensifyResultPE = null;

    int errorCode = 0;

    bool needRequestPlayerInfo = false;
    bool doRequestPlayerInfo = false;

    public int selectCardSkillID = 0;

    public BreakPopWnd breakPopWnd;

    public PackElement guideTargetCard = null;
    public PackElement guideConsumeCard = null;
	public PackElement guideTargetEquipCard = null;

    public bool isGuideSelectTargetCard = false;
    public bool isGuideIntensify = false;
    bool isGuideBackToBag = false;

    string equipUpgradeEffectPath = "Prefabs/Effects/UIEffect/ui_juese_zhuangbeiqianghua";
    string expUpgradeEffectPath = "Prefabs/Effects/UIEffect/qianghua_jingyantiao";
    string intensifySuccessEffectPah = "Prefabs/Effects/UIEffect/flyingspark_qianghua";
    string intensifySuccessEndEffectPath = "Prefabs/Effects/UIEffect/ui_juese_kuang";

    GameObject equipUpgradeEffectPrefab = null;
    GameObject expUpgradeEffectPrefab = null;
    GameObject intensifySuccessEffectPrefab = null;
    GameObject intensifySuccessEndEffectPrefab = null;
	
	public GameObject intensifySuccessEffectNode = null;
    public GameObject levelUpEffectNode;
    public bool isOpenBySpirit = false;

    int curNpcId;
    int nullPackNum;



    int mid; //当前玩家激活的领奖id编号//

    int mnum; //当前玩家的冥想次数//

    bool isAll;

    bool isReceiveData = true;

    public bool isScrollViewCome = false;


    /************************请求购买购买start***************************/
    //购买物品的类型 1 金币， 2 体力， 3 扫荡券， 4 挑战次数//
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
    /************************请求购买购买end***************************/
	
	public int curMaxLevel;
	
	private Transform _myTransform;
	
	bool forbitIntensifyFlag = false;
	
	bool intensifyEquip10Times = false;

    public UIScrollBar listValue;
	
	float needRTime = 0;
	
    void Awake()
    {
		_myTransform = transform;
        init();
    }
    // Use this for initialization
    void Start()
    {
        if (PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_QH)
        {
			if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
			{
				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
			}
            intensifyCtrl.SetActive(false);
            //获取界面数据//
            requestType = 1;
            PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify, 1), this);
            //播放声音//
            string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MENU).music;
            MusicManager.playBgMusic(musicName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (receiveData)
        {
            receiveData = false;
			if(errorCode == -3)
				return;
			
            switch (requestType)
            {
            case 1:
            {
                targetData = null;
				if(allItemList!=null)
                allItemList.Clear();
				if(allCells!=null)
				{
					allCells.Clear();
				}
				else
				{
					allCells = new List<PackElement>();	
				}
				for(int i = 0;i<allFromIdList.Count;i++)
				{
					allItemList.Add(allFromIdList[i].pe);
					allCells.Add(allFromIdList[i].pe);
				}
                consumeDataList.Clear();
                if (PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_QH)
                {
                    PlayerInfo.getInstance().BattleOverBackType = 0;
                    isGuideBackToBag = false;
                    isGuideIntensify = false;
                    isGuideSelectTargetCard = false; 
                    _myTransform.gameObject.SetActive(true);
                    showPage(PAGETYPE.PAGE_PACK);
                    if (errorCode == 70)		//vip等级不足//
                    {
						string str = TextsData.getData(493).chinese + "/" + VipData.getData(PlayerInfo.getInstance().player.vipLevel).autoupgrade 
							+ TextsData.getData(494).chinese;
//                        ToastWindow.mInstance.showText(str);
						//提示去充值//
						UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
                    }
                }
                else
                {
                    mIntensifyType = tempIntensifyType;
                    if (mPageType != PAGETYPE.PAGE_PACK)
                    {
                        showPage(PAGETYPE.PAGE_PACK);
                    }
                    else
                    {
                        showPackContents();
                    }
                }
                if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyCard))
                {
                    if (isGuideBackToBag)
                    {
                        isGuideBackToBag = false;
						GuideUI_Intesnify.mInstance.showStep(6);
                    }
                }
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
				{
					if(GuideUI_IntensifyEquip.mInstance.normalType)
					{
						if(GuideUI_IntensifyEquip.mInstance.runningStep == 2)
						{
							GuideUI_IntensifyEquip.mInstance.showStep(3);
						}
						else if(GuideUI_IntensifyEquip.mInstance.runningStep == 3)
						{
							guideTargetEquipCard = sortItemList[0];
							GuideUI_IntensifyEquip.mInstance.showStep(4);
						}
					}
				}
		
            } break;
            case 2:
            {
                sortItemList = allCells;
                isRefresh = false;
                showPage(PAGETYPE.PAGE_INTENSIFY);
                allCells = null;
                notSetExp = false;
                if (GuideManager.getInstance().isRunningGuideID(((int)GuideManager.GuideType.E_IntensifyCard)))
                {
                    if (isGuideSelectTargetCard && sortItemList.Count > 0)
                    {
                        guideConsumeCard = sortItemList[0];
                        isGuideSelectTargetCard = false;
                        GuideUI_Intesnify.mInstance.showStep(3);
                    }
                    if (isGuideIntensify)
                    {
                        isGuideIntensify = false;
                        GuideUI_Intesnify.mInstance.showStep(5);
                    }
                }
            } break;
            case 3:
            {
				
				if(intensifyResultPE == null)
				{
					if(needRTime < 1.5f)
					{
						needRTime+= Time.deltaTime;
					}
					else
					{
						Invoke("recoverIntensifyFlag",5f);
						needRTime = 0;
						return;
					}
					
					receiveData = true;
					return;
				}
				Invoke("recoverIntensifyFlag",1f);
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
				{
					GuideUI_IntensifyEquip.mInstance.hideAllStep();
					GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_IntensifyEquip);
				}
				string cardType = "";
				//1角色卡,2主动技能,3被动技能,4装备,5材料//
                if (intensifyResultPE!=null)
                {
                    switch (intensifyResultPE.eType)
                    {
                        case 1:
                            cardType = "Hero";
                            break;
                        case 2:
                            cardType = "Kill";
                            break;
                        case 3:
                            cardType = "PassiveSkills";
                            break;
                        case 4:
                            cardType = "Equip";
                            break;
                    }

                    string eventName = "Intensify";
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    if (intensifyResult >= 0)
                    {
                        dic.Add("Result", "Success");//结果//
                    }
                    else
                    {
                        dic.Add("Result", "Lose");//结果//
                    }

                    dic.Add("outlay", moneyCost.ToString());//金币花费//
                    dic.Add("PlayerId", PlayerPrefs.GetString("username"));//用户id//
                    dic.Add("CardId", intensifyResultPE.dataId.ToString());//卡牌id//
                    dic.Add("CardType", cardType);//卡牌类型//
                    dic.Add("CardLv", intensifyResultPE.lv);//卡牌等级//

                    TalkingDataManager.SendTalkingDataEvent(eventName, dic);//发送强化talkingdata//
                }
                handleIntensifyResult();
				
				
            } break;
            case 10:
            {
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD);
				SpriteWroldUIManager spriteWorld = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD, "SpriteWroldUIManager")as SpriteWroldUIManager;
                spriteWorld.SetData(nullPackNum, curNpcId, mid, mnum);
                HeadUI.mInstance.hide();
                hide();
            } break;
            }
        }
        if (doRequestPlayerInfo)
        {
            HeadUI.mInstance.requestPlayerInfo();
            doRequestPlayerInfo = false;
        }
    }
	
	void recoverIntensifyFlag()
	{
		forbitIntensifyFlag = false;
	}


    bool isRefresh;


    void handleIntensifyResult()
    {
        switch (errorCode)
        {
            case 0:
                {
                    //==旧的突破次数==//
                    //int oldBreakNum = 0;
                    //if (targetData != null && targetData.eType == 1)
                    //{
                    //    oldBreakNum = targetData.bn;
                    //}
                    targetData = intensifyResultPE;
			
                    consumeDataList.Clear();
                    if (targetData != null)
                    {
                        showPage(PAGETYPE.PAGE_INTENSIFY);
                    }
                    needShowResurlt = false;
                    intensifyResult = -1;
                    intensifyResultPE = null;
                    

                    if (mIntensifyType == INTENSIFYTYPE.TYPE_HERO)
                    {
                        //CardData cd = CardData.getData(targetData.dataId);
                        notSetExp = true;
                        if (targetData != null)
                        {
                            oldExp = targetData.curExp;
                            oldLevel = targetData.lv;
                        }
						requestType = 2;
                        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, targetData.i, (int)mIntensifyType, (int)mSortType), this);
                    }
                    else if (mIntensifyType == INTENSIFYTYPE.TYPE_SKILL)
                    {
                        notSetExp = true;
                        if (targetData != null)
                        {
                            oldExp = targetData.curExp;
                            oldLevel = targetData.lv;
                        }
						requestType = 2;
                        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, targetData.i, (int)mIntensifyType, 0), this);
                    }
                    else if (mIntensifyType == INTENSIFYTYPE.TYPE_PASSIVESKILL)
                    {
                        notSetExp = true;
                        if (targetData != null)
                        {
                            oldExp = targetData.curExp;
                            oldLevel = PassiveSkillData.getData(targetData.dataId).level;
                        }
						requestType = 2;
                        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, targetData.i, (int)mIntensifyType, 0), this);
                    }
                    else if (mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
                    {
                        HeadUI.mInstance.requestPlayerInfo();
						needRequestPlayerInfo = false;
                        isReceiveData = true;
                        return;
                    }

                    if (mIntensifyType != INTENSIFYTYPE.TYPE_EQUIP || mIntensifyType != INTENSIFYTYPE.TYPE_NULL)
                    {
                        GameObject effectObj = GameObject.Instantiate(intensifySuccessEffectPrefab) as GameObject;
                        if (effectObj != null)
                        {
							GameObjectUtil.destroyGameObjectAllChildrens(UISceneEffectNodeManager.mInstance.intensifySuccessFlyEffectNode);
							CancelInvoke("playSuccessEndEffect");
                            GameObjectUtil.gameObjectAttachToParent(effectObj, UISceneEffectNodeManager.mInstance.intensifySuccessFlyEffectNode);
                            GameObjectUtil.setGameObjectLayer(effectObj, STATE.LAYER_ID_UIEFFECT);
                            Invoke("playSuccessEndEffect", 0.3f);
                            GameObject.DestroyObject(effectObj, 1);
                        }
                    }
                    needRequestPlayerInfo = true;

                } break;
            case 10:				//金币不足//
                {

                    int buyType = 1;
                    int jsonType = 1;
                    int costType = 1;
                    ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_INTENSIFY);
                    isReceiveData = true;

                } break;
            case 11:		//卡牌等级达到上限//
                {
                    ToastWindow.mInstance.showText(TextsData.getData(43).chinese);
                    isReceiveData = true;
                } break;
            case 12:		//装备等级达到上限//
                {
                    ToastWindow.mInstance.showText(TextsData.getData(45).chinese);
                    isReceiveData = true;
                } break;
            case 16:		//技能强化等级无法超过军团等级!//
                {
                    ToastWindow.mInstance.showText(TextsData.getData(323).chinese);
                    isReceiveData = true;
                } break;
        }
    }

    void playSuccessEndEffect()
    {

        if (mPageType != PAGETYPE.PAGE_INTENSIFY)
            return;
        GameObject effectObj = GameObject.Instantiate(intensifySuccessEndEffectPrefab) as GameObject;
        if (effectObj)
        {
            GameObjectUtil.gameObjectAttachToParent(effectObj, UISceneEffectNodeManager.mInstance.intensifySuccessEndEffectNode);
            GameObjectUtil.setGameObjectLayer(effectObj, STATE.LAYER_ID_UIEFFECT);
            GameObject.DestroyObject(effectObj, 0.3f);
        }
        //isRefresh = false;
   }

    public void init()
    {
		if(isScrollViewCome)
		{
			mPageType = PAGETYPE.PAGE_INTENSIFY;
		}
		else
		{
       		mPageType = PAGETYPE.PAGE_PACK;
		}
        mIntensifyType = INTENSIFYTYPE.TYPE_HERO;

        //card2DDesLabel.gameObject.GetComponent<TweenScale>().enabled = false;
		
		c2DHeroATKLabel.gameObject.GetComponent<TweenScale>().enabled = false;
		c2DHeroDEFLabel.gameObject.GetComponent<TweenScale>().enabled = false;
		c2DHeroHPLabel.gameObject.GetComponent<TweenScale>().enabled = false;
		c2DOtherDescLabel.gameObject.GetComponent<TweenScale>().enabled = false;
		
		
        intensifyExp.setPanelType(NewExpManager.PANELTYPE.E_Intensify);
        if (equipUpgradeEffectPrefab == null)
        {
            equipUpgradeEffectPrefab = Resources.Load(equipUpgradeEffectPath) as GameObject;
        }
        if (expUpgradeEffectPrefab == null)
        {
            expUpgradeEffectPrefab = Resources.Load(expUpgradeEffectPath) as GameObject;
        }
        if (intensifyPackCardPerfab == null)
        {
            intensifyPackCardPerfab = Resources.Load(packCardPrefabPath) as GameObject;
        }
        if (intensifySuccessEffectPrefab == null)
        {
            intensifySuccessEffectPrefab = Resources.Load(intensifySuccessEffectPah) as GameObject;
        }
        if (intensifySuccessEndEffectPrefab == null)
        {
            intensifySuccessEndEffectPrefab = Resources.Load(intensifySuccessEndEffectPath) as GameObject;
        }
        if (sortItemList == null)
        {
            sortItemList = new List<PackElement>();
        }
        if (allCells == null)
        {
            allCells = new List<PackElement>();
        }
    }

    public void show()
    {
	
        isGuideBackToBag = false;
        isGuideIntensify = false;
        isGuideSelectTargetCard = false;

        if (equipUpgradeEffectPrefab == null)
        {
            equipUpgradeEffectPrefab = Resources.Load(equipUpgradeEffectPath) as GameObject;
        }

        if (expUpgradeEffectPrefab == null)
        {
            expUpgradeEffectPrefab = Resources.Load(expUpgradeEffectPath) as GameObject;
        }

        if (intensifyPackCardPerfab == null)
        {
            intensifyPackCardPerfab = Resources.Load(packCardPrefabPath) as GameObject;
        }

        if (intensifySuccessEffectPrefab == null)
        {
            intensifySuccessEffectPrefab = Resources.Load(intensifySuccessEffectPah) as GameObject;
        }

        if (intensifySuccessEndEffectPrefab == null)
        {
            intensifySuccessEndEffectPrefab = Resources.Load(intensifySuccessEndEffectPath) as GameObject;
        }

        targetData = null;
        consumeDataList.Clear();
		if(!isScrollViewCome)
		{
			showPage(PAGETYPE.PAGE_PACK);
			isScrollViewCome = true;
		}
		else
		{
			sortItemList = allCells;
		}
    }

    public void hide()
    {
        isOpenBySpirit = false;
        
//        base.hide();
        gc();
		
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY);
    }

    private void gc()
    {
        targetData = null;
        consumeDataList.Clear();
        intensifyPackCardPerfab = null;
        if (sortItemList != null)
        {
            //sortItemList.Clear();
        }
        //sortItemList = null;
        packGridList.Clear();
        consumeGirdList.Clear();
        if (allCells != null)
        {
            allCells.Clear();
        }
		if(allFromIdList!=null)
		{
			allFromIdList.Clear();
		}
        //allCells = null;
        
        intensifyResultPE = null;
        guideTargetCard = null;
        guideConsumeCard = null;
		guideTargetEquipCard = null;
        equipUpgradeEffectPrefab = null;
        expUpgradeEffectPrefab = null;
        intensifySuccessEffectPrefab = null;
        intensifySuccessEndEffectPrefab = null;
		
        GameObjectUtil.destroyGameObjectAllChildrens(packGridListCtrl);
        GameObjectUtil.destroyGameObjectAllChildrens(consumeListGrid);

        Resources.UnloadUnusedAssets();
    }
	
	void clearEffectObj()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(UISceneEffectNodeManager.mInstance.intensifySuccessFlyEffectNode);
		GameObjectUtil.destroyGameObjectAllChildrens(UISceneEffectNodeManager.mInstance.intensifySuccessEndEffectNode);
		GameObjectUtil.destroyGameObjectAllChildrens(UISceneEffectNodeManager.mInstance.intensifyEquipSuccessEffectNode);
	}

    public void setIntensifyType(INTENSIFYTYPE type)
    {
        mIntensifyType = type;
    }

    /*从强化界面退回强化背包界面需要重新刷新可强化的卡牌列表*/
    public void backToIntensifyPackRequest()
    {
        tempIntensifyType = mIntensifyType;
        requestType = 1;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify, (int)tempIntensifyType), this);
    }

    // 点击强化目标卡片物品 //
    public void onClickTargetCard()
    {
        targetData = null;
        mSortType = INTENSIFYSORTTYPE.SORT_ALL;
        backToIntensifyPackRequest();
        consumeDataList.Clear();
    }
    // 点击强化界面中的按钮 按钮参数 1 回退，2 强化,3 sort btn 排序// 

    public void warnningSure()
    {
        if (targetData == null)
        {
            return;
        }
        if (isMaxLevelAndBreakNum)
        {
            ToastWindow.mInstance.showText(TextsData.getData(44).chinese);
            return;
        }
        if (mustSelectBreakCard)
        {
            ToastWindow.mInstance.showText(TextsData.getData(43).chinese);
            return;
        }
        if (equipLevelMax)
        {
            ToastWindow.mInstance.showText(TextsData.getData(45).chinese);
            return;
        }
        // click intensify btn
        if (mIntensifyType == INTENSIFYTYPE.TYPE_HERO)
        {
            if (consumeDataList.Count == 0)
            {
                return;
            }
            if (PlayerInfo.getInstance().player.gold < moneyCost)		//金币不足//
            {
                int buyType = 1;
                int jsonType = 1;
                int costType = 1;
                ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_INTENSIFY);
                return;
            }

            //播放音效//
            MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SUCC);

            PackElement dbc = targetData;
            List<int> list = new List<int>();
            for (int i = 0; i < consumeDataList.Count; ++i)
            {
                if (consumeDataList[i] != null)
                {
                    list.Add(consumeDataList[i].i);
                }
            }

            int type = (int)mIntensifyType;
            int index = dbc.i;
            oldExp = dbc.curExp;
            oldLevel = dbc.lv;
            requestType = 3;
           	levelValueText.text = oldLevel.ToString() + " / " + curMaxLevel.ToString();
            PlayerInfo.getInstance().sendRequest(new IntensifyJson(type, index, list,1), this);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_SKILL)
        {
            if (consumeDataList.Count == 0)
            {
                return;
            }

            if (PlayerInfo.getInstance().player.gold < moneyCost)			//金币不足//
            {
                int buyType = 1;
                int jsonType = 1;
                int costType = 1;
                ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_INTENSIFY);
                return;
            }

            //播放音效//
            MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SUCC);

            PackElement dbs = targetData;
            List<int> list = new List<int>();
            foreach (PackElement o in consumeDataList)
            {
                list.Add(o.i);
            }
            int type = (int)mIntensifyType;
            int index = dbs.i;
            oldExp = dbs.curExp;
            oldLevel = dbs.lv;
            requestType = 3;
            levelValueText.text = oldLevel.ToString() + " / " + curMaxLevel.ToString();
            PlayerInfo.getInstance().sendRequest(new IntensifyJson(type, index, list,1), this);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_PASSIVESKILL)
        {
            if (consumeDataList.Count == 0)
            {
                return;
            }

            //播放音效//
            MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SUCC);

            PackElement dbs = targetData;
            List<int> list = new List<int>();
            foreach (PackElement o in consumeDataList)
            {
                list.Add(o.i);
            }
            int type = (int)mIntensifyType;
            int index = dbs.i;
            oldExp = dbs.curExp;
            oldLevel = PassiveSkillData.getData(dbs.dataId).level;
            requestType = 3;
            levelValueText.text = oldLevel.ToString() + " / " + curMaxLevel.ToString();
            PlayerInfo.getInstance().sendRequest(new IntensifyJson(type, index, list,1), this);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
        {
            if (PlayerInfo.getInstance().player.gold < moneyCost)			//金币不足//
            {
				 int buyType = 1;
                int jsonType = 1;
                int costType = 1;
                ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_INTENSIFY);
                return;

            }

            //播放音效//
            MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SURE);

            int type = (int)mIntensifyType;
            int index = targetData.i;
			lastEquipLevel = targetData.lv;
            List<int> list = new List<int>();
            requestType = 3;
            PlayerInfo.getInstance().sendRequest(new IntensifyJson(type, index, list,1), this);
        }
    }

    public void warnningCancel()
    { 
        
    }
    public void onClickIntensifyCtrlsBtn(int param)
    {
        bool isReach = false;
        switch (param)
        {
            case 1:
                {
					curLevel = 0;
                    //播放音效//
                    MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
                    backToIntensifyBag();
			
                }
                break;
            case 2:
                {
					if(HeadUI.mInstance.wairRequestPlayerInfo)
					{
						return;	
					}
                	if(forbitIntensifyFlag)
					{
						return;
					}
					forbitIntensifyFlag = true;	
                    foreach (PackElement info in consumeDataList)
                    {
                        if (info.eType == (int)INTENSIFYTYPE.TYPE_HERO)
                        {
                            CardData cd = CardData.getData(info.dataId);
                            if (cd.star >= 3)
                            {
                                isReach = true;

                            }
                        }
						else if (info.eType == (int)INTENSIFYTYPE.TYPE_SKILL)
                        {
                            if(curLevel>= PlayerInfo.getInstance().player.level)
							{
								ToastWindow.mInstance.showText(TextsData.getData(323).chinese);
								return;
							}
                        }
                    }
                    if(isReach)
                        ToastWarnUI.mInstance.showWarn(TextsData.getData(378).chinese, this);
                    else
                        warnningSure();
                } 
                break;
            case 3:
                {
                    //播放音效//
                    MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
                    if (mSortType == INTENSIFYSORTTYPE.SORT_ALL)
                    {
                        mSortType = INTENSIFYSORTTYPE.SORT_RACE;
                    }
                    else if (mSortType == INTENSIFYSORTTYPE.SORT_RACE)
                    {
                        mSortType = INTENSIFYSORTTYPE.SORT_ALL;
                    }
                    PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, targetData.i, (int)mIntensifyType, (int)mSortType), this);
                } break;

            case 4:
                {
					if(HeadUI.mInstance.wairRequestPlayerInfo)
					{
						return;	
					}
                    //一键选取//
                        if (mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
                        {
                            if (VipData.getData(PlayerInfo.getInstance().player.vipLevel).autoupgrade == 0)	//vip等级不足,无法强化10次//
                            {
								string str = TextsData.getData(493).chinese + VipData.getLevelForIntensify() + TextsData.getData(494).chinese;
//                                ToastWindow.mInstance.showText(str);
								UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
                            }
                            else
                            {
                                //this.transform.FindChild("IntensifyCtrl/Equip").gameObject.SetActive(true);
                               // InvokeRepeating("IntensifyAll", 0f, 1.0f);
                                IntensifyAll();
								intensifyEquip10Times = true;
                                curlevel = targetData.lv;
								lastEquipLevel = targetData.lv;
                            }
                        }
                        else
                            GetAll();
                   
                }
                break;
        }
    }
    int curlevel;
    public void IntensifyAll()
    {
        if (isReceiveData == false)
        {
            return;
        }

        if (targetData == null)
        {
            return;
        }
        else
        {
            //if (equipsIndexs == 10)
            //{
            //    CancelInvoke("IntensifyAll"); 
            //    this.transform.FindChild("IntensifyCtrl/Equip").gameObject.SetActive(false);
            //    return;
            //}
            //if (equipLevelMax)
            //{
            //    ToastWindow.mInstance.showText(TextsData.getData(45).chinese);
            //    this.transform.FindChild("IntensifyCtrl/Equip").gameObject.SetActive(false);
            //    CancelInvoke("IntensifyAll");
            //    return;
            //}
            if (PlayerInfo.getInstance().player.gold < moneyCost)			//金币不足//
            {
                int buyType = 1;
                int jsonType = 1;
                int costType = 1;
                ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_INTENSIFY);
                //CancelInvoke("IntensifyAll");
                this.transform.FindChild("IntensifyCtrl/Equip").gameObject.SetActive(false);
                return;
            }
            //equipsIndexs++;
            //播放音效//
            MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SURE);
            int type = (int)mIntensifyType;
            int index = targetData.i;
            List<int> list = new List<int>();
            requestType = 3;
            PlayerInfo.getInstance().sendRequest(new IntensifyJson(type, index, list, 10), this);
            isReceiveData = false;
        }
    }


    public void GetAll()
    {
        if (isRefresh)
        {
            return;
        }
        isRefresh = true;
        if (consumeDataList.Count < 8)
        {

            for (int i = 0; i < sortItemList.Count; i++)
            {
                try
                {
                    PackElement itemInfo = sortItemList[i];
                    foreach (PackElement pe in consumeDataList)
                    {
                        if (pe.i == itemInfo.i)
                        {
                            isAll = true;
                        }
                    }
                    if (isAll)
                    {
                        isAll = false;
                        continue;
                    }
                    consumeDataList.Add(itemInfo);
                    ListCardInfo info = consumeGirdList[i];
                    info.selectObj.SetActive(true);
                    showIntensifyPageAllInfo();
                    if (consumeDataList.Count > 7)
                    {
                        return;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }
        }
    }
    public void backToIntensifyBag()
    {
        targetData = null;
        consumeDataList.Clear();
        mSortType = INTENSIFYSORTTYPE.SORT_ALL;
        backToIntensifyPackRequest();
    }

    public void guideClickIntensifyBackToBagBtn()
    {
        backToIntensifyBag();
        isGuideBackToBag = true;
    }

    public void guideClickIntensify()
    {
        if (targetData == null)
            return;

        PackElement dbc = targetData;
        List<int> list = new List<int>();
        foreach (PackElement o in consumeDataList)
        {
            list.Add(o.i);
        }
        int type = (int)mIntensifyType;
        int index = dbc.i;
        oldExp = dbc.curExp;
        oldLevel = dbc.lv;
        requestType = 3;
        PlayerInfo.getInstance().sendRequest(new IntensifyJson(type, index, list,1), this);
    }
	
	public void guideClickEquipIntensify()
	{
		if(targetData == null)
		{
			return;
		}
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SURE);

        int type = (int)mIntensifyType;
        int index = targetData.i;
        List<int> list = new List<int>();
        requestType = 3;
        PlayerInfo.getInstance().sendRequest(new IntensifyJson(type, index, list,1), this);
		
	}

    public void closeIntensifyPanel()
    {
        if (isOpenBySpirit)
        {
            requestType = 10;
            PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SPRITEWROLD_INTO), this);
        }
        else
        {
			UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
			GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
			if(obj!=null)
			{
				MainMenuManager main = obj.GetComponent<MainMenuManager>();
				main.SetData(STATE.ENTER_MAINMENU_BACK);
			}
            hide();
        }
        mIntensifyType = INTENSIFYTYPE.TYPE_HERO;
    }

    // 点击背包界面中按钮//
    // 按钮参数 1 回退，2 equip Intensify，3 卡牌强化，4 skill 强化,5 passive skill //
    public void onClickIntensifyPackCtrlsBtn(int param)
    {
        listValue.value = 0;
        switch (param)
        {
            case 1:
                {
                    //播放音效//
                    MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
                    closeIntensifyPanel();
					return;
                } 
            case 2:
                {
                    //播放音效//
                    MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
                    intensifyCtrl.transform.FindChild("AllIntensifyBtn/Background/Label").GetComponent<UILabel>().text = TextsData.getData(450).chinese;
                    if (mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
                        return;
                    tempIntensifyType = INTENSIFYTYPE.TYPE_EQUIP;
                } break;
            case 3:
                {
                    //播放音效//
                    MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
                    intensifyCtrl.transform.FindChild("AllIntensifyBtn/Background/Label").GetComponent<UILabel>().text = TextsData.getData(451).chinese;
                    if (mIntensifyType == INTENSIFYTYPE.TYPE_HERO)
                        return;
                    tempIntensifyType = INTENSIFYTYPE.TYPE_HERO;
                } break;
            case 4:
                {
                    //播放音效//
                    MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
                    intensifyCtrl.transform.FindChild("AllIntensifyBtn/Background/Label").GetComponent<UILabel>().text = TextsData.getData(451).chinese;
                    if (mIntensifyType == INTENSIFYTYPE.TYPE_SKILL)
                        return;
                    tempIntensifyType = INTENSIFYTYPE.TYPE_SKILL;
                } break;
            case 5:
                {
                    //播放音效//
                    MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
                    intensifyCtrl.transform.FindChild("AllIntensifyBtn/Background/Label").GetComponent<UILabel>().text = TextsData.getData(451).chinese;
                    if (mIntensifyType == INTENSIFYTYPE.TYPE_PASSIVESKILL)
                        return;
                    tempIntensifyType = INTENSIFYTYPE.TYPE_PASSIVESKILL;

                } break;
        }
        requestType = 1;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify, (int)tempIntensifyType), this);
    }

    public PackElement getSortItem(int index)
    {
        foreach (PackElement pe in sortItemList)
        {
            if (pe.i == index)
            {
                return pe;
            }
        }
        return null;
    }

    public void onSelectConsumeCardItem(int index)
    {
        if (index == -1)
            return;
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
        PackElement data = getSortItem(index);
        bool isInConsumeList = false;
        PackElement needRemoveData = null;
        for (int i = 0; i < consumeDataList.Count; ++i)
        {
            PackElement itemInfo = consumeDataList[i];
            if (itemInfo.i == data.i)
            {
                needRemoveData = itemInfo;
                isInConsumeList = true;
                break;
            }
        }
        for (int i = 0; i < consumeGirdList.Count; ++i)
        {

            ListCardInfo info = consumeGirdList[i];
            if (info.data.i == data.i)
            {
                if (isInConsumeList)
                {
                    info.selectObj.SetActive(false);
                    consumeDataList.Remove(needRemoveData);
                }
                else
                {
                    if (mIntensifyType != INTENSIFYTYPE.TYPE_PASSIVESKILL && consumeDataList.Count >= maxConsumeCardCount)
                        return;
                    info.selectObj.SetActive(true);
                    consumeDataList.Add(data);
                }
                break;
            }
        }
        showIntensifyPageAllInfo();
    }

    //guide select consume card,only one exp card,after selected,GuideUI7 run next step
    public void guideSelectConsumeCard()
    {
        if (guideConsumeCard == null)
        {
            return;
        }

        PackElement data = guideConsumeCard;

        for (int i = 0; i < consumeGirdList.Count; ++i)
        {
            ListCardInfo info = consumeGirdList[i];
            if (info.data.i == data.i)
            {

                info.selectObj.SetActive(true);
                consumeDataList.Add(data);
                break;
            }
        }
        showIntensifyPageAllInfo();
        isGuideIntensify = true;
    }

    //  选择强化包中的卡片物品 // 
    public void onSelectPackCardItem(int index)
    {
        if (index == -1)
            return;

        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
        requestType = 2;

        PackElement data = getSortItem(index);
        targetData = data;

        if (mIntensifyType == INTENSIFYTYPE.TYPE_HERO)
        {
            PackElement dbc = data;
            if (dbc != null)
            {
                oldExp = dbc.curExp;
                oldLevel = dbc.lv;
            }
            PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, targetData.i, (int)mIntensifyType, 0), this);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_SKILL)
        {
            PackElement dbs = data;
            if (dbs != null)
            {
                oldExp = dbs.curExp;
                oldLevel = dbs.lv;
            }
            PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, targetData.i, (int)mIntensifyType, 0), this);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_PASSIVESKILL)
        {
            PackElement dbs = data;
            if (dbs != null)
            {
                oldExp = dbs.curExp;
                oldLevel = PassiveSkillData.getData(dbs.dataId).level;
            }
            PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, targetData.i, (int)mIntensifyType, 0), this);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
        {
            showPage(PAGETYPE.PAGE_INTENSIFY);
        }
    }

    // guide select intensify target card
    public void selectGuideTargetCard()
    {
        if (guideTargetCard == null)
        {
            return;
        }
        requestType = 2;
        targetData = guideTargetCard;
        oldExp = targetData.curExp;
        oldLevel = targetData.lv;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, targetData.i, (int)mIntensifyType, 0), this);
        isGuideSelectTargetCard = true;
    }
	
	// guide select intensify target equip
	public void selectGuideTargetEquipCard()
	{
		if(guideTargetEquipCard == null)
		{
			return;
		}
		requestType = 2;
		targetData = guideTargetEquipCard;
		showPage(PAGETYPE.PAGE_INTENSIFY);
		
	}

    public void drawConsumeListGirds()
    {
		if(mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
		{
			return;
		}
        for (int i = 0; i < consumeGirdList.Count; ++i)
        {
            ListCardInfo info = consumeGirdList[i];
            info.clear();
        }

        consumeListGrid.GetComponent<UIGrid2>().repositionNow = true;
        consumeListPanelCtrl.transform.localPosition = Vector3.zero;
        consumeListPanelCtrl.GetComponent<UIPanel>().clipRange = new Vector4(0, 0, 592, 390);
        consumeListScrollBar.value = 0;

        if (consumeGirdList.Count < 8)
        {
            for (int i = 0; i < 8; ++i)
            {
                GameObject obj = GameObject.Instantiate(intensifyPackCardPerfab) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(obj, consumeListGrid,true);
                ListCardInfo info = new ListCardInfo();
                info.data = null;
                info.sInfo = obj.GetComponent<SimpleCardInfo1>();
                info.sInfo.bm.target = _myTransform.gameObject;
                info.sInfo.bm.functionName = "onSelectConsumeCardItem";
                info.selectObj = GameObjectUtil.findGameObjectByName(obj, "MarkSelect");
                info.usedObj = GameObjectUtil.findGameObjectByName(obj, "MarkUsed");
                info.clear();
                consumeGirdList.Add(info);
            }
        }
        int hasItemLineCount = (sortItemList.Count - 1) / 4 + 1;
        int totalGridLineCout = (consumeGirdList.Count - 1) / 4 + 1;
        if (hasItemLineCount <= totalGridLineCout)
        {
            for (int i = 0; i < consumeGirdList.Count; ++i)
            {
                ListCardInfo info = consumeGirdList[i];
                if (i < sortItemList.Count)
                {
                    info.data = sortItemList[i];
                }
                else
                {
                    info.data = null;
                }
                setCardItemDisplay(info);
                
                // TODO
                if (checkCardIsSelectInPack(info.data))
                {
                    info.selectObj.SetActive(true);
                }
                else
                {
                    info.selectObj.SetActive(false);
                }
                if (info.data == null)
                {
                    if (i > (hasItemLineCount * 4 - 1) && i > 7)
                    {
                        info.sInfo.gameObject.SetActive(false);
                    }
                    else
                    {
                        info.sInfo.gameObject.SetActive(true);
                    }
                }
                else
                {
                    info.sInfo.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < consumeGirdList.Count; ++i)
            {
                ListCardInfo info = consumeGirdList[i];
                info.data = sortItemList[i];
                setCardItemDisplay(info);
                if (checkCardIsSelectInPack(info.data))
                {
                    info.selectObj.SetActive(true);
                }
                else
                {
                    info.selectObj.SetActive(false);
                }
            }
            for (int i = totalGridLineCout * 4; i < hasItemLineCount * 4; ++i)
            {
                GameObject obj = GameObject.Instantiate(intensifyPackCardPerfab) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(obj, consumeListGrid,true);
                ListCardInfo info = new ListCardInfo();
                info.data = null;
                info.sInfo = obj.GetComponent<SimpleCardInfo1>();
                info.sInfo.bm.target = _myTransform.gameObject;
                info.sInfo.bm.functionName = "onSelectConsumeCardItem";
                info.selectObj = GameObjectUtil.findGameObjectByName(obj, "MarkSelect");
                info.usedObj = GameObjectUtil.findGameObjectByName(obj, "MarkUsed");
                info.clear();
                if (i < sortItemList.Count)
                {
                    info.data = sortItemList[i];
                }
                else
                {
                    info.data = null;
                }
                setCardItemDisplay(info);
                
                if (checkCardIsSelectInPack(info.data))
                {
                    info.selectObj.SetActive(true);
                }
                else
                {
                    info.selectObj.SetActive(false);
                }
                consumeGirdList.Add(info);
            }
        }
    }

    public void drawIntensifyPackGrids()
    {
        if (packGridList.Count < 10)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject obj = GameObject.Instantiate(intensifyPackCardPerfab) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(obj, packGridListCtrl,true);
                ListCardInfo info = new ListCardInfo();
                info.data = null;
                info.sInfo = obj.GetComponent<SimpleCardInfo1>();
                info.sInfo.bm.target = _myTransform.gameObject;
                info.sInfo.bm.functionName = "onSelectPackCardItem";
                info.selectObj = GameObjectUtil.findGameObjectByName(obj, "MarkSelect");
                info.usedObj = GameObjectUtil.findGameObjectByName(obj, "MarkUsed");
                info.clear();
                packGridList.Add(info);
            }
        }
        int hasItemLineCount = (sortItemList.Count - 1) / 5 + 1;
        int totalGridLineCout = (packGridList.Count - 1) / 5 + 1;
        if (hasItemLineCount <= totalGridLineCout)
        {
            for (int i = 0; i < packGridList.Count; ++i)
            {
                ListCardInfo info = packGridList[i];
                if (i < sortItemList.Count)
                {
                    info.data = sortItemList[i];
                }
                else
                {
                    info.data = null;
                }
				if(i<allFromIdList.Count)
				{
                	setCardItemDisplay(info,allFromIdList[i].type);
				}
				else
				{
					setCardItemDisplay(info);
				}
                if (info.data == null)
                {
                    if (i > (hasItemLineCount * 5 - 1) && i > 9)
                    {
                        info.sInfo.gameObject.SetActive(false);
                    }
                    else
                    {
                        info.sInfo.gameObject.SetActive(true);
                    }
                }
                else
                {
                    info.sInfo.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < packGridList.Count; ++i)
            {
                ListCardInfo info = packGridList[i];
                info.data = sortItemList[i];
                setCardItemDisplay(info,allFromIdList[i].type);
                
            }
            for (int i = totalGridLineCout * 5; i < hasItemLineCount * 5; ++i)
            {
                GameObject obj = GameObject.Instantiate(intensifyPackCardPerfab) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(obj, packGridListCtrl,true);
                ListCardInfo info = new ListCardInfo();
                info.data = null;
                info.sInfo = obj.GetComponent<SimpleCardInfo1>();
                info.sInfo.bm.target = _myTransform.gameObject;
                info.sInfo.bm.functionName = "onSelectPackCardItem";
                info.selectObj = GameObjectUtil.findGameObjectByName(obj, "MarkSelect");
                info.usedObj = GameObjectUtil.findGameObjectByName(obj, "MarkUsed");
				
                info.clear();
                if (i < sortItemList.Count)
                {
                    info.data = sortItemList[i];
					setCardItemDisplay(info,allFromIdList[i].type);
                }
                else
                {
                    info.data = null;
					setCardItemDisplay(info);
                }
                
                packGridList.Add(info);
            }
        }
    }

    public void showPage(PAGETYPE pageType)
    {
        mPageType = pageType;
        switch (mPageType)
        {
            case PAGETYPE.PAGE_PACK:
                {
                    intensifyCtrl.SetActive(false);
                    intensifyBagCtrl.SetActive(true);
                    showPackCtrlsDisplay();
                    showPackContents();
					needShowResurlt = false;
					clearEffectObj();
                } break;
            case PAGETYPE.PAGE_INTENSIFY:
                {
                    intensifyCtrl.SetActive(true);
                    intensifyBagCtrl.SetActive(false);
                    showIntensifyPageCtrlsDisplay();
                    drawConsumeListGirds();
                    showIntensifyPageAllInfo();
                } break;
        }
    }

    public void showPackCtrlsDisplay()
    {
        toggleCtrl.SetActive(true);
        if (mIntensifyType == INTENSIFYTYPE.TYPE_HERO)
        {
            heroCardToggle.value = true;
            skillToggle.value = false;
            passiveToggle.value = false;
            equipToggle.value = false;
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_SKILL)
        {
            heroCardToggle.value = false;
            skillToggle.value = true;
            passiveToggle.value = false;
            equipToggle.value = false;
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_PASSIVESKILL)
        {
            heroCardToggle.value = false;
            skillToggle.value = false;
            passiveToggle.value = true;
            equipToggle.value = false;
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
        {
            heroCardToggle.value = false;
            skillToggle.value = false;
            passiveToggle.value = false;
            equipToggle.value = true;
        }
    }

    public void showPackContents()
    {
        for (int i = 0; i < packGridList.Count; ++i)
        {
            ListCardInfo info = packGridList[i];
            info.clear();
        }
        packGridListCtrl.GetComponent<UIGrid2>().repositionNow = true;
        packWindowScrollBar.value = 0;
        packWindowScrollPanel.transform.localPosition = Vector3.zero;
        packWindowScrollPanel.GetComponent<UIPanel>().clipRange = new Vector4(0, 0, 720, 390);
        sortItemList = allCells;
        drawIntensifyPackGrids();
        //allCells = null;
    }

    public void showIntensifyPageCtrlsDisplay()
    {
        if (mIntensifyType == INTENSIFYTYPE.TYPE_HERO)
        {
            // view 2d card ctrl
           	cardLeftCtrl.SetActive(true);
			c2DHeroDescCtrl.SetActive(true);
			c2DOtherDescCtrl.SetActive(false);
			equipRightCtrl.SetActive(false);
			
            PackElement dbc = targetData;
            if (dbc != null && !notSetExp)
            {
                CardData cd = CardData.getData(dbc.dataId);
                if (cd != null)
                {
                    intensifyExp.hideLightAndYellowBar();
                    intensifyExp.setData(STATE.EXP_TYPE_RESULT_CARD, oldExp, oldLevel, dbc.curExp, dbc.lv, cd.star);
                }
            }
            // text ctrl
           	//canEarnExpCtrl.SetActive(true);
            costValueCtrl.SetActive(true);
            // consume list ctrl
            consumeListCtrl.SetActive(true);
            useCardNumText.gameObject.SetActive(true);
            // sort ctrl
            sortBtnCtrl.SetActive(true);
            showSortLabelCtrl();

            intensifyExp.hideLightAndYellowBar();
            intensifyExp.gameObject.SetActive(true);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_SKILL)
        {
            // view 2d card ctrl
           	cardLeftCtrl.SetActive(true);
			c2DHeroDescCtrl.SetActive(false);
			c2DOtherDescCtrl.SetActive(true);
            equipRightCtrl.SetActive(false);
            //card2DDesLabel.gameObject.SetActive(false);
            //card2DDesLabel.gameObject.SetActive(true);
            // text ctrl
            //canEarnExpCtrl.SetActive(true);
            costValueCtrl.SetActive(true);
            //consume list ctrl
            consumeListCtrl.SetActive(true);
            useCardNumText.gameObject.SetActive(true);
            // sort ctrl
            sortBtnCtrl.SetActive(false);

            intensifyExp.hideLightAndYellowBar();
            intensifyExp.gameObject.SetActive(true);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_PASSIVESKILL)
        {
            // view 2d card ctrl
           	cardLeftCtrl.SetActive(true);
			c2DHeroDescCtrl.SetActive(false);
			c2DOtherDescCtrl.SetActive(true);
            equipRightCtrl.SetActive(false);
            //card2DDesLabel.gameObject.SetActive(false);
           // card2DDesLabel.gameObject.SetActive(true);
            // text ctrl
            //canEarnExpCtrl.SetActive(true);
            costValueCtrl.SetActive(false);
            //consume list ctrl
            consumeListCtrl.SetActive(true);
            useCardNumText.gameObject.SetActive(true);
            // sort ctrl
            sortBtnCtrl.SetActive(false);

            intensifyExp.gameObject.SetActive(true);
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
        {
            // view 2d card ctrl
           	cardLeftCtrl.SetActive(false);
            intensifyCtrl.transform.FindChild("AllIntensifyBtn/Background/Label").GetComponent<UILabel>().text = TextsData.getData(450).chinese;
            equipRightCtrl.SetActive(true);
            equipIntensifyResultSprite.gameObject.SetActive(false);
            equipIntensifyResultSpriteCrit.gameObject.SetActive(false);

			costValueCtrl.SetActive(true);
            consumeListCtrl.SetActive(false);
            useCardNumText.gameObject.SetActive(false);
            sortBtnCtrl.SetActive(false);
			upInfoObj.SetActive(false);
        }

    }

    public void showIntensifyPageAllInfo()
    {

       //maxLevelCtrl.SetActive(false);
       //needBreakCtrl.SetActive(false);
        isMaxLevelAndBreakNum = false;
        mustSelectBreakCard = false;
        equipLevelMax = false;

        if (mIntensifyType == INTENSIFYTYPE.TYPE_HERO)
        {
            if (targetData == null)
                return;
            PackElement dbc = targetData;
            if (dbc == null)
                return;
			CardData cd = CardData.getData(dbc.dataId);
			if(cd == null)
				return;
			card2DInfo.clear();
			card2DInfo.setSimpleCardInfo(dbc.dataId,GameHelper.E_CardType.E_Hero,dbc);
			card2DNameLabel.text = cd.name;	
			
			
            useCardNumText.text = consumeDataList.Count.ToString() + " / 8";
            if (consumeDataList.Count == maxConsumeCardCount)
            {
                useCardNumText.color = Color.red;
            }
            else
            {
                useCardNumText.color = Color.white;
            }
            int maxLevel = 0;
            if (dbc.bn > 0)
            {
                maxLevel = cd.maxLevel + EvolutionData.getData(cd.star, dbc.bn).lvl;
            }
            else
            {
                maxLevel = cd.maxLevel;
            }

            int curLevel = dbc.lv;
            //levelValueText.text = curLevel.ToString() + " / " + maxLevel.ToString();
            int canEarnExp = 0;  /*消耗卡牌提供的总经验*/
            //int breakNum = 0;/*消耗卡牌提供的总突破次数*/
            for (int i = 0; i < consumeDataList.Count; ++i)
            {
                PackElement cdbc = consumeDataList[i];
                if (cdbc != null)
                {
                    CardData ccd = CardData.getData(cdbc.dataId);
                    if (ccd.race == cd.race)
                    {
                        canEarnExp += (int)(cdbc.lv * ccd.exp * 1.5f);
                    }
                    else
                    {
                        canEarnExp += cdbc.lv * ccd.exp;
                    }
                    /*if((ccd.race == STATE.RACE_BREAK && ccd.star == cd.star)||(cdbc.dataId==dbc.dataId))
                    {
                        breakNum++;
                    }*/
                }
            }
            moneyCost = consumeDataList.Count * CardExpData.getCost(dbc.lv);
            costValueText.text = moneyCost.ToString();
            int canLevelUpNum = 0;/*可以升级数*/
            int curExp = dbc.curExp;
            int nextLevel = curLevel + 1;
            int totalExp = canEarnExp;
            while (totalExp > 0)
            {
                if (nextLevel < maxLevel + 1)
                {
                    int curLevelNeedExp = CardExpData.getExp(nextLevel, cd.star) - curExp;
                    totalExp -= curLevelNeedExp;
                    if (totalExp > 0)
                    {
                        canLevelUpNum++;
                        nextLevel++;
                        curExp = 0;
                    }
                }
                else
                {
                    totalExp = 0;
                }
            }

            if (curLevel == maxLevel)
            {
                if (dbc.bn == maxBreakCount)
                {
                    /*等级和突破次数达上限 则不允许强化*/
                    isMaxLevelAndBreakNum = true;
                    //maxLevelCtrl.SetActive(true);
                    //canEarnExpCtrl.SetActive(false);
                    //needBreakCtrl.SetActive(false);
                }
                else
                {
                    //if(canBreakNum == 0)
                    //{

                    //}
                    /*等级达上限 没有进行突破则得不到经验不允许强化*/
                    mustSelectBreakCard = true;
                    //maxLevelCtrl.SetActive(false);
                    //canEarnExpCtrl.SetActive(false);
                    //needBreakCtrl.SetActive(true);
                }
            }
            else
            {
                //canEarnExpCtrl.SetActive(true);
                //maxLevelCtrl.SetActive(false);
                //needBreakCtrl.SetActive(false);
                //canEarnValueText.text = canEarnExp.ToString();
            }
            if (intensifyExp.finishShowExp)
            {
                string ss = curLevel.ToString() ;
                /*if(canLevelUpNum !=0)
                {
                    ss += "(+" + canLevelUpNum+ ")";				
                }
                ss += "/";
                ss += maxLevel.ToString();
                if(canBreakNum != 0)
                {
                    ss += "(+" + (canBreakNum*10).ToString() + ")";
                }*/
                if (canLevelUpNum != 0)
                {
                    ss = ss + " +" + canLevelUpNum.ToString();
                }
                levelValueText.text = ss + " / " + maxLevel.ToString();
				intensifyExp.maxLevel = maxLevel;
				curMaxLevel = maxLevel;
				
				GameHelper.setCardAttr(dbc.dataId,dbc,dbc.lv,dbc.bn,c2DHeroATKLabel,c2DHeroDEFLabel,c2DHeroHPLabel,dbc.lv+canLevelUpNum,-1);
            }


        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_SKILL)
        {
            if (targetData == null)
                return;
            PackElement dbs = targetData;

            useCardNumText.text = consumeDataList.Count.ToString() + " / 8";
            if (consumeDataList.Count == maxConsumeCardCount)
            {
                useCardNumText.color = Color.red;
            }
            else
            {
                useCardNumText.color = Color.white;
            }
            if (dbs != null)
            {
                SkillData sd = SkillData.getData(dbs.dataId);
				if(sd == null)
					return;
                // skill card ctrl info
				card2DInfo.clear();
				card2DInfo.setSimpleCardInfo(dbs.dataId,GameHelper.E_CardType.E_Skill,dbs);
                card2DNameLabel.text = sd.name;
				c2DOtherDescLabel.text = Statics.getSkillValueForUIShow(sd.index, dbs.lv);

                if (!notSetExp)
                {
                    if (needShowResurlt)
                    {
                        intensifyExp.setData(STATE.EXP_TYPE_MOVE_SKILL, oldExp, oldLevel, dbs.curExp, dbs.lv, sd.star, dbs);
                    }
                    else
                    {
                        intensifyExp.setData(STATE.EXP_TYPE_STATIC_SKILL, oldExp, oldLevel, dbs.curExp, dbs.lv, sd.star, dbs);
                    }
                }
                int canLevelUpNum = 0;
                if (sd.level >= 5)
                {
                    // to do
                    //maxLevelCtrl.SetActive(true);
                    isMaxLevelAndBreakNum = true;
                    //canEarnExpCtrl.SetActive(false);
                }
                else
                {
                    //maxLevelCtrl.SetActive(false);
                    //canEarnExpCtrl.SetActive(true);
                    //int nextLevelSkillId = dbs.dataId + 1;
                    int canEarnExpValue = 0;
                    moneyCost = 0;
                    for (int i = 0; i < consumeDataList.Count; ++i)
                    {
                        PackElement cdbs = consumeDataList[i];
                        if (cdbs != null)
                        {
                            SkillData csd = SkillData.getData(cdbs.dataId);
                            if (csd != null)
                            {
                                moneyCost += SkillExpData.getCost(dbs.lv);
                                if (csd.exptype == 1)
                                {
                                    int cardExp = csd.exp;
                                    int tempExp = cdbs.curExp;
                                    for (int j = cdbs.lv; j > 0; --j)
                                    {
                                        tempExp += SkillExpData.getExp(j, csd.star);
                                    }
                                    int exp = cardExp + tempExp;
                                    canEarnExpValue += exp;
                                }
                                else if (csd.exptype == 2)
                                {
                                    canEarnExpValue += csd.exp;
                                }
                            }
                        }
                    }
                    //canEarnValueText.text = canEarnExpValue.ToString();
                    costValueText.text = moneyCost.ToString();
                    int totalExp = canEarnExpValue;

                    //int curExp = dbs.curExp;
                    int curLevel = dbs.lv;

                    while (totalExp > 0)
                    {
                        if (curLevel >= 100)
                        {
                            totalExp = 0;
                        }
                        else
                        {
                            totalExp -= SkillExpData.getExp(curLevel + 1, sd.star);//SkillData.getData(dbs.dataId+canLevelUpNum+1).exp - curExp;
                            if (totalExp >= 0)
                            {
                                canLevelUpNum++;
                                curLevel++;
                            }
                        }
                    }
                }

                /*if(canLevelUpNum == 0)
                {
                    levelValueText.text = sd.level.ToString() + "/100" ;
                }
                else
                {
                    string ss= string.Empty;
                    ss+= sd.level.ToString();
                    ss+= "(+";
                    ss+=canLevelUpNum.ToString();
                    ss+=")/100";
                    levelValueText.text = ss;
                }*/
                if (intensifyExp.finishShowExp)
                {
                    string ss = dbs.lv.ToString() ;
                    if (canLevelUpNum != 0)
                    {
						if( (dbs.lv +canLevelUpNum) > PlayerInfo.getInstance().player.level)
						{
							canLevelUpNum = PlayerInfo.getInstance().player.level - dbs.lv;
						}
                        ss = ss + " +" + canLevelUpNum.ToString() ;
                    }
                    levelValueText.text = ss+ " / " + PlayerInfo.getInstance().player.level;
                }
				intensifyExp.maxLevel = PlayerInfo.getInstance().player.level;
				curMaxLevel = PlayerInfo.getInstance().player.level;
            }
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_PASSIVESKILL)
        {
            if (targetData == null)
                return;
            PackElement dbps = targetData;

            useCardNumText.text = "";// consumeDataList.Count.ToString() + " / 8";
            /*if(consumeDataList.Count == maxConsumeCardCount)
            {
                useCardNumText.color = Color.red;
            }
            else
            {
                useCardNumText.color = Color.white;
            }*/

            if (dbps != null)
            {
                PassiveSkillData psd = PassiveSkillData.getData(dbps.dataId);
                // skill card ctrl info
				if(psd == null)
					return;
				card2DInfo.clear();
				card2DInfo.setSimpleCardInfo(dbps.dataId,GameHelper.E_CardType.E_PassiveSkill,dbps);
                card2DNameLabel.text = psd.name;
				c2DOtherDescLabel.text = psd.describe;

                // TODO
                if (!notSetExp)
                {
                    if (needShowResurlt)
                    {
                        intensifyExp.setData(STATE.EXP_TYPE_MOVE_PASSIVESKILL, oldExp, oldLevel, dbps.curExp, psd.level, 0, dbps);
                    }
                    else
                    {
                        intensifyExp.setData(STATE.EXP_TYPE_STATIC_PASSIVESKILL, oldExp, oldLevel, dbps.curExp, psd.level, 0, dbps);
                    }
                }
                int canLevelUpNum = 0;
                if (psd.level >= 10)
                {
                    // to do
                    //maxLevelCtrl.SetActive(true);
                    isMaxLevelAndBreakNum = true;
                    //canEarnExpCtrl.SetActive(false);
                }
                else
                {
                    //maxLevelCtrl.SetActive(false);
                    //canEarnExpCtrl.SetActive(true);
                    //int nextLevelSkillId = dbps.dataId + 1;
                    int canEarnExpValue = 0;
                    for (int i = 0; i < consumeDataList.Count; ++i)
                    {
                        PackElement cdbps = consumeDataList[i];
                        if (cdbps != null)
                        {
                            PassiveSkillData cpsd = PassiveSkillData.getData(cdbps.dataId);
                            if (cpsd != null)
                            {
                                PassiveSkillBasicExpData psbed = PassiveSkillBasicExpData.getData(cpsd.star);
                                if (psbed != null)
                                {
                                    int tempExp = cdbps.curExp;
                                    for (int j = cpsd.level; j > 0; --j)
                                    {
                                        tempExp += PassiveSkillData.getData(cdbps.dataId - (cpsd.level - j)).exp;
                                    }
                                    int exp = cdbps.curExp + psbed.basicexp + tempExp;
                                    canEarnExpValue += exp;
                                }
                            }
                        }
                    }
                    //canEarnValueText.text = canEarnExpValue.ToString();
                    int totalExp = canEarnExpValue;

                    int curExp = dbps.curExp;
                    int curLevel = psd.level;

                    while (totalExp > 0)
                    {
                        if (curLevel >= 10)
                        {
                            totalExp = 0;
                        }
                        else
                        {
                            totalExp -= PassiveSkillData.getData(dbps.dataId + canLevelUpNum + 1).exp - curExp;
                            if (totalExp >= 0)
                            {
                                canLevelUpNum++;
                                curLevel++;
                            }
                        }
                    }
                }
                if (intensifyExp.finishShowExp)
                {
                    string ss = psd.level.ToString()  ;
                    if (canLevelUpNum != 0)
                    {
                        ss = ss + " +" + canLevelUpNum.ToString() ;
                    }
                    levelValueText.text = ss + " / 10";
                }
				intensifyExp.maxLevel = 10;
				curMaxLevel = 10;

                /*if(canLevelUpNum == 0)
                {
                    levelValueText.text = psd.level.ToString() + "/10" ;
                }
                else
                {
                    string ss= string.Empty;
                    ss+= psd.level.ToString();
                    ss+= "(+";
                    ss+=canLevelUpNum.ToString();
                    ss+=")/10";
                    levelValueText.text = ss;
					
                }*/

            }
        }
        else if (mIntensifyType == INTENSIFYTYPE.TYPE_EQUIP)
        {
            if (targetData == null)
                return;
            EquipData ed = EquipData.getData(targetData.dataId);
            if (ed != null)
            {
				equipCardInfo.setSimpleCardInfo(targetData.dataId,GameHelper.E_CardType.E_Equip,targetData,targetData.lv);
				equipDescLabel.text = GameHelper.getEquipAttrDescText(targetData);
                
                int playerLevel = PlayerInfo.getInstance().player.level;
                EquipUpgradeData eqd = EquipUpgradeData.getData(targetData.lv);
                if (eqd != null)
                {
                    moneyCost = eqd.cost;
                    costValueText.text = moneyCost.ToString();
                }
                if (targetData.lv >= playerLevel * 3)
                {
                    equipLevelMax = true;
                }
				equipIntensifyCount.text = targetData.lv.ToString() + "/" + (playerLevel * 3).ToString();
				UILabel parentLabel = equipIntensifyCount.transform.parent.gameObject.GetComponent<UILabel>();
				if(equipLevelMax)
				{
					parentLabel.color = new Color(1,0,0,1);
					equipIntensifyCount.color = new Color(1,0,0,1);
				}
				else
				{
					parentLabel.color = new Color(1,1,1,1);
					equipIntensifyCount.color = new Color(1,1,1,1);
				}
                if (needShowResurlt)
                {
                    bool success = false;
                    switch (intensifyResult)
                    {
                    case -1:		//失败//
                    {
						GameObjectUtil.destroyGameObjectAllChildrens(UISceneEffectNodeManager.mInstance.intensifyEquipSuccessEffectNode);
						Resources.UnloadUnusedAssets();
						
                        //播放音效//
                        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_FAIL);
                        equipIntensifyResultText.text = TextsData.getData(40).chinese;
                        equipIntensifyResultSprite.gameObject.SetActive(true);
                        equipIntensifyResultSpriteCrit.gameObject.SetActive(false);
                        equipIntensifyResultSprite.spriteName = "intensify_fail";
						GameObjectUtil.playForwardUITweener(equipIntensifyResultSprite.gameObject.GetComponent<TweenScale>());
						GameObjectUtil.playForwardUITweener(equipIntensifyResultSprite.gameObject.GetComponent<TweenAlpha>());
						GameObjectUtil.playForwardUITweener(equipIntensifyResultSprite.gameObject.GetComponent<TweenPosition>());
						
                    } break;
                    case 0:			//成功//
                    {
                        success = true;
                        string lv;
						Invoke("showIntensifySuccessText",0.5f);
						if(intensifyEquip10Times)
						{
							  if (intensifyResultPE != null)
	                        {
	                            lv = TextsData.getData(41).chinese.Replace("1", (intensifyResultPE.lv - curlevel).ToString());
	                            equipIntensifyResultText.text = lv;
	                        }
	                        else
	                        {
	                            equipIntensifyResultText.text = "";
	                        }
						}
						else
						{
							intensifyEquip10Times = false;
							equipIntensifyResultText.text = TextsData.getData(41).chinese;
						}
						
						GameObjectUtil.destroyGameObjectAllChildrens(UISceneEffectNodeManager.mInstance.intensifyEquipSuccessEffectNode);
						Resources.UnloadUnusedAssets();
						GameObject effectObj = GameObject.Instantiate(equipUpgradeEffectPrefab) as GameObject;
                        GameObjectUtil.gameObjectAttachToParent(effectObj, UISceneEffectNodeManager.mInstance.intensifyEquipSuccessEffectNode);
                        GameObjectUtil.setGameObjectLayer(effectObj, STATE.LAYER_ID_UIEFFECT);
                        effectObj.transform.localPosition = new Vector3(effectObj.transform.localPosition.x, effectObj.transform.localPosition.y, -300);
                    } break;
                    case 1:			//成功//
                    {
                        success = true;
						Invoke("showIntensifySuccessCritText",0.5f);
						equipIntensifyResultText.text = TextsData.getData(42).chinese;
						
						GameObjectUtil.destroyGameObjectAllChildrens(UISceneEffectNodeManager.mInstance.intensifyEquipSuccessEffectNode);
						Resources.UnloadUnusedAssets();
                        GameObject effectObj = GameObject.Instantiate(equipUpgradeEffectPrefab) as GameObject;
                        GameObjectUtil.gameObjectAttachToParent(effectObj, UISceneEffectNodeManager.mInstance.intensifyEquipSuccessEffectNode);
                        GameObjectUtil.setGameObjectLayer(effectObj, STATE.LAYER_ID_UIEFFECT);
                        effectObj.transform.localPosition = new Vector3(effectObj.transform.localPosition.x, effectObj.transform.localPosition.y, -300);

                    } break;
                    }
                    if (success)
                    {
						upInfoText.text = (GameHelper.getEquipAttrNum(targetData.dataId,targetData.lv) - GameHelper.getEquipAttrNum(targetData.dataId,lastEquipLevel)).ToString();
						upInfoObj.SetActive(true);
                        //播放音效//
                        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SUCC);
                       	GameObjectUtil.playForwardUITweener(upInfoText.gameObject.GetComponent<TweenScale>());
                    }
                }
                else
                {
                    equipIntensifyResultText.text = string.Empty;
                }
            }
        }
    }
	
	void showIntensifySuccessText()
	{
		equipIntensifyResultSprite.gameObject.SetActive(true);
        equipIntensifyResultSpriteCrit.gameObject.SetActive(false);
        equipIntensifyResultSprite.spriteName = "intensify_success";
        GameObjectUtil.playForwardUITweener(equipIntensifyResultSprite.gameObject.GetComponent<TweenScale>());
		GameObjectUtil.playForwardUITweener(equipIntensifyResultSprite.gameObject.GetComponent<TweenAlpha>());
		GameObjectUtil.playForwardUITweener(equipIntensifyResultSprite.gameObject.GetComponent<TweenPosition>());
	}
	
	void showIntensifySuccessCritText()
	{
        
        equipIntensifyResultSprite.gameObject.SetActive(false);
        equipIntensifyResultSpriteCrit.gameObject.SetActive(true);
        GameObjectUtil.playForwardUITweener(equipIntensifyResultSpriteCrit.gameObject.GetComponent<TweenScale>());
		GameObjectUtil.playForwardUITweener(equipIntensifyResultSpriteCrit.gameObject.GetComponent<TweenAlpha>());
		GameObjectUtil.playForwardUITweener(equipIntensifyResultSpriteCrit.gameObject.GetComponent<TweenPosition>());
        equipIntensifyResultSpriteCrit.spriteName = "intensify_success_crit";
	}

    public bool checkCardIsSelectInPack(PackElement data)
    {
        bool result = false;
        if (data == null)
        {
            return result;
        }
        for (int i = 0; i < consumeDataList.Count; ++i)
        {
            PackElement cpe = consumeDataList[i];

            if (cpe.i == data.i)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    public void setCardItemDisplay(ListCardInfo cardInfo,int fromId = 0)
    {
        if (cardInfo.data == null)
        {
            cardInfo.clear();
            return;
        }
        if (cardInfo.data.use == 0)
        {
            cardInfo.usedObj.SetActive(false);
        }
        else if (cardInfo.data.use == 1)
        {
            cardInfo.usedObj.SetActive(true);
        } 
		GameHelper.E_CardType ct = GameHelper.E_CardType.E_Null;
        switch (mIntensifyType)
        {
        case INTENSIFYTYPE.TYPE_HERO:
        {
			ct = GameHelper.E_CardType.E_Hero;
	    } break;
        case INTENSIFYTYPE.TYPE_SKILL:
        {
			ct = GameHelper.E_CardType.E_Skill;
        } break;
        case INTENSIFYTYPE.TYPE_PASSIVESKILL:
        {
			ct = GameHelper.E_CardType.E_PassiveSkill;
        } break;
        case INTENSIFYTYPE.TYPE_EQUIP:
        {
			ct = GameHelper.E_CardType.E_Equip;
        } break;
        }
		cardInfo.sInfo.setCardUserInfo(fromId);
		cardInfo.sInfo.setSimpleCardInfo(cardInfo.data.dataId,ct,cardInfo.data);
		
		cardInfo.sInfo.bm.gameObject.name = cardInfo.data.i.ToString();
        cardInfo.sInfo.bm.param = cardInfo.data.i;
    }

    public void showSortLabelCtrl()
    {
        if (mSortType == INTENSIFYSORTTYPE.SORT_ALL)
        {
            sortAllCtrl.SetActive(false);
            sortRaceCtrl.SetActive(true);
        }
        else if (mSortType == INTENSIFYSORTTYPE.SORT_RACE)
        {
            sortAllCtrl.SetActive(true);
            sortRaceCtrl.SetActive(false);
        }
    }

    /**处理返回信息**/
    public void receiveResponse(string json)
    {
		
		//关闭连接界面的动画//
		PlayerInfo.getInstance().isShowConnectObj = false;
        if(json != null)
        {
           // isReceiveData = true;
            switch (requestType)
            {
            case 1:
            {
                PackResultJson prj = JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
				allFromIdList = prj.pejs;
                receiveData = true;
            } break;
            case 2:
            {
                SelectCardResultJson prj = JsonMapper.ToObject<SelectCardResultJson>(json);
				errorCode = prj.errorCode;
                allCells = prj.pes;
                selectCardSkillID = prj.i;
                receiveData = true;
                if (needRequestPlayerInfo)
                {
                    needShowResurlt = false;
					needRequestPlayerInfo = false;
                    doRequestPlayerInfo = true;
                }
            } break;
            case 3:
            {
                intensifyResult = -1;
                intensifyResultPE = null;
                needShowResurlt = true;
                IntensifyResultJosn irj = JsonMapper.ToObject<IntensifyResultJosn>(json);
                intensifyResult = irj.state;
                intensifyResultPE = irj.pe;
                errorCode = irj.errorCode;
				needRTime = 0;
				receiveData = true;
            } break;
            case 10:
            {
                ImaginationResultJson irj = JsonMapper.ToObject<ImaginationResultJson>(json);
                errorCode = irj.errorCode;
                if (errorCode == 0)
                {
                    curNpcId = irj.id;
                    nullPackNum = irj.i;

                    mid = irj.mid;
                    mnum = irj.mnum;
                    PlayerInfo.getInstance().player.gold = irj.g;
                }
                receiveData = true;
            } break;
			}
        }
    }
	
    public void notifyLevelUp(int level,int maxLevel)
    {
		if(targetData == null)
			return;
		curLevel = level;
        GameObject levelUpEffectObj = GameObject.Instantiate(expUpgradeEffectPrefab) as GameObject;
        GameObjectUtil.gameObjectAttachToParent(levelUpEffectObj, levelUpEffectNode);
        levelValueText.text = level.ToString() +"/" + maxLevel.ToString();
        GameObjectUtil.playForwardUITweener(levelValueText.gameObject.GetComponent<TweenScale>());
		
		if(mIntensifyType == INTENSIFYTYPE.TYPE_HERO)
		{
			notifyCardAttr(level);
		}
		else
		{
			//notifyCardOtherDesc(level);
		}

        GameObject.DestroyObject(levelUpEffectObj, 5);
    }
	
	public void notifyCardAttr(int level)
	{
		if(targetData == null)
			return;
		if(c2DHeroATKLabel  == null || c2DHeroDEFLabel == null || c2DHeroHPLabel == null)
			return;
		GameHelper.setCardAttr(targetData.dataId,null,level,targetData.bn,c2DHeroATKLabel,c2DHeroDEFLabel,c2DHeroHPLabel);
		GameObjectUtil.playForwardUITweener(c2DHeroATKLabel.gameObject.GetComponent<TweenScale>());
		GameObjectUtil.playForwardUITweener(c2DHeroDEFLabel.gameObject.GetComponent<TweenScale>());
		GameObjectUtil.playForwardUITweener(c2DHeroHPLabel.gameObject.GetComponent<TweenScale>());
	}
	
	public void notifyCardOtherDesc(int level)
	{
		if(mIntensifyType == INTENSIFYTYPE.TYPE_SKILL)
		{
			SkillData sd = SkillData.getData(targetData.dataId);
			if(sd == null)
				return;
			
			c2DOtherDescLabel.text = Statics.getSkillValueForUIShow(sd.index, level);
			GameObjectUtil.playForwardUITweener(c2DOtherDescLabel.gameObject.GetComponent<TweenScale>());
		}
		else if(mIntensifyType == INTENSIFYTYPE.TYPE_PASSIVESKILL)
		{
			PassiveSkillData psd = PassiveSkillData.getData(targetData.dataId);
			if(psd == null)
				return;
			c2DOtherDescLabel.text = psd.describe;
		}
	}
}
