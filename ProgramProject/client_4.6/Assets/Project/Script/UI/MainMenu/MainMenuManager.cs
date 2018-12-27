using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour, ProcessResponse
{

    //	public static MainMenuManager mInstance;
    //	public GameObject mainMenuBg;
    public GameObject canCloseBtns;
    public GameObject models;
    //显示3d数据的根节点//
    public GameObject Model3dRoot;
    //3d模型的碰撞框//
    public GameObject Model3dBoxCliders;
    //	public List<GameObject> btnList;
    public List<GameObject> modelPosList;
    //界面上的所有的按钮的父节点//
    public Transform BtnsParent;

    //3dCamera//
    public GameObject CameraObj;
    //	bool needChange = false;
    //	int clickPage= 0;			//clickType: 0 军团(即主菜单界面)， 1卡组， 2 商城, 3 other, 4 战斗//


    public bool isCanClick = false;

    public float xSpeed = 1;
    public float xBegin = 190, xEnd = 226;
    public float x = 0;
    public float y = 0;
    float distance = 16.5f;
    public GameObject cameraLookAtObj;
    private Vector3 initEulerAngles;
    private Vector3 initPosition;
    private Quaternion initRotation;
    private int curEnterType = STATE.ENTER_MAINMENU_FRIST;
    //出战的卡牌//
    private List<int> battleCardIds;
    //获取各个模块是否有更新//
    private List<string> modularUpdateData;
    //用于存储每个按钮的tipMark按钮//
    private Hashtable BtnTipMark;
    /*1 获得界面信息（即出战卡牌信息）， 2 召唤（抽卡）， 3 PVE战斗, 4 成就， 5 阵容（卡组）， 6背包, 7 强化（吞噬）, 8 合成
	 *9 符文， 10 好友 ,11 sign , 12 break,	13 邮箱,	14活动 , 15充值时获取玩家当前信息, 16充值特权时获取玩家当前信息
	 *17 冥想, 18 商城, 19任务, 21 合体技
	 */
    private int requestType;
    private int errorCode;
    private bool receiveData;
    //存储模型的list//
    //	private List<GameObject> cardList;
    //存储模型的animtor//
    private List<Animator> animList;
    //成就界面的json//
    private AchieveResultJson achiveRJ;
    //邮件界面的json//
    private MailUIResultJson mailRJ;
    //签到的json//
    private SignResultJson signRJ;
    //好友的json//
    private FriendResultJson friendRJ;
    //突破的json//
    private BreakResultJson breakRJ;
    //符文的json//
    private RuneResultJson runeRJ;
    //卡组界面的json//
    private CardGroupResultJson cardGroupRJ;
    //总动界面的json//
    private ActivityResultJson activeRJ;

    //充值界面的json//
    private RechargeUiResultJson rechargeRJ;
    //推图界面的json//
    private MapResultJson mapRJ;
    //抽卡界面的json//
    private LotResultJson lotRJ;
    //背包界面的json//
    private PackResultJson packRJ;

    //礼包界面的json//
    public GiftRewardResultJson gtRJ;

    //冥想界面json//
    private ImaginationResultJson spriteRJ;

    //商店json//
    private ShopResultJson shopRJ;

    //任务界面json//
    private TaskResultJson taskRJ;

    //竞技场//
    private RankResultJson rankJson;

    public GiftUI gifts;



    public int nextGift = 1;

    public int curTime_h;

    public int curTime_m;

    public int curTime_s;

    public int updateTime;

    public int giftId;

    public int onlineTime;

    public GameObject Gift;

    bool isBeginCount = true;

    public int markId;

    public int type;

    //是否充值过？0表示没有,>0表示之前充值过//
    public int cost;

    private UISprite czBg;

    string firstCZSpriteName = "icon-sc";
    string nFirstCZSpriteName = "icon-cz";

    public List<GameObject> objIcons = new List<GameObject>();
    Dictionary<int, GameObject> objIconsMark = new Dictionary<int, GameObject>();
    public List<GameObject> objIconsRow = new List<GameObject>();
    public List<GameObject> objIconsCol = new List<GameObject>();
    public GameObject objIconPoint;
    public GameObject objIconPointMark;
    public int objPosLen = 80;
    List<GameObject> objIconsRowNow = new List<GameObject>();
    List<GameObject> objIconsColNow = new List<GameObject>();
    private static float TIME_ICON_MOVE = 0.1f;
    private static float SCALE_ICON_TO = 0.7f;

    public List<GameObject> particleObjs = new List<GameObject>();

    ComposeResultJson _crj;

    bool isNeedShowUnitSkill = false;

    public NewUnitSkillResultJson nusrj;

    public GameObject btnCornucopia;

    void Awake()
    {
        //		mInstance = this;
        //		_MyObj = mInstance.gameObject;
        isNeedShowUnitSkill = false;
        isCanClick = false;
        init();
        show();
        //int sceneID = Application.loadedLevel;

        czBg = transform.FindChild("AlwaysShowBtn2/but21/icon").GetComponent<UISprite>();
        if (PlayerInfo.isFirstLogin)
        {
            PlayerInfo.isFirstLogin = false;
            SendToGetData();
        }
    }

    public void init()
    {
        animList = new List<Animator>();
        modularUpdateData = new List<string>();
        battleCardIds = new List<int>();
        BtnTipMark = new Hashtable();
    }

    //根据服务器下发的数据创建模型//
    private void initData()
    {
        animList.Clear();
        //		return ;
        GameObjectUtil.destroyGameObjectAllChildrens(models);

        GameObject shadowPrefab = Resources.Load("Prefabs/Item/shadow") as GameObject;
        for (int i = 0; i < battleCardIds.Count; i++)
        {
            if (i < modelPosList.Count && battleCardIds[i] > 0)
            {
                CardData cd = CardData.getData(battleCardIds[i]);
                GameObject card = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel, 0)) as GameObject;
                card.transform.parent = models.transform;
                GameObjectUtil.setGameObjectLayer(card, models.layer);
                card.transform.position = modelPosList[i].transform.position;
                card.transform.localRotation = modelPosList[i].transform.localRotation;
                card.transform.localScale = new Vector3(card.transform.localScale.x * 1.35f, card.transform.localScale.y * 1.35f, card.transform.localScale.z * 1.35f);
                GameObject shadowObj = GameObject.Instantiate(shadowPrefab) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(shadowObj, card.transform.FindChild("foot").gameObject);
                shadowObj.transform.localScale = new Vector3(1.3f, 1, 1.3f);

                Animator anim = card.GetComponent<Animator>();
                animList.Add(anim);
            }
            else if (battleCardIds[i] <= 0)
            {
                animList.Add(null);
            }
        }

        //for(int i = 0;modularUpdateData!=null && i < modularUpdateData.Count;i++)
        //{
        //    string str = modularUpdateData[i];
        //    string[] ss = str.Split('-');
        //    int id = StringUtil.getInt( ss[0]);
        //    int mark = StringUtil.getInt( ss[1]);		//是否有更新，需要提示//
        //    if (!BtnTipMark.ContainsKey(id)) continue;
        //    GameObject tipMark = (GameObject)BtnTipMark[id];
        //    int lockMark = PlayerInfo.getInstance().getUnLockData(id);


        //    if(lockMark == 0)
        //    {
        //        tipMark.SetActive(false);
        //    }
        //    else if(lockMark == 1)
        //    {
        //        if(mark == 0)
        //        {

        //            if (id == 21)
        //            {
        //                markId = mark;
        //            }
        //            tipMark.SetActive(false);
        //        }
        //        else
        //        {
        //            if (id == 21)
        //            {
        //                markId = mark;
        //            }
        //            tipMark.SetActive(true);
        //            tipMark.GetComponent<UISprite>().spriteName = "tip_mark_" + mark.ToString();
        //        }
        //    }
        //}

        for (int i = 0; i < modularUpdateData.Count; i++)
        {
            string[] str = modularUpdateData[i].Split('-');
            int id = int.Parse(str[0]);
            int mark = int.Parse(str[1]);

            GameObject objMark = objIconsMark[id];
            int markValue = PlayerInfo.getInstance().getUnLockData(id);
            if (markValue == 1)//已解锁//
            {
                if (mark == 0)
                {
                    objMark.SetActive(false);
                }
                else
                {
                    objMark.SetActive(true);
                    objMark.GetComponent<UISprite>().spriteName = "tip_mark_" + mark.ToString();
                }

                if (id == 21)
                    markId = mark;
            }
        }
        refreshIconPointMark();

        if (cost == 0)
        {
            czBg.spriteName = firstCZSpriteName;
        }
        else
        {
            czBg.spriteName = nFirstCZSpriteName;
        }

    }
    // Use this for initialization
    void Start()
    {
        //RefreshGiftTime();
    }

    public void initCameraData()
    {


        //		initPosition=CameraObj.transform.position;
        //		initRotation=CameraObj.transform.rotation;

        CameraData camera = CameraData.getData(13);
        ViewPathData vpd = ViewPathData.getData(camera.path);
        iTweenPath iPath = CameraObj.GetComponent<iTweenPath>();
        iPath.nodes = vpd.nodes;
        initPosition = vpd.nodes[2];
        initRotation = CameraObj.transform.rotation;

        initEulerAngles = CameraObj.transform.eulerAngles;
        x = initEulerAngles.y;
    }

    public void MoveCamera()
    {
        isCanClick = false;
        CameraData camera = CameraData.getData(13);
        ViewPathData vpd = ViewPathData.getData(camera.path);
        iTweenPath iPath = CameraObj.GetComponent<iTweenPath>();
        iPath.nodes = vpd.nodes;
        CameraObj.transform.position = vpd.nodes[0];
        //		SetCameraPos(1);
        iTween.MoveTo(CameraObj, iTween.Hash("path", iTweenPath.GetPath(iPath.pathName), "time", 2f, "oncomplete", "MoveCameraCallBack",
                "oncompletetarget", gameObject, "easetype", iTween.EaseType.linear));

    }

    //type 1 需要移动 2， 不需要移动//
    public void SetCameraPos(int type)
    {
        CameraData camera = CameraData.getData(13);
        ViewPathData vpd = ViewPathData.getData(camera.path);
        iTweenPath iPath = CameraObj.GetComponent<iTweenPath>();
        iPath.nodes = vpd.nodes;
        if (type == 1)
        {
            CameraObj.transform.position = vpd.nodes[0];
        }
        else
        {
            CameraObj.transform.position = vpd.nodes[2];
        }
    }

    public void waitCanClickBtn()
    {
        isCanClick = true;
    }

    public void MoveCameraCallBack()
    {
        Invoke("waitCanClickBtn", 0.5f);
        initCameraData();
        // 第一次登陆 //
        if (curEnterType == STATE.ENTER_MAINMENU_FRIST)
        {
            // force guide
            if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
            {
                UISceneDialogPanel.mInstance.showDialogID(2);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam))
            {
                UISceneDialogPanel.mInstance.showDialogID(28);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_UseCombo3))
            {
                GuideUI_UseCombo3.mInstance.showStep(0);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill))
            {
                GuideUI_UintSkill.mInstance.showStep(0);
            }

            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
            {
                GuideUI_Bounes.mInstance.showStep(0);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_KO_Exchange))
            {
                if (!GuideUI7_KOExchange.mInstance.finishExchange)
                {
                    GuideUI7_KOExchange.mInstance.showStep(0);
                }
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam2))
            {
                GuideUI_CardInTeam2.mInstance.showStep(0);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ChangePlayerName))
            {
                UISceneDialogPanel.mInstance.showDialogID(46);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard2))
            {
                if (GuideUI8_Achievement.mInstance.runningStep == -1)
                {
                    UISceneDialogPanel.mInstance.showDialogID(31);
                }
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
            {
                GuideUI_IntensifyEquip.mInstance.normalType = true;
                UISceneDialogPanel.mInstance.showDialogID(44);
            }

            // unlock guide
            int curGuideID = GuideManager.getInstance().getCurrentGuideID();
            switch (curGuideID)
            {
                case (int)GuideManager.GuideType.E_IntensifyCard:
                    {
                        if (GuideUI_Intesnify.mInstance.runningStep == -1)
                        {
                            if (RequestUnlockManager.mInstance.isCanUnlockFunctionByFinishMissionID((int)RequestUnlockManager.MODELID.E_Intensify))
                            {
                                RequestUnlockManager.mInstance.showUnlockPanel((int)RequestUnlockManager.MODELID.E_Intensify);
                                GuideManager.getInstance().runGuide();
                            }
                        }
                        else if (GuideUI_Intesnify.mInstance.runningStep == 6)
                        {
                            UISceneDialogPanel.mInstance.showDialogID(10);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Achievement:
                    {
                        if (RequestUnlockManager.mInstance.isCanUnlockFunctionByFinishMissionID((int)RequestUnlockManager.MODELID.E_Achievement))
                        {
                            RequestUnlockManager.mInstance.showUnlockPanel((int)RequestUnlockManager.MODELID.E_Achievement);
                            GuideManager.getInstance().runGuide();
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Equip:
                    {
                        if (PlayerInfo.getInstance().player.missionId >= GuideManager.getInstance().finishGiveEquipMissionID)
                        {
                            GuideManager.getInstance().runGuide();
                            GuideUI12_Equip.mInstance.needRunStep = 2;
                            UISceneDialogPanel.mInstance.showDialogID(27);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_UnlockCompose:
                    {
                        if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Compose))
                        {
                            RequestUnlockManager.mInstance.requestUnlockMsg();
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Compose:
                    {
                        GuideManager.getInstance().runGuide();
                        UISceneDialogPanel.mInstance.showDialogID(14);
                    }
                    break;
                case (int)GuideManager.GuideType.E_ActiveCopy:
                    {
                        if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_ActiveCopy))
                        {
                            RequestUnlockManager.mInstance.requestUnlockMsg();
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_WarpSpace:
                    {
                        if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_WarpSpace))
                        {
                            RequestUnlockManager.mInstance.requestUnlockMsg();
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Skill:
                    {
                        GuideManager.getInstance().runGuide();
                        if (GuideUI18_Skill.mInstance.runningStep == -1)
                        {
                            UISceneDialogPanel.mInstance.showDialogID(33);
                            GuideUI18_Skill.mInstance.isOffLine = true;
                        }
                        else
                        {
                            GuideUI18_Skill.mInstance.showStep(5);
                        }

                    }
                    break;
                case (int)GuideManager.GuideType.E_PVP:
                    {
                        if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_PVP))
                        {
                            RequestUnlockManager.mInstance.requestUnlockMsg();
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Rune:
                    {
                        if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Rune))
                        {
                            RequestUnlockManager.mInstance.requestUnlockMsg();
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_UnlockBreak:
                    {
                        if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Break))
                        {
                            RequestUnlockManager.mInstance.requestUnlockMsg();
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Break:
                    {
                        GuideManager.getInstance().runGuide();
                        UISceneDialogPanel.mInstance.showDialogID(11);
                    }
                    break;
                case (int)GuideManager.GuideType.E_Spirit:
                    {
                        if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Spirit))
                        {
                            RequestUnlockManager.mInstance.requestUnlockMsg();
                        }
                    }
                    break;

            }
        }
        // 从其他界面切到营地 //
        else if (curEnterType == STATE.ENTER_MAINMENU_BACK)
        {
            if (isNeedShowUnitSkill)
                return;
            //else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
            //{
            //	UISceneDialogPanel.mInstance.showDialogID(2);
            //}
            if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam))
            {
                UISceneDialogPanel.mInstance.showDialogID(28);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill))
            {
                GuideUI_UintSkill.mInstance.showStep(0);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyCard))
            {
                if (GuideUI_Intesnify.mInstance.runningStep == -1)
                {
                    if (RequestUnlockManager.mInstance.isCanUnlockFunctionByFinishMissionID((int)RequestUnlockManager.MODELID.E_Intensify))
                    {
                        UISceneDialogPanel.mInstance.showDialogID(4);
                    }
                }
                else if (GuideUI_Intesnify.mInstance.runningStep == 6)
                {
                    UISceneDialogPanel.mInstance.showDialogID(10);
                }
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
            {
                GuideUI_Bounes.mInstance.showStep(0);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_KO_Exchange))
            {
                if (GuideUI7_KOExchange.mInstance.finishExchange)
                {
                    //UISceneDialogPanel.mInstance.showDialogID(43);
                }
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam2))
            {
                if (GuideUI_CardInTeam2.mInstance.needShowDialog)
                {
                    UISceneDialogPanel.mInstance.showDialogID(43);
                }
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ChangePlayerName))
            {
                if (GuideUI_ChangePlayerName.mInstance.runningStep == -1)
                {
                    UISceneDialogPanel.mInstance.showDialogID(46);
                }
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard2))
            {
                UISceneDialogPanel.mInstance.showDialogID(31);
            }

            // unlock guide
            int curGuideID = GuideManager.getInstance().getCurrentGuideID();
            switch (curGuideID)
            {
                case (int)GuideManager.GuideType.E_Achievement:
                    {
                        if (GuideManager.getInstance().isGuideRunning())
                        {
                            UISceneDialogPanel.mInstance.showDialogID(25);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Equip:
                    {
                        if (PlayerInfo.getInstance().player.missionId >= GuideManager.getInstance().finishGiveEquipMissionID)
                        {
                            GuideManager.getInstance().runGuide();
                            GuideUI12_Equip.mInstance.showStep(2);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_UnlockCompose:
                    {
                        if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Compose))
                        {
                            RequestUnlockManager.mInstance.requestUnlockMsg();
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Compose:
                    {
                        if (GuideManager.getInstance().isGuideRunning())
                        {
                            UISceneDialogPanel.mInstance.showDialogID(14);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_ActiveCopy:
                    {
                        if (GuideManager.getInstance().isGuideRunning())
                        {
                            GuideUI16_ActiveCopy.mInstance.showStep(0);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_WarpSpace:
                    {
                        if (GuideManager.getInstance().isGuideRunning())
                        {
                            UISceneDialogPanel.mInstance.showDialogID(19);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Skill:
                    {
                        if (GuideManager.getInstance().isGuideRunning() && GuideUI18_Skill.mInstance.runningStep != -1)
                        {
                            GuideUI18_Skill.mInstance.showStep(5);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_PVP:
                    {
                        if (GuideManager.getInstance().isGuideRunning())
                        {
                            UISceneDialogPanel.mInstance.showDialogID(15);
                            //GuideUI19_PVP.mInstance.showStep(0);
                        }
                    }
                    break;
                case (int)GuideManager.GuideType.E_Rune:
                    {
                        if (GuideManager.getInstance().isGuideRunning())
                        {
                            UISceneDialogPanel.mInstance.showDialogID(23);
                        }
                        else
                        {
                            if (RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Rune))
                            {
                                RequestUnlockManager.mInstance.requestUnlockMsg();
                            }
                        }

                    }
                    break;
                case (int)GuideManager.GuideType.E_Break:
                    {
                        GuideManager.getInstance().runGuide();
                        UISceneDialogPanel.mInstance.showDialogID(11);
                    }
                    break;
                case (int)GuideManager.GuideType.E_Spirit:
                    {
                        if (GuideManager.getInstance().isGuideRunning())
                        {
                            UISceneDialogPanel.mInstance.showDialogID(35);
                        }
                    }
                    break;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (receiveData)
        {
            receiveData = false;

            if (errorCode == -3)
            {
                return;
            }
            switch (requestType)
            {
                case 1:             //获得界面信息（出战卡牌信息）//

                    initData();
                    //移动摄像机//
                    if (curEnterType == STATE.ENTER_MAINMENU_FRIST && PlayerInfo.getInstance().BattleOverBackType == 0)
                    {
                        MoveCamera();
                    }
                    else if (curEnterType == STATE.ENTER_MAINMENU_BACK || PlayerInfo.getInstance().BattleOverBackType != 0)
                    {
                        MoveCameraCallBack();
                        //					HeadUI.mInstance.requestPlayerInfo();
                    }
                    if (type == 0)
                    {
                        btnCornucopia.SetActive(false);
                    }
                    else
                    {
                        btnCornucopia.SetActive(true);
                    }
                    //更新头像信息//

                    //打开主界面后如果是查看新合体技则打开合体技界面//
                    if (isNeedShowUnitSkill)
                    {
                        OpenUniteSkillPanel();
                    }
                    else
                    {
                        //向服务器请求判断是否有新解锁合体技//
                        {
                            requestType = 23;
                            PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
                        }
                    }
                    break;
                case 2:
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
                    {
                        GuideUI_GetCard.mInstance.showStep(1);
                    }
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard2))
                    {
                        GuideUI9_GetCard2.mInstance.hideAllStep();
                        GuideManager.getInstance().onlyShowLotCardPointer = true;
                        GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_GetCard2);
                        //GuideUI9_GetCard2.mInstance.showStep(1);
                    }

                    //				LotCardUI.mInstance.lrj=lrj;
                    //				LotCardUI.mInstance.show();

                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT);
                    LotCardUI lotCard = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT, "LotCardUI") as LotCardUI;
                    lotCard.lrj = lotRJ;
                    lotCard.freeTimes = lotRJ.t;
                    lotCard.show();

                    //				hide();
                    //隐藏主城//
                    //				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                    hide();

                    lotRJ = null;
                    break;
                case 3:
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
                    //				MissionUI2.mInstance.show();
                    MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
                        "MissionUI2") as MissionUI2;
                    MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI") as MissionUI;
                    mission.mrj = mapRJ;
                    mission2.show();
                    //				MissionUI.mInstance.mrj=mj;
                    //				MissionUI.mInstance.show();
                    mission.show();


                    //				hide();
                    //隐藏主城//
                    //				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                    hide();
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill))
                    {
                        GuideUI_UintSkill.mInstance.showStep(1);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
                    {
                        GuideUI_Bounes.mInstance.showStep(1);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_KO_Exchange))
                    {
                        GuideUI7_KOExchange.mInstance.hideAllStep();
                        UISceneDialogPanel.mInstance.showDialogID(40);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ActiveCopy))
                    {
                        GuideUI16_ActiveCopy.mInstance.showStep(1);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace))
                    {
                        GuideUI17_WarpSpace.mInstance.showStep(1);
                    }
                    break;
                case 4:
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Achievement))
                    {
                        GuideUI8_Achievement.mInstance.hideAllStep();
                        UISceneDialogPanel.mInstance.showDialogID(26);
                    }
                    //				AchievementPanel.mInstance.show();
                    if (errorCode == 0)
                    {

                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACHIVEMENT);
                        AchievementPanel achive = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACHIVEMENT,
                            "AchievementPanel") as AchievementPanel;
                        achive.arj = achiveRJ;
                        achive.show();
                        achiveRJ = null;
                    }
                    curEnterType = STATE.ENTER_MAINMENU_BACK;

                    break;

                case 5:         //进入卡组界面//
                                //				CombinationInterManager.mInstance.SetData(0, 1);
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
                    CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager") as CombinationInterManager;
                    combination.curCardGroup = cardGroupRJ.transformCardGroup();
                    PlayerInfo.getInstance().curCardGroup = combination.curCardGroup;
                    combination.SetData(1);

                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam))
                    {
                        GuideUI_CardInTeam.mInstance.showStep(1);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_UseCombo3))
                    {
                        GuideUI_UseCombo3.mInstance.hideAllStep();
                        UISceneDialogPanel.mInstance.showDialogID(37);
                        //GuideUI_UseCombo3.mInstance.showStep(1);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam2))
                    {
                        GuideUI_CardInTeam2.mInstance.showStep(1);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Equip))
                    {
                        GuideUI12_Equip.mInstance.showStep(3);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
                    {
                        GuideUI18_Skill.mInstance.showStep(6);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
                    {
                        GuideUI22_Break.mInstance.hideAllStep();
                        UISceneDialogPanel.mInstance.showDialogID(12);
                        // GuideUI22_Break.mInstance.showStep(1);
                    }
                    //关闭主菜单选项卡//
                    //				hide();
                    //隐藏主城//
                    //				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                    hide();
                    break;

                case 6:         //进入背包界面//
                                //				PackUI.mInstance.pejs = prj.pejs;
                                //				PackUI.mInstance.show();

                    //显示背包//
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_PACK);
                    PackUI pack = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_PACK, "PackUI") as PackUI;
                    pack.prj = packRJ;
                    pack.show();


                    hide();
                    //隐藏主城//
                    //				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                    packRJ = null;
                    break;

                case 7:         //吞噬（强化）//
                                //				IntensifyPanel.mInstance.allCells=prj.list;
                                //				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyCard))
                                //				{
                                //					IntensifyPanel.mInstance.guideTargetCard = prj.list[1];
                                //				}
                                //				IntensifyPanel.mInstance.show();

                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY);
                    IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY,
                        "IntensifyPanel") as IntensifyPanel;
                    intensify.allCells.Clear();
                    for (int i = 0; i < packRJ.pejs.Count; i++)
                    {
                        intensify.allCells.Add(packRJ.pejs[i].pe);
                    }
                    intensify.allFromIdList = packRJ.pejs;
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyCard))
                    {
                        intensify.guideTargetCard = packRJ.pejs[1].pe;
                    }

                    intensify.show();


                    //关闭主菜单选项卡//
                    hide();
                    //隐藏主城//
                    //				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyCard))
                    {
                        GuideUI_Intesnify.mInstance.showStep(2);
                    }
                    else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
                    {
                        GuideUI_IntensifyEquip.mInstance.showStep(3);
                    }
                    packRJ = null;
                    break;
                case 8:         //合成//
                    if (errorCode == 0)
                    {
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE);
                        ComposePanel compose = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE,
                            "ComposePanel") as ComposePanel;


                        if (compose != null)
                        {
                            compose.newItemIDList.Clear();
                            compose.crj = _crj;
                            compose.packItemInfoList = compose.crj.cs;

                            compose.mark = _crj.mark;

                            compose.GetTip();
                            compose.setComposePage(ComposePanel.PageType.E_ComposePackPage);
                            compose.setComposeType(ComposePanel.ComposeType.E_Equip);
                            compose.show();
                        }

                        //关闭主菜单选项卡//
                        //				hide();
                        //隐藏主城//
                        //					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                        hide();
                        _crj = null;
                    }

                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Compose))
                    {
                        GuideUI14_Compose.mInstance.showStep(2);
                    }
                    break;
                case 9:         //符文//
                    if (errorCode == 0)
                    {

                        //					RuneUI.mInstance.show();
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_RUNE);
                        RuneUI rune = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_RUNE,
                            "RuneUI") as RuneUI;
                        //					friend.frj = friendRJ;
                        rune.rrj = runeRJ;
                        rune.show();
                        runeRJ = null;
                        //				hide();
                        //隐藏主城//
                        //					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                        hide();
                        HeadUI.mInstance.hide();
                        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Rune))
                        {
                            GuideUI20_Rune.mInstance.hideAllStep();
                            UISceneDialogPanel.mInstance.showDialogID(24);
                        }
                    }
                    break;
                case 10:        //好友//
                                //				FriendUI.mInstance.show();
                                //显示好友//
                    if (errorCode == 0)
                    {
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_FRIEND);
                        FriendUI friend = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_FRIEND,
                            "FriendUI") as FriendUI;
                        friend.frj = friendRJ;
                        friend.mark = markId;
                        friend.show();
                        friendRJ = null;
                    }
                    //				hide();
                    //隐藏主城//
                    //				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                    hide();
                    break;
                case 11:        //签到//
                                //				SignUI.mInstance.show();
                                //打开签到界面//
                    if (errorCode == 0)
                    {
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SIGN);
                        SignUI sign = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SIGN,
                            "SignUI") as SignUI;
                        sign.srj = signRJ;
                        sign.show();
                        signRJ = null;
                    }
                    curEnterType = STATE.ENTER_MAINMENU_BACK;
                    break;
                case 12: // card break 
                         //				CardBreakPanel.mIntance.show();
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_BREAKCARD);
                    CardBreakPanel cardBreak = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_BREAKCARD,
                            "CardBreakPanel") as CardBreakPanel;
                    cardBreak.sortItemList = breakRJ.pes;
                    cardBreak.breakCardList = breakRJ.pes;
                    cardBreak.show();

                    //				hide();
                    //隐藏主城//
                    //				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                    hide();

                    break;
                case 13://邮箱//
                    curEnterType = STATE.ENTER_MAINMENU_BACK;
                    //				MailUI.mInstance.show();
                    //打开邮箱界面//
                    if (errorCode == 0)
                    {
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAIL);
                        MailUI mail = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAIL,
                            "MailUI") as MailUI;
                        mail.mrj = mailRJ;
                        mail.show();
                        mailRJ = null;
                    }
                    break;
                case 14://活动//
                        //				ActivityPanel.mInstance.aes = activityj.acts;
                        //				ActivityPanel.mInstance.show();
                    curEnterType = STATE.ENTER_MAINMENU_BACK;
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE);
                    ActivityPanel active = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE,
                        "ActivityPanel") as ActivityPanel;
                    active.aes = activeRJ.acts;
                    active.show();
                    break;
                case 15://充值界面//
                        //				ChargePanel.mInstance.curRechargeResult = rechargej;
                        //				ChargePanel.mInstance.show();

                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
                    ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ,
                            "ChargePanel") as ChargePanel;
                    charge.curRechargeResult = rechargeRJ;
                    charge.firstCharge = cost;
                    charge.show();

                    break;
                case 17:        //冥想界面//
                    int curNpcId = spriteRJ.id;
                    int nullPackNum = spriteRJ.i;

                    int mid = spriteRJ.mid;

                    int mnum = spriteRJ.mnum;

                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD);
                    SpriteWroldUIManager spriteWorld = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD,
                        "SpriteWroldUIManager") as SpriteWroldUIManager;
                    spriteWorld.SetData(nullPackNum, curNpcId, mid, mnum);

                    HeadUI.mInstance.hide();

                    spriteRJ = null;
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Spirit))
                    {
                        GuideUI23_Spirit.mInstance.hideAllStep();
                        UISceneDialogPanel.mInstance.showDialogID(17);
                    }

                    hide();
                    break;
                case 18:        //商城//
                                //curEnterType = STATE.ENTER_MAINMENU_BACK;
                    if (errorCode == 0)
                    {
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SHOP);
                        ShopUI shop = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SHOP, "ShopUI") as ShopUI;
                        shop.ShopResJson = shopRJ;
                        shopRJ = null;

                        shop.show(2);
                    }
                    break;
                case 19:        //任务界面//
                    if (errorCode == 0)
                    {
                        curEnterType = STATE.ENTER_MAINMENU_BACK;
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_TASK);
                        TaskPanel task = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_TASK,
                            "TaskPanel") as TaskPanel;
                        task.firstCharge = cost;
                        task.tes = taskRJ.tes;
                        task.activeValue = taskRJ.active;
                        task.activeState = taskRJ.activeState;
                        task.show();
                    }
                    break;
                case 20:        //竞技场//
                    if (errorCode == 0)
                    {
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA);
                        ArenaUIManager arena = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA,
                            "ArenaUIManager") as ArenaUIManager;
                        arena.SetData(0, rankJson.s, rankJson.ss, rankJson.sAward, rankJson.sPknum, rankJson.cdtime, rankJson.cardIds);

                        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_PVP))
                        {
                            UISceneDialogPanel.mInstance.showDialogID(16);
                        }
                        hide();
                    }
                    break;
                case 21://聚宝盆界面//
                    {
                        curEnterType = STATE.ENTER_MAINMENU_BACK;
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE);
                        ActivityPanel active1 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE,
                            "ActivityPanel") as ActivityPanel;
                        active1.aes = activeRJ.acts;
                        //获取聚宝盆当前所在的页面id//
                        int pagePos = 0;
                        foreach (ActivityElement e in active1.aes)
                        {
                            if (e.type == 6)
                                break;
                            pagePos++;
                        }
                        active1.show(pagePos);
                    }
                    break;

                case 22:    //合体技//
                    curEnterType = STATE.ENTER_MAINMENU_BACK;
                    if (errorCode == 0)
                    {
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_UNITESKILL);
                        UniteSkillUI usu = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_UNITESKILL,
                               "UniteSkillUI") as UniteSkillUI;
                        usu.setCurUniteSkillId(cardGroupRJ.unitId);
                        usu.curCardGroup = cardGroupRJ.transformCardGroup();
                        usu.Show();
                        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam2))
                        {
                            GuideUI_CardInTeam2.mInstance.hide();
                            UISceneDialogPanel.mInstance.showDialogID(52);
                        }
                    }
                    break;
                case 23:
                    if (nusrj.errorCode == 0)
                    {
                        if (!nusrj.unitskills.Equals(""))
                        {
                            UniteSkillUnlockManager.mInstance.SetDataAndShow(nusrj.unitskills, null);
                        }
                    }
                    break;
            }


        }

        //如果当前model的状态是skill状态，则将其改回stay状态//
        foreach (Animator anim in animList)
        {
            if (anim != null && anim.GetBool("idle2unitSkill") && isPlayTheName("Base Layer.unitSkill", anim))
            {
                anim.SetBool("idle2unitSkill", false);
            }
        }
        if (isCanClick)
        {

            //			mouseControl();
        }

        //		if(needChange)
        //		{
        //			needChange = false;
        //			switch(clickPage){
        //			case 0:
        //				
        //				MainMenuManager.mInstance.show();
        //				// xy //
        //				MainUI.mInstance.hide();
        //				MainCardSetPanel.mInstance.hide();
        //				ShopUIManager.mInstance.hide();
        //				OtherUIManager.mInstance.hide();
        //				break;
        //			case 1:
        //				//打开卡组菜单界面，关闭其他界面//
        //				MainCardSetPanel.mInstance.show();
        //				CloseSpr();
        //				MainUI.mInstance.hide();
        //				ShopUIManager.mInstance.hide();
        //				OtherUIManager.mInstance.hide();
        //				break;
        //							
        //			case 2:
        ////				MainCardSetPanel.mInstance.hide();
        ////				MainUI.mInstance.hide();
        ////				MainMenuManager.mInstance.show();
        ////				ToastWindow.mInstance.showText("can not used, please wait!");
        ////				CleanToggle();
        ////				btnList[0].GetComponent<UIToggle2>().value = true;
        //				ShopUIManager.mInstance.show();
        //				CloseSpr();
        //				MainUI.mInstance.hide();
        //				MainCardSetPanel.mInstance.hide();
        //				OtherUIManager.mInstance.hide();
        //				break;
        //			case 3:
        ////				MainCardSetPanel.mInstance.hide();
        ////				MainUI.mInstance.hide();
        ////				MainMenuManager.mInstance.show();
        ////				ToastWindow.mInstance.showText("can not used, please wait!");
        ////				CleanToggle();
        ////				btnList[0].GetComponent<UIToggle2>().value = true;
        //				OtherUIManager.mInstance.show();
        //				CloseSpr();
        //				MainUI.mInstance.hide();
        //				MainCardSetPanel.mInstance.hide();
        //				ShopUIManager.mInstance.hide();
        //				break;
        //		
        //			case 4:
        //				//打开战斗主界面关闭其他界面//
        //				MainUI.mInstance.show();
        //				CloseSpr();
        //				MainCardSetPanel.mInstance.hide();
        //				OtherUIManager.mInstance.hide();
        //				ShopUIManager.mInstance.hide();
        //				break;
        //			}
        //		}  

        if (nextGift == 0)
        {
            Gift.SetActive(false);
        }
        else
            Gift.SetActive(true);
        if (isBeginCount)
        {
            CancelInvoke("RefreshGiftTime");
            InvokeRepeating("RefreshGiftTime", 0, 1.0f);
            isBeginCount = false;
        }
    }
    public void baseShow()
    {
        //		base.show();
    }

    public void RefreshGiftTime()
    {
        //GiftData gt = GiftData.getData(giftId);
        if (onlineTime == 0 && (curTime_s + curTime_m + curTime_h) == 0)
        {
            Gift.transform.FindChild("time").GetComponent<UILabel>().text = TextsData.getData(442).chinese;
            //Gift.transform.FindChild("mark").gameObject.SetActive(true);
            CancelInvoke("RefreshGiftTime");
        }
        else
        {
            if (onlineTime > 0)
            {
                onlineTime--;
                LineRefresh();
            }
            string cts, ctm, cth;
            if (curTime_s < 10)
                cts = "0" + curTime_s;
            else
                cts = curTime_s.ToString();
            if (curTime_m < 10)
                ctm = "0" + curTime_m;
            else
                ctm = curTime_m.ToString();
            if (curTime_h < 10)
                cth = "0" + curTime_h;
            else
                cth = curTime_h.ToString();
            if (curTime_h < 1)
                Gift.transform.FindChild("time").GetComponent<UILabel>().text = ctm + ":" + cts;
            else
                Gift.transform.FindChild("time").GetComponent<UILabel>().text = cth + ":" + ctm + ":" + cts;

            Gift.transform.FindChild("mark").gameObject.SetActive(false);
        }
    }

    public void show()
    {
        //设置摄像机的位置//	
        //第一次进入需要移动摄像机//
        if (curEnterType == STATE.ENTER_MAINMENU_FRIST && PlayerInfo.getInstance().BattleOverBackType == 0)
        {
            SetCameraPos(1);
        }
        else if (curEnterType == STATE.ENTER_MAINMENU_BACK || PlayerInfo.getInstance().BattleOverBackType != 0)
        {
            SetCameraPos(2);
        }

        //显示3d摄像机//
        //		if(!Model3dRoot.activeSelf){
        //			Model3dRoot.SetActive(true);
        //		}
        if (!Model3dRoot.activeSelf)
        {
            Main3dCameraControl.mInstance.show();
        }

        HeadUI.mInstance.show();

        if (!canCloseBtns.activeSelf)
        {

            canCloseBtns.SetActive(false);
        }

        if (!Model3dBoxCliders.activeSelf)
        {
            Model3dBoxCliders.SetActive(true);
        }

        //显示model//
        if (!models.activeSelf)
        {
            models.SetActive(true);
        }

        for (int i = 0; i < modelPosList.Count; i++)
        {
            GameObject pos = modelPosList[i];
            if (!pos.activeSelf)
            {
                pos.SetActive(true);
            }
        }

        //判断界面上的所有按钮模块是否解锁//
        //initBtns();
        initBtns2();

        //获得界面信息//
        //		requestType = 1;
        //		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAIN),this);

        //播放声音//
        string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MENU).music;
        MusicManager.playBgMusic(musicName);
        btnCornucopia.SetActive(false);
        Gift.SetActive(false);
    }

    public void initBtns()
    {
        BtnTipMark.Clear();
        for (int i = 0; i < BtnsParent.childCount; i++)
        {
            Transform child = BtnsParent.GetChild(i);
            UIButtonMessage ubm = child.GetComponent<UIButtonMessage>();
            int mark = PlayerInfo.getInstance().getUnLockData(ubm.param);
            GameObject bg = child.FindChild("Background").gameObject;
            GameObject locked = child.FindChild("Locked").gameObject;
            GameObject tipMark = child.FindChild("TipMark").gameObject;
            if (mark == 0)      //未解锁//
            {
                bg.SetActive(false);
                locked.SetActive(true);
            }
            else if (mark == 1) //已解锁//
            {
                bg.SetActive(true);
                locked.SetActive(false);
            }

            if (tipMark != null)
            {
                tipMark.SetActive(false);
            }
            BtnTipMark.Add(ubm.param, tipMark);
        }
    }

    void initBtns2()
    {
        objIconsMark.Clear();
        foreach (GameObject obj in objIcons)
        {
            UIButtonMessage butMess = obj.GetComponent<UIButtonMessage>();
            int mark = PlayerInfo.getInstance().getUnLockData(butMess.param);
            if (mark == 0)
            {
                obj.SetActive(false);
            }
            else if (mark == 1)
            {
                obj.SetActive(true);
            }

            GameObject temp = obj.transform.FindChild("mark").gameObject;
            temp.SetActive(false);
            objIconsMark.Add(butMess.param, temp);
        }

        objIconsRowNow.Clear();
        foreach (GameObject obj in objIconsRow)
        {
            if (obj.activeSelf)
            {
                objIconsRowNow.Add(obj);
                if (PlayerInfo.getInstance().mainIconShow)
                    obj.transform.localPosition = objIconPoint.transform.localPosition + new Vector3(-objIconsRowNow.Count * objPosLen, 0, 0);
                else
                {
                    obj.transform.localPosition = objIconPoint.transform.localPosition;
                    //obj.transform.localScale = new Vector3(SCALE_ICON_TO, SCALE_ICON_TO, 1);  //这个地方不能缩放,否则有问题 改用setIconPicShow//
                    //setIconPicShow(obj, false);
                }
            }
        }

        objIconsColNow.Clear();
        foreach (GameObject obj in objIconsCol)
        {
            if (obj.activeSelf)
            {
                objIconsColNow.Add(obj);
                if (PlayerInfo.getInstance().mainIconShow)
                    obj.transform.localPosition = objIconPoint.transform.localPosition + new Vector3(0, objIconsColNow.Count * objPosLen, 0);
                else
                {
                    obj.transform.localPosition = objIconPoint.transform.localPosition;
                    //obj.transform.localScale = new Vector3(SCALE_ICON_TO, SCALE_ICON_TO, 1);
                    //setIconPicShow(obj, false);
                }
            }
        }

        if (PlayerInfo.getInstance().mainIconShow)
        {
            objIconPoint.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            objIconPoint.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }

    void refreshIconPointMark()
    {
        if (PlayerInfo.getInstance().mainIconShow)
        {
            objIconPointMark.SetActive(false);
        }
        else
        {
            bool flag = false;
            foreach (GameObject obj in objIconsRowNow)
            {
                if (obj.transform.FindChild("mark").gameObject.activeSelf)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                foreach (GameObject obj in objIconsColNow)
                {
                    if (obj.transform.FindChild("mark").gameObject.activeSelf)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            objIconPointMark.SetActive(flag);
        }
    }

    //void setIconPicShow(GameObject obj, bool flag)
    //{
    //Transform trans = obj.transform.FindChild("icon");

    //if (trans != null)
    //{
    //    trans.gameObject.SetActive(flag);
    //}
    //}

    //entryType 0表示第一次进入，即进入游戏后的第一次进入, 1为从其他界面返回//
    public void SetData(int enterType)
    {
        this.curEnterType = enterType;
        show();
        SendToGetData();

    }

    //获取界面提示信息//
    public void SendToGetData()
    {
        //获得界面信息//
        requestType = 1;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAIN), this);
    }

    public void SetEnterType(int enterType)
    {
        this.curEnterType = enterType;
    }


    public void SetEf(bool isShow)
    {
        foreach (GameObject obj in particleObjs)
        {
            if (obj == null) continue;
            obj.SetActive(isShow);
        }
    }

    public void hide()

    {
        CloseData();
        gameObject.SetActive(false);
        gc();

        //		//隐藏主城//
        //		if(UISceneStateControl.mInstace!= null)
        //		{
        //			UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
        //		}
    }

    private void gc()
    {
        GameObjectUtil.destroyGameObjectAllChildrens(models);
        Resources.UnloadUnusedAssets();
    }

    /*
	 * id 1 阵容， 2 背包，3 吞噬， 4 合成， 5 符文， 6 图鉴， 7世界boss ,8 异世界， 9 竞技场， 10 灵界(冥想)， 11 扭曲空间 ， 12 好友援助， 13 合体技
     * 14 Bonus 15 精英副本， 16 召唤， 17 突破， 18， 挑战， 19 活动， 20 成就， 21 好友， 22 邮件， 23 签到， 24 充值， 25 礼包， 26 pve, 27 聚宝盆界面
     * 38 任务, 39 公会, 41 合体技, 100 商店, 101 伸缩按钮
	 */
    void OnBtnIcon(int param)
    {
        //Debug.Log("OnBtnIcon param:" + param);
        if (!isCanClick) return;
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        switch (param)
        {
            case 1:
                openCardGroup();
                break;
            case 2:
                requestType = 6;
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK, 1), this);
                break;
            case 3:
                openIntensify();
                break;
            case 4:
                openCompose();
                break;
            case 5:
                openRune();
                break;
            case 6:
                string str = TextsData.getData(142).chinese;
                ToastWindow.mInstance.showText(str);
                break;
            case 9:
                openPVP();
                break;
            case 10:
                OpenSpriteWorldPanel();
                break;
            case 16:
                openLotPanel();
                break;
            case 17:
                openCardBreakPanel();
                break;
            case 18:
                openChallenge();
                break;
            case 19:
                openActivityPanel();
                break;
            case 20:
                openAchievement();
                break;
            case 21:
                requestType = 10;
                PlayerInfo.getInstance().sendRequest(new FriendJson(0, 0), this);
                break;
            case 22:
                requestType = 13;
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Mail), this);
                break;
            case 23:
                openSign();
                break;
            case 24:
                openChargePanel();
                break;
            case 25:
                openGift();
                break;
            case 26:
                openMap();
                break;
            case 27:
                openCornucopiaPanel();
                break;
            case 38:
                openTaskPanel();
                break;
            case 39:
                ToastWindow.mInstance.showText("功能暂未开放");
                break;
            case 41:
                OpenUniteSkillPanel();
                break;
            case 100:
                openShopPanel();
                break;
            case 101:
                openIcons();
                break;
            default:
                break;
        }
    }

    /*
	 * id 1 阵容， 2 背包，3 吞噬， 4 合成， 5 符文， 6 图鉴， 7世界boss ,8 异世界， 9 竞技场， 10 灵界(冥想)， 11 扭曲空间 ， 12 好友援助， 13 合体技
	 * 14 Bonus 15 精英副本， 16 召唤， 17 突破， 18， 挑战， 19 活动， 20 成就， 21 好友， 22 邮件， 23 签到， 24 充值， 25 礼包， 26 pve
	 */

    public void OnClickBtn(int id)
    {
        //播放音效//

        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (isCanClick)
        {
            int unlockMark = PlayerInfo.getInstance().getUnLockData(id);
            if (unlockMark == 0)                //未解锁//
            {
                //string str = TextsData.getData(142).chinese;
                string s2 = UnlockData.getData(id).description;
                string ss = s2;
                ToastWindow.mInstance.showText(ss);
            }
            else if (unlockMark == 1)       //已解锁//
            {

                switch (id)
                {
                    case 1:         //阵容（卡组界面//）
                                    //					MainCardSetPanel.mInstance.show();
                        openCardGroup();
                        break;
                    case 2:         //背包//
                        requestType = 6;
                        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK, 1), this);
                        break;
                    case 3:         //吞噬（强化）//
                        openIntensify();

                        break;
                    case 4:         //合成//
                        openCompose();
                        break;
                    case 5:         //符文//
                        openRune();
                        break;
                    case 6:         //图鉴//
                        ToastWindow.mInstance.showText(TextsData.getData(142).chinese);
                        return;

                    case 10:            //冥想//
                        OpenSpriteWorldPanel();

                        break;
                    case 16:        //召唤（抽卡）//
                        openLotPanel();
                        break;
                    case 17:
                        openCardBreakPanel();
                        break;
                    case 18:        //挑战//			
                        openChallenge();
                        break;
                    case 19:        //活动//
                        openActivityPanel();
                        break;
                    case 20:        //成就//
                        openAchievement();
                        break;
                    case 21:        //好友//
                        requestType = 10;
                        PlayerInfo.getInstance().sendRequest(new FriendJson(0, 0), this);
                        break;
                    case 22:        //邮箱//
                        requestType = 13;
                        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Mail), this);
                        break;
                    case 23:        //签到//
                        openSign();
                        break;
                    case 24:
                        openChargePanel();
                        break;
                    case 25:
                        //在线礼包//
                        openGift();
                        break;
                    case 26:        //PVE(战斗)//
                        openMap();
                        break;

                }
            }
        }

    }

    public void ConfirmGetClick()
    {
        this.transform.FindChild("Gift").gameObject.SetActive(false);
    }
    //点击人物模型修改动作//
    public void OnClickModelCallBack(int modelId)
    {

        Debug.Log(" battleCardIds[modelId] ======= " + battleCardIds[modelId]);
        if (modelId < animList.Count && battleCardIds[modelId] > 0 && animList[modelId] != null)
        {
            //播放音效//
            MusicManager.playCardSoundEffect(battleCardIds[modelId], STATE.CARD_MUSIC_EFFECT_TYPE_ACTION);

            //修改模型状态//
            Animator anim = animList[modelId];
            //胜利//
            anim.SetBool("idle2unitSkill", true);
            anim.GetBool("idle2unitSkill");
        }
    }

    public void mouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
        else if (Input.GetMouseButton(0))
        {
            float xDelta = Input.GetAxis("Mouse X");
            x += xDelta * xSpeed;
            //x = Mathf.Clamp(x, xBegin,xEnd);
            float yDelta = Input.GetAxis("Mouse Y");
            y += yDelta * xSpeed;
            cameraMove(x, y);
        }
    }

    public void cameraMove(float x, float y)
    {
        Quaternion rotation = Quaternion.Euler(y, x, initEulerAngles.y);

        Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * disVector + cameraLookAtObj.transform.position;
        position = CameraObj.transform.parent.transform.InverseTransformPoint(position);
        position = position.normalized * distance;
        position = CameraObj.transform.parent.transform.TransformPoint(position);

        CameraObj.transform.position = position;
        CameraObj.transform.LookAt(cameraLookAtObj.transform.position);
    }

    //打开冥想界面//
    public void OpenSpriteWorldPanel()
    {
        requestType = 17;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SPRITEWROLD_INTO), this);
    }

    public void openLotPanel()
    {
        requestType = 2;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_LOT), this);
    }

    public void openCardBreakPanel()
    {
        requestType = 12;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CardBreak), this);
    }

    public void openCardGroup()
    {
        requestType = 5;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0), this);
    }

    public void openMap()
    {
        requestType = 3;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP), this);
        //Debug.Log("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
    }

    public void openIntensify()
    {
        requestType = 7;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify, 1), this);
    }

    public void openSign()
    {
        requestType = 11;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SIGN), this);
    }

    public void openActivityPanel()
    {
        requestType = 14;
        PlayerInfo.getInstance().sendRequest(new ActivityJson(), this);

        //ActivityPanel.mInstance.show();
    }
    public void openCornucopiaPanel()
    {
        requestType = 21;
        PlayerInfo.getInstance().sendRequest(new ActivityJson(), this);
    }

    public void openTaskPanel()
    {
        requestType = 19;
        PlayerInfo.getInstance().sendRequest(new TaskJson(), this);
    }

    public void openChargePanel()
    {
        requestType = 15;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE), this);
    }

    public void openAchievement()
    {
        requestType = 4;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_ACHIEVEMENT), this);
    }

    public void openCompose()
    {
        requestType = 8;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose, (int)ComposePanel.ComposeType.E_Equip), this);
    }

    public void openChallenge()
    {
        //		MainUI.mInstance.show();
        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CHALLENGE);
        MainUI mainUI = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CHALLENGE, "MainUI") as MainUI;
        mainUI.show();
        //		hide();
        //隐藏主城//
        //		UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
        hide();
    }

    public void openRune()
    {
        requestType = 9;
        PlayerInfo.getInstance().sendRequest(new RuneJson(0), this);
    }

    public void openPVP()
    {
        requestType = 20;
        PlayerInfo.getInstance().sendRequest(new RankJson(0), this);
    }

    public void openGift()
    {
        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GIFT);
        GiftUI gift = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GIFT, "GiftUI") as GiftUI;
        gtRJ = gift.gtrj;
        gift.id = giftId;
        //gift.onlineTime = onlineTime;
        if (onlineTime == 0)
        {
            gift.times = onlineTime;
        }
        else
            gift.times = 1;
        gift.show();
        gift.main = this;

    }

    public void RefreshGolds()
    {
        requestType = 1;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAIN), this);
    }

    void openShopPanel()
    {
        requestType = 18;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SHOP, 2), this);
    }

    public void OpenUniteSkillPanel()
    {
        //请求阵容界面拿现在装备的合体技id//
        requestType = 22;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0), this);
    }
    public void openIcons()
    {
        if (PlayerInfo.getInstance().mainIconShow)
        {
            foreach (GameObject obj in objIconsRowNow)
            {
                menuIconTween(obj, TIME_ICON_MOVE, objIconPoint.transform.localPosition, 1, SCALE_ICON_TO);
            }
            foreach (GameObject obj in objIconsColNow)
            {
                menuIconTween(obj, TIME_ICON_MOVE, objIconPoint.transform.localPosition, 1, SCALE_ICON_TO);
            }
            TweenRot(objIconPoint, TIME_ICON_MOVE, new Vector3(0, 0, 0), new Vector3(0, 0, 90));
        }
        else
        {
            int temp = 0;
            foreach (GameObject obj in objIconsRowNow)
            {
                menuIconTween(obj, TIME_ICON_MOVE, objIconPoint.transform.localPosition + new Vector3(-(++temp) * objPosLen, 0, 0), SCALE_ICON_TO, 1);
                //setIconPicShow(obj, true);
            }
            temp = 0;
            foreach (GameObject obj in objIconsColNow)
            {
                menuIconTween(obj, TIME_ICON_MOVE, objIconPoint.transform.localPosition + new Vector3(0, (++temp) * objPosLen, 0), SCALE_ICON_TO, 1);
                //setIconPicShow(obj, true);
            }
            TweenRot(objIconPoint, TIME_ICON_MOVE, new Vector3(0, 0, 90), new Vector3(0, 0, 0));
        }

        PlayerInfo.getInstance().mainIconShow = !PlayerInfo.getInstance().mainIconShow;

        refreshIconPointMark();
    }

    public void OpenTimes()
    {
        isBeginCount = true;

    }
    public void LineRefresh()
    {
        curTime_s = onlineTime % 60;
        curTime_m = onlineTime / 60;
        if (onlineTime >= 3600)
        {
            curTime_m = 59;
            if (curTime_m >= 60)
            {
                curTime_h = curTime_m / 60;
            }
        }
        if (curTime_s < 0)
        {
            curTime_s = 0;
        }
    }
    public void CloseData()
    {

        //去掉3d模型的碰撞框//
        Model3dBoxCliders.SetActive(false);
        if (Main3dCameraControl.mInstance != null)
        {
            Main3dCameraControl.mInstance.reset();
            Main3dCameraControl.mInstance.hide();
        }
        CleanModels();
        isCanClick = false;
        if (modularUpdateData != null)
        {
            modularUpdateData.Clear();
        }
        if (BtnTipMark != null)
        {
            BtnTipMark.Clear();
        }
    }

    public void CleanModels()
    {
        //隐藏掉模型//
        GameObjectUtil.destroyGameObjectAllChildrens(models);
        //隐藏坐标//
        //		foreach(GameObject obj in modelPosList){
        //			obj.SetActive(false);
        //		}

        //清空数据列表//
        if (battleCardIds != null && battleCardIds.Count > 0)
        {

            battleCardIds.Clear();
        }

        //		//清空存储model的anim的链表//
        if (animList != null)
        {
            animList.Clear();
        }

        //隐藏3d摄像机//
        if (Model3dRoot != null && Model3dRoot.activeSelf)
        {
            Model3dRoot.SetActive(false);
        }
    }


    //	public void CleanToggle(){
    //		foreach(GameObject obj in btnList){
    //			UIToggle2 toggle2 = obj.GetComponent<UIToggle2>();
    //			if(obj!= null && toggle2!= null){
    //				toggle2.value = false;
    //				toggle2.activeSprite.gameObject.SetActive(toggle2.value);
    //			}
    //		}
    //	}
    //	
    //	public void SetToggleSpr(UIToggle2 toggle){
    ////		toggle.value = isShow;
    //		toggle.activeSprite.gameObject.SetActive(toggle.value);
    //	}
    //1 获得界面信息， 2 抽卡， 3 pve//
    public void receiveResponse(string json)
    {
        //Debug.Log("MainMenu : json ====== " + json);
        Debug.Log("requestType : type ====== " + requestType);
        if (json != null)
        {
            //关闭连接界面的动画//
            PlayerInfo.getInstance().isShowConnectObj = false;
            switch (requestType)
            {
                case 1:
                    modularUpdateData.Clear();
                    battleCardIds.Clear();
                    MainResultJson mrj = JsonMapper.ToObject<MainResultJson>(json);
                    errorCode = mrj.errorCode;
                    giftId = mrj.giftId;
                    type = mrj.type;
                    onlineTime = mrj.onlineT;
                    cost = mrj.cost;
                    if (!isBeginCount)
                    {
                        isBeginCount = true;
                    }
                    LineRefresh();
                    if (errorCode == 0 || errorCode == 99)
                    {
                        battleCardIds = mrj.getList();
                        modularUpdateData = mrj.s;
                    }
                    if (errorCode == 99)
                    {
                        nextGift = 0;
                    }


                    receiveData = true;
                    break;
                case 2:
                    LotResultJson lrj = JsonMapper.ToObject<LotResultJson>(json);
                    errorCode = lrj.errorCode;
                    //				LotCardUI.mInstance.lrj=lrj;
                    lotRJ = lrj;
                    receiveData = true;
                    break;
                case 3:
                    MapResultJson mj = JsonMapper.ToObject<MapResultJson>(json);
                    errorCode = mj.errorCode;
                    if (errorCode == 0)
                    {
                        //					MissionUI.mInstance.mrj=mj;

                        mapRJ = mj;
                    }
                    receiveData = true;
                    break;
                case 4:
                    AchieveResultJson arj = JsonMapper.ToObject<AchieveResultJson>(json);
                    errorCode = arj.errorCode;
                    if (errorCode == 0)
                    {
                        achiveRJ = null;
                        //					AchievementPanel.mInstance.arj=arj;
                        achiveRJ = arj;
                    }
                    receiveData = true;
                    break;

                case 5:			//阵容（卡组）//
                    CardGroupResultJson cgrj = JsonMapper.ToObject<CardGroupResultJson>(json);
                    errorCode = cgrj.errorCode;
                    if (errorCode == 0)
                    {
                        //					CombinationInterManager.mInstance.curCardGroup=cgrj.transformCardGroup();
                        //					CombinationInterManager.mInstance.curPage=0;
                        //					CombinationInterManager.mInstance.isUsed=true;
                        cardGroupRJ = null;
                        cardGroupRJ = cgrj;
                    }
                    receiveData = true;
                    break;
                case 6:			//背包//
                    PackResultJson prj = JsonMapper.ToObject<PackResultJson>(json);
                    errorCode = prj.errorCode;
                    if (errorCode == 0)
                    {
                        //PackUI.mInstance.elements=prj.list;
                        //List<PackElement> list = new List<PackElement>();
                        //for (int i = 0; i < prj.pejs.Count; i++)
                        //{
                        //    list.Add(prj.pejs[i].pe);
                        //}
                        //                    PackUI.mInstance.pejs = prj.pejs;
                        packRJ = prj;
                    }
                    receiveData = true;
                    break;

                case 7:			//强化（吞噬）//
                    prj = JsonMapper.ToObject<PackResultJson>(json);
                    errorCode = prj.errorCode;
                    if (errorCode == 0)
                    {
                        //					IntensifyPanel.mInstance.allCells=prj.list;
                        //					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyCard))
                        //					{
                        //						IntensifyPanel.mInstance.guideTargetCard = prj.list[1];
                        //					}

                        packRJ = prj;
                    }
                    receiveData = true;

                    break;

                case 8:			//合成//
                    ComposeResultJson crj = JsonMapper.ToObject<ComposeResultJson>(json);
                    errorCode = crj.errorCode;
                    _crj = crj;
                    receiveData = true;
                    break;

                case 9:			//符文//
                    RuneResultJson rrj = JsonMapper.ToObject<RuneResultJson>(json);
                    errorCode = rrj.errorCode;
                    if (errorCode == 0)
                    {

                        //					RuneUI.mInstance.rrj=rrj;
                        runeRJ = rrj;
                    }
                    receiveData = true;
                    break;
                case 10:		//好友//
                    FriendResultJson frj = JsonMapper.ToObject<FriendResultJson>(json);
                    errorCode = frj.errorCode;
                    if (errorCode == 0)
                    {
                        //					FriendUI.mInstance.frj=frj;
                        friendRJ = frj;
                    }
                    receiveData = true;
                    break;
                case 11:		//签到//
                    SignResultJson srj = JsonMapper.ToObject<SignResultJson>(json);
                    errorCode = srj.errorCode;
                    if (errorCode == 0)
                    {
                        //					SignUI.mInstance.srj=srj;
                        signRJ = srj;
                    }
                    receiveData = true;
                    break;
                case 12: // card break
                    BreakResultJson brj = JsonMapper.ToObject<BreakResultJson>(json);
                    errorCode = brj.errorCode;
                    if (errorCode == 0)
                    {
                        //					CardBreakPanel.mIntance.sortItemList = brj.pes;
                        breakRJ = brj;
                    }
                    receiveData = true;
                    break;
                case 13:	//邮箱//
                    MailUIResultJson mailj = JsonMapper.ToObject<MailUIResultJson>(json);
                    errorCode = mailj.errorCode;
                    if (errorCode == 0)
                    {
                        //					MailUI.mInstance.mrj=mailj;
                        //					MailUI.mInstance.mrj.sort();
                        mailRJ = mailj;
                        mailRJ.sort();
                    }
                    receiveData = true;
                    break;
                case 14:	//活动//
                    Debug.Log("receive 14!");
                    ActivityResultJson activityj = JsonMapper.ToObject<ActivityResultJson>(json);
                    errorCode = activityj.errorCode;
                    if (errorCode == 0)
                    {
                        //					ActivityPanel.mInstance.aes = activityj.acts;
                        activeRJ = activityj;
                    }
                    receiveData = true;
                    break;
                case 15:
                    Debug.Log("recharge result json:" + json);
                    RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
                    errorCode = rechargej.errorCode;
                    if (errorCode == 0)
                    {
                        //					ChargePanel.mInstance.curRechargeResult = rechargej;
                        rechargeRJ = rechargej;
                    }
                    receiveData = true;
                    break;

                case 17:			//冥想界面//
                    receiveData = true;

                    ImaginationResultJson irj = JsonMapper.ToObject<ImaginationResultJson>(json);
                    errorCode = irj.errorCode;
                    if (errorCode == 0)
                    {
                        //					curNpcId = irj.id;
                        //					nullPackNum = irj.i;
                        spriteRJ = irj;
                        PlayerInfo.getInstance().player.gold = irj.g;
                    }

                    break;
                case 18:
                    receiveData = true;

                    ShopResultJson temp = JsonMapper.ToObject<ShopResultJson>(json);
                    errorCode = temp.errorCode;
                    if (errorCode == 0)
                    {
                        shopRJ = temp;
                    }
                    break;
                case 19:		//任务//
                    TaskResultJson tRJ = JsonMapper.ToObject<TaskResultJson>(json);
                    errorCode = tRJ.errorCode;
                    if (errorCode == 0)
                    {
                        Debug.Log("TaskResultJson:" + json);
                        taskRJ = tRJ;
                    }
                    receiveData = true;
                    break;
                case 20:
                    receiveData = true;

                    RankResultJson tempJson = JsonMapper.ToObject<RankResultJson>(json);
                    errorCode = tempJson.errorCode;
                    if (errorCode == 0)
                    {
                        rankJson = tempJson;
                    }
                    break;
                case 21:
                    Debug.Log("activityj result json:" + json);
                    ActivityResultJson activityj1 = JsonMapper.ToObject<ActivityResultJson>(json);
                    errorCode = activityj1.errorCode;
                    if (errorCode == 0)
                    {
                        //					ActivityPanel.mInstance.aes = activityj.acts;
                        activeRJ = activityj1;
                    }
                    receiveData = true;
                    break;


                //合体技界面//
                case 22:
                    CardGroupResultJson cgrjUS = JsonMapper.ToObject<CardGroupResultJson>(json);
                    errorCode = cgrjUS.errorCode;
                    if (errorCode == 0)
                    {
                        cardGroupRJ = null;
                        cardGroupRJ = cgrjUS;
                    }
                    receiveData = true;
                    break;
                case 23:
                    nusrj = JsonMapper.ToObject<NewUnitSkillResultJson>(json);
                    errorCode = nusrj.errorCode;
                    receiveData = true;
                    break;
            }
        }

    }

    public void reset()
    {
        x = initEulerAngles.y;
        CameraObj.transform.position = initPosition;
        CameraObj.transform.rotation = initRotation;
    }

    //判断当前model的状态//
    public bool isPlayTheName(string name, Animator anim)
    {
        AnimatorStateInfo asi = anim.GetCurrentAnimatorStateInfo(0);
        return asi.IsName(name);
    }

    public int setCardDatei(int i)
    {
        return i;
    }

    void menuIconTween(GameObject obj, float t, Vector3 toPos, float scaleFrom, float scaleTo)
    {
        TweenPos(obj, t, toPos);
        TweenSca(obj, t, new Vector3(scaleFrom, scaleFrom, 1), new Vector3(scaleTo, scaleTo, 1));
    }

    void TweenPos(GameObject obj, float t, Vector3 vec, bool callbackFlag = false)
    {
        TweenPosition tweenPos = TweenPosition.Begin(obj, t, vec);
        if (callbackFlag)
            EventDelegate.Add(tweenPos.onFinished, OnFinishedCallback);
    }

    void TweenSca(GameObject obj, float t, Vector3 v1, Vector3 v2, bool callbackFlag = false)
    {
        TweenScale tweenScale = UITweener.Begin<TweenScale>(obj, t);
        tweenScale.from = v1;
        tweenScale.to = v2;
        if (callbackFlag)
            EventDelegate.Add(tweenScale.onFinished, OnFinishedCallback);
    }

    void TweenRot(GameObject obj, float t, Vector3 v1, Vector3 v2, bool callbackFlag = false)
    {
        TweenRotation tweenRot = UITweener.Begin<TweenRotation>(obj, t);
        tweenRot.from = v1;
        tweenRot.to = v2;
        if (callbackFlag)
            EventDelegate.Add(tweenRot.onFinished, OnFinishedCallback);
    }

    void OnFinishedCallback()
    {
        //Debug.Log("------ OnFinishedCallback");
    }

    public void setNeedShowUnitSkill(bool isNeed)
    {
        isNeedShowUnitSkill = isNeed;
    }
}
