var PetID : int;
//var sell : SellInventory;
function Start(){
	yield WaitForSeconds(0.1);
	animation.Play("sit");
//	if(PetID == 0){
//		this.active = false;
//	}
}
function Update () {
}

function OnMouseDown(){
			audio.Play();
//	if(PetID != 0 && !SpawnPrefab2.cooper){
//		if(sell == null){
//			sell = FindObjectOfType(SellInventory);
//		}
//		sell.SelectPet(PetID);

//	}
}

function OnEnable(){
//	//print("dun xia");
	animation.Play("sit");
	idle();
}

function idle(){
	while (true){
	yield WaitForSeconds(Random.Range(10,25));
//	animation["idle0"].layer = 6;
	animation.CrossFade("idle0",0.4);
//    yield WaitForSeconds(animation["idle0"].length-0.2);	
	yield WaitForSeconds(Random.Range(10,15));
	animation.CrossFade("look",0.4);	
    yield WaitForSeconds(Random.Range(5,25));	
	animation.CrossFade("sit",0.4);    	
	}
	}