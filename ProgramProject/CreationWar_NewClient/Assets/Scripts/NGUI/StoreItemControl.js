#pragma strict

var obj : GameObject;
var yt : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("storeitem","id");
var mm : boolean = false;
var storeItem : StoreItem;
var panelStore :PanelStore;
var invParent : Transform;
var invParent1 : Transform;
var invItemArray : StoreItem[];
//var invMaker : Inventorymaker;
var infoBar : GameObject;
//function Awake(){
//	invMaker = AllResources.InvmakerStatic;
//}

//var LabelBloodNum1 : UILabel;
//var LabelBloodName1 : UILabel;
//var LabelBloodNum2 : UILabel;
//var LabelBloodName2 : UILabel;
//var LabelBloodNum3 : UILabel;
//var LabelBloodName3 : UILabel;
//var LabelBloodNum4 : UILabel;
//var LabelBloodName4 : UILabel;
function Start () {
	panelStore=this.GetComponent(PanelStore);
	AllManage.storeItemCL = this;
//	var yt1 :  = InRoom.GetInRoomInstantiate().ytGameItem;
		var yt1 :  yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("","");
		yt1 = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytStoreItem;
		
		yt.Rows = yt1.SelectRowsList("ItemType" , "0");
		yt = yt1;
//	while(!mm){
//		if(!yt.IsUpdate){
//			InRoom.GetInRoomInstantiate().GetYuanTable("select * from StoreItem where ItemType = 0","DarkSword2",yt);  
//		}
//		if(yt.Count > 0){
//			mm = true;
//		}
//		//print(Time.time);
//		yield;
//	}
	ShowEquepmentItem(); 
	ShowEquepmentItem1();
}

var GID : UIGrid;
function ShowEquepmentItem(){
    for(var rows : yuan.YuanMemoryDB.YuanRow in yt.Rows){
	if(rows["ItemType"].YuanColumnText.Trim() == "0")
    {
				var Obj : GameObject = Instantiate(storeItem.gameObject); 
				Obj.transform.parent = invParent; 
				Obj.transform.localScale= Vector3(1,1,1);
				Obj.transform.localPosition = Vector3(0,0,0);
				var useEBI : StoreItem; 
				var inv : InventoryItem;
				var btnEBI = Obj.GetComponent(BtnItem);
				panelStore.listBtnEquipment.Add(btnEBI);
        // btnEBI.ItemID=rows["ItemID"].YuanColumnText + ",01";
                btnEBI.ItemID=rows["ItemID"].YuanColumnText;
				useEBI = Obj.GetComponent(StoreItem);
				useEBI.uid = invParent;

                //useEBI.dazhe = parseInt(rows["isStart"].YuanColumnText);
                var dt : System.DateTime = DateTime.Parse(rows["ItemEndTime"].YuanColumnText.Trim());
                var now : System.DateTime = System.DateTime.Now;
                if ((dt.Hour - now.Hour) <= 0 && (dt.Minute - now.Minute) <= 0 && (dt.Second - now.Second) <= 0)
                {
                    // 结束打折
                    useEBI.dazhe = 0;
                }
                else
                {
                    // 正在打折
                    useEBI.dazhe = 1;
                }
	
				useEBI.zheshu = (rows["ItemDiscount"].YuanColumnText);
//				//print(rows["ItemDiscount"].YuanColumnText + " ===== " + rows["ItemID"].YuanColumnText);
				useEBI.timeEnd = (rows["ItemEndTime"].YuanColumnText);
				useEBI.GetComponent(UIButtonMessage) .target=this.gameObject;
				useEBI.GetComponent(UIButtonMessage) .functionName ="SetEtemInfo";
				useEBI.sicl = this;  
				useEBI.infoBar = infoBar;
									var objParms: Object[]=new Object[2];
			objParms[0]=btnEBI.ItemID;
			objParms[1]=btnEBI.spriteBackground;
			PanelStatic.StaticBtnGameManager.invcl.SendMessage ("SetYanSeAsID",objParms,SendMessageOptions.DontRequireReceiver);
				useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(rows["ItemID"].YuanColumnText + ",01" , inv) , rows["ItemNeedCash"].YuanColumnText , rows["ItemNeedBlood"].YuanColumnText , rows["id"].YuanColumnText);
//				addInvItem(useEBI);				
//				//print("sljdflajdsflasjdfljasldfjasljdflaskjdfljsdfk");
				if(!GID.gameObject.active)
				{
					Obj.SetActiveRecursively (false);
				}
}
	}
	yield;
	GID.repositionNow = true;
	panelStore.SetFirstEquipment();
} 

 var GIDZhe : UIGrid;
function ShowEquepmentItem1(){
	for(var rows : yuan.YuanMemoryDB.YuanRow in yt.Rows){  
		if(rows["ItemType"].YuanColumnText.Trim() == "0")
			{
		if(parseInt(rows["isStart"].YuanColumnText) == 1){
				var Obj : GameObject = Instantiate(storeItem.gameObject); 
				Obj.transform.parent = GIDZhe.transform; 
				Obj.transform.localScale= Vector3(1,1,1);
				Obj.transform.localPosition = Vector3(0,0,0);
				var useEBI : StoreItem; 
				var inv : InventoryItem;
			 var btnEBI = Obj.GetComponent(BtnItem);
			 panelStore.listBtnEquipment.Add(btnEBI);
	    //btnEBI.ItemID=rows["ItemID"].YuanColumnText + ",01";
                btnEBI.ItemID=rows["ItemID"].YuanColumnText;
				useEBI = Obj.GetComponent(StoreItem);
				useEBI.uid = GIDZhe.transform;  
				useEBI.dazhe = parseInt(rows["isStart"].YuanColumnText);
				useEBI.zheshu = (rows["ItemDiscount"].YuanColumnText);
				useEBI.timeEnd = (rows["ItemEndTime"].YuanColumnText);
				useEBI.GetComponent(UIButtonMessage) .target=this.gameObject;
				useEBI.GetComponent(UIButtonMessage) .functionName ="SetFavorableInfo";
				useEBI.sicl = this;  
				useEBI.infoBar = infoBar;
									var objParms: Object[]=new Object[2];
			objParms[0]=btnEBI.ItemID;
			objParms[1]=btnEBI.spriteBackground;
			PanelStatic.StaticBtnGameManager.invcl.SendMessage ("SetYanSeAsID",objParms,SendMessageOptions.DontRequireReceiver);				
				useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(rows["ItemID"].YuanColumnText + ",01", inv) , rows["ItemNeedCash"].YuanColumnText ,rows["ItemNeedBlood"].YuanColumnText  , rows["id"].YuanColumnText);
//				//print("sljdflajdsflasjdfljasldfjasljdflaskjdfljsdfk");
				if(!GIDZhe.gameObject.active)
				{
					Obj.SetActiveRecursively (false);
				}
		}
		}
	}
	yield;
	GIDZhe.repositionNow = true;
	panelStore.SetFirstEquipment();
}

//function addInvItem(ebi : StoreItem){
//	var use : StoreItem[]; 
//	use = invItemArray; 
//	invItemArray = new Array(invItemArray.length + 1);
//	for(var i=0; i<(invItemArray.length - 1); i++){
//		 invItemArray[i] = use[i];
//	} 
//	invItemArray[invItemArray.length - 1] = ebi;
//}
//
//function invClear(){
//	for(var i=0; i<invItemArray.length; i++){
//		if(invItemArray[i]){
//			Destroy(invItemArray[i].gameObject);
//		}
//	}
//	invItemArray = new Array(0);
//}
// 
//function SelectOneItem(inv : InventoryItem){
//	for(var i=0; i<invItemArray.length; i++){
//		if(invItemArray[i] == inv){
//			invItemArray[i].gameObject.SendMessage("isS");
//		}else{
//			invItemArray[i].gameObject.SendMessage("noS");		
//		}
//	}
//} 
