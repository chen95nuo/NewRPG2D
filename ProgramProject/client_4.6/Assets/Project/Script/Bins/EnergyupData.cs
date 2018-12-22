using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnergyupData : PropertyReader {
	
	//==购买次数==//
	public int number{get;set;}
	//==怒气数量==//
	public int energy{get;set;}
	//==需求vip等级==//
	public int viplevel{get;set;}
	//==花费水晶==//
	public int cost{get;set;}
	
	private static Hashtable data=new Hashtable();
	private static List<EnergyupData> listData=new List<EnergyupData>();
	
	public void addData()
	{
		data.Add(number,this);
		listData.Add(this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static EnergyupData getData(int number)
	{
		return (EnergyupData)data[number];
	}
	
	//获取购买次数//
	public static int getNumber(int maxEnergy)
	{
		int temp=maxEnergy-Constant.InitMaxEnergy;
		for(int i=0;i<listData.Count;i++)
		{
			EnergyupData ed=listData[i];
			if(temp<ed.energy)
			{
				return ed.number-1;
			}
			else
			{
				temp-=ed.energy;
			}
		}
		return listData[listData.Count-1].number;
	}
}
