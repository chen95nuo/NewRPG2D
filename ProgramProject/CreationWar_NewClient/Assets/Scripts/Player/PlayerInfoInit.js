#pragma strict

	var PES : PersonEquipment[];
	var soulEqueps : InventoryItem[];
	private var JStr : String = "#";
	private var QStr : String = "@";
	private var Fstr : String = ";";
	var EquipStatus : int[];
	var ASkill : ActiveSkill;
	var PSkill : PassiveSkill;
	var ps : PlayerStatus;
	private var pRobot : PlayerRobot;

	function Awake(){
		ps = GetComponent(PlayerStatus);
		ASkill = GetComponent(ActiveSkill);
		PSkill = GetComponent(PassiveSkill);
		TPWeapon = GetComponent(ThirdPersonWeapon);
	}
	
	var proID : int;
	var playerName : String;
	var equipItem : String;
	var EquipItemSoul : String;
	var skillBranch : int;
	var skillskill : String;
	var playerLevel : int;
	var Stamina : int;
	var Strength : int;
	var Agility : int;
	var Intellect : int;
	var Focus : int;
	
	var playerSkillStr : String;
	var useSKstr : String[];
	var EquipItemStr : String;
	var equepmentIDs : int[];
	var playerID : int = 0;
	var battlefieldLabel : String = "";
	
	var rankInt : int;
	var selectTitle : String;
	var title : String;
	var VIPLevel : String;
	var GuildName : String;
	var PVPmyTeam : String;
	var forceValue : int = 0;
	var playerDuelState : int = 0;
    function SetBasisValue(objs : Object[]){
        SendMessage("SetRemontepos" , transform.position , SendMessageOptions.DontRequireReceiver);
		proID = objs[0];
		playerName = objs[1];
		equipItem = objs[2];
		EquipItemSoul = objs[3];
		skillBranch = objs[4];
		skillskill = objs[5];
		playerLevel = objs[6];
		Stamina = objs[7];
		Strength = objs[8];
		Agility = objs[9];
		Intellect = objs[10];
		Focus = objs[11];
		playerID = objs[12];
		battlefieldLabel = objs[13];
		
		ps.PVPmyTeam = battlefieldLabel;

		selectTitle = objs[15];
		rankInt = AllManage.InvclStatic.GetNPCRankLevelAsName(selectTitle);
		title = objs[16];
		VIPLevel = objs[17];
		GuildName = objs[18];
		PVPmyTeam = objs[19];
		forceValue = objs[24];
		var isRiding : int = 0;
		isRiding = objs[20];
		ps.Maxhealth = objs[22].ToString();
		ps.Maxmana = objs[23].ToString();
		ps.GuildName = GuildName;
		ps.PVPRank = objs[25];
 		playerDuelState = objs[26];
//		if(ArenaControl.areType == ArenaType.juedou && AllManage.areCLStatic && Application.loadedLevelName != "Map321"){
			if(ps.PVPmyTeam == "Red"){
				AllManage.areCLStatic.isArenaRed = true;
			}else
			if(ps.PVPmyTeam == "Blue"){
				AllManage.areCLStatic.isArenaBlue = true;
			}		
//		}
		ps.PlayerID = playerID; 
		ps.ProID = proID;
		SetEquepSoulItem(EquipItemSoul);
//		PlayerEquipmentInit(equipItem);

	
		
//		myYt = yt;
		if(UIControl.mapType != MapType.zhucheng){
			playerSkillStr = skillskill; 
			useSKstr = playerSkillStr.Split(Fstr.ToCharArray());
			ASkill.SetSkillLevel(useSKstr);
			PSkill.SetSkillLevel(useSKstr);	
			ASkill.RepeatingResetSkillAttribute();
//			PSkill.resetattr();
		}
		var i : int = 0;
		var o : int = 0;
		var useInv : InventoryItem;
		var useInvID : String[];
		
//		SkillPositions = new Array(4);
//		SaveSkillButtonStr = myYt.Rows[0]["SkillsPostion"].YuanColumnText;
//		useSKStr = SaveSkillButtonStr.Split(Fstr.ToCharArray());  
//		if(useSKStr.length > 2){
//			for(i =0; i<4; i++){
//				uStr = useSKStr[i].Split(DStr.ToCharArray()); 
//				if(uStr.length > 2){
//					SkillPositions[i] = uStr[1];
//				}else{
//					SkillPositions[i] = "";				
//				}
//			}
//		}
		
		EquipItemStr = equipItem;
		useInvID = EquipItemStr.Split(Fstr.ToCharArray());
		pRobot = GetComponent(PlayerRobot);
		if(pRobot){
			pRobot.enabled = true;		
		}
		ps.TBteam();
		ps.Health = objs[14].ToString();
		ps.Level = playerLevel.ToString();  
		ps.PlayerName = playerName;
		
		if(AllManage.areCLStatic){
			AllManage.areCLStatic.otherJueDouName = playerName;
			AllManage.areCLStatic.otherPlayerLevel = playerLevel;
		}
        //显示玩家昵称
		var colStr : String;
		if(VIPLevel == 9){
		    colStr = "[ff8400]";
		}else{
		    colStr = getNameColor("" + playerLevel);	
		}
		if( PlayerPrefs.GetInt("ShowNickNamePlayers", 1) == 1){
		    AllResources.FontpoolStatic.SpawnEffect(7 , transform , AllManage.InvclStatic.yanseLevel[ rankInt / 2 ] + selectTitle + "\n" + colStr+playerName + "\n" + GuildName , 5 , PVPmyTeam , GetComponent(PlayerStatus) , rankInt);			
			ps.ShowMyName();
//			ps.ShowMyName(AllManage.InvclStatic.yanseLevel[ rankInt / 2 ] + selectTitle + "\n" + colStr+playerName + "\n" + GuildName);		
		}
        yield;
		if(useInvID.length >= 2){
			for(i=0; i<useInvID.length; i++){	 
				if(useInvID[i] != ""){ 
					useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
					TPWeapon.ShowWeapon(useInv , equepmentIDs[i]);
					for(o=0; o<PES.length; o++){
						if(o == equepmentIDs[i]){
							PES[o].inv = useInv;
						}
					}
				}
			}
//			for(i=0; i<PES.length; i++){
//				if(PES[i].inv != null){
//					if(PES[i].inv.itemID != ""){
//						GetEquipStatus(PES[i] , i);			
//					}
//				}
//			}
//			BuildEquepPes();
			if(PES[10].inv == null){
				ps.weaponType = PlayerWeaponType.empty;
				ps.ChangeWeapons(PlayerWeaponType.empty);
			}else{
				ps.weaponType = PES[10].inv.WeaponType;
				ps.ChangeWeapons(PES[10].inv.WeaponType);			
			}
		}
		yield;
		yield;
		yield;
		yield;
		if(isRiding == 1){
			var selectMounts : String = "";
			selectMounts = objs[21];
				var canRideBool : boolean = false;
				if(UIControl.mapType == MapType.zhucheng){
					canRideBool = true;
				}else{
					if(selectMounts != "" && parseInt(selectMounts.Substring(4,1)) > 3)
						canRideBool = true;
				}
			if(canRideBool)
				ps.rideOn(parseInt(selectMounts.Substring(2,2)) - 1 , parseInt(selectMounts.Substring(5,1)));
		}
		return;
//		
//		switch(ps.ProID){
//			case 1:
//				ps.MultiStamina = 6;
//				break;
//			case 2:
//				ps.MultiStamina = 5;
//				break;
//			case 3:
//				ps.MultiStamina = 4;
//				break;
//		} 
//		ps.Stamina = Stamina;
//		ps.Strength = Strength;
//		ps.Agility =  Agility;
//		ps.Intellect = Intellect;
//		ps.Focus =  Focus;
//		ps.EquipItem = equipItem; 
//		ps.SetEquepInfo(EquipStatus);
//		ps.TBteam();
//		if(ps.ProID==1 || ps.ProID==2)
//			ps.Mana = "30";
//		else
//			ps.Mana = ps.Maxmana;
//	    if(UIControl.mapType == MapType.jingjichang){
//	    	 ps.Bei = 5;
//	    	 ps.Maxhealth = (parseInt(ps.Maxhealth) * 5).ToString();
//	    }else{
//	    	 ps.Bei = 1;
//	    	 ps.Maxhealth = (parseInt(ps.Maxhealth) * 1).ToString();	    
//	    }
//		ps.Health = ps.Maxhealth;  
//		ps.DoRetrie();
//		pRobot = GetComponent(PlayerRobot);
//		if(pRobot){
//			pRobot.enabled = true;		
//		}
    }

	function SetPlayerDuelState(state : int){
		playerDuelState = state;
		if(playerDuelState == 2){
		AllManage.UICLStatic.ObjSkillBox.SetActive(false);
		}
	}

    function getNameColor(str : String) : String{
        var useInt : int;
        useInt = parseInt(str);
        if(useInt < 20){
            return "[ffffff]";
        }else
            if(useInt < 40){
                return "[00ff00]";
            }else
                if(useInt < 60){
                    return "[0096ff]";
                }else
                    if(useInt < 80){
                        return "[b400ff]";
                    }
        return "";
    }
	
	/// <summary>
	///  配置魔魂
	/// </summary>
	/// <param name="invID">魔魂字段</param>
	function SetEquepSoulItem(invID : String){
		var i : int = 0;
		var useInv : InventoryItem;
		var useInvID : String[];
		useInvID = invID.Split(QStr.ToCharArray());	
		if(useInvID[0] != "" && useInvID.length > 1){
			useInv = AllResources.InvmakerStatic.GetItemInfo(invID , useInv);
			soulEqueps[6] = useInv;
			gameObject.SendMessage("InitCallObjectSoul",useInv.itemProAbt);
			
			var useInvID1 : String[]; 
			useInvID1 = useInvID[1].Split(JStr.ToCharArray());
			for(i=0; i<useInvID1.length; i++){	 
				if(useInvID1[i] != "" && parseInt(useInvID1[i]) > 0){ 
					useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID1[i] , useInv);
					soulEqueps[i] = useInv;
				}
			}
		}
	}

//var EquipStatus : int[];
var usePes : PersonEquipment;
private var EquipBei : float;
function GetEquipStatus(pes : PersonEquipment , useI : int){
	usePes = pes;
	var itemType : SlotType = pes.invType;
	var weaponType : PlayerWeaponType = pes.inv.WeaponType;
	var pfType : ProfessionType = pes.inv.professionType;
	var level : float = pes.inv.itemLevel;
	var quality : float = pes.inv.itemQuality;
	var endurance : float = pes.inv.itemEndurance;
	var proAbt : float = pes.inv.itemProAbt;
	var abt1 : String = pes.inv.itemAbt1;
	var abt2 : String = pes.inv.itemAbt2;
	var abt3 : String = pes.inv.itemAbt3;
	var hAttr1 : HoleAttr = pes.inv.holeAttr1;
	var hAttr2 : HoleAttr = pes.inv.holeAttr2;
	var hAttr3 : HoleAttr = pes.inv.holeAttr3;
	
	if(useI == 11){
		EquipBei = 5.0;
	}else{
		EquipBei = 1.0;
	}
	var intTen : int = 0;
	intTen = AllResources.InvmakerStatic.GetInvFenMuAsQuality(pes.inv);
	
	var pinzhi : float = 0;
	pinzhi = getQuality(quality , level);	

	if(weaponType == PlayerWeaponType.weapon1 || weaponType == PlayerWeaponType.weapon2){
		if(weaponType == PlayerWeaponType.weapon1 && pfType == ProfessionType.Soldier ){
			EquipStatus[0] += (0.56 * pinzhi + 40) / EquipBei;
			EquipStatus[1] += ((0.6*pinzhi + 40)*1.2) / EquipBei;		
				EquipStatus[0] = Mathf.Clamp(EquipStatus[0] , 1 , 999999);
				EquipStatus[1] = Mathf.Clamp(EquipStatus[1] , 1 , 999999);
		}else{
			EquipStatus[0] += (0.68 * pinzhi + 56) / EquipBei;			
				EquipStatus[0] = Mathf.Clamp(EquipStatus[0] , 1 , 999999);
		}
	}else{
		if(pfType == ProfessionType.Master){
			EquipStatus[1] += ((0.4 * pinzhi + 12) *AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;	
				EquipStatus[1] = Mathf.Clamp(EquipStatus[1] , 1 , 999999);
		}else
		if(pfType == ProfessionType.Robber){
			EquipStatus[1] += ((0.6 * pinzhi + 26) *AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;	
				EquipStatus[1] = Mathf.Clamp(EquipStatus[1] , 1 , 999999);
		}else
		if(pfType == ProfessionType.Soldier){
			EquipStatus[1] += ((0.8 * pinzhi + 40) *AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;	
				EquipStatus[1] = Mathf.Clamp(EquipStatus[1] , 1 , 999999);
		}
	}
	
	EquipStatus[5] += (pinzhi * endurance / intTen * 1.5 * AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;
				EquipStatus[5] = Mathf.Clamp(EquipStatus[5] , 1 , 999999);
//						print(EquipStatus[5] + " =a= " + pinzhi + " =b= " + endurance + " =c= " + itemType + " =d= " + AllManage.InvclStatic.itemXiuZheng[itemType - 1]);

	switch(pfType){
		case ProfessionType.Soldier : 		
			EquipStatus[6] += (pinzhi * proAbt / intTen  * AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;  break;
				EquipStatus[6] = Mathf.Clamp(EquipStatus[6] , 1 , 999999);
		case ProfessionType.Robber : 
			EquipStatus[7] += (pinzhi * proAbt / intTen * AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;  break;
				EquipStatus[7] = Mathf.Clamp(EquipStatus[7] , 1 , 999999);
		case ProfessionType.Master : 
			EquipStatus[8] += (pinzhi * proAbt / intTen * AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;  break;
				EquipStatus[8] = Mathf.Clamp(EquipStatus[8] , 1 , 999999);
	}
	getAtb(abt1 , pes.inv.ATabt1.iValue, itemType , pfType);
	getAtb(abt2 , pes.inv.ATabt2.iValue, itemType , pfType);
	getAtb(abt3 , pes.inv.ATabt3.iValue, itemType , pfType); 
	getHole(hAttr1 , pes.inv.itemHole1);
	getHole(hAttr2 , pes.inv.itemHole2);
	getHole(hAttr3 , pes.inv.itemHole3);
}

function getHole(hAttr : HoleAttr , hole : String){ 
	if(hole == "00"){
		return;
	}
	var holeValue : int = 0;
	for(var rows : yuan.YuanMemoryDB.YuanRow in InventoryControl.GameItem.Rows){
		var str : String;
		str = "81" + parseInt(hAttr.hType).ToString() + hAttr.hValue.ToString();
		if(rows["ItemID"].YuanColumnText == str){
			holeValue = parseInt(rows["ItemValue"].YuanColumnText);
		}
	}
	switch(hAttr.hType){
		case holeType.atk :
			 EquipStatus[0] += (holeValue) / EquipBei; break;
		case holeType.zhuanzhu :
			 EquipStatus[3] += (holeValue) / EquipBei;  break;
		case holeType.baoji :
			 EquipStatus[4] += (holeValue) / EquipBei;  break;
		case holeType.def :
			 EquipStatus[2] += (holeValue) / EquipBei; break;
		case holeType.mokang :
			 EquipStatus[11] += (holeValue) / EquipBei; break;
	}
}

function getAtb(str : String , pin : int , type : SlotType , pfType : ProfessionType){
	var a1 : int = parseInt(str.Substring(0,1)); 
	var a2 : int = parseInt(str.Substring(1,1));  
	pin /= EquipBei;
	switch(a1){
		case 1: EquipStatus[4] += pin; break;
		case 2: EquipStatus[3] += pin; break;
		case 3: EquipStatus[2] += pin; break;
		case 4: 
			if(pfType == ProfessionType.Soldier) 
				EquipStatus[0] += pin; else
			if(pfType == ProfessionType.Robber) 
				EquipStatus[0] += pin; else
			if(pfType == ProfessionType.Master) {
				EquipStatus[0] += pin ; 
			}
			break;
		case 5: EquipStatus[11] += pin ; break;
		case 6: EquipStatus[12] += pin ; break;
		case 7: EquipStatus[9] += pin; break;
		case 8: 
			if(pfType == ProfessionType.Soldier) 
				EquipStatus[6] += pin; else
			if(pfType == ProfessionType.Robber) 
				EquipStatus[7] += pin; else
			if(pfType == ProfessionType.Master) 
				EquipStatus[8] += pin;
			break;
		case 9: 
			if(pfType == ProfessionType.Soldier) 
				EquipStatus[7] += pin ; else
			if(pfType == ProfessionType.Robber) 
				EquipStatus[6] += pin; else
			if(pfType == ProfessionType.Master) 
				EquipStatus[7] += pin ;
			break;
	}
}


function getQuality(qua : int , lv : int):int{
	if(qua >= 6){
		qua -= 4;
	}
	switch(qua){
		case 1: return lv*2;
		case 2: return lv*3+6;
		case 3: return lv*7+12;
		case 4: return lv*12+24;
		case 5: return lv*15+56;
	}
}


	/// <summary>
	/// 配置替身装备
	/// </summary>
	/// <param name="equStr">装备字段</param>
	function PlayerEquipmentInit( equStr : String){
			var i : int = 0;
			var useInv : InventoryItem;
			var useInvID : String[];
			useInvID = equStr.Split(Fstr.ToCharArray());
			if(useInvID.length < 2){
				return;
			}
			for(i=0; i<12; i++){	 
				if(useInvID[i] != ""){ 
					useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
					var myInv : BagItemType = new BagItemType(); 
					myInv.inv = useInv;
					myInv.myType = AllManage.InvclStatic.useEquipType[i];			
					UpdatePES(useInv , AllManage.InvclStatic.equepmentIDs[i]);
					GoShowWeapon(myInv , AllManage.InvclStatic.equepmentIDs[i]);		
				}
			}
	}

	function UpdatePES(inv : InventoryItem , equepmentID : int){
		var i : int = 0;
		for(i=0; i<PES.length; i++){
			if(i == equepmentID){
				PES[i].inv = inv;
			}
		}
	}

	var TPWeapon : ThirdPersonWeapon;
	var skillit : SkillItem[];
	function GoShowWeapon(myInv : BagItemType , equepmentID : int){
		if(TPWeapon == null){
			return;
		}
		var i : int = 0;
		if(myInv.myType == SlotType.Bag){
			if(myInv.inv.itemmodle1){
				myInv.inv.itemmodle1.transform.parent = null;
				myInv.inv.itemmodle1.SetActiveRecursively(false);
			}
			if(myInv.inv.itemmodle2){
				myInv.inv.itemmodle2.transform.parent = null;
				myInv.inv.itemmodle2.SetActiveRecursively(false);
			}
		}else
		{
			TPWeapon.ShowWeapon(myInv.inv , equepmentID);
		}
		for(i=0; i<skillit.length; i++){
			skillit[i].StartSetSkill();
		}
		GetPersonEquipment();
		
		
		for(i=0; i<EquipStatus.length; i++){
			EquipStatus[i] = AllManage.InvclStatic.EquipStatus[i];
		}
	}
	function GetPersonEquipment(){
		var i : int = 0;
		for(i=0; i<AllManage.InvclStatic.EquipStatus.length; i++){
			AllManage.InvclStatic.EquipStatus[i] = 0;
		}
		
		for(i=0; i<PES.length; i++){
			if(PES[i].inv != null){
				if(PES[i].inv.itemID != ""){
					AllManage.InvclStatic.GetEquipStatus(PES[i] , i);			
				}
			}
		}
		BuildEquepPes();
	}
	
	function BuildEquepPes(){
		var i : int = 0;
		for(i=0; i<soulEqueps.length; i++){
			if(soulEqueps[i] != null){
				if(i < 6){
					RealPes(soulEqueps[i]);
				}else
				if(i == 6){
					SetRealSoul(soulEqueps[i]);
				}
			}
		}
	}

	function RealPes(inv : InventoryItem){
		switch(inv.itemProAbt){
			case 1:
				EquipStatus[0] += inv.itemLevel * inv.itemQuality;
				break;
			case 2:
				EquipStatus[2] += inv.itemLevel * inv.itemQuality;
				break;
			case 3:
				EquipStatus[5] += inv.itemLevel * inv.itemQuality;
				break;
			case 4:
				EquipStatus[3] += inv.itemLevel * inv.itemQuality;
				break;
			case 5:
				EquipStatus[4] += inv.itemLevel * inv.itemQuality;
				break;
			case 6:
				EquipStatus[6] += inv.itemLevel * inv.itemQuality;
				break;
			case 7:
				EquipStatus[7] += inv.itemLevel * inv.itemQuality;
				break;
			case 8:
				EquipStatus[8] += inv.itemLevel * inv.itemQuality;
				break;
		}
	}

	var rs : RealSoul = new RealSoul();
	var peson : Transform;
	function SetRealSoul(inv : InventoryItem){
		rs.name = inv.itemName;
		rs.level = inv.itemLevel;
		rs.quality = inv.itemQuality;
		rs.attr = inv.itemProAbt;
		rs.attrLevel = inv.SoulExp;
		rs.skillLevel = inv.SkillLevel;
	}
