#pragma strict
var SpriteBiankuang : UISprite;

var usey : float; 
static var useColor : String[]; 
var bool : boolean = false;
function Awake(){
//	invMaker = AllResources.InvmakerStatic;
//	if(invcl == null){
//		invcl = AllManage.InvclStatic;
//	}
}

function Start () { 
	if(useColor == null){
		useColor = colors;
	}
	if(InfoObj){ 
		if(bool){
			usey = (-450);
		}else{
			usey = InfoObj.transform.localPosition.y; 		
		}
		InfoObj.transform.localPosition.y = 3000;
	}
}

//var invMaker : Inventorymaker;
var inv : InventoryItem = null; 
var invSprite : UISprite;
var LabelName : UILabel;
var Labelleixing : UILabel;
var Labelzhiye : UILabel;
var Labelshuxing : UILabel;

var labelWhite : UILabel ; 
var labelGreen : UILabel ;
var labelPurple : UILabel ; 


var Labeldefen : UILabel;  
var LabelLevel : UILabel;  
var LabelJinBi : UILabel;
var LabelInfo : UILabel;
var LaberQiangHuaHou : UILabel;
var hanzi : String[];
var InfoObj : GameObject;
var BagItems : BagItem[];
var OneBagItem : BagItem;
var NeedBagItem : BagItem[];
var oo : int = 0;
var ObjBagJiaoYi : Transform; 
var colors : String[];
var newInv : InventoryItem = new InventoryItem();
var XiaoHaoButton1 : GameObject;
var XiaoHaoButton2 : GameObject;
var isMaiChu : boolean = false;
function showEquItemInfo(iv : InventoryItem , obt : BagItem){   

	if(buttonsBijiao)
		buttonsBijiao.localPosition.y = -240;
	if(buttons4)		
		buttons4.localPosition.y = 0;		
	inv = iv;
	LabelInfo.text = "";
	Labeldefen.text = "";
	if(inv.slotType < 12){
		SpriteBiankuang.enabled = true;	
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
	}else{ 
		if(inv.slotType  != SlotType.Soul && inv.slotType == SlotType.Digest ){
			SpriteBiankuang.enabled = true;	
			SpriteBiankuang.spriteName = "yanse" + 1;	
		}else{
			var intI : int = 0;
			if(inv.slotType != SlotType.Formula){
				intI = parseInt(inv.itemID.Substring(8,1));
			}else{
				intI = parseInt(inv.itemID.Substring(5,1));			
			}
			
			if(intI < 6){
				SpriteBiankuang.spriteName = "yanse" + intI;				
			}else{
				SpriteBiankuang.spriteName = "yanse" + (intI-4);	
			}
		}
	}
	if(inv.itemQuality == 0)
	SpriteBiankuang.enabled = false;
	
	if(EquepmentLabel1){	
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages061");
			AllManage.AllMge.SetLabelLanguageAsID(EquepmentLabel1);
//		EquepmentLabel1.text = "装备";
	}
//	//print("zhe li le");
		if(LabelMaiRu){
			isMaiChu = true;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages042");
			AllManage.AllMge.SetLabelLanguageAsID(LabelMaiRu);
//			LabelMaiRu.text = "卖出";		
		}
	ButtonMoveOut(true);
	if(oo == 1){
		bagParent.localPosition.y = 3000;	
	}
	if(LaberQiangHuaHou){
		LaberQiangHuaHou.text = "";
	}
	OneBagItem = obt;
	if(InfoObj){
		InfoObj.transform.localPosition.y = usey;
		InfoObj.SetActiveRecursively(true);	
	}
	if(DaoJuButton){
		DaoJuButton.SetActiveRecursively(false);
	}
	if(ObjBagJiaoYi){
		ObjBagJiaoYi.localPosition.y = 3000;
	}
	var colorstr : String;
		if(inv.slotType < 12){
			if(inv.slotType  == 10 || inv.slotType == 11 ){
				if(inv.itemQuality < 6){
					colorstr = colors[inv.itemQuality] ;				
				}else{
					colorstr =colors[inv.itemQuality - 4] ;	
				}
			}else{
				if(inv.itemQuality < 6){
					colorstr = colors[inv.itemQuality] ;				
				}else{
					colorstr =colors[inv.itemQuality - 4] ;	
				}
			}
		}else{ 
			if(inv.slotType  != SlotType.Soul && inv.slotType == SlotType.Digest ){
			}else{
//				//print(inv.itemID.Substring(8,1) + " == inv.itemID.Substring(8,1");
				if(parseInt(inv.itemID.Substring(8,1)) < 6){
					colorstr = colors[parseInt(inv.itemID.Substring(8,1))];				
				}else{
					colorstr = colors[parseInt(inv.itemID.Substring(8,1)) - 4];				
				}
			}
		}
		AllManage.AllMge.Keys.Clear();
		AllManage.AllMge.Keys.Add("messages062");
		AllManage.AllMge.Keys.Add(iv.itemLevel + "");
		AllManage.AllMge.SetLabelLanguageAsID(LabelLevel);
//	LabelLevel.text = "等级:" + iv.itemLevel ;

	if(inv.slotType != SlotType.Formula){
		AllManage.AllMge.Keys.Clear();
	    //AllManage.AllMge.Keys.Add("messages063");
		AllManage.AllMge.Keys.Add("meg0159"); //售出价:
		if(OneBagItem!= null && OneBagItem.isShangDian){
			AllManage.AllMge.Keys.Add(ToInt.StrToInt(iv.costGold) * 2 + "");
		}else{
			AllManage.AllMge.Keys.Add(ToInt.StrToInt(iv.costGold) + "");
		}
		AllManage.AllMge.SetLabelLanguageAsID(LabelJinBi);
//		LabelJinBi.text = "金币:" +  iv.costGold; 
		var useInv : InventoryItem;
		useInv = AllResources.InvmakerStatic.GetItemInfo(iv.itemID , iv);
		if( ToInt.StrToInt(iv.costBlood) > 0){
	//		print(useInv.costBlood + " == "  + iv.costBlood);
			if(useInv.costBlood != iv.costBlood || (OneBagItem!= null && OneBagItem.isShangDian)){
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add(LabelJinBi.text + "");
				AllManage.AllMge.Keys.Add("\n");
				AllManage.AllMge.Keys.Add("messages053");
				AllManage.AllMge.Keys.Add(":");
				AllManage.AllMge.Keys.Add(ToInt.StrToInt(iv.costBlood) + "");
				AllManage.AllMge.SetLabelLanguageAsID(LabelJinBi);
	//			LabelJinBi.text += "\n血石:" +  iv.costBlood; 		
			}else{
				AllManage.AllMge.Keys.Clear();
			    //AllManage.AllMge.Keys.Add("messages063");
				AllManage.AllMge.Keys.Add("meg0159"); //售出价:
				AllManage.AllMge.Keys.Add(Mathf.Clamp((ToInt.StrToInt(iv.costGold) + ToInt.StrToInt(iv.costBlood)*500) , 0 , 20000) + "");
				AllManage.AllMge.SetLabelLanguageAsID(LabelJinBi);
	//			LabelJinBi.text = "金币:" +  (iv.costGold + iv.costBlood*500); 
			}
		}
	}else{
		AllManage.AllMge.Keys.Clear();
		AllManage.AllMge.Keys.Add("info1060");
		AllManage.AllMge.Keys.Add(iv.needHeroBadge + "");
		AllManage.AllMge.SetLabelLanguageAsID(LabelJinBi);		
	}
	
	if(inv.ItemInfo != ""){
		LabelInfo.text = inv.ItemInfo;
	}else{
		LabelInfo.text = "";		
	}
	invSprite.spriteName = inv.atlasStr;
	LabelName.text = colorstr + inv.itemName;
	if(inv.itemHole > 0){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelName.text + "");
			AllManage.AllMge.Keys.Add("\n[a0ffff]");
			AllManage.AllMge.Keys.Add("messages116");
			AllManage.AllMge.SetLabelLanguageAsID(LabelName);
//		LabelName.text += "\n[a0ffff]已绑定";
	}
//	//print(inv.weaponTypeStr[inv.slotType]);
	try{
		Labelleixing.text = AllManage.AllMge.Loc.Get( inv.weaponTypeStr[inv.slotType] );
		if(inv.WeaponType == 0){
			Labelzhiye.text = AllManage.AllMge.Loc.Get(inv.professionTypeStr[inv.professionType]);		
		}else{
			Labelzhiye.text = AllManage.AllMge.Loc.Get(Inventorymaker.wuqiTypeStrStatic[ inv.WeaponPlus + parseInt(inv.WeaponType)]) ;					
		}
		if(inv.slotType == SlotType.Rear){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(Labelzhiye.text + "\n");
			AllManage.AllMge.Keys.Add("messages064");
			AllManage.AllMge.Keys.Add(inv.needPVPPoint + "");
			AllManage.AllMge.SetLabelLanguageAsID(Labelzhiye);
//			Labelzhiye.text = Labelzhiye.text + "\nPVP点数:" + inv.needPVPPoint;
		}
	}catch(e){
	}
	Labelshuxing.text = "";
	if(labelWhite){
	labelWhite.text = "";
	}
	if(labelGreen){
	labelGreen.text = "";
	}
	if(labelPurple){
	labelPurple.text = "";
	}
	if(inv.slotType < 12 || inv.slotType == SlotType.Formula){
		if(inv.slotType == SlotType.Weapon1 || inv.slotType == SlotType.Weapon2 || inv.buildItemSlotType == SlotType.Weapon1 || inv.buildItemSlotType == SlotType.Weapon2){
			if( (inv.slotType == SlotType.Weapon1 && inv.professionType == ProfessionType.Soldier) ||  (inv.buildItemSlotType == SlotType.Weapon1 && inv.professionType == ProfessionType.Soldier)){
			if(labelWhite){
				labelWhite.text += inv.ATatkStr  + inv.ATatk+ " " + inv.AtarmorStr + inv.ATarmor+ "\n";
			}
			}else{
			//攻击
			if(labelWhite){
				labelWhite.text +=  inv.ATatkStr + inv.ATatk +    "\n";
			}
			}
		}else{
		//护甲
		if(labelWhite){
			labelWhite.text += inv.AtarmorStr + inv.ATarmor  +  "\n";
		}
		}
		//耐力
		if(labelWhite){
		labelWhite.text +=  inv.ATnailiStr  + inv.ATnaili +    "\n";
		} 
	//	//print(parseInt(iv.professionType));
		switch(iv.professionType){
			case 1: if(labelWhite){labelWhite.text += inv.ATliliangStr+ inv.ATliliang + " \n";} break;
			case 2: if(labelWhite){labelWhite.text +=inv.ATminjieStr +inv.ATminjie +  " \n";} break;
			case 3: if(labelWhite){labelWhite.text +=inv.ATzhiliStr+inv.ATzhili + " \n";} break;
		}
	}
	var abt : abtType;
	if(inv.itemBuild){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("[03e6f7]");
			AllManage.AllMge.Keys.Add("messages065");
			AllManage.AllMge.Keys.Add(parseInt(inv.itemBuild) + "\n");
			AllManage.AllMge.Keys.Add("info871");
			AllManage.AllMge.Keys.Add(inv.ATzongfen + "");
			AllManage.AllMge.SetLabelLanguageAsID(Labeldefen);
//		Labeldefen.text = "[03e6f7]" + "强化等级:"+ parseInt(inv.itemBuild) +"\n综合得分" + inv.ATzongfen.ToString();
//		var useInt : int  = parseInt(inv.itemBuild) + 6;
//		inv.itemBuild = useInt.ToString();	
	}
//	//print(iv.ATabt1);
	if(LaberQiangHuaHou && parseInt(inv.itemID.Substring(0,1)) <= 6){
		newInv.itemID = getNewItemID(inv.itemID);
		newInv = AllResources.InvmakerStatic.GetItemInfo(newInv.itemID , newInv);
	}
	try{
		var intTen : int = 0;
		intTen = AllResources.InvmakerStatic.GetInvFenMuAsQuality(iv); 
		var qua : int = 0; 
		qua = iv.itemQuality;
		if(qua >= 6){
			qua -= 4;
		}
		if(iv.ATabt1){
			abt = iv.ATabt1.iType;
			if(qua >= 2){
			//攻击力Label
			if(labelGreen){
				labelGreen.text += "[00ff00]" + AllManage.AllMge.Loc.Get( iv.ATabt1.iStr[iv.ATabt1.iType] )+ "+" + inv.ATabt1.iValue + "\n";	
				} 
		//		//print(LaberQiangHuaHou);
				if(LaberQiangHuaHou && parseInt(inv.itemID.Substring(0,1)) <= 6){
					LaberQiangHuaHou.text +=  "[00ff00]+" +  AllResources.InvmakerStatic.getAtb(newInv.itemAbt1 , newInv.ItemPinZhiLevel , newInv.slotType , newInv.professionType , newInv , 1 , intTen).ATabt1.iValue +  AllManage.AllMge.Loc.Get( iv.ATabt1.iStr[iv.ATabt1.iType] )  +"\n";			
		//			//print(LaberQiangHuaHou.text);
				}
			}else{ 
			if(labelGreen){
				labelGreen.text += "\n"; 
				}
				if(LaberQiangHuaHou)
				LaberQiangHuaHou.text += "\n";
			}
	
		}
		if(iv.ATabt2){
			abt = iv.ATabt2.iType;
			if(qua >= 3){
				//专注  智力
				if(labelGreen){
				labelGreen.text += "[00ff00]" + AllManage.AllMge.Loc.Get( iv.ATabt2.iStr[iv.ATabt2.iType] ) + "+" + inv.ATabt2.iValue + "\n";
				}
				if(LaberQiangHuaHou && parseInt(inv.itemID.Substring(0,1)) <= 6){
					LaberQiangHuaHou.text +=  "[00ff00]+" +  AllResources.InvmakerStatic.getAtb(newInv.itemAbt2 ,  newInv.ItemPinZhiLevel  , newInv.slotType , newInv.professionType , newInv , 2 , intTen).ATabt2.iValue +  AllManage.AllMge.Loc.Get( iv.ATabt2.iStr[iv.ATabt2.iType])  +"\n";	
				}
			}else{
			if(labelGreen){
				labelGreen.text += "\n"; 
				}
				if(LaberQiangHuaHou)
				LaberQiangHuaHou.text += "\n";				
			}
	
		}
		if(iv.ATabt3){
			abt = iv.ATabt3.iType;
			if(qua >= 4){
			//智力  敏捷
			if(labelGreen){
				labelGreen.text += "[00ff00]" + AllManage.AllMge.Loc.Get(iv.ATabt3.iStr[iv.ATabt3.iType]) + "+"+ inv.ATabt3.iValue + "\n"; 
				}
				if(LaberQiangHuaHou && parseInt(inv.itemID.Substring(0,1)) <= 6){
					LaberQiangHuaHou.text +=  "[00ff00]+" +  AllResources.InvmakerStatic.getAtb(newInv.itemAbt3 ,  newInv.ItemPinZhiLevel  , newInv.slotType , newInv.professionType , newInv , 3 , intTen).ATabt3.iValue + AllManage.AllMge.Loc.Get(iv.ATabt3.iStr[iv.ATabt3.iType])  +"\n";	
				}
			}else{
			if(labelGreen){
				labelGreen.text += "\n"; 
				}
				if(LaberQiangHuaHou)
				LaberQiangHuaHou.text += "\n";								
			}
		}
	}catch(e){
	}
	
	if(inv.slotType == SlotType.Formula){
		var InvFormula : InventoryItem;
		InvFormula = new InventoryItem();
		InvFormula = AllResources.InvmakerStatic.GetItemInfo(inv.itemID.Substring(1,25) , InvFormula);

		if(InvFormula.slotType < 12){
			if(InvFormula.slotType  == 10 || InvFormula.slotType == 11 ){
				if(inv.itemQuality < 6){
					colorstr = colors[inv.itemQuality] ;				
				}else{
					colorstr =colors[inv.itemQuality - 4] ;	
				}
			}else{
//				//print(inv.itemID.Substring(8,1) + " == inv.itemID.Substring(8,1");
				if(InvFormula.itemQuality < 6){
					colorstr = colors[InvFormula.itemQuality] ;				
				}else{
					colorstr =colors[InvFormula.itemQuality - 4] ;	
				}
			}
		}
				
		LabelName.text = colorstr + inv.itemName;
		if(EquepmentButton2){		
			EquepmentButton2.SetActiveRecursively(false);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages067");
			AllManage.AllMge.SetLabelLanguageAsID(EquepmentLabel1);
//			EquepmentLabel1.text = "学习"; 
//			//print(EquepmentLabel1.text + " == EquepmentLabel1.text");
		}
		var CLinv : InventoryItem;
		if(NeedBagItem.length > 0){
			CLinv = AllResources.InvmakerStatic.GetItemInfo(inv.formulaItemNeed1.consumablesID + ","+inv.formulaItemNeed1.consumablesNum,CLinv);
			NeedBagItem[0].inv = null;
			NeedBagItem[0].SetInv(CLinv);
			CLinv = AllResources.InvmakerStatic.GetItemInfo(inv.formulaItemNeed2.consumablesID + ","+inv.formulaItemNeed2.consumablesNum,CLinv);
			NeedBagItem[1].inv = null;
			NeedBagItem[1].SetInv(CLinv);
			CLinv = AllResources.InvmakerStatic.GetItemInfo(inv.formulaItemNeed3.consumablesID + ","+inv.formulaItemNeed3.consumablesNum,CLinv);
			NeedBagItem[2].inv = null;
			NeedBagItem[2].SetInv(CLinv);
			CLinv = AllResources.InvmakerStatic.GetItemInfo(inv.formulaItemNeed4.consumablesID + ","+inv.formulaItemNeed4.consumablesNum,CLinv);
			NeedBagItem[3].inv = null;
			NeedBagItem[3].SetInv(CLinv);
		}
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add( AllManage.AllMge.Loc.Get( inv.weaponTypeStr[inv.buildItemSlotType]  ) + "\n");
			AllManage.AllMge.Keys.Add("messages068");
			AllManage.AllMge.Keys.Add(inv.needPVEPoint + "");
			AllManage.AllMge.SetLabelLanguageAsID(Labelleixing);
//		Labelleixing.text = inv.weaponTypeStr[inv.buildItemSlotType] + "\nPVE点数:"+inv.needPVEPoint;
//		//print(iv.itemID);
	}else
	if(inv.slotType == SlotType.Packs){
//		LabelName.text = "[0000ff]" + inv.itemName;
		if(EquepmentButton2){		
			EquepmentButton2.SetActiveRecursively(false);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages069");
			AllManage.AllMge.SetLabelLanguageAsID(EquepmentLabel1);
//			EquepmentLabel1.text = "打开";
		}
		Labelleixing.text = AllManage.AllMge.Loc.Get(inv.weaponTypeStr[inv.buildItemSlotType] ) ;
	}
	 
	if(inv.slotType != SlotType.Formula){
		if(XiaoHaoButton1){
			if(parseInt(iv.itemID.Substring(0,1)) < 7){
				XiaoHaoButton1.SetActiveRecursively(true);
				XiaoHaoButton2.SetActiveRecursively(true);
			}else{
				XiaoHaoButton1.SetActiveRecursively(false);		
				XiaoHaoButton2.SetActiveRecursively(false);		
			}
		}
	}
	if(inv.itemID.Substring(0,2) == "83"){
		if(XiaoHaoButton1){
			XiaoHaoButton1.SetActiveRecursively(true);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages070");
			AllManage.AllMge.SetLabelLanguageAsID(EquepmentLabel1);
//			EquepmentLabel1.text = "吃"; 	
		}
	}else
	if(inv.itemID.Substring(0,3) == "891" || inv.itemID.Substring(0,3) == "890" || inv.itemID.Substring(0,3) == "893" || (inv.itemID.Substring(0,2) == "89" && parseInt(inv.itemID.Substring(2,1)) >=4 )){
		if(XiaoHaoButton1){
			XiaoHaoButton1.SetActiveRecursively(true);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages071");
			AllManage.AllMge.SetLabelLanguageAsID(EquepmentLabel1);
//			EquepmentLabel1.text = "开启"; 			
		}
	}
	
	if( parseInt(inv.itemID.Substring(0,1)) <= 6  && inv.itemQuality != 0){
		var useInt : int;
		var useInt2 : int;
		if(inv.itemHole1 != "00"){
			spriteHolesAttr[0].spriteName = "UIP_Gem_" + inv.itemHole1;
			useInt = parseInt(inv.itemHole1.Substring(0,1));
			useInt2 = parseInt(inv.itemHole1.Substring(1,1));
			if(labelPurple){
			labelPurple.text += "\n"+"\n"+"\n"+"       [ff00ff]" + useInt2 + AllManage.AllMge.Loc.Get(AllResources.StoneTypeInfo[useInt - 1])  + " + " + getHole( inv.holeAttr1 , inv.itemHole1) + AllManage.AllMge.Loc.Get(AllResources.StoneValueInfo[useInt - 1]) +"\n"; 
		}
		}else{
			spriteHolesAttr[0].spriteName = "UIM_Gem_Bar ";
			if(labelPurple){
			labelPurple.text += "\n";	
		}
		}
		if(inv.itemHole2 != "00"){
			spriteHolesAttr[1].spriteName = "UIP_Gem_" + inv.itemHole2;
			useInt = parseInt(inv.itemHole2.Substring(0,1));
			useInt2 = parseInt(inv.itemHole2.Substring(1,1));
			if(labelPurple){
			labelPurple.text += "\n"+"       [ff00ff]" +useInt2 + AllManage.AllMge.Loc.Get(AllResources.StoneTypeInfo[useInt - 1]) + " + " + getHole( inv.holeAttr2 , inv.itemHole2 ) + AllManage.AllMge.Loc.Get(AllResources.StoneValueInfo[useInt - 1]) +"\n"; 
		}
		}else{
			spriteHolesAttr[1].spriteName = "UIM_Gem_Bar ";
			if(labelPurple){
			labelPurple.text += "\n";	
		}
		}
		if(inv.itemHole3 != "00"){
			spriteHolesAttr[2].spriteName = "UIP_Gem_" + inv.itemHole3;
			useInt = parseInt(inv.itemHole3.Substring(0,1));
			useInt2 = parseInt(inv.itemHole3.Substring(1,1));
			if(labelPurple){
			labelPurple.text += "\n"+"       [ff00ff]" +useInt2 + AllManage.AllMge.Loc.Get(AllResources.StoneTypeInfo[useInt - 1]) + " + " +getHole( inv.holeAttr3 , inv.itemHole3 ) + AllManage.AllMge.Loc.Get(AllResources.StoneValueInfo[useInt - 1]) +"\n"; 
		}
		}else{
			spriteHolesAttr[2].spriteName = "UIM_Gem_Bar ";
			if(labelPurple){
			labelPurple.text += "\n";	
		}
		}
	}else{
		spriteHolesAttr[0].gameObject.SetActiveRecursively(false);
		spriteHolesAttr[1].gameObject.SetActiveRecursively(false);
		spriteHolesAttr[2].gameObject.SetActiveRecursively(false);
	} 
	if(iv.itemQuality == 0){
		if(parseInt(inv.itemID.Substring(0,1)) < 7){
			LabelInfo.text = "";		
		}
		if(labelPurple){
		labelPurple.text = "";
	}
		Labeldefen.text = "";
		if(XiaoHaoButton2){
			XiaoHaoButton2.SetActiveRecursively(false);			
		}
	}
	if(inv.slotType == SlotType.Formula){
		if(buttonsBijiao){
			buttonsBijiao.localPosition.y = -240;		
		}	
	}
//	//print(obt.myType);
	if(obt != null && obt.myType == SlotType.Cangku){
		buttonsBijiao.localPosition.y = 3000;
		buttons4.localPosition.y = 3000;		
	}
	ShowGem();
}


function getHole(hAttr : HoleAttr , hole : String) : int{ 
	if(hole == "00"){
		return 0;
	}
	var holeValue : int = 0;
	for(var rows : yuan.YuanMemoryDB.YuanRow in InventoryControl.GameItem.Rows){
		var str : String;
		str = "81" + parseInt(hAttr.hType).ToString() + hAttr.hValue.ToString();
//		//print(str + " == str");
		if(rows["ItemID"].YuanColumnText == str){
			holeValue = parseInt(rows["ItemValue"].YuanColumnText);
			return holeValue;
		}
	}
	return 0;
}

function getNewItemID(oldID : String) : String{
	var m : int = 6;
	var useInt : int;
	var useStr : String;
//	//print(invID);
	var invitemQuality : int;
	invitemQuality =  parseInt(oldID.Substring(4,1));
	if(invitemQuality >= 6){
		invitemQuality -= 4;
	}
	switch(invitemQuality){
		case 1: m = 0; break;
		case 2: m = 4; break;
		case 3: m = 8; break;
		case 4: m = 12; break;
		case 5: m = 12; break;
	}
	useInt = parseInt(oldID.Substring(15,3)) + m;
	useStr = useInt.ToString();
	if(useStr.length == 1){
		useStr = "00" + useStr;
	}else
	if(useStr.length == 2){
		useStr = "0" + useStr;		
	}
//	//print(oldID + " == " + m);
	oldID = oldID.Substring(0,15) + useStr + oldID.Substring(18,7); 
	return oldID;
}

var ProduceButton : UISprite;
function SetProduceItemButton(bool : boolean){
	if(bool){
		ProduceButton.spriteName = "UIH_Main_Button_N";
	}else{
		ProduceButton.spriteName = "UIH_Main_Button_O";
	}
}

var UILabelNeedNum : UILabel[];
function SetProduceItemNum(NeedNum : String , MyNum : int, num : int){
//	//print(num);
	UILabelNeedNum[num].text = MyNum.ToString() + "/" + NeedNum.ToString();
}

var EquepmentButton1 : GameObject;
var EquepmentButton2 : GameObject;
var EquepmentButton3 : GameObject;
var EquepmentButton4 : GameObject;
var EquepmentLabel1 : UILabel;
var EquepmentLabel2 : UILabel;
var EquepmentLabel3 : UILabel;
var EquepmentLabel4 : UILabel;


var spriteHoles : UISprite[];
var spriteHolesAttr : UISprite[];
var StoneTypeInfo : String[];
var StoneValueInfo : String[];
var uiColor :  Color;

function showHoleEquItemInfo(iv : InventoryItem , obt : BagItem){
	var i : int = 0;
	OneBagItem = obt;
	InfoObj.transform.localPosition.y = usey;
	InfoObj.SetActiveRecursively(true);
	if(DaoJuButton){
		DaoJuButton.SetActiveRecursively(false);
	}
	ButtonMoveOut(true);
	if(oo == 1){
		bagParent.localPosition.y = 3000;	
	}
	if(LaberQiangHuaHou){
		LaberQiangHuaHou.text = "";
	}
	OneBagItem = obt;
	if(InfoObj){
		InfoObj.transform.localPosition.y = usey;
		InfoObj.SetActiveRecursively(true);	
	}
	if(DaoJuButton){
		DaoJuButton.SetActiveRecursively(false);
	}
	if(ObjBagJiaoYi){
		ObjBagJiaoYi.localPosition.y = 3000;
	}
	inv = iv;
	var colorstr : String;
		if(inv.slotType < 12){
			if(inv.slotType  == 10 || inv.slotType == 11 ){
				if(inv.itemQuality < 6){
					colorstr = colors[inv.itemQuality] ;				
				}else{
					colorstr =colors[inv.itemQuality - 4] ;	
				}
			}else{
				if(inv.itemQuality < 6){
					colorstr = colors[inv.itemQuality] ;				
				}else{
					colorstr =colors[inv.itemQuality - 4] ;	
				}
			}
		}
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages062");
			AllManage.AllMge.Keys.Add(iv.itemLevel + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelLevel);
//	LabelLevel.text = "等级:" + iv.itemLevel ;
			AllManage.AllMge.Keys.Clear();
			//AllManage.AllMge.Keys.Add("messages063");
			AllManage.AllMge.Keys.Add("meg0159"); //售出价:
			if(OneBagItem!= null && OneBagItem.isShangDian){
				AllManage.AllMge.Keys.Add(ToInt.StrToInt(iv.costGold) * 2 + "");
			}else{
				AllManage.AllMge.Keys.Add(ToInt.StrToInt(iv.costGold) + "");
			}
			AllManage.AllMge.SetLabelLanguageAsID(LabelJinBi);
//	LabelJinBi.text = "金币:" +  iv.costGold; 
	var useInv : InventoryItem;
	useInv = AllResources.InvmakerStatic.GetItemInfo(iv.itemID , iv);
	if( ToInt.StrToInt(iv.costBlood) > 0 || OneBagItem.isShangDian){
		if(useInv.costBlood != iv.costBlood){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelName.text + "");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("messages053");
			AllManage.AllMge.Keys.Add(":");
			AllManage.AllMge.Keys.Add(ToInt.StrToInt(iv.costBlood) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelJinBi);
//			LabelJinBi.text += "\n血石:" +  iv.costBlood; 		
		}else{
			AllManage.AllMge.Keys.Clear();
		    //AllManage.AllMge.Keys.Add("messages063");
			AllManage.AllMge.Keys.Add("meg0159"); //售出价:
			AllManage.AllMge.Keys.Add(Mathf.Clamp((ToInt.StrToInt(iv.costGold) + ToInt.StrToInt(iv.costBlood)*500) , 0 , 20000) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelJinBi);
//			LabelJinBi.text = "金币:" +  (iv.costGold + iv.costBlood*500); 
		}
	}
	if(inv.ItemInfo != ""){
		LabelInfo.text = inv.ItemInfo;
	}else{
		LabelInfo.text = "";		
	}
	invSprite.spriteName = inv.atlasStr;
	LabelName.text = colorstr + inv.itemName;
	if(inv.itemHole > 0){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelName.text + "");
			AllManage.AllMge.Keys.Add("\n[a0ffff]");
			AllManage.AllMge.Keys.Add("messages116");
			AllManage.AllMge.SetLabelLanguageAsID(LabelName);
//		LabelName.text += "\n[a0ffff] + 已绑定";
	}
//	//print(inv.weaponTypeStr[inv.slotType]);
	Labelleixing.text = AllManage.AllMge.Loc.Get(inv.weaponTypeStr[inv.slotType])  ;
		if(inv.WeaponType == 0){
			Labelzhiye.text = AllManage.AllMge.Loc.Get(inv.professionTypeStr[inv.professionType]);	
		}else{
			Labelzhiye.text = AllManage.AllMge.Loc.Get(Inventorymaker.wuqiTypeStrStatic[ inv.WeaponPlus + parseInt(inv.WeaponType)]) ;			
		}
		if(inv.slotType == SlotType.Rear){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(Labelzhiye.text + "\n");
			AllManage.AllMge.Keys.Add("messages064");
			AllManage.AllMge.Keys.Add(inv.needPVPPoint + "");
			AllManage.AllMge.SetLabelLanguageAsID(Labelzhiye);
//			Labelzhiye.text = Labelzhiye.text + "\nPVP点数:" + inv.needPVPPoint;
		}
	Labelshuxing.text = "";
	if(labelWhite){
	labelWhite.text = "";
	}
	if(labelGreen){
	labelGreen.text = "";
	}
	if(labelPurple){
	labelPurple.text = "";
	}
	if(inv.slotType == SlotType.Weapon1 || inv.slotType == SlotType.Weapon2 || inv.buildItemSlotType == SlotType.Weapon1 || inv.buildItemSlotType == SlotType.Weapon2){
		if( (inv.slotType == SlotType.Weapon1 && inv.professionType == ProfessionType.Soldier) ||  (inv.buildItemSlotType == SlotType.Weapon1 && inv.professionType == ProfessionType.Soldier)){
			labelWhite.text += inv.ATatkStr+inv.ATatk +  " " + inv.AtarmorStr +inv.ATarmor + "\n";
		}else{
			labelWhite.text +=  inv.ATatkStr + inv.ATatk +   "\n";
		}
	}else{
		labelWhite.text += inv.AtarmorStr + inv.ATarmor + "\n";
	}
	labelWhite.text +=  inv.ATnailiStr  + "+" + inv.ATnaili +   "\n"; 
//	//print(parseInt(iv.professionType));
	switch(iv.professionType){
		case 1: labelWhite.text +=inv.ATliliangStr+inv.ATliliang + " \n"; break;
		case 2: labelWhite.text +=inv.ATminjieStr + inv.ATminjie +  " \n"; break;
		case 3: labelWhite.text +=inv.ATzhiliStr+inv.ATzhili + " \n"; break;
	}
	var abt : abtType;
	if(inv.itemBuild){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("[03e6f7]");
			AllManage.AllMge.Keys.Add("messages065");
			AllManage.AllMge.Keys.Add(parseInt(inv.itemBuild) + "\n");
			AllManage.AllMge.Keys.Add("info871");
			AllManage.AllMge.Keys.Add(inv.ATzongfen + "");
			AllManage.AllMge.SetLabelLanguageAsID(Labeldefen);
//		Labeldefen.text = "[03e6f7]" + "强化等级:"+ parseInt(inv.itemBuild) +"\n综合得分" + inv.ATzongfen.ToString();
//		var useInt : int  = parseInt(inv.itemBuild) + 6;
//		inv.itemBuild = useInt.ToString();	
	}
//	//print(iv.ATabt1);
	if(LaberQiangHuaHou && parseInt(inv.itemID.Substring(0,1)) <= 6){
		newInv.itemID = getNewItemID(inv.itemID);
		newInv = AllResources.InvmakerStatic.GetItemInfo(newInv.itemID , newInv);	
	}
	var intTen : int = 0;
	intTen = AllResources.InvmakerStatic.GetInvFenMuAsQuality(iv);
	var qua : int = 0; 
	qua = iv.itemQuality;
	if(qua >= 6){
		qua -= 4;
	}	
	if(iv.ATabt1){
		abt = iv.ATabt1.iType;
		if(qua >= 2){
			labelGreen.text += "[00ff00]" +AllManage.AllMge.Loc.Get(iv.ATabt1.iStr[iv.ATabt1.iType])+ "+"  + inv.ATabt1.iValue + "\n";	 
			if(LaberQiangHuaHou && parseInt(inv.itemID.Substring(0,1)) <= 6){
					LaberQiangHuaHou.text +=  "[00ff00]+" +  AllResources.InvmakerStatic.getAtb(newInv.itemAbt1 , newInv.ItemPinZhiLevel , newInv.slotType , newInv.professionType , newInv , 1 , intTen).ATabt1.iValue +  AllManage.AllMge.Loc.Get(iv.ATabt1.iStr[iv.ATabt1.iType] ) +"\n";	
			}
		}else{
			labelGreen.text += "\n"; 
			if(LaberQiangHuaHou)
			LaberQiangHuaHou.text += "\n";
		}			
	}
	if(iv.ATabt2){
		abt = iv.ATabt2.iType;
		if(qua >= 3){
			labelGreen.text += "[00ff00]" + AllManage.AllMge.Loc.Get(iv.ATabt2.iStr[iv.ATabt2.iType] )+ "+" +inv.ATabt2.iValue + "\n";
			if(LaberQiangHuaHou && parseInt(inv.itemID.Substring(0,1)) <= 6){
					LaberQiangHuaHou.text +=  "[00ff00]+" +  AllResources.InvmakerStatic.getAtb(newInv.itemAbt2 ,  newInv.ItemPinZhiLevel  , newInv.slotType , newInv.professionType , newInv , 2 , intTen).ATabt2.iValue +  AllManage.AllMge.Loc.Get(iv.ATabt2.iStr[iv.ATabt2.iType] )  +"\n";	
			}
		}else{ 
			labelGreen.text += "\n";
			if(LaberQiangHuaHou)
			LaberQiangHuaHou.text += "\n";
		}			
	}
	if(iv.ATabt3){
		abt = iv.ATabt3.iType;
		if(qua >= 4){
			labelGreen.text += "[00ff00]" + AllManage.AllMge.Loc.Get(iv.ATabt3.iStr[iv.ATabt3.iType] )+"+" +inv.ATabt3.iValue + "\n"; 
			if(LaberQiangHuaHou && parseInt(inv.itemID.Substring(0,1)) <= 6){
					LaberQiangHuaHou.text +=  "[00ff00]+" +  AllResources.InvmakerStatic.getAtb(newInv.itemAbt3 ,  newInv.ItemPinZhiLevel  , newInv.slotType , newInv.professionType , newInv , 3 , intTen).ATabt3.iValue + AllManage.AllMge.Loc.Get(iv.ATabt3.iStr[iv.ATabt3.iType] )  +"\n";	
			}
		}else{
			labelGreen.text += "\n";
			if(LaberQiangHuaHou)
			LaberQiangHuaHou.text += "\n";
		}		
	}

	for(i=1; i<4; i++){
		if(i+1 <= inv.itemHole){
			spriteHoles[i-1].spriteName = "UIM_Gem_Bar ";
		}else{
			spriteHoles[i-1].spriteName = "UIB_Punch";		
		}
	}
	var useInt : int;
	var useInt2 : int;
	if(inv.itemHole1 != "00"){
		spriteHolesAttr[0].spriteName = "UIP_Gem_" + inv.itemHole1;
		spriteHoles[0].spriteName = "UIP_Gem_" + inv.itemHole1;
		useInt = parseInt(inv.itemHole1.Substring(0,1));
		useInt2 = parseInt(inv.itemHole1.Substring(1,1));
		labelPurple.text += "    [ff00ff]" + useInt2 + AllManage.AllMge.Loc.Get(AllResources.StoneTypeInfo[useInt - 1]) + " + " + getHole( inv.holeAttr1 , inv.itemHole1 ) + AllManage.AllMge.Loc.Get(AllResources.StoneValueInfo[useInt - 1])+"\n"; 
	}else{
		spriteHolesAttr[0].spriteName = "UIM_Gem_Bar ";
		labelPurple.text += "\n";	
	}
	if(inv.itemHole2 != "00"){
		spriteHolesAttr[1].spriteName = "UIP_Gem_" + inv.itemHole2;
		spriteHoles[1].spriteName = "UIP_Gem_" + inv.itemHole2;
		useInt = parseInt(inv.itemHole2.Substring(0,1));
		useInt2 = parseInt(inv.itemHole2.Substring(1,1));
		labelPurple.text +="    [ff00ff]" +useInt2 + AllManage.AllMge.Loc.Get(AllResources.StoneTypeInfo[useInt - 1]) + " + " + getHole( inv.holeAttr2 , inv.itemHole2 ) + AllManage.AllMge.Loc.Get(AllResources.StoneValueInfo[useInt - 1]) +"\n"; 
	}else{
		spriteHolesAttr[1].spriteName = "UIM_Gem_Bar ";
		labelPurple.text += "\n";	
	}
	if(inv.itemHole3 != "00"){
		spriteHolesAttr[2].spriteName = "UIP_Gem_" + inv.itemHole3;
		spriteHoles[2].spriteName = "UIP_Gem_" + inv.itemHole3;
		useInt = parseInt(inv.itemHole3.Substring(0,1));
		useInt2 = parseInt(inv.itemHole3.Substring(1,1));
		labelPurple.text += "    [ff00ff]" +useInt2 + AllManage.AllMge.Loc.Get(AllResources.StoneTypeInfo[useInt - 1]) + " + " + getHole( inv.holeAttr3 , inv.itemHole3 ) + AllManage.AllMge.Loc.Get(AllResources.StoneValueInfo[useInt - 1]) +"\n"; 
	}else{
		spriteHolesAttr[2].spriteName = "UIM_Gem_Bar ";
		labelPurple.text += "\n";	
	}
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("[03e6f7]");
			AllManage.AllMge.Keys.Add("messages065");
			AllManage.AllMge.Keys.Add(parseInt(inv.itemBuild) + "\n");
			AllManage.AllMge.Keys.Add("info871");
			AllManage.AllMge.Keys.Add(inv.ATzongfen + "");
			AllManage.AllMge.SetLabelLanguageAsID(Labeldefen);
//	Labeldefen.text = "[03e6f7]" + "强化等级:"+ parseInt(inv.itemBuild) +"\n综合得分" + inv.ATzongfen.ToString();
}

var EquItems : BagItem[]; 
var inventoryProduce : InventoryProduceControl;
function ZhuangBeiItem(){
	var isSet : boolean = false; 
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	if(inv.slotType == SlotType.Formula){ 
//		AllManage.UIALLPCStatic.show0();
		AllManage.UIALLPCStatic.showHeCheng3();
		yield;
		yield;
		yield;
		inventoryProduce = AllManage.inventoryProduceStatic;
		if(parseInt(ps.Prestige) >= inv.needPVEPoint){
			isSet = inventoryProduce.XueXi(inv.itemID); 
			//print(isSet + " == isSet");
			if(isSet){
				AllManage.InvclStatic.ClearOneBagItem(inv.itemID);
				InfoClose();
			}else{
				AllManage.tsStatic.Show("tips012");
			}
		}else{
			AllManage.tsStatic.Show("tips013");
		}
	}else
	if(inv.slotType == SlotType.Packs){
		if(!AllManage.InvclStatic.isBagFull()){
			if((inv.itemID.Substring(0,3) == "890" || (inv.itemID.Substring(0,2) == "89" && parseInt(inv.itemID.Substring(2,1)) >=4 )) && parseInt(ps.Level) >= inv.LevelNum){  
				AllManage.InvclStatic.LookLiBaoItemAsID(inv.itemID);
	//			AllManage.InvclStatic.useBagItem(inv.itemID , 1);
				InfoClose();
	//			AllManage.UIALLPCStatic.show0();
			}else{
				if(parseInt(ps.Level) < inv.LevelNum){
					AllManage.tsStatic.Show("tips017");
				}
			}
			if(inv.itemID.Substring(0,3) == "891"){
				if(AllManage.InvclStatic.useBagItem(inv.itemID.Substring(0,2) + "2" + inv.itemID.Substring(3,1)+ ",01," + inv.itemID.Substring(8,1) , 1)){
	//				//print(inv.itemID + " == inv.itemID");
					AllManage.InvclStatic.LookLiBaoItemAsID(inv.itemID);
					InfoClose();
	//				AllManage.UIALLPCStatic.show0();
				}else{
					AllManage.tsStatic.Show("info996");
				}
			}//		AllManage.InvclStatic.LookLiBaoItemAsID(inv.itemID);
			if(inv.itemID.Substring(0,3) == "893"){
	//				//print(inv.itemID + " == inv.itemID");
					AllManage.InvclStatic.LookLiBaoItemAsID(inv.itemID);
					InfoClose();
			}//		AllManage.InvclStatic.LookLiBaoItemAsID(inv.itemID);
		}else{
			AllManage.qrStatic.ShowQueRen(gameObject,"","","tips030");
		}
	}else{
		for(var i=0; i<EquItems.length; i++){
			isSet = EquItems[i].OtherZhuangBei(inv);
			if(isSet){
				OneBagItem.OtherYiChu();
				EquItems[i].canJiaoHuan = true;
				EquItems[i].RealSetZHuangbei(inv);
				InfoClose();
				return;
			}
		} 
		if(inv.itemID.Substring(0,2) == "83"){ 
			if(!ps.battlemod){	
				if(ps != null){
					ps.PlayerAction(inv.itemID);
					AllManage.UIALLPCStatic.show0();
				}
//				if(inv.itemLevel <= 10){ 
//					PlayerStatus.MainCharacter.gameObject.SendMessage("UseXuePing",inv.itemLevel,SendMessageOptions.DontRequireReceiver); 
//				}else{
//					PlayerStatus.MainCharacter.gameObject.SendMessage("AddPower",(inv.itemLevel - 10) * (-2),SendMessageOptions.DontRequireReceiver); 				
//				}
				AllManage.InvclStatic.useBagItem(inv.itemID , 1);
				InfoClose();
			}else{
				 AllManage.tsStatic.Show("tips015");
			}
		}
	}
}

var ptime : float;
var charbar : GameObject;
var ColorZi : Color;
var ColorCheng : Color;
function FaLiaoTian(){
	if(Time.time < ptime){
		return;
	}
	ptime = Time.time + 2;
	var obj : Object[];
	var col : Color;
	obj = new Array(2);
	obj[0] = "["+inv.itemID+"]";
	if(inv.slotType < 12){
			var useInt : int = 0;
		if(inv.slotType  == 10 || inv.slotType == 11 ){
//			var useInt : int = 0;
			
			if(inv.itemQuality < 6){
				useInt = inv.itemQuality;
			}else{
				useInt = inv.itemQuality-4;
			}
			switch(useInt){
				case 1 : col = Color.white; break;
				case 2 : col = Color.green; break;
				case 3 : col = Color.blue; break;
				case 4 : col = ColorZi; break;
				case 5 : col = ColorCheng; break;
			}
//			switch(inv.itemQuality){
//				case 1 : col = Color.white; break;
//				case 2 : col = Color.green; break;
//				case 3 : col = Color.blue; break;
//				case 4 : col = ColorZi; break;
//				case 5 : col = ColorCheng; break;
//			}
		}else{
//			var useInt : int = 0;
			
			if(inv.itemQuality < 6){
				useInt = inv.itemQuality;
			}else{
				useInt = inv.itemQuality-4;
			}
			switch(useInt){
				case 1 : col = Color.white; break;
				case 2 : col = Color.green; break;
				case 3 : col = Color.blue; break;
				case 4 : col = ColorZi; break;
				case 5 : col = ColorCheng; break;
			}
		}
	}

	obj[1] = col;
	////print(obj[0]);
	//charbar.SendMessage("AddText",obj,SendMessageOptions.DontRequireReceiver);
	var tempStr:String[]=new String[1];
	var invStr : String;
	if(inv.itemID.Substring(0,1) =="9" || parseInt(inv.itemID.Substring(0,1)) < 7){
		invStr = inv.itemID.Split(DStr.ToCharArray())[0];	
	}else{
		invStr = inv.itemID;
		if(invStr.Length > 9){
			invStr = invStr.Substring(0 , 9);
		}
	}
	tempStr[0]="33";
	InRoom.GetInRoomInstantiate ().SendMessage (tempStr,yuan.YuanPhoton.MessageType.All,"["+invStr+"]",BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText);
	AllManage.UIALLPCStatic.show0();
}

private var DStr : String = ",";
var EquepParent : Transform;
var OtherinvInfoObj : Transform;
var invInfoObj : EquepmentItemInfo;
//var TS : TiShi;
function BiJiaoItem(){
	AllManage.InvclStatic.BagGuiWeiNoInfo();
//	AllManage.InvclStatic.QieHuanEquepShuXing();
	OtherinvInfoObj.gameObject.SetActiveRecursively(true);	
	OtherinvInfoObj.transform.localPosition.y = 0;	
	OtherinvInfoObj.transform.localPosition.x = -350;	
	EquepParent.localPosition.y = 3000;
	AllManage.InvclStatic.BiJiaoNewPes(inv);
//	for(var i=0; i<EquItems.length; i++){
//		if(EquItems[i].myType == inv.slotType){
//			if(EquItems[i].inv != null){
//				EquepParent.localPosition.y = 3000;
//				invInfoObj.showEquItemInfo(EquItems[i].inv,null);
//				invInfoObj.gameObject.SetActiveRecursively(true);	
//				invInfoObj.transform.localPosition.y = 0;	
//				return;
//			}else{ 
//				 InfoClose();
//				TS.Show("需要有装备物品，才能比较。");
////				pri nt("zhuang bei wei kong");
//			}
//		}
//	}
}

private var ps : PlayerStatus;
//var LT : LootTable;
var MaiBag : BagItem;
var cantMaichu : boolean = false;
private var useBuyStr : String = "";
function MaichuItem(){
	if(isMaiChu){
		if(cantMaichu){
			cantMaichu = false;
			AllManage.tsStatic.Show("tips016");
			InfoClose();
			return;
		}
		AllManage.inv = inv;
		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1&&inv.itemQuality>3)
			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiChu" , "NoMai" , AllManage.AllMge.Loc.Get("info295") +inv.itemName+AllManage.AllMge.Loc.Get("info285") + Mathf.Clamp((ToInt.StrToInt(inv.costGold) + ToInt.StrToInt(inv.costBlood) * 500) , 0 , 20000) + AllManage.AllMge.Loc.Get("info296"),1);
//			AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.Sell , 0 , 0 , inv.itemID , gameObject , "YesMaiChuTips");
		else
			YesMaiChu();
	}else{
		AllManage.inv = inv;
		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1 && !(AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 1)){
			if(AllManage.inv.slotType == SlotType.Formula){
//				AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsBuyStoreClient , 0 , 0 , inv.itemID , gameObject , "YesMaiChuTipsneedHeroBadge");
				AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiRu" , "NoMai" , AllManage.AllMge.Loc.Get("info298") + ""+(inv.needHeroBadge)+AllManage.AllMge.Loc.Get("info1064") + "" + AllManage.AllMge.Loc.Get("messages072") + inv.itemName+"？");				
			}else
			if(AllManage.inv.slotType == SlotType.Formula){
//				AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsBuyStoreClient , 0 , 0 , inv.itemID , gameObject , "YesMaiChuTipsneedConquerBadge");
				AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiRu" , "NoMai" , AllManage.AllMge.Loc.Get("info298") + ""+(inv.needConquerBadge)+AllManage.AllMge.Loc.Get("info1065") + "" + AllManage.AllMge.Loc.Get("messages072") + inv.itemName+"？");				
			}else{
				if(ToInt.StrToInt(inv.costBlood) != 0){
					useBuyStr = ToInt.StrToInt(inv.costBlood)+AllManage.AllMge.Loc.Get("info297");
				}else{
					useBuyStr = "";
				}			
//				AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsBuyStoreClient , 0 , 0 , inv.itemID , gameObject , "YesMaiRuTips");
				AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiRu" , "NoMai" , AllManage.AllMge.Loc.Get("info298") + ""+(ToInt.StrToInt(inv.costGold) * 2)+AllManage.AllMge.Loc.Get("info296") + useBuyStr + AllManage.AllMge.Loc.Get("messages072") + inv.itemName+"？");	
			}
		}else{
			YesMaiRu();
		}
	}
	InfoClose();
}

	function YesMaiChuTips(objs : Object[]){
		AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiChu" , "NoMai" , AllManage.AllMge.Loc.Get("info295") +inv.itemName+AllManage.AllMge.Loc.Get("info285") + objs[1] + AllManage.AllMge.Loc.Get("info296"));
	}


	function YesMaiChuTipsneedHeroBadge(objs : Object[]){
		AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiRu" , "NoMai" , AllManage.AllMge.Loc.Get("info298") + ""+objs[5]+AllManage.AllMge.Loc.Get("info1064") + "" + AllManage.AllMge.Loc.Get("messages072") + inv.itemName+"？");				
	}


	function YesMaiChuTipsneedConquerBadge(objs : Object[]){
		AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiRu" , "NoMai" , AllManage.AllMge.Loc.Get("info298") + ""+objs[6]+AllManage.AllMge.Loc.Get("info1065") + "" + AllManage.AllMge.Loc.Get("messages072") + inv.itemName+"？");				
	}

	function YesMaiRuTips(objs : Object[]){
				if(objs[1] != 0){
					useBuyStr = objs[1]+AllManage.AllMge.Loc.Get("info297");
				}else{
					useBuyStr = "";
				}			
		AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesMaiRu" , "NoMai" , AllManage.AllMge.Loc.Get("info298") + ""+objs[2]+AllManage.AllMge.Loc.Get("info296") + useBuyStr + AllManage.AllMge.Loc.Get("messages072") + inv.itemName+"？");	
	}

function YesMaiChu(){
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.Sell , 500 , 0 , AllManage.inv.itemID , gameObject , "realYesMaiChu");
//	AllManage.AllMge.UseMoney( ToInt.StrToInt(AllManage.inv.costGold) * (-1)  +  ToInt.StrToInt(AllManage.inv.costBlood) * 500 * (-1)  , 0 , UseMoneyType.Sell , gameObject , "realYesMaiChu");
}

var NowClickItemIndex : int = 0;
function realYesMaiChu(){
	if(AllManage.InvclStatic.DesBagItem(AllManage.inv)){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
//		ps.UseMoney(AllManage.inv.costGold * (-1)  + AllManage.inv.costBlood * 500 * (-1) , 0 );
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.OutItem , AllManage.inv.itemID);
//			ps.UseBloodStone(0 , );
		AllManage.InvclStatic.UpdateBagItem();
	}
	AllManage.inv = null;
}

function YesMaiRu(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	if((AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 1)){
	    if((AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 1 && AllManage.InvclStatic.TutorialsDetectionAsID("51")) || ps.isBlood( ToInt.StrToInt(AllManage.inv.costBlood)) && ps.isMoney( ToInt.StrToInt(AllManage.inv.costGold))){
//			print((AllManage.InvclStatic.TutorialsDetectionAsID("51") && AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 1) + " == + bool");
			if((AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 1) && ! (AllManage.InvclStatic.TutorialsDetectionAsID("51")))
//				AllManage.AllMge.UseMoney(AllManage.inv.costGold, AllManage.inv.costBlood , UseMoneyType.YesMaiRu , gameObject , "");
//				ps.UseMoney(AllManage.inv.costGold , AllManage.inv.costBlood);
				var invStr : String;
				var iiv : InventoryItem;
				iiv = new InventoryItem();
			if(AllManage.inv.itemQuality == 0 && parseInt( AllManage.inv.itemID.Substring(0,1)) < 7){
				var ran : int;
				var ran1 : int;
				ran = Random.Range(1,1000);
				if(ran > 998){
					ran1 = 4;
				}else
				if(ran > 900){
					ran1 = 3;
				}else
				if(ran > 200){
					ran1 = 2;
				}else{
					ran1 = 1;					
				}
				invStr = AllResources.staticLT.MakeItemID2(invStr, ran1 , parseInt(AllManage.inv.itemID.Substring(0,1)) , parseInt(AllManage.inv.itemID.Substring(1,1))); 
				iiv = AllResources.InvmakerStatic.GetItemInfo(invStr , iiv);
				iiv.costGold = ToInt.IntToStr(ToInt.StrToInt(iiv.costGold) / 2);
				iiv.costBlood  = ToInt.IntToStr(ToInt.StrToInt(iiv.costBlood) / 2);
				AllManage.InvclStatic.AddBagItem(iiv);			
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , iiv.itemID);
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.ClientBuy , iiv.itemID);
	//			AllManage.InvclStatic.AddBagItem(AllManage.inv);			
	//			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , AllManage.inv.itemID);
			}else{
				invStr = AllManage.inv.itemID;
				iiv = AllResources.InvmakerStatic.GetItemInfo(invStr , iiv);
				iiv.costGold = ToInt.IntToStr(ToInt.StrToInt(iiv.costGold) / 2);
				iiv.costBlood = ToInt.IntToStr(ToInt.StrToInt(iiv.costBlood) / 2);
				AllManage.InvclStatic.AddBagItem(iiv);			
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , AllManage.inv.itemID);
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.ClientBuy , AllManage.inv.itemID);
			}
			if(MaiBag != null){
				MaiBag.OtherYiChu();
				MaiBag = null;
			}
			AllManage.InvclStatic.UpdateBagItem();
		}	
	}else{
	    if(AllManage.inv.slotType == SlotType.Formula || AllManage.inv.slotType == SlotType.Rear || (AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 1 && AllManage.InvclStatic.TutorialsDetectionAsID("51")) || ps.isBlood( ToInt.StrToInt(AllManage.inv.costBlood)) && ps.isMoney( ToInt.StrToInt(AllManage.inv.costGold))){
//			print("11111111");
			if((AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 1 && AllManage.InvclStatic.TutorialsDetectionAsID("51")) ){
	//			realYesMaiRu();
					//print(AllManage.inv.costGold + " == "  + AllManage.inv.costBlood);
//				AllManage.tsStatic.RefreshBaffleOn();
				PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().BuyStoreClient(AllManage.invCangKuStatic.nowSelectStoreType , NowClickItemIndex , 0 , 0 , DungeonControl.level,AllManage.inv.itemID));		
			}else{
				if(AllManage.inv.itemQuality == 0 && parseInt( AllManage.inv.itemID.Substring(0,1)) < 7){
					//print(AllManage.inv.costGold + " == "  + AllManage.inv.costBlood);
//					AllManage.tsStatic.RefreshBaffleOn();
//					print(NowClickItemIndex + " == NowClickItemIndex");
					PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().GetRandomItem(NowClickItemIndex - 12 , DungeonControl.level ,  ToInt.StrToInt(AllManage.inv.costGold) * (-1) * 2 ,  ToInt.StrToInt(AllManage.inv.costBlood) * (-1)));
				}else{
//			print("2222222");
//					AllManage.tsStatic.RefreshBaffleOn();
					//print(AllManage.inv.costGold + " == "  + AllManage.inv.costBlood);
					PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().BuyStoreClient(AllManage.invCangKuStatic.nowSelectStoreType , NowClickItemIndex ,  ToInt.StrToInt(AllManage.inv.costGold) * (-1) * 2 ,  ToInt.StrToInt(AllManage.inv.costBlood) * (-1) , DungeonControl.level , AllManage.inv.itemID));
				}
	//			AllManage.AllMge.UseMoney(AllManage.inv.costGold , AllManage.inv.costBlood , UseMoneyType.YesMaiRu , gameObject , "realYesMaiRu");
			}
	//			ps.UseMoney(AllManage.inv.costGold , AllManage.inv.costBlood);
				
		}
	}
	AllManage.inv = null;
}

function realYesMaiRu(){
			var invStr : String;
			var iiv : InventoryItem;
			iiv = new InventoryItem();
		if(AllManage.inv.itemQuality == 0 && parseInt( AllManage.inv.itemID.Substring(0,1)) < 7){
			var ran : int;
			var ran1 : int;
			ran = Random.Range(1,1000);
			if(ran > 998){
				ran1 = 4;
			}else
			if(ran > 900){
				ran1 = 3;
			}else
			if(ran > 200){
				ran1 = 2;
			}else{
				ran1 = 1;					
			}
			invStr = AllResources.staticLT.MakeItemID2(invStr, ran1 , parseInt(AllManage.inv.itemID.Substring(0,1)) , parseInt(AllManage.inv.itemID.Substring(1,1))); 
			iiv = AllResources.InvmakerStatic.GetItemInfo(invStr , iiv);
			iiv.costGold = ToInt.IntToStr(ToInt.StrToInt(iiv.costGold) / 2);
			iiv.costBlood = ToInt.IntToStr(ToInt.StrToInt(iiv.costBlood) / 2);
			AllManage.InvclStatic.AddBagItem(iiv);			
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , iiv.itemID);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.ClientBuy , iiv.itemID);
//			AllManage.InvclStatic.AddBagItem(AllManage.inv);			
//			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , AllManage.inv.itemID);
		}else{
			invStr = AllManage.inv.itemID;
			iiv = AllResources.InvmakerStatic.GetItemInfo(invStr , iiv);
			iiv.costGold = ToInt.IntToStr(ToInt.StrToInt(iiv.costGold) / 2);
			iiv.costBlood = ToInt.IntToStr(ToInt.StrToInt(iiv.costBlood) / 2);
			AllManage.InvclStatic.AddBagItem(iiv);			
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , AllManage.inv.itemID);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.ClientBuy , AllManage.inv.itemID);
		}
		if(MaiBag != null){
			MaiBag.OtherYiChu();
			MaiBag = null;
		}
		AllManage.InvclStatic.UpdateBagItem();
}

function NoMai(){

}

var DaoJuNonButtons : GameObject[];
var DaoJuid : String;
var DaoJuButton : GameObject;
function ShouDaoJuItem(id : String , Name : String , info : String , icon : String){
	LabelName.text = "";
	Labelleixing.text = "";
	Labelzhiye.text = "";
	Labelshuxing.text = "";
	if(labelWhite){
	labelWhite.text = "";
	}
	if(labelGreen){
	labelGreen.text = "";
	}
	if(labelPurple){
	labelPurple.text = "";
	}	
	Labeldefen.text = ""; 
	LabelLevel.text = "";
	LabelJinBi.text = "";
	LabelInfo.text = "";
	if(buttonsBijiao)
		buttonsBijiao.localPosition.y = -240;
	if(buttons4)		
		buttons4.localPosition.y = 0;		
	inv = new InventoryItem();
	DaoJuid = id;
	InfoObj.transform.localPosition.y = usey;
	InfoObj.SetActiveRecursively(true);
	invSprite.spriteName = icon;
	LabelName.text = "[0000ff]" + Name;
	Labelshuxing.text = info;
	for(var i=0; i<DaoJuNonButtons.length; i++){
		DaoJuNonButtons[i].SetActiveRecursively(false);
	}
	try{
		spriteHolesAttr[0].gameObject.SetActiveRecursively(false);
		spriteHolesAttr[1].gameObject.SetActiveRecursively(false);
		spriteHolesAttr[2].gameObject.SetActiveRecursively(false);	
	}catch(e){
	
	}
}

function UseDaoJu(){
	AllManage.InvclStatic.UseDaojuAsID(DaoJuid);
	InfoClose();
}

var uicl : UIControl;
//var ParentButton
function ShowDaoJuInfo(){
	
}

var buttons4 : Transform;
var buttonsBijiao : Transform;
function ButtonMoveOut(bool : boolean){
//	//print(bool);
	if(buttons4 != null){
		if(!bool){
			buttons4.localPosition.y = 3000;			
		}else{
			buttonsBijiao.localPosition.y = -240;
			buttons4.localPosition.y = 0;		
		}
	}
}
var ButtonsMaiRu : Transform;
var LabelMaiRu : UILabel;
function ButtonMoveOutBiJiao(bool : boolean){
	yield;
	if(buttonsBijiao != null){
		if(!bool){
			buttonsBijiao.localPosition.y = 3000;			
		}else{
			buttonsBijiao.localPosition.y = -240;
		}
		if(LabelMaiRu){
//			ButtonsMaiRu.localPosition.y = 0;
			isMaiChu = false;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages072");
			AllManage.AllMge.SetLabelLanguageAsID(LabelMaiRu);
//			LabelMaiRu.text = "买入";		
		}
	}
}



var bagParent : Transform;
//var invcl : InventoryControl;
//var uicl : UIControl;
var ShowOtherPlayer : rendercamerapic;
function InfoClose(){
	if(ShowOtherPlayer){
			ShowOtherPlayer.enabled = true;
			ShowOtherPlayer.cancelrenderCSNow();
			}
	if(AllManage.InvclStatic.isCangku){
	}else
	if(AllManage.InvclStatic.isShangdian){
	
	}else{
		if(EquepParent){
			EquepParent.localPosition.y = 0;		
		}
	}
		bagParent.localPosition.y = 0;
		InfoObj.transform.localPosition.y = 3000;
		InfoObj.SetActiveRecursively(false);
		if(invInfoObj){
			invInfoObj.gameObject.SetActiveRecursively(false);		
		}
	if(uicl != null){
		if(uicl.isStore){
			 uicl.returnStore();
		}
	}
}

var gem : GameObject[];
function ShowGem(){
	
		var i : int = 0;
		if(gem){
		
		if(parseInt(inv.itemID.Substring(0,1)) < 7){
		for(i = 0;i<gem.length;i++){

		gem[i].SetActive(true);
			}
			}else{
		for(i = 0;i<gem.length;i++){
		gem[i].SetActive(false);
		}
			}
		
	}
}

function CloseDownButton(){
	for(var i=0; i<DaoJuNonButtons.length; i++){
		DaoJuNonButtons[i].SetActiveRecursively(false);
	}
}


