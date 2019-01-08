#pragma strict

private	var	tipsControl	:TipsControl;
var	Obj	:GameObject;
function Start () {
	
}



function Update () {

}

function OnEnable(){
tipsControl	=	FindObjectOfType(	TipsControl	);
	if(tipsControl.isClickTips&&tipsControl)
		{
		Obj.gameObject.SetActiveRecursively(true);
		tipsControl.isClickTips = false;
		ShowMe();
		}
		

}

function ShowMe()
{
		AllManage.UICLStatic.PlayerSelectSkillMe();
}