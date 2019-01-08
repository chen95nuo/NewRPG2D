
#pragma strict

@System.Serializable														
class BuffEffect {
    var effectPool: GameObjectPool;
    var effectprefab : GameObject; 
    var up =3.8;
    var cache : int = 0; 
}

var EffectP : BuffEffect[];


function Awake(){
	AllResources.BuffefmanageStatic = this;
for (var i:int = 0; i < EffectP.length; i++) {
    EffectP[i].effectPool = GameObjectPool(EffectP[i].effectprefab,EffectP[i].cache*2, InitializeGameObject, false);
//	EffectP[i].effectPool.PrePopulate(EffectP[i].cache);
  }
}

function Start(){
	if( !AllResources.BuffefmanageStatic){
		AllResources.BuffefmanageStatic = this;
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

function SpawnEffect(i : int ,t:float, spawnP : Transform){
    var spawnPoint = spawnP.transform.position + spawnP.transform.up*EffectP[i].up;
    var spawnRotation = spawnP.transform.rotation;
    if(EffectP[i].effectprefab)
	var newEffect = EffectP[i].effectPool.Spawn(spawnPoint, spawnRotation);
	if(newEffect)
	newEffect.transform.parent = spawnP.transform;
	yield WaitForSeconds(t); // 这里是产生n秒钟后就把它返回到内存池，时间限制
	if(newEffect){	
    newEffect.transform.parent = null;
	UnspawnEffect(i,newEffect);
	}
}

// 返回到内存方法
function UnspawnEffect(i : int,obj : GameObject){
EffectP[i].effectPool.Unspawn(obj);
}