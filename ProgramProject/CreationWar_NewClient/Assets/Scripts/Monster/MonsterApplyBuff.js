#pragma strict
 var buff : BuffStatus[];
private var photonView : PhotonView;
private var pnumber :int;
private var bb	=	false;	//击飞状态标记//
private var cc	=	false;	//击倒状态标记//
private var aa=true;
private var houtui	=	0.0;	//积累后退的距离//
private var high =-25.0;
private var move: Vector3;
private var controller : CharacterController;
private var Status:MonsterStatus;
private var MAI:MonsterAI;

//ADD	上传之后ID+1用于确认加血buff生效//
private	var	AddHealthBuffStatusID: int =	0;

function	Awake () 
{
	photonView	=	GetComponent(PhotonView);
	Status		=	GetComponent(MonsterStatus);
	MAI			=	GetComponent(MonsterAI);
	controller	=	GetComponent(CharacterController);
	outterColor	=	new Array (3);
	outterColor[0]	=	Color(1, 0, 0, 1);
	outterColor[1]	=	Color(1, 0.7, 0, 1);
	outterColor[2]	=	Color(0, 0.7, 1, 1);
	buff	=	new	Array (8);
	for (var i : int = 0; i < 8; i++) 
	{ 	
		buff[i]	=	new BuffStatus();
	}
}

private var Buffbusy = false;	//标记正在过滤buff//

function	ApplyBuff(info : int[])
{
	if(	!Buffbusy	)
		SendMessage(	"leachBuff",info	); 
}

@RPC
function	leachBuff(info : int[])
{
	Buffbusy	=	true;
    if(	info.Length	<	4	)
		return;
    pnumber	=	info[0];
	for( var i : int = 0; i < 8; i++)
	{ 
		if( ( info[1] == buff[i].buffID && info[2] >= buff[i].buffvalue ) || info[1] == 19 || info[1] == 20 )
		{	//相同buffID的，kill掉前面的buff后执行
			buff[i].off			= !buff[i].off;	//关闭前面的那个持续Buff//
			buff[i].buffID		= info[1];
			buff[i].buffvalue	= info[2];
			buff[i].bufftime	= info[3];
			yield;
			try
			{
				ReceiveBuff( i,	info[0]	);     
			}
			catch(e)
			{     }
			break;  
		}
		else 
		if(	buff[i].buffID	==	0	)
		{	//不同buffID的可以执行
			buff[i].buffID		= info[1];
			buff[i].buffvalue	= info[2];
			buff[i].bufftime	= info[3];
			try
			{
				ReceiveBuff( i,	info[0]	);     
			}
			catch(e)
			{     }
			break;
		}
	}
	Buffbusy	=	false;      
}

//被过滤之后的buf生效
function	ReceiveBuff (i : int,	AttackerInstID : int )
{
	var	info = new int[4];
		info[0]= i;
		info[1]= buff[i].buffID;            
		info[2]= buff[i].buffvalue; 
		info[3]= buff[i].bufftime;     
	SendMessage("PlaybuffEffect",info);               
	if(	buff[i].buffID	==	1	)
		setyunself(i);									//~	setyunself
	else if(buff[i].buffID==2||buff[i].buffID==3)
	{
		setbingself(i);  								//~	setbingself
		wudiself(i);									//~	wudiself
	}
	else if(buff[i].buffID==4)
		jitui(buff[i].buffvalue,1,i,false,false,AttackerInstID);
	else if(buff[i].buffID==5)
		jitui(buff[i].buffvalue,buff[i].bufftime,i,true,false,AttackerInstID); 
	else if(buff[i].buffID==6)			//击倒//
		jitui(buff[i].buffvalue,buff[i].bufftime,i,true,true,AttackerInstID);     
	else if(buff[i].buffID==7||buff[i].buffID==8)   // 定身
	{ 
		if((Status.BTMode && Status.modetime >=1)||Status.getMonsterLevel()<2)
		{
			AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
			return;
		}
		info[1]=4;
		info[2]=1;   
		addattribute(info); 
	}
	else if(buff[i].buffID==9||buff[i].buffID==10||buff[i].buffID==19)// 减速
	{
		info[1]=4; 
		addattribute(info); 
	}  
	else if((buff[i].buffID>=11&&buff[i].buffID<=16)||(buff[i].buffID>=35 && buff[i].buffID<=38))
		healthdebuff(i);					//加血的Buff1//11-16是缓伤//35-38是缓伤//
	else if(buff[i].buffID==17)		//17持续治疗//
		healthhot(i); 						//加血的buff//
	else if(buff[i].buffID==18)		//18治疗大加血//
	{
		buff[i].bufftime	=	1;
		healthhot(i);						//加血的buff//
	}  
	else if(buff[i].buffID==20)
	{
		info[1]=3;
		addattribute(info);       
	} 
	else if(buff[i].buffID==21)
	{
		info[1]=1;
		addattribute(info); 
		ChangeColor(0,buff[i].bufftime); 
		ChangeSize(1.2,buff[i].bufftime); 
	}
	else if(buff[i].buffID==30)
	{    
		MAI.targetp = Status.FindWithID(pnumber,MAI.Enemytag).transform; 
	} 
	else if(buff[i].buffID==31)  
		dunqiang(i);  
	else if(buff[i].buffID==33) 
		wudiself(i);  
	else if(buff[i].buffID==34)
	{    
		info[1]=5;
		addattribute(info); 
	}  

	if(buff[i].buffID==12||buff[i].buffID==36||buff[i].buffID==38)
	{    
		info[1]=4;
		info[2]=40;
		addattribute(info);  
	}  
	else if(buff[i].buffID==37)
		jitui(8,8,i,true,true,AttackerInstID);  
	else if(buff[i].buffID==16 )
	{
		yield WaitForSeconds(buff[i].bufftime);
		bigBoom(pnumber,buff[i].buffvalue*5);
	}   	 
}

//进行buff的倒计时//
function	Timer (i:int)
{ 
	while (buff[i].bufftime>=0 )
	{  
		buff[i].bufftime -= Time.deltaTime;         
		yield; 
		if(buff[i].breako==buff[i].off)
		{
			buff[i].breako = !buff[i].off;
			return;
		}
	}
}

///大爆炸///
function	bigBoom(PlayerID:int,buffvalue:int)
{
	Status.huandon( 3,	transform.position	);
        //photonView.RPC("huandon",3,transform.position);
	SendMessage(	"PlayskilloEffect",	38	);
 	var colliders : Collider[] = Physics.OverlapSphere ( transform.position, 8 );
	for( var hit in colliders)
	{
		var	closestPoint	=	hit.ClosestPointOnBounds(transform.position);
		var distance		=	Vector3.Distance(closestPoint, transform.position);
		var hitPoints		=	1.0 - Mathf.Clamp01(distance / 6);
		hitPoints	*=	buffvalue*5;
		if(	hit.tag	==	this.tag	)
		{
			var settingsArray	=	new int[5];
			settingsArray[0]	=	PlayerID;
			settingsArray[1]	=	hitPoints;
			settingsArray[2]	=	hitPoints;
			settingsArray[3]	=	3;
			settingsArray[4]	=	Status.Level; 
			hit.SendMessageUpwards(	"ApplyDamage",	settingsArray,	SendMessageOptions.DontRequireReceiver	);			
		}
	}  
}

 function	setyunself(i: int)
 {
 	if((Status.BTMode && Status.modetime >=1)||Status.getMonsterLevel()<2)
 	{
 		AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
 		return;
 	} 
	SendMessage("stunself");
	var settingsArray		=	new	int[3];
		settingsArray[0]	=	pnumber;
		settingsArray[1]	=	0;
		settingsArray[2]	=	300;
		SendMessageUpwards("saveHatred",settingsArray,SendMessageOptions.DontRequireReceiver );
	yield	Timer(i);
	SendMessage("huiself");
	buff[i].buffID	=	0;  
}

 function	setbingself(i : int)
 {
	if((Status.BTMode && Status.modetime >=1)||Status.getMonsterLevel()<2)
	{
 		AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
 		return;
 	}
	SendMessage("bingself",true);
	var settingsArray = new int[3];
		settingsArray[0]=pnumber;
		settingsArray[1]=0;
		settingsArray[2]=200;
		SendMessageUpwards("saveHatred",settingsArray,SendMessageOptions.DontRequireReceiver );
	yield Timer(i);
	SendMessage("huaself");	
	buff[i].buffID = 0;
}

function	wudiself(i : int)
{
//	KDebug.Log("无敌~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ 无敌 ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ", transform, Color.magenta );
	Status.Unableattack=100;
	yield	Timer(i);
	Status.Unableattack=0;
	buff[i].buffID = 0; 
}
 
function	dunqiang(i : int)
{
	Status.Unableattack=buff[i].buffvalue; 
	yield	Timer(i); 
	Status.Unableattack=0; 
	buff[i].buffID = 0;   	  
}

function	healthhot(i : int)	//治疗调用//buffID相等且
{
	AddHealthBuffStatusID	++	;
	MonsterServerRequest.MonsterAddHealthBuff( Status.PlayerID, AddHealthBuffStatusID, buff[i].buffID, buff[i].buffvalue, buff[i].bufftime, 2.9 );
	while (	buff[i].bufftime	>=	0	)
{

	AllResources.FontpoolStatic.SpawnEffect(1,transform,"+"+ buff[i].buffvalue,controller.height*0.5);

	yield	WaitForSeconds(	2.9	);
		buff[i].bufftime	-=	2.9;
		if(	buff[i].breako	==	buff[i].off	)
		{
			buff[i].breako	=	!buff[i].off;
			break;
		}
	}
	buff[i].buffID	=	0; 
}

function	healthdebuff(i : int)	//缓伤调用//
{
	AddHealthBuffStatusID	++	;
	//KDebug.Log( "@缓伤Buff Added@", transform, Color.grey );
	MonsterServerRequest.MonsterAddHealthBuff( Status.PlayerID, AddHealthBuffStatusID, buff[i].buffID, -buff[i].buffvalue, buff[i].bufftime, 2.9 );
	while ( buff[i].bufftime	>=	0	)	//bufftime	Needed//
	{
		yield	WaitForSeconds(	2.9	);
		AllResources.FontpoolStatic.SpawnEffect(0,transform,"-"+ buff[i].buffvalue,controller.height*0.5);
	//	Status.AddHealth( - buff[i].buffvalue );	//持续减血//
		var settingsArray		=	new int[3];
			settingsArray[0]	=	pnumber;
			settingsArray[1]	=	buff[i].buffvalue;
			settingsArray[2]	=	buff[i].buffvalue*0.6;
			SendMessageUpwards("saveHatred",settingsArray,SendMessageOptions.DontRequireReceiver );    
		buff[i].bufftime -=2.9;
		if(	buff[i].breako	==	buff[i].off	)
		{
			buff[i].breako	=	!buff[i].off;
			break;
		}	  
	}
	buff[i].buffID = 0;  
}

function	addattribute(info: int[])
{
	var i = info[0];
	var n = info[1];	
	var buffvalue = info[2];
	switch (n)
	{				
		case 1:
			var temp1 = buffvalue*0.01*Status.ATK; 
			Status.ATK += temp1;
			yield Timer(i);
			Status.ATK -= temp1;
			break;
		case 2:
			var temp2 = buffvalue*0.5*0.01*Status.ATK;
			var temp3 = buffvalue*50; 
			Status.ATK += temp2;
			Status.Defense += temp3;  
			yield Timer(i);
			Status.ATK -= temp2;
			Status.Defense -= temp3; 
			break;
		case 3:
			Status.Attackspeed += buffvalue*0.01-1;
			yield Timer(i);
			Status.Attackspeed -= buffvalue*0.01-1;	
			break;
		case 4:
			if ( MAI.Movespeed	>=	0	)
			{
				MAI.Movespeed += buffvalue*0.01-1;
				yield Timer(i);
				MAI.Movespeed -= buffvalue*0.01-1;
			}
			break;
		case 5:
			var	temp4	=	buffvalue*0.01*Status.ATK;
			var	temp5	=	buffvalue*10;  
			Status.ATK -= temp4;
			if( temp5 > Status.Defense)
				temp5 = Status.Defense;
			Status.Defense -= temp5; 
			yield Timer(i);
			Status.ATK += temp4;
			Status.Defense +=temp5;  
			break;
	}
	buff[i].buffID = 0; 
}

private	var	busy	=	false;	//标记此时不能加状态//
//击退、击倒、击飞//
function	jitui(	dist:int,	heigh:int,	i:int,	fei:boolean,	dao:boolean,	AttackerID:int	)
{
	//KDebug.Log(	"~ dist ~ " + dist +" ~ heigh ~ " + heigh + " ~ Type ~ " + buff[i].buffID + " ~ fei ~ " + fei.ToString() + " ~ dao ~ " + dao.ToString() + " ~", transform, Color.yellow	);
	if(	(Status.BTMode && Status.modetime >=1)	||	Status.getMonsterLevel()	<	2	)
	{
		AllResources.FontpoolStatic.SpawnEffect(8,transform,"免疫！",2);  
		return;
	}
	if(	!busy )
	{
//		if(	dao	)
//		{
//			KDebug.Log(	"不冷却倒地", transform, Color.green	);
//		}
//		if(	cc	)
//		{
//			KDebug.Log(	"-------------重复倒地--------------", transform, Color.blue	);
//		}
		hitdirection		=	Status.hitDirection(	AttackerID	);		//重新回去被击退方向//
		MAI.agent.enabled	=	false;
		bb		=	fei;
		cc		=	dao;
		houtui	=	dist;
		high	=	heigh;
		busy	=	true;
		yield	WaitForSeconds(0.2);
		if(	fei	)
			yield	WaitForSeconds(0.6);
		if(	dao	)
			yield	WaitForSeconds(2);
		MAI.agent.enabled	=	true;
		buff[i].buffID		=	0;
		busy	=	false;
	}

}
 
private	var	hitdirection	=	Vector3(0,0,-1);
function	Update()
{
}

function	FixedUpdate () 
{
	if(	 busy	)
	{
 		if(	houtui > 0	&&	!bb	)	//后退但是没有击飞//
		{
			move	=	hitdirection	*	25	*	Time.deltaTime;	//击退生效//
			controller.Move(move);  
			houtui	-=	25*Time.deltaTime;
		}
		else	if( bb )	//击飞//
		{	
			if( aa )
			{
				SendMessage( "bingself", false );
				if(	cc	) 
					SendMessage( "down" );  
				aa	=	false;
			}	    
			move	=	hitdirection	*	3	*	houtui	*	Time.deltaTime;	//击退生效//	
			move	+=	transform.up	*	3	*	high	*	Time.deltaTime;	//击退生效//
			controller.Move(	move	);
			high	-=	25	*	Time.deltaTime;
			if(	IsGrounded()	&&	!aa	)
			{
				bb	=	false;
				if(	cc	&&	!Status.dead	)	//没死并且击倒//
					SendMessage(	"Ground"	);
				else
					SendMessage(	"huaself"	);
				aa		=	true;
				busy	=	false;
			}
		}	
	}	
}

function	IsGrounded () 
{
	if(	controller.collisionFlags & CollisionFlags.Below)
		return	true;
	else
		return	false;
//	return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
}

@RPC
function	down()
{
	if(	animation["down"]	)
	{
		animation["down"].layer = 8;
		animation.Play("down",PlayMode.StopAll);
	}
}

@RPC
function	Ground()
{
	cc =false;
	yield	WaitForSeconds(1);
	try
	{
		if(!Status.dead && animation["standup"])
		{
			animation["standup"].layer = 9;
			animation.Play("standup",PlayMode.StopAll);
		}
	}
	catch(e)
	{ 
    }   
	yield	WaitForSeconds(0.6);
	try
	{   
		if( !Status.dead	)
		{
			if(animation["standup"])
			{
				animation["down"].layer = -5;
				animation["standup"].layer = -5;
				animation.Stop("standup");
			}
			SendMessage("huaself"); 

		}
	}
	catch(e)
	{ 
	}
}
//	private var selectedShader : Shader;
	private var outterColor : Color[] ;
//	private var myColor:Color ;
//	private var myShader:Shader ;
function	ChangeColor(i : int,t :float)
{
	var bodymesh= GetComponentInChildren (SkinnedMeshRenderer);
	var m_Mat = bodymesh.renderer.material;
	var ttint =0.0;
	if(m_Mat.HasProperty("_RimColor"))
	{
		var tempcolor = m_Mat.GetColor("_RimColor");
		var temppower = m_Mat.GetFloat("_RimPower");
		m_Mat.SetColor("_RimColor",outterColor[i]);
		while(t>=0)
		{
			ttint  = parseInt(Time.time) +0.5;
			m_Mat.SetFloat("_RimPower", Mathf.Abs(Time.time - ttint)*2+0.5);
			t -= Time.deltaTime;
			yield;
		}
		m_Mat.SetColor("_RimColor",tempcolor);
		m_Mat.SetFloat("_RimPower",temppower);
	}	 	 
}

function	ChangeSize(i:float,t :float)
{
	var	tempscale	=	transform.localScale.y;
	var	yy : float	=	transform.localScale.y;
	if(	i>1	)
	{
		while(	yy < i*tempscale	)
		{
			yy += 5*Time.deltaTime;
			transform.localScale = Vector3(yy,yy,yy);
			yield;
		}
		yield WaitForSeconds(t);
		while(	yy > tempscale	)
		{
			yy -= 5*Time.deltaTime;
			transform.localScale = Vector3(yy,yy,yy);
			yield;
		}  
	}
	else
	{
		while(	yy > i*tempscale	)
		{
			yy -= 5*Time.deltaTime;
			transform.localScale = Vector3(yy,yy,yy);
			yield;
		}
		yield	WaitForSeconds(t);
		while(	yy < tempscale	)
		{
			yy += 5*Time.deltaTime;
			transform.localScale = Vector3(yy,yy,yy);
			yield;
		}  
	}
}

//@RPC
function	PlaybuffEffect(info : int[])
{ 
	var ttime	=	info[3];
	if(	info[1]==4	||	info[1]==5	||	info[1]==6	)
		ttime = 2;
	if(	AllResources.BuffefmanageStatic	)
	{
		AllResources.BuffefmanageStatic.SpawnEffect(info[1],info[3],transform);  
	}
}	

//	@RPC
function	PlayskilloEffect(i:int)
{
	AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}	
						