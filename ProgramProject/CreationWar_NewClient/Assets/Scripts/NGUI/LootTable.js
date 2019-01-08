
private var ps : PlayerStatus;
function Awake(){
	AllResources.staticLT = this;
//    var Selid = PlayerPrefs.GetInt("SelectP");
//	var t=Selid+"Chalv";
//	cslevel=PlayerPrefs.GetInt(t,1)-2;
//	if(cslevel<1)
//	cslevel=1;
}

private var useStr : String;
private var usePlayerType : int;
private var useItemType : int;
private var useQuality : int;
private var useEndurance : int;
private var useProAbt : int;
private var useAbt1 : String;
private var useAbt2 : String;
private var useAbt3 : String;
private var useBuild : String;
private var useHole : int;
var useInt : int[];
private var cslevel =1;
private var levelstring: String; 
private var uuuseInt : int;
private var mmmInt : int;
function MakeItemID1(iid : String , pinzhi : int){ 
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
//		cslevel = parseInt(ps.Level);
	}
		mmmInt = Random.Range(0,100);
//		//print(mmmInt);
		if(mmmInt < 50){
			uuuseInt = 1;
		}else{
			uuuseInt = 0;
		}
		
		usePlayerType = ps.ProID + 3*uuuseInt;
	cslevel = DungeonControl.level;
		var i : int = 0;
		var elevel = cslevel+Random.Range(0,3);
		if(elevel<10)
		levelstring = "0"+elevel.ToString();
		else
		levelstring = elevel.ToString();
		
//		usePlayerType = Random.Range(1,7); 
		if(usePlayerType <= 3){
			useQuality = pinzhi;
			useItemType = Random.Range(1,3);
			useHole = 0;
		}else{ 
			useQuality = pinzhi;
			useItemType = Random.Range(1,10);
			if(useItemType == 6){
				useItemType = Random.Range(1,6);
			}
			useItemType = GetOtherInt(useItemType , elevel);
			useHole = 0;
		}
		
		useInt = new Array(5);
		useInt = GetRandomInt(useInt);
		useBuild = "00000";
		useProAbt = useInt[0];
		useEndurance = useInt[1];
		useAbt1 = Random.Range(1,10).ToString() + useInt[2].ToString();
		useAbt2 = Random.Range(1,10).ToString() + useInt[3].ToString();
		useAbt3 = Random.Range(1,10).ToString() + useInt[4].ToString();
		iid = usePlayerType.ToString() + useItemType.ToString() + levelstring + useQuality.ToString() + useEndurance.ToString() + useProAbt.ToString() + useAbt1 + useAbt2 + useAbt3 + useBuild.ToString() + useHole.ToString() + "000000";
	return iid;
}

function GetOtherInt(useInt : int , lv : int) : int{
	var i : int = 0;
	i = useInt;
	if((useInt == 3 && lv < 10) || (useInt == 9 && lv < 15) || (useInt == 8 && lv < 20) || (useInt == 6 && lv < 30)){
		var ran : int = 0;
		ran = Random.Range(0,100);
		if(ran < 20){
			i = 1;
		}else
		if(ran < 40){
			i = 2;	
		}else
		if(ran < 60){
			i = 4;	
		}else
		if(ran < 80){
			i = 5;	
		}else
		{
			i = 7;	
		}
	}
	return i;
}

function MakeItemID2(iid : String , pinzhi : int , type : int){ 
			if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
	cslevel = DungeonControl.level;
		var i : int = 0;
		var elevel = cslevel+Random.Range(0,3);
		if(elevel<10)
		levelstring = "0"+elevel.ToString();
		else
		levelstring = elevel.ToString();
		
		usePlayerType = type; 
		if(usePlayerType <= 3){
			useQuality = pinzhi;
			useItemType = Random.Range(1,3);
			useHole = 0;
		}else{ 
			useQuality = pinzhi;
			useItemType = Random.Range(1,10);
			useItemType = GetOtherInt(useItemType , elevel);
			useHole = 0;
		}
		
		useInt = new Array(5);
		useInt = GetRandomInt(useInt);
		useBuild = "00000";
		useProAbt = useInt[0];
		useEndurance = useInt[1];
		useAbt1 = Random.Range(1,10).ToString() + useInt[2].ToString();
		useAbt2 = Random.Range(1,10).ToString() + useInt[3].ToString();
		useAbt3 = Random.Range(1,10).ToString() + useInt[4].ToString();
//		//print(type + " == type");
		if(type == 81 || type == 87){
			if(type == 81){
				iid = type + Random.Range(1,6).ToString() +  Random.Range(1,10).ToString() + ",01";					
			}else{
				var useIIt : int;
				var useSSt : String;
				useIIt = Random.Range(1,13);
				if(useIIt<10){
					useSSt = "0" + useIIt;
				}else{
					useSSt = useIIt + "";
				}
//				//print(type.ToString  + " == " + useSSt);
				iid = type + useSSt + ",01";			
//				//print(iid + " == iid");						
			}
		}else
		if(type == 0){
			usePlayerType = Random.Range(1,7); 
			if(usePlayerType <= 3){
				useQuality = pinzhi;
				useItemType = Random.Range(1,3);
				useHole = 0;
			}else{ 
				useQuality = pinzhi;
				useItemType = Random.Range(1,10);
			useItemType = GetOtherInt(useItemType , elevel);
				useHole = 0;
			}
			iid = usePlayerType.ToString() + useItemType.ToString() + levelstring + "0" + useEndurance.ToString() + useProAbt.ToString() + useAbt1 + useAbt2 + useAbt3 + useBuild.ToString() + useHole.ToString() + "000000";
		}else
		if(type == 6){
			usePlayerType = ps.ProID + 3;
			levelstring = Random.Range(3,6) + "0";
			useQuality = Random.Range(3,5);
			useItemType = 6;
			iid = usePlayerType.ToString() + useItemType.ToString() + levelstring + useQuality.ToString() + useEndurance.ToString() + useProAbt.ToString() + useAbt1 + useAbt2 + useAbt3 + useBuild.ToString() + useHole.ToString() + "000000";
		}else{
			iid = usePlayerType.ToString() + useItemType.ToString() + levelstring + useQuality.ToString() + useEndurance.ToString() + useProAbt.ToString() + useAbt1 + useAbt2 + useAbt3 + useBuild.ToString() + useHole.ToString() + "000000";
		}
		
	return iid;
}
function MakeItemID2(iid : String , pinzhi : int , type1 : int , type2 : int){ 
	cslevel = DungeonControl.level;
		var i : int = 0;
		var elevel = cslevel+Random.Range(0,3);
		if(elevel<10)
		levelstring = "0"+elevel.ToString();
		else
		levelstring = elevel.ToString();
		
		usePlayerType = type1; 
		if(usePlayerType <= 3){
			useQuality = pinzhi;
			useHole = 0;
		}else{ 
			useQuality = pinzhi;
			useHole = 0;
		}
		useItemType = type2;
		
		useInt = new Array(5);
		useInt = GetRandomInt(useInt);
		useBuild = "00000";
		useProAbt = useInt[0];
		useEndurance = useInt[1];
		useAbt1 = Random.Range(1,10).ToString() + useInt[2].ToString();
		useAbt2 = Random.Range(1,10).ToString() + useInt[3].ToString();
		useAbt3 = Random.Range(1,10).ToString() + useInt[4].ToString();
		iid = usePlayerType.ToString() + useItemType.ToString() + levelstring + useQuality.ToString() + useEndurance.ToString() + useProAbt.ToString() + useAbt1 + useAbt2 + useAbt3 + useBuild.ToString() + useHole.ToString() + "000000";
	return iid;
}


function MakeItemIDAsBot(iid : String , elevel : int , usePlayerType : int , useItemType : int){ 
	var i : int = 0;
	
	if(elevel<10)
		levelstring = "0"+elevel.ToString();
	else
		levelstring = elevel.ToString();
	
	useHole = 0;
	useQuality = Random.Range(2,4);		
	useInt = new Array(5);
	useInt = GetRandomInt(useInt);
	useBuild = "00000";
	useProAbt = useInt[0];
	useEndurance = useInt[1];
	useAbt1 = Random.Range(1,10).ToString() + useInt[2].ToString();
	useAbt2 = Random.Range(1,10).ToString() + useInt[3].ToString();
	useAbt3 = Random.Range(1,10).ToString() + useInt[4].ToString();
	iid = usePlayerType.ToString() + useItemType.ToString() + levelstring + useQuality.ToString() + useEndurance.ToString() + useProAbt.ToString() + useAbt1 + useAbt2 + useAbt3 + useBuild.ToString() + useHole.ToString() + "000000";
	return iid;
}

function GetRandomInt(it : int[]) : int[]{
	it[0] = Random.Range(1,6);
	it[1] = Random.Range(1,6-it[0]);
	it[2] = Random.Range(2,6-it[1]);
	it[3] = Random.Range(0,5-it[2]);
	it[4] = 10 - it[0] - it[1] - it[2] - it[3];
	return it;
}

function MakeItemRandom(objs : Object[]){ 
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		cslevel = parseInt(ps.Level);
	}
	if(Random.Range(0,2) == 0){
		var i : int = 0;
		var elevel = Random.Range(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) - 2 , parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) + 2);
		if(elevel<10)
		levelstring = "0"+elevel.ToString();
		else
		levelstring = elevel.ToString();
		
		usePlayerType = Random.Range(1,7);
		if(usePlayerType <= 3){
			useItemType = Random.Range(1,3);
			useHole = 0;
		}else{ 
			useItemType = Random.Range(1,10);
			useItemType = GetOtherInt(useItemType , elevel);
			useHole = 0;
		}
//		useQuality = objs[0];
		var ranQuality : int = 0;
		ranQuality = Random.Range(0,10000);
		if(ranQuality < 1000){
			ranQuality = 2;
		}else
		if(ranQuality < 8000){
			ranQuality = 3;
		}else
		if(ranQuality < 9998){
			ranQuality = 4;
		}else{
			ranQuality = 5;
		}
		useInt = new Array(5);
		useInt = GetRandomInt(useInt);
		useBuild = "00000";
		useProAbt = useInt[0];
		useEndurance = useInt[1];
		useAbt1 = Random.Range(1,10).ToString() + useInt[2].ToString();
		useAbt2 = Random.Range(1,10).ToString() + useInt[3].ToString();
		useAbt3 = Random.Range(1,10).ToString() + useInt[4].ToString();
		useStr = usePlayerType.ToString() + useItemType.ToString() + levelstring + useQuality.ToString() + useEndurance.ToString() + useProAbt.ToString() + useAbt1 + useAbt2 + useAbt3 + useBuild.ToString() + useHole.ToString() + "000000";
	}else{
		var yuanRow : yuan.YuanMemoryDB.YuanRow[];
		var ArrayRow : Array=new Array();
		
		yuanRow = InventoryControl.GameItem.SelectRowsEqual("ItemLevel" , objs[0].ToString());
		for(var row : yuan.YuanMemoryDB.YuanRow in yuanRow){
			if(row["id"].YuanColumnText != "217" 
				&&row["id"].YuanColumnText != "218" 
				&&row["id"].YuanColumnText != "219" 
				&&row["id"].YuanColumnText != "220" 
				&&row["id"].YuanColumnText != "221" 
				&&row["id"].YuanColumnText != "222" 
				&&row["id"].YuanColumnText != "223" 
				&&row["id"].YuanColumnText != "224" 
				&&row["id"].YuanColumnText != "225" 
				&&row["id"].YuanColumnText != "226" 
				&&row["id"].YuanColumnText != "135" 
				&&row["id"].YuanColumnText != "136" 
				&&row["id"].YuanColumnText != "137" 
				&&row["id"].YuanColumnText != "138" 
				&&row["id"].YuanColumnText != "139" 
			){
				if( row["ItemType"].YuanColumnText != "8"){
					ArrayRow.Add(row);			
				}
				else
				{
					if(row["ItemID"].YuanColumnText.Substring(0,3) == "891"
					||row["ItemID"].YuanColumnText.Substring(0,3) == "892"
					||row["ItemID"].YuanColumnText.Substring(0,3) == "893")
					{
						ArrayRow.Add(row);	
					}
				}
			}
		}
		if(ArrayRow){
			var useRow : yuan.YuanMemoryDB.YuanRow;
			var ranInt : int;
			ranInt = Random.Range(0,ArrayRow.Count);
			useRow = ArrayRow[ranInt];
			useStr = useRow["ItemID"].YuanColumnText + ",01," + objs[0];
		}
	}
	var inv : InventoryItem;
	inv = AllResources.InvmakerStatic.GetItemInfo(useStr , inv);
	var spriteInv : SpriteForGamble;
	spriteInv = objs[1];
	spriteInv.lblNum.text = "";
	spriteInv.spriteBenefits.spriteName = inv.atlasStr;
	spriteInv.itemID = useStr;
	spriteInv.MyLevel = objs[0];
}

function ShowItemRandom(objs : Object[]){
	var useStr : String = "";
	var inv : InventoryItem;
	useStr = objs[0];
	
	inv = AllResources.InvmakerStatic.GetItemInfo(useStr , inv);
	var spriteInv : SpriteForGamble;
	spriteInv = objs[1];
	spriteInv.lblNum.text = "";
	spriteInv.spriteBenefits.spriteName = inv.atlasStr;
	spriteInv.itemID = useStr;
	spriteInv.MyLevel = inv.itemQuality;

}
