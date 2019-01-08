#pragma strict

var isTaskObject : boolean = false;
var isTaskID : String;
var isTaskName : String;
var isTaskSeconds : int;
var viewID : int;
var flagID : String = "";
//var photonView : PhotonView;

//var isDontClose : boolean = false;
function	Start()
{
	photonView	=	GetComponent(PhotonView);
	if(	photonView	)
	{
		viewID	=	photonView.viewID;
	}
	if(	Application.loadedLevelName != "Map421")
	{
		if(	!isTaskObject	)
		{
			while(	AllManage.dungclStatic == null || !AllManage.dungclStatic.alreadySetYu	)
			{
				//print("AllManage.dungclStatic wei kong == " + Time.time);
				yield;			
			}
			if(	myType == ConsumablesType.Fish	)
			{
				if(	DungeonControl.ItemYu == ""	)
				{
					gameObject.SetActiveRecursively(false);
				}
				else
				{
					myName	= DungeonControl.ItemYuName;
					myID	= DungeonControl.ItemYu + ",01";
					myLevel = parseInt(DungeonControl.ItemYuLevel);
				}
				if(	Application.loadedLevelName == "Map111"	)
				{
					myLevel = 1;
				}
			}
			else
			if(	myType == ConsumablesType.Stone	)
			{
				//print(DungeonControl.ItemStone + " == "+ this.name);
				if(	DungeonControl.ItemStone == ""	)
				{
					gameObject.SetActiveRecursively(false);
				}
				else
				{
					var mm : int;
					mm = Random.Range(0,100);
		//			//print("mm == " + mm);
					if(	mm < 50	)
					{
						myName	=	DungeonControl.ItemStoneName;
						myID	=	DungeonControl.ItemStone + ",01";
						myLevel	=	parseInt(	DungeonControl.ItemStoneLevel	);
					}
					else
					{
						myName	=	DungeonControl.ItemStoneName1;
						myID	=	DungeonControl.ItemStone1	+	",01";
						myLevel	=	parseInt(	DungeonControl.ItemStoneLevel1	);				
					}
					mm	=	Random.Range(0,100);
					if(	mm	>	98	)
					{
						myID	=	DungeonControl.ItemStone1.Substring(0,2) + "1" + Random.Range(1,3)+ ",01";
					}
				}
			}
		}
		else	//大炮//
		{
			var	rows	:	yuan.YuanMemoryDB.YuanRow;
			rows	=	GetNMRowAsID(	isTaskID	);
			if(	rows	)
			{
				isTaskName		=	rows["Name"].YuanColumnText;
				isTaskSeconds	=	parseInt( rows["Sconds"].YuanColumnText);
			}
		}
	}
}

function	GetNMRowAsID(	id	:	String	) : yuan.YuanMemoryDB.YuanRow
{
	for(var rows : yuan.YuanMemoryDB.YuanRow in InventoryControl.TaskItem.Rows){
		if(rows["ItemID"].YuanColumnText == id){
			return rows;
		}
	}
	return null;	
}

var myID : String;
var myName : String;
var myLevel : int;
var functionStr : String;
var myType : ConsumablesType;
function	OnTriggerStay(other : Collider)
{
//	print(other.gameObject.tag + " == " + AllManage.WakCLStatic.busy);
	if(	other.CompareTag ("Player") && !AllManage.WakCLStatic.busy	)
	{
//		//print(Time.time);
		other.SendMessage(functionStr,this,SendMessageOptions.DontRequireReceiver);
	}
}

var functionStr2 : String;
function	OnTriggerExit(other : Collider)
{
//	//print(other.tag);
	if(	other.CompareTag ("Player"	)	)
	{
		other.SendMessage(functionStr2,this,SendMessageOptions.DontRequireReceiver);
	}
}

var aniObject : GameObject;
function PlayerAni(Str:String){
try{
	aniObject.animation.Play(Str);
}catch(e){
	//print(aniObject);
}
}

var myFlagID : String = "No";
private var photonView : PhotonView;
function SetFlagAsID(str : String){
	////print(str + " == " + gameObject);
//	myFlagID = str;
	RPCSetFlagAsID(str);
//	photonView = GetComponent(PhotonView); 
//	photonView.RPC("RPCSetFlagAsID",PhotonTargets.AllBuffered,myFlagID);
}

//var LableTitle : UILabel;
var flags : GameObject[];
//@RPC
function	RPCSetFlagAsID(	str : String	)
{
	if(	PhotonNetwork.connected	)
	{
    	AllManage.dungclStatic.staticRoomSP["flags" + viewID] = str;
    	AllManage.dungclStatic.SetMonsterStaticRoom();		
	}
	if(myFlagID != str){
		AllResources.EffectGamepoolStatic.SpawnEffect(108,transform);
	}
		myFlagID = str;
		switch(myFlagID){
			case "No" : 
	//		LableTitle.text = "无归属";
			flags[0].active = false;
			flags[1].active = false;
			flags[2].active = true;
			 break;
			case "Red" : 
	//		LableTitle.text = "当前归属红队";
			flags[0].active = true;
			flags[1].active = false;
			flags[2].active = false;
			 break;
			case "Blue" : 
	//		LableTitle.text = "当前归属蓝队";
			flags[0].active = false;
			flags[1].active = true;
			flags[2].active = false;
			 break;
		}
}

function DoSomething(time : int){
if(animation&&animation["idle"])
animation.Play("idle");
}

function Reset(){
if(animation&&animation["idle0"])
animation.Play("idle0");
}