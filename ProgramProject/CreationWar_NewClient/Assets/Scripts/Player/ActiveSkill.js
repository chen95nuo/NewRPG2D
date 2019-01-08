#pragma strict
	/// <summary>
	/// 触发物体移动方式 -
	/// </summary>
	enum	SkillObjectMoveType
	{
		Self		=	0,			//无-
		Combat		=	1,			//近战前2不动		离地2	-
		LinkForward =	2,			//前6link不动	离地0，	-
		Refers		=	3,			//前2指向前进 离地2 move  短距离并放大	-
		Ownrange	=	4,			//自身中心点link，放大-
		Targetrange	=	5,			//目标中心点，放大（离地1单位）-
		Bullet		=	6,			//前2移动到木标   move-
		Pet			=	7,			//前6不动  离地0-
		ForwardSlow	= 	8,			//前2缓慢向目标移动-
		Stay		= 	9,			//前6不动  离地0 停留内受伤-
		Randon		=	10,
		Targetflow	=	11
	}
	/// <summary>
	/// 物体发射方式 -
	/// </summary>
	enum	SkillObjectSpawnType
	{
	    None		= 0,
		One			= 1,	//一个-
		Sequence3	= 2,	//顺次3个-
		Multiple3	= 3,	//一次三个-
		MultipleNo	= 4,	//一次三个两个不导航-
		Sequence2	= 5		//顺次2个-
	}
	/// <summary>
	/// 发招时候人物移动方式 -
	/// </summary>
	enum	SkillMoveType
	{
		Non				=	0,	//无变化-
		Ding			=	1,	//定身-
		Forwardshot		=	2,	//短前移动-
	    Forwardmidle	=	3,
		Forwardlong		=	4,	//长移动（到目标）-
		Target			=	5	//直接移动到目标后面-
	}
	/// <summary>
	/// 发招时间	-
	/// </summary>
	enum	SkillTimeType
	{	
		Fast			=	0,	//瞬发
		ReadyFast		=	1,	//ready1.5秒后瞬发
		LoopcanBreak	=	2,	//循环4.5秒可随时打断	
	    Continued		=	3,	//循环4.5秒不被打断
	    Continuedshot	=	4
	}
	/// <summary>
	/// 伤害属性 -
	/// </summary>
	enum	Damagestype
	{
	    PhysicalSharps		=	0,	//物理攻击//
	    PhysicalBlunt		=	1,	//奥术//
	    IceDamage			=	2,	//冰//
	    FireDamage			=	3,	//火//
	    DarkFamage			=	4,	//暗//
	    PoisonDamage		=	5,	//毒//
	    //////同上的暴击标记//
	    PhysicalSharpsCrit	=	6,
	    PhysicalBluntCrit	=	7,
	    IceCrit				=	8,
	    FireCrit			=	9,  
	    DarkCrit			=	10,
	    PoisonCrit			=	11,     
	}
	
	/// <summary>
	/// 技能属性类 -
	/// </summary>
	class	Skillclass 
	{
	    var	ID		:	int;   // 
	    var explain	:	String;     //格式：消耗#V点法力，对目标造成#X武器伤害外加额外的#Y点伤害，并且****目标 #W 点伤害 #Z秒。
	    var name	:	String;
	    var Branch0name :String;
	    var Branch1name :String;
	    var Branch2name :String;    
	    var Branch0 : String;
	    var Branch1 : String;
	    var Branch2 : String;
	    
	    var BranchProperty : String[];
//	    var BranchProperty1 : String;
//	    var BranchProperty2 : String;
	    
	    var CDtime		=	1.0;		//CoolDownTime//
	    var cost		: String ="00000";    //技能花销，前两位为百分比，后三位为额外点数//
	    var animation	: AnimationClip;
	    var skillsound	: AudioClip;
	    var objlife		=	0.5;
	    var starteffect : int; 
	    var mideffect	: int;
	    var endeffect	: int; 
	    var fxobject	: GameObject;	//中间物体//
	    var ObjectMoveType	:	SkillObjectMoveType;    
	    var CSMoveType		:	SkillMoveType ;
	    var ObjectSpawnType	:	SkillObjectSpawnType;
	    var TimeType		:	SkillTimeType ;
	    var Damagetype		:	Damagestype;	//0为默认物理伤害，1~4为魔法，1是技能（奥术），2是冰，3是火，4是暗影，5是毒（自然）//
	    var level			:	int;			//2位 第一位等级，第二位变化//
	    
	    var levelattributeStr	:	String; 	//按位序列化的技能每一级数据//
	    var levelattribute	:	String[]; 	//按位序列化的技能每一级数据//
	    
		//前三排为等级属性，D000000 等级属性为 伤害前三位为武器伤害百分比 后三位为额外点数，（治疗在buff中分），//
		//后三排为属性变更，B为改buff效果前两位为buffID，后三位buff值，最后两位时间。  D为加伤害，H为加治疗，T为加仇恨。C为减少cd时间。后面跟6位//
	    var cdstime		=	-300.0;		//上次使用的时间点//
	    var Damage		:	int;   //被计算的技能伤害数值//
	    var Hatred		:	int;
	    var buffID		:	int;
	    var buffvalue	:	int;
	    var bufftime	:	int;   //被计算的数值//
	    var manajia		=	0;
	    var size		=	1.0;
	    var rehitime	=	2.0;
	    var aGet		:	boolean	=	false;
	    var maxLevel : int = 3;
	}
	/// <summary>
	/// 角色所有的技能,通过预设初值记录当前职业所有的技能信息 -
	/// </summary>
	var	SkillP	:	Skillclass[];
	private var	targetp			:	Vector3;
	private var cc				:	GameObject;			//临时变量//
	private var hittime			=	-1.0;				//防连点标记，1秒后可再次使用//
	private var photonView		:	PhotonView;
	private var Status			:	PlayerStatus;		//玩家数据//
	private var TPWeapon		:	ThirdPersonWeapon;
	private var TController		:	ThirdPersonController; 
	private var skilltime		=	1.0;
	private var tempdamage		=	0;
	private var tempbuffvalue	=	0;
	private var tempbufftime	=	0;
	private var CanBreak		=	false;
	private var addskillbuff	=	0;

	private var aGet			:	boolean		=	false;
	private var _SkillControl	:	SkillControl;
	private var ts				:	TiShi;		//信息提示板//
	var	busy			=	false;

function	Awake()
{

	busy		=	false;
	TPWeapon	=	GetComponent(ThirdPersonWeapon);
	Status		=	GetComponent(PlayerStatus);
	photonView	=	GetComponent(PhotonView);
	TController	=	GetComponent(ThirdPersonController);
	
		OriginUsageDescribtion	=	new	Array(	SkillP.length	);//备份原始技能描述//
		OrigSkill	=	new	Array(SkillP.length);
	
	for(var i=0; i<SkillP.length; i++){
		OriginUsageDescribtion[i]	=	SkillP[i].explain;
		OrigSkill[i] = new Skillclass();
        OrigSkill[i].CDtime		=	SkillP[i].CDtime;
        OrigSkill[i].cost		=	SkillP[i].cost;
        OrigSkill[i].objlife	=	SkillP[i].objlife;
        OrigSkill[i].level		=	SkillP[i].level;
        OrigSkill[i].levelattribute	=	SkillP[i].levelattribute;	
        OrigSkill[i].cdstime	=	SkillP[i].cdstime;
        OrigSkill[i].Damage		=	SkillP[i].Damage;
        OrigSkill[i].Hatred		=	SkillP[i].Hatred;
        OrigSkill[i].buffID		=	SkillP[i].buffID;
        OrigSkill[i].buffvalue	=	SkillP[i].buffvalue;
        OrigSkill[i].bufftime	=	SkillP[i].bufftime;
        OrigSkill[i].manajia	=	SkillP[i].manajia;
        OrigSkill[i].size		=	SkillP[i].size;
        OrigSkill[i].rehitime	=	SkillP[i].rehitime;
	}


	if(	!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)	)
		return;
	controller		=	GetComponent(CharacterController);	
	Playrobot		=	GetComponent(PlayerRobot);
	_SkillControl	=	FindObjectOfType(SkillControl);
	
	
	InvokeRepeating("RepeatingResetSkillAttribute", 0, 2); 

}

/// <summary>
/// 每1秒重新初始化更新技能信息 -
/// </summary>
function	RepeatingResetSkillAttribute()
{
	for(	var i : int = 0; i <	SkillP.length;	i++	)
	{ 
        Leveattr(i);
	}
}

function	Start()
{
	
	if(	!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)	)
	    return;
	ts	=	FindObjectOfType(TiShi);
}

function	initSkill()
{
	if(!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		return;
	AllManage.SkillCLStatic.getSkill(this);
}

private	var	fanhui	:	boolean = false;
function	AttackSkill(i : int ) : boolean
{
	fanhui = false;
	if(	!Status.dead && TController.isControllable && hittime + 1 < Time.time )
	{
		targetp	= transform.position + transform.up * 2 + transform.forward * 40;
		hittime	= Time.time;
		Skill( i );  
		SendMessage( "DaDuan", SendMessageOptions.DontRequireReceiver );
		return	fanhui;
	}
	return	false;
}

var responseTargetp		: Vector3;
var responseInstanceID	: int = 0;
function	ResponseSkill(objs : Object[])
{
	var skillType	:	int;
	var skillid		:	int;
	skillType			=	objs[0];
	skillid				=	objs[1];
	responseTargetp		=	objs[2];
	responseInstanceID	=	objs[3];
	Skill(skillid);
}

private var temptarget	:	Transform;	//释放技能时用于储存被锁定的目标//
private var targetPS : PlayerStatus;
private var targetMS : MonsterStatus;
private var skillContinue : boolean = false;
/// <summary>
/// 使用技能的接口，传入技能在SkillP中的下标索引 -
/// </summary>
function	Skill(	i : int	)
{	//-------------------------------------------------------------------技能释放-------------------------------------------------------------------//
//	if(	!PhotonNetwork.room	)
//		return;
	Status.ridemod=false;
	temptarget	=	null;
	if(	Status.isMine	&&	qiuai.objs	)
	{
		temptarget	=	qiuai.objs;
    }	
	else 
	if(	!Status.isMine	&&	Playrobot	&&	Playrobot.targetp)
	{
		temptarget	=	Playrobot.targetp;      
    } 
    ///获取目标//
    
    try{
	if(	PlayerUtil.isMine( GetComponent( PlayerStatus ).instanceID )	)
	{
	}
	else
	{
			var go : GameObject;	
			if(	responseInstanceID == PlayerUtil.myID	)
			{
				go = PlayerStatus.MainCharacter.gameObject;
			}
			else
			{
				go = ObjectAccessor.getAOIObject(responseInstanceID);			
			}
			if(	go == null)
			{
				go = MonsterHandler.GetInstance().FindMonsterByMonsterID(responseInstanceID);			
			}
			if(	go	)
			{
				temptarget = go.transform;
			}
	}
    }catch(e){
//    	print(e.ToString());
    }
	tempdamage		=	SkillP[i].Damage;
	tempbuffvalue	=	SkillP[i].buffvalue;
	tempbufftime	=	SkillP[i].bufftime;
//	if(	PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	)	)
//	{
		if(	SkillP[i].level	<	10	||	busy	)
		{	//等级信息不足	或	公共冷却//
			fanhui	=	false;
			if(	PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	))
				return ;
		}
//		print(Time.time + "======CDTime============================"	+	SkillP[i].CDtime + " ====================== " + SkillP[i].cdstime	);
		if(	Time.time	<	SkillP[i].cdstime	+	SkillP[i].CDtime )
		{	//冷却中//
			if(	ts	&& PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	))
				AllManage.tsStatic.ShowRed(	"tips092"	);
			fanhui	=	false;
			if(	PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	))
				return ;
		}
		else 
		if(	parseInt(	Status.Mana	) < costmana(	i	)	)
		{	//文字提示魔法不足//
			if(	ts		&& PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	))
			{
				if(	Status.ProID==1	)
					AllManage.tsStatic.ShowRed("tips093");
				else if(	Status.ProID==2	)
					AllManage.tsStatic.ShowRed("tips094");
				else 
			     	AllManage.tsStatic.ShowRed("tips095");
			}     
			fanhui = false;
			if(	PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	))
				return ;
		}
		else 
		if(	SkillP[i].CSMoveType	>=	1	&&	Move(SkillP[i].CSMoveType)==false)
		{
			fanhui	=	false;
			if(ts 	&& PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	))
				AllManage.tsStatic.ShowRed(	"tips096"	); 
			if(	PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	))
				return;
		}
//	}

	try{
	if(	PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	)	)
	{
		if(	temptarget	)	//玩家属性//
		{
			targetPS	=	temptarget.gameObject.GetComponent(	PlayerStatus	);
			if(	targetPS	)
			{
				ServerRequest.requestUseSkill(	1,	i,	targetp,	targetPS.instanceID	);		
			}
			else			//怪属性//
			{
				targetMS	=	temptarget.gameObject.GetComponent(	MonsterStatus	);
				if(targetMS)
				{
					ServerRequest.requestUseSkill(1 , i , targetp , targetMS.PlayerID);		
				}
			}
		}
		else
		{
			ServerRequest.requestUseSkill(1 , i , targetp , 0);
		}
	}
    }catch(e){
    	print(e.ToString());
    }
	
    fanhui	=	true;   
    SkillP[i].cdstime	=	Time.time; 
    busy	=	true;
    TPWeapon.UsingSkillAsID(i , busy);
    if(	SkillP[i].starteffect	!=	0	)
    {
    	PlayEffect(SkillP[i].starteffect);
//    	photonView.RPC("PlayEffect",SkillP[i].starteffect);
    }
    if(	SkillP[i].skillsound	)
    {
    	Playaudio(i);
//    	photonView.RPC("Playaudio",i);
    }
	switch(	SkillP[i].TimeType	)
	{       
		case	0:	//瞬发//
			skilltime = 0.2;   
			CanBreak = false;   
			SendMessage("CallAnimation", SkillP[i].animation.name); 
			break;
		case	1:	//ready1.5秒后瞬发//            
			skilltime = 0.2;   
			CanBreak = false;     
			yield	StartCoroutine("Readyskill",SkillP[i].ID);  
//			if(!PhotonNetwork.room)
//			{
//				busy	=	false;
//				return;
//			}
			SendMessage("CallAnimation", SkillP[i].animation.name);    	 
			break; 
		case 2:		//循环4.5秒可随时打断//
			if(i==15)
				skilltime = 5;
     		else if(i==18)
     			skilltime = 15;
			else         
				skilltime = 4;
			CanBreak = true;
			SendMessage("Callloop", SkillP[i].animation.name);    
//			photonView.RPC("Callloop",SkillP[i].animation.name);
			break;
		case 3:		//循环4.5秒不被打断//
			skilltime = 3.5;
			CanBreak = false;
			SendMessage("Callloop", SkillP[i].animation.name);    
//			photonView.RPC("Callloop",SkillP[i].animation.name);
			break;
		case 4:
			skilltime = 1;
			CanBreak = false;
			SendMessage("Callloop", SkillP[i].animation.name);    
//			photonView.RPC("Callloop",SkillP[i].animation.name);
			break;     
	} 
 
	if(	Status.ProID == 3 && Status.Pearl>0)
	{
		if(	SkillP[i].ID==304	||	SkillP[i].ID==307	)
		{
			tempdamage *=1.5;    
			Status.Pearl -=1;
		}
		else if(	SkillP[i].ID==312	)
		{
			tempbuffvalue *= Status.Pearl;
			Status.Pearl =0;
		}
		else if(	SkillP[i].ID==314	)
		{
			tempdamage *= Status.Pearl*0.5 +1;
			Status.Pearl =0;
		}
		else if(	SkillP[i].ID==309	||	SkillP[i].ID==315	)
		{
			skilltime = Status.Pearl+1;
			Status.Pearl =0;
		}
		SendMessage(	"GhostPearl",	Status.Pearl,	SendMessageOptions.DontRequireReceiver	);         
 	}
	else
	if(	SkillP[i].ID==204&&Status.Hide	)	//暗影突袭隐藏时1.5倍伤害//
		tempdamage	*=	1.5;

	yield	WaitForSeconds(0.2);
//	if(	!PhotonNetwork.room	)
//	{
//		busy = false;
//		return;
//	}

	if(	SkillP[i].CSMoveType==2 || SkillP[i].CSMoveType==3|| SkillP[i].CSMoveType==4)
		qianjin	=	qian;
	if(	SkillP[i].ID==101||SkillP[i].ID==103||SkillP[i].ID==104||SkillP[i].ID==110||SkillP[i].ID==111||SkillP[i].ID==112||SkillP[i].ID==204||SkillP[i].ID==205||SkillP[i].ID==206){  
		SendMessage("showswipe" , true , SendMessageOptions.DontRequireReceiver);
//		photonView.RPC("showswipe",true); 
	}
	skillContinue = true;
	while(	skilltime>0 && skillContinue && TController.isControllable &&(CanBreak==false || ((Mathf.Abs(Input.GetAxisRaw("Vertical"))<0.2) && (Mathf.Abs(Input.GetAxisRaw("Horizontal"))<0.2))))// && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))))
	{
		yield WaitForSeconds(SkillP[i].animation.length-0.8);
//		if(!PhotonNetwork.room)
//		{
//			busy = false;
//			return;
//		}
		if(skilltime<=0 || !TController.isControllable ||(CanBreak==true && (alljoy.v>0.2)))// && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))))
			break; 
 
		switch (SkillP[i].ObjectSpawnType)
 		{		
  			case 0:
				break; 
			case 1:
				CallObject(i,0);
				break;  
			case 2:
				CallObject(i,0);
				yield	WaitForSeconds(0.1); 
//				if(!PhotonNetwork.room)
//				{
//					busy = false;
//					return;
//				}
				CallObject(i,0);
				yield	WaitForSeconds(0.1); 
//				if(!PhotonNetwork.room)
//				{
//					busy	=	false;
//					return;
//				}
				CallObject(i,0);
				break;
			case 3:
				CallObject(i,0);
				yield; 
//				if(!PhotonNetwork.room)
//				{
//					busy = false;
//					return;
//				}
				CallObject(i,3);
				yield; 
//				if(!PhotonNetwork.room)
//				{
//					busy = false;
//					return;
//				}
				CallObject(i,-3);           	
				break;
			case 4:
				CallObject(i,0);
	    		yield; 
//				if(!PhotonNetwork.room)
//				{
//				    busy = false;
//					return;
//				}
				targetp	=	transform.position + transform.up * 2 + transform.forward * 40 + transform.right * 10;
				CallObject(i,20);     
				yield; 
//				if(!PhotonNetwork.room)
//				{
//				    busy = false;
//					return;
//				}
				targetp	=	transform.position + transform.up * 2 + transform.forward * 40 - transform.right * 10;
				CallObject(i,-20);              	
				break;
			case 5:
				CallObject(i,0);
				yield	WaitForSeconds(0.1); 
//				if(	!PhotonNetwork.room	)
//				{
//				    busy = false;
//					return;
//				}
				CallObject(i,0);   
				break;
 		}    
   		if(	SkillP[i].ObjectMoveType==0
   			||	SkillP[i].ID==106
   			||	SkillP[i].ID==107
   			||	SkillP[i].ID==115
   			||	SkillP[i].ID==203
   			||	SkillP[i].ID==208	)
   		{
			var setArray = new int[4];
            setArray[0]= Status.instanceID;
            setArray[1]= SkillP[i].buffID;           
            setArray[2]= tempbuffvalue;
            setArray[3]= tempbufftime;   
            if(SkillP[i].ID==208)
            {
				setArray[1]= 23;
				setArray[2]= 0; 
				setArray[3]= 3;    
			}  						
			SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver );  
		}
		
		yield WaitForSeconds(0.3);
		huandong(SkillP[i].ID,transform.position);
		yield WaitForSeconds(0.4);
//		if(!PhotonNetwork.room)
//		{
//		    busy = false;
//			return;
//		}
		skilltime -= SkillP[i].animation.length;
	}
	skillContinue = false;
	if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_skillContinue, "1");
	
	temposition		=	Vector3(0,0,0);
	SendMessage("changelayer" , SkillP[i].animation.name , SendMessageOptions.DontRequireReceiver );
//    photonView.RPC(	"changelayer",	SkillP[i].animation.name	);	
//    photonView.RPC(	"huandong",		SkillP[i].ID,transform.position	); 
	if(PlayerUtil.isMine( GetComponent( PlayerStatus ).instanceID || GetComponent( PlayerStatus ).isMine)){
	    var	tempmana	=	parseInt(Status.Mana);
	    tempmana		-=	costmana(i);
	    tempmana		+=	SkillP[i].manajia;
	    Status.Mana		=	tempmana.ToString();
	}
    busy			=	false;
    SendMessage("showswipe" , false , SendMessageOptions.DontRequireReceiver);
//    photonView.RPC(	"showswipe",	false	); 
    TPWeapon.UsingSkillAsID(	i,	busy	);
	if(	SkillP[i].CSMoveType	==	1	)
		TController.Buffspeed	=	1; 
    if(	SkillP[i].ID	!=	203	&&	SkillP[i].ID	!=	208	)
		Status.Hide=false;      

}	//-------------------------------------------------------------------技能释放-------------------------------------------------------------------//

function changeskillContinue(param : String){
	skillContinue = false;
}

/// <summary>
/// 震屏效果 -
/// </summary>
//@RPC
function	huandong(	ID:int,	position:Vector3	)
{
	if(	ID==303	||	ID==308	)
		newCamera.huandong(1,position);
	else if(	ID ==102	||	ID==103	||	ID==106	||	ID==312	||	ID==313	)
		newCamera.huandong(2,position);
	else if(	ID ==108)
		newCamera.huandong(3,position);
}

private var Playrobot : PlayerRobot;

/// <summary>
/// i为技能index，创建技能发射物体 -
/// </summary>
function	CallObject(i:int,type:int)
{
	if((SkillP[i].ID == 302 || SkillP[i].ID == 109  || SkillP[i].ID == 115 )&& ! PlayerUtil.isMine( GetComponent( PlayerStatus ).instanceID )){
		return;
	}
	var tempdtype = 0;
	cc	=	PhotonNetwork.Instantiate(	this.SkillP[i].fxobject.name,	ObjectPosition(SkillP[i].ObjectMoveType),	transform.rotation,	0	);
	if(	!cc	)
		return;
	if(	Status.ProID	!=	3	)
		Physics.IgnoreCollision(	cc.collider,	collider	);     
	var skillobject : Skillobject	=	cc.GetComponent(	Skillobject	); 
	skillobject.subskillmove	=	type;
	
	try{
	if(	PlayerUtil.isMine( GetComponent( PlayerStatus ).instanceID )	)
	{
		if(	type != 20 && type != -20 )
			skillobject.target = temptarget;
		skillobject.targetp = targetp;
	}
	else
	{
		if(	type != 20 && type != -20 )
		{
			var go : GameObject;	
			if(	responseInstanceID == PlayerUtil.myID	)
			{
				go = PlayerStatus.MainCharacter.gameObject;
			}
			else
			{
				go = ObjectAccessor.getAOIObject(responseInstanceID);			
			}
			if(	go == null)
			{
				go = MonsterHandler.GetInstance().FindMonsterByMonsterID(responseInstanceID);			
			}
			if(	go	)
			{
				skillobject.target		=	go.transform;
				skillobject.targetID	=	responseInstanceID;
			}
		}
		skillobject.targetp	=	responseTargetp;		
	}
    }catch(e){
//    	print(e.ToString());
    }

	skillobject.PlayerID	=	Status.instanceID;
	skillobject.attackerLV	=	parseInt(Status.Level);
	skillobject.skillID		=	SkillP[i].ID;
	skillobject.skilltype	=	SkillP[i].ObjectMoveType;   
	skillobject.mideffectID =	SkillP[i].mideffect;           
	skillobject.endeffectID =	SkillP[i].endeffect;
	if(	Random.Range(0,10000)	<=	parseInt(Status.Crit)	)
	{
		skillobject.damage		=	2*tempdamage-Random.Range(0,tempdamage*0.4);  
		skillobject.Damagetype	=	SkillP[i].Damagetype+6;
	}
    else
    {     
		skillobject.damage		=	tempdamage-Random.Range(0,tempdamage*0.2);
		skillobject.Damagetype	=	SkillP[i].Damagetype;
	}
	skillobject.Hatred			=	SkillP[i].Hatred;
	if(SkillP[i].buffID!=23)
	skillobject.buffID			=	SkillP[i].buffID;
	skillobject.buffvalue		=	tempbuffvalue ;	
	skillobject.bufftime		=	tempbufftime ; 
	skillobject.Size			=	SkillP[i].size ; 
	skillobject.Objlife			=	SkillP[i].objlife;
 	skillobject.rehitime		=	SkillP[i].rehitime;
	skillobject.addbuffvalue	=	addskillbuff;
	skillobject.Status = Status;
	//KDebug.Log( "CallObject tag = " + this.tag, transform, Color.red );
	skillobject.selftag			=	this.tag;
    if(	SkillP[i].ObjectMoveType	==	1	||	SkillP[i].ObjectMoveType	==	2	||	SkillP[i].ObjectMoveType	==	3	||	SkillP[i].ObjectMoveType	==	4	)
    	cc.transform.parent		=	this.transform;    
}

private var Fstr : String = ";";
function ReturnCallSkull(skullInfo : String){
	var strs = skullInfo.Split(Fstr.ToCharArray());
	if(strs.length > 5){
		CallSkull(parseFloat(strs[0]) , parseInt(strs[1]) , parseFloat(strs[2]) , parseFloat(strs[3]) , parseFloat(strs[4]) , parseInt(strs[5]));
	}
}

//	_Damage伤害,		MonsterInstanceID骷髅实例	//
function	CallSkull(	_Damage:float,	_buffID:int,	_tempbuffvalue:float,	_tempbufftime:float,	_Objlife:float,	MonsterInstanceID:int	)
{
//if(objKuLou == null)
//	objKuLou = Resources.Load("Petkulou", GameObject);
//	cc	=	Instantiate(objKuLou	 ,	transform.position	+	transform.right*2.0,	transform.rotation);
	if(Status.ProID == 1){
		cc	=	PhotonNetwork.Instantiate(	"Petwarrior",	transform.position	+	transform.right*2.0,	transform.rotation,	0	);
	}else
	if(Status.ProID == 3){
		cc	=	PhotonNetwork.Instantiate(	"Petkulou",	transform.position	+	transform.right*2.0,	transform.rotation,	0	);
	}
	if(	!cc	)
		return;
//	var Status : MonsterStatus = GetComponent(MonsterStatus);
//	Status.ATK			=	damage	*	0.8;
//	Status.Maxhealth	=	5*damage;
//	Status.Health		=	Status.Maxhealth;
//	Status.Pettime		=	Objlife	-	3;
	while(! Status.isTBteam){
		yield;
	}
	AddMyPet(cc);
	var skillobject	:	Skillobject	=	cc.GetComponent(	Skillobject	); 
	skillobject.PlayerID			=	Status.instanceID;
	skillobject.attackerLV			=	parseInt(	Status.Level	);
	skillobject.damage		=	_Damage;  
	skillobject.buffID		=	_buffID;
	skillobject.buffvalue	=	_tempbuffvalue ;	
	skillobject.bufftime	=	_tempbufftime ; 
	skillobject.Objlife		=	_Objlife;
	skillobject.selftag		=	this.tag;
	if(Status.ProID == 1){
		skillobject.skillID = 115;
	}else
	if(Status.ProID == 3){
		skillobject.skillID = 302;
	}
	
	var	SData	:	String;
	SData	=	""	+	MonsterInstanceID;
	cc.SendMessage(	"AcceptSummon",	SData	);
}

//@RPC
function	PlayEffect(i:int)
{	// 生成技能影响特效 //
	AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}

//@RPC
function	Playaudio(i:int)
{	// 播放玩家技能声音 //
	audio.PlayOneShot(SkillP[i].skillsound,0.8);
}

function	Startloop(aname:String)
{
	SendMessage("Callloop" , aname , SendMessageOptions.DontRequireReceiver);
	if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_Callloop, aname);
//	photonView.RPC("Callloop",aname);
}

function	stopanimation(aname:String)
{
	SendMessage("changelayer" , aname , SendMessageOptions.DontRequireReceiver);
	if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_changelayer, aname);
//	photonView.RPC("changelayer",aname);
}

private	var temposition:Vector3;
private	function ObjectPosition(i : int) : Vector3
{  
	var ST :Vector3;
	switch (i)
	{				
		case 1:
		case 3:
		case 6:
		case 8:
			ST = transform.position+transform.up*2+transform.forward*2; 
			break;
		case 0:  
		case 4: 
			ST = transform.position+transform.up*2;  
			break;   
		case 5:
		case 11:
			if(	temptarget	)
			{
				ST = temptarget.transform.position+temptarget.transform.up*2;
				temposition = ST;
			}
			else
			{
				if(	temposition	!=	Vector3(0,0,0)	)
					ST = temposition;
				else
					ST = transform.position+transform.forward*6+transform.up*2;     
			}
			break;
		case 2:
		case 7:   
		case 9:  
			ST = transform.position+transform.forward*4+transform.up*0.3;
			break;
		case 10:  
			ST = transform.position -transform.forward +transform.up*Random.Range(2.0,8.0) + transform.right*Random.Range(-3.5,3.5);
			break;            
	}  
	return ST;  
}

/// <summary>
/// 根据Cost值计算实际需要消耗的魔法值 -
/// </summary>
private	function	costmana(i : int) : int
{  
	var costpercent	=	parseInt(SkillP[i].cost.Substring(0,2));
	var costpoint	=	parseInt(SkillP[i].cost.Substring(2,3));
	costpoint		+=	parseInt(Status.Maxmana)*costpercent*0.01;
	return	costpoint;
}

function	getSkillMana(i : int) : boolean
{
	if(	parseInt(	Status.Mana	) >= costmana(	i	)	)
	{
		return false;
	}
	else
	{
		return true;
	}
}

/// <summary>
/// 原始技能数据备份 -
/// </summary>
private	var	OrigSkill	:	Skillclass[];
/// <summary>
/// 原始技能解释信息,Search By SkillIndex In List< SkillP >-
/// </summary>
private	var	OriginUsageDescribtion	:	String[];
/// <summary>
/// 同步更新技能数据，包含战斗数据转换，特殊技能数值更新 -
/// </summary>
private	function	Leveattr(	i : int)
{
// 	for( var o=0; o<SkillP.length; o++)
// 	{	//赋值技能说明//
		SkillP[i].explain	=	OriginUsageDescribtion[i];
//	}

 	SkillP[i].aGet		=	true;
//	SkillP[i].explain	=	AllManage.AllMge.Loc.Get(	SkillP[i].explain	);
	
	SkillP[i].ID		=	Status.ProID	*	100	+	i	+	1;
 	if(	SkillP[i].level	==	0	)
 	{
		SkillP[i].explain	= SkillP[i].explain.Replace("#V",0 +"");
		SkillP[i].explain	= SkillP[i].explain.Replace("#X",0 +"%");
		SkillP[i].explain	= SkillP[i].explain.Replace("#Y",0 +"");
		SkillP[i].explain	= SkillP[i].explain.Replace("#W",0 +"");
		SkillP[i].explain	= SkillP[i].explain.Replace("#Z",0 +"");
		SkillP[i].explain	= SkillP[i].explain.Replace("#T",0 +"");
		SkillP[i].explain	= SkillP[i].explain.Replace("#N",0 +"");     	
 	}
	if(	i>=15	||	SkillP[i].level<10	)
		return;
	var	stATK			=	parseInt(	Status.MaxATK	);
	var	stFou			=	parseInt(	Status.AllFocus	);
	var over1:int		=	parseInt(SkillP[i].level.ToString().Substring(1,1));
		switch (over1)
	{				
	case 0:
		break;
	case 1:    
		SkillP[i].explain = SkillP[i].Branch1;
		break;  
	case 2: 
		SkillP[i].explain = SkillP[i].Branch2;
		break;
	}
	var biao: String    =	SkillP[i].BranchProperty[over1].Substring(0,1);
 	var level1			=	parseInt(SkillP[i].level.ToString().Substring(0,1))-1;
 	var Dapercent		=	parseInt(SkillP[i].levelattribute[level1].Substring(0,3));
 	var damagep			=	parseInt(SkillP[i].levelattribute[level1].Substring(3,3)); 
	SkillP[i].Damage	=	(	stATK	*	Dapercent	*	0.01 + damagep)* (stFou*0.0003+0.7) ;	//技能伤害计算公式//
	SkillP[i].Hatred	=	SkillP[i].Damage ;
	if(	SkillP[i].BranchProperty[0].Substring(0,1)	==	"B"	)
	{
		SkillP[i].buffID	=	parseInt(SkillP[i].BranchProperty[0].Substring(1,2));
		SkillP[i].buffvalue	=	parseInt(SkillP[i].BranchProperty[0].Substring(3,3));
		SkillP[i].bufftime	=	parseInt(SkillP[i].BranchProperty[0].Substring(6,2));
 	}
 	if(	SkillP[i].ID	==	302	)
 	{	//特殊技能初始化//
		if(	level1	==	0	)
			SkillP[i].ObjectSpawnType	=	SkillObjectSpawnType.One;
		else if(	level1	==	1	)
			SkillP[i].ObjectSpawnType	=	SkillObjectSpawnType.Sequence2;
		else if(	level1	==	2	)
			SkillP[i].ObjectSpawnType	=	SkillObjectSpawnType.Sequence3;
	}
	OrigSkill[i].Damage	=	SkillP[i].Damage;
	OrigSkill[i].Hatred	=	SkillP[i].Hatred;
  
	if(	over1	!=	0 )
	{
		if(	biao	==	"B"	)				//解释协议//
		{
			SkillP[i].buffID	=	parseInt(	SkillP[i].BranchProperty[over1].Substring(1,2)	);
			SkillP[i].buffvalue	=	parseInt(	SkillP[i].BranchProperty[over1].Substring(3,3)	);
			SkillP[i].bufftime	=	parseInt(	SkillP[i].BranchProperty[over1].Substring(6,2)	); 
		}
		else if(	biao=="D"	)			//解释协议//
		{
			var	Dpercent		=	parseInt(SkillP[i].BranchProperty[over1].Substring(1,3)); 
			SkillP[i].Damage	=	OrigSkill[i].Damage*Dpercent*0.01;
		}
		else if(	biao=="T"||biao=="H")	//解释协议//
		{
			var Hatpercent		=	parseInt(SkillP[i].BranchProperty[over1].Substring(1,3));
			SkillP[i].Hatred	=	OrigSkill[i].Damage*Hatpercent*0.01;     
		}
		else if(biao=="M")					//解释协议//
		{
			var Manapercent		=	parseInt(SkillP[i].BranchProperty[over1].Substring(1,3));
			SkillP[i].manajia	=	OrigSkill[i].manajia + Manapercent;     
		} 
		else if(biao=="C")					//解释协议//
		{
			var CDpercent		=	parseInt(SkillP[i].BranchProperty[over1].Substring(1,2));
			SkillP[i].CDtime	=	OrigSkill[i].CDtime - CDpercent;     
		}  
		else if(biao=="S")					//解释协议//
		{
			var Sizpercent		=	parseInt(SkillP[i].BranchProperty[over1].Substring(1,3));
			SkillP[i].size		=	OrigSkill[i].size*(Sizpercent*0.01);     
		}
		else if(biao=="E")					//解释协议//
		{
			var Timepercent		=	parseInt(SkillP[i].BranchProperty[over1].Substring(1,3));
			SkillP[i].objlife	=	OrigSkill[i].objlife*(Timepercent*0.01);     
		}
	}
	//替换格式//
	SkillP[i].explain	=	SkillP[i].explain.Replace("#V",	costmana(i)	+	""	);
	SkillP[i].explain	=	SkillP[i].explain.Replace("#X",	Dapercent	+	"%"	);
	SkillP[i].explain	=	SkillP[i].explain.Replace("#Y",	damagep	+	"");
	SkillP[i].explain	=	SkillP[i].explain.Replace("#W",	SkillP[i].buffvalue +"");
	SkillP[i].explain	=	SkillP[i].explain.Replace("#Z",	SkillP[i].bufftime	+"");
	SkillP[i].explain	=	SkillP[i].explain.Replace("#T",	SkillP[i].objlife-5 +"");
	SkillP[i].explain	=	SkillP[i].explain.Replace("#N",	level1	+	1	+	"");
}

private var	targetposition	=	Vector3.zero;
private var targetDirection	=	Vector3.zero;
private var move			=	Vector3.zero;
private var controller : CharacterController;
private var qian			=	-0.1;
private var qianjin			=	-0.1;

/// <summary>
/// 传入发招时候人物移动方式，更改动作状态机冰返回是否需要移动 -
/// </summary>
private	function	Move(	i	:	int	) : boolean
{
	switch (i)
	{				
	case 0:
		break;
	case 1:    
		TController.Buffspeed=0.01; 
		return true;   
	case 2: 
		qian = 4.0;  
		return true;  
	case 3:     
		if(	temptarget&&Vector3.Distance(temptarget.transform.position, transform.position)<=12.0)
		{
			qian = Vector3.Distance(temptarget.transform.position, transform.position)-2;
			return true;
		}
	    else
	    {
			qian = 8.0;  
			return true;	    
		}    
		break;
   case 4:     
		if(	temptarget&&Vector3.Distance(temptarget.transform.position, transform.position)<=16.0)
		{
			qian = Vector3.Distance(temptarget.transform.position, transform.position)-2;
			return true;
		}
	    else
	    {
			qian = 12.0;  
			return true;	    
		}    
		break;
	case 5:
		var hit : RaycastHit;
		var layerMask = 1 << 8;
		layerMask = ~layerMask;
		if(	temptarget	)
		{
			var ppotion = temptarget.transform.position - temptarget.transform.forward*2 +transform.up*2;
//        if (Physics.Linecast (transform.position+transform.up*2, ppotion,hit,layerMask))
//        transform.position = hit.point;
//        else
			transform.position =ppotion;
			return true;
		}
		else
		{
//         Debug.Log("Out of range!");  
			var fwd = transform.TransformDirection (Vector3.forward);
			if (Physics.Raycast (transform.position+transform.up*2, fwd, hit,12,layerMask)) 
				transform.position = hit.point;
			else
				transform.position =transform.position + transform.forward*12+transform.up*2; 
			return true;
		} 
		break;
	}
}

function	FixedUpdate ()
{
	if(	qianjin	>	0	)
	{
		if(	temptarget	)
			targetposition	=	temptarget.position;
		else
			targetposition	=	transform.position + transform.forward*10;
	
		targetDirection		=	targetposition - transform.position;   
		if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
			move				=	targetDirection.normalized*40*Time.deltaTime;
			controller.Move(move);
		}
		qianjin				-=	40*Time.deltaTime;
	}
}

function	AddskillBuff(i:int)
{
	addskillbuff	=	i;
}

//private function Readyskill(ID:int){
//var tt = 1.5;
//var aname : String ="castr0";
//if(ID==305){
// tt=2.0;
//
//}
//    photonView.RPC("Callloop",aname);
//	 yield Timer(tt);
//    photonView.RPC("changelayer",aname);
//}

//private function Timer (ttime: float) {
//	var maxTime : float;
//	maxTime = ttime;
//	while (ttime>=0) {
//		AllManage.actionProCL.SetActionProgress(ttime , maxTime);
//		yield ;  
//		ttime -= Time.deltaTime;  
//		if( !TController.isControllable || alljoy.v==0){
//			skilltime = -1;
////			Debug.Log("Skill Cancel!"); 
//			AllManage.tsStatic.ShowRed("tips097");
//			AllManage.actionProCL.BreakActionProgress();
//			return;
//		}
//	}
//	AllManage.actionProCL.CloseActionProgress();
//}

private function Readyskill(ID:int)
{
	var tt = 1.5;
	var aname : String ="castr0";
	if(ID==305)
	{
		tt=2.0;
	}
	SendMessage("Callloop" , aname , SendMessageOptions.DontRequireReceiver);
//	photonView.RPC("Callloop",aname);
	yield	Timer(tt);
	SendMessage("changelayer" , aname , SendMessageOptions.DontRequireReceiver);
//	photonView.RPC("changelayer",aname);
}
///// <summary>
/////	重新传入当前的技能等级	-
///// </summary>
private	function	Timer(ttime: float)
{ 	
	var maxTime : float;
	maxTime = ttime;
	while (ttime>=0)
	{  
		if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
			AllManage.actionProCL.SetActionProgress(ttime , maxTime);
		yield ;  
		ttime -= Time.deltaTime;  
		if( !TController.isControllable || (alljoy.v>0.2 && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))	)
		{
			skilltime = -1;
			AllManage.tsStatic.ShowRed("tips097");
			if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
				AllManage.actionProCL.BreakActionProgress();
			return;
		}       
	}
	if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		AllManage.actionProCL.CloseActionProgress();
}
/// <summary>
///	重新传入当前的技能等级	-
/// </summary>
function	SetSkillLevel(str : String[])
{
	var i : int = 0;
	for( i=0; i<15; i++)
	{
		SkillP[i].level	=	parseInt(str[i]);
	}
}
/// <summary>
///	获取技能	-
/// </summary>
function	GetSkillInfoAsID(id : int)
{
	for(	var i=0; i<15; i++	)
	{
		Leveattr(i);
		if(	SkillP[i].ID == id	)
		{
//			if(SkillP[i].level.ToString().Substring(0,1) != 3){
//				useSkill = new Skillclass();
//				useSkill = SkillP[i].CopyTo();
//				useSkill = Leveattr1(i); 
//				useSkill.explain = "当前等级: \n" + SkillP[i].explain +"\n 下个等级: \n"+ useSkill.explain;
//				return useSkill;
//			}else{
				return SkillP[i];			
//			}
		}
	} 
	return null;
}

/// <summary>
///	获取技能	-
/// </summary>
function	GetSkillAsID(id : int)
{
	for(var i=0; i<15; i++)
	{
		Leveattr(i);
		if(	SkillP[i].ID	==	id	)
		{
			if(	SkillP[i].level.ToString().Substring(0,1) != "3")
			{
				useSkill = new Skillclass();
				useSkill = Leveattr1(i); 
				if(SkillP[i].level.ToString().Substring(0,1) == "0")
				{
					useSkill.explain = AllManage.AllMge.Loc.Get("info839")+": \n" + AllManage.AllMge.Loc.Get("info689") + "\n [ffff00]" + AllManage.AllMge.Loc.Get("info840") + ": \n"+ useSkill.explain;				
				}
				else
				{
					useSkill.explain = AllManage.AllMge.Loc.Get("info839") + ": \n" + SkillP[i].explain +"\n [ffff00]" + AllManage.AllMge.Loc.Get("info840") + ": \n"+ useSkill.explain;		
				}
				return useSkill;
			}
			else
			{
				SkillP[i].explain = AllManage.AllMge.Loc.Get("info839") + ":\n"+ SkillP[i].explain;
				return SkillP[i];			
			}
		}
	}
	return null;
}

/// <summary>
///	不同步直接获取技数值	-
/// </summary>
function	GetSkillInfoWithI(i : int)
{
	return	SkillP[i];
}

var	useSkill : Skillclass;	//获取信息时用于作为返回值的临时变量//
/// <summary>
///	同步信息	-
/// </summary>
private	function	Leveattr1(i : int) : Skillclass
{
	if(	!SkillP[i].aGet	)
	{
		SkillP[i].aGet = true;
//		SkillP[i].explain = AllManage.AllMge.Loc.Get(SkillP[i].explain);
	}
	useSkill.level	=	SkillP[i].level; 

	if(	useSkill.level == 0	)
	{
		useSkill.level = 10;
	}else{
		useSkill.level	=	(parseInt(useSkill.level.ToString().Substring(0,1)) + 1)*10 + parseInt(useSkill.level.ToString().Substring(1,1));	
	}
	useSkill.ID		=	Status.ProID*100+i+1;
	if(i>=15 || useSkill.level<10)
		return;
	var stATK = parseInt(Status.MaxATK);
	var stFou = parseInt(Status.AllFocus);
	var over1:int = parseInt(useSkill.level.ToString().Substring(1,1));
	switch (over1)
	{				
	case 0:
		break;
	case 1:    
		useSkill.explain = SkillP[i].Branch1;
		break;  
	case 2: 
		useSkill.explain = SkillP[i].Branch2;
		break;
	}
	var biao: String = SkillP[i].BranchProperty[over1].Substring(0,1);
	var level1		=	parseInt(	useSkill.level.ToString().Substring(0,1)	)-1;
	var Dapercent	=	parseInt(	SkillP[i].levelattribute[level1].Substring(0,3));
	var damagep= parseInt(SkillP[i].levelattribute[level1].Substring(3,3)); 
	useSkill.Damage =( stATK* Dapercent*0.01 + damagep)*(stFou*0.0003+0.7);
	useSkill.Hatred = useSkill.Damage ;
	if(SkillP[i].BranchProperty[0].Substring(0,1)=="B")
	{
		useSkill.buffID = parseInt(SkillP[i].BranchProperty[0].Substring(1,2));
		useSkill.buffvalue = parseInt(SkillP[i].BranchProperty[0].Substring(3,3));
		useSkill.bufftime = parseInt(SkillP[i].BranchProperty[0].Substring(6,2));
	}
	if(	SkillP[i].ID	==	302	)	//法师召唤骷髅，根据等级变更创建物体的个数//
	{
		if(	level1 ==0	)
			useSkill.ObjectSpawnType = SkillObjectSpawnType.One;
		else if(level1 ==1)
			useSkill.ObjectSpawnType = SkillObjectSpawnType.Sequence2;
		else if(level1 ==2)
			useSkill.ObjectSpawnType = SkillObjectSpawnType.Sequence3;
	}
  
	if(	over1!=0 && SkillP[i].BranchProperty[over1] )
	{
		if(biao=="B")
		{
			useSkill.buffID		=	parseInt(SkillP[i].BranchProperty[over1].Substring(1,2));
			useSkill.buffvalue	=	parseInt(SkillP[i].BranchProperty[over1].Substring(3,3));
			useSkill.bufftime	=	parseInt(SkillP[i].BranchProperty[over1].Substring(6,2)); 
		}
		else 
		if(biao=="D")
		{
			var Dpercent= parseInt(SkillP[i].BranchProperty[over1].Substring(1,3)); 
			useSkill.Damage = useSkill.Damage*Dpercent*0.01;
		}
		else 
		if(biao=="T"||biao=="H")
		{
			var Hatpercent= parseInt(SkillP[i].BranchProperty[over1].Substring(1,3));
			useSkill.Hatred = useSkill.Damage*Hatpercent*0.01;     
		}
		else 
		if(biao=="M")
		{
			var Manapercent= parseInt(SkillP[i].BranchProperty[over1].Substring(1,3));
			useSkill.manajia =SkillP[i].manajia + Manapercent;     
		} 
		else 
		if(biao=="C")
		{
			var CDpercent= parseInt(SkillP[i].BranchProperty[over1].Substring(1,2));
			useSkill.CDtime =SkillP[i].CDtime - CDpercent;     
		}  
		else 
		if(biao=="S")
		{
			var Sizpercent= parseInt(SkillP[i].BranchProperty[over1].Substring(1,3));
			useSkill.size = SkillP[i].size*(Sizpercent*0.01);     
		}
		else 
		if(biao=="E")
		{
			var Timepercent= parseInt(SkillP[i].BranchProperty[over1].Substring(1,3));
			useSkill.objlife = SkillP[i].objlife*(Timepercent*0.01);     
		}
	}
	useSkill.explain = AllManage.AllMge.Loc.Get(OriginUsageDescribtion[i]).Replace("#V",costmana(i) +"");
	useSkill.explain = useSkill.explain.Replace("#X",Dapercent +"%");
	useSkill.explain = useSkill.explain.Replace("#Y",damagep +"");
	useSkill.explain = useSkill.explain.Replace("#W",SkillP[i].buffvalue +"");
	useSkill.explain = useSkill.explain.Replace("#Z",SkillP[i].bufftime +"");
	useSkill.explain = useSkill.explain.Replace("#T",SkillP[i].objlife-5 +"");
	useSkill.explain = useSkill.explain.Replace("#N",level1+1 +"");     
	return	useSkill;
}

function	resetSkill()
{
	for(	var	i : int = 0; i < SkillP.length; i++)
	{ 
        SkillP[i].cdstime	=	-300;
	}
	busy	=	false;
}

var petArray : GameObject[] = null;
function AddMyPet(obj : GameObject){
	if(petArray == null){
		petArray = new GameObject[1];
		petArray[0] = obj;
	}else
	{
		var usePetArray = new GameObject[petArray.length + 1];
		usePetArray = new GameObject[petArray.length + 1];
		for(var i=0; i<petArray.length; i++){
			usePetArray[i] = petArray[i];
		}
		usePetArray[usePetArray.length - 1] = obj;
		petArray = usePetArray;
	}
}

function ClearPetArray(){
	for(var i=0; i<petArray.length; i++){
		if(petArray[i]){
			Destroy(petArray[i]);		
		}
	}
	petArray = null;
}

function SetConfigurationSkills(ytSkill : yuan.YuanMemoryDB.YuanTable , proid : String){
	var i : int = 0;
	for(i=0; i<SkillP.length; i++){
		for(var rows : yuan.YuanMemoryDB.YuanRow in ytSkill.Rows){
//			print(rows["proType"].YuanColumnText);
//			print(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText);
//			print(rows["skillID"].YuanColumnText);
//			print(SkillP[i].ID.ToString());
			if(rows["proType"].YuanColumnText == (parseInt(proid) - 1).ToString() && rows["skillId"].YuanColumnText == i.ToString()){
				SetSkillAsRow(rows , SkillP[i]);
			}
		}
	}
}

function SetSkillAsRow(rows : yuan.YuanMemoryDB.YuanRow , skill : Skillclass){
	skill.explain = rows["Info"].YuanColumnText;
	skill.name = rows["Name"].YuanColumnText;
	skill.Branch0name = rows["BranchName0"].YuanColumnText;
	skill.Branch1name = rows["BranchName1"].YuanColumnText;
	skill.Branch2name = rows["BranchName2"].YuanColumnText;
	skill.Branch0 = rows["BranchInfo0"].YuanColumnText; 					//分支说明
	skill.Branch1 = rows["BranchInfo1"].YuanColumnText;					//分支说明
	skill.Branch2 = rows["BranchInfo2"].YuanColumnText;		
	skill.BranchProperty = new String[3];			//分支说明
	skill.BranchProperty[0] = rows["BranchProperty0"].YuanColumnText;		//分支属性
	skill.BranchProperty[1] = rows["BranchProperty1"].YuanColumnText;		//分支属性
	skill.BranchProperty[2] = rows["BranchProperty2"].YuanColumnText;		//分支属性
	
	skill.CDtime = parseFloat(rows["Cdtime"].YuanColumnText)/1000;
	skill.starteffect = parseInt(rows["Starteffect"].YuanColumnText);
	skill.mideffect = parseInt(rows["Mideffect"].YuanColumnText);
	skill.endeffect = parseInt(rows["Endeffect"].YuanColumnText);
	skill.ObjectMoveType = parseInt(rows["ObjectMoveType"].YuanColumnText);
	skill.CSMoveType = parseInt(rows["CSMoveType"].YuanColumnText);
	skill.ObjectSpawnType = parseInt(rows["ObjectSpawnType"].YuanColumnText);
	skill.TimeType = parseInt(rows["TimeType"].YuanColumnText);
	skill.Damagetype = parseInt(rows["Damagetype"].YuanColumnText);
	skill.levelattributeStr = rows["levelattribut"].YuanColumnText;
	skill.size = parseFloat(rows["Size"].YuanColumnText)/10;
	skill.rehitime = parseFloat(rows["Rehitime"].YuanColumnText)/1000;
	skill.objlife = parseFloat(rows["Lasttime"].YuanColumnText)/1000;
	skill.maxLevel = parseFloat(rows["maxLevel"].YuanColumnText);

	var skillInfos : String[];
	var skillattrs : String[];
	var i : int = 0;
	skillInfos = skill.levelattributeStr.Split(";"[0]);
	skill.levelattribute = new String[skill.maxLevel];
	
	for(i=0; i<skill.maxLevel; i++){
		skillattrs = skillInfos[i].Split(","[0]);
		if(skillattrs.length > 2){
			skill.levelattribute[i] = skillattrs[1];
		}
	}
}

