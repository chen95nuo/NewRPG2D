#pragma strict
var myYt :  yuan.YuanMemoryDB.YuanTable;
var ASkill : ActiveSkill;
var PSkill : PassiveSkill;
var ps : PlayerStatus;
var TPWeapon : ThirdPersonWeapon;
var PES : PersonEquipment[];

var useSKstr : String[];
private var Fstr : String = ";";
var playerSkillStr : String;
var EquipItemStr : String;
var equepmentIDs : int[];

var FStr : String = ";";
var DStr : String = ",";
var useSKStr : String[];
var uStr : String[];
var SaveSkillButtonStr : String = "";
var SkillPositions : String[];
var photonView : PhotonView;
function SetPlayerInfoAsYt(yt :  yuan.YuanMemoryDB.YuanTable){
	myYt = yt;
	playerSkillStr = myYt.Rows[0]["Skill"].YuanColumnText; 
	useSKstr = playerSkillStr.Split(Fstr.ToCharArray());
	ASkill.SetSkillLevel(useSKstr);
	PSkill.SetSkillLevel(useSKstr);	
	var i : int = 0;
	var o : int = 0;
	var useInv : InventoryItem;
	var useInvID : String[];
	
	SkillPositions = new Array(4);
	SaveSkillButtonStr = myYt.Rows[0]["SkillsPostion"].YuanColumnText;
	useSKStr = SaveSkillButtonStr.Split(Fstr.ToCharArray());  
	if(useSKStr.length > 2){
		for(i =0; i<4; i++){
			uStr = useSKStr[i].Split(DStr.ToCharArray()); 
			if(uStr.length > 2){
				SkillPositions[i] = uStr[1];
			}else{
				SkillPositions[i] = "";				
			}
		}
	}
	
	EquipItemStr = yt.Rows[0]["EquipItem"].YuanColumnText;
	useInvID = EquipItemStr.Split(Fstr.ToCharArray());
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
		for(i=0; i<PES.length; i++){
			if(PES[i].inv != null){
				if(PES[i].inv.itemID != ""){
					GetEquipStatus(PES[i] , i);			
				}
			}
		}

		if(PES[10].inv == null){
			ps.weaponType = PlayerWeaponType.empty;
			ps.ChangeWeapons(PlayerWeaponType.empty);
		}else{
			ps.weaponType = PES[10].inv.WeaponType;
			ps.ChangeWeapons(PES[10].inv.WeaponType);			
		}
	}
	ps.PlayerID = parseInt(myYt.Rows[0]["PlayerID"].YuanColumnText); 
	ps.ProID = parseInt(myYt.Rows[0]["ProID"].YuanColumnText);
					switch(ps.ProID){
					case 1:
						ps.MultiStamina = 6;
						break;
					case 2:
						ps.MultiStamina = 5;
						break;
					case 3:
						ps.MultiStamina = 4;
						break;
				} 
	ps.Level = myYt.Rows[0]["PlayerLevel"].YuanColumnText;  
	ps.PlayerName = myYt.Rows[0]["PlayerName"].YuanColumnText;
	ps.Exmult = parseInt(myYt.Rows[0]["Exmult"].YuanColumnText); 
	ps.Experience = myYt.Rows[0]["Exp"].YuanColumnText; 
	ps.Power = GetBDInfoInt("Power" , 0).ToString();
	ps.Prestige = GetBDInfoInt("Prestige" , 0).ToString();  
	ps.PVPPoint = GetBDInfoInt("PVPPoint" , 0).ToString();
	ps.ServingMoney = GetBDInfoInt("ServingMoney" , 0);
	ps.Stamina = GetBDInfoInt("Stamina" , 0);
	ps.Strength = GetBDInfoInt("Strength" , 0);
	ps.Agility =  GetBDInfoInt("Agility" , 0);
	ps.Intellect = GetBDInfoInt("Intellect" , 0);
	ps.Focus =  GetBDInfoInt("Focus" , 0);
	ps.NonPoint = GetBDInfoInt("NonPoint" , 0);
	ps.Bloodstone = GetBDInfoInt("Bloodstone" , 0).ToString();
	ps.Money = GetBDInfoInt("Money" , 0).ToString();
	ps.Soul = GetBDInfoInt("Soul" , 0);
	ps.SoulPower = GetBDInfoInt("SoulPower" , 0); 
	ps.PVPRank = GetBDInfoInt("Rank" , 0); 
	ps.EquipItem = myYt.Rows[0]["EquipItem"].YuanColumnText; 
	ps.HideHelmet = GetBDInfoInt("HideHelmet" , 0); 
	ps.AutoAITime = GetBDInfoInt("AutoAITime" , 0);
		if(ps.ProID==1 || ps.ProID==2)
			ps.Mana = "30";
		else
			ps.Mana = ps.Maxmana;
	    if(UIControl.mapType == MapType.jingjichang){
	    	 ps.Bei = 8;
//	    	 ps.Maxhealth = (parseInt(ps.Maxhealth) * 8).ToString();
	    }else{
	    	 ps.Bei = 2;
//	    	 ps.Maxhealth = (parseInt(ps.Maxhealth) * 2).ToString();	    
	    }
	ps.SetEquepInfo(EquipStatus);

ps.Health = ps.Maxhealth;  
	ps.DoRetrie();
	if(!photonView)
		photonView = GetComponent(PhotonView);
	if(photonView.isMine){
		GetComponent(PlayerRobot).enabled = true;
	}

	ASkill.RepeatingResetSkillAttribute();
}

function Start(){
	photonView = GetComponent(PhotonView);
if( ! photonView.isMine){
   yield WaitForSeconds(1);
   if(ps.Maxhealth == "0")
   photonView.RPC("Readnowattr",PhotonTargets.All);	
  }

}

@RPC
function Readnowattr(){
if(photonView.isMine){
  photonView.RPC("tongbushuxing",ps.Maxhealth,ps.Health,ps.Defense,ps.Resist,ps.Maxmana,ps.Mana);
  }
}

@RPC
function tongbushuxing(a1:String,a2:String,a3:int,a4:int,a5:String,a6:String){
if(! photonView.isMine){
ps.Maxhealth = a1;
ps.Health = a2;
ps.Defense = a3;
ps.Resist = a4;
ps.Maxmana = a5;
ps.Mana = a6;
}
}

function GetBDInfoInt(bd : String , it : int) : int{  
	var iii : int = 0;
	try{ 
		iii = parseInt(myYt.Rows[0][bd].YuanColumnText);
		return  iii;
	}catch(e){
		return it;	
	}
}


var EquipStatus : int[];
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
		}else{
			EquipStatus[0] += (0.68 * pinzhi + 56) / EquipBei;			
		}
	}else{
		if(pfType == ProfessionType.Master){
			EquipStatus[1] += ((0.4 * pinzhi + 12) *AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;	
		}else
		if(pfType == ProfessionType.Robber){
			EquipStatus[1] += ((0.6 * pinzhi + 26) *AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;	
		}else
		if(pfType == ProfessionType.Soldier){
			EquipStatus[1] += ((0.8 * pinzhi + 40) *AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;	
		}
	}
	
	EquipStatus[5] += (pinzhi * endurance / intTen * 1.5 * AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;
	switch(pfType){
		case ProfessionType.Soldier : 		
			EquipStatus[6] += (pinzhi * proAbt / intTen  * AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;  break;
		case ProfessionType.Robber : 
			EquipStatus[7] += (pinzhi * proAbt / intTen * AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;  break;
		case ProfessionType.Master : 
			EquipStatus[8] += (pinzhi * proAbt / intTen * AllManage.InvclStatic.itemXiuZheng[itemType - 1]) / EquipBei;  break;
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
