#pragma strict

var ats : Attack_simple;
function Start () {
	if(ats == null && PlayerStatus.MainCharacter ){
		ats = PlayerStatus.MainCharacter.gameObject.GetComponent(Attack_simple);
	}
}

var ptime : float = 30;
var stime : float = 0;
var SpriteQuan : UISprite;
function Update(){
	if(ptime < 30 && ptime >= 0){
		ptime += Time.deltaTime;
		iconCD.fillAmount = ptime / 30;
		if(ptime > 30){
			ptime = 30;
		}
	}
	if(Time.time > stime && SpriteQuan){
		stime = Time.time + 0.2;
		if(ptime >= 10){
			if( !SpriteQuan.enabled){
				SpriteQuan.enabled = true;
			}
		}else{
			if( SpriteQuan.enabled){
				SpriteQuan.enabled = false;
			}			
		}
	}
//	ptime += Time.deltaTime;
//	//print(ptime + " == " + Time.time);
}

//var skC : SkillControl;
//var canDu : boolean = false;
function OnClick(){
//	if(ats == null && PlayerStatus.MainCharacter ){
//		ats = PlayerStatus.MainCharacter.gameObject.GetComponent(Attack_simple);
//	}
	if(AllManage.SkillCLStatic.ASkill && ptime > 10 && !AllManage.SkillCLStatic.ASkill.busy){
		ptime -= 10;
//		canDu = true;
		AllManage.SkillCLStatic.jump();
//		SkillCoolDown(ats.JumpCD);
	}
}

var iconCD : UIFilledSprite;
function SkillCoolDown(o : float){
//	//print("o == " + o);
	var cd : int = Time.time + o;
	iconCD.fillAmount = 1;
	while(iconCD.fillAmount > 0){
		iconCD.fillAmount -= 1.0 / o * Time.deltaTime;
		yield;
	}
//	canDu = false;
}
