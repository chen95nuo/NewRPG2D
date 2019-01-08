	#pragma strict
class BagItem extends Song{
	enum TutorialType{
		empty = 0,
		equipSoul = 1,
		equipDigest = 2
	}
	
	var itemMove : ItemMove;
	var myType : SlotType;
	var myProfessionType : ProfessionType;
//	var invcl : InventoryControl;
	var invObj : GameObject;
	var serverIndex : int = 0;
	var tutorialType : TutorialType;
	function Awake(){
		if( ! itemMove){
			itemMove = AllManage.ItMoveStatic;
		}
//		if(invcl == null){
//			invcl = AllManage.InvclStatic;
//		}
		if(invObj == null){
			invObj = AllManage.InvclStatic.gameObject;	
		}
	//	if(invMaker == null){
	//		invMaker = AllResources.InvmakerStatic;
	//	}
		inv = null;
		if(YuanInfo == null){
			YuanInfo = AllManage.yuaninfoStatic;
		}
	}

	function Start () {
//		inv = null;
	}

	var mm : int = 0;

	function OnPress(){
		itemMove.SetJiaoHuan(this);
	}

	var EquepInfo : EquepmentItemInfo;
	var isChaKan : boolean = false;
	var isShangDian : boolean = false;
	var YuanInfo : LabelLinkItemInfo;
	var isEquepment : boolean = false;
	var isJiaoYi : boolean = false;
	var isFanPai : boolean = false;
	var ShowOtherPlayer : rendercamerapic;
	function OnClick(){
		if(inv != null && EquepInfo != null){
	//		//print(inv.slotType + " == inv.slotType");
			if(inv.slotType != SlotType.Digest && inv.slotType != SlotType.Soul){
				if(!isChaKan){
					invObj.SendMessage("ItemInfoOn",SendMessageOptions.DontRequireReceiver);		
				}
				EquepInfo.showEquItemInfo(inv , this); 
				if(isEquepment){
					EquepInfo.cantMaichu = true;
				}else{
					EquepInfo.cantMaichu = false;			
				}
	//			//print(isShangDian + " == isShangDian");
				if(isShangDian){
					EquepInfo.NowClickItemIndex = serverIndex;
					EquepInfo.ButtonMoveOut(false);
	//				if(parseInt(inv.itemID.Substring(0,1)) < 7 || inv.slotType == SlotType.Formula){
						EquepInfo.ButtonMoveOutBiJiao(false);
	//				}else{
	//					EquepInfo.ButtonMoveOutBiJiao(true);				
	//				}
					EquepInfo.MaiBag = this;
				}
				if(myType == SlotType.Cangku){
					EquepInfo.CloseDownButton();
				}
			}
			if(ShowOtherPlayer){
			ShowOtherPlayer.enabled = false;
			yield WaitForSeconds(1f);
			ShowOtherPlayer.enabled = true;
			}
			
		}
		if(inv != null){
			if(inv.slotType == SlotType.Digest || inv.slotType == SlotType.Soul){
					try{
						invObj.GetComponent(SoulControl).LookDitemInfo(inv);			
					}catch(e){
						YuanInfo.gameObject.SetActiveRecursively(true);	
						YuanInfo.transform.position = transform.position;	
						YuanInfo.SetItemID(inv.itemID);
					}
			}
			if( (!canMove || isJiaoYi) && EquepInfo == null && YuanInfo != null){
				YuanInfo.gameObject.SetActiveRecursively(true);	
				YuanInfo.transform.position = transform.position;	
				YuanInfo.SetItemID(inv.itemID);
			}
		}
		if(myType == SlotType.BagDigest && MySoulLevel >= SoulControl.NumDigestButtons && invObj != null){
			invObj.SendMessage("ButtonPlusDigestBag" , SendMessageOptions.DontRequireReceiver);
		}
		if(isFanPai){
				YuanInfo.gameObject.SetActiveRecursively(true);	
				YuanInfo.transform.position = transform.position;	
				YuanInfo.SetItemID(inv.itemID);			
		}
	}

	function GetInvAsID(id : String){
		var iiv : InventoryItem;
		iiv = new InventoryItem();
		iiv = AllResources.InvmakerStatic.GetItemInfo(id,iiv);
		SetInv(iiv);
	}

	function GetItemID(auction : AuctionSell)
	{
	    if(null != inv)
	    {
	        auction.ItemID = inv.itemID;
	    }
	}

//	@SerializeField
//	@HideInInspector
	var inv : InventoryItem = null; 
	private var BagI : BagItem;
	//var QR : QueRen;
	//var TS : TiShi;
	var canZB : boolean = false; 
	private var ps : PlayerStatus;
	//var LT : LootTable;
	private var bool1 : boolean = false;
	private var bool2 : boolean = false;
	private var bool3 : boolean = false;
	private var bool4 : boolean = false;
	private var bool5 : boolean = false;
	private var bool6 : boolean = false;
	private var bool7 : boolean = false;
	var SoulBag : BagItem;
	var MySoulLevel : int;
	var MySoul2Level : int;
	var isLookBangDing : boolean;
	var isEquepSoul : boolean = false;
	var canJiaoHuan : boolean = false;
	var isAuctionItem : boolean = false;
	function OnDrop(go : GameObject){
		if(ItemMove.mInv != null){
			BagI = go.GetComponent(BagItem); 
			if(! BagI)
				return;
			bool1 = (myType == SlotType.Bag && BagI.myType != SlotType.Shangdian);
			bool2 = (myType == ItemMove.mInv.slotType && ItemMove.mInv.professionType == InventoryControl.PlayerProfession && parseInt(InventoryControl.Plys.Level) >= ItemMove.mInv.itemLevel && parseInt(InventoryControl.Plys.PVPPoint) >= ItemMove.mInv.needPVPPoint);
			bool3 = (myType == SlotType.Cangku);
			bool4 = (myType == SlotType.Bag && BagI.myType != myType && inv != null); 
//			bool5 = ((myType == SlotType.BagDigest && ItemMove.mInv.slotType == SlotType.Digest && MySoulLevel < SoulControl.NumDigestButtons) || (myType == ItemMove.mInv.slotType) || (myType == SlotType.Soul && ItemMove.mInv.slotType == SlotType.Soul));
			bool5 = ( (myType == SlotType.BagSoul && ItemMove.mInv.slotType == SlotType.Soul && inv == null) || (myType == SlotType.BagDigest && ItemMove.mInv.slotType == SlotType.Digest && MySoulLevel < SoulControl.NumDigestButtons) || (myType == ItemMove.mInv.slotType) || (myType == SlotType.Soul && ItemMove.mInv.slotType == SlotType.Soul));
			bool6 = true;
			bool7 = true;
			
			if(ItemMove.mInv.slotType == SlotType.Weapon1 && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "0" && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "3"){
				if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
					bool7 = (ItemMove.mInv.WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
				}else{
					bool7 = (ItemMove.mInv.WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
				}
			}
			if(ItemMove.mInv.slotType == SlotType.Soul ){
				
			}
			if(myType == ItemMove.mInv.slotType){
				if(ItemMove.mInv.professionType != InventoryControl.PlayerProfession){
					if(ItemMove.mInv.professionType != 0)
						AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info612"));
				}else
				if(parseInt(InventoryControl.Plys.Level) < ItemMove.mInv.itemLevel){
					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info613"));
				}else
				if(parseInt(InventoryControl.Plys.PVPPoint) < ItemMove.mInv.needPVPPoint){
					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info611"));
				}
			}
			if(! bool7){
			    if(!isAuctionItem)
				    AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1115"));
			}
			if(bool1 || bool2 || bool3 || bool5){  
				if(bool4){
					return;
				}
				if( !bool2 && ItemMove.mInv.slotType < 12 && ItemMove.mInv.slotType == myType){
					return;
				}
				if( !bool7 && ItemMove.mInv.slotType < 12 && ItemMove.mInv.slotType == myType){
					return;
				}
				if(isLookBangDing && ItemMove.mInv.itemHole > 0){
				    if(isAuctionItem) AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1171"));
					return;
				}
				if(myType == SlotType.Digest && SoulBag != null && SoulBag.inv == null){
					return;
				}
				if(myType == SlotType.Digest){
					if(ps == null && PlayerStatus.MainCharacter){
						ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
					}
					if(MySoul2Level > parseInt(ps.Level)){
						return;
					}
				}
				if(myType == SlotType.Weapon1){
					if(AllManage.InvclStatic.CanSkillAsID(ItemMove.mInv.WeaponType , EquepmentID)){
						return;
					}
				}
				if(myType == SlotType.Weapon1 && gameObject.name == "Equepment2"){
					if(ps == null && PlayerStatus.MainCharacter){
						ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
					}
					if(parseInt(ps.Level) < 50){
						return;
					}
				}
				switch(tutorialType){
					case TutorialType.equipSoul :
						if(AllManage.jiaochengCLStatic && AllManage.jiaochengCLStatic.JiaoChengID == 8 && AllManage.jiaochengCLStatic.step == 2){
							AllManage.jiaochengCLStatic.NextStep();
						}
						break;
					case TutorialType.equipDigest :
						if(AllManage.jiaochengCLStatic && AllManage.jiaochengCLStatic.JiaoChengID == 8 && AllManage.jiaochengCLStatic.step == 4){
							AllManage.jiaochengCLStatic.NextStep();
						}
						break;
				}
				AllManage.ItMoveStatic.isDrop = false;
				canJiaoHuan = true;
				var exchangeInv : InventoryItem;
				exchangeInv = itemMove.MoveClear();
				if(inv != null && inv.slotType == SlotType.Consumables && exchangeInv.slotType == SlotType.Consumables && inv.itemID.Substring(0,4) ==  exchangeInv.itemID.Substring(0,4)){
					AllManage.UICLStatic.contShowCategoryTips = false;
					AllManage.InvclStatic.AddBagItem(exchangeInv);
				}else{
					SetInv(exchangeInv);
				}
//				
				AllManage.InvclStatic.CopyToBagIt();
				invObj.SendMessage("UpdateBagItem",SendMessageOptions.DontRequireReceiver);
	//			invObj.SendMessage("UpdateEquipItem",SendMessageOptions.DontRequireReceiver);
				if( myType == SlotType.BagSoul || myType == SlotType.BagDigest || myType == SlotType.Soul || myType == SlotType.Digest ){
					invObj.SendMessage("UpdateBagSoulItem",SendMessageOptions.DontRequireReceiver);
					invObj.SendMessage("UpdateBagDigestItem",SendMessageOptions.DontRequireReceiver);	
					if(myType == SlotType.Soul){					
						invObj.SendMessage("UpdateOnEquepSoulItem",SendMessageOptions.DontRequireReceiver);	
					}else{
						invObj.SendMessage("UpdateEquepSoulItem",SendMessageOptions.DontRequireReceiver);	
					}
				}
			}else{
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
	 			if(myType > 11 && inv == null ){
					if(myType == SlotType.Shangdian &&  BagI.myType != SlotType.Shangdian){
						AllManage.inv = ItemMove.mInv;
						AllManage.realit = AllManage.bagit;
//						if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
//							AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiChu" , "NoMai" , AllManage.AllMge.Loc.Get("info284")+ItemMove.mInv.itemName+AllManage.AllMge.Loc.Get("info285") + ( ItemMove.mInv.costGold + ItemMove.mInv.costBlood * 500 )+AllManage.AllMge.Loc.Get("info286"));	
//						else
							YesMaiChu();
					}else
					if(myType == SlotType.Bag){
						AllManage.inv = ItemMove.mInv;
//						if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1){
//							if(ItemMove.mInv.costBlood != 0){
//								useStr = ItemMove.mInv.costBlood+AllManage.AllMge.Loc.Get("info297");
//							}else{
//								useStr = "";
//							}
//							AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiRu" , "NoMai" , AllManage.AllMge.Loc.Get("info287") +ItemMove.mInv.costGold+AllManage.AllMge.Loc.Get("info286")  + useStr + AllManage.AllMge.Loc.Get("messages072") + ItemMove.mInv.itemName+"？");	
//						}else{
							mServerIndex = BagI.serverIndex;
							YesMaiRu();
//						}
					}
				}
			}
		}
	}  

	private var useStr : String = "";
	function YesMaiChu(){
		AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.Sell , 500 , 0 , AllManage.inv.itemID , gameObject , "realYesMaiChu");
//		ps.UseMoney(  ToInt.StrToInt(AllManage.inv.costGold) * (-1) +  ToInt.StrToInt(AllManage.inv.costBlood) * 500 * (-1), 0);
//		AllManage.inv.costGold = ToInt.IntToStr(ToInt.StrToInt(AllManage.inv.costGold) * 2);
//		AllManage.inv.costBlood = ToInt.IntToStr(ToInt.StrToInt(AllManage.inv.costBlood) * 2);
//		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.OutItem , AllManage.inv.itemID);
//		SetInv(AllManage.inv);
//		AllManage.realit.OtherYiChu();
//		AllManage.bagit = null;
//		AllManage.realit = null;
//		AllManage.inv = null;
//		AllManage.InvclStatic.CopyToBagIt();
//		invObj.SendMessage("UpdateBagItem",SendMessageOptions.DontRequireReceiver);
	}
	
	function realYesMaiChu(){
		if(AllManage.InvclStatic.DesBagItem(AllManage.inv)){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.OutItem , AllManage.inv.itemID);
			AllManage.InvclStatic.UpdateBagItem();
		}
		AllManage.inv = null;
	}

	private var mServerIndex : int = 0;
	function YesMaiRu(){
	    if(AllManage.inv.slotType == SlotType.Formula || AllManage.inv.slotType == SlotType.Rear || (AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 1 && AllManage.InvclStatic.TutorialsDetectionAsID("51")) || ps.isBlood( ToInt.StrToInt(AllManage.inv.costBlood)) && ps.isMoney( ToInt.StrToInt(AllManage.inv.costGold))){
			if(AllManage.inv.itemQuality == 0 && parseInt( AllManage.inv.itemID.Substring(0,1)) < 7){
				PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().GetRandomItem(mServerIndex - 12 , DungeonControl.level ,  ToInt.StrToInt(AllManage.inv.costGold) * (-1) * 2 ,  ToInt.StrToInt(AllManage.inv.costBlood) * (-1)));
			}else{
				PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().BuyStoreClient(AllManage.invCangKuStatic.nowSelectStoreType , mServerIndex ,  ToInt.StrToInt(AllManage.inv.costGold) * (-1) * 2,  ToInt.StrToInt(AllManage.inv.costBlood) * (-1) , DungeonControl.level , AllManage.inv.itemID));
			}
		}
		AllManage.inv = null;
//		if(AllManage.inv&& ps.isMoney(ToInt.StrToInt(AllManage.inv.costGold)) && ps.isBlood(ToInt.StrToInt(AllManage.inv.costBlood)) ){
//			ps.UseMoney(ToInt.StrToInt(AllManage.inv.costGold) , ToInt.StrToInt(AllManage.inv.costBlood));
//			var invStr : String;
//			var iiv : InventoryItem;
//			iiv = new InventoryItem();
//			if(AllManage.inv.itemQuality == 0 && parseInt( AllManage.inv.itemID.Substring(0,1)) < 7){
//				var ran : int;
//				var ran1 : int;
//				ran = Random.Range(1,1000);
//				if(ran > 998){
//					ran1 = 4;
//				}else
//				if(ran > 900){
//					ran1 = 3;
//				}else
//				if(ran > 200){
//					ran1 = 2;
//				}else{
//					ran1 = 1;					
//				}
//				invStr = AllResources.staticLT.MakeItemID2(invStr, ran1 , parseInt(AllManage.inv.itemID.Substring(0,1)) , parseInt(AllManage.inv.itemID.Substring(1,1))); 
//				iiv = AllResources.InvmakerStatic.GetItemInfo(invStr , iiv);
//				iiv.costGold = ToInt.IntToStr(ToInt.StrToInt(iiv.costGold) / 2);
//				iiv.costBlood = ToInt.IntToStr(ToInt.StrToInt(iiv.costBlood) / 2);
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , iiv.itemID);
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.ClientBuy , iiv.itemID);
//				itemMove.MoveCleraReal();
//				SetInv(iiv);						
//			}else{
//				invStr = AllManage.inv.itemID;
//				iiv = AllResources.InvmakerStatic.GetItemInfo(invStr , iiv);
//				iiv.costGold = ToInt.IntToStr(ToInt.StrToInt(iiv.costGold) / 2);
//				iiv.costBlood = ToInt.IntToStr(ToInt.StrToInt(iiv.costBlood) / 2);
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , iiv.itemID);
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.ClientBuy , iiv.itemID);
//				SetInv(iiv);						
//			}
//			yield;
//				AllManage.InvclStatic.CopyToBagIt();
//			invObj.SendMessage("UpdateBagItem",SendMessageOptions.DontRequireReceiver);
//	//		invObj.SendMessage("UpdateEquipItem",SendMessageOptions.DontRequireReceiver);	
//		}else{
//			AllManage.tsStatic.Show("tips001");													
//		}
//		AllManage.bagit.OtherYiChu();
	}

	function NoMai(){
		AllManage.inv = null;
	}

	function OtherZhuangBei(otInv : InventoryItem) : boolean{
		var  bbool : boolean = true;
			if(otInv.slotType == SlotType.Weapon1 && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "0" && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "3"){
				if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
					bbool = (otInv.WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
				}else{
					bbool = (otInv.WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
				}
			}
		if(myType == otInv.slotType && otInv.professionType == InventoryControl.PlayerProfession && parseInt(InventoryControl.Plys.Level) >= otInv.itemLevel  && parseInt(InventoryControl.Plys.PVPPoint) >= otInv.needPVPPoint && bbool){  
			return true;
		}else{
			if(myType == otInv.slotType){
				if(otInv.professionType != InventoryControl.PlayerProfession){
					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info612"));
				}else
				if(parseInt(InventoryControl.Plys.Level) < otInv.itemLevel){
					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info613"));
				}else
				if(parseInt(InventoryControl.Plys.PVPPoint) < otInv.needPVPPoint){
					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info611"));
				}
			}
			if(! bbool){
			    if(!isAuctionItem) 
			        AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1115"));
			}
		}
		return false;
	}

	function OtherZhuangBeiAuto(otInv : InventoryItem) : boolean{
		var  bbool : boolean = true;
			if(otInv.slotType == SlotType.Weapon1 && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "0" && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "3"){
				if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
					bbool = (otInv.WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
				}else{
					bbool = (otInv.WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
				}
			}
		if(myType == otInv.slotType && otInv.professionType == InventoryControl.PlayerProfession && parseInt(InventoryControl.Plys.Level) >= otInv.itemLevel  && parseInt(InventoryControl.Plys.PVPPoint) >= otInv.needPVPPoint && bbool){  
			return true;
		}else{
//			if(myType == otInv.slotType){
//				if(otInv.professionType != InventoryControl.PlayerProfession){
//					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info612"));
//				}else
//				if(parseInt(InventoryControl.Plys.Level) < otInv.itemLevel){
//					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info613"));
//				}else
//				if(parseInt(InventoryControl.Plys.PVPPoint) < otInv.needPVPPoint){
//					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info611"));
//				}
//			}
//			if(! bbool){
//				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1115"));
//			}
		}
		return false;
	}

	function OtherZhuangBeiAuto111(otInv : InventoryItem) : boolean{
		var  bbool : boolean = true;
			if(otInv.slotType == SlotType.Weapon1){
				if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
					bbool = (otInv.WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
				}else{
					bbool = (otInv.WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
				}
			}
		if(myType == otInv.slotType && otInv.professionType == InventoryControl.PlayerProfession && parseInt(InventoryControl.Plys.Level) >= otInv.itemLevel  && parseInt(InventoryControl.Plys.PVPPoint) >= otInv.needPVPPoint && bbool){  
			return true;
		}else{
//			if(myType == otInv.slotType){
//				if(otInv.professionType != InventoryControl.PlayerProfession){
//					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info612"));
//				}else
//				if(parseInt(InventoryControl.Plys.Level) < otInv.itemLevel){
//					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info613"));
//				}else
//				if(parseInt(InventoryControl.Plys.PVPPoint) < otInv.needPVPPoint){
//					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info611"));
//				}
//			}
//			if(! bbool){
//				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1115"));
//			}
		}
		return false;
	}

	function RealSetZHuangbei(otInv : InventoryItem){
			SetInv(otInv);
				AllManage.InvclStatic.CopyToBagIt();
				if(invObj)
				invObj.SendMessage("UpdateBagItem",SendMessageOptions.DontRequireReceiver);
	//			invObj.SendMessage("UpdateEquipItem",SendMessageOptions.DontRequireReceiver);
	}

	function InvSave(){
				AllManage.InvclStatic.CopyToBagIt();
				if(invObj)
				invObj.SendMessage("UpdateBagItem",SendMessageOptions.DontRequireReceiver);
	//			invObj.SendMessage("UpdateEquipItem",SendMessageOptions.DontRequireReceiver);
	}

	function InvOnDrapSave(){
				AllManage.InvclStatic.CopyToBagIt();
		invObj.SendMessage("UpdateBagItem",SendMessageOptions.DontRequireReceiver);
	//	invObj.SendMessage("UpdateEquipItem",SendMessageOptions.DontRequireReceiver);
		if( myType == SlotType.BagSoul || myType == SlotType.BagDigest || myType == SlotType.Soul || myType == SlotType.Digest ){
			invObj.SendMessage("UpdateBagSoulItem",SendMessageOptions.DontRequireReceiver);
			invObj.SendMessage("UpdateBagDigestItem",SendMessageOptions.DontRequireReceiver);	
			if(myType == SlotType.Soul){					
				invObj.SendMessage("UpdateOnEquepSoulItem",SendMessageOptions.DontRequireReceiver);	
			}else{
				invObj.SendMessage("UpdateEquepSoulItem",SendMessageOptions.DontRequireReceiver);	
			}
		}
	}

	var invSprite : UISprite;
	//var invMaker : Inventorymaker;
	var labelItemNum : UILabel; 
	var SpriteBiankuang : UISprite;
	var EquepmentID : int;
	var isMail : boolean = false;
	var mail : MailInfo;
	var SpriteWen : UISprite;
	function SetInv(v : InventoryItem){ 
//		print(v.itemID + " == v");
		try{
			if( ! itemMove){
				itemMove = AllManage.ItMoveStatic;	
			}
		}catch(e){
		}
		itemMove.MoveCleraReal();
		if(myType <= 11){
			//TD_info.changeEqui(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText);
			InvSave();
		}
		if(inv == null){	
			inv = v;
			invSprite.enabled = true;
			invSprite.spriteName = inv.atlasStr;
		}else{ 
			try{
				if(inv.itemmodle1 != null){
	//				Destroy(inv.itemmodle1);
					inv.itemmodle1.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle1.active = true;
				}
				if(inv.itemmodle2 != null){
	//				Destroy(inv.itemmodle2);
					inv.itemmodle2.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle2.active = true;
				}		
			}catch(e){
			}
			if(canJiaoHuan){
				canJiaoHuan = false;
				itemMove = AllManage.ItMoveStatic;
				itemMove.ItemJiaoHuan(inv);
			}
			inv = v;
			invSprite.spriteName = inv.atlasStr;
		}
	//	if(myType <= 11){
	//		InvOnDrapSave();
	//	}

	//	inv = v;
		if(isMail){
		if(mail!=null)
			mail.invStr = inv.itemID;
		}
		if(inv.slotType == SlotType.Consumables || inv.slotType == SlotType.Packs){
			if(labelItemNum){
				labelItemNum.text = inv.consumablesNum.ToString();
			}
		}else{
			if(labelItemNum){		
				labelItemNum.text = "";		
			}
		}
		var myInv : BagItemType = new BagItemType(); 
		myInv.inv = inv;
		myInv.myType = myType;
		SpriteBiankuang.enabled = true;
		if(inv.slotType < 12){
			if(inv.slotType  == 10 || inv.slotType == 11 ){
				if(inv.itemQuality < 6){
					SpriteBiankuang.spriteName = "yanse" + inv.itemQuality;				
				}else{
					SpriteBiankuang.spriteName = "yanse" + (inv.itemQuality-4);	
				}
			}else{
				if(inv.itemQuality < 6){
					SpriteBiankuang.spriteName = "yanse" + inv.itemQuality;				
				}else{
					SpriteBiankuang.spriteName = "yanse" + (inv.itemQuality-4);	
				}
			}
			
			switch(AllManage.InvclStatic.LookCanForceOver(inv)){
				case CommonDefine.Force_NON: 
//					labelItemNum.text = "[FF0000]↑";
					break;
				case CommonDefine.Force_Higher: 
				if(labelItemNum){
					labelItemNum.text = "[00ff21]▲";
					}
					break;
				case CommonDefine.Force_Lower: 
				if(labelItemNum){
					labelItemNum.text = "[FF0000]▼";
					}
					break;
			}
	
		}else{ 
			if(inv.slotType  != SlotType.Soul && inv.slotType == SlotType.Digest ){
				SpriteBiankuang.spriteName = "yanse" + 1;	
			}else{
				var intI : int = 0;
				if(inv.slotType != SlotType.Formula){
					intI = parseInt(inv.itemID.Substring(8,1));
				}else{
					intI = parseInt(inv.itemID.Substring(5,1));			
				}
				
				if(inv.slotType  == SlotType.Soul){
				if(intI < 6){
					SpriteBiankuang.spriteName = "yanse" + inv.itemQuality;				
				}else{
					SpriteBiankuang.spriteName = "yanse" + (intI-4);	
				}
				}else{
				if(intI < 6){
					SpriteBiankuang.spriteName = "yanse" + inv.itemQuality;				
				}else{
					SpriteBiankuang.spriteName = "yanse" + (intI-4);	
				}
	//			SpriteBiankuang.enabled = false;			
			}
			}
		}
		if(InventoryControl.Plys == null&&PlayerStatus.MainCharacter !=null){
			InventoryControl.Plys = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		} 

			if((parseInt(InventoryControl.Plys.PVPPoint) < inv.needPVPPoint || parseInt(InventoryControl.Plys.Level) < inv.itemLevel || inv.professionType != InventoryControl.PlayerProfession) && inv.slotType < 12){  
				canZB = false;
			}else{
				canZB = true;
			}
			if(!canZB){
			if(labelItemNum){
				labelItemNum.text = "[FF0000]X";
				}
//					SpriteBiankuang.spriteName = "yanse" + 6;		
			}
			if(inv.itemQuality == 0 && parseInt(inv.itemID.Substring(0,1)) < 7){
				if(SpriteWen){
					SpriteWen.enabled = true;			
				}
				SpriteBiankuang.spriteName = "yanse" + 1;					
			}else{
				if(SpriteWen){
					SpriteWen.enabled = false;			
				}
			}
		if(mm == 9){ 
			SpriteBiankuang.enabled = true;
			SpriteBiankuang.spriteName = "yuanY" + inv.itemQuality;		
		}else
		if(mm == 10){
			SpriteBiankuang.enabled = true;
			invSprite.enabled = true;
			SpriteBiankuang.spriteName = "yuanY" + inv.itemQuality;		
		}
		
		if(mm == 0){
	//		//print(myType);
			if(invObj != null && myType < 11 && myType > 0){	
				if(invObj.GetComponent(InventoryControl)){
					invObj.GetComponent(InventoryControl).UpdatePES(inv , EquepmentID);
					invObj.GetComponent(InventoryControl).GoShowWeapon(myInv,EquepmentID);		
				}
			}
		}else
		if(mm == 5){
			if(invObj != null){	
				if(invObj.GetComponent(SoulControl)){
					invObj.GetComponent(SoulControl).GoShowSoul(myInv.inv);		
				}
			}	
		}
	}

	function ColseWen(){
		if(SpriteWen){
			SpriteWen.enabled = false;	
		}
	}

	function ColseWenOther(){
		if(SpriteWen){
			SpriteWen.enabled = false;	
		}
		if(labelItemNum){		
			labelItemNum.text = "";
		} 
		if(SpriteBiankuang){
			SpriteBiankuang.enabled = false;	
		}
	}

	function showConsumablesNum(){
			if(labelItemNum){		
			if(inv.consumablesNum == 0){
				labelItemNum.text = "";
			}else{
				labelItemNum.text = inv.consumablesNum.ToString();
			}
			}
	}

	var canMove : boolean = true;
	function OnDrag (delta : Vector2){
		if( Input.touchCount < 2){
			if(inv != null && (inv.slotType == SlotType.Soul || inv.slotType == SlotType.Digest) ){
				if(AllManage.SoulCLStatic.Building){
					return;
				}
			}
			if(invSprite.enabled && canMove){
				ItemMove.itemMove = true;
				AllManage.bagit = this;
				if(inv.slotType < 11){
					AllManage.InvclStatic.ShowLiangKuang(inv);
				}
				UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
				itemMove.MoveStart(inv,this);
				invSprite.enabled = false;
				if(inv.itemmodle1 != null){
	//				Destroy(inv.itemmodle1);
					inv.itemmodle1.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle1.active = true;
				}
				if(inv.itemmodle2 != null){
					inv.itemmodle2.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle2.active = true;
	//				Destroy(inv.itemmodle2);
				}
				if(inv.slotType == SlotType.Breastplate && myType == SlotType.Breastplate){
					AllManage.InvclStatic.GoShowMainTexture();
				}
				if(myType < 11){
					AllManage.InvclStatic.CloseInvItem(myType , EquepmentID);
				}
				inv = null;
				AllManage.InvclStatic.CopyToBagIt();
				AllManage.InvclStatic.UpdateBagItem();
				AllManage.ItMoveStatic.isDrop = true;
	//			invcl.UpdateEquipItem();
			if(mm == 0){
				if(invObj != null && myType < 11 && myType > 0){	
					iib = invObj.GetComponent(InventoryControl);
					if(iib){
						var myInv : BagItemType = new BagItemType(); 
						myInv.inv = inv;
						myInv.myType = myType;
						iib.UpdatePES(inv , EquepmentID);
						iib.GoShowWeapon(myInv,EquepmentID);		
	//					iib.UpdateEquipItem();		
	//					iib.();GetPersonEquipment
					}
				}
			}
				if(labelItemNum){		
					labelItemNum.text = "";		
				} 
				SpriteBiankuang.enabled = false;
				if(mm == 0){
					if(invObj != null && myType < 11 && myType > 0){	
						if(invObj.GetComponent(InventoryControl)){
							invObj.GetComponent(InventoryControl).reMoveWeapon(EquepmentID);		
						}
					}
				}else
				if(mm == 5){
					if(invObj != null){	
						if(invObj.GetComponent(SoulControl)){
							invObj.GetComponent(SoulControl).reMoveSoul();		
						}
					}	
				}
				
			if(isMail){
			if(mail!=null)
				mail.invStr = "";
			}
			}
			ColseWen();	
		}
	}
	var iib : InventoryControl;

	function OtherYiChu(){
			invSprite.enabled = false;
		if(inv != null){
			if(inv.itemmodle1 != null){
	//			Destroy(inv.itemmodle1);
					inv.itemmodle1.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle1.active = true;
			}
			if(inv.itemmodle2 != null){
					inv.itemmodle2.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle2.active = true;
	//			Destroy(inv.itemmodle2);
			}
		}
			inv = null;
		if(isMail){
		if(mail!=null)
			mail.invStr = "";
		}
			if(labelItemNum){		
				labelItemNum.text = "";		
			}
			SpriteBiankuang.enabled = false;
				AllManage.InvclStatic.CopyToBagIt();
			AllManage.InvclStatic.UpdateBagItem();
	//		invcl.UpdateEquipItem();
			ColseWen();
	}

	function invClear(){
		if(invSprite.enabled){
			invSprite.enabled = false;
		}
			if(labelItemNum){		
				labelItemNum.text = "";		
			}
			inv = null;
		SpriteBiankuang.enabled = false;
		if(isMail){
		if(mail!=null)
			mail.invStr = "";
		}
		ColseWen();
	}

	var liangKuang : UISprite;
	function OnEnable(){
		if(inv && ! inv.itemID){
			inv = null;
//			print(inv.itemID);
		}
	}
}

	function SetInv(v : InventoryItem , sss : int){ 
//		print(v.itemID + " == v");
		try{
			if( ! itemMove){
				itemMove = AllManage.ItMoveStatic;	
			}
		}catch(e){
		}
//		itemMove.MoveCleraReal();
		if(myType <= 11){
			//TD_info.changeEqui(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText);
			InvSave();
		}
		if(inv == null){	
			inv = v;
			invSprite.enabled = true;
			invSprite.spriteName = inv.atlasStr;
		}else{ 
			try{
				if(inv.itemmodle1 != null){
	//				Destroy(inv.itemmodle1);
					inv.itemmodle1.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle1.active = true;
				}
				if(inv.itemmodle2 != null){
	//				Destroy(inv.itemmodle2);
					inv.itemmodle2.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle2.active = true;
				}		
			}catch(e){
			}
			if(canJiaoHuan){
				canJiaoHuan = false;
				itemMove = AllManage.ItMoveStatic;
				itemMove.ItemJiaoHuan(inv);
			}
			inv = v;
			invSprite.spriteName = inv.atlasStr;
		}
	//	if(myType <= 11){
	//		InvOnDrapSave();
	//	}

	//	inv = v;
		if(isMail){
		if(mail!=null)
			mail.invStr = inv.itemID;
		}
		if(inv.slotType == SlotType.Consumables || inv.slotType == SlotType.Packs){
			if(labelItemNum){
				labelItemNum.text = inv.consumablesNum.ToString();
			
			}
		}else{
			if(labelItemNum){		
				labelItemNum.text = "";		
			}
		}
		var myInv : BagItemType = new BagItemType(); 
		myInv.inv = inv;
		myInv.myType = myType;
		SpriteBiankuang.enabled = true;
		if(inv.slotType < 12){
			if(inv.slotType  == 10 || inv.slotType == 11 ){
				if(inv.itemQuality < 6){
					SpriteBiankuang.spriteName = "yanse" + inv.itemQuality;				
				}else{
					SpriteBiankuang.spriteName = "yanse" + (inv.itemQuality-4);	
				}
			}else{
				if(inv.itemQuality < 6){
					SpriteBiankuang.spriteName = "yanse" + inv.itemQuality;				
				}else{
					SpriteBiankuang.spriteName = "yanse" + (inv.itemQuality-4);	
				}
			}
			
			switch(AllManage.InvclStatic.LookCanForceOver(inv)){
				case CommonDefine.Force_NON: 
//					labelItemNum.text = "[FF0000]↑";
					break;
				case CommonDefine.Force_Higher: 
				if(labelItemNum){
					labelItemNum.text = "[00ff21]▲";
					}
					break;
				case CommonDefine.Force_Lower: 
				if(labelItemNum){
					labelItemNum.text = "[FF0000]▼";
					}
					break;
			}
			
		}else{ 
			if(inv.slotType  != SlotType.Soul && inv.slotType == SlotType.Digest ){
				SpriteBiankuang.spriteName = "yanse" + 1;
		
			}else{
				var intI : int = 0;
				if(inv.slotType != SlotType.Formula){
					intI = parseInt(inv.itemID.Substring(8,1));
				}else{ 
					intI = parseInt(inv.itemID.Substring(5,1));			
				}
				
				if(inv.slotType  == SlotType.Soul){
				if(intI < 6){
					SpriteBiankuang.spriteName = "yanse" + intI;				
				}else{
					SpriteBiankuang.spriteName = "yanse" + (intI-4);	
				}
				}else{
				if(intI < 6){
					SpriteBiankuang.spriteName = "yanse" + intI;				
				}else{
					SpriteBiankuang.spriteName = "yanse" + (intI-4);	
				}
	//			SpriteBiankuang.enabled = false;			
			}
			}
		}
		if(InventoryControl.Plys == null&&PlayerStatus.MainCharacter !=null){
			InventoryControl.Plys = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		} 

			if((parseInt(InventoryControl.Plys.PVPPoint) < inv.needPVPPoint || parseInt(InventoryControl.Plys.Level) < inv.itemLevel || inv.professionType != InventoryControl.PlayerProfession) && inv.slotType < 12){  
				canZB = false;
			}else{
				canZB = true;
			}
			if(!canZB){
			if(labelItemNum){
				labelItemNum.text = "[FF0000]X";
				}
//					SpriteBiankuang.spriteName = "yanse" + 6;		
			}
			if(inv.itemQuality == 0 && parseInt(inv.itemID.Substring(0,1)) < 7){
				if(SpriteWen){
					SpriteWen.enabled = true;			
				}
				SpriteBiankuang.spriteName = "yanse" + 1;					
			}else{
				if(SpriteWen){
					SpriteWen.enabled = false;			
				}
			}
		if(mm == 9){ 
			SpriteBiankuang.enabled = true;
			SpriteBiankuang.spriteName = "yuanY" + inv.itemQuality;		
		}else
		if(mm == 10){
			SpriteBiankuang.enabled = true;
			invSprite.enabled = true;
			SpriteBiankuang.spriteName = "yuanY" + inv.itemQuality;			
			
		}
		
		if(mm == 0){
	//		//print(myType);
			if(invObj != null && myType < 11 && myType > 0){	
				if(invObj.GetComponent(InventoryControl)){
					invObj.GetComponent(InventoryControl).UpdatePES(inv , EquepmentID);
					invObj.GetComponent(InventoryControl).GoShowWeapon(myInv,EquepmentID);		
				}
			}
		}else
		if(mm == 5){
			if(invObj != null){	
				if(invObj.GetComponent(SoulControl)){
					invObj.GetComponent(SoulControl).GoShowSoul(myInv.inv);		
				}
			}	
		}
	}
