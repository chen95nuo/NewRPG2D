#pragma strict

var soucl : SoulControl;
var bagit : BagItem;
var itemMove : ItemMove;
var emits :  ParticleEmitter;
function OnDrop(go : GameObject){ 
//	//print(" ==== " + itemMove.mInv);
//	//print(" ==== " + itemMove.mInv.itemID);
	if(itemMove.mInv != null){
		soucl.ButtonDesDS(itemMove.mInv);
		 emits.emit=true;
		 yield WaitForSeconds(0.1);
		 emits.emit=false;
	}
} 
