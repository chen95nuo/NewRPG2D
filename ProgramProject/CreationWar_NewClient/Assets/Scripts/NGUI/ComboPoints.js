
var target : GameObject;
var pichit : GameObject;
var picjia : GameObject;
//var not : boolean = false;
var PanelGet : TweenScale;
var PanelMove : TweenScale;
var fennum : Transform;
var UIPobj : GameObject;
var beng : boolean = false;
var canMove : boolean = false;
var hits : hitsss;
var fenMove : boolean = false;
var jiafen : boolean = false;
var points : int;
var numText : UILabel;
var numObj : GameObject;
var PointText : UILabel;
function Awake(){
	numObj.SetActiveRecursively(false);
	PointText.gameObject.active = false;
}

function MoveCombo(num : int){
	numObj.SetActiveRecursively(false);
	if(target != null){
		target.SendMessage("adds", num ,SendMessageOptions.DontRequireReceiver);
	}
}

function GetCombo (num : int){
	numObj.SetActiveRecursively(true);
	numText.text = "x" + num.ToString();
	PanelGet.Play(true);
	canMove = true;
}
