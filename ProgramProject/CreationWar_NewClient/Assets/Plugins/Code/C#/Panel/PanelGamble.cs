using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelGamble : MonoBehaviour {
	public static PanelGamble panelGamble;

    /// <summary>
    /// 翻牌按钮
    /// </summary>
	public BtnDisable btnRefresh;
    /// <summary>
    /// 抽奖按钮
    /// </summary>
	public BtnDisable btnEnter;
	public UILabel lblNeedBloodRefresh;
	public UILabel lblNeedBloodEnter;
	public List<SpriteForGamble> listBox=new List<SpriteForGamble>();
	
    void Awake()
    {
        panelGamble = this;
        for (int i = 0; i < 9; i++)
        {
            listItemsInfo.Add(new InfoForGamble());
        }
    }

	void Start () {
		btnEnter.myMessage.target=this.gameObject;
		btnEnter.myMessage.functionName="BtnEnter";
		btnRefresh.myMessage.target=this.gameObject;
		btnRefresh.myMessage.functionName="GetItems";

		Invoke("ClearItems",0.1f);
		
		//listGetItems.AddRange (listBox);
		//canStartEnter=true;
	}
	
	public void show0()
	{
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage ("show0",SendMessageOptions.DontRequireReceiver);
	}
	
	void OnEnable()
	{
        SetBloodInfo();
		Invoke ("RemeberItems",0.5f);
	}

    void OnDisable()
    { 
        // 下面的逻辑是为了防止，抽奖动画还没播放完时，抽奖面板就被关闭了，导致再次打开抽奖面板点击开始抽奖按钮没反应
        if(!canStartEnter)
        {
            canStartEnter=true;
            listGetItems.Remove(targetItem);
            listHadItems.Add(targetItem);

            if (null != targetItem.itemID)
            {
                PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("CategoryTipsAsID", targetItem.itemID, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    /// <summary>
    /// 设置血石数量
    /// </summary>
    private void SetBloodInfo()
    {
        lblNeedBloodRefresh.text = "...";
        lblNeedBloodEnter.text = "...";

        InRoom.GetInRoomInstantiate().GambleInfo();
    }

    /// <summary>
    /// 服务器返回的血石数量信息
    /// </summary>
    /// <param name="cardBlood">翻牌一次消耗的血石</param>
    /// <param name="lotteryBlood">抽奖一次消耗的血石</param>
    public void ReturnBloodInfo(int cardBlood, int lotteryBlood)
    {
        lblNeedBloodRefresh.text = cardBlood.ToString();
        lblNeedBloodEnter.text = lotteryBlood.ToString();
    }

	public void RemeberItems()
	{
		for(int i=0;i<listHadItems.Count;i++)
		{
			listHadItems[i].picSelect.gameObject.active=false;
			listHadItems[i].lblIsGet.gameObject.active=true;
		}
		for(int i=0;i<listGetItems.Count;i++)
		{
			listGetItems[i].picSelect.gameObject.active=false;
			listGetItems[i].lblIsGet.gameObject.active=false;
		}

        if (0 == listHadItems.Count && 0 == listGetItems.Count)
        {
            foreach (SpriteForGamble spriteForGamble in listBox)
            {
                spriteForGamble.picSelect.gameObject.SetActive(false);
                spriteForGamble.lblIsGet.gameObject.SetActive(false);
            }
        }
	}
	
	public void ClearItems()
	{
		for(int i=0;i<listBox.Count;i++)
		{
			listBox[i].spriteBenefits.spriteName="UIB_Bar_items1";
			listBox[i].lblNum.text="";
			listBox[i].MyLevel=1;
			listBox[i].picSelect.gameObject.active=false;
			listBox[i].lblIsGet.gameObject.active=false;
		}

		btnEnter.Disable=true;

        //lblNeedBloodRefresh.text="5";
        //lblNeedBloodEnter.text="50";

		numRandom=0;
		numTemp=0;
		numEnter=0;
		canStartEnter=true;
		listGetItems.Clear ();
		listHadItems.Clear ();
	}

    private int bloodStone = 0;
	public void GetItems()
	{
		if(canStartEnter)
		{
            if (int.TryParse(BtnGameManager.yt[0]["Bloodstone"].YuanColumnText, out bloodStone) && bloodStone >= 5)
            {
                //GetItemIDs();
            }

            // 请求翻牌
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GambleCard(true));

            /*
            string[] tempItemIDs = new string[9];

            for (int i = 0; i < tempItemIDs.Length; i++)
            {
                tempItemIDs[i] = listItemsInfo[i].itemID;
            }

            // 请求翻牌
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GambleCard(tempItemIDs));
            */
		}
	}

    /// <summary>
    /// 翻牌教程任务用
    /// </summary>
    public void GetItemsTask()
    {
        if (canStartEnter)
        {
            if (int.TryParse(BtnGameManager.yt[0]["Bloodstone"].YuanColumnText, out bloodStone) && bloodStone >= 0)
            {
                //GetItemIDs();
            }

            // 请求翻牌
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GambleCard(false));
        }
    }

    /// <summary>
    /// 获取随机生成的九宫格装备的id
    /// </summary>
    public void GetItemIDs()
    {
        ClearItems();
        int num = random.Next(0, 9);
        for (int i = 0; i < 9; i++)
        {
            if (i == num)
            {

                object[] objList = new object[2];
                objList[0] = random.Next(3, 5);
                objList[1] = listBox[i];
                PanelStatic.StaticBtnGameManager.invcl.SendMessage("MakeInvItemRandom", objList, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                object[] objList = new object[2];
                objList[0] = 2;
                objList[1] = listBox[i];
                PanelStatic.StaticBtnGameManager.invcl.SendMessage("MakeInvItemRandom", objList, SendMessageOptions.DontRequireReceiver);
            }
            listGetItems.Add(listBox[i]);
            listItemsInfo[i].itemID = listBox[i].itemID;
            listItemsInfo[i].picName = listBox[i].spriteBenefits.spriteName;
        }
        //ClearItems();
        btnEnter.Disable = true;
    }

    /// <summary>
    /// 翻牌请求返回成功时调用此方法
    /// </summary>
    public void RunGetItems(string[] itemIds)
    {
        /*
        for (int i = 0; i < listBox.Count;i++ )
        {
            listBox[i].itemID = listItemsInfo[i].itemID;
            listBox[i].spriteBenefits.spriteName = listItemsInfo[i].picName;
        }
        btnEnter.Disable = false;
        */
        ClearItems();
        for (int i = 0; i < itemIds.Length; i++)
        {
            object[] objList = new object[2];
            objList[0] = itemIds[i];
            objList[1] = listBox[i];
            PanelStatic.StaticBtnGameManager.invcl.SendMessage("ShowInvItemRandom", objList, SendMessageOptions.DontRequireReceiver);

            listGetItems.Add(listBox[i]);
            listItemsInfo[i].itemID = listBox[i].itemID;
            listItemsInfo[i].picName = listBox[i].spriteBenefits.spriteName;
        }

        for (int i = 0; i < listBox.Count; i++)
        {
            listBox[i].itemID = listItemsInfo[i].itemID;
            listBox[i].spriteBenefits.spriteName = listItemsInfo[i].picName;
        }
        btnEnter.Disable = false;
    }
	
	private SpriteForGamble selectItem;
    private SpriteForGamble targetItem;// 中奖的item
    private List<InfoForGamble> listItemsInfo = new List<InfoForGamble>();
	private List<SpriteForGamble> listGetItems=new List<SpriteForGamble>();
	private List<SpriteForGamble> listHadItems=new List<SpriteForGamble>();
	private System.Random random=new System.Random((int)System.DateTime.Now.Ticks);
	private int numRandom=0;
	private int numTemp=0;
	private int numEnter=0;
	public static bool canStartEnter=false;

    /// <summary>
    /// 点击开始抽奖按钮时调用的方法
    /// </summary>
    public void BtnEnter()
    {
        int index = 0;
        if (!btnEnter.Disable && canStartEnter && listGetItems.Count > 0)
        {
            canStartEnter = false;
            if (listGetItems.Count > 1)
            {
                while (numRandom == numTemp)
                {
                    numRandom = random.Next(0, listGetItems.Count);
                }
                targetItem = listGetItems[numRandom];

                index = listBox.IndexOf(targetItem);
            }
            else
            {
                targetItem = listGetItems[0];

                index = listBox.IndexOf(targetItem);
            }

            numRandom = random.Next(0, listGetItems.Count);
            listGetItems[numRandom].picSelect.gameObject.active = true;
            selectItem = listGetItems[numRandom];
            numTemp = numRandom;

            //PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GambleLottery(targetItem.itemID,index, true));
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GambleLottery(true));
        }
    }

    /// <summary>
    /// 教程任务调用
    /// </summary>
    public void BtnEnterTask()
    {
        int index = 0;
        if (!btnEnter.Disable && canStartEnter && listGetItems.Count > 0)
        {
            canStartEnter = false;
            if (listGetItems.Count > 1)
            {
                while (numRandom == numTemp)
                {
                    numRandom = random.Next(0, listGetItems.Count);
                }
                targetItem = listGetItems[numRandom];

                index = listBox.IndexOf(targetItem);
            }
            else
            {
                targetItem = listGetItems[0];

                index = listBox.IndexOf(targetItem);
            }

            numRandom = random.Next(0, listGetItems.Count);
            listGetItems[numRandom].picSelect.gameObject.active = true;
            selectItem = listGetItems[numRandom];
            numTemp = numRandom;

            //PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GambleLottery(targetItem.itemID, index, false));
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GambleLottery(false));
        }
    }

    /// <summary>
    /// 服务器返回开始抽奖成功调用的方法
    /// </summary>
    public void StartLottery(string itemID)
    {
        StartCoroutine(RunLottery(itemID));
    }

    private IEnumerator RunLottery(string itemID)
	{
        targetItem = null;// 这里只是为了不注释BtnEnter方法中的相关逻辑，免得引起bug，targetItem是通过服务器下发选中item的id，然后判断九个中哪个是targetItem
        if(null != selectItem)
        {
	        selectItem.picSelect.gameObject.SetActive(false);
        }

        if (null != targetItem)
        {
            targetItem.picSelect.gameObject.SetActive(false);
        }
        else
        {
            for(int i=0;i<listGetItems.Count;i++)
            {
                if(listGetItems[i].itemID.Equals(itemID))
                {
                    targetItem = listGetItems[i];
                }
            }
        }

        if(listGetItems.Count>1)
        {
	        yield return new WaitForSeconds(0.1f);
	        FlashItem ();
	        yield return new WaitForSeconds(0.1f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.1f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.1f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.1f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.1f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.1f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.2f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.3f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.4f);
	        FlashItem ();	
	        yield return new WaitForSeconds(0.5f);
	        FlashItem ();	
	        if(numEnter<3&&selectItem.MyLevel>2)
	        {
		        FlashItem ();
	        }
            selectItem.picSelect.gameObject.active = false;
            selectItem = targetItem;
                        
	        listGetItems.Remove (selectItem);
	        listHadItems.Add(selectItem);

            selectItem.picSelect.gameObject.active = true;
	        selectItem.lblIsGet.gameObject.active=true;
        }
        else
        {
	        listGetItems[0].picSelect.gameObject.active=true;
	        listGetItems[0].lblIsGet.gameObject.active=true;
	        selectItem=listGetItems[0];
	        listGetItems.RemoveAt (0);
	        listHadItems.Add(selectItem);
        }

        if(listGetItems.Count==0)
        {
	        btnEnter.Disable=true;
        }
        canStartEnter=true;

        PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("CategoryTipsAsID", itemID, SendMessageOptions.DontRequireReceiver);
	}

    /// <summary>
    /// 前一次超时，后一次抽奖将刷新九宫格
    /// </summary>
    /// <param name="index"></param>
    public void RefreshSudoku(int index)
    {
        if(index < listBox.Count)
        {
            selectItem = listBox[index];
            selectItem.picSelect.gameObject.active = true;
            selectItem.lblIsGet.gameObject.active = true;
            listGetItems.Remove(selectItem);
            listHadItems.Add(selectItem);
        } 
    }

	public AudioClip impact;
	public void FlashItem()
	{
		audio.PlayOneShot(impact);
		selectItem.picSelect.gameObject.active=false;
		numRandom=random.Next (0,listGetItems.Count);
		while(numRandom==numTemp)
		{
			numRandom=random.Next (0,listGetItems.Count);
		}
		numTemp=numRandom;
		listGetItems[numRandom].picSelect.gameObject.active=true;
		selectItem=listGetItems[numRandom];
	}
	
	public static bool IsBagFull()
	{
		int numPage=int.Parse (BtnGameManager.yt.Rows[0]["InventoryNum"].YuanColumnText);
		for(int i=0;i<numPage;i++)
		{
			string[] strInv=BtnGameManager.yt.Rows[0][string.Format ("Inventory{0}",(i+1).ToString ())].YuanColumnText.Split (';');
			for(int j=0;j<15;j++)
			{
				if(strInv[j].Trim ()=="")
				{
					return false;
				}
			}
		}
		return true;
	}
	
	
}
