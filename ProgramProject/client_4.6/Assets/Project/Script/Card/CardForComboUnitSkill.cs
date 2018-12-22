using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardForComboUnitSkill : MonoBehaviour
{
	public enum StageType : int
	{
		E_STNULL = -1, 
		E_STComboIdle = 0,
		E_STComboMove = 1,
		E_STComboHitFly = 2,
		E_STComboAttack = 3,
		E_STComboRush = 4,
		E_STComboRushStop = 5,
		E_STComboEnd = 6,
		E_STOnAir = 7,
		E_STOnAirEnd = 8,
		E_STComboFinish = 9,
		E_STIdleToComboEnd = 10,
	}
	
	public enum MoveType : int
	{
		E_MTNULL = -1,
		E_MTToHitFly = 0,
		E_MTToComboAttack = 1,
		E_MTToComboEnd = 2,
	}
	
	public enum CardType : int
	{
		E_CTCloseCombat = 0,
		E_CTMagic = 1,
		E_CTBow = 2,
	}
	
	public Animator animator;
	public GameObject head;
	public GameObject body;
	public GameObject foot;
	public GameObject front;
	public GameObject attack_front;
	//public DaoGuangController dgController;
	private CardForComboUnitSkill _myCard;
	
	public GameObject _myObj;
	
	public int cardFormID;
	public CardType cardType;
	
	CardForComboUnitSkill targetCard;
	
	public int comboType =-1; // 0 : combo 3, 1: combo 6
	
	
	
	/// <summary>
	/// 待销毁效果集
	/// </summary>
	private ArrayList effects;
	
	/*stage相关*/
	float stageTime = -1;
	float curStageTime;
	StageType mStageType = StageType.E_STNULL;
	
	float moveTime = 0.333f;
	
	MoveType mMoveType = MoveType.E_MTNULL;
	
	void Awake()
	{
		_myCard = this;
		_myObj = _myCard.gameObject;
		animator= GetComponent<Animator>();
		head = _myObj.transform.FindChild("head").gameObject;
		body = _myObj.transform.FindChild("body").gameObject;
		foot = _myObj.transform.FindChild("foot").gameObject;
		front = _myObj.transform.FindChild("front").gameObject;
		attack_front = _myObj.transform.FindChild("attack_front").gameObject;
		//dgController = GetComponent<DaoGuangController>();
		//if(dgController != null && dgController.trail != null)
		//{
			//dgController.trail.gameObject.SetActive(false);
		//}
	}
	
	void Start()
	{
		effects=new ArrayList();		
		setStageType(StageType.E_STComboIdle);
	}
	
	void Update()
	{
		if(stageTime >= 0)
		{
			if(curStageTime >= stageTime)
			{
				switch(mStageType)
				{
				case StageType.E_STNULL:
				{
					// do nothing

				}break;
				case StageType.E_STComboIdle:
				{
					// do nothing

				}break;
				case StageType.E_STComboMove:
				{	
					switch(mMoveType)
					{
					case MoveType.E_MTNULL:
					{
						setStageType(StageType.E_STNULL);
					}break;
					case MoveType.E_MTToHitFly:
					{
						setStageType(StageType.E_STComboHitFly);
					}break;
					case MoveType.E_MTToComboAttack:
					{
						setStageType(StageType.E_STComboAttack);
					}break;
					case MoveType.E_MTToComboEnd:
					{
						setStageType(StageType.E_STComboEnd);
					}break;
					}
				}break;		  
				
				case StageType.E_STOnAir:
				{
					setStageType(StageType.E_STOnAirEnd);		
				}break;
				case StageType.E_STComboRush:
				case StageType.E_STComboRushStop:
				case StageType.E_STComboHitFly:
				case StageType.E_STComboEnd:
				case StageType.E_STOnAirEnd:
				case StageType.E_STIdleToComboEnd:
				{
					setStageType(StageType.E_STNULL);		
				}break;
				case StageType.E_STComboAttack:
				{
					setStageType(StageType.E_STComboFinish);
				}break;
				}
			}
			else
			{
				curStageTime += Time.deltaTime;
			}
		}
	}
	
	public void setStageType(StageType stageType)
	{
		resetTransitions();
		mStageType = stageType;
		curStageTime = 0;
		switch(mStageType)
		{
		case StageType.E_STNULL:
		{
			// do nothing
			stageTime = -1;
		}break;
		case StageType.E_STComboIdle:
		{
			animator.SetBool("idle2comboIdle",true);
			stageTime = -1;
		}break;
		case StageType.E_STComboMove:
		{
			if(mMoveType != MoveType.E_MTNULL)
			{
				stageTime = moveTime;
				animator.SetBool("comboIdle2comboMove",true);
			}
		}break;		  
		case StageType.E_STComboHitFly:
		{				
			stageTime = 1.0f;
			animator.SetBool("comboMove2hitFly",true);
			ArrayList targetList = new ArrayList();
			targetList.Add(targetCard);
			if(comboType == 0)
			{
				effects.AddRange(createEffect(4,"hetiji_shangchongqi",0.7f,1,this,targetList,0.1f));
			}
			else if(comboType == 1)
			{
				effects.AddRange(createEffect(4,"hetiji04_Atk01",0.7f,1,this,targetList,0.1f));
			}
			
		}break;	
		case StageType.E_STComboAttack:
		{
			stageTime = 1.0f;
			animator.SetBool("comboMove2comboAttack",true);
			
			ArrayList targetList = new ArrayList();
			targetList.Add(targetCard);
			effects.AddRange(createEffect(5,"hetiji_hurt05",1.5f,1,this,targetList,0.0f));
			
		}break;
		case StageType.E_STComboRush:
		{
			stageTime = 0.1f;
			animator.SetBool("comboIdle2rush",true);
		}break;
		case StageType.E_STComboEnd:
		{
			stageTime = 1.0f;
			ArrayList targetList = new ArrayList();
			targetList.Add(targetCard);
			effects.AddRange(createEffect(5,"hetiji_hurt02",1.0f,1,null,targetList,0.25f));
			animator.SetBool("comboMove2comboEnd",true);
		}break;
		case StageType.E_STComboRushStop:
		{
			stageTime = -1f;
			animator.SetBool("rush2comboIdle",true);
		}break;
		case StageType.E_STOnAir:
		{
			stageTime = -1;
			animator.SetBool("comboIdle2onAir",true);
		}break;
		case StageType.E_STOnAirEnd:
		{
			stageTime = 0.47f;
			animator.SetBool("onAir2onAirEnd",true);
			
		}break;
		case StageType.E_STComboFinish:
		{
			stageTime = 0.1f;
			animator.SetBool("comboAttack2back",true);
		}break;
		case StageType.E_STIdleToComboEnd:
		{
			stageTime = 0.1f;
			animator.SetBool("comboIdle2comboEnd",true);
			playComobEndEffect();
		}break;
		}

	}
	
	
	public void playEndEffectCombo3(GameObject tarObject)
	{
		ArrayList tars = new ArrayList();
		tars.Add(tarObject);
		effects.AddRange(createEffect(0,"hetiji_zadi",1.0f,1,null,tars,0.0f));
	}
	
	public void playEndEffectCombo6(GameObject tarObject)
	{
		ArrayList tars = new ArrayList();
		tars.Add(tarObject);
		effects.AddRange(createEffect(0,"zhaohuanshou_monkey_Atk02",1.0f,1,null,tars,0.0f));
	}
	
	public void playComboRushEffect(GameObject targetObject)
	{
		ArrayList tars = new ArrayList();
		CardForComboUnitSkill cfcus = targetObject.GetComponent<CardForComboUnitSkill>();
		if(cfcus != null)
		{
			tars.Add(cfcus);
		}
		//effects.AddRange(createEffect(3,"hetiji03_hurt01",0.2f,1,null,tars,0.025f));
		effects.AddRange(createEffect(3,"hetiji04_Atk02",0.2f,1,null,tars,0.025f));
	}
	
	public void playChargePlayerEffect()
	{
		//ArrayList tars = new ArrayList();
		effects.AddRange(createEffect(1,"hetiji03_chongji02",0.2f,1,this,null,0,true));
	}
	
	public void playComobEndEffect()
	{
		//effects.AddRange(createEffect(7,"hetiji02_hurt01",1.0f,1,this,null,0.25f));
		effects.AddRange(createEffect(7,"hetiji04_Atk03",1.0f,1,this,null,0.25f));
	}
	
	public StageType getStageType()
	{
		return mStageType;
	}
	
	public void setMoveType(MoveType moveType)
	{
		mMoveType = moveType;
	}
	
	void resetTransitions()
	{
		if(animator.GetBool("idle2comboIdle"))
		{
			animator.SetBool("idle2comboIdle",false);
		}
		if(animator.GetBool("comboIdle2idle"))
		{
			animator.SetBool("comboIdle2idle",false);
		}
		if(animator.GetBool("comboIdle2comboMove"))
		{
			animator.SetBool("comboIdle2comboMove",false);
		}
		if(animator.GetBool("comboMove2comboAttack"))
		{
			animator.SetBool("comboMove2comboAttack",false);
		}
		if(animator.GetBool("comboMove2comboEnd"))
		{
			animator.SetBool("comboMove2comboEnd",false);
		}
		if(animator.GetBool("comboMove2hitFly"))
		{
			animator.SetBool("comboMove2hitFly",false);
		}
		if(animator.GetBool("comboIdle2rush"))
		{
			animator.SetBool("comboIdle2rush",false);
		}
		if(animator.GetBool("rush2comboIdle"))
		{
			animator.SetBool("rush2comboIdle", false);
		}
		if(animator.GetBool("comboIdle2onAir"))
		{
			animator.SetBool("comboIdle2onAir",false);
		}
		if(animator.GetBool("onAir2onAirEnd"))
		{
			animator.SetBool("onAir2onAirEnd",false);
		}
		if(animator.GetBool("comboAttack2back"))
		{
			animator.SetBool("comboAttack2back",false);
		}
		if(animator.GetBool("comboIdle2comboEnd"))
		{
			animator.SetBool("comboIdle2comboEnd",false);
		}
	}
	
	public void setTargetCard(CardForComboUnitSkill target)
	{
		if(target != null)
		{
			targetCard = target;
		}
	}
	
	void LateUpdate()
	{
		if(effects != null)
		{
			checkEffect();
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
				if(effect.isDelayOver(ActionCameraControl.mInstance.curTime))
				{
					effect.create();
				}
			}
			if(!effect.isValid(ActionCameraControl.mInstance.curTime))
			{
				effects.RemoveAt(i);
				DestroyImmediate(effect.getEffectObj());
				effect.gc();
			}
		}
	}

	private ArrayList createEffect(int pos,string effectName,float keepTime,int moveType,CardForComboUnitSkill src,ArrayList tars,float delayTime,bool isAttachToTargetObj = false)
	{
		ArrayList result=new ArrayList();
		if("-1".Equals(effectName.Trim()))
		{
			return result;
		}
		switch(pos)
		{
		case 0:
		{
			if(tars == null || tars.Count == 0)
				return result;
			GameObject t = (GameObject)tars[0];
			Vector3 position = t.transform.position;
			position = new Vector3(position.x,position.y + 0.5f,position.z);
			Quaternion rotation = t.transform.rotation;
			Effect effect = new Effect(effectName,keepTime,delayTime,position,position,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
			result.Add(effect);
		}break;
		case 1://攻击者身上//
		{
			Vector3 srcPos=src.body.transform.position;
			Vector3 tarPos=Vector3.zero;
			Quaternion rotation=src.body.transform.rotation;
			Effect effect=null;
			if(moveType == 0)
			{
				foreach(CardForComboUnitSkill targetCard in tars)
				{
					tarPos = targetCard.body.transform.position;
					effect = new Effect(effectName,keepTime,delayTime,srcPos,tarPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
				}
			}
			else
			{
				if(isAttachToTargetObj)
				{
					effect = new Effect(effectName,keepTime,delayTime,srcPos,srcPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION,src.body);
				}
				else
				{
					effect = new Effect(effectName,keepTime,delayTime,srcPos,srcPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
				}
				
			}
			result.Add(effect);
		}break;
		case 2://攻击者脚下//
		{
			Vector3 srcPos=src.foot.transform.position;
			Vector3 tarPos=Vector3.zero;
			Quaternion rotation=src.foot.transform.rotation;
			Effect effect=null;
			if(moveType == 0)
			{
				foreach(CardForComboUnitSkill targetCard in tars)
				{
					tarPos = targetCard.foot.transform.position;
					effect = new Effect(effectName,keepTime,delayTime,srcPos,tarPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
				}
			}
			else
			{
				if(isAttachToTargetObj)
				{
					effect = new Effect(effectName,keepTime,delayTime,srcPos,srcPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION,src.foot);
				}
				else
				{
					effect = new Effect(effectName,keepTime,delayTime,srcPos,srcPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
				}
			}
			result.Add(effect);
		}break;
		case 3://目标身上//
		{
			if(tars!=null && tars.Count>0)
			{
				foreach(CardForComboUnitSkill tar in tars)
				{
					Effect effect = new Effect(effectName,keepTime,delayTime,tar.body.transform.position,tar.body.transform.position,tar.body.transform.rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
					result.Add(effect);
				}
			}
		}break;
		case 4://目标脚下//
		{
			if(tars!=null && tars.Count>0)
			{
				foreach(CardForComboUnitSkill tar in tars)
				{
					Effect effect = new Effect(effectName,keepTime,delayTime,tar.foot.transform.position,tar.foot.transform.position,tar.foot.transform.rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
					result.Add(effect);
				}
			}
		}break;
		case 5://目标head//
		{
			if(tars!=null && tars.Count>0)
			{
				foreach(CardForComboUnitSkill tar in tars)
				{
					Effect effect = new Effect(effectName,keepTime,delayTime,tar.head.transform.position,tar.head.transform.position,tar.head.transform.rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
					result.Add(effect);
				}
			}
		}break;
			
		case 6:  //目标Pos//
		{
			
		}break;
			
		case 7: //攻击者 front //
		{
			Vector3 srcPos=src.front.transform.position;
			Vector3 tarPos=Vector3.zero;
			Quaternion rotation=src.front.transform.rotation;
			
			Effect effect=null;
			if(moveType == 0)
			{
				foreach(CardForComboUnitSkill targetCard in tars)
				{
					tarPos = targetCard.body.transform.position;
					effect = new Effect(effectName,keepTime,delayTime,srcPos,tarPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
				}
			}
			else
			{
				if(isAttachToTargetObj)
				{
					effect = new Effect(effectName,keepTime,delayTime,srcPos,srcPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION,src.front);
				}
				else
				{
					effect = new Effect(effectName,keepTime,delayTime,srcPos,srcPos,rotation,ActionCameraControl.mInstance.curTime,STATE.LAYER_ID_COMBOACTION);
				}
			}
		
			result.Add(effect);
		}break;
		}
		return result;
	}
	
	public void gc()
	{
		
	}
}

