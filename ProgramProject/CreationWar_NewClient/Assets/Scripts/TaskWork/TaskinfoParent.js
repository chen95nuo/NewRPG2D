#pragma strict
var ptask : PrivateTask;
var mtw : MainTaskWork;


function Start () {
	mtw = FindObjectOfType(MainTaskWork);
}

function Update () {

}
var LabeltaskInfo : UILabel;
private var useStr : String;
function SetTask(t : PrivateTask){ 
//	//print("jie shou dao le");
	ptask = t;
	useStr = ptask.doneNum + "/" + ptask.task.doneNum;
	LabeltaskInfo.text = useStr +  "\n";
}

function FindWay(){
	mtw.FindWay(ptask);
}
