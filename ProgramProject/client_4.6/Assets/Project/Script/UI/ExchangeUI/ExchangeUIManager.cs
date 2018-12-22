using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExchangeUIManager : MonoBehaviour, ProcessResponse {
	
//	public static ExchangeUIManager mInstance;
	public GameObject[] patchLabels;
	public GameObject GridList;
	public GameObject ScrollBar;
	public GameObject ScrollView;
	//创建的格子的obj//
	public GameObject gridItem;
	//各个物品的图集//
	public UIAtlas cardAtlas;
	public UIAtlas equipAtlas;
	public UIAtlas itemAtlas;
	public UIAtlas passiveSkillAtlas;
	
	private Transform _myTransform;
	/**1 兑换 **/
	private int requestType;
	private bool receiveData;
	private 
	//碎片的类表//
	List<PackElement> patchList;
	//兑换物品的列表//
	List<ExchangeItem> exchangeItemList;
	int[] patchIds = {70001, 70002, 70003};
	//存储三种碎片的个数//
	int[] patchNums = {0,0,0};
	
	//当前选中要兑换的物品//
	int curSelItemId;
	//兑换成功的物品的id//
	int resultExchangeId = -1;
	//错误数据//
	int errorCode;
	//冥想界面已经点亮的npc的ID//
	int mx_npcId;
	//背包剩余格子数//
	int nullPackNum;




    int mid; //当前玩家激活的领奖id编号//

    int mnum; //当前玩家的冥想次数//


	void Awake(){
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		_myTransform = transform;
		init();
//		hide();
	}
	

	// Use this for initialization
	void Start () {
		
	}
	
	public void init ()
	{
//		base.init ();
		patchList = new List<PackElement>();
		exchangeItemList = new List<ExchangeItem>();
		
		equipAtlas = LoadAtlasOrFont.LoadAtlasByName("EquipIcon");
		itemAtlas = LoadAtlasOrFont.LoadAtlasByName("ItemIcon");
		passiveSkillAtlas = LoadAtlasOrFont.LoadAtlasByName("PassiveSkillIcon");
		
	}
	
	
	// Update is called once per frame
	void Update () {
		if(receiveData){
			receiveData = false;
			if(errorCode == -3)
				return;
			switch(requestType){
			case 1:				//返回到冥想界面//
				
//				SpriteWroldUIManager.mInstance.SetData(nullPackNum, mx_npcId);
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD);
				SpriteWroldUIManager spriteWorld = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD, "SpriteWroldUIManager")as SpriteWroldUIManager;
                spriteWorld.SetData(nullPackNum, mx_npcId, mid, mnum);
				HeadUI.mInstance.hide();
				hide();
				break;
				
			case 2:				//兑换物品//
				if(errorCode == 0){			//兑换成功//
					string str = TextsData.getData(85).chinese;
					ToastWindow.mInstance.showText(str);
					//修改碎片数量//
//					ImaginationcomposeData icd = ImaginationcomposeData.getData(resultExchangeId);
//					for(int i = 0;i < patchIds.Length;i ++){
//						if(icd.material == patchIds[i]){
//							GameObject patch = patchLabels[i];
//							UILabel numLabel = patch.transform.FindChild("Num").GetComponent<UILabel>();
//							patchNums[i] -= icd.number;
//							numLabel.text = patchNums[i].ToString();
//						}
//					}
					ChangePatchLabel();
					
					Dictionary<string,object> dic = new Dictionary<string, object>();
					ImaginationcomposeData icd = ImaginationcomposeData.getData(resultExchangeId);
					PassiveSkillData psd = PassiveSkillData.getData(icd.composite);
					dic.Add("ExchangeID",psd.name);
					dic.Add("PlayerId",PlayerPrefs.GetString("username"));
					TalkingDataManager.SendTalkingDataEvent("ExchangePassiveSkill",dic);
				}
				else if(errorCode == 20){		//碎片不足//
					string str = TextsData.getData(86).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 21)		//兑换物品不存在//
				{
					string str = TextsData.getData(351).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 53)		//背包空间不足//
				{
					string str = TextsData.getData(78).chinese;
					ToastWindow.mInstance.showText(str);
				}
				
				resultExchangeId = -1;
				break;
			}
			
			
		}
	}
	
	public void show ()
	{
//		base.show ();
		CleanScrollView();
	}
	
	
	public void SetData(List<PackElement> str){
		show();
		//修改碎片个数的显示//
		patchList = str;
		for(int i = 0; i< patchLabels.Length;i++){
			ItemsData item = ItemsData.getData(patchIds[i]);
			GameObject patch = patchLabels[i];
			UISprite icon = patch.transform.FindChild("Icon").GetComponent<UISprite>();
			//因为有两处用这个信息，但是图片不同，所以当成图标的那种前面加ui_//
			icon.spriteName = "ui_" + item.icon;
			UILabel num = patch.transform.FindChild("Num").GetComponent<UILabel>();
			num.text = "0";
			patchNums[i] = 0;
		}
		
		ChangePatchLabel();
		curSelItemId = -1;
		
		//显示兑换界面//
		ShowExchangeInterface();
	}
	
	//修改界面上碎片的数据//
	public void ChangePatchLabel(){
		//清空碎片个数//
		for(int m = 0; m< patchIds.Length;m++){
			//ItemsData item = ItemsData.getData(patchIds[m]);
			GameObject patch = patchLabels[m];
			UILabel num = patch.transform.FindChild("Num").GetComponent<UILabel>();
			num.text = "0";
			patchNums[m] = 0;
			
			
		}
		//修改碎片个数//
		for(int i = 0; i< patchList.Count; i ++){
			PackElement pe = patchList[i];
			for(int m = 0; m< patchIds.Length;m++){
				if(pe.dataId == patchIds[m]){
					//ItemsData item = ItemsData.getData(patchIds[m]);
					GameObject patch = patchLabels[m];
					UILabel num = patch.transform.FindChild("Num").GetComponent<UILabel>();
					num.text = pe.pile.ToString();
					patchNums[m] = pe.pile;
				}
				
			}
		}
	}
	
	public void ShowExchangeInterface(){
		exchangeItemList.Clear();
		for(int i = 1;i<= ImaginationcomposeData.getCount();i++){
			GameObject item = Instantiate(gridItem) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(item,GridList);
			ExchangeItem ei = item.GetComponent<ExchangeItem>();
			UIButtonMessage message = item.GetComponent<UIButtonMessage>();
			message.target = _myTransform.gameObject;
			message.param = i;
			ImaginationcomposeData icd = ImaginationcomposeData.getData(i);
			//兑换物品在表里的id//
			int exchangeItemId = icd.composite;
			//兑换物品的icon的名字//
			//string spriteName = "empty";
			//物品的名字//
			string name = "";
			//星级//
			//int star = 1;
			//描述//
			string des = "";
			//兑换需要的物品//
			ItemsData costItem = ItemsData.getData(icd.material);
			//兑换需要消耗的物品的个数//
			int costNum = icd.number;
			//string s1 = TextsData.getData(93).chinese;
			//string s2 = TextsData.getData(94).chinese;
			string str = "x" + costNum;
			//UIAtlas atl = null;
			GameHelper.E_CardType exCardType = GameHelper.E_CardType.E_Null;
			
			//要兑换的物品的类型//
			switch(icd.type){
			case 1:				//卡牌//
				CardData cd = CardData.getData(exchangeItemId);
//				ei.Icon.atlas = cardAtlas;
//				spriteName = cd.icon;
				name = cd.name;
				//star = cd.star;
				//atl = cardAtlas;
				exCardType = GameHelper.E_CardType.E_Hero;

				des = "";
				break;
			case 2:				//装备//
				EquipData ed = EquipData.getData(exchangeItemId);
//				ei.Icon.atlas = equipAtlas;
//				spriteName = ed.icon;
				name = ed.name;
				//star = ed.star;
				des = ed.description;
				//atl = equipAtlas;
				
				exCardType = GameHelper.E_CardType.E_Equip;
				break;
			case 3:				//item//
				ItemsData itemData = ItemsData.getData(exchangeItemId);
//				ei.Icon.atlas = itemAtlas;
//				spriteName = itemData.icon;
				name = itemData.name;
				//star = itemData.star;
				des = itemData.discription;
				//atl = itemAtlas;
				exCardType = GameHelper.E_CardType.E_Item;
				
				break;
			case 4:				//被动技能//
				PassiveSkillData psd = PassiveSkillData.getData(exchangeItemId);
//				ei.Icon.atlas = passiveSkillAtlas;
//				spriteName = psd.icon;
				name = psd.name;
				//star = psd.star;
				des = psd.describe;
				//atl = passiveSkillAtlas;
				exCardType = GameHelper.E_CardType.E_PassiveSkill;
				break;
			}
			ei.cardInfo.setSimpleCardInfo(exchangeItemId, exCardType);
//			ei.Icon.spriteName = spriteName;
//			ei.Star.spriteName = "card_star0" + star;
			ei.Name.text = name;
			ei.Des.text = des;
			ei.CostIcon.spriteName = "ui_" + costItem.icon;
			ei.CostNum.text = str;
			ei.Selected.SetActive(false);
			ei.id = i;
			exchangeItemList.Add(ei);
		}
		
	}
	
	
	public void hide ()
	{
//		base.hide ();
		CleanData();
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_EXCHANGE);
	}
	
	private void gc()
	{
		patchList.Clear();
		equipAtlas = null;
		itemAtlas = null;
		passiveSkillAtlas = null;
	}
	
	public void CleanScrollView(){
		GridList.GetComponent<UIGrid2>().repositionNow = true;
		ScrollBar.GetComponent<UIScrollBar>().value = 0;
		ScrollView.transform.localPosition = Vector3.zero;
		ScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,620,380);
	}
	
	public void CleanData(){
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
		exchangeItemList.Clear();
	}
	
	
	//按键响应1， 退出，2， 兑换 //
	public void OnClickBtn(int id){
		requestType = id;
		switch(id){
		case 1:			//返回到冥想界面//
			
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SPRITEWROLD_INTO),this);
			break;
		case 2:			//兑换//
			
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			
			PlayerInfo.getInstance().sendRequest(new ImaginationComposeJson(curSelItemId),this);
			break;
		}
	}
	
	
	//兑换物品框的点击事件//
	public void OnClickItemBtn(int itemId){
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		curSelItemId = itemId;
		foreach(ExchangeItem ei in exchangeItemList){
			if(ei.id == itemId){
				if(!ei.Selected.activeSelf){
					
					ei.Selected.SetActive(true);
				}
			}
			else {
				if(ei.Selected.activeSelf){
					
					ei.Selected.SetActive(false);
				}
			}
		}
	}
	
	
	public void receiveResponse (string json)
	{
		if(json!= null){
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			Debug.Log("ExchangeUIManager : " + json);
			switch(requestType){
			case 1:			//返回获取冥想界面数据//
				ImaginationResultJson irj = JsonMapper.ToObject<ImaginationResultJson>(json);
				errorCode = irj.errorCode;
				if(errorCode == 0)
				{
					mx_npcId = irj.id;
					nullPackNum = irj.i;
                    mid = irj.mid;
                    mnum = irj.mnum;
					PlayerInfo.getInstance().player.gold = irj.g;
				}
				receiveData = true;
				break;
			case 2:			//兑换//
				ImaginationComposeResultJson icrj = JsonMapper.ToObject<ImaginationComposeResultJson>(json);
				errorCode = icrj.errorCode;
				if(errorCode == 0)
				{
					resultExchangeId = icrj.id;
					patchList = icrj.s;
				}
				receiveData = true;
				break;
			}
		}
	}
}
