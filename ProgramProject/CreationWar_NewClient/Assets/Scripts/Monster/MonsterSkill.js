#pragma strict
//class GameSkill{
//	var sName : String;
//	var skillType : int;     0近战1冲锋2方向3自身范围4目标范围5弹道6pet7缓慢移动8停留9zhoubian  伤害ring0，debuff1 buff2 behit伤害3 behitdebuff4 hit5 kill6
//  var skillTime : int;
//	var startID : int;    
//	var middleID : int;
//	var endID : int;
//	var scope : int;      范围0小球1中球2大球3大大球4方的5未定
//	var damageValue : int;  倍数
//	var damageType : int;   0物理1技能2冰3火4暗影5毒
//	var buffID : String;  
//	var buffValue : int;   100上限
//	var buffTime : int;
//	var sType : int;  主动0  被动1
//	var info : String;    说明
//	var batterNum : int;  连击
//	var fxobject : GameObject;
//  var OpenR = false;
//}

var SkillP	:	GameSkill[];
var busy	=	false;
var Remoteskill	=	0;
private var PlayerID : int;
private var targetp : Vector3;
private var cc : GameObject;
private var Status : MonsterStatus;

private var Enemytag :String;
//private var Skillpool : DungeonControl;
private var MAI:MonsterAI;
private var anim_ : Animation;
private var CanCombat	=	1;
private var CanChange	=	1;
private var CanRefes	=	1;
private var CanSelf		=	1;

function	Awake()
{
	if(	this.CompareTag ("Enemy"))
		Enemytag = "Player";
	else if(	this.CompareTag ("Player"))
		Enemytag = "Enemy";
	Status	= GetComponent.<MonsterStatus> ();

	controller = GetComponent.<CharacterController> ();	
	MAI	 = GetComponent.<MonsterAI> ();
	PlayerID	=	Status.PlayerID;

	anim_  = GetComponent.<Animation>();
}

function	Start()
{
	var	MNetView	:	MonsterNetView	=	GetComponent(MonsterNetView);
	PlayerID	=	MNetView.MonsterID;	//
}
//初始化怪物技能//
//@RPC
function	ReadyMosterSkill(info : int[])
{
	SkillP		=	new Array (6);
	SkillP[0]	=	DungeonControl.AllSkiss[44];//juexing
	SkillP[4]	=	DungeonControl.AllSkiss[45];//miaosha
	SkillP[5]	=	DungeonControl.AllSkiss[57];//baonu
	if( anim_["attackf1"] )
	{
		SkillP[0].animation = anim_["attackf1"].clip; 
		SkillP[4].animation = anim_["attackf1"].clip; 
		SkillP[5].animation = anim_["attackf1"].clip; 
	}
	else if( anim_["attacka1"] )
	{
		SkillP[0].animation = anim_["attacka1"].clip; 
		SkillP[4].animation = anim_["attacka1"].clip;
		SkillP[5].animation = anim_["attacka1"].clip; 
	}
	else
	{
		SkillP[0].animation = anim_["battle"].clip; 
		SkillP[4].animation = anim_["battle"].clip; 
		SkillP[5].animation = anim_["battle"].clip; 
	}
	for( var i : int = 1; i < 4; i++)
	{
		SkillP[i] = new GameSkill();
		if(	info[i-1]>=0 )
		{
			SkillP[i]= DungeonControl.AllSkiss[info[i-1]-1];
			if(anim_["attackf1"])
				SkillP[i].animation = anim_["attackf1"].clip; 
			else if(anim_["attacka1"])
				SkillP[i].animation = anim_["attacka1"].clip; 
			else
				SkillP[i].animation = anim_["battle"].clip; 
			if(SkillP[i].sType==0)
			{
				if(	SkillP[i].skillType>=2	)
					Remoteskill += 1;
				switch (SkillP[i].skillType)
				{ 
					case 0:
						if(anim_["attackc1"])
						{
							if( anim_["attackc"+CanCombat] )
							{
								SkillP[i].animation = anim_["attackc"+CanCombat].clip;
								CanCombat +=1;
							}
							else
							{
								CanCombat =1; 
								SkillP[i].animation = anim_["attackc1"].clip;
							}
						}
						else
						{
							CanCombat =0;
							if( anim_["attacka1"] )
								SkillP[i].animation = anim_["attacka1"].clip;   
							else if(anim_["attackb1"])
								SkillP[i].animation = anim_["attackb1"].clip;
							else
								SkillP[i].animation = anim_["battle"].clip;
						}     
						break; 
					case 1:
						if( anim_["attackd1"] )
						{
							if(anim_["attackd"+CanChange])
							{
								SkillP[i].animation = anim_["attackd"+CanChange].clip;
								CanChange +=1;
							}
							else
							{
								CanChange =1; 
								SkillP[i].animation = anim_["attackd1"].clip;
							}
						}
						else
						{
							CanChange =0;
							if(anim_["attacka1"])
								SkillP[i].animation = anim_["attacka1"].clip;   
							else if(anim_["attackb1"])
								SkillP[i].animation = anim_["attackb1"].clip;
							else
								SkillP[i].animation = anim_["battle"].clip; 
						}     
						break; 
					case 2:
					case 4:
					case 5:
					case 7:
						if( anim_["attacke1"] )
						{
							if(anim_["attacke"+CanRefes])
							{
								SkillP[i].animation = anim_["attacke"+CanRefes].clip;
								CanRefes +=1;
							}
							else
							{
								CanRefes =1; 
								SkillP[i].animation = anim_["attacke"+CanRefes].clip;
							}
						}
						else
						{
							CanRefes =0;
							if(anim_["attackb1"])
								SkillP[i].animation = anim_["attackb1"].clip;   
							else if(anim_["attacka1"])
								SkillP[i].animation = anim_["attacka1"].clip; 
							else
								SkillP[i].animation = anim_["battle"].clip;
						} 
						break;
					case 6:
						Monsterprefab = Resources.Load(SkillP[i].buffID + "", GameObject);
						break;    
				}	//-----------------------------Switch	-	skillType
 			}
 			else
 			{	//sType!=0
				if(	!anim_["attackf1"]	)
					CanSelf = 0;
 			} 
		}	//info[i-1]>=0承诺
	}	//for (var i : int = 1; i < 4; i++)
}	//初始化技能-----

//使用技能的接口//
private var combo = 1;
function	Skill(i : int)
{
//Boss即将施放技能，判断是主客户端
	if(AllManage.tsStatic){
// AllManage.tsStatic.LiaoTian(AllManage.AllMge.Loc.Get("info1188"), Color.red);
 	AllManage.btnGMBStatic.my.warnings.warningAllTime1.Show("",AllManage.AllMge.Loc.Get("info1188"));
 	BtnRollingClick();
	}
	if(	SkillP.Length	>	i && SkillP[i] )
	{
		busy = true;
		switch( SkillP[i].sType )
		{				
			case 0:
				yield	ActiveSkill(i);
				busy = false;
				break;
			case 1:
				OpenRing(i);
				yield	WaitForSeconds(0.5);
				busy = false;
				break;
		}
		busy = false;
	}
}

function BtnRollingClick(){
if(Application.loadedLevelName == "Map200"||(Application.loadedLevelName == "Map212"&& !AllManage.InvclStatic.CanMapManaged("2121"))){
 	if(AllManage.UICLStatic.StorageSpr){
 	AllManage.UICLStatic.StorageSpr.enabled = true;
 	yield WaitForSeconds(3f);
 	AllManage.UICLStatic.StorageSpr.enabled = false;
	}
	}else{
	AllManage.UICLStatic.StorageSpr.enabled = false;
	}
}

function	ActiveSkill(i:int)
{
	if(	MAI.targetp	)
		targetp = MAI.targetp.transform.position + transform.forward*20 +transform.up*2;
	else
		targetp = transform.position + transform.up*2 + transform.forward*40;
		Objectp = ObjectPosition(SkillP[i].skillType);
    SendMessage( "PlayEffect", 89 );   //yujing
    yield	WaitForSeconds(0.5);
	if( SkillP[i].startID != 0 )
		SendMessage( "PlayEffect", SkillP[i].startID );
	if(	SkillP[i].skillType == 1 )
		qianjin = 8;
	else if( SkillP[i].skillType == 3 )
	{
//		MAI.stun			=	true;
		MAI.agent.enabled	=	false;
	}	
	combo = SkillP[i].batterNum;
	if(!MAI.AbleToTakeAction())
	return;
    while(	combo	>	0 && MAI.AbleToTakeAction()	)
    {
		if(	SkillP[i].animation	!=	null	){
			SendMessage( "SyncAnimation", SkillP[i].animation.name );
     	   yield	WaitForSeconds( SkillP[i].animation.length - 0.3 ); 
     	   }
		if(	SkillP[i].skillType==6)      
			CallPet(i);		//召唤宠物//
        else 
			CallObject(i);	//创建子弹//
        Objectp = ObjectPosition(SkillP[i].skillType);
        yield	WaitForSeconds(0.5);
        
        combo	-=	1;
	} 
    if(	SkillP[i].skillType	==	3	)
    {
 //   	MAI.stun	=	false;
	    MAI.agent.enabled	=	true;
	}
	return;
}

//创建子弹//
function	CallObject(i:int)
{   
	cc	=	PhotonNetwork.Instantiate(	this.SkillP[i].fxobject.name, Objectp,	transform.rotation,	0	);
	var	skillobject : MonsterSkillobject = cc.GetComponent(MonsterSkillobject); 
		skillobject.targetp		=	targetp;
		skillobject.PlayerID	=	Status.PlayerID;
		skillobject.Damagetype	=	SkillP[i].damageType;
		skillobject.attackerLV	=	Status.Level;
		skillobject.skilltype	=	SkillP[i].skillType;  
		skillobject.skilltime   =   SkillP[i].skillTime; 
		skillobject.scope		=	SkillP[i].scope; 
		skillobject.mideffectID	=	SkillP[i].middleID;    
		skillobject.endeffectID =	SkillP[i].endID;
		skillobject.damage		=	SkillP[i].damageValue*Status.ATK*0.01;
		skillobject.buffID		=	SkillP[i].buffID;
		skillobject.buffvalue	=	SkillP[i].buffValue ;//百分比
		skillobject.bufftime	=	SkillP[i].buffTime ; 
		skillobject.selftag		=	this.tag ; 
	if(	SkillP[i].skillType	<=	1 ||	SkillP[i].skillType	==	3)
		cc.transform.parent		=	this.transform;               
}
    
//宠物接口//
private var Monsterprefab:GameObject;     
function	CallPet(i:int)
{
//	if(	Monsterprefab	==	null	)
//		Monsterprefab	=	Resources.Load( SkillP[i].buffID + "", GameObject );
//	if(	Monsterprefab.name == "57600" || Monsterprefab.name == "57700"	)
//	{
//		cc	= PhotonNetwork.Instantiate( Monsterprefab.name, ObjectPosition( 9 ), transform.rotation, 0 );
//	}
//	else
//	{
//		cc	= PhotonNetwork.Instantiate( Monsterprefab.name, ObjectPosition(SkillP[i].skillType), transform.rotation, 0 );
//	}
//	var	PetStatus		=	cc.GetComponent(	MonsterStatus	);  
//	PetStatus.ATK		=	SkillP[i].damageValue		*	Status.ATK	*	0.01;
//	PetStatus.Maxhealth	=	(	SkillP[i].damageValue	*	Status.ATK	*	0.1	).ToString();
//	PetStatus.Pettime	=	SkillP[i].buffTime	-	2;
	if(msp){
		msp.CallMonsterSkillMonster(WaveID , SkillP[i].damageValue*Status.ATK*0.1 , SkillP[i].buffID + "");
	}
}

private var Objectp : Vector3;
//用于获取技能召唤物出现的位置//
private	function	ObjectPosition(i : int) : Vector3
{  
	var ST :Vector3;
	switch ( i )
	{				
		case 0:
		case 1:
		case 5:
		case 7:
			ST	=	transform.position+transform.up*2+transform.forward*2; 
			break;
		case 2:
			ST	=	transform.position+transform.forward*6; 
			break;
		case 3: 
			ST	=	transform.position+transform.up*2;  
			break;
		case 4:
			if( MAI.targetp )
				ST	=	MAI.targetp.transform.position+transform.up*2;
			else
				ST	=	transform.position+transform.forward*10+transform.up*2;     
			break; 
		case 6:  
			ST = transform.position+transform.forward*4 + transform.right*Random.Range(-5,5) ;
			break;
		case 8:  
			ST = transform.position+transform.up*2 + transform.forward*Random.Range(0,10) + transform.right*Random.Range(-10,10) ;
			break;
		case 9:  
			var rotation = Quaternion.Euler( 0, 45 * Random.Range(0,8), 0);
			ST = transform.position+transform.up*2 - rotation * Vector3.forward * 10; 
			break;         
	}  
	return ST;  
}

private var move	=	Vector3.zero;
private var controller	:	CharacterController;
private var qianjin	=	-0.1;
function	FixedUpdate ()
{
	if( qianjin	>	0 )
	{ 
		move	=	transform.forward * 30 * Time.deltaTime;
		controller.Move( move );
		qianjin	-=	30 * Time.deltaTime;
	}
}

function	OpenRing(i:int)
{
	if( CanSelf > 0 )
		SendMessage( "SyncAnimation", "attackf" + CanSelf);
	yield	WaitForSeconds(0.2); 
	if(	SkillP[i].startID	!=	0	)
		SendMessage("PlayEffect",SkillP[i].startID );
	if(	SkillP[i].skillType	<=	2	)
		Ring(SkillP[i].skillType,i);  
	SkillP[i].OpenR	=	true;            
	yield	WaitForSeconds(15);    
	SkillP[i].OpenR	=	false;        
}
  
	private var bb = true;
function	Ring (tpye:int,i:int)
{
	while(SkillP[i].OpenR && tpye==0 &&!Status.dead )
	{
		if(bb)
		{
			bb = false;
			allhit(	(SkillP[i].scope+1)*3+2,SkillP[i].buffValue,SkillP[i].damageType	);//allhit
    		if(	SkillP[i].middleID	!=	0	)
				SendMessage("PlayEffect",SkillP[i].middleID);
			yield	WaitForSeconds(3);
			bb = true;
		}
		yield;
	}
	while(	SkillP[i].OpenR && tpye	==	1&&!Status.dead )
	{
		if(	bb	)
		{
			bb = false;
			allbuff((SkillP[i].scope+1)*3+2,SkillP[i].buffID,SkillP[i].buffValue,SkillP[i].buffTime,Enemytag);
			if(SkillP[i].middleID !=0)
				SendMessage("PlayEffect",SkillP[i].middleID);
			yield WaitForSeconds(3);
			bb = true;
		}
		yield;
	}
	while(SkillP[i].OpenR && tpye==2&&!Status.dead )
	{
		if(bb)
		{
		bb = false;
		allbuff((SkillP[i].scope+1)*3+2,SkillP[i].buffID,SkillP[i].buffValue,SkillP[i].buffTime,this.tag);
		if(	SkillP[i].middleID !=	0	)
			SendMessage("PlayEffect",SkillP[i].middleID);
		yield	WaitForSeconds(3);
		bb = true;
		}
		yield;
	}
}

///被攻击之后//
function	Behit(target:Transform)
{
	if(	Status.dead	)
		return;
	for (var i : int = 0; i < SkillP.length; i++)
	{ 
		if(SkillP[i].OpenR && SkillP[i].skillType==3&&Time.time > hittime+2)
		{
			hittime = Time.time;
			givehit( target, SkillP[i].buffValue,SkillP[i].damageType);  
			if(	SkillP[i].middleID !=0)
				SendMessage("PlayEffect",SkillP[i].middleID);;
		}
        else 
        if(SkillP[i].skillType==4&&SkillP[i].OpenR&&Time.time > hittime+2)
        { 
			hittime = Time.time;
			givebuff( target, SkillP[i].buffID, SkillP[i].buffValue, SkillP[i].buffTime, Enemytag ); 
			if(	SkillP[i].middleID != 0 ) 
				SendMessage(	"PlayEffect",	SkillP[i].middleID	);
        }         
     }
}

private var hittime = 0.0;
function	Hit(target:Transform)
{
	if(	Status.dead	)
  		return;
	for(var i : int = 0; i < SkillP.length; i++)
	{ 
		if(	SkillP[i].OpenR && SkillP[i].skillType==5&&Time.time > hittime+2)
		{ 
			hittime = Time.time;
			givebuff(target,SkillP[i].buffID,SkillP[i].buffValue,SkillP[i].buffTime,Enemytag);  
			if(	SkillP[i].middleID	!=0	)
				SendMessage("PlayEffect",SkillP[i].middleID);
        }         
	}
}

function	Kills()
{
	if(	Status.dead	)
		return;
	for(var i : int = 0; i < SkillP.length; i++)
	{ 
		if(SkillP[i].OpenR && SkillP[i].skillType==6)
		{ 
			givebuff(transform,SkillP[i].buffID,SkillP[i].buffValue,SkillP[i].buffTime,this.tag);  
			if(	SkillP[i].middleID !=0)
        		SendMessage("PlayEffect",SkillP[i].middleID);
        }         
	}
}

private	function	allhit(radius:int,buffValue:int,damagetype:int)
{
	var	colliders : Collider[]	=	Physics.OverlapSphere ( transform.position,radius);
	var settingsArray	=	new int[5];
		settingsArray[0]=	PlayerID;
		settingsArray[1]=	buffValue*Status.ATK*0.01;
		settingsArray[2]=	buffValue*Status.ATK*0.01;
		settingsArray[3]=	damagetype;
		settingsArray[4]=	Status.Level; 
	for (var hit in colliders)
	{
		if(	hit.CompareTag (Enemytag)	)
			hit.SendMessageUpwards("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );			
	}  
}

private	function	allbuff(radius:int,buffID:int,buffValue:int,bufftime :int,tag:String)
{
	var colliders : Collider[] = Physics.OverlapSphere ( transform.position,radius);
	var settingsArray = new int[4];
		settingsArray[0]=PlayerID;
		settingsArray[1]=buffID;
		settingsArray[2]=buffValue;
		settingsArray[3]=bufftime;	
	for (var hit in colliders)
	{
		if(	hit.CompareTag (tag)	)
			hit.SendMessageUpwards("ApplyBuff",settingsArray,SendMessageOptions.DontRequireReceiver );			
	}  
}

private	function	givehit(	hit:Transform,	buffValue:int,	damagetype:int	)
{
	var settingsArray	=	new int[5];
		settingsArray[0]=	PlayerID;
		settingsArray[1]=	buffValue*Status.ATK*0.01;
		settingsArray[2]=	buffValue*Status.ATK*0.01;
		settingsArray[3]=	damagetype;
		settingsArray[4]=	Status.Level; 
	hit.SendMessageUpwards("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );			 
}

private	function	givebuff(	hit:Transform,	buffID:int,	buffValue:int,	bufftime :int,	tag:String	)
{
	var settingsArray = new int[4];
		settingsArray[0]=PlayerID;
		settingsArray[1]=buffID;
		settingsArray[2]=buffValue;
		settingsArray[3]=bufftime;	
	if(	hit.CompareTag (tag)	)
		hit.SendMessageUpwards("ApplyBuff",settingsArray,SendMessageOptions.DontRequireReceiver );			
}

var WaveID : int = 0;
function SetWaveSpawned(id : int){
	WaveID = id;
}

private var msp : MonsterSpawn;
function SetSpawnObject(sp : MonsterSpawn){
	msp = sp;
}
