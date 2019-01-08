#pragma strict

function Awake(){
//	uiallpcl = AllManage.UIALLPCStatic;
}

function Start(){
	ShowUIValue();
}
function OnEnable(){
	ShowUIValue();
}

function Update () {

}

var VipTip : UISprite[];
function Select0(){
	SelectOneVipTipAsID(0);
}
function Select1(){
	SelectOneVipTipAsID(1);
}
function Select2(){
	SelectOneVipTipAsID(2);
}
function Select3(){
	SelectOneVipTipAsID(3);
}
function Select4(){
	SelectOneVipTipAsID(4);
}
function Select5(){
	SelectOneVipTipAsID(5);
}
function Select6(){
	SelectOneVipTipAsID(6);
}
function Select7(){
	SelectOneVipTipAsID(7);
}
function Select8(){
	SelectOneVipTipAsID(8);
}

function TriggerOnAsID(id : String){
	switch(id){
		case "VipItem - 1": Select0(); break;
		case "VipItem - 2": Select1(); break;
		case "VipItem - 3": Select2(); break;
		case "VipItem - 4": Select3(); break;
		case "VipItem - 5": Select4(); break;
		case "VipItem - 6": Select5(); break;
		case "VipItem - 7": Select6(); break;
		case "VipItem - 8": Select7(); break;
		case "VipItem - 9": Select8(); break;
	}
}

var useVipText : String[];
var LabelVIP : UILabel;
function SelectOneVipTipAsID(Sid : int){
	var i : int = 0;
	for(i=0; i<VipTip.length; i++){
		if(Sid == i){
			VipTip[i].spriteName = "UIM_Prompt_On";
		}else{
			VipTip[i].spriteName = "UIM_Prompt_Off";			
		}
	}
}

var LabelLevel : UILabel;
var LabelServingMoney : UILabel;
var SliderServingMoney : UISlider;
var trasRecharge : Transform;
function Cost(){
	//AllManage.UICLStatic.Cost();	
    PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StoreMoveOn", SendMessageOptions.DontRequireReceiver);
}

var LabelLevel2 : UILabel;
function ShowUIValue(){
	
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages104");
			AllManage.AllMge.Keys.Add(ps.VIPLevel + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelLevel);
//	LabelLevel.text = "您当前的VIP等级:"+ps.VIPLevel;
	ps.maxServingMoney = getNowServingMoney(ps.VIPLevel);
	LabelServingMoney.text = ps.ServingMoney + "/" + ps.maxServingMoney;
	var f1 : float;
	var f2 : float;
	f1 = ps.ServingMoney;
	f2 = ps.maxServingMoney;
	LabelLevel2.text = ps.VIPLevel + "";
	SliderServingMoney.sliderValue = f1 / f2;
}
function getNowServingMoney(lv : int) : float{
	var max : int = 0;
	switch(lv){
		case 0 : max = 10; break;
		case 1 : max = 50; break;
		case 2 : max = 150; break;
		case 3 : max = 500; break;
		case 4 : max = 1000; break;
		case 5 : max = 2500; break;
		case 6 : max = 5000; break;
		case 7 : max = 10000; break;
		case 8 : max = 20000; break;			
	}
	return max;
} 

//var uiallpcl : UIAllPanelControl;
function show0(){
	trasRecharge.localPosition.y = 3000;
	AllManage.UIALLPCStatic.show0();
}

private var ps : PlayerStatus;
function UpDateVIP(){
}