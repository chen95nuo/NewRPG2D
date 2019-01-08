#pragma strict
var task : MainTask;
var mtw : MainTaskWork;

function Start () {

}

function Update () {

}

function SetTask(g : GameValue){
	task = g.task;
	mtw = g.mtw;
}

function OnTriggerEnter(other : Collider){
	if(other.CompareTag ("Player")){
		mtw.TaskInfoValue(task,TaskValueType.monster);
//		other.SendMessage("",task,SendMessageOptions.DontRequireReceiver);
	}
}
