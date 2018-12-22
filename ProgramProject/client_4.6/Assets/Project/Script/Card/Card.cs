using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Card : MonoBehaviour
{
	public enum PSEffectType : int
	{
		E_Normal = 0,
		E_ForbitAttack = 1,
		E_BacklashDamage = 2,
		E_ReduceEnergy = 3,
		//E_ReduceDamage = 4,
	}
	
	public Animator animator;
	
	//basic attribution
	public int curHp;
	public int maxHp;
	public CardData cardData;
	public SkillData skill;
	public int skillLevel;
	private SkillData beHurtSkill;
	private SkillData resumeSkill;
	private SkillData beTankSkill;
	public List<string> passiveSkills;
	/**equipId-level**/
	public List<string> equipInfos;
	public int level;
	//==第一天赋==//
	public int talent1;
	//==第二天赋==//
	public int talent2;
	//==第三天赋==//
	public int talent3;
	//==种族加成属性==//
	public string[] raceAtts;
	//tank attribution
	private int _active;

	// hurt第一个元素为伤害值或回血值;第二个元素为:1暴击,2闪避,3正常攻击,4回血,5反击,6吸血;第三个元素为:1死亡 第4个元素为: 1 克制,2 反弹伤害 //
	private int[] hurt;
	
	// psEffectType 受到被动技能效果  : 1变羊，2反弹，3打怒气 //
	PSEffectType psEffectType = PSEffectType.E_Normal;
	int psEffectNum;
		
	// FA = forbit attack
	int FAStartRound = -1; 
	
	//true表示抵挡型的飘血,false正常飘血//
	private bool defendBlood;
	//初始位置//
	private Vector3 initPosition;
	//技能目标//
	private List<Card> targets;
	//Tank//
	private List<Card> tanks;
	private Card _myCard;
	//被T的目标//
	public List<Card> beTanks;
	public Vector3 tPos;
	
	
	private bool running;
	//用秒来表示//
	private float frame;
	/// <summary>
	/// 待销毁效果集
	/// </summary>
	private List<Effect> effects;
	private List<string> effectMark;
	
	public GameObject head;
	public GameObject body;
	public GameObject foot;
	public GameObject front;
	public GameObject attack_front;
	
	//mark -- cuixl//
	public float createAttackEffectTime = 2.6f;
	public GameObject bloodBar;
	//名字//
	public GameObject nameObj;
	BloodManager bloodMana;
	public Camera mainCamera;
	public Camera NGUICamera;
	
	//阴影//
	GameObject shadow;
	
	//mark -- cuixl 合体技特效信息//
	public string UniteSkillName;
	public float UniteKeepTime;
	public List<Card> UniteTargets;
	
	//站位序列0--5//
	public int sequence;
	//站位方,0左边,1右边//
	public int team;
	
	public bool ignoreT;
	
	//mark--xuyan//
	private float gameObjectDisappearTime;
	bool needWaitForDisappear = false;
	List<string> shaderNameList = new List<string>();
	
	//人物移动的目标位置//
	Vector3 cardMovePos;

	public Transform _myTransform;
	
	//mark --cuixl 添加经验值//
	public float maxExp = 500;
	public float lastExp = 300;
	public float curExp = 350;
	
	
	/**mission怪物,固定死的atk,def,maxhp**/
	public bool missionMonster;
	public int atk;
	public int def;
	public int cri;
	public int avi;
	public bool boss;
	public string runeId;
	public int breakNum;
	
	public GameObject fbxObj;
	public bool isHide = false;
	
	public bool isCritAttack = false;
	
	public bool isSetInfo = false;//是否初始化赋值属性完毕//
	
	public string sheepCardName = "";
	
	public Card backlashDamageTarget;
	public int backlashDamageNum;

	void Awake()
	{
		_myTransform = transform;
		_myCard = this;
		head = _myTransform.FindChild("head").gameObject;
		body = _myTransform.FindChild("body").gameObject;
		foot = _myTransform.FindChild("foot").gameObject;
		front = _myTransform.FindChild("front").gameObject;
		attack_front = _myTransform.FindChild("attack_front").gameObject;
		
		for(int i = 0; i < _myTransform.childCount; ++i)
		{
			GameObject childObj = _myTransform.GetChild(i).gameObject;
			if(childObj == head || childObj == body || childObj == foot || childObj == front || childObj == attack_front)
			{
				continue;
			}
			fbxObj = childObj;
		}
	}
	
	public bool canForbitAttack()
	{
		//if(team == 0)
		//	return false;
		//return true;
		if(isDeath())
			return false;
		if(isForbitAttack())
			return false;
		if(breakNum < 6)
			return false;
		if(!cardData.isCanDoForbitAttack())
			return false;
		int chance = cardData.PPSprobability2;
		int num = UnityEngine.Random.Range(0,100);
		if(num < chance)
			return true;
		return false;
	}
	
	public int getForbitAttackCount()
	{
		return cardData.getForbitAttackCountOrReduceEnergy();
	}
	
	public bool isHaveRestrain(STATE.SKILL_TYPE st)
	{
		if(breakNum < 6)
			return false;
		if(isForbitAttack())
			return false;
		return cardData.isHaveRestrain(st);
	}
	
	public float getRestrainNum(float damage)
	{
		return cardData.getRestrainNum(damage);
	}
	
	public bool isCanReduceEnergy()
	{
		//return true;
		if(isDeath())
			return false;
		if(isForbitAttack())
			return false;
		if(breakNum < 6)
			return false;
		if(!cardData.isCanReduceEnergy())
			return false;
		int chance = cardData.PPSprobability2;
		int num = UnityEngine.Random.Range(0,100);
		if(num < chance)
			return true;
		return false;
	}
	
	public bool isCanBacklashDamage()
	{
		//return true;
		if(isDeath())
			return false;
		if(isForbitAttack())
			return false;
		if(breakNum < 6)
			return false;
		if(!cardData.isCanBacklashDemage())
			return false;
		int chance = cardData.PPSprobability2;
		int num = UnityEngine.Random.Range(0,100);
		if(num < chance)
			return true;
		return false;
	}
	
	public bool isDeath()
	{
		if(getCurHp() <= 0)
			return true;
		return false;
	}
	
	public void setDefendBlood(bool defendBlood)
	{
		this.defendBlood=defendBlood;
	}
	
	public CardData getCardData()
	{
		return cardData;
	}
	
	public List<string> getEquipInfos()
	{
		return equipInfos;
	}
	
	public int getLevel()
	{
		return level;
	}
	
	public List<string> getPassiveSkill()
	{
		return passiveSkills;
	}
	
	public SkillData getSkill()
	{
		return skill;
	}
	
	public void setCurHp(int curHp)
	{
		if(curHp <= 0)
			curHp = 0;
		this.curHp=curHp;
		if(this.curHp >= getMaxHp())
		{
			this.curHp = getMaxHp();
		}
	}
	
	public int getCurHp()
	{
		return curHp;
	}
	
	public int getMaxHp()
	{
		return maxHp;
	}
	
	public float getCurHpPercent()
	{
		return ((float)curHp)/maxHp;
	}
	
	public float getAtk(Card[] teamers,Card target,int roundNum)
	{
		if(missionMonster)
		{
			return atk;
		}
		return Statics.getAllAttack(this,teamers,target,roundNum);
	}
	
	public float getDef(Card[] teamers,Card attacker,int roundNum)
	{
		if(missionMonster)
		{
			return def;
		}
		return Statics.getAllDef(this,teamers,attacker,roundNum);
	}
	
	public int getCriRate()
	{
		return cri;
	}
	
	public int getAviRate()
	{
		return avi;
	}
	
	public bool isBoss()
	{
		return boss;
	}
	
	public void setHurt(int[] hurt)
	{
		this.hurt=hurt;
	}
	
	public int[] getHurt()
	{
		return hurt;
	}
	
	public bool getHurtIsCrit()
	{
		if(hurt[1] == 1)
			return true;
		return false;
	}
	
	public bool isAvi()
	{
		if(hurt[1] == 2)
			return true;
		return false;
	}
	
	public void setPSEffectType(PSEffectType eType)
	{
		psEffectType = eType;
	}
	
	public PSEffectType getPSEffectType()
	{
		return psEffectType;
	}
	
	public void setPSEffectNum(int num)
	{
		psEffectNum = num;
	}
	
	public int getPSEffectNum()
	{
		return psEffectNum;
	}
	
	public int getFAStartRound()
	{
		return FAStartRound;
	}
	
	public void setFAStartRound(int cRound)
	{
		FAStartRound = cRound;
	}
	
	public bool isForbitAttack()
	{
		if(psEffectType == PSEffectType.E_ForbitAttack)
		{
			return true;
		}
		return false;
	}
	
	public void doForbitAttack()
	{
		pve.players[team].doForbitAttackEffect(sequence);
	}
	
	public bool canResumeAttack(int cRound)
	{
		if((cRound - FAStartRound) >= psEffectNum)
		{
			doResuemAttack();
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public void doResuemAttack()
	{
		FAStartRound 	= -1;
		psEffectType = PSEffectType.E_Normal;
		psEffectNum = -1;
		BattleEffectHelperControl.mInstance.createChangeSheepEffect(body.transform.position);
		pve.players[team].doResumeForbitAttack(sequence);
		
	}
	
	public int getActive()
	{
		return _active;
	}
	
	public void setActive(int n)
	{
		this._active=n;
	}
	
	public void setBehurtSkill(SkillData beHurtSkill)
	{
		this.beHurtSkill=beHurtSkill;
	}
	
	public void setResumeSkill(SkillData resumeSkill)
	{
		this.resumeSkill=resumeSkill;
	}
	
	public void setBeTankSkill(SkillData beTankSkill)
	{
		this.beTankSkill=beTankSkill;
	}
	
	/// <summary>
	/// Sets the data.
	/// </summary>
	///
	public void setData(int team,int sequence,int cardId,int breakNum,int skillId,int skillLevel, 
		List<string> psList, List<string> equipInfos,int level,bool missionMonster,int atk,int def,int maxHp,
		int bossMark,string runeId,int criRate,int aviRate,int talent1,int talent2,int talent3,string[] raceAtts)
	{
		this.team=team;
		this.sequence=sequence;
		this.cardData=CardData.getData(cardId);
		this.breakNum=breakNum;
		this.skill=SkillData.getData(skillId);
		this.skillLevel=skillLevel;
		this.passiveSkills=psList;
		this.equipInfos=equipInfos;
		this.level=level;
		this.missionMonster=missionMonster;
		this.runeId=runeId;
		this.talent1=talent1;
		this.talent2=talent2;
		this.talent3=talent3;
		this.raceAtts=raceAtts;
		//==怪的攻防血在这里设置==//
		if(missionMonster)
		{
			this.atk=atk;
			this.def=def;
			this.maxHp=maxHp;
			this.cri=criRate;
			this.avi=aviRate;
			this.boss=bossMark==1;
		}
	}
	
	//==设置属性==//
	public void setAttribute(Card[] teamers)
	{
		if(!missionMonster)
		{
			this.maxHp=Statics.getMaxHp(this,teamers);
			this.cri=Statics.getCriRate(this,teamers);
			this.avi=Statics.getAviRate(this,teamers);
		}
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE&&this.team == 0)
		{
			if(PlayerInfo.getInstance().curMazeBattleCardHp.Count!=0&&PlayerInfo.getInstance().curMazeBattleCardHp.Count!=0)
			{
				curHp = PlayerInfo.getInstance().curMazeBattleCardHp[cardData.id];
				this.maxHp = PlayerInfo.getInstance().maxMazeBattleCardHp[cardData.id];
			}
		}
		else
		{
			curHp=this.maxHp;	
		}
		isSetInfo = true;
	}
	
	/// <summary>
	/// 设置目标
	/// </summary>
	public void setTargets(List<Card> targets,List<Card> tanks)
	{
		this.targets=targets;
		this.tanks=tanks;
	}
	
	public void setBeTanks(List<Card> beTanks)
	{
		this.beTanks=beTanks;
	}

	public void resetTargets()
	{
		if(targets!=null)
		{
			foreach(Card c in targets)
			{
				if(c!=this)
				{
					c.resetTargets();
				}
			}
			targets=null;
		}	
		if(tanks!=null)
		{
			foreach(Card c in tanks)
			{
				if(c!=this)
				{
					c.resetTargets();
				}
			}
			tanks=null;
		}
		if(beTanks!=null)
		{
			foreach(Card c in beTanks)
			{
				if(c!=this)
				{
					c.resetTargets();
				}
			}
			beTanks=null;
		}
		resetOtherSkill();
	}
	
	public void resetOtherSkill()
	{
		this.beHurtSkill=null;
		this.resumeSkill=null;
		this.beTankSkill=null;
		this.hurt=null;
		effectMark.Clear();
	}
	
	/**阶段持续时间**/
	private float stageTime;
	/**阶段开始时间**/
	private float stageStartTime;
	/**当前阶段**/
	private int curStage;
	private PVESceneControl pve;
	
	public void setDirect(int direct)
	{
		/**重置所有转换条件**/
		resetTransitions();
		effectMark.Clear();
		curStage=direct;
		stageStartTime=pve.curTime;
		switch(curStage)
		{
		case -1://待机//
			if(hurt!= null && hurt[2] == 1)
			{
				//播放卡牌死亡音效//
				MusicManager.playCardSoundEffect(cardData.id, 1);
				animator.SetBool("idle2die",true);
				stageTime=0;
				//清空特效//
				CleanEffect();
			}
			isCritAttack = false;
			break;
		case 0://蓄力//
			
			animator.SetBool("idle2storePower",true);
			switch(skill.type)
			{
			case 1:
				stageTime=skill.atkChargeENDTime;
				break;
			case 2:
				stageTime=skill.defActionENDTime;
				break;
			case 3:
				stageTime=skill.healActionENDTime;
				break;
			}
			playStoreEffect();
			//如果当前的卡牌不需要移动攻击的话,则让t移动,如果卡牌需要移动的话,则等攻击者移动时,t才开始移动//
			if(skill.type==1 && skill.position == 0)
			{
				
				TMoveToPos();
			}
			break;
		case 1://受伤//
			
			animator.SetBool("idle2beHurt",true);
			/*if(beHurtSkill!=null)
			{
				stageTime=beHurtSkill.hurtENDTime;
			}
			else
			{
				stageTime=0.5f;
			}*/
			stageTime = 0.1f;
			playBehurtEffect();
			break;
		case 2://回复//
			animator.SetBool("idle2resume",true);
			stageTime=resumeSkill.healedENDTime;
			playResumeEffect();
			break;
		case 3://T独自受伤//
			animator.SetBool("idle2tank",true);
			stageTime=0;
			playBehurtForTEffect();
			break;
		case 4://T移动//
			animator.SetBool("idle2goForT",true);
			stageTime=moveTime;
			break;
		case 5://T为别人抵挡,自己受伤//
			//只要是t就不播受伤动画，播加成动画  mark -- cuixl//
			animator.SetBool("idle2beHurtForT",true);
			stageTime=skill.defENDTime;
			playBehurtForTEffect();
			break;
		case 6://T移回原位//
			animator.SetBool("beHurtForT2backForT",true);
			stageTime=moveTime;
			break;
		case 7://被T//
			animator.SetBool("idle2beTank", true);
			stageTime=beHurtSkill.hurtENDTime;
			playBeTankEffect();
			break;
		case 8://T站立//
			stageTime=0;
			break;
		case 9://合体技//
			animator.SetBool("idle2unitSkill",true);
			stageTime=0.1f;
			playUnitSkillEffect();
			break;
		case 10://合体技//
			animator.SetBool("unitSkill2idle",true);
			stageTime=0.1f;
			break;
		case 11://原地攻击//
			animator.SetBool("storePower2useSkill",true);
			switch(skill.type)
			{
			case 1:
				stageTime=skill.spawnENDTime;
				break;
			case 2:
				stageTime=skill.defENDTime;
				break;
			case 3:
				stageTime=skill.healActionENDTime;
				break;
			}
			playUseSkillEffect();
			break;
		case 15://移动攻击//
			animator.SetBool("storePower2go",true);
			stageTime=skill.spawnENDTime;
			//此时攻击者开始移动了,则让t也跟着移动//
			if(skill.type==1)
			{
				TMoveToPos();
			}
			break;
		case 16://复活//
			animator.SetBool("die2idle",true);
			playRecoverEffect();
			stageTime=getAnimatorStateInfoLength("Base Layer.idle");
			gameObjectAppear();
			break;
		}
		running=true;
		frame=0.1f;
	}
	
	//void Start()
	private bool haveStarted;
	void start2()
	{
		//mark -- xuyan//
		SkinnedMeshRenderer smRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		foreach(UnityEngine.Material material in smRenderer.materials)
		{
			shaderNameList.Add(material.shader.name);
		}
		
		animator=gameObject.GetComponent<Animator>();
		
		
		initPosition=gameObject.transform.position;
		effects=new List<Effect>();
		effectMark=new List<string>();
		
		if(mainCamera == null){
			mainCamera = Camera.main;
		}
		
		if(NGUICamera == null){
			NGUICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		}
		
		//创建血条//
		if(bloodBar == null)
		{
			bloodBar = Instantiate(GameObjectUtil.LoadResourcesPrefabs("BloodBar02", 2)) as GameObject;
		}
		
		//创建影子//
		if(shadow == null)
		{
			shadow = Instantiate(GameObjectUtil.LoadResourcesPrefabs("Shadow", 2)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(shadow,foot);
		}
		
		pve=mainCamera.GetComponent<PVESceneControl>();
		curStage=-1;
		
		CreateName();
	}
	
	void setBlood()
	{
//		bloodBar.transform.parent = PVESceneControl.mInstance.BloodParent.transform;
		GameObjectUtil.gameObjectAttachToParent(bloodBar,  PVESceneControl.mInstance.BloodParent);
		bloodBar.transform.localScale = Vector3.one;
		//bloodBar.GetComponent<BloodManager>().init();
		
		//计算需条的坐标//
		//bloodBar.transform.position = new Vector3(head.transform.position.x, head.transform.position.y, 0);
		Vector3 worldPos = new Vector3(head.transform.position.x + 0.5f, head.transform.position.y, head.transform.position.z);
		Vector2 screenPos = mainCamera.WorldToScreenPoint(worldPos);
		//screenPos = new Vector2(screenPos.x, Screen.height - screenPos.y);
		Vector3 curPos = NGUICamera.ScreenToWorldPoint(screenPos);
		bloodBar.transform.position = curPos;
		//bloodBar.transform.position = new Vector3(pos.x, pos.y, 0);
		bloodMana = bloodBar.GetComponent<BloodManager>();
		bloodMana.myCard = this;
		bloodMana.posObject = head;
		bloodMana.NGUICamera = NGUICamera;
		bloodMana.init();
	}
	
	void Update()
	{
		//==构建一些信息==//
		if(!haveStarted)
		{
			haveStarted=true;
			start2();
		}
		
		if(isSetInfo)
		{
			isSetInfo = false;
			setBlood();
		}
		
		/**当前阶段结束**/
		if(pve!=null && pve.curTime-stageStartTime>=stageTime)
		{
			switch(curStage)
			{
			case 0://蓄力//
				if(skill.position == 0)
				{
					setDirect(11);
				}
				else
				{
					//setDirect(12);
					setDirect(15);
				}
				break;
			case 11://原地攻击//
			case 15://移动攻击//
				setDirect(-1);
				if(targets!=null)
				{
					//不在攻击目标里的T//
					if(tanks!=null)
					{
						foreach(Card tank in tanks)
						{
							if(!targets.Contains(tank))
							{
								tank.setDirect(5);
							}
						}
					}
					//攻击目标//
					foreach(Card c in targets)
					{
						switch(skill.type)
						{
						case 1:
							//攻击目标里的T//
							if(tanks.Contains(c))
							{
								c.setDirect(5);
								continue;
							}
							//被T的目标和不被T的目标//
							bool isBeTank=false; 
							foreach(Card tank in tanks)
							{
								if(tank.beTanks!=null && tank.beTanks.Contains(c))
								{
									isBeTank=true;
									break;
								}
							}
							if(isBeTank)
							{
								c.setDirect(7);
							}
							else
							{
								c.setDirect(1);
								
							}
							break;
						case 2:
							c.setBeTankSkill(skill);
							c.playBeTankEffect();
							break;
						case 3:
							c.setDirect(2);
							break;
						}
					}
				}
				AddEnergy();
				//震动屏幕，增加打击感//
				if(skill.type == 1)
				{
					ViewControl.mInstacne.ChangeCameraPos();
				}
				break;
			case 4://T移动//
				setDirect(8);
				break;
			case 5://T为别人抵挡,自己受伤//
				setDirect(6);
				break;
			case 8://T站立//
				//此处等待攻击者来改变T的状态//
				break;
			case 1://受伤//
			case 2://回复//
			case 3://T独自受伤//
			case 6://T移回原位//
			case 7://被T//
			case 9://合体技//
			case 10://合体技//
			case 16://复活//
				setDirect(-1);
				break;
			}
		}
		
		if(effectMark==null)
		{
			return;
		}
		
		if(isPlayTheName("Base Layer.go") && !effectMark.Contains("go"))
		{
			effectMark.Add("go");
			playGoEffect();
		}
		if(isPlayTheName("Base Layer.back") && !effectMark.Contains("back"))
		{
			effectMark.Add("back");
			playBackEffect();
		}
		if(isPlayTheName("Base Layer.goForT") && !effectMark.Contains("goForT"))
		{
			effectMark.Add("goForT");
			playGoForTEffect();
		}
		if(isPlayTheName("Base Layer.backForT") && !effectMark.Contains("backForT"))
		{
			effectMark.Add("backForT");
			playBackEffect();
		}
		if(isPlayTheName("Base Layer.phyAttack") && !effectMark.Contains("phyAttack") && curStage==15)
		{
			effectMark.Add("phyAttack");
			playUseSkillEffect();
		}
		else if(!isPlayTheName("Base Layer.phyAttack"))
		{
		}
		
		if(isPlayTheName("Base Layer.die") && (effectMark!=null && !effectMark.Contains("die")) && curHp==0)
		{
			effectMark.Add("die");
			playDieEffect();
		}
	}
	
	//mark -- xuyan//
	public void gameObjectDisappear(float time)
	{
		GameObjectUtil.hideCardEffect(this.gameObject);
		SkinnedMeshRenderer smRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		if(smRenderer != null && smRenderer.materials!=null)
		{
			
			for(int i =0; i < smRenderer.materials.Length;++i)
			{
				smRenderer.materials[i].shader = Shader.Find("Transparent/Diffuse");	
			}
			iTween.FadeTo(gameObject,0.0f,time);
		}
		
	}
	
	public void gameObjectAppear()
	{
		GameObjectUtil.showCardEffect(this.gameObject);
		SkinnedMeshRenderer smRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		for(int i =0; i < smRenderer.materials.Length;++i)
		{
			smRenderer.materials[i].shader = Shader.Find(shaderNameList[i]);
		}	
	}
	
	void LateUpdate()
	{
		if(effects != null)
		{
			checkEffect();
		}
		if(frame>0)
		{
			frame-= Time.deltaTime;
		}
		else
		{
			checkRunning();
		}
		//mark -- xuyan//
		if(needWaitForDisappear)
		{
			if(gameObjectDisappearTime >= 0)
			{
				gameObjectDisappearTime -= Time.deltaTime;
			}
			else
			{
				needWaitForDisappear = false;
				gameObjectDisappear(1.0f);
			}
		}

	}
	
	/**检查销毁特效**/
	private void checkEffect()
	{
		for(int i=effects.Count-1;i>=0;i--)
		{
			Effect effect=(Effect)effects[i];
			if(effect.isDelayEffect() && effect.getEffectObj()==null)
			{
				if(effect.isDelayOver(pve.curTime))
				{
					effect.create();
				}
			}
			if(!effect.isValid(pve.curTime))
			{
				effects.RemoveAt(i);
				Destroy(effect.getEffectObj());
				effect.gc();
			}
		}
	}
	
	//删除所有特效//
	public void CleanEffect(){
		for(int i=effects.Count-1;i>=0;i--)
		{
			Effect effect=(Effect)effects[i];
			effects.RemoveAt(i);
			Destroy(effect.getEffectObj());
			effect.gc();
		}
	}
	
	//隐藏特效//
	public void HideEffect()
	{
		for(int i=effects.Count-1;i>=0;i--)
		{
			Effect effect=(Effect)effects[i];
			effect.effectObj.SetActive(false);
		}
	}
	
	//显示特效//
	public void ShowEffect(){
		for(int i=effects.Count-1;i>=0;i--)
		{
			Effect effect=(Effect)effects[i];
			effect.effectObj.SetActive(true);
		}
	}
	
	private void playDieEffect()
	{
		psEffectType = PSEffectType.E_Normal;
		psEffectNum = 0;
		FAStartRound = -1;
		
		if(bloodBar != null)
		{
			bloodBar.SetActive(false);
		}
		if(nameObj != null)
		{
			nameObj.SetActive(false);
		}
		shadow.SetActive(false);
		//mark -- xuyan//
		gameObjectDisappearTime = getAnimatorStateInfoLength("Base Layer.die");
		needWaitForDisappear = true;
		
		/**更新合体技按钮**/
		updateUnitBtn();
	}
	
	private void playRecoverEffect()
	{
		bloodBar.SetActive(true);
		shadow.SetActive(true);
		nameObj.SetActive(true);
		//绘制掉血数字//
		createHitNumObject(hurt[0], hurt[1]);
		/**更新合体技按钮**/
		updateUnitBtn();
	}
	
	private void playBehurtForTEffect()
	{
		if(beHurtSkill==null)
		{
			return;
		}
		string effectName=beHurtSkill.hurtSE;
		int pos=beHurtSkill.hurtSEPositionType;
		float keepTime=beHurtSkill.hurtSETime;
		int moveType=1;
		List<Card> temp=new List<Card>();
		temp.Add(this);
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,null,temp,0,false,-1,this));
		beHurtSkill=null;
		moveType=1;
	
		effectName=skill.defSE;
		pos=skill.defSEPositionType;
		keepTime=skill.defSETime;
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,this,targets,0,false,-1,this));
		//绘制掉血数字//
		createHitNumObject(hurt[0], hurt[1],hurt[3]);
		
		if(getPSEffectType() == PSEffectType.E_BacklashDamage)
		{
			effects.AddRange(createEffect(1,"fanzhen",0.8f,1,this,null,0,false,-1,this));
			int blNum = hurt[0];
			blNum = (int)((1- cardData.getPSEffectNum())*blNum);
			backlashDamageTarget.setCurHp(backlashDamageTarget.getCurHp() - blNum);
			backlashDamageTarget.backlashDamageNum += blNum;
			if(backlashDamageTarget.isDeath())
			{
				int[] tempHurt =new int[4];
				tempHurt[2]=1;
				backlashDamageTarget.setHurt(tempHurt);
			}
			backlashDamageTarget.doBacklash();
			setPSEffectType(PSEffectType.E_Normal);
			backlashDamageTarget = null;
		}
	}
	
	//是防御特效时，先播蓄力特效defSE， 然后播放施法特效defActionSE，并且这个特效一直持续到下一次播放，目标特效是defTargetSE//
	private void playStoreEffect()
	{
		//播放蓄力音效//
		MusicManager.playSkillSoundEffect(skill.index, 0);
		
		string effectName="";
		int pos=0;
		float keepTime=0;
		int moveType=1;
		switch(skill.type)
		{
		case 1://攻击//
			effectName=skill.atkChargeSE;
			pos=skill.atkChargeSEPositionType;
			keepTime=skill.atkChargeSETime;
			break;
		case 2://防御//
			//防御特效在每次蓄力的时候清空//
			CleanEffect();
			effectName = skill.defSE;
			pos = skill.defSEPositionType;
			keepTime = skill.defSETime;
			break;
		case 3://回复//
			effectName=skill.healChargeSE;
			pos=skill.healChargeSEPositionType;
			keepTime=skill.healChargeSETime;
			break;
		}
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,this,targets,0,false,-1,this));
	}
	
	public void TMoveToPos()
	{
		
		if(tanks!= null)
		{
			foreach(Card tank in tanks)
			{
				if(skill.atkTarget!=3)
				{
					if(tank.beTanks.Count>2)
					{
						if(tank.team == 0)
						{
							tank.tPos=SceneInfo.getInstance().getCenter0().transform.position;
						}
						else if(tank.team == 1)
						{
							tank.tPos=SceneInfo.getInstance().getCenter1().transform.position;
						}
						tank.setDirect(4);
					}
					else if(tank.beTanks.Count == 1)
					{
						if((Card)tank.beTanks[0]!=tank)
						{
							tank.tPos=((Card)tank.beTanks[0]).front.transform.position;
							tank.setDirect(4);
						}
					}
					else if(tank.beTanks.Count == 2)
					{
						if(!tank.beTanks.Contains(tank))
						{
							if(tank.team == 0)
							{
								tank.tPos=SceneInfo.getInstance().getCenter0().transform.position;
							}
							else if(tank.team == 1)
							{
								tank.tPos=SceneInfo.getInstance().getCenter1().transform.position;
							}
						}
						else
						{
							Card beTank=null;
							foreach(Card beT in tank.beTanks)
							{
								if(beT!=tank)
								{
									beTank=beT;
									break;
								}
							}
							tank.tPos=beTank.front.transform.position;
						}
						tank.setDirect(4);
					}
				}
			}
		}
	}
	
	
	float moveTime = 0.333f;
	
	private void playGoEffect()
	{
		switch(skill.position)
		{
		case 0://不移动//
			break;
		case 1://移动到单体面前//
		{
			if(!isCritAttack)
			{
				//判断，如果当前有t，并且是单体攻击，倍攻击卡牌是t的情况下攻击者到达的位置是attack_front//
				if(tanks!= null &&targets!=null && targets.Count == 1 && !tanks.Contains((Card)targets[0]))
				{	
					cardMovePos = ((Card)targets[0]).attack_front.transform.position;
				}
				else
				{
					cardMovePos = ((Card)targets[0]).front.transform.position;
				}
				if(targets!=null && targets.Count>0)
				{
					iTween.MoveTo(_myTransform.gameObject,iTween.Hash("position",cardMovePos,"time",moveTime,"easyType",iTween.EaseType.linear));
				}
			}
			else
			{
				iTween.MoveTo(_myTransform.gameObject,iTween.Hash("position",ViewControl.mInstacne.actionCardAtkOneEndPos.transform.position,"time",moveTime,"easyType",iTween.EaseType.linear));
			}
			
		}break;
		case 2://移动到前排3人面前//
		{
			Vector3 pos = Vector3.zero;
			if(_myCard.team == 0)
			{
				pos = SceneInfo.getInstance().getCenter0().transform.position 	;
			}
			else if(_myCard.team == 1)
			{
				pos = SceneInfo.getInstance().getCenter1().transform.position 	;
			}
			cardMovePos = pos;
			iTween.MoveTo(_myTransform.gameObject,iTween.Hash("position",pos,"time",moveTime,"easyType",iTween.EaseType.linear));
		}break;
		}
	}
	
	private void playBackEffect()
	{
		if(Vector3.Distance(gameObject.transform.position,initPosition)>0F)
		{
			iTween.MoveTo(_myTransform.gameObject,iTween.Hash("position",initPosition,"time",moveTime,"easyType",iTween.EaseType.linear));
		}
	}
	
	//是防御特效时，先播蓄力特效defSE， 然后播放施法特效defActionSE，并且这个特效一直持续到下一次播放，目标特效是defTargetSE//
	private void playUseSkillEffect()
	{
		
		//播放施法音效//
		MusicManager.playSkillSoundEffect(skill.index, 1);
		
		string effectName="";
		int pos=0;
		float keepTime=0;
		float delayTime=0;
		int moveType=1;
		bool atkEffect=false;
		int atkTarget = -1;
		int atkPosIndex = -1;
		Card cd = null;
		switch(skill.type)
		{
		case 1://攻击//
			effectName=skill.spawnID;
			pos=skill.spawnPositionType;
			keepTime=skill.spawnTime;
			moveType=skill.spawn;
			delayTime=skill.spawnDelayTime;
			atkTarget = skill.atkTarget;
			Card cc = (Card)targets[0];
			if(atkTarget == 1){		//竖排攻击//
				if(cc.sequence>2){
					atkPosIndex = cc.team * 6 + cc.sequence;
				}
				else {
					atkPosIndex = cc.team * 6 + 3 + cc.sequence;
				}
			}
			else if(atkTarget == 2){		//横排攻击//
				if(cc.sequence>2){
					atkPosIndex = cc.team * 6 + 4;
				}
				else {
					atkPosIndex = cc.team * 6 + 1;
				}
			}
			atkEffect=true;
			break;
		case 2://防御//
			effectName = skill.defActionSE;
			pos = skill.defActionSEPositionType;
			keepTime = skill.defActionSETime;
			cd = this;
			break;
		case 3://回复//
			effectName=skill.healActionSE;
			pos=skill.healActionSEPositionType;
			keepTime=skill.healActionSETime;
			break;
		}
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,this,targets,delayTime, atkEffect, atkPosIndex, cd));	
	}
	
	private void playBehurtEffect()
	{
		//播放受伤音效//
		if(beHurtSkill!=null)
		{
			MusicManager.playSkillSoundEffect(beHurtSkill.index, 2);
		}
		
		//绘制掉血数字//
		if(hurt != null)
		{
			createHitNumObject(hurt[0], hurt[1],hurt[3]);
			if(getPSEffectType() == PSEffectType.E_BacklashDamage)
			{
				effects.AddRange(createEffect(1,"fanzhen",0.8f,1,this,null,0,false,-1,this));
				int blNum = hurt[0];
				blNum = (int)((1- cardData.getPSEffectNum())*blNum);
				backlashDamageTarget.setCurHp(backlashDamageTarget.getCurHp() - blNum);
				backlashDamageTarget.backlashDamageNum += blNum;
				if(backlashDamageTarget.isDeath())
				{
					int[] tempHurt =new int[4];
					tempHurt[2]=1;
					backlashDamageTarget.setHurt(tempHurt);
				}
				backlashDamageTarget.doBacklash();
				setPSEffectType(PSEffectType.E_Normal);
				backlashDamageTarget = null;
			}
		}
		if(beHurtSkill==null)
		{
			return;   
		}
		//播放受伤音效//
		MusicManager.playSkillSoundEffect(beHurtSkill.index, 2);
		
		string effectName=beHurtSkill.hurtSE;
		int pos=beHurtSkill.hurtSEPositionType;
		float keepTime=beHurtSkill.hurtSETime;
		int moveType=1;
		List<Card> temp=new List<Card>();
		temp.Add(this);
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,null,temp,0,false,-1,this));
		beHurtSkill=null;
		if(beTankSkill==null)
		{
			return;
		}
		//被防御的人身上播放defTargetSE//
		effectName=beTankSkill.defTargetSE;
		pos=beTankSkill.defTargetSEPositionType;
		keepTime=beTankSkill.defTargetSETime;
		moveType=1;
		List<Card> temp2=new List<Card>();
		temp2.Add(this);
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,null,temp2,0,false,-1,this));
		beTankSkill=null;
	}
	
	private void playResumeEffect()
	{
		if(resumeSkill==null)
		{
			return;
		}
		string effectName=resumeSkill.healedSE;
		int pos=resumeSkill.healedSEPositionType;
		float keepTime=resumeSkill.healedSETime;
		int moveType=1;
		List<Card> temp=new List<Card>();
		temp.Add(this);
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,null,temp,0,false,-1,this));
		resumeSkill=null;
		//绘制加血数字//
		createHitNumObject(hurt[0], hurt[1]);
	}
	
	private void playTankEffect()
	{
		string effectName=skill.defActionSE;
		int pos=skill.defActionSEPositionType;
		float keepTime=skill.defActionSETime;
		int moveType=1;
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,this,null,0,false,-1,this));
	}
	
	private void playGoForTEffect()
	{
		if(isCritAttack)
		{
			iTween.MoveTo(gameObject,iTween.Hash("position",ViewControl.mInstacne.tankEndPos.transform.position,"time",moveTime,"easyType",iTween.EaseType.linear));
		}
		else
		{
			iTween.MoveTo(gameObject,iTween.Hash("position",tPos,"time",moveTime,"easyType",iTween.EaseType.linear));
		}

	}
	
	//mark -- cuixl//
	private void playBeTankEffect()
	{
		if(beTankSkill==null)
		{
			return;
		}
		//被防御的人身上播放defTargetSE//
		string effectName=beTankSkill.defTargetSE;
		int pos=beTankSkill.defTargetSEPositionType;
		float keepTime=beTankSkill.defTargetSETime;
		int moveType=1;
		List<Card> temp2=new List<Card>();
		temp2.Add(this);
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,null,temp2,0,false,-1,this));
		beTankSkill=null;
		//绘制掉血数字//
		if(hurt!=null)
		{
			createHitNumObject(hurt[0], hurt[1],hurt[3]);
			if(getPSEffectType() == PSEffectType.E_BacklashDamage)
			{
				effects.AddRange(createEffect(1,"fanzhen",0.8f,1,this,null,0,false,-1,this));
				int blNum = hurt[0];
				blNum = (int)((1- cardData.getPSEffectNum())*blNum);
				backlashDamageTarget.setCurHp(backlashDamageTarget.getCurHp() - blNum);
				backlashDamageTarget.backlashDamageNum += blNum;
				if(backlashDamageTarget.isDeath())
				{
					int[] tempHurt =new int[4];
					tempHurt[2]=1;
					backlashDamageTarget.setHurt(tempHurt);
				}
				backlashDamageTarget.doBacklash();
				setPSEffectType(PSEffectType.E_Normal);
				backlashDamageTarget = null;
			}
		}
	}
	
	public void playUnitSkillEffect()
	{
		string effectName = UniteSkillName;
		//脚下//
		int pos = 2;
		float keepTime = UniteKeepTime;
		int moveType = 1;
		effects.AddRange(createEffect(pos,effectName,keepTime,moveType,this,null,0,false,-1,this));
	}
	
	//pos 特效产生的位置, tarsPosIndex 只有在攻击特效且是攻击多人（横排或竖排）时才出现//
	//isAddToCard 该特效是否放在卡牌身上//
	private List<Effect> createEffect(int pos,string effectName,float keepTime,int moveType,Card src,List<Card> tars,
		float delayTime,bool atkEffect=false, int tarsPosIndex = -1, Card card = null)
	{
		GameObject parent = null;
		List<Effect> result=new List<Effect>();
		if(string.IsNullOrEmpty(effectName.Trim()))
		{
			return result;
		}
		switch(pos)
		{
		case 1://攻击者身上//
		{
			Vector3 srcPos=src.body.transform.position;
			Vector3 tarPos=Vector3.zero;
			Quaternion rotation=src.body.transform.rotation;
			Effect effect=null;
			if(card != null)
			{
				parent = card.body;
			}
			if(moveType == 0)
			{
				if(tars != null && tars.Count > 0)
				{
					tarPos = ((Card)tars[0]).body.transform.position;
					effect = new Effect(effectName,keepTime,delayTime,srcPos,tarPos,rotation,pve.curTime, 0, parent);
				}
			}
			else
			{
				effect = new Effect(effectName,keepTime,delayTime,srcPos,srcPos,rotation,pve.curTime, 0, parent);
			}
			result.Add(effect);
		}break;
		case 2://攻击者脚下//
		{
			Vector3 srcPos=src.foot.transform.position;
			Vector3 tarPos=Vector3.zero;
			Quaternion rotation=src.foot.transform.rotation;
			Effect effect=null;
			if(card != null)
			{
				parent = card.foot;
			}
			if(moveType == 0)
			{
				if(tars != null && tars.Count > 0)
				{
					tarPos = ((Card)tars[0]).body.transform.position;
					effect = new Effect(effectName,keepTime,delayTime,srcPos,tarPos,rotation,pve.curTime, 0, parent);
				}
			}
			else
			{
				effect = new Effect(effectName,keepTime,delayTime,srcPos,srcPos,rotation,pve.curTime, 0, parent);
			}
			result.Add(effect);
		}break;
		case 3://目标身上//
			if(card != null)
			{
				parent = card.body;
			}
			if(tars!=null && tars.Count>0)
			{
				if(atkEffect)
				{
					if(tarsPosIndex > -1)
					{
						GameObject posTemp=SceneInfo.getInstance().getPosition(tarsPosIndex);
						Effect effect = new Effect(effectName,keepTime,delayTime,posTemp.transform.position,
							posTemp.transform.position,posTemp.transform.rotation,pve.curTime, 0, parent);
						result.Add(effect);
					}
					else
					{
						Card tar=(Card)tars[0];
						Effect effect = new Effect(effectName,keepTime,delayTime,tar.body.transform.position,tar.body.transform.position,tar.body.transform.rotation,pve.curTime);
						result.Add(effect);
					}
				}
				else
				{
					foreach(Card tar in tars)
					{
						Effect effect = new Effect(effectName,keepTime,delayTime,tar.body.transform.position,
							tar.body.transform.position,tar.body.transform.rotation,pve.curTime, 0, parent);
						result.Add(effect);
					}
				}
			}
			break;
		case 4://目标脚下//
		{
			if(card != null)
			{
				parent = card.foot;
			}
			if(tars!=null && tars.Count>0)
			{
				if(atkEffect)
				{
					if(tarsPosIndex > -1)
					{
						GameObject posTemp=SceneInfo.getInstance().getPosition(tarsPosIndex);
						Effect effect = new Effect(effectName,keepTime,delayTime,posTemp.transform.position,
							posTemp.transform.position,posTemp.transform.rotation,pve.curTime, 0, parent);
						result.Add(effect);
					}
					else
					{
						Card tar=(Card)tars[0];
						Effect effect = new Effect(effectName,keepTime,delayTime,tar.foot.transform.position,
							tar.foot.transform.position,tar.foot.transform.rotation,pve.curTime, 0, parent);
						result.Add(effect);
					}
				}
				else
				{
					foreach(Card tar in tars)
					{
						Effect effect = new Effect(effectName,keepTime,delayTime,tar.foot.transform.position,
							tar.foot.transform.position,tar.foot.transform.rotation,pve.curTime, 0, parent);
						result.Add(effect);
					}
				}
			}
		}break;
		case 5://横排前后//
		case 6://横排后前//
		{
			if(tars != null && tars.Count > 0)
			{
				Card temp=(Card)tars[0];
				int posIndex=0;
				if(temp.sequence>=3)
				{
					posIndex=temp.team*6+4;
				}
				else
				{
					posIndex=temp.team*6+1;
				}
				GameObject posTemp=SceneInfo.getInstance().getPosition(posIndex);
				Effect effect = new Effect(effectName,keepTime,delayTime,posTemp.transform.position,posTemp.transform.position,
					src.transform.rotation,pve.curTime, 0, parent);
				result.Add(effect);
			}
		}break;
		case 7://场景中间//
		{
			GameObject centerObj = SceneInfo.getInstance().getCenter0();
			if(_myCard.team == 1)
			{
				centerObj = SceneInfo.getInstance().getCenter1();
			}
			Effect effect = new Effect(effectName,keepTime,delayTime,centerObj.transform.position,centerObj.transform.position,
				centerObj.transform.rotation,pve.curTime, 0, parent);
			result.Add(effect);
		}break;
		case 8://屏幕下方, 出现在被攻击方的最后排中间人的身上//
		{
			GameObject downObject=SceneInfo.getInstance().getPosition(4);
			if(_myCard.team == 0)
			{
				downObject=SceneInfo.getInstance().getPosition(10);
			}
			Effect effect = new Effect(effectName,keepTime,delayTime,downObject.transform.position,downObject.transform.position,
				downObject.transform.rotation,pve.curTime, 0, parent);
			result.Add(effect);
		}break;
		}
		return result;
	}
	
	private void resetTransitions()
	{
		if(animator.GetBool("idle2beHurt"))
		{
			animator.SetBool("idle2beHurt",false);
		}
		if(animator.GetBool("die2idle"))
		{
			animator.SetBool("die2idle",false);
		}
		if(animator.GetBool("idle2tank"))
		{
			animator.SetBool("idle2tank",false);
		}
		if(animator.GetBool("idle2storePower"))
		{
			animator.SetBool("idle2storePower",false);
		}
		if(animator.GetBool("idle2resume"))
		{
			animator.SetBool("idle2resume",false);
		}
		if(animator.GetBool("storePower2useSkill"))
		{
			animator.SetBool("storePower2useSkill",false);
		}
		if(animator.GetBool("idle2goForT"))
		{
			animator.SetBool("idle2goForT",false);
		}
		if(animator.GetBool("idle2beTank"))
		{
			animator.SetBool("idle2beTank", false);
		}
		if(animator.GetBool("go2phyAttack"))
		{
			animator.SetBool("go2phyAttack",false);
		}
		if(animator.GetBool("phyAttack2back"))
		{
			animator.SetBool("phyAttack2back",false);
		}
		if(animator.GetBool("idle2beHurtForT"))
		{
			animator.SetBool("idle2beHurtForT", false);
		}
		if(animator.GetBool("beHurtForT2backForT"))
		{
			animator.SetBool("beHurtForT2backForT",false);
		}
		if(animator.GetBool("idle2unitSkill"))
		{
			animator.SetBool("idle2unitSkill",false);
		}
		if(animator.GetBool("storePower2go"))
		{
			animator.SetBool("storePower2go", false);
		}
		if(animator.GetBool("unitSkill2idle"))
		{
			animator.SetBool("unitSkill2idle",false);
		}
		if(animator.GetBool("idle2die"))
		{
			animator.SetBool("idle2die",false);
		}
	}
	
	private void checkRunning()
	{
		if(effects==null)
		{
			return;
		}
		//攻击者的特效是否结束//
		//只有在当前技能不是防御特效时//
		if(!running || (effects.Count>0 && skill.type != 2) || frame>0)
		{
			return;
		}
		//目标列表//
		if(targets!=null)
		{
			foreach(Card c in targets)
			{
				if(ignoreT)
				{
					if(c!=this && c.getSkill().type!=2 && c.isRunning())
					{
						//Debug.Log("team:"+c.team+",sequence:"+c.sequence);
						return;
					}
				}
				else
				{
					if(c!=this && c.isRunning())
					{
						//Debug.Log("team:"+c.team+",sequence:"+c.sequence);
						return;
					}
				}
			}
		}
		//t的//
		if(tanks!=null)
		{
			foreach(Card tank in tanks)
			{
				if(tank!=this && tank.isRunning())
				{
					//Debug.Log("team:"+tank.team+",sequence:"+tank.sequence);
					return;
				}
			}
		}
		//防御目标//
		if(beTanks!=null)
		{
			foreach(Card c in beTanks)
			{
				if(c!=this && c.isRunning())
				{
					//Debug.Log("team:"+c.team+",sequence:"+c.sequence);
					return;
				}
			}
		}
		//检测自身动画状态//
		if(curStage==-1 && (isPlayTheName("Base Layer.idle") || isPlayTheName("Base Layer.die")))
		{
			running=false;
		}
	}
	
	/// <summary>
	/// 是否忙碌
	/// </summary>
	/// <returns>
	/// The running.
	/// </returns>
	public bool isRunning()
	{
		return running;
	}
	
	public bool isPlayTheName(string name)
	{
		AnimatorStateInfo asi=animator.GetCurrentAnimatorStateInfo(0);
		return asi.IsName(name);
	}
	
	//mark -- xuyan//
	public float getAnimatorStateInfoLength(string name)
	{
		AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
		return asi.length; 
	}
	
	/// <summary>
	/// cuxl -- Creates the hit number object. 
	/// </summary>
	/// <param name='hurtNum'>
	/// Hurt number. 创建受伤的数字//
	/// </param>
	/// <param name='type'>
	/// Type.		//数值的类型，3是掉血，4是加血//
	/// </param>
	public void createHitNumObject(int hurtNum, int type,int restrain = 0)
	{
       
		GameObject hitNum = null;

        if ((type == 3 || type == 1 )&& team == 1)
        {
            UIInterfaceManager.mInstance.SetHurtNum(hurtNum);
        }
		//当攻击的目标是t时，泽播放正常数字，当攻击多个//
		if(defendBlood && type != 4)
		{
			hitNum = Instantiate(GameObjectUtil.LoadResourcesPrefabs("HitNumForT", 2)) as GameObject;
		}
		else	if(type == 1)
		{
			hitNum = 	Instantiate(GameObjectUtil.LoadResourcesPrefabs("HitNumForCrit", 2)) as GameObject;
		}
		else
		{
			hitNum = Instantiate(GameObjectUtil.LoadResourcesPrefabs("HitNum", 2)) as GameObject;
		}
		//指定父节点//
		if(hitNum != null)
		{
			hitNum.transform.parent = PVESceneControl.mInstance.hitNumParent.transform;
			hitNum.transform.localScale = Vector3.one;
			//计算坐标//
			Vector3 worldPos = body.transform.position;
			Vector2 screenPos = mainCamera.WorldToScreenPoint(worldPos);
			Vector3 curPos = NGUICamera.ScreenToWorldPoint(screenPos);
			hitNum.transform.position = curPos;
			HitNumManager numManager = hitNum.GetComponent<HitNumManager>();
			numManager.posObject = body;
			numManager.desObject = head;
			numManager.mainCamera = mainCamera;
			numManager.NGUICamera = NGUICamera;
			
			if(type==2)
			{
				numManager.CreateHitNum("MISS", type);
			}
			else if(restrain == 1)
			{
				numManager.CreateHitNum(TextsData.getData(700).chinese + hurtNum,type);
			}
			else
			{
				numManager.CreateHitNum(hurtNum + "", type);
			}
		}
		//修改血条//
		bloodMana.changeBloodBar();
		
	}
	
	public void CreateName()
	{
		if(nameObj == null)
		{
			nameObj = 	Instantiate(GameObjectUtil.LoadResourcesPrefabs("CardName", 2)) as GameObject;
			GameObject parent = PVESceneControl.mInstance.hitNumParent;
			GameObjectUtil.gameObjectAttachToParent(nameObj, parent);
			
			Vector3 worldPos = new Vector3(head.transform.position.x + 0.5f, head.transform.position.y, head.transform.position.z);
			Vector2 screenPos = mainCamera.WorldToScreenPoint(worldPos);
			Vector3 curPos = NGUICamera.ScreenToWorldPoint(screenPos);
			nameObj.transform.position = curPos;
			
			CardNameManager cnManager = nameObj.GetComponent<CardNameManager>();
			cnManager.SetData(head, mainCamera, NGUICamera);
			
			UILabel label = nameObj.transform.FindChild("Name").GetComponent<UILabel>();
			if(sheepCardName != "")
			{
				label.text = sheepCardName;
			}
			else
			{
				label.text = cardData.name;
			}
		}
	}
	
	//修改血条//
	public void changeBloodBar()
	{
		//修改血条//
		bloodMana.changeBloodBar();
	}
	
	public void AddEnergy()
	{
		pve.players[0].tempEnergyToEnergy();
		pve.players[1].tempEnergyToEnergy();
		
		pve.energyManager.energyChange();
		updateUnitBtn();
	}
	
	/**更新合体技按钮**/
	private void updateUnitBtn()
	{
		UIEffectData ued = UIEffectData.getData(10006);
		string btnEffPath = ued.path + ued.name;
		pve.uniteManager.CreatePowerEf(btnEffPath , -1);
		pve.uniteManager.RemovePowerEf();
	}
	
	public void hideObj()
	{
		if(!haveStarted)
		{
			haveStarted=true;
			start2();
		}
		
		isHide = true;
		fbxObj.SetActive(false);
		shadow.SetActive(false);
		bloodBar.SetActive(false);
		nameObj.SetActive(false);
		HideEffect();
	}
	
	public void recoverShowObj()
	{
		isHide = false;
		fbxObj.SetActive(true);
		shadow.SetActive(true);
		bloodBar.SetActive(true);
		nameObj.SetActive(true);
		ShowEffect();
	}
	
	public void resetPos()
	{
		_myTransform.position = initPosition;
	}
	

	public void playCastPSEffect()
	{
		effects.AddRange(createEffect(2,"lightCharge01",1.0f,1,this,null,0,false,-1,this));
	}
	
	public void doBacklash()
	{
		//每次普通攻击后判断一次是否有一方死亡//
		if(PVESceneControl.mInstance.players[1].isDead())
		{
			PVESceneControl.mInstance.needShowWinCameraEffect = true;
			PVESceneControl.mInstance.setPlayerWinInfo();
			PVESceneControl.mInstance.setBattleOver(true);
			PVESceneControl.mInstance.setBattleResult(1);
		}
		else	if(PVESceneControl.mInstance.players[0].isDead())
		{
			PVESceneControl.mInstance.setPlayerLoseInfo();
			PVESceneControl.mInstance.setBattleOver(true);
			PVESceneControl.mInstance.setBattleResult(0);
		}
		
		Invoke("showBacklashDemageNum",0.1f);

		if(curStage != -1)
			return;
		if(hurt!= null && hurt[2] == 1)
		{
			//播放卡牌死亡音效//
			MusicManager.playCardSoundEffect(cardData.id, 1);
			animator.SetBool("idle2die",true);
			stageTime=0;
			//清空特效//
			CleanEffect();
		}
		isCritAttack = false;
		running=true;
		frame=0.1f;
	}
	
	void showBacklashDemageNum()
	{
		if(backlashDamageNum > 0)
		{
			createHitNumObject(backlashDamageNum,3);
		}
		backlashDamageNum = 0;
	}
	
	public void gc()
	{
		cardData=null;
		skill=null;
		beHurtSkill=null;
		resumeSkill=null;
		beTankSkill=null;
		if(passiveSkills != null)
		{
			passiveSkills.Clear();
			passiveSkills=null;
		}
		
		if(equipInfos!=null)
		{
			equipInfos.Clear();
			equipInfos=null;
		}
		hurt=null;
		if(targets!=null)
		{
			targets.Clear();
			targets=null;
		}
		if(tanks!=null)
		{
			tanks.Clear();
			tanks=null;
		}
		_myCard=null;
		if(beTanks!=null)
		{
			beTanks.Clear();
			beTanks=null;
		}
		if(effects!=null)
		{
			foreach(Effect e in effects)
			{
				e.gc();
			}
			effects.Clear();
			effects=null;
		}
		if(effectMark!=null)
		{
			effectMark.Clear();
			effectMark=null;
		}
		if(bloodMana!=null)
		{
			bloodMana.gc();
		}
		if(UniteTargets!=null)
		{
			UniteTargets.Clear();
			UniteTargets=null;
		}
		shaderNameList.Clear();
		shaderNameList=null;
		pve=null;
		GameObject.Destroy(bloodBar);
		bloodBar=null;
		GameObject.Destroy(shadow);
		shadow=null;
		GameObject.Destroy(nameObj);
		nameObj=null;
		GameObject.Destroy(gameObject);
		//Resources.UnloadUnusedAssets();
	}
}
