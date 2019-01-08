var biao: Transform ;
static var objs: Transform;
private var cc: Transform ;
private var ccuv: AutoUV ;
private var Status:PlayerStatus;
//static var AutoSelect = false; 
var canFindTarget : boolean = true;
function	Start ()
{
	canFindTarget = true;
	Status =GetComponent(PlayerStatus);
	cc =Instantiate(biao, Vector3(0,-1000,0),Quaternion.identity);
	ccuv = cc.GetComponent(AutoUV);
	InvokeRepeating("FindEnemys", 2, 0.3);
}
	
private	function	FindEnemys()
{
	if(!canFindTarget && !this.enabled){
		this.enabled = true;
	}
	if( ( BtnGameManager.isPlayerSelectEnamy || !objs) && canFindTarget )
	{
		var colliders : Collider[] = Physics.OverlapSphere ( transform.position, 31);
		for (var hit in colliders) 
		{
			if(hit.CompareTag ("Enemy")||hit.CompareTag ("NPC")||hit.CompareTag ("Neutral"))
				FindEnemy(hit.transform);
		}
	}
}


private var aa = true;
var ptime : float = 0;
function	Update()
{	
//	if(canFindTarget){
		if(Time.time > ptime){
			ptime = Time.time + 0.5;
			if(	objs	&&	(objs.position - transform.position).magnitude	>	32	)
				objs	=	null;
		}
		if(	objs	)
		{
			 cc.transform.position = objs.transform.position+objs.transform.up * 0.2;
			 aa = true;
		}		    
	    else 
		if(!objs&& aa ){
		    Status.SetNowMonster();	
		    Feno=28;
			cc.transform.position = Vector3(0,-1000,0);
			aa = false;
	    }
//    }
//	print(objs + " ================= qiu ai");
}


private var Feno = 28.0;	
private var targetDir : Vector3;
private var angle = 0.0;
private var distance = 31.0;

private function FindEnemy(target : Transform)
{	
	if(target.CompareTag ("Enemy")){
	Status.battlemod=true;
	Status.battletime =Time.time;
	}

    targetDir = target.position - transform.position;   
    angle = Mathf.Abs(Vector3.Angle(targetDir, transform.forward));
    distance = targetDir.magnitude; 
//	//print("ttt==" + target + angle + " ddd=" +distance);
	if(angle>90 ||distance>30) {
	  if(target ==objs )
	    objs = null;
		return;    
	  } 
	else { 
    var Fen = ( distance*0.1+1 )*( angle*0.02 + 5);
    if(Fen<Feno){
	  objs=target;
	  Feno=Fen;
	  ChangeSelect();
	  }
	 } 
}

function ChangeSelect(){
	if(objs.gameObject.CompareTag ("Enemy"))
     ccuv.SetUV(1);	
    else if(objs.gameObject.CompareTag ("Neutral"))
     ccuv.SetUV(3);  
    else if(objs.gameObject.CompareTag ("NPC"))
     ccuv.SetUV(2); 
     var object : Object = null;
     object = objs.GetComponent(MonsterStatus);
     if(object != null){
		 Status.SetNowMonster(objs.GetComponent(MonsterStatus));     
     }else{
		 Status.SetNowMonster(objs.GetComponent(PlayerStatus));          
     }
}