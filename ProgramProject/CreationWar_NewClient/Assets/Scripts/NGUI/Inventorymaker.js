var loot : InventoryItem;	//output

class InventoryObjectClass{
	var BreastplateTexture : Texture[]; 
	
	var helmetModleMeshs : Mesh[];
	var shoudleModleMeshsR : Mesh[];
	var shoudleModleMeshsL : Mesh[];
	var wristModleMeshsR : Mesh[];
	var wristModleMeshsL : Mesh[];
	var LegModelMeshsR : Mesh[];
	var LegModelMeshsL : Mesh[];
	var RearModelMeshs : Mesh[]; 
	
	var LongswordModleMeshs : Mesh[]; 
	var shortswordModleMeshs : Mesh[]; 
	var dunModleMeshs : Mesh[];
	var midlName : String[];
	var backName : String[];
	
	var iconSprite : UISprite;
}
//					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[8];
//					newModleMaterials.mainTexture = newModleTexture;
//					makeItem1.renderer.material = newModleMaterials;
//					useRearParticle = newRearParticles[8];

var zhanshiObj : InventoryObjectClass;
var fashiObj : InventoryObjectClass;
var gongshouObj : InventoryObjectClass;

var BreastplateTexture : Texture[];  

var helmetModleMeshs : Mesh[];
var shoudleModleMeshsR : Mesh[];
var shoudleModleMeshsL : Mesh[];
var wristModleMeshsR : Mesh[];
var wristModleMeshsL : Mesh[];
var LegModelMeshsR : Mesh[];
var LegModelMeshsL : Mesh[];
var RearModelMeshs : Mesh[]; 
var newRearModelMeshs : Mesh[]; 
var newRearParticles : GameObject[]; 

var newLTModelMeshs : Mesh[]; 
var newLTModleTexture : Texture;
var newLTModleMaterials : Material;
var newLTBreastplateTexture : Texture[];  


var newModleMaterials : Material;
var newModleTexture : Texture;
var LongswordModleMeshs : Mesh[]; 
var shortswordModleMeshs : Mesh[]; 
var dunModleMeshs : Mesh[];

var MaterialsArmmo : Material[]; 
var MaterialsWeapon : Material[];

var EffectsLongsword : GameObject[];
var EffectsLongaxe : GameObject[];

var Effectsshortsword : GameObject[];
var Effectsdun : GameObject[];

var EffectsBow : GameObject[];
var EffectsShortstaff : GameObject[];
var EffectsLongstaff : GameObject[];

var EffectsRear : GameObject[];
var Effectsshoudle : GameObject[];

var Lastsword : GameObject;
var LastEZicon : GameObject;
var LastEffect : GameObject;


var midlName : String[];
var backName : String[];

private var maketype = 0;
private var ModleMeshs1 : Mesh[];
private var ModleMeshs2 : Mesh[];
private var ModleMaterials : Material[]; 
private var BreastTexture : Texture[];
private var MainBodyTexture : Texture;

private var Effects1 : GameObject[];
private var Effects2 : GameObject[];
private var itemNametemp : String;
//////////////////////////////////////////
//private var SS : SkillsStats;
private var typ : int;
private var wei : int;
//////////////////////////////////////////
enum SlotType 	{Helmet=1,Breastplate=2,Spaulders=3,Gauntlets=4,Leggings=5,Rear=6,Belt=7,Collar=8,Ring=9,Weapon1=10,Weapon2=11,Consumables=12,Formula=13,Packs=14,Hand, Chest, Wrist,Expendable,Empty,Bag,Cangku,Shangdian,BagSoul,BagDigest,Soul,Digest,Ride}
enum ProfessionType {Soldier = 1, Robber = 2,  Master= 3}
enum WeaponType {jiandun, shuangdao, dajian, hammer,  Empty}
//enum abtType {baoji=0, jingzhun=1, def=2, atk=3,  mDef=4 , mana=5, huifu=6 , liliang=7 , minjie=8 , zhili=9}
enum abtType {baoji=1, zhuanzhu=2, def=3, atk=4,  mDef=5 , mana=6, huifu=7 , liliang=8 , minjie=9 , zhili=0}
enum holeType {atk=1, zhuanzhu=2, baoji=3, def=4,  mokang=5}
enum ConsumablesType{Gem=1, Fish=2, Cooking=3, Ore=4, Stone=5, Material1=6, Material2=7, Material3=8, Key=9 , Flag=10 , Food=11 , Box=12 , WaK=13}
enum PlayerWeaponType {empty=0 , weapon1=1 , weapon2=2}
var StoreItem : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("StoreItem","id");
var PlayerPet : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("PlayerPet1","id");
var mm : boolean = false;

function Awake(){
	AllResources.InvmakerStatic = this;
}

function Start(){
	if( !AllResources.InvmakerStatic){
		AllResources.InvmakerStatic = this;
	}
	wuqiTypeStrStatic = wuqiTypeStr;
	StoreItem = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytGameItem;
	PlayerPet = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerPet;
//	while(!mm){
//		if(!StoreItem.IsUpdate){
//			InRoom.GetInRoomInstantiate().GetYuanTable("select * from GameItem","DarkSword2",StoreItem);
//		}
//		if(StoreItem.Count > 0){
//			mm = true;
//		}
//		yield;
//	}
}

@System.Serializable
class itemABT{
	var iType : abtType;
	var iValue : int;
	var iStr : String[];
}

@System.Serializable
class HoleAttr{
	var hType : holeType;
	var hValue : int;
} 

@System.Serializable
class FormulaItemNeed{
	var consumablesType : ConsumablesType;
	var consumablesNum : String;
	var consumablesID : String;
}

@System.Serializable														//	Our Representation of an InventoryItem
class InventoryItem {
	var itemID : String;
	var itemName : String;													//	What the item will be called in the inventory
	var itemmodle1:GameObject;
	var itemmodle2:GameObject;
	var itemtexture:Texture;
	var itemLevel:int;	
	var itemAmmont:int;													//	What the item will look like in the inventory
	var itemQuality:int;	
	var itemEndurance : int;
	var itemProAbt : int;
	var itemBuild : String;
	var itemHole : int;									//	What the item will look like in the inventory
	var itemHole1 : String;
	var itemHole2 : String;
	var itemHole3 : String;
	var slotType : SlotType; 
	var ItemPinZhiLevel : int;
	var professionType : ProfessionType;
	var itemAtlas : UIAtlas;
	var atlasStr : String;
	var holeItems : String[];
	var holeAttr1 : HoleAttr;
	var holeAttr2 : HoleAttr;
	var holeAttr3 : HoleAttr;
	var consumablesType : ConsumablesType; 
	var consumablesValue : String;
	var consumablesNum : int;
	var itemAbt1 : String;
	var itemAbt2 : String;
	var itemAbt3 : String; 
	var ATabt1 : itemABT;
	var ATabt2 : itemABT;
	var ATabt3 : itemABT;
	var formulaItemNeed1 : FormulaItemNeed;
	var formulaItemNeed2 : FormulaItemNeed;
	var formulaItemNeed3 : FormulaItemNeed;
	var formulaItemNeed4 : FormulaItemNeed;
	var costGold : String = "0";
	var costBlood : String = "0";
	var weaponTypeStr : String[];
	var professionTypeStr : String[];
	var buildItemSlotType : SlotType;
	var ATatk : int = 0;
	var ATatkStr : String = "攻击";
	var ATarmor : int = 0;
	var AtarmorStr : String = "护甲";
	var ATdef : int = 0;
	var ATdefStr : String = "魔防";
	var ATmoDef : int = 0;
	var ATmoDefrStr : String = "防御";
	var ATnaili : int = 0;
	var ATnailiStr : String = "耐力";
	var ATliliang : int = 0;
	var ATliliangStr : String = "力量";
	var ATminjie : int = 0;
	var ATminjieStr : String = "敏捷";
	var ATzhili : int = 0;
	var ATzhiliStr : String = "智力";
	var ATbaoji : int = 0;
	var ATbaojiStr : String = "暴击";
	var ATZhuanZhu : int = 0;
	var ATZhuanZhuStr : String = "精准";
	var ATmana : int = 0;
	var ATmanaStr : String = "魔法";
	var AThuifu : int = 0;
	var AThuifuStr : String = "恢复";
	var ATzongfen : int = 0;
	var ATzongfenStr : String = "总分";
	var WeaponType : PlayerWeaponType;
	var WeaponPlus : int = 0;
	var ItemInfo : String;
	var SoulExp : int;
	var SkillLevel : int;
	var needPVPPoint : int = 0;
	var needPVEPoint : int = 0;
	var needHeroBadge : int = 0;
	var needConquerBadge : int = 0;
	var LevelNum : int = 0;
}

var WeaponTypeStr : String[];
var ProfessionTypeStr : String[];
var wuqiTypeStr : String[];
static var wuqiTypeStrStatic : String[];

var AbtTypeStr : String[];
var FormulaTypeStrZhanShi : String[];
var FormulaTypeStrFaShi : String[];
var FormulaTypeStrYouXia : String[];
@System.Serializable														//	Our Representation of an Equipment Slot
class EquipmentSlot {
	var slotName : String;
    var slotType : SlotType; 
}

var InvAtlas : UIAtlas;
private var useID : int = 0;
function GetItem(ItemID : String, inv: InventoryItem){
	if(ItemID.Substring(0,1) == "x"){
		ItemID = parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText) + ItemID.Substring(1,ItemID.Length - 1);
	}else
	if(ItemID.Substring(0,1) == "y"){
		ItemID = (parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText) + 3) + ItemID.Substring(1,ItemID.Length - 1);
	}

	var isXiaoHao : boolean = false;
	inv = new InventoryItem();
	inv.weaponTypeStr = WeaponTypeStr;
	inv.professionTypeStr = ProfessionTypeStr;
	inv.holeItems = new Array(3);
	inv.itemID = ItemID;
	useID = parseInt(ItemID.Substring(0,1));
	
	if(useID <= 3){  
		inv.slotType = parseInt(ItemID.Substring(1,1)) + 9;  
	}else 
	if(useID < 7){ 
		inv.slotType = parseInt(ItemID.Substring(1,1));	
	}else
	if(useID == "J"){
		inv = GetFormulaInfo(ItemID,inv); 
		return inv;
	}else
	if(useID == 7){
		inv = GetSoulInfo(ItemID,inv);
		return inv;
	}else
	{
		isXiaoHao = true;
	}
var proID = 1;	
if(isXiaoHao){
	inv.slotType = parseInt(ItemID.Substring(0,1));
	inv.consumablesType = parseInt(ItemID.Substring(1,1)); 
	inv.consumablesValue = ItemID.Substring(2,2);
	inv.consumablesNum = parseInt(ItemID.Substring(6,2)); 
	inv.atlasStr = "UIP_Gem_" + ItemID.Substring(2,2);
}
else
{
	if(useID == 1 || useID == 4){
		inv.professionType = ProfessionType.Soldier; 
		inv.WeaponPlus = 0;
		proID =1;
	}else
	if(useID == 2 || useID == 5){
		inv.professionType = ProfessionType.Robber;	
		inv.WeaponPlus = 2;
		proID =2;
	}else
	if(useID == 3 || useID == 6){
		inv.professionType = ProfessionType.Master;	
		inv.WeaponPlus = 4;
		proID =3;	
	}
	
//	//print("get item == " + ItemID);
	inv.itemLevel = parseInt(ItemID.Substring(2,2)); 
	inv.itemQuality =  parseInt(ItemID.Substring(4,1));
	inv.itemEndurance = parseInt(ItemID.Substring(5,1));  
	inv.itemProAbt = parseInt(ItemID.Substring(6,1)); 
	inv.itemAbt1 = ItemID.Substring(7,2);   
	inv.itemAbt2 = ItemID.Substring(9,2); 
	inv.itemAbt3 = ItemID.Substring(11,2);    
	inv.itemBuild = ItemID.Substring(15,3);   
	inv.itemHole = parseInt(ItemID.Substring(18,1));   
	
	inv.itemHole1 = ItemID.Substring(19,2);   
	inv.itemHole2 = ItemID.Substring(21,2);   
	inv.itemHole3 = ItemID.Substring(23,2);
	inv.holeAttr1 = GetHoleAttr(inv.holeAttr1 , inv.itemHole1);
	inv.holeAttr2 = GetHoleAttr(inv.holeAttr2 , inv.itemHole2);
	inv.holeAttr3 = GetHoleAttr(inv.holeAttr3 , inv.itemHole3);
	
	var qNum : int = 0;
	if(inv.itemQuality > 5){
		qNum = inv.itemQuality - 4;
	}else{
		qNum = inv.itemQuality;		
	}
	inv.atlasStr =GetIconNum(inv.professionType , inv.slotType , inv.itemLevel , qNum);

	var pinzhi : int = 0;
	pinzhi = getQuality(inv.itemQuality , inv.itemLevel); 
	inv.ItemPinZhiLevel =  pinzhi ;
	if(inv.slotType == SlotType.Weapon1 || inv.slotType == SlotType.Weapon2){
		if(inv.slotType == SlotType.Weapon1 && inv.professionType == ProfessionType.Soldier ){
			inv.ATatk = 0.56 * pinzhi + 40;
			inv.ATarmor = (0.6*pinzhi + 40)*1.2;		
		}else{
			inv.ATatk = 0.68 * pinzhi + 56;			
		}
	}else{
		if(inv.professionType == ProfessionType.Master){
			inv.ATarmor = (0.4 * pinzhi + 12) *itemXiuZheng[inv.slotType - 1];	
		}else
		if(inv.professionType == ProfessionType.Robber){
			inv.ATarmor = (0.6 * pinzhi + 26) *itemXiuZheng[inv.slotType - 1];	
		}else
		if(inv.professionType == ProfessionType.Soldier){
			inv.ATarmor = (0.8 * pinzhi + 40) *itemXiuZheng[inv.slotType - 1];	
		}
	}
//	//print(inv.slotType); 
	var intTen : int = 0;
	intTen = GetInvFenMuAsQuality(inv);
	inv.ATnaili = pinzhi * inv.itemEndurance / intTen / 0.66 * itemXiuZheng[inv.slotType - 1];
	switch(inv.professionType){
		case ProfessionType.Soldier : 		
			inv.ATliliang = pinzhi * inv.itemProAbt / intTen / 0.66 * itemXiuZheng[inv.slotType - 1];  
			inv.ATliliang = Mathf.Clamp(inv.ATliliang , 1 , 999999);
			break;
		case ProfessionType.Robber : 
			inv.ATminjie = pinzhi * inv.itemProAbt / intTen / 0.66 * itemXiuZheng[inv.slotType - 1];  
			inv.ATminjie = Mathf.Clamp(inv.ATminjie , 1 , 999999);
			break;
		case ProfessionType.Master : 
			inv.ATzhili = pinzhi * inv.itemProAbt / intTen / 0.66 * itemXiuZheng[inv.slotType - 1];  
			inv.ATzhili = Mathf.Clamp(inv.ATzhili , 1 , 999999);
			break;
	}
	inv = getAtb(inv.itemAbt1 , pinzhi , inv.slotType , inv.professionType , inv , 1 , intTen);
	inv = getAtb(inv.itemAbt2 , pinzhi , inv.slotType , inv.professionType , inv , 2 , intTen);
	inv = getAtb(inv.itemAbt3 , pinzhi , inv.slotType , inv.professionType , inv , 3 , intTen);
	inv.ATzongfen = pinzhi + parseInt(inv.itemBuild);
//	switch(inv.professionType){
//		case 0 :
	var m:int;
	var n:int;
	m = inv.itemLevel/10*midlName.Length*0.1; 
	var Qnum =  inv.itemQuality;
	if(inv.itemQuality>5)
	 Qnum = inv.itemQuality- 4;
	n = Qnum+proID*5-6;
	var endName : String;
	endName = AllManage.AllMge.Loc.Get(inv.weaponTypeStr[inv.slotType]) ;
	if(inv.slotType == SlotType.Weapon1 || inv.slotType == SlotType.Weapon2){
		endName = GetWeaponName(ItemID , inv);
	}
	inv.itemName = AllManage.AllMge.Loc.Get(backName[n]) + AllManage.AllMge.Loc.Get(midlName[m]) + endName;
			inv = SoldierInv(ItemID , inv);
	
}
	if(inv.slotType == SlotType.Weapon2){
		inv.slotType = SlotType.Weapon1;
		inv.WeaponType = PlayerWeaponType.weapon2;
	}else
	if(inv.slotType == SlotType.Weapon1){
		inv.WeaponType = PlayerWeaponType.weapon1;
	}
	inv.ATatk = Mathf.Clamp(inv.ATatk , 1 , 999999);
	inv.ATarmor = Mathf.Clamp(inv.ATarmor , 1 , 999999);
	inv.ATnaili = Mathf.Clamp(inv.ATnaili , 1 , 999999);
	if(inv.ATabt1){
		inv.ATabt1.iValue = Mathf.Clamp(inv.ATabt1.iValue , 1 , 999999);	
	}
	if(inv.ATabt2){
		inv.ATabt2.iValue = Mathf.Clamp(inv.ATabt2.iValue , 1 , 999999);
	}
	if(inv.ATabt3){
		inv.ATabt3.iValue = Mathf.Clamp(inv.ATabt3.iValue , 1 , 999999);
	}
	return inv;
}


function GetHoleAttr(hAttr : HoleAttr , hole : String){
	if(hole != "00"){
		hAttr = new HoleAttr();
		hAttr.hType = parseInt(hole.Substring(0,1));
		hAttr.hValue = parseInt(hole.Substring(1,1));
		return hAttr;
	}
	return null;
}

private var TopLevel : int = 100;
var Effectbeijia : GameObject[];
var Effectdun : GameObject[];
var Effectfutou : GameObject[];
var Effectgong : GameObject[];
var Effectfazhang : GameObject[];
var Effectdajian : GameObject[];
var EffectShoudleL : GameObject[];
var EffectShoudleR : GameObject[];
var EffectShuangDao : GameObject[];

function SoldierInv(ItemID : String, inv: InventoryItem){  
	var i:int;
	var j:int;      //胸甲等级
	var m:int;      //模型等级
	var n:int;      //材质等级
	var p:int;      //特效等级和backName（中文前缀）
//	var q:int;      //中间名字
	var create : boolean = false;
	
	var EffectWuQi1 : GameObject[];
	var EffectWuQi2 : GameObject[];
	var EffectWuQi3 : GameObject[];
		create = true;
			switch(inv.professionType){
				case 1 : 
					shortswordModleMeshs = zhanshiObj.shortswordModleMeshs;
					dunModleMeshs = zhanshiObj.dunModleMeshs; 
					LongswordModleMeshs = zhanshiObj.LongswordModleMeshs; 
					
					EffectWuQi1 = Effectdajian;
					EffectWuQi2 = Effectdun;
					EffectWuQi3 = Effectdajian;
					
	                BreastplateTexture = zhanshiObj.BreastplateTexture ;
					helmetModleMeshs  = zhanshiObj.helmetModleMeshs ;
					shoudleModleMeshsR  = zhanshiObj.shoudleModleMeshsR ;
					shoudleModleMeshsL  = zhanshiObj.shoudleModleMeshsL ;
					wristModleMeshsR  = zhanshiObj.wristModleMeshsR ;
					wristModleMeshsL  = zhanshiObj.wristModleMeshsL ;
					LegModelMeshsR  = zhanshiObj.LegModelMeshsR ;
					LegModelMeshsL  = zhanshiObj.LegModelMeshsL ;
					RearModelMeshs  = zhanshiObj.RearModelMeshs ;
					break;
				case 2 :
					shortswordModleMeshs = gongshouObj.shortswordModleMeshs;
					dunModleMeshs = gongshouObj.dunModleMeshs; 
					LongswordModleMeshs = gongshouObj.LongswordModleMeshs; 

					EffectWuQi1 = EffectShuangDao;
					EffectWuQi2 = EffectShuangDao;
					EffectWuQi3 = Effectgong;

	                BreastplateTexture = gongshouObj.BreastplateTexture ;
					helmetModleMeshs  = gongshouObj.helmetModleMeshs ;
					shoudleModleMeshsR  = gongshouObj.shoudleModleMeshsR ;
					shoudleModleMeshsL  = gongshouObj.shoudleModleMeshsL ;
					wristModleMeshsR  = gongshouObj.wristModleMeshsR ;
					wristModleMeshsL  = gongshouObj.wristModleMeshsL ;
					LegModelMeshsR  = gongshouObj.LegModelMeshsR ;
					LegModelMeshsL  = gongshouObj.LegModelMeshsL ;
					RearModelMeshs  = gongshouObj.RearModelMeshs ;
					break;
				case 3 :
					shortswordModleMeshs = fashiObj.shortswordModleMeshs;
					dunModleMeshs = null; 
					LongswordModleMeshs = fashiObj.LongswordModleMeshs; 

					EffectWuQi1 = Effectfazhang;
					EffectWuQi2 = Effectfazhang;
					EffectWuQi3 = Effectfazhang;

	                BreastplateTexture = fashiObj.BreastplateTexture ;
					helmetModleMeshs  = fashiObj.helmetModleMeshs ;
					shoudleModleMeshsR  = fashiObj.shoudleModleMeshsR ;
					shoudleModleMeshsL  = fashiObj.shoudleModleMeshsL ;
					wristModleMeshsR  = fashiObj.wristModleMeshsR ;
					wristModleMeshsL  = fashiObj.wristModleMeshsL ;
					LegModelMeshsR  = fashiObj.LegModelMeshsR ;
					LegModelMeshsL  = fashiObj.LegModelMeshsL ;
					RearModelMeshs  = fashiObj.RearModelMeshs ;
					break;
			}

	
	switch(inv.slotType){
		case SlotType.Helmet:
			ModleMeshs1 = helmetModleMeshs;
			ModleMeshs2 = null; 
			ModleMaterials = MaterialsArmmo;
			Effects1 = null;
			Effects2 = null;
			create = true; 
			break;
		case SlotType.Breastplate:
			BreastTexture = BreastplateTexture;
			j =  inv.itemLevel/10*BreastTexture.Length*0.1 + inv.itemQuality/3;
			j = Mathf.Clamp(j , 0 , BreastTexture.length-1);
			MainBodyTexture =BreastTexture[j];
			create = false;	
			Effects1 = null;
			Effects2 = null;
			break;
		case SlotType.Spaulders:
			ModleMeshs1 = shoudleModleMeshsL;
			ModleMeshs2 = shoudleModleMeshsR;
			ModleMaterials = MaterialsArmmo;
			Effects1 = EffectShoudleL;
			Effects2 = EffectShoudleR;
			create = true;
			break;
		case SlotType.Gauntlets:
			ModleMeshs1 = wristModleMeshsL;
			ModleMeshs2 = wristModleMeshsR;
			ModleMaterials = MaterialsArmmo;
			Effects1 = null;
			Effects2 = null;
			create = true;
			break;
		case SlotType.Leggings:
			ModleMeshs1 = LegModelMeshsL;
			ModleMeshs2 = LegModelMeshsR;
			ModleMaterials = MaterialsArmmo;
			Effects1 = null;
			Effects2 = null;
			create = true;
			break;
		case SlotType.Rear:
			ModleMeshs1 = RearModelMeshs;
			ModleMeshs2 = null;
			ModleMaterials = MaterialsArmmo;
			Effects1 = Effectbeijia;
			Effects2 = null;
			create = true;
			break;
		case SlotType.Ring:
			create = false;
			break;
		case SlotType.Collar:
			create = false;
			break;
		case SlotType.Belt:
			create = false;
			break;
		case SlotType.Weapon1: 
					ModleMeshs1 = shortswordModleMeshs;
					ModleMeshs2 = dunModleMeshs;
			        ModleMaterials = MaterialsWeapon;
					Effects1 = EffectWuQi1;
					Effects2 = EffectWuQi2;
					create = true;
			break;
		case SlotType.Weapon2:
//					//print("wu qi2");
					ModleMeshs1 = LongswordModleMeshs;
					ModleMeshs2 = null;
			        ModleMaterials = MaterialsWeapon;
					Effects1 = EffectWuQi3;
					Effects2 = null;
					create = true;
			break;
	}
//	//print(ModleMeshs1 + " ==== ModleMeshs1");
	var createEffect : boolean = true;
	if(create){
		create = false;
	var Qnum =  inv.itemQuality;
	if(inv.itemQuality>5)
	 Qnum = inv.itemQuality- 4;
		m = (inv.itemLevel/12 + Qnum*0.5)*ModleMeshs1.Length*0.1;
		n = (inv.itemLevel/12+ Qnum*0.5)*ModleMaterials.Length*0.1;		 
		if(m>=ModleMeshs1.Length)
		  m = ModleMeshs1.Length-1;
		if(n>=ModleMaterials.Length)
		  n = ModleMaterials.Length-1;		
		if(inv.itemQuality == 9){
			if(inv.professionType == ProfessionType.Soldier){
				p = 4;
			}else
			if(inv.professionType == ProfessionType.Robber){
				p = 5;
			}else
			if(inv.professionType == ProfessionType.Master){
				p = 6;
			}
		}else{ 
			var intb : int = parseInt(inv.itemBuild);
			if(intb < 100){
				createEffect = false;
			}else
			if(intb < 200){
				p = 0;
			}else
			if(intb < 300){
				p = 1;
			}else
			if(intb < 400){
				p = 2;
			}else
			if(intb < 500){
				p = 3;
			}else{
				p = 7;
			}
		}
	var makeItem1 : GameObject;
	var rearParticle : GameObject = null;
	
	if(ModleMeshs1.length > 0){

		makeItem1 = new GameObject("makeditem1"+ItemID.ToString());
		makeItem1.AddComponent("MeshFilter");
		makeItem1.AddComponent("MeshRenderer");
		makeItem1.GetComponent(MeshFilter).mesh = ModleMeshs1[m]; 
		makeItem1.AddComponent("CloneMesh");
		makeItem1.GetComponent(MeshRenderer).enabled = false;	
		makeItem1.renderer.useLightProbes = true;
		makeItem1.renderer.material = ModleMaterials[n]; 
		var useRearParticle : GameObject = null;
		
			switch(inv.itemID.Substring(1,12)){
				case "620922422232":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[0];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material = newModleMaterials;
					useRearParticle = newRearParticles[0];
					break;
				case "640913121212":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[1];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material = newModleMaterials;
					useRearParticle = newRearParticles[1];
					break;
				case "660913422212":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[2];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material = newModleMaterials;
					useRearParticle = newRearParticles[2];
					break;
				case "660913824212":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[3];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material = newModleMaterials;
					useRearParticle = newRearParticles[3];
					break;
				case "660922432281":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[4];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material = newModleMaterials;
					useRearParticle = newRearParticles[4];
					break;
				case "660913822212":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[5];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material= newModleMaterials;
					useRearParticle = newRearParticles[5];
					break;
				case "660931826272":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[6];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material= newModleMaterials;
					useRearParticle = newRearParticles[6];
					break;
				case "660931325222":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[7];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material = newModleMaterials;
					useRearParticle = newRearParticles[7];
					break;
				case "660922824212":
					makeItem1.GetComponent(MeshFilter).mesh = newRearModelMeshs[8];
					newModleMaterials.mainTexture = newModleTexture;
					makeItem1.renderer.material = newModleMaterials;
					useRearParticle = newRearParticles[8];
					break;

			
				case "660822827262":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[0];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "660813122282":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[0];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "360813822212":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[1];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "360831826272":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[1];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "360813824212":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[2];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "360822432281":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[2];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "460813824212":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[3];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "460822432281":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[3];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "560813824212":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[4];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "560822432281":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[4];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "360831325222":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[5];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "360822824212":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[5];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "160831325222":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[6];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "160822824212":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[6];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "560822824212":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[7];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "560831325222":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[7];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "460831325222":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[8];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "460822824212":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[8];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "660822124282":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[9];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;
				case "660831325222":
					makeItem1.GetComponent(MeshFilter).mesh = newLTModelMeshs[9];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem1.renderer.material = newLTModleMaterials;
					break;				
			}
//var newLTModelMeshs : Mesh[]; 
//var newLTModleTexture : Texture;
//var newLTModleMaterials : Material;
		makeItem1.transform.position = Vector3(-1,-1000,0);
		makeItem1.name = ModleMeshs1[m].name;
		if(useRearParticle != null){
			rearParticle = GameObject.Instantiate(useRearParticle);
			rearParticle.transform.parent = makeItem1.transform;
			rearParticle.transform.localPosition = Vector3.zero;
			if(makeItem1.GetComponent(CloneMesh))
				makeItem1.GetComponent(CloneMesh).effectRear = rearParticle;
		}
		if(ModleMeshs1[m].name.Substring(0,4) == "futo"){
			Effects1 = Effectfutou;
		}
	}
		
	if(ModleMeshs2!=null && ModleMeshs2.length > 0){
		var makeItem2 : GameObject;
		makeItem2 = new GameObject("makeditem2"+ItemID.ToString());
		makeItem2.AddComponent("MeshFilter");
		makeItem2.AddComponent("MeshRenderer");
		makeItem2.GetComponent(MeshFilter).mesh = ModleMeshs2[m]; 
		makeItem2.AddComponent("CloneMesh");
		makeItem2.GetComponent(MeshRenderer).enabled = false;
		makeItem2.renderer.useLightProbes = true;
		makeItem2.renderer.material = ModleMaterials[n]; 
		
//---------------------		
			switch(inv.itemID.Substring(1,12)){
				case "360813822212":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[10];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "360831826272":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[10];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "360813824212":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[11];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "360822432281":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[11];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "460813824212":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[12];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "460822432281":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[12];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "560813824212":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[13];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "560822432281":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[13];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "360831325222":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[14];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "360822824212":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[14];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "560822824212":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[15];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "560831325222":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[15];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "460831325222":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[16];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;
				case "460822824212":
					makeItem2.GetComponent(MeshFilter).mesh = newLTModelMeshs[16];
					newModleMaterials.mainTexture = newLTModleTexture;
					makeItem2.renderer.material = newLTModleMaterials;
					break;			
			}
//---------------------
		makeItem2.transform.position = Vector3(1,-1000,0); 
		makeItem2.name = ModleMeshs2[m].name;
	}
		
		if(createEffect){
			if(Effects1!= null&&Effects1[p]!= null){
				var tempeffect1 = GameObject.Instantiate(Effects1[p],Vector3(-1,-1000,0), Quaternion.identity) as GameObject;           
				tempeffect1.transform.position = makeItem1.transform.position;
				tempeffect1.transform.rotation = makeItem1.transform.rotation;
				tempeffect1.transform.parent = makeItem1.transform;
				if(makeItem1.GetComponent(CloneMesh))
					makeItem1.GetComponent(CloneMesh).effect = tempeffect1;
			}
			if(Effects2!= null&&Effects2[p]!= null && makeItem2 != null){
				var tempeffect2 = GameObject.Instantiate(Effects2[p],Vector3(1,-1000,0), Quaternion.identity) as GameObject;           
				tempeffect2.transform.position = makeItem2.transform.position;
				tempeffect2.transform.rotation = makeItem2.transform.rotation;
				tempeffect2.transform.parent = makeItem2.transform;
				if(makeItem2.GetComponent(CloneMesh))
					makeItem2.GetComponent(CloneMesh).effect = tempeffect2;
			}
		}
		inv.itemmodle1 = makeItem1;
		inv.itemmodle2 = makeItem2; 
		if(makeItem1){
			makeItem1.SetActiveRecursively(false); 
		}
		if(makeItem2){		
			makeItem2.SetActiveRecursively(false);
		}
	}
		switch(inv.itemID.Substring(1,12)){
			case "260813822212" :
				MainBodyTexture = newLTBreastplateTexture[0];
				break;
			case "260831826272" :
				MainBodyTexture = newLTBreastplateTexture[0];
				break;
			case "260813824212" :
				MainBodyTexture = newLTBreastplateTexture[1];
				break;
			case "260822432281" :
				MainBodyTexture = newLTBreastplateTexture[1];
				break;
			case "260831325222" :
				MainBodyTexture = newLTBreastplateTexture[2];
				break;
			case "260822824212" :
				MainBodyTexture = newLTBreastplateTexture[2];
				break;
		}
		if(MainBodyTexture){
			inv.itemtexture = MainBodyTexture;		
		}
		return inv;
}

function MasterInv(ItemID : String, inv: InventoryItem){
	
	return inv;
}

function RobberInv(ItemID : String, inv: InventoryItem){
	
	return inv;
}

var NameSoul : String[];
var NameDigest : String[];
var InfoSoul : String[];
var InfoDigest : String[];

function GetSoulInfo(useID : String , inv : InventoryItem) : InventoryItem{
	var ItemID =  useID; 
	inv = new InventoryItem();
	inv.itemID = ItemID;
	inv.itemLevel = parseInt(ItemID.Substring(2,2));
	inv.itemQuality = parseInt(ItemID.Substring(4,1));
	inv.itemProAbt = parseInt(ItemID.Substring(5,1));
	if(ItemID.Substring(1,1) == "0"){
		inv.slotType = SlotType.Digest;
		inv.atlasStr = "Soul70" + inv.itemProAbt;
//		inv.itemName = NameDigest[inv.itemProAbt - 1];
//		inv.ItemInfo = InfoDigest[inv.itemProAbt - 1];
		inv.itemName = GetPetIDAsStr("ItemID" , ItemID.Substring(0,6) + "0000", "Name");
		inv.ItemInfo = GetPetIDAsStr("ItemID" , ItemID.Substring(0,6) + "0000", "ItemInfo");
	}else
	if(ItemID.Substring(1,1) == "1"){
		inv.slotType = SlotType.Soul;
		inv.atlasStr = "Soulpet" + inv.itemProAbt;
		inv.SoulExp = parseInt(useID.Substring(6,2));
		inv.SkillLevel = parseInt(useID.Substring(8,2));
//		inv.itemName = NameSoul[inv.itemProAbt - 1];
//		inv.ItemInfo = InfoSoul[inv.itemProAbt - 1];
		inv.itemName = GetPetIDAsStr("ItemID" , ItemID.Substring(0,10) , "Name");
		inv.ItemInfo = GetPetIDAsStr("ItemID" , ItemID.Substring(0,10) , "ItemInfo");
	}else
	if(ItemID.Substring(1,1) == "2"){
		inv.slotType = SlotType.Ride;
		inv.atlasStr = "Ride_UI" + ItemID.Substring(2,2);
		inv.itemName = GetRideIDAsStr("ItemID" , ItemID.Substring(0,10) , "Name");
		inv.ItemInfo = GetRideIDAsStr("ItemID" , ItemID.Substring(0,10) , "ItemInfo");
	}
	return inv;
}

private var DStr : String = ",";
function GetFormulaInfo(useID : String , inv : InventoryItem) : InventoryItem{
	var ItemID : String = "";
	var bool : boolean = false;
//	print(useID.Substring(0,1));
	if(useID.Substring(0,1) == "J"){
		useID = useID.Split(DStr.ToCharArray())[0];
		bool = true;
		var rows : yuan.YuanMemoryDB.YuanRow;
//		print(useID.Substring(1,useID.Length - 1) + " ==  jie qu de");
		rows = GetPeiFangAsID(useID.Substring(1,useID.Length - 1));
//		print(rows["BlueprintID"].YuanColumnText );
		useID = rows["BlueprintID"].YuanColumnText;
		ItemID =  useID.Substring(1,15); 
//		print(ItemID + " ==  huo de de ");
	}else{
		ItemID =  useID.Substring(1,15); 
	}
	inv = new InventoryItem();
	inv.weaponTypeStr = WeaponTypeStr;
	inv.professionTypeStr = ProfessionTypeStr;
	inv.holeItems = new Array(3);
	inv.itemID = ItemID;
	inv.slotType = SlotType.Formula;
	
	var thisuseID =  parseInt(ItemID.Substring(0,1));
	var thisslotType : SlotType;  
	
	if(thisuseID <= 3){  
		thisslotType = parseInt(ItemID.Substring(1,1)) + 9;  
	}else
	if(thisuseID <= 6){ 
		thisslotType = parseInt(ItemID.Substring(1,1));	
	}
	inv.buildItemSlotType = thisslotType;
	if(thisuseID == 1 || thisuseID == 4){
		inv.professionType = ProfessionType.Soldier; 
	}else
	if(thisuseID == 2 || thisuseID == 5){
		inv.professionType = ProfessionType.Robber;	
	}else
	if(thisuseID == 3 || thisuseID == 6){
		inv.professionType = ProfessionType.Master;		
	}
	
	inv.itemLevel = parseInt(ItemID.Substring(2,2)); 
	inv.itemQuality =  parseInt(ItemID.Substring(4,1));
	inv.itemEndurance = parseInt(ItemID.Substring(5,1));  
	inv.itemProAbt = parseInt(ItemID.Substring(6,1)); 
	inv.itemAbt1 = ItemID.Substring(7,2);   
	inv.itemAbt2 = ItemID.Substring(9,2); 
	inv.itemAbt3 = ItemID.Substring(11,2);    
	inv.itemBuild = "000";   
//	inv.itemHole = parseInt(ItemID.Substring(16,1));   
	
//	inv.itemHole1 = ItemID.Substring(17,2);   
//	inv.itemHole2 = ItemID.Substring(19,2);   
//	inv.itemHole3 = ItemID.Substring(21,2);
//	inv.holeAttr1 = GetHoleAttr(inv.holeAttr1 , inv.itemHole1);
//	inv.holeAttr2 = GetHoleAttr(inv.holeAttr2 , inv.itemHole2);
//	inv.holeAttr3 = GetHoleAttr(inv.holeAttr3 , inv.itemHole3); 
	
	var pinzhi : int = 0;
	pinzhi = getQuality(inv.itemQuality , inv.itemLevel); 
	inv.ItemPinZhiLevel =  pinzhi ;
	inv.needPVEPoint = inv.itemLevel*10 + pinzhi*10;	
	if(thisslotType == SlotType.Weapon1 || thisslotType == SlotType.Weapon2){
		if(thisslotType == SlotType.Weapon1 && inv.professionType == ProfessionType.Soldier ){
			inv.ATatk = 0.56 * pinzhi + 40;
			inv.ATarmor = (0.6*pinzhi + 40)*1.2;		
		}else{
			inv.ATatk = 0.68 * pinzhi + 56;			
		}
	}else{
		if(inv.professionType == ProfessionType.Master){
			inv.ATarmor = (0.4 * pinzhi + 12) *itemXiuZheng[thisslotType - 1];	
		}else
		if(inv.professionType == ProfessionType.Robber){
			inv.ATarmor = (0.6 * pinzhi + 26) *itemXiuZheng[thisslotType - 1];	
		}else
		if(inv.professionType == ProfessionType.Soldier){
			inv.ATarmor = (0.8 * pinzhi + 40) *itemXiuZheng[thisslotType - 1];	
		}
	}
//	//print(inv.slotType); 
	var intTen : int = 0;
	intTen = GetInvFenMuAsQuality(inv);
	inv.ATnaili = pinzhi * inv.itemEndurance / intTen *1.5 * itemXiuZheng[thisslotType - 1];
	switch(inv.professionType){
		case ProfessionType.Soldier : 		
			inv.ATliliang = pinzhi * inv.itemProAbt / intTen * itemXiuZheng[thisslotType - 1];  
			inv.ATliliang = Mathf.Clamp(inv.ATliliang , 1 , 999999);
			break;
		case ProfessionType.Robber : 
			inv.ATminjie = pinzhi * inv.itemProAbt / intTen  * itemXiuZheng[thisslotType - 1]; 
			inv.ATminjie = Mathf.Clamp(inv.ATminjie , 1 , 999999);
			break;
		case ProfessionType.Master : 
			inv.ATzhili = pinzhi * inv.itemProAbt / intTen  * itemXiuZheng[thisslotType - 1]; 
			inv.ATzhili = Mathf.Clamp(inv.ATzhili , 1 , 999999);
			break;
	}
	inv = getAtb(inv.itemAbt1 , pinzhi , thisslotType , inv.professionType , inv , 1 , intTen);
	inv = getAtb(inv.itemAbt2 , pinzhi , thisslotType, inv.professionType , inv , 2 , intTen);
	inv = getAtb(inv.itemAbt3 , pinzhi , thisslotType , inv.professionType , inv , 3 , intTen);
	inv.ATzongfen = pinzhi + inv.ATabt1.iValue + inv.ATabt2.iValue + inv.ATabt3.iValue + inv.ATnaili + inv.ATliliang + inv.ATminjie + inv.ATzhili;
	
	if(inv.itemLevel <= 25){
		inv.atlasStr = "Reel_" + 1;	
	}else
	if(inv.itemLevel <= 45){
		inv.atlasStr = "Reel_" + 2;
	}else
	{
		inv.atlasStr = "Reel_" + 3;
	}
	switch(inv.professionType){
		case 0:  inv.itemName = AllManage.AllMge.Loc.Get(FormulaTypeStrZhanShi[inv.itemQuality]) ;
		case 1:  inv.itemName = AllManage.AllMge.Loc.Get( FormulaTypeStrFaShi[inv.itemQuality]);
		case 2:  inv.itemName = AllManage.AllMge.Loc.Get(FormulaTypeStrYouXia[inv.itemQuality]) ;
	}
//	var consumablesType : ConsumablesType;
//	var consumablesNum : int;
//	var consumablesID : String;
	inv.itemID = useID;
	
	var useInt : int = 0;
	var useStr : String = "";
	inv.formulaItemNeed1 = new FormulaItemNeed();
	inv.formulaItemNeed1.consumablesID = useID.Substring(16,4);
	useInt = Mathf.Clamp((parseInt(useID.Substring(20,2) ) * itemXiuZheng[thisslotType - 1]) , 1 , 20);
	if(useInt < 10){
		useStr = "0" + useInt;
	}else{
		useStr = useInt.ToString() ;
	}
	inv.formulaItemNeed1.consumablesNum = useStr;
//	inv.formulaItemNeed1.consumablesNum = "0" + inv.formulaItemNeed1.consumablesNum;
	
	inv.formulaItemNeed2 = new FormulaItemNeed();
	inv.formulaItemNeed2.consumablesID = useID.Substring(22,4);
	useInt = Mathf.Clamp((parseInt(useID.Substring(26,2) ) * itemXiuZheng[thisslotType - 1]) , 1 , 20);
	if(useInt < 10){
		useStr = "0" + useInt;
	}else{
		useStr = useInt.ToString() ;
	}
	inv.formulaItemNeed2.consumablesNum = useStr;
//	inv.formulaItemNeed2.consumablesNum = "0" + inv.formulaItemNeed2.consumablesNum;
	inv.formulaItemNeed3 = new FormulaItemNeed();
	inv.formulaItemNeed3.consumablesID = useID.Substring(28,4);
	useInt = Mathf.Clamp((parseInt(useID.Substring(32,2) ) * itemXiuZheng[thisslotType - 1]) , 1 , 20);
	if(useInt < 10){
		useStr = "0" + useInt;
	}else{
		useStr = useInt.ToString() ;
	}
	inv.formulaItemNeed3.consumablesNum = useStr;
//	inv.formulaItemNeed3.consumablesNum = "0" + inv.formulaItemNeed3.consumablesNum;

	inv.formulaItemNeed4 = new FormulaItemNeed();
	inv.formulaItemNeed4.consumablesID = useID.Substring(34,4);
	useInt = Mathf.Clamp((parseInt(useID.Substring(38,2) ) * itemXiuZheng[thisslotType - 1]) , 1 , 20);
	if(useInt < 10){
		useStr = "0" + useInt;
	}else{
		useStr = useInt.ToString() ;
	}
	inv.formulaItemNeed4.consumablesNum = useStr;
//	inv.formulaItemNeed3.consumablesNum = "0" + inv.formulaItemNeed3.consumablesNum;
//	inv.slotType=14; 
	if(bool){
		inv.itemName = rows["BlueprintName"].YuanColumnText;
	}else{
		inv.itemName =  GetPeiFangNameAsID(useID); 
	}
	
	var rowsBlueprint : yuan.YuanMemoryDB.YuanRow;
	rowsBlueprint = GetPeiFangAsBlueprintID(useID);
	if(rowsBlueprint != null && rowsBlueprint["HeroStone"].YuanColumnText != ""){
		inv.needHeroBadge = parseInt(rowsBlueprint["HeroStone"].YuanColumnText);
	}
	
	inv.ATatk = Mathf.Clamp(inv.ATatk , 1 , 999999);
	inv.ATarmor = Mathf.Clamp(inv.ATarmor , 1 , 999999);
	inv.ATnaili = Mathf.Clamp(inv.ATnaili , 1 , 999999);
	if(inv.ATabt1){
		inv.ATabt1.iValue = Mathf.Clamp(inv.ATabt1.iValue , 1 , 999999);	
	}
	if(inv.ATabt2){
		inv.ATabt2.iValue = Mathf.Clamp(inv.ATabt2.iValue , 1 , 999999);
	}
	if(inv.ATabt3){
		inv.ATabt3.iValue = Mathf.Clamp(inv.ATabt3.iValue , 1 , 999999);
	}
	
	return inv;
}

function GetItemIDAsStr(selectStr : String , selectID : String , str : String) : String{
	for(var rows : yuan.YuanMemoryDB.YuanRow in StoreItem.Rows){
		if(rows[selectStr].YuanColumnText == selectID){
				return rows[str].YuanColumnText;
		}
	}
	return "";
}

function GetPacksLevelAsStr(selectStr : String , selectID : String , str : String) : String{
	for(var rows : yuan.YuanMemoryDB.YuanRow in StoreItem.Rows){
		if(rows[selectStr].YuanColumnText == selectID){
				return rows[str].YuanColumnText;
		}
	}
	return "";
}

function GetPetIDAsStr(selectStr : String , selectID : String , str : String) : String{
	for(var rows : yuan.YuanMemoryDB.YuanRow in PlayerPet.Rows){
		if(rows[selectStr].YuanColumnText.Substring(0,2) == selectID.Substring(0,2) && rows[selectStr].YuanColumnText.Substring(5,1) == selectID.Substring(5,1)){
				return rows[str].YuanColumnText;
		}
	}
	return "";
}

function GetRideIDAsStr(selectStr : String , selectID : String , str : String) : String{
	for(var rows : yuan.YuanMemoryDB.YuanRow in PlayerPet.Rows){
		if(rows[selectStr].YuanColumnText == selectID){
				return rows[str].YuanColumnText;
		}
	}
	return "";
}

function GetBDInfoInt(bd : String , it : int) : int{  
	var iii : int = 0;
	try{ 
		iii = parseInt(bd);
		return  iii;
	}catch(e){
		return it;	
	}
}

var invCangKu : InventoryCangku;
var itemXiuZheng : float[];
//enum SlotType 	{Helmet=1,Breastplate=2,Spaulders=3,Gauntlets=4,Leggings=5,Rear=6,Ring=7,Collar=8,Belt=9,Weapon1=10,Weapon2=11,Consumables=12,Formula=13,Packs=14,Hand, Chest, Wrist,Expendable,Empty,Bag,Cangku,Shangdian,BagSoul,BagDigest,Soul,Digest}
function GetItemInfo(ItemID : String, inv: InventoryItem){
try{
	if(ItemID.Substring(0,1) == "x"){
		ItemID = parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText) + ItemID.Substring(1,ItemID.Length - 1);
	}else
	if(ItemID.Substring(0,1) == "y"){
		ItemID = (parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText) + 3) + ItemID.Substring(1,ItemID.Length - 1);
	}
	ItemID = ItemID.Replace("-" , "");
	var isXiaoHao : boolean = false;
	inv = new InventoryItem();
	inv.weaponTypeStr = WeaponTypeStr;
	inv.professionTypeStr = ProfessionTypeStr;
	inv.holeItems = new Array(3);
	inv.itemID = ItemID;
//	//print(ItemID);
//	//print(ItemID + " == 1");
	if(ItemID.Substring(0,1) != "J"){
		useID = parseInt(ItemID.Substring(0,1));	
	}else{
		inv = GetFormulaInfo(ItemID,inv); 
		return inv;				
	}
//	//print(ItemID + " == 1");
	
	if(useID <= 3){  
		inv.slotType = parseInt(ItemID.Substring(1,1)) + 9;  
	}else
	if(useID <= 6){ 
		inv.slotType = parseInt(ItemID.Substring(1,1));	
	}else
	if(useID == 9){
		inv = GetFormulaInfo(ItemID,inv); 
		return inv;
	}else
	if(useID == 7){
		inv = GetSoulInfo(ItemID,inv);
		return inv;
	}else
	{
		isXiaoHao = true;	
	}
  
  var proID =1;
if(isXiaoHao){
	if(ItemID.Length == 4){
		ItemID += ",01";
		inv.itemID = ItemID;
	}
	inv.slotType = 12;
	var str1 : String;
	str1 = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "ItemLevel");
	var ItemCask : String;
	var ItemBlood : String;
	ItemCask = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "ItemCash");
	ItemBlood = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "ItemBlood");
	if(inv.itemID.Length < 8 && str1 != ""){
		inv.itemID = inv.itemID +","+ str1;
	}
	if(inv.itemID.Length > 8){
	    inv.itemQuality = parseInt(inv.itemID.Substring(8,1));
	}
	inv.consumablesType = parseInt(ItemID.Substring(1,1)); 
	inv.consumablesValue = ItemID.Substring(2,2);
	inv.consumablesNum = parseInt(ItemID.Substring(5,2)); 
	inv.itemName = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "Name");
	inv.itemLevel = GetBDInfoInt(GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "LevelID") , 0);
	inv.costGold = ToInt.IntToStr(Mathf.Clamp(inv.itemLevel * 100 * inv.consumablesNum * parseInt(str1) * parseInt(ItemCask) , 0 , 20000)) ;
	if(inv.itemID.Substring(0 , 2) == "82"){
		inv.costGold = ToInt.IntToStr(ToInt.StrToInt(inv.costGold) / 2);
	}
	
	if(GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "LevelNum") != "")
		inv.LevelNum = parseInt(GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "LevelNum"));
	if(ItemBlood != "0"){
		inv.costBlood = ToInt.IntToStr(inv.consumablesNum * parseInt(ItemBlood));
	}
	inv.ItemInfo = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "ItemInfo");
	switch(inv.consumablesType){
		case 1 : 	inv.atlasStr = "UIP_Gem_" + ItemID.Substring(2,2); break;
		case 2 : 	inv.atlasStr = "Yu" + ItemID.Substring(2,2); break;
		case 3 : 	inv.atlasStr = "PengRen_" + ItemID.Substring(2,2); break;
		case 4 : 	inv.atlasStr = "KuangShi" + ItemID.Substring(2,2); break;			
		case 5 : 	inv.atlasStr = "ShiTou_" + ItemID.Substring(2,2); break;			
		case 6 : 	inv.atlasStr = "6" + ItemID.Substring(2,2); break;			
		case 7 : 	inv.atlasStr = ItemID.Substring(1,3); break;			
		case 8 : 	inv.atlasStr = ItemID.Substring(0,3); break;			
		case 9 :
			inv.slotType = SlotType.Packs;
			switch(inv.itemID.Substring(2,1)){
				case "0" :
					inv.atlasStr = "box-"+ItemID.Substring(0,4); 
					break;
				case "1" :
					inv.atlasStr = "box-"+ItemID.Substring(0,4); 
					break;
				case "2" :
					inv.atlasStr = "key-"+ItemID.Substring(0,4); 
					break;
				case "3" :
					inv.atlasStr = "box-891"+ItemID.Substring(3,1); 
					break;
			}
			if(inv.itemID.Substring(0,3) == "890" || (inv.itemID.Substring(0,2) == "89" && parseInt(inv.itemID.Substring(2,1)) >=4 )){
				inv.atlasStr = "890";
			}
		break;	
	}
}
else
{
	inv.ItemInfo = "";
	if(useID == 1 || useID == 4){
		inv.professionType = ProfessionType.Soldier; 
		inv.WeaponPlus = 0;
		proID = 1;
	}else
	if(useID == 2 || useID == 5){
		inv.professionType = ProfessionType.Robber;	
		inv.WeaponPlus = 2;
		proID = 2;
	}else
	if(useID == 3 || useID == 6){
		inv.professionType = ProfessionType.Master;	
		inv.WeaponPlus = 4;
		proID = 3;	
	}
//	//print(ItemID + " == ");
	inv.itemLevel = parseInt(ItemID.Substring(2,2)); 
	inv.itemQuality =  parseInt(ItemID.Substring(4,1));
	var qNum : int = 0;
	if(inv.itemQuality > 5){
		qNum = inv.itemQuality - 4;
	}else{
		qNum = inv.itemQuality;		
	}
	inv.atlasStr =GetIconNum(inv.professionType , inv.slotType , inv.itemLevel , qNum);
	
	if(inv.itemQuality == 0){
		inv.itemName = "未知装备";
		if(inv.slotType == SlotType.Weapon2){
			inv.slotType = SlotType.Weapon1;
			inv.WeaponType = PlayerWeaponType.weapon2;
		}else
		if(inv.slotType == SlotType.Weapon1){
			inv.WeaponType = PlayerWeaponType.weapon1;
		}
		inv.costGold = ToInt.IntToStr(Mathf.Clamp(200 + (inv.itemLevel *  getQuality(3 , inv.itemLevel))*0.5 + itemXiuZheng[inv.slotType - 1] , 0 , 20000)) ;
		inv.costBlood = ToInt.IntToStr(Mathf.Clamp(inv.itemLevel * getQuality(3 , inv.itemLevel) * 0.02 , 1 , 9999999));
		return inv;
	}
	inv.itemEndurance = parseInt(ItemID.Substring(5,1));  
	inv.itemProAbt = parseInt(ItemID.Substring(6,1)); 
	inv.itemAbt1 = ItemID.Substring(7,2);   
	inv.itemAbt2 = ItemID.Substring(9,2); 
	inv.itemAbt3 = ItemID.Substring(11,2);    
	inv.itemBuild = ItemID.Substring(15,3);   
	inv.itemHole = parseInt(ItemID.Substring(18,1));   
	
	inv.itemHole1 = ItemID.Substring(19,2);   
	inv.itemHole2 = ItemID.Substring(21,2);   
	inv.itemHole3 = ItemID.Substring(23,2);
	inv.holeAttr1 = GetHoleAttr(inv.holeAttr1 , inv.itemHole1);
	inv.holeAttr2 = GetHoleAttr(inv.holeAttr2 , inv.itemHole2);
	inv.holeAttr3 = GetHoleAttr(inv.holeAttr3 , inv.itemHole3);

	var pinzhi : int = 0;
	pinzhi = getQuality(inv.itemQuality , inv.itemLevel); 
	inv.ItemPinZhiLevel =  pinzhi ;
	if(inv.slotType == SlotType.Weapon1 || inv.slotType == SlotType.Weapon2){
		if(inv.slotType == SlotType.Weapon1 && inv.professionType == ProfessionType.Soldier ){
			inv.ATatk = 0.56 * pinzhi + 40;
			inv.ATarmor = (0.6*pinzhi + 40)*1.2;		
		}else{
			inv.ATatk = 0.68 * pinzhi + 56;			
		}
	}else{
		if(inv.professionType == ProfessionType.Master){
			inv.ATarmor = (0.4 * pinzhi + 12) *itemXiuZheng[inv.slotType - 1];	
		}else
		if(inv.professionType == ProfessionType.Robber){
			inv.ATarmor = (0.6 * pinzhi + 26) *itemXiuZheng[inv.slotType - 1];	
		}else
		if(inv.professionType == ProfessionType.Soldier){
			inv.ATarmor = (0.8 * pinzhi + 40) *itemXiuZheng[inv.slotType - 1];	
		}
	}
	inv.costGold = ToInt.IntToStr(Mathf.Clamp(200 + (inv.itemLevel * pinzhi +  parseInt(inv.itemBuild) )*0.5 + itemXiuZheng[inv.slotType - 1] , 0 , 20000));
//	var needPVPPoint : int = 0;
//	var needPVEPoint : int = 0;
	if(inv.itemQuality == 2 || inv.itemQuality == 3 || inv.itemQuality == 4 || inv.itemQuality == 5 || inv.itemQuality == 7 || inv.itemQuality == 8 || inv.itemQuality == 9){
		inv.costBlood = ToInt.IntToStr(Mathf.Clamp(inv.itemLevel * pinzhi * 0.004 , 1 , 9999999));
	}
	if(inv.slotType == SlotType.Rear){
		inv.needPVPPoint = inv.itemLevel*5 + pinzhi*3;
	}
//	//print(inv.slotType);
	var intTen : int = 0;
	intTen = GetInvFenMuAsQuality(inv);
	inv.ATnaili = pinzhi * inv.itemEndurance / intTen * 1.5 * itemXiuZheng[inv.slotType - 1];
	switch(inv.professionType){
		case ProfessionType.Soldier : 		
			inv.ATliliang = pinzhi *inv.itemProAbt / intTen  * itemXiuZheng[inv.slotType - 1];  
			inv.ATliliang = Mathf.Clamp(inv.ATliliang , 1 , 999999);
			break;
		case ProfessionType.Robber : 
			inv.ATminjie = pinzhi * inv.itemProAbt / intTen * itemXiuZheng[inv.slotType - 1];  
			inv.ATminjie = Mathf.Clamp(inv.ATminjie , 1 , 999999);
			break;
		case ProfessionType.Master : 
			inv.ATzhili = pinzhi * inv.itemProAbt / intTen  * itemXiuZheng[inv.slotType - 1];  
			inv.ATzhili = Mathf.Clamp(inv.ATzhili , 1 , 999999);
			break;
	}
	
	inv = getAtb(inv.itemAbt1 , pinzhi , inv.slotType , inv.professionType , inv , 1 , intTen);
	inv = getAtb(inv.itemAbt2 , pinzhi , inv.slotType , inv.professionType , inv , 2 , intTen);
	inv = getAtb(inv.itemAbt3 , pinzhi , inv.slotType , inv.professionType , inv , 3 , intTen);
//	inv.ATzongfen = pinzhi + parseInt(inv.itemBuild);
	
	var m:int;
	var n:int;
	m = inv.itemLevel/10*midlName.Length*0.1; 
	var Qnum =  inv.itemQuality;
	if(inv.itemQuality>5)
	 Qnum = inv.itemQuality- 4;
	n = Qnum+proID*5-6;
	
//	m = inv.itemLevel/10*midlName.Length*0.1;
//	n = inv.itemLevel/10*backName.Length*0.1;
	if(inv.itemQuality < 7){
		var endName : String;
		endName = AllManage.AllMge.Loc.Get(inv.weaponTypeStr[inv.slotType]);
		if(inv.slotType == SlotType.Weapon1 || inv.slotType == SlotType.Weapon2){
			endName = GetWeaponName(ItemID , inv);
		}
		m = Mathf.Clamp(m , 0, midlName.length - 1);
		n = Mathf.Clamp(n , 0, backName.length - 1);
		inv.itemName = AllManage.AllMge.Loc.Get(backName[n]) + AllManage.AllMge.Loc.Get(midlName[m]) + endName;
	}else{
		inv.itemName = GetPeiFangNameAsID("9"+ItemID);	
	}
}
	if(inv.slotType == SlotType.Weapon2){
		inv.slotType = SlotType.Weapon1;
		inv.WeaponType = PlayerWeaponType.weapon2;
	}else
	if(inv.slotType == SlotType.Weapon1){
		inv.WeaponType = PlayerWeaponType.weapon1;
	}
	inv.ATatk = Mathf.Clamp(inv.ATatk , 1 , 999999);
	inv.ATarmor = Mathf.Clamp(inv.ATarmor , 1 , 999999);
	inv.ATnaili = Mathf.Clamp(inv.ATnaili , 1 , 999999);
	inv.ATzongfen = 0;
	switch(inv.professionType){
		case ProfessionType.Soldier : 		
			inv.ATzongfen += inv.ATnaili*1 + inv.ATliliang + inv.ATminjie*0.5+ inv.ATzhili*0.2 + inv.ATatk*2 + inv.ATarmor*0.2;
			break;
		case ProfessionType.Robber : 
			inv.ATzongfen += inv.ATnaili*1 + inv.ATliliang*0.5 + inv.ATminjie + inv.ATatk*2 + inv.ATarmor*0.2;
			break;
		case ProfessionType.Master : 
			inv.ATzongfen += inv.ATnaili*1 + inv.ATliliang*0.2 + inv.ATminjie*0.5+ inv.ATzhili + inv.ATatk*2 + inv.ATarmor*0.2;
			break;
	}
	if(inv.ATabt1){
		inv.ATabt1.iValue = Mathf.Clamp(inv.ATabt1.iValue , 1 , 999999);	
		inv.ATzongfen += GetCombatAsType(inv.ATabt1.iType , inv.ATabt1.iValue);
	}
	if(inv.ATabt2){
		inv.ATabt2.iValue = Mathf.Clamp(inv.ATabt2.iValue , 1 , 999999);
		inv.ATzongfen += GetCombatAsType(inv.ATabt2.iType , inv.ATabt2.iValue);
	}
	if(inv.ATabt3){
		inv.ATabt3.iValue = Mathf.Clamp(inv.ATabt3.iValue , 1 , 999999);
		inv.ATzongfen += GetCombatAsType(inv.ATabt3.iType , inv.ATabt3.iValue);
	}
	
	return inv;
}catch(e){
	return null;
}
}

function GetInvFenMuAsQuality(iv : InventoryItem) : int{
	var fenmu : int;
	var qua = iv.itemQuality;
	if(qua >= 6){
		qua -= 4;
	}
	switch(qua){
		case 1: fenmu = iv.itemEndurance + iv.itemProAbt;  break;
		case 2: fenmu = iv.itemEndurance + iv.itemProAbt + parseInt(iv.itemAbt1.Substring(1,1)); break;
		case 3: fenmu = iv.itemEndurance + iv.itemProAbt + parseInt(iv.itemAbt1.Substring(1,1)) + parseInt(iv.itemAbt2.Substring(1,1)); break;
		case 4: fenmu = iv.itemEndurance + iv.itemProAbt + parseInt(iv.itemAbt1.Substring(1,1)) + parseInt(iv.itemAbt2.Substring(1,1)) + parseInt(iv.itemAbt3.Substring(1,1)); break;
		case 5: fenmu = iv.itemEndurance + iv.itemProAbt + parseInt(iv.itemAbt1.Substring(1,1)) + parseInt(iv.itemAbt2.Substring(1,1)) + parseInt(iv.itemAbt3.Substring(1,1)); break;
	}
	return fenmu;
}

//function GetString(out str : String);

function GetWeaponName(id : String , iv : InventoryItem) : String{
	var realName : String;
	var obj : Mesh;
	obj = new Mesh();
	obj = SoldierInvOther(id,iv);
	if(obj){
		switch(obj.name.Substring(0,3)){
			case "daj":
				realName = "巨剑";
				break;
			case "jia":
				realName = "剑盾";
				break;
			case "faz":
				realName = "法杖";
				break;
			case "zha":
				realName = "短杖";
				break;
			case "gon":
				realName = "弓";
				break;
			case "shu":
				realName = "双刀";
				break;
			case "fut":
				realName = "长斧";
				break;
		}
	}
	return realName;
}

class IconNum{
	var Helmet : String[];
	var Breastplate : String[];
	var Spaulders : String[];
	var Gauntlets : String[];
	var Leggings : String[];
	var Rear : String[];
	var Ring : String[];
	var Collar : String[];
	var Belt : String[];
	var Weapon1 : String[];
	var Weapon2 : String[];
}

var iconZhanShi : IconNum;
var iconFaShi : IconNum;
var iconYouXia : IconNum;
function GetIconNum(ptype : ProfessionType , stype : SlotType , level : int , Qnum : int) : String{
	var str : String;
	var id : int;
	var useIcon : IconNum;
	switch(ptype){
		case ProfessionType.Soldier :
//			str += "ZanShi_"; 
			useIcon = iconZhanShi;
			break;
		case ProfessionType.Robber : 
//			str += "GongShou_"; 
			useIcon = iconYouXia;
			break;
		case ProfessionType.Master : 
//			str += "FaShi"; 
			useIcon = iconFaShi;
			break;
	}
	var useArray : String[];
	switch(stype){
		case SlotType.Helmet :
			useArray = useIcon.Helmet;
			break;
		case SlotType.Breastplate :
			useArray = useIcon.Breastplate;
			break;
		case SlotType.Spaulders :
			useArray = useIcon.Spaulders;
			break;
		case SlotType.Gauntlets :
			useArray = useIcon.Gauntlets;
			break;
		case SlotType.Leggings :
			useArray = useIcon.Leggings;
			break;
		case SlotType.Rear :
			useArray = useIcon.Rear;
			break;
		case SlotType.Ring :
			useArray = useIcon.Ring;
			break;
		case SlotType.Collar :
			useArray = useIcon.Collar;
			break;
		case SlotType.Belt :
			useArray = useIcon.Belt;
			break;
		case SlotType.Weapon1 :
			useArray = useIcon.Weapon1;
			break;
		case SlotType.Weapon2 :
			useArray = useIcon.Weapon2;
			break;
	}
	id = (level/12 + Qnum*0.5)*useArray.Length*0.1; 
//	 Mathf.Clamp(level/10*useArray.Length*0.1 , 0 , useArray.Length - 1);
//	//print(level  + " == " + useArray.Length + " == " + id);
	str = useArray[id];
	return str;
}

//	var ATatk : int = 0; 			//pes shuxing	
//	var ATarmor : int = 0;       //0 : gongji    
//	var ATdef : int = 0;         //1 : hujia		
//	var ATmoDef : int = 0;       //2 : fangyu	
//	var ATnaili : int = 0;       //3 : zhunque	
//	var ATliliang : int = 0;     //4 : baoji		
//	var ATminjie : int = 0;      //5 : naili		
//	var ATzhili : int = 0;       //6 : liliang	
//	var ATbaoji : int = 0;       //7 : minjie	
//	var ATjingzhun : int = 0;    //8 : zhili		
//	var ATmana : int = 0;        //9 : huifusudu	
//	var AThuifu : int = 0;       //10 : Matk		
//	var ATzongfen : int = 0;     //11 : Mdef 
                                 //12 : Mana		

function getAtb(str : String , pin : int , type : SlotType , pfType : ProfessionType , iv : InventoryItem , id : int , intTen : int){
	var a1 : int = parseInt(str.Substring(0,1)); 
	var a2 : int = parseInt(str.Substring(1,1));   
	var useiType : abtType; 
	var ivalue : int;
//	//print("fen mu == " + fenmu);
	var fenmu : int = Mathf.Clamp(parseInt(iv.itemID.Substring(8,1)) + parseInt(iv.itemID.Substring(10,1)) + parseInt(iv.itemID.Substring(12,1)) , 1,9999);
	var BuildInt : int;
	BuildInt = parseInt(iv.itemBuild) * a2 / fenmu;
	switch(a1){
		case 1: ivalue += pin * a2 / intTen * 1 * itemXiuZheng[type - 1] ; useiType = a1; break;
		case 2: ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; useiType = a1;break;
		case 3:ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; useiType = a1;break;
		case 4: ivalue += Mathf.CeilToInt(pin * a2 / intTen * 0.25) * itemXiuZheng[type - 1]; BuildInt = Mathf.CeilToInt( BuildInt* 0.25 ); useiType = a1;break;
		case 5: ivalue+= pin * a2 / intTen * 1 * itemXiuZheng[type - 1]; useiType = a1;break;
		case 6: ivalue += Mathf.CeilToInt( pin * a2 / intTen * 0.5) * itemXiuZheng[type - 1];  BuildInt = Mathf.CeilToInt( BuildInt * 0.5) ; useiType = a1;break;
		case 7: ivalue+= pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; useiType = a1;break;
		case 8: 
			if(pfType == ProfessionType.Soldier){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1];
				useiType = abtType.liliang;
				}else
			if(pfType == ProfessionType.Robber){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1];  
				useiType = abtType.minjie;
				}else
			if(pfType == ProfessionType.Master){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1];
				useiType = abtType.zhili;
				}
			break;
		case 9: 
			if(pfType == ProfessionType.Soldier){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1];   
				useiType = abtType.minjie;
				}else
			if(pfType == ProfessionType.Robber){
				ivalue+= pin * a2 / intTen / 1 * itemXiuZheng[type - 1];  
				useiType = abtType.liliang;
				}else
			if(pfType == ProfessionType.Master){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; 
				useiType = abtType.minjie;
				}
			break;
	} 
	var qua = iv.itemQuality; 
	var AtbNums : int; 
	var fenmu2 : int = 1; 
	
	if(qua >= 6){
		qua -= 4;
	}
	switch(qua){
		case 1: AtbNums = 0;  break;
		case 2: 
			if(id <= 1){
				AtbNums = 1;
			}else{
				AtbNums = 0;
			}
			fenmu2 = parseInt(iv.itemID.Substring(8,1));
			break;
		case 3: 
			if(id <= 2){
				AtbNums = 1;
			}else{
				AtbNums = 0;
			}
			fenmu2 = parseInt(iv.itemID.Substring(8,1)) + parseInt(iv.itemID.Substring(10,1));
			break;
		case 4: 
			AtbNums = 1; 
			fenmu2 = parseInt(iv.itemID.Substring(8,1)) + parseInt(iv.itemID.Substring(10,1)) + parseInt(iv.itemID.Substring(12,1));
			break;
		case 5: 
			AtbNums = 1; 
			fenmu2 = parseInt(iv.itemID.Substring(8,1)) + parseInt(iv.itemID.Substring(10,1)) + parseInt(iv.itemID.Substring(12,1));
			break;
	}
//				//print(pin + "-" +a2+"-"+itemXiuZheng[type - 1]);
	switch(id){
		case 1: iv.ATabt1 = new itemABT(); iv.ATabt1.iStr = AbtTypeStr; iv.ATabt1.iType = useiType; iv.ATabt1.iValue = (ivalue + parseInt(iv.itemBuild) * parseInt(iv.itemID.Substring(8,1)) / fenmu2)*AtbNums;  break;
		case 2: iv.ATabt2 = new itemABT(); iv.ATabt2.iStr = AbtTypeStr; iv.ATabt2.iType = useiType; iv.ATabt2.iValue = (ivalue + parseInt(iv.itemBuild) * parseInt(iv.itemID.Substring(10,1)) / fenmu2)*AtbNums; break;
		case 3: iv.ATabt3 = new itemABT(); iv.ATabt3.iStr = AbtTypeStr; iv.ATabt3.iType = useiType; iv.ATabt3.iValue = (ivalue + parseInt(iv.itemBuild) * parseInt(iv.itemID.Substring(12,1)) / fenmu2)*AtbNums; break;
	}
	return iv;
}
function getAtbOther(str : String , pin : int , type : SlotType , pfType : ProfessionType , iv : InventoryItem , id : int , intTen : int){
	var a1 : int = parseInt(str.Substring(0,1)); 
	var a2 : int = parseInt(str.Substring(1,1));   
	var useiType : abtType; 
	var ivalue : int;
//	//print("fen mu == " + fenmu);
	switch(a1){
		case 1: ivalue += Mathf.CeilToInt( pin * a2 / intTen *0.25) * itemXiuZheng[type - 1]; useiType = a1; break;
		case 2: ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; useiType = a1;break;
		case 3:ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; useiType = a1;break;
		case 4: 
			if(pfType == ProfessionType.Soldier) 
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; else
			if(pfType == ProfessionType.Robber) 
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; else
			if(pfType == ProfessionType.Master) 
				ivalue += pin * a2 / intTen / 5 * itemXiuZheng[type - 1];  
				useiType = a1;
			break;
		case 5: ivalue+= pin * a2 / intTen * 1.5 * itemXiuZheng[type - 1]; useiType = a1;break;
		case 6: ivalue += Mathf.CeilToInt( pin * a2 / intTen / 0.5) * itemXiuZheng[type - 1]; useiType = a1;break;
		case 7: ivalue+= pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; useiType = a1;break;
		case 8: 
			if(pfType == ProfessionType.Soldier){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1];
				useiType = abtType.liliang;
				}else
			if(pfType == ProfessionType.Robber){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1];  
				useiType = abtType.minjie;
				}else
			if(pfType == ProfessionType.Master){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1];
				useiType = abtType.zhili;
				}
			break;
		case 9: 
			if(pfType == ProfessionType.Soldier){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1];   
				useiType = abtType.minjie;
				}else
			if(pfType == ProfessionType.Robber){
				ivalue+= pin * a2 / intTen / 1 * itemXiuZheng[type - 1];  
				useiType = abtType.liliang;
				}else
			if(pfType == ProfessionType.Master){
				ivalue += pin * a2 / intTen / 1 * itemXiuZheng[type - 1]; 
				useiType = abtType.minjie;
				}
			break;
	}
//				//print(pin + "-" +a2+"-"+itemXiuZheng[type - 1]);
	var qua = iv.itemQuality; 
	var AtbNums : int; 
	var fenmu : int = 1; 
	
	if(qua >= 6){
		qua -= 4;
	}
	switch(qua){
		case 1: AtbNums = 0;  break;
		case 2: 
			if(id <= 1){
				AtbNums = 1;
			}else{
				AtbNums = 0;
			}
			fenmu = parseInt(iv.itemID.Substring(8,1));
			break;
		case 3: 
			if(id <= 2){
				AtbNums = 1;
			}else{
				AtbNums = 0;
			}
			fenmu = parseInt(iv.itemID.Substring(8,1)) + parseInt(iv.itemID.Substring(10,1));
			break;
		case 4: 
			AtbNums = 1; 
			fenmu = parseInt(iv.itemID.Substring(8,1)) + parseInt(iv.itemID.Substring(10,1)) + parseInt(iv.itemID.Substring(12,1));
			break;
		case 5: 
			AtbNums = 1; 
			fenmu = parseInt(iv.itemID.Substring(8,1)) + parseInt(iv.itemID.Substring(10,1)) + parseInt(iv.itemID.Substring(12,1));
			break;
	}
	switch(id){
		case 1: iv.ATabt1 = new itemABT(); iv.ATabt1.iStr = AbtTypeStr; iv.ATabt1.iType = useiType; iv.ATabt1.iValue = (ivalue + parseInt(iv.itemBuild) * parseInt(iv.itemID.Substring(8,1)) / fenmu)*AtbNums; break;
		case 2: iv.ATabt2 = new itemABT(); iv.ATabt2.iStr = AbtTypeStr; iv.ATabt2.iType = useiType; iv.ATabt2.iValue = (ivalue + parseInt(iv.itemBuild) * parseInt(iv.itemID.Substring(10,1)) / fenmu)*AtbNums; break;
		case 3: iv.ATabt3 = new itemABT(); iv.ATabt3.iStr = AbtTypeStr; iv.ATabt3.iType = useiType; iv.ATabt3.iValue = (ivalue + parseInt(iv.itemBuild) * parseInt(iv.itemID.Substring(12,1)) / fenmu)*AtbNums; break;
	}
	return iv;
}


function getQuality(qua : int , lv : int){ 
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

function GetID(ItemID : int){
	return ItemID;
}

  
function MakeItem (ItemID :int, inv: InventoryItem) : InventoryItem{
	return inv;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function DeleItem (ItemID){
var tempitem1 : GameObject = GameObject.Find("makeditem1"+ItemID.ToString());
if(tempitem1)
Destroy(tempitem1);
var tempitem2 : GameObject = GameObject.Find("makeditem2"+ItemID.ToString());
if(tempitem2)
Destroy(tempitem2);
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function FindItem (ItemID :int , inv : InventoryItem) : InventoryItem{
 return inv;
}

////////////////////////////////////////////////////////////////////////////////
//function LastMakeItem (ItemID :int, inv: InventoryItem) : InventoryItem{
//	return inv;
//}

////////////////////////////////////////////////////////////////////////////////
//function LastFindItem (ItemID :int, inv: InventoryItem) : InventoryItem{
//	return inv;
//}
function SoldierInvOther(ItemID : String, inv: InventoryItem) : Mesh{  
	var i:int;
	var j:int;      //胸甲等级
	var m:int;      //模型等级
	var n:int;      //材质等级
	var p:int;      //特效等级和backName（中文前缀）
//	var q:int;      //中间名字
	var create : boolean = false;
	
	var EffectWuQi1 : GameObject[];
	var EffectWuQi2 : GameObject[];
	var EffectWuQi3 : GameObject[];
		create = true;
			switch(inv.professionType){
				case 1 : 
					shortswordModleMeshs = zhanshiObj.shortswordModleMeshs;
					dunModleMeshs = zhanshiObj.dunModleMeshs; 
					LongswordModleMeshs = zhanshiObj.LongswordModleMeshs; 
					
					EffectWuQi1 = Effectdun;
					EffectWuQi2 = Effectfutou;
					EffectWuQi3 = Effectdajian;
					
	                BreastplateTexture = zhanshiObj.BreastplateTexture ;
					helmetModleMeshs  = zhanshiObj.helmetModleMeshs ;
					shoudleModleMeshsR  = zhanshiObj.shoudleModleMeshsR ;
					shoudleModleMeshsL  = zhanshiObj.shoudleModleMeshsL ;
					wristModleMeshsR  = zhanshiObj.wristModleMeshsR ;
					wristModleMeshsL  = zhanshiObj.wristModleMeshsL ;
					LegModelMeshsR  = zhanshiObj.LegModelMeshsR ;
					LegModelMeshsL  = zhanshiObj.LegModelMeshsL ;
					RearModelMeshs  = zhanshiObj.RearModelMeshs ;
					break;
				case 2 :
					shortswordModleMeshs = gongshouObj.shortswordModleMeshs;
					dunModleMeshs = gongshouObj.dunModleMeshs; 
					LongswordModleMeshs = gongshouObj.LongswordModleMeshs; 

					EffectWuQi1 = EffectShuangDao;
					EffectWuQi2 = EffectShuangDao;
					EffectWuQi3 = Effectgong;

	                BreastplateTexture = gongshouObj.BreastplateTexture ;
					helmetModleMeshs  = gongshouObj.helmetModleMeshs ;
					shoudleModleMeshsR  = gongshouObj.shoudleModleMeshsR ;
					shoudleModleMeshsL  = gongshouObj.shoudleModleMeshsL ;
					wristModleMeshsR  = gongshouObj.wristModleMeshsR ;
					wristModleMeshsL  = gongshouObj.wristModleMeshsL ;
					LegModelMeshsR  = gongshouObj.LegModelMeshsR ;
					LegModelMeshsL  = gongshouObj.LegModelMeshsL ;
					RearModelMeshs  = gongshouObj.RearModelMeshs ;
					break;
				case 3 :
					shortswordModleMeshs = fashiObj.shortswordModleMeshs;
					dunModleMeshs = null; 
					LongswordModleMeshs = fashiObj.LongswordModleMeshs; 

					EffectWuQi1 = Effectfazhang;
					EffectWuQi2 = Effectfazhang;
					EffectWuQi3 = Effectfazhang;

	                BreastplateTexture = fashiObj.BreastplateTexture ;
					helmetModleMeshs  = fashiObj.helmetModleMeshs ;
					shoudleModleMeshsR  = fashiObj.shoudleModleMeshsR ;
					shoudleModleMeshsL  = fashiObj.shoudleModleMeshsL ;
					wristModleMeshsR  = fashiObj.wristModleMeshsR ;
					wristModleMeshsL  = fashiObj.wristModleMeshsL ;
					LegModelMeshsR  = fashiObj.LegModelMeshsR ;
					LegModelMeshsL  = fashiObj.LegModelMeshsL ;
					RearModelMeshs  = fashiObj.RearModelMeshs ;
					break;
			}

	
	switch(inv.slotType){
		case SlotType.Helmet:
			ModleMeshs1 = helmetModleMeshs;
			ModleMeshs2 = null; 
			ModleMaterials = MaterialsArmmo;
			Effects1 = null;
			Effects2 = null;
			create = true; 
			break;
		case SlotType.Breastplate:
			BreastTexture = BreastplateTexture;
			j =  inv.itemLevel/10*BreastTexture.Length*0.1 + inv.itemQuality/3;
			j = Mathf.Clamp(j , 0 , BreastTexture.length - 1);
			MainBodyTexture =BreastTexture[j];
			create = false;	
			Effects1 = null;
			Effects2 = null;
			break;
		case SlotType.Spaulders:
			ModleMeshs1 = shoudleModleMeshsL;
			ModleMeshs2 = shoudleModleMeshsR;
			ModleMaterials = MaterialsArmmo;
			Effects1 = EffectShoudleL;
			Effects2 = EffectShoudleR;
			create = true;
			break;
		case SlotType.Gauntlets:
			ModleMeshs1 = wristModleMeshsL;
			ModleMeshs2 = wristModleMeshsR;
			ModleMaterials = MaterialsArmmo;
			Effects1 = null;
			Effects2 = null;
			create = true;
			break;
		case SlotType.Leggings:
			ModleMeshs1 = LegModelMeshsL;
			ModleMeshs2 = LegModelMeshsR;
			ModleMaterials = MaterialsArmmo;
			Effects1 = null;
			Effects2 = null;
			create = true;
			break;
		case SlotType.Rear:
			ModleMeshs1 = RearModelMeshs;
			ModleMeshs2 = null;
			ModleMaterials = MaterialsArmmo;
			Effects1 = Effectbeijia;
			Effects2 = null;
			create = true;
			break;
		case SlotType.Ring:
			create = false;
			break;
		case SlotType.Collar:
			create = false;
			break;
		case SlotType.Belt:
			create = false;
			break;
		case SlotType.Weapon1: 
					ModleMeshs1 = shortswordModleMeshs;
					ModleMeshs2 = dunModleMeshs;
			        ModleMaterials = MaterialsWeapon;
					Effects1 = EffectShuangDao;
					Effects2 = Effectdun;
					create = true;
			break;
		case SlotType.Weapon2:
//					//print("wu qi2");
					ModleMeshs1 = LongswordModleMeshs;
					ModleMeshs2 = null;
			        ModleMaterials = MaterialsWeapon;
					Effects1 = EffectWuQi3;
					Effects2 = null;
					create = true;
			break;
	}
	var createEffect : boolean = true;
	var makeItem1 : Mesh;
//	//print(create);
	if(create){
		create = false;
		var Qnum =  inv.itemQuality;
	if(inv.itemQuality>5)
	 Qnum = inv.itemQuality- 4;
		m = (inv.itemLevel/12 + Qnum*0.5)*ModleMeshs1.Length*0.1;
		n = (inv.itemLevel/12+ Qnum*0.5)*ModleMaterials.Length*0.1;	
		if(m>=ModleMeshs1.Length)
		  m = ModleMeshs1.Length-1;
		if(n>=ModleMaterials.Length)
		  n = ModleMaterials.Length-1;
//		m = inv.itemLevel/10*ModleMeshs1.Length*0.1;
//		n = inv.itemLevel/10*ModleMaterials.Length*0.1;		 
		
		if(inv.itemQuality == 9){
			if(inv.professionType == ProfessionType.Soldier){
				p = 4;
			}else
			if(inv.professionType == ProfessionType.Robber){
				p = 5;
			}else
			if(inv.professionType == ProfessionType.Master){
				p = 6;
			}
		}else{ 
			var intb : int = parseInt(inv.itemBuild);
			if(intb < 100){
				createEffect = false;
			}else
			if(intb < 200){
				p = 0;
			}else
			if(intb < 300){
				p = 1;
			}else
			if(intb < 400){
				p = 2;
			}else
			if(intb < 500){
				p = 3;
			}else{
				p = 7;
			}
		}
//		//print(ModleMeshs1.length);
		if(ModleMeshs1.length > 0){
			m = Mathf.Clamp(m , 0, ModleMeshs1.length - 1);
			makeItem1 = ModleMeshs1[m]; 
		}
//		//print(makeItem1 + " === " + m);
	}
//	//print(makeItem1);
	return makeItem1;
}

function GetCombatAsType(type : abtType , iValue : int){
	switch(type){
		case abtType.baoji : 
			return iValue*0.8;
		case abtType.zhuanzhu : 
			return iValue*2;
		case abtType.def : 
			return iValue*0.2;
		case abtType.liliang : 
			return iValue*0.5;
		case abtType.minjie : 
			return iValue*1.5;
		case abtType.zhili	 : 
			return iValue*0.5;
	}
}

function SpriteName(objs : Object[]){
	var id : String;
	var iv : InventoryItem;
	var useSprite : UISprite;
	id = objs[0];
	useSprite = objs[1];
	iv = GetItemInfo(id,iv);
	useSprite.spriteName = iv.atlasStr;
}

var yanseStrs : String[];
function LabelName(objs : Object[]){
		var id : String;
		var iv : InventoryItem;
		var useSprite : UILabel;
		id = objs[0];
		useSprite = objs[1];
		iv = GetItemInfo(id,iv);
//		print(id + " == id");
//		print(iv + " == iv");
		var str : String;
//		if(iv != null && iv.slotType < 12){
		if(iv != null){
			if(iv.itemQuality < 6){
				str =  yanseStrs[iv.itemQuality];				
			}else{
				str =  yanseStrs[iv.itemQuality-4];	
			}
			useSprite.text += str + "|" + iv.itemName + "|";
		}
}

var toukuis : Mesh[];
function LookTou(name : String) : boolean{ 
//	toukuis.name = "1";
	for(var i=0; i<toukuis.length; i++){
		if(toukuis[i] != null){ 
//			//print(toukuis[i].name + " ******* " + name);
			if(toukuis[i].name == name){
				return true;
			}
		}
	}
	return false;
}

function GetPeiFangNameAsID(id : String) : String{
	var name : String = "未知装备";
	for(var rows : yuan.YuanMemoryDB.YuanRow in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBlueprint.Rows){
		if(rows["BlueprintID"].YuanColumnText.Substring(0,9) == id.Substring(0,9)){
			name = rows["BlueprintName"].YuanColumnText;
		}
	}
	return name;
}

function GetPeiFangAsID(id : String) : yuan.YuanMemoryDB.YuanRow{
	for(var rows : yuan.YuanMemoryDB.YuanRow in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBlueprint.Rows){
		if(rows["id"].YuanColumnText == id){
			return rows;
		}
	}
	return null;
}

function GetPeiFangAsBlueprintID(BlueprintID : String) : yuan.YuanMemoryDB.YuanRow{
	for(var rows : yuan.YuanMemoryDB.YuanRow in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBlueprint.Rows){
		var str : String = "";
		str = rows["BlueprintID"].YuanColumnText;
//		var str1 : String = "";
//		str1 = rows["HeroStone"].YuanColumnText;
//		print(str1 + " str1str1str1str1str1str1str1str1");
		if(str.Substring(0,8) == BlueprintID.Substring(0,8)){
			return rows;
		}
	}
	return null;
}

var colors : String[];
function ResolveInventory(objs : Object[]){
	var strItem : String;
	var uiName : UILabel;
	var iv : InventoryItem;
	strItem = objs[0];
	uiName = objs[1];
	iv = GetItemInfo(strItem,iv);
	var colorstr : String;
	if(iv.slotType < 12){
		if(iv.slotType  == 10 || iv.slotType == 11 ){
			if(iv.itemQuality < 6){
				colorstr = colors[iv.itemQuality] ;				
			}else{
				colorstr =colors[iv.itemQuality - 4] ;	
			}
		}else{
			if(iv.itemQuality < 6){
				colorstr = colors[iv.itemQuality] ;				
			}else{
				colorstr =colors[iv.itemQuality - 4] ;	
			}
		}
	}
	uiName.text = colorstr + iv.itemName;
}
