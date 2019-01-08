#pragma strict
@System.Serializable														
class classHP {
    var mi : float;
    var ma : float; 
    var rot : Quaternion;
    var gao : float;
    var lv : int;
    var name : String;
    var rankLevel : int;
    var ps : PlayerStatus;
}
var EffectP : EffectSS[];
var hpObj : classHP;


function Awake(){
	AllResources.FontpoolStatic = this;
for (var i:int = 0; i < EffectP.length; i++) {
    EffectP[i].effectPool = GameObjectPool(EffectP[i].effectprefab,EffectP[i].cache*2, InitializeGameObject, false);
//	EffectP[i].effectPool.PrePopulate(EffectP[i].cache);
  }
}

function Start(){
	if(!AllResources.FontpoolStatic){
		AllResources.FontpoolStatic = this;
	}
}

private var childs : Transform[];
function Clear(){
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

private var px : int;
private var py : int;

function SpawnEffect(i : int , spawnP : Transform , str : String , high : float){
//	print("123123123");
    var spawnPoint = spawnP.transform.position + spawnP.transform.up*EffectP[i].offsetup +  spawnP.transform.right*EffectP[i].offsetforward + Vector3(0,high,0);
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
		 newEffect.SendMessage ("setx", str, SendMessageOptions.DontRequireReceiver);
	if(EffectP[i].parents)
	newEffect.transform.parent = spawnP.transform;
//	else
//	newEffect.transform.parent = transform;
	}
	StartCoroutine(AutoUpspwn(EffectP[i].dtime,i,newEffect));
//	yield WaitForSeconds(EffectP[i].dtime); // 这里是产生n秒钟后就把它返回到内存池，时间限制

//	UnspawnEffect(i,newEffect);
}

function AutoUpspwn(mTime:float,mNum:int,mObj:GameObject)
{
	yield WaitForSeconds(mTime); // 这里是产生n秒钟后就把它返回到内存池，时间限制
		if(mObj)	
    mObj.transform.parent = null;
	UnspawnEffect(mNum,mObj);
}

var uicam : Camera;
var ezx : UIPlayerName;
function SpawnEffect(i : int , spawnP : Transform, name : String , shengao : float , ss : String , ps : PlayerStatus , rankLevel : int){
//	print("123123123");
    var spawnPoint = spawnP.transform.position + spawnP.transform.up*EffectP[i].offsetup +  spawnP.transform.right*EffectP[i].offsetforward;
    var spawnRotation = spawnP.transform.rotation;
	var newEffect = EffectP[i].effectPool.Spawn(spawnPoint, spawnRotation);
		if(newEffect != null){
			newEffect.transform.rotation = Quaternion.identity;
			hpObj.gao = shengao;
			hpObj.rot = newEffect.transform.rotation;
			hpObj.name = name;
			hpObj.rankLevel = rankLevel;
			hpObj.ps = ps;
			ezx = newEffect.gameObject.GetComponent(UIPlayerName);
			ezx.par = spawnP;
			ezx.cameraToLookAt = uicam;
			ezx.setx(hpObj);
			ezx.xx = i;
			ezx.SpriteTeam.enabled = false;
//			ezx.ftPool = this;
			newEffect.SendMessage("setx",hpObj,SendMessageOptions.DontRequireReceiver);
			if(Application.loadedLevelName == "Map431"){
				hpObj.name= "[00ffff]+" + hpObj.name;
			}else
			if(UIControl.mapType == MapType.jingjichang){
				if(ss == "Red"){
					if(ss != UIControl.myTeamInfo){
						hpObj.name= "[00ffff]+" + hpObj.name;
					}
					ezx.SpriteTeam.enabled = true;
					ezx.SpriteTeam.spriteName = "UIM_Captain2";
				}else
				if(ss == "Blue"){
					if(ss != UIControl.myTeamInfo){
						hpObj.name= "[00ffff]+" + hpObj.name;
					}
					ezx.SpriteTeam.enabled = true;
					ezx.SpriteTeam.spriteName = "UIM_Captain1";
				}
			}
			if(ps != null){
				
				ps.LabelTitle = newEffect;		
			}
		}
	if(newEffect){
	if(EffectP[i].parents)
	newEffect.transform.parent = spawnP.transform;
	else
	newEffect.transform.parent = null;
	}
}

function SpawnEffect(i : int , spawnP : Transform){
    var spawnPoint = spawnP.transform.position + spawnP.transform.up*EffectP[i].offsetup +  spawnP.transform.right*EffectP[i].offsetforward;
    var spawnRotation = spawnP.transform.rotation;
	var newEffect = EffectP[i].effectPool.Spawn(spawnPoint, spawnRotation);	
	
	if(EffectP[i].parents){
		newEffect.transform.parent = spawnP.transform;
		newEffect.transform.localPosition = Vector3.zero;
	}
	else
	{
		newEffect.transform.parent = transform;
	}
	StartCoroutine(AutoUpspwn(EffectP[i].dtime,i,newEffect));
	yield WaitForSeconds(EffectP[i].dtime); // 这里是产生n秒钟后就把它返回到内存池，时间限制

	UnspawnEffect(i,newEffect);
}

// 返回到内存方法
function UnspawnEffect(i : int,obj : GameObject){
if(obj)
EffectP[i].effectPool.Unspawn(obj);
}