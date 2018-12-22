using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetWayPanelManager : MonoBehaviour, ProcessResponse  {
	
	private Transform _myTransform;
	
	private int requestType;
	private int errorCode;
	private bool receiveData;
	private DropGuideResultJson drj;
	//private int curUniteId;
	//进入的类型，1 合成（不走getWayPanel），2 卡牌详细, 3 阵容//
	private int curComeType;
	private List<int> uniteCardIds = new List<int>();
	//float startX = -66;
	//float startY = -63;
	//float offY1 = -155;
	//float offY2 = -185;
	
	float getWayItemStartY = 180f;
	float getWayItemOffY = 160f;
	
	public GameObject getWayDragParent;
	//合体技获得信息的prefab//
	public GameObject getWayItemPrefab;
	public GameObject getWayDragPanel;
	public GameObject scrollBar;
	
	void Awake()
	{
		_myTransform = transform;
		hide();
	}
	
	void init()
	{
		
	}
	
	// Use this for initialization
	void Start () {
	
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
			case 1:
				if(errorCode == 0)
				{
					//进入的类型，1 合成（不走getWayPanel），2 卡牌详细, 3 阵容 ,4 合体技//
					DropGuidePanel.mInstance.SetData(drj, curComeType);
				}
				break;
			}
		}
	}
	
	
	public void ShowGetWayData()
	{
//		getWayPanel.SetActive(true);
		
//		List<int> cardIds = (List<int>)uniteSkillCards[curUniteId];
		List<int> cardIds = uniteCardIds;
		//有几个卡牌就绘制几个item//
		for(int i = 0; cardIds!=null && i < cardIds.Count;i ++)
		{
			CardData cd = CardData.getData(cardIds[i]);
			GameObject item = Instantiate(getWayItemPrefab) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(item, getWayDragParent);
			float y = getWayItemStartY - i * getWayItemOffY;
			item.transform.localPosition = new Vector3(0, y , 0);
			
			GetWayItem gwi = item.GetComponent<GetWayItem>();
			gwi.sci2.setSimpleCardInfo(cardIds[i], GameHelper.E_CardType.E_Hero);
			gwi.Name.text = cd.name;
			
			//修改卡牌出处//
			string getWay = cd.waytoget;
			string[] str = getWay.Split(',');
			string showData = "";
            bool bBtnGo = false;
			for(int m = 0;m < str.Length; m++)
			{
				int id = StringUtil.getInt(str[m]);
				string ss = "";
				switch(id)
				{
				case 0:
					ss = TextsData.getData(517).chinese;
					break;
				case 1:
					ss = TextsData.getData(510).chinese;
					break;
				case 2:
					ss = TextsData.getData(511).chinese;
					break;
				case 3:
					ss = TextsData.getData(512).chinese;
					break;
				case 4:
					ss = TextsData.getData(513).chinese;
					break;
				case 5:
					ss = TextsData.getData(514).chinese;
					break;
				case 6:			//关卡掉咯//
                    bBtnGo = true;
					ss = TextsData.getData(555).chinese;
					break;
                case 7:
                    ss = TextsData.getData(647).chinese;
                    break;
				}
				showData += ss + "\r\n";
			}
            gwi.Btn_Go.SetActive(bBtnGo);
            if (bBtnGo)
            {
                UIButtonMessage ubm = gwi.Btn_Go.GetComponent<UIButtonMessage>();
                ubm.target = _myTransform.gameObject;
                ubm.functionName = "OnClickCardDropBtn";
                ubm.param = cardIds[i];
            }
			gwi.Des.text = showData;
		}
	
		
	}
	
	
	public void show()
	{
		if(_myTransform == null)
		{
			_myTransform = transform;
		}
		_myTransform.gameObject.SetActive(true);
		_myTransform.localPosition = new Vector3(0, 0, -720);
		ShowGetWayData();
	}
	
	public void SetData(int uniteId, List<int> cardIds, int comeType)
	{
		//curUniteId = uniteId ;
		this.uniteCardIds = cardIds;
		this.curComeType = comeType;
		show();
	}
	
	public void hide()
	{
		UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GETWAYPANEL);
		CleanScrollData();
		gc ();
	}
	
	public void CleanScrollData()
	{
		scrollBar.GetComponent<UIScrollBar>().value = 0;
		getWayDragPanel.transform.localPosition = Vector3.zero;
		getWayDragPanel.GetComponent<UIPanel>().clipRange = new Vector4(0,0,431,485);
		GameObjectUtil.destroyGameObjectAllChildrens(getWayDragParent);
	}
	
	public void gc()
	{
		_myTransform = null;
		uniteCardIds.Clear();
		uniteCardIds = null;
		drj = null;
	}
	
	public void OnClickCardDropBtn(int cardId)
	{
		ComposeData data = ComposeData.getData(cardId,1);//参数1是英雄卡的类型//
		string str = data.material_num[0];
		string[] ss = str.Split('-');
		int itemId = StringUtil.getInt( ss[0]);
		requestType = 1;
		Debug.Log(itemId );
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_DROP_GUIDE,itemId),this);
	}
	
	public void OnClickGetWayCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		hide();
	}
	
	public void receiveResponse (string json)
	{
		if(json != null)
		{
			Debug.Log("GetWayPanelManage : json = " + json);
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				DropGuideResultJson drj=JsonMapper.ToObject<DropGuideResultJson>(json);
				errorCode = drj.errorCode;
				this.drj=drj;
				receiveData = true;
				break;
			}
		}
	}
}
