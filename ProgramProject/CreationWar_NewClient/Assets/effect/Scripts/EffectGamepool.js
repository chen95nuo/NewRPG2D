
#pragma strict

@System.Serializable														
class EffectSS {
    var effectPool: GameObjectPool;
    var effectprefab : GameObject; 
    var cache : int = 0; 
    var parents :boolean;
    var dtime : float;
    var offsetup : float;
    var offsetforward : float;
}

var EffectP : EffectSS[];


function Awake(){
	AllResources.EffectGamepoolStatic = this;
	for (var i:int = 0; i < EffectP.length; i++) {
	EffectP[i].effectPool = new GameObjectPool(EffectP[i].effectprefab,EffectP[i].cache*2, InitializeGameObject, false);
  }

}

function Start(){
	if(! AllResources.EffectGamepoolStatic){
		AllResources.EffectGamepoolStatic = this;
	}
	if(AllManage.UICLStatic && AllManage.UICLStatic.mapType != MapType.zhucheng){
		for (var i:int = 0; i < EffectP.length; i++) {
			if(EffectP[i].cache > 0){	
				for(var m=0; m<EffectP[i].cache; m++){
					SpawnEffect(i , transform , "");
				}
			}
		////    EffectP[i].effectPool = new GameObjectPool(EffectP[i].effectprefab,EffectP[i].cache*2, InitializeGameObject, false);
		////	EffectP[i].effectPool.PrePopulate(EffectP[i].cache);
		  }
	}
}

function Clear(){
//for (var i:int = 0; i < EffectP.length; i++) {
//    EffectP[i].effectPool.Clear();
//  }
	var child : Transform;
	for(child in transform){
		for (var i:int = 0; i < EffectP.length; i++) {
			if(EffectP[i].effectprefab && child.name.Substring(0 , child.name.Length - 7) == EffectP[i].effectprefab.name){
				UnspawnEffect(i , child.gameObject);
			}
		}
	}
}

function InitializeGameObject(target : GameObject){
	//target.SendMessage ("SetCentralController", this, SendMessageOptions.DontRequireReceiver);
}

private var px : int;
private var py : int;

function SpawnEffect(i : int , spawnP : Transform){
try
{
//	print("123123123123123");
    var spawnPoint = spawnP.transform.position + spawnP.transform.up*EffectP[i].offsetup +  spawnP.transform.forward*EffectP[i].offsetforward;
    var spawnRotation = spawnP.transform.rotation;
        px = spawnPoint.x*0.03125 +11;
        py = spawnPoint.z*0.03125 +11;
    var PositionID = px +""+ py;
     for (var n : int = 0; n < 9; n++) { 
       if(PositionID ==Minimap.ppIDlist[n]){
         var newEffect = EffectP[i].effectPool.Spawn(spawnPoint, spawnRotation);
         break;
         }
          }	  
	if(newEffect){
	if(EffectP[i].parents)
	newEffect.transform.parent = spawnP.transform;
	else
	newEffect.transform.parent = transform;
//	Debug.Log("--------------"+EffectP[i].dtime);
	StartCoroutine(AutoUpspwn(EffectP[i].dtime,i,newEffect));
	//yield WaitForSeconds(EffectP[i].dtime); // 这里是产生n秒钟后就把它返回到内存池，时间限制
	//UnspawnEffect(i,newEffect);
	}
	}
	catch(ex:System.Exception)
	{
		Debug.LogError (ex.ToString ());
	}
}

function AutoUpspwn(mTime:float,mNum:int,mObj:GameObject)
{
	yield WaitForSeconds(mTime); // 这里是产生n秒钟后就把它返回到内存池，时间限制
	UnspawnEffect(mNum,mObj);
}



function SpawnEffect(i : int , spawnP : Transform , str : String){
//	print("5566123123");
    var spawnPoint = spawnP.transform.position + spawnP.transform.up*EffectP[i].offsetup +  spawnP.transform.forward*EffectP[i].offsetforward;
    var spawnRotation = spawnP.transform.rotation;
    var newEffect = EffectP[i].effectPool.Spawn(spawnPoint, spawnRotation);
	newEffect.transform.parent = transform;
	UnspawnEffect(i,newEffect);
}

// 返回到内存方法
function UnspawnEffect(i : int,obj : GameObject){
	if(EffectP[i].parents && obj)	
    obj.transform.parent = transform;
    EffectP[i].effectPool.Unspawn(obj);
}