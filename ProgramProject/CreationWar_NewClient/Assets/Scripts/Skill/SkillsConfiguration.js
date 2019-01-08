#pragma strict

static var PlayerSkill : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("ytPlayerSkill1","id");
function Start () {
	PlayerSkill = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerSkill;
	ConfigurationSkills();
}

var aSkillSoldier : ActiveSkill;
var aSkillRanger : ActiveSkill;
var aSkillMaster : ActiveSkill;
function ConfigurationSkills(){
	aSkillSoldier.SetConfigurationSkills(PlayerSkill , "1");
	aSkillRanger.SetConfigurationSkills(PlayerSkill , "2");
	aSkillMaster.SetConfigurationSkills(PlayerSkill , "3");
	yield;
//	aSkillSoldierNew.SetConfigurationSkills(PlayerSkill , "1");
//	aSkillRangerNew.SetConfigurationSkills(PlayerSkill , "2");
//	aSkillMasterNew.SetConfigurationSkills(PlayerSkill , "3");
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	aSkillSoldierTiShen.SetConfigurationSkills(PlayerSkill , "1");
	aSkillRangerTiShen.SetConfigurationSkills(PlayerSkill , "2");
	aSkillMasterTiShen.SetConfigurationSkills(PlayerSkill , "3");
}

var aSkillSoldierNew : ActiveSkill;
var aSkillRangerNew : ActiveSkill;
var aSkillMasterNew : ActiveSkill;


var aSkillSoldierTiShen : ActiveSkill;
var aSkillRangerTiShen : ActiveSkill;
var aSkillMasterTiShen : ActiveSkill;

