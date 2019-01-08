#pragma strict
var level : int;
private var gan : Joystick;
var goWayStr : String = "GoWay";
static var chuansongmen : TriggerLoadLevel;
static var chuansongmen1 : TriggerLoadLevel;
static var fishPlace : Transform;
function Awake(){
	if(transform.parent.name == "ChuanSongMeng"){
		chuansongmen = this;
	}else
	if(transform.parent.name == "ChuanSongMeng1"){
		chuansongmen1 = this;
	}
	if(level == 152){
		fishPlace = this.transform;
	}
}

private var pTime : float;
function Update(){
	if(Time.time > pTime){
		pTime  = Time.time + 0.2;
		OpenThis();
	}
}

function OpenThis(){
//		print("111111");
	if(bool && !barOpen){
		AllManage.mtwStatic.setnowMen(this);
//		bool = false;
//		print("sdfsdfsdfsdf");
		#if UNITY_IPHONE || UNITY_ANDROID
			gan = FindObjectOfType(Joystick);
			if(gan)
				gan.ResetJoystick();
	    #endif
	    	if(level == 0){
//				if(AllManage.InvclStatic.isBagFull() && !AllManage.UICLStatic.boolFullBag){	
////					AllManage.UICLStatic.boolFullBag = true;
//					AllManage.qrStatic.ShowQueRen(gameObject,"","","info885");	
//				}else{
					RealGoLevelLeft();
//				}
	    	}else{
	    		if(level == 153){
					otherObj.SendMessage("showTS","当前版本未开放此功能。",SendMessageOptions.DontRequireReceiver);	
	    		}else{
	    			if(UIControl.mapType == MapType.zhucheng)
		     		DungeonControl.ReLevel = Application.loadedLevelName;
					otherObj.SendMessage("LookisGoOn",SendMessageOptions.DontRequireReceiver);	
//		     		PhotonNetwork.LeaveRoom();
					Loading.Level = "Map" + level.ToString();
					if(otherPos){
						otherObj.SendMessage("SaveAnotherPos",otherPos.position , SendMessageOptions.DontRequireReceiver);	
					}
					alljoy.DontJump = true;
					yield;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
						AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
		
	    		}
    	}
	}
}

function RealGoLevelLeft(){
	otherObj.SendMessage(goWayStr,SendMessageOptions.DontRequireReceiver);		
}

var bool : boolean = false;
var barOpen : boolean = false;
var otherPos : Transform;
var otherObj : GameObject;
var playerS : PlayerStatus;
function OnTriggerEnter(other : Collider){
	if(other.CompareTag ("Player")){
		playerS = other.GetComponent(PlayerStatus);
		if(playerS && PlayerUtil.isMine(playerS.instanceID)){
			bool = true;		
			otherObj = other.gameObject;
			otherObj.SendMessage("SetNowPortal" , this , SendMessageOptions.DontRequireReceiver);
		}
	}
}

function OnTriggerExit(other : Collider){
	if(other.CompareTag ("Player")){
		playerS = other.GetComponent(PlayerStatus);
		if(playerS && PlayerUtil.isMine(playerS.instanceID)){
			bool = false;		
			otherObj = other.gameObject;
			if(barOpen){
				AllManage.mtwStatic.show0();
			}
		}
	}
}
