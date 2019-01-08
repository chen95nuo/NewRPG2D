#pragma strict

private var mAI : MonsterAI; 
private var ms : MonsterStatus; 
var obj : GameObject;
var buffID : int = 0;
var buffValue : int = 0;
var buffTime : int = 0;

function	Start()
{
	mAI	= obj.GetComponent(MonsterAI);
	ms	= obj.GetComponent(MonsterStatus);
}

function	OnTriggerEnter(Other : Collider)
{
	if(	Other.tag == "Enemy"	&&	mAI.targetm == Other.transform	)
	{
			var setArray = new int[4];
			setArray[0]= ms.PlayerID;
			setArray[1]= buffID;            
			setArray[2]= buffValue; 
			setArray[3]= buffTime;                                          						
			Other.SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver ); 
			obj.SendMessage("RPCDie",setArray,SendMessageOptions.DontRequireReceiver ); 
	}
}