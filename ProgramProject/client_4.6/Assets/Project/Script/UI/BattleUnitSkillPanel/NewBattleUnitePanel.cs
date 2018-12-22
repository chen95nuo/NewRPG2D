using UnityEngine;
using System.Collections;

public class NewBattleUnitePanel : BWUIPanel {
	
	public static NewBattleUnitePanel mInstance;
	
	public GameObject Line_Up;
	public GameObject Line_Down;
	public GameObject[] Pos_Up;				//标识line所在的位置，顺序是左中右//
	public GameObject[] Pos_Down;			//标识line所在的位置，顺序是左中右//
	public UISprite NameIcon;
	
	//1 合体技，2 bonus//
	private int curShowType;
	private int curShowSkillId;
	//1开始移动-中间，2中间-结束移动， 3 结束//
	private int lineMoveStep;
	
	TweenPosition UpLineTween;
	TweenPosition DownLineTween;
	TweenScale NameIconTween;
	
	float lineLeftPosX = -1000f;
	float lineRightPosX = 1000f;
	float lineUpPosY = 50f;
	float lineDownPosY = -50f;
	float lineUpPosMidX = 125f;
	float lineDownPosMidX = -125f;
	float lineMoveTime = 0.5f;
	float lineStayTime = 1.5f;
	float nameScaleTime = 1f;
	float frameCount;
	bool isStartFrameCount;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
	}
	
	public override void init ()
	{
		base.init ();
		if(UpLineTween == null && Line_Up!= null)
		{
			UpLineTween = Line_Up.GetComponent<TweenPosition>();
		}
		if(DownLineTween == null && Line_Down != null)
		{
			DownLineTween = Line_Down.GetComponent<TweenPosition>();
		}
		if(NameIconTween == null && NameIcon != null)
		{
			NameIconTween = NameIcon.GetComponent<TweenScale>();
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void initData()
	{
		
		frameCount = 0;
		isStartFrameCount = false;
		lineMoveStep = 0;
		StartShow();
	}
	
	public void StartShow()
	{
		if(curShowType == 2)
		{
			string name = "max-bonus";
			if(NameIcon != null)
			{
				NameIcon.spriteName = name;
			}
		}
		else if(curShowType == 1)
		{
			UnitSkillData usd = UnitSkillData.getData(curShowSkillId);
			if(NameIcon != null)
			{
				NameIcon.spriteName = usd.nameicon;
			}
		}
		NameIcon.gameObject.SetActive(false);
		//开始播线移动,上面的线从右向左，下面的线从左向右//
		lineMoveStep = 1;
		Vector3 fromAnim = new Vector3(lineRightPosX, lineUpPosY, 0);
		Vector3 toAnim = new Vector3(lineUpPosMidX, lineUpPosY, 0);
		float time = lineMoveTime;
		PlayTweenAnim(fromAnim, toAnim, time, UpLineTween, false, "");
		
		
		fromAnim = new Vector3(lineLeftPosX, lineDownPosY, 0);
		toAnim = new Vector3(lineDownPosMidX, lineDownPosY, 0);
		time = lineMoveTime;
		PlayTweenAnim(fromAnim, toAnim, time, DownLineTween, false, "LineMoveCallBack");
		
	}
	
	//播tweener动画//
	public void PlayTweenAnim(Vector3 fromAnim, Vector3 toAnim, float time, TweenPosition tweener, bool isCallBack, string callBackName)
	{
		tweener.from = fromAnim;
		tweener.to = toAnim;
		tweener.duration = time;
		if(isCallBack )
		{
			if(tweener.onFinished!=null && tweener.onFinished.Count>0)
			{
				tweener.onFinished[0].methodName = callBackName;
			}
		}
		GameObjectUtil.playForwardUITweener(tweener);
	}
	
	public void LineMoveCallBack()
	{
		lineMoveStep ++;
		isStartFrameCount = true;
		if(lineMoveStep == 2)		//播放名字动画//
		{
			//UpLineTween.tweenFactor = 0;
			//DownLineTween.tweenFactor = 0;
			NameIcon.gameObject.SetActive(true);
			NameIconTween.from = Vector3.zero;
			NameIconTween.to = Vector3.one;
			NameIconTween.duration = nameScaleTime;
			GameObjectUtil.playForwardUITweener(NameIconTween);
			//==播个特效==//
			if(curShowType==1)
			{
				UnitSkillData usd=UnitSkillData.getData(curShowSkillId);
				GameObject nameEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/"+usd.nameeffect,1)) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(nameEffect,_MyObj);
				GameObjectUtil.setGameObjectLayer(nameEffect,_MyObj.layer);
				Destroy(nameEffect,2f);
			}
			else if(curShowType==2)
			{
				GameObject nameEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/BattleUnite_htzj",1)) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(nameEffect,_MyObj);
				GameObjectUtil.setGameObjectLayer(nameEffect,_MyObj.layer);
				Destroy(nameEffect,2f);
			}
		}
		else if(lineMoveStep == 3)	//线移动结束，关闭该界面，并显示其他界面//
		{
			//UpLineTween.tweenFactor = 0;
			//DownLineTween.tweenFactor = 0;
			if(curShowType == 1)
			{
				
			}
			else if(curShowType == 2)		//bonus界面，开始倒计时//
			{
				BattleBounesPanel.mInstance.runBonuce();
			}
			hide ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isStartFrameCount)
		{
			if(lineMoveStep == 2)		//到达中间位置//
			{
				frameCount+=Time.deltaTime;
				if(frameCount >= lineStayTime)		//线继续移动//
				{
					Vector3 fromAnim = new Vector3(lineUpPosMidX, lineUpPosY, 0);
					Vector3 toAnim = new Vector3(lineLeftPosX, lineUpPosY, 0);
					float time = lineMoveTime;
					PlayTweenAnim(fromAnim, toAnim, time, UpLineTween, false, "");
					
					
					fromAnim = new Vector3(lineDownPosMidX, lineDownPosY, 0);
					toAnim = new Vector3(lineRightPosX, lineDownPosY, 0);
					time = lineMoveTime;
					PlayTweenAnim(fromAnim, toAnim, time, DownLineTween, false, "LineMoveCallBack");
					frameCount = 0;
					isStartFrameCount = false;
				}
			}
		}
		 
	}
	
	public override void show ()
	{
		base.show ();
		initData();
	}
	
	//type 1 表示合体技，2 表示bonus, formId, 合体技在表中的id, 当type = 2时，formId没用//
	public void SetData(int type, int formId)
	{
		if(ViewControl.mInstacne.isRunNormalCamareMove())
		{
			ViewControl.mInstacne.stopNormalCamareMove();
		}
		CleanData();
		curShowType = type;
		curShowSkillId = formId;
		show ();
	}
	
	
	
	public override void hide ()
	{
		base.hide ();
		CleanData();
	}
	
	public void CleanData()
	{
		curShowType = -1;
		curShowSkillId = -1;
		lineMoveStep = 0;
		isStartFrameCount = false;
		frameCount = 0;
		if(NameIconTween != null)
		{
			
			NameIconTween.tweenFactor = 0;
		}
		if(UpLineTween != null)
		{
			UpLineTween.tweenFactor = 0;
		}
		
		if(DownLineTween != null)
		{
			DownLineTween.tweenFactor = 0;
		}
	}
	
	public void gc()
	{
		GameObject.Destroy(_MyObj);		
		mInstance = null;
		_MyObj = null;
		Resources.UnloadUnusedAssets();
	}
	
}
