#pragma strict

function Start () {

}

function Update () {

}

var invs1 : BagItem[];
var invs2 : BagItem[];
var invs3 : BagItem[];
var trans1 : Transform;
var trans2 : Transform;
var trans3 : Transform;
function ShowWeaponAsID(iv1 : InventoryItem , iv2 : InventoryItem , iv3 : InventoryItem , iv4 : InventoryItem , id : int){
	switch(id){
		case 1:
			if(iv1 != null){
				trans1.localPosition.x = 0;
				invs1[0].SetInv(iv1);
			}else{
				invs1[0].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv2 != null){
				trans1.localPosition.x = 0;
				invs1[1].SetInv(iv2);
			}else{
				invs1[1].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv3 != null){
				trans1.localPosition.x = 0;
				invs1[2].SetInv(iv3);
			}else{
				invs1[2].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv4 != null){
				trans1.localPosition.x = 0;
				invs1[3].SetInv(iv4);
			}else{
				invs1[3].gameObject.SetActiveRecursively(false);				
			}
			break;
		case 2:
			if(iv1 != null){
				trans2.localPosition.x = 0;
				invs2[0].SetInv(iv1);
			}else{
				invs2[0].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv2 != null){
				trans2.localPosition.x = 0;
				invs2[1].SetInv(iv2);
			}else{
				invs2[1].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv3 != null){
				trans2.localPosition.x = 0;
				invs2[2].SetInv(iv3);
			}else{
				invs2[2].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv4 != null){
				trans2.localPosition.x = 0;
				invs2[3].SetInv(iv4);
			}else{
				invs2[3].gameObject.SetActiveRecursively(false);				
			}
			break;
		case 3:
			if(iv1 != null){
				trans3.localPosition.x = 0;
				invs3[0].SetInv(iv1);
			}else{
				invs3[0].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv2 != null){
				trans3.localPosition.x = 0;
				invs3[1].SetInv(iv2);
			}else{
				invs3[1].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv3 != null){
				trans3.localPosition.x = 0;
				invs3[2].SetInv(iv3);
			}else{
				invs3[2].gameObject.SetActiveRecursively(false);				
			}
			
			if(iv4 != null){
				trans3.localPosition.x = 0;
				invs3[3].SetInv(iv4);
			}else{
				invs3[3].gameObject.SetActiveRecursively(false);				
			}
			break;
	}
}
