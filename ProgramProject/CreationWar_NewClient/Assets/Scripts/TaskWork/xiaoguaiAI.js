#pragma strict
var task : MainTask = null;
private var mtw : MainTaskWork;

function Start () {

}

function Update () {

}

function OnTriggerEnter(other : Collider){
	if(other.CompareTag ("Player")){
		animation.Play("idle");
		mtw.TaskInfoValue(task,TaskValueType.monster);
	}
}

function SetTask(g : GameValue){
	task = g.task;
	mtw = g.mtw;
}