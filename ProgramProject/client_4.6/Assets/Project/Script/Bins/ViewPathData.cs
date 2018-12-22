using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// View path data.
/// </summary>
public class ViewPathData : PropertyReader {

	public string pathName{get;set;}
	public int size{get;set;}
	public List<Vector3> nodes;
	
	private static ArrayList data=new ArrayList();
	
	public void addData()
	{
		data.Add(this);
		//Debug.Log("pathName:"+pathName+",size:"+size);
		//Debug.Log("ViewPathData:"+data.Count);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{
		pathName=StringUtil.getString(ss[0]);
		size=StringUtil.getInt(ss[1]);
		nodes=new List<Vector3>();
		for(int i=0;i<size;i++)
		{
			int pos=i*3+2;
			Vector3 v3=new Vector3(StringUtil.getFloat(ss[pos]),StringUtil.getFloat(ss[pos+1]),StringUtil.getFloat(ss[pos+2]));
			nodes.Add(v3);
		}
		addData();
	}
	
	public static ViewPathData getData(string pathName)
	{
		foreach(ViewPathData vp in data)
		{
			if(vp.pathName.Equals(pathName))
			{
				return vp;
			}
		}
		return null;
	}
	
}
