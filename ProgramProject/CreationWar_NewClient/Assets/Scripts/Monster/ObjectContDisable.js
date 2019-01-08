#pragma strict

function OnDisable(){
	try{
		if(gameObject)
			gameObject.active = true;
	}catch(e){
	
	}
}

function Awake(){
	try{
		gameObject.active = true;
	}catch(e){
	
	}
}
