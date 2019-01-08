#pragma strict

function Awake(){
	dungcl = AllManage.dungclStatic;
//	invMaker = AllResources.InvmakerStatic;
	mtw = AllManage.mtwStatic;
}

var PlayerTask : MainPlayerStatus;
var tncItem : TaskNowControlItem[];
var TweenNowTask : TweenPosition;
private var dungcl : DungeonControl;
var wuneirong : GameObject;

var ebItem : TaskNowControlItem;
var invItemArray : TaskNowControlItem[];
var invParent : Transform; 
var GID : UIGrid;
function SetPlayerTask(p : MainPlayerStatus){
	infoObj.gameObject.SetActiveRecursively(false);
	var i : int = 0;
	PlayerTask = p;
	yield;
	yield;
	yield;
//	for(i=0; i<tncItem.length; i++){
//		
//		tncItem[i].gameObject.SetActiveRecursively(false);
//		tncItem[i].gameObject.transform.localPosition.x = 1000;
//		tncItem[i].pTask = null;
//	}
	invClear();
//	print(PlayerTask.nowTask.length + " == PlayerTask.nowTask.length");
	for(i=0; i<PlayerTask.nowTask.length; i++){
//		print(PlayerTask.nowTask[i] + " == PlayerTask.nowTask[i]");
		var useEBI : TaskNowControlItem;
		if(PlayerTask.nowTask[i] != null){
//			tncItem[i].gameObject.transform.localPosition.x = -200;
//			tncItem[i].gameObject.SetActiveRecursively(true);
//			tncItem[i].ShowNowTaskItem(PlayerTask.nowTask[i]);
			wuneirong.transform.localPosition.y = 3000;
			var Obj : GameObject = Instantiate(ebItem.gameObject); 
			Obj.transform.parent = invParent;
			Obj.transform.localPosition.z = 0;
			useEBI = Obj.GetComponent(TaskNowControlItem);
			useEBI.ShowNowTaskItem(PlayerTask.nowTask[i]);
			useEBI.TaskNowCL = this;
			useEBI.id = i;
			addInvItem(useEBI);			
//			print(useEBI + " == useEBI");			
		}
		if(useEBI && i == 0){
			useEBI.FindWay();
		}
	}
	yield;
	GID.repositionNow = true;
	
//	for(i=0; i<PlayerTask.nowTask.length; i++){
//		if(tncItem[i].gameObject.active){
//			tncItem[i].FindWay();
//			return;
//		}
//	}
}

function resetList(){
//	SetEqupmentList("");
}

function SelectOneInv(ebi : TaskNowControlItem){
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
		if(invItemArray[i] == ebi){  
			invItemArray[i].SelectMe();
		}else{
			invItemArray[i].DontSelectMe();		
		}
	}
} 

function addInvItem(ebi : TaskNowControlItem){
	var use : TaskNowControlItem[]; 
	use = invItemArray; 
	invItemArray = new Array(invItemArray.length + 1);
	for(var i=0; i<(invItemArray.length - 1); i++){
		 invItemArray[i] = use[i];
	} 
	invItemArray[invItemArray.length - 1] = ebi;
}

function invClear(){
	for(var i=0; i<invItemArray.length; i++){
		if(invItemArray[i]){
			Destroy(invItemArray[i].gameObject);
		}
	}
	invItemArray = new Array(0);
}

function TweenBack(){
	var i : int = 0;
	for(i=0; i<tncItem.length; i++){
		tncItem[i].pTask = null;
	}
//	TweenNowTask.Play(false); 
	infoObj.gameObject.SetActiveRecursively(false);
	
//	for(i=0; i<SpriteBianKuang.length; i++){
//		SpriteBianKuang[i].enabled = false;		
//	}
	AllManage.UIALLPCStatic.show0();
}

var LabelName : UILabel;
var LabelInfo : UILabel;
var LabelNPC1 : UILabel;
var LabelNPC2 : UILabel;
var LabelReward : UILabel;
var LabelGold : UILabel;
var LabelExp : UILabel;
var LabelBlood : UILabel;
var LableJinDu : UILabel;

var SpriteExp : UISprite;
var invSprite : UISprite;
var ObjPanel  : UIPanel; 
var QualitySprite : UISprite;
var inv : InventoryItem = null; 
var tdi : TaskDescriptionItem; 
//var invMaker : Inventorymaker;  
var infoObj : GameObject; 
var pTask : PrivateTask;
var SpriteBianKuang : UISprite[];
var objItem : GameObject;
var objGold : GameObject;
var objExp : GameObject;
var objRank : GameObject;
function ShowTaskInfo(p : PrivateTask , id : int){ 
	pTask = p;
//	//print(p+"====PrivateTask");
	var useInt : int = 0;
	useInt = p.doneNum;
	if(useInt > p.task.doneNum){
		 useInt =  p.task.doneNum;
	}
	LableJinDu.text =   "("+UIControl.taskLeixingStrs[pTask.task.leixing] + ")" + pTask.task.ComplateInfo + " "+ useInt + "/" + p.task.doneNum;
	infoObj.gameObject.SetActiveRecursively(true);
	LabelName.text =  "("+UIControl.taskLeixingStrs[pTask.task.leixing] + ")" + p.task.taskName;
	LabelInfo.text = p.task.reward.taskDescription;
	LabelNPC1.text = dungcl.GetNPCNameAsbianhao(p.task.mainNPC.id);

	if(p.task.taskType == MainTaskType.duihua){
		LabelNPC2.text = dungcl.GetNPCNameAsID(parseInt(p.task.doneType.Substring(0,4)).ToString());	
	}else{	
		LabelNPC2.text = dungcl.GetNPCNameAsbianhao(p.task.mainNPC.id);
	}
	if(p.task.reward.gold != 0){
		LabelGold.text = p.task.reward.gold.ToString();	
		SpriteExp.atlas = AllResources.ar.GetTaskRewardAtlasAsType(p.task.reward.rewardExpType);
		SpriteExp.spriteName = AllResources.ar.GetTaskRewardIconAsType(p.task.reward.rewardExpType);
		SpriteExp.width = AllResources.ar.GetTaskRewardRectAsType(p.task.reward.rewardExpType).x;
		SpriteExp.height = AllResources.ar.GetTaskRewardRectAsType(p.task.reward.rewardExpType).y;
	}else{
		objGold.SetActiveRecursively(false);
	}
	if(p.task.reward.exp != 0){
		LabelExp.text = p.task.reward.exp.ToString();	
	}else{
		objExp.SetActiveRecursively(false);
	}
	if(p.task.reward.rank != 0){
		LabelBlood.text = p.task.reward.rank.ToString();	
	}else{
		objRank.SetActiveRecursively(false);
	}

	if(p.task.reward.itemId != ""){
		inv = AllResources.InvmakerStatic.GetItemInfo(p.task.reward.itemId,inv); 
		//print(p.task.reward.itemId + " == p.task.reward.itemId");
		if(inv != null){
			invSprite.enabled = true; 
			ObjPanel.enabled = true;
			invSprite.spriteName = inv.atlasStr;
			if(QualitySprite){
			QualitySprite.enabled = true;
			QualitySprite.spriteName ="yanse"+ inv.itemQuality.ToString();
			}
			tdi.inv = inv;	
			tdi.ShowLabelName();
		}else{
			objItem.SetActiveRecursively(false);
			inv = null;
			invSprite.enabled = false; 
			ObjPanel.enabled = false;
			if(QualitySprite){
			QualitySprite.enabled = false;
			}
		}
	}else{
		objItem.SetActiveRecursively(false);
		inv = null;
		invSprite.enabled = false; 	
		ObjPanel.enabled = false;
		if(QualitySprite){
		QualitySprite.enabled = false;
		}
	}
//	for(var i=0; i<SpriteBianKuang.length; i++){
//		if(i == id){
//			SpriteBianKuang[i].enabled = true;
//		}else{
//			SpriteBianKuang[i].enabled = false;		
//		}
//	}
}

function fangqi(){
	mtw.FangQiAs(pTask.task);
	AllManage.UIALLPCStatic.show0();
}

var rewardObj1 : GameObject;
var rewardObj2 : GameObject;
var rewardObj3 : GameObject;
var rewardObj4 : GameObject;
function ShowTaskInfooo(p : MainTask){ 
	infoObj.gameObject.SetActiveRecursively(true);
	LabelName.text = "("+UIControl.taskLeixingStrs[p.leixing] + ")" + p.taskName;
	LabelInfo.text = p.reward.taskDescription;
	LabelNPC1.text = dungcl.GetNPCNameAsbianhao(p.mainNPC.id);
	if(p.taskType == MainTaskType.duihua){
//		LabelNPC2.text = dungcl.GetNPCNameAsbianhao(p.needId);
		LabelNPC2.text = dungcl.GetNPCNameAsID(parseInt(p.doneType.Substring(0,4)).ToString());	
	}else{	
		LabelNPC2.text = dungcl.GetNPCNameAsbianhao(p.mainNPC.id);
	}
	if(p.reward.gold <= 0){
		if(rewardObj1)
		rewardObj1.SetActiveRecursively(false);
	}
	if(p.reward.exp <= 0){
		if(rewardObj2)
		rewardObj2.SetActiveRecursively(false);
	}
	if(p.reward.rank <= 0){
		if(rewardObj4)
		rewardObj4.SetActiveRecursively(false);
	}
	LabelGold.text = p.reward.gold.ToString();
	LabelExp.text = p.reward.exp.ToString();
		SpriteExp.atlas = AllResources.ar.GetTaskRewardAtlasAsType(p.reward.rewardExpType);
		SpriteExp.spriteName = AllResources.ar.GetTaskRewardIconAsType(p.reward.rewardExpType);
		SpriteExp.width = AllResources.ar.GetTaskRewardRectAsType(p.reward.rewardExpType).x;
		SpriteExp.height = AllResources.ar.GetTaskRewardRectAsType(p.reward.rewardExpType).y;
	LabelBlood.text = p.reward.rank.ToString();

	if(p.reward.itemId != ""){
//		//print(p.reward.itemId + " =================");
		inv = AllResources.InvmakerStatic.GetItemInfo(p.reward.itemId,inv); 
		invSprite.enabled = true; 
		invSprite.spriteName = inv.atlasStr;
		if(QualitySprite){
		QualitySprite.enabled = true;
		QualitySprite.spriteName ="yanse"+ inv.itemQuality.ToString();
		}
		tdi.inv = inv;		
		tdi.ShowLabelName();
	}else{
		inv = null;
		invSprite.enabled = false; 	
		if(QualitySprite){
		QualitySprite.enabled = false;
		}
		if(rewardObj3)
		rewardObj3.SetActiveRecursively(false);
	}
//	for(var i=0; i<SpriteBianKuang.length; i++){
//		if(i == id){
//			SpriteBianKuang[i].enabled = true;
//		}else{
//			SpriteBianKuang[i].enabled = false;		
//		}
//	}
}

private var mtw : MainTaskWork;
private var ps : PlayerStatus;
function FindWay(){
	if(UIControl.mapType == MapType.zhucheng || UIControl.mapType == MapType.fuben){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if( ps != null && ! ps.dead){
			mtw.FindWay(pTask); 
			TweenBack();
		}
	}else{
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info598"));
	}
} 

//var uiallpcl : UIAllPanelControl;
function show0(){
	AllManage.UIALLPCStatic.show0();
}