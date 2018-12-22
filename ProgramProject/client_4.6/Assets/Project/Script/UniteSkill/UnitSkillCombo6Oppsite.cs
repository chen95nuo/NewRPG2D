using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSkillCombo6Oppsite : MonoBehaviour
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

	
	// card list
	public List<GameObject> leftCardList;
	public List<GameObject> rightCardList;
	
	// rush end pos
	public GameObject[] rushEndPos;
	
	// card combo end start pos
	public GameObject cardComboEndStartPos;
	
	public GameObject objectListNode;
	
	float stateTime = 0.0f;
	float stateRuningTime = 0.0f;
	
	GameObject lookAtCard = null;
	bool isLookAtCard = false;
	
	bool isRunning = false;
	
	bool waitForFly = false;
	
	bool needWaitChangeCamera = false;
	bool needChangeTimeSpeed = false;
	bool	needPlayEndEffect = false;
	
	string soundEffName = "unitskill_qtgj";
	
	public enum COMBOSTATE
	{
		E_NULL 								= -1,
		E_InitPos 							= 0,
		E_FirstMove							= 1,
		E_HitFly								= 2,
		E_Rush1								= 3,
		E_Rush2								= 4,
		E_Rush3								= 5,
		E_ComboEnd						= 6,
		E_OnAirEnd							= 7,
		E_End									= 8,
	}
	COMBOSTATE	comboState;
	
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
		
		// leftCardIDList.cout == 1~6
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
		// rightCardIDList.cout = 6
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
			stateRuningTime = 0;
			stateTime = 0;
			r = true;
			isLookAtCard = false;
			lookAtCard = null;
		}
		return r;
	}
	
	public void checkComboState()
	{
		switch(comboState)
		{
		case COMBOSTATE.E_NULL:
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
				setComboState(COMBOSTATE.E_HitFly,0.4f);
				GameObjectUtil.AddSpeed(0.3f);
			}
			else 
			{
				if(stateRuningTime > 0.01f && needWaitChangeCamera)
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
				setComboState(COMBOSTATE.E_Rush1,0.2f);
			}
			else
			{
				if(stateRuningTime > 0.1f && waitForFly)
				{
					waitForFly = false;
					leftCardList[0].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STOnAir);
					iTween.MoveTo(leftCardList[0],iTween.Hash("position",leftCardPos2[0].transform.position,"time",0.19f));
				}
			}
		}break;
		case COMBOSTATE.E_Rush1:
		{
			if(isComboStateFinish())
			{
				rightCardList[1].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboRushStop);
				Destroy(rightCardList[1].GetComponent<iTween>());
				setComboState(COMBOSTATE.E_Rush2,0.2f);
			}
		}break;
		case COMBOSTATE.E_Rush2:
		{
			if(isComboStateFinish())
			{
				rightCardList[2].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboRushStop);
				Destroy(rightCardList[2].GetComponent<iTween>());
				setComboState(COMBOSTATE.E_Rush3,0.2f);
				
			}

		}break;
		case COMBOSTATE.E_Rush3:
		{
			if(isComboStateFinish())
			{
				Destroy(rightCardList[3].GetComponent<iTween>());
				Destroy(rightCardList[4].GetComponent<iTween>());
				rightCardList[3].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboRushStop);
				rightCardList[4].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboRushStop);
				setComboState(COMBOSTATE.E_ComboEnd,0.35f);
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
				if(stateRuningTime > 0.0f && needChangeTimeSpeed)
				{
					needChangeTimeSpeed = false;
					GameObjectUtil.AddSpeed(0.3f);
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
				CardForComboUnitSkill.StageType t = leftCardList[0].GetComponent<CardForComboUnitSkill>().getStageType();
				if(t == CardForComboUnitSkill.StageType.E_STOnAir)
				{
					if(stateRuningTime > 0.22f && needPlayEndEffect)
					{
						leftCardList[0].GetComponent<CardForComboUnitSkill>().playEndEffectCombo6(leftCardPos6[0]);
						needPlayEndEffect = false;
					}
					if(stateRuningTime > 0.4f )
					{
						leftCardList[0].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STOnAirEnd);
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
	
	public void setComboState(COMBOSTATE state,float t)
	{
		comboState = state;
		stateTime = t;
		switch(comboState)
		{
		case COMBOSTATE.E_NULL:
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
		case COMBOSTATE.E_Rush1:
		{
			doRush1();
		}break;
		case COMBOSTATE.E_Rush2:
		{
			doRush2();
		}break;
		case COMBOSTATE.E_Rush3:
		{
			doRush3();
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
			//doEnd();
		}break;
		}
	}
	
	void doInitPos()
	{
		GameObjectUtil.copyTarnsformValue(cameraPos[0],ActionCameraControl.mInstance.cameraObj);
		ActionCameraControl.mInstance.playMovePath(pathNodeList0,1f);
		isLookAtCard = true;
		lookAtCard = rightCardList[0].GetComponent<CardForComboUnitSkill>().body;
		// left cards pos
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos0);
		// right cards pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos0);
	}
	
	void doFirstMove()
	{
		needWaitChangeCamera = true;
		// right cards pos
		List<GameObject> rightList = new List<GameObject>();
		rightList.Add(rightCardList[0]);
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos1,rightList);
		rightCardList[0].GetComponent<CardForComboUnitSkill>().setMoveType(CardForComboUnitSkill.MoveType.E_MTToHitFly);
		rightCardList[0].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboMove);
		rightCardList[0].GetComponent<CardForComboUnitSkill>().setTargetCard(leftCardList[0].GetComponent<CardForComboUnitSkill>());
		rightCardList[0].GetComponent<CardForComboUnitSkill>().comboType = 1;
		iTween.MoveTo(rightCardList[0],iTween.Hash("position",rightCardPos1[0].transform.position,"time",0.333f,"easetype",iTween.EaseType.linear));
		
		// left card pos
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos1);
	}
	
	void doHitFly()
	{
		waitForFly = true;
		rightCardList[1].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboRush);
		rightCardList[2].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboRush);
		rightCardList[3].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboRush);
		rightCardList[4].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STComboRush);
	}
	
	void doRush1()
	{
		// camera pos
		GameObjectUtil.copyTarnsformValue(cameraPos[2],ActionCameraControl.mInstance.cameraObj);
		
		// left card pos
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos2);
		
		// right card pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos2);
		rightCardList[1].GetComponent<CardForComboUnitSkill>().playComboRushEffect(leftCardList[0]);
	
		// TODO
		rightCardList[1].GetComponent<CardForComboUnitSkill>().playChargePlayerEffect();
	
		
		iTween.MoveTo(rightCardList[1],iTween.Hash("position",rushEndPos[0].transform.position,"time",1f));//,"easetype",iTween.EaseType.linear));
		
				
	}
	
	void doRush2()
	{
		// camera pos
		GameObjectUtil.copyTarnsformValue(cameraPos[3],ActionCameraControl.mInstance.cameraObj);
		
		// right card pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos3);
		rightCardList[2].GetComponent<CardForComboUnitSkill>().playComboRushEffect(leftCardList[0]);
		
		// TODO
		rightCardList[2].GetComponent<CardForComboUnitSkill>().playChargePlayerEffect();
		
		iTween.MoveTo(rightCardList[2],iTween.Hash("position",rushEndPos[1].transform.position,"time",1f));//,"easetype",iTween.EaseType.linear));
		
		
		// left card pos
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos3);
		
		
	}
	
	void doRush3()
	{
		// camera pos
		GameObjectUtil.copyTarnsformValue(cameraPos[4],ActionCameraControl.mInstance.cameraObj);
		
		// right card pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos4);
		rightCardList[3].GetComponent<CardForComboUnitSkill>().playComboRushEffect(leftCardList[0]);
		rightCardList[4].GetComponent<CardForComboUnitSkill>().playComboRushEffect(leftCardList[0]);
		
		// TODO
		rightCardList[3].GetComponent<CardForComboUnitSkill>().playChargePlayerEffect();
		rightCardList[4].GetComponent<CardForComboUnitSkill>().playChargePlayerEffect();
		
		iTween.MoveTo(rightCardList[3],iTween.Hash("position",rushEndPos[2].transform.position,"time",1f));//,"easetype",iTween.EaseType.linear));
		iTween.MoveTo(rightCardList[4],iTween.Hash("position",rushEndPos[3].transform.position,"time",1f));//,"easetype",iTween.EaseType.linear));
		
		
		// left card pos
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos4);
		
	}
	
	void doComboEnd()
	{
		
		// camera pos
		GameObjectUtil.copyTarnsformValue(cameraPos[5].gameObject,ActionCameraControl.mInstance.cameraObj);
		
		needChangeTimeSpeed = true;
		needPlayEndEffect = true;
		
		// right cards pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos5);
		rightCardList[5].GetComponent<CardForComboUnitSkill>().setTargetCard(leftCardList[0].GetComponent<CardForComboUnitSkill>());
		rightCardList[5].GetComponent<CardForComboUnitSkill>().setStageType(CardForComboUnitSkill.StageType.E_STIdleToComboEnd);
		
		
		// left card pos
		List<GameObject> leftList = new List<GameObject>();
		leftList.Add(leftCardList[0]);
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos5);
			
	}
	
	void doOnAirEnd()
	{
		isLookAtCard = true;
		lookAtCard = leftCardList[0].GetComponent<CardForComboUnitSkill>().body;
		
		// camera pos
		//GameObjectUtil.copyTarnsformValue(cameraPos[6].gameObject,ActionCameraControl.mInstance.cameraObj);
		// right cards pos
		ActionCameraControl.mInstance.setCardObjsPos(rightCardList,rightCardPos6);
		
		// left card pos
		List<GameObject> leftList = new List<GameObject>();
		leftList.Add(leftCardList[0]);
		ActionCameraControl.mInstance.setCardObjsPos(leftCardList,leftCardPos6,leftList);
		iTween.MoveTo(leftCardList[0],iTween.Hash("position",leftCardPos6[0].transform.position,"time",0.4f,"easetype",iTween.EaseType.easeInOutExpo));
		//needShakeCamera = true;
	}
	
	public void gc()
	{
		
	}
	
}
