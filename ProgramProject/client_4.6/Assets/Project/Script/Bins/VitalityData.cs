using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VitalityData : PropertyReader {

	public int vitality{get;set;}
	public string icon{get;set;}
	public int goodstype1{get;set;}
	public string itemId1{get;set;}
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
	public int goodstype7{get;set;}
	public string itemId7{get;set;}
	
	
	public static Hashtable data=new Hashtable();
	public static List<VitalityData> vitalityDataList = new List<VitalityData>();
	List<string> vitalityRewardList = new List<string>();
	
	public void addData()
    {
        data.Add(vitality, this);
		vitalityDataList.Add(this);
		if(goodstype1!= 0 && itemId1!= "")
		{
			vitalityRewardList.Add(goodstype1+"-"+itemId1);
		}
		if(goodstype2!= 0 && itemId2!= "")
		{
			vitalityRewardList.Add(goodstype2+"-"+itemId2);
		}
		if(goodstype3!= 0 && itemId3!= "")
		{
			vitalityRewardList.Add(goodstype3+"-"+itemId3);
		}
		if(goodstype4!= 0 && itemId4!= "")
		{
			vitalityRewardList.Add(goodstype4+"-"+itemId4);
		}
		if(goodstype5!= 0 && itemId5!= "")
		{
			vitalityRewardList.Add(goodstype5+"-"+itemId5);
		}
		if(goodstype6!= 0 && itemId6!= "")
		{
			vitalityRewardList.Add(goodstype6+"-"+itemId6);
		}
		if(goodstype7!= 0 && itemId7!= "")
		{
			vitalityRewardList.Add(goodstype7+"-"+itemId7);
		}
    }
	
	public void resetData()
    {
        data.Clear();
    }
	
	public void parse(string[] ss)
    {
		
	}
	
	public static VitalityData getData(int vitality)
	{
		return (VitalityData)data[vitality];
	}
	
	//得到宝箱奖励字典//
	public static List<string> getHYDRewardList(int id)
	{
		VitalityData vData = (VitalityData)data[id];
		return vData.vitalityRewardList;
	}
}
