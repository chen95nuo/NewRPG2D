#pragma strict
enum ActiveType{
	level = 1,
	task = 2
}

enum MapType{
	zhucheng = 0,
	fuben = 1,
	jingjichang = 2,
	yewai = 3
}

enum UseMoneyType{
	Sell1 = 0,									//卖出物品1
	WashAttributePoints = 1,				//洗技能点						
	SaveBranch = 2,							//保存转职
	YesBuyCard = 3,							//副本翻牌
	YesDoCD = 4,								//重置铁毡冷却
	Sell = 5,										//卖出物品
	Buy = 6,										//买入物品
	YesOpenCangKu = 7,					//开启仓库格子
	CostGold = 8,								//花费金币
	OpenSpreeItemAsID = 9,				//打开礼包
	YesOpenPackage = 10,				//打开道具包
	YAddGold = 11,							//增加金币
	YAddBlood = 12,							//增加血石
	YesPVP8Times = 13,					//重置8人pvp
	YesPVPTimes = 14,						//重置pvp
	Resurrection2 = 15,						//复活2
	Resurrection3 = 16,						//复活3
	mmm31 = 17,								//内购1
	mmm32 = 18,								//内购2
	mmm33 = 19,								//内购3
	mmm34 = 20,								//内购4
	UpDateOneSkill = 21,					//升级技能
	UpDateOneSoulItemSkill = 22,		//升级魔魂技能
	ButtonBuildSoulAndDigest = 23,	//生产魔魂&灵魂精华
	NumDigestButtonsPlus = 24,			//开启灵魂精华包裹
	PlusBottle = 25,							//增加药水
	ApplyPickup1 = 26,						//拾取1
	ApplyPickup2 = 27,						//拾取2
	TaskDone = 28,							//完成任务
	SpendingReturns11 = 29,				//花费返回
	RaidsStart = 30,							//扫荡开始
	CostRaidsNow = 31,					//立即完成扫荡
	Cost = 32,									//花费
	UpLevelBottle = 33,						//升级药水等级
	PlusBottle1 = 34,							//增加药水1
	FullBottle = 35,							//补满药水
	YesReturn = 36,							//确认返回
	YesManufacture = 37,					//打开制造按钮
	YesSoul = 38,								//打开魔魂按钮
	YesGem = 39,								//打开宝石镶嵌按钮
	YesDuplicate = 40,						//确认副本
	YesResetPVP = 41,						//重置pvp
	yesCook = 42,								//打开烤鱼按钮
	YesBuy = 43,								//确认购买
	Getreward = 44,							//获得奖励
	doneCard = 45							//完成副本
}

enum NPCType{
	monster = 0,
	npc1 = 1,
	npc2 = 2,
	npc3 = 3
}
enum MonsterType{
	pro = 0,
	strong = 1,
	rogue = 2,
	magic = 3
}
	
enum MonsterLEVEL{
	RAID = 0,
	BIGBOSS = 1,
	BOSS = 2,
	Elite = 3,
	Monster = 4,
	PET = 5
}

class ButtonsActive{
	var activeType : ActiveType;
	var attr1 : int;
	var attr2 : String;
}

class PersonEquipment{
	var invType : SlotType;
	var inv : InventoryItem;
	var iValue : int = 0;
}

class MoneyClass{
	var mon : int;
	var blo : int;
	var type : UseMoneyType;
	var object : GameObject;
	var returnFunction : String;
}

class RealSoul{
	var name : String;
	var level : int;
	var quality : int;
	var attr : int; 
	var attrLevel : int;
	var skillLevel : int;
}

class GameNMInfo{
	var name : String;
	var npcType : NPCType;
	var NPCID : String;
	var MapID : String;
	var isTaskNPC : int;
	var npcTitle : String;
	var npcTalk : String;
	var monsterParent : String;
	var monsterParentID : String;
	var monsterTitle : String;
	var monsterMapID : int;
	var monsterLevel : MonsterLEVEL;
	var monsterType : MonsterType;
	var monsterSkill1 : int;
	var monsterSkill2 : int;
	var monsterSkill3 : int;
}

class GameSkill{
	var sName : String;
	var skillType : int;
	var startID : int;
	var middleID : int;
	var endID : int;
	var scope : int;
	var damageValue : int;
	var damageType : int;
	var buffID : int;
	var buffValue : int;
	var buffTime : int;
	var sType : int;
	var info : String;
	var batterNum : int;
	var fxobject : GameObject;
    var OpenR = false;
    var animation : AnimationClip;
    var CoolDown :int;
    var Cost :int;
    var skillTime :int;
}

class BagItemType{
	var inv : InventoryItem;
	var myType : SlotType;
}

class Song extends MonoBehaviour{
	function Awake(){
	
	}
	
	function Start() : IEnumerator{
	
	}
//function v10toX(n,m)
//{
//	m=String(m).replace(/ /gi,"");
//	if(m=="")
//		return "";
//	if(parseInt(m)!=m){
//		M("请输入整数！");
//	return ""
//}
//var t=""
//var a=ss.substr(0,n)
//while(m!=0)
//{
// var b=m%n
// t=a.charAt(b)+t
// m=(m-b)/n
//}
//return t
//}
//
//function vXto10(n,m)
//{
//	m=String(m).replace(/ /gi,"")
//	if(m=="")return ""
//	var a=ss.substr(0,n)
//	if(eval("m.replace(/["+a+"]/gi,'')")!=""){M("请输入"+n+"进制数！");return ""}
//	var t=0,c=1
//	for(var x=m.length-1;x>-1;x--)
//	{
//		t+=c*(a.indexOf(m.charAt(x)))
//		c*=n
//	}
//	return t
//}
//
//
//function vXtoY(n,m,y)
//{
//	a=vXto10(n*1,m)
//
//	if(a=="")
//		return ""
//		a=v10toX(y,a)
//	return a
//}
	
//	function OnLevelWasLoaded (level : int) {
//		if(level != 15 && level != 16 && level != 0 && level != 1){
//			Awake();
//			Start();
//		}
//	}
}
