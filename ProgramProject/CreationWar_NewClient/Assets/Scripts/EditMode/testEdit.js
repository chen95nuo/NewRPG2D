#pragma strict
@script ExecuteInEditMode
function Start () {

}

var bool : boolean = false;
var ts1 : Transform[];
function Update () {
	if(bool){
		bool = false;
		var ts = new Array();
		for(var trans : Transform in transform){
			if(!trans.active){
				ts.Add(trans);
			}
		}
		ts1 = new Array(ts.Count);
		for(var i=0; i<ts.length; i++){
			ts1[i] = ts[i];
		}
	}
}