#pragma strict

var objPar : GameObject;
var mesh : Mesh;
var matl : Material;
var effect : GameObject;
var effectRear: GameObject;
function Start(){
	objPar = transform.parent.gameObject; 
	if(objPar){
		mesh = GetComponent(MeshFilter).mesh;
		matl = gameObject.renderer.material;
	}
	if( mesh != null && matl != null){ 
		bool = true;
		if(effect)
		effect.transform.parent = transform.parent;
		if(effectRear)
		effectRear.transform.parent = transform.parent;
		objPar.AddComponent("MeshFilter");
		objPar.AddComponent("MeshRenderer");
		objPar.GetComponent(MeshFilter).mesh = mesh; 
		objPar.renderer.useLightProbes = true;
		objPar.renderer.material = matl; 
		gameObject.active = false;
//		//print(gameObject);
	}
}

var bool : boolean = false;;
var  functionID : int = 1;
function OnEnable(){ 
//	//print(functionID + " == functionID" + gameObject);
	switch(functionID){
		case 1: 
//			try{
			if(transform.parent){
				objPar = transform.parent.gameObject; 
				if(objPar){				
					mesh = GetComponent(MeshFilter).mesh;
					matl = gameObject.renderer.material;
				}
				if( mesh != null && matl != null){ 
					bool = true;
					if(effect)
					effect.transform.parent = transform.parent;
					if(effectRear)
					effectRear.transform.parent = transform.parent;
					if(objPar.GetComponent(MeshFilter)){
						objPar.GetComponent(MeshFilter).mesh = mesh; 
						objPar.renderer.useLightProbes = true;
						objPar.renderer.material = matl; 
					}
					gameObject.active = false;
				}
			}
//			}catch(e){
//				//print("OnEnable");				
//			}
			break;
		case 2: 
			DestoryMe();
			break;
		case 3: 
			ChangeParent();
			break;
	}
}

function OnDisable(){
	if(bool){
		bool = false;
	}else{
		if(objPar){
			if(effect)
			//effect.transform.parent = transform;
			if(effectRear)
			//effectRear.transform.parent = transform;
			if(objPar.GetComponent(MeshFilter))
			objPar.GetComponent(MeshFilter).mesh = null; 
		}
	}
//	gameObject.SetActiveRecursively(false);
	gameObject.active = false;
}

function DestoryMe(){
	try{
		if(objPar && objPar.GetComponent(MeshFilter)){ 
			if(effect)
			effect.transform.parent = transform;
			if(effectRear)
			effectRear.transform.parent = transform;
			objPar.GetComponent(MeshFilter).mesh = null; 
		}
		Destroy(gameObject);
	}catch(e){
		//print("DestoryMe");
	}
}

function ChangeParent(){
	try{
		if(objPar && objPar.GetComponent(MeshFilter)){ 
			if(effect)
			effect.transform.parent = transform;
			if(effectRear)
			effectRear.transform.parent = transform;
			objPar.GetComponent(MeshFilter).mesh = null; 
		}
		gameObject.active = false;
	}catch(e){
		//print("ChangeParent");	
	}
}
