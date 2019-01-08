#pragma strict
var ProID : int;
var anim_ : Animation;
var EFtrans :Transform;

private var AcSkill	:	ActiveSkill;
var attacksound : AudioClip;
static var SelectID : int;
private var dd = true;
private var canselect = true;
private var last : Vector3;
private var Status:PlayerStatus;
private var buttonManager : GameObject;

function Start () {
Status = GetComponent(PlayerStatus);
Status.weaponType = 3;
	PhotonNetwork.offlineMode = true;
	PhotonNetwork.offlineMode_inRoom = true;
	
			      if(PhotonNetwork.room != null){
	    		   	PhotonNetwork.isMessageQueueRunning = false;
					}
					
	buttonManager = GameObject.Find("ButtonManager");
	AcSkill		= GetComponent(ActiveSkill);
 last = transform.position + transform.forward*2;
 anim_.wrapMode = WrapMode.Loop;
 anim_["sit"].wrapMode = WrapMode.Once; 
 anim_["up"].wrapMode = WrapMode.Once; 
 anim_["siting"].layer = 0;  
 yield	WaitForSeconds(1);
 anim_.CrossFade("siting");
 
 	Minimap.ppIDlist = new Array (9);
	for (var i : int = 0; i < 9; i++) { 	
	    Minimap.ppIDlist[i]="1312";
	}

}

function Update () {
if(!dd && SelectID !=ProID)
 Back ();
 
}

var tpw1 : ThirdPersonWeapon;
function OnMouseDown() {
if(canselect){
    SelectID = ProID;
	    if(dd)
	    {	
	    	buttonManager.SendMessage("BeforeSelectCharacter", true, SendMessageOptions.RequireReceiver);
			Standup();
		}
	}
}

function Standup () {
    dd=false;
    canselect = false;
    tpw1.OnFight(true);
    anim_["siting"].layer = 0;   
 	anim_.CrossFade("up"); 
   yield WaitForSeconds(anim_["up"].length-0.3);	
   if(SelectID !=ProID){
    Back ();
   return;
   }
    anim_.CrossFade("idle");
	var angle : float;
	angle = 180.0;
	while (angle > 10 )
	{   
		angle = Mathf.Abs(RotateTowardsPosition(EFtrans.position));	
		yield;
		if(SelectID !=ProID){
        Back ();		
        return;
        }
	}  
	yield WaitForSeconds(0.3); 
	if(SelectID !=ProID){
	    Back ();
     return; 
     }         
    anim_.CrossFade("battle",0.2);
    yield WaitForSeconds(0.5); 
   if(SelectID !=ProID){
     Back ();
     return; 
   }
    Callaction();	
}

function Back () {
    tpw1.OnFight(false);
    dd = true;
	var angle : float;
	angle = 180.0;
	while (angle > 10 )
	{  
		angle = Mathf.Abs(RotateTowardsPosition(last));	
		yield;
	}
	canselect = true;	
anim_.CrossFade("siting");
 anim_["siting"].layer = 0;  
}

function RotateTowardsPosition (targetPos : Vector3) : float
{
	var relative = transform.InverseTransformPoint(targetPos);
	var angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
	var maxRotation = 200 * Time.deltaTime;
	var clampedAngle = Mathf.Clamp(angle, -maxRotation, maxRotation);
	transform.Rotate(0, clampedAngle, 0);
	return angle;
}

function Callaction(){
	if (attacksound)
	audio.PlayOneShot(attacksound);
	if(ProID!=3)
    yield AcSkill.AttackSkill(1);
    else
     yield AcSkill.AttackSkill(2);
 //   animation[NXaction.name].wrapMode = WrapMode.Once; 
 //   animation[NXaction.name].layer = 3;
// 	animation.CrossFade(NXaction.name,0.2);
	//	PlayEffect();
//	yield WaitForSeconds(NXaction.length+0.2);	
    anim_.CrossFade("idle");
}


