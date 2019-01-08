#pragma strict
var emits	:	ParticleEmitter;
var Popeff	:	GameObject;
var Dontdis	=	false;
function	Start ()
{
	if(	emits	)
		emits.emit	=	false; 
	if(	Popeff	)
		Popeff.SetActiveRecursively(	false	);
}

var colder : Collider;
function	DoSomething(time : int)
{
	colder	=	GetComponent(	Collider	);
	if(	colder	)
	{
		colder.enabled	=	false;
	}
	if(	emits	)
		emits.emit		=	true; 
	yield	WaitForSeconds(	1	);
	if(	Popeff	)
		Popeff.SetActiveRecursively(	true	);
    TimeOut(	time	);
}


function	TimeOut(	time	:	int	)
{
	yield	WaitForSeconds(	time	);
	if(	!Dontdis	)
		PhotonNetwork.Destroy(	gameObject	);  
}

