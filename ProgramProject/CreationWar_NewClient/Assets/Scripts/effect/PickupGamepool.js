
#pragma strict

@System.Serializable														
class PickupSS {
    var pickPool: GameObjectPool;
    var pickprefab : GameObject; 
    var cache : int = 0; 
    var dtime : float;

}

var PickupP : PickupSS[];


function Awake(){
	AllResources.PickpoolStatic = this;
for (var i:int = 0; i < PickupP.length; i++) {
    PickupP[i].pickPool = GameObjectPool(PickupP[i].pickprefab,PickupP[i].cache*2, InitializeGameObject, false);
//	PickupP[i].pickPool.PrePopulate(PickupP[i].cache);
  }
}

function Start(){
	if(!AllResources.PickpoolStatic){
		AllResources.PickpoolStatic = this;
	}
}

function Clear(){
//for (var i:int = 0; i < PickupP.length; i++) {
//    PickupP[i].pickPool.Clear();
//  }
	var child : Transform;
	for(child in transform){
		for (var i:int = 0; i < PickupP.length; i++) {
			if(child.name.Substring(0 , child.name.Length - 7) == PickupP[i].pickprefab.name){
				UnspawnPickup(i , child.gameObject);
			}
		}
	}
}

function InitializeGameObject(target : GameObject){
	//target.SendMessage ("SetCentralController", this, SendMessageOptions.DontRequireReceiver);
}

///传入i为决定爆物品的类型,spawnP为位置//
function	SpawnPickup(	i : int , spawnP : Transform	)
{
    var spawnPoint	=	Vector3(
    	spawnP.transform.position.x+Random.Range(-3.0,3.0),
    	spawnP.transform.position.y,
    	spawnP.transform.position.z+Random.Range(-3.0,3.0));
    var spawnRotation	=	spawnP.transform.rotation;
	var newEffect		=	PickupP[i].pickPool.Spawn(spawnPoint, spawnRotation);
	if(	newEffect	)
	{
//	newEffect.transform.parent = transform;
		if(	newEffect.GetComponent(Pickup)	!=	null	)
			newEffect.GetComponent(Pickup).xx = i;
	}
    if(	PickupP[i].dtime!=0	)
    {
		StartCoroutine(	AutoUpspwn(	PickupP[i].dtime,	i,	newEffect	)	);
//	yield WaitForSeconds(PickupP[i].dtime); // 这里是产生n秒钟后就把它返回到内存池，时间限制
//	if(newEffect)
//	UnspawnPickup(i,newEffect);
	}
}

//自动清除这个战利品//
function	AutoUpspwn(	mTime:float, mNum:int, mObj:GameObject )
{
	yield	WaitForSeconds( mTime ); // 这里是产生n秒钟后就把它返回到内存池，时间限制
	if(	mObj )
		UnspawnPickup( mNum, mObj );
}

// 返回到内存方法
function	UnspawnPickup(i : int,obj : GameObject)
{
	PickupP[i].pickPool.Unspawn(obj);
}