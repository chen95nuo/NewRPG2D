using UnityEngine;
using System.Collections;

public class BreakData : PropertyReader {
	
	public int cardID;
	public int MAXbreak;
	public int firstatk;
	public int firstdef;
	public int firsthp;
	public int[] atk;
	public int[] def;
	public int[] hp;

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(cardID, this);
	}

	public void resetData()
	{
		data.Clear();
	}

	public void parse(string[] ss)
	{
		cardID=StringUtil.getInt(ss[0]);
		MAXbreak=StringUtil.getInt(ss[1]);
		firstatk=StringUtil.getInt(ss[2]);
		firstdef=StringUtil.getInt(ss[3]);
		firsthp=StringUtil.getInt(ss[4]);
		int length=(ss.Length-5)/3;
		atk=new int[length];
		def=new int[length];
		hp=new int[length];
		for(int i=0;i<length;i++)
		{
			int pos=5+3*i;
			atk[i]=StringUtil.getInt(ss[pos]);
			def[i]=StringUtil.getInt(ss[pos+1]);
			hp[i]=StringUtil.getInt(ss[pos+2]);
		}
		addData();
	}
}
