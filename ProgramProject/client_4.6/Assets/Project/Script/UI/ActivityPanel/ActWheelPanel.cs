using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActWheelPanel : MonoBehaviour {
	
	public GameObject activityPanel;
	
	public GameObject objGrid;
	public GameObject wheelPanel;
	public GameObject noWheelPanel;
	public UILabel actRuleLabel;
	
	//int activityId;
    //ActTopIcon topIcon;
	List<ActWheelCell> wheelCells;
	
	const string ActWheelCellPath = "Prefabs/UI/ActivityPanel/actWheelCell";
	GameObject wheelCellPrefab;
	RunnerUIResultJson rUIResultJson;
	//当前激活的活动类型//
	int activeType;
	
	int curMsgNum;
	//当前移动速度//
	float curFloatingSpeed;
	List<Announce> actTempAnnounces;
	List<Announce> actAnnounces;
	
	public GameObject marqueeGroup;
	public UILabel marqueeText;
	
	float mTextLocalScale;
	
	bool isTextMoving;
	
	void Awake()
	{
		wheelCells = new List<ActWheelCell>();
		actTempAnnounces = new List<Announce>();
		actAnnounces = new List<Announce>();
	}
	
	// Use this for initialization
	void Start () {
		
		mTextLocalScale = marqueeText.transform.localScale.x;
		InvokeRepeating("RequestAnnounceInfo", 0, 60.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Init(GameObject actPanel,RunnerUIResultJson rTempJson,int activityId, ActTopIcon topIcon)
    {
		this.rUIResultJson = rTempJson;
		//this.activityId = activityId;
		//this.topIcon = topIcon;
		this.activityPanel = actPanel;
		
		wheelPanel.SetActive(false);
		noWheelPanel.SetActive(false);
		
		wheelCells.Clear();
		GameObjectUtil.destroyGameObjectAllChildrens(objGrid);
		//int curWheelCellNum = RunnerData.GetCurValidRunDataNum();
		activeType = RunnerData.GetCurActiveActType();
		int curWheelCellNum = rUIResultJson.list.Count;
		for(int i=0;i<curWheelCellNum;i++)
		{
			if(wheelCellPrefab == null)
			{
				wheelCellPrefab = Resources.Load(ActWheelCellPath) as GameObject;
			}
			GameObject obj = Instantiate(wheelCellPrefab) as GameObject;
			obj.name = "Cell"+(1000+i).ToString();
			obj.transform.parent = objGrid.transform;
			obj.transform.localScale = Vector3.one;
			
			ActWheelCell cell = obj.GetComponent<ActWheelCell>();
			int id = rUIResultJson.list[i].id;
			int rotNum = rUIResultJson.list[i].num;
			cell.Init(id,activeType,activityPanel,wheelPanel,noWheelPanel,rotNum,i);
			
			obj.GetComponent<UIButtonMessage>().param = id;
			
			//Debug.Log("runner data gear:"+RunnerData.getData(i+1).gear);
		}
		objGrid.GetComponent<UIGrid>().repositionNow = true;
		
		actRuleLabel.text = TextsData.getData(697).chinese;
	}
	
	//更新风车转动次数//
	public void UpdateWheelCellData(int id, int rotNum,int index)
	{
		foreach(Transform trans in objGrid.transform)
		{
			if(trans.GetComponent<UIButtonMessage>().param == id)
			{
				trans.GetComponent<ActWheelCell>().Init(id,activeType,activityPanel,wheelPanel,noWheelPanel,rotNum,index);
			}
		}
	}
	
	void RequestAnnounceInfo()
	{
		if(GameObject.Find("MarqueePanel"))
		{
			GameObject marqueeObj = GameObject.Find("MarqueePanel");
			actTempAnnounces = marqueeObj.GetComponent<MarqueeControl>().actAnnounces;
			marqueeGroup.SetActive(true);
			RefreshActAnnounceList();
			ShowMarqueeLogic();
		}
	}
	
	void RefreshActAnnounceList()
	{
		for(int i=0;i<actTempAnnounces.Count;i++)
		{
			Announce tAnn = actTempAnnounces[i];
			actAnnounces.Add(tAnn);
		}
	}
	
	void ShowMarqueeLogic()
	{
		curMsgNum = actAnnounces.Count;
		if(isTextMoving)
		{
			return;
		}
		if(actAnnounces.Count>0)
		{
			ShowMarqueeText(actAnnounces[0].announce);
		}
		else
		{
			marqueeGroup.SetActive(false);
		}
	}
	
	void ShowMarqueeText(string mText)
	{
		isTextMoving = true;
		
		marqueeText.text = mText;
		int mTextLength = NGUIText.StripSymbols(mText).Length;
		
		marqueeText.width = mTextLength * 25;
		marqueeText.transform.localPosition = new Vector3(600,-6,0);
		Vector3 tarLocalPos = new Vector3(-marqueeText.width*mTextLocalScale,-6,0);
		if(curMsgNum>10)
		{
			curFloatingSpeed = 0.006f;
		}
		else if(curMsgNum>6)
		{
			curFloatingSpeed = 0.01f;
		}
		else
		{
			curFloatingSpeed = 0.02f;
		}
		iTween.MoveTo(marqueeText.gameObject,iTween.Hash("position",tarLocalPos,"islocal",true,"time",(float)curFloatingSpeed*marqueeText.width,"easetype",iTween.EaseType.linear,"oncomplete","OnShowThisComplete","oncompletetarget",gameObject,"ignoretimescale",true));
	}
	
	void OnShowThisComplete()
	{	
		if(actAnnounces.Count>0)
		{
			actAnnounces.RemoveAt(0);
			isTextMoving = false;
		}
		ShowMarqueeLogic();
	}
	
}
