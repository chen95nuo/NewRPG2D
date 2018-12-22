using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionCameraControl : MonoBehaviour 
{
	public static ActionCameraControl mInstance;
	public GameObject cameraObj = null;
	//float showingTime = 3.0f;
	GameObject _myGameObj;
	
	public GameObject skyBoxObj ;
	
	COMBOTYPE comboType = COMBOTYPE.E_Combo3;
	
	//float stateTime = 0.0f;
	//float stateRuningTime = 0.0f;
	
	bool finish = false;
		
	public Camera mainCamera; 
	
	public float curTime;
	
	bool isDebug = false;
	 
	public UnitSkillCombo3 usc3;
	public UnitSkillCombo6 usc6;
	public UnitSkillCombo3Oppsite usc3o;
	public UnitSkillCombo6Oppsite usc6o;
	
	public enum COMBOTYPE
	{
		E_Combo3 = 0,
		E_Combo6 = 1,
		E_Combo3_Oppsite = 2,
		E_Combo6_Oppsite = 3,
	}
	
	void Awake()
	{
		mInstance = this;
		_myGameObj = this.gameObject;
		if(!isDebug)
		{
			hide ();
		}
		
		
	}
	
	// Use this for initialization
	void Start ()
	{
		
		if(isDebug)
		{
			List<int> l = new List<int>();
			l.Add(46001);
			l.Add(46001);
			l.Add(46001);
			l.Add(36003);
			l.Add(46001);
			l.Add(46001);
			List<int> r = new List<int>();
			r.Add(46001);
			r.Add(45003);
			r.Add(45005);
			r.Add(34002);
			r.Add(44001);
			r.Add(44004);
			showComboAction(r,l,COMBOTYPE.E_Combo6);
			//showComboAction(l,r,COMBOTYPE.E_Combo6_Oppsite);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		curTime += Time.deltaTime;
	}
	
	public void showComboAction(List<int> leftCardIDList,List<int> rightCardIDList,COMBOTYPE cType)
	{
		finish = false;
		curTime = 0;
		if(mainCamera == null)
		{
			GameObject mainCameraObj =  GameObject.Find("MainCameraParent/Main Camera");
			if(mainCameraObj != null)
			{
				mainCamera = mainCameraObj.GetComponent<Camera>();
				mainCamera.enabled = false;
			}
		}
		_myGameObj.SetActive(true);
		cameraObj.SetActive(true);
		if(!isDebug)
		{
			if(skyBoxObj == null)
			{
				// TODO
				skyBoxObj = PVESceneControl.mInstance.curSkyBox;
				if(skyBoxObj != null)
				{
					//skyBoxObj.SetActive(false);
				}
			}
			
		}
		
		comboType = cType;
		Debug.Log("comboType === " + cType.ToString());
		switch(comboType)
		{
		case COMBOTYPE.E_Combo3:
		{
			usc3.runShow(leftCardIDList,rightCardIDList);
		}break;
		case COMBOTYPE.E_Combo6:
		{
			usc6.runShow(leftCardIDList,rightCardIDList);
		}break;
		case COMBOTYPE.E_Combo3_Oppsite:
		{
			usc3o.runShow(leftCardIDList,rightCardIDList);
		}break;
		case COMBOTYPE.E_Combo6_Oppsite:
		{
			usc6o.runShow(leftCardIDList,rightCardIDList);
		}break;
		}
		GameObjectUtil.showDarkFog();
	}
	
	public bool isVisible()
	{
		return _myGameObj.activeSelf;
	}
	
	public void hide()
	{
		curTime = 0;
		if(mainCamera != null)
		{
			mainCamera.enabled = true;
		}
		if(skyBoxObj != null)
		{
			skyBoxObj.SetActive(true);
		}
		
		cameraObj.SetActive(false);
		_myGameObj.SetActive(false);
		if(PVESceneControl.mInstance != null)
		{
			PVESceneControl.mInstance.showFog();
		}
		
	}

	public bool isFinish()
	{
		return finish;
	}
	
	public void setCardObjsPos(List<GameObject> cardObjs,GameObject[] posObjs,List<GameObject> doNotSetPosList = null)
	{
		for(int i = 0;i < cardObjs.Count;++i)
		{
			GameObject cardObj = cardObjs[i];
			if(doNotSetPosList != null && doNotSetPosList.Contains(cardObj))
			{
				continue;
			}
			if(cardObj != null)
			{
				GameObject posObj = posObjs[i];
				if(posObj != null)
				{
					GameObjectUtil.copyTarnsformValue(posObj,cardObj);
				}
			}
		}
	}
	
	public  List<Vector3> getPathNodeList(Transform[] nodeList)
	{
		List<Vector3> list = new List<Vector3>();
		for(int i = 0; i < nodeList.Length;++i)
		{
			Vector3 pos = nodeList[i].transform.position;
			list.Add(pos);
		}
		return list;
	}
	
	public void playMovePath(Transform[] pathNodeList,float time)
	{
		iTweenPath iPath = cameraObj.GetComponent<iTweenPath>();
		iPath.nodes = getPathNodeList(pathNodeList);
		cameraObj.transform.localPosition = iPath.nodes[0];
		iTween.MoveTo(cameraObj,iTween.Hash("path",iTweenPath.GetPath(iPath.pathName),"time",time,"easetype",iTween.EaseType.linear));
	}
	
	public void lookAtGameObject(GameObject targetObj)
	{
		cameraObj.transform.LookAt(targetObj.transform);
	}
	
	public void finishShow()
	{
		finish = true;
		if(ViewControl.mInstacne != null)
		{
			ViewControl.mInstacne.startWaitNormalCamareMove();
		}
		hide();
	}
	
	public COMBOTYPE getComboType()
	{
		return comboType;
	}
	
	public void gc()
	{
	}
}
