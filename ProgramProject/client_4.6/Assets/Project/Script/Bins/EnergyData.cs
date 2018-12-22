using UnityEngine;
using System.Collections;

public class EnergyData : PropertyReader {
	
	public int type{get;set;}
	public string typeDes{get;set;}
	public float energySingle{get;set;}
	public float energySwing{get;set;}
	public float energyLine{get;set;}
	public float energyAll{get;set;}
	
	public float[] energys;
	
	/// <summary>
	/// 单体
	/// </summary>
	public const int ScopeSingle=0;
	/// <summary>
	/// 竖排
	/// </summary>
	public const int ScopeSwing=1;
	/// <summary>
	/// 横排
	/// </summary>
	public const int ScopeLine=2;
	/// <summary>
	/// 全体
	/// </summary>
	public const int ScopeAll=3;
	

	private static ArrayList data=new ArrayList();
	
	public void addData()
	{
		energys=new float[4];
		energys[0]=energySingle;
		energys[1]=energySwing;
		energys[2]=energyLine;
		energys[3]=energyAll;
		data.Add(this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{
		
	}
	
	/// <summary>
	/// 获取士气值
	/// </summary>
	/// <returns>
	/// The energy.
	/// </returns>
	/// <param name='type'>
	/// 类型
	/// </param>
	/// <param name='scope'>
	/// 作用范围
	/// </param>
	public static float getEnergy(int type,int scope)
	{
		foreach(EnergyData ed in data)
		{
			if(ed.type==type)
			{
				return ed.energys[scope];
			}
		}
		return 0;
	}
	
}
