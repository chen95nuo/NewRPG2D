#pragma strict
private var Status:MonsterStatus;
var buffID :int;
var buffvalue : int;
var bufftime : int;

function	Start () 
{
	Status = GetComponent(MonsterStatus);
}

function	Behit(target:Transform)
{
	if(	buffID !=0	)		//升级减速之后受攻击给攻击者施加减速//
	{
		var setArray	= new int[4];
			setArray[0]	= Status.PlayerID;
			setArray[1]	= buffID;
			setArray[2]	= buffvalue;
			setArray[3]	= bufftime;
		target.SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver);
	}
}