using UnityEngine;
using System.Collections;

public class TalentData :PropertyReader {
	
	public int index{get;set;}
	public string name{get;set;}
	/**1对技能类型加成,2对元素,3对士气槽,4对特定技能,5对本方单位,6对敌方卡牌,7特殊**/
	public int type{get;set;}
	/**对于每个type都有不同的意义**/
	public int class1{get;set;}
	public int effect{get;set;}
	public float number{get;set;}
	public int action{get;set;}
	public string icon{get;set;}
	public string description{get;set;}

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(index,this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{}
	
	public static TalentData getData(int talentId)
	{
		return (TalentData)data[talentId];
	}
		
}
