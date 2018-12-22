using UnityEngine;
using System.Collections.Generic;
using System;

public class RuneUI : MonoBehaviour ,ProcessResponse{
	
//	public static RuneUI mInstance;
	
	
	public UILabel runeValue;
	public UILabel label1;
	public UILabel label2_1;
	public UILabel label2_2;
	public UILabel label2_3;
	public UILabel label2_4;
	public UILabel label3_1;
	public UILabel label3_2;
	public UILabel label3_3;
	public UILabel label3_4;
	public UILabel label4_1;

    public UILabel[] RuneLabels;
    public GameObject[] Groups;
    public GameObject[] Tages;

    string[] cuiTests = new string[4];

	public GameObject runeParentGo;
	public GameObject arrowLeft;
	public GameObject arrowRight;
	public GameObject btnLighten;
	public GameObject btnBack;
	public GameObject[] effectGos;
	
	public RuneResultJson rrj;
	
	private Color colorHigh;
	private Color colorLow;
	private float xOffset;
	
	//==当前符文Id==//
	private string curRuneId;
	
	private int times;
	private int[] pageNums;
	private int[] runeAdds;
	
	private GameObject[] runeGos;
	private bool canBack=true;
	//==当前符文图号,1--6==//
	private int curPage;
	private int prePage;
	//==点亮特效==//
	private GameObject lightEffect;
	private Vector3[] initPos=new Vector3[3];
	
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
	//private Transform _myTransform;
	
	public Transform runeTips;
	
	void Awake()
	{
//		_MyObj=gameObject;
//		mInstance=this;
		//_myTransform = transform;
		init();
	}
	
	public void init()
	{
		colorHigh=label1.color;
		colorLow=label3_4.color;
		
		for(int i=0;i<initPos.Length;i++)
		{
			effectGos[i].GetComponent<TweenPosition>().enabled=false;
			effectGos[i].GetComponent<TweenPosition>().duration=0.5f;
			initPos[i]=effectGos[i].transform.localPosition;
		}
		
		//==设置6个符文图的位置==//
		runeGos=new GameObject[6];
		for(int i=0;i<runeParentGo.transform.childCount;i++)
		{
			Transform tf=runeParentGo.transform.GetChild(i);
			tf.localPosition=new Vector3(0,0,0);
			tf.gameObject.SetActive(true);
			for(int k=0;k<tf.childCount;k++)
			{
				UISprite sprite=tf.GetChild(k).GetComponent<UISprite>();
				sprite.spriteName=null;
			}
			runeGos[i]=tf.gameObject;
		}
	}
	
	// Use this for initialization
	void Start () {
//		init();

//		close();

        Option(times - 1);
        if (curPage != 0)
        {
            Tages[curPage - 1].GetComponent<UIToggle>().value = true;
        }

        if (times != 0)
        {
            Groups[times-1].GetComponent<UIToggle>().value = true;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:
//				if(errorCode == 30)			//点亮符文失败//
//				{
//					ToastWindow.mInstance.showText(TextsData.getData(372).chinese);
//				}
				
				requestType=2;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PLAYER),this);
				break;
				
			case 2:
				HeadUI.mInstance.refreshPlayerInfo();
				draw();
				break;
			}

            if (cuiTests != null && times != 0)
            {
                RuneLabels[times - 1].text = cuiTests[times - 1];
            }
		}


    }
  
        
    public void Pages()
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);

        TweenAlpha ta = runeGos[prePage - 1].GetComponent<TweenAlpha>();
        ta.from = 1f;
        ta.to = 0;
        ta.tweenFactor = 0;
        ta.enabled = true;
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
        btnLighten.SetActive(false);
        canBack = false;
      
        if (curPage != 0)
        {
            Tages[curPage - 1].GetComponent<UIToggle>().value = true;
        }
        
    }

    public void Option(int param)
    {
        for (int t = 0; t < Groups.Length; t++)
        {
            if (t > param)
            {
                Groups[t].GetComponent<UIToggle>().enabled = false;
                Groups[t].transform.FindChild("Background").gameObject.SetActive(false);
            }
        }
    }
	public void onClickArrow(int direction)
	{
        


        ////播放音效//
        //MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);

        prePage = curPage;
        curPage = ((6 + curPage - 1) + direction) % 6 + 1;
        Pages();
        //TweenAlpha ta = runeGos[prePage - 1].GetComponent<TweenAlpha>();
        //ta.from = 1f;
        //ta.to = 0;
        //ta.tweenFactor = 0;
        //ta.enabled = true;
        
	}
	
	public void alphaOver()
	{
		runeGos[prePage-1].SetActive(false);
		runeGos[curPage-1].GetComponent<TweenAlpha>().alpha=1f;
		runeGos[curPage-1].SetActive(true);
		
		if(RuneData.getNextData(times,curPage,pageNums[curPage-1])==null)
		{
			btnLighten.SetActive(false);
		}
		else
		{
			btnLighten.SetActive(true);
		}
		drawLabels();
		drawRuneLight(false);
		drawArrows();
		canBack=true;
	}

	public void show()
	{
        //RefreshGroup();
       // RefreshTag();
//		base.show();

		curRuneId=rrj.id;
		initData();
		//==获取第一个没全部点亮的页==//
		curPage=1;
		for(int k=0;k<pageNums.Length;k++)
		{
			if(RuneData.getNextData(times,k+1,pageNums[k])!=null)
			{
				curPage=k+1;
				break;
			}
		}
		
		RuneData nextRd=RuneData.getNextData(times,curPage,pageNums[curPage-1]);
		if(nextRd==null)
		{
			btnLighten.SetActive(false);
		}
		else
		{
			btnLighten.SetActive(true);
		}
		btnBack.SetActive(true);
		drawLabels();
		drawRuneLight(true);
		drawArrows();
		showCurRune();
	}
	
	private void showCurRune()
	{
		//==显示当前的符文==//
		for(int k=0;k<runeGos.Length;k++)
		{
			if(k==curPage-1)
			{
				runeGos[k].GetComponent<TweenAlpha>().alpha=1f;
				runeGos[k].SetActive(true);
			}
			else
			{
				runeGos[k].SetActive(false);
			}
		}
	}
	
	private void draw()
	{
		if(rrj.errorCode==28)
		{
			ToastWindow.mInstance.showText(TextsData.getData(74).chinese);


           btnLighten.SetActive(true);
			canBack=true;
		}
		else if(rrj.errorCode==0)//==点亮成功==//
		{
			drawEffect();
			ShowTips(0);
		}
		else if(rrj.errorCode==30)//==点亮失败==//
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_FAIL);
			initData();
			drawLabels();
			drawArrows();
			ShowTips(1);
		}
	}
	
	private void initData()
	{
		string[] ss=curRuneId.Split('-');
		times=StringUtil.getInt(ss[0]);
		pageNums=new int[Constant.RunePageLength];
		for(int k=0;k<pageNums.Length;k++)
		{
			pageNums[k]=StringUtil.getInt(ss[k+1]);
		}
		ss=rrj.pro.Split('-');
		runeAdds=new int[Constant.RunePageLength];
		for(int k=0;k<runeAdds.Length;k++)
		{
			runeAdds[k]=StringUtil.getInt(ss[k]);
		}
	}

    private void CurTag()
    {
        drawLabels();   
        btnBack.SetActive(true);
   
        drawRuneLight(true);
        drawArrows();
        showCurRune();
    }

    public void ClickTags(int param)
    {
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        curPage = param;
        prePage = curPage;
        Pages();
        btnBack.SetActive(true);
        drawLabels();
        drawRuneLight(true);
        drawArrows();
        showCurRune();
    }

    public void ClickGroups(int param)
    {
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        times = param;
        CurTag();
    }

    private void ShowRuneName()
    {
        for (int r = 0; r < RuneLabels.Length; r++)
        {
            //int k = (r + 1) * 100 + curPage;
            //RuneTotalData rtdd = RuneTotalData.getData(k);

            string[] ss = curRuneId.Split('-');
            int ctime = StringUtil.getInt(ss[0]);
            switch(r+1)
            {
                case 1:
                    RuneLabels[r].text = TextsData.getData(379).chinese;
                    RuneLabels[r].transform.parent.GetComponent<UIButtonMessage>().enabled = true;
                    if (ctime < r + 1)
                    {
                        RuneLabels[r].text = "????";
                        RuneLabels[r].transform.parent.GetComponent<UIButtonMessage>().enabled = false;
                    }
                    break;
                case 2:
                    RuneLabels[r].text = TextsData.getData(380).chinese;
                    RuneLabels[r].transform.parent.GetComponent<UIButtonMessage>().enabled = true;
                    if (ctime < r + 1)
                    {
                        RuneLabels[r].text = "????";
                        RuneLabels[r].transform.parent.GetComponent<UIButtonMessage>().enabled = false;
                    }
                    break;
                case 3:
                    RuneLabels[r].text = TextsData.getData(381).chinese;
                    RuneLabels[r].transform.parent.GetComponent<UIButtonMessage>().enabled = true;
                    if (ctime < r + 1)
                    {
                        RuneLabels[r].text = "????";
                        RuneLabels[r].transform.parent.GetComponent<UIButtonMessage>().enabled = false;
                    }
                    break;
                case 4:
                    RuneLabels[r].text = TextsData.getData(382).chinese;
                    RuneLabels[r].transform.parent.GetComponent<UIButtonMessage>().enabled = true;
                    if (ctime < r + 1)
                    {
                        RuneLabels[r].text = "????";
                        RuneLabels[r].transform.parent.GetComponent<UIButtonMessage>().enabled = false;
                    }
                    break;
            }
        }
        for (int i = 0; i < Groups.Length; i++)
        {
            cuiTests[i] = RuneLabels[i].text;
            if (i != times - 1)
            {
                RuneLabels[i].text = "[808080]" + cuiTests[i];
            }
            else
            {
                RuneLabels[i].text = cuiTests[i];
            }
        }
    }
    
    private void drawLabels()
	{
        ShowRuneName();
		runeValue.text=rrj.num.ToString();
		//==1全部点亮,0部分点亮==//
		int lightMark=0;
		RuneData rd=RuneData.getNextData(times,curPage,pageNums[curPage-1]);
        string[] ss = curRuneId.Split('-');
		if(rd==null)
        {
            lightMark = 1;
		}
		else
		{
			if (times != StringUtil.getInt(ss[0]))
	        {
	            for (int i = 0; i < pageNums.Length; i++)
	            {
	                pageNums[i] = 12;
	            }
	            lightMark = 1;
	        }
	        else
	        {
	            lightMark = 0;
	        }
		}
		switch(lightMark)
		{
		case 1:
			int key=times*100+curPage;
			RuneTotalData rtd=RuneTotalData.getData(key);
			string attri=getAttriString(rtd.proprety);
			label1.text=rtd.name;
			label2_1.enabled=false;
			label2_2.enabled=false;
			label2_3.enabled=false;
			label2_4.enabled=false;
			label3_1.text=TextsData.getData(70).chinese;
			label3_2.text=attri+"+"+RuneData.getValues(key,pageNums[curPage-1]);
			label3_3.color=colorHigh;
			label3_4.color=colorHigh;
			label3_3.text=TextsData.getData(71).chinese;
			label3_4.text=attri+"+"+rtd.value;
			label4_1.enabled=true;
			label4_1.color=colorHigh;
			label4_1.text=TextsData.getData(76).chinese;
			break;
		case 0:
			key=times*100+curPage;
			rtd=RuneTotalData.getData(key);
			attri=getAttriString(rtd.proprety);
			label1.text=rtd.name;
			label2_1.enabled=true;
			label2_2.enabled=true;
			label2_3.enabled=true;
			label2_4.enabled=true;
			label2_1.text=TextsData.getData(67).chinese;
			label2_2.text=attri+"+"+rd.value;
			label2_3.text=TextsData.getData(68).chinese+rd.cost;
			label2_4.text=TextsData.getData(69).chinese+rd.successrate+"%(+"+runeAdds[curPage-1]+"%)";
			label3_1.text=TextsData.getData(70).chinese;
			label3_2.text=attri+"+"+RuneData.getValues(key,pageNums[curPage-1]);
			label3_3.color=colorLow;
			label3_4.color=colorLow;
			label3_3.text=TextsData.getData(71).chinese;
			label3_4.text=attri+"+"+rtd.value;
			label4_1.enabled=false;
			break;
		}
      
	}
	
	private string getAttriString(int attri)
	{
		string result="";
		switch(attri)
		{
		case 1:
			result=TextsData.getData(259).chinese;
			break;
		case 2:
			result=TextsData.getData(260).chinese;
			break;
		case 3:
			result=TextsData.getData(261).chinese;
			break;
		case 4:
			result=TextsData.getData(515).chinese;
			break;
		case 5:
			result=TextsData.getData(516).chinese;
			break;
		}
		return result;
	}
	
	private void drawRuneLight(bool all)
	{
        int key = times * 100 + curPage;
        RuneTotalData rtd = RuneTotalData.getData(key);
        string[] ss = curRuneId.Split('-');
        //==设置6个符文图的位置==//
		for(int i=0;i<runeParentGo.transform.childCount;i++)
		{
			Transform tf=runeParentGo.transform.GetChild(i);
			//==设置点亮==//
            for (int k = 0; (all || curPage == i + 1) && k < tf.childCount; k++)
            {
                UISprite sp = tf.FindChild("light-" + (k + 1)).gameObject.GetComponent<UISprite>();
                tf.FindChild("light-" + (k + 1) + "/effect").localScale = new Vector3(0.6f, 0.6f, 0.6f);
                GameObject fire = tf.FindChild("light-" + (k + 1) + "/effect/fire_fuwen").gameObject;
                fire.transform.localPosition = new Vector3(0, 35f, 0);
                //==符文火特效帧率==//
                fire.GetComponent<UISpriteAnimation>().framesPerSecond = 20;
                if (k < pageNums[i])
                {
                    sp.spriteName = times + "-" + RuneLightData.getRuneName(i + 1, k + 1);
                    fire.SetActive(true);
                }
                else if (k == pageNums[i])
                {
                    sp.spriteName = null;
                    fire.SetActive(false);
                }
                else
                {
                    sp.spriteName = null;
                    fire.SetActive(false);
                }
                if (PlayerInfo.getInstance().player.level < rtd.level)
                {
                    sp.gameObject.SetActive(false);
                }
                else
                {
                    sp.gameObject.SetActive(true);
                }
            }
		}
        for (int i = 0; i < pageNums.Length; i++)
        {
            pageNums[i] = StringUtil.getInt(ss[i + 1]);
        }
	}
	
	private void drawArrows()
	{
		arrowLeft.SetActive(true);
		arrowRight.SetActive(true);
	}
	
	private void drawEffect()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_FW_SUCC);
		
		Transform tfParent=runeParentGo.transform.GetChild(curPage-1);
		Transform tf=tfParent.FindChild("light-"+(1+pageNums[curPage-1]));
		for(int i=0;i<initPos.Length;i++)
		{
			if(lightEffect==null)
			{
				lightEffect=GameObjectUtil.LoadResourcesPrefabs("UIEffect/flyingspark",1);
			}
			GameObject effect=Instantiate(lightEffect) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(effect,effectGos[i]);
			TweenPosition tp=effectGos[i].GetComponent<TweenPosition>();
			tp.from=initPos[i];
			tp.to=tf.localPosition;
			tp.tweenFactor=0;
			tp.enabled=true;
			Destroy(effect,tp.duration+0.2f);
		}
		
		
		//btnLighten.SetActive(false);
		
	}
	
	private void ShowTips(int textType)//0成功，1失败//
	{
		Transform tfParent=runeParentGo.transform.GetChild(curPage-1);
		Transform tf=tfParent.FindChild("light-"+(1+pageNums[curPage-1]));
		
		if(textType == 0)
		{
			int key=times*100+curPage;
			RuneTotalData rtd=RuneTotalData.getData(key);
			string attri=getAttriString(rtd.proprety);
			RuneData rd=RuneData.getNextData(times,curPage,pageNums[curPage-1]);
			runeTips.GetComponent<UILabel>().text = attri+"+"+rd.value;
			runeTips.GetComponent<UILabel>().color = Color.green;
			runeTips.localPosition = new Vector3(1.2f,1.2f,1.2f);
		}
		else if (textType == 1)
		{
			runeTips.GetComponent<UILabel>().text = TextsData.getData(518).chinese;
			runeTips.GetComponent<UILabel>().color = Color.red;
			runeTips.localPosition = new Vector3(1.2f,1.2f,1.2f);
		}
		if(!runeTips.gameObject.activeSelf)
		{
			runeTips.gameObject.SetActive(true);
		}
		runeTips.parent = tf;
		runeTips.localPosition = Vector3.zero;
		TweenPosition position = runeTips.GetComponent<TweenPosition>();
		position.from = runeTips.localPosition;
		position.to = new Vector3(runeTips.localPosition.x,runeTips.localPosition.y+50,runeTips.localPosition.z);
		position.duration=0.5f;
		position.tweenFactor=0;
		position.enabled = true;
    	EventDelegate.Add (position.onFinished, OnFinished);
	}
	
	public void OnFinished()
	{
		runeTips.gameObject.SetActive(false);
		runeTips.GetComponent<UILabel>().text = "";
		btnLighten.SetActive(true);
		canBack=true;
		if(RuneData.getNextData(times,curPage,pageNums[curPage-1])==null)
		{
			btnLighten.SetActive(false);
		}
		else
		{
			btnLighten.SetActive(true);
		}
	}
	
	public void effectOver()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SUCC);
		
		bool drawAllLight=false;
		curRuneId=rrj.id;
		string[] ss=curRuneId.Split('-');
		int timesNew=StringUtil.getInt(ss[0]);
		//==点亮一遍,从头开始==//
		if(timesNew!=times)
		{
			ToastWindow.mInstance.showText(TextsData.getData(75).chinese);
            ShowRuneName();


            for (int i = 0; i < Groups.Length; i++)
            {
                if (i == times)
                {
                    Groups[i].GetComponent<UIToggle>().enabled = true;
                    Groups[i].GetComponent<UIToggle>().value = true;
                    Groups[i].transform.FindChild("Background").gameObject.SetActive(true);
                }
                else
                {
                    Groups[i].GetComponent<UIToggle>().value = false;
                }
            }
            //curPage = 1;
			drawAllLight=true;
			showCurRune();
		}
		initData();
//		if(RuneData.getNextData(times,curPage,pageNums[curPage-1])==null)
//		{
//			btnLighten.SetActive(false);
//		}
//		else
//		{
//			btnLighten.SetActive(true);
//		}
		drawLabels();
		drawRuneLight(drawAllLight);
		drawArrows();
	}
	
	public void hide()
	{
//		base.hide();
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_RUNE);
	}
	
	private void gc()
	{
		rrj=null;
		lightEffect=null;
		Resources.UnloadUnusedAssets();
	}
	
	public void onClickLighten()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		RuneData nextRd=RuneData.getNextData(times,curPage,pageNums[curPage-1]);
		if(nextRd==null)
		{
			return;
		}
		if(rrj.num<nextRd.cost)
		{
			ToastWindow.mInstance.showText(TextsData.getData(74).chinese);
			return;
		}
        int key = times * 100 + curPage;
        RuneTotalData rtd = RuneTotalData.getData(key);


        if (PlayerInfo.getInstance().player.level < rtd.level)
        {
            ToastWindow.mInstance.showText(TextsData.getData(371).chinese.Replace("num",rtd.level.ToString()));
        }
        else
        {

            //==发送==//
            requestType = 1;
            PlayerInfo.getInstance().sendRequest(new RuneJson(curPage), this);
			btnLighten.SetActive(false);
			canBack=false;
        }
	}
	
	public void onClickBack()
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		if(!canBack)
		{
			return;
		}
//		MainCardSetPanel.mInstance.show();
		//显示主菜单下方按钮//
//		MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
		//返回主城界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
		if(main!= null)
		{
			main.SetData(STATE.ENTER_MAINMENU_BACK);
		}
		hide();
		rrj=null;
//		HeadUI.mInstance.requestPlayerInfo();
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			Debug.Log("runeUI : json ========= " + json);
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				
				rrj=JsonMapper.ToObject<RuneResultJson>(json);
				errorCode = rrj.errorCode;
				receiveData = true;
//				if(errorCode == 0)
//				{
//				}
//				else 
//				{
//					receiveData = true;
//				}
				break;
			case 2:
				PlayerResultJson prj = JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode = prj.errorCode;
				PlayerElement pe=prj.list[0];
				PlayerInfo.getInstance().player=pe;
				if(errorCode == 0)
				{
					
				}
				receiveData=true;
				break;
			}
			
		}
	}
}
