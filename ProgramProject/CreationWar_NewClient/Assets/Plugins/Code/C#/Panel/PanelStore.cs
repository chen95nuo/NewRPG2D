using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PanelStore : MonoBehaviour {

    public yuan.YuanMemoryDB.YuanTable ytStoreItem = new yuan.YuanMemoryDB.YuanTable("StoreItem", "");
    public GameObject infoBar;
    //public yuan.YuanMemoryDB.YuanTable ytGameItem = new yuan.YuanMemoryDB.YuanTable("GameItem", "");
	// Use this for initialization
	void Start () {
        ytStoreItem=YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytStoreItem;
		GetBtn ();
        //StartCoroutine("GetItemInfo");
        //Invoke("GetBtn", 5);
	}
	
	void Awake()
	{
		infoBar = PanelStatic.StaticIteminfo;
	}


    void OnEnable()
    {
        gridFavorable.repositionNow = true;
        gridItem.repositionNow = true;
    }

    
    private IEnumerator GetItemInfo()
    {
        if (!InRoom.GetInRoomInstantiate().ServerConnected)
        {
            yield return new WaitForSeconds(0.5f);
        }
        //InRoom.GetInRoomInstantiate().GetYuanTable("Select id,ItemEndTime,ItemID,ItemInfo,ItemNeedBlood,ItemNeedCash,ItemDiscount,isStart,ItemType from StoreItem", "DarkSword2", ytStoreItem);
       // InRoom.GetInRoomInstantiate().GetYuanTable("Select id,Name,ItemID,ItemInfo from GameItem", "DarkSword2", ytGameItem);
        if ( ytStoreItem.IsUpdate)
        {
            yield return new WaitForSeconds(0.5f);
        }
       
    }

	public void SetFirstFavorable()
	{
		if(listBtnFavorable.Count>0)
		{
			SetFavorableInfo (listBtnFavorable[0].gameObject);
			UIToggle ckb= listBtnFavorable[0].GetComponent<UIToggle>();
			if(ckb!=null)
			{
				ckb.value=true;
			}
		}
	}

	public void SetFirstItem()
	{
		if(listBtnItem.Count>0)
		{
			SetItemInfo (listBtnItem[0].gameObject);
			UIToggle ckb= listBtnItem[0].GetComponent<UIToggle>();
			if(ckb!=null)
			{
				ckb.value=true;
			}
		}
	}

	public void SetFirstEquipment()
	{
		if(listBtnEquipment.Count>0)
		{
			SetEtemInfo (listBtnEquipment[0].gameObject);
			UIToggle ckb= listBtnEquipment[0].GetComponent<UIToggle>();
			if(ckb!=null)
			{
				ckb.value=true;
			}
		}
	}

	public void SetFirstMonster()
	{
		if(listBtnMonster.Count>0)
		{
			SetMtemInfo (listBtnMonster[0].gameObject);
			UIToggle ckb= listBtnMonster[0].GetComponent<UIToggle>();
			if(ckb!=null)
			{
				ckb.value=true;
			}
		}
	}

    public GameObject btnItem;
    public UIGrid gridFavorable;
    public UIGrid gridItem;
	public UIGrid gridMonster;
	public List<BtnItem> listBtnFavorable=new List<BtnItem>();
	public List<BtnItem> listBtnItem=new List<BtnItem>();
	public List<BtnItem> listBtnEquipment=new List<BtnItem>();
	public List<BtnItem> listBtnMonster=new List<BtnItem>();
    private void GetBtn()
    {
        yuan.YuanMemoryDB.YuanRow yrGameItem = null;
        foreach (yuan.YuanMemoryDB.YuanRow yrStore in ytStoreItem.Rows)
        {



			if(yrStore["ItemType"].YuanColumnText.Trim() == "1")
			{
                //if (yrStore["isStart"].YuanColumnText.Trim() == "1")
                //{
                //    InsBtn(yrGameItem, yrStore, gridFavorable, "SetFavorableInfo",true);
                //}

                System.TimeSpan timeSpan = new System.TimeSpan(0, 0, 1);
                DateTime dt = DateTime.Parse(yrStore["ItemEndTime"].YuanColumnText.Trim());
                //TimeSpan ts = dt.TimeOfDay - DateTime.Now.TimeOfDay;
                TimeSpan ts = dt.TimeOfDay - InRoom.GetInRoomInstantiate().serverTime.TimeOfDay;
                ts = ts.Subtract(timeSpan);
                if (ts.Hours <= 0 && ts.Minutes <= 0 && ts.Seconds <= 0)
                {
                    // 结束打折
                }
                else
                {
                    // 正在打折
                    InsBtn(yrGameItem, yrStore, gridFavorable, "SetFavorableInfo", true,listBtnFavorable);
                }
	            InsBtn(yrGameItem, yrStore, gridItem, "SetItemInfo",true,listBtnItem);
			}
			else if(yrStore["ItemType"].YuanColumnText.Trim() == "2")
			{
				if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.PetBuySwitch)=="1")
				{
                    //if (yrStore["isStart"].YuanColumnText.Trim() == "1")
                    //{
                    //    InsBtn(yrGameItem, yrStore, gridFavorable, "SetFavorableInfo",true);
                    //}

                    System.TimeSpan timeSpan = new System.TimeSpan(0, 0, 1);
                    DateTime dt = DateTime.Parse(yrStore["ItemEndTime"].YuanColumnText.Trim());
                    //TimeSpan ts = dt.TimeOfDay - DateTime.Now.TimeOfDay;
                    TimeSpan ts = dt.TimeOfDay - InRoom.GetInRoomInstantiate().serverTime.TimeOfDay;
                    ts = ts.Subtract(timeSpan);
                    if (ts.Hours <= 0 && ts.Minutes <= 0 && ts.Seconds <= 0)
                    {
                        // 结束打折
                    }
                    else
                    {
                        // 正在打折
                        InsBtn(yrGameItem, yrStore, gridFavorable, "SetFavorableInfo", true,listBtnFavorable);
                    }

		            InsBtn(yrGameItem, yrStore, gridMonster, "SetMtemInfo",true,listBtnMonster);
				}
			}
        }
	
        gridFavorable.repositionNow = true;
        gridItem.repositionNow = true;
		gridMonster.repositionNow=true;

		SetFirstFavorable();
		SetFirstItem();
		SetFirstMonster();
    }

    private void InsBtn(yuan.YuanMemoryDB.YuanRow yrGameItem,yuan.YuanMemoryDB.YuanRow yrStore,UIGrid mGrid,string btnFunction,bool mActive,List<BtnItem> list)
    { 
        yrGameItem = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytGameItem.SelectRow("ItemID", yrStore["ItemID"].YuanColumnText.Substring(0,4));
		if(yrGameItem==null)
		{
			yrGameItem=YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytPlayerPet.SelectRow ("ItemID",yrStore["ItemID"].YuanColumnText);
		}
		if(yrGameItem==null&&yrStore["ItemID"].YuanColumnText.IndexOf ("J")!=-1)
		{
			yrGameItem=YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytBlueprint.SelectRow ("id",yrStore["ItemID"].YuanColumnText.Substring (1,yrStore["ItemID"].YuanColumnText.Length-1));
		}
        if (yrGameItem != null)
        {
            GameObject tempObj = (GameObject)Instantiate(btnItem);
            BtnItem tempBtn = tempObj.GetComponent<BtnItem>();
			list.Add (tempBtn);


			if(yrGameItem.ContainsKey ("Name"))
			{
            	tempBtn.lblName.text = yrGameItem["Name"].YuanColumnText;
			}
			else if(yrGameItem.ContainsKey ("BlueprintName"))
			{
				tempBtn.lblName.text = yrGameItem["BlueprintName"].YuanColumnText;
			}
            tempBtn.infoBar = this.infoBar;
            tempBtn.ItemNeedBlood = int.Parse(yrStore["ItemNeedBlood"].YuanColumnText);
            tempBtn.ItemNeedCash = int.Parse(yrStore["ItemNeedCash"].YuanColumnText);
            tempBtn.lblFavorable.text = yrStore["ItemDiscount"].YuanColumnText;
            tempBtn.storeItemID = yrStore["id"].YuanColumnText;
            tempBtn.ItemID = yrStore["ItemID"].YuanColumnText;

            tempBtn.myMessage.target = this.gameObject;
            tempBtn.myMessage.functionName = btnFunction;
            tempBtn.gameObject.SendMessage("SetBtn",tempBtn);
			object[] objParms=new object[2];
			objParms[0]=tempBtn.ItemID;
			objParms[1]=tempBtn.spriteBackground;
			PanelStatic.StaticBtnGameManager.invcl.SendMessage ("SetYanSeAsID",objParms,SendMessageOptions.DontRequireReceiver);

            //if (yrStore["isStart"].YuanColumnText.Trim() == "1")
            //{
            //    tempBtn.IsFavorable = true;
            //    tempBtn.DtEnd = System.DateTime.Parse(yrStore["ItemEndTime"].YuanColumnText);
            //}
            //else
            //{
            //    tempBtn.IsFavorable = false;
            //}

            System.TimeSpan timeSpan = new System.TimeSpan(0, 0, 1);
            DateTime dt = DateTime.Parse(yrStore["ItemEndTime"].YuanColumnText.Trim());
            //TimeSpan ts = dt.TimeOfDay - DateTime.Now.TimeOfDay;
            TimeSpan ts = dt.TimeOfDay - InRoom.GetInRoomInstantiate().serverTime.TimeOfDay;
            ts = ts.Subtract(timeSpan);
            if (ts.Hours <= 0 && ts.Minutes <= 0 && ts.Seconds <= 0)
            {
                // 结束打折
                tempBtn.IsFavorable = false;
            }
            else
            {
                // 正在打折
                tempBtn.IsFavorable = true;
                tempBtn.DtEnd = System.DateTime.Parse(yrStore["ItemEndTime"].YuanColumnText);
            }

            tempBtn.gameObject.SetActiveRecursively(mActive);
			if(!mGrid.gameObject.active)
			{
				tempObj.SetActiveRecursively (false);
			}

            tempBtn.yr = yrStore;
			tempBtn.transform.parent = mGrid.transform;
			tempBtn.myCheck.group = 6;
			tempBtn.transform.localScale = new Vector3(1, 1, 1);
			tempBtn.transform.localPosition = Vector3.zero;

		
        }
    }

    public UISprite picF;
	public UISprite picFBack;
    public UILabel lblFInfoName;
    public UILabel lblFInfoFavorable;
    public UILabel lblFInfoOriginPriceGold;
    public UILabel lblFOriginPriceBlood;
    public UILabel lblFNowPriceGold;
    public UILabel lblFNowPriceBlood;
    public UILabel lblFTime;
    public UIButtonMessage btnFEnter;
    private string itemidF = string.Empty;
    /// <summary>
    /// 设置限时优惠售价信息
    /// </summary>
    /// <param name="obj"></param>
    private void SetFavorableInfo(GameObject obj)
    {
      BtnItem  btnItem = obj.GetComponent<BtnItem>();
        if (btnItem != null)
        {
            //picF.atlas = btnItem.pic.atlas;
            picF.spriteName = btnItem.pic.spriteName;
			picFBack.spriteName=btnItem.spriteBackground.spriteName;
            lblFInfoName.text = btnItem.lblName.text;
            
            if (btnItem.isStart)
            {
                lblFInfoFavorable.text = btnItem.lblFavorable.text;

                if (null != lblFInfoFavorable && !lblFInfoFavorable.enabled) lblFInfoFavorable.enabled = true;

                if (btnItem.lblFavorable.text.Equals("0") || btnItem.lblFavorable.text.Equals("10"))
                {
                    lblFInfoFavorable.text = "10";
                    lblFInfoFavorable.enabled = false;
                }
            }
            else
            {
                lblFInfoFavorable.text = "10";

                if (null != lblFInfoFavorable && lblFInfoFavorable.enabled) lblFInfoFavorable.enabled = false;
            }

            lblFInfoOriginPriceGold.text = btnItem.ItemNeedCash.ToString();
			lblFOriginPriceBlood.text = "[s]"+btnItem.ItemNeedBlood.ToString()+"[/s]";

            lblFNowPriceGold.text = (btnItem.ItemNeedCash * int.Parse(lblFInfoFavorable.text) / 10).ToString();
            lblFNowPriceBlood.text = (btnItem.ItemNeedBlood * int.Parse(lblFInfoFavorable.text) / 10).ToString();

            lblFInfoFavorable.text = lblFInfoFavorable.text + StaticLoc.Loc.Get("info991"); // 在折扣数字后加一个“折”字

            lblFTime.text = btnItem.lblInfo.text;
            itemidF = btnItem.storeItemID;

			lblFInfoName.text=btnItem.lblName.text;
		
            btnFEnter.target = this.gameObject;
            btnFEnter.functionName = "SetFavorableInfoBtn";
        }
    }


    /// <summary>
    /// ÏÞÊ±ÓÅ»ÝÉÌÆ·¹ºÂò
    /// </summary>
    private void SetFavorableInfoBtn()
    {
        if (itemidF != string.Empty)
        {
            if(PlayerPrefs.GetInt ("ConsumerTip")==1)
			{
            	PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target=this.gameObject;
				PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName="SetFavorableInfoBtnEnter";
				PanelStatic.StaticWarnings.warningAllEnterClose.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info499"));
			}
			else
			{
				SetFavorableInfoBtnEnter ();
			}
        }
    }
	
	private float tempFavorableTime=0;
	private void SetFavorableInfoBtnEnter()
	{
		if(Time.time-tempFavorableTime>=5)
		{
			tempFavorableTime=Time.time;
			PanelStatic.StaticWarnings.warningAllEnterClose.Close ();
		StartCoroutine(PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate().BuyItem(itemidF)));
		}
		else
		{
			PanelStatic.StaticWarnings.warningAllEnter.Show (StaticLoc.Loc.Get ("info359"),StaticLoc.Loc.Get ("tips037"));
		}
	}

    public UISprite picI;
	public UISprite picIBack;
    public UILabel lblIInfoName;
    public UILabel lblIInfoFavorable;
    public UILabel lblIInfoOriginPriceGold;
    public UILabel lblIOriginPriceBlood;
    public UILabel lblINowPriceGold;
    public UILabel lblINowPriceBlood;
    public UILabel lblITime;
    public UIButtonMessage btnIEnter;
    private string itemidI = string.Empty;
    /// <summary>
    /// 设置物品售价信息
    /// </summary>
    /// <param name="obj"></param>
    private void SetItemInfo(GameObject obj)
    {
        BtnItem btnItem = obj.GetComponent<BtnItem>();
        if (btnItem != null)
        {
            //picI.atlas = btnItem.pic.atlas;
            picI.spriteName = btnItem.pic.spriteName;

			picIBack.spriteName=btnItem.spriteBackground.spriteName;
            lblIInfoName.text = btnItem.lblName.text;
            
            if (btnItem.isStart)
            {
                lblIInfoFavorable.text = btnItem.lblFavorable.text;

                if (null != lblIInfoFavorable && !lblIInfoFavorable.enabled) lblIInfoFavorable.enabled = true;

                if (btnItem.lblFavorable.text.Equals("0") || btnItem.lblFavorable.text.Equals("10"))
                {
                    lblIInfoFavorable.text = "10";
                    lblIInfoFavorable.enabled = false;
                }
            }
            else
            {
                lblIInfoFavorable.text = "10";

                if (null != lblIInfoFavorable && lblIInfoFavorable.enabled) lblIInfoFavorable.enabled = false;
            }

            lblIInfoOriginPriceGold.text = btnItem.ItemNeedCash.ToString();
            lblIOriginPriceBlood.text = "[s]"+btnItem.ItemNeedBlood.ToString()+"[/s]";

            //lblINowPriceGold.text = (btnItem.ItemNeedCash * int.Parse(btnItem.lblFavorable.text) / 10).ToString();
            //lblINowPriceBlood.text = (btnItem.ItemNeedBlood * int.Parse(btnItem.lblFavorable.text) / 10).ToString();
            lblINowPriceGold.text = (btnItem.ItemNeedCash * int.Parse(lblIInfoFavorable.text) / 10).ToString();
            lblINowPriceBlood.text = (btnItem.ItemNeedBlood * int.Parse(lblIInfoFavorable.text) / 10).ToString();

            lblIInfoFavorable.text = lblIInfoFavorable.text + StaticLoc.Loc.Get("info991"); // 在折扣数字后加一个“折”字

            lblITime.text = btnItem.lblInfo.text;
            itemidI = btnItem.storeItemID;

			lblIInfoName.text=btnItem.lblName.text;
			
            btnIEnter.target = this.gameObject;
            btnIEnter.functionName = "SetItemInfoBtn";
        }
    }

    /// <summary>
    /// µÀ¾ßÉÌÆ·¹ºÂò
    /// </summary>
    private void SetItemInfoBtn()
    {
        if (itemidI != string.Empty)
        {
            if(PlayerPrefs.GetInt ("ConsumerTip")==1)
			{
            	PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target=this.gameObject;
				PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName="SetItemInfoBtnEnter";
				PanelStatic.StaticWarnings.warningAllEnterClose.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info499"));
			}
			else
			{
				SetItemInfoBtnEnter ();
			}
        }
    }
	
	private float tempItemTime=0;
	private void SetItemInfoBtnEnter()
	{

		if(Time.time-tempItemTime>=5)
		{
			tempItemTime=Time.time;
			PanelStatic.StaticWarnings.warningAllEnterClose.Close ();
			StartCoroutine( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate().BuyItem(itemidI)));
		}
		else
		{
			PanelStatic.StaticWarnings.warningAllEnter.Show (StaticLoc.Loc.Get ("info359"),StaticLoc.Loc.Get ("tips037"));
		}		
	}
 
    public UISprite picE;
	public UISprite picEBack;
	public UILabel lblEInfoName;
    public UILabel lblEInfoFavorable;
    public UILabel lblEInfoOriginPriceGold;
    public UILabel lblEOriginPriceBlood;
    public UILabel lblENowPriceGold;
    public UILabel lblENowPriceBlood;
    public UILabel lblETime;
    public UIButtonMessage btnEEnter;
    private string itemidE = string.Empty;
    /// <summary>
    /// 设置装备售价信息
    /// </summary>
    /// <param name="obj"></param>
    private void SetEtemInfo(GameObject obj)
    {
        BtnItem btnItem = obj.GetComponent<BtnItem>();
        if (btnItem != null)
        {
            //picI.atlas = btnItem.pic.atlas;
            picE.spriteName = btnItem.pic.spriteName;
			picEBack.spriteName=btnItem.spriteBackground.spriteName;
            lblEInfoName.text = btnItem.lblName.text;
            if (btnItem.isStart)
            {
                lblEInfoFavorable.text = btnItem.lblFavorable.text;

                if (null != lblEInfoFavorable && !lblEInfoFavorable.enabled) lblEInfoFavorable.enabled = true;

                if (btnItem.lblFavorable.text.Equals("0") || btnItem.lblFavorable.text.Equals("10"))
                {
                    lblEInfoFavorable.text = "10";
                    lblEInfoFavorable.enabled = false;
                }
            }
            else
            {
                lblEInfoFavorable.text = "10";

                if (null != lblEInfoFavorable && lblEInfoFavorable.enabled) lblEInfoFavorable.enabled = false;
            }
            lblEInfoOriginPriceGold.text = btnItem.ItemNeedCash.ToString();

            lblEOriginPriceBlood.text = "[s]"+btnItem.ItemNeedBlood.ToString()+"[/s]";

            //lblENowPriceGold.text = (btnItem.ItemNeedCash * int.Parse(btnItem.lblFavorable.text) / 10).ToString();
            //lblENowPriceBlood.text = (btnItem.ItemNeedBlood * int.Parse(btnItem.lblFavorable.text) / 10).ToString();
            lblENowPriceGold.text = (btnItem.ItemNeedCash * int.Parse(lblEInfoFavorable.text) / 10).ToString();
            lblENowPriceBlood.text = (btnItem.ItemNeedBlood * int.Parse(lblEInfoFavorable.text) / 10).ToString();

            lblEInfoFavorable.text = lblEInfoFavorable.text + StaticLoc.Loc.Get("info991"); // 在折扣数字后加一个“折”字
			lblEInfoName.text=btnItem.lblName.text;
            lblETime.text = btnItem.lblInfo.text;
            itemidE = btnItem.storeItemID;
            btnEEnter.target = this.gameObject;
            btnEEnter.functionName = "SetEtemInfoBtn";
        }
    }

    /// <summary>
    /// µÀ¾ßÉÌÆ·¹ºÂò
    /// </summary>
    private void SetEtemInfoBtn()
    {
        if (itemidE != string.Empty)
        {
//            Debug.LogError("-----------------------" + itemidE);
			if(PlayerPrefs.GetInt ("ConsumerTip")==1)
			{
            	PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target=this.gameObject;
				PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName="SetEtemInfoBtnEnter";
				PanelStatic.StaticWarnings.warningAllEnterClose.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info499"));
			}
			else
			{
				SetEtemInfoBtnEnter ();
			}
        }
    }
	
	private float tempEtemTime=0;
	private void SetEtemInfoBtnEnter()
	{

		if(Time.time-tempEtemTime>=5)
		{
			tempEtemTime=Time.time;
			PanelStatic.StaticWarnings.warningAllEnterClose.Close ();
		StartCoroutine (	PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate().BuyItem(itemidE)));
		}
		else
		{
			PanelStatic.StaticWarnings.warningAllEnter.Show (StaticLoc.Loc.Get ("info359"),StaticLoc.Loc.Get ("tips037"));
		}
	}
	
    public UISprite picM;
	public UISprite picMBack;
	public UILabel lblMInfoName;
    public UILabel lblMInfoFavorable;
    public UILabel lblMInfoOriginPriceGold;
    public UILabel lblMOriginPriceBlood;
    public UILabel lblMNowPriceGold;
    public UILabel lblMNowPriceBlood;
    public UILabel lblMTime;
    public UIButtonMessage btnMEnter;
    private string itemidM = string.Empty;
    /// <summary>
    /// 设置宠物售价信息
    /// </summary>
    /// <param name="obj"></param>
    private void SetMtemInfo(GameObject obj)
    {
        BtnItem btnItem = obj.GetComponent<BtnItem>();
        if (btnItem != null)
        {
            //picI.atlas = btnItem.pic.atlas;
            picM.spriteName = btnItem.pic.spriteName;
			picMBack.spriteName=btnItem.spriteBackground.spriteName;
            lblMInfoName.text = btnItem.lblName.text;
            
            if (btnItem.isStart)
            {
                lblMInfoFavorable.text = btnItem.lblFavorable.text;

                if (null != lblMInfoFavorable && !lblMInfoFavorable.enabled) lblMInfoFavorable.enabled = true;

                if (btnItem.lblFavorable.text.Equals("0") || btnItem.lblFavorable.text.Equals("10"))
                {
                    lblMInfoFavorable.text = "10";
                    lblMInfoFavorable.enabled = false;
                }
            }
            else
            {
                lblMInfoFavorable.text = "10";

                if (null != lblMInfoFavorable && lblMInfoFavorable.enabled) lblMInfoFavorable.enabled = false;
            }

            lblMInfoOriginPriceGold.text = btnItem.ItemNeedCash.ToString();
            lblMOriginPriceBlood.text = "[s]"+btnItem.ItemNeedBlood.ToString()+"[/s]";

            //lblMNowPriceGold.text = (btnItem.ItemNeedCash * int.Parse(btnItem.lblFavorable.text) / 10).ToString();
            //lblMNowPriceBlood.text = (btnItem.ItemNeedBlood * int.Parse(btnItem.lblFavorable.text) / 10).ToString();
            lblMNowPriceGold.text = (btnItem.ItemNeedCash * int.Parse(lblMInfoFavorable.text) / 10).ToString();
            lblMNowPriceBlood.text = (btnItem.ItemNeedBlood * int.Parse(lblMInfoFavorable.text) / 10).ToString();

            lblMInfoFavorable.text = lblMInfoFavorable.text + StaticLoc.Loc.Get("info991"); // 在折扣数字后加一个“折”字
			lblMInfoName.text=btnItem.lblName.text;
            lblMTime.text = btnItem.lblInfo.text;
            itemidM = btnItem.storeItemID;
            btnMEnter.target = this.gameObject;
            btnMEnter.functionName = "SetMtemInfoBtn";
        }
    }

    /// <summary>
    /// µÀ¾ßÉÌÆ·¹ºÂò
    /// </summary>
    private void SetMtemInfoBtn()
    {
        if (itemidM != string.Empty)
        {
//            Debug.LogError("-----------------------" + itemidM);
			if(PlayerPrefs.GetInt ("ConsumerTip")==1)
			{
            	PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target=this.gameObject;
				PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName="SetMtemInfoBtnEnter";
				PanelStatic.StaticWarnings.warningAllEnterClose.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info499"));
			}
			else
			{
				SetMtemInfoBtnEnter ();
			}
        }
    }
	
	private float tempMtemTime=0;
	private void SetMtemInfoBtnEnter()
	{

		if(Time.time-tempMtemTime>=5)
		{
			tempMtemTime=Time.time;
			
			PanelStatic.StaticWarnings.warningAllEnterClose.Close ();
		StartCoroutine (	PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate().BuyItem(itemidM)));
		}
		else
		{
			PanelStatic.StaticWarnings.warningAllEnter.Show (StaticLoc.Loc.Get ("info359"),StaticLoc.Loc.Get ("tips037"));
		}			
	}	
}
