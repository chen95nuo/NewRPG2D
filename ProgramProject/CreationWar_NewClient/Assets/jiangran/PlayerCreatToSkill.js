#pragma strict


//
function MasterClickGeneral()
{
	Skill(3,2);
}

function MasterClickSkill1()
{
	Skill(3,12);
}

function MasterClickSkill2()
{
	Skill(3,7);
}
//迅影者
function RobberClickGeneral()
{
	Skill(2,1);
}

function RobberClickSkill1()
{
	Skill(2,10);
}

function RobberClickSkill2()
{
	Skill(2,5);
}

//战士
function SoldierClickGeneral()
{
	Skill(1,1);
}

function SoldierClickSkill1()
{
	Skill(1,7);
}

function SoldierClickSkill2()
{
	Skill(1,11);
}


function IsBusyClick(){
	busy = false;
}


//var objects : int[];
var aSkillSoldier : ActiveSkill;
var aSkillRanger : ActiveSkill;
var aSkillMaster : ActiveSkill;
//////
//技能id , ClickSkillID不能传1 

private var busy = false;
function Skill(ClickPro : int ,ClickSkillID : int )
{
if(busy)
return;
busy = true;
		switch(ClickPro){
			case 1:
				aSkillSoldier.AttackSkill(ClickSkillID);
				break;
			case 2:
				aSkillRanger.AttackSkill(ClickSkillID);
				break;
			case 3:
				aSkillMaster.AttackSkill(ClickSkillID);
				break;
		}
yield WaitForSeconds(2);
busy = false;
		
}