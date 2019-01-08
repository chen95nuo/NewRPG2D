#pragma strict
class TimeDaoJi extends Song{
var Loc1 : UILocalize;
var Loc2 : UILocalize;
var Loc3 : UILocalize;

var fsNU : UIFilledSprite; 
var LabelLeft : UILabel;
var LableRight : UILabel; 
var LableUp : UILabel;
var selectLater : boolean = false;
var parentTanKuang : GameObject;
var returnObj : GameObject;
var returnYes : String;
var returnOtherYes : String;
var returnNo : String;
var returnLater : boolean = false;
var returnTime : float;
var times : int = 0;
var canShowMeFirst : boolean = false;
function Start(){
		parentTanKuang.SetActiveRecursively(false);
	canShowMeFirst = false;
//	AllManage.timeDJStatic = this;
}

function Show(obj : GameObject , oYes : String , rYes : String  , rNo : String , time : float , str1 : String , str2 : String , str3 : String , haveLater : boolean){ 
//	print("111111111");
	canShowMeFirst = true;
	returnObj = obj;
	returnYes = rYes;
	returnNo = rNo;
	returnLater = haveLater;
	returnTime = time;
	returnOtherYes = oYes;
	LabelLeft.text = AllManage.AllMge.Loc.Get(str1);
	LableRight.text = AllManage.AllMge.Loc.Get(str2);
	LableUp.text = AllManage.AllMge.Loc.Get(str3);
	parentTanKuang.SetActiveRecursively(true);
	selectLater = false;
	var num : float = 0.0;
	times += 1;
	var myTimes : int = 0;
	myTimes = times;
	while(num < returnTime && !selectLater && myTimes == times){
		if(!parentTanKuang.active){
			parentTanKuang.SetActiveRecursively(true);
		}
		num += Time.deltaTime;
		fsNU.fillAmount = num / returnTime;
		yield;
	}
	if(!selectLater && myTimes == times && canShowMeFirst){
		parentTanKuang.SetActiveRecursively(false);
		obj.SendMessage(rNo , SendMessageOptions.DontRequireReceiver);
		times += 1;
	}
}

function Show(obj : GameObject , rYes : String  , rNo : String , time : float , str1 : String , str2 : String , str3 : String , haveLater : boolean){ 
//	print("222222");
	canShowMeFirst = true;
	returnObj = obj;
	returnYes = rYes;
	returnNo = rNo;
	returnLater = haveLater;
	returnTime = time;
	LabelLeft.text = AllManage.AllMge.Loc.Get(str1);
	LableRight.text = AllManage.AllMge.Loc.Get(str2);
	LableUp.text = AllManage.AllMge.Loc.Get(str3);
	parentTanKuang.SetActiveRecursively(true);
	selectLater = false;
	var num : float = 0.0;
	while(num < returnTime && !selectLater){
		if(!parentTanKuang.active){
			parentTanKuang.SetActiveRecursively(true);
		}
		num += Time.deltaTime;
		fsNU.fillAmount = num / returnTime;
		yield;
	}
	if(!selectLater && canShowMeFirst){
		parentTanKuang.SetActiveRecursively(false);
		obj.SendMessage(rYes , SendMessageOptions.DontRequireReceiver);
	}
}

var ButtonCanClose : boolean = false;
function NowDone(){
	if(! ButtonCanClose){
		parentTanKuang.SetActiveRecursively(false);	
	}
	selectLater = true;
	returnObj.SendMessage(returnYes , SendMessageOptions.DontRequireReceiver);
}

function LaterDone(){
	if(! ButtonCanClose){
		parentTanKuang.SetActiveRecursively(false);
	}
	if(!returnLater){
		selectLater = true;
		returnObj.SendMessage(returnNo , SendMessageOptions.DontRequireReceiver);
	}else{
		if(!selectLater){
			selectLater = true;
			yield WaitForSeconds(15);
			if(canShowMeFirst)
			Show(returnObj , returnYes , returnNo , returnTime , LabelLeft.text , LableRight.text , LableUp.text , returnLater);
		}
	}
}

function OtherNowDone(){
//	times += 1;
	returnObj.SendMessage(returnOtherYes , SendMessageOptions.DontRequireReceiver);
}
}