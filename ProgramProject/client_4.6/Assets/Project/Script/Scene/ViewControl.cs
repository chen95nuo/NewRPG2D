using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class ViewControl : MonoBehaviour {
	
	public static ViewControl mInstacne;
	
	private Vector3 initPosition;
	private Quaternion initRotation;
	private Vector3 initEulerAngles;
	
	public GameObject cameraObject;
	public GameObject lookAtGameObject;
	private bool canLookAt;
	
	public GameObject battleLookAtCenterObj;

	public float xSpeed = 1;
	public float xBegin = 190, xEnd = 226;
	
	public float x = 0;
	float distance = 36.7f;
	public iTween.EaseType type = iTween.EaseType.easeInOutQuad;
	public enum CameraShowState : int
	{
		E_ShowActionCard = 0,
		E_ShowTargetCard = 1,
		E_Finish = 2,
	}
	int cShowState;
	public enum CameraShowType
	{
		E_Null = -1,
		E_One = 1,
		E_Mul = 2,
		E_CST3 = 3,
		
	}
	CameraShowType cShowType;
	Vector3 lastCamarePos;
	Quaternion lastRotation;
	float lastTimeScale;
	float lastFov ;
	bool runCameraShow;
	float cShowStateTotalTime = 0;
	float cShowStateTime = 0;
	Card actionCard;
	List<Card> actionTargets;
	List<Card> actionTanks;
	public GameObject actionCardStartPos;
	public GameObject actionCardAtkOneEndPos;
	public GameObject targetOneCardPos;
	public GameObject tankStartPos;
	public GameObject tankEndPos;
	private PVESceneControl pveControl;
	
	public GameObject cPosOne;
	public GameObject cPosMul;
	public GameObject cPosShow3;
	
	bool playWinCameraEffect = false;
	Vector3 winCameraEffectLookAtPos = Vector3.zero;
	
	Vector3 startFightCameraEffectLookAtPos = Vector3.zero;
	bool playFightCameraEffect = false;
	
	Vector3 battleCameraStartPos = Vector3.zero;
	
	//打击感添加，修改摄像机位置//
	float yOff = 0.5f;
	float hitTime = 0.02f;
	float hitFrameCount = 0;
	bool isStartCount = false;
	Vector3 startPos;
	Vector3 endPos;
	
	
	bool waitCMove = false;
	float waitTotalTime = 0f;
	float sTime = 0.0f;
	bool runCMove = false;
	
	bool startWaitAutoCMove = false;
	float waitAutoCMoveTotalTime = 0;
	float sTimeWaitAuto = 0;
	
	
	void Awake()
	{
		mInstacne = this;
		
		battleCameraStartPos = cameraObject.transform.position;
		cameraObject.transform.LookAt(battleLookAtCenterObj.transform.position );
		pveControl = cameraObject.GetComponent<PVESceneControl>();
	}
	
	// Use this for initialization
	void Start () 
	{
		initPosition=cameraObject.transform.position;
		initRotation=cameraObject.transform.rotation;
		
		initEulerAngles = cameraObject.transform.eulerAngles;
		x = initEulerAngles.y;
		cameraMove(x);

	}
	
	// Update is called once per frame
	void Update () {
		
		if(canLookAt)
		{
			Transform transform = lookAtGameObject.transform.FindChild("body");
			if(transform == null)
			{
				cameraObject.transform.LookAt(lookAtGameObject.transform.position);
			}
			else
			{
				cameraObject.transform.LookAt(lookAtGameObject.transform.FindChild("body").position);
			}
			
		}
		if(playWinCameraEffect)
		{
			cameraObject.transform.LookAt(winCameraEffectLookAtPos);
			return;
		}
		if(playFightCameraEffect)
		{
			cameraObject.transform.LookAt(startFightCameraEffectLookAtPos);
			return;
		}
		if(Input.GetKey("a"))
		{
			move("path1",3f);
		}
		if(Input.GetKey("s"))
		{
			move("path2",3f);
		}
		if(Input.GetKey("d"))
		{
			reset();
		}
		//把镜头路径写入文件//
		if(Input.GetKey("w"))
		{
			iTweenPath[] itps=gameObject.GetComponents<iTweenPath>();
			int maxLength=0;
			foreach(iTweenPath itp in itps)
			{
				if(itp.nodes.Count>maxLength)
				{
					maxLength=itp.nodes.Count;
				}
			}
			int colNum=maxLength*3+2;
			int rowNum=itps.Length+1;
			ViewPathLog.getInstance().writeHead(colNum,rowNum);
			for(int i=0;i<itps.Length;i++)
			{
				iTweenPath itp=itps[i];
				ViewPathLog.getInstance().writeUTF(itp.pathName);
				ViewPathLog.getInstance().writeUTF(itp.nodes.Count+"");
				for(int j=0;j<itp.nodes.Count;j++)
				{
					ViewPathLog.getInstance().writeUTF(itp.nodes[j].x+"");
					ViewPathLog.getInstance().writeUTF(itp.nodes[j].y+"");
					ViewPathLog.getInstance().writeUTF(itp.nodes[j].z+"");
				}
				if(itp.nodes.Count*3+2<colNum)
				{
					for(int k=itp.nodes.Count*3+2;k<colNum;k++)
					{
						ViewPathLog.getInstance().writeUTF("");
					}
				}
			}
			ViewPathLog.getInstance().close();
		}

		if(runCameraShow)
		{
			if(cShowStateTime >= cShowStateTotalTime)
			{
				++cShowState;
				cShowStateTime = 0;
				runCameraShowState();
			}
			else
			{
				cShowStateTime += Time.deltaTime;
			}
		}
		
//		if( runCameraShow || (pveControl!= null && pveControl.isCastingUnitSkill()) || (NewBattleUnitePanel.mInstance != null && NewBattleUnitePanel.mInstance.isVisible()))
//			return;
	
//#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_OSX
//		touchControl();
//#else
//		mouseControl();
//#endif
		
		
		//打击感计时//
		if(isStartCount)
		{
			if(hitFrameCount >= hitTime)
			{
				hitFrameCount = 0;
				isStartCount = false;
				gameObject.transform.position = startPos;
				
			}
			hitFrameCount+=Time.deltaTime;
			
		}
		
		// 等待战斗场景中摄像机来回自动移动的开始 //
		if(runCMove)
		{
			if(waitCMove)
			{
				if(sTime < waitTotalTime)
				{
					sTime += Time.deltaTime;
				}
				else
				{
					runNormalCamareMove();
				}
			}
			
			if(startWaitAutoCMove)
			{
				if(sTimeWaitAuto < waitAutoCMoveTotalTime)
				{
					sTimeWaitAuto += Time.deltaTime;
				}
				else
				{
					startWaitNormalCamareMove();
				}
			}
		}
		
		
	}
	
	public void mouseControl()
	{
		if(Input.GetMouseButtonDown(0))
		{
			
		}
		else if(Input.GetMouseButton(0))
		{
			float xDelta = Input.GetAxis("Mouse X");
            x += xDelta * xSpeed ;
			x = Mathf.Clamp(x, xBegin,xEnd);
            cameraMove(x);
		}
	}
	
	public void doCameraShow(Card card,List<Card> targets,List<Card> tanks,ViewControl.CameraShowType cst)
	{
		if(isRunNormalCamareMove())
		{
			stopNormalCamareMove();
		}
		cShowType = cst;
		lastCamarePos = cameraObject.transform.position;
		lastRotation = cameraObject.transform.rotation;
		lastFov = cameraObject.GetComponent<Camera>().fieldOfView;
		cameraObject.GetComponent<Camera>().fieldOfView = 50;
		lastTimeScale = Time.timeScale;
		runCameraShow = true;
		actionCard = card;
		actionTargets = targets;
		actionTanks = tanks;
		SkillData skill = actionCard.getSkill();
		if(skill == null)
			return;
		if(skill.atkTarget == 0)
		{
			if(targets.Count > 0)
			{
				Card targetCard = (Card)targets[0];
				targetCard.isCritAttack = true;
				targetCard._myTransform.position = targetOneCardPos.transform.position;
				if(tanks.Count > 0)
				{
					Card tankCard = (Card)tanks[0];
					if(tankCard != targetCard)
					{
						tankCard.isCritAttack = true;
						tankCard._myTransform.position = tankStartPos.transform.position;
					}
				}
			}
			else
			{
				if(tanks.Count > 0)
				{
					Card tankCard = (Card)tanks[0];
					tankCard.isCritAttack = true;
					tankCard._myTransform.position = targetOneCardPos.transform.position;
				}
			}
			
			
		}
		actionCard._myTransform.position = actionCardStartPos.transform.position;
		actionCard.isCritAttack = true;
		
		
		cShowState = (int)CameraShowState.E_ShowActionCard;
		lookAtGameObject = actionCard.gameObject;
		cShowStateTime = 0;
		runCameraShowState();
	}
	
	public void runCameraShowState()
	{
		switch(cShowState)
		{
		case (int)CameraShowState.E_ShowActionCard:
		{
			//GameObjectUtil.AddSpeed(0.5f);
//			GameObjectUtil.AddSpeed(1);
			//UIInterfaceManager.mInstance.doSpeedChange(1);
			cShowStateTotalTime = actionCard.getSkill().atkChargeENDTime - 0.05f;
			ViewPathData vpd = ViewPathData.getData("path9");
			iTweenPath iPath = cameraObject.gameObject.GetComponent<iTweenPath>();
			iPath.nodes = vpd.nodes;
			cameraObject.transform.position = vpd.nodes[0];
			iTween.MoveTo(cameraObject,iTween.Hash("path",iTweenPath.GetPath(iPath.pathName),"time",actionCard.getSkill().atkChargeENDTime - 0.1f,"easetype",iTween.EaseType.linear));
			canLookAt = true;
			
		}break;
		case (int)CameraShowState.E_ShowTargetCard:
		{
			canLookAt = false;
			//cShowType = CameraShowType.E_CST3;
			if(cShowType == CameraShowType.E_One)
			{
				if(actionCard.getSkill().position == 0)
				{
					GameObjectUtil.copyTarnsformValue(cPosMul,cameraObject);
				}
				else
				{
					GameObjectUtil.copyTarnsformValue(cPosOne,cameraObject);
				}
			}
			else if(cShowType == CameraShowType.E_Mul)
			{
				GameObjectUtil.copyTarnsformValue(cPosMul,cameraObject);
			}
			else if(cShowType == CameraShowType.E_CST3)
			{
				GameObjectUtil.copyTarnsformValue(cPosShow3,cameraObject);
			}
			
			cShowStateTotalTime = actionCard.getSkill().spawnENDTime + actionCard.getSkill().hurtENDTime;
		}break;
		case (int)CameraShowState.E_Finish:
		{
			finishCameraShow();
		}break;
		}
	}
	
	public void finishCameraShow()
	{
		actionCard.resetPos();
		for(int i = 0; i  < actionTargets.Count ; ++i ) 
		{
			Card targetC = (Card)actionTargets[i];
			targetC.resetPos();
		}
		for(int i = 0 ; i < actionTanks.Count ; ++i )
		{
			Card tankC = (Card)actionTanks[i];
			tankC.resetPos();
		}
		cShowStateTotalTime = 0;
		cShowStateTime = 0;
		cameraObject.transform.position = lastCamarePos;
	 	cameraObject.transform.rotation = lastRotation;
		cameraObject.GetComponent<Camera>().fieldOfView = lastFov;
//		GameObjectUtil.AddSpeed(lastTimeScale);
		UIInterfaceManager.mInstance.doSpeedChange(lastTimeScale);
		canLookAt = false;
		runCameraShow = false;
		pveControl.showCardsForFinishCritCameraShow();
		
		startWaitNormalCamareMove();
	}
	
	public bool getIsRunCameraShow()
	{
		return runCameraShow;
	}
	
	public void touchControl()
	{
        if (Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			float xDelta = touchDeltaPosition.x;
			x += xDelta * xSpeed * 0.5f;
			x = Mathf.Clamp(x, xBegin,xEnd);
			cameraMove(x);
		}
	}
	
	public void cameraMove(float x)
	{
		Quaternion rotation = Quaternion.Euler(initEulerAngles.x, x,initEulerAngles.y);

        Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * disVector + battleLookAtCenterObj.transform.position;
        position = transform.InverseTransformPoint(position);
		position = position.normalized * distance;
		position = transform.TransformPoint(position);

        cameraObject.transform.position = position;
        cameraObject.transform.LookAt(battleLookAtCenterObj.transform.position);
	}
	
	public void reset()
	{
		x = initEulerAngles.y;
		resetLookAt();
		cameraObject.transform.position=initPosition;
		cameraObject.transform.rotation=initRotation;
	}
	
	public void setLookAt(GameObject lookAtGameObject)
	{
		this.lookAtGameObject=lookAtGameObject;
		canLookAt=true;
	}
	
	public void resetLookAt()
	{
		canLookAt=false;
		lookAtGameObject=null;
	}
	
	public void setRotation(Vector3 rotation)
	{
		cameraObject.transform.eulerAngles=rotation;
	}
	
	public void move(string pathName,float time)
	{
		iTween.MoveTo(cameraObject,iTween.Hash("path",iTweenPath.GetPath(pathName),"time",time,"easetype",iTween.EaseType.linear));
	}
	
	public void move(string pathName,Vector3 lookTarget,float time)
	{
		iTween.MoveTo(cameraObject,iTween.Hash("path",iTweenPath.GetPath(pathName),"time",time,"easetype",type,"Looktarget",lookTarget));
	}
	
	public void playWinCameraPath()
	{
		//隐藏interface界面//
		UIInterfaceManager.mInstance.hide();
		//删除血条//
		GameObjectUtil.destroyGameObjectAllChildrens(PVESceneControl.mInstance.BloodParent);
		//删除名称//
		GameObjectUtil.destroyGameObjectAllChildrens(PVESceneControl.mInstance.hitNumParent);
		CameraData cameraData=CameraData.getData(STATE.WIN_CAMERA_DATA_ID);
		if(cameraData == null)
		{
			return;
		}
		iTweenPath iPath = cameraObject.GetComponent<iTweenPath>();
		ViewPathData viewData = ViewPathData.getData(cameraData.path);
		iPath.nodes = viewData.nodes;
		winCameraEffectLookAtPos=cameraData.lookAt;
		cameraObject.transform.localPosition = iPath.nodes[0];
		iTween.MoveTo(cameraObject,iTween.Hash("path",iTweenPath.GetPath(iPath.pathName),"time",cameraData.pathTime,"easetype",iTween.EaseType.linear));
		playWinCameraEffect = true;
	}
	
	
	public void playStartFightCameraPath()
	{
		CameraData cameraData=CameraData.getData(STATE.START_FIGHT_CAMERA_DATA_ID);
		if(cameraData == null)
		{
			pveControl.finishStartFightCameraShow = true;
			return;
		}
		iTweenPath iPath = cameraObject.GetComponent<iTweenPath>();
		ViewPathData viewData = ViewPathData.getData(cameraData.path);
		if(viewData == null)
		{
			pveControl.finishStartFightCameraShow = true;
			return;
		}
		iPath.nodes = viewData.nodes;
		startFightCameraEffectLookAtPos=cameraData.lookAt;
		cameraObject.transform.localPosition = iPath.nodes[0];
		iTween.MoveTo(cameraObject,iTween.Hash("path",iTweenPath.GetPath(iPath.pathName),"time",cameraData.pathTime,"easetype",iTween.EaseType.linear));
		playFightCameraEffect = true;
		Invoke("finishStartFightCameraShow",cameraData.pathTime);
	}
	
	public void finishStartFightCameraShow()
	{
		pveControl.finishStartFightCameraShow = true;
		pveControl.waitStartFightUIEffect = true;
		playFightCameraEffect = false;
		cameraObject.transform.position = battleCameraStartPos;
		cameraObject.transform.LookAt(battleLookAtCenterObj.transform.position );
	}
	
	//修改屏幕位置//
	public void ChangeCameraPos()
	{
		if(runCameraShow)
			return;
		startPos = gameObject.transform.position;
		endPos = new Vector3(startPos.x, startPos.y + yOff, startPos.z);
		isStartCount = true;
		hitFrameCount = 0;
		gameObject.transform.position = endPos;
	}
	
	public void setWaitNormalTime(float t)
	{
		waitTotalTime = t;
	}
	
	public void startWaitNormalCamareMove()
	{
		runCMove = true;
		waitCMove = true;
		sTime = 0;
		//UnityEngine.Random.Range(2.0f,5.5f);
	}
	
	
	public void stopNormalCamareMove()
	{
		runCMove = false;
		startWaitAutoCMove = false;
		waitCMove = false;
		sTime = 0;
		Animation anim = cameraObject.GetComponent<Animation>();
		if(anim != null)
		{
			anim.Stop();
		}
		reset();
	}
	
	public void runNormalCamareMove()
	{
		waitTotalTime = 0;
		waitCMove = false;
		sTime = 0;
		Animation anim = cameraObject.GetComponent<Animation>();
		if(anim != null && !anim.IsPlaying("BattleCamareMove0"))
		{
			anim["BattleCamareMove0"].wrapMode = WrapMode.Once;
			anim.Play("BattleCamareMove0");
			float t = anim["BattleCamareMove0"].length + 0.2f;
			waitAutoCMoveTotalTime = t;
			sTimeWaitAuto = 0;
			startWaitAutoCMove = true;
		}
	}
	
	public bool isRunNormalCamareMove()
	{
		return runCMove;
	}

	public void gc()
	{
		actionCard=null;
		if(actionTargets!=null)
		{
			actionTargets.Clear();
			actionTargets=null;
		}
		if(actionTanks!=null)
		{
			actionTanks.Clear();
			actionTanks=null;
		}
		pveControl=null;
	}
	
}
