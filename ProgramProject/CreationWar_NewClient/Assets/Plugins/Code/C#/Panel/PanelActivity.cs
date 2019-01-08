using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TaskFunType
{
    CanAccept=0,
    Doding=1,
    CanConsign=2,
    Done,
}

public class PanelActivity : MonoBehaviour {
    public static PanelActivity panelActivity;

	public static Dictionary<string, int> activityIDAndState = new Dictionary<string, int>();// 活动的公共数据
    public static List<string> activityReward = new List<string>();// 当前玩家的可领取奖励的活动列表，需要和当前玩家绑定

    public List<string> ActivityReward
    {
        get { return activityReward; }
        set { activityReward = value; }
    }

    public UILabel lblTitle;
    public UILabel lblActivityInfo;
    public UILabel lblActivityTime;
    public UIGrid gridReward;
    public UIGrid gridActivity;
	public UIScrollView dragReward;
    public UIButtonMessage btnMessEnter;
    [HideInInspector]
    public yuan.YuanMemoryDB.YuanTable yt ;
    public yuan.YuanMemoryDB.YuanRow yrSelect;
    public GameObject ckbActivity;
    public Transform ActivityType;

    public bool isActivity = true;

    public BtnActivity btnActivity;

	public bool isRefresh=false;
	
	void Awake()
	{
        panelActivity = this;

        activityReward.Clear();
        
        invMaker = PanelStatic.StaticBtnGameManager.InvMake;
        yt=YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytActivity;
	}
	
    void Start()
    {
      
        //yuan.YuanMemoryDB.YuanRow yr = new yuan.YuanMemoryDB.YuanRow();
        //yr.Add("ActivityName", "首充活动");
        //yr.Add("ActivityInfo", "");
        //yr.Add("ActivityTimeStart", "");
        //yr.Add("ActivityTimeEnd", "");
        //yr.Add("AcitvityTool1", "");
        //yr.Add("AcitvityTool2", "");
        //yr.Add("AcitvityTool3", "");
        //yr.Add("isStart", "1");
        //yr.Add("ActivityTaskID", "-1");
        //if(btnActivity!=null)
        //{
        //btnActivity.yr = yr;
        //btnActivity.MyMessage.target = this.gameObject;
        //btnActivity.MyMessage.functionName = "BtnActivityClick";
        //BtnActivityClick(btnActivity.gameObject);
        //}

        //string[] canFinishActivity = BtnGameManager.yt[0]["CanFinishActivity"].YuanColumnText.Split(';');
        //if(canFinishActivity.Length > 0)
        //{
        //    activityReward.AddRange(canFinishActivity);
        //}
    }
	void Update()
	{
		if(isRefresh)
		{
			isRefresh=false;
			RefreshActivityGrid ();
		}
	}

    private void OnEnable()
    {
        SetRewardBoxActive(false);

        string[] canFinishActivity = BtnGameManager.yt[0]["CanFinishActivity"].YuanColumnText.Split(';');
        if (canFinishActivity.Length > 0)
        {
            activityReward.AddRange(canFinishActivity);
        }

        if (isActivity)
        {
            //StartCoroutine("LoadAcitvity");
//			LoadAcitvity ();
        }
		
		//ShowActivityType();
		RefreshActivityGrid();

        EnableItemCollider(true);

        //Transform firstObj = gridActivity.transform.GetChild(0);
        //if (null != firstObj)
        //{
        //    firstObj.SendMessage("OnClick", SendMessageOptions.RequireReceiver);
        //}

        StartCoroutine(FirstItemClick());
    }

    IEnumerator FirstItemClick()
    {
        while (null == ActivityControl.activityControl)
        {
            yield return null; 
        }

        yield return null;
        for (int i=0 ;i<gridActivity.transform.childCount; i++)
        {
            Transform child = gridActivity.transform.GetChild(i);

            if (child)
            {
                child.GetComponent<UIToggle>().value = false;
            }
        }

        yield return null;
        Transform firstChild = gridActivity.transform.GetChild(0);

        if (null != firstChild && firstChild.gameObject.activeSelf)
        {
            firstChild.SendMessage("OnClick", SendMessageOptions.RequireReceiver);
        }
    }

    private Dictionary<string, GameObject> activityItems = new Dictionary<string, GameObject>();

    public Dictionary<string, GameObject> ActivityItems
    {
        get { return activityItems; }
        set { activityItems = value; }
    }

    /// <summary>
    /// 当正在排队中时，左边的item不允许再次点击
    /// </summary>
    /// <param name="isEnable"></param>
    public void EnableItemCollider(bool isEnable)
    {
        foreach(GameObject go in activityItems.Values)
        {
            go.collider.enabled = isEnable;
        }
    }

    /// <summary>
    /// 增加活动
    /// </summary>
    /// <param name="id">活动id</param>
    /// <param name="name">活动名称</param>
    /// <param name="time">活动时间</param>
    public void AddActivityItem(string id, string name, string time)
    {
        //Debug.Log("=====================AddActivityItem==========" + id);
        GameObject activityItem = null;
        //当同一活动变化时，字典中的对象也要跟着变化
        if (!activityItems.TryGetValue(id, out activityItem))
        {
            activityItem = (GameObject)Instantiate(ckbActivity);
            activityItems.Add(id, activityItem);
        }
         
        BtnActivity btnActivity = activityItem.GetComponent<BtnActivity>();
        btnActivity.transform.parent = ActivityType;
        btnActivity.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        btnActivity.transform.localPosition = Vector3.zero;
        btnActivity.lblText.text = name;
        btnActivity.lblTime.text = time;
        btnActivity.activityID = id;
        btnActivity.canGetReward = activityReward.Contains(id) ? true : false;
        btnActivity.MyMessage.target = this.gameObject;
        btnActivity.MyMessage.functionName = "BtnActivityOnClick";
        listBtnActivity.Add(btnActivity);

        //gridActivity.repositionNow = true;
    }

    /// <summary>
    /// 移除活动
    /// </summary>
    /// <param name="id">活动id</param>
    public void RemoveActivityItem(string id)
    {
        GameObject item = null;
        if(activityItems.TryGetValue(id, out item))
        {
            activityIDAndState.Remove(id);
            activityItems.Remove(id);
            Destroy(item);
        }
		
        //gridActivity.repositionNow = true;
    }

    public void AddActivityReward(string id)
    {
        activityReward.Add(id);
        BtnGameManager.yt[0]["CanFinishActivity"].YuanColumnText = string.Format("{0};{1}", BtnGameManager.yt[0]["CanFinishActivity"].YuanColumnText, id);
        GameObject item = null;
        if (activityItems.TryGetValue(id, out item))
        {
            item.GetComponent<BtnActivity>().canGetReward = true;
        }
    }

    /// <summary>
    /// 刷新活动列表
    /// </summary>
	void RefreshActivityGrid()
    {
        //Debug.Log("==============RefreshActivityGrid==========" + activityIDAndState.Count);
		foreach (KeyValuePair<string, int> kvp in activityIDAndState)
		{
			string activityID = kvp.Key;
			int activityState = kvp.Value;
			string activityName = null;
			string activityTimeStart = null;
            int activityMinLevel = -1;
            int activityMaxLevel = -1;
            int playerLevel = -1;
			yuan.YuanMemoryDB.YuanRow yrActivity = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytActivity.SelectRowEqual("id", activityID);

            playerLevel = int.Parse(BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText);
			if (yrActivity != null)
			{
				activityName = yrActivity["ActivityName"].YuanColumnText;
                //activityTimeStart = yrActivity["ActivityTimeStart"].YuanColumnText;
                activityTimeStart = StaticLoc.Loc.Get("meg0174") + yrActivity["ActivityTimePeriods"].YuanColumnText;
                activityMinLevel = int.Parse(yrActivity["ActivityLevel"].YuanColumnText);
                activityMaxLevel = int.Parse(yrActivity["ActivityLevelEnd"].YuanColumnText);
			}

            if (1 == activityState && playerLevel >= activityMinLevel && playerLevel <= activityMaxLevel)
			//if (1 == activityState)
            {
				AddActivityItem(activityID, activityName, activityTimeStart);
			}
            else if (2 == activityState || playerLevel < activityMinLevel || playerLevel > activityMaxLevel)
			//else if (2 == activityState)
            {
				RemoveActivityItem(activityID);
			}

            gridActivity.repositionNow = true;
		}
    }

    public const int ACTIVITY_TYPE_BATTLEFIELD = 1;//"战场活动";
    public const int ACTIVITY_TYPE_TASK = 2;//"任务活动";
    public const int ACTIVITY_TYPE_NORAML = 4;//"玩法活动";
    public const int ACTIVITY_TYPE_BOSS = 3;//"BOSS活动";

    /// <summary>
    /// 活动Item被点击
    /// </summary>
    /// <param name="sender"></param>
    public void BtnActivityOnClick(GameObject sender)
    {
        BtnActivity btnActivity = sender.GetComponent<BtnActivity>();
        yuan.YuanMemoryDB.YuanRow yrActivity = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytActivity.SelectRowEqual("id", btnActivity.activityID);

        string activityInfo = "";//活动内容
        string activityConditionInfo = "";//活动条件
        string victoryCondition = "";// 胜利条件
        string[] activityTool = new string[3];//三种奖励物品
        int activityType = 0;//活动类型

        if (yrActivity != null)
        {
            activityInfo = yrActivity["ActivityInfo"].YuanColumnText;
            activityConditionInfo = yrActivity["ActivityConditionInfo"].YuanColumnText;
            victoryCondition = yrActivity["winInfo"].YuanColumnText;

            activityTool[0] = yrActivity["AcitvityTool1"].YuanColumnText;
            activityTool[1] = yrActivity["AcitvityTool2"].YuanColumnText;
            activityTool[2] = yrActivity["AcitvityTool3"].YuanColumnText;

            listItem.Clear();
            if (!string.IsNullOrEmpty(activityTool[0]))
            {
                listSprite[0].gameObject.SetActive(true);
                string[] tool1 = activityTool[0].Split(',');
                listItem.Add(tool1[0] + ",01");
            }
            else
            {
                listSprite[0].gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(activityTool[1]))
            {
                listSprite[1].gameObject.SetActive(true);
                string[] tool2 = activityTool[1].Split(',');
                listItem.Add(tool2[0] + ",01");
            }
            else
            {
                listSprite[1].gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(activityTool[2]))
            {
                listSprite[2].gameObject.SetActive(true);
                string[] tool3 = activityTool[2].Split(',');
                listItem.Add(tool3[0] + ",01");
            }
            else
            {
                listSprite[2].gameObject.SetActive(false);
            }

            activityType = int.Parse(yrActivity["ActivityType"].YuanColumnText);
        }

        BtnEnterActivity.activityID = btnActivity.activityID;
        BtnEnterActivity.activityType = activityType;

        ActivityControl.activityControl.ShowActivityInfo(activityInfo, activityConditionInfo, victoryCondition, activityTool);
        if (btnActivity.canGetReward && activityType != ACTIVITY_TYPE_BOSS)
        {
            BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.GetReward);//如果能领取奖励按钮上就显示领取奖励
        }
        else if (btnActivity.isCancelQueue)
        {
            BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.CancelQueue); // 如果已经排过队了，按钮状态需要变成退出排队状态
        }
        else
        {
            if (activityType == ACTIVITY_TYPE_NORAML || activityType == ACTIVITY_TYPE_BOSS)
            {
                BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.JoinActivity);
            }
            else if (activityType == ACTIVITY_TYPE_BATTLEFIELD)
            {
                BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.ActivityQueue);
            }
            else if (activityType == ACTIVITY_TYPE_TASK)
            {
                //BtnGameManagerBack.my.taskActivityState

                string temp = BtnGameManager.yt.Rows[0]["EveryDayTaskActivity"].YuanColumnText;
                if(!temp.Equals(""))
                {
                    string[] active = temp.Split(';');

                    if (temp.IndexOf(btnActivity.activityID)!=-1)
                    {
                        for (int i = 0; i < active.Length; i++)
                        {
                            string[] singleActive = active[i].Split(',');
                            if (btnActivity.activityID == singleActive[0])
                            {
                                if (0 == int.Parse(singleActive[1]))
                                {
                                    BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.ContinueActivity);
                                }
                                else if (1 == int.Parse(singleActive[1]))
                                {
                                    BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.GetReward);
                                }
                                if (2 == int.Parse(singleActive[1]))
                                {
                                    BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.WasDone);
                                }
                            }
                        }
                    }
                    else
                    {
                        BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.JoinActivity);
                    }
                }
                else
                {
                    BtnEnterActivity.btnEnterActivity.SwitchBtnState(BtnState.JoinActivity);
                }
            }
        }
        
        // BtnEnterActivity.activityID = btnActivity.activityID;
    }

    /// <summary>
    /// 设置三个奖励框的显示状态
    /// </summary>
    /// <param name="isActive"></param>
    void SetRewardBoxActive(bool isActive)
    {
        foreach(SpriteForBenefits sfb in listSprite)
        {
            sfb.gameObject.SetActive(isActive);
        }
    }

    /// <summary>
    /// 设置左边活动Item,是否处于可取消排队状态
    /// </summary>
    public void SetItemCancelState(string activityID, bool isCancel)
    {
        GameObject activityItem = null;
        if (activityItems.TryGetValue(activityID, out activityItem))
        {
            BtnActivity btnActivity = activityItem.GetComponent<BtnActivity>();
            btnActivity.isCancelQueue = isCancel;
        }
    }
	
	#region=====================目前只有一个活动，所以这个地方写死了===============
	private GameObject tempObj1;
	private void ShowActivityType()
	{
		if(tempObj1==null){
		    tempObj1 = (GameObject)Instantiate(ckbActivity);
		}
        BtnActivity btnTemp1 = tempObj1.transform.GetComponent<BtnActivity>();
        btnTemp1.transform.parent = ActivityType;
        btnTemp1.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        btnTemp1.transform.localPosition = Vector3.zero;
        //btnTemp1.MyCheckbox.radioButtonRoot = this.transform;
//        SetBtnAcitivty(btnTemp1, yr);
		btnTemp1.lblText.text = StaticLoc.Loc.Get("info759");
        btnTemp1.lblTime.text = "";
		btnTemp1.MyMessage.target = this.gameObject;
        btnTemp1.MyMessage.functionName = "BtnActivityClick";
        listBtnActivity.Add(btnTemp1);
		
		gridActivity.repositionNow = true;
	}
	#endregion

    /// <summary>
    /// Ìí¼Ó½±ÀøÎïÆ·
    /// </summary>
    /// <param name="item"></param>
	public void AddItem(UIDragScrollView item)
    {
        item.transform.parent = gridReward.transform;
        item.transform.localScale = new Vector3(1, 1, 1);
        item.draggablePanel = this.dragReward;
        gridReward.repositionNow = true;
    }


    private List<BtnActivity> listBtnActivity = new List<BtnActivity>();
    /// <summary>
    /// ¶ÁÈ¡»î¶¯
    /// </summary>
    /// <returns></returns>
    private void LoadAcitvity()
    {
        foreach (BtnActivity item in listBtnActivity)
        {
            item.gameObject.SetActiveRecursively(false);
        }
		
			
        //InRoom.GetInRoomInstantiate().GetYuanTable("Select * from Activity", "DarkSword2", yt);
        //while (yt.IsUpdate)
        //{
        //    yield return new WaitForSeconds(0.5f);
        //}
        int num = 0;
		
		int myLevel=InRoom.isUpdatePlayerLevel?InRoom.playerLevel.Parse (0) :int.Parse (BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText);
		int levelStart;
		int levelEnd;
		
		List<yuan.YuanMemoryDB.YuanRow> listType0=yt.SelectRowsListEqual("Type", "0");
		
		if(listType0!=null)
		{
	        foreach (yuan.YuanMemoryDB.YuanRow yr in listType0)
	        {
	            if (yr["isStart"].YuanColumnText.Trim() == "1")
	            {
					levelStart=int.Parse (yr["ActivityLevel"].YuanColumnText);
					levelEnd=int.Parse (yr["ActivityLevelEnd"].YuanColumnText);
					if(myLevel>=levelStart&&myLevel<=levelEnd)
					{
		                if (num < listBtnActivity.Count)
		                {
		                    listBtnActivity[num].gameObject.SetActiveRecursively(true);
		                    SetBtnAcitivty(listBtnActivity[num], yr);
		                }
		                else
		                {
		                    GameObject tempObj = (GameObject)Instantiate(ckbActivity);
		                    BtnActivity btnTemp = tempObj.transform.GetComponent<BtnActivity>();
		                    btnTemp.transform.parent = ActivityType;
		                    btnTemp.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		                    btnTemp.transform.localPosition = Vector3.zero;
		                    //btnTemp.MyCheckbox.radioButtonRoot = this.transform;
		                    SetBtnAcitivty(btnTemp, yr);
		                    listBtnActivity.Add(btnTemp);
		                }
	                	num++;
					}
	            }
	        }
		}
		
		List<yuan.YuanMemoryDB.YuanRow> listType1=yt.SelectRowsListEqual("Type", "1");
		if(listType1!=null)
		{
	        foreach (yuan.YuanMemoryDB.YuanRow yr in listType1)
	        {
				
	            if (yr["isStart"].YuanColumnText.Trim() == "1")
	            {
					levelStart=int.Parse (yr["ActivityLevel"].YuanColumnText);
					levelEnd=int.Parse (yr["ActivityLevelEnd"].YuanColumnText);
					if(myLevel>=levelStart&&myLevel<=levelEnd)
					{
		                if (num < listBtnActivity.Count)
		                {
		                    listBtnActivity[num].gameObject.SetActiveRecursively(true);
		                    SetBtnAcitivty(listBtnActivity[num], yr);
		                }
		                else
		                {
		                    GameObject tempObj = (GameObject)Instantiate(ckbActivity);
		                    BtnActivity btnTemp = tempObj.transform.GetComponent<BtnActivity>();
		                    btnTemp.transform.parent = ActivityType;
		                    btnTemp.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		                    btnTemp.transform.localPosition = Vector3.zero;
		                    //btnTemp.MyCheckbox.radioButtonRoot = this.transform;
		                    SetBtnAcitivty(btnTemp, yr);
		                    listBtnActivity.Add(btnTemp);
		                }
		                num++;
					}
	            }
	        }
		}
		
		List<yuan.YuanMemoryDB.YuanRow> listType2=yt.SelectRowsListEqual("Type", "2");
		if(listType1!=null)
		{
	        foreach (yuan.YuanMemoryDB.YuanRow yr in listType2)
	        {
				
	            if (yr["isStart"].YuanColumnText.Trim() == "1")
	            {
					levelStart=int.Parse (yr["ActivityLevel"].YuanColumnText);
					levelEnd=int.Parse (yr["ActivityLevelEnd"].YuanColumnText);
					if(myLevel>=levelStart&&myLevel<=levelEnd)
					{
		                if (num < listBtnActivity.Count)
		                {
		                    listBtnActivity[num].gameObject.SetActiveRecursively(true);
		                    SetBtnAcitivty(listBtnActivity[num], yr);
		                }
		                else
		                {
		                    GameObject tempObj = (GameObject)Instantiate(ckbActivity);
		                    BtnActivity btnTemp = tempObj.transform.GetComponent<BtnActivity>();
		                    btnTemp.transform.parent = ActivityType;
		                    btnTemp.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		                    btnTemp.transform.localPosition = Vector3.zero;
		                    //btnTemp.MyCheckbox.radioButtonRoot = this.transform;
		                    SetBtnAcitivty(btnTemp, yr);
		                    listBtnActivity.Add(btnTemp);
		                }
		                num++;
					}
	            }
	        }
		}
		
        gridActivity.repositionNow = true;
    }

    /// <summary>
    /// ÉèÖÃ»î¶¯°´Å¥
    /// </summary>
    /// <param name="btn">°´Å¥</param>
    /// <param name="yr">±í</param>
    private void SetBtnAcitivty(BtnActivity btn,yuan.YuanMemoryDB.YuanRow yr)
    {
        btn.yr = yr;
        btn.lblText.text = btn.yr["ActivityName"].YuanColumnText;
        btn.lblTime.text = btn.yr["ActivityTimeStart"].YuanColumnText.Substring (9,btn.yr["ActivityTimeStart"].YuanColumnText.Length-9) + "-" + btn.yr["ActivityTimeEnd"].YuanColumnText.Substring (9,btn.yr["ActivityTimeEnd"].YuanColumnText.Length-9);

//        if (BtnGameManager.yt.Rows[0]["CompletTask"].YuanColumnText.IndexOf( btn.yr["ActivityTaskID"].YuanColumnText) == -1)
//        {
//            btn.btnDisable.Disable = false;
//        }
//        else
//        {
//            btn.btnDisable.Disable = true;
//        }

        btn.MyMessage.target = this.gameObject;
        btn.MyMessage.functionName = "BtnActivityClick";
    }


    public SpriteForBenefits SpriteForBenefits;
    public List<SpriteForBenefits> listSprite = new List<SpriteForBenefits>();
    public GameObject invMaker;
    public BtnDisable btnEnter;
    public UILabel lblEnter;
    public TaskFunType taskFunType = TaskFunType.Done;
	public List<string> listItem=new List<string>();
    private void BtnActivityClick(GameObject sender)
    {
        BtnActivity tempBtn = sender.GetComponent<BtnActivity>();
        if (tempBtn != null&&tempBtn.yr!=null&&tempBtn.yr.ContainsKey ("ActivityName"))
        {
            lblTitle.text = tempBtn.yr["ActivityName"].YuanColumnText;
            lblActivityInfo.text = tempBtn.yr["ActivityInfo"].YuanColumnText;
            lblActivityTime.text = tempBtn.yr["ActivityTimeStart"].YuanColumnText.Substring (9,tempBtn.yr["ActivityTimeStart"].YuanColumnText.Length-9)+ "-" + tempBtn.yr["ActivityTimeEnd"].YuanColumnText.Substring (9,tempBtn.yr["ActivityTimeEnd"].YuanColumnText.Length-9);
            yrSelect = tempBtn.yr;
//			if(yrSelect["Type"].YuanColumnText=="2")
//			{
//				SetTaskFun("ActivtyPVP");
//			}
//			else
//			{
            	SetTaskFun();
//			}
			listItem.Clear ();
            for (int i = 1; i < 4; i++)
            {
                if (tempBtn.yr["AcitvityTool" + i.ToString()].YuanColumnText != "")
                {
                    string[] strTemp=tempBtn.yr["AcitvityTool" + i.ToString()].YuanColumnText.Split(',');
                    listSprite[i-1].gameObject.SetActiveRecursively(true);

                    if (strTemp.Length > 1)
                    {
                        listSprite[i - 1].lblNum.text = strTemp[1];
                    }
                    else
                    {
                        listSprite[i - 1].lblNum.text = "01";
                    }
                    listSprite[i-1].itemID=strTemp[0] + ",01";
					listItem.Add (strTemp[0] + ",01");
                    object[] parms=new object[2];
                    parms[0] = strTemp[0] + ",01";
                    parms[1]=listSprite[i-1].spriteBenefits;
                    invMaker.SendMessage("SpriteName", parms, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    listSprite[i-1].gameObject.SetActiveRecursively(false);
                }
            }
            gridReward.repositionNow = true;
        }
    }
	
	/// <summary>
	/// 获得奖励
	/// </summary>
	public void SetItem()
	{
		foreach(string item in listItem)
		{
			if(item.Substring (0,2)=="88")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewDaojuItemAsID", item, SendMessageOptions.DontRequireReceiver);
			}
			else if(item.Substring (0,2)=="72")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewRideItemAsID", item, SendMessageOptions.DontRequireReceiver);
			}
			else if(item.Substring (0,2)=="70")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagDigestItemAsID",  item, SendMessageOptions.DontRequireReceiver);
			}
			else if(item.Substring (0,2)=="71")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagSoulItemAsID",  item, SendMessageOptions.DontRequireReceiver);
			}			
			else
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagItemAsID", item, SendMessageOptions.DontRequireReceiver);
			}
		}
		PanelStatic.StaticWarnings.OpenBoxBar("0","0",listItem.ToArray ());
	}
	

	
    /// <summary>
    /// 设置按钮权限
    /// </summary>
    public void SetTaskFun()
    {
      
            if (BtnGameManager.yt.Rows[0]["ActivtyTask"].YuanColumnText.IndexOf(yrSelect["ActivityTaskID"].YuanColumnText) == -1)
            {
				     lblEnter.text = StaticLoc.Loc.Get("info328")+"";
                        taskFunType = TaskFunType.CanAccept;
                        btnEnter.Disable = false;
            }
            else
            {
			    int taskID = GetTaskFun(yrSelect["ActivityTaskID"].YuanColumnText,"ActivtyTask");
                switch (taskID)
                {
                    case 0:
                        lblEnter.text = StaticLoc.Loc.Get("info328")+"";
                        taskFunType = TaskFunType.CanAccept;
                        btnEnter.Disable = false;
                        break;
                    case 1:
                        lblEnter.text = StaticLoc.Loc.Get("info329")+"";
                        taskFunType = TaskFunType.Doding;
                        btnEnter.Disable = false;
                        break;
                    case 2:
                        lblEnter.text = StaticLoc.Loc.Get("info330")+"";
                        taskFunType = TaskFunType.CanConsign;
                        btnEnter.Disable = false;
                        break;
				    case 3:
                        lblEnter.text = StaticLoc.Loc.Get("info331")+"";
                        taskFunType = TaskFunType.Done;
                		btnEnter.Disable = true;
                        break;
                }
            }
    }
	
	 /// <summary>
    /// 设置按钮权限
    /// </summary>
    public void SetTaskFun(string mRowName)
    {
      
            if (BtnGameManager.yt.Rows[0]["mRowName"].YuanColumnText.IndexOf(yrSelect["ActivityTaskID"].YuanColumnText) == -1)
            {
				     lblEnter.text = StaticLoc.Loc.Get("info328")+"";
                        taskFunType = TaskFunType.CanAccept;
                        btnEnter.Disable = false;
            }
            else
            {
				
			    int taskID = GetTaskFun(yrSelect["ActivityTaskID"].YuanColumnText,"ActivtyTask");
                switch (taskID)
                {
                    case 0:
                        lblEnter.text = StaticLoc.Loc.Get("info328")+"";
                        taskFunType = TaskFunType.CanAccept;
                        btnEnter.Disable = false;
                        break;
                    case 1:
                        lblEnter.text = StaticLoc.Loc.Get("info329")+"";
                        taskFunType = TaskFunType.Doding;
                        btnEnter.Disable = false;
                        break;
                    case 2:
                        lblEnter.text = StaticLoc.Loc.Get("info330")+"";
                        taskFunType = TaskFunType.CanConsign;
                        btnEnter.Disable = false;
                        break;
				    case 3:
                        lblEnter.text = StaticLoc.Loc.Get("info331")+"";
                        taskFunType = TaskFunType.Done;
                		btnEnter.Disable = true;
                        break;
                }
            }
    }

    /// <summary>
    /// 获得任务进度
    /// </summary>
    /// <param name="taskID"></param>
    /// <returns></returns>
    int GetTaskFun(string taskID,string mRowName)
    {
		
        string[] strTemp = BtnGameManager.yt.Rows[0]["Task"].YuanColumnText.Split(';');
        string[] strTaskID;
		foreach (string item in strTemp)
        {
            if (item != "")
            {
                strTaskID = item.Split(',');
                if (strTaskID[0] == taskID&&int.Parse (strTaskID[1])==2)
                {
                    return 2;
                }
            }
        }
		
		
		strTemp = BtnGameManager.yt.Rows[0][mRowName].YuanColumnText.Split(';');
        foreach (string item in strTemp)
        {
            if (item != "")
            {
                strTaskID = item.Split(',');
                if (strTaskID[0] == taskID)
                {
                    return int.Parse(strTaskID[1]);
                }
            }
        }
        return 0;
    }

}
