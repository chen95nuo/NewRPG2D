#pragma strict

function Start () {
//	pTask = null;
}

function Update () {

}

var TaskNowCL : TaskNowControl;
var pTask : PrivateTask;
var TextTaskName : UILabel;
var strLeiXing : String[];
var SpriteLeiXing : UISprite;
var biankuang : UISprite;
function ShowNowTaskItem(p : PrivateTask){
	pTask = p;
	TextTaskName.text =  "("+UIControl.taskLeixingStrs[pTask.task.leixing] + ")" + p.task.taskName;
	SpriteLeiXing.spriteName = strLeiXing[pTask.task.leixing];
//	//print(gameObject.name);
}

var TaskInfost : TaskInfoList;
var id : int = 0;
function FindWay(){	
	
	TaskNowCL.SelectOneInv(this);
	TaskNowCL.ShowTaskInfo(pTask , id);
//	//print(pTask.task.taskName);
}

function otherFindWay(){
	TaskInfost.setNow(pTask);
	TaskInfost.FindWay();
}

function SelectMe(){
	if(pTask){
		AllManage.taskILStatic.setNow(pTask);	
	}
	biankuang.enabled = true;
}

function DontSelectMe(){
	biankuang.enabled = false;
}