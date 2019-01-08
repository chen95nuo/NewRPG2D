#pragma strict

//var task : MainTask = null;
private var once : boolean = false;
//var ItemType : ProducedItemType;
var produced : ProducedObject;
var col : Collider;
function Awake(){
	if(UIControl.mapType == MapType.zhucheng){
//		transform.localScale = Vector3(10000,10000,10000);
	}
}

function OnEnable(){
	once = true;
}

function Start(){
	yield;
	yield;
	yield;
	while(PlayerStatus.MainCharacter == null){
		yield;
	}
		if(produced != null && once &&produced.task.taskType != MainTaskType.daoda){
			produced.spawnObj = gameObject;
			PlayerStatus.MainCharacter.SendMessage("CreateTaskObject",produced,SendMessageOptions.DontRequireReceiver);
			once = false;
			clear = false;
			col.enabled = false;
//			print(produced.task.taskName + " == scenetask");
			if(produced.task.jindu == 1 && produced.task.taskType == MainTaskType.guaiwu){
			}else{
//				//print(transform.position);
				Destroy(gameObject);
			}
		}
}

var ps : PlayerStatus;
function OnTriggerEnter(other : Collider){
	if(other.CompareTag ("Player")){
		ps = other.GetComponent(PlayerStatus);
		if(ps == null){
			return;
		}
	   if(PlayerUtil.isMine(ps.instanceID)){
		if(produced != null && once){
			produced.spawnObj = gameObject;
			other.SendMessage("CreateTaskObject",produced,SendMessageOptions.DontRequireReceiver);
			once = false;
			clear = false;
			col.enabled = false;
			if(produced.task.jindu == 1 && produced.task.taskType == MainTaskType.guaiwu){
			}else{
				Destroy(gameObject);
			}
		}
		}
	}
}

var npcID : String;
function SetNPCID(id : String){
	npcID = id;
}

var clear : boolean = false;
function MonsterDie(){
	clear = true;
}
