#pragma strict
class TalkClass{
	var head : String[];
	var info : String[];
	var type : String[];
	var audio : AudioClip[];
	var audioName : String[];
}

var Sound : AudioSource;
var LevelTalk : TalkClass[];
var duncl : DungeonControl;
var invcl : InventoryControl;
private var JStr : String = "#";
private var QStr : String = "@"; 
var thisMapLevelTalk : TalkClass;
var buttonAutoPlay : Transform;
var secObj : GameObject;
function Start () {
	
	if(Application.loadedLevelName != "Map200"){
		yield;
		yield; 
		for(var rows : yuan.YuanMemoryDB.YuanRow in duncl.yt.Rows){
//			print(MainTaskWork.MapID + " == " + rows["MapID"].YuanColumnText + " == " + rows.Count);
			if(MainTaskWork.MapID == rows["MapID"].YuanColumnText && ! invcl.CanShowTalkAsMapID(Application.loadedLevelName)){
				thisMapLevelTalk = GetThisTalk(rows); 
				LevelTalk[0] = thisMapLevelTalk; 
				LevelID = 0;
				if(thisMapLevelTalk.info.length > 1){
					ShowTalkAsStep(gameObject , "FuncNon");
					buttonAutoPlay.transform.position.y = 3000;
					secObj.SendMessage("ShowSectionMap" , rows["MapID"].YuanColumnText , SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}

function changeIconAsProID( ProID : String){
	switch(ProID){
		case "1":
			LevelTalk[3].head[1] = "head-zhanshi";
//			LevelTalk[3].head[2] = "head-zhanshi";
//			LevelTalk[4].head[0] = "head-youxia";
			break;
		case "2":
			LevelTalk[3].head[1] = "head-youxia";
//			LevelTalk[3].head[2] = "head-youxia";
//			LevelTalk[4].head[0] = "head-fashi";
			break;
		case "3":
			LevelTalk[3].head[1] = "head-fashi";
//			LevelTalk[3].head[2] = "head-fashi";
//			LevelTalk[4].head[0] = "head-zhanshi";
			break;
	}
}

function FuncNon(){
    invcl.AddTalkShow(Application.loadedLevelName);
    
    PlayerStatus.MainCharacter.gameObject.SendMessage("FirstClickGOBtn", SendMessageOptions.DontRequireReceiver);   
}

var useJstr : String[]; 
function GetThisTalk(rows : yuan.YuanMemoryDB.YuanRow) : TalkClass{
	var LTalk : TalkClass;
	LTalk = new TalkClass(); 
	var useStr : String[] = rows["Speak"].YuanColumnText.Split(QStr.ToCharArray());
	
	var useHead : String[];
		useHead = new Array(useStr.length);
	var useInfo : String[];
		useInfo = new Array(useStr.length);
	var useType : String[];
		useType = new Array(useStr.length);
	var useAudio : AudioClip[];
		useAudio = new Array(useStr.length);
	var audioName : String[];
		audioName = new Array(useStr.length);
	
	for(var i=0; i<useStr.length; i++){
//		print(useStr[i]);
		if(useStr[i].Length > 5){
			useJstr = useStr[i].Split(JStr.ToCharArray());
			useHead[i] = "NPC_" + useJstr[0].ToString();
			useInfo[i] = useJstr[2].ToString();
			useType[i] = useJstr[1].ToString();
		}
	}
	LTalk.head = useHead;
	LTalk.info = useInfo;
	LTalk.type = useType;
	LTalk.audio = useAudio;
	LTalk.audioName = audioName;
	return LTalk;
}

var LevelParent : GameObject;
var LabelTalk : UILabel;
var SpriteTalk : UISprite; 
var SpritePlayer : UISprite;
var LevelID : int;
var step : int;
//var Ground1 : GameObject;
//var Ground2 : GameObject;
//var Ground3 : GameObject;
var cam1 : Camera;
var ButtonSanJiao : GameObject;
var isFirstShow : Boolean = true;// 主城中章节是否第一次显示
//static var isSectionShow : Boolean = false;// 章节是否已经显示过
function ShowTalkAsStep(obj : GameObject , funcStr : String){
	NowObj = obj;
	NowFunc = funcStr;

    LevelParent.SetActiveRecursively(true);
    if(isFirstShow && Application.loadedLevelName == "Map111")
	{
	    isFirstShow = false;
	    LevelParent.SetActiveRecursively(false);
        PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show41",SendMessageOptions.DontRequireReceiver);
        SectionLabel.sectionLabel.Finish0.enabled = true;
        SectionLabel.sectionLabel.ObjPart1.enabled = true;
        SectionLabel.sectionLabel.LabText.text = StaticLoc.Loc.Get("info973");
	}


	ButtonCloseAndOpen();
//	Ground1.transform.localScale = Vector3(0,0,0);
//	Ground2.transform.localScale = Vector3(0,0,0);
//	Ground3.transform.localScale = Vector3(0,0,0);
	cam1.enabled = false;
	LabelTalk.text = AllManage.AllMge.Loc.Get(LevelTalk[LevelID].info[step]).Replace("XXXX" , InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText); 
	if(LevelTalk[LevelID].audioName[step] != "" && LevelTalk[LevelID].audioName[step] != null){
		if(LevelTalk[LevelID].audio[step] == null){
			LevelTalk[LevelID].audio[step] = Resources.Load(LevelTalk[LevelID].audioName[step] , AudioClip);
		}
	}
	if(LevelTalk[LevelID].audio[step] != null){
//		print("zhe li le ~~~~");
//		Sound.PlayOneShot(LevelTalk[LevelID].audio[step]);
		Sound.clip = LevelTalk[LevelID].audio[step];
		Sound.Play();
	}
//	print(LevelTalk[LevelID].head[step]); 
	if(LevelTalk[LevelID].type[step] == "0"){
		SpriteTalk.spriteName = LevelTalk[LevelID].head[step];
	 
	}else
	if(LevelTalk[LevelID].type[step] == "1"){
		SpriteTalk.spriteName = SpritePlayer.spriteName;
	}
	if(LevelID == 4 && step == 0){
		NextStep();
	}
}

    function SetActivePanel()
    {
        // 防止第一章已经完成时，仍然把电影剧情效果又给弹出来了，出现剧情里显示默认“角角角角。。。”的情况
//        if(BtnGameManager.yt.Rows[0]["CompletTask"].YuanColumnText == "")
		if(! invcl.CanShowTalkAsMapID(Application.loadedLevelName))
        {
//            isSectionShow = true;
            LevelParent.SetActiveRecursively(true);
        }
    }

function ButtonCloseAndOpen(){
	ButtonSanJiao.SetActiveRecursively(false);
	yield WaitForSeconds(1);
	ButtonSanJiao.SetActiveRecursively(true);	
}


var NowObj : GameObject;
var NowFunc : String;
function NextStep(){
//	print("jin lai le");
	if(step == LevelTalk[LevelID].info.Length - 2){
//		print("wan cheng"); 
		LevelParent.SetActiveRecursively(false);
	cam1.enabled = true;
//	Ground1.transform.localScale = Vector3(1,1,1);
//	Ground2.transform.localScale = Vector3(1,1,1);
//	Ground3.transform.localScale = Vector3(1,1,1);
		if(NowObj)
		NowObj.SendMessage(NowFunc , SendMessageOptions.DontRequireReceiver);
	}else{	
		step += 1;
		ShowTalkAsStep(NowObj , NowFunc);
	}
}
