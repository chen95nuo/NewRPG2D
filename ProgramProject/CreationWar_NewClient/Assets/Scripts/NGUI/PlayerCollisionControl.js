#pragma strict

class PlayerCollisionControl extends XmlControl{
	function Start () {
//		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
//		{
////			UICL = FindObjectOfType(UIControl);
//			agentmotion	= GetComponent(agentLocomotion);
//		}
	}
		
	function Stone(ts : TriggerStone){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		AllManage.UICLStatic.FindOreStone(ts);
	}
	
	function ExitStone(){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		AllManage.UICLStatic.ExitOreStone();
	}
	
	function ExitFish(){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		AllManage.UICLStatic.ExitFish();
	}
	
	function ExitFlag(){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		AllManage.UICLStatic.ExitFlag();
	}
	
	function ExitFood(){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		AllManage.UICLStatic.ExitFood();
	}
	
	function ExitBox(){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		AllManage.UICLStatic.ExitBox();
	}
	
	function ExitWaK(){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		AllManage.UICLStatic.ExitWaK();
	}
	
	
	function DaDuan(){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
			AllManage.UICLStatic.BeiDaDuan();
		}
	}
	
	function MapNext(){
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		{
			AllManage.dungclStatic.DungeonNextLoad();
		}
	}
}