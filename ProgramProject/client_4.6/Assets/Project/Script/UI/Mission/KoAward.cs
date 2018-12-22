using UnityEngine;
using System.Collections;

public class KoAward : MonoBehaviour ,ProcessResponse,BWWarnUI{
	
	int requestType;
	
	public ChangeCardDetail ccd;
	
	bool receiveData;
	
	public GameObject maps;
	
	public GameObject other;
	
	public MissionUI mission;
	
	//private int haveNum;
	
	CardGroupResultJson cardGroupRJ;
	
	KOExchangeResultJson koerj;
	
	private PackElement card;
	
	int errorCode;
	
	int curExchangeId;
	
	 NewUnitSkillResultJson nusrj;
	//public GameObject PointPos;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			switch(requestType){
			case 2:
				hide();
				mission.show();
				break;
			case 3:
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_KO_Exchange))
				{
					GuideUI7_KOExchange.mInstance.hideAllStep();
					UISceneDialogPanel.mInstance.showDialogID(42);
				}
				ToastWarnUI.mInstance.showWarn(TextsData.getData(85).chinese,this,TextsData.getData(9).chinese,TextsData.getData(179).chinese);
				//添加卡牌获得统计@zhangsai//
				KOAwardData kad = KOAwardData.getData(curExchangeId);
				int id = StringUtil.getInt(kad.reward1.Split(',')[0]);
				if(!UniteSkillInfo.cardUnlockTable.ContainsKey(id))
					UniteSkillInfo.cardUnlockTable.Add(id,true);
				break;
			case 4:
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
				CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,"CombinationInterManager")as CombinationInterManager;
				combination.curCardGroup=cardGroupRJ .transformCardGroup();
				combination.SetData(1);
				mission.hide();
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_KO_Exchange))
				{
					ToastWarnUI.mInstance.hide();
					GuideUI7_KOExchange.mInstance.hide();
					GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_KO_Exchange);
					GuideUI_CardInTeam2.mInstance.showStep(1);
				}
				break;
			case 5:
				mission.showKoExchange();
				requestType = 6;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
				break;
			case 6:	
				if(nusrj.errorCode == 0)
				{
					if(!nusrj.unitskills.Equals(""))
					{
						UniteSkillUnlockManager.mInstance.SetDataAndShow(nusrj.unitskills,mission.gameObject);
						hide();
					}
				}
				break;
			}
		}
	}
	
	public void show()
	{
		gameObject.SetActive(true);
		maps.SetActive(true);
		other.SetActive(true);
		//PointPos.SetActive(false);
		transform.parent.GetComponent<MissionUI2>().canMove = false;
		
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_KO_Exchange))
		{
			GuideUI7_KOExchange.mInstance.hideAllStep();
			UISceneDialogPanel.mInstance.showDialogID(41);
		}
		
	}
	
	public void hide()
	{
		transform.parent.GetComponent<MissionUI2>().canMove = true;
		gameObject.SetActive(false);
		gc();
	}
	
	public void gc()
	{
		
	}
	
	public void back()
	{
		//PointPos.SetActive(true);
		//hide();
		requestType=2;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
	}
	
	public void clickGuideBtn()
	{
//		onClickBtn(1);
	}
	
	void onClickCardShowDetail(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		gameObject.SetActive(false);
		maps.SetActive(false);
		other.SetActive(false);
		ccd.setContent(param);
	}
	
	//点击确认,返回ko兑换界面//
	public void warnningCancel()
	{
		requestType = 5;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_KO_EXCHANGE1),this);
	}
	
	//点击去阵容//
	public void warnningSure()
	{
		requestType = 4;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0),this);
	}
	
	//点击兑换按钮//
	public void OnClickKoExchange(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(param == -1)
		{
			//提示已经兑换//
			ToastWindow.mInstance.showText(TextsData.getData(619).chinese);
			return;
		}
		curExchangeId = param;
		requestType = 3;
		UIJson uijson = new UIJson();
		uijson.UIJsonForGift(STATE.UI_KO_EXCHANGE,param);
		PlayerInfo.getInstance().sendRequest(uijson,this);
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 2:
				MapResultJson mrj = JsonMapper.ToObject<MapResultJson>(json);
				errorCode = mrj.errorCode;
				if(errorCode == 0)
				{
					receiveData = true;
				}
				break;
			case 3:
				koerj = JsonMapper.ToObject<KOExchangeResultJson>(json);
				errorCode = koerj.errorCode;
				if(errorCode == 0)
				{
					//haveNum = koerj.point;
					mission.koerj = koerj;
					receiveData = true;
				}
				else if(errorCode == 20)
				{
					
				}
				break;
			case 4:
				//阵容（卡组）//
				CardGroupResultJson cgrj=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj.errorCode;
				if(errorCode == 0)
				{
					cardGroupRJ = null;
					cardGroupRJ = cgrj;
					receiveData = true;
				}
				break;
			case 5:
				//刷新ko界面数据//
				KOExchangeResultJson kerj = JsonMapper.ToObject<KOExchangeResultJson>(json);
				errorCode = kerj.errorCode;
				if(errorCode == 0)
				{
					mission.koerj = kerj;
					receiveData = true;
				}
				break;
			case 6:	
				nusrj = JsonMapper.ToObject<NewUnitSkillResultJson>(json);
				errorCode = nusrj.errorCode;
				receiveData = true;
				break;
			}
		}
	}
}
