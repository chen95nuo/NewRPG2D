#pragma strict


var buttonAttack : UIButtonMessage;
var buttonSkill1 : SkillItem;
var buttonSkill2 : SkillItem;
var buttonSkill3 : SkillItem;
var buttonSkill4 : SkillItem;
function Update () {
	if(Input.GetKeyDown(KeyCode.J)){
		AllManage.SkillCLStatic.AttackSimple();
	}
	if(Input.GetKeyDown(KeyCode.K)){
		buttonSkill1.OnClick();
	}
	if(Input.GetKeyDown(KeyCode.L)){
		buttonSkill2.OnClick();
	}
	if(Input.GetKeyDown(KeyCode.I)){
		buttonSkill3.OnClick();
	}
	if(Input.GetKeyDown(KeyCode.O)){
		buttonSkill4.OnClick();
	}
	if(Input.GetKeyDown(KeyCode.Y)){
		if(AllManage.uicamStatic.enabled){
			AllManage.uicamStatic.enabled = false;
			AllManage.uicam2Static.enabled = false;
		}else{
			AllManage.uicamStatic.enabled = true;
			AllManage.uicam2Static.enabled = true;		
		}
	}
}
