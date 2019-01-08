
var s : UILabel;
var UIPM : TweenScale;
var UIPM1 : TweenPosition;
var gensui : boolean = false;
private var v: Vector3;

var ptime : float;

//var sinA : float = 6;
//var sinTime : float = 0;
//var sinK : float = 20;
//var sinSpeed : float = 5;
function setx(x : String)
{
	gensui = true;
	s.text=x;
UIPM.Play(true);
if(UIPM1){
UIPM1.Play(true);
}
//	sinTime = Time.time;
	while(gensui){
		if(Camera.main)
		v=Camera.main.transform.position-transform.position;
		v.x=0;
		v.z=0;
		transform.LookAt(Camera.main.transform.position - v); 
		transform.Rotate(0,180,0);
		transform.position.y+=4*Time.deltaTime;
//		transform.position.y = Mathf.Sin((Time.time - sinTime) * sinSpeed  + sinK) * sinA;
		yield;
	}
}

function OnDisable (){
	gensui = false;
	transform.localPosition = Vector3(0,0.4,0);
}
function OnEnable(){
	transform.localPosition.x += 2;
	transform.localPosition.z -= 2;
}

function PlayFalse(){
	UIPM.Play(false);
	if(UIPM1){
	UIPM1.Play(false);
	}
}