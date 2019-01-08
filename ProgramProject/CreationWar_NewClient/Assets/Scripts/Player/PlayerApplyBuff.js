#pragma strict
@System.Serializable														
class BuffStatus {
    var buffID = 0;
    var buffvalue = 0; 
    var bufftime :float;
    var off = false;
    var breako = true;
}

var buff : BuffStatus[];

private var pnumber :int;
var bb=false;
private var cc=false;
private var aa=true;
var houtui = 0.0;
private var high = 0.0;
private var move: Vector3;

private var resistyun = false;
private var resistfei = false;
private var resistbing = false;
private var resistjian = false;
private var Animatc:ThirdAnimation; 
private var photonView : PhotonView;
private var controller : CharacterController;
private var TController : ThirdPersonController; 
private var Status:PlayerStatus;

function Awake(){
	Animatc = GetComponent(ThirdAnimation);
photonView = GetComponent(PhotonView);
Status = GetComponent(PlayerStatus);
controller = GetComponent(CharacterController);
TController = GetComponent(ThirdPersonController);
outterColor =  new Array (3);
outterColor[0]=Color(1, 0, 0, 1);
outterColor[1]=Color(1, 0.7, 0, 1);
outterColor[2]=Color(0, 0.7, 1, 1);
buff = new Array (8);
	for (var i : int = 0; i < 8; i++) { 	
	    buff[i]=new BuffStatus();
	}
}

function Start(){
	tempscale=transform.localScale.y;
	InvokeRepeating("reset", 1, 1); 
}

function reset(){
	for (var i : int = 0; i < 8; i++) {
    if(buff[i].buffID!=0)
    return;
    }
    if(!Status.dead){
  if(!Status.ridemod)  
 TController.Movespeed =1;
 Status.Attackspeed =1;   
 }
}

private var Buffbusy = false;
function ApplyBuff(info : int[]){

if(!Buffbusy){
	if(info[0] > 0){
		//只有击退是先走网络，所以只能这么写了
		if(Status.isMine)
		if(info[1] == 4 ||
	   	    info[1] == 5 ||
	   		info[1] == 6 ||
	   		info[1]== 37)
	   {
	   		var go : GameObject = AllResources.ar.FindGameObjectByInstanceID(info[0]);
	   		if(go)
	   		{
	   			var selfPos = transform.position;
	   			selfPos.y = 0;
	   			var enemyPos = go.transform.position;
	   			enemyPos.y = 0;
	   		
	   			var direction : Vector3 = (selfPos - enemyPos).normalized;
	   			ServerRequest.requestForceMove(Status.instanceID, direction, info);
	   		}
	   		
	   		return;
	   }
	}
//	photonView.RPC("leachBuff",info); 
	leachBuff(info);
//	print(String.Format("{0};{1};{2};{3}", info[0] , info[1] , info[2] , info[3]) + " ====================== SendBuff");
	ServerRequest.syncAct(Status.instanceID , 	CommonDefine.ACT_ApplyBuff, String.Format("{0};{1};{2};{3}", info[0] , info[1] , info[2] , info[3]));
	
//	SendMessage("SetNowBuff" , info , SendMessageOptions.DontRequireReceiver);
}
}



//@RPC
function leachBuff(info : int[]){
//	if(ObjectAccessor.getAOIObject(info[0]) != null){
//		return;
//	}
//	print(String.Format("{0};{1};{2};{3}", info[0] , info[1] , info[2] , info[3]) + " ====================== ApplyBuff");
 Buffbusy = true;
    if(info.Length<4)
    return;
    pnumber = info[0];
	for (var i : int = 0; i < 8; i++) { 
		if(info[1]==33){
			if(i == 7){
				buff[i].buffID = info[1];
				buff[i].buffvalue = info[2];
				buff[i].bufftime = info[3];
				ReceiveBuff(i);
			}
		}
		else if(( info[1]==buff[i].buffID && info[2] >= buff[i].buffvalue)|| info[1]==19 ||info[1]==20){  //相同buffID的，kill掉前面的buff后执行
          buff[i].off = !buff[i].off;
          buff[i].buffID = info[1];
          buff[i].buffvalue = info[2];
          buff[i].bufftime = info[3];
          yield;
          try{
          		ReceiveBuff(i);
          }catch(e){          
          }
          break;   
         }
       else if(buff[i].buffID==0 ){    //不同buffID的可以执行
          buff[i].buffID = info[1];
          buff[i].buffvalue = info[2];
          buff[i].bufftime = info[3];
          try{
          		ReceiveBuff(i);
          }catch(e){          
          }
          break;
         }
      }
 Buffbusy = false;
}
function ReceiveBuff (i : int)
{		var info = new int[4];
            info[0]= i;
            info[1]= buff[i].buffID;            
            info[2]= buff[i].buffvalue; 
            info[3]= buff[i].bufftime;  
// //print("buffID ====" + info[1]+", buffvalue ===="+info[2] + ", bufftime ===="+info[3]);
  			ServerRequest.syncAct(Status.instanceID , CommonDefine.ACT_PlaybuffEffect, String.Format("{0};{1};{2};{3}", info[0] , info[1] , info[2] , info[3]));
  			SendMessage("PlaybuffEffect", info , SendMessageOptions.DontRequireReceiver);
//photonView.RPC("PlaybuffEffect",info);       
 if(buff[i].buffID==1)
   setyunself(i);
 else if(buff[i].buffID==2||buff[i].buffID==3){
   setbingself(i);  
   wudiself(i);
   } 
 else if(buff[i].buffID==4)
    jitui(buff[i].buffvalue,1,i,false,false);
 else if(buff[i].buffID==5)
    jitui(buff[i].buffvalue,buff[i].bufftime,i,true,false); 
 else if(buff[i].buffID==6)
    jitui(buff[i].buffvalue,buff[i].bufftime,i,true,true);  
       
 else if(buff[i].buffID==7||buff[i].buffID==8){ 
    if(resistbing){
     AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
     return;
    } 
    info[1]=4;
    info[2]=1;   
   addattribute(info); 
     }
 else if(buff[i].buffID==9||buff[i].buffID==10||buff[i].buffID==19){
    info[1]=4; 
   addattribute(info); 
    } 
  else if(buff[i].buffID==25){    
  info[1]=0; 
   addattribute(info);  
  }
  else if(buff[i].buffID==27){    
       info[1]=1;
   addattribute(info);  
  }
  else if(buff[i].buffID==29||buff[i].buffID==28){    
       info[1]=8;
   addattribute(info);   
  }
  else if(buff[i].buffID==30){    
       info[1]=7;
   addattribute(info); 
  }    
 else if((buff[i].buffID>=11&&buff[i].buffID<=16)||(buff[i].buffID>=35 && buff[i].buffID<=38))
   healthdebuff(i);
 else if(buff[i].buffID==17)
   healthhot(i); 
  else if(buff[i].buffID==18){
   buff[i].bufftime=1;
   healthhot(i);  
  }  
  else if(buff[i].buffID==20){
     info[1]=3;
     addattribute(info);       
   } 
  else if(buff[i].buffID==21){
       info[1]=1;
     addattribute(info); 
   ChangeColor(0,buff[i].bufftime); 
   ChangeSize(1.1,buff[i].bufftime); 
   Resistyun(buff[i].bufftime);
   Resistbing(buff[i].bufftime);
  }
  else if(buff[i].buffID==22){
      info[1]=2;
      addattribute(info);  
   ChangeColor(1,buff[i].bufftime); 
   ChangeSize(1.2,buff[i].bufftime);
    Resistfei(buff[i].bufftime);

 
  }
  else if(buff[i].buffID==23)
   Shadowmode(i);
  else if(buff[i].buffID==24)  
       Impulse(i); 

  else if(buff[i].buffID==26){
       info[1]=4;
      addattribute(info);        
  }  
  
  else if(buff[i].buffID==31)  
   dunqiang(i);    
  else if(buff[i].buffID==32){    
       info[1]=1;
      addattribute(info); 
//   ChangeColor(0,buff[i].bufftime);    
  } 
  else if(buff[i].buffID==33) 
   wudiself(i);   
  else if(buff[i].buffID==34){    
      info[1]=9;
      addattribute(info); 
      }
  else if(buff[i].buffID==39){    
   info[1]=5;
   addattribute(info); 
   ChangeColor(0,buff[i].bufftime); 
   Resistbing(buff[i].bufftime);
   Resistjian(buff[i].bufftime);
  
  } 
  else if(buff[i].buffID==40){ 
        info[1]=6;
      addattribute(info); 
 }
  else if(buff[i].buffID==41){ 
        info[1]=10;
      addattribute(info); 
 }
  else if(buff[i].buffID==42){ 
        info[1]=11;
      addattribute(info); 
 }
  
 if(buff[i].buffID==12||buff[i].buffID==35||buff[i].buffID==36||buff[i].buffID==38){    
       info[1]=4;
       info[2]=40;
      addattribute(info);  
   }  
 else if(buff[i].buffID==37)
    jitui(8,8,i,true,true);  
 else if(buff[i].buffID==16 && PlayerUtil.isMine(Status.instanceID)){
  yield WaitForSeconds(buff[i].bufftime);
  			ServerRequest.syncAct(Status.instanceID , CommonDefine.ACT_PlayskillEffect, "38" );
  			ServerRequest.syncAct(Status.instanceID , CommonDefine.ACT_BigBoom, String.Format("{0};{1};{2}", buff[i].buffvalue*2,this.tag,10) );
  			PlayskillEffect(38);
			bigBoom(buff[i].buffvalue*2,this.tag,10);
//   photonView.RPC("PlayskillEffect",38);
//   photonView.RPC("bigBoom",buff[i].buffvalue*2,this.tag,10);
//   bigBoom(buff[i].buffvalue*2);
  } 
  	 
}

private var jituiPos : Vector2 = Vector2.zero;

function ReciveJitui(objects : Object[])
{
	var i :int;
	var info : int[];
	jituiPos = objects[0];
	info = objects[1];
	
//	Debug.Log("InstanceID " + info[0] + "PosX :" + jituiPos.x + "PosY : " + jituiPos.y);
	
	leachBuff(info);
}

var canTimer : boolean = false;
function Timer (i:int) { 
	canTimer = true;
   while (buff[i].bufftime>=0 && canTimer) {  
    buff[i].bufftime -= Time.deltaTime;         
    yield; 
    if(buff[i].breako==buff[i].off){
    buff[i].breako = !buff[i].off;
    return;
    }
   }
}

function ClearBuff(){
	canTimer = false;
	for(var i=0; i<buff.length; i++){
		if(buff[i] != null){
			buff[i].buffID = 0;
			buff[i].buffvalue = 0;
			buff[i].bufftime = 0;
		}
	}
}

function Shadowmode(i : int) {
  Status.Hide = true;
  yield Timer(i);
  Status.Hide = false;
  buff[i].buffID = 0;
}


function Impulse(i : int) {
while (buff[i].bufftime>=0 ){
   var tempmana = parseInt(Status.Mana);
   tempmana +=20*buff[i].buffvalue*0.01;
   Status.Mana = tempmana.ToString();
   if(tempmana>=parseInt(Status.Maxmana))
     Status.Mana=Status.Maxmana;
	 yield WaitForSeconds(2);
	 buff[i].bufftime -=2;
	if(buff[i].breako==buff[i].off){
    buff[i].breako = !buff[i].off;
    break;
    }
 }
  buff[i].buffID = 0; 
}

function ReturnBigBoom(sts : String[]){
	bigBoom(parseInt(sts[0]) , sts[1] , parseInt(sts[2]));
}

@RPC
function bigBoom(buffvalue:int,Enemytag:String,r:int){
if(PlayerUtil.isMine(Status.instanceID)){
 			ServerRequest.syncAct(Status.instanceID , CommonDefine.ACT_Huandon, String.Format("{0};{1}" , "3" , AllResources.ar.Vector3ToString(transform.position)));
			huandon(3,transform.position);
//   photonView.RPC("huandon",3,transform.position);
 	var colliders : Collider[] = Physics.OverlapSphere ( transform.position, r);
	for (var hit in colliders) {
		var closestPoint = hit.ClosestPointOnBounds(transform.position);
		var distance = Vector3.Distance(closestPoint, transform.position);
		var hitPoints = 1.0 - Mathf.Clamp01(distance / 10);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
		hitPoints *= buffvalue*5;
		if(hit.CompareTag (Enemytag)){
	     var settingsArray = new int[5];
			settingsArray[0]=pnumber;
			settingsArray[1]=hitPoints;
			settingsArray[2]=hitPoints;
			settingsArray[3]= 3;
			settingsArray[4]=parseInt( Status.Level); 
		hit.SendMessageUpwards("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );			
		}
	} 
   } 
}

function Retrunhuandon(strs : String[]){
	try{
		huandon(parseInt(strs[0]) , Vector3(parseInt(strs[1]) , parseInt(strs[2]) , parseInt(strs[3])));
	}catch(e){
	
	}
}

@RPC
function huandon(ID:int,position:Vector3){
	newCamera.huandong(ID,position);
}

 function setyunself(i: int){
  if(resistyun){
 AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
 return;
 } 
  
	TController.yun=true;
	if(PlayerUtil.isMine(Status.instanceID)  || ! Status.isMine)
	TController.isControllable = false;
    yield Timer(i);
	TController.yun=false;
	if(PlayerUtil.isMine(Status.instanceID)  || ! Status.isMine)
	TController.isControllable = true;

  buff[i].buffID = 0;  
}


 function setbingself(i : int){
   if(resistbing){
 AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
 return;
 } 
  if(PlayerUtil.isMine(Status.instanceID)  || ! Status.isMine){
	TController.bing=true;
	TController.isControllable = false;
    yield Timer(i);
	TController.bing=false;
	TController.isControllable = true;
	}
  buff[i].buffID = 0;
}


function wudiself(i : int){
	Status.Unableattack=100;
  yield Timer(i);
    Status.Unableattack=0;
 buff[i].buffID = 0; 
 }
 

function dunqiang(i : int){
  Status.Unableattack=buff[i].buffvalue; 
  yield Timer(i); 
  Status.Unableattack=0; 
 buff[i].buffID = 0;   	  
}

//private var dd = true;
function  healthhot(i : int){
while ( buff[i].bufftime>=0){
    Status.AddHealth(buff[i].buffvalue);
     yield WaitForSeconds(2.9);
	 buff[i].bufftime -=2.9;
	 if(buff[i].breako==buff[i].off){
     buff[i].breako = !buff[i].off;
     break;
     }
 }
  buff[i].buffID = 0; 
}


function  healthdebuff(i : int){
while ( buff[i].bufftime>=0){
	yield WaitForSeconds(2.9);
	var Resist = parseInt(Status.Armor)*4000/(parseInt(Status.Armor) +400+60*parseInt(Status.Level))+ Status.extraResist;
    var damageT = -buff[i].buffvalue*(10000-Resist)*0.0001;    
    Status.AddHealth(damageT);
	  buff[i].bufftime -=2.9;
	 if(buff[i].breako==buff[i].off){
     buff[i].breako = !buff[i].off;
     break;
     }	  
 }
 buff[i].buffID = 0;  
}


function addattribute(info: int[]){
  var i = info[0];
  var n = info[1];	
  var buffvalue = info[2];
switch (n)
{				
  case 0:
  Status.buffint[8]+= buffvalue*10;
  yield Timer(i);
  Status.buffint[8]-= buffvalue*10;
  break;
  
  case 1:
  var temp1 = parseInt(Status.MaxATK)*buffvalue*0.01;
  Status.buffint[0] += temp1;
  yield Timer(i);
  Status.buffint[0] -= temp1;
  break;
    
  case 2:
  var temp2 = parseInt(Status.MaxATK)*buffvalue*0.5*0.01;
  var temp3 = buffvalue*40;
  Status.buffint[0] += temp2;
  Status.buffint[3] += temp3;  
  yield Timer(i);
  Status.buffint[0] -= temp2;
  Status.buffint[3] -= temp3;
  break;
  
  case 3:
  Status.Attackspeed += buffvalue*0.01-1;
  yield Timer(i);
  Status.Attackspeed -= buffvalue*0.01-1;	
  break;
  
  case 4:
  if(resistjian){
   AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
   break;;
  }   
  if(PlayerUtil.isMine(Status.instanceID) && TController.Movespeed>=1-buffvalue*0.01){
  TController.Movespeed += buffvalue*0.01-1;
  yield Timer(i);
  TController.Movespeed -= buffvalue*0.01-1;
  }
  break;
  
  case 5:
//  //print(buffvalue*0.01 + " == " + buffvalue);
  Status.Attackspeed += buffvalue*0.01;
  Status.buffint[4] += buffvalue*100;
  yield Timer(i);
//  //print(buffvalue*0.01 + " == " + buffvalue);
  Status.Attackspeed -= buffvalue*0.01;
  Status.buffint[4] -= buffvalue*100;
  break;
  
  case 6:
  Status.buffint[7] += buffvalue*1000;
  yield Timer(i);
  Status.buffint[7] -= buffvalue*1000;
  break;
  
  case 7:
  var temp6 = buffvalue*0.01*Status.sArmor;
  Status.buffint[2] += temp6;
  yield Timer(i);
  Status.buffint[2] -= temp6;
  break;
  
  case 8:
  Status.buffint[3] += buffvalue;
  yield Timer(i);
  Status.buffint[3] -= buffvalue;
  break;
  
  case 9:
  var temp8 = buffvalue*0.01*parseInt(Status.ATK);
  var temp9 = buffvalue*10;  
  Status.buffint[0] -= temp8;
  Status.buffint[3] -= temp9;  
  yield Timer(i);
  Status.buffint[0] += temp8;
  Status.buffint[3] += temp9;  
  break;

  case 10:
  Status.buffint[13] += buffvalue;
  yield Timer(i);
  Status.buffint[13] -= buffvalue;
  break;
  
  case 11:
  Status.buffint[1] += buffvalue;
  yield Timer(i);
  Status.buffint[1] -= buffvalue;
  break;  
  }
  
 buff[i].buffID = 0; 
}

private function ServerResponsejitui(ints : int[]){
	var boolfei : boolean = true;
	var booldao : boolean = true;
	if(ints[3] == 1){
		boolfei = false;
	}
	if(ints[4] == 1){
		booldao = false;
	}
	//jitui(ints[0] , ints[1] , ints[2] , boolfei , booldao);
}




var busy = false;

 function jitui(dist:int,heigh:int,i:int,fei:boolean,dao:boolean){
   if(resistjian){
   AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
   return;
  }
// var intFei : int = 0;
// var intDao : int = 0;
// if(! fei){
// 	intFei = 1;
// }
// if(! dao){
// 	intDao = 1;
// }
	 if(!busy){ //&& /PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID) && ){
//	 if(!busy && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
//		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , 	CommonDefine.ACT_ApplyBuff_JiTui, String.Format("{0};{1};{2};{3};{4}",dist , heigh , i , intFei , intDao ) );
		busy = true;
		
		//方向以服务器发过来的位置计算出来
		if(jituiPos == Vector2.zero)
		{
			hitdirection = Status.hitDirection();
		}
		else
		{
			var jituiPosition = new Vector3(jituiPos.x, transform.position.y, jituiPos.y);
			hitdirection = (jituiPosition - transform.position).normalized;
			jituiPos = Vector3.zero;
		}
		
		houtui = dist;
		high = heigh;
		bb=fei;
		cc=dao;
		yield WaitForSeconds(0.7);
		if(fei)
		yield WaitForSeconds(0.6);
		if(dao)
		yield WaitForSeconds(1.6);
		buff[i].buffID = 0;
		busy = false;  
	}
}
 
 private var hitdirection =Vector3(0,0,-1);
 function FixedUpdate () {
 	
if(busy){ //PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID) && ){
 	if(houtui >0&&!bb )
	{
		move=hitdirection*25*Time.deltaTime;	
		controller.Move(move);  
		houtui-=25*Time.deltaTime;
		
		TController.Remontepos = transform.position;
		
		//if(houtui <= 0 && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		//	ServerRequest.requestMove(	Vector3(0 , 0 , 0), Vector3(transform.position.x , transform.position.y , transform.position.z));
	}
	else if(bb)
	{	if(aa){
	    	if(cc){
//	    		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , 	CommonDefine.ACT_ApplyBuff_Down, "");
	    		down();
//	     		photonView.RPC("down",PhotonTargets.All);  	    	
	    	}
	    	 aa=false;
	     }  
	move = hitdirection*3*houtui*Time.deltaTime;		
	move += transform.up*3*high*Time.deltaTime;
	controller.Move(move);
	high-=25*Time.deltaTime;
	
	TController.Remontepos = transform.position;
	
	
	if((IsGrounded() ||high<-6.0 )&&!aa){
	
	//if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		//ServerRequest.requestMove(	Vector3(0 , 0 , 0), Vector3(transform.position.x , transform.position.y , transform.position.z));
	
	bb=false;
	if(cc&&!Status.dead){
//		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , 	CommonDefine.ACT_ApplyBuff_Ground, "");
		Ground();
//		photonView.RPC("Ground",PhotonTargets.All);
    }
    aa = true;
   	YieldBusy();
	}
	}	
 }	
}

function YieldBusy(){
	yield WaitForSeconds(0.2);
	busy = false;
}

function IsGrounded () {
 if (controller.collisionFlags & CollisionFlags.Below)
        return true;
 else
        return false;
//	return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
}

//@RPC
function down()
{
if(PlayerUtil.isMine(Status.instanceID) || ! Status.isMine){
	TController.down = true;
	TController.isControllable = false;
	}
	Animatc.anim_["down"].layer = 8;
	Animatc.anim_.Play("down",PlayMode.StopAll);
}

//@RPC
function Ground(){
   cc =false;
   yield WaitForSeconds(0.5);
   if(!Status.dead){
	   Animatc.anim_["standup"].layer = 9;
	   Animatc.anim_.Play("standup",PlayMode.StopAll);
   }
   yield WaitForSeconds(0.6);
   if(!Status.dead ){
	   Animatc.anim_["down"].layer = -1;
	   Animatc.anim_["standup"].layer = -1;
		if(PlayerUtil.isMine(Status.instanceID)  || ! Status.isMine){
		   TController.down = false;
		   TController.isControllable = true;
	   }
	   Animatc.anim_.Stop("standup");
	   Animatc.anim_.Play("idle");
   }
    yield WaitForSeconds(0.2);
   busy = false;
}

	private var outterColor : Color[] ;
//	private var myColor:Color ;
function ChangeColor(i : int,t :float){
 var bodymesh= GetComponentInChildren (SkinnedMeshRenderer);
 if(!bodymesh)
 return;
 var m_Mat = bodymesh.renderer.material;
  var ttint =0.0;
  var tempcolor = m_Mat.GetColor("_RimColor");
  var temppower = m_Mat.GetFloat("_RimPower");
	 m_Mat.SetColor("_RimColor",outterColor[i]);
	 while(t>=0){
	 ttint  = parseInt(Time.time) +0.5;
	 m_Mat.SetFloat("_RimPower", Mathf.Abs(Time.time - ttint)*2+0.5);
	 t -= Time.deltaTime;
	 yield;
	 }
 m_Mat.SetColor("_RimColor",tempcolor);
 m_Mat.SetFloat("_RimPower",temppower);	
	 	 
}
private var tempscale : float;
function ChangeSize(i:float,t :float){
	//print("ChangeSize Time == " + t);
 var yy : float=transform.localScale.y;
 if(i>1)
 {
while(yy < i*tempscale){
  yy += 5*Time.deltaTime;
  transform.localScale = Vector3(yy,yy,yy);
  yield;
 }
  yield WaitForSeconds(t);
while(yy > tempscale){
  yy -= 5*Time.deltaTime;
  transform.localScale = Vector3(yy,yy,yy);
  yield;
 }
 transform.localScale = Vector3(tempscale,tempscale,tempscale);  
 }
 else{
while(yy > i*tempscale){
  yy -= 5*Time.deltaTime;
  transform.localScale = Vector3(yy,yy,yy);
  yield;
 }
  yield WaitForSeconds(t);
while(yy < tempscale){
  yy += 5*Time.deltaTime;
  transform.localScale = Vector3(yy,yy,yy);
  yield;
 }  
 transform.localScale = Vector3(tempscale,tempscale,tempscale);  
 }
}

 function Resistyun(t){
 if(PlayerUtil.isMine(Status.instanceID)  || ! Status.isMine){
	TController.isControllable = true;
	}
TController.yun=false;
 resistyun = true;
 yield WaitForSeconds(t);
 resistyun = false;
 }

 function Resistfei(t){
if(PlayerUtil.isMine(Status.instanceID)  || ! Status.isMine){
   TController.down = false;
   TController.isControllable = true;
   }
 resistfei = true;
 yield WaitForSeconds(t);
 resistfei = false;
 }
 
  function Resistbing(t){
 if(PlayerUtil.isMine(Status.instanceID)  || ! Status.isMine){
	TController.bing=false;
	TController.isControllable = true;
	}
 resistbing = true;
 yield WaitForSeconds(t);
 resistbing = false;
 }
 
  function Resistjian(t){
 if(PlayerUtil.isMine(Status.instanceID)  || ! Status.isMine)
 TController.Movespeed=1;
 resistjian = true;
 yield WaitForSeconds(t);
 resistjian = false;
 }
 
@RPC
function PlaybuffEffect(info : int[]){
var ttime =info[3];
if(info[1]==4||info[1]==5||info[1]==6)
  ttime = 2;
  	if(AllResources.BuffefmanageStatic == null){
  		AllResources.BuffefmanageStatic = FindObjectOfType(Buffefmanage);
  	}
//  	print(AllResources.BuffefmanageStatic);
//  	//print(info[1]);
  	if(AllResources.BuffefmanageStatic != null)
    AllResources.BuffefmanageStatic.SpawnEffect(info[1],ttime,transform);  
}	

@RPC
function PlayskillEffect(i:int){
    AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}			