using UnityEngine;
using System.Collections;

public class StartFightPanel : BWUIPanel
{
	public static StartFightPanel mInstance = null;
	// common****
	public GameObject commonCtrl;
	public GameObject commonLeft = null;
	public GameObject commonRight = null;
	//****
	
	// pvp****
	public GameObject pvpCtrl;
	
	public GameObject pvpLeft;
	public GameObject leftFristObj;
	public UILabel leftUpInfo;
	public UILabel leftDownInfo;
	
	public GameObject pvpRight;
	public GameObject rightFirstObj;
	public UILabel rightUpInfo;
	public UILabel rightDownInfo;
	
	public GameObject vsObj;
	//****
	
	bool isPVP = false;
	
	bool needWaitTime = false;
	float waitTime = 1f;
	float curWatiTime = 0;
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		hide ();
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(needWaitTime)
		{
			if(curWatiTime > waitTime)
			{
				waitTime = 0;
				needWaitTime = false;
				hide();
			}
			else
			{
				curWatiTime += Time.deltaTime;
			}
		}
	}
	
	public override void show()
	{
		base.show();
		if(isPVP)
		{
			GameObjectUtil.playForwardUITweener(pvpLeft.GetComponent<TweenPosition>());
			GameObjectUtil.playForwardUITweener(pvpRight.GetComponent<TweenPosition>());
		}
		else
		{
			setCommonInfo();
			GameObjectUtil.playForwardUITweener(commonLeft.GetComponent<TweenPosition>());
			GameObjectUtil.playForwardUITweener(commonRight.GetComponent<TweenPosition>());
		}
		
	}
	
	public override void hide()
	{
		Resources.UnloadUnusedAssets();
		base.hide();
	
	}
	
	public void setCommonInfo()
	{
		pvpCtrl.SetActive(false);
		commonCtrl.SetActive(true);
		isPVP = false;
	}
	
	public void setPVPInfo(string name0,int level0,int bp0,string name1,int level1,int bp1)
	{
		isPVP = true;
		pvpCtrl.SetActive(true);
		commonCtrl.SetActive(false);
		vsObj.SetActive(false);
		if(bp0 >= bp1)
		{
			leftFristObj.SetActive(true);
			rightFirstObj.SetActive(false);
		}
		else
		{
			leftFristObj.SetActive(false);
			rightFirstObj.SetActive(true);
		}
		
		leftUpInfo.text = name0 + " Lv." + level0.ToString();
		leftDownInfo.text = bp0.ToString();
		
		rightUpInfo.text = name1+ " Lv." + level1.ToString();
		rightDownInfo.text = bp1.ToString();
	}
	
	// common show finish
	public void finishShow()
	{
		needWaitTime = true;
		GameObject effect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/fight",1)) as GameObject;
		GameObjectUtil.gameObjectAttachToParent(effect,_MyObj);
		GameObjectUtil.setGameObjectLayer(effect,_MyObj.layer);
		Destroy(effect,2f);
	}
	
	public void finishPVPInfoMove()
	{
		vsObj.SetActive(true);
		GameObjectUtil.playForwardUITweener(vsObj.GetComponent<TweenScale>());
		GameObject effect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/vs",1)) as GameObject;
		GameObjectUtil.gameObjectAttachToParent(effect,_MyObj);
		GameObjectUtil.setGameObjectLayer(effect,_MyObj.layer);
		Destroy(effect,2f);
	}
	
	public void finishPVPShow()
	{
		needWaitTime = true;

	}
	
	public void gc()
	{
		GameObject.Destroy(_MyObj);		
		mInstance = null;
		_MyObj = null;
		//Resources.UnloadUnusedAssets();
	}
}
