#pragma strict
class HelpClass{
	var helpType : int;
	var helpName : String;
	var helpInfo : String;
}

var allHelps : HelpClass[];
function Start () {
	yield; WaitForSeconds(1);
	GetAllHelps();
}

function GetAllHelps(){
	var i : int = 0;
	allHelps = new Array(Loading.GameHelp.Rows.Count);
//	//print(Loading.GameHelp.Rows.Count + " == Loading.GameHelp.Rows.Count");
	for(i=0; i<Loading.GameHelp.Rows.Count; i++){
		allHelps[i] = new HelpClass();
		allHelps[i].helpType = parseInt(Loading.GameHelp.Rows[i]["HelpType"].YuanColumnText);
		allHelps[i].helpName = (Loading.GameHelp.Rows[i]["HelpName"].YuanColumnText);
		allHelps[i].helpInfo = (Loading.GameHelp.Rows[i]["HelpInfo"].YuanColumnText);
	}
}