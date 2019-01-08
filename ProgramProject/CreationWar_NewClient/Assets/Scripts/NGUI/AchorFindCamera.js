#pragma strict
var anch : UIAnchor;
function Start () {
	anch = GetComponent(UIAnchor);
	anch.uiCamera = AllManage.UICLStatic.UICamStatic;
}

//@script ExecuteInEditMode