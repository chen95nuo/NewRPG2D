#pragma strict
var LabelPVPStone : UILabel;
var LabelPVPStoneFall : UILabel;
var SpriteOutOf : UISprite;
var cards : CardItem[];
var parCards : Transform; 
var ps : PlayerStatus;
var ObjParent : GameObject;
var ObjBtns : GameObject;
function Awake(){
	AllManage.CardCLStatic = this;
}

var spriteTouXiang : UISprite;
function Start () {
	var str : String;
	str = AllManage.InvclStatic.yt.Rows[0]["ProID"].YuanColumnText;
	switch(str){
		case "1" :
			spriteTouXiang.spriteName = "tou1";
			break;
		case "2" :
			spriteTouXiang.spriteName = "tou2";
			break;
		case "3" :
			spriteTouXiang.spriteName = "tou3";
			break;
	}
}

function OnEnable(){
			if(LabelPVPStoneFall&&SpriteOutOf)
			{
			if(LabelPVPStoneFall.text!=""){
			SpriteOutOf.enabled = true ; 
			}else{
			SpriteOutOf.enabled = false ; 
			}
			}
}

var invs : InventoryItem[];
var posCard : float[];
var moveSpeed : int = 0;
var cube : Transform;
var tweenParCard : TweenPosition;
var tweenCards : TweenPosition[];
var freeTimes : int = 0; 
var thisTimesCost : int = 0; 
var j : int = 0; 
var LabelFreeTimes : UILabel;
var LabelthisTimesCost : UILabel[];
var scaleParCard : GameObject;
var canCube : boolean = false;

var  EixtReward : ParticleEmitter;
function GoShowCards(ivs : InventoryItem[] , fTimes : int){
//		for(j=0;j<LabelthisTimesCost.length;j++){
//		LabelthisTimesCost[j].gameObject.transform.localPosition.y = 3000;	
//	}
//	
	if(EixtReward){
	 EixtReward.Emit();
	 }
	if(returnLevelButton&&UIControl.mapType != MapType.zhucheng){
	returnLevelButton.localPosition.y = 0;
	}
	freeTimes = fTimes; 
	thisTimesCost = Mathf.Clamp(parseInt(DungeonControl.level) / 5 , 5 , 50);
	thisTimesCost = thisTimesCost + thisTimesCost*(clickTimes + 1) / 5; 
	if(freeTimes > 0){
		for(j=0;j<LabelthisTimesCost.length;j++){
		LabelthisTimesCost[j].text = AllManage.AllMge.Loc.Get("info559") + freeTimes + AllManage.AllMge.Loc.Get("info558");	
	}
	}else{
		for(j=0;j<LabelthisTimesCost.length;j++){
		LabelthisTimesCost[j].text = AllManage.AllMge.Loc.Get("info560") + thisTimesCost + AllManage.AllMge.Loc.Get("info297");			
	}
	}
//	LabelFreeTimes.text = AllManage.AllMge.Loc.Get("info559") + freeTimes;  
	
	clickTimes = 0;
	cube.localPosition.y = 0;
	canCube = false;
	invs = ivs;
	var i : int = 0;
	var iTime : int = 0;
	for(i=0; i<cards.length ; i++){
		posCard[i] = cards[i].gameObject.transform.localPosition.x;
	}
	tweenParCard.Play(true);
	for(i=0; i<cards.length ; i++){
		cards[i].SetInv(invs[i]);
		cards[i].canClick = true;
	}
	yield WaitForSeconds(3);
	for(i=0; i<cards.length ; i++){
		tweenCards[i].Play(true);
	}
	yield WaitForSeconds(0.2); 
	scaleParCard.SendMessage("Play" , true , SendMessageOptions.DontRequireReceiver);
	yield WaitForSeconds(0.2); 
	for(i=0; i<cards.length ; i++){
		cards[i].trunOff();
	}
	yield WaitForSeconds(0.2); 	
	for(i=0; i<cards.length ; i++){
		tweenCards[i].Play(false);
	}
	yield WaitForSeconds(0.2); 	
	cube.localPosition.y = 3000; 
	canCube = true;
}

var clickTimes : int = 0;
var clickNum  : int = 0;
var rewardID : int = 0;
var getInv : InventoryItem; 
var nowCard : CardItem;
var nowBagIt : BagItem;
var returnLevelButton : Transform;
function ClickButton(card : CardItem , bagit : BagItem){  
	if(returnLevelButton && UIControl.mapType != MapType.zhucheng)
	returnLevelButton.localPosition.y = 0;
	nowBagIt = bagit; 
	nowCard = card;
//	//print("2222"+ freeTimes);
	if(freeTimes > 0)
		freeTimes -= 1;
//		RealOpenCard();
//	}else{
		if(clickTimes != 0){
			if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1){
				AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsOpenCard , AllManage.dungclStatic.NowMapLevel , clickTimes , "" , gameObject , "YesBuyCardTips");
			}else{
				YesBuyCard();			
			}
		}
		else{
			YesBuyCard();
		}
//	}
}
	function YesBuyCardTips(objs : Object[]){
		if(objs[2] != 0 ){
			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesBuyCard" , "" , AllManage.AllMge.Loc.Get("info287")+ objs[2] + AllManage.AllMge.Loc.Get("info297") +"？");					
		}else
		if(objs[1] != 0 ){
			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesBuyCard" , "" , AllManage.AllMge.Loc.Get("info287")+ objs[1] + AllManage.AllMge.Loc.Get("info335") +"？");					
		}
	}

function YesBuyCard(){
//	AllManage.tsStatic.RefreshBaffleOn();
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().OpenCard());
//	InRoom.GetInRoomInstantiate().OpenCard();
//	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
//		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
//	}
//	AllManage.AllMge.UseMoney(0 , thisTimesCost , UseMoneyType.YesBuyCard , gameObject , "RealOpenCard");
//	if(ps.UseMoney(0 , thisTimesCost)){
//		RealOpenCard();
//	}
}

function RealOpenCard(itemID : String){
	InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.DuplicateFlop).ToString());
//	rewardID = 0;
//	getRewardIDAsTimes(clickTimes);
	clickTimes += 1; 
	clickNum += 1; 
	
	getInv = AllResources.InvmakerStatic.GetItemInfo(itemID , getInv);
//	var arr : Array;
//	arr = new Array(invs.length);
//	var i : int = 0;
//	for(i=0; i<arr.length; i++){
//		arr[i] = invs[i];
//	}
//	arr.RemoveAt(rewardID);
//	invs = new Array(arr.Count);
//	for(i=0; i<arr.length; i++){
//		invs[i] = arr[i];
//	}
	nowCard.trunOn();
	nowCard.canClick = false;
	nowBagIt.OtherYiChu();
	nowBagIt.SetInv(getInv); 
//	AllManage.InvclStatic.AddBagItem(getInv);
	thisTimesCost = Mathf.Clamp(parseInt(DungeonControl.level) / 5 , 5 , 50);
	thisTimesCost = thisTimesCost + thisTimesCost*(clickTimes + 1) / 5; 
	if(freeTimes > 0){
		for(j=0;j<LabelthisTimesCost.length;j++){
		LabelthisTimesCost[j].text = AllManage.AllMge.Loc.Get("info559") + freeTimes + AllManage.AllMge.Loc.Get("info558");	
	}
	}else{
		AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsOpenCard , AllManage.dungclStatic.NowMapLevel , clickTimes , "" , gameObject , "YesBuyCardTipsCard");
	}
	if(clickNum>=5)
	{
		for(j=0;j<LabelthisTimesCost.length;j++){
		LabelthisTimesCost[j].text = "";
		}			
	}
	
//	LabelFreeTimes.text = AllManage.AllMge.Loc.Get("info559") + freeTimes; 
}

function YesBuyCardTipsCard(objs : Object[]){
		if(objs[2] != 0 ){
			for(j=0;j<LabelthisTimesCost.length;j++){
				LabelthisTimesCost[j].text = AllManage.AllMge.Loc.Get("info560") + objs[2] + AllManage.AllMge.Loc.Get("info297");			
			}
//			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesBuyCard" , "" , AllManage.AllMge.Loc.Get("info287")+ objs[2] + AllManage.AllMge.Loc.Get("info297") +"？");					
		}else
		if(objs[1] != 0 ){
			for(j=0;j<LabelthisTimesCost.length;j++){
				LabelthisTimesCost[j].text = AllManage.AllMge.Loc.Get("info560") + objs[1] + AllManage.AllMge.Loc.Get("info335");			
			}
//			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesBuyCard" , "" , AllManage.AllMge.Loc.Get("info287")+ objs[1] + AllManage.AllMge.Loc.Get("info335") +"？");					
		}

}

var Rans : int[];
function getRewardIDAsTimes(times : int){
	if(clickTimes >=4){
		return;
	}
	var random : int;
	random = Random.Range(0,1000);
//	//print(random);
	Rans = new Array(4 - clickTimes);
	switch(clickTimes){
		case 0:
			Rans[0] = 998;
			Rans[1] = 989;
			Rans[2] = 899;
			Rans[3] = 500;
			break;
		case 1:
			Rans[0] = 989;
			Rans[1] = 899;
			Rans[2] = 500;
			break;
		case 2:
			Rans[0] = 899;
			Rans[1] = 500;
			break;
		case 3:
			Rans[0] = 500;
			break;
		case 4:
			break;
	}
	rewardID = GetRandom(Rans , random);
}

function GetRandom(rans : int[] , ran : int){
	var id : int = 0;
	for(var i=0; i<rans.length; i ++){
		if(ran > rans[i]){
			id = rans.length - i;
			return id;
		}
	}
	return id;
}
