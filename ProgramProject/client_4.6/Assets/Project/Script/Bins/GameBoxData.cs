using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoxData : PropertyReader {

	public int id{get;set;}
	public int keyid{get;set;}						//钥匙id//
	public int boxstar{get;set;}					//宝箱id//
	public int droptpye{get;set;}					//掉落类型//
	public int goodstpye{get;set;}					//物品的类型 1 item， 2 equip 3 card客户端只显示这三个//
	public int itemid{get;set;}						//物品id//
	public int number{get;set;}						//物品数量//
	public int cost{get;set;}						//需求金额//
	public int probability{get;set;}				//几率//
	public int showup{get;set;}						//是否展示 0 不显示， 1 显示//
	public int totaltime{get;set;}					//全局时限//
	public int totalnumber{get;set;}				//全局数量//
	
	
//	private static Hashtable data=new Hashtable();
	private static List<GameBoxData> data = new List<GameBoxData>();
	
	public void addData()
	{
		data.Add(this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss){}
	
	public static GameBoxData getData(int id)
	{
		return data[id];
	}
	
	//formId 为宝箱的formId//
	public static List<GameBoxData> getListData(int formId)
	{
		List<GameBoxData> temp = new List<GameBoxData>();
		foreach(GameBoxData gbd in data)
		{
			if(gbd.boxstar == formId && gbd.showup == 1)
			{
				temp.Add(gbd);
			}
		}
		
		//对temp进行排序//
		List<GameBoxData> cardList = new List<GameBoxData>();
		List<GameBoxData> equipList = new List<GameBoxData>();
		List<GameBoxData> itemList = new List<GameBoxData>();
		for(int i = 0;i < temp.Count ; i++)
		{
			GameBoxData gbd = temp[i];
			if(gbd.goodstpye == 3)		//card//
			{
				cardList.Add(gbd);
			}
			else if(gbd.goodstpye == 2)	//equip//
			{
				equipList.Add(gbd);
			}
			else if(gbd.goodstpye == 1)		//item//
			{
				itemList.Add(gbd);
			}
		}
		//此时清空temp列表//
		temp.Clear();
		List<GameBoxData> temp2 = new List<GameBoxData>();
		
		//对Card列表进行排序//
		temp2 = getGridCard(cardList);
		//将排序后的list放到temp中, 顺序是card-equip-item//
		for(int i = 0 ;i < temp2.Count; i ++)
		{
			GameBoxData gbd = temp2[i];
			temp.Add(gbd);
		}
		cardList.Clear();
		temp2.Clear();
		
		//对equip列表进行排序//
		temp2 = getGridEquip(equipList);
		for(int i = 0 ;i < temp2.Count; i ++)
		{
			GameBoxData gbd = temp2[i];
			temp.Add(gbd);
		}
		equipList.Clear();
		temp2.Clear();
		
		//对item列表进行排序//
		temp2 = getGridItem(itemList);
		for(int i = 0 ;i < temp2.Count; i ++)
		{
			GameBoxData gbd = temp2[i];
			temp.Add(gbd);
		}
		itemList.Clear();
		temp2.Clear();
		
		//此时temp为排序后的temp , 顺序是 card-equip-item, 每个选项里面的顺序是星级由高到低//
		return temp;
	}
	
	//对card类型进行排序，顺序是star由高到低//
	public static  List<GameBoxData> getGridCard(List<GameBoxData> cardList)
	{
		List<GameBoxData> temp2 = new List<GameBoxData>();
		

		for(int i = 0;i < cardList.Count;i++)
		{
			GameBoxData gbd = cardList[i];
			CardData cd = CardData.getData(gbd.itemid);
			if(!temp2.Contains(gbd))
			{
				temp2.Add(gbd);
			}
			else 
			{
				for(int k = 0 ;k < temp2.Count;k ++)
				{
					GameBoxData tempGbd = temp2[k];
					CardData tempCd = CardData.getData(tempGbd.itemid);
					if(cd.star > tempCd.star)
					{
						 temp2.Insert(k, gbd);
                          break;
					}
				}
			}
		}
		return temp2;
	}
	
	//对equip类型进行排序，顺序同上//
	public static List<GameBoxData> getGridEquip(List<GameBoxData> equipList)
	{
		List<GameBoxData> temp2 = new List<GameBoxData>();
		

		for(int i = 0;i < equipList.Count;i++)
		{
			GameBoxData gbd = equipList[i];
			EquipData ed = EquipData.getData(gbd.itemid);
			if(!temp2.Contains(gbd))
			{
				temp2.Add(gbd);
			}
			else 
			{
				for(int k = 0 ;k < temp2.Count;k ++)
				{
					GameBoxData tempGbd = temp2[k];
					EquipData tempEd = EquipData.getData(tempGbd.itemid);
					if(ed.star > tempEd.star)
					{
						 temp2.Insert(k, gbd);
                          break;
					}
				}
			}
		}
		return temp2;
	}
	
	
	//对item类型进行排序，顺序同上//
	public static List<GameBoxData> getGridItem(List<GameBoxData> itemList)
	{
		List<GameBoxData> temp2 = new List<GameBoxData>();
		

		for(int i = 0;i < itemList.Count;i++)
		{
			GameBoxData gbd = itemList[i];
			ItemsData item = ItemsData.getData(gbd.itemid);
			if(!temp2.Contains(gbd))
			{
				temp2.Add(gbd);
			}
			else 
			{
				for(int k = 0 ;k < temp2.Count;k ++)
				{
					GameBoxData tempGbd = temp2[k];
					ItemsData tempItem = ItemsData.getData(tempGbd.itemid);
					if(item.star > tempItem.star)
					{
						 temp2.Insert(k, gbd);
                          break;
					}
				}
			}
		}
		return temp2;
	}
	
	
}
