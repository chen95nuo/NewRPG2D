using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeprobabilityData : PropertyReader {
	/**编号**/
	public int id;
	/**迷宫编号**/
	public int mazeId;
	/**战斗类型**/
	public int battlestyle;
	/**首杀奖励**/
	public int awardfirst;
	/**奖励物品及概率 格式：奖励列表编号-概率-是否展示**/
	public List<string>  dropId_pro_isLook;
	
	private static List<MazeprobabilityData> Mpbd = new List<MazeprobabilityData>();
	
	private static Hashtable data=new Hashtable();
	
	public void addData() 
	{
		data.Add(id, this);
	}
	
	public void parse(string[] ss)
	{
		int location =0;
		id = StringUtil.getInt(ss[location]);
		mazeId = StringUtil.getInt(ss[location+1]);
		battlestyle = StringUtil.getInt(ss[location+2]);
		awardfirst = StringUtil.getInt(ss[location+3]);
		
		dropId_pro_isLook = new List<string>();
		for(int i = 0;i<21;i++)
		{
			location = 4+i*3;
			int dropId = StringUtil.getInt(ss[location]);
			int pro = StringUtil.getInt(ss[location+1]);
			int isLook = StringUtil.getInt(ss[location+2]);
			if(dropId ==0 && pro==0)
			{
				continue;
			}
			string dropIdpro = dropId+"-"+pro+"-"+isLook;
			dropId_pro_isLook.Add(dropIdpro);
		}
		addData();
	}
	
	public void resetData() {
		data.Clear();
	}
	
	public static List<MazeprobabilityData> getData(int mazeId){
		Mpbd.Clear();
		foreach(MazeprobabilityData mpbd in data.Values)
		{
			//MazeprobabilityData mpbd = de.Value as MazeprobabilityData;
			if(mpbd.mazeId == mazeId)
			{
				Mpbd.Add(mpbd);
			}
		}
		return Mpbd;
	}
}

