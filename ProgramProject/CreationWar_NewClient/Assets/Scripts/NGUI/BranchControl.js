#pragma strict
class BranchClass{
	var kuang : UISprite;
	var icon : UISprite;
	var wuqi : UISprite;
	var WeaPonName : UILabel;
	var name : UILabel;
	var info : UILabel;
	var select : UILabel;
}
class BranchStr{
	var name : String;
	var info : String;
	var icon : String;
	var wuqi : String;
	var WeaPonName : String;
}

static var BranchID : int;
var ProID : int;
var PlayerLevel : int; 
var branchClass : BranchClass[];
var brhStr1 : BranchStr[];
var brhStr2 : BranchStr[];
var brhStr3 : BranchStr[];
var brhStr4 : BranchStr[];
var brhStr5 : BranchStr[];
var ObjDown : GameObject;

var LabelCost : UILabel;
var buttons : GameObject[];
function Start () {
//	uiallcl = AllManage.UIALLPCStatic;
	mtw = AllManage.mtwStatic;
//	ts = AllManage.tsStatic;
	jiaochengCL = FindObjectOfType(TaskJiaoChengControl);
	jiaochengCL.allJiaoCheng[4].obj[0] = buttons[0];
	jiaochengCL.allJiaoCheng[4].obj[1] = buttons[1];
//	jiaochengCL.allJiaoCheng[4].obj[2] = buttons[2];
	
	var mm : boolean = false;
	while(!mm){
		if(InventoryControl.ytLoad){
			if(InventoryControl.yt.Rows.Count > 0){			
				BranchID = GetBDInfoInt("SkillsBranch" , 0); 
				ProID = GetBDInfoInt("ProID" , 0); 	
				PlayerLevel = GetBDInfoInt("PlayerLevel" , 1);		
			}
		}
		yield;
	}
	LookPlayerBranch(PlayerLevel , ProID , BranchID);
	LabelCost.text = AllManage.AllMge.Loc.Get("meg0207")+ BtnGameManager.dicClientParms["SelectBranch"] + AllManage.AllMge.Loc.Get("messages053");
}


function Update () {

}

var useBch : int;
private var jiaochengCL : TaskJiaoChengControl;
function select1(){ 
//	//print("yun xing 1");
			if(jiaochengCL.JiaoChengID == 4 && jiaochengCL.step == 0){
				jiaochengCL.NextStep();
			}
	if(BranchID != 3){
		loadPlayer();
//		if(PlayerLevel >= 10){
			useBch = 1;
//		}
		showSelectAsID(useBch);
	}
}

function select2(){
			if(jiaochengCL.JiaoChengID == 4 && jiaochengCL.step == 0){
				jiaochengCL.NextStep();
			}
	if(BranchID != 3){
		loadPlayer();
//		if(PlayerLevel >= 10){
			useBch = 2;
//		}
		showSelectAsID(useBch); 
	}
}

var LabelHuaFei : UILabel;
function showSelectAsID(id : int){ 
//	//print("yun xing 2 == " + id + " == " + BranchID);
	if(id != BranchID){  
//		//print(BranchID);
		if(BranchID != 0){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages052");
			AllManage.AllMge.Keys.Add(PlayerLevel*10 + "");
			AllManage.AllMge.Keys.Add("messages053");
			AllManage.AllMge.SetLabelLanguageAsID(LabelHuaFei);
//			LabelHuaFei.text = "花费" + PlayerLevel*10 + "血石";
		}else{
			LabelHuaFei.text = "";			
		}
		ObjDown.transform.localPosition.y = 0;		
	}else{
//		//print("yun xing 3");
		ObjDown.transform.localPosition.y = 1000;			
	}
//	//print("yun xing 4");
	if(id == 1){
		branchClass[0].kuang.enabled = true;
		branchClass[1].kuang.enabled = false;
	}else
	if(id == 2){
		branchClass[0].kuang.enabled = false;
		branchClass[1].kuang.enabled = true;	
	}
}
private var ps : PlayerStatus; 
//var ts : TiShi;
function SaveBranch(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	} 
	if(BranchID != 0){
		if((AllManage.jiaochengCLStatic.JiaoChengID == 4 && AllManage.jiaochengCLStatic.step == 1) && AllManage.InvclStatic.TutorialsDetectionAsID("41")  ){
			realSaveBranch();
//			ts.Show("花费了" + PlayerLevel*10 + "血石");
		}else
		{
			AllManage.qrStatic.ShowBuyQueRen1(gameObject , "YesSaveBranch" , "" , AllManage.AllMge.Loc.Get("meg0207")+ BtnGameManager.dicClientParms["SelectBranch"] + AllManage.AllMge.Loc.Get("messages053") + AllManage.AllMge.Loc.Get("meg0208")+ "");		
//			AllManage.tsStatic.RefreshBaffleOn();

		}
	}else{  
		if(mtw.MainPS != null){
			if(lookQianZhiTaskAsID("23")){
//				//print(useBch);
				BranchID = useBch;
				InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText = BranchID.ToString(); 		
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.ProChange).ToString());
				AllManage.UIALLPCStatic.show0();
			}else{
				AllManage.tsStatic.Show("tips002");		
			}
		}
	}
}

function YesSaveBranch(){
	AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.SaveBranch , GetBDInfoInt("PlayerLevel" , 1) , 0 , "" , gameObject , "realSaveBranch");
}

function realSaveBranch(){
	BranchID = useBch;
	InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText = BranchID.ToString(); 
	InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.ProChange).ToString());
	AllManage.UIALLPCStatic.show0();
//	realYesXiDian();
	AllManage.tsStatic.Show("info888");		
}

private var mtw : MainTaskWork;
function lookQianZhiTaskAsID(id : String) : boolean{
	for(var ids : String in mtw.MainPS.player.doneTaskID){
		if(ids == id){
			return true;
		}
	}
	return false;
}

function LookPlayerBranch(level : int , pro : int , brh : int){
	var useBrhStr : BranchStr[];
	switch(pro){
		case 1: useBrhStr = brhStr1; break;
		case 2: useBrhStr = brhStr2; break;
		case 3: useBrhStr = brhStr3; break;
		case 4: useBrhStr = brhStr4; break;
		case 5: useBrhStr = brhStr5; break;
	}
	branchClass[0].name.text = useBrhStr[0].name;
	branchClass[0].info.text = useBrhStr[0].info;
	branchClass[0].icon.spriteName = useBrhStr[0].icon;
	branchClass[0].wuqi.spriteName = useBrhStr[0].wuqi;
	branchClass[0].WeaPonName.text = useBrhStr[0].WeaPonName;
	branchClass[0].select.enabled = false;
	branchClass[0].kuang.enabled = false;
	
	branchClass[1].name.text = useBrhStr[1].name;
	branchClass[1].info.text = useBrhStr[1].info;
	branchClass[1].icon.spriteName = useBrhStr[1].icon;
	branchClass[1].wuqi.spriteName = useBrhStr[1].wuqi;
	branchClass[1].WeaPonName.text = useBrhStr[1].WeaPonName;
	branchClass[1].select.enabled = false;
	branchClass[1].kuang.enabled = false;
	
	switch(brh){
		case 0: 
				branchClass[0].select.enabled = false;
				branchClass[0].kuang.enabled = false;
				branchClass[1].select.enabled = false;
				branchClass[1].kuang.enabled = false;				
			break;
		case 1: 
				branchClass[0].select.enabled = true;
				branchClass[0].kuang.enabled = true;
				branchClass[1].select.enabled = false;
				branchClass[1].kuang.enabled = false;				
			break;
		case 2: 
				branchClass[0].select.enabled = false;
				branchClass[0].kuang.enabled = false;
				branchClass[1].select.enabled = true;
				branchClass[1].kuang.enabled = true;				
			break;
		case 3: 
				branchClass[0].select.enabled = true;
				branchClass[0].kuang.enabled = true;
				branchClass[1].select.enabled = true;
				branchClass[1].kuang.enabled = true;				
			break;
	}
	
	ObjDown.transform.localPosition.y = 1000;
}

//var uiallcl : UIAllPanelControl;
function OpenBranch(){
	loadPlayer();
 	LookPlayerBranch(PlayerLevel , ProID , BranchID);
//	uiallcl.show15();
}

function show0(){
	AllManage.UIALLPCStatic.show0();
}

function loadPlayer(){
	if(InventoryControl.yt.Rows.Count > 0){			
		BranchID = GetBDInfoInt("SkillsBranch" , 0); 
		ProID = GetBDInfoInt("ProID" , 0); 	
		PlayerLevel = GetBDInfoInt("PlayerLevel" , 1);		
	}
}

function GetBDInfoInt(bd : String , it : int) : int{  
	var iii : int = 0;
	try{ 
		iii = parseInt(InventoryControl.yt.Rows[0][bd].YuanColumnText);
		return  iii;
	}catch(e){
		return it;	
	}
}

var useSKstr : String[];
var playerSkillStr : String;
private var Fstr : String = ";";
function realYesXiDian(){
		if(AllManage.SkillCLStatic){
			AllManage.SkillCLStatic.realYesXiDian();
		}else{
			var i : int = 0; 
			playerSkillStr = InventoryControl.yt.Rows[0]["Skill"].YuanColumnText; 
			useSKstr = playerSkillStr.Split(Fstr.ToCharArray());	
			playerSkillStr = "";
			if(useSKstr.Length > 1){
				for(i=0; i<24; i++){ 
					if(useSKstr[i].length >0){
						playerSkillStr += useSKstr[i].Substring(0,1) + "0" + ";";
					}
				}
			}  
			InventoryControl.yt.Rows[0]["Skill"].YuanColumnText = playerSkillStr;
			InventoryControl.yt.Rows[0]["SkillPoint"].YuanColumnText = AllManage.psStatic.Level;		
		}
}
