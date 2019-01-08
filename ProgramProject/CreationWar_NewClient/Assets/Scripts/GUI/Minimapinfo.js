#pragma strict
private var MonsterSp :MonsterSpawn[];
private var MonsterSpt : Transform[];
private var Monsters : GameObject[];
private var teamPlayers : Transform[];

private var NPC: GameObject[];
private var Portal: TriggerLoadLevel[];
private var ExitPortal:TriggerExit;

private var Monsterdot : Transform[];
var reddot : Transform;
var greendot : Transform;
var bluedot : Transform;
var selfdot : Transform;
var ePortal :Transform;
var mPortal :Transform;
private var mcamera :MapCamera;
private var offsetx = 0.0;
private var offsety = 0.0;
private var size = 110;
var LabelZhuobiao : UILabel;

function Start(){
mcamera = FindObjectOfType(MapCamera);
offsetx = mcamera.transform.position.x;
offsety = mcamera.transform.position.z;
   size = mcamera.mapsize;
reddot.localPosition.y = 300;
selfdot.localPosition.y = 300;
MonsterSp = FindObjectsOfType(MonsterSpawn);
UpdateMonster();
GreatReddot();
UpdateNPCposition();
UpdatePortalosition();
CreatTeamDot();
}

function Callmap () {
UpSelfdot();
Updateposition();
UpdateMonster();
Updateteamposition();
}



private var PlayerSpt : Transform[];	
function CreatTeamDot(){
    PlayerSpt = new Array (7);
	for (var i : int = 0; i < 7; i++) {
	PlayerSpt[i] = Instantiate (bluedot);
	PlayerSpt[i].parent = this.transform;
	PlayerSpt[i].localScale = bluedot.localScale;
	PlayerSpt[i].localPosition.y =300; 	
  }
}

function Updateteamposition(){
	if(UIControl.mapType == MapType.jingjichang)
    teamPlayers = FindPVPPlayers();	
    else
    teamPlayers = FindFBPlayers();
    
	var n =0;
	for (var i : int = 0; i < teamPlayers.Length; i++) {	
	  if(teamPlayers[i] != null){	
         PlayerSpt[i].localPosition.x = (teamPlayers[i].position.x-offsetx)*80/size;
         PlayerSpt[i].localPosition.y = (teamPlayers[i].position.z-offsety)*80/size;
         PlayerSpt[i].localPosition.z = 0;
         }
      else
         PlayerSpt[i].localPosition.y =300;    
      }
}

private var gos : PlayerStatus[];
private function FindPVPPlayers () : Transform[]{ 
 var Players : Transform[];
    Players = new Array (7);
    var i = 0;
 if(PlayerStatus.MainCharacter)
  var selfTeamID = PlayerStatus.MainCharacter.GetComponent(PlayerStatus).PVPmyTeam;
  if(selfTeamID !=""){
      var diff:String;
  gos = FindObjectsOfType(PlayerStatus);
for (var go : PlayerStatus in gos) {
    if(go.transform !=PlayerStatus.MainCharacter)
    diff = go.PVPmyTeam; 
    if (diff== selfTeamID){
       Players[i] = go.transform;
       i +=1;
       }
   }
   }
   return Players;
}

private function FindFBPlayers () : Transform[]{ 
 var Players : Transform[];
    Players = new Array (4);
    var i = 0;
 if(PlayerStatus.MainCharacter)
  var selfTeamID = PlayerStatus.MainCharacter.GetComponent(PlayerStatus).TeamID;
  if(selfTeamID !=-1){
   var diff:int;
  gos = FindObjectsOfType(PlayerStatus);
for (var go : PlayerStatus in gos) {
    if(go.transform !=PlayerStatus.MainCharacter)
    diff = go.TeamID; 
    if (diff== selfTeamID){
       Players[i] = go.transform;
       i +=1;
       }
   }
   }
   return Players;
   
}
       
private var k =0;
function	UpdateMonster()
{
	Monsters	=	GameObject.FindGameObjectsWithTag(	"Enemy"	);
    MonsterSpt	=	new	Array( MonsterSp.length );
    k=0;
	for( var i : int = 0; i < MonsterSp.length; i++ )
	{ 
		if(	MonsterSp[i].IsAbleToSpawn() && !MonsterSp[i].IsCleared()	)
		{	
			MonsterSpt[k] = MonsterSp[i].transform;
			k	+=	1;
	    }
    }
	if(	k	==	MonsterSp.length	)
		return;
	for( var n : int = 0; n < Monsters.length; n++ )
	{ 
		if(	k<MonsterSpt.length && MonsterSpt[k]	==	null	&&	Monsters[n]	)
		{	
		    MonsterSpt[k] = Monsters[n].transform;
		    k +=1;
	    }  
    }
}

function UpdateNPCposition(){
	NPC = GameObject.FindGameObjectsWithTag("NPC");
 var NPCdot : Transform[];	
  NPCdot = new Array (NPC.length);
	for (var i : int = 0; i < NPC.length; i++) {
	NPCdot[i] = Instantiate (greendot) ;
	NPCdot[i].parent = this.transform;
	NPCdot[i].localScale = greendot.localScale;	
    NPCdot[i].localPosition.x = (NPC[i].transform.position.x-offsetx)*80/size;
    NPCdot[i].localPosition.y = (NPC[i].transform.position.z-offsety)*80/size;
    NPCdot[i].localPosition.z = 0;
    }
}

function UpdatePortalosition(){
	Portal =FindObjectsOfType(TriggerLoadLevel);
	ExitPortal = FindObjectOfType(TriggerExit); 
var Portalp : Transform;
var Portalh : Transform;
if(Portal.length>=1)
Portalp = Portal[0].transform;
else if(ExitPortal)
Portalp = ExitPortal.transform;
if(Portalp){
ePortal.localPosition.x = (Portalp.position.x-offsetx)*80/size;
ePortal.localPosition.y = (Portalp.position.z-offsety)*80/size;
ePortal.localPosition.z = 0;
}
if(Portal.length>=2)
Portalh = Portal[1].transform;
if(Portalh){
mPortal.localPosition.x = (Portalh.position.x-offsetx)*80/size;
mPortal.localPosition.y = (Portalh.position.z-offsety)*80/size;
mPortal.localPosition.z = 0;
}
}

function GreatReddot(){
   Monsterdot = new Array (MonsterSpt.length);
	for (var i : int = 0; i < MonsterSpt.length; i++) { 
	Monsterdot[i] = Instantiate (reddot);
	Monsterdot[i].parent = this.transform;
	Monsterdot[i].localScale = reddot.localScale;
    }
}

function Updateposition(){
	for (var i : int = 0; i < Monsterdot.length; i++) {
	if(MonsterSpt[i]){	
    Monsterdot[i].localPosition.x = (MonsterSpt[i].position.x-offsetx)*80/size;
    Monsterdot[i].localPosition.y = (MonsterSpt[i].position.z-offsety)*80/size;
    Monsterdot[i].localPosition.z = 0;
    }
    else
    Monsterdot[i].localPosition.y = 200;
    }
}
private var zuobiaox : int;
private var zuobiaoy : int;
function UpSelfdot(){
if(PlayerStatus.MainCharacter){
zuobiaox = PlayerStatus.MainCharacter.position.x;
zuobiaoy = PlayerStatus.MainCharacter.position.z;
selfdot.localPosition.x = (zuobiaox-offsetx)*80/size;
selfdot.localPosition.y = (zuobiaoy-offsety)*80/size;
selfdot.localPosition.y = 0;
selfdot.eulerAngles.z = -PlayerStatus.MainCharacter.eulerAngles.y;
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add("messages122");
//			AllManage.AllMge.Keys.Add(zuobiaox + ",");
//			AllManage.AllMge.Keys.Add(zuobiaoy + "");
//			AllManage.AllMge.SetLabelLanguageAsID(LabelZhuobiao);
LabelZhuobiao.text ="当前位置坐标："+  zuobiaox +"," + zuobiaoy;
}
}
