using UnityEngine;
using System.Collections.Generic;

public class DaylyData : PropertyReader {
	
	//==编号==//
	public int id{get;set;}
	//==奖励类型==//
	public int type{get;set;}
	//==物品ID==//
	public int cardID{get;set;}
	//==数量==//
	public int number{get;set;}
	
	public int viptype{get;set;}
	public int viplevel{get;set;}
	public int number2{get;set;}

	
	private static List<DaylyData> data=new List<DaylyData>();
	
	public void addData()
	{
		data.Add(this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static List<DaylyData> getDatas()
	{
		return data;
	}
	
	public string getRewardInfo(bool isVip,int level)
	{
        int n = 0;
        if (level == 0)
        {
            level = 1;
        }
        if (isVip)
            n = this.number * level;
        else
            n = this.number;
		string result="";
		switch(type)
		{
		case 1://卡牌//
			CardData cd=CardData.getData(cardID);
            result = cd.name + "*" + n + "\n" + cd.description;
			break;
		case 2://装备//
			EquipData ed=EquipData.getData(cardID);
            result = ed.name + "*" + n + "\n" + ed.description;
			break;
		case 3://主动技能//
			SkillData sd=SkillData.getData(cardID);
            result = sd.name + "*" + n + "\n" + sd.description;
			break;
		case 4://被动技能//
			PassiveSkillData psd=PassiveSkillData.getData(cardID);
            result = psd.name + "*" + n + "\n" + psd.describe;
			break;
		case 5://道具//
			ItemsData itemData=ItemsData.getData(cardID);
            result = itemData.name + "*" + n + "\n" + itemData.discription;
			break;
		case 6://符文值//
            result = TextsData.getData(221).chinese + "*" + n + "\n" + TextsData.getData(220).chinese;
			break;
		case 7://钻石//
            result = TextsData.getData(49).chinese + "*" + n + "\n" + TextsData.getData(219).chinese;
			break;
		case 8://金刚心//
			result = TextsData.getData(749).chinese + "*" + n + "\n" + TextsData.getData(750).chinese;
			break;
		case 9://金币//
			result = TextsData.getData(59).chinese + "*" + n + "\n" + TextsData.getData(751).chinese;
			break;
		}
		return result;
	}
	
}
