private var emitter: ParticleEmitter;
private var photonView : PhotonView;
private var damage =1;
private var aa=true;
function Awake () 
{  
	photonView	=	GetComponent(PhotonView);
	emitter		=	transform.Find("Particle").GetComponent(ParticleEmitter);
	animation["idle"].wrapMode = WrapMode.ClampForever ;
	animation.Stop();
	this.tag	=	"Neutral" ;
}


function	OnClick()
{
	ApplyDamage ();
}

private	var	LocalInstanceID		:	int;
function	OnEnable ()
{
	LocalInstanceID	=	PlayerUtil.GetNewLocalInstanceID();
	PlayerUtil.RegisterNewLocalObject( LocalInstanceID );
}
function	OnDisable ()
{
	PlayerUtil.UnregisterLocalObject( LocalInstanceID );
}

function ApplyDamage ()
{	
	//photonView.RPC("Bomba",PhotonTargets.All);
	Bomba();
}

//受到伤害直接爆炸//
//@RPC
function	Bomba()
{	
	if(	aa	)	//Ready//
	{
		aa	=	false;
		emitter.emit = true;
		yield	WaitForSeconds(3);
		try
		{
			damage	=	DungeonControl.level*100+50;
			emitter.emit = false;
			animation.Play("idle");
			AllResources.EffectGamepoolStatic.SpawnEffect(112,transform);
			//if(	PhotonNetwork.isMasterClient	)
    			Bomb(damage);			//主客户端管理爆炸//
			if(	collider	)
				collider.enabled = false;
			this.tag = "Ground" ;
			if(	qiuai.objs == this.transform)
				qiuai.objs = null;
		}
		catch(	e	)
		{} 
		yield	WaitForSeconds(5);    

		try
		{
			aa	=	true;
			//if(	PhotonNetwork.isMasterClient	)
				PhotonNetwork.Destroy(	gameObject	);  
		}
		catch(	e	){} 
	}
}	

private	var	explosionRadius	=	15.0;

//造成伤害处理//
function	Bomb(	explosionDamage:int	)
{
	var	explosionPosition	=	transform.position;
	var colliders : Collider[] = Physics.OverlapSphere (explosionPosition, explosionRadius);
	for( var hit	in	colliders )
	{
		var closestPoint = hit.ClosestPointOnBounds(explosionPosition);
		var distance = Vector3.Distance(closestPoint, explosionPosition);
		var hitPoints = 1.0 - Mathf.Clamp01(distance / explosionRadius);
		hitPoints *= explosionDamage;
        var setArray	=	new int[5];
			setArray[0]	=	LocalInstanceID;
			setArray[1]	=	hitPoints;
			setArray[2]	=	hitPoints;
			setArray[3]	=	3;
			setArray[4]	=	DungeonControl.level;
		hit.SendMessageUpwards(	"ApplyDamage",	setArray,	SendMessageOptions.DontRequireReceiver	);
	}
	colliders	=	Physics.OverlapSphere(	explosionPosition,	explosionRadius	);
	for( var hit in colliders )
	{
		if(	hit.rigidbody	)
			hit.rigidbody.AddExplosionForce( 10, explosionPosition, explosionRadius, 3.0 );
	}

}    
	    