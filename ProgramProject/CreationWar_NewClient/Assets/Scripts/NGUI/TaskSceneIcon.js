#pragma strict

var task : MainTask;
var mtw : MainTaskWork;
var mapID : String;
var dungcl : DungeonControl;
var MapNandu : int;
function SelectThisMap(){
	mtw.SelectOneMap(mapID , MapNandu);  
	mtw.ChuansongRightBan.SetActiveRecursively(false);
	yield;
	TaskSceneIcon.LeftLevel = LabelMapName.text;
}
function Awake()
{
AllManage.stictaskSceneIcon = this;
}

var ButtonZuDui : GameObject;
function CloseButtonZuDui(){
	yield;
	yield;
	yield;
	ButtonZuDui.SetActiveRecursively(false);
}

var iconSprite : UISprite;
var LabelMapName : UILabel;
function SetMap(str : String , name : String , nandu : int){
//	//print(str + " == " + name + " == " + gameObject.name);
	MapNandu = nandu;
	mapID = str;
	if(mapID.Substring(0,1) == "1")
	LabelMapName.text ="[00ff00]"+name;
	else if (mapID.Substring(0,1) == "4")
	LabelMapName.text ="[ff0000]"+name;
	else if(mapID.Substring(0,1) == "7")
	LabelMapName.text ="[ff0000]"+name;
	else
	LabelMapName.text = "[ff8c00]"+name;
	iconSprite.spriteName = str.Substring(0,2);
//								if(mapID.Substring(0,1) == "1"){
//									transform.localPosition.y = 2000;
//								}else{
//									transform.localPosition.y = -40;
//								}
}

var nandu : int;
var LabelTuijian :UILabel; 
function SetRigthMap(str : String , nd : int){
	if(dungcl == null){
		dungcl = FindObjectOfType(DungeonControl);
	}
	
	mapID = str;
	nandu = nd;
//	if(nd == 5){
//		LabelMapName.text = dungcl.getMapName(mapID).Replace("1" , "");
//	}else{
//		LabelMapName.text = dungcl.getMapName(mapID);	
//	}
//	SelectOneRightMap();
//	mapInfo = dungcl.getMapInfo(mapID);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages080");
			AllManage.AllMge.Keys.Add(dungcl.getMapLevel(mapID,nandu) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelTuijian);
//	LabelTuijian.text = "建议等级：" + dungcl.getMapLevel(mapID,nandu);
//	iconSprite.spriteName = str.Substring(0,2);
//	SelectOneRightMap();
}

var mapInfo : String;
var LabelMapInfo : UILabel;
function SelectOneRightMap(){
		if(!mtw){
			mtw = FindObjectOfType(MainTaskWork);
		}
	mtw.SelectMapRigthAsID(mapID , nandu , this);
	
	if(nandu==1){
			mtw.star.Difficulty(0);
			if(LabelMapInfo){
			LabelMapInfo.enabled = true;
				}
	}else
	if(nandu==5){
			mtw.star.Difficulty(1);
			if(LabelMapInfo){
			LabelMapInfo.enabled = false;
				}
	}else
	if(nandu==2){
			mtw.star.Difficulty(2);
			if(LabelMapInfo){
			LabelMapInfo.enabled = true;
				}
	}
	mtw.star.RefreshBtns();
	
}

private var ps : PlayerStatus;
//var ts : TiShi; 
static var LeftLevel : String; 
var mapNameList : String[];
var YBM : BtnGameManager;
var StrID : String;
function GoLevel(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	var costPower : int = 0;
	costPower = AllResources.GetMapCostPower(mapID , nandu);
//	if(parseInt(ps.Power) >= costPower || mapID.Substring(0,1) == "1"){   
		if(!mtw){
			mtw = FindObjectOfType(MainTaskWork);
		}
		if(!YBM){
			 YBM = FindObjectOfType(BtnGameManager);
		}
		YBM.selectTempListName = mapID + ","+ nandu;
		mtw.SelectYouMapItem(mapID , nandu);
		YBM.GetTempTeam();
		yield;
		yield;
		yield;
		mtw.ChuansongRightBan.SetActiveRecursively(true);
		AllManage.UICLStatic.RefreshTempTeamButton();

//	}else{
//		var Istrue : boolean = false;
//		var StrItem = InventoryControl.yt.Rows[0]["Item"].YuanColumnText;
//		var str : String[] = StrItem.Split(";"[0]); 
//		for(var j= 0 ; j<str.Length ; j++)
//		{
//			if(str[j]!="")
//				
//				if(str[j].Substring(0,3).Equals("884")){
//					StrID = str[j];
//					Istrue = true;
//					AllManage.qrStatic.ShowQueRen(gameObject,"UseDaoju" , "","info1119");
//				}else{
//				
//				}
//				if(!Istrue){
//					AllManage.qrStatic.ShowQueRen(gameObject,"UseShangdian" , "","info1120");
//				}
//		}
//	}
}

function GoLevelTipsPower(isBool : boolean){
	if(isBool){
		realGoLevel();
	}else{
		var Istrue : boolean = false;
		var StrItem = InventoryControl.yt.Rows[0]["Item"].YuanColumnText;
		var str : String[] = StrItem.Split(";"[0]); 
		for(var j= 0 ; j<str.Length ; j++)
		{
			if(str[j]!="")
				if(str[j].Substring(0,3).Equals("884")){
					StrID = str[j];
					Istrue = true;
					AllManage.qrStatic.ShowQueRen(gameObject,"UseDaoju" , "","info1119");
				}
				else{
				
				}
				if(!Istrue){
					AllManage.qrStatic.ShowQueRen(gameObject,"UseShangdian" , "","info1120");
				}
		}
		
	}
}

function realGoLevel(){
	if(AllManage.InvclStatic.isBagFull() && !AllManage.UICLStatic.boolFullBag && mapID.Substring(0,1) != "1"){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info885"));
	}else{
		ClickMapNow();
	}
}

function GoLevelLeft(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	var costPower : int = 0;
	costPower = AllResources.GetMapCostPower(mapID , nandu);
	if(mapID.Substring(0,1) == "1"){
		realGoLevel();
	}else{
		if(nandu == 5){
			AllManage.AllMge.TipsNewPower(yuan.YuanPhoton.CostPowerType.EliteDungeon , nandu , 0 , "" , gameObject , "GoLevelTipsPower");		
		}else{
			AllManage.AllMge.TipsNewPower(yuan.YuanPhoton.CostPowerType.NormalDungeon , nandu , 0 , "" , gameObject , "GoLevelTipsPower");					
		}

//		if(parseInt(ps.Power) >= costPower || ){
//	//		if(AllManage.InvclStatic.isBagFull() && !AllManage.UICLStatic.boolFullBag && mapID.Substring(0,1) != "1"){
//	//			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info885"));
//	//		}else{
//	//			ClickMapNow();
//	//		}
//		}else{
//	//	        			var Istrue : boolean = false;
//	//						var StrItem = InventoryControl.yt.Rows[0]["Item"].YuanColumnText;
//	//						var str : String[] = StrItem.Split(";"[0]); 
//	//						for(var j= 0 ; j<str.Length ; j++)
//	//						{
//	//						if(str[j]!="")
//	//							
//	//							if(str[j].Substring(0,3).Equals("884")){
//	//							StrID = str[j];
//	//							Istrue = true;
//	//							AllManage.qrStatic.ShowQueRen(gameObject,"UseDaoju" , "","info1119");
//	//							}
//	//						else{
//	//							
//	//							}
//	//					if(!Istrue){
//	//							AllManage.qrStatic.ShowQueRen(gameObject,"UseShangdian" , "","info1120");
//	//					}
//	//							
//	//						}
//		}
	}
}

function UseDaoju()
{
		AllManage.InvclStatic.UseDaojuAsID(StrID);
}

function UseShangdian()
	{
		AllManage.UICLStatic.StoreOpenMoveOn();
	}
	
function ClickMapNow(){
       
		if(nandu==2){
//		ClickMapYes();
//		PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().getHuntingMap(yuan.YuanPhoton.UseMoneyType.TipsHuntingUp,parseInt(AllManage.dungclStatic.getMapLevel(mapID , nandu)),2),"");
		AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsHuntingUp , parseInt(AllManage.dungclStatic.getMapLevel(mapID , nandu)),3, "" , gameObject , "ShowTipsUseMonsy");
//		Debug.Log("ran=================================================++++++"+nandu.ToString());
		}else{
		RealGoLevelLeft();
			}
	}
	
	function ShowTipsUseMonsy(objs : Object[])
	{
	 	var str : String ;
		str = String.Format("{0} {1} {2}",AllManage.AllMge.Loc.Get("info1181"),objs[2],AllManage.AllMge.Loc.Get("info1182"));
		AllManage.qrStatic.ShowQueRen(gameObject,"ClickMapYes","ClickMapNo",str);
	}
	
	function ClickMapYes()
	{
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().getHuntingMap(parseInt(dungcl.getMapLevel(mapID,nandu)),mapID,nandu,3));
//		RealGoLevelLeft();
	}
	function ClickMapNo()
	{
		
	}

function RealGoLevelLeft(){
		if(UIControl.mapType == MapType.zhucheng)

		DungeonControl.ReLevel = Application.loadedLevelName;
		DungeonControl.NowMapLevel = nandu;
		Loading.Level = "Map" + mapID;
//		Debug.Log("ran================================================="+nandu.ToString());
		Loading.nandu = nandu.ToString();
		alljoy.DontJump = true;
		yield;
		AllManage.UICLStatic.teamHeadMapName = Loading.Level + Loading.nandu;
//		AllManage.UICLStatic.DuiZHangGoFB(Loading.Level + Loading.nandu);
		PhotonNetwork.LeaveRoom();
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");	

}

function RealGoLevelLeft1(strMapId : String , mapNandu : int){
		if(UIControl.mapType == MapType.zhucheng)

		DungeonControl.ReLevel = Application.loadedLevelName;
		DungeonControl.NowMapLevel = mapNandu;
		Loading.Level = "Map" + strMapId;
//		Debug.Log("ran================================================="+nandu.ToString());
		Loading.nandu = mapNandu.ToString();
		alljoy.DontJump = true;
		yield;
		AllManage.UICLStatic.teamHeadMapName = Loading.Level + Loading.nandu;
//		AllManage.UICLStatic.DuiZHangGoFB(Loading.Level + Loading.nandu);
		PhotonNetwork.LeaveRoom();
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");	

}

public function desP(){
	while(PhotonNetwork.isMasterClient){
		yield;
	}
	PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
}

var tanhao : UISprite;
function OpenTanHao(bool : boolean){
	if(bool){
		tanhao.enabled = true;
	}else{
		tanhao.enabled = false;	
	}
}

var SpriteisSelect : UISprite;
var parti1 : GameObject;
var parti2 : GameObject;
function OpenIsSelect(bool : boolean){
	if(!SpriteisSelect){
		return;
	}
	if(bool){
		if(parti1){
			parti1.active = true;
			parti2.active = true;
		}
		SpriteisSelect.enabled = true;
	}else{
		if(parti1){
			parti1.active = false;
			parti2.active = false;
		}
		SpriteisSelect.enabled = false;	
	}
}