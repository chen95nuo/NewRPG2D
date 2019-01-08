#pragma strict

var childObj : GameObject;
function ViewOpen(){
	if(childObj)
		childObj.SetActive(true);
}

function ViewClose(){
	if(childObj)
		childObj.SetActive(false);
}
