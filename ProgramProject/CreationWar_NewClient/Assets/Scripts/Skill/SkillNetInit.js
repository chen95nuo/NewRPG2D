class	SkillNetInit	extends	Photon.MonoBehaviour
{
	private var subskillmove = 0.0;
	private var Size=1.0;
	private var skillID :int;
	private var target : Transform;
	private var targetp : Vector3;

	private var Enforcetarget = false;
	private var Enlarge	=	false;		//是否会变大//
	private var EnMove = false;
	private var ttr = 0.0;
	private var aa = false ;
	private var tempscalex	=	1.0;
	private var tempscaley	=	1.0;
	private var tempscalez	=	1.0;
	private var sizecale	=	0.1;
	private var speed		=	1.0;
	var bb : Skillobject;
	private var thistransform:Transform;
	
	function	Awake()
	{
		thistransform	=	this.transform;
	}

	function	OnPhotonInstantiate (msg : PhotonMessageInfo)
//	function	Start ()
	{
	    tempscalex	= thistransform.localScale.x;
	    tempscaley	= thistransform.localScale.y;
	    tempscalez	= thistransform.localScale.z;
	    sizecale	= 0.1*tempscalex;     
	    if(	photonView.isMine	)
	    {
	        bb.enabled = true;
	        bb.timeout();
	        this.collider.enabled = true;
	    }
	    else
	    {
	        bb.enabled = false;
	    	this.collider.enabled = false;
	    }
	}

	//@RPC
	function	IDsy(objs : Object[])
	{   //skillID,subskillmove,Size,skilltype
		var ID:int;
		var submove:float;
		var size:float;
		var i:int;
		
		ID = objs[0];
		submove = objs[1];
		size = objs[2];
		i = objs[3];

		skillID = ID;
		subskillmove = submove;
		Size = size;
		thistransform.localScale = Vector3(tempscalex*Size,tempscalex*Size,tempscalex*Size);
		switch (i)
		{
			case 0:						
			case 1:	
			case 2:
			case 7:	
			case 9:
				Enforcetarget = false;
				Enlarge = false;
				EnMove = false;
		//		transform.localScale = Vector3(4*Size,4*Size,4*Size);
				break;
			
			case 3:		
				Enforcetarget = false;
				Enlarge = true;	
				EnMove = true;
				thistransform.localScale = Vector3(0.1*tempscalex,0.1*tempscaley,0.1*tempscalez);
				break;
					
			case 4:	
			case 5:
				Enforcetarget = false;	
				Enlarge = true;
				EnMove = false;
				thistransform.localScale = Vector3(0.1*tempscalex,0.1*tempscaley,0.1*tempscalez);
				break;
						
			case 6:	
			case 8:
			case 10:			
				Enforcetarget = true;
				Enlarge = false;
				EnMove = true;
				if(skillID==310)
					speed = 0.01;
				break;	
			
			case 11:
				Enforcetarget = true;
				Enlarge = false;
				EnMove = true;
				speed = 0.05;
				break;	
		}


	}
	//private var ObjFind : PhotonView;
	private var ObjFind : GameObject;
	//@RPC
	function	findtarget(ID:int)
	{
		if(	ID == PlayerUtil.myID	)
		{
			ObjFind = PlayerStatus.MainCharacter.gameObject;
		}
		else
		{
			ObjFind = ObjectAccessor.getAOIObject(ID);			
		}
		
		if(ID < 0){
			ObjFind = FindOtherTarget(ID);
		}
		
		if(	ObjFind == null	)
		{
			ObjFind = MonsterHandler.GetInstance().FindMonsterByMonsterID(ID);			
		}
		if(	ObjFind	)
		{
			target = ObjFind.transform;
		}
	}

	private var otherPlayerS : PlayerStatus[];
	function FindOtherTarget(id : int) : GameObject{
		otherPlayerS = FindObjectsOfType(PlayerStatus);
		for(var i=0; i<otherPlayerS.length; i++){
			if(otherPlayerS[i] != null && otherPlayerS[i].instanceID == id){
				return otherPlayerS[i].gameObject;
			}
		}
		return null;
	}
	
	//@RPC
	function	findtargetp(	pp	:	Vector3	)
	{
		if(	pp	==	Vector3.zero	)
		{
			targetp	=	transform.position	+	transform.forward	*	40;
		}
		else
		{
			//KDebug.Log(	"findtargetp + "	+	pp.ToString()	);
			targetp	=	pp;
		}
		//targetp	=	pp;
		ttr		=	Time.time;
		aa		=	true;
		if(	speed	==	0.05	&&	!target	)
			EnMove = false;
	}

	function	Update()
	{
		if(	EnMove && aa && Enforcetarget && target )	//追击目标//
		{
			thistransform.position	=	Vector3.Lerp( thistransform.position + thistransform.right * subskillmove * 0.1, target.position + target.transform.up*2, (Time.time-ttr)* 0.8*speed);
			thistransform.LookAt(	target.position	+	target.transform.up	*	2,	Vector3.up	);
		}
		else
		if(	EnMove&&aa && ( !Enforcetarget || !target ) )	//不追击目标//
		{
			thistransform.position	=	Vector3.Lerp (thistransform.position, targetp,(Time.time-ttr)* 0.1*speed);
			thistransform.LookAt(	targetp,	Vector3.up );
		}
		if(	aa && Enlarge && sizecale <= Size * tempscalex	)
		{
			sizecale += 10*Time.deltaTime;
			thistransform.localScale	+=	Vector3( 10 * Time.deltaTime, 10 * Time.deltaTime, 10 * Time.deltaTime );
		}
	}

}