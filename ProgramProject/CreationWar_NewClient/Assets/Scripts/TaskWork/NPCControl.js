#pragma strict
static var NowGoID : String = "";

function Awake(){
	AllManage.npcclStatic = this;
}

function SetNPCName(yt : yuan.YuanMemoryDB.YuanTable){
	for(var rows : yuan.YuanMemoryDB.YuanRow in yt.Rows){
		for(var npc : npcAI in NPCs){
			if(npc.npcid == rows["NPCID"].YuanColumnText){
				npc.setName(rows["NPCName"].YuanColumnText , rows["NPCTitle"].YuanColumnText , rows["NPCTalk"].YuanColumnText , rows["npcInfo"].YuanColumnText );
			}
		}
//		GUI.Label(Rect(100,100,100,100),"ssss");
	}
}

var NPCs : npcAI[];
function SetNPCTask(gv : GameValue) : GameObject{
//	//print(gv.npcid);
	for(var i=0; i<NPCs.length; i++){
		if(NPCs[i].npcid == gv.npcid){
			NPCs[i].SetTask(gv); 
			return NPCs[i].gameObject;
		}
	}
	return null;
} 

function GetNPCAsID(id : String) : npcAI{
	for(var i=0; i<NPCs.length; i++){
		if(NPCs[i].npcid == id){
			return NPCs[i];
		}
	}
	return null;
}

function GetNPCPosition(id : String) : Vector3{
	for(var i=0; i<NPCs.length; i++){
		if(NPCs[i].npcid == id){
			return NPCs[i].gameObject.transform.position;
		}
	}
}

function NPCGuanBi(id : String){
	for(var i=0; i<NPCs.length; i++){
		if(NPCs[i].npcid == id){
			NPCs[i].GuanBiFuHao();
		}
	}	
}

function SelectOneTaskAsNPC(task : MainTask , id : String){
	for(var i=0; i<NPCs.length; i ++){
		if(NPCs[i].npcid == id){
			NPCs[i].OpenNPCInfo(task);
		}
	}
}
