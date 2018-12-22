using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSkill
{
	/// <summary>
	/// 释放合体技的一方,0左边,1右边
	/// </summary>
	private int team;
	/// <summary>
	/// 合体技静态数据
	/// </summary>
	public UnitSkillData unitSkill;
	/**是否为援护技**/
	private bool helpSkill;
	
	private int maxStage;
	private List<Card> targetCards;
	List<Card> unitSkillShowCardList;
	public int stage;
	private float stageTime;
	private float preTime;
	private Camera mainCamera;
	private ViewControl view;
	private iTweenPath itPath;
	
	public GameObject callUpObject;
	public ArrayList uniteEffectList;
	public ArrayList uniteDelayEffectList;
	
	public float frameCount = 0;
	public bool isCanCountFrame = false;
	
	GameObject cameraParent;
	
	private PVESceneControl timer;
	
	public UnitSkill(int team,UnitSkillData unitSkill,PVESceneControl timer,bool helpSkill)
	{
		this.team=team;
		this.unitSkill=unitSkill;
		this.timer=timer;
		this.helpSkill=helpSkill;
	}
	
	public bool isHelpSkill()
	{
		return helpSkill;
	}
	
	public void init(List<Card> targetCards,int maxStage,Player srcPlayer,Player targetPlayer,List<Card> unitSkillShowCards)
	{
		this.targetCards=targetCards;
		unitSkillShowCardList = unitSkillShowCards;
		if(unitSkillShowCardList != null)
		{
			for(int i= 0; i < unitSkillShowCardList.Count;++i)
			{
				unitSkillShowCardList[i].recoverShowObj();	
			}
		}
		
		
		//计算效果//
		switch(unitSkill.type)
		{
		case 1://攻击//
			Statics.calculateHurtForUnitSkill(timer.getRound(),targetCards,srcPlayer,targetPlayer,unitSkill.effect1,unitSkill.effect2);
			break;
		case 2://防护//
			break;
		case 3://回复//
			Statics.resumeForUnitSkill(timer.getRound(),targetCards,srcPlayer,targetPlayer,unitSkill.effect1,unitSkill.effect2);
			break;
		case 4://BUFF//
			break;
		case 5://复活//
			//设置指定百分比的血量//
			for(int i=0;targetCards!=null && i<targetCards.Count;i++)
			{
				Card c=(Card)targetCards[i];
				int recoverHp=c.getMaxHp()*unitSkill.effect1/100;
				Statics.recoverForUnitSkill(c,recoverHp);
			}
			break;
		case 6://献祭//
			break;
		case 7://变身//
			break;
		case 8://定身//
			break;
		}
		
		this.maxStage=maxStage;
		if(mainCamera == null)
		{
			mainCamera = Camera.main;
		}
		
		if(cameraParent == null){
			cameraParent = GameObject.Find("MainCameraParent");
			view = cameraParent.GetComponent<ViewControl>();
		}
		
		if(mainCamera != null){
			itPath = mainCamera.GetComponent<iTweenPath>();
		}
		
		
		if(uniteEffectList == null){
			uniteEffectList = new ArrayList();
		}
		if(uniteDelayEffectList==null)
		{
			uniteDelayEffectList=new ArrayList();
		}
	}
	
	public void nextStage()
	{
		stage++;
		view.resetLookAt();
		//Debug.Log("stage:"+stage+","+System.DateTime.Now.ToString("HH:mm:ss"));
		switch(stage)
		{
		case 1://镜头移动1(蓄力镜头): 播人物攻击动画 及特效//
		{
			MusicManager.playUniteSkillSoundEffect(unitSkill.index, 1);
			if(team==0)
			{
				stageTime=unitSkill.chargePWSETime;
			}
			else
			{
				stageTime=0;
			}
			preTime=timer.curTime;
			//移动镜头//
			if(team == 0)
			{
				runCamera(unitSkill.chargeCameraMove);
			}
			if(unitSkillShowCardList!=null)
			{
				foreach(Card c in unitSkillShowCardList)
				{
					c.UniteSkillName = unitSkill.chargeEffect;
					c.UniteKeepTime = unitSkill.chargeEffectTime;
					c.UniteSkillName = "phycharge01";
					c.setDirect(9);
				}
			}
		}break;
		case 2://镜头移动2(召唤兽出现镜头): 召唤兽出现//
		{
			//播音效//
			MusicManager.playUniteSkillSoundEffect(unitSkill.index, 0);
			stageTime= unitSkill.castPWSETime;
			preTime=timer.curTime;
			//创建召唤兽//
			createCallUp();
			
			//移动镜头//
			if(team == 0)
			{
				runCamera(unitSkill.castCameraMove);
			}
		}break;
		case 3://镜头移动3(召唤兽攻击镜头): 召唤兽攻击动作//
		{
			//移动镜头//
			stageTime=unitSkill.actionPWSETime;
			preTime=timer.curTime;
			if(team == 0)
			{
				runCamera(unitSkill.actionCameraMove);
			}
			
			
			if(unitSkill.castPostion == 2)
			{
				if(team == 0)
				{
					callUpObject.GetComponent<CallUp>().movePos(SceneInfo.getInstance().getSkyCallUpEndPosObj().transform.position,SceneInfo.getInstance().getEnemyCenter().transform.position,stageTime - 2.5f,iTween.EaseType.easeInCirc);
				}
				else
				{
					callUpObject.GetComponent<CallUp>().movePos(SceneInfo.getInstance().getEnemySkyCallUpEndPosObj().transform.position,SceneInfo.getInstance().getMyCenter().transform.position,stageTime - 2.5f,iTween.EaseType.easeInCirc);
				}
				
			}
			
			//添加特效//
			string effectName = unitSkill.actionSETime;
			float keepTime = unitSkill.actionEffectTime;
			int actionPositionType = unitSkill.actionPositionType;
			createUniteEffect(effectName,keepTime,unitSkill.actionPrepareTime,actionPositionType);
			//添加振屏//
			if(unitSkill.screenShake == 1)
			{
				isCanCountFrame = true;
			}
		}break;
		case 4://镜头移动4(移回初始位置): 播放特效//
		{
			playEnd();
		}break;
		}
		
	}
	
	public void playEnd()
	{
		stageTime=unitSkill.hurtPWSETime;
		preTime=timer.curTime;
		//合体技阶段在敌人受伤时显示血条//
		if(!timer.BloodParent.activeSelf)
		{
			timer.BloodParent.SetActive(true);
		}
		if(!timer.hitNumParent.activeSelf)
		{
			timer.hitNumParent.SetActive(true);
		}
		switch(unitSkill.type)
		{
		case 5://复活//
			for(int i=0;targetCards!=null && i<targetCards.Count;i++)
			{
				Card c=(Card)targetCards[i];
				c.setDirect(16);
				createUniteEffect(unitSkill.hurtEffect,unitSkill.hurtPWSETime,0,c.gameObject);
			}
			break;
		case 1:
		case 2:
		case 3:
		case 4:
		case 6:
		case 7:
		case 8:
			foreach(Card c in targetCards)
			{
				c.setDirect(1);
				createUniteEffect(unitSkill.hurtEffect,unitSkill.hurtPWSETime,0,c.gameObject);
			}
			break;
		}

	}
	
	public void clear()
	{
		if(targetCards!=null)
		{
			foreach(Card c in targetCards)
			{
				c.resetTargets();
			}
			targetCards=null;
		}
		unitSkill=null;
		stage = 0;
	}
	
	public bool canRunNextStage()
	{
		return timer.curTime-preTime>=stageTime;
	}
	
	public bool stageOver()
	{
		if(stage>maxStage)
		{
			if(targetCards!=null)
			{
				foreach(Card c in targetCards)
				{
					if(c.isRunning())
					{
						return false;
					}
				}
			}
			return true;
		}
		return false;
	}
	
	public int getTeam()
	{
		return team;
	}
	
	public int getOtherTeam()
	{
		if(team == 0)
		{
			return 1;
		}
		else
		{
			return 0;	
		}
	}
	
	public UnitSkillData getUnitSkill()
	{
		return unitSkill;
	}
	
	public void createCallUp()
	{
		string name = unitSkill.castEffect;
		if(name == string.Empty)
		{
			callUpObject =  GameObject.Instantiate(GameObjectUtil.LoadResourcesPrefabs("EmptyCard",0)) as GameObject;
			
			GameObject pos = null;
			if(team == 0)
			{
				pos= SceneInfo.getInstance().getMyCenter();
			}
			else
			{
				pos= SceneInfo.getInstance().getEnemyCenter();
			}
			callUpObject.transform.position = pos.transform.position;
			callUpObject.transform.rotation = pos.transform.rotation;
		}
		else
		{
			callUpObject =  GameObject.Instantiate(GameObjectUtil.LoadResourcesPrefabs(name,0)) as GameObject;
			CallUp callup = callUpObject.GetComponent<CallUp>();
			callup.scaleTime = 2;
			int createCallUpPos = unitSkill.castPostion;
			switch(createCallUpPos)
			{
			case 1:
			{
				callup.needScale = true;
				GameObject pos= null;
				if(team == 0)
				{
					pos= SceneInfo.getInstance().getMyCenter();
				}
				else
				{
					pos= SceneInfo.getInstance().getEnemyCenter();
				}
				callUpObject.transform.position = pos.transform.position;
				callUpObject.transform.rotation = pos.transform.rotation;
			}break;
			case 2:
			{
				callup.needScale = false;
				GameObject pos = null;
				if(team == 0)
				{
					pos = SceneInfo.getInstance().getSkyCallUpStartPosObj();
					callUpObject.transform.position = pos.transform.position;
					callUpObject.transform.rotation = pos.transform.rotation;
					callup.movePos(pos.transform.position,SceneInfo.getInstance().getSkyCallUpEndPosObj().transform.position,stageTime-0.05f);
					//createUniteEffect("phyatk_single_s03_polongji",stageTime + 0.5f,0,SceneInfo.getInstance().getSkyEffectPosObj());
				}
				else
				{
					pos = SceneInfo.getInstance().getEnemySkyCallUpStartPosObj();
					callUpObject.transform.position = pos.transform.position;
					callUpObject.transform.rotation = pos.transform.rotation;
					callup.movePos(pos.transform.position,SceneInfo.getInstance().getEnemySkyCallUpEndPosObj().transform.position,stageTime-0.05f);
				}
	
			}break;
			case 3:
			{
				callup.needScale = true;
				GameObject pos= null;
				if(team == 0)
				{
					pos= SceneInfo.getInstance().getEnemyCenter();
					callUpObject.transform.localEulerAngles = new Vector3(0,270,0);
				}
				else
				{
					pos= SceneInfo.getInstance().getMyCenter();
					callUpObject.transform.localEulerAngles = new Vector3(0,90,0);
				}
				
				callUpObject.transform.position = pos.transform.position;
			}break;
			}
		}
	}
	
	/// <summary>
	/// delayTime大于0时是延迟产生的特效,等于0是直接产生的特效
	/// </summary>
	/// <param name='effectName'>
	/// Effect name.
	/// </param>
	/// <param name='keepTime'>
	/// Keep time.
	/// </param>
	/// <param name='delayTime'>
	/// Delay time.
	/// </param>
	private void createUniteEffect(string effectName, float keepTime,float delayTime,int posType)
	{
		if(string.IsNullOrEmpty(effectName) || effectName.Trim().Equals("-1"))
		{
			return;
		}
		GameObject pos = null;
		/*1,我方中央脚下
		2,敌方中央脚下
		3,场中央脚下
		4,召唤兽脚下*/
		switch(posType)
		{
		case 1:
		{
			if(team == 0)
			{
				pos = SceneInfo.getInstance().getMyCenter(); 
			}
			else if(team == 1)
			{
				pos = SceneInfo.getInstance().getEnemyCenter();
			}
		}break;
		case 2:
		{
			if(team == 0)
			{
				pos = SceneInfo.getInstance().getEnemyCenter();
			}
			else if(team == 1)
			{
				pos = SceneInfo.getInstance().getMyCenter();
			}
		}break;
		case 3:
		{
			if(team == 0)
			{
				pos = SceneInfo.getInstance().getCenter1();
			}
			else if(team == 1)
			{
				pos = SceneInfo.getInstance().getCenter0();
			}
		}break;
		case 4:
		{
			if(callUpObject != null)
			{
				Transform footT = callUpObject.transform.FindChild("foot");
				if(footT)
				{
					pos = callUpObject.transform.FindChild("foot").gameObject;
				}
				else
				{
					pos = callUpObject;
				}
				
			}
		}break;
		}
		
		Vector3 position=pos.transform.position;
		Quaternion rotation=pos.transform.rotation;
		Effect effect=new Effect(effectName,keepTime,delayTime,position,position,rotation,timer.curTime,0,null,5);
		if(delayTime<=0)
		{
			uniteEffectList.Add(effect);
		}
		else
		{
			uniteDelayEffectList.Add(effect);
		}
	}
	
	private void createUniteEffect(string effectName, float keepTime,float delayTime,GameObject pos)
	{
		if(string.IsNullOrEmpty(effectName) || effectName.Trim().Equals("-1"))
		{
			return;
		}
		Vector3 position=pos.transform.position;
		Quaternion rotation=pos.transform.rotation;
		
		Effect effect=new Effect(effectName,keepTime,delayTime,position,position,rotation,timer.curTime);
		if(delayTime<=0)
		{
			uniteEffectList.Add(effect);
		}
		else
		{
			uniteDelayEffectList.Add(effect);
		}
		
	}
	
	/// <summary>
	/// 按照镜头ID移动.如果cameraInfo是%开头,那么主摄像机位置要先切到路径的第一个节点
	/// </summary>
	/// <param name='cameraId'>
	/// Camera identifier.
	/// </param>
	private void runCamera(string cameraInfo)
	{
		//判断是否有%//
		int cameraId=0;
		bool needSwitch=false;
		if(cameraInfo.StartsWith("%"))
		{
			needSwitch=true;
			cameraId=StringUtil.getInt(cameraInfo.Substring(1));
		}
		else
		{
			cameraId=StringUtil.getInt(cameraInfo);
		}
		CameraData cameraData=CameraData.getData(cameraId);
		if(cameraData == null)
			return;
		ViewPathData viewData = ViewPathData.getData(cameraData.path);
		if(viewData == null)
			return;
		itPath.nodes = viewData.nodes;
		//主摄像机位置要先切到路径的第一个节点//
		if(needSwitch)
		{
			mainCamera.transform.position = viewData.nodes[0];
		}
		//设置朝向,按路径移动//
		if(cameraData.haveLookAtPrefab())
		{
			view.setLookAt(callUpObject);
			view.move(itPath.pathName,cameraData.pathTime-0.1f);
		}
		else
		{
			if(cameraData.haveLookAtPos())
			{
				Vector3 lookAt=cameraData.lookAt;
				view.move(itPath.pathName,lookAt,cameraData.pathTime-0.1f);
			}
			else
			{
				view.setRotation(cameraData.rotation);
				view.move(itPath.pathName,cameraData.pathTime-0.1f);
			}
		}
	}
	
	//屏幕震动//
	public void ShakeScreen()
	{
		iTween.ShakePosition(cameraParent, iTween.Hash("x", 0.5f, "y", 0.5f, "z", 0.5f, "time", unitSkill.screenShakeLast + 0.5f));
	}
	
	public void setStage(int s)
	{
		stage = s;	
	}
	
	public void gc()
	{
		unitSkill=null;
		if(targetCards!=null)
		{
			targetCards.Clear();
			targetCards=null;
		}
		callUpObject=null;
		if(uniteEffectList!=null)
		{
			foreach(Effect e in uniteEffectList)
			{
				e.gc();
			}
			uniteEffectList.Clear();
			uniteEffectList=null;
		}
		if(uniteDelayEffectList!=null)
		{
			foreach(Effect e in uniteDelayEffectList)
			{
				e.gc();
			}
			uniteDelayEffectList.Clear();
			uniteDelayEffectList=null;
		}
		timer=null;
		//Resources.UnloadUnusedAssets();
	}
}
