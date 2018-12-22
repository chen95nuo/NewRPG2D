using UnityEngine;
using System.Collections;

public class BattleData : PropertyReader{
	
	public int ID{get;set;}
	public string cardmodel{get;set;}
	public int cardlevel{get;set;}
	public int position{get;set;}
	public int skillID{get;set;}
	public int passiveskillID{get;set;}
	public int equip1ID{get;set;}
	public int equiplevel1{get;set;}
	public int equip2ID{get;set;}
	public int equiplevel2{get;set;}
	public int equip3ID{get;set;}
	public int equiplevel3{get;set;}
	
	private static ArrayList data=new ArrayList();
	
	public void addData()
	{
		data.Add(this);
	}
	
	public void resetData()
	{
		data.Clear();
	}
	
	public void parse(string[] ss)
	{}
	
	public static BattleData getData(int team,int sequence)
	{
		int k=0;
		for(int i=0;i<data.Count;i++)
		{
			BattleData bd=(BattleData)data[i];
			if(bd.position==team)
			{
				k++;
				if(k==sequence)
				{
					return bd;
				}
			}
		}
		return null;
	}
	
	/// <summary>
	/// Gets the team data.
	/// </summary>
	/// <returns>
	/// The team data.
	/// </returns>
	/// <param name='team'>
	/// 1 or 2
	/// </param>
	public static ArrayList getTeamData(int team)
	{
		ArrayList result=new ArrayList();
		int pos=1;
		for(int i=0;i<data.Count;i++)
		{
			BattleData bd=(BattleData)data[i];
			if(bd.position==team)
			{
				string s=pos+","+bd.ID+","+bd.cardlevel+","+bd.skillID+","+bd.passiveskillID+",";
				if(bd.equip1ID>0)
				{
					s+=bd.equip1ID+"-"+bd.equiplevel1;
				}
				if(bd.equip2ID>0)
				{
					s+="="+bd.equip2ID+"-"+bd.equiplevel2;
				}
				if(bd.equip3ID>0)
				{
					s+="="+bd.equip3ID+"-"+bd.equiplevel3;
				}
				
				result.Add(s);
				pos++;
			}
		}
		return result;
	}
	
}
