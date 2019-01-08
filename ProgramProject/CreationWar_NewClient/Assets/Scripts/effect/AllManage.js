	  #pragma strict
	class UseMoneyReturnObject{
		var type : yuan.YuanPhoton.UseMoneyType;
		var obj : GameObject;
		var functionName : String;
	}
	class CostPowerReturnObject{
		var type : yuan.YuanPhoton.CostPowerType;
		var obj : GameObject;
		var functionName : String;
	}
	
class AllManage extends Song{
	//static var EffectGamepoolStatic : EffectGamepool;
	//var Effectpool : EffectGamepool;
	//static var BuffefmanageStatic : Buffefmanage;
	//var Buffefpool : Buffefmanage;
	//static var FontpoolStatic : Fontpool;
	//var Fontpools : Fontpool;
	//static var FuhaopoolStatic : ReGamePool;
	//var Fuhaopool :ReGamePool;
	//static var PickpoolStatic : PickupGamepool;
	//var Pickpool : PickupGamepool;
	//var invmaker : Inventorymaker;
	//static var InvmakerStatic : Inventorymaker;

	static var AllMge : AllManage;
	private var uicl : UIControl;
	static var UICLStatic : UIControl;
	private var invcl : InventoryControl;
	static var InvclStatic : InventoryControl;
	private var uiallPC : UIAllPanelControl;
	static var UIALLPCStatic : UIAllPanelControl;
	private var jiaochengCL : TaskJiaoChengControl;
	static var jiaochengCLStatic : TaskJiaoChengControl; 
	private var ts : TiShi;
	static var tsStatic : TiShi;
	private var qr : QueRen;
	static var qrStatic : QueRen; 
	private var uus : UseItemNum; 
	static var uusStatic : UseItemNum;
	private var dungcl : DungeonControl;
	static var dungclStatic : DungeonControl;
	private var mtw : MainTaskWork;
	static var mtwStatic : MainTaskWork;
	private var ItMove: ItemMove;
	static var ItMoveStatic : ItemMove;
	private var PlyUICl : PlayerUIControl;
	static var PlyUIClStatic : PlayerUIControl;
	var uicam : Camera;
	static var uicamStatic : Camera;
	var uicam2 : Camera;
	static var uicam2Static : Camera;
	var quanSoul : UISprite;
	static var quanSoulStatic : UISprite;
	var yuanBM : BtnGameManager;
	static var yuanBMStatic : BtnGameManager;
	var btnGMB : BtnGameManagerBack;
	static var btnGMBStatic : BtnGameManagerBack;
	private var btnInfo : ButtonForwardInfo;
	static var btnInfoStatic : ButtonForwardInfo;
	private var pDead : PlayerDead;
	static var pDeadStatic : PlayerDead;
	var pvpQuan : UISprite;
	static var pvpQuanStatic : UISprite;
	private var SkillCL : SkillControl;
	static var SkillCLStatic : SkillControl;
	private var WakCL : WaKControl;
	static var WakCLStatic : WaKControl;
	private var areCL : ArenaControl;
	static var areCLStatic : ArenaControl;
	var timeDJ : TimeDaoJi;
	static var timeDJStatic : TimeDaoJi;
	var SaoDangDJ : TimeDaoJi;
	static var SaoDangDJstatic : TimeDaoJi;
	private var inventoryProduce : InventoryProduceControl;
	static var inventoryProduceStatic : InventoryProduceControl;
	private var cookCL : CookControl;
	static var cookCLStatic : CookControl;
	private var yuaninfo : LabelLinkItemInfo;
	static var yuaninfoStatic : LabelLinkItemInfo;
	var Noisemap : Texture ;
	static var NoisemapStatic : Texture;
	private var AttackButton : TestButton;
	static var AttackButtonStatic : TestButton;
	private var SoulCL : SoulControl;
	static var SoulCLStatic : SoulControl;
	private var Language : String;
	static var CardCLStatic : CardControl;
	static var mmp : Minimap;
	static var SkillObjDet : SkillObjectDetails;
	var NGUIJoy : NGUIJoystick;
	static var NGUIJoyStatic : NGUIJoystick;
	static var npcclStatic : NPCControl;
	static var psStatic : PlayerStatus;
	static var taskILStatic : TaskInfoList;
	static var MonsterUICLStatic : MonsterUIControl;
	static var invCangKuStatic : InventoryCangku;
	static var buttonMessCL : ButtonMessageControl;
	static var exhbtControl : ExhibitionControl;
	static var storeItemCL : StoreItemControl;
	static var equipBreakCL : EquipmentBreakdownControl;
	static var tipclStatic : TipsControl;
	static var actionProCL : ActionProgressControl;
	static var pAIStatic : PlayerAI;
	static var buyStoreControl : BuyStoreControl;
	static var creatFriendTeam : CreatAndFriendTeam;
	static var everyDayAction : EverydayAnimAction;
	var useSkillcl : SkillControl;
	var publicMapNameList  : String[];
	var mainCamera : Transform;
	static var mainCameraStatic : Transform;
	var NpcName : GameObject;
	static var NpcNameStatic : GameObject;
	static var camStoryStatic : Storycamera;
	static var stictaskSceneIcon : TaskSceneIcon;
	var btnYingMo : GameObject;
	var btnFish : GameObject;
	var btnSoul : GameObject;
	static var newUseItemCLStatic : NewUseItemControl;
	function Awake () {
		mainCameraStatic = mainCamera;
		NpcNameStatic = NpcName;
		NGUIJoyStatic = NGUIJoy;
	//	EffectGamepoolStatic = Effectpool;
	//	BuffefmanageStatic = Buffefpool;
	//	FontpoolStatic = Fontpools;
	//	FuhaopoolStatic = Fuhaopool;
	//	PickpoolStatic = Pickpool;
	//	InvmakerStatic = invmaker;
	//	Loc.currentLanguage = Language;
		AllMge = this;
		SaoDangDJstatic = SaoDangDJ;
	//	AttackButtonStatic = AttackButton;
	//	yuaninfoStatic = yuaninfo;
		timeDJStatic= timeDJ;
	//	WakCLStatic = WakCL;
	//	SkillCLStatic = SkillCL;
		pvpQuanStatic = pvpQuan;
	//	if(pDead != null){
	//		pDeadStatic = pDead;	
	//	}
	//	btnInfoStatic = btnInfo;
		btnGMBStatic = btnGMB;
		yuanBMStatic = yuanBM;
		quanSoulStatic = quanSoul;
		uicamStatic = uicam;
		uicam2Static = uicam2;
	//	PlyUIClStatic = PlyUICl;
	//	ItMoveStatic = ItMove;
	//	mtwStatic = mtw;
	//	dungclStatic = dungcl;
	//	uusStatic = uus;
	//	qrStatic = qr;
	//	tsStatic = ts;
	//	jiaochengCLStatic = jiaochengCL;
	//	InvclStatic = invcl;
	//	UICLStatic = uicl;
	//	UIALLPCStatic = uiallPC;
		NoisemapStatic = Noisemap;
		inv = null;
		obj = gameObject;
		bagit = null;
		realit = null;
	}


	var isGoto : boolean = false;
	static var isDie : boolean = false;
	static var isUpgrade : boolean = false;
	static var isOutMainMap : boolean = false;
	static var TimeXuepingNum : int = 0;
	function Start(){
		yield WaitForSeconds(1);
		try{
			if(! isGoto && Application.loadedLevelName != "Loading 1"){
				isGoto = true;
				if(isDie||(TimeXuepingNum!=0&&isOutMainMap == true)){
					isDie = false;
					TimeXuepingNum=0;
					isOutMainMap = false;
//					if(AllManage.UICLStatic.ButtonsShowZhu[1].active == true)
//回城提示升级以及变强UI
				AllManage.UIALLPCStatic.show49();
				if(isUpgrade&&UICLStatic.mapType == MapType.zhucheng){
					isUpgrade = false;
					AllManage.tipclStatic.ShowUpgrade(psStatic.Level);
//					if(psStatic.NewSkillStr!="")
//					{
//						AllManage.tipclStatic.ShowNewSkills(psStatic.NewSkillName,psStatic.NewSkillStr);
//					}
					}
				}else
				if(isUpgrade&&UICLStatic.mapType == MapType.zhucheng){
					isUpgrade = false;
					AllManage.tipclStatic.ShowUpgrade(psStatic.Level);
//					if(psStatic.NewSkillStr!="")
//					{
//						AllManage.tipclStatic.ShowNewSkills(psStatic.NewSkillName,psStatic.NewSkillStr);
//					}
				}
			}
		}catch(e){
		
		}
	}

	static var inv : InventoryItem = null;
	static var obj : GameObject;
	static var bagit : BagItem;
	static var realit : BagItem;

	var Keys : Array = new Array(String);
	private var Val : String = "";
	private var OldVal : String = "";
	var Loc : Localization;
	function SetLabelLanguageAsID(Lab : UILabel){
		if(! Lab)
			return;
		OldVal = Lab.text;
		Lab.text = "";
		for(var i=0; i<Keys.Count ; i++){
			Val = Loc.Get(Keys[i]);
			Lab.text += Val;
		}
	}

//---------------------------------

	var powerTipsReturnObj : CostPowerReturnObject[];
	function TipsNewPower(type : yuan.YuanPhoton.CostPowerType , num1 : int , num2 : int , itemID : String , obj : GameObject , functionName : String){
		for(var i=0; i<powerTipsReturnObj.length; i++){
			if(type == powerTipsReturnObj[i].type){
				powerTipsReturnObj[i].functionName = functionName;
				powerTipsReturnObj[i].obj = obj;
			}
		}
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().Showstrength(type , num1 , num2 , itemID));	
	}

	function ReturnTipsPower(objs : Object[]){
		var type : yuan.YuanPhoton.CostPowerType;
		type = objs[0];
		for(var i=0; i<powerTipsReturnObj.length; i++){
			if(type == powerTipsReturnObj[i].type){
				powerTipsReturnObj[i].obj.SendMessage(powerTipsReturnObj[i].functionName , objs[2] , SendMessageOptions.DontRequireReceiver);
			}
		}
	}

//---------------------------------

	var powerReturnObj : CostPowerReturnObject[];
	function CostNewPower(type : yuan.YuanPhoton.CostPowerType , num1 : int , num2 : int , itemID : String , obj : GameObject , functionName : String){
		for(var i=0; i<powerReturnObj.length; i++){
			if(type == powerReturnObj[i].type){
				powerReturnObj[i].functionName = functionName;
				powerReturnObj[i].obj = obj;
			}
		}
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().Coststrength(type , num1 , num2 , itemID));	
	}

	function ReturnCostPower(type : yuan.YuanPhoton.CostPowerType){
		for(var i=0; i<powerReturnObj.length; i++){
			if(type == powerReturnObj[i].type && powerReturnObj[i].obj != null){
				powerReturnObj[i].obj.SendMessage(powerReturnObj[i].functionName , SendMessageOptions.DontRequireReceiver);
			}
		}
	}

//---------------------------------

	var allReturnTips : UseMoneyReturnObject[];
	function TipsMoney(type : yuan.YuanPhoton.UseMoneyType , num1 : int , num2 : int , itemID : String , obj : GameObject , functionName : String){
		for(var i=0; i<allReturnTips.length; i++){
			if(type == allReturnTips[i].type){
				allReturnTips[i].functionName = functionName;
				allReturnTips[i].obj = obj;
			}
		}
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().ShowALLMoney(type , num1 , num2 , itemID));	
	}

	function ReturnUseTips(objs : Object[]){
		var type : yuan.YuanPhoton.UseMoneyType ;
		type = objs[0];
		var mGold : int  = objs[1]; 
		var mBlood : int  = objs[2];
		var mMarrowIron : int  = objs[3];
		var mMarrowGold : int  = objs[4];
		
//		if(type > 45 && type != 55){
//			return;
//		}
		for(var i=0; i<allReturnTips.length; i++){
			if(type == allReturnTips[i].type && allReturnTips[i].obj != null){
				allReturnTips[i].obj.SendMessage(allReturnTips[i].functionName , objs , SendMessageOptions.DontRequireReceiver);
			}
		}
	}
//---------------------------------
	var allReturnObj : UseMoneyReturnObject[];
	function UseNewMoney(type : yuan.YuanPhoton.UseMoneyType , num1 : int , num2 : int , itemID : String , obj : GameObject , functionName : String){
		for(var i=0; i<allReturnObj.length; i++){
			if(type == allReturnObj[i].type){
				allReturnObj[i].functionName = functionName;
				allReturnObj[i].obj = obj;
			}
		}
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().UseMoney(type , num1 , num2 , itemID));
	}

	function ReturnUseMoney(type : yuan.YuanPhoton.UseMoneyType){
//		if(type > 45 && type != 55){
//			return;
//		}
		for(var i=0; i<allReturnObj.length; i++){
			if(type == allReturnObj[i].type && allReturnObj[i].obj != null){
				allReturnObj[i].obj.SendMessage(allReturnObj[i].functionName , SendMessageOptions.DontRequireReceiver);
				if(type == yuan.YuanPhoton.UseMoneyType.Sell){
					InvclStatic.ReInitItem();
				}
				return;
			}
		}
	}

	var ptime : int = 0;
	function Update(){
		if(Time.time > ptime && ! isMoneying){
			ptime = Time.time + 2;
			if(ArrayMoney.Count > 0){
				realUseMoney();
			}
		}
	}

	private var isMoneying : boolean = false;
	function UseMoney(mon : int , blo : int , type : UseMoneyType  , object : GameObject , returnFunction : String){
		var mc : MoneyClass;
		mc = new MoneyClass();
		mc.mon = mon;
		mc.blo = blo;
		mc.type = type;
		mc.object = object;
		mc.returnFunction = returnFunction;
		
		ArrayMoney.Add(mc);
		realUseMoney();
	}

	var ArrayMoney : Array = new Array();
	function realUseMoney(){
		if(isMoneying){
			return;
		}
		for(var i=0; i<ArrayMoney.Count; i++){
			isMoneying = true;
			tsStatic.RefreshBaffleOn();
			
			var mc : MoneyClass;
			mc = new MoneyClass();
			mc = ArrayMoney[i];
			var YuanBack : YuanBackInfo;
			YuanBack = new YuanBackInfo(mc.type.ToString()+Time.time.ToString());
			mc.mon *= -1;
			mc.blo *= -1;
			InRoom.GetInRoomInstantiate().ClientMoney(ToInt.IntToStr(mc.mon) , ToInt.IntToStr(mc.blo) , YuanBack);
		//	zhuanquan
			while(YuanBack && YuanBack.isUpate){
				yield;
			}
	//		//print(YuanBack.opereationResponse);
			switch(YuanBack.opereationResponse.ReturnCode){
				case yuan.YuanPhoton.ReturnCode.Yes:
					if(mc && mc.object){
						mc.object.SendMessage(mc.returnFunction , SendMessageOptions.DontRequireReceiver);				
					}
					break;
				case yuan.YuanPhoton.ReturnCode.NoGold :
		//			tishi
					break;
				case yuan.YuanPhoton.ReturnCode.NoBloodStone :
		//			tishi
					break;
				case yuan.YuanPhoton.ReturnCode.Error :
		//			tishi
					break;			
			}
			if(ArrayMoney){
				ArrayMoney.Remove(ArrayMoney[i]);	
			}
			tsStatic.RefreshBaffleOff();
			isMoneying = false;
	//		guanbi
		}
	}
	
	function ActivitySet(){
//		print("JoinActivity : 5");
		InRoom.GetInRoomInstantiate().JoinActivity("5");
	}
	
	function qingqiuzhanchang(){
//		print("qingqiuzhanchang");
		InRoom.GetInRoomInstantiate().BattlefieldInfo();
	}	
	
	function addyu(){
		InvclStatic.AddBagItemAsID("y210433315172000000000000");
	}
	
	function addKuangshi(){
		InvclStatic.AddBagItemAsID("y425431325272000000000000");	
	}
	
	var personSkills : PersonSkill[];

function SoulAddLevel(nowQual : int , nowLevel : int , plusLevel : int) : int[]{
	var useInts : int[] = new int[2];
	var pInt : int = 0;
	
	
	if(nowLevel + plusLevel > 99){
		pInt = nowLevel + plusLevel;
		nowLevel = pInt % 100;
		while(pInt > 99){
			if(nowQual < 4){
				nowQual += 1;
			}
			pInt -= 99;
		}
	}else{
		nowLevel += plusLevel;
	}
	
	useInts[0] = nowQual;
	useInts[1] = nowLevel;
	return useInts;
}

function SoulAddExp(nowQual : int , nowLevel : int , nowExp : int , plusExp : int) : int[]{
	var useInts : int[] = new int[3];
	var plevelInts : int[] = new int[2];
	var pInt : int = 0;
	
	if(nowExp + plusExp > 99){
		pInt = nowExp + plusExp;
		nowExp = pInt % 100;
		while(pInt > 99){
			plevelInts = SoulAddLevel(nowQual , nowLevel , 1);
			nowQual = plevelInts[0];
			nowLevel = plevelInts[1];
			pInt -= 99;
		}
	}else{
		nowExp += plusExp;
	}
	
	useInts[0] = nowQual;
	useInts[1] = nowLevel;	
	useInts[2] = nowExp;	
	return useInts;
}

function TipsUpLevel(plus : int , point : Transform){
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0213") + plus);		
	AllResources.FontpoolStatic.SpawnEffect(13 , point);	
}

function TipsUpQual(plus : int , point : Transform){
	yield WaitForSeconds(0.5);
	AllResources.FontpoolStatic.SpawnEffect(14 , point);	
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0214") + plus);		
}

function TipsUpExp(plus : int){
	yield WaitForSeconds(1);
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0215") + plus);		
}

function GetColorAsQuality(qual : int) : String{
	var colorStr : String = "";
	switch(qual){
		case 1:
			colorStr = "[FFFFFF]";
			break;
		case 2:
			colorStr = "[00FF00]";
			break;
		case 3:
			colorStr = "[00C8FF]";
			break;
		case 4:
			colorStr = "[FF00FF]";
			break;
	}
	return colorStr; 
}
	
	var useItemID : String;
	function AddBagItem(){
		InvclStatic.AddBagItemAsID(useItemID);
	}
	
	function IsGetPlace(id : String) : boolean{
		if(InventoryControl.yt.Rows[0]["GetPlace"].YuanColumnText.IndexOf("511") > 0){
			return true;
		}
		return false;
	}
	
}