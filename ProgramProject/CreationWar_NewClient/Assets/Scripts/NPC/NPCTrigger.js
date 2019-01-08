#pragma strict

private var ai : npcAI;
var vec : Vector3;
function Awake(){
vec = transform.localScale;
ai = transform.parent.GetComponent(npcAI);
}
function OnTriggerEnter(other : Collider){
//		//print("55555");
	if(other.CompareTag ("Player")){
//		//print("1111111111");
		if(ai.canFuhao){
			other.SendMessage("closeNpcTalkYes" , ai.fuhaoID , SendMessageOptions.DontRequireReceiver);
		}else{
			other.SendMessage("closeNpcTalkYes" , -1 , SendMessageOptions.DontRequireReceiver);
		}
//		other.SendMessage("closeNpcTalkYes" , ai.fuhaoID , SendMessageOptions.DontRequireReceiver);
//		if(other.GetComponent(agentLocomotion).enabled){
////		//print("22222222222");
//			ai.TriggerOn();
//		}
	}
}

function OnTriggerExit(other : Collider){
	if(other.CompareTag ("Player")){
		other.SendMessage("closeNpcTalkNo" , ai.fuhaoID , SendMessageOptions.DontRequireReceiver);
	}
}

function initMe(){
	transform.localScale = Vector3.zero;
	yield;
	yield;
	yield;
	yield;
	yield;
	transform.localScale = vec;
}

function OnClick(){
	ai.OnClick();
}