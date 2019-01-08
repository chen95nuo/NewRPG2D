#pragma strict

var UIName : UILabel;
var UITeam : UISprite;
function Update () {
	if(AllManage.mainCameraStatic)
		transform.LookAt(AllManage.mainCameraStatic);
		
		gameObject.layer=12;
}
