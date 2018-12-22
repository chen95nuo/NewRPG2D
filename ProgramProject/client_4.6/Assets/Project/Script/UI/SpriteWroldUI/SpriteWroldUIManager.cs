using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpriteWroldUIManager : MonoBehaviour, ProcessResponse {
	
//	public static SpriteWroldUIManager mInstance;
	
	//五个npc//
	public GameObject[] npcList;
	//获取物品的十个格子//
	public GameObject[] itemGridList;
	//被动技能Altas//
	public UIAtlas unActiveSkillAltas;
	//item 的Altas//
	public UIAtlas itemAltas;
	//interface Atlas//
	public UIAtlas interface01Atlas;
	//背包空余//
	public UILabel packCanUseNum;
	//金币//
	public UILabel goldLabel;
	//获得物品提示//
	public GameObject GetItemTip;
	//提示的label//
	public UILabel GetItemLabel;
	
	//private Transform _myTransform;
	
	/**1 兑换 , 2 一键清空, 3 点击冥想按钮，获取数据， 4 退出, 5 点击强化按钮，获取界面信息， 6 获取购买金币界面信息**/
	private int requestType;
	private bool receiveData;
	
	//当前激活的npc在表中的id//
	int curNPCId;
	//获得的物品，要放的格子的id 0-9//
	int nextGridId;
	//当前剩余的背包格子数//
	int unUsedBackGrid;




    int mid; //当前玩家激活的领奖id编号//

    int mnum; //当前玩家的冥想次数//
	List<SpriteWroldItem> curItemList;
	//服务器返回的字符串//
	string receiveStr = "";
	//处理后获得的金币//
	int goldNum;
	//冥想前的金币//
	int oldGold;
	//兑换界面用到的屌丝碎片信息：碎片id-数量//
	List<PackElement> patchList;
	//错误代码//
	int errorCode;
	
	/************************请求购买购买start***************************/
	//购买物品的类型 1 金币， 2 体力， 3 扫荡券， 4 挑战次数//
	private int buyType;
	//请求类型,1 请求购买界面信息， 2 购买//
	private int jsonType;
	//花费类型 ，1 水晶， 2 金币//
	private int costType;
	//购买花费的水晶数//
	private int costCrystal;
	//要购买的金币或体力的个数//
	private int num;
	//当天剩余的购买次数//
	private int times;
	/************************请求购买购买end***************************/
	
	//背包的json//
	PackResultJson packRJ;
	
    List<int> itemID = new List<int>();

    List<int> itemTypes = new List<int>();

    public GameObject[] effect;   

    public GameObject[] reward;

    public UILabel rewardLabelNum;


    public UISprite rewardIcon;


	//bool waitForIntensify = false;
	
	//控制按钮是否可点，如果可点，设为true,然后发消息。等到消息接受完毕之后把这个变量设为false.如果2秒内还没有收到消息，自动把这个按钮设为true,即可点//
    bool isLock = true;
	float startTime;
	float endTime;

    public UISprite spriteIcon;

    public UILabel LabelNums;

	void Awake(){
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		//_myTransform = transform;
		init();
//		hide();
	}
	
	
	public void init ()
	{
//		base.init ();
		//修改npc中需要花费的金币数//
		for(int i =0;i < npcList.Length ; i++){
			UILabel num = npcList[i].transform.FindChild("Label").GetComponent<UILabel>();
			//获取冥想数据//
			ImaginationData imd = ImaginationData.getData(i + 1);
			num.text = imd.spend.ToString();
			GameObject npcLight = npcList[i].transform.FindChild("Light").gameObject;
			if(npcLight != null)
			{
				npcLight.SetActive(false);
			}
			GameObject npcBlack = npcList[i].transform.FindChild("Black").gameObject;
			if(npcBlack != null)
			{
				npcBlack.SetActive(true);
			}
		}
		curItemList = new List<SpriteWroldItem>();
		patchList = new List<PackElement>();
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData){
			receiveData = false;
			if(errorCode == -3)
				return;
			
			switch(requestType){
			case 1:			//兑换//
			case 2:			//一键清空//
			case 4:			//退出按钮//
			case 5:
				
				nextGridId = 0;
				//显示提示，并增加金币,之后再清空列表//
				if(curItemList.Count>0){
					
					ShowClearTip();
					//清空物品类表//
					CleanData();
					//修改背包提示文字//
					ChangePackTip();
					
//					//播放增加金币音效//
//					MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COIN);
					Dictionary<string,object> dic = new Dictionary<string, object>();
					dic.Add("GetGold",goldNum.ToString());
					TalkingDataManager.SendTalkingDataEvent("MeditationSaleGetGold",dic);
				}
				else {
					if(requestType == 1){
						if(errorCode == 0)
						{
							//跳转到兑换界面//
							//						ExchangeUIManager.mInstance.SetData(patchList);
							UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_EXCHANGE);
							ExchangeUIManager exchange = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_EXCHANGE, "ExchangeUIManager") as ExchangeUIManager;
							exchange.SetData(patchList);
							hide();
						}
					}
					else if(requestType == 2){
						//提示没有要清理的物品//
						string str = TextsData.getData(96).chinese;
						ToastWindow.mInstance.showText(str);
					}
					else if(requestType == 4){
						//跳转到主菜单界面//
						ChangeToMainUI();
					}
				}

				break;
			case 3:			//冥想按钮//
				if(errorCode == 0){
					
					isLock = true;

                    GetReward();
                    if (receiveStr.Equals("200"))
                    {
                        ToastWindow.mInstance.showText(TextsData.getData(652).chinese);
                        return;
                    }
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Spirit))
					{
						UISceneDialogPanel.mInstance.showDialogID(18);
					}
					
					//播放音效//
					MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_QH_SUCC);
					
					//激活npc//
					ChangeNPCShow();
					//添加物品到链表中，并且将获得的物品在格子中亮起//
					if(receiveStr != null && receiveStr != ""){
						
						AddToItemList(receiveStr);
					}
					refreshGoldInfo();
					int gold = PlayerInfo.getInstance().player.gold;
					HeadUI.mInstance.setPreGold(gold);
					
					Dictionary<string,object> dic = new Dictionary<string, object>();
					string eventName = "Meditation";
					int useGold = oldGold - PlayerInfo.getInstance().player.gold;
					string[] ss = receiveStr.Split('-');
					int type = StringUtil.getInt(ss[0]);
					int itemId = StringUtil.getInt(ss[1]);
					dic.Add("PlayerId",PlayerPrefs.GetString("username"));//id//
					dic.Add("UseGold",useGold.ToString());//使用金币//
					if(type == 1 || type == 3)		//item or 废品//
					{
						ItemsData itd = ItemsData.getData(itemId);
						if(itd.type == 3)
						{
							if(itd.star == 4||itd.star == 5)
							{
								dic.Add("SkillFragment",itd.star.ToString());//四星五星碎片//
							}
						}
					}
					TalkingDataManager.SendTalkingDataEvent(eventName,dic);//发送talkingdata//
				}
				else if(errorCode == 19){				//金币不足//
					//请求购买界面信息//
					
					requestType = 6;
					buyType = 1;
					jsonType = 1;
					costType = 1;
					PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType),this);
				}
                else if (errorCode == 131)
                {				//冥想任务奖励未领取不能继续冥想//
                    ToastWindow.mInstance.showText(TextsData.getData(746).chinese);
                    return;
                }
                else if (errorCode == 132)
                {				//冥想任务领取奖励条件不足//
                    ToastWindow.mInstance.showText(TextsData.getData(747).chinese);
                    return;
                }
				break;
				
			case 100:
			{
//				IntensifyPanel.mInstance.allCells=prj.list;
//				IntensifyPanel.mInstance.setIntensifyType(IntensifyPanel.INTENSIFYTYPE.TYPE_PASSIVESKILL);
//				IntensifyPanel.mInstance.show();
//				IntensifyPanel.mInstance.isOpenBySpirit = true;
				
				if(errorCode == 0)
				{
					UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY);
					IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
						"IntensifyPanel")as IntensifyPanel;
					intensify.allCells.Clear();
					for(int i=0;i<packRJ.pejs.Count;i++)
					{
						intensify.allCells.Add(packRJ.pejs[i].pe);
					}
					intensify.allFromIdList = packRJ.pejs;
					intensify.setIntensifyType(IntensifyPanel.INTENSIFYTYPE.TYPE_PASSIVESKILL);
					intensify.show();
					intensify.isOpenBySpirit = true;
					HeadUI.mInstance.show();
					
					hide ();
					packRJ = null;
				}
				else if(errorCode == 56)
				{
					ToastWindow.mInstance.showText(TextsData.getData(678).chinese);
				}
				
			}break;
				
			case 6:					//请求购买金币界面信息//
				
				isLock = true;
				
				if(errorCode == 0)
				{
					BuyTipManager.mInstance.SetData(buyType, costType , costCrystal, num, times, 0, BuyTipManager.UI_TYPE.UI_SWRITEWORLD, 0);
				}
				else if(errorCode == 19)			//水晶不足,去商城//
				{
					//跳转到商城界面//
					string str = TextsData.getData(244).chinese;
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
					hide ();
				}
				else if(errorCode == 79)			//vip等级不足   go购买次数已达上限//
				{
					string str = TextsData.getData(508).chinese;
//					ToastWindow.mInstance.showText(str);

					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
					
				}
				else if(errorCode == 80)
				{
					Debug.Log("Request type error!");
				}
				break;
			}
		}
		
		//如果网络响应延迟时间超过2秒，则自动设置按钮状态为激活状态//
		CheckElapseTime();
	}

    void GetReward()
    {
        MeditationData td = MeditationData.GetData(mid);
        LabelNums.text = mnum + " / " + td.timeNumber;
        List<string> s = td.reward;
        rewardLabelNum.text = s[0].Split('-')[0];
        spriteIcon.spriteName = ItemsData.getData(td.itemID).icon;
    }
    public void RewardsClick(int param)
    {
        if (RewardsDatasControl.mInstance.gameObject.activeSelf)
        {
            RewardsDatasControl.mInstance.hide();
        }
        else
        {
            ShowReward(param);
        }
    }
    //显示物品信息//
    public void ShowReward(int param)
    {
        //string iconName = "";
        string frameName = "";
        string name = "";
        string des = "";
        int sell = 0;
        int formID = itemID[param];
        int level = 0;
        int type = 0;
        GameHelper.E_CardType cardType = GameHelper.E_CardType.E_Null;

        switch (itemTypes[param])
        {
            case 1:				//items//
                ItemsData item = ItemsData.getData(itemID[param]);
                if (item == null)
                    return;
                cardType = GameHelper.E_CardType.E_Item;
                //iconName = item.icon;
                name = item.name;
                des = item.discription;
                sell = item.sell;
                break;
            case 2:				//equip//
                PassiveSkillData psd = PassiveSkillData.getData(itemID[param]);
                if (psd == null)
                    return;
                cardType = GameHelper.E_CardType.E_PassiveSkill;
                //iconName = psd.icon;
                name = psd.name;
                des = psd.describe;
                sell = psd.sell;
                level = psd.level;
                break;
            case 3:				//card//
                ItemsData itd = ItemsData.getData(itemID[param]);
                if (itd == null)
                    return;
                cardType = GameHelper.E_CardType.E_Item;
                //iconName = itd.icon;
                name = itd.name;
                des = itd.discription;
                sell = itd.sell;
                break;
         
        }
        
        RewardsDatasControl.mInstance.SetData(formID, cardType, name, frameName, des, level, sell, type);

        Vector3 vt = reward[param].transform.position;
        RewardsDatasControl.mInstance.transform.position = new Vector3(vt.x+0.26f, vt.y + 0.4f, 0f);
    }
	
	//修改npc的显示//
	public void ChangeNPCShow(){
		for(int i = 0 ;i < npcList.Length;i++){
			GameObject npc = npcList[i];
			GameObject npcLight = npc.transform.FindChild("Light").gameObject;
			GameObject npcBlack = npcList[i].transform.FindChild("Black").gameObject;
			
			if(i == curNPCId - 1){
//				npc.transform.FindChild("Light").gameObject.SetActive(true);
				npcLight.SetActive(true);
				npcBlack.SetActive(false);
			}
			else {
				npcLight.SetActive(false);
				npcBlack.SetActive(true);
//				npc.transform.FindChild("Light").gameObject.SetActive(false);
			}
		}
		
	}
	
	//修改背包提示文字//
	public void ChangePackTip(){
		string ss = TextsData.getData(80).chinese;
		packCanUseNum.text = ss + unUsedBackGrid;
	}

    //显示提示//
    public void ShowClearTip()
    {
        if (!GetItemTip.activeSelf)
        {
            GetItemTip.SetActive(true);
        }
        string s1 = "[F0F0A0]" + TextsData.getData(57).chinese + "[-]";		//获得物品//
        string s2 = "[F0F0A0]" + TextsData.getData(81).chinese + "[-]";		//处理物品//
        string s3 = "[F0F0A0]" + TextsData.getData(82).chinese + "[-]";		//获得金币// 
        string str0 = "";				//获得的被动技能//
        string str1 = "";				//获得的物品//
        string str2 = "";				//处理的废品//
        string str3 = s3 + goldNum;		//获得金币//
        int lineNum = curItemList.Count + 7;
        foreach (SpriteWroldItem item in curItemList)
        {
            if (item.type == 1)
            {				//废品//
                ItemsData itd = ItemsData.getData(item.id);
                str2 += "\r\n" + itd.name + "*" + "[00F0F0]" + item.num + "[-]";
            }
            else if (item.type == 2)
            {		//被动技能//
                PassiveSkillData psd = PassiveSkillData.getData(item.id);
                str0 += "\r\n" + psd.name + "*" + "[00F0F0]" + item.num + "[-]";
            }
            else if (item.type == 3)
            {		//item//
                ItemsData itd = ItemsData.getData(item.id);
                str1 += "\r\n" + itd.name + "*" + "[00F0F0]" + item.num + "[-]";
            }
        }
        string ss = s1 + ":" + str0 + str1 + "\r\n" + "\r\n" + s2 + str2 + "\r\n" + "\r\n" + str3;
        GetItemLabel.text = ss;
        GetItemLabel.height = lineNum * GetItemLabel.fontSize;
    }
	
	public void refreshGoldInfo()
	{
		goldLabel.text = PlayerInfo.getInstance().player.gold.ToString();		
	}
	
	//清空格子中数据以及列表//
	public void CleanData(){
		foreach(GameObject obj in itemGridList){
			obj.GetComponent<SpritWroldGridItem>().Child.SetActive(false);
		}
		
		curItemList.Clear();
		
		nextGridId = 0;
		
		receiveStr = null;
		
	}
	
	public void show ()
	{
//		base.show ();
	}
	
	
	public void SetData(int nullGrid, int npcId,int mid,int mnum){
		show();
		this.unUsedBackGrid = nullGrid;
//		unUsedBackGrid = 9;
		this.curNPCId = npcId;

        this.mid = mid;
        this.mnum = mnum;
        GetReward();
//		goldLabel.text = PlayerInfo.getInstance().player.gold.ToString();
		refreshGoldInfo();
		GetItemTip.SetActive(false);
		ChangePackTip();
		ChangeNPCShow();
		CleanData();
		
		//播放声音//
		string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_SPRITEWORLD).music;
		MusicManager.playBgMusic(musicName);
	}
	
	public void SetUnUsedBackGrid(int num)
	{
		this.unUsedBackGrid = num;
	}
	
	
	public void hide ()
	{
//		base.hide ();
		gc();
		
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD);
	}
	
	private void gc()
	{
		if(curItemList != null)
		{
			curItemList.Clear();
		}
		if(patchList != null)
		{
			
			patchList.Clear();
		}
		itemAltas = null;
		interface01Atlas = null;
		unActiveSkillAltas = null;
	}

    public void IdClear()
    {
        itemID.Clear();
        //itemID.Add(itemid);
        itemTypes.Clear();
        //itemTypes.Add(itemt);
    }
	
	//想物品类表中添加数据 ss 是要添加的物品的信息。格式：类型-id//
	public void AddToItemList(string ss){
		
		string[] str = ss.Split('-');
		int type = -1;
		int id = -1;

		if(str.Length>0){
			type = StringUtil.getInt(str[0]);
		}
		if(str.Length>1){
			id = StringUtil.getInt(str[1]);
		}
		
		//想物品类表中添加数，如果类表中没有数据，则直接填，若有相同物品，则将该物品的个数+1//
		bool isAdd = true;
		if(curItemList != null){
			if(curItemList.Count > 0){
				for(int i =0;i< curItemList.Count; i++){
					SpriteWroldItem swi = curItemList[i];
					if(swi.type == type && swi.id == id){
						swi.num += 1;
						isAdd = false;
					}
				}
				
				if(isAdd){
					SpriteWroldItem swi = new SpriteWroldItem(type, id, 1);
					curItemList.Add(swi);
				}
			}
			else {
				SpriteWroldItem swi = new SpriteWroldItem(type, id, 1);
				curItemList.Add(swi);
			}
		}
		
		
		//将获得的物品放到格子中//
		GameObject obj = itemGridList[nextGridId];
		SpritWroldGridItem swgi = obj.GetComponent<SpritWroldGridItem>();
		swgi.Child.SetActive(true);
		//string iconName = "";
		string name = "";
		//int star = 1;

		if(type == 1 || type == 3)
		{		//item or 废品//
			ItemsData item = ItemsData.getData(id);
			if(item != null)
			{
				swgi.cardInfo.setSimpleCardInfo(id,GameHelper.E_CardType.E_Item);
                if (itemID.Count > 9 && itemTypes.Count > 9)
                {
                    IdClear();
                   
                }
                itemID.Add(id);
                itemTypes.Add(type);
                showEffect(itemGridList[nextGridId].transform.localPosition);
			}
			name = item.name;
		}
		else if(type == 2)
		{				//被动技能//
			PassiveSkillData psd = PassiveSkillData.getData(id);
			if(psd != null)
			{
				swgi.cardInfo.setSimpleCardInfo(id,GameHelper.E_CardType.E_PassiveSkill);
                if (itemID.Count > 9 && itemTypes.Count > 9)
                {
                    IdClear();
                  
                }
                itemID.Add(id);
                itemTypes.Add(type);
                showEffect(itemGridList[nextGridId].transform.localPosition);
			}
			name = psd.name;
		}
		swgi.Name.text = name;
		nextGridId ++;
	}
    public void showEffect(Vector3 v2)
    {
        if (nextGridId > 9)
        {
            return;
        }
        string path = "Prefabs/Effects/UIEffect/flyingspark";
        GameObject loadPrefab = Resources.Load(path) as GameObject;
        GameObject effectObj = GameObject.Instantiate(loadPrefab) as GameObject;
        //GameObject effectPosObj = effect[curNPCId-1];
        GameObjectUtil.gameObjectAttachToParent(effectObj, effect[0]);
        effectObj.AddComponent<TweenPosition>();
        effectObj.transform.localPosition = effect[curNPCId-1].transform.localPosition;
        effectObj.GetComponent<TweenPosition>().enabled = true;
        effectObj.GetComponent<TweenPosition>().from = npcList[curNPCId - 1].transform.localPosition;
        effectObj.GetComponent<TweenPosition>().to = v2;
        effectObj.GetComponent<TweenPosition>().duration = 0.1f;
        effectObj.GetComponent<TweenPosition>().tweenFactor = 0;
        effectObj.GetComponent<TweenPosition>().PlayForward();
        go.Add(effectObj);
        i++;
        EffectFinish();
        
    }

    int i = 0;

    List<GameObject> go = new List<GameObject>();

    public void EffectFinish()
    {
        Destroy(go[i-1],0.5f);
    }
	//将物品列表变换成给服务器发送的字符串类表//
	public List<string> createStrList(List<SpriteWroldItem> swiList){
		List<string> str = new List<string>();
		if(swiList != null){
			for(int i = 0;i < swiList.Count;i++){
				SpriteWroldItem swi = swiList[i];
				string s = swi.type + "-" + swi.id + "-" + swi.num;
				str.Add(s);
			}
		}
		return str;
	}
	
	
	//按键响应1 兑换， 2 一键清空， 3 冥想， 4 退出(退出时也发送一键清空请求),5 intensify//
	public void OnClickBtn(int id){
		
		switch(id)
		{
		case 1:			//兑换//
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			requestType = id;
			PlayerInfo.getInstance().sendRequest(new ImaginationClearJson(createStrList(curItemList), 2), this);
		}break;
		case 2:			//一键清空//
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(curItemList.Count>0)
			{
				requestType = id;
				PlayerInfo.getInstance().sendRequest(new ImaginationClearJson(createStrList(curItemList), 1), this);
			}
			else 
			{
				//提示没有要清理的物品//
				string str = TextsData.getData(96).chinese;
				ToastWindow.mInstance.showText(str);
			}
		}break;
		case 3:			//冥想按钮//
		{

            doMuse();
		}break;
		case 4:			//退出按钮//
		{
			
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			
			if(curItemList.Count > 0)
			{
				requestType = id;
				PlayerInfo.getInstance().sendRequest(new ImaginationClearJson(createStrList(curItemList), 1), this);
			}
			else 
			{
				ChangeToMainUI();
			}
		}break;
		case 5:
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			
			//waitForIntensify = true;
			
			
			if(curItemList.Count > 0)
			{
				requestType = id;
				PlayerInfo.getInstance().sendRequest(new ImaginationClearJson(createStrList(curItemList), 1), this);
			}
			else 
			{
				requestType = 100;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,(int)IntensifyPanel.INTENSIFYTYPE.TYPE_PASSIVESKILL),this);
			}
			
		}break;

        case 6:
            {
                //播放音效//
                MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);

                requestType = 3;
                PlayerInfo.getInstance().sendRequest(new ImaginationClickJson(-1), this);
             

            } break;
		}
	}
	
	public void doMuse()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		//如果背包空间不足则提示清理背包//
		if(unUsedBackGrid < 10)
		{
//			string str = TextsData.getData(78).chinese;
//			ToastWindow.mInstance.showText(str);
			string str = TextsData.getData(78).chinese;
			//UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_PACKAGE, str);
			UIJumpTipManager.mInstance.SetPackageTypeData(UIJumpTipManager.UI_JUMP_TYPE.UI_EXTENDPACKAGE, str);
			UIJumpTipManager.mInstance.SetSpriteWorldBool(true);
		}
		else 
		{
            if (isLock)
            {
                //冥想界面最多一次能获得10个物品//
                if (nextGridId < itemGridList.Length)
                {
                    requestType = 3;
                    PlayerInfo.getInstance().sendRequest(new ImaginationClickJson(curNPCId), this);
					isLock = false;
					startTime = Time.time;
					endTime = Time.time;
                }
                else
                {
                    string str = TextsData.getData(83).chinese;
                    ToastWindow.mInstance.showText(str);
                }
//                isLock = false;
            }
		}
	}
	
	void CheckElapseTime()
	{
		if(!isLock)
		{
			endTime += Time.deltaTime;
			if(endTime - startTime > 2.0f)
			{
				isLock = true;
			}
		}
	}
	
	//dragPanel退出响应//
	public void OnClickDragBack(){
		int gold = PlayerInfo.getInstance().player.gold;
		HeadUI.mInstance.setPreGold(gold);
//		goldLabel.text = PlayerInfo.getInstance().player.gold.ToString();
		refreshGoldInfo();
		GetItemTip.SetActive(false);
		if(requestType == 1){
			//跳转到兑换界面//
//			ExchangeUIManager.mInstance.SetData(patchList);
			UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_EXCHANGE);
			ExchangeUIManager exchange = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_EXCHANGE, "ExchangeUIManager") as ExchangeUIManager;
			exchange.SetData(patchList);
			hide();
		}
		else if(requestType == 2){
			
		}
		else if(requestType == 4){
			ChangeToMainUI();
		}
		else if(requestType == 5)
		{
			requestType = 100;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,(int)IntensifyPanel.INTENSIFYTYPE.TYPE_PASSIVESKILL),this);
		}
	}
	
	public void ChangeToMainUI(){
//		MainUI.mInstance.show();
		//返回主城界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU,
			"MainMenuManager") as MainMenuManager;
		main.SetData(STATE.ENTER_MAINMENU_BACK);
		
		HeadUI.mInstance.show();
		hide();
	}
	
	public void receiveResponse (string json)
	{
		
		if(json != null){
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			
			Debug.Log("SpriteWroldUIManager    json : " + json);
			switch(requestType){
			case 1:			//兑换 , 一键清空，并获取兑换界面数据，即屌丝碎片的个数//
			case 2:			//一键清空//
			case 4:			//退出//
			case 5:			//go to intensify ,clear items // 
				ImaginationClearResultJson icrj=JsonMapper.ToObject<ImaginationClearResultJson>(json);
				errorCode = icrj.errorCode;
				if(errorCode == 0){
					
					goldNum = icrj.gold;		//清理后获得金币数//
					unUsedBackGrid = icrj.i;	//背包空余格子数//
					patchList = icrj.s;			//兑换界面的屌丝碎片信息//
					PlayerInfo.getInstance().player.gold = icrj.g;
				}
				receiveData = true;
				break;
				
			case 3:			//冥想//
				ImaginationClickResultJson iclrj=JsonMapper.ToObject<ImaginationClickResultJson>(json);
				errorCode =iclrj.errorCode;
				if(errorCode == 0){
					
					curNPCId = iclrj.id;	 //激活的npc//
					receiveStr = iclrj.s;	 //服务器返回的字符串，格式：类型-物品id//
                    mid = iclrj.mid;
                    mnum = iclrj.mnum; 
					oldGold = PlayerInfo.getInstance().player.gold;
					PlayerInfo.getInstance().player.gold = iclrj.g;
				}
				receiveData = true;
				break;
			case 100:  // intensify   获取强化界面信息//
			{
				PackResultJson prj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
				if(errorCode == 0)
				{
					packRJ = prj;
				}
				receiveData=true;
			}break;
			case 6:							//请求购买金币信息界面//
				BuyPowerOrGoldResultJson brj = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = brj.errorCode;
				this.costCrystal = brj.crystal;
				this.num = brj.num;
				this.times = brj.times;	
				
				receiveData=true;
				break;
			}
		}
	}
}
