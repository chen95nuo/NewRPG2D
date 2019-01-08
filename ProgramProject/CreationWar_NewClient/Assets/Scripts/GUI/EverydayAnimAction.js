#pragma strict

function Start () {
	AllManage.everyDayAction = this;
}

function GoAsID(id : int){
	switch(id){
	    case CommonDefine.GoDungeon:
	        GoDungeon();
			break;
	    case CommonDefine.OpenTraining:
	        OpenTraining();
			break;
	    case CommonDefine.OpenStrengthen:
	        OpenStrengthen();
			break;
	    case CommonDefine.GoFishing:
	        GoFishing();
			break;
	    case CommonDefine.OpenShadowFiend:
	        OpenShadowFiend();
			break;
	    case CommonDefine.OpenStore:
	        OpenStore();
			break;
	    case CommonDefine.OpenRecharge:
	        OpenRecharge();
			break;
	    case CommonDefine.OpenAlchemy:
	        OpenAlchemy();
			break;
	    case CommonDefine.OpenSoul:
	        OpenSoul();
			break;
	    case CommonDefine.OpenActivities:
	        OpenActivities();
			break;
	    case CommonDefine.OpenPVPone:
	        OpenPVPone();
			break;
	    case CommonDefine.GoCooking:
	        GoCooking();
			break;
	    case CommonDefine.OpenBattle:
	        OpenBattle();
			break;

	}
}

/// <summary>
///	打开战场
/// </summary>
function OpenBattle(){
	AllManage.UICLStatic.BattleMoveOn();
}

/// <summary>
///	go到传送门
/// </summary>
function GoDungeon(){
	AllManage.mtwStatic.Gochuansong();
	AllManage.UIALLPCStatic.show0();
}

/// <summary>
///	打开训练面板
/// </summary>
function OpenTraining(){
	AllManage.UICLStatic.XunLianOverMoveOn();
}

/// <summary>
///	打开强化装备面板
/// </summary>
function OpenStrengthen(){
	AllManage.UIALLPCStatic.show2();
}

/// <summary>
///	自动寻路到附近鱼点
/// </summary>
var stones : TriggerStone[];
function GoFishing(){
	var pos : Vector3 = Vector3.zero;
	if(Application.loadedLevelName != "Map151"){
		stones = FindObjectsOfType(TriggerStone);
		for(var i=0; i<stones.length; i++){
			if(pos == Vector3.zero && stones[i].myType == ConsumablesType.Fish){
				pos = stones[i].gameObject.transform.position;
			}
		}
	}else{
		if(TriggerLoadLevel.fishPlace != null)
			pos = TriggerLoadLevel.fishPlace.position;
	}
	if(pos != Vector3.zero){
		PlayerStatus.MainCharacter.gameObject.SendMessage("UdateAgentTargets" , pos , SendMessageOptions.DontRequireReceiver);
	}
}

/// <summary>
///	打开烤鱼面板
/// </summary>
function GoCooking(){
	AllManage.UICLStatic.cookReslist();
}

/// <summary>
///	打开挑战影魔
/// </summary>
function OpenShadowFiend(){
	AllManage.UICLStatic.yingmoOverMoveOn();
}

/// <summary>
///	打开商城面板
/// </summary>
function OpenStore(){
	AllManage.UICLStatic.StoreOpenMoveOn();
}

/// <summary>
///	打开充值面板
/// </summary>
function OpenRecharge(){
	AllManage.UICLStatic.StoreMoveOn();
}

/// <summary>
///	打开炼金面板
/// </summary>
function OpenAlchemy(){
    //AllManage.UIALLPCStatic.show28();

    if(DailyBenefitsPanelSelect.My)
    {
        DailyBenefitsPanelSelect.My.btnMakeGold.OnClick();
    }
}

/// <summary>
///	打开炼魂面板
/// </summary>
function OpenSoul(){
	AllManage.UICLStatic.kaiQiSoul();
}

/// <summary>
///	打开活动面板
/// </summary>
function OpenActivities(){
	AllManage.UIALLPCStatic.show42();
}

/// <summary>
///	打开头像菜单PVP自动排队
/// </summary>
function OpenPVPone(){
	AllManage.UICLStatic.PVPMoveOn();
}
function OpenBattleone(){
	AllManage.UICLStatic.BattleMoveOn();
}
