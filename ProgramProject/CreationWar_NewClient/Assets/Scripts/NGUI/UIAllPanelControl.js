#pragma strict
class AllPanel{ 
	var name : String;
	var ap : GameObject[];
	var chName : String = "";
	var uiclFunction : String = "";
	var closelFunction : String = "";
}

var allPanel : AllPanel[];
var allPanelobject : GameObject[];
var Zhucaidan : UIPanel;
var MoveUI : UIPanel;
var SkillUI : UIPanel;
var ZhuUI : UIPanel;
var btnJump : GameObject;
var btnFishing : GameObject;
var btnBuy : GameObject;

function Awake(){
	AllManage.UIALLPCStatic = this;
}

function Start () { 
//rennman = GetComponent(rendercamerapic);
    yield;
	yield;
	yield;
	yield;
	show0();

}

//function OnLevelWasLoaded(level : int){
//	for(var j=0; j<allPanel.length; j++){
//		if(allPanel[j].ap){
//			for(var i=0; i<allPanel[j].ap.length; i++){
//				if(allPanel[j].ap[i] && allPanel[j].ap[i].name.IndexOf("(Clone)") > 0){
//					Destroy(allPanel[j].ap[i]);
//				}
//			}
//		}
//	}
//}

static var isGround : boolean = false;
var uicl : UIControl;
var be : BlurEffect;
var nowStr : String;
var joy : alljoy;
function showThisPanel(str : String){
//	print(str + " == iuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu");
	nowStr = str;
	if(AllManage.invCangKuStatic){
		AllManage.invCangKuStatic.transCangku.localPosition.y = 1000;	
	}
	if(str == "Ground"){
		if(AllManage.UICLStatic.ButtonJingJi)
			AllManage.UICLStatic.ButtonJingJi.parent.localPosition.y = 3000;

//		be.enabled = false;
//	    Zhucaidan.enabled = false;
	    MoveUI.enabled = true;
		isGround = true;
		  SkillUI.enabled = true;
		  btnJump.SetActive(true);
		  btnBuy.SetActive(true);
	}else{
//		be.enabled = true;
		isGround = false;
		MoveUI.enabled = false;
		SkillUI.enabled = false;
		btnJump.SetActive(false);
		btnFishing.SetActive(false);
		btnBuy.SetActive(false);
//		Zhucaidan.enabled = true;
	}
	if(str == "Ground" || str == "chuansong"){
		if(joy)
			joy.ShowJoy(true);
//		AllManage.UICLStatic.joystick.localScale = Vector3.one;
	}else{
		if(joy)
			joy.ShowJoy(false);
//		AllManage.UICLStatic.joystick.localScale = Vector3.zero;	
	}
//	if(str == "Bag" || isRemoveGround(str)){
//	
//	}else{
//		
//	}
	var useObj : GameObject;
		for(var j=0; j<allPanel.length; j++){
			if(allPanel[j].name == str && allPanel[j].ap[0]){ 
//				print(allPanel[j].ap[0] + " == allPanel[j].ap[0]");
				useObj = allPanel[j].ap[0].gameObject;
			}
		}
	
	for(var n=0; n<allPanelobject.length; n++){
		if(allPanelobject[n] && useObj != allPanelobject[n]){	
//			if(allPanel[n].closelFunction != ""){
//				AllManage.UICLStatic.gameObject.SendMessage(allPanel[n].closelFunction , SendMessageOptions.DontRequireReceiver);
//			}
			allPanelobject[n].SetActiveRecursively(false);
			if(n == 4){
				allPanelobject[n].active = true;
			}
		}
//	yield;
	}
	if(str=="Bag" || isRemoveGround(str)){
	   SkillUI.widgetsAreStatic = false;
	   ZhuUI.widgetsAreStatic = false;
//		rennman.RenderM();

	}else{
		uicl.closeNewPlayerInfo();
		uicl.reMoveGround();
	}


	var m : int = 0;
	for(var i=0; i<allPanel.length; i++){
		if(allPanel[i] && allPanel[i].name == str){
			if(allPanel[i].chName != "" && allPanel[i].chName != null){
				//TD_info.panelStatistics(allPanel[i].chName);
			}
			for(m=0; m<allPanel[i].ap.length; m++){
				if(allPanel[i].ap[m] && allPanel[i].ap[m].gameObject){
					if(str == "DailyBenefits"){
						if( !allPanel[i].ap[m].gameObject.active){
							allPanel[i].ap[m].gameObject.SetActiveRecursively(true);		
							if(allPanel[i].ap[m].transform.localScale == Vector3.zero){
								allPanel[i].ap[m].transform.localScale = Vector3.one;
							}					
						}
					}else{
						allPanel[i].ap[m].gameObject.SetActiveRecursively(true);						
						if(allPanel[i].ap[m].transform.localScale == Vector3.zero){
							allPanel[i].ap[m].transform.localScale = Vector3.one;
						}					
					}
				}
				if(allPanel[i].uiclFunction != ""){
					AllManage.UICLStatic.gameObject.SendMessage(allPanel[i].uiclFunction , SendMessageOptions.DontRequireReceiver);
				}
				if(str == "Ground"){
//					print("Open Ground All");
				}
				yield;
				if((str == "Soul" || str == "dazao") && allPanel[i].ap[m] ){
					allPanel[i].ap[m].gameObject.SendMessage("Play" , true,SendMessageOptions.DontRequireReceiver);
				}
			}
			if(str == "Ground"){
				uicl.CloseWenHao();
			   SkillUI.widgetsAreStatic = true;
			   ZhuUI.widgetsAreStatic = true;
			}
			return;
		}
	}
}

var RemoveList : String[];
function isRemoveGround(str : String){
	for(var i=0; i<RemoveList.length; i++){
		if(RemoveList[i] == str){
			return true;
		}
	}
	return false;
}

function show0(){
	yield showThisPanel("Ground");
//	yield;
//	yield;
//	yield;
	AllManage.UICLStatic.UpDateLevelShowZhuButtons();
}

function show1(){
	showThisPanel("Bag");
}

//var preDazao : GameObject;
private var objDazao : GameObject;
var parentDazao : Transform;
function show2(){ 
	if(objDazao == null){
		var preDazao = Resources.Load("Anchor - Equpment - Build", GameObject);
		objDazao = InstantiateGUI(preDazao , parentDazao , "dazao" , 8); 
	}
	yield;	                                          
	showThisPanel("dazao");
	yield;	                                          
	yield;	                                          
	yield;	                                          
	objDazao.SendMessage( "showDaZao" , SendMessageOptions.DontRequireReceiver);
}

//var preShengchan : GameObject;
private var objShengchan : GameObject;
var parentShengchan : Transform;
function show3(){ 
	if(objShengchan == null){
		var preShengchan = Resources.Load("Anchor - HeCheng", GameObject);
		objShengchan = InstantiateGUI(preShengchan , parentShengchan , "shengchan" , 9); 
	}
	yield;	                                          
	showThisPanel("shengchan");
	yield;	                                          
	yield;	                                          
	yield;	                                          
		objShengchan.SendMessage( "resetList" , SendMessageOptions.DontRequireReceiver);
}

function showHeCheng3(){ 
	if(objShengchan == null){
		var preShengchan = Resources.Load("Anchor - HeCheng", GameObject);
		objShengchan = InstantiateGUI(preShengchan , parentShengchan , "shengchan" , 9); 
		objShengchan.SendMessage( "resetList" , SendMessageOptions.DontRequireReceiver);
		objShengchan.SetActiveRecursively(false);
	}
}

function show4(){
	showThisPanel("dakong");
}


//var preKaoyu : GameObject;
private var objKaoyu : GameObject;
var parentKaoyu : Transform;
function show5(){ 
	if(objKaoyu == null){
		var preKaoyu = Resources.Load("Anchor - Cook", GameObject);
		objKaoyu = InstantiateGUI(preKaoyu , parentKaoyu , "kaoyu" , 11); 
	}
	yield;	                                          
	showThisPanel("kaoyu");
	yield;	                                          
	yield;	                                          
	yield;	                                          
		objKaoyu.SendMessage( "ReLoadFish" , SendMessageOptions.DontRequireReceiver);
}

//var preNowTask : GameObject;
private var objNowTask : GameObject;
var parentNowTask : Transform;
function show6( Player : MainPlayerStatus ){ 
//	print(objNowTask + "123");
	if(objNowTask == null){
		var preNowTask = Resources.Load("Anchor - NowTask", GameObject);
		objNowTask = InstantiateGUI(preNowTask , parentNowTask , "NowTask" , 12); 
	}
//	print(Player + " == Player");                           
	yield;	                                          
//	print(Player + " == Player");                           
	showThisPanel("NowTask");
//	print(Player + " == Player");                           
	objNowTask.SendMessage( "SetPlayerTask" , Player , SendMessageOptions.DontRequireReceiver);
//	yield;	                                          
////	print(Player + " == Player");                           
//	yield;	                                          
////	print(Player + " == Player");                           
//	yield;	               
//	print(Player + " == Player");                           
//	objNowTask.SendMessage( "SetPlayerTask" , Player , SendMessageOptions.DontRequireReceiver);
}

//var preMail : GameObject;
private var objMail : GameObject;
var parentMail : Transform; 

function show7(){ 
	if( parseInt(AllManage.psStatic.Level) > 0 ){
		if(objMail == null){
			var preMail = Resources.Load("Anchor - Mail", GameObject);
			objMail = InstantiateGUI(preMail , parentMail , "mail" , 13); 
			objMail.SetActiveRecursively(false);
		}
		yield;	                                          
		showThisPanel("mail");
	}else{
		ts.Show("tips054");
	}
}

function showGM(){ 
	if( parseInt(AllManage.psStatic.Level) > 0 ){
		if(objMail == null){
			var preMail = Resources.Load("Anchor - Mail", GameObject);
			objMail = InstantiateGUI(preMail , parentMail , "mail" , 13); 
			objMail.SetActiveRecursively(false);
		}
		yield;	                                          
		showThisPanel("mail");
	}else{
		ts.Show("tips054");
	}
}

static var mi : MailInfo;
function  callMail(){
	while( !mi){
		yield;
	}
	yield WaitForSeconds(0.5);
	StartCoroutine(mi.OpenGMMail());
}

//var preActivity : GameObject;
private var objActivity : GameObject;
var parentActivity : Transform;
function show8(){
	if(objActivity == null){
		var preActivity = Resources.Load("Anchor - Activity", GameObject);
		objActivity = InstantiateGUI(preActivity , parentActivity , "Activity" , 14);
	}
	yield;
	showThisPanel("Activity");
}

//var preDailyBenefits : GameObject;
private var objDailyBenefits : GameObject;
var parentDailyBenefits : Transform;
function show9(){
	if(Application.loadedLevelName == "Map200"){
		return;
	}
	if(AllManage.UICLStatic.canOpenViewAsTaskID("572") ||( AllManage.jiaochengCLStatic.JiaoChengID == 14 && AllManage.jiaochengCLStatic.step == 0 ) || parseInt(AllManage.psStatic.Level) > 4){
		if(objDailyBenefits == null){
			var preDailyBenefits = Resources.Load("Anchor - DailyBenefits", GameObject);
			objDailyBenefits = InstantiateGUI(preDailyBenefits , parentDailyBenefits , "DailyBenefits" , 15);
		}
		yield;
		showThisPanel("DailyBenefits");
	}
}

//var prePaihang : GameObject;
private var objPaihang : GameObject;
var parentPaihang : Transform;
function show10(){
	if(AllManage.UICLStatic.canOpenViewAsTaskID()){
	    if(parseInt(AllManage.psStatic.Level)==1){
	    		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info747")); 
	    }else{
			if(objPaihang == null){
				var prePaihang = Resources.Load("Anchor - Ranking", GameObject);
				objPaihang = InstantiateGUI(prePaihang , parentPaihang , "paihang" , 16);
			}
			yield;
			showThisPanel("paihang");
		}	
	}
}

//var preStore : GameObject;
private var objStore : GameObject;
var parentStore : Transform;
function show11(){
	if(objStore == null){
		var preStore = Resources.Load("Anchor - SongStore", GameObject);
		objStore = InstantiateGUI(preStore , parentStore , "Store" , 21);
	}
	yield;
	showThisPanel("Store");
}

function show12(){
    showThisPanel("juedou");
    //showThisPanel("Newjuedou");
}

//²é¿´Õ½¼¨
function show122()
{
    showThisPanel("juedou");
}

function show13(){
//	showThisPanel("jingjichang");
}

function show14(){
//	showThisPanel("zhanchang");
}

//var preBranch : GameObject;
private var objBranch : GameObject;
var parentBranch : Transform;
function show15(){ 
	if(objBranch == null){
		var preBranch = Resources.Load("Anchor - Branch", GameObject);
		objBranch = InstantiateGUI(preBranch , parentBranch , "branch" , 30); 
	}
	yield;	                                          
	showThisPanel("branch");
	yield;	                                          
	yield;	                                          
	yield;	                                          
		objBranch.SendMessage( "OpenBranch" , SendMessageOptions.DontRequireReceiver);
}

function showbranch15(){ 
	if(objBranch == null){
		var preBranch = Resources.Load("Anchor - Branch", GameObject);
		objBranch = InstantiateGUI(preBranch , parentBranch , "branch" , 30); 
		objBranch.SendMessage( "OpenBranch" , SendMessageOptions.DontRequireReceiver);
		objBranch.SetActiveRecursively(false);
	}
}

//var preVIP : GameObject;
private var objVIP : GameObject;
var parentVIP : Transform;
function show16(){
	if(uicl.mapType == MapType.fuben){
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info772"));
	}else{
	if(Application.loadedLevelName == "Map200"){
		return;
	}
	if(objVIP == null){
		var preVIP = Resources.Load("Anchor - VIP", GameObject);
		objVIP = InstantiateGUI(preVIP , parentVIP , "VIP" , 32); 
	}
	yield;	                                          
	showThisPanel("VIP");
}
}

//var preSoul : GameObject;
private var objSoul : GameObject;
var parentSoul : Transform;
function show17(){ 
	if(objSoul == null){
		var preSoul = Resources.Load("Anchor - Soul", GameObject);
		objSoul = InstantiateGUI(preSoul , parentSoul , "Soul" , 33); 
	}
	yield;	                                          
	showThisPanel("Soul");
}

function showSoul17(){ 
	if(objSoul == null){
		var preSoul = Resources.Load("Anchor - Soul", GameObject);
		objSoul = InstantiateGUI(preSoul , parentSoul , "Soul" , 33); 
	}
}

function isSoul17(){
	if(objSoul != null){
		return true;
	}else{
		return false;
	}
}

//var preHole : GameObject;
private var objHole : GameObject;
var parentHole : Transform;
function show18(){ 
	if(objHole == null){
		var preHole = Resources.Load("Anchor - Equpment - Hole", GameObject);
		objHole = InstantiateGUI(preHole , parentHole , "Hole" , 34); 
	}
	yield;	                                          
	showThisPanel("Hole");
	yield;	                                          
	yield;	                                          
	yield;	                                          
		objHole.SendMessage( "showXiangQian" , SendMessageOptions.DontRequireReceiver);
}

//var preEverydayAim : GameObject;
private var objEverydayAim : GameObject;
var parentEverydayAim : Transform;
function show19(){
	if(objEverydayAim == null){
		var preEverydayAim = Resources.Load("Anchor - EverydayAim", GameObject);
		objEverydayAim = InstantiateGUI(preEverydayAim , parentEverydayAim , "EverydayAim" , 36);
	}
	yield;
	showThisPanel("EverydayAim");
}

//var prePVP : GameObject;
private var objPVP : GameObject;
var parentPVP : Transform;
function show20(){
//	print("123");
	if(objPVP == null){
		var prePVP = Resources.Load("Anchor - PVP", GameObject);
		objPVP = InstantiateGUI(prePVP , parentPVP , "PVP" , 35);
	}
	yield;
	showThisPanel("PVP");
}
function showPVP20(){
//	print("456");
	if(objPVP == null){
		var prePVP = Resources.Load("Anchor - PVP", GameObject);
		objPVP = InstantiateGUI(prePVP , parentPVP , "PVP" , 35);
		objPVP.SetActiveRecursively(false);
	}
}

//var preMakeGold : GameObject;
private var objMakeGold : GameObject;
var parentMakeGold : Transform;
function show21(){
	if(objMakeGold == null){
		var preMakeGold = Resources.Load("Anchor - MakeGold", GameObject);
		objMakeGold = InstantiateGUI(preMakeGold , parentMakeGold , "MakeGold" , 37);
	}
	yield;
	showThisPanel("MakeGold");
}

//var preXunLian : GameObject;
private var objXunLian : GameObject;
var parentXunLian : Transform;
function show22(){
	if(objXunLian == null){
		var preXunLian = Resources.Load("Anchor - XunLian", GameObject);
		objXunLian = InstantiateGUI(preXunLian , parentXunLian , "xunlian" , 38);
	}
	yield;
	showThisPanel("xunlian");
}

//var preRenWu : GameObject;
private var objRenWu : GameObject;
var parentRenWu : Transform;
function show23(){
	if(objRenWu == null){
		var preRenWu = Resources.Load("Anchor - Task", GameObject);
		objRenWu = InstantiateGUI(preRenWu , parentRenWu , "renwu" , 10);
	}
	yield;
	showThisPanel("renwu");
	if(AllManage.UICLStatic.objMainUI){
	AllManage.UICLStatic.objMainUI.SetActive(false);
}
}

//var preChuanSong : GameObject;
private var objChuanSong : GameObject;
var parentChuanSong : Transform;
function show24(){
	if(objChuanSong == null){
		var preChuanSong = Resources.Load("Anchor - ChuanSong", GameObject);
		objChuanSong = InstantiateGUI(preChuanSong , parentChuanSong , "chuansong" , 6);
	}
	yield;
	showThisPanel("chuansong");
}

//var preJiaoYi : GameObject;
private var objJiaoYi : GameObject;
var parentJiaoYi : Transform;
function show25(){
	if(objJiaoYi == null){
		var preJiaoYi = Resources.Load("Anchor - ChaKan & jIaoYi", GameObject);
		objJiaoYi = InstantiateGUI(preJiaoYi , parentJiaoYi , "jiaoyi" , 4);
	}
	yield;
	showThisPanel("jiaoyi");
}

function showJiaoYi25(){
	if(objJiaoYi == null){
		var preJiaoYi = Resources.Load("Anchor - ChaKan & jIaoYi", GameObject);
		objJiaoYi = InstantiateGUI(preJiaoYi , parentJiaoYi , "jiaoyi" , 4);
	}
}

//var preFuBen : GameObject;
private var objFuBen : GameObject;
var parentFuBen : Transform;
function show26(){
	if(objFuBen == null){
		var preFuBen = Resources.Load("Anchor - Dungeon", GameObject);
		objFuBen = InstantiateGUI(preFuBen , parentFuBen , "fuben" , 7);
	}
	yield;
	showThisPanel("fuben");
}


private var objAchievementPnael : GameObject;
var parentAchievementPnael : Transform;
function show31(){
	if(objAchievementPnael == null){
		var preAchievementPnael = Resources.Load("Anchor - Honor", GameObject);
		objAchievementPnael = InstantiateGUI(preAchievementPnael , parentAchievementPnael , "AchievementPnael" , 7);
	}
	yield;
	showThisPanel("AchievementPnael");
	yield;
	objAchievementPnael.SendMessage("CheckboxChecked" , SendMessageOptions.DontRequireReceiver);
}

////var preFuBen : GameObject;
private var objMiniMap : GameObject;
var parentMiniMap : Transform;
function show27(){
	if(Application.loadedLevelName == "Map200" || AllManage.dungclStatic.DungeonIsDone || nowStr != "Ground"){
		return;
	}
	if(objMiniMap == null){
		var preMiniMap = Resources.Load("Anchor - MiniMap", GameObject);
		objMiniMap = InstantiateGUI(preMiniMap , parentMiniMap , "MiniMap" , 39);
	}
	yield;
	showThisPanel("MiniMap");
	yield;	                                          
	yield;	                                          
	yield;	                                          
		objMiniMap.SendMessage( "SetNPCList" , "" , SendMessageOptions.DontRequireReceiver);
}

////var preFuBen : GameObject;
private var objGamble : GameObject;
var parentGamble : Transform;
function show28(){
	if(AllManage.UICLStatic.canOpenViewAsTaskID() ||( AllManage.jiaochengCLStatic.JiaoChengID == 13 && AllManage.jiaochengCLStatic.step == 0 )){
		if(parseInt(AllManage.psStatic.Level)==1 && !( AllManage.jiaochengCLStatic.JiaoChengID == 13 && AllManage.jiaochengCLStatic.step == 0 )){
	    		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info747")); 
	    }else{
			if(objGamble == null){
				var preGamble = Resources.Load("Anchor - Gamble", GameObject);
				objGamble = InstantiateGUI(preGamble , parentGamble , "Gamble" , 40);
			}
			yield;
		    showThisPanel("Gamble");
		}
	}
}

private var objOfflinePlayer : GameObject;
var parentOfflinePlayer : Transform;
function show29(){
	if(objOfflinePlayer == null){
		var preOfflinePlayer = Resources.Load("Anchor - OfflinePlayer", GameObject);
		objOfflinePlayer = InstantiateGUI(preOfflinePlayer , parentOfflinePlayer , "OfflinePlayer" , 41);
	}
	yield;
	showThisPanel("OfflinePlayer");
}

private var objReward : GameObject;
var parentReward : Transform;
function show30(){
	if(objReward == null){
		var preReward = Resources.Load("Anchor - Reward", GameObject);
		objReward = InstantiateGUI(preReward , parentReward , "Reward" , 42);
	}
	yield;
	showThisPanel("Reward");
}

function DesObj30(){
	if(objReward != null)
		Destroy(objReward);
}

private var objSkill : GameObject;
var parentSkill : Transform;
function show32(){
	parentSkill.gameObject.SetActiveRecursively(true);
	if(objSkill == null){
		var preSkill = Resources.Load("Anchor - Skill", GameObject);
		objSkill = InstantiateGUI(preSkill , parentSkill , "Skill" , 43);
	}
	yield;
	showThisPanel("Skill");
}
function showSkill32(){
	if(objSkill == null){
		var preSkill = Resources.Load("Anchor - Skill", GameObject);
		objSkill = InstantiateGUI(preSkill , parentSkill , "Skill" , 43);
	}
}

private var objNewGM : GameObject;
var parentNewGM : Transform;
function show33(){
	if(objNewGM == null){
		var preNewGM = Resources.Load("Anchor - NewGM", GameObject);
		objNewGM = InstantiateGUI(preNewGM , parentNewGM , "NewGM" , 44);
	}
	yield;
	showThisPanel("NewGM");
}

private var objInvite : GameObject;
var parentInvite : Transform;
function show34(){
	
	if(objInvite == null){
		var preInvite = Resources.Load("Anchor - InviteFriends", GameObject);
		objInvite = InstantiateGUI(preInvite , parentInvite , "InviteFriends" , 45);
		objInvite.transform.localScale = Vector3.one;
	}
	objInvite.SetActiveRecursively(true);
	//yield;
	//showThisPanel("InviteFriends");
}

private var objsocietyTask : GameObject;
var parentsocietyTask : Transform;
function show35(){
	if(objsocietyTask == null){
		var presocietyTask = Resources.Load("Anchor - GuildTask", GameObject);
		objsocietyTask = InstantiateGUI(presocietyTask , parentsocietyTask , "societyTask" , 46);
	}
	yield;
	showThisPanel("societyTask");
}

private var objLuckey : GameObject;
var parentLuckey : Transform;
function show36(){
	if(objLuckey == null){
		var preLuckey = Resources.Load("Anchor - Lotto", GameObject);
		objLuckey = InstantiateGUI(preLuckey , parentLuckey , "Luckey" , 47);
	}
	yield;
	showThisPanel("Luckey");
}

private var objExhibition : GameObject;
var parentExhibition : Transform;
function show37(){
	if(objExhibition == null){
		var preExhibition = Resources.Load("Anchor - Exhibition", GameObject);
		objExhibition = InstantiateGUI(preExhibition , parentExhibition , "Exhibition" , 48);
	}
	yield;
	showThisPanel("Exhibition");
}

function showExhibition37(){
	if(objExhibition == null){
		var preExhibition = Resources.Load("Anchor - Exhibition", GameObject);
		objExhibition = InstantiateGUI(preExhibition , parentExhibition , "Exhibition" , 48);
		objExhibition.SetActiveRecursively(false);
	}
}

private var objBreakdown : GameObject;
var parentBreakdown : Transform;
function show38(){
	if(objBreakdown == null){
		var preBreakdown = Resources.Load("Anchor - Equpment - Breakdown", GameObject);
		objBreakdown = InstantiateGUI(preBreakdown , parentBreakdown , "Breakdown" , 49);
	}
	yield;
	showThisPanel("Breakdown");
}

private var objPanelFrist : GameObject;
var parentPanelFrist : Transform;
function show39(){
	if(uicl.mapType != MapType.jingjichang){
		if(uicl.mapType == MapType.fuben){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info772"));
		}else{
		if(objPanelFrist == null){
			var prePanelFrist = Resources.Load("Anchor - PanelFirstCharge", GameObject);
			objPanelFrist = InstantiateGUI(prePanelFrist , parentPanelFrist , "PanelFrist" , 50);
		}
		yield;
		showThisPanel("PanelFrist");
		}
	}
}

private var objTeamCreat : GameObject;
var parentTeamCreat : Transform;
function show40(){
	if(objTeamCreat == null){
		var preTeamCreat = Resources.Load("Anchor - NewCreatTeam", GameObject);
		objTeamCreat = InstantiateGUI(preTeamCreat , parentTeamCreat , "TeamCreat" , 51);
	}
	yield;
	showThisPanel("TeamCreat");
}

private var objSection : GameObject;
var parentSection : Transform;
function show41(){
	if(objSection == null){
		var preSection = Resources.Load("Anchor - Section-tips", GameObject);
		objSection = InstantiateGUI(preSection , parentSection , "Section" , 52);
	}
	yield;
	showThisPanel("Section");
}
private var objNewActivity : GameObject;
var parentNewActivity : Transform;
function show42(){
			if(parseInt(AllManage.psStatic.Level)<20){
	    		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1169")); 
	    		return;
	    }	
	if(objNewActivity == null){
		var preNewActivity = Resources.Load("Anchor - NewActivity", GameObject);
		objNewActivity = InstantiateGUI(preNewActivity , parentNewActivity , "NewActivity" , 53);
	}
	yield;
	showThisPanel("NewActivity");
}

private var objNearPlayer : GameObject;
var parentNearPlayer : Transform;
function show43(){
	if(objNearPlayer == null){
		var preNearPlayer = Resources.Load("Anchor -NearPlayer", GameObject);
		objNearPlayer = InstantiateGUI(preNearPlayer , parentNearPlayer , "NearPlayer" , 54);
	}
	yield;
	showThisPanel("NearPlayer");
}

private var objSelectChannel : GameObject;
var parentSelectChannel : Transform;
function show44(){
	if(objSelectChannel == null){
		var preSelectChannel = Resources.Load("Anchor - Anchor-SelectChannel", GameObject);
		objSelectChannel = InstantiateGUI(preSelectChannel , parentSelectChannel , "SelectChannel" , 55);
	}
	yield;
	showThisPanel("SelectChannel");
}

private var objPVPWin : GameObject;
var parentPVPWin : Transform;
function show45(){
	if(objPVPWin == null){
		var prePVPWin = Resources.Load("Anchor - PVPWIN", GameObject);
		objPVPWin = InstantiateGUI(prePVPWin , parentPVPWin , "PVPWin" , 56);
	}
	yield;
	showThisPanel("PVPWin");
}

private var objPVPLose : GameObject;
var parentPVPLose : Transform;
function show46(){
	if(objPVPLose == null){
		var prePVPLose = Resources.Load("Anchor - PVPLose", GameObject);
		objPVPLose = InstantiateGUI(prePVPLose , parentPVPLose , "PVPLose" , 57);
	}
	yield;
	showThisPanel("PVPLose");
}

private var objBrushMagic : GameObject;
var parentBrushMagic : Transform;
function show47(){

	if(parseInt(AllManage.psStatic.Level)<15){
	    		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1167")); 
	    		return;
	    }
	if(objBrushMagic == null){
		var preBrushMagic = Resources.Load("Anchor - BrushMagic", GameObject);
		objBrushMagic = InstantiateGUI(preBrushMagic , parentBrushMagic , "BrushMagic" , 58);
	}
	yield;
	showThisPanel("BrushMagic");
}

private var objWorldBoos : GameObject;
var parentWorldBoos : Transform;
function show48(){
	if(objWorldBoos == null){
		var preWorldBoos = Resources.Load("Anchor - WorldBoos", GameObject);
		objWorldBoos = InstantiateGUI(preWorldBoos , parentWorldBoos , "WorldBoos" , 59);
	}
	yield;
	showThisPanel("WorldBoos");
}

private var objBackhome : GameObject;
var parentBackhome : Transform;
function show49(){
	if(uicl.mapType != MapType.zhucheng){
	return;
	}
	if(objBackhome == null){
		var preBackhome = Resources.Load("Anchor - Panel - BackHome", GameObject);
		objBackhome = InstantiateGUI(preBackhome , parentBackhome , "Backhome" , 60);
	}
	yield;
	showThisPanel("Backhome");
}

private var objActivityNotice : GameObject;
var parentActivityNotice : Transform;
function show50(){
	if(objActivityNotice == null){
		var preActivityNotice = Resources.Load("Anchor - Panel - ActivityNotice", GameObject);
		objActivityNotice = InstantiateGUI(preActivityNotice , parentActivityNotice , "ActivityNotice" , 61);
	}
	yield;
	showThisPanel("ActivityNotice");
}


function CloseInvite(){
	objInvite.SetActiveRecursively(false);

}

function EndRewardClose(){
		if(objReward!=null){
			Destroy(objReward);
			}
			show0();
}


function InstantiateGUI(pre : GameObject , parent : Transform , nameAPap : String , intAPobj : int) : GameObject{
	var obj : GameObject; 
	var i : int = 0;
	obj = GameObject.Instantiate( pre );
	obj.transform.parent = parent;
	obj.transform.localPosition = Vector3.zero;	 
	for(i=0; i<allPanel.length ; i++){
		if(nameAPap == allPanel[i].name){
			allPanel[i].ap[0] = obj;		
		}
	}
	allPanelobject[intAPobj] = obj;
	return obj;
}
function SetGUI(obj : GameObject , nameAPap : String , intAPobj : int) : GameObject{
	var i : int = 0;
	for(i=0; i<allPanel.length ; i++){
		if(nameAPap == allPanel[i].name){
			allPanel[i].ap[0] = obj;		
		}
	}
	allPanelobject[intAPobj] = obj;
	return obj;
}

var ts : TiShi;