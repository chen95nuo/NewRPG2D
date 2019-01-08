#pragma strict
class MapContact{
	var thisMapID : String;
	var contactMap : String[];
}

var MapXinShou : String[];// 冬青谷111
var MapJiGuang : String[];// 极光镇121
var MapOuNuo : String[];// 欧诺城131
var MapHeiFeng : String[];
var MapJiYe : String[]; //极夜城151
var MapJiYeChengMen : String[];
var HeroMap : String[];

var HeroMapJiGuang : String[];// 极光镇精英本
var HeroMapOuNuo : String[];// 欧诺城精英本
var HeroMapJiYe : String[];// 极夜城精英本

function Start()
{
    var mapLevel : yuan.YuanMemoryDB.YuanTable = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel;
    var xinShou : yuan.YuanMemoryDB.YuanRow = mapLevel.SelectRowEqual("MapID", "111");
    var jiGuang : yuan.YuanMemoryDB.YuanRow = mapLevel.SelectRowEqual("MapID", "121");
    var ouNuo : yuan.YuanMemoryDB.YuanRow = mapLevel.SelectRowEqual("MapID", "131");
    var jiYe : yuan.YuanMemoryDB.YuanRow = mapLevel.SelectRowEqual("MapID", "151");

	var myList = new List.<String>();
    var tempMapXinShou : String[] = xinShou["normalMap"].YuanColumnText.Trim().Split(";"[0]);
    for(var i:int=0;i<tempMapXinShou.length;i++)
    {
        if(tempMapXinShou[i].Equals(""))
        {
            continue;
        }
        myList.Add(tempMapXinShou[i]);
        //MapXinShou[i] = tempMapXinShou[i];
    }
    MapXinShou = myList.ToArray();
    myList.Clear();
    
    var tempMapJiGuang : String[] = jiGuang["normalMap"].YuanColumnText.Trim().Split(";"[0]);
    for(var j:int=0;j<tempMapJiGuang.length;j++)
    {
        if(tempMapJiGuang[j].Equals(""))
        {
            continue;
        }
        myList.Add(tempMapJiGuang[j]);
    }
    MapJiGuang = myList.ToArray();
    myList.Clear();
    
    var tempMapOuNuo : String[] = ouNuo["normalMap"].YuanColumnText.Trim().Split(";"[0]);
    for(var k:int=0;k<tempMapOuNuo.length;k++)
    {
        if(tempMapOuNuo[k].Equals(""))
        {
            continue;
        }
        myList.Add(tempMapOuNuo[k]);
    }
    MapOuNuo = myList.ToArray();
    myList.Clear();
    
    var tempMapJiYe : String[] = jiYe["normalMap"].YuanColumnText.Trim().Split(";"[0]);
    for(var m:int=0;m<tempMapJiYe.length;m++)
    {
        if(tempMapJiYe[m].Equals(""))
        {
            continue;
        }
        myList.Add(tempMapJiYe[m]);
    }
    MapJiYe = myList.ToArray();
    myList.Clear();

    var tempHeroMapJiGuang : String[] = jiGuang["eliteMap"].YuanColumnText.Trim().Split(";"[0]);
    for(var n:int=0;n<tempHeroMapJiGuang.length;n++)
    {
        if(tempHeroMapJiGuang[n].Equals(""))
        {
            continue;
        }
        myList.Add(tempHeroMapJiGuang[n]);
    }
    HeroMapJiGuang = myList.ToArray();
    myList.Clear();
    
    var tempHeroMapOuNuo : String[] = ouNuo["eliteMap"].YuanColumnText.Trim().Split(";"[0]);
    for(var p:int=0;p<tempHeroMapOuNuo.length;p++)
    {
        if(tempHeroMapOuNuo[p].Equals(""))
        {
            continue;
        }
        myList.Add(tempHeroMapOuNuo[p]);
    }
    HeroMapOuNuo = myList.ToArray();
    myList.Clear();
    
    var tempHeroMapJiYe : String[] = jiYe["eliteMap"].YuanColumnText.Trim().Split(";"[0]);
    for(var q:int=0;q<tempHeroMapJiYe.length;q++)
    {
        if(tempHeroMapJiYe[q].Equals(""))
        {
            continue;
        }
        myList.Add(tempHeroMapJiYe[q]);
    }
    HeroMapJiYe = myList.ToArray();
    myList.Clear();
    
    switch(AllManage.mtwStatic.thisMapID){
    	case "121":
    		HeroMap = HeroMapJiGuang;
    		break;
    	case "131":
    		HeroMap = HeroMapOuNuo;
    		break;
    	case "151":
    		HeroMap = HeroMapJiYe;
    		break;
    }
    
}

function GetMapContact(id : String , mc : MapContact){
//	//print(id);
	switch(id){
		case "11":
			mc.thisMapID = "11";
			mc.contactMap = MapXinShou;
			break;
		case "12":
			mc.thisMapID = "12";
			mc.contactMap = MapJiGuang;
			break;
		case "13":
			mc.thisMapID = "13";
			mc.contactMap = MapOuNuo;
			break;
		case "14":
			mc.thisMapID = "14";
			mc.contactMap = MapHeiFeng;
			break;
		case "15":
			mc.thisMapID = "15";
			mc.contactMap = MapJiYe;
			break;
		case "71":
			mc.thisMapID = "71";
			mc.contactMap = MapJiYeChengMen;
			break;
	}
	return mc;
}

var MapName : String[];
var sevMapName : String[];
function GetMapName(id : int) : String{
	var i : int = 0;
	for(i=0; i<MapName.length; i++){
		if(i == id){
			return AllManage.AllMge.Loc.Get(MapName[i]);
		}
	}
}
function GetMapName1(id : int) : String{
	var i : int = 0;
	for(i=0; i<sevMapName.length; i++){
		if(i == id){
			return AllManage.AllMge.Loc.Get(sevMapName[i]);
		}
	} 
}
