private var photonView : PhotonView;
private var dropped : Transform;
private var aa=true;
function Awake () 
{
	photonView = GetComponent(PhotonView);
	animation["idle"].wrapMode = WrapMode.ClampForever ;
	animation.Stop();
	this.tag = "Neutral" ;
}

function	OnClick()
{
	ApplyDamage ();
}

function	ApplyDamage ()
{  
	var ttt =Random.Range(0,100);
	photonView.RPC("Bomba",ttt);
}

@RPC
function	Bomba(ttt:int)
{
	if(aa)
	{
		aa	=	false;
		yield	WaitForSeconds(0.3);
		if(	AllResources.EffectGamepoolStatic == null	)
		{
			return;
		}
		animation.Play("idle");
		AllResources.EffectGamepoolStatic.SpawnEffect(105,transform);
		if(	collider	)
			collider.enabled = false;
		if(	ttt<50	)
			AllResources.PickpoolStatic.SpawnPickup(0,transform);
		else 
		if(	ttt>=50&&ttt<70)
		{
			AllResources.PickpoolStatic.SpawnPickup(1,transform);
		}
		else 
		if(	ttt>=70	&&ttt<90)
		{
			AllResources.PickpoolStatic.SpawnPickup(2,transform);
		}
		else 
		if(	ttt>90	) 
		{
			AllResources.PickpoolStatic.SpawnPickup(3,transform);
		}	        
		this.tag = "Ground" ;
		if(	qiuai.objs	==	this.transform)
			qiuai.objs	=	null;
		yield	WaitForSeconds(5);
		if(	AllResources.EffectGamepoolStatic	==	null	)
		{
			return;
		}
		try
		{
			aa	=	true;
			//if(PhotonNetwork.isMasterClient)
				PhotonNetwork.Destroy(gameObject);  
        }
        catch(e)
        {} 
	}    
}	 


	    