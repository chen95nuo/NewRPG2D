using UnityEngine;
using System.Collections;

public class DropGuidePanel : BWUIPanel, ProcessResponse {
	
	public static DropGuidePanel mInstance;
	
//	public GameObject dropGuide;
	public GameObject dropHead;
	public GameObject dropParent;
	
	private Transform _myTransform;
	private int requestType;
	private int errorCode;
	private bool receiveDate;
	
	private GameObject dropGuideCell;
	private int curGotoMissionId;
	//从哪个界面进来的，1 合成， 2 卡牌详细, 3 阵容, 4 合体技//
	private int comeUIType;
	
	private DropGuideResultJson drj;
	//推图界面的json//
	MapResultJson mapRJ;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		_myTransform = transform;
		init();
		hide();
	}
	
	
	public override void init ()
	{
//		base.init ();
		_myTransform.localPosition = new Vector3(0, 0, -720f);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveDate )
		{
			receiveDate = false ;
			if(errorCode == -3)
				return;
			
			switch(requestType )
			{
			case 1:
				//如果推图界面存在的话，则先删除推图界面，在跳转//
				MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI")as MissionUI;
				if(mission!= null)
				{
					mission.hide();
				}
				gotoMission();
				break;
			}
		}
	}
	
	private void gotoMission()
	{
		
		//隐藏合成界面但不删除数据//
		if(comeUIType == 1)
		{
			UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE);
		}
		else if(comeUIType == 2)
		{
			UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO);
		}
		else if(comeUIType == 3)		//关闭卡组界面//
		{
//			UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
			CombinationInterManager comb = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
			comb.hide();
		}
		else if(comeUIType == 4)
		{
			UniteSkillUI usu = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_UNITESKILL, "UniteSkillUI")as UniteSkillUI;
			usu.setIsComeToMission(true);
			usu.Hide();
		}
		//隐藏获得途径界面//
		GetWayPanelManager getway = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GETWAYPANEL, "GetWayPanelManager")as GetWayPanelManager;
		if(getway!=null)
		{
			getway.hide();
		}
		//打开推图界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI2")as MissionUI2;
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI")as MissionUI;
		mission.mrj=mapRJ;
		mission2.showFromCompose(curGotoMissionId);
		
		mission.showFromCompose(curGotoMissionId, comeUIType);
		
		hide();
	}
	
	public void showDropGuide()
	{
        if (drj != null)
        {
            //ItemsData itemData = ItemsData.getData(drj.id);
            //        if (drj.ds.Count == 0)
            //        {
            //            GetIn(drj.id);
            //            return;
            //        }
            //		dropGuide.SetActive(true);
            //		dropGuide.transform.FindChild("scroll-bar").GetComponent<UIScrollBar>().value=0;
            _myTransform.FindChild("scroll-bar").GetComponent<UIScrollBar>().value = 0;

            GameObjectUtil.destroyGameObjectAllChildrens(dropParent);

            dropHead.transform.FindChild("ItemInfo").GetComponent<SimpleCardInfo2>().clear();
            dropHead.transform.FindChild("ItemInfo").GetComponent<SimpleCardInfo2>().setSimpleCardInfo(drj.id, GameHelper.E_CardType.E_Item);
            for (int i = 0; drj.ds != null && i < drj.ds.Count; i++)
            {
                if (dropGuideCell == null)
                {

                    dropGuideCell = GameObjectUtil.LoadResourcesPrefabs("ComposePanel/DropCell", 3);
                }
                GameObject cell = Instantiate(dropGuideCell) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(cell, dropParent);
                GameObjectUtil.setGameObjectLayer(cell, dropParent.layer);
                //==关卡id-是否解锁-已完成次数==//
                string s = drj.ds[i];
                string[] ss = s.Split('-');
                int missionId = StringUtil.getInt(ss[0]);
                int unlock = StringUtil.getInt(ss[1]);
                int times = StringUtil.getInt(ss[2]);

                MissionData md = MissionData.getData(missionId);

                UIButtonMessage msg = cell.GetComponent<UIButtonMessage>();
                if (unlock == 1)
                {
                    msg.target = _myTransform.gameObject;
                    msg.functionName = "onClickDropGuideCell";
                    msg.param = md.id;
                    if (md.missiontype == 1)
                    {
                        cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "map_base_01";
                    }
                    else
                    {
                        cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "map_base_03";
                    }
                    cell.transform.FindChild("times").GetComponent<UILabel>().text = "(" + (md.times - times) + "/" + md.times + ")";
                }
                else
                {
                    msg.target = null;
                    if (md.missiontype == 1)
                    {
                        cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "map_base_02";
                    }
                    else
                    {
                        cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName = "map_base_04";
                    }
                    cell.transform.FindChild("times").GetComponent<UILabel>().text = TextsData.getData(320).chinese;
                }
                cell.transform.FindChild("zoneName").GetComponent<UILabel>().text = md.zonename;
                cell.transform.FindChild("missionName").GetComponent<UILabel>().text = md.name;
            }
            if (dropParent.transform.childCount <= 5)
            {
                //            dropGuide.transform.FindChild("panel").GetComponent<UIDraggablePanel>().disableDragIfFits = true;
                _myTransform.FindChild("panel").GetComponent<UIDraggablePanel>().disableDragIfFits = true;
            }
            else
            {
                //            dropGuide.transform.FindChild("panel").GetComponent<UIDraggablePanel>().disableDragIfFits = false;
                _myTransform.FindChild("panel").GetComponent<UIDraggablePanel>().disableDragIfFits = false;

            }
            dropParent.GetComponent<UIGrid>().repositionNow = true;
        }
	}
	
	
	public override void show ()
	{
		base.show ();
		showDropGuide();
	}
	
	public void SetData(DropGuideResultJson dropRJ, int comeType)
	{
		this.drj = dropRJ;
		this.comeUIType = comeType;
		show();
	}
	
	public override void hide ()
	{
		base.hide ();
		gc();
	}
	
	public void gc()
	{
		mapRJ = null;
		drj = null;
		dropGuideCell=null;
		GameObjectUtil.destroyGameObjectAllChildrens(dropParent);
		
	}
	
	
	public void onClickDropGuideCell(int param)
	{
		curGotoMissionId=param;
		requestType=1;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
	}
	
	public void OnClickCloseBtn()
	{
		hide();
	}
	
	public void receiveResponse (string json)
	{
		if(json != null)
		{
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:				//前往map//
				MapResultJson mrj=JsonMapper.ToObject<MapResultJson>(json);
				errorCode = mrj.errorCode;
//				MissionUI.mInstance.mrj=mrj;
				mapRJ = mrj;
				
				receiveDate = true;
				break;
			}
		}
	}
}
