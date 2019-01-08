#pragma strict
var Loc1 : UILocalize;
var Loc2 : UILocalize;
var Loc3 : UILocalize;

var returnObj : GameObject;
var returnFunctionN : String;
var returnFunctionY : String;
var label : UILabel;
var myObj : GameObject;
//var be : BlurEffect;
var isQR : boolean = false;
function Awake(){
//	if(isQR)
	AllManage.qrStatic = this;
}

function Update () {
	if(Time.time > ptimeTextButton && ZhuangBei){
	ZhuangBei.SetActiveRecursively(false);
	}
	}

function ShowQueRen(obj : GameObject , funcY : String , str : String){
	returnObj = obj;
	returnFunctionY = funcY;
	returnFunctionN = funcY;
	label.text = AllManage.AllMge.Loc.Get(str);
	myObj.SetActiveRecursively(true);
	//be.enabled = true;
}

function ShowQueRen1(obj : GameObject , funcY : String , str : String){
	returnObj = obj;
	returnFunctionY = funcY;
	returnFunctionN = funcY;
	myObj.SetActiveRecursively(true);
	label.text = str;
	//be.enabled = true;
}

function ShowQueRen(obj : GameObject , funcY : String ,funcN : String , str : String){
	returnObj = obj;
	returnFunctionY = funcY;
	returnFunctionN = funcN;
	label.text = AllManage.AllMge.Loc.Get(str);
	myObj.SetActiveRecursively(true);
	//be.enabled = true;
}


function ShowQueRen1(obj : GameObject , funcY : String ,funcN : String , str : String){
	returnObj = obj;
	returnFunctionY = funcY;
	returnFunctionN = funcN;
	myObj.SetActiveRecursively(true);
	label.text = str;
	//be.enabled = true;
}

var ZhuangBei : GameObject;
var labelZhuangBei : UILabel;
var SpriteZhuangBei : UISprite;
var SpriteZhuangBeiKuang : UISprite;
var ptimeTextButton : int = 0;
function ShowZhuangBei(obj : GameObject , funcY : String ,funcN : String , str : String ,str1 : String,ZBitemQuality : int){
	returnObj = obj;
	returnFunctionY = funcY;
	returnFunctionN = funcN;
	ZhuangBei.SetActiveRecursively(true);
	SpriteZhuangBei.spriteName = str;
	labelZhuangBei.text = str1;
	ptimeTextButton = Time.time + 12;
	
	var strItem : String;
	switch(ZBitemQuality){
	case 1:
			strItem = "yanse1";
	break;
	case 2:
			strItem = "yanse2";
	break;
	case 3:
			strItem = "yanse3";
	break;
	case 4:
			strItem = "yanse4";
	break;
	case 5:
			strItem = "yanse5";
	break;
	
	case 6:
			strItem = "yanse2";
	break;
	case 7:
			strItem = "yanse3";
	break;
	case 8:
			strItem = "yanse4";
	break;
	case 9:
			strItem = "yanse5";
	break;
	
	}
	
	SpriteZhuangBeiKuang.spriteName = strItem;
	//be.enabled = true;
}

function Yes(){
	returnObj.SendMessage(returnFunctionY , SendMessageOptions.DontRequireReceiver);
	myObj.SetActiveRecursively(false);
	MyObj2.SetActiveRecursively(false); 
	ZhuangBei.SetActiveRecursively(false);
	//be.enabled = false;
}

function No(){
	if(returnFunctionN != ""){
		returnObj.SendMessage(returnFunctionN , SendMessageOptions.DontRequireReceiver);		
		returnFunctionN = "";
	}
	myObj.SetActiveRecursively(false);
	MyObj2.SetActiveRecursively(false); 
	ZhuangBei.SetActiveRecursively(false);
	//be.enabled = false;
}

var MyObj2 : GameObject; 
var label2 : UILabel;
var LabelNexShow : UILabel;
function ShowBuyQueRen(obj : GameObject , funcY : String ,funcN : String , str : String){
	returnObj = obj;
	returnFunctionY = funcY;
	returnFunctionN = funcN;
	label2.text = AllManage.AllMge.Loc.Get(str);
	MyObj2.SetActiveRecursively(true); 
	//be.enabled = true;
	isUseBloodStone = false;
	SpriteUseBloodStone.spriteName = "UIM_Prompt_Off";	
}
function ShowBuyQueRen1(obj : GameObject , funcY : String ,funcN : String , str : String){
	this.ShowBuyQueRen1(obj, funcY, funcN, str, 0);
}

function ShowBuyQueRen1(obj : GameObject , funcY : String ,funcN : String , str : String,NextShowText : int){
	returnObj = obj;
	returnFunctionY = funcY;
	returnFunctionN = funcN;
	MyObj2.SetActiveRecursively(true); 
	if(LabelNexShow){
	if(NextShowText==1){
	LabelNexShow.text = AllManage.AllMge.Loc.Get("info1231");
	}else{
	LabelNexShow.text = AllManage.AllMge.Loc.Get("info1230");
	}
	}
	//be.enabled = true;
	isUseBloodStone = false;
	SpriteUseBloodStone.spriteName = "UIM_Prompt_Off";	
	label2.text = str;
}

var MyObj3 : GameObject; 
var label3 : UILabel;
var LabelInput : UILabel;
function ShowTimesQueRen(obj : GameObject , funcY : String ,funcN : String , str : String , times : int){
	LabelInput.text = times.ToString();
	returnObj = obj;
	returnFunctionY = funcY;
	returnFunctionN = funcN;
	label3.text = AllManage.AllMge.Loc.Get(str);
	MyObj3.SetActiveRecursively(true);
	//be.enabled = true;
}
function Yes3(){
	AllManage.mtwStatic.SaoDangTimes = parseInt(LabelInput.text);
	returnObj.SendMessage(returnFunctionY , SendMessageOptions.DontRequireReceiver);
	myObj.SetActiveRecursively(false);
	MyObj2.SetActiveRecursively(false); 
	MyObj3.SetActiveRecursively(false); 
	//be.enabled = false;
}

function No3(){
	if(returnFunctionN != ""){
		returnObj.SendMessage(returnFunctionN , SendMessageOptions.DontRequireReceiver);		
		returnFunctionN = "";
	}
	myObj.SetActiveRecursively(false);
	MyObj2.SetActiveRecursively(false); 
	MyObj3.SetActiveRecursively(false); 
	//be.enabled = false;
}

function CloseAll(){
	myObj.SetActiveRecursively(false);
	MyObj2.SetActiveRecursively(false); 
	MyObj3.SetActiveRecursively(false); 
}

var SpriteUseBloodStone : UISprite;
var isUseBloodStone : boolean = false;
function UseBloodStone(){
	if(!isUseBloodStone){
		isUseBloodStone = true;
		SpriteUseBloodStone.spriteName = "UIM_Prompt_On"; 
		PlayerPrefs.SetInt("ConsumerTip" , 0);
	}else{
		isUseBloodStone = false;
		SpriteUseBloodStone.spriteName = "UIM_Prompt_Off";	
		PlayerPrefs.SetInt("ConsumerTip" , 1);
	}
} 

var labelServer : UILabel;
var myObjServer : GameObject;
var returnServerY : String = "";
var returnServerN : String = "";
var returnServerObj : GameObject;
function ShowServerQueRen(obj : GameObject , funcY : String , str : String){
	returnServerObj = obj;
	returnServerY = funcY;
	returnServerN = funcY;
	labelServer.text = AllManage.AllMge.Loc.Get(str);
	myObjServer.SetActiveRecursively(true);
}

function YesServer(){
	returnServerObj.SendMessage(returnServerY , SendMessageOptions.DontRequireReceiver);
	myObjServer.SetActiveRecursively(false);
}

function NoServer(){
	if(returnServerN != ""){
		returnServerObj.SendMessage(returnServerN , SendMessageOptions.DontRequireReceiver);		
		returnServerN = "";
	}
	myObjServer.SetActiveRecursively(false);
}


var returnDuelObj : GameObject;
var returnDuelFunctionN : String;
var returnDuelFunctionY : String;
var labelDuel : UILabel;
var labelDuelName : UILabel;
var labelDuelLevel : UILabel;
var labelDuelPower : UILabel;
var SpriteDuelPlayer : UISprite;
var myDuelObj : GameObject;
function ShowDuelQueRen(obj : GameObject , funcY : String , funcN : String ,str : String , name : String , level : String , power : String , proID : int){
	returnDuelObj = obj;
	returnDuelFunctionY = funcY;
	returnDuelFunctionN = funcN;
	
	labelDuelName.text = name.ToString();
	labelDuelLevel.text = level;
	labelDuelPower.text = power;
	labelDuel.text = str;
	myDuelObj.SetActiveRecursively(true);
	switch(proID){
		case 1 : 
			SpriteDuelPlayer.spriteName = "head-zhanshi";
			break;
		case 2 : 
			SpriteDuelPlayer.spriteName = "head-youxia";
			break;
		case 3 : 
			SpriteDuelPlayer.spriteName = "head-fashi";
			break;
	}
	//be.enabled = true;
}

function YesDuel(){
	returnDuelObj.SendMessage(returnDuelFunctionY , SendMessageOptions.DontRequireReceiver);
	myDuelObj.SetActiveRecursively(false);
	MyObj2.SetActiveRecursively(false); 
	ZhuangBei.SetActiveRecursively(false);
	//be.enabled = false;
}

function NoDuel(){
	if(returnDuelFunctionN != ""){
		returnDuelObj.SendMessage(returnDuelFunctionN , SendMessageOptions.DontRequireReceiver);		
		returnDuelFunctionN = "";
	}
	myDuelObj.SetActiveRecursively(false);
	MyObj2.SetActiveRecursively(false); 
	ZhuangBei.SetActiveRecursively(false);
	//be.enabled = false;
}
