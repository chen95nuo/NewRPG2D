using UnityEngine;
using System.Collections;

public class EvolutionData : PropertyReader {
	
	//==编号==//
	public int id{get;set;}
	//==星级==//
	public int stars{get;set;}
	//==突破次数==//
	public int evo{get;set;}
	//==需要卡数==//
	public int cards{get;set;}
	//==上限提升==//
	public int lvl{get;set;}
	//==属性提升==//
	public int status{get;set;}
	//==每张卡消耗金币==//
	public int moneypercard{get;set;}

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(stars+"-"+evo,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static EvolutionData getData(int star,int evo)
	{
		return (EvolutionData)data[star+"-"+evo];
	}
}
