using UnityEngine;
using System.Collections;

public class UIInterfaceManager : BWUIPanel {
	public PVESceneControl pveControl;
	//private Transform _myTransform;
	private float curScaleNum = STATE.SPEED_BATTLE_NORMAL;
	private float showTipFrame = 0;
	private int curRoundNum = 0;
	private UILabel speedTipLabel;
	private GameObject koFinishedEff;
	
	public static UIInterfaceManager mInstance;
	public UILabel scaleLabel;
	public UILabel RoundLabel;

    public UILabel hurt;

    public GameObject hurtControl;
	public GameObject x2;
	public GameObject x1;
	public GameObject UI_UpControl;
	public GameObject UI_DownControl;
	public GameObject AddSpeedTip;
	public GameObject addSpeedCtrl;
	public GameObject skipBtn;
	public GameObject KOShowTipObj;
//	public UILabel KOShowLabel;
	public GameObject KO_Finished;
	public GameObject KO_UnFinished;
	
	
	public GameObject demoBattleToBlack;
	int  curType = -1;

   
	void Awake(){
		
		//_myTransform = transform;
		mInstance = this;
		_MyObj = mInstance.gameObject;
	}
	
	public override void init ()
	{
		base.init ();
		AddSpeedTip.SetActive(false);
		speedTipLabel = AddSpeedTip.transform.FindChild("Label").GetComponent<UILabel>();
		string str = TextsData.getData(321).chinese;
		speedTipLabel.text = str;
		showTipFrame = 0;
		
	}
	
	// Use this for initialization
	void Start () {
		if(x2 != null && x1 != null){
			
//			scaleLabel.text = "x2" ;
			
			x1.SetActive(true);
			x2.SetActive(false);
		}

		if(UI_UpControl == null){
			UI_UpControl = _MyObj.transform.FindChild("UI_UpControl").gameObject;
		}
		if(UI_DownControl == null)
		{
			UI_UpControl = _MyObj.transform.FindChild("UI_DownControl").gameObject;
		}
		
		RoundLabel.gameObject.SetActive(false);
		//隐藏上下ui//
		UpContrl(false);
		DownContrl(false);
		KOShowTipObj.SetActive(false );
	}
	
	// Update is called once per frame
	void Update () {
		if(AddSpeedTip.activeSelf)
		{
			if(showTipFrame > 2)
			{
				showTipFrame = 0;
				AddSpeedTip.SetActive(false);
			}
			showTipFrame+=Time.deltaTime;
		}
	}
    int nums = 0;

    int hurts;
    public void SetHurtNum(int num)
    {
        if (isEventBattle)
        {
            hurts += num;
            hurt.gameObject.GetComponent<ChangeNumber>().SetData(hurt, nums, (nums += num), 1, -1, 0);
            hurt.gameObject.GetComponent<ChangeNumber>().StartChange();
            PVESceneControl.mInstance.hurt = hurts;

            PlayerInfo.getInstance().hurt = hurts;
        }
        else
            return;
    }
	
	//加速按钮//
	public void AddSpeedBtn(float num = -1)
	{
		if(ViewControl.mInstacne.getIsRunCameraShow())
			return;
		
		int unlockLevel = UnlockData.getData(33).method;
		if(PlayerInfo.getInstance().player.level < unlockLevel)
		{
			if(!AddSpeedTip.activeSelf)
			{
				
				AddSpeedTip.SetActive(true);
			}
		}
		else
		{
			if(num == 1)
			{
				num = STATE.SPEED_BATTLE_NORMAL;
			}
			else if(num == 2)
			{
				num = STATE.SPEED_BATTLE_2X;	
			}
			doSpeedChange(num);
			//记录玩家修改的数值//
			recodeScaleNum(num);
		}
	}
	
	public void recodeScaleNum(float num)
	{
		float scaleN = 0;
		if(num <= 0)
		{
			//scaleN += 1;
			//if(scaleN > 2){
			//	scaleN = 1;
			//}
			scaleN = STATE.SPEED_BATTLE_NORMAL;
		}
		else 
		{
			scaleN = num ;
		}
		PlayerInfo.getInstance().curTimeScale = scaleN;
	}
	
	public void doSpeedChange(float num = -1)
	{
		
		if(num <= 0)
		{
			curScaleNum = STATE.SPEED_BATTLE_NORMAL;
			//curScaleNum += 1;
			//if(curScaleNum > 2){
			//	curScaleNum = 1;
			//}
		}
		else 
		{
			curScaleNum = num ;
			
		}
		//在最开始按钮上显示的x2,点击后速度增加为2倍，然后按钮上显示的数字是1//
		GameObjectUtil.AddSpeed(curScaleNum);
		
		if(curScaleNum == STATE.SPEED_BATTLE_NORMAL || curScaleNum == STATE.SPEED_NORMAL){
			x1.SetActive(true);
			x2.SetActive(false);
		}
		else if(curScaleNum == STATE.SPEED_BATTLE_2X){
			
			x2.SetActive(true);
			x1.SetActive(false);
		}
	}
	
	
	public float getCurScale(){
		return curScaleNum;
	}




    bool isEventBattle;
	//修改回合数//
	public void ChangeRoundNum(int num,int needNum = 0)
	{
		if(curRoundNum != num)
		{
			curRoundNum = num;
			if(curRoundNum > 0)
			{	
				if(!RoundLabel.gameObject.activeSelf)
				{
					RoundLabel.gameObject.SetActive(true);
				}
				//隐藏上下ui//
				
				if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_DEMO)
				{
					addSpeedCtrl.SetActive(false);
				}
				if(!UI_UpControl.activeSelf)
				{
					UpContrl(true);
				}
				if(!UI_DownControl.activeSelf)
				{
					DownContrl(true);
					if(PlayerInfo.getInstance().curTimeScale > 0)
					{
						doSpeedChange(PlayerInfo.getInstance().curTimeScale);
					}
				}
                if (needNum != 0)
                {
                    isEventBattle = true;
                    hurtControl.SetActive(true);

                    RoundLabel.text = TextsData.getData(724).chinese + num + "/" + 5;
                }
                else
                    RoundLabel.text = TextsData.getData(317).chinese + num + TextsData.getData(318).chinese;
			}
			else 
			{
				RoundLabel.gameObject.SetActive(false);
				//隐藏上下ui//
				UpContrl(false);
				DownContrl(false);
			}
		}
	}
	
	public void ResetTimeScale(){
//		GameObjectUtil.AddSpeed(1);
		UIInterfaceManager.mInstance.doSpeedChange(STATE.SPEED_NORMAL);
	}
	
	public override void show ()
	{
		base.show ();
	}
	
	public override void hide ()
	{
		base.hide ();
	}
	
	public void UpContrl(bool isShow){
		UI_UpControl.SetActive(isShow);
	}
	
	public void DownContrl(bool isShow){
		UI_DownControl.SetActive(isShow);
	}
	
	public void showDemoBattleTopBlack()
	{
		demoBattleToBlack.SetActive(true);
		GameObjectUtil.playForwardUITweener(demoBattleToBlack.GetComponent<TweenAlpha>());
	}
	
	//curType 为-1时表示没有显示过这个界面，为0或者1时表示显示过了//
	public int GetKOTipType()
	{
		return curType;
	}
	
	//显示ko提示界面 type ko条件是否达成 0 未达成， 1 已达成//
	public void showKOTip(int type)
	{
		curType = type;
		KOShowTipObj.SetActive(true);
//		string str = "";
		if(type == 0)			//未达成//
		{
//			str = "ko Unfinished@=@";
			KO_UnFinished.SetActive(true);
			KO_Finished.SetActive(false);
			TweenScale bgScale = KO_UnFinished.transform.FindChild("Bg").GetComponent<TweenScale>();
			bgScale.enabled = true;
		}
		else if(type == 1)		//已达成//
		{
//			str = "ko finished ^_^";
			KO_UnFinished.SetActive(false);
			KO_Finished.SetActive(true);
			TweenScale bgScale = KO_Finished.transform.FindChild("Bg").GetComponent<TweenScale>();
			bgScale.enabled = true;
		}
//		KOShowLabel.text = str;
		
		
		
		Invoke("hideKoTip", 1);
	}
	
	public void hideKoTip()
	{
		KOShowTipObj.SetActive(false );
		PVESceneControl.mInstance.SetIsShowKOTip(false);
	}
	
	
	public void gc()
	{
		if(EnergyManager.mInstance!= null)
		{
			
			EnergyManager.mInstance.gc();
		}
		if(UniteBtnManager.mInstance!= null)
		{
			UniteBtnManager.mInstance.gc();
		}
		pveControl=null;
		GameObject.Destroy(_MyObj);		
		mInstance = null;
		_MyObj = null;
		//_myTransform = null;
	}
	
	public void onClickSkipBtn()
	{
		pveControl.skipFirstBattle();
	}
	
}
