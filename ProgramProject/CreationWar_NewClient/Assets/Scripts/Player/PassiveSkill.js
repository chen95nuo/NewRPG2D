#pragma strict

@System.Serializable	
																										
class Skillpassive {
    var ID: int;   //
    var explain : String;  
    var name : String;
    var SkillType = 1;  //光环=1,自身值=2,挨打=3,打中=4，杀死=5，其他人死=6，自己死=7，
    var effectID : int;
    var level: int;     //1位 
    var levelattribute : int[]; 
    var buffvalue:int;  
    var aGet : boolean = false;
}

var SkillP : Skillpassive[];

private var selfteamID : int;
private var buffID:int;
var photonView : PhotonView;
var Status:PlayerStatus;
private var Enemytag :String;
var PearlPrefeb: Transform;
private var Pearlp: Transform[];
private var sklC : SkillControl;
private var Branch : String ;
private var aGet : boolean = false;
private var agentmotion : agentLocomotion;

function Awake () {
//Status = GetComponent(PlayerStatus);
//photonView = GetComponent(PhotonView);
if (!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
    return;
    agentmotion	= GetComponent(agentLocomotion);
	useExplain = new Array(SkillP.length);
	for(var o=0; o<SkillP.length; o++){
		useExplain[o] = SkillP[o].explain;
	}
if(this.CompareTag ("Enemy"))
Enemytag = "Player";
else if(this.CompareTag ("Player"))
Enemytag = "Enemy";
selfteamID = Status.TeamID;
//photonView = GetComponent(PhotonView);
sklC = AllManage.SkillCLStatic;
}
function Start () {
if(Status.ProID==3&&PearlPrefeb){
Pearlp = new Array (3);
Pearlp[0] = Instantiate(PearlPrefeb, transform.position+transform.up*3.5-transform.forward*0.6,transform.rotation);
Pearlp[0].transform.parent = transform;
yield WaitForSeconds(0.2);
Pearlp[1] = Instantiate(PearlPrefeb, transform.position+transform.up*3.5-transform.forward*0.6,transform.rotation);
Pearlp[1].transform.parent = transform;
yield WaitForSeconds(0.2);
Pearlp[2] = Instantiate(PearlPrefeb, transform.position+transform.up*3.5-transform.forward*0.6,transform.rotation);
Pearlp[2].transform.parent = transform;
for (var n : int = 0; n < Pearlp.length; n++){
Pearlp[n].gameObject.SetActiveRecursively(false);
}
}
if (!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
    return;

yield WaitForSeconds(1);
 Branch = InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText;
for (var i : int = 0; i < SkillP.length; i++) { 
        Leveattr(i);
        var cansk =(Branch=="1"&&i>=2&&i<=4)||(Branch=="2"&&i>=5&&i<=7);
        if(SkillP[i].SkillType==1 && SkillP[i].level !=0&&cansk)
        Ring(SkillP[i].ID, SkillP[i].buffvalue,i);
        else if((SkillP[i].SkillType==2||SkillP[i].ID==33) && SkillP[i].level !=0&&cansk)
        Changeself(SkillP[i].ID,SkillP[i].buffvalue);
        else if(SkillP[i].ID==34 && SkillP[i].level !=0&&cansk)
        SendMessage("AddskillBuff",SkillP[i].buffvalue,SendMessageOptions.DontRequireReceiver ); 
        yield WaitForSeconds(0.2);       
}

 InvokeRepeating("resetattr", 0, 1); 
}

function resetattr(){
	for (var i : int = 0; i < SkillP.length; i++) { 
        Leveattr(i);
	}
}

function initSkill(){
	if(!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		return;
	AllManage.SkillCLStatic.getSkill1(this);
}

private var useExplain : String[];
private function Leveattr(i : int) {
  SkillP[i].ID = Status.ProID*10+i+1;
   if(SkillP[i].level >0)
   
  SkillP[i].buffvalue = SkillP[i].levelattribute[parseInt(SkillP[i].level.ToString().Substring(0,1))-1];  
  
 	for(var o=0; o<SkillP.length; o++){
		SkillP[i].explain = useExplain[i];
	}
//	if(! SkillP[i].aGet){
		SkillP[i].aGet = true;
		SkillP[i].explain = AllManage.AllMge.Loc.Get(SkillP[i].explain);
//	}
	if(SkillP[i].level == 0){
		SkillP[i].explain = SkillP[i].explain.Replace("#W",0 +"");
	}else{
		SkillP[i].explain = SkillP[i].explain.Replace("#W",SkillP[i].buffvalue +"");
	}
  }
  
var	useSkill : Skillpassive;	//获取信息时用于作为返回值的临时变量//
private function Leveattr1(i : int) : Skillpassive{
	SkillP[i].ID = Status.ProID*10+i+1;
	
	useSkill.level	=	SkillP[i].level; 
	useSkill.ID = SkillP[i].ID;
	if(	useSkill.level == 0	)
	{
		useSkill.level = 10;
	}else{
		useSkill.level	=	(parseInt(useSkill.level.ToString().Substring(0,1)) + 1)*10 + parseInt(useSkill.level.ToString().Substring(1,1));	
	}
   
	useSkill.buffvalue = SkillP[i].levelattribute[parseInt(useSkill.level.ToString().Substring(0,1))-1];  
  
// 	for(var o=0; o<SkillP.length; o++){
//		SkillP[o].explain = useExplain[o];
//	}
	useSkill.explain = useExplain[i];
//	if(! SkillP[i].aGet){
		useSkill.aGet = true;
		useSkill.explain = AllManage.AllMge.Loc.Get(useSkill.explain);
//	}
	if(useSkill.level == 0){
		useSkill.explain = useSkill.explain.Replace("#W",0 +"");
	}else{
		useSkill.explain = useSkill.explain.Replace("#W",useSkill.buffvalue +"");
	}
	return useSkill;
  }
  
function Ring (ID:int,buffvalue:int,i:int) {
   if(ID==11)
   buffID =28;
   else if(ID==21)
   buffID=26;
   else if(ID==26)
   buffID=27;
   else if(ID==31)
   buffID=25;
   ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_PlayEffect, SkillP[i].effectID.ToString() );
   SendMessage("PlayEffect",SkillP[i].effectID , SendMessageOptions.DontRequireReceiver);
//   photonView.RPC("PlayEffect",SkillP[i].effectID);
   var gos : GameObject[];
  	  	   var setArray = new int[4];
            setArray[0]= Status.instanceID;
            setArray[1]= buffID;            
            setArray[2]= buffvalue; 
            setArray[3]= 300;  
   while(true){
	   	if( setArray[1] != 0){
			  gos = GameObject.FindGameObjectsWithTag(this.tag);
			  for (var go : GameObject in gos) {
			      if(go.GetComponent(PlayerStatus) && go.GetComponent(PlayerStatus).TeamID == selfteamID )                           						
				      go.SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver );
				  }
	   	}
	  yield WaitForSeconds(5);
	}
}

function Changeself(ID:int,buffvalue:int){
if(ID==12)
Status.buffint[8] += buffvalue*5;
else if(ID==14)
Status.buffint[3] += buffvalue;
else if(ID==16)
Status.Strength += buffvalue;
else if(ID==15)
Status.buffint[2] += buffvalue;
else if(ID==18)
Status.buffint[1] += buffvalue;
else if(ID==22)
Status.Agility += buffvalue;
else if(ID==23)
Status.buffint[4] += buffvalue;
else if(ID==25)
Status.buffint[0] += buffvalue;
else if(ID==33)
Status.buffint[2] += buffvalue;
else if(ID==35)
Status.buffint[0] += buffvalue*Status.Intellect*0.01;

}

//@RPC
function Behit(){ 
	if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
		fanji();
		if(!Status.battlemod){
			Status.battlemod=true;
			Status.battletime =Time.time;
		}
		SendMessage("DaDuan",SendMessageOptions.DontRequireReceiver);
	//	qiuai.objs = PhotonView.Find(ID).transform;
		Behita();
		if(PlayerAI.AutoAI==false)
			Autoctrl.Wayfinding = false; 
   }
}

function fanji(){
		if(agentmotion.enabled){
			yield WaitForSeconds(0.5);
			if(! AllManage.pAIStatic.AutoAI)
				AllManage.pAIStatic.AutoAttackSimple();
		}
}

function Behita(){
//print("BeHit///////");
        if(SkillP[2].ID==13 && SkillP[2].level !=0 && (Branch=="1"||Branch=="3")&&Time.time > hittime+2){
                hittime = Time.time;
           allhit(3,SkillP[2].buffvalue);
  			ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_PlayEffect, SkillP[2].effectID.ToString() );
			SendMessage("PlayEffect",SkillP[2].effectID , SendMessageOptions.DontRequireReceiver);
//        photonView.RPC("PlayEffect",SkillP[2].effectID);
        }
        else if(SkillP[2].ID==33 && SkillP[2].level !=0  && (Branch=="2"||Branch=="3") &&Time.time > hittime+2){
//        print("BeHit///////ApplyBuff");
        hittime = Time.time;
        var setArray = new int[4];
            setArray[0]= Status.instanceID;
            setArray[1]= 9;            
            setArray[2]= 40; 
            setArray[3]= 3;  
 	    var colliders : Collider[] = Physics.OverlapSphere ( transform.position,4);   
	    for (var hit in colliders) {
		if(hit.CompareTag (Enemytag))
		hit.SendMessageUpwards("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver );			
	    }          
        }  
        else if(SkillP[6].ID==37 && Status.Pearl>0 && SkillP[6].level !=0 && (Branch=="1"||Branch=="3")&&Time.time > hittime+2 ){
        hittime = Time.time;
        Status.AddHealth(SkillP[6].buffvalue);
   			ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_PlayEffect, SkillP[6].effectID.ToString() );
			SendMessage("PlayEffect", SkillP[6].effectID , SendMessageOptions.DontRequireReceiver);
//        photonView.RPC("PlayEffect",SkillP[6].effectID);
        }
        if(Status.ProID==1)
        Status.AddMana(2);
}


function Kills(){
        if(SkillP[6].ID==17 && SkillP[6].level !=0 && (Branch=="2"||Branch=="3") ){
  			ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_PlayEffect, SkillP[6].effectID.ToString() );
			SendMessage("PlayEffect", SkillP[6].effectID , SendMessageOptions.DontRequireReceiver);
//          photonView.RPC("PlayEffect",SkillP[6].effectID);
          Status.buffint[0] += SkillP[6].buffvalue;
          yield WaitForSeconds(5);
          Status.buffint[0] -= SkillP[6].buffvalue;
        } 
        else if (SkillP[6].ID==27 && SkillP[6].level !=0 && (Branch=="2"||Branch=="3") )
          Status.AddHealth(SkillP[6].buffvalue);  
}

private var hittime = 0.0;
function Hit(target:Transform){
if(Status.ProID==2&&Time.time > hittime+2){
        hittime = Time.time;
        if(SkillP[3].ID==24 && SkillP[3].level !=0 && (Branch=="1"||Branch=="3") )
        Status.AddHealth(SkillP[3].buffvalue);
        else if(SkillP[7].ID==28 && SkillP[7].level !=0 && (Branch=="2"||Branch=="3") )
        givehit(target,Status.ATK*SkillP[7].buffvalue*0.01);
     }

}

private function givehit( hit:Transform,buffvalue:int){
          yield WaitForSeconds(0.3);
	     var settingsArray = new int[5];
			settingsArray[0]=Status.instanceID;
			settingsArray[1]=buffvalue;
			settingsArray[2]=buffvalue;
			settingsArray[3]= 1;
			settingsArray[4]= parseInt(Status.Level); 
		if(hit)
		hit.SendMessageUpwards("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );			 
}

private function allhit(radius:int,buffvalue:int){
 	var colliders : Collider[] = Physics.OverlapSphere ( transform.position,radius);
	     var settingsArray = new int[5];
			settingsArray[0]=Status.instanceID;
			settingsArray[1]=buffvalue;
			settingsArray[2]=buffvalue;
			settingsArray[3]= 1;
			settingsArray[4]=parseInt( Status.Level); 
	for (var hit in colliders) {
		if(hit.CompareTag (Enemytag))
		hit.SendMessageUpwards("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );			
	}  
}


function MonsterDie(){
        if(SkillP[1].ID==32 &&Status.Pearl<= SkillP[1].buffvalue  && SkillP[1].level !=0){
         Status.Pearl +=1;
         GhostPearl(Status.Pearl);
  			ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_PlayEffect, SkillP[1].effectID.ToString() );
			SendMessage("PlayEffect", SkillP[1].effectID , SendMessageOptions.DontRequireReceiver);
//        photonView.RPC("PlayEffect",SkillP[1].effectID);         
         }
        else if(SkillP[5].ID==36 && SkillP[5].level !=0 && (Branch=="2"||Branch=="3") )
        Status.AddMana(SkillP[5].buffvalue);
}

 function GhostPearl(i:int){
for (var n : int = 0; n < Pearlp.length; n++){
     if(n < i)
     Pearlp[n].gameObject.SetActiveRecursively(true);
     else   
     Pearlp[n].gameObject.SetActiveRecursively(false);
  }
}

function SelfDie(){
if(SkillP[7].ID==38 && SkillP[7].level !=0 && (Branch=="1"||Branch=="3") ){
        var setArray = new int[4];
            setArray[0]= Status.instanceID;
            setArray[1]= 18;            
            setArray[2]= SkillP[7].buffvalue; 
            setArray[3]= 2; 
  			ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_PlayEffect, SkillP[7].effectID.ToString() );
			SendMessage("PlayEffect", SkillP[7].effectID , SendMessageOptions.DontRequireReceiver);
//			photonView.RPC("PlayEffect",SkillP[7].effectID);
        var colliders : Collider[] = Physics.OverlapSphere ( transform.position,15);
	for (var hit in colliders) {
		if(hit.tag==this.tag) 
       hit.SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver);		
         }
       }
}

function SetSkillLevel(str : String[]){
	var i : int = 0;
	for(i=0; i<SkillP.length; i++){
		SkillP[i].level = parseInt(str[i+15]);
	}
	 Branch = InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText;
	switch(Branch){
		case "0" :
			SkillP[2].level = 0;
			SkillP[3].level = 0;
			SkillP[4].level = 0;
			SkillP[5].level = 0;
			SkillP[6].level = 0;
			SkillP[7].level = 0;
			break;
		case "1" :
			SkillP[5].level = 0;
			SkillP[6].level = 0;
			SkillP[7].level = 0;
			break;
		case "2" :
			SkillP[2].level = 0;
			SkillP[3].level = 0;
			SkillP[4].level = 0;
			break;
	}
}

function GetSkillexplainAsID(id : int){
	id -= 16;
		Leveattr(id);
		useSkill = new Skillpassive();
			if(SkillP[id].level.ToString().Substring(0,1) != "3"){
				useSkill = Leveattr1(id); 
				if(SkillP[id].level.ToString().Substring(0,1) == "0")
				{
					useSkill.explain = AllManage.AllMge.Loc.Get("info839")+": \n" + AllManage.AllMge.Loc.Get("info689") + "\n [ffff00]" + AllManage.AllMge.Loc.Get("info840") + ": \n"+ useSkill.explain;				
				}
				else
				{
					useSkill.explain = AllManage.AllMge.Loc.Get("info839") + ": \n" + SkillP[id].explain +"\n [ffff00]" + AllManage.AllMge.Loc.Get("info840") + ": \n"+ useSkill.explain;		
				}
			}else{
				useSkill.explain = AllManage.AllMge.Loc.Get("info839") + ": \n" + SkillP[id].explain;			
			}
	return useSkill;
}

function GetSkillInfoAsID(id : int){
	id -= 16;
	return SkillP[id];
}
