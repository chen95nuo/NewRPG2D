#pragma strict

function Update () {

}

var inv : InventoryItem;
var invSprite : UISprite;
var Labelname : UILabel;
var Labeltype : UILabel;
var LabelisEqu : UILabel;
var Labelzhiye : UILabel;
var Labeldengji : UILabel;
var SpriteBiankuang : UISprite;
var SpriteGround : UISprite;
function setInv(iv : InventoryItem){
	inv = iv;
	invSprite.spriteName =  inv.atlasStr;
	var colorstr : String;
		if(inv.slotType < 12){
			if(inv.slotType  == 10 || inv.slotType == 11 ){
				if(inv.itemQuality < 6){
					colorstr = EquepmentItemInfo.useColor[inv.itemQuality] ;				
				}else{
					colorstr = EquepmentItemInfo.useColor[inv.itemQuality - 4] ;	
				}
			}else{
				if(inv.itemQuality < 6){
					colorstr = EquepmentItemInfo.useColor[inv.itemQuality] ;				
				}else{
					colorstr = EquepmentItemInfo.useColor[inv.itemQuality - 4] ;	
				}
			}
		}
	Labelname.text = colorstr+inv.itemName;
	Labeltype.text = AllManage.AllMge.Loc.Get( inv.weaponTypeStr[inv.buildItemSlotType]);
	Labelzhiye.text = AllManage.AllMge.Loc.Get(inv.professionTypeStr[inv.professionType]);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages060");
			AllManage.AllMge.Keys.Add(inv.itemLevel + "");
			AllManage.AllMge.SetLabelLanguageAsID(Labeldengji);
//	Labeldengji.text = "等级" + inv.itemLevel.ToString();
	if(inv.itemHole > 0){
			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add(LabelisEqu.text + "");
			AllManage.AllMge.Keys.Add("[a0ffff]");
			AllManage.AllMge.Keys.Add("messages116");
			AllManage.AllMge.SetLabelLanguageAsID(LabelisEqu);
//		LabelisEqu.text += "[a0ffff]已绑定";
	}else{
			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add(LabelisEqu.text + "");
			AllManage.AllMge.Keys.Add("[a0ffff]");
			AllManage.AllMge.Keys.Add("info851");
			AllManage.AllMge.SetLabelLanguageAsID(LabelisEqu);	
	}
	LabelisEqu.text = "";
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
	}else{ 
		if(inv.slotType  != SlotType.Soul && inv.slotType == SlotType.Digest ){
			SpriteBiankuang.spriteName = "yanse" + 1;	
		}else{
			SpriteBiankuang.enabled = false;			
		}
	}
}

var ebCL : InventoryProduceControl;
function GoSelect(){
	ebCL.SelectOneInv(this , inv);
}

var jiantou : UISprite;
function SelectMe(){
	jiantou.enabled = true;
}

function DontSelectMe(){
	jiantou.enabled = false;
}

var alreadyStudy : boolean = true;
function SwitchAlreadyStudy(){
	alreadyStudy = false;
	SpriteGround.spriteName = "UIB_Union_Player_O";
}
