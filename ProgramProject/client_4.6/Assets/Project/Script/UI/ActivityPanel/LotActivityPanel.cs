using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LotActivityPanel : MonoBehaviour,ProcessResponse {
	
	public UILabel activityStartTime;//活动开始时间//
	
	public UILabel activityEndTime;//活动结束时间//
	
	public UILabel activitySurplusTime;//活动剩余时间//
	
	public UILabel playerIntegral;//积分//
	
	public UILabel playerRank;//排名//
	
	public UILabel playerCrystal;//钻石//
	
	public UILabel freeLotTime;//免费抽卡剩余时间//
	
	public UILabel freeLotTime2;
	
	public UILabel lotCardNeedCry;//抽卡需要钻石//
	
	public UILabel explainLab;
	
	public UILabel rewardInfo;
	
	public UILabel lotTips;
	
	public GameObject explainGobj;
	
	public GameObject white;//闪白的特效//
	
	public GameObject lotEffect;//抽卡时的特效//
	
	public GameObject lotResult;//抽卡结果//
	
	public GameObject lotActUi;//抽卡活动界面//
	
	private GameObject fazhenEffect;
	
	private GameObject nvws;
	
	public GameObject word;
	
	private GameObject rotatEffect;
	
	private GameObject headEffect;
	
	public GameObject newMark;
	
	public GameObject cardParent;//内部抽卡展示//
	
	public GameObject actLotCardParent;//外部界面展示//
	
	public GameObject oneCardCtrl;
	
	public GameObject card3DNode;
	
	public GameObject oneCardEffect;
	
	public GameObject label1Ctrl;
	
	public GameObject cardShowParent;
	
	private GameObject curCell;
	
	private GameObject headEffect2;
	
	public UILabel card3DName;
	
	public UISprite card3DStarIcon;
	
	private int activitySurplusTimeNum;
	
	private int freeLotTimeNum;
	
	private int cardsNum;//抽卡卡库//
	
	public List<UILabel> rankList = new List<UILabel>();
	
	public List<UILabel> rankNumList = new List<UILabel>();
	
	public UIButtonMessage freeLotBtn;
	
	public UIButtonMessage crystalLotBtn;
	
	private int requestType;
	
	private bool receiveData;
	
	private bool canClickBtn=true;
	
	public static bool isHaveBtnClick = false;
	
	private int errorCode;
	
	private int whiteMark;
	
	public Color selfColor;
	
	public Color notSelfColor;
	
	public Color outlineColor;
	
	LotEventRankResultJson lerrj;
	
	public PopCardDetailUI pcdUI;
	
	LotResultJson lrjTemp;
	void Start () {
		lotEffect.SetActive(false);
		lotResult.SetActive(false);
		word.SetActive(false);
	}
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			if(errorCode == 0)
			{
				switch(requestType)
				{
				case 1:
					ShowEffect();
					break;
				case 2:
					Init(lerrj);
					break;
				}
				
			}
			else
			{
				switch(errorCode)
				{
				case 100:
					ToastWindow.mInstance.showText(TextsData.getData(742).chinese);
					//活动为空//
					break;
				case 130:
					ToastWindow.mInstance.showText(TextsData.getData(741).chinese);
					//活动时间无效//
					break;
				case 114:
					ToastWindow.mInstance.showText(TextsData.getData(740).chinese);
					//活动未开启//
					break;
				case 71:
				{
					string str = TextsData.getData(244).chinese;
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}break;
				case 127:
				{
					//抽卡免费时间未到//
					ToastWindow.mInstance.showText(TextsData.getData(739).chinese);
				}
				break;
				case 53:
				{
					//背包不足//
					string errorString = TextsData.getData(78).chinese;
					UIJumpTipManager.mInstance.SetPackageTypeData(UIJumpTipManager.UI_JUMP_TYPE.UI_EXTENDPACKAGE, errorString);
				}
				break;
				}
				isHaveBtnClick = false;
			}
		}
	}
	
	public void Init(LotEventRankResultJson lerrj)
	{
		lotActUi.SetActive(true);
		lotEffect.SetActive(false);
		lotResult.SetActive(false);
		word.SetActive(false);
		explainGobj.SetActive(false);
		CancelInvoke("ChangeFreeLotTime");
		CancelInvoke("ChangeActivitySurplusTime");
		HeadUI.mInstance.show();
		PlayerInfo.getInstance().player.crystal = lerrj.crystal;
		this.lerrj = lerrj;
		playerIntegral.text = lerrj.score.ToString();
		playerRank.text = lerrj.rank.ToString();
		playerCrystal.text = lerrj.crystal.ToString();
		activityStartTime.text =TextsData.getData(743).chinese + "  " +  lerrj.bt;
		activityEndTime.text = TextsData.getData(744).chinese + "  " + lerrj.et;
		lotCardNeedCry.text = lerrj.uc.ToString();
		explainLab.text = TextsData.getData(734).chinese;
		rewardInfo.text = TextsData.getData(733).chinese;
		
		List<int> tCardList = CardKuData.getCardIdList(2);
		int xStarNum = 0;
		if(tCardList != null && tCardList.Count > 0)
		{
			CardData tcd = CardData.getData(tCardList[0]);
			if(tcd != null)
			{
				xStarNum = tcd.star;
			}
		}
		if(lerrj.n != 0)
		{
			lotTips.text = TextsData.getData(735).chinese.Replace("num",lerrj.n.ToString());
			lotTips.text = lotTips.text.Replace("star",xStarNum.ToString());
		}
		else
		{
			lotTips.text = TextsData.getData(745).chinese.Replace("star",xStarNum.ToString());
		}
		
		
		for(int i = 0;i<rankList.Count;i++)
		{
			try
			{
				string[] str = lerrj.sr[i].Split('-');
				rankList[i].gameObject.SetActive(true);
				rankList[i].text = str[0] + " 、  "+str[1];
				rankNumList[i].text = str[2];
				if(StringUtil.getInt(str[3]) == 1)
				{
					rankList[i].color = selfColor;
					rankNumList[i].color = selfColor;
					rankList[i].effectStyle = UILabel.Effect.Outline;
					rankList[i].effectColor = outlineColor;
					rankNumList[i].effectStyle = UILabel.Effect.Outline;
					rankNumList[i].effectColor = outlineColor;
				}
				else
				{
					rankList[i].color = notSelfColor;
					rankNumList[i].color = notSelfColor;
					rankList[i].effectStyle = UILabel.Effect.None;
					rankNumList[i].effectStyle = UILabel.Effect.None;
				}
			}
			catch
			{
				rankList[i].gameObject.SetActive(false);
				rankNumList[i].gameObject.SetActive(false);
			}
		}
		
		List<int> cardList = CardKuData.getCardIdList(3);//lerrj.cs);
		
		
		GameObjectUtil.destroyGameObjectAllChildrens(actLotCardParent);
		GameObject cardCell=GameObjectUtil.LoadResourcesPrefabs("ActivityPanel/ActLotCell",3);
		for(int i = 0;i<cardList.Count;i++)
		{
			CardData cd = CardData.getData(cardList[i]);
			GameObject cell = Instantiate(cardCell) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(cell,actLotCardParent,true);
			SimpleCardInfo1 scInfo = cell.GetComponent<SimpleCardInfo1>();
			if(scInfo == null)
			{
				Destroy(cell);
				continue;
			}
			scInfo.setSimpleCardInfo(cd.id,GameHelper.E_CardType.E_Hero);
			scInfo.bm.target = gameObject;
			scInfo.bm.functionName = "onClickCKCard";
			scInfo.bm.param = cd.id;
		}
		actLotCardParent.GetComponent<UIGrid>().repositionNow = true;
		actLotCardParent.GetComponent<UICenterOnChild>().springStrength = cardList.Count;
		
		activitySurplusTimeNum = lerrj.ht;
		freeLotTimeNum = lerrj.lt;
		cardsNum = lerrj.cs;
		InvokeRepeating("ChangeFreeLotTime",0,1);
		InvokeRepeating("ChangeActivitySurplusTime",0,1);
		freeLotBtn.param = 1;
		freeLotBtn.functionName = "OnClickLotCard";
		freeLotBtn.target = gameObject;
		crystalLotBtn.param = 2;
		crystalLotBtn.functionName = "OnClickLotCard";
		crystalLotBtn.target = gameObject;
	}
	
	public void onClickCKCard(int param)
	{
		if(!isHaveBtnClick)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
			pcdUI.setOnlyCardDataContent(param);
			isHaveBtnClick = true;
		}
	}
	
	public void ShowEffect()
	{
				//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGET_MODEL);
		lotEffect.SetActive(true);
		lotResult.SetActive(false);
		lotActUi.SetActive(false);
		HeadUI.mInstance.hide();
		white.GetComponent<UISprite>().alpha=0;
		//播特效//
		if(fazhenEffect==null)
		{
			fazhenEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/haoyou_fazhen_02",1)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(fazhenEffect,lotEffect);
			GameObjectUtil.setGameObjectLayer(fazhenEffect,lotEffect.layer);
			fazhenEffect.transform.localRotation=Quaternion.Euler(new Vector3(30f,180f,0));
			fazhenEffect.transform.localPosition=new Vector3(0,-170f,80f);
		}
		
		if(nvws==null)
		{
			nvws=Instantiate(GameObjectUtil.LoadResourcesPrefabs("chouka",0)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(nvws,lotEffect);
			GameObjectUtil.setGameObjectLayer(nvws,lotEffect.layer);
			nvws.transform.localScale=new Vector3(180f,180f,180f);
			nvws.transform.localRotation=Quaternion.Euler(new Vector3(30f,180f,0));
			nvws.transform.localPosition=new Vector3(0,-170f,80f);
		}

		rotatEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_rotatcard_zi",1)) as GameObject;
		GameObjectUtil.gameObjectAttachToParent(rotatEffect,lotEffect);
		GameObjectUtil.setGameObjectLayer(rotatEffect,lotEffect.layer);
		rotatEffect.transform.localPosition=new Vector3(0,-360f,0);
		rotatEffect.transform.localRotation=Quaternion.Euler(new Vector3(340f,0,0));
		rotatEffect.transform.localScale=new Vector3(100f,100f,100f);
		
		if(headEffect==null)
		{
			headEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/guang_chouka",1)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(headEffect,lotEffect);
			GameObjectUtil.setGameObjectLayer(headEffect,lotEffect.layer);
			headEffect.transform.localPosition=new Vector3(0,80f,0);
		}
		
		Invoke("ShowWhiteEffect",2);
		whiteMark = 1;
	}
	
	public void ShowWhiteEffect()
	{
		TweenAlpha ta=white.GetComponent<TweenAlpha>();
		ta.from=0;
		ta.to=1f;
		ta.duration=1f;
		ta.tweenFactor=0;
		ta.enabled=true;
		
		canClickBtn=false;
	}
	
	//==白背景变换完毕==//
	public void tweenOver()
	{
		if(whiteMark==1)
		{
			Destroy(rotatEffect);
			rotatEffect=null;
			showResult();
			TweenAlpha ta=white.GetComponent<TweenAlpha>();
			ta.from=1f;
			ta.to=0;
			ta.duration=1f;
			ta.tweenFactor=0;
			ta.enabled=true;
			whiteMark=3;
		}
		else if(whiteMark==2)
		{
			//==移动金属字==//
			word.SetActive(true);
			word.GetComponent<UISprite>().transform.localPosition=new Vector3(-480f,0,0);
			TweenPosition tp=word.GetComponent<TweenPosition>();
			tp.from=new Vector3(-480f,0,0);
			tp.to=new Vector3(480f,0,0);
			tp.tweenFactor=0;
			tp.duration=1f;
			tp.enabled=true;
		}
		else if(whiteMark==3)
		{
			moveOver();
		}
		else if(whiteMark==4)
		{
			canClickBtn=true;
		}
	}
	
	private int showResult()
	{
		clearOneCardCtrl();
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_GETCARD);
	
		lotResult.SetActive(true);
		lotEffect.SetActive(false);
		GameObjectUtil.destroyGameObjectAllChildrens(cardParent);
		
		//==按星级降序排序(取消排序)==//
		List<PackElement> pes=new List<PackElement>();
		for(int i=0;i<lrjTemp.list.Count;i++)
		{
			PackElement pe=lrjTemp.list[i];
			pes.Add(pe);
		}
		
		clearOneCardCtrl();
		for(int i=0;i<pes.Count;i++)
		{
			GameObject cell = null;
			PackElement pe=pes[i];
			if(pe == null)
				continue;
			CardData cd=CardData.getData(pe.dataId);
			if(cd == null)
				continue;
			
			oneCardCtrl.SetActive(true);

			GameObject cardModel = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel,STATE.PREFABS_TYPE_CARD))as GameObject;
			if(cardModel == null)
				continue;
			GameObjectUtil.gameObjectAttachToParent(cardModel,card3DNode,true);
			GameObjectUtil.setGameObjectLayer(cardModel,STATE.LAYER_ID_NGUI);
			cardModel.transform.localPosition = new Vector3(0,cd.modelposition,0);
			cardModel.transform.localScale = new Vector3(cd.modelsize,cd.modelsize,cd.modelsize);
			float rotaY = cd.modelrotation;
			cardModel.transform.localEulerAngles =new Vector3(0,rotaY,0);
			GameObjectUtil.hideCardEffect(cardModel);
			card3DName.text = "LV.1  " +cd.name;
			card3DStarIcon.spriteName = "card_side_s"+cd.star.ToString();
			cell = oneCardCtrl;
			
			GameObject cardStarEffect=null;
			
			switch(cd.star)
			{
			case 3:
				break;
			case 4:
				cardStarEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_star_s04",1)) as GameObject;
				break;
			case 5:
				cardStarEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_star_s05",1)) as GameObject;
				break;
			case 6:
				cardStarEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_star_s06",1)) as GameObject;
				break;
			}
			if(cardStarEffect!=null)
			{
				if(pes.Count!=10)
				{
					GameObjectUtil.gameObjectAttachToParent(cardStarEffect,oneCardEffect);
				}
				else
				{
					GameObjectUtil.gameObjectAttachToParent(cardStarEffect,cell);
				}
				GameObjectUtil.setGameObjectLayer(cardStarEffect,cell.layer);
				if(pes.Count!=10)
				{
					cardStarEffect.transform.localPosition = new Vector3(cardStarEffect.transform.localPosition.x,cardStarEffect.transform.localPosition.y,-700);
				}
				cardStarEffect.transform.localScale=new Vector3(200f,200f,200f);
			}
			
			UIButtonMessage msg=cell.GetComponent<UIButtonMessage>();
			msg.target = gameObject;
			msg.functionName="onClickCard";
			msg.param=lrjTemp.list.IndexOf(pe);
			
			//设置位置//
			if(pes.Count==10)
			{
				Vector3 pos=Vector3.zero;
				if(i<pes.Count/2)
				{
					pos=new Vector3((i-2)*140f,90f,0);
				}
				else
				{
					pos=new Vector3((i-2-pes.Count/2)*140f,-90f,0);
				}
				float during=0.3f;
				float delay=0.09f*i;
				
				cell.transform.localPosition=new Vector3(0,600f,0);
				cell.transform.localScale=new Vector3(2f,2f,2f);
				
				TweenPosition tp=cell.GetComponent<TweenPosition>();
				tp.from=new Vector3(0,600f,0);
				tp.to=pos;
				tp.duration=during;
				tp.delay=delay;
				tp.tweenFactor=0;
				tp.enabled=true;
				
				TweenScale ts=cell.GetComponent<TweenScale>();
				ts.from=new Vector3(2f,2f,2f);
				ts.to=new Vector3(0.8f,0.8f,0.8f);
				ts.duration=during;
				ts.delay=delay;
				ts.tweenFactor=0;
				ts.enabled=true;
			}
			else
			{
				
				switch(cd.star)
				{
				case 3:
					word.SetActive(true);
					word.GetComponent<TweenPosition>().enabled=false;
					word.GetComponent<UISprite>().spriteName="title_3";
					break;
				case 4:
					word.SetActive(true);
					word.GetComponent<TweenPosition>().enabled=false;
					word.GetComponent<UISprite>().spriteName="title_4";
					break;
				case 5:
					word.SetActive(true);
					word.GetComponent<TweenPosition>().enabled=false;
					word.GetComponent<UISprite>().spriteName="title_5";
					break;
				case 6:
					word.SetActive(true);
					word.GetComponent<TweenPosition>().enabled=false;
					word.GetComponent<UISprite>().spriteName="title_6";
					break;
				}
				word.SetActive(false);
				if(pe.nw==1)
				{
					//cell.transform.FindChild("new-mark").gameObject.SetActive(false);
					curCell=cell;
				}
			}
		}
		if(headEffect2==null)
		{
			headEffect2=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/guang_chouka",1)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(headEffect2,lotResult);
			GameObjectUtil.setGameObjectLayer(headEffect2,lotResult.layer);
			headEffect2.transform.localPosition=new Vector3(0,80f,0);
		}
		label1Ctrl.SetActive(true);
		//向服务器请求判断是否有新解锁合体技//
		{
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
		}
		return pes.Count;
	}
	
	//==移动金属字完毕==//
	public void moveOver()
	{
		word.SetActive(false);
		
		if(curCell!=null)
		{
			newMark.SetActive(true);
			newMark.transform.localPosition=new Vector3(0f,225f,-720f);
			newMark.transform.localScale=new Vector3(5f,5f,1f);
			TweenScale ts=newMark.GetComponent<TweenScale>();
			ts.from=newMark.transform.localScale;
			ts.to=new Vector3(1.4f,1.4f,1f);
			ts.tweenFactor=0;
			ts.duration=0.3f;
			ts.enabled=true;
		}
		else
		{
			canClickBtn=true;
		}
	}
	
	//==移动new-mark完毕==//
	public void moveOver2()
	{
		curCell=null;
		Invoke("waitForSetCanClickBtn",0.2f);
	}
	
	public void waitForSetCanClickBtn()
	{
		canClickBtn=true;
	}
	
	//==显示卡牌详细信息==//
	public void onClickCard(int param)
	{
		if(!canClickBtn)
		{
			return;
		}
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		
		PackElement pe= lrjTemp.list[param];
		lotResult.transform.FindChild("result-lot").gameObject.SetActive(false);
		PopCardDetailUI pop=lotResult.transform.FindChild("result-cardDetail/pop-cardDetail").GetComponent<PopCardDetailUI>();
		pop.fromActLot=true;
		pop.setContent(pe,null,null,null);
		if(headEffect2 != null)
		{
			headEffect2.SetActive(false);
		}
	}
	
	void clearOneCardCtrl()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(card3DNode);
		GameObjectUtil.destroyGameObjectAllChildrens(oneCardEffect);
		card3DName.text = string.Empty;
		card3DStarIcon.spriteName = "";
		oneCardCtrl.SetActive(false);
	}
	
	public void ChangeFreeLotTime()
	{
		freeLotTime.text = GetNumToTime(freeLotTimeNum);
		if(freeLotTimeNum>0)
		{
			freeLotTime2.text = TextsData.getData(738).chinese;
			freeLotTimeNum--;
		}
		else
		{
			freeLotTime2.text = TextsData.getData(737).chinese;
			CancelInvoke("ChangeFreeLotTime");
		}
	}
	
	public void ChangeActivitySurplusTime()
	{
		activitySurplusTime.text =  TextsData.getData(736).chinese+"  "+GetNumToTime(activitySurplusTimeNum);
		if(activitySurplusTimeNum>0)
		{
			activitySurplusTimeNum--;
		}
		else
		{
			CancelInvoke("ChangeActivitySurplusTime");
		}
	}
	
	private string GetNumToTime(int second)
	{
		int minute = second/60;
		int hour = minute/60;
		second -= minute*60;
		minute -= hour *60;
		string time = hour<10?("0"+hour):hour.ToString();
		time+= minute<10?(":0"+minute):(":"+minute);
		time+= second<10?(":0"+second):(":"+second);
		return time;
	}
	
	public void onClickResultBtn(int param)
	{
		if(!canClickBtn)
		{
			return;
		}
		
		isHaveBtnClick = false;
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		switch(param)
		{
		case 2://确定//
			clickOKBtn();
			break;
		}
	}
	
	public void clickOKBtn()
	{
		requestType = 2;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_ACT_LOTCARD), this);
	}
	
	public void OnClickLotCard(int param)
	{
		if(isHaveBtnClick)
			return;
		isHaveBtnClick = true;
		requestType = 1;
		
		if(param == 1)
		{
			//免费抽卡//
			if(freeLotTimeNum == 0)
			{
				PlayerInfo.getInstance().sendRequest(new LotJson(STATE.ActLotCardType_Free,cardsNum),this);
			}
			else
			{
				ToastWindow.mInstance.showText(TextsData.getData(739).chinese);
				isHaveBtnClick = false;
			}
		}
		else if(param == 2)
		{
			//钻石抽卡//
			if(PlayerInfo.getInstance().player.crystal>=lerrj.uc)
			{
				PlayerInfo.getInstance().sendRequest(new LotJson(STATE.ActLotCardType_Crystal,cardsNum),this);
			}
			else
			{
				string str = TextsData.getData(244).chinese;
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				isHaveBtnClick = false;
			}
		}
	}
	
	public void OnClickShowExplain()
	{
		if(isHaveBtnClick)
			return;
		isHaveBtnClick = true;
		explainGobj.SetActive(true);
	}
	
	public void OnClickHideExplain()
	{
		isHaveBtnClick = false;
		explainGobj.SetActive(false);
	}
	
	public void receiveResponse(string json)
	{
		if (json != null)
        {
            //关闭连接界面的动画//
            PlayerInfo.getInstance().isShowConnectObj = false;
			switch (requestType)
			{
			case 1:
				lrjTemp=JsonMapper.ToObject<LotResultJson>(json);
				errorCode = lrjTemp.errorCode;
				receiveData=true;
				break;
			case 2:
				lerrj = JsonMapper.ToObject<LotEventRankResultJson>(json);
				errorCode = lerrj.errorCode;
				receiveData = true;
				break;
			}
		}
	}
}
