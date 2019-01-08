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
function setInv(iv : InventoryItem){
	inv = iv;
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
	invSprite.spriteName = inv.atlasStr;
	Labelname.text = colorstr+inv.itemName;
	Labeltype.text =  AllManage.AllMge.Loc.Get(  inv.weaponTypeStr[inv.slotType]) ;
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

var ebCL : EquepmentPunchControl;
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
