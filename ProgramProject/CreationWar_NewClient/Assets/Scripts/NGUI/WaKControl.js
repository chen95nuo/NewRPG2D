	#pragma strict
class WaKControl extends Song{
	//var tishi : TiShi;
	var dungCL : DungeonControl;
	var UIStone : GameObject;
	var UIFish : GameObject;
	var UIFlag : GameObject;
	var UIBox : GameObject;
	var UIFood : GameObject;
	var UIWaK : GameObject;
	var UIFilledSpriteStoneGuang : UIFilledSprite;
	var UIFilledSpriteFishGuang : UIFilledSprite;
	var UIFilledSpriteFlagGuang : UIFilledSprite;
	var UIFilledSpriteBoxGuang : UIFilledSprite;
	var UIFilledSpriteFoodGuang : UIFilledSprite;
	var UIFilledSpriteWaKGuang : UIFilledSprite;
	var TiaoStone : GameObject;
	var TiaoFish : GameObject;
	var TiaoFlag : GameObject;
	var TiaoBox : GameObject;
	var TiaoFood : GameObject;
	var TiaoWaK : GameObject;

	var Storetubiao : UISprite;
	var nowStone : TriggerStone;
	private var ag : agentLocomotion;

	function Awake(){
		AllManage.WakCLStatic = this;
	}

	function Start(){
		for(var trans : Transform in transform){
			trans.gameObject.SetActiveRecursively(false);
		}
		if(UIControl.mapType == MapType.jingjichang){
			Storetubiao.spriteName = "hand";
		}
	}

	var busy : boolean = false;
	function FindStone(ts : TriggerStone){ 
		busy = true;
		nowStone = ts;
		if(ts.myType == ConsumablesType.Stone){
			UIStone.transform.parent.active = true;
			UIStone.SetActiveRecursively(true);
			UIFilledSpriteStoneGuang.fillAmount = 0;
		}else
		if(ts.myType == ConsumablesType.Fish){
			UIFish.transform.parent.active = true;
			UIFish.SetActiveRecursively(true);
			UIFilledSpriteFishGuang.fillAmount = 1; 
			LabelFish.text = "10";
		}else
		if(ts.myType == ConsumablesType.Flag){
			UIFlag.transform.parent.active = true;
			UIFlag.SetActiveRecursively(true);
			UIFilledSpriteFlagGuang.fillAmount = 0;
		}else
		if(ts.myType == ConsumablesType.Food){
			UIFood.transform.parent.active = true;
			UIFood.SetActiveRecursively(true);
			UIFilledSpriteFoodGuang.fillAmount = 0;
		}else
		if(ts.myType == ConsumablesType.Box){
			UIBox.transform.parent.active = true;
			UIBox.SetActiveRecursively(true);
			UIFilledSpriteBoxGuang.fillAmount = 0;
		}else
		if(ts.myType == ConsumablesType.WaK){
			UIWaK.transform.parent.active = true;
			UIWaK.SetActiveRecursively(true);
			UIFilledSpriteWaKGuang.fillAmount = 0;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	WaKuang
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	 var LabelWaK : UILabel;
	function StartWaK(){
		if(ag == null && PlayerStatus.MainCharacter ){
			ag = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
		}
		if(ag.enabled)
			return;
		if(WaStoneIng)
		return;
		BreakRide();
		LabelWaK.text = nowStone.isTaskName;
		PlayerStatus.MainCharacter.SendMessage("Skill", 16);
		TiaoWaK.gameObject.SetActiveRecursively(true);
		WaStoneIng = true;
		waTime = 0;
		var timeStone : int;
		var useFloat : float;
		useFloat = nowStone.isTaskSeconds;
		 while(nowStone && WaStoneIng && waTime <nowStone.isTaskSeconds){
		 	waTime += Time.deltaTime;
			UIFilledSpriteWaKGuang.fillAmount = waTime / useFloat;
		 	if(nowStone && waTime >= nowStone.isTaskSeconds){
		 		GetWaK();
		 	}
		 	yield;
		 }
	}

	function GetWaK(){
		waTime = 0;
		WaStoneIng = false;
		UIWaK.SetActiveRecursively(false);
		TiaoWaK.gameObject.SetActiveRecursively(false);
		if(nowStone){
			AllManage.mtwStatic.DoneWaKuang(nowStone.isTaskID);
			nowStone.gameObject.SendMessage("DoSomething",nowStone.isTaskSeconds-1 , SendMessageOptions.DontRequireReceiver);
		}
		busy = false;
	//	yield WaitForSeconds(nowStone.isTaskSeconds-1);
	//	if(!nowStone.isDontClose)
	//	nowStone.gameObject.SetActiveRecursively(false);
	}

	function FallWaK(){
		if(nowStone)
			nowStone.gameObject.SendMessage("Reset",SendMessageOptions.DontRequireReceiver);
		TiaoWaK.gameObject.SetActiveRecursively(false);
		AllManage.tsStatic.Show("tips003");	
		waTime = 0;
		WaStoneIng = false;
		UIFilledSpriteWaKGuang.fillAmount = 0;	
		busy = false;
	}

	function ExitWaK(){
		if(nowStone)
			nowStone.gameObject.SendMessage("Reset",SendMessageOptions.DontRequireReceiver);
		waTime = 0;
		WaStoneIng = false;
		UIFilledSpriteWaKGuang.fillAmount = 0;
		UIWaK.SetActiveRecursively(false);
		TiaoWaK.gameObject.SetActiveRecursively(false);
		nowStone = null;
		busy = false;
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Box
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	function StartWaBox(){
		if(ag == null && PlayerStatus.MainCharacter ){
			ag = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
		}
		if(ag.enabled)
			return;
		if(WaStoneIng)
		return;
		BreakRide();
		PlayerStatus.MainCharacter.SendMessage("Skill", 16);
		TiaoBox.gameObject.SetActiveRecursively(true);
		WaStoneIng = true;
		waTime = 0;
		var timeStone : int;
		 while(WaStoneIng && waTime <5){
		 	waTime += Time.deltaTime;
			UIFilledSpriteBoxGuang.fillAmount = waTime / 5.0;
		 	if(waTime >= 5){
		 		GetBox();
		 	}
		 	yield;
		 }
	}


	function GetBox(){ 
	if(nowStone)
	    nowStone.PlayerAni("idle");
		waTime = 0;
		WaStoneIng = false;
		UIBox.SetActiveRecursively(false);
		TiaoBox.gameObject.SetActiveRecursively(false);
		if(nowStone){
		    Getreward();
			nowStone.gameObject.SetActiveRecursively(false); 
		}
		busy = false;
	}

	function FallBox(){
	if(nowStone)
	nowStone.PlayerAni("idle0");
		TiaoBox.gameObject.SetActiveRecursively(false);
		AllManage.tsStatic.Show("tips003");	
		waTime = 0;
		WaStoneIng = false;
		UIFilledSpriteBoxGuang.fillAmount = 0;	
		busy = false;
	}

	function ExitBox(){
	if(nowStone)
	nowStone.PlayerAni("idle0");
		waTime = 0;
		WaStoneIng = false;
		UIFilledSpriteBoxGuang.fillAmount = 0;
		UIBox.SetActiveRecursively(false);
		TiaoBox.gameObject.SetActiveRecursively(false);
		nowStone = null;
		busy = false;
	}

	////////////////////////////////////////////////
	var OpenB : OpenBox;
	//var LT : LootTable;
	private var ps : PlayerStatus;
	function Getreward(){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}

		var GetGold : int = 0;
		var newRanInt : int = 0;
		var inv1 : InventoryItem = null;
		var inv2 : InventoryItem = null;
		var inv3 : InventoryItem = null;
		var invStr : String; 
		var rewardItemStr : String[];
		rewardItemStr = DungeonControl.rewardItemStr1;
		if(rewardItemStr[0] != ""){
			newRanInt = Random.Range(0,100);
			if(newRanInt > 60){
				inv1 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[0] , inv1);
			}else{
				invStr = AllResources.staticLT.MakeItemID1(invStr, Random.Range(1,3)); 
				inv1 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv1);
			}
			invCL.AddBagItem(inv1);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv1.itemID);
		}
		if(rewardItemStr[1] != "") { 
			newRanInt = Random.Range(0,100);
			if(newRanInt > 60){
				inv2 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[1] , inv2); 
			}else{
				invStr = AllResources.staticLT.MakeItemID1(invStr, Random.Range(1,3)); 
				inv2 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv2);
			}
			invCL.AddBagItem(inv2);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv2.itemID);
		}
		var useRan : int = 0;
		useRan = Random.Range(0,10000);
		if(useRan > 9998){
			invStr = "89" + Random.Range(1,4) + Mathf.Clamp(DungeonControl.level / 20 + 1 , 1 , 5) + ", 01";
			inv3 = AllResources.InvmakerStatic.GetItemInfo(invStr , inv3); 
			invCL.AddBagItem(inv3);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv3.itemID);
		}
		GetGold = DungeonControl.level * 20 * Random.Range(50,150) / 100;
	//	ps.UseMoney(GetGold * (-1) , 0);
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.Getreward , DungeonControl.level  , 0 , "" , gameObject , "");
//		AllManage.AllMge.UseMoney((DungeonControl.level * 20 * Random.Range(50,150) / 100) * (-1) , 0 , UseMoneyType.Getreward , gameObject , "");
		OpenB.open(1,GetGold,0,inv1,inv2,inv3,null);
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Food
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	function StartEatFood(){
		if(ag == null && PlayerStatus.MainCharacter ){
			ag = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
		}
		if(ag.enabled)
			return;
		if(WaStoneIng)
		return;
		BreakRide();
	  PlayerStatus.MainCharacter.SendMessage("Startloop", "eat"); 
		TiaoFood.gameObject.SetActiveRecursively(true);
		UIFood.SetActiveRecursively(true);
		WaStoneIng = true;
		waTime = 0;
		var timeStone : int;
		 while(WaStoneIng && waTime <12){
		 	waTime += Time.deltaTime;
			UIFilledSpriteFoodGuang.fillAmount = waTime / 12.0;
		 	if(waTime >= 12){
			    PlayerStatus.MainCharacter.SendMessage("stopanimation", "eat"); 
				waTime = 0;
				WaStoneIng = false;
				UIFood.SetActiveRecursively(false);
				TiaoFood.gameObject.SetActiveRecursively(false);
				busy = false;
		 	}
		 	yield;
		 }
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		ps.Eating = false;
	}

	function StartWaFood(){
		if(ag == null && PlayerStatus.MainCharacter ){
			ag = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
		}
		if(ag.enabled)
			return;
		if(WaStoneIng)
		return;
		BreakRide();
	  PlayerStatus.MainCharacter.SendMessage("Startloop", "eat"); 
		TiaoFood.gameObject.SetActiveRecursively(true);
		UIFood.SetActiveRecursively(true);
		WaStoneIng = true;
		waTime = 0;
		var timeStone : int;
		 while(WaStoneIng && waTime <12){
		 	waTime += Time.deltaTime;
			UIFilledSpriteFoodGuang.fillAmount = waTime / 12.0;
		 	if(waTime >= 12){
		 		GetFood();
		 	}
		 	yield;
		 }
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		ps.Eating = false;
	}

	function GetFood(){ 
	    PlayerStatus.MainCharacter.SendMessage("stopanimation", "eat"); 
		waTime = 0;
		WaStoneIng = false;
		UIFood.SetActiveRecursively(false);
		TiaoFood.gameObject.SetActiveRecursively(false);
		if(nowStone){
			PlayerStatus.MainCharacter.SendMessage("PlayerAction", nowStone.myID); 
	//		yield WaitForSeconds(3);
			if(nowStone)
				nowStone.gameObject.SetActiveRecursively(false);
		}
		busy = false;
	//	if(nowStone.myFlagID != UIControl.myTeamInfo){
	//		nowStone.SetFlagAsID(UIControl.myTeamInfo); 
	//		arecl.AddJiqiao(10);
	//	}
	}

	function FallFood(){
	    PlayerStatus.MainCharacter.SendMessage("stopanimation", "eat");
	    PlayerStatus.MainCharacter.SendMessage("StopEat");
		TiaoFood.gameObject.SetActiveRecursively(false);
		UIFood.SetActiveRecursively(false);
		AllManage.tsStatic.Show("tips003");	
		waTime = 0;
		WaStoneIng = false;
		UIFilledSpriteFoodGuang.fillAmount = 0;	
		busy = false;
	}

	function ExitFood(){
		waTime = 0;
		WaStoneIng = false;
		UIFilledSpriteFoodGuang.fillAmount = 0;
		UIFood.SetActiveRecursively(false);
		TiaoFood.gameObject.SetActiveRecursively(false);
		nowStone = null;
		busy = false;
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Flag
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	function StartWaFlag(){
		if(ag == null && PlayerStatus.MainCharacter ){
			ag = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
		}
		if(ag.enabled)
			return;
		if(WaStoneIng)
		return;
		BreakRide();
		PlayerStatus.MainCharacter.SendMessage("Skill", 16);
		TiaoFlag.gameObject.SetActiveRecursively(true);
		WaStoneIng = true;
		waTime = 0;
		var timeStone : int;
		 while(WaStoneIng && waTime <5){
		 	waTime += Time.deltaTime;
			UIFilledSpriteFlagGuang.fillAmount = waTime / 5.0;
		 	if(waTime >= 5){
		 		GetFlag();
		 	}
		 	yield;
		 }
	}

	function GetFlag(){ 
		waTime = 0;
		WaStoneIng = false;
		UIFlag.SetActiveRecursively(false);
		TiaoFlag.gameObject.SetActiveRecursively(false); 
		if(nowStone){
			if(nowStone.myFlagID != UIControl.myTeamInfo){
//				nowStone.SetFlagAsID(UIControl.myTeamInfo); 
				AllManage.areCLStatic.AddJiqiao(nowStone.flagID);
				if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
					ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
				}
				if(ps){
					ps.AddPVPPoint(20 * (-1));
					ps.AddPVP8Info(PVP8InfoType.Flag , 20);
				}
			}
		}
		busy = false;
	}

	function FallFlag(){
		TiaoFlag.gameObject.SetActiveRecursively(false);
		AllManage.tsStatic.Show("tips003");	
		waTime = 0;
		WaStoneIng = false;
		UIFilledSpriteFlagGuang.fillAmount = 0;	
		busy = false;
	}

	function ExitFlag(){
		waTime = 0;
		UIFilledSpriteFlagGuang.fillAmount = 0;
		UIFlag.SetActiveRecursively(false);
		TiaoFlag.gameObject.SetActiveRecursively(false);
		nowStone = null;
		busy = false;
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Fish
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	var UISpriteFishQuan : UISprite;
	function StartWaFish(){
	 	if(ag == null && PlayerStatus.MainCharacter ){
			ag = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
		}
		if(ag.enabled)
			return;
	 	PlayerStatus.MainCharacter.SendMessage("CallAnimation", "fish");
		if(UISpriteFishQuan.enabled){
			GetFish();
		}else{
			if(!WaStoneIng){
				BreakRide();
				GoStartFish();		
			}else{
				 FallFish();
			}
		}
	}

	 function GetFish(){ 
	 if(!nowStone)
	 return;
	 	waTime = 0;
		WaStoneIng = false;
		UIFish.SetActiveRecursively(false);
	 	UISpriteFishQuan.enabled = false; 	
	    PlayerStatus.MainCharacter.SendMessage("stopanimation", "fishing"); 
	  	PlayerStatus.MainCharacter.SendMessage("Skill", 17);
	  	
		var useStr : String;
		var useInt : int;
		useInt = parseInt(nowStone.myID.Substring(2,2));
//		useInt += Random.Range(-3,1);
		useInt = Random.Range(1 , useInt+1);
		if(useInt < 1){
			useInt = 1;
		}else
		if(useInt > 10){
			useInt = 10;
		}
		useStr =useInt.ToString();
		if(useStr.Length < 2){
			useStr = "0" + useStr;
		}
		if(Random.Range(0,1000) > 998){
			useStr = "1" + Random.Range(1,3);
		}
		var useStoneID : String;
		var useStoneName : String;
		useStoneID = nowStone.myID.Substring(0,2) + useStr + nowStone.myID.Substring(4,3);
		if(Application.loadedLevelName == "Map111"){
			useStoneID = "8201,01";
		}
		useStoneName = dungCL.GetItemNameAsID(useStoneID);
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages149") + useStoneName); 
		InventoryControl.yt.Rows[0]["AimFishing"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimFishing"].YuanColumnText) + 1).ToString();
	    InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.FishingNum ,1);
		inv =  AllResources.InvmakerStatic.GetItemInfo(useStoneID , inv);
		invCL.AddBagItem(inv);
	 	TiaoFish.gameObject.SetActiveRecursively(false);
	 	invCL.GetOneFish(useStoneID , nowStone.myLevel);
		busy = false;
	}
	 
	private var FishTime1 : int;
	private var FishTime2 : int; 
	 var LabelFish : UILabel;
	 var TipsLabelFish : UILabel;
	 function GoStartFish(){
	  PlayerStatus.MainCharacter.SendMessage("Startloop", "fishing"); 
	  var levelF : int;
	  levelF = AllManage.InvclStatic.expFishing;
	  
		TiaoFish.gameObject.SetActiveRecursively(true);
	 	WaStoneIng = true;
	 	waTime = 40 - levelF*0.2;
	 	waTime = Mathf.Clamp(waTime , 15 , 40);
	 	FishTime1 = Random.Range(5,waTime - 5);
	 	FishTime2 = FishTime1 + 2 + levelF*0.01; 
	 	var timeFish : int;
	//	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
	//		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	//	}
	//	if(nowStone){
	//		ps.RotateToPosition(nowStone.gameObject.transform.position);
	//	}
	 	while(WaStoneIng && waTime > 0){
	 		waTime -= Time.deltaTime; 
	 		UIFilledSpriteFishGuang.fillAmount = waTime / (40 - levelF*0.2);
	 		if(waTime < FishTime2 && waTime > FishTime1){
	 			 UISpriteFishQuan.enabled = true;
	 			 TipsLabelFish.text = AllManage.AllMge.Loc.Get("info845");
	 		}else{
	 			TipsLabelFish.text = AllManage.AllMge.Loc.Get("info844");
	 		 	UISpriteFishQuan.enabled = false;	
	 		}
	 		if(waTime <= 0){
	 			FallFish();
	 		}
	 		timeFish = waTime;
	 		LabelFish.text = timeFish.ToString();
	 		yield;
	 	}
	 }
	 
	 function FallFish(){
	    PlayerStatus.MainCharacter.SendMessage("stopanimation", "fishing"); 
	 	UIFilledSpriteFishGuang.fillAmount = 1; 
	 	WaStoneIng = false;
	 	UISpriteFishQuan.enabled = false;	
	 	AllManage.tsStatic.Show("tips057");	
		LabelFish.text = "10";
		TiaoFish.gameObject.SetActiveRecursively(false);
		busy = false;
	}
	 
	 function ExitFish(){
	 	UIFilledSpriteFishGuang.fillAmount = 1; 
	 	WaStoneIng = false;
	 	UISpriteFishQuan.enabled = false;
	 	UIFish.SetActiveRecursively(false);
	 	TiaoFish.gameObject.SetActiveRecursively(false);
		busy = false;
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Ore
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	var WaStoneIng : boolean = false;
	var waTime : float = 0; 
	var LabelStone : UILabel;
	function StartWaStone(){
		if(ag == null && PlayerStatus.MainCharacter ){
			ag = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
		}
		if(ag.enabled)
			return;
		if(WaStoneIng)
		return;
		BreakRide();
		 	PlayerStatus.MainCharacter.SendMessage("Skill", 15);
		TiaoStone.gameObject.SetActiveRecursively(true);
		WaStoneIng = true;
		waTime = 0;
		var timeStone : int;
		 while(WaStoneIng && waTime <5){
		 	waTime += Time.deltaTime;
			UIFilledSpriteStoneGuang.fillAmount = waTime / 5.0;
		 	if(waTime >= 5){
		 		GetStone();
		 	}
		 	yield;
		 }
	}
	 
	 var invCL : InventoryControl; 
	 var inv : InventoryItem; 
	// var invMaker : Inventorymaker; 
	 var arecl : ArenaControl;
	function GetStone(){ 
		waTime = 0;
		WaStoneIng = false;
		UIStone.SetActiveRecursively(false);
		nowStone.gameObject.SetActiveRecursively(false);
		TiaoStone.gameObject.SetActiveRecursively(false);
	//	 PlayerStatus.MainCharacter.SendMessage("stopanimation", "caikuang"); 
		if(nowStone){
			nowStone.myName = dungCL.GetItemNameAsID(nowStone.myID);
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages150") + nowStone.myName +" X1");   
			InventoryControl.yt.Rows[0]["AimMining"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimMining"].YuanColumnText) + 1).ToString();
		    InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.MiningNum , 1);
			inv =  AllResources.InvmakerStatic.GetItemInfo(nowStone.myID , inv);
			invCL.AddBagItem(inv);
		 	invCL.GetOneStone(nowStone.myID , nowStone.myLevel); 
		}
		busy = false;
	}
	 
	function ExitStone(){
		waTime = 0;
		WaStoneIng = false;
		UIFilledSpriteStoneGuang.fillAmount = 0;
		UIStone.SetActiveRecursively(false);
		TiaoStone.gameObject.SetActiveRecursively(false);
		nowStone = null;
		busy = false;
	}

	function DaDuanStone(){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(ps.Eating){
			FallFood();
		}
		if(nowStone != null){
			if(WaStoneIng){
				if(nowStone.myType == ConsumablesType.Stone){
				    PlayerStatus.MainCharacter.SendMessage("stopanimation", "caikuang");
					TiaoStone.gameObject.SetActiveRecursively(false);
					AllManage.tsStatic.Show("tips003");	
					waTime = 0;
					WaStoneIng = false;
					UIFilledSpriteStoneGuang.fillAmount = 0;	
				}else
				if(nowStone.myType == ConsumablesType.Fish){
					FallFish();
				}else
				if(nowStone.myType == ConsumablesType.Flag){
					FallFlag();
				}else
				if(nowStone.myType == ConsumablesType.Box){
					FallBox();
				}else
				if(nowStone.myType == ConsumablesType.Food){
					FallFood();
				}else
				if(nowStone.myType == ConsumablesType.WaK){
					FallWaK();
				}
	//			UIStone.SetActiveRecursively(false);
	//			nowStone = null;
			}
			busy = false;
			WaStoneIng = false;
		}else
		if(UIFood.active){
			busy = false;
			FallFood();
			UIFood.SetActiveRecursively(false);
		}
	}

	function BreakRide(){
		PlayerStatus.MainCharacter.SendMessage("DaDuan",SendMessageOptions.DontRequireReceiver);
		PlayerStatus.MainCharacter.SendMessage("BreakRide",SendMessageOptions.DontRequireReceiver);
	}
}