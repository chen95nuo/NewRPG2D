#pragma strict

var EffectP : EffectSS[];


function Awake(){
	AllResources.FuhaopoolStatic = this;
for (var i:int = 0; i < EffectP.length; i++) {
    EffectP[i].effectPool = GameObjectPool(EffectP[i].effectprefab,EffectP[i].cache*2, InitializeGameObject, false);
//	EffectP[i].effectPool.PrePopulate(EffectP[i].cache);
  }
}

function Start(){
	if(!AllResources.FuhaopoolStatic){
		AllResources.FuhaopoolStatic = this;
	}
}


function Clear(){
//for (var i:int = 0; i < EffectP.length; i++) {
//    EffectP[i].effectPool.Clear();
//  }
	var child : Transform;
	for(child in transform){
		for (var i:int = 0; i < EffectP.length; i++) {
			if(child.name.Substring(0 , child.name.Length - 7) == EffectP[i].effectprefab.name){
				UnspawnEffect(i , child.gameObject);
			}
		}
	}
}

function InitializeGameObject(target : GameObject){
	//target.SendMessage ("SetCentralController", this, SendMessageOptions.DontRequireReceiver);
}

function SpawnEffect(i : int , spawnP : Transform , str : String , high : float , func : String){
//	//print(high);
	if(i >= EffectP.length){
		return;
	}
    var spawnPoint = spawnP.transform.position + spawnP.transform.up*EffectP[i].offsetup +  spawnP.transform.forward*EffectP[i].offsetforward + Vector3(0,high,0);
    var spawnRotation = spawnP.transform.rotation;
	var newEffect = EffectP[i].effectPool.Spawn(spawnPoint, spawnRotation);
	if(newEffect){
		spawnP.gameObject.SendMessage(func , newEffect , SendMessageOptions.DontRequireReceiver);
		if(str != ""){
			newEffect.SendMessage ("setx", str, SendMessageOptions.DontRequireReceiver);		
		}
		if(EffectP[i].parents)
			newEffect.transform.parent = spawnP.transform;
//		else
//			newEffect.transform.parent = transform;
	}
	yield WaitForSeconds(EffectP[i].dtime); // 这里是产生n秒钟后就把它返回到内存池，时间限制
	if(EffectP[i].parents&&newEffect)	
    newEffect.transform.parent = this.transform;
	UnspawnEffect(i,newEffect);
}

// 返回到内存方法
function UnspawnEffect(i : int,obj : GameObject){
//	obj.SendMessage("PlayFalse" , SendMessageOptions.DontRequireReceiver);
//	yield;
////print(i + " == " + obj);
EffectP[i].effectPool.Unspawn(obj);
}