#pragma strict
var bool : boolean = false;
var functionName : String;
var uiws : UIWidget[];
var useuiws : UIWidget;
function Update () {
	if(bool){
		gameObject.SendMessage(functionName , SendMessageOptions.DontRequireReceiver);
		bool = false;
//		uiws = FindObjectsOfType(UIWidget);
		useuiws.MakePixelPerfect();
		for(var i=0; i<uiws.length; i++){
			uiws[i].MakePixelPerfect();
		}
//		NGUISnap.Recalculate(useuiws);
	}
}	
@script ExecuteInEditMode