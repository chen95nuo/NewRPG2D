using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExpManager : MonoBehaviour {
	//type 要显示的经验条的类型，0是经验条走。//
	int type;
	int lastMaxExp = 500;
	int curMaxExp = 500;
	int lastExp = 300;
	int curExp = 400;
	//object data = null;
	int starLevel = 0;
	//经验条的背景框//
	public UISprite expSpr;
	

	//需要变化的经验条//
	public GameObject exp;
	public UILabel expChangeLabel;
	public Vector3 expScale = new Vector3(1,1,1) ;
	public float scaleTime = 5f;
	public GameObject Exp_Full_Yellow;
	public GameObject Exp_Full_Light;
	//float showFullLightTime = 0.4f;
	//float totalShowTime = 1f;
	float emptyToFullExpTime = 0.5f;
	float showExpFrame = 0;
	
	//初始状态是这些//
	//public int expBoxW = 94;
	//public int expW = 84;
	
//	//经验条在界面上的坐标//
//	public float posX = -116f;
//	public float posY = 238.58f;
//	
//	
	/*private float playerChangedExpNum;
	private float changeDesExp; 
	private float ExpFrameCount;
	private float changeExpTime ;*/
	//private Battle_Player_Info battlePlayerInfo;
	// mark - xuyan //
	float curChangeExp = 0.0f;
	//float curChangeTime = 0.0f;
	float curStartExp = 0.0f;
	float curEndExp = 0.0f;
	float needChangeTotalExp = 0.0f;			//要改变的总经验值，不会改变//
	float needChangeExp = 0.0f;					//当前等级要走的经验值//
	int lastLevel = 0;
	int  curLevel = 0;
	//float remainChangeTime = 0.0f;
	float remainExp = 0.0f;						//当前要修改的经验值//
	float curStartScale = 0.0f;					//当前的经验条的比例//
	float curEndScale = 0.0f;					//加完经验值后经验条的缩放比例//
	float runningTime = 0.0f;
	bool running = false;
	// mark end - xuyan //
	
	bool isPauseRunning = false;
	//float pauseFrameCount = 0;
	
	bool needLevelUp = false;
	
//	GameObject modelOfExp;						//当前的经验条对应的模型//
//	Vector3 effStartPos ;						//升级特效的位置//
//	Vector3 levelUpStartPos;					//卡牌升级图片的起始位置//
//	Vector3 levelUpEndPos;						//卡牌升级图片的结束位置//
//	List<Effect> levelUpEffList = new List<Effect>();				//升级特效列表//
//	float curTime;
//	float keepTime;
	
	//int modelPosId;
	ResultCardManager resultCardMan ;
	SweepUIManager sweepUiMan;
	

	// Use this for initialization
	void Start () 
	{
		if(expSpr == null)
		{
			expSpr = exp.GetComponent<UISprite>();
		}
	}
	
	public void init()
	{
		if(type< 10)
		{
			//battlePlayerInfo = Battle_Player_Info.GetInstance();
			//remainChangeTime = battlePlayerInfo.scaleExpTime;
			switch(type)
			{
			case STATE.EXP_TYPE_RESULT_CARD:
			case STATE.EXP_TYPE_MOVE_SWEEP_CARD:
			{
				if(curLevel > lastLevel)
				{
					needChangeTotalExp = curExp;
					for(int i = curLevel; i > lastLevel;--i)
					{
						needChangeTotalExp += CardExpData.getExp(i, starLevel);
					}
					needChangeTotalExp -= lastExp;
				}
				else
				{
					needChangeTotalExp = curExp - lastExp;
				}
			}break;
			case STATE.EXP_TYPE_RESULT_PLAYER:
			{
				if(curLevel > lastLevel)
				{
					needChangeTotalExp = curExp;
					for(int i = curLevel; i > lastLevel;--i)
					{
						needChangeTotalExp += getPlayerMaxExpByLevel(i);
					}
					needChangeTotalExp -= lastExp;
				}
				else
				{
					needChangeTotalExp = curExp - lastExp;
				}
				
			}break;
			
			}
			if(expChangeLabel != null)
			{
				expChangeLabel.text = "+" + needChangeTotalExp.ToString();
				expChangeLabel.gameObject.SetActive(true);
			}
			remainExp = needChangeTotalExp;
			curStartExp = lastExp;
			setPerformanceParams();
		}
		else if(type >= 10)
		{
			float sN = calcScale(curExp,curMaxExp);
			expSpr.fillAmount = sN;
		}
	}
	
	public float GetNeedChangeNum(){
		return needChangeExp;
	}
	
	void setPerformanceParams()
	{
		switch(type)
		{
		case STATE.EXP_TYPE_RESULT_CARD:
		case STATE.EXP_TYPE_MOVE_SWEEP_CARD:
		{
			lastMaxExp = CardExpData.getExp(lastLevel + 1, starLevel);
		}break;
		case STATE.EXP_TYPE_RESULT_PLAYER:
		{
			lastMaxExp = getPlayerMaxExpByLevel(lastLevel + 1);
		}break;
		
		}
		
		curEndExp = lastMaxExp; 
		float deltaExp = curEndExp - curStartExp;
		if(deltaExp > remainExp)
		{
			curChangeExp = remainExp;
		}
		else
		{
			curChangeExp = deltaExp;
		}
		remainExp -= curChangeExp;
		curStartScale = calcScale(curStartExp,curEndExp);
		curEndScale = calcScale(curStartExp+curChangeExp,curEndExp);
		if(curChangeExp == 0)
		{

			running = false;
			if(expChangeLabel != null && expChangeLabel.gameObject.activeSelf)
			{
				expChangeLabel.gameObject.SetActive(false);
			}
		}
		else
		{
			running  = true;
			runningTime = 0;

		}

		if(curStartScale <= 0)
		{
			curStartScale = 0;
		}
		if(expSpr == null)
		{
			expSpr = exp.GetComponent<UISprite>();
		}
		expSpr.fillAmount = curStartScale;
		
	}
	
	float calcScale(float exp,float endExp)
	{
		if(exp <= 0)
		{
			return 0;
		}
		else
		{
			return exp/endExp;	
		}
	}
	
//	//从卡牌表中获得卡牌的经验值的上限//
//	int getCardMaxExpByLevel(int level)
//	{
//		// todo
//		return 0;
//	}
	
	//从人物表中根据人物的等级获取人物的经验值上限//
	int getPlayerMaxExpByLevel(int playerLevel)
	{
		PlayerData pd = PlayerData.getData(playerLevel);
		return pd.exp;
	}
//	int showExp = 0;
	// Update is called once per frame
	void Update ()
	{
		// mark - xuyan //
		if(running)
		{
			if(runningTime <= emptyToFullExpTime)
			{
				int showExp = (int)(curStartExp + curChangeExp*runningTime/emptyToFullExpTime);
				if(expChangeLabel != null && expChangeLabel.gameObject.activeSelf)
				{
					showExpFrame+=Time.deltaTime;
					if(showExpFrame > 0.3f)
					{
						showExpFrame = 0f;
						expChangeLabel.gameObject.SetActive(false);
					}
				}
				
				float scaleX = calcScale(showExp, curEndExp);
				if(scaleX == 0)
				{
					scaleX = 0;
				}
				expSpr.fillAmount = scaleX;
				runningTime+= Time.deltaTime;
				if(remainExp <=0 && curChangeExp <= 0){
					if(type == STATE.EXP_TYPE_RESULT_CARD){
//						
						if(ResultTipManager.mInstance!= null){
							
							ResultTipManager.mInstance.ShowCard(ResultTipManager.mInstance.state2ShowCardNum);
						}

					}
					else if(type == STATE.EXP_TYPE_RESULT_PLAYER){
						
						//如果有bounes奖励，则开始播放bounes动画//
						if(ResultTipManager .mInstance!= null){
							
							ResultTipManager.mInstance.ShowBounesAnim();
						}
					}
				}
			}
			else
			{
				running = false;
				int showExp = 0;
				
				showExp = (int)(curStartExp + curChangeExp);
				if(expChangeLabel != null)
				{
//					expChangeLabel.text = showExp +"/" + curEndExp;
				}
				
				if(showExp >= curEndExp)
				{
					needLevelUp = true;
				}
				else 
				{
					needLevelUp = false;
				}
				
				float scaleX = curEndScale;
//				exp.transform.localScale = new Vector3(expScale.x * scaleX, expScale.y , expScale.z);
				expSpr.fillAmount = scaleX;
				
				if(needLevelUp)
				{
					isPauseRunning = true;
					//播放音效//
					MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_LEVELUP);
					if(Exp_Full_Light != null && Exp_Full_Yellow != null)
					{
						Exp_Full_Yellow.SetActive(true);
						Exp_Full_Light.SetActive(true);
						GameObjectUtil.PlayTweenAlpha(Exp_Full_Light, 1f, 0.3f, "LightFlashingChange", 0.29f);
					}
				}
				
				
				if(remainExp <= 0)
				{
					
					if(type == STATE.EXP_TYPE_RESULT_PLAYER){
						
						//如果有bounes奖励，则开始播放bounes动画//
						if(ResultTipManager .mInstance!= null){
							
							ResultTipManager.mInstance.ShowBounesAnim();
						}
					}
					
					else if(type == STATE.EXP_TYPE_RESULT_CARD){
						if(ResultTipManager.mInstance!= null){
							
							ResultTipManager.mInstance.ShowCard(ResultTipManager.mInstance.state2ShowCardNum);
                        }
					}
				}
				
//				if(Exp_Full_Light == null)
//				{
//					if(remainExp > 0)
//					{
//						running = true;
//						curStartExp = 0;
//						lastLevel++;
//						
////						exp.transform.localScale = new Vector3(0.0001f, expScale.y , expScale.z);
//						expSpr.fillAmount = 0;
//						setPerformanceParams();
//						
//						if(type == STATE.EXP_TYPE_RESULT_CARD && resultCardMan != null)
//						{
//							resultCardMan.ChangeLvLabel(lastLevel);
//							resultCardMan.ShowCardLevelUpEff();
//						}
//						else if(type == STATE.EXP_TYPE_RESULT_PLAYER)
//						{
//							ResultTipManager.mInstance.ChangePlayerLevel(lastLevel);
//						}
//					}
//					else if(remainExp == 0 && needLevelUp)
//					{
//						curStartExp = 0;
//						lastLevel++;
////						exp.transform.localScale = new Vector3(0.0001f, expScale.y , expScale.z);
//						expSpr.fillAmount = 0;
//						setPerformanceParams();
//						if(expChangeLabel != null)
//						{
//							expChangeLabel.text =   "0/" + curEndExp.ToString();
//						}
//						needLevelUp = false;
//						
//						if(type == STATE.EXP_TYPE_RESULT_CARD && resultCardMan != null)
//						{
//							resultCardMan.ChangeLvLabel(lastLevel);
//							resultCardMan.ShowCardLevelUpEff();
//						}
//						else if(type == STATE.EXP_TYPE_RESULT_PLAYER)
//						{
//							ResultTipManager.mInstance.ChangePlayerLevel(lastLevel);
//						}
//					}
//					return;
//				}
//				else 
//				{
//					if(!isPauseRunning && pauseFrameCount==0 && expSpr.fillAmount>=1 )
//					{
//						isPauseRunning = true;
//						//播放音效//
//						MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_LEVELUP);
////						//显示高光图片//
//						if(Exp_Full_Yellow != null)
//						{
//							Exp_Full_Yellow.SetActive(true);
//							if(type == STATE.EXP_TYPE_RESULT_CARD){
//								BattleResultControl.mInstance.CreateEffectById(10003, modelPosId);
//								BattleResultControl.mInstance.CreateEffectByName("CardLevelUp", modelPosId, 0.5f, 2);
//							}
//						}
//					}
//				}

			}	
		}
		
		if(isPauseRunning )
		{
			if(type == STATE.EXP_TYPE_RESULT_CARD || type == STATE.EXP_TYPE_RESULT_PLAYER || type == STATE.EXP_TYPE_MOVE_SWEEP_CARD)
			{
				isPauseRunning = false;
				//pauseFrameCount = 0;
			
				if(remainExp >= 0)
				{
					running = true;
					curStartExp = 0;
					lastLevel++;
					expSpr.fillAmount = 0;
					setPerformanceParams();
					if((type == STATE.EXP_TYPE_RESULT_CARD || type == STATE.EXP_TYPE_MOVE_SWEEP_CARD) && resultCardMan != null)
					{
						resultCardMan.ChangeLvLabel(lastLevel);
						resultCardMan.ShowCardLevelUpEff();
					}
					else if(type == STATE.EXP_TYPE_RESULT_PLAYER)
					{
						ResultTipManager.mInstance.ChangePlayerLevel(lastLevel);
						if(!TalkingDataManager.isTDPC)
						{
                        	//TDGAAccount account = TDGAAccount.SetAccount(PlayerPrefs.GetString("username"));
                        	//account.SetLevel(lastLevel);
						}
					}
					
				}
				else if(remainExp == 0 && needLevelUp)
				{
					curStartExp = 0;
					lastLevel++;
					expSpr.fillAmount = 0;
					setPerformanceParams();
					needLevelUp = false;
					
					if((type == STATE.EXP_TYPE_RESULT_CARD || type == STATE.EXP_TYPE_MOVE_SWEEP_CARD) && resultCardMan != null)
					{
						resultCardMan.ChangeLvLabel(lastLevel);
						resultCardMan.ShowCardLevelUpEff();
					}
					else if(type == STATE.EXP_TYPE_RESULT_PLAYER)
					{
						ResultTipManager.mInstance.ChangePlayerLevel(lastLevel);
						if(!TalkingDataManager.isTDPC)
						{
                        	TDGAAccount account = TDGAAccount.SetAccount(PlayerPrefs.GetString("username"));
							account.SetLevel(lastLevel);
						}
					}
				}
			}
			
		}
	}
	
	//posId第几个模型对应的经验条//
	public void setData02(int expType, float lastE, int lastL, float curE, int curL, int star,
		object dataParam = null, int posId = 0, ResultCardManager rcm = null)
	{
		//modelPosId = posId;
		resultCardMan = rcm;
		setData(expType, lastE, lastL, curE, curL, star, dataParam);
	}
	
	/// <summary>
	/// Sets the data.
	/// </summary>
	/// <param name='expType'>
	/// Exp type. 当前当前经验条的类型，是否动0 动 在结算界面用， 1不动//
	/// </param>
	/// <param name='lastE'>
	/// Last e. 战斗之前的经验值
	/// </param>
	/// <param name='lastL'>
	/// Last level.		战斗之前的Level
	/// </param>
	/// <param name='curE'>
	/// Current e.		战斗之后的经验值
	/// </param>
	/// <param name='curL'>
	/// Current l.		战斗后的等级
	/// </param>
	/// <param name='star'>
	/// Star.			如果当前经验条是卡牌的经验条，则还需要卡牌的星级
	/// </param>
	/// <param name='dbID'>
	/// dbID.		可以不传 目前仅技能需要没有升级的数据库ID
	/// </param>
	public void setData(int expType, float lastE, int lastL, float curE, int curL, int star,object dataParam = null)
	{
		reset();
		type = expType;
		lastExp =  (int)lastE;
		curExp = (int)curE;
		lastLevel = lastL;
		curLevel = curL;
		//data = dataParam;
		starLevel = star;
		switch(type)
		{
		case STATE.EXP_TYPE_RESULT_CARD:
		case STATE.EXP_TYPE_MOVE_SWEEP_CARD:
		{
			curMaxExp = CardExpData.getExp(curLevel + 1, starLevel);
			lastMaxExp = CardExpData.getExp(lastLevel + 1, starLevel);
			if(lastMaxExp <= 0){
				lastMaxExp = curMaxExp;
			}
		}break;
		case STATE.EXP_TYPE_OTHER_CARD:
		{
			curMaxExp = CardExpData.getExp(curLevel + 1, starLevel);
		}break;
		case STATE.EXP_TYPE_RESULT_PLAYER:
		{
			lastMaxExp = getPlayerMaxExpByLevel(lastLevel + 1);
			curMaxExp = getPlayerMaxExpByLevel(curLevel + 1);
			if(lastMaxExp <= 0){
				lastMaxExp = curMaxExp;
			}
		}break;
		case STATE.EXP_TYPE_OTHER_PALYER:
		{
			curMaxExp = getPlayerMaxExpByLevel(curLevel + 1);
		}break;
		
			
		}
		init();
	}
	
	public void LightFlashingChange()
	{
		GameObjectUtil.PlayTweenAlpha(Exp_Full_Light, 0.3f, 1f, "", 0.3f);
	}
    
	public void reset()
	{
		running = false;
		isPauseRunning = false;
		expScale = new Vector3(1,1,1);
		exp.transform.localScale = expScale;
		expSpr.fillAmount = 1;
	}
	
	
	
	
	
}
