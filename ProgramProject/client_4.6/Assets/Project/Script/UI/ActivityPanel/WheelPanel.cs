using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WheelPanel : MonoBehaviour {
	
	public GameObject actWheelPanel;
	public GameObject ruleUIGrid;
	public GameObject staticCirCleGroup;
	public GameObject circleGroupRoot;
	public GameObject circleGroup;
	public GameObject rightTopGroup;
	public GameObject rightTopItemIcon;
	public UILabel rightTopName;
	public UILabel rightTopNum;
	
	GameObject activityPanel;
	RunnerResultJson rResultJson;
	
	const string ItemIconPath = "Prefabs/UI/UI-Shop/ItemIcon";
	GameObject itemIconPrefab;
	const string ruleCellPath = "Prefabs/UI/ActivityPanel/RuleCell";
	GameObject ruleCellPrefab;
	
	const string effectPath = "Prefabs/Effects/UIEffect/huode_ui";
	GameObject effectPrefab = null;
	
	int id;
	//当前转动次数//
	public int rotNum;
	//当前激活的活动类型//
	int curActiveType;
	//当前档位//
	int curActiveRun;
	
	//当前档位的index//
	int curIndex;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitWheelPanel(GameObject actPanel,RunnerResultJson rrJson,int id,int rNum,int activeType,int activeRun,int index)
	{
		this.activityPanel = actPanel;
		this.id = id;
		this.rotNum = rNum;
		this.curActiveType = activeType;
		this.curActiveRun = activeRun;
		this.curIndex = index;
		
		RunnerData rData = RunnerData.getData(id);
		if(rData == null)
		{
			return;
		}
		//初始化转盘背景图//
		circleGroupRoot.transform.FindChild("Sprite1").GetComponent<UISprite>().spriteName = "fc"+(this.curIndex+2).ToString();
		circleGroupRoot.transform.FindChild("Sprite2").GetComponent<UISprite>().spriteName = "fc"+(this.curIndex+2).ToString();
		circleGroupRoot.transform.FindChild("Sprite3").GetComponent<UISprite>().spriteName = "fc"+(this.curIndex+2).ToString();
		circleGroupRoot.transform.FindChild("Sprite4").GetComponent<UISprite>().spriteName = "fc"+(this.curIndex+2).ToString();
		
		int circleItemNum = rData.goodsType.Count;
		for(int i=0;i<circleItemNum;i++)
		{
			if(itemIconPrefab == null)
			{
				itemIconPrefab = Resources.Load(ItemIconPath) as GameObject;
			}
			GameObject iObj = Instantiate(itemIconPrefab) as GameObject;
			iObj.transform.parent = circleGroup.transform.Find("Pos"+(i+1).ToString());
			iObj.transform.localPosition = Vector3.zero;
			iObj.transform.localScale = new Vector3(0.5f,0.5f,1);
			iObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
			iObj.name = "ItemIcon"+(i+1).ToString();
			//Debug.Log("goodsType:"+rData.goodsType[i]+"goodsIds:"+rData.goodsIds[i]+"goodsNum:"+rData.goodsNum[i]);
			iObj.GetComponent<ItemIcon>().Init(rData.goodsType[i],rData.goodsIds[i],rData.goodsNum[i],ItemIcon.ItemUiType.NameDownNum);
			iObj.GetComponent<BoxCollider>().enabled = false;
		}
		
		InitRightTopPanel(rrJson,1);
		
		InitRightBottomPanel();
		
		//传递当前档位id给大风车//
		circleGroupRoot.GetComponent<RotateWheelControl>().activityPanel = activityPanel;
		circleGroupRoot.GetComponent<RotateWheelControl>().cellID = id;
		circleGroupRoot.GetComponent<RotateWheelControl>().curActiveType = curActiveType;
		circleGroupRoot.GetComponent<RotateWheelControl>().curIndex = curIndex;
		circleGroupRoot.GetComponent<RotateWheelControl>().staticCirCleGroup = this.staticCirCleGroup;
	}
	
	void CloseWheelPanel()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(ruleUIGrid);
		for(int i=0;i<8;i++)
		{
			GameObject tempObj = circleGroup.transform.Find("Pos"+(i+1).ToString()).gameObject;
			GameObjectUtil.destroyGameObjectAllChildrens(tempObj);
		}
		circleGroupRoot.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
		UpdateRotNum();
		gameObject.SetActive(false);
		Resources.UnloadUnusedAssets();
	}
	
	//初始化右边上面的面板信息,type-1:界面初始化类型，无需播放特效,type-2:刷新右边上面的面板信息，需要播放特效//
	void InitRightTopPanel(RunnerResultJson rrJson,int type)
	{
		this.rResultJson = rrJson;
		if(this.rResultJson.type == 0)
		{
			rightTopGroup.SetActive(false);
		}
		else
		{
			//需要播放特效//
			if(type == 2)
			{
				if(effectPrefab == null)
				{
					effectPrefab = Resources.Load(effectPath) as GameObject;
				}
				rightTopGroup.SetActive(true);
				GameObject effectObj = Instantiate(effectPrefab) as GameObject;
				
				effectObj.transform.parent = rightTopGroup.transform;
				effectObj.name = "huode_ui";
				effectObj.transform.localPosition = new Vector3(230,125,0);
				effectObj.transform.localScale = Vector3.one;
				
				//GameObjectUtil.gameObjectAttachToParent(effectObj,rightTopGroup);
				//GameObjectUtil.setGameObjectLayer(effectObj, STATE.LAYER_ID_UIEFFECT);
				Destroy(effectObj, 1.5f);
				Invoke("InitRightTopContent", 0.3f);
			}
			else
			{
				rightTopGroup.SetActive(true);
				InitRightTopContent();
			}
			
		}
			
	}
	
	void InitRightTopContent()
	{
		int itemID = 0;
		int itemNum = 0;
		string iName = "";
		if(this.rResultJson.type < 6)
		{
			string[] ss = this.rResultJson.id.Split(',');
			itemID = StringUtil.getInt(ss[0]);
			itemNum = StringUtil.getInt(ss[1]);
		}
		else
		{
			itemNum = StringUtil.getInt(this.rResultJson.id);
		}
		rightTopItemIcon.GetComponent<ItemIcon>().Init(this.rResultJson.type,itemID,itemNum,ItemIcon.ItemUiType.NullTextShow);
		if(this.rResultJson.type < 6)
		{
			iName = rightTopItemIcon.GetComponent<ItemIcon>().labelName.text;
		}
		else
		{
			switch(this.rResultJson.type)
			{
			case 6:
				iName = TextsData.getData(658).chinese;
				break;
			case 7:
				iName = TextsData.getData(662).chinese;
				break;
			case 8:
				iName = TextsData.getData(659).chinese;
				break;
			case 9:
				iName = TextsData.getData(660).chinese;
				break;
			case 10:
				iName = TextsData.getData(661).chinese;
				break;
			case 11:
				iName = TextsData.getData(657).chinese;
				break;
			}
		}
		rightTopName.text = iName;
		rightTopNum.text = " x "+itemNum.ToString();
	}
	
	//初始化右边下面的面板信息//
	void InitRightBottomPanel()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(ruleUIGrid);
		//得到当前档位的Rule Cell list//
		List<RunData> tRunDataList = RunData.getCurRunList(curActiveType,curActiveRun);
		for(int i=0;i<tRunDataList.Count;i++)
		{
			if(ruleCellPrefab == null)
			{
				ruleCellPrefab = Resources.Load(ruleCellPath) as GameObject;
			}
			GameObject rCell = Instantiate(ruleCellPrefab) as GameObject;
			rCell.transform.parent = ruleUIGrid.transform;
			rCell.transform.localScale = Vector3.one;
			rCell.name = ruleCellPrefab.name+(9999-i).ToString();
			
			//查看服务器返回的列表中是否含有当前cell的id,没有的话状态为－1，有的话根据服务器的结果初始化，0表示未领取，1表示已领取//
			//bool isContains = false;
			int state = -1;
			for(int j=0;j<this.rResultJson.list.Count;j++)
			{
				ShopElement temp = this.rResultJson.list[j];
				if(temp.id == tRunDataList[i].id)
				{
					//isContains = true;
					state = temp.num;
				}
			}
			//得到当前id的可转动次数//
			int num = 0;
			for(int k=0;k<this.rResultJson.view.Count;k++)
			{
				ShopElement temp = this.rResultJson.view[k];
				if(temp.id == tRunDataList[i].id)
				{
					num = temp.num;
				}
			}
			rCell.GetComponent<RuleCell>().Init(tRunDataList[i].id,state,num);
		}
		ruleUIGrid.GetComponent<UIGrid>().repositionNow = true;
	}
	
	//刷新右边上下面板的数据信息//
	public void UpdateWheelPanelData(RunnerResultJson rrJson)
	{
		InitRightTopPanel(rrJson,2);
		InitRightBottomPanel();
	}
	
	//刷新对应档位次数//
	public void UpdateRotNum()
	{
		actWheelPanel.GetComponent<ActWheelPanel>().UpdateWheelCellData(id,rotNum,curIndex);
	}
}
