#pragma strict

function Start () {

}

var btnBuild 		: 	UIButton;
var btnTraning 		: 	UIButton;
var btnSoul 		: 	UIButton;
var btnGem 			: 	UIButton;
var btnFrist 		: 	UIButton;

var btnTreasure		: 	UIButton;
var btnMagic 		: 	UIButton;
var btnActivity 	: 	UIButton;
var btnBattle		: 	UIButton;
var btnArenas 		: 	UIButton;


function OnEnable() {
	ShowMyBtn();
}

function ShowMyBtn()
{

		//强化
		if(AllManage.UICLStatic.canOpenViewAsTaskID("13")){
			btnBuild.isEnabled = true;
		}else{
			btnBuild.isEnabled = false;
		}
		
		//训练O
		if(AllManage.UICLStatic.canOpenViewAsTaskID("62")){
			btnTraning.isEnabled = true;
		}else{
			btnTraning.isEnabled = false;
		}
		
		//炼魂
		if(AllManage.UICLStatic.canOpenViewAsTaskID("122")){
			btnSoul.isEnabled = true;
		}else{
			btnSoul.isEnabled = false;
		}
		
		//宝石
		if(AllManage.UICLStatic.canOpenViewAsTaskID("127")){
			btnGem.isEnabled = true;
		}else{
			btnGem.isEnabled = false;
		}
		
		//首充
		if(parseInt(BtnGameManager.yt[0]["IsChargeReward"].YuanColumnText)==0){
			btnFrist.isEnabled = true;
		}else{
			btnFrist.isEnabled = false;
		}
		
		
		//宝藏
		if(parseInt(AllManage.psStatic.Level) >=10){
		btnTreasure.isEnabled  = true ;
		}else{
		btnTreasure.isEnabled  = false ;
		}
		
		//困魔塔
		if(parseInt(AllManage.psStatic.Level)>=15){
		btnMagic.isEnabled  = true ;
		}else{
		btnMagic.isEnabled  = false ;
		}
		
		//活动
		if(parseInt(AllManage.psStatic.Level)>=10){
		btnActivity.isEnabled  = true ;
		}else{
		btnActivity.isEnabled  = false ;
		}
		
		//战场
		if(parseInt(AllManage.psStatic.Level)>=30){
		btnBattle.isEnabled  = true ;
		}else{
		btnBattle.isEnabled  = false ;
		}
		
		//角斗场
		if(parseInt(AllManage.psStatic.Level)>=10){
		btnArenas.isEnabled  = true ;
		}else{
		btnArenas.isEnabled  = false ;
		}
		
}


//强化
function ClickBuild()
{
		AllManage.UIALLPCStatic.show2();
}
//训练
function ClickTraning()
{
		AllManage.UICLStatic.XunLianOverMoveOnNew();
}
//炼魂
function ClickSoul()
{
		AllManage.UIALLPCStatic.show17();
}
//宝石
function ClickGem()
{
		AllManage.UIALLPCStatic.show18();
}
//首充
function ClickFrist()
{
		AllManage.UIALLPCStatic.show39();
}

//down

//宝藏
function ClickTreasure()
{
		AllManage.UIALLPCStatic.show28();
}

//困魔塔
function ClickMagic()
{
		AllManage.UIALLPCStatic.show47();
}
//活动
function ClickActivity()
{
		AllManage.UIALLPCStatic.show42();
}
//战场 需宋宏澍提供接口
function ClickBattle()
{
		AllManage.everyDayAction.OpenBattleone();
}
//角斗场 需宋宏澍提供接口
function ClickArenas()
{
		AllManage.everyDayAction.OpenPVPone();
}