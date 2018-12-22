using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionUI2 : MonoBehaviour
{
    //	public static MissionUI2 mInstance;

    private float ScreenNum;
    private float ScreenWidth;
    private float MaxX;
    private float MinX;

    public UIAtlas nearAtlas;
    public UIAtlas middleAtlas;
    public UIAtlas remoteAtlas;

    public GameObject near;
    public GameObject middle;
    public GameObject remote;
    public GameObject zones;

    public GameObject arrowLeft;
    public GameObject arrowRight;

    //精英，普通两个按钮//
    public GameObject mapMode1;		//普通//
    public GameObject mapMode2;		//精英//
    public GameObject[] mapMode = new GameObject[3];

    public GameObject mapsPanel;
    public GameObject activityPanel;

    public UISprite texBg;

    public bool canMove = true;
    private float xOffset;

    private GameObject loadZone;

    public Hashtable zoneGos = new Hashtable();
    //==是否可释放大地图==//
    private bool markOfLoadDestory;

    public int curMissionType;

    Transform _myTransform;
    MissionUI misson1;

    //List<GameObject> warFog = new List<GameObject>();

    public GameObject[] locks;

    public GameObject[] foglefts;

    public GameObject[] fogrights;
	
	public GameObject koAwardBtn;

    int missionType1 = 1;
    
    //bool isFog;
	
	
    void Awake()
    {
        //		_MyObj=gameObject;
        //		mInstance=this;
        //		
        _myTransform = transform;

        ScreenNum = MapStructData.ScreenNum;
        ScreenWidth = MapStructData.ScreenWidth;
        MaxX = -480f;
        MinX = -480f - ScreenWidth * (ScreenNum - 1);
    }

    // Use this for initialization
    void Start()
    {
        init();
		//int i = PlayerInfo.getInstance().player.missionId;
        if (PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_MAP)
        {
            show();
        }
    }

    public void init()
    {
        //		base.init();
        if (misson1 == null)
        {
            misson1 = _myTransform.GetComponent<MissionUI>();
        }
    }

    public void show()
    {
      
        //		base.show();
        if (PlayerInfo.getInstance().player.missionId < 110407)
        {
            locks[0].SetActive(true);
        }
        if (PlayerInfo.getInstance().getUnLockData(40) == 0)
        {
            locks[1].SetActive(true);
        }
        if (misson1 == null)
        {
            misson1 = _myTransform.GetComponent<MissionUI>();
        }
        if (misson1.mrj == null)
        {
            misson1.mrj = PlayerInfo.getInstance().mrj;
        }
        int lastMissionId = PlayerInfo.getInstance().lastMissionId;
        int missionType = 1;
        int screenNum = 0;
        if (lastMissionId != 0)
        {
            MissionData md = MissionData.getData(lastMissionId);
            //显示上次挑战的卷轴//
            screenNum = MissionData.getOldScreenNum(md.id);
            missionType = md.missiontype;
        }
        else
        {
            //显示最新的卷轴//
            screenNum = MissionData.getNewScreenNum(misson1.mrj.m1);
        }
        if (PlayerInfo.getInstance().bBackActivity)
        {
            PlayerInfo.getInstance().bBackActivity = false;
            missionType = 3;
        }

        this.missionType1 = missionType;

        setBgColorTint(missionType1);
        drawMapBtn(missionType1);
        if (missionType1 == 3)
        {
            activityPanel.SetActive(true);
            mapsPanel.SetActive(false);
            activityPanel.GetComponent<MainUI>().show();
            HeadUI.mInstance.show();
        }
        else
        {
            activityPanel.SetActive(false);
            mapsPanel.SetActive(true);

            drawMap(screenNum, missionType);
        }
        
        //播放声音//
        string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MISSION).music;
        MusicManager.playBgMusic(musicName);
        
    }

    public void showFromCompose(int param)
    {
        MissionData md = MissionData.getData(param);
        int screenNum = MissionData.getOldScreenNum(md.id);
        int missionType = md.missiontype;
        drawMap(screenNum, missionType);
    }
	
	

    public void baseShow()
    {
        //		base.show();
        UISceneStateControl.mInstace.ShowObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
    }

    public void baseHide()
    {
        //		base.hide();
        UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
    }

    public void hide()
    {   
        //		base.hide();
        canMove = true;
        gc();
        if (UISceneStateControl.mInstace != null)
        {
            UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
        }
    }

    private void gc()
    {
        loadZone = null;
        nearAtlas = null;
        middleAtlas = null;
        remoteAtlas = null;

        zoneGos.Clear();
        GameObjectUtil.destroyGameObjectAllChildrens(near);
        GameObjectUtil.destroyGameObjectAllChildrens(middle);
        GameObjectUtil.destroyGameObjectAllChildrens(remote);
        GameObjectUtil.destroyGameObjectAllChildrens(zones);
        Resources.UnloadUnusedAssets();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GuideManager.getInstance().isGuideRunning())
        {
            //滑屏//
            if (canMove)
            {
#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_OSX
					touchMove();
#else
                mouseMove();
#endif
            }
        }

        Vector3 middlePos = middle.transform.localPosition;
        if (middlePos.x >= MaxX - 20)
        {
            if (arrowLeft.activeSelf)
            {
                arrowLeft.SetActive(false);
            }
            if (!arrowRight.activeSelf)
            {
                arrowRight.SetActive(true);
            }
        }
        else if (middlePos.x <= MinX + 80)
        {
            if (!arrowLeft.activeSelf)
            {
                arrowLeft.SetActive(true);
            }
            if (arrowRight.activeSelf)
            {
                arrowRight.SetActive(false);
            }
        }
        else
        {
            if (!arrowLeft.activeSelf)
            {
                arrowLeft.SetActive(true);
            }
            if (!arrowRight.activeSelf)
            {
                arrowRight.SetActive(true);
            }
        }

        if (markOfLoadDestory)
        {
            loadSprite(1, MaxX - near.transform.localPosition.x, near, nearAtlas);
            loadSprite(2, MaxX - middle.transform.localPosition.x, middle, middleAtlas);
            loadSprite(3, MaxX - remote.transform.localPosition.x, remote, remoteAtlas);

            //destroySprite(MaxX - near.transform.localPosition.x, near);
            //destroySprite(MaxX - middle.transform.localPosition.x, middle);
            //destroySprite(MaxX - remote.transform.localPosition.x, remote);
        }
    }

    private void drawMap(int screenNum, int missionType)
    {
        setPosition(1, near, screenNum);
        setPosition(2, middle, screenNum);
        setPosition(3, remote, screenNum);
        loadSprite();
        loadZones();
        drawZones(missionType);
        //修改精英和普通按钮的显示//
        drawMapBtn(missionType);
    }

    private void loadZones()
    {
        MapStructData msd = MapStructData.getData("zone");
		//清除zones下面的所有的子节点//
		GameObjectUtil.destroyGameObjectAllChildrens(zones);
        for (int i = 0; i < msd.elements.Count; i++)
        {
            string[] info = msd.elements[i].Split('-');
            string[] position = StringUtil.getString(info[0]).Split(',');
            float x = StringUtil.getFloat(position[0]);
            float y = StringUtil.getFloat(position[1]);
            string[] zoneInfo = StringUtil.getString(info[1]).Split(',');
            int mapNum = StringUtil.getInt(zoneInfo[0]);
            int zoneNum = StringUtil.getInt(zoneInfo[1]);
            if (loadZone == null)
            {
                loadZone = GameObjectUtil.LoadResourcesPrefabs("UI-mission/zone", 3);
            }
            GameObject zoneTemp = Instantiate(loadZone) as GameObject;
            zoneTemp.name = mapNum + "-" + zoneNum;
            GameObjectUtil.gameObjectAttachToParent(zoneTemp, zones);
            zoneTemp.transform.localPosition = new Vector3(x, y, 0);
            GameObject city = zoneTemp.transform.FindChild("city").gameObject;
            city.GetComponent<UISprite>().spriteName = "map_base_01";
            city.GetComponent<UISprite>().depth = 1;
            city.GetComponent<UIButtonMessage3>().target = _myTransform.gameObject;
            city.GetComponent<UIButtonMessage3>().functionName = "onClickZone";
        }
    }

    private void drawZones(int missionType)
    {
        if (misson1 == null)
        {
            misson1 = _myTransform.GetComponent<MissionUI>();
        }
        if (missionType == 2)
        {
            misson1.pointMapModel2.SetActive(false);
        }


        curMissionType = missionType;
        zoneGos.Clear();
        for (int i = 0; i < zones.transform.childCount; i++)
        {
            GameObject zoneTemp = zones.transform.GetChild(i).gameObject;
            string[] ss = zoneTemp.name.Split('-');
            int map = StringUtil.getInt(ss[0]);
            int zone = StringUtil.getInt(ss[1]);
            GameObject city = zoneTemp.transform.FindChild("city").gameObject;
            GameObject wf = zoneTemp.transform.FindChild("warFog").gameObject;
            city.GetComponent<UIButtonMessage3>().stringParam = map + "-" + zone + "-" + missionType;
            zoneGos.Add(map + "-" + zone + "-" + missionType, zoneTemp);
            //==设置区域icon==//
            if (!misson1.canClick(map + "-" + zone + "-" + missionType))
            {
                if (missionType == 1)
                {
                    city.GetComponent<UISprite>().spriteName = "map_base_02";
                    wf.SetActive(true);
                    //if (isFog)
                    //    GetFogShow(wf, true);
                    //city.gameObject.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    city.GetComponent<UISprite>().spriteName = "map_base_04";
                    GetFogShow(wf, true);
                }
            }
            else
            {
                if (missionType == 1)
                {
                    city.GetComponent<UISprite>().spriteName = "map_base_01";
                    //if (isFog)
                    //{
                    //    if (wf.activeSelf)
                    //    {
                    //        GetFogShow(wf, false);
                    //    }
                    //    else
                    //    {
                    //        wf.SetActive(false);
                    //        city.gameObject.GetComponent<Collider>().enabled = true;
                    //    }
                    //}
                    //else
                    //{
                       wf.SetActive(false);
                       city.gameObject.GetComponent<Collider>().enabled = true;
                    //}
                }
                else
                {
                    city.GetComponent<UISprite>().spriteName = "map_base_03";
                    //city.gameObject.GetComponent<Collider>().enabled = true;
                    wf.SetActive(false);
                }
            }
			
			wf.SetActive(false);
			
            //==设置区域星级==//
            List<MissionData> mds = MissionData.getData(map, zone, missionType);
            int starNums = 0;
            foreach (MissionData md in mds)
            {
                int sequence = md.getSequence();
                char star = '0';
                if ((md != null && md.missiontype == 1) && (misson1 != null && misson1.mrj != null
                    && misson1.mrj.star1 != null
                    && misson1.mrj.star1.Length >= sequence))
                {
                    star = misson1.mrj.star1[sequence - 1];
                }
                if (md.missiontype == 2 && misson1.mrj.star2.Length >= sequence)
                {
                    star = misson1.mrj.star2[sequence - 1];
                }
                int starNum = 0;
                switch (star)
                {
                    case '0':
                        starNum = 0;
                        break;
                    case '1':
                        starNum = 1;
                        break;
                    case '2':
                        starNum = 2;
                        break;
                    case '3':
                    case '4':
                        starNum = 3;
                        break;
                }
                starNums += starNum;
            }
            GameObject star1 = zoneTemp.transform.FindChild("city/star1").gameObject;
            GameObject star2 = zoneTemp.transform.FindChild("city/star2").gameObject;
            GameObject star3 = zoneTemp.transform.FindChild("city/star3").gameObject;
            if (starNums == 0)
            {
                star1.GetComponent<UISprite>().spriteName = "map_star_2";
                star2.GetComponent<UISprite>().spriteName = "map_star_2";
                star3.GetComponent<UISprite>().spriteName = "map_star_2";
            }
            else if (starNums >= 1 && starNums < mds.Count * 2)
            {
                star1.GetComponent<UISprite>().spriteName = "map_star";
                star2.GetComponent<UISprite>().spriteName = "map_star_2";
                star3.GetComponent<UISprite>().spriteName = "map_star_2";
            }
            else if (starNums < mds.Count * 3)
            {
                star1.GetComponent<UISprite>().spriteName = "map_star";
                star2.GetComponent<UISprite>().spriteName = "map_star";
                star3.GetComponent<UISprite>().spriteName = "map_star_2";
            }
            else
            {
                star1.GetComponent<UISprite>().spriteName = "map_star";
                star2.GetComponent<UISprite>().spriteName = "map_star";
                star3.GetComponent<UISprite>().spriteName = "map_star";
            }
            GameObject newMark = zoneTemp.transform.FindChild("new").gameObject;
			GameObject sprite=zoneTemp.transform.FindChild("sprite").gameObject;
            if (misson1.isNewZone(map + "-" + zone + "-" + missionType))
            {
                newMark.SetActive(true);
                zoneTemp.transform.FindChild("warFog").gameObject.SetActive(false);
            }
            else
            {
                newMark.SetActive(false);
            }
			//==如果本区域有未完成的小关,显示光圈==//
			if(misson1.isZoneNotCompleted(map + "-" + zone + "-" + missionType))
			{
				sprite.SetActive(true);
			}
			else
			{
				sprite.SetActive(false);
			}
            string markName = MissionData.getZoneName(map, zone, missionType);
            GameObject name = zoneTemp.transform.FindChild("name").gameObject;
            name.GetComponent<UILabel>().text = markName;
        }
    }

    private void mouseMove()
    {
        if (Input.GetMouseButton(0))
        {
            xOffset += Input.GetAxis("Mouse X");
            if (xOffset != 0)
            {
                move(xOffset);
            }
        }
        else
        {
            if (xOffset != 0)
                xOffset = 0;
        }
    }

    private void touchMove()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            xOffset += Input.GetTouch(0).deltaPosition.x;
            if (xOffset != 0)
            {
                move(xOffset);
            }
        }
        else
        {
            if (xOffset != 0)
                xOffset = 0;
        }
    }

    private void move(float offset)
    {
        float mul = 30;

        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            mul = mul / 10;
        }

        setTween(1, offset * MapStructData.nearFactor * mul, near);
        setTween(2, offset * MapStructData.middleFactor * mul, middle);
        setTween(3, offset * MapStructData.remoteFactor * mul, remote);

        markOfLoadDestory = true;
    }

    //**type:1,2,3**//
    private void setTween(int type, float offset, GameObject go)
    {
        TweenPosition tw = go.GetComponent<TweenPosition>();
        tw.from = go.transform.localPosition;
        float toX = tw.from.x + offset;

        float rate = 0;
        if (type == 1)
        {
            rate = MapStructData.nearFactor;
        }
        else if (type == 2)
        {
            rate = MapStructData.middleFactor;
        }
        else if (type == 3)
        {
            rate = MapStructData.remoteFactor;
        }

        float max = MaxX;
        float min = MaxX - ScreenWidth * (ScreenNum - 1) * rate;

        min += 36f;

        if (toX > max)
        {
            toX = max;
        }
        if (toX < min)
        {
            toX = min;
        }
		
		//==超出了当前挑战范围==//
		int maxScreenNum=0;
		if(curMissionType==1)
		{
			maxScreenNum=MissionData.getNewScreenNum(misson1.mrj.m1);
		}
		else
		{
			maxScreenNum=MissionData.getNewScreenNum(misson1.mrj.m2);
		}
		if(maxScreenNum>=0 && toX<MaxX-maxScreenNum*ScreenWidth)
		{
			toX=MaxX-maxScreenNum*ScreenWidth;
		}
		
        tw.to = new Vector3(toX, tw.from.y, tw.from.z);
        tw.tweenFactor = 0;
        tw.enabled = true;
        if (type == 2)
        {
            TweenPosition tw2 = zones.GetComponent<TweenPosition>();
            tw2.from = tw.from;
            tw2.to = tw.to;
            tw2.tweenFactor = 0;
            tw2.enabled = true;
        }
    }

    private void setPosition(int type, GameObject go, int screenNum)
    {
        float offset = -1 * screenNum * MapStructData.moveSteps[type - 1];

        Vector3 initV3 = go.transform.localPosition;
        go.transform.localPosition = new Vector3(-480 + offset, initV3.y, initV3.z);
        if (type == 2)
        {
            zones.transform.localPosition = new Vector3(-480 + offset, initV3.y, initV3.z);
        }
    }

    public void tweenOver()
    {
        markOfLoadDestory = false;
    }

    private void loadSprite()
    {
        loadSprite(1, MaxX - near.transform.localPosition.x, near, nearAtlas);
        loadSprite(2, MaxX - middle.transform.localPosition.x, middle, middleAtlas);
        loadSprite(3, MaxX - remote.transform.localPosition.x, remote, remoteAtlas);
    }

    private void loadSprite(int type, float xDistance, GameObject parentGo, UIAtlas atlas)
    {
        //xDistance:卷轴景层相对于最左边移动的x距离//
        List<int> list = new List<int>();
        for (int i = 0; i < parentGo.transform.childCount; i++)
        {
            Transform childTf = parentGo.transform.GetChild(i);
            int index = StringUtil.getInt(childTf.name.Split('-')[0]);
            list.Add(index);
        }
        float distance = 0;
        MapStructData msd = MapStructData.getData(type + "");
        for (int i = 0; i < msd.elements.Count; i++)
        {
            string spriteName = msd.elements[i];
            int eleWidth = MapStructData.getWidth(spriteName);
            if (eleWidth + distance >= xDistance - 20 && distance <= xDistance + ScreenWidth + 20 && !list.Contains(i))
            {
                UISprite sprite = NGUITools.AddChild<UISprite>(parentGo);
                sprite.gameObject.name = i + "-" + spriteName;
                //sprite.atlas=atlas;

                if (type == 2)
                {
                	sprite.atlas = middleAtlas;
                }
                else
                {
                    sprite.atlas = atlas;
                }

                sprite.spriteName = spriteName;
                sprite.pivot = UIWidget.Pivot.BottomLeft;
                sprite.transform.localPosition = new Vector3(distance, 0, 0);
                sprite.width = eleWidth;
                sprite.height = MapStructData.getHeight(spriteName);
            }
            distance += eleWidth - 1 * 3;
        }
    }

    //==释放地图==//
    /*private void destroySprite(float xDistance, GameObject parentGo)
    {

        if (1 == 1)
        {
            return;
        }

        //xDistance:卷轴景层相对于最左边移动的x距离//
        for (int k = parentGo.transform.childCount - 1; k >= 0; k--)
        {
            Transform childTf = parentGo.transform.GetChild(k);
            int spriteWidth = childTf.GetComponent<UISprite>().width;
            if (childTf.localPosition.x + spriteWidth < xDistance - 20 || childTf.localPosition.x > xDistance + ScreenWidth + 20)
            {
                GameObject.DestroyImmediate(childTf.gameObject);
            }
        }
    }*/

    public void drawMapBtn(int missionType)
    {
        for (int i = 0; i < mapMode.Length; i++)
        {
            if (i + 1 == missionType)
                mapMode[i].SetActive(false);
            else
                mapMode[i].SetActive(true);
        }
    }

    public void GetFogShow(GameObject go, bool isShow)
    {
        if (!go.activeSelf)
        {
            go.SetActive(true);
        }
        TweenAlpha ta = go.GetComponent<TweenAlpha>();
        if (isShow)
        {
            ta.from = 0;
            ta.to = 1;
        }
        else
        {
            ta.from = 1;
            ta.to = 0;
        }
        ta.duration = 2;
        ta.tweenFactor = 0;
        ta.enabled = true;
        go.transform.parent.FindChild("city").gameObject.GetComponent<Collider>().enabled = !isShow;
    }

    public void GetCut()
    {
        setBgColorTint(missionType1);

        if (missionType1 == 3)
        {
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace))
			{
				GuideUI17_WarpSpace.mInstance.showStep(2);
			}
			else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ActiveCopy))
			{
				GuideUI16_ActiveCopy.mInstance.showStep(2);
			}
            //openBattleActivity();
			
            activityPanel.SetActive(true);
            mapsPanel.SetActive(false);
            activityPanel.GetComponent<MainUI>().show();
			koAwardBtn.SetActive(false);
        }
        else
        {
            activityPanel.SetActive(false);
            mapsPanel.SetActive(true);

            MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
            int screenNum = 0;
            if (missionType1 == 1)
            {
                screenNum = MissionData.getNewScreenNum(misson1.mrj.m1);
            }
            else
            {
                screenNum = MissionData.getNewScreenNum(misson1.mrj.m2);
            }
            drawMap(screenNum, missionType1);
			koAwardBtn.SetActive(true);
        }
    }

    void setBgColorTint(int missionType)
    {
        if (missionType == 3)
            texBg.color = new Color(255, 255, 255);
        else
            texBg.color = new Color(135, 126, 126);
    }

	//==1普通,2精英==//
    public void onClickModelBtn(int missionType)
    {
        
        TweenPosition tw0 = foglefts[0].GetComponent<TweenPosition>();
        if (tw0.enabled == true)
        {
            return;
        }
        //==当前显示的模式和点击的模式一样==//
        //if (missionType == 1 && !mapMode1.transform.FindChild("Black").gameObject.activeSelf)
        //{
        //    return;
        //}
        //if (missionType == 2 && !mapMode2.transform.FindChild("Black").gameObject.activeSelf)
        //{
        //    return;
        //}
        if (missionType == 2 && PlayerInfo.getInstance().player.missionId < 110407)
        {
            ToastWindow.mInstance.showText(TextsData.getData(591).chinese);
            return;
        }
        else if (missionType == 3 && PlayerInfo.getInstance().getUnLockData(40) == 0)
        {
            ToastWindow.mInstance.showText(TextsData.getData(590).chinese);
            return;
        }
        if (this.missionType1 == missionType)
        {
            return;
        }
        this.missionType1 = missionType;
        //for (int i = 0; i < mapMode.Length; i++)
        //{
        //    if (i + 1 == missionType && !mapMode[i].activeSelf)
        //        return;
        //}
		
        //播放音效//
    
        stopTweenPositions();
        GetFog();
        Invoke("GetCut", 1.6f);
        drawMapBtn(missionType);
    }

    bool isShowFogs;
    public void GetFog()
    {
       
        for(int i=0;i<foglefts.Length;i++)
        {

            TweenPosition tw = foglefts[i].GetComponent<TweenPosition>();

            tw.PlayForward();

            TweenPosition tw1 = fogrights[i].GetComponent<TweenPosition>();
            tw1.PlayForward();
        }
        isShowFogs = true;
    }

    public void FogBack()
    {
        for (int i = 0; i < foglefts.Length; i++)
        {

            TweenPosition tw = foglefts[i].GetComponent<TweenPosition>();
            tw.PlayReverse();

            TweenPosition tw1 = fogrights[i].GetComponent<TweenPosition>();

            tw1.PlayReverse();
        }
    }

    public void FogReturn()
    {
        if (isShowFogs)
        {
            Invoke("FogBack", 0.5f);
        }
        isShowFogs = false;
    }
	public void openBattleActivity()
	{
	 	MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CHALLENGE);
        MainUI mainUI = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CHALLENGE, "MainUI") as MainUI;
        mainUI.show();
        hide();
	}
	
	private void stopTweenPositions()
	{
		near.GetComponent<TweenPosition>().enabled=false;
		middle.GetComponent<TweenPosition>().enabled=false;
		zones.GetComponent<TweenPosition>().enabled=false;
		remote.GetComponent<TweenPosition>().enabled=false;
	}
}