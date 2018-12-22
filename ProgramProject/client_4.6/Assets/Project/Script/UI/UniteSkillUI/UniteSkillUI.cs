using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UniteSkillUI : MonoBehaviour ,ProcessResponse,BWWarnUI{
	
	private int requestType;
	
	private bool receiveData;
	
	private int errorCode;
	
	public GameObject parent;
	
	private int curUniteSkillId;
	
	public CardGroup curCardGroup;
	
	public GameObject getWayDataPanel;
	
	//private DropGuideResultJson drj;
	
	public int curComeType = 1;
	
	private Hashtable uniteSkillCards = new Hashtable();
	
	private List<UniteSkillItemInfo> usList = new List<UniteSkillItemInfo>();
	public UILabel energyLabel;
	
	private bool isGoToMission;
	
	public void Show()
	{
		gameObject.SetActive(true);
        Main3dCameraControl.mInstance.SetBool(true);
		//初始化怒气值//
		energyLabel.text = PlayerInfo.getInstance().player.maxEnergy.ToString();
		DrawUniteSkill();
		MainMenuManager mainmenue = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager")as MainMenuManager;
		if(mainmenue != null)
		{
			mainmenue.setNeedShowUnitSkill(false);
		}
	}
	
	
	public void Hide()
	{
		gameObject.SetActive(false);
        Main3dCameraControl.mInstance.SetBool(false);
		MainMenuManager mainmenue = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager")as MainMenuManager;
		if(!isGoToMission)
		{
			mainmenue.MoveCameraCallBack();
		}
		else
		{
			mainmenue.hide();
		}
		if(UISceneStateControl.mInstace != null)
		{
			UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_UNITESKILL);
		}
	}


	void Update () {
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			
			if(errorCode==70)			//vip等级不足！//
			{
				string str = TextsData.getData(243).chinese;
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			if(errorCode==71)			//水晶不足//
			{
				string str = TextsData.getData(244).chinese;
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			if(errorCode==72)			//购买次数已达上限！//
			{
				string str = TextsData.getData(240).chinese;
			    //提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			switch(requestType)
			{
			case 1:
				Hide();
				break;
			case 3:
				if(errorCode == 0)
				{
					HeadUI.mInstance.refreshPlayerInfo();
					//==怒气上限==//
					energyLabel.text=PlayerInfo.getInstance().player.maxEnergy+"";
					
					//统计购买怒气消费//
					if(!TalkingDataManager.isTDPC)
					{
						int number=EnergyupData.getNumber(PlayerInfo.getInstance().player.maxEnergy);
						EnergyupData ed=EnergyupData.getData(number);
						if(ed!=null)
						{
							TDGAItem.OnPurchase("BuyEnergy",1,ed.cost);
						}
					}
				}
				break;
			}
		}
	}
	
	public void DrawUniteSkill()
	{
		usList.Clear();
		GameObjectUtil.destroyGameObjectAllChildrens(parent);
        List<UnitSkillData> usd = UnitSkillData.getAllUnitSkillDataShow(curUniteSkillId);
		GameObject loadPrefab = GameObjectUtil.LoadResourcesPrefabs("UniteSkill/UniteSkill-Cell",3);
		for(int i = 0;i<usd.Count;i++)
		{
			GameObject uniteSkillCell = Instantiate (loadPrefab) as GameObject;
			if(uniteSkillCell==null)
				continue;
            uniteSkillCell.name = "UniteSkill-Cell" + (1000 + i);
			if(usd[i].index == curUniteSkillId)
			{
				uniteSkillCell.GetComponent<UniteSkillItemInfo>().SetData(usd[i].index,gameObject,true);	
			}
			else
			{
				uniteSkillCell.GetComponent<UniteSkillItemInfo>().SetData(usd[i].index,gameObject);	
			}
			GameObjectUtil.gameObjectAttachToParent(uniteSkillCell,parent);
			usList.Add(uniteSkillCell.GetComponent<UniteSkillItemInfo>());

            uniteSkillCell.transform.localScale = new Vector3(0.92f, 0.92f, 0.92f);
		}
		parent.GetComponent<UIGrid>().repositionNow = true;
		loadPrefab = null;
		Resources.UnloadUnusedAssets();
	}
	
	public void setCurUniteSkillId(int curId)
	{
		curUniteSkillId = curId;
	}
	
	public void setIsComeToMission(bool isToMission)
	{
		isGoToMission = isToMission;
	}
	
	public void OnSelectUniteSkill(int param)
	{
				        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		UniteSkillItemInfo usii = null;
		
		foreach(UniteSkillItemInfo usItem in usList)
		{
			int uniteskillid = usItem.getUniteSkillId();
			if(uniteskillid == param)
			{
				usii = usItem;
				break;
			}
		}
		
		if(usii !=null)
		{
			if(!usii.getIsUse())
			{
				//如果未选中就选中//
				curCardGroup.changeMark = 1;
        		curCardGroup.unitSkillId = param;
				usii.setIsUse(true);
				for(int i = 0;i<usList.Count;i++)
				{
					if(usList[i].getUniteSkillId() != param)
					{
						usList[i].setIsUse(false);
						usList[i].isUseTween.from = 1;
						usList[i].isUseTween.to = 0;
						usList[i].isUseTween.PlayForward();
					}
				}
				usii.isUseTween.from = 0;
				usii.isUseTween.to = 1;
				usii.isUseTween.PlayForward();
			}
			else
			{
				//如果已经选中就取消//
				curCardGroup.changeMark = 1;
				curCardGroup.unitSkillId = 0;
				usii.setIsUse(false);
				usii.isUseTween.from = 1;
				usii.isUseTween.to = 0;
				usii.isUseTween.PlayForward();
			}
		}
		
	}
	void uniteSkillClick(int param)
	{
		
		ShowGetWayData(param);
	}
	//合体技的id//
    public void ShowGetWayData(int index)
    {
        //CardGroup cg = curCardGroup;
		//合体技//
        List<int> uniteSkillIds = new List<int>();

        UnitSkillData data = UnitSkillData.getData(index);
        foreach (int c in data.cards)
        {
            if (c != 0)
            {
                uniteSkillIds.Add(c);
            }
        }
		uniteSkillCards.Clear();
		//float nextOffY = 0;
		//float x = 0;
		//修改需要卡牌id//
		List<int> ids = new List<int>();
		int d = data.card1;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card2;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card3;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card4;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card5;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card6;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card7;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card8;
		if (d > 0)
		{
			ids.Add(d);
		}
            
        List<int> cardIds = ids;
		//显示合体技获得途径界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GETWAYPANEL);
		GetWayPanelManager getWay = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GETWAYPANEL, "GetWayPanelManager")as GetWayPanelManager;
		//3 表示从阵容界面进入//
		getWay.SetData(index, cardIds, 4);


    }
	public void OnClickBack()
	{
		requestType = 1;
		PlayerInfo.getInstance().sendRequest(new SaveCGJson(curCardGroup),this);
	}
	
	public void receiveResponse(string json)
	{
		if(json != null)
		{
			PlayerInfo.getInstance().isShowConnectObj = false;//隐藏等待//
			switch(requestType)
			{
			case 1:
				CardGroupResultJson cgrj=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj.errorCode;
				if(errorCode == 0)
				{
					curCardGroup=cgrj.transformCardGroup();
					PlayerInfo.getInstance().curCardGroup = curCardGroup;
				}
				receiveData=true;
				break;
			//case 2:
				//DropGuideResultJson drj=JsonMapper.ToObject<DropGuideResultJson>(json);
				//errorCode = drj.errorCode;
				//this.drj=drj;
				//receiveData=true;
			//	break;
			case 3:			//购买怒气//
				PlayerResultJson prj=JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode=prj.errorCode;
				if(errorCode==0)
				{
					PlayerInfo.getInstance().player=prj.list[0];
				}
				receiveData=true;
				break;
			}	
		}
	}
	
	//购买怒气//
	public void AddEnergy(int param)
	{
		int number=EnergyupData.getNumber(PlayerInfo.getInstance().player.maxEnergy);
		int canBuyNumber = VipData.getData(PlayerInfo.getInstance().player.vipLevel).maxenergy;
		if(number >= canBuyNumber)
		{
			ToastWindow.mInstance.showText(TextsData.getData(240).chinese);
			return;
		}
		EnergyupData ed=EnergyupData.getData(number+1);
		if(ed==null)
		{
			ToastWindow.mInstance.showText(TextsData.getData(240).chinese);
			return;
		}
		else
		{
			ToastWarnUI.mInstance.showWarn(TextsData.getData(245).chinese.Replace("num1",ed.cost+"").Replace("num2",ed.energy+""),this);
		}
	}
	
	public void warnningSure()
	{
		int number=EnergyupData.getNumber(PlayerInfo.getInstance().player.maxEnergy);
		EnergyupData ed=EnergyupData.getData(number+1);
		if(ed==null)
		{
			return;
		}
		else
		{
			if(PlayerInfo.getInstance().player.vipLevel<ed.viplevel)
			{
				ToastWindow.mInstance.showText(TextsData.getData(241).chinese.Replace("num",ed.viplevel+""));
				return;
			}
			if(PlayerInfo.getInstance().player.crystal<ed.cost)
			{
				ToastWindow.mInstance.showText(TextsData.getData(244).chinese);
				return;
			}
			requestType=3;
			PlayerInfo.getInstance().sendRequest(new EnergyJson(),this);
		}
	}
	
	public void warnningCancel(){}
}
