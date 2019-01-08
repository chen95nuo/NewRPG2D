
#pragma strict 
var Poplist		: GameObject[];
var Taskpoplist	: GameObject[];
var baoxiang	: GameObject;
private var reloadtime	=	300;	//6分钟重新刷一次//
var PopPrefab	: GameObject ;
var pp			: boolean = true;
var currentObject : GameObject;
var PopName		: String;
var photonView	: PhotonView;
private	var	ObjectID = 0;	//记录当前的这个生产出的物品的ID//

var Popid : String = "";
function	Start ()
{
	move();
	while(!AllManage.mtwStatic || !AllManage.mtwStatic.PopInit)
	{
		yield;
	}
	yield;
	yield;
	yield;
	MakePop();
	InvokeRepeating("MakePop",0,reloadtime+1);
}

function	move()
{
	var hit : RaycastHit;
	if(	Physics.Raycast(	transform.position,	-Vector3.up,	hit,	200.0	)	)
	{ 
		if(	hit.collider.CompareTag ("Ground")	)   
			transform.position = hit.point + Vector3(0,0.5,0) ;
    }
}

function	randomPop()	:	GameObject 
{
	var pPrefab	=	Poplist[Random.Range(0,Poplist.Length)];	
	var useInt : int = 0;
	useInt		=	Random.Range(0,10);
	if(	Popid.Length	>	2	)
	{
		pPrefab	=	Taskpoplist[Mathf.Clamp( parseInt(Popid) - 1  , 0,	12	)];
	}
	else 
	if(	Random.Range(	0,	1000	)	>	998	)
		pPrefab	=	baoxiang;
	return	pPrefab;
}

//物品生成点//
function	MakePop ()
{
	PopPrefab	=	randomPop();
	if(	pp && currentObject	==	null)
	{
		currentObject	=	PhotonNetwork.InstantiateSceneObject(PopPrefab.name, transform.position, Quaternion.identity, 0 , null);
		currentObject.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
		pp	=	false;
		//photonView.RPC("TongBupp",PhotonTargets.AllBuffered, false);        	
		ObjectID	=	currentObject.GetComponent(PhotonView).viewID;       	
		//photonView.RPC("TongBuID",PhotonTargets.AllBuffered,ObjectID );	
		cooldown();	
	}
}

//@RPC
//function TongBupp( p:boolean){
//pp=p;
//
//}

//@RPC
//function TongBuID(q:int){
//ObjectID =q;
//}

function	cooldown()	
{
	yield	WaitForSeconds(	reloadtime	);
	pp	=	true;
	//photonView.RPC("TongBupp",PhotonTargets.AllBuffered, true); 
}
