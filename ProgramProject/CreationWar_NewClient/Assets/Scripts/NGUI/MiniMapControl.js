#pragma strict

var npccl : NPCControl;
function Start () {
	
	npccl = FindObjectOfType(NPCControl);
	if(AllManage.mtwStatic.chuansongObj){
		itemChuanSong.localPosition.y = 0;
	}else{
		itemChuanSong.localPosition.y = 3000;		
	}
	if(AllManage.mtwStatic.chuansongObj1){
		itemChuanSong1.localPosition.y = 0;
	}else{
		itemChuanSong1.localPosition.y = 3000;		
	}
	
//	SetNPCList("");
}


var ebItem : MiniMapItem;
var invItemArray : MiniMapItem[];
var invParent : Transform; 
var GID : UIGrid;
var wuneirong : GameObject;
var LabelMapName : UILabel;
var monsterSP : MonsterSpawn[];
var LabelInfo : UILabel;
var mapdot:Minimapinfo;
var itemChuanSong : Transform;
var itemChuanSong1 : Transform;
var ObjChannel : GameObject;
function SetNPCList(equStr : String){
     mapdot.Callmap();
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages079");
			AllManage.AllMge.Keys.Add(DungeonControl.NowMapLevel + "\n");
			AllManage.AllMge.Keys.Add("messages080");
			AllManage.AllMge.Keys.Add(AllManage.dungclStatic.getMapLevel(MainTaskWork.MapID , DungeonControl.NowMapLevel) + "\n");
			AllManage.AllMge.Keys.Add(AllManage.dungclStatic.getMapInfo(MainTaskWork.MapID) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelInfo);
//	LabelInfo.text = "难度:"+ DungeonControl.NowMapLevel +"\n建议等级：" + AllManage.dungclStatic.getMapLevel(MainTaskWork.MapID , DungeonControl.NowMapLevel) + "\n" + AllManage.dungclStatic.getMapInfo(MainTaskWork.MapID);
	LabelMapName.text = DungeonControl.MapName;
	invClear();
	if(AllManage.mtwStatic.chuansongObj){
		itemChuanSong.localPosition.y = 0;
	}else{
		itemChuanSong.localPosition.y = 3000;		
	}
	if(AllManage.mtwStatic.chuansongObj1){
		itemChuanSong1.localPosition.y = 0;
	}else{
		itemChuanSong1.localPosition.y = 3000;		
	}
	var i : int = 0;
	var Obj : GameObject ;
	var useEBI : MiniMapItem;
	if(UIControl.mapType == MapType.zhucheng){
		for(i=0; i<npccl.NPCs.length; i++){
				Obj = Instantiate(ebItem.gameObject); 
	//			wuneirong.transform.localPosition.y = 3000;
				Obj.transform.parent = invParent;
				useEBI = Obj.GetComponent(MiniMapItem);
				useEBI.setInv(npccl.NPCs[i]);
				useEBI.ebCL = this; 
				addInvItem(useEBI);				
		}
	}else{
		monsterSP = FindObjectsOfType(MonsterSpawn);
		for(i=0; i<monsterSP.length; i++){
			if(monsterSP[i].spType == SpawnPointType.boss1 || monsterSP[i].spType == SpawnPointType.boss2){
				Obj = Instantiate(ebItem.gameObject); 
	//			wuneirong.transform.localPosition.y = 3000;
				Obj.transform.parent = invParent;
				useEBI = Obj.GetComponent(MiniMapItem);
				useEBI.setInv(monsterSP[i]);
				useEBI.ebCL = this; 
				addInvItem(useEBI);	
			}			
		}	
	}
	yield;
	GID.repositionNow = true;
}

var  myEBI : MiniMapItem;
function SelectOneInv(ebi : MiniMapItem){
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
		if(invItemArray[i] == ebi){  
			myEBI = ebi;
			invItemArray[i].SelectMe();
		}else{
			invItemArray[i].DontSelectMe();		
		}
	}
}

function addInvItem(ebi : MiniMapItem){
	var use : MiniMapItem[]; 
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

function GoNPCWay(pos : Vector3){
	AllManage.mtwStatic.FindWay(pos);
	KaoYuExit();
}

function KaoYuExit(){
	AllManage.UIALLPCStatic.show0();
}

function GoPutong(){
	AllManage.mtwStatic.Gochuansong();
	AllManage.UIALLPCStatic.show0();
}

function GoJingYing(){
	AllManage.mtwStatic.Gochuansong1();
	AllManage.UIALLPCStatic.show0();
}
var isclick : boolean = true;
function SelectChannel(){
		if(isclick){
		ObjChannel.SetActive(true);
		ObjChannel.transform.localPosition.y = 0;
		isclick = false;
		}else
		if(ObjChannel.active==false&&!isclick){
			ObjChannel.SetActive(true);
//			ObjChannel.transform.localPosition.y = 0;
			}else{
			ObjChannel.SetActive(false);
//			ObjChannel.transform.localPosition.y = 3000;
			}
	}
