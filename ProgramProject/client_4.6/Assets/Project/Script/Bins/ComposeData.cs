using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComposeData : PropertyReader {

	public int id;
	/**合成类型**/
	public int type;
	/**合成物品**/
	public int composite;
	/**解锁类型**/
	public int style;
	/**解锁条件**/
	public int unlockmission;
	/**金币消耗**/
	public int cost;
	/**成功几率**/
	public int probability;
	/**材料-数量**/
	public List<string>  material_num;
	
	private static Hashtable data=new Hashtable();

	public void addData()
	{
		data.Add(id, this);
	}
	
	public static ComposeData getData(int formID,int type)
	{
		foreach(DictionaryEntry de in data)
		{
			if(((ComposeData)de.Value).composite==formID && ((ComposeData)de.Value).type == type)
			{
				return (ComposeData)de.Value;
			}
		}
		return null;
	}

	public void parse(string[] ss)
	{
		int location =0;
		id=StringUtil.getInt(ss[location]);
		type =StringUtil.getInt(ss[location+1]);
		composite =StringUtil.getInt(ss[location+2]);
		style = StringUtil.getInt(ss[location+3]);
		unlockmission =StringUtil.getInt(ss[location+4]);
		cost = StringUtil.getInt(ss[location+5]);
		probability =StringUtil.getInt(ss[location+6]);
		material_num =new List<string>();
		for(int i=0;i<4;i++)
		{
			location=7+i*2;
			int material =StringUtil.getInt(ss[location]);
			if(material==0)
			{
				continue;
			}
			int number =StringUtil.getInt(ss[location+1]);
			string material_numStr =material+"-"+number;
			material_num.Add(material_numStr);
		}
		addData();
	}

	public void resetData()
	{
		data.Clear();
	}

}
