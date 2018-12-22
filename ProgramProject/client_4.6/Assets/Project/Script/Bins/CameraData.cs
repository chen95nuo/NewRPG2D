using UnityEngine;
using System.Collections;

public class CameraData : PropertyReader
{
	public int id{get;set;}
	public string path{get;set;}
	public float pathTime{get;set;}
	public string lookatPrefab{get;set;}
	public float lookatX{get;set;}
	public float lookatY{get;set;}
	public float lookatZ{get;set;}
	public Vector3 lookAt;
	public float rotationX{get;set;}
	public float rotationY{get;set;}
	public float rotationZ{get;set;}
	public Vector3 rotation;
	
	private static ArrayList data=new ArrayList();
	
	public void addData()
	{
		if("-1".Equals(lookatPrefab.Trim()))
		{
			if(lookatX==-1)
			{
				rotation=new Vector3(rotationX,rotationY,rotationZ);
			}
			else
			{
				lookAt=new Vector3(lookatX,lookatY,lookatZ);
			}
		}
		data.Add(this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{
	}
	
	public static CameraData getData(int id)
	{
		foreach(CameraData cd in data)
		{
			if(cd.id==id)
			{
				return cd;
			}
		}
		return null;
	}
	
	public bool haveLookAtPrefab()
	{
		return !("-1".Equals(lookatPrefab.Trim()));
	}
	
	public bool haveLookAtPos()
	{
		return !(lookatX==-1);
	}
	
}
