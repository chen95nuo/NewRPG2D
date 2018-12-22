using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewExpManager : MonoBehaviour 
{
	public enum PANELTYPE : int
	{
		E_Null = -1,
		E_Intensify = 0,
		E_CardBreak = 1,
	}
	
	PANELTYPE panelType = PANELTYPE.E_Null;
	
	//type 要显示的经验条的类型//
	float needChangeTotalExp = 0.0f;					//要改变的总经验值，不会改变//
	int type;
	int lastMaxExp = 0;
	int curMaxExp = 0;
	int lastExp = 0;
	int curExp = 0;
	int lastLevel = 0;
	int  curLevel = 0;
	
	float curStartExp = 0.0f;
	float curEndExp = 0.0f;
	
	float curChangeExp = 0.0f;					//当前等级要走的经验//
	float remainExp = 0.0f;						//当前要修改的经验值//
	float curStartScale = 0.0f;					//当前的经验条的比例//
	float curEndScale = 0.0f;					//加完经验值后经验条的缩放比例//
	
	object data = null;
	int starLevel = 0;
	
	public UILabel expChangeLabel;
	public UISprite expSpr;
	public GameObject Exp_Full_Yellow;
	public GameObject Exp_Full_Light;
	
	public UILabel shakeExpChangeLabel;
	public UISprite shakeExpSpr;
	
	// exp full flicker time
	float showFullLightTime = 0.3f;
	float emptyToFullExpTime = 0.5f;

	float runningTime = 0.0f;
	bool running = false;
	
	bool isPauseRunning = false;
	float pauseFrameCount = 0;
	
	bool needLevelUp = false;
	
	public bool finishShowExp = true;
	
	public int expBrPositionX;
	public int expBrLength =146;
	
	public int maxLevel = 0;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	public float GetNeedChangeNum()
	{
		return needChangeTotalExp;
	}
	
	void setPerformanceParams()
	{
		switch(type)
		{
		case STATE.EXP_TYPE_RESULT_CARD:
		{
			lastMaxExp = CardExpData.getExp(lastLevel + 1, starLevel);
		}break;
		case STATE.EXP_TYPE_MOVE_SKILL:
		{
			if(data == null)
			{
				return;
			}
			PackElement dbs = (PackElement)data;
			if(dbs != null)
			{
				if(lastLevel>=PlayerInfo.getInstance().player.level)
				{
					lastMaxExp = 0;
				}
				else
				{
					lastMaxExp = SkillExpData.getExp(lastLevel + 1,starLevel);	
				}
			}
			
		}break;
		case STATE.EXP_TYPE_MOVE_PASSIVESKILL:
		{
			if(data == null)
			{
				return;
			}
			PackElement dbps = (PackElement)data;
			if(dbps != null)
			{
				PassiveSkillData psd = PassiveSkillData.getData(dbps.dataId);
				if(psd != null)
				{					
					int cLevel = psd.level;
					int lastPSDID = dbps.dataId - (cLevel - lastLevel) + 1;
					PassiveSkillData lastPSD = PassiveSkillData.getData(lastPSDID);
					if(lastPSD != null)
					{
						lastMaxExp = lastPSD.exp;
					}
					else
					{
						lastMaxExp = 0;
					}
				}
			}
		}break;
		case STATE.EXP_TYPE_MOVE_BREAK:
		{
			if(data == null)
			{
				return;
			}
			PackElement pe = (PackElement)data;
			if(pe == null)
				return;
			CardData cd = CardData.getData(pe.dataId);
			if(cd == null)
				return;
			if(lastLevel >= 5)
			{
				lastMaxExp = 0;
			}
			else
			{
				lastMaxExp = EvolutionData.getData(cd.star,lastLevel + 1).cards;
			}
			
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
			finishShowExp = true;
			running = false;
			if(expChangeLabel != null)
			{
				expChangeLabel.text = curStartExp +"/" + curEndExp;
				if(curEndExp == 0)
				{
					expChangeLabel.text = "max";
				}
			}
		}
		else
		{
			if(curChangeExp <= 10)
			{
				emptyToFullExpTime = 0.02f * curChangeExp;
			}
			else
			{
				emptyToFullExpTime = 0.5f;
			}
			runningTime = 0;
			running = true;
		}
		if(curStartScale <= 0)
		{
			curStartScale = 0;
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
	
	void Update ()
	{
		if(running)
		{
			if(runningTime <= emptyToFullExpTime)
			{
				int showExp = (int)(curStartExp + curChangeExp * runningTime/emptyToFullExpTime);
				if(expChangeLabel != null)
				{
					expChangeLabel.text = showExp +"/" + curEndExp;
				}
				float scaleX = calcScale(showExp,curEndExp);
				if(scaleX == 0)
				{
					scaleX = 0;
				}
				expSpr.fillAmount = scaleX;
				runningTime+= Time.deltaTime;
			}
			else
			{
				running = false;
				int showExp = 0;
				showExp = (int)(curStartExp + curChangeExp);
				if(expChangeLabel != null)
				{
					expChangeLabel.text = showExp +"/" + curEndExp;
					if(curEndExp == 0)
					{
						expChangeLabel.text = "max";
					}
				}
				if(showExp >= curEndExp)
				{
					needLevelUp = true;
				}
				else 
				{
					finishShowExp = true;
					needLevelUp = false;
				}
				float scaleX = curEndScale;
				expSpr.fillAmount = scaleX;
				
				if(needLevelUp)
				{
					isPauseRunning = true;
					//播放音效//
					MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_LEVELUP);
					//显示高光图片//
					Exp_Full_Yellow.SetActive(true);
					Exp_Full_Light.SetActive(true);
					GameObjectUtil.PlayTweenAlpha(Exp_Full_Light, 1f, 0.3f, "LightFlashingChange", 0.29f);
				}
			}
		}
		
		if(isPauseRunning)
		{
			if(pauseFrameCount <= showFullLightTime)
			{
				pauseFrameCount+= Time.deltaTime;
			}
			else
			{
				isPauseRunning = false;
				pauseFrameCount = 0;
				Exp_Full_Light.SetActive(false);
				Exp_Full_Yellow.SetActive(false);
				if(remainExp > 0)
				{
					running = true;
					curStartExp = 0;
					lastLevel++;
					expSpr.fillAmount = 0;
					setPerformanceParams();
					
				}
				else if(remainExp == 0 && needLevelUp)
				{
					curStartExp = 0;
					lastLevel++;
					expSpr.fillAmount = 0;
					setPerformanceParams();
					if(expChangeLabel != null)
					{
						expChangeLabel.text =   "0/" + curEndExp.ToString();
						if(curEndExp == 0)
						{
							expChangeLabel.text = "max";
						}
					}
					needLevelUp = false;
				}
				switch(panelType)
				{
				case PANELTYPE.E_Intensify:
				{
					IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
						"IntensifyPanel")as IntensifyPanel;
					
					if(intensify != null)
					{
//						IntensifyPanel.mInstance.notifyLevelUp(lastLevel);
						intensify.notifyLevelUp(lastLevel,maxLevel);
					}	
				}break;
				case PANELTYPE.E_CardBreak:
				{
					// TODO	
					CardBreakPanel cardBreak = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_BREAKCARD, "CardBreakPanel")as CardBreakPanel;
					if(cardBreak != null)
					{
						cardBreak.notifyLevelUp(lastLevel);
					}
				}break;
				}
				
			}
		}
	}

	public void setData(int expType, float lastE, int lastL, float curE, int curL, int star,object dataParam = null)
	{
		reset();
		type = expType;
		lastExp =  (int)lastE;
		curExp = (int)curE;
		lastLevel = lastL;
		curLevel = curL;
		data = dataParam;
		starLevel = star;
		if(type < 10)
		{
			finishShowExp = false;
			switch(type)
			{
			case STATE.EXP_TYPE_RESULT_CARD:
			{
				curMaxExp = CardExpData.getExp(curLevel + 1, starLevel);
				lastMaxExp = CardExpData.getExp(lastLevel + 1, starLevel);
				if(lastMaxExp <= 0)
				{
					lastMaxExp = curMaxExp;
				}
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
			case STATE.EXP_TYPE_MOVE_SKILL:
			{
				if(data == null)
				{
					return;
				}
				PackElement pe = (PackElement)data;
				if(pe == null)
				{
					return;
				}
				if(lastLevel>=PlayerInfo.getInstance().player.level)
				{
					return;
				}
				curMaxExp = SkillExpData.getExp(curLevel + 1, starLevel);
				lastMaxExp = SkillExpData.getExp(lastLevel + 1, starLevel);
				if(curLevel > lastLevel)
				{
					needChangeTotalExp = curExp;
					for(int i = curLevel; i > lastLevel; --i)
					{
						needChangeTotalExp += SkillExpData.getExp(i,starLevel);
					}
					needChangeTotalExp -= lastExp;
				}
				else
				{
					needChangeTotalExp = curExp - lastExp;
				}
			}break;
			case STATE.EXP_TYPE_MOVE_PASSIVESKILL:
			{
				if(data == null)
				{
					return;
				}
				PackElement pe = (PackElement)data;
				if(pe == null)
				{
					return;
				}
				int lastPSDID = pe.dataId - (curLevel - lastLevel) +1;
				PassiveSkillData lastPSD = PassiveSkillData.getData(lastPSDID);
				if(lastPSD != null)
				{
					lastMaxExp = lastPSD.exp;
				}
				if(curLevel > lastLevel)
				{
					needChangeTotalExp = curExp;
					
					int curPassiveSkillID = pe.dataId;
					for(int i = curLevel; i > lastLevel; --i)
					{
						int tempPassiveSkillID = curPassiveSkillID - (curLevel - i);
						PassiveSkillData tpsd = PassiveSkillData.getData(tempPassiveSkillID);
						if(tpsd != null)
						{
							needChangeTotalExp += tpsd.exp;
						}
					}
					needChangeTotalExp -= lastExp;
				}
				else
				{
					needChangeTotalExp = curExp - lastExp;
				}
			}break;	
			case STATE.EXP_TYPE_MOVE_BREAK:
			{
				if(data == null)
					return;
				PackElement pe = (PackElement)data;
				if(pe == null)
					return;
				CardData cd = CardData.getData(pe.dataId);
				if(cd == null)
					return;
				if(curLevel >= 5)
				{
					curMaxExp = 0;
				}
				else
				{
					curMaxExp = EvolutionData.getData(cd.star,curLevel+1).cards;
				}
				if(lastLevel >= 5)
				{
					lastMaxExp = 0;
				}
				else
				{
					lastMaxExp = EvolutionData.getData(cd.star,lastLevel+1).cards;
				}
				if(curLevel > lastLevel)
				{
					needChangeTotalExp = curExp;
					for(int i = curLevel; i > lastLevel;--i)
					{
						needChangeTotalExp += EvolutionData.getData(cd.star,i).cards;
					}
					needChangeTotalExp -= lastExp;
				}
				else
				{
					needChangeTotalExp = curExp - lastExp;
				}
			}break;
			}
			remainExp = needChangeTotalExp;
			curStartExp = lastExp;
			setPerformanceParams();
		}
		else
		{
			finishShowExp = true;
			switch(type)
			{
			case STATE.EXP_TYPE_OTHER_CARD:
			{
				curMaxExp = CardExpData.getExp(curLevel + 1, starLevel);
			}break;
			case STATE.EXP_TYPE_STATIC_SKILL:
			{
				if(data == null)
				{
					return;
				}
				PackElement pe = (PackElement)data;
				if(pe == null)
				{
					return;
				}
				
				if(lastLevel>=PlayerInfo.getInstance().player.level)
				{
					return;
				}
				else
				{
					curMaxExp =  SkillExpData.getExp(pe.lv+1,star);
				}
				
			}break;
			case STATE.EXP_TYPE_STATIC_PASSIVESKILL:
			{
				if(data == null)
				{
					return;
				}
				PackElement pe = (PackElement)data;
				if(pe != null)
				{
					PassiveSkillData nextPSD = PassiveSkillData.getData(pe.dataId+1);
					if(nextPSD != null)
					{
						curMaxExp = nextPSD.exp; 
					}
					else
					{
						curMaxExp = 0;
					}
				}
			}break;
			}
			if(expChangeLabel != null)
			{
				if(curMaxExp == 0)
				{
					expChangeLabel.text = "max";
					expSpr.fillAmount = 0;
				}
				else
				{
					expChangeLabel.text = curExp.ToString() + "/" + curMaxExp.ToString();
					float sN = calcScale(curExp,curMaxExp);
					expSpr.fillAmount = sN;
				}
			}
		}
	}
	
	public void LightFlashingChange()
	{
		GameObjectUtil.PlayTweenAlpha(Exp_Full_Light, 0.3f, 1f, "", 0.3f);
	}
    
	public void reset()
	{
		running = false;
		isPauseRunning = false;
	}
	
	public void setPanelType(PANELTYPE pt)
	{
		panelType = pt;
	}
	
	public void recover()
	{
		expSpr.gameObject.SetActive(true);
		expSpr.gameObject.GetComponent<TweenAlpha>().enabled = false;
		expSpr.alpha = 1;
		expChangeLabel.gameObject.SetActive(true);
		
		shakeExpSpr.gameObject.GetComponent<TweenAlpha>().enabled = false;
		shakeExpSpr.alpha = 0;
		shakeExpSpr.gameObject.SetActive(false);
		shakeExpChangeLabel.gameObject.SetActive(false);
	}
	
	public void showExpShake(int expType,int curL,int curE,PackElement pe)
	{
		switch(expType)
		{
		case STATE.EXP_TYPE_MOVE_BREAK:
		{
			if(pe == null)
				return;
			CardData cd = CardData.getData(pe.dataId);
			if(cd == null)
				return;
		
			if(curL == 5)
			{
				shakeExpChangeLabel.text = "max";
				shakeExpSpr.fillAmount = 0;
			}
			else
			{
				shakeExpChangeLabel.text = curE + "/" + EvolutionData.getData(cd.star,curL+1).cards;
				shakeExpSpr.fillAmount = ((float)curE)/((float)EvolutionData.getData(cd.star,curL+1).cards);
			}
		}break;
		}
		
		showShake();
	}
	
	public void showShake()
	{
		expSpr.gameObject.SetActive(false);
		expChangeLabel.gameObject.SetActive(false);
		
		shakeExpSpr.gameObject.SetActive(true);
		shakeExpChangeLabel.gameObject.SetActive(true);
		
		GameObjectUtil.playForwardUITweener(shakeExpSpr.gameObject.GetComponent<TweenAlpha>());
	}
	
	public void hideShake()
	{
		expSpr.gameObject.SetActive(true);
		expChangeLabel.gameObject.SetActive(true);
		
		shakeExpSpr.gameObject.SetActive(false);
		shakeExpChangeLabel.gameObject.SetActive(false);
		
		GameObjectUtil.playForwardUITweener(expSpr.gameObject.GetComponent<TweenAlpha>());
	}
	
	public void hideLightAndYellowBar()
	{
		Exp_Full_Light.SetActive(false);
		Exp_Full_Yellow.SetActive(false);
	}

}
