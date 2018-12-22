using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeskilldropData : PropertyReader {
	/**编号**/
	public int id{get;set;}
	/**技能掉落类型**/
	public int droptypeID{get;set;}
	/**物品类型**/
	public int type{get;set;}
	/**技能ID**/
	public string skillID{get;set;}
	/**几率**/
	public int probability{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	private static List<MazeskilldropData> Msdd = new List<MazeskilldropData>();
	
	public void addData() 
	{
		data.Add(id, this);
	}
	
	public void parse(string[] ss)
	{
		
	}
	
	public void resetData() {
		data.Clear();
	}
	
	public static List<MazeskilldropData> GetAllAwardSkillId(int droptypeID)
	{
		Msdd.Clear();
		foreach(MazeskilldropData msdd in data.Values)
		{
			if(msdd.droptypeID == droptypeID)
			{
				Msdd.Add(msdd);
			}
		}
		return Msdd;
	}
}
