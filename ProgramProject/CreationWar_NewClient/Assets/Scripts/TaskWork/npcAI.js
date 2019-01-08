#pragma strict

enum NPCFunctions{
	Non =0,
   WeaponShop = 1,
   Grocerystore = 2,
   GuildShop = 3,
   HonorStore =4,
   Duel = 5,
   Arena = 6,
   Battlefield = 7,
   Skill=8,
   RandomStore =9,
   Arena4v4 = 10,
   PVPStore = 11
}
	
var npcid : String;
var npctype : NPCFunctions;
var task : MainTask[] = null;
var mtw : MainTaskWork;
var icon : GameObject[];
private var isItem : boolean = false;
var uicl : UIControl;
private var aa : Vector3;
private var bb =false;
var regPool : ReGamePool;

function Start () {
	if(uicl == null){
		uicl = FindObjectOfType(UIControl);
	}
	regPool =AllResources.FuhaopoolStatic;

	aa = transform.position + transform.forward;
    animation.wrapMode = WrapMode.Loop;
    animation.CrossFade("idle");
    if(animation["work"] != null){
     animation["work"].layer = 1;   	
	 animation.CrossFade("work");
	}

	if(animation["talk"] != null){
    animation["talk"].layer = 2;	
	animation["talk"].wrapMode = WrapMode.Once;
	}
    InvokeRepeating("ShowFuHao", 3, 2); 
}

private var ptime : float = 0;
var yieldOnClickTimes : int = 0;
function OnClick(){
	yieldOnClickTimes += 1;
	var yTimes : int = 0;
	yTimes = yieldOnClickTimes;
//	while(AllManage.mtwStatic.MainPS.openNPCGo){
//		yield;
//	}
//	if(ptime > Time.time){
//		return;
//	}
	ptime = Time.time + 0.4;
	if(uicl == null){
		uicl = FindObjectOfType(UIControl);
	}
//	print(this.name);
	var target : Transform = PlayerStatus.MainCharacter;	
	if(target && ! canClick)	
	{
//		StartCoroutine("TestDistance",target);
		TestDistance(target);
		yield WaitForSeconds(0.5);  
		bb = true;
		canClick = false;
		if(bb){
			if(Vector3.Distance(target.position, transform.position) >=5){
				return;
			}
			uicl.GetNowNpcID(npcid);
			if(mtw == null){
				uicl.NPCTalk(NPCTalk);
			}else{
				if(PhotonNetwork.connected && MainPlayerS == null){
					MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
				}
				//			yield MainPlayerS.PrivateStatusInit();
				uicl.OpenNPC(task , NPCTalk);
			}
			uicl.SHowNPCTalk(NPCTalk , npctype , NPCname);
			if(animation["talk"] != null)
				animation.Play("talk");    
			while (true){
				yield WaitForSeconds(1);  
//				StartCoroutine("TestDistance",target);
				TestDistance(target);
				yield WaitForSeconds(0.5);  
				if(!bb){
					canClick = false;
					return; 
				}    
			}
		}
	}
}

function NpcMoveOn(){
			if(Vector3.Distance(PlayerStatus.MainCharacter.position, transform.position) >=5){
				return;
			}
	uicl.GetNowNpcID(npcid);
//		if(AllManage.UICLStatic.canOpenNPCTaskGO	){
			uicl.OpenNPC(task , NPCTalk);
			uicl.SHowNPCTalk(NPCTalk , npctype , NPCname);
//		}
}

function TriggerOn(){
//	//print(NPCControl.NowGoPosition + " == " +  transform.position);
	if(npcid == NPCControl.NowGoID){
	//	//print("123123123123");
		yield;
		yield;
		yield;
		if(uicl == null){
			uicl = FindObjectOfType(UIControl);
		}
		uicl.GetNowNpcID(npcid);
		if(mtw == null){
	      		uicl.NPCTalk(NPCTalk);
		}else{
			if(PhotonNetwork.connected && MainPlayerS == null){
				MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
			}
//			yield MainPlayerS.PrivateStatusInit();
	      		uicl.OpenNPC(task , NPCTalk);
		}
		uicl.SHowNPCTalk(NPCTalk , npctype , NPCname);
	}
}

function ShowNpcTieJiang(){
	uicl.SHowNPCTalk(NPCTalk , npctype , NPCname);
}

function OpenNPCInfo(ts : MainTask){ 
//	print("123wqjlwejr");
		if(ts.jindu == 2){
			isItem = false;
		}else
		if(ts.jindu == 1 && ts.taskType == MainTaskType.duihua){
			isItem = true;
		}else
		if(ts.jindu == 0){
			isItem = false;
		}else{
			isItem = false;		
		}
	
					if(!isItem){	
						mtw.TaskInfoValue(ts,TaskValueType.info);
					}else{
						mtw.TaskInfoValue(ts,TaskValueType.item);	
					}
//	if(ts.taskType == MainTaskType.duihua){	 
//		mtw.Step = 0;
//		mtw.TaskInfoValue(ts,TaskValueType.info);
//	}else{
//		mtw.Step = 0;
//		mtw.TaskInfoValue(ts,TaskValueType.item);	
//	}
}

var xmlc : XmlControl;
function FindOtherCanShow() : boolean{
	if(!xmlc){
		xmlc = FindObjectOfType(XmlControl);
	}
	ShowFuHao();
	for(var i=0; i<task.length; i ++){
		if(task[i] != null && MainPlayerS.LookTaskIsDone(task[i].id)){
			if(task[i].jindu == 2){
				return true;
			}else
			if(task[i].jindu == 1 && task[i].taskType == MainTaskType.duihua && npcid == xmlc.GetItemIDAsLevel(task[i].doneType.Substring(0,4))){
				return true;			
			}else
			if(task[i].jindu == 0 ){
				return true;					
			}
		}
	}
	return false;
}

var NPCTalk : String;
var NPCname : String;
var NPCTypeName : String;
var objMyName : GameObject;
function setName(name : String , title : String , talk : String ,  TypeName : String){
		objMyName = AllManage.NpcNameStatic;
	AllResources.FontpoolStatic.SpawnEffect(7 , transform , name + "\n" + title , 3 , null , null , 0);
//				var objName = GameObject.Instantiate(objMyName);
//				objName.transform.parent = transform;
//				objName.transform.localPosition = Vector3.zero;
//				var labelMyName : NameControl;
//				labelMyName = objName.GetComponent(NameControl);
//				if(labelMyName){
//					labelMyName.UIName.text = name + "\n" + title ;
//				}
	NPCTypeName = TypeName;
	NPCname = name;
	NPCTalk = talk;
}

var useTask : MainTask[];
function addTask(ts : MainTask){ 
	for(var o=0; o<task.length; o++){
		if(task[o] != null){
			if(task[o].id == ts.id){
				if( (ts.leixing == 5 && ts.jindu == 0) || ts.jindu == 2){
					task[o].jindu = ts.jindu;
				}
				return;
			}
		}
	}
//	for(var t:MainTask in task){
//		if(t != null){
//			if(t.id == ts.id){
//				if(ts.leixing == 5 && ts.jindu == 0){
//					
//				}
//				return;
//			}
//		}
//	}
//	//print(ts.jindu);
	useTask = new Array(task.length + 1);
	for(var i=1; i<task.length; i++){
		useTask[i] = task[i-1];
	}
	useTask[0] = ts;
	task = useTask;
	
}

function SetTask(g : GameValue){
	if(AllManage.mtwStatic.MainPS.openNpcID == npcid)
		yield;
	AllManage.mtwStatic.MainPS.openNPCGo = false;
	AllManage.mtwStatic.MainPS.openNpcID = npcid;
//	print("456456456456");
	if(g.task.leixing != 3){
		addTask(g.task);
		mtw = g.mtw; 
		ShowFuHao();
	}
}

var fuhao : GameObject;
var fuhaoID : int = -1;
private var oldFuhaoID : int = 100;
var canFuhao : boolean = false;
function SetItemTask(g : GameValue){
//	print("789789789789");
	isItem = true;
	addTask(g.task);
	mtw = g.mtw;
	ShowFuHao();
}

function SendTaskJinDuAsID(id : String , jindu : int){
	for(var i=0; i<task.length; i++){
		if(task[i].id == id){
			task[i].jindu = jindu;
		}
	}
}

function SendJindu(t : MainTask){
	try{
		if(t != null){
			SendTaskJinDuAsID(t.id , t.jindu); 
		}
	}catch(e){
	
	}
	ShowFuHao();
}

function removeTask(tks : MainTask){
	for(var i=0; i<task.length; i++){
		if(task[i] && task[i].id == tks.id){
			task[i] = null;
		}
	}
	ShowFuHao();
}

function GuanBiFuHao(){
	if(fuhao != null){
	if(regPool == null){
		regPool = AllResources.FuhaopoolStatic;
	}
		regPool.UnspawnEffect(fuhaoID , fuhao);
	}
	//print(transform);
	canFuhao = false;
	yield WaitForSeconds(1);
	yield;
	yield;
	yield;
	AllManage.mmp.CreateTaskDot();
}
var MainPlayerS : MainPersonStatus;
private var maxFuHaoID : int = 0;
function ShowFuHao(){
//	//print(gameObject);
	if(regPool == null){
		regPool = AllResources.FuhaopoolStatic;
	}
	if(!xmlc){
		xmlc = FindObjectOfType(XmlControl);
	}
	if(fuhao != null){
		regPool.UnspawnEffect(fuhaoID , fuhao);
	}
	fuhao = null;
	canFuhao = false;
	maxFuHaoID = 0;
	for(var i=0; i<task.length; i++){
		if(task[i] != null){		
			if(PhotonNetwork.connected && MainPlayerS == null){
				MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
			}
	//		//print(task[i].id);
			if(MainPlayerS.LookTaskIsDone(task[i].id) || task[i].leixing == 5 || (task[i].leixing == 4 && !MainPlayerS.LookEveryDayActivityIsDone( task[i].id))){
				if(task[i].jindu == 2){
//					print("1111" + gameObject.name);
					if(fuhao != null){
						regPool.UnspawnEffect(fuhaoID , fuhao);
					}
					fuhao = null;
					regPool.SpawnEffect(2 , transform , "" , 2 , "GetFuHao"); 
					fuhaoID = 2; 
					canFuhao = true;
					isItem = false;
					task[i].readyDone = true;
					maxFuHaoID = 2;
					return;
				}else
				if(task[i].jindu == 1 && task[i].taskType == MainTaskType.duihua && npcid ==   xmlc.GetItemIDAsLevel(task[i].doneType.Substring(0,4))){
//					print("2222" + gameObject.name);
					if(fuhao != null){
						regPool.UnspawnEffect(fuhaoID , fuhao);
					}
					fuhao = null;
					regPool.SpawnEffect(2 , transform , "" , 2 , "GetFuHao");  
					isItem = true;
					fuhaoID = 2;
					canFuhao = true;
					task[i].readyDone = true;
					maxFuHaoID = 2;
					return;			
				}else
				if(task[i].jindu == 0){
					if(maxFuHaoID < 2){
//						print("3333" + gameObject.name);
						if(fuhao != null){
							regPool.UnspawnEffect(fuhaoID , fuhao);
						}
						fuhao = null;
						regPool.SpawnEffect(0 , transform , "" , 2 , "GetFuHao"); 
						fuhaoID = 0;
						canFuhao = true;
						task[i].readyDone = false;
						maxFuHaoID = 1;
	//					return;					
					}
				}else{
					if(maxFuHaoID < 1){
//						print("4444" + gameObject.name);
						if(fuhao != null){
							regPool.UnspawnEffect(fuhaoID , fuhao);
						}
						fuhao = null;
						regPool.SpawnEffect(1 , transform , "" , 2 , "GetFuHao"); 
						task[i].readyDone = false;
						fuhaoID = 1;	
						canFuhao = true;
						isItem = false;		
						maxFuHaoID = 0;
		//				return;	
					}
					if(task[i].jindu == 3){
//						print("5555" + gameObject.name);
						if(fuhao != null){
							regPool.UnspawnEffect(fuhaoID , fuhao);
						}
						fuhao = null;
						canFuhao = true;
						maxFuHaoID = 0;
					}
				}
			}
		}
	}
	if(oldFuhaoID != fuhaoID){
		oldFuhaoID = fuhaoID;
			yield WaitForSeconds(1);
			yield;
			yield;
			yield;
		if(AllManage.mmp){
			AllManage.mmp.CreateTaskDot();	
		}
	}
	
}

function GetFuHao(obj : GameObject){
	fuhao = obj;
} 

function InitTaskNPCOn(){
//	print("123123123123");
	for(var i=0; i<task.length; i++){
		if(task[i] != null){
			if(task[i].jindu == 0){
				 uicl.OpenNPC(task , NPCTalk);
				 return;
			}
		}
	}
}

private var canClick : boolean = false;
private function TestDistance (target : Transform ){
if(!target)
return;
	canClick = true;
		var distanceToPlayer = Vector3.Distance(target.position, transform.position);
		 var angle = 180.0;
		if (distanceToPlayer > 5.0) {
//			Debug.Log ("Player has left !");
			animation["idle"].layer = 0; 
            if(animation["work"] != null)  	
	        animation.CrossFade("work");
			while (angle > 5 )
	              {                 
		         angle = Mathf.Abs(RotateTowardsPosition(aa));	
		         yield;
	              }    
	              bb=false;	        		
     canClick = false;
			      return;
			}
		else{	 animation["idle"].layer = 2;  
	             animation.CrossFade("idle");
	             if(animation["work"] != null)
	             animation.Stop("work");		     
		         while (target && angle > 5 )
	              {   
		         angle = Mathf.Abs(RotateTowardsPosition(target.position));	
		         yield;
	              }
	             bb= true;
     canClick = false;
	     		 return; 
	         }         
	 canClick = false;
}


private function RotateTowardsPosition (targetPos : Vector3) : float
{
	var relative = transform.InverseTransformPoint(targetPos);
	var angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
	var maxRotation = 200 * Time.deltaTime;
	var clampedAngle = Mathf.Clamp(angle, -maxRotation, maxRotation);
	transform.Rotate(0, clampedAngle, 0);
	return angle;
}

