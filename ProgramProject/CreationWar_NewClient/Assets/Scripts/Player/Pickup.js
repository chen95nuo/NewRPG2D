enum PickupType { Health = 0, Money = 1,BloodStone =2}
var pickupType = PickupType.Health;
var amount = 1;
var sound : AudioClip;
var xx : int;
private var tempamount:int;
private var i:int;

function Awake(){
    tempamount = amount;
}

var canFly : boolean = false;
var flyDistance : int = 3;
function Update(){
	if(Time.time > flyTime){
		if(PlayerStatus.MainCharacter != null && canFly){
			transform.position = Vector3.Lerp(transform.position , PlayerStatus.MainCharacter.position + Vector3(0 , 1 , 0), Time.deltaTime * 4);
		}
	}
	if(! canFly && PlayerStatus.MainCharacter != null ){
		if(Vector3.Distance( PlayerStatus.MainCharacter.position , transform.position) < flyDistance){
			canFly = true;
		}
	}
}

var flyTime : int = 0;
function OnEnable ()
{   
	flyTime = Time.time + 2;
	amount = tempamount;
	
	if(pickupType==PickupType.Health){
    amount = DungeonControl.level*30 + amount*DungeonControl.level*2;
	}
	else if(pickupType==PickupType.Money)
	amount = Random.Range(0,DungeonControl.level+3)+ amount*DungeonControl.level;
	else if(pickupType==PickupType.BloodStone)
	amount = Random.Range(0,DungeonControl.level*0.1)+ 1;

	if (sound)
	AudioSource.PlayClipAtPoint(sound, transform.position, 1);
}

var ints : int[] = new int[2];
function ApplyPickup (playerStatus : PlayerStatus)
{
	switch (pickupType)
	{		
		case PickupType.Health:
		playerStatus.AddHealth(amount);
		i=72;
    	break;
	
		case PickupType.Money:
			ints[0] = amount;
			AllManage.UICLStatic.CharBarTextMoney(ints);
			AllManage.dungclStatic.pickGold += amount;
//			AllManage.AllMge.UseMoney(-amount , 0 , UseMoneyType.ApplyPickup1 , gameObject , "");
//        playerStatus.UseMoney(-amount , 0);
        i=5;
		break;
								
		case PickupType.BloodStone:
			AllManage.dungclStatic.pickBlood += amount;
// 			AllManage.AllMge.UseMoney(0 , -amount , UseMoneyType.ApplyPickup2 , gameObject , "");
//       playerStatus.UseMoney(0,-amount);
        i=5;
	    break;	
	            
	}	
	return true;
}

function OnTriggerEnter (col : Collider) {
//	if(mover && mover.enabled) return;

	var playerStatus : PlayerStatus = col.GetComponent(PlayerStatus);
	if	(playerStatus)
	ApplyPickup (playerStatus);
	else
		return;
	if (sound)
		AudioSource.PlayClipAtPoint(sound, transform.position, 1);
		AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
		AllResources.PickpoolStatic.UnspawnPickup(xx,gameObject);
		canFly = false;
		flyTime = 0;
}

function Des(){
	flyTime = 0;
	canFly = false;
	AllResources.PickpoolStatic.UnspawnPickup(xx,gameObject);
}

