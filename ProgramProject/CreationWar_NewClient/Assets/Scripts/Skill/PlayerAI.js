#pragma strict
class	PlayerAI	extends	Song
{
	var agent	:	NavMeshAgent;
	private var Status	:	PlayerStatus;
	var CS		:	Transform;
	var targetp	:	Transform;
	private var delayAttackTime	=	1.8;
	private var lastatktime	=	0.0;
	private var lastsktime	=	0.0;
	private var remoteRange =	20;
	
	private var skilltime	=	0.0;
	private var	skillCD		=	8;
	private var offset		:	Vector3;
	var	Canremote	=	false;

	var	Skillbutton		:	SkillItem[];
	var attackbutton	:	GameObject;
	var xuepinbutton	:	GameObject;
	private	var	Starspeed	=	0.0;
	private	var	TController	:	ThirdPersonController;
	private var quan : qiuai;
	var buttonMess : UIButtonMessage;
	var textBtn : TestButton;
	var HangP : HangUp;
	function	Start()
	{
		if(PlayerPrefs.GetInt("AutoAttack", 0) == 1){
			buttonMess.enabled = true;
			textBtn.enabled = false;
		}else
		if(PlayerPrefs.GetInt("AutoAttack", 0) == 0){
			buttonMess.enabled = false;
			textBtn.enabled = true;
		}
		AllManage.pAIStatic = this;
		AutoAI	=	false;
		yield;
		yield;
		yield;
		while(	!PlayerStatus.MainCharacter	)
		{
			yield;
		}
		CS			=	PlayerStatus.MainCharacter;
		TController	=	CS.GetComponent(ThirdPersonController);
		agent		=	CS.GetComponent.<NavMeshAgent>();
		Starspeed	=	agent.speed;
		Status		=	CS.GetComponent(PlayerStatus);
		yield;
		yield;
		yield;
		switch(	Status.ProID	)
		{ 
			case 1: 
				Canremote = false;
				break; 
			case 2:  									
				if(	Status.weaponType	==	PlayerWeaponType.weapon2	)
					Canremote = true;
				else
					Canremote = false;
				break;
			case 3:
				Canremote = true;
				break; 
		}
	}

	function SwitchAttack(){
		if(PlayerPrefs.GetInt("AutoAttack", 0) == 1){
			AutoAttackSimple();
		}else
		if(PlayerPrefs.GetInt("AutoAttack", 0) == 0){
			Attackn();
		}
	}

	var AutoAIType : int = 0;
	function AutoAttackSimple(){
		if(AllManage.SkillCLStatic == null){
			return;
		}
		if(AllManage.dungclStatic.isAutoAttackDungeonClear()){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0120"));
			return;
		}
		if(!AutoAI){
			StartAI();
		}
		AutoAIType = CommonDefine.AutoAttackSimple;
	}
	
	function AutoTrusteeship(){
		if(! AllManage.InvclStatic.CanMapManaged( AllManage.dungclStatic.mtwMapID + AllManage.dungclStatic.NowMapLevel.ToString())){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1078"));
			return;
		}
		if(AllManage.dungclStatic.isAutoAttackDungeonClear()){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0120"));
			return;
		}
		if(AllManage.SkillCLStatic == null || Status == null){
			return;
		}
//		if(parseInt(Status.Level) >= 10){
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.AutoPlay , "");		
			if(!AutoAI){
				StartAI();
			}
		AutoAIType = CommonDefine.AutoTrusteeship;
//		}else{
//			AllManage.tsStatic.Show("meg0113");
//		}
	}

	static	var	AutoAI	=	false;
	var	quanUI : UISprite;
	private var boolCondition : boolean = false;
	private var autoTargetp : Transform;
	function	StartAI()
	{
		if(	AutoAI	==	false && Status)
		{
		 	if(quan == null&& PlayerStatus.MainCharacter){
				quan = PlayerStatus.MainCharacter.gameObject.GetComponent(qiuai);
			}	    
		    quan.canFindTarget = false;
//		    qiuai.objs = null;
		    
			if(AutoAIType == CommonDefine.AutoTrusteeship){
				if(Status.AutoAITime <= 1){
					GouMai();
					return;
				}
		    	AllManage.tsStatic.Show( "tips069" );
			}
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior( yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt( yuan.YuanPhoton.GameFunction.AutoPlay ).ToString() );
			AutoAI = true;
			
		    	
		    if(AutoAIType == CommonDefine.AutoAttackSimple){
		    	while(CanSeeTarget() == 0){
					if(! AllManage.AttackButtonStatic.Faguan.enabled){
						quanUI.enabled	= false;
						AllManage.AttackButtonStatic.Faguan.enabled = true;
					}
		    		if(! BreakAutoAttack()) break;			
		    		yield Idle();
		    	}
				autoTargetp =	FindClosestEnemy();
		    }
		    boolCondition = GetCondition();
		    
			while(boolCondition &&! AllManage.dungclStatic.DungeonIsDone)
			{
				if(HangP.IsEnable){
					HangP.IsEnable = false;
				}
				if(AutoAIType == CommonDefine.AutoTrusteeship && !quanUI.enabled){
					quanUI.enabled	= true;
					AllManage.AttackButtonStatic.Faguan.enabled = false;
				}else
				if(AutoAIType == CommonDefine.AutoAttackSimple && ! AllManage.AttackButtonStatic.Faguan.enabled){
					quanUI.enabled	= false;
					AllManage.AttackButtonStatic.Faguan.enabled = true;
				}
				if(	parseInt(	Status.Maxhealth	)	*	0.3 > parseInt(	Status.Health	)	&& AutoAIType == CommonDefine.AutoTrusteeship)
				{
					UseXueping();
				}
				if(	CanSeeTarget()	==	2	)
				{
					if(! BreakAutoAttack()) break;			
					yield StartCoroutine( "Attack" );
				}
				else 
				if(	CanSeeTarget()	==	1	)
				{
					if(! BreakAutoAttack()) break;			
					yield StartCoroutine( "FindTarget" );
				}
				else{
					if(! BreakAutoAttack()) break;			
					yield Idle();
				}
					
				boolCondition = GetCondition();
			}
			if(! HangP.IsEnable){
				HangP.IsEnable = false;
			}
		    quan.canFindTarget = true;
			
			if(AutoAIType == CommonDefine.AutoTrusteeship)
				AllManage.tsStatic.Show( "tips070" );
			quanUI.enabled	=	false;
			AutoAI	=	false;	
			AllManage.AttackButtonStatic.Faguan.enabled = false;	
		}
		else
		{
			if( Status.AutoAITime <= 1 && AutoAIType == CommonDefine.AutoTrusteeship )
			{
				GouMai();
	//			ts.Show("托管时间不足。");		
			}
		}
		AutoAIType = CommonDefine.AutoNON;
	}

	private var joy : alljoy;
	function GetCondition() : boolean{
		if(joy == null && PlayerStatus.MainCharacter){
			joy = PlayerStatus.MainCharacter.gameObject.GetComponent(alljoy);
		}
		if(joy == null){
			return false;
		}
	    if(AutoAIType == CommonDefine.AutoTrusteeship){
	    	return !Status.dead && AutoAI && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.1 && Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.1 && !AllManage.dungclStatic.isAutoAttackDungeonClear() && AutoAIType != CommonDefine.AutoNON && joy.GetJoyStickV() < 0.1 && joy.GetJoyStickH() < 0.1;//&& Status.AutoAITime > 1 
	    }else
	    if(AutoAIType == CommonDefine.AutoAttackSimple){
	    	return !Status.dead && AutoAI && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.1 && Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.1 && !AllManage.dungclStatic.isAutoAttackDungeonClear() && autoTargetp != null && (autoTargetp.tag == "Enemy" || autoTargetp.tag == "Neutral") && AutoAIType != CommonDefine.AutoNON  && joy.GetJoyStickV() < 0.1 && joy.GetJoyStickH() < 0.1; 
	    }
	    else
	    if(AutoAIType == CommonDefine.AutoNON){
	    AutoAIType = CommonDefine.AutoTrusteeship;
	    return true;
	    }
	    return false;
	}

	var ptime	: int;
	var useStr	: String;
	var useInt	: int;
	var LabelTime	: UILabel;
	var LabelTime1	: UILabel;
	function	Update()
	{
//		print(TController.Movespeed	+ " == " + 	Starspeed	+ " == " + 	TController.Buffspeed);
//		if(	Time.time > ptime && Status	)
//		{
//			if(	AutoAI && AutoAIType == CommonDefine.AutoTrusteeship)
//			{
//				ptime = Time.time + 1;
//				Status.AddAutoAITime(1);
//			}
//			if(	Status.AutoAITime < 60	)
//			{
//				useStr = Status.AutoAITime + "s";
//			}
//			else
//			if(	Status.AutoAITime < 3600	)
//			{
//				useStr = (Status.AutoAITime / 60) + "m";			
//			}
//			else
//			{
//				useStr = (Status.AutoAITime / 3600) + "h";						
//			}
//			if(	LabelTime.text != useStr	)
//			{
//				LabelTime.text = useStr;		
//			}
//			if(	LabelTime1.text != useStr	)
//			{
//				LabelTime1.text = useStr;		
//			}
//		}
	}

	function	GouMai()
	{
		//TD_info.panelStatistics("托管");
		if(	PlayerPrefs.GetInt(	"ConsumerTip" , 0) == 1	)
			AllManage.qrStatic.ShowBuyQueRen(gameObject ,"YesMai" , "NoMai" , "messages015");	
		else
			YesMai();
	}

	function	YesMai()
	{
		if(	Status == null && PlayerStatus.MainCharacter )
		{
			Status = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesBuy , 0 , 0 , "" , gameObject , "realYesMai");
		//AllManage.AllMge.UseMoney(0 , 50 , UseMoneyType.YesBuy , gameObject , "realYesMai");
		//if(Status.UseMoney(0 , 50)){
		//}
	}

	function	realYesMai()
	{
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.BuyAutoPlay).ToString());
		AddTuoGuanTime();
	}
	function	NoMai()
	{
	}
	function	AddTuoGuanTime()
	{
		if(	Status	==	null	&&	PlayerStatus.MainCharacter	)
		{
			Status	=	PlayerStatus.MainCharacter.gameObject.GetComponent(	PlayerStatus	);
		}
		Status.AddAutoAITime(-300);
	}

	function	CanSeeTarget () : int 
	{
		targetp	=	FindClosestEnemy();
		if(	!targetp	)
			return 0;	
		else
		{
			var dis	=	Vector3.Distance(	CS.position,	targetp.position	);    
			if(	dis > 100	)
				return 0;	
			else
			if(	dis > 26	)
			  return 1;	
			else
			{	  
				//var hit : RaycastHit;	
				//if (Physics.Linecast (CS.position, targetp.position, hit))
				return 2;
			}
		}
	}

	function	Idle ()
	{
		if(	!targetp)
			targetp	=	FindClosestEnemysp();
		if(	targetp	)
			{
				Findway(	targetp.position	); 
//				yield	WaitForSeconds(2); 
				if(! BreakAutoAttack()) return;			
				yield	WaitForSeconds(0.33); 
				if(! BreakAutoAttack()) return;			
				yield	WaitForSeconds(0.33); 
				if(! BreakAutoAttack()) return;			
				yield	WaitForSeconds(0.33); 
				
				if(AutoAIType == CommonDefine.AutoTrusteeship)
					AllManage.tsStatic.Show(	"tips071"	); 
			}
	}
	
	private	var	TempPos	:	Vector3;
	function	FindTarget()
	{
		TempPos				=	FindClosestEnemy().position;
		AttackFindeWayPos	=	transform.position	-	TempPos;
		AttackFindeWayPos	=	AttackFindeWayPos.normalized;
		AttackFindeWayPos	=	TempPos	+	AttackFindeWayPos;
		Findway(	TempPos	);
		if(! BreakAutoAttack()) return;		
		yield	WaitForSeconds(0.25); 
		if(! BreakAutoAttack()) return;		
		yield	WaitForSeconds(0.25); 
	}

	function	UseXueping()
	{
		xuepinbutton.SendMessage(	"OnClick",	SendMessageOptions.DontRequireReceiver	);
	}

	private var snum	:	int	=	1;
	private var angle	=	180.0;
	private	var	AttackFindeWayPos	:	Vector3;
	function	Attack()
	{
		agent.speed	=	TController.Movespeed	*	Starspeed	*	TController.Buffspeed; 
		if(	!targetp	)
			return;
		offset = CS.position - targetp.position;   
		if((Canremote && offset.magnitude < remoteRange)||(!Canremote && offset.magnitude < 7.0))
		{
			if(	agent.enabled	)
				agent.destination	=	CS.position;
			angle	=	180.0;
			while(	angle > 5	&&	!TController.yun	&&	!TController.bing	&&	!Status.dead	&&	targetp	)
			{     
				angle	=	Mathf.Abs(	RotateTowardsPosition(	targetp.position,	200	)	);
				if(! BreakAutoAttack()) return;	
				yield;
			}
			if(AutoAIType == CommonDefine.AutoTrusteeship){
				for(	var	Skilla :	SkillItem	in	Skillbutton	)
				{
					if(	Time.time > 1.5 + lastsktime  && 
						Skilla.skillID !="" && 
						Skilla.invSprite.enabled && 
						!Skilla.SpriteNotMana.enabled && 
						((Canremote && offset.magnitude < remoteRange)||
						(!Canremote && offset.magnitude < 7.0)	)
						)
					{
						lastsktime = Time.time ;
						Skilla.OnClick();
					}
					if(! BreakAutoAttack()) return;		
					yield;
				}	
			}
			if(! BreakAutoAttack()) return;	
			yield;
			if(	Time.time > delayAttackTime*Status.Attackspeed + lastatktime )
			{
				if(! BreakAutoAttack()) return;	
				Attackn();
//				StartCoroutine("Attackn");
			}

		}
		else
		{
			AttackFindeWayPos	=	transform.position	-	targetp.position;
			AttackFindeWayPos	=	AttackFindeWayPos.normalized;
			AttackFindeWayPos	=	targetp.position	+	AttackFindeWayPos;
			Findway(	AttackFindeWayPos	); 
		}  
		if(	TController.yun	||	TController.bing	||	Status.dead	)
		{
			try
			{
				if(	agent.enabled	)
					agent.destination	=	CS.position;
				//agent.Stop();
			}
			catch(e)
			{}
			return;
		}
		else 
		if(	!targetp	) 
		{   if(	agent.enabled	)
			agent.Resume();
			return;
		}  
		return;
	}

	function	Attackn()
	{
//		print(BreakAutoAttack() + "   Attackn +++++++++++ " + Time.time + " vvvvvvvv " + Input.GetAxisRaw("Vertical") + "  hhhhhhhhhhhh " + Input.GetAxisRaw("Horizontal"));
		AllManage.SkillCLStatic.AttackSimple();
//		attackbutton.SendMessage(	"OnPress", true , SendMessageOptions.DontRequireReceiver	);
//		yield	WaitForSeconds(	0.1	);
//		attackbutton.SendMessage(	"OnClick", SendMessageOptions.DontRequireReceiver	);
	}

	function	Findway(targetPosition : Vector3)
	{
//		KDebug.Log( "寻路",targetPosition,Color.red );
//		if(	!agent.enabled	)
//		{
//			if(	(	targetPosition	-	CS.position	).sqrMagnitude	<	0.1f	)
//			{
//				KDebug.Log( "原地寻路",transform,Color.red );
//				return;
//			}
//		}
		agent.enabled	=	true;
		agent.speed		=	TController.Movespeed * Starspeed * TController.Buffspeed;
		CS.SendMessage(	"UdateAgentTargetsNoTs" , targetPosition, SendMessageOptions.DontRequireReceiver	);
	}

	private function	RotateTowardsPosition (targetPos : Vector3, rotateSpeed : float) : float
	{
		var relative = CS.InverseTransformPoint(targetPos);
		var angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
		var maxRotation = rotateSpeed * Time.deltaTime;
		var clampedAngle = Mathf.Clamp(angle, -maxRotation, maxRotation);
		CS.Rotate(0, clampedAngle, 0);
		return angle;
	}

	private	function	FindClosestEnemy () : Transform
	{

		if(AutoAIType == CommonDefine.AutoAttackSimple && qiuai.objs != null){
//			print(qiuai.objs + " ================ ");
			return qiuai.objs;
		}
		var gos : GameObject[];
		gos = GameObject.FindGameObjectsWithTag("Enemy");
		var closest : GameObject;
		var distance = Mathf.Infinity;
		for (var go : GameObject in gos) 
		{
			var diff = (go.transform.position - CS.position);
			var curDistance = diff.sqrMagnitude;
			if(	curDistance < distance	) 
			{
				closest = go;
				distance = curDistance;
			}
		}
		if(	closest	){
			qiuai.objs = closest.transform;
//			print(closest + " ================ ");
			return closest.transform;
		}
		else
		{
			if(qiuai.objs != null){
				closest = qiuai.objs.gameObject;
//				print(closest + " ================ ");
				return closest.transform;
			}else{
				return null;			
			}
		}
	}

	private	function	FindClosestEnemysp () : Transform
	{ 
	   var clostenemy : Transform = FindClosestEnemy();
	   if(clostenemy!=null)
	   return clostenemy;
	   else{
		var gos : MonsterSpawn[];
		gos	=	FindObjectsOfType(MonsterSpawn);
		var closest : MonsterSpawn;
		var distance = Mathf.Infinity;
		for (var go : MonsterSpawn in gos) 
		{
			if(	go	)
			{
				var diff = (go.transform.position - CS.position);
				var curDistance = diff.sqrMagnitude;
				if (go.IsAbleToSpawn() && !go.IsCleared() && curDistance < distance) 
				{
					closest = go;
					distance = curDistance;
				}
			}
		}
		if(	closest	)
			return	closest.transform;
		else
			return	null;
			}
	}	
	
	function BreakAutoAttack(){
		if(joy == null && PlayerStatus.MainCharacter){
			joy = PlayerStatus.MainCharacter.gameObject.GetComponent(alljoy);
		}
		if(joy == null){
			return false;
		}	
		if( Mathf.Abs(Input.GetAxisRaw("Vertical"))>= 0.1 || Mathf.Abs(Input.GetAxisRaw("Horizontal")) >= 0.1 || joy.GetJoyStickV() >= 0.1 || joy.GetJoyStickH() >= 0.1){
//			print(Time.time);
			return false;
		}
		return true;
	}
	
	function GetStarspeed(){
		return Starspeed;
	}
	
	function StopAutoAttack(){
		boolCondition = false;
		AutoAIType = CommonDefine.AutoNON;
	}
	
}