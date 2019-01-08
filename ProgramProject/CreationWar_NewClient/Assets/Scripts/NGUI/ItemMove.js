import UnityEngine;
import System.Collections.Generic;

function Awake(){
	AllManage.ItMoveStatic = this;
}

function Start(){
	mInv = null;
}

var SongCursor : UICursor;
var SongSprite : UIAtlas;

static var mInv : InventoryItem; 
function MoveClear(){
//	//print("123123");
	jiaohuanBag = BagIt;
	return mInv;
}

var isDrop : boolean = false;
function MoveCleraReal(){
//	//print("456789");
//	if(isDrop){
//		isDrop = false;
//		BagIt.SetInv(mInv , 1);
//	}
	isMove = false;  
	mInv = null;
	BagIt = null;
	try{
		if(SongCursor){	
			SongCursor.Clear();
		}
	}catch(e){
	
	}
}

var BagIt : BagItem;
var isMove : boolean = false; 
var useBagIt : BagItem;
function MoveStart(inv : InventoryItem,bit : BagItem){
//	//print("inv chu le");
	BagIt = bit; 
	useBagIt = BagIt;
	mInv = inv;
	isMove = true;
	SongCursor.Set(SongSprite,inv.atlasStr);
	return isMove;
}

private var jiaohuanBag : BagItem;
function ItemJiaoHuan(v : InventoryItem){
	try{
		if(jiaohuanBag != null){
			jiaohuanBag.SetInv(v);	
		}
	}catch(e){
		return;
	}
	jiaohuanBag = null;
}

function SetJiaoHuan(bit : BagItem){
	jiaohuanBag = bit;
}

function Update(){
	if (Input.GetButtonUp ("Fire1") || Input.touchCount > 1) {  
		reMove();
	}
}

var invcl : InventoryControl;
static var itemMove : boolean = false;
function reMove(){ 
	yield;
//	//print(isMove + " == isMove");
//	//print(mInv + " == mInv");
	if(isMove == true && mInv != null){
		BagIt.SetInv(mInv);
				AllManage.InvclStatic.CopyToBagIt();
				AllManage.InvclStatic.UpdateBagItem();

//		yield;		
//		useBagIt.InvOnDrapSave();
	}else{
		if(mInv != null){ 
			if(BagIt){
				BagIt.SetInv(mInv);	
//				yield;		
//				BagIt.InvOnDrapSave();
			}else{
				useBagIt.SetInv(mInv);
//				yield;		
//				useBagIt.InvOnDrapSave();
			}
		}
	}
	MoveCleraReal();
	yield;
	if(itemMove){
		itemMove = false;
		invcl.UpdateBagItem();
	//	invcl.UpdateEquipItem();
	}
}
 
function nowReMove(){
//	BagIt.SetInv(mInv);
//	MoveCleraReal();
//	if(itemMove){
//		itemMove = false;
//		invcl.UpdateBagItem();
//	//	invcl.UpdateEquipItem();
//	}
}
