#pragma strict

enum	TrapType
{
	dici	= 1,
	qiangci	= 2
}

var tweenDiCI		: GameObject;
var myTripType		: TrapType;
var myTripObjName	: String = "";
function	Start(){}

private var triggerStart : boolean = false;
private var ptime : float = 0;
function	OnTriggerStay(	Other	:	Collider	)
{
	if(	Time.time	>	ptime	&&	!isTrip	)
	{
		if(	(	Other.tag	==	"Player"	||	Other.tag	==	"Enemy"	) && !triggerStart	)
		{
			triggerStart	=	true;
			TripComOn(	true	);
			yield	WaitForSeconds(3);
			TripComOn(	false	);
			ptime	=	Time.time + 2;	
			triggerStart	=	false;
		}
	}
}

private var isTrip : boolean = false; 
function	TripComOn(bool : boolean)
{
	isTrip	=	bool;
	if(	isTrip	)
	{
		TripBoolTrue();
	}
	else
	{
		TripBoolFalse();
	}
}

private var cc : GameObject;
function	TripBoolTrue()
{
	switch(	myTripType	)
	{
		case TrapType.dici : 
			TripMove(	tweenDiCI.transform , "y" , -20 , -18 , 10 , 0.3);
			yield	WaitForSeconds(1);
			TripMove(	tweenDiCI.transform , "y" , -18 , -10 , 40 , 0.3);
			break;
		case TrapType.qiangci : 
			TripMove(	tweenDiCI.transform , "x" , -1 , 0 , 10 , 0.3);
			yield	WaitForSeconds(1);
    		cc	=	PhotonNetwork.Instantiate(myTripObjName , transform.position , transform.rotation,0);			
			yield	WaitForSeconds(0.2);
   			cc	=	PhotonNetwork.Instantiate(myTripObjName , transform.position , transform.rotation,0);		
   			TripMove(	tweenDiCI.transform , "x" , 0 , -1 , 20 , 0.3);	
			yield	WaitForSeconds(0.2);
   			cc	=	PhotonNetwork.Instantiate(myTripObjName , transform.position , transform.rotation,0);			
			break;
	}
}

function	TripBoolFalse()
{
	switch(myTripType)
	{
		case	TrapType.dici : 
			TripMove(	tweenDiCI.transform , "y" , -10 , -20 , 10 , 0.3);
			break;
	}
}

private var TMtimes : int = 0;
function TripMove(trans : Transform , strVec : String , mahStartPosition : float , mahEndPosition : float , mahSpeed : float , mahTime : float)
{
	var bool : boolean = true;
	var times : int = 0;
	TMtimes += 1;
	times = TMtimes;
	switch(strVec)
	{
		case "x" : 
			trans.localPosition.x = mahStartPosition;
			while(	bool && times == TMtimes)
			{
				if(mahStartPosition > mahEndPosition)
				{
					if(trans.localPosition.x > mahEndPosition)
					{
						trans.localPosition.x -= mahSpeed * Time.deltaTime;
					}
					else
					{
						trans.localPosition.x = mahEndPosition;
						bool = false;
					}
				}
				else
				if(mahStartPosition < mahEndPosition)
				{
					if(trans.localPosition.x < mahEndPosition)
					{
						trans.localPosition.x += mahSpeed * Time.deltaTime;
					}
					else
					{
						trans.localPosition.x = mahEndPosition;
						bool = false;
					}	
				}
				yield;
			}
			break;
		case "y" :
			trans.localPosition.y = mahStartPosition;
			while(bool && times == TMtimes)
			{
				if(mahStartPosition > mahEndPosition)
				{
					if(trans.localPosition.y > mahEndPosition)
					{
						trans.localPosition.y -= mahSpeed * Time.deltaTime;
					}
					else
					{
						trans.localPosition.y = mahEndPosition;
						bool = false;
					}
				}
				else
				if(	mahStartPosition < mahEndPosition	)
				{
					if(	trans.localPosition.y < mahEndPosition	)
					{
						trans.localPosition.y += mahSpeed * Time.deltaTime;
					}
					else
					{
						trans.localPosition.y = mahEndPosition;
						bool = false;
					}	
				}
				yield;
			}
			break;
		case "z" :
			trans.localPosition.z	=	mahStartPosition;
			while(	bool && times	==	TMtimes	)
			{
				if(	mahStartPosition > mahEndPosition	)
				{
					if(trans.localPosition.z > mahEndPosition)
					{
						trans.localPosition.z -= mahSpeed * Time.deltaTime;
					}
					else
					{
						trans.localPosition.z = mahEndPosition;
						bool = false;
					}
				}
				else
				if(	mahStartPosition < mahEndPosition	)
				{
					if(	trans.localPosition.z < mahEndPosition	)
					{
						trans.localPosition.z += mahSpeed * Time.deltaTime;
					}
					else
					{
						trans.localPosition.z = mahEndPosition;
						bool = false;
					}	
				}
				yield;
			}
			break;  
	}
	bool	=	false;
}
