using UnityEngine;
using System.Collections;

public class CardExpData : PropertyReader {

	public int level{get;set;}
	public int starexp1{get;set;}
	public int starexp2{get;set;}
	public int starexp3{get;set;}
	public int starexp4{get;set;}
	public int starexp5{get;set;}
	public int starexp6{get;set;}
	public int cost{get;set;}
	
	public int[] starexps;
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		starexps=new int[6];
		starexps[0]=starexp1;
		starexps[1]=starexp2;
		starexps[2]=starexp3;
		starexps[3]=starexp4;
		starexps[4]=starexp5;
		starexps[5]=starexp6;
		
		data.Add(level,this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss){}
	
	public static int getExp(int level,int starLevel)
	{
		CardExpData ced=(CardExpData)data[level];
		if(ced==null || starLevel<0 || starLevel>6)
		{
			return 0;
		}
		return ced.starexps[starLevel-1];
	}
	
	public static int getCost(int level)
	{
		CardExpData ced = (CardExpData)data[level];
		if(ced == null)
			return 0;
		return ced.cost;
	}
}
