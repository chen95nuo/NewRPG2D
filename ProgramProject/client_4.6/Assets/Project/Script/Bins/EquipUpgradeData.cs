using UnityEngine;
using System.Collections;

public class EquipUpgradeData : PropertyReader {

	public int level{get;set;}	
	public int cost{get;set;}
	public int probability1{get;set;}
	public int probability3{get;set;}
	public int sell{get;set;}

	private static Hashtable data=new Hashtable();
		
	public void addData()
	{
		data.Add(level,this);
	}
	
	public void resetData(){data.Clear();}
	
	public void parse(string[] ss){}
	
	public static EquipUpgradeData getData(int level)
	{
		return (EquipUpgradeData)data[level];
	}
}
