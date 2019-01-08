class	MonsterSkillNetInit	extends	Photon.MonoBehaviour 
{
	private var Enlarge = false;
	private var EnMove = false;
	private var skillTime = 0.5;
	private var ttr = 0.0;
	private var aa = false ;
	private var tempscale =1.0;
	private var targetp : Vector3;
	private var speed = 1.0;
	private var Size = 1.0;
	private var sizecale = 0.0;

//	var	bb	:	MonsterSkillobject;
	private var	thistransform:Transform;
	function	Awake()
	{
		thistransform	=	this.transform;
	}
 
	function	OnPhotonInstantiate (	msg	:	PhotonMessageInfo	) 
	{
		tempscale	=	thistransform.localScale.x;
		speed		= 	1.0;
	}

	@RPC
	function	scopesy(pp:int,i : int,stime:int)
	{  //范围0小球1中球2大球3大大球4方的5未定
		switch (pp)
		{				
			case 0:
				Size =1;	   
			    thistransform.localScale = Vector3(1,1,1);
			    break;
			case 1:
				Size =2;	   
			    thistransform.localScale = Vector3(2,2,2);
			    break;
			case 2:	
				Size =5;   
			    thistransform.localScale = Vector3(5,5,5);
			    break;
			case 3:	
				Size =8;   
			    thistransform.localScale = Vector3(8,8,8);
			    break;
			case 4:	   
				break;
		}
	 	switch (i)   //0近战1冲锋2方向link3自身范围4目标范围5弹道6pet7缓慢移动8停留陷阱 9zhoubian
		{				
			case 0:	
			case 1:			
				Enlarge = false;
				EnMove = false;
				skillTime = 0.5;
				break;
			case 2:		
				Enlarge = false;
				EnMove = false;
				skillTime = 1.0;
				break;
			case 3:	
				Enlarge = true;
		        thistransform.localScale = Vector3(0.1,0.1,0.1);		
				EnMove = false;
				skillTime = 0.6;
				break;
			case 4:	
				Enlarge = true;
		        thistransform.localScale = Vector3(0.1,0.1,0.1);
				EnMove = false;
				skillTime = 0.5;
				break;
			case 5:	
				Enlarge = false;
				EnMove = true;
				skillTime = 0.8;
				break;
			case 6:	
				Enlarge = false;
				EnMove = false;
				skillTime = 30;
				break;
			case 7:	
				Enlarge = false;
				EnMove = true;
				speed =0.02;
				skillTime = 10;
				break;
			case 8:	
				Enlarge = false;
				EnMove = false;
				skillTime = 10;
				break;
			case 9:	
				Enlarge = false;
				EnMove = true;
				speed =0.02;
				skillTime = 10;
				break;
		}
		if(stime!=500){
		var sstime : float = stime;
		skillTime = sstime*0.001;
		}
	}

	@RPC
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
		GetComponent(	MonsterSkillobject	).skillTime	=	skillTime;
	}

	function	Update ()
	{
		if(	EnMove	&&	aa	)
			thistransform.position = Vector3.Lerp (thistransform.position, targetp,(Time.time-ttr)* 0.1*speed);
		if(	aa	&&	Enlarge	&&	sizecale	<=	Size	)
		{
			sizecale += 10*Time.deltaTime;
			thistransform.localScale += Vector3(10*Time.deltaTime,10*Time.deltaTime,10*Time.deltaTime);
		}
	}

}