using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VipGiftData : PropertyReader {

	public int giftid{get;set;}				//礼包id//
	public string icon{get;set;}				//礼包icon//
	public int goodstype1{get;set;}		//物品类型1//
	public string itemId1{get;set;}		//物品id1//
	public int goodstype2{get;set;}
	public string itemId2{get;set;}
	public int goodstype3{get;set;}
	public string itemId3{get;set;}
	public int goodstype4{get;set;}
	public string itemId4{get;set;}
	public int goodstype5{get;set;}
	public string itemId5{get;set;}
	public int goodstype6{get;set;}
	public string itemId6{get;set;}
	
	private static Hashtable data=new Hashtable();
	List<string> itemList = new List<string>();
	
	public void addData()
	{
		data.Add(giftid,this);
		if(goodstype1!= 0 && itemId1!= "")
		{
			itemList.Add(goodstype1+"-"+itemId1);
		}
		if(goodstype2 !=0 && itemId2!= "")
		{
			itemList.Add(goodstype2+"-"+itemId2);
		}
		if(goodstype3 !=0 && itemId3!= "")
		{
			itemList.Add(goodstype3+"-"+itemId3);
		}
		if(goodstype4 !=0 && itemId4!= "")
		{
			itemList.Add(goodstype4+"-"+itemId4);
		}
		if(goodstype5 !=0 && itemId5!= "")
		{
			itemList.Add(goodstype5+"-"+itemId5);
		}
		if(goodstype6 !=0 && itemId6!= "")
		{
			itemList.Add(goodstype6+"-"+itemId6);
		}
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static VipGiftData getData(int gId)
	{
		return (VipGiftData)data[gId];
	}
	
	//得到礼包项目列表//
	public static List<string> getItemList(int gId)
	{
		VipGiftData tData = (VipGiftData)data[gId];
		return tData.itemList;
	}
}
