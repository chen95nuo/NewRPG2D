using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleBounesPanel : BWUIPanel {
	
	public static BattleBounesPanel mInstance;
	//Camera mainCamera;
	Camera BounesCamera;
	public GameObject TimeBoxObj;
	public GameObject AddCoinsBoxObj;
	public GameObject ModelParent;
	public GameObject LinesParent;
	public GameObject HitEffectParent;
	public GameObject WhiteBg;
	public GameObject ClickTipObj;
	public GameObject[] KOObjs;
	public GameObject GetCardPanel;
	public GameObject CardParent;
	public GameObject[] TX_Pos;
	public SimpleCardInfo1 GiftCard;
	
	public UIAtlas CardAtlas;
	public UIAtlas SkillAtlas;
	public UIAtlas PassiveSkillAtlas;
	public UIAtlas itemAtlas;
	public UIAtlas equipAtlas;        
	GameObject CurModel;
	GameObject Line01;
	GameObject Line02;
	GameObject CardBgEffect;
	List<GameObject> addTimeEff;
	ChangeNumber timeBarCN;
	ArrayList linesList = new ArrayList();
	
	Animator modelAnim;
	
	
	UILabel TimeLabel;					//显示的时间的倍数//
//	UILabel AddCoinsTimesLabel;			//显示的获得金币的倍数//
	//ChangeNumber TimeBoxChange;		
	
	UISprite TimeBarSpr;				//显示时间的进度条//
	UISprite AddCoinsBarSpr;			//显示获得金币倍数的进度条//
	
	
	float curLeftTime ;					//当前bounes 所剩的时间//
	float totalTime ;					//bounes 持续的时间//
	float curAddCoinsTimes;				//当前bounes增加的获得金币的倍数//
//	float totalAddCoinsTimes;			//增加金币的最大倍数//
	float AddTimes;						//每次增加多少倍//
	float curAddCoinsBarScale;
	//float lineStartX = -800f;
	//float lineStartY = 400f;
	//float createNewLineY = -450f;
	//float lineEndY = -875f;
	//float lineEndX = 950f;
	float curGoldMul = 1.2f;
	float preGoldMul = -1f;
	int AddCoinsBarW = 1000 ;			//整个获得金币倍数的框的最大宽度//
	int AddTimesW = 250;						//每次增加0.5倍，需要的数值//
	int ClickAddNum;					//每次点击增加的数值//
	int ReduceNum;						//每0.5秒减少的数量//
	int curClickNum;					//当前点击的次数//
	Vector3 AddCoinsBarStartScale;		//金币进度条的scale初始值//
//	Vector3 TimeBarStartScale;	
	//string linePrefabsPath = "";
	int bounesHitEff_l = 10001;			//大金币特效//
	int bounesHitEff_s = 10002;			//金币小特效//
	int bounesGiftId = 10001;			//获得奖励物品时的特效id//
	int bonusAddTimesEff = 10005;		//增加倍率时在节点添加的特效//
	int addTimeEffPosId = 0;
	ArrayList hitEffList = new ArrayList();
	float keepTime_s = 0.5f;
	float curTime;
	//float keepTime_l = 1f;
	float mouseDownX_L ;
	float mouseDownX_R ;
	float mouseDownY_T;
	float mouseDownY_D;
	//减少进度条的计时器//
	float reduceFrameCount;
	
	
	//bool isCreateHitEff = false;
	bool isCanCountDown = false;
	//创建奖励大特效后0.2秒在让屏幕泛白，//
	bool isCountBounesGiftEffect = false;
	bool isEndBounes = false;
	float bounesGiftEffFrame = 0;
	
	//bounes掉落物品领取标识//
	//int bounesDropMark;
	
	//当前屏幕泛白的类型，1淡入，2淡出//
	int curWhiteBgType ;
	
	int bonusKoNum;
	
	bool isGuidePauseInStart = false;
	bool isGuidePauseInEnd = false; 
	
	
	void Awake(){
		mInstance = this;
		_MyObj = mInstance.gameObject;
		BounesCamera = _MyObj.GetComponent<Camera>();
		//mainCamera = Camera.main;
		init();
		hide();
//		SetData("card07");
//		show();
	}

	// Use this for initialization
	void Start () {
		if(TimeBarSpr == null && TimeBoxObj != null)
		{
			Transform TimeBar = TimeBoxObj.transform.FindChild("TimeBar");
			if(TimeBar != null)
			{
				TimeBarSpr = TimeBar.GetComponent<UISprite>();
				TimeBarSpr.fillAmount = 1;
			}
		}
		
	}
	
	public override void init ()
	{
		base.init ();
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL){
			//读表//
			initData();
			
			
			if(TimeBoxObj != null){
				TimeLabel = TimeBoxObj.transform.FindChild("TimeBar/TimeNumLabel").GetComponent<UILabel>();
				//TimeBoxChange = TimeLabel.gameObject.GetComponent<ChangeNumber>();
			}
			
			if(AddCoinsBoxObj != null){
				AddCoinsBarSpr = AddCoinsBoxObj.transform.FindChild("AddCoinsBar").GetComponent<UISprite>();
//				AddCoinsBarStartScale = AddCoinsBarSpr.transform.localScale;
				AddCoinsBarSpr.fillAmount = 0;
			}
		}
		
		WhiteBg.SetActive(false);
		GiftCard.gameObject.SetActive(false);
		GetCardPanel.SetActive(false);
		ClickTipObj.SetActive(false);
		addTimeEff = new List<GameObject>();
		addTimeEffPosId = 0;
		bonusKoNum = 0;
		
	}
	
	public void initData(){
		int missionId = PlayerInfo.getInstance().brj.md;
		if(missionId <= 0){
			missionId = 110102;
		}
		totalTime = MissionData.getData( missionId).lastTime;
		curLeftTime = totalTime;
		curAddCoinsTimes = 1.5f;
//		totalAddCoinsTimes = 3.5f;
		AddTimes = 0.5f;
		AddCoinsBarW = 1000;
		AddTimesW = AddCoinsBarW / 4;
		ClickAddNum = 50;
		ClickAddNum = MissionData.getData( missionId).bonusGrow;
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
		{
			ReduceNum = 0;
		}
		else
		{
			ReduceNum = MissionData.getData(missionId).bonusReduce;	
		}
		//		Debug.Log("missionId ================= " + missionId + " - " + "ClickAddNum" + ClickAddNum );
		curClickNum = 0;
		curAddCoinsBarScale = 0.0001f;
		curGoldMul = 1.2f;
		preGoldMul = curGoldMul;
		linesList.Clear();
		mouseDownX_L = Screen.width/3;
		mouseDownX_R = Screen.width*2/3;
		mouseDownY_D = Screen.height/5;
		mouseDownY_T = Screen.height*2/3;
		
	}
	
	public void resetData(){
		initData();
		
		if(AddCoinsBarSpr != null){
//			AddCoinsBarSpr.transform.localScale = Vector3.one ;
			AddCoinsBarSpr.fillAmount = 0;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isPlayTheName("BounesLayer.hurt"))
		{
			modelAnim.SetBool("bounes_fly2hurt", false);
		}
		if(isGuidePauseInStart || isGuidePauseInEnd)
			return;
//		Debug.Log("curLeftTime ====== " +curLeftTime + "_" + "timeBarCN.GetShowNum() =============== " + timeBarCN.GetShowNum());
		if( !isEndBounes && (curAddCoinsBarScale >= 1 || curLeftTime <= 0) )
		{
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
			{
				isGuidePauseInEnd = true;
				DialogPanel.mInstance.showGuideDialogID(6);
			}
			else
			{
				finishBounes();
			}
			
		}
		
		if(isCountBounesGiftEffect)
		{
			bounesGiftEffFrame += Time.deltaTime;
			if(bonusKoNum < 2)
			{
				if(bounesGiftEffFrame > 0.2f){
					isCountBounesGiftEffect = false;
					bounesGiftEffFrame = 0;
					
					//创建ko图片，ko显示完后在开始屏幕泛白//
					CreateKOAnim();
				}
				
			}
			else 
			{
				if(bounesGiftEffFrame > 0.5f){
					isCountBounesGiftEffect = false;
					bounesGiftEffFrame = 0;
					
					//white fade in屏幕开始泛白//
					ChangeSceneWhite(1);
					for(int i = 0;i < 2; i++)
					{
						KOObjs[i].SetActive(false);
					}
				}
				
				
			}
		}
		
		
		curTime += Time.deltaTime;
		//每过0.5秒减少一次//
		if(curAddCoinsBarScale < 1 && curLeftTime > 0)
		{	
			reduceFrameCount += Time.deltaTime;
			if(reduceFrameCount > 0.5f){
				ReduceCoinsBar();
				reduceFrameCount = 0;
			}
			
			//修改时间条//
		}
		CheckEffect();
		
	}
	
//	public void OnGUI()
//	{
//		if(GUI.Button(new Rect(200,200,100,50), "开始"))
//		{
//			KOObjs[0].SetActive(false);
//			KOObjs[1].SetActive(false);
//			CreateKOAnim();
//		}
//	}
	
	//创建ko动画//
	public void CreateKOAnim()
	{
		
		if(bonusKoNum < 2)
		{
			
			KOObjs[bonusKoNum].SetActive(true);
			GameObjectUtil.PlayerMoveAndScaleAnim(KOObjs[bonusKoNum], 0.3f, iTween.EaseType.easeInOutBounce, _MyObj, "CreateKOAnim");
			bonusKoNum ++;
		}
		else 
		{
//			bonusKoNum = 0;
//			return;
//			ChangeSceneWhite(1);
			MissionData md = MissionData.getData(PVESceneControl.mInstance.GetCurMissionId());
			string rewardData = md.specialaward1;
			string[] str = rewardData.Split(',');
			int id = StringUtil.getInt( str[0]);
			if(id > 0)	//有掉落物品//
			{
				isCountBounesGiftEffect = true;
			}
			else 		//没有掉落物品，可以直接切界面//
			{
				ClickTipObj.SetActive(true);
			}
			
		}
	}
	
	public void finishBounes()
	{
		isGuidePauseInEnd = false;
		isEndBounes = true;
		//停止计时//
		timeBarCN.StopChange();
		//MissionData md = MissionData.getData(PVESceneControl.mInstance.GetCurMissionId());
		//int missionType = md.type;
//		if(bounesDropMark == 0 && curAddCoinsBarScale >= 1)		//未领取奖励，则播放领取动画，既先拨特效，然后再播放开箱子//
		if(curAddCoinsBarScale >= 1 || curLeftTime <= 0)		//点击完成后//
		{
			//隐藏人物模型//
			CurModel.SetActive(false);
			//创建大特效//
			UIEffectData ued = UIEffectData.getData(bounesGiftId);
			string path = ued.path + ued.name;
			float keepTime = ued.keepTime;
			CreateHitEffect(path ,Vector3.zero, keepTime);
			isCountBounesGiftEffect = true;
		}
//		else if(curLeftTime <= 0)
//		if(bounesDropMark == 1 || curLeftTime <= 0)
//		{							//已领取过物品，直接跳转界面//
//			//关闭当前界面，并切换到结算界面//
//			ChangeInterface();
//		}
	}
	
	//切换界面//
	public void ChangeInterface(){
		//关闭当前界面，并切换到结算界面//
	
		PVESceneControl.mInstance.SetBounesGlodMul(curGoldMul);
		PVESceneControl.mInstance.SetIsPlayBounes(false);
		hide();
	}
	
	//创建屏幕泛白，type = 1,淡入， type= 2， 淡出//
	public void ChangeSceneWhite(int type){
		curWhiteBgType = type;
		WhiteBg.SetActive(true);
		float start = 0;
		float to = 1;
		if(type == 2){
			start = 1;
			to = 0;
		}
		GameObjectUtil.PlayTweenAlpha(WhiteBg, start, to, "SceneWhiteFadeCallBack", 0.3f);
	}
	
	public void SceneWhiteFadeCallBack(){
		if(curWhiteBgType == 1){	//淡入结束后，显示卡牌，并设置卡牌信息,然后使白图片淡出//
			ShowGiftCard();
			
			//设置泛白fadeOut//
			ChangeSceneWhite(2);
		}
		else if(curWhiteBgType == 2)		//淡出，则隐藏图片，显示点击屏幕提示//
		{
			WhiteBg.SetActive(false);
			ClickTipObj.SetActive(true);
			
		}
	}
	
	public void ShowGiftCard(){
		MissionData md = MissionData.getData(PVESceneControl.mInstance.GetCurMissionId());
		int type = md.specialtype1;
		string rewardData = md.specialaward1;
		string[] str = rewardData.Split(',');
		int id = StringUtil.getInt( str[0]);
		if(type > 0 && id > 0)
		{
			
			GiftCard.gameObject.SetActive(true);
			GetCardPanel.SetActive(true);
			int num = 0;
			if(str.Length > 1)
			{
				num = StringUtil.getInt(str[1]);//如果是卡牌则表示等级，如果是其他的则表示数量//
			}
			GameHelper.E_CardType ct = GameHelper.E_CardType.E_Null;
			//UIAtlas at = CardAtlas;
			switch(type){
			case 1:			//items//
				ct = GameHelper.E_CardType.E_Item;
				//at = itemAtlas;
				break;
			case 2:			//equip//
				ct = GameHelper.E_CardType.E_Equip;
				//at = equipAtlas;
				break;
			case 3:			//card//
				ct = GameHelper.E_CardType.E_Hero;
				//at = CardAtlas;
				break;
			case 4:			//skill//
				ct = GameHelper.E_CardType.E_Skill;
				//at = SkillAtlas;
				break;
			case 5:			//passiveskill//
				ct = GameHelper.E_CardType.E_PassiveSkill;
				//at = PassiveSkillAtlas;
				break;
			}
			if(num > 0)
			{
				GiftCard.setSimpleCardInfo(id, ct);
				GiftCard.setNumText(num);
			}
			else 
			{
				GiftCard.setSimpleCardInfo(id, ct);
			}
			
			//创建特效//
			//创建背景特效//
			if(CardBgEffect != null)
			{
				Destroy(CardBgEffect);
			}
			if(CardBgEffect == null)
			{
				CardBgEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_starbackground",1)) as GameObject;
			}
			GameObjectUtil.gameObjectAttachToParent(CardBgEffect,CardParent.gameObject);
			GameObjectUtil.setGameObjectLayer(CardBgEffect,CardParent.gameObject.layer);
		}
	}
	
	//减少进度条//
	public void ReduceCoinsBar(){
		
		ChangeData(-ReduceNum);
	}
	
	void LateUpdate(){
		if(isGuidePauseInStart || isGuidePauseInEnd)
			return;
		
		if(!isEndBounes)
		{
			mouseCallBack();
		}
		
		if(isCanCountDown && curLeftTime != timeBarCN.GetShowNum()){
			curLeftTime = timeBarCN.GetShowNum();
			if(curLeftTime == 0 ){
				isCanCountDown = false;
			}
			//修改时间条//
			TimeBarSpr.fillAmount = curLeftTime / totalTime;
		}
	}
	
	public void mouseCallBack()
	{
		if(!NewBattleUnitePanel.mInstance.gameObject.activeSelf &&  Input.GetMouseButtonDown(0) && CurModel.activeSelf)
		{
			
			Vector3 posV3 = Input.mousePosition ;
			if(posV3.x >= mouseDownX_L && posV3.x <= mouseDownX_R && posV3.y >= mouseDownY_D && posV3.y <= mouseDownY_T)
			{
				showClickBounceEffect(posV3.x,posV3.y);
			}
		}
	}
	
	public void showClickBounceEffect(float x,float y)
	{
		//修改一下数据//
		ChangeData(ClickAddNum);
		int showEffId = bounesHitEff_s;
		bool isCreateAddTimeEff = false;
		if(preGoldMul > 0)
		{
			if(  preGoldMul < curGoldMul)
			{
				showEffId = bounesHitEff_l;
				preGoldMul = curGoldMul;
				//播放音效//
				MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BONUS_ADD_TIMES);
				//创建倍率特效//
				isCreateAddTimeEff = true;
				
			}
			else if(preGoldMul == curGoldMul)
			{
				showEffId = bounesHitEff_s;
				//播放音效//
				MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BONUS_CLIKC);
				
			}
		}
		//创建特效//
		Vector3 localPos = BounesCamera.ScreenToWorldPoint(new Vector2(x,y));
		localPos = new Vector3(localPos.x,localPos.y, -1);

		UIEffectData ued = UIEffectData.getData(showEffId);
		string path = ued.path + ued.name;
		CreateHitEffect(path ,localPos, keepTime_s);
		
		//创建增加倍率特效//
		if(isCreateAddTimeEff && addTimeEffPosId < TX_Pos.Length)
		{
			ued = UIEffectData.getData(bonusAddTimesEff);
			path = ued.path + ued.name;
			GameObject obj=Instantiate(GameObjectUtil.LoadResourcesPrefabs(path,-1)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(obj,HitEffectParent.gameObject);
			GameObjectUtil.setGameObjectLayer(obj,HitEffectParent.gameObject.layer);
			obj.transform.position = TX_Pos[addTimeEffPosId].transform.position;
			addTimeEffPosId++;
			addTimeEff.Add(obj);
			
		}
		
		//添加震动特效//
		iTween.ShakePosition(CurModel, iTween.Hash("x", 0.1, "time", 0.3));
	}
	
	
	public void OnClickBtn(){
		if(ClickTipObj != null && ClickTipObj.activeSelf ){
			ChangeInterface();
		}
	}
	

	//点击的时候是为正数，减少的时候是为负数//
	public void ChangeData(int num){
		if(curAddCoinsBarScale < 1)
		{	
//			isCreateHitEff = true;
			curClickNum+=num;
			if(curClickNum < 0){
				curClickNum = 0;
			}
			int times = curClickNum/AddTimesW;
			//只有增加的时候才计算，即如果点到3倍之后，在减少到2.5倍，则得到的仍然是3倍的奖励//
			float mul = curAddCoinsTimes + times * AddTimes;
			if(curGoldMul < mul){
				
				curGoldMul = mul;
			}
			curAddCoinsBarScale = ((float)curClickNum )/ AddCoinsBarW;
			if(curAddCoinsBarScale >= 1){
				curAddCoinsBarScale = 1f;
			}
			
			float scaleNum = curAddCoinsBarScale;
			if(scaleNum <=0){
				scaleNum = 0f;
			}
			
//			AddCoinsBarSpr.transform.localScale = new Vector3(AddCoinsBarStartScale.x * scaleNum,
//				AddCoinsBarStartScale.y, AddCoinsBarStartScale.z);
			AddCoinsBarSpr.fillAmount = scaleNum;
			
		}
	}
	
	public void ShowTimeBoxBarAndNum(){
		
	}
	
	public override void show ()
	{
		
		base.show ();
		
		//隐藏暂不显示的模块//
		WhiteBg.SetActive(false);
		GiftCard.gameObject.SetActive(false);
		GetCardPanel.SetActive(false);
		for(int i = 0;i < KOObjs.Length;i++)
		{
			KOObjs[i].SetActive(false);
		}
		//将时间流速还原，在bounes界面不修改时间流速//
//		GameObjectUtil.AddSpeed(1f);
		UIInterfaceManager.mInstance.doSpeedChange(STATE.SPEED_NORMAL);
		if(timeBarCN == null)
		{
			timeBarCN = TimeLabel.gameObject.GetComponent<ChangeNumber>();
		}
		timeBarCN.SetData(TimeLabel, curLeftTime, 0f, totalTime, -1f);
		if(TimeLabel!= null)
		{
			TimeLabel.text = totalTime.ToString();
		}
		
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
		{
			isGuidePauseInStart = true;
			DialogPanel.mInstance.showGuideDialogID(5);
			timeBarCN.StopChange();
			return;
		}
//		runBonuce();
		
		//显示bonus和合体技名字界面//
		if(NewBattleUnitePanel.mInstance != null)
		{
			NewBattleUnitePanel.mInstance.SetData(2, -1);
		}
	}
	
	public void runBonuce()
	{
		isGuidePauseInStart = false;
		timeBarCN.StartChange();
		isCanCountDown = true;
		
		//修改增加金币倍数的进度条//
		float scale = ((float)curClickNum)/AddCoinsBarW;
		if(scale <= 0)
		{
			scale = 0f;
		}
//		AddCoinsBarSpr.transform.localScale = new Vector3(AddCoinsBarStartScale.x * scale,
//			AddCoinsBarStartScale.y, AddCoinsBarStartScale.z);
		AddCoinsBarSpr.fillAmount = scale;
		
		
		//创建速度线//
		CreateSpeedLine();
		
	}
	
	//bounesMark bounes掉落物品领取标识，（0未领取，1已领取），如果未领取，则在bounes结束后掉落物品//
	public void SetData(string modelName, int bounesMark){
		show ();
		//bounesDropMark = bounesMark;
		initData();
		CurModel = Instantiate(GameObjectUtil.LoadResourcesPrefabs(modelName, STATE.PREFABS_TYPE_CARD))as GameObject;
		CurModel.transform.parent = ModelParent.transform;
		GameObjectUtil.setGameObjectLayer(CurModel, ModelParent.layer);
		CurModel.transform.localPosition = Vector3.zero;
		CurModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
		CurModel.transform.localScale = Vector3.one;
		modelAnim = CurModel.GetComponent<Animator>();
		if(modelAnim!= null)
		{
			modelAnim.SetBool("bounes_idle2fly", true);
		}
		CurModel.transform.localPosition = Vector3.zero;
		
		//隐藏刀光//
		//DaoGuangController daoGuang = CurModel.GetComponent<DaoGuangController>();
		//if(daoGuang != null && daoGuang.trail != null){
		//	daoGuang.trail.gameObject.SetActive(false);
		//}
	}
	
	//float moveTime =0.7f;
	public void CreateSpeedLine(){
		
		return ;
		
		//GameObject line = Instantiate(GameObjectUtil.LoadResourcesPrefabs("Bounes_Line", 4)) as GameObject;
		//line.transform.parent = LinesParent.transform;
		//line.transform.localPosition = new Vector3(lineStartX, lineStartY, 0);
		//line.transform.localScale = Vector3.one;
		//Vector3 localPos = new Vector3(lineEndX, lineEndY, 0);
		//Vector3 desPos = GameObjectUtil.localPosToWorldPosForGameObject(line, localPos);
		//GameObjectUtil.PlayMoveToAnim(line, desPos, moveTime, _MyObj, "LineMoveCallBack", 0.0f, line);
		//linesList.Add(line);
		
	}
	
	
	public void LineMoveCallBack(GameObject line){
		linesList.Remove(line);
		Destroy(line);
		
		//创建速度线//
		CreateSpeedLine();
	}
	
	public void CreateHitEffect(string effName, Vector3 pos, float keepTime){
		Effect hitEff = new Effect(effName, keepTime, 0, pos, pos, Quaternion.Euler(Vector3.zero), 
			curTime, STATE.LAYER_ID_NGUI, HitEffectParent, -1);
		hitEffList.Add(hitEff);
	}
	
	public void CheckEffect(){
		//如果特效过多，则强制删除一些特效//
		if(hitEffList.Count >=10){
			for(int i = 0;i < 5;){
				Effect eff = (Effect)hitEffList[i];
				hitEffList.Remove(eff);
				Destroy(eff.getEffectObj());
				eff.gc();
			}
		}
		//判断当前特效是否有失效的，若有，则删除该特效//
		for(int i = 0; i< hitEffList.Count;){
		    Effect eff = (Effect)hitEffList[i];
			if(!eff.isValid(curTime)){
				hitEffList.Remove(eff);
				Destroy(eff.getEffectObj());
				eff.gc();
			}
			else {
				i++;
			}
		}
	}
	
	public bool isPlayTheName(string name)
	{
		AnimatorStateInfo asi=modelAnim.GetCurrentAnimatorStateInfo(0);
		return asi.IsName(name);
	}
	
	
	public override void hide ()
	{
		Destroy(CurModel);
		if(CardBgEffect != null)
		{
			Destroy(CardBgEffect);
		}
		for(int i =0;i< linesList.Count;){
			GameObject line = (GameObject)linesList[i];
			Destroy(line);
			linesList.Remove(line);
		}
		linesList.Clear();
		isCanCountDown = false;
		isEndBounes = false;
		isCountBounesGiftEffect = false;
		//将时间流速修改为战斗中的时间流速//
		if(UIInterfaceManager.mInstance!=null){
			UIInterfaceManager.mInstance.doSpeedChange(UIInterfaceManager.mInstance .getCurScale());
//			GameObjectUtil.AddSpeed(UIInterfaceManager.mInstance .getCurScale());
		}
		if(TimeBarSpr != null)
		{
			
			TimeBarSpr.fillAmount = 1;
		}
		
		//清空特效//
		if(addTimeEff != null)
		{
			
			for(int i = addTimeEff.Count-1;i >= 0;i--)
			{
				GameObject go = addTimeEff[i];
				addTimeEff.Remove(go);
				Destroy(go);
			}
			addTimeEff.Clear();
		}
		addTimeEffPosId = 0;
		
		base.hide ();
	}
	
	public void gc()
	{
		CurModel=null;
		CardBgEffect=null;
		if(linesList!=null)
		{
			linesList.Clear();
			linesList=null;
		}
		if(hitEffList!=null)
		{
			foreach(Effect e in hitEffList)
			{
				e.gc();
			}
			hitEffList.Clear();
			hitEffList=null;
		}
		GameObject.Destroy(_MyObj);
		mInstance = null;
		_MyObj = null;
		//Resources.UnloadUnusedAssets();
	}
	
}
