class	BulletNetInit	extends	Photon.MonoBehaviour
{
	private var targetp	:	Vector3;
	private var target	:	Transform;
	private var ttr		=	0.0;
	private var aa		=	false ;
	var	useTrans	:	PhotonView;
 	private	var	thistransform:Transform;
	function	Awake()
	{
		thistransform	=	this.transform;
	}
	//@RPC
	var	go	:	GameObject;
	function	findtarget( ID : int )
	{
		if(	ID	==	PlayerUtil.myID	)
		{
			go	=	PlayerStatus.MainCharacter.gameObject;
		}
		else
		{
			go	=	ObjectAccessor.getAOIObject(ID);			
		}
		if(	go	==	null	)
		{
			go = MonsterHandler.GetInstance().FindMonsterByMonsterID(ID);			
		}
		if(	go	!=	null	)
		{
			target = go.transform;	
		}
		ttr		=	Time.time;
		aa		=	true;
	}
	//@RPC
	function	findtargetp(pp:Vector3)
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
		ttr		=	Time.time;
		aa		=	true;
	}

	function	Update ()
	{
		if(	aa	&&	target	)
		{
			thistransform.position = Vector3.Lerp (thistransform.position, target.position+target.transform.up*2,(Time.time-ttr)* 0.9);
			thistransform.LookAt(	target.position	+	target.transform.up	*	2	);
		}
		else 
		if(	aa	&&	!target	)
		{
			thistransform.position = Vector3.Lerp (thistransform.position, targetp,(Time.time-ttr)* 0.3);
			thistransform.LookAt(	targetp	);
		}
	}
}