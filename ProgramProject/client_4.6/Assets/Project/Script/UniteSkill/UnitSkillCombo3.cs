using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSkillCombo3 : MonoBehaviour 
{
	// camera pos
	public GameObject[] cameraPos;
	
	// left card pos 
	public GameObject[] leftCardPos0;
	public GameObject[] leftCardPos1;
	public GameObject[] leftCardPos2;
	public GameObject[] leftCardPos3;
	public GameObject[] leftCardPos4;
	public GameObject[] leftCardPos5;
	public GameObject[] leftCardPos6;
	
	// rigth card pos
	public GameObject[] rightCardPos0;
	public GameObject[] rightCardPos1;
	public GameObject[] rightCardPos2;
	public GameObject[] rightCardPos3;
	public GameObject[] rightCardPos4;
	public GameObject[] rightCardPos5;
	public GameObject[] rightCardPos6;
	
	// path node pos
	public Transform[] pathNodeList0;
	public Transform[] pathNodeList1;
	public Transform[] pathNodeList2;
	public Transform[] pathNodeList3;
	public Transform[] pathNodeList4;
	
	// card list
	public List<GameObject> leftCardList;
	public List<GameObject> rightCardList;	
	
	float stateTime = 0.0f;
	float stateRuningTime = 0.0f;
	
	GameObject lookAtCard = null;
	bool isLookAtCard = false;
		
	public GameObject objectListNode;
	
	bool needWaitChangeCamera = false;
	bool needPlayEndEffect = false;
	bool needChangeTimeSpeed = false;
	//bool needSetEndCameraPos = false;
	bool needMoveComboAttackObj = false;
	
	//bool needChangeRushSpeed = false;
	//bool needRecoverRushSpeed = false;
	
	bool isRunning = false;
	
	string soundEffName = "unitskill_sjcj";
	
	
	public enum COMBOSTATE
	{
		E_Null = -1,
		E_InitPos = 0,
		E_FirstMove = 1,
		E_HitFly = 2,
		E_DoFly = 3,
		E_ComboAttack = 4,
		E_ComboEnd = 5,
		E_OnAirEnd = 6,
		E_End = 7,
	}
	
	COMBOSTATE comboState;
	
	// Update is called once per frame
	void Update ()
	{
		if(!isRunning)
		{
			return;
		}
		checkComboState();
		if(isLookAtCard)
		{
			ActionCameraControl.mInstance.lookAtGameObject(lookAtCard);
		}
	}
	
	public void runShow(List<int> leftCardIDList,List<int> rightCardIDList)
	{
		isRunning = true;
		leftCardList.Clear();
		rightCardList.Clear();
		
		// leftCardIDList.cout == 3
		for(int i = 0; i < leftCardIDList.Count;++i)
		{
			int id = (int)leftCardIDList[i];
			if(id > 0)
			{
				CardData cd = CardData.getData(id);
				if(cd != null)
				{
					GameObject obj = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel,0)) as GameObject;
					if(obj != null)
					{
						
						obj.AddComponent<CardForComboUnitSkill>();
						obj.GetComponent<CardForComboUnitSkill>().cardFormID = id;
						obj.GetComponent<CardForComboUnitSkill>().cardType = (CardForComboUnitSkill.CardType)cd.type;
						GameObjectUtil.setGameObjectLayer(obj,STATE.LAYER_ID_COMBOACTION);
						leftCardList.Add(obj);
						GameObjectUtil.gameObjectAttachToParent(obj,objectListNode);
					}
				}
			}
		}
		// rightCardIDList.cout = 1~6
		for(int i = 0; i < rightCardIDList.Count;++i)
		{
			int id = (int)rightCardIDList[i];
			if(id > 0)
			{
				CardData cd = CardData.getData(id);
				if(cd != null)
				{
					GameObject obj = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel,0)) as GameObject;
					if(obj != null)
					{
						obj.AddComponent<CardForComboUnitSkill>();
						obj.GetComponent<CardForComboUnitSkill>().cardFormID = id;
						obj.GetComponent<CardForComboUnitSkill>().cardType = (CardForComboUnitSkill.CardType)cd.type;
						GameObjectUtil.setGameObjectLayer(obj,STATE.LAYER_ID_COMBOACTION);
						rightCardList.Add(obj);
						GameObjectUtil.gameObjectAttachToParent(obj,objectListNode);
					}
				}
			}
		}
		setComboState(COMBOSTATE.E_InitPos,1.5f);
	}
	
	bool isComboStateFinish()
	{
		bool r = false;
		if(stateRuningTime < stateTime)
		{
			stateRuningTime += Time.deltaTime;
		}
		else
		{
			isLookAtCard = false;
			lookAtCard = null;
			stateRuningTime = 0;
			stateTime = 0;
			r = true;
		}
		return r;
	}
	
	public void checkComboState()
	{
		switch(comboState)
		{
		case COMBOSTATE.E_Null:
		{
		}break;
		case COMBOSTATE.E_InitPos:
		{
			if(isComboStateFinish())
			{
				setComboState(COMBOSTATE.E_FirstMove,0.333f);
			}
		}break;
		case COMBOSTATE.E_FirstMove:
		{
			//播放音效//
			MusicManager.playUniteSkillSoundEffByName(soundEffName, 0);
			if(isComboStateFinish())
			{
				setComboState(COMBOSTATE.E_HitFly,0.1f);
				GameObjectUtil.AddSpeed(0.3f);
				
			}
			else 
			{
				if(stateRuningTime > 0.1f && needWaitChangeCamera)
				{
					needWaitChangeCamera = false;
					GameObjectUtil.copyTarnsformValue(cameraPos[1],ActionCameraControl.mInstance.cameraObj);
				}
			}
		}break;
		case COMBOSTATE.E_HitFly:
		{
			if(isComboStateFinish())
			{
				setComboState(COMBOSTATE.E_DoFly,0.633f);
				GameObjectUtil.AddSpeed(1);
			}

		}break;
		case COMBOSTATE.E_DoFly:
		{
			if(isComboStateFinish())
			{
				setComboState(COMBOSTATE.E_ComboAttack,0.8f);
			}
			else
			{
				if(stateRuningTime > 0.3f &&needMoveComboAttackObj)
				{
					needMoveComboAttackObj = false;
					leftCardList[1].GetComponent<CardForComboUnitSkill>().setMoveType(CardForComboUnitSkill.MoveType.E_MTToComboAttack);
					leftCardList[1].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboMove);
					leftCardList[1].GetComponent<CardForComboUnitSkill>().setTargetCard(rightCardList[0].GetComponent<CardForComboUnitSkill>());
					iTween.MoveTo(leftCardList[1],iTween.Hash("position",leftCardPos2[1].transform.position,"time",0.333f,"easetype",iTween.EaseType.linear));
				}
			}
		}break;
		case COMBOSTATE.E_ComboAttack:
		{
			if(isComboStateFinish())
			{
				setComboState(COMBOSTATE.E_ComboEnd,0.65f);

			}
		}break;
		case COMBOSTATE.E_ComboEnd:
		{
			if(isComboStateFinish())
			{
				setComboState(COMBOSTATE.E_OnAirEnd,0.8f);
				GameObjectUtil.AddSpeed(1);
			}
			else
			{
				if(stateRuningTime > 0.4f && needChangeTimeSpeed)
				{
					needChangeTimeSpeed = false;
					GameObjectUtil.AddSpeed(0.3f);
					ActionCameraControl.mInstance.playMovePath(pathNodeList3,0.25f);
				}
			}
		}break;
		case COMBOSTATE.E_OnAirEnd:
		{
			if(isComboStateFinish())
			{
				setComboState(COMBOSTATE.E_End,0.5f);
			}
			else
			{
				CardForComboUnitSkill.StageType t = rightCardList[0].GetComponent<CardForComboUnitSkill>().getStageType();
				if(t == CardForComboUnitSkill.StageType.E_STOnAir)
				{
					if(stateRuningTime > 0.22f && needPlayEndEffect)
					{
						GameObjectUtil.copyTarnsformValue(cameraPos[6],ActionCameraControl.mInstance.cameraObj);
						rightCardList[0].GetComponent<CardForComboUnitSkill>().playEndEffectCombo3(rightCardPos6[0]);
						needPlayEndEffect = false;
					}
					if(stateRuningTime > 0.4f )
					{
						rightCardList[0].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STOnAirEnd);
					}
				}
				
			}
		}break;
		case COMBOSTATE.E_End:
		{
			if(isComboStateFinish())
			{
				isRunning = false;
				ActionCameraControl.mInstance.finishShow();
				GameObjectUtil.destroyGameObjectAllChildrens(objectListNode);
			}
		}break;
		}
	}

	public void finishShow()
	{
		isRunning = false;
		lookAtCard = null;
		isLookAtCard = false;
		GameObjectUtil.destroyGameObjectAllChildrens(objectListNode);
	}
	
	public void setComboState(COMBOSTATE state,float t)
	{
		comboState = state;
		stateTime = t;
		switch(comboState)
		{
		case COMBOSTATE.E_Null:
		{
		}break;
		case COMBOSTATE.E_InitPos:
		{
			doInitPos();
		}break;
		case COMBOSTATE.E_FirstMove:
		{
			doFirstMove();
		}break;
		case COMBOSTATE.E_HitFly:
		{
			doHitFly();
		}break;
		case COMBOSTATE.E_DoFly:
		{
			doFly();
		}break;
		case COMBOSTATE.E_ComboAttack:
		{
			doComboAttack();
		}break;
		case COMBOSTATE.E_ComboEnd:
		{
			doComboEnd();
		}break;
		case COMBOSTATE.E_OnAirEnd:
		{
			doOnAirEnd();
		}break;
		case COMBOSTATE.E_End:
		{
			doEnd();
		}break;
		}
	}
	
	void doInitPos()
	{
		GameObjectUtil.copyTarnsformValue(cameraPos[0],ActionCameraControl.mInstance.cameraObj);	
		ActionCameraControl.mInstance.playMovePath(pathNodeList0,1f);
		isLookAtCard = true;
		lookAtCard = leftCardList[0].GetComponent<CardForComboUnitSkill>().body;
		// left cards pos
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos0);
		// right cards pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos0);
		
	}

	void doFirstMove()
	{
		needWaitChangeCamera = true;
		// left cards pos
		List<GameObject> leftList = new List<GameObject>();
		leftList.Add(leftCardList[0]);
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos1,leftList);
		leftCardList[0].GetComponent<CardForComboUnitSkill>().setMoveType(CardForComboUnitSkill.MoveType.E_MTToHitFly);
		leftCardList[0].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboMove);
		leftCardList[0].GetComponent<CardForComboUnitSkill>().setTargetCard(rightCardList[0].GetComponent<CardForComboUnitSkill>());
		leftCardList[0].GetComponent<CardForComboUnitSkill>().comboType = 0;
		iTween.MoveTo(leftCardList[0],iTween.Hash("position",leftCardPos1[0].transform.position,"time",0.333f,"easetype",iTween.EaseType.linear));
		
		// right card pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos1);
	}
	
	void doHitFly()
	{
		isLookAtCard = true;
		lookAtCard = rightCardList[0].GetComponent<CardForComboUnitSkill>().body;
	}
	
	void doFly()
	{
		needMoveComboAttackObj = true;
		ActionCameraControl.mInstance.playMovePath(pathNodeList1,0.3f);
		
		isLookAtCard = true;
		lookAtCard = rightCardList[0].gameObject.GetComponent<CardForComboUnitSkill>().body;
		
		// left cards pos
		List<GameObject> leftList = new List<GameObject>();
		leftList.Add(leftCardList[1]);
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos2,leftList);
		
		// right card pos
		List<GameObject> rightList = new List<GameObject>();
		rightList.Add(rightCardList[0]);
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos2,rightList);
		rightCardList[0].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STOnAir);
		iTween.MoveTo(rightCardList[0],iTween.Hash("position",rightCardPos2[0].transform.position,"time",0.633f,"easetype",iTween.EaseType.linear));
	}
	
	void doComboAttack()
	{
		// camera pos 
		// left cards pos
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos3);
		// right card pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos3);
	}
	

	void doComboEnd()
	{
		needChangeTimeSpeed = true;
		needPlayEndEffect = true;
		
		ActionCameraControl.mInstance.playMovePath(pathNodeList2,0.3f);
				
		isLookAtCard = true;
		lookAtCard = rightCardList[0].GetComponent<CardForComboUnitSkill>().body;
		
		// left cards pos
		List<GameObject> leftList = new List<GameObject>();
		leftList.Add(leftCardList[1]);
		leftList.Add(leftCardList[2]);
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos5,leftList);
		iTween.MoveTo(leftCardList[1],iTween.Hash("position",leftCardPos5[1].transform.position,"time",0.333f,"easetype",iTween.EaseType.linear));
		leftCardList[2].GetComponent<CardForComboUnitSkill>().setTargetCard(rightCardList[0].GetComponent<CardForComboUnitSkill>());
		leftCardList[2].GetComponent<CardForComboUnitSkill>().setMoveType(CardForComboUnitSkill.MoveType.E_MTToComboEnd);
		leftCardList[2].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboMove);
		iTween.MoveTo(leftCardList[2],iTween.Hash("position",leftCardPos5[2].transform.position,"time",0.7f));
		
		// right card pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos5);
	}
	
	void doOnAirEnd()
	{
		isLookAtCard = true;
		lookAtCard = rightCardList[0].GetComponent<CardForComboUnitSkill>().body;
		
		// left cards pos
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos6);
		
		// right card pos
		List<GameObject> rightList = new List<GameObject>();
		rightList.Add(rightCardList[0]);
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos6,rightList);
		iTween.MoveTo(rightCardList[0],iTween.Hash("position",rightCardPos6[0].transform.position,"time",0.4f,"easetype",iTween.EaseType.easeInOutExpo));
	}
	
	void doEnd()
	{
	
	}
	
	
	public void gc()
	{
		
	}
}
