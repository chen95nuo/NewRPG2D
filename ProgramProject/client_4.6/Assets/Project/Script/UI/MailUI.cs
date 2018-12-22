using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MailUI : MonoBehaviour,ProcessResponse,BWWarnUI {
	
//	public static MailUI mInstance;
	
	public UIAtlas itemCircleAtals;
	public UIAtlas equipCircleAtals;
	public UIAtlas heroCircleAtlas;
	public UIAtlas skillCircleAtals;
	public UIAtlas pSkillCircleAtals;
	
	public GameObject detail;
	public GameObject pop;
	
	public UILabel mailUnread;
	public GameObject cellParent;
	public GameObject scrollBar;
	
	private GameObject mailCell;
	
	public MailUIResultJson mrj;
	private MailResultJson mailDetail;
	private int curIndex;
	private int errorCode;
	private int requestType;
	private bool receiveData;
	private int getCrystalNum;
	
	private Hashtable mailGos=new Hashtable();
	private Transform _myTransform;
	
	public NewUnitSkillResultJson nusrj;
	void Awake()
	{
//		_MyObj=gameObject;
//		mInstance=this;
		_myTransform = transform;
	}
	
	// Use this for initialization
	void Start ()
	{
//		base.init();
//		close();
//		itemCircleAtals = LoadAtlasOrFont.LoadAtlasByName("ItemCircularIcon");
//		equipCircleAtals = LoadAtlasOrFont.LoadAtlasByName("EquipCircularIcon");
//		heroCircleAtlas = LoadAtlasOrFont.LoadAtlasByName("HeadIcon");
//		skillCircleAtals = LoadAtlasOrFont.LoadAtlasByName("SkillCircularIcom");
//		pSkillCircleAtals = LoadAtlasOrFont.LoadAtlasByName("PassSkillCircularIcon");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(receiveData)
		{
			receiveData=false;
			
			if(errorCode == -3)
				return;
			
			/**76邮件详细索引出错
			 * 77领取附件出错
			 * 78没有附件可领取**/
			if(errorCode!=0)
			{
				ToastWindow.mInstance.showText(TextsData.getData(474).chinese,this);
				return;
			}
			
			switch(requestType)
			{
			case 1://查看邮件具体信息//
				//GameObject curCell=cellParent.transform.FindChild(curIndex+"").gameObject;
				GameObject curCell=(GameObject)mailGos[curIndex];
				setCellMark(curCell,0);
				showDetail();
				break;
			case 2:
				//邮件领取//
				if(!TalkingDataManager.isTDPC && errorCode == 0)
				{
					TDGAVirtualCurrency.OnReward(getCrystalNum, "mailaward");
				}
				AddCardDictionary(mailDetail);
				requestType=3;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PLAYER),this);
				break;
			case 3://请求玩家数据//
				HeadUI.mInstance.refreshPlayerInfo();
				closeDetail();
				showPop();
				
				//向服务器请求判断是否有新解锁合体技//
				{
					requestType = 4;
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
				}
				break;
			case 4:	
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
	
	public void show()
	{
//		base.show();

        Main3dCameraControl.mInstance.SetBool(true);
		
		mailUnread.text=TextsData.getData(298).chinese.Replace("num",mrj.ur+"");
		GameObjectUtil.destroyGameObjectAllChildrens(cellParent);
		mailGos.Clear();
		for(int i=0;i<mrj.ms.Count;i++)
		{
			if(mailCell==null)
			{
				mailCell=GameObjectUtil.LoadResourcesPrefabs("UI-mail/mail-cell",3);
			}
			GameObject cell=Instantiate(mailCell) as GameObject;
			MailUIResultElement ss=mrj.ms[i];
			mailGos.Add(ss.index,cell);
			GameObjectUtil.gameObjectAttachToParent(cell,cellParent);
			GameObjectUtil.setGameObjectLayer(cell,cellParent.layer);
			setCellData(cell,ss);
		}
		cellParent.GetComponent<UIGrid>().repositionNow=true;
		detail.SetActive(false);
		pop.SetActive(false);
		scrollBar.GetComponent<UIScrollBar>().value=0;
	}
	
	public void hide()
	{
//		base.hide();
		_myTransform. gameObject.SetActive(false);
        Main3dCameraControl.mInstance.SetBool(false);
		closeDetail();
		TalkMainToGetData();
		gc();
		
	}
	
	//cxl---通知主界面发请求//
	public void TalkMainToGetData()
	{
		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager")as MainMenuManager;
		if(main!= null)
		{
			if( main.gameObject.activeSelf)
			{
				main.SendToGetData();
			}
		}
	}
	
	private void gc()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(cellParent);
		mailGos.Clear();
		mrj=null;
		mailDetail=null;
		mailCell=null;
		Resources.UnloadUnusedAssets();
	}
	
	private void setCellData(GameObject cell,MailUIResultElement info)
	{
		//==邮件标题,发件人,发件时间(yyyy-MM-dd HH:ss),是否新邮件(2未读邮件,0已读邮件),index==//
		cell.transform.FindChild("name").GetComponent<UILabel>().text=info.title;
		cell.transform.FindChild("sender").GetComponent<UILabel>().text=info.sender;
		cell.transform.FindChild("send-time").GetComponent<UILabel>().text=info.sendTime;
		setCellMark(cell,info.mark);
		UIButtonMessage msg=cell.GetComponent<UIButtonMessage>();
//		msg.target=_MyObj;
		msg.target = _myTransform.gameObject;
		msg.functionName="lookDetail";
		msg.param=info.index;
	}
	
	private void setCellMark(GameObject cell,int newMark)
	{
		if(newMark==2)
		{
			cell.transform.FindChild("cover").gameObject.SetActive(false);
			cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName="mail_unread";
			cell.transform.FindChild("new-mark").gameObject.SetActive(true);
		}
		else if(newMark==0)
		{
			cell.transform.FindChild("cover").gameObject.SetActive(true);
			cell.transform.FindChild("icon").GetComponent<UISprite>().spriteName="mail_read";
			cell.transform.FindChild("new-mark").gameObject.SetActive(false);
		}
	}
	
	private void lookDetail(int index)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		curIndex=index;
		requestType=1;
		PlayerInfo.getInstance().sendRequest(new MailJson(index,1),this);
	}
	
	private void showDetail()
	{
		detail.SetActive(true);
		detail.transform.FindChild("scroll-bar").GetComponent<UIScrollBar>().value=0;
		GameObject panel=detail.transform.FindChild("panel").gameObject;
		panel.transform.localPosition=new Vector3(0,0,0);
		panel.GetComponent<UIPanel>().clipRange=new Vector4(0,0f,600f,150f);
		
		
		detail.transform.FindChild("title").GetComponent<UILabel>().text=mailDetail.title;
		UILabel content=detail.transform.FindChild("panel/detail-parent/content").GetComponent<UILabel>();
		content.text=mailDetail.content;
		
		float y=content.transform.localPosition.y-content.height-32;
		float yOffset=70f;
		
		if(setValue(detail.transform.FindChild("panel/detail-parent/gold").gameObject,mailDetail.gold,y,6))
		{
			y-=yOffset;
		}
		if(setValue(detail.transform.FindChild("panel/detail-parent/crystal").gameObject,mailDetail.crystal,y,7))
		{
			y-=yOffset;
		}
		if(setValue(detail.transform.FindChild("panel/detail-parent/rune").gameObject,mailDetail.runeNum,y,8))
		{
			y-=yOffset;
		}
		if(setValue(detail.transform.FindChild("panel/detail-parent/power").gameObject,mailDetail.power,y,9))
		{
			y-=yOffset;
		}
		if(setValue(detail.transform.FindChild("panel/detail-parent/friend").gameObject,mailDetail.friendNum,y,10))
		{
			y-=yOffset;
		}
		if(setValue(detail.transform.FindChild("panel/detail-parent/honor").gameObject,mailDetail.honor,y,11))
		{
			y-=yOffset;
		}
		if(setValue(detail.transform.FindChild("panel/detail-parent/diamond").gameObject,mailDetail.diamond,y,12))
		{
			y-=yOffset;
		}
		if(setReward(detail.transform.FindChild("panel/detail-parent/reward1").gameObject,mailDetail.reward1,y))
		{
			y-=yOffset;
		}
		if(setReward(detail.transform.FindChild("panel/detail-parent/reward2").gameObject,mailDetail.reward2,y))
		{
			y-=yOffset;
		}
		if(setReward(detail.transform.FindChild("panel/detail-parent/reward3").gameObject,mailDetail.reward3,y))
		{
			y-=yOffset;
		}
		if(setReward(detail.transform.FindChild("panel/detail-parent/reward4").gameObject,mailDetail.reward4,y))
		{
			y-=yOffset; 
		}
		if(setReward(detail.transform.FindChild("panel/detail-parent/reward5").gameObject,mailDetail.reward5,y))
		{
			y-=yOffset;
		}
		if(setReward(detail.transform.FindChild("panel/detail-parent/reward6").gameObject,mailDetail.reward6,y))
		{
			y-=yOffset;
		}
		
		GameObject btn=detail.transform.FindChild("btn").gameObject;
		if(mailDetail.mark==1)
		{
			btn.transform.FindChild("Label").GetComponent<UILabel>().text=TextsData.getData(304).chinese;
			UIButtonMessage msg=btn.GetComponent<UIButtonMessage>();
//			msg.target=_MyObj;
			msg.target = _myTransform.gameObject;
			msg.functionName="getMailReward";
			msg.param=curIndex;
		}
		else
		{
			btn.transform.FindChild("Label").GetComponent<UILabel>().text=TextsData.getData(305).chinese;
			UIButtonMessage msg=btn.GetComponent<UIButtonMessage>();
//			msg.target=_MyObj;
			msg.target = _myTransform.gameObject;
			msg.functionName="closeDetail";
		}
	}
	
	private bool setReward(GameObject rewardGo,string reward,float y)
	{
		rewardGo.SetActive(false);
		if(!string.IsNullOrEmpty(reward))
		{
			string[] ss=reward.Split('&');
			int type=StringUtil.getInt(ss[0]);
			int rewardId=StringUtil.getInt(ss[1]);
			int number=StringUtil.getInt(ss[2]);
			if(type>0 && rewardId>0 && number>0)
			{
				rewardGo.SetActive(true);
				UILabel label=rewardGo.transform.FindChild("Label").GetComponent<UILabel>();
                UILabel name = rewardGo.transform.FindChild("Name").GetComponent<UILabel>();
				SimpleCardInfo2 cardInfo = rewardGo.transform.FindChild("Cardinfo").GetComponent<SimpleCardInfo2>();
				cardInfo.clear();
				label.text="x"+number;
				GameHelper.E_CardType cardType = GameHelper.E_CardType.E_Null;
				switch(type)
				{
				case 1:
					cardType = GameHelper.E_CardType.E_Item;
                    name.text = GetName(rewardId, GameHelper.E_CardType.E_Item);
					break;
				case 2:
					cardType = GameHelper.E_CardType.E_Equip;
                    name.text = GetName(rewardId, GameHelper.E_CardType.E_Equip);
					break;
				case 3:
					cardType = GameHelper.E_CardType.E_Hero;
                    name.text = GetName(rewardId, GameHelper.E_CardType.E_Hero);
					break;
				case 4:
					cardType = GameHelper.E_CardType.E_Skill;
                    name.text = GetName(rewardId, GameHelper.E_CardType.E_Skill);
					break;
				case 5:
					cardType = GameHelper.E_CardType.E_PassiveSkill;

                    name.text = GetName(rewardId, GameHelper.E_CardType.E_PassiveSkill);
					break;
				}
				cardInfo.setSimpleCardInfo(rewardId,cardType);

             
				Vector3 pos=rewardGo.transform.localPosition;
				rewardGo.transform.localPosition=new Vector3(pos.x,y,pos.z);
				return true;
			}
		}
		return false;
	}
    public string GetName(int CardId, GameHelper.E_CardType type)
    {
        string name = "";
        switch (type)
        {
            case GameHelper.E_CardType.E_Hero:
                {
                    CardData cd = CardData.getData(CardId);
                    if (cd == null)
                        return "";
                    name = cd.name;
                } break;
            case GameHelper.E_CardType.E_Equip:
                {
                    EquipData ed = EquipData.getData(CardId);
                    if (ed == null)
                        return "";
                    name = ed.name;
                } break;
            case GameHelper.E_CardType.E_Item:
                {
                    ItemsData itemData = ItemsData.getData(CardId);
                    if (itemData == null)
                        return "";
                    name = itemData.name;
                } break;
            case GameHelper.E_CardType.E_Skill:
                {
                    SkillData sd = SkillData.getData(CardId);
                    if (sd == null)
                        return "";
                    name = sd.name;
                } break;
            case GameHelper.E_CardType.E_PassiveSkill:
                {
                    PassiveSkillData psd = PassiveSkillData.getData(CardId);
                    if (psd == null)
                        return "";
                    name = psd.name;
                } break;
        }
        return name;
    }
	private bool setValue(GameObject valueGo,int number,float y,int type)
	{
		valueGo.SetActive(false);
		if(number>0)
		{
			valueGo.SetActive(true);
			valueGo.transform.FindChild("Label").GetComponent<UILabel>().text="x"+number;
            UILabel name = valueGo.transform.FindChild("Name").GetComponent<UILabel>();
            switch (type)
            {
                case 6:

                    name.text = TextsData.getData(658).chinese;
                    break;
                case 7:

                    name.text = TextsData.getData(659).chinese;
                    break;
                case 8:

                    name.text = TextsData.getData(660).chinese;
                    break;
                case 9:

                    name.text = TextsData.getData(661).chinese;
                    break;
                case 10:

                    name.text = TextsData.getData(657).chinese;
                    break;
                case 11:

                    name.text = TextsData.getData(714).chinese;
                    break;
                case 12:

                    name.text = TextsData.getData(713).chinese;
                    break;
            }
			Vector3 pos=valueGo.transform.localPosition;
			valueGo.transform.localPosition=new Vector3(pos.x,y,pos.z);
			return true;
		}
		return false;
	}
	
	public void getMailReward(int param)
	{
		requestType=2;
		PlayerInfo.getInstance().sendRequest(new MailJson(param,2),this);
	}
	
	public void closeDetail()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		detail.SetActive(false);
	}
	
	private void showPop()
	{
		pop.SetActive(true);
		
		pop.transform.FindChild("scroll-bar").GetComponent<UIScrollBar>().value=0;
		GameObject panel=pop.transform.FindChild("panel").gameObject;
		panel.transform.localPosition=new Vector3(0,5f,0);
		//panel.GetComponent<UIPanel>().clipRange=new Vector4(0,-50,600f,200f);
		
		float y=0;
		float yOffset=70f;
		
		if(setValue(pop.transform.FindChild("panel/pop-parent/pop-info/gold").gameObject,mailDetail.gold,y,6))
		{
			y-=yOffset;
		}
		if(setValue(pop.transform.FindChild("panel/pop-parent/pop-info/crystal").gameObject,mailDetail.crystal,y,7))
		{
			y-=yOffset;
		}
		if(setValue(pop.transform.FindChild("panel/pop-parent/pop-info/rune").gameObject,mailDetail.runeNum,y,8))
		{
			y-=yOffset;
		}
		if(setValue(pop.transform.FindChild("panel/pop-parent/pop-info/power").gameObject,mailDetail.power,y,9))
		{
			y-=yOffset;
		}
		if(setValue(pop.transform.FindChild("panel/pop-parent/pop-info/friend").gameObject,mailDetail.friendNum,y,10))
		{
			y-=yOffset;
		}
		if(setValue(pop.transform.FindChild("panel/pop-parent/pop-info/honor").gameObject,mailDetail.honor,y,11))
		{
			y-=yOffset;
		}
		if(setValue(pop.transform.FindChild("panel/pop-parent/pop-info/diamond").gameObject,mailDetail.diamond,y,12))
		{
			y-=yOffset;
		}
		if(setReward(pop.transform.FindChild("panel/pop-parent/pop-info/reward1").gameObject,mailDetail.reward1,y))
		{
			y-=yOffset;
		}
		if(setReward(pop.transform.FindChild("panel/pop-parent/pop-info/reward2").gameObject,mailDetail.reward2,y))
		{
			y-=yOffset;
		}
		if(setReward(pop.transform.FindChild("panel/pop-parent/pop-info/reward3").gameObject,mailDetail.reward3,y))
		{
			y-=yOffset;
		}
		if(setReward(pop.transform.FindChild("panel/pop-parent/pop-info/reward4").gameObject,mailDetail.reward4,y))
		{
			y-=yOffset;
		}
		if(setReward(pop.transform.FindChild("panel/pop-parent/pop-info/reward5").gameObject,mailDetail.reward5,y))
		{
			y-=yOffset;
		}
		if(setReward(pop.transform.FindChild("panel/pop-parent/pop-info/reward6").gameObject,mailDetail.reward6,y))
		{
			y-=yOffset;
		}
	}
	
	public void closePop()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		pop.SetActive(false);
	}
	
	public void OnClickCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		hide();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAIL);
	}
	
	public void AddCardDictionary(MailResultJson mrjs)
	{
		//添加卡牌获得统计@zhangsai//
		List<string> str = new List<string>();
		str.Add(mrjs.reward1);
		str.Add(mrjs.reward2);
		str.Add(mrjs.reward3);
		str.Add(mrjs.reward4);
		str.Add(mrjs.reward5);
		str.Add(mrjs.reward6);
		for(int i = 0;i<str.Count;i++)
		{
			if(!str[i].Equals(""))
			{
				string[] ss = str[i].Split('&');
				if(StringUtil.getInt(ss[0]) == 3)
				{
					if(!UniteSkillInfo.cardUnlockTable.ContainsKey(StringUtil.getInt(ss[1])))
					{
						UniteSkillInfo.cardUnlockTable.Add(StringUtil.getInt(ss[1]),true);	
					}
				}
			}
		}
	}
	
	public void warnningSure(){}
	
	public void warnningCancel()
	{
		hide();	
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			PlayerInfo.getInstance().isShowConnectObj=false;
			switch(requestType)
			{
			case 1://打开邮件//
				MailResultJson mrj=JsonMapper.ToObject<MailResultJson>(json);
				errorCode=mrj.errorCode;
				if(errorCode==0)
				{
					this.mailDetail=mrj;
				}
				receiveData=true;
				break;
			case 2://领取附件//
				mrj=JsonMapper.ToObject<MailResultJson>(json);
				errorCode=mrj.errorCode;
				getCrystalNum = mrj.crystal;
				receiveData=true;
				break;
			case 3://请求player数据//
				PlayerResultJson prj=JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode=prj.errorCode;
				if(errorCode==0)
				{
					PlayerInfo.getInstance().player=prj.list[0];
				}
				receiveData=true;
				break;
			case 4://查看是否新解锁合体技//
				nusrj = JsonMapper.ToObject<NewUnitSkillResultJson>(json);
				errorCode = nusrj.errorCode;
				receiveData = true;
				break;
			}
		}
	}
}
