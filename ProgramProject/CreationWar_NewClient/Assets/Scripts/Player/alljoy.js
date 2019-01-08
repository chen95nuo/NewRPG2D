class alljoy extends Photon.MonoBehaviour {
static	var v = 0.0;
static	var h = 0.0;
static var jumpButton = false;
static var rollButton = false;
static var attackButton = false;
static var skillButton = false;
static var rideButton = false;

private var moveJoystick : Joystick;
//private var moveJoystick : NGUIJoystick;
public  var moveJoy : GameObject;
private var joystickMoveGO : GameObject;
private var getUserInput : boolean = true;
private var Status : PlayerStatus; 
private var TController : ThirdPersonController; 
private var atkSlmp : Attack_simple;
function Awake () {
  Status = GetComponent(PlayerStatus); 
TController = GetComponent(ThirdPersonController);
atkSlmp = GetComponent(Attack_simple);
	if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
	getUserInput=true;
	else
	getUserInput=false;
	
		if (getUserInput) {
		#if UNITY_IPHONE || UNITY_ANDROID
			// Create left joystick
			joystickMoveGO = Instantiate (moveJoy) as GameObject;
			joystickMoveGO.name = "Joystick Move";
			moveJoystick = joystickMoveGO.GetComponent.<Joystick> ();
			AllManage.UIALLPCStatic.joy = this;
//			moveJoystick = joystickMoveGO.GetComponent.<NGUIJoystick> ();	
//		    Joystick.funk=false;

	    #endif
	}

}
static var DontJump : boolean = false;
function GanBack(){
if(moveJoystick)
	   	moveJoystick.SendMessage("back", SendMessageOptions.DontRequireReceiver);
}

function ShowJoy(bool : boolean){
	if(moveJoystick)
	moveJoystick.gameObject.active = bool;
}

private var tempinput = 0.0 ;
private var startime = 0.0;
function Update () {
if (PhotonNetwork.room && PhotonNetwork.offlineMode ==false && !PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID) )
    return;
//    if(!DontJump){
//		if(Time.time >startime){
//			startime = Time.time+0.3;
//		    tempinput = Input.acceleration.x;
//		}
		
//		if(!Status.dead && TController.isControllable == true && Mathf.Abs(tempinput-Input.acceleration.x)>0.3)
//		jumpButton = true;
//	}
	if (getUserInput ) {
//	print(AllManage.NGUIJoyStatic);
//	print(skillButton + " == skillButton");
if(moveJoystick != null && !atkSlmp.GetBusy()){
//	print(AllManage.NGUIJoyStatic.position.y);
//	 v = AllManage.NGUIJoyStatic.position.y;
//	 h = AllManage.NGUIJoyStatic.position.x;
	 v = moveJoystick.position.y;
	 h = moveJoystick.position.x;
}
	if(Input.touchCount==0 && !atkSlmp.GetBusy())
	{ 
		v=Input.GetAxisRaw("Vertical");
	    h=Input.GetAxisRaw("Horizontal");
	}
if(Input.GetKeyDown ("space"))
   jumpButton = true;
if(attackButton||skillButton){
	Status.battlemod=true;
	Status.battletime =Time.time;
	}
if(v!=0||h!=0||jumpButton == true||attackButton||skillButton){
	Autoctrl.Wayfinding = false;
	}
//
	if(Time.time > ptime && (attackButton || skillButton)){
		if(attackButton){
			attackButton = false;		
		}
		if(skillButton){
			skillButton = false;		
		}
	}
 }
}
static var ptime : int = 0;
}

function GetJoyStickV() : float{
	if(moveJoystick){
		return moveJoystick.position.y;
	}else{
		return 0;
	}
}

function GetJoyStickH() : float{
	if(moveJoystick){
		return moveJoystick.position.x;
	}else{
		return 0;
	}
}
