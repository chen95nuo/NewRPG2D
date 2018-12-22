using UnityEngine;
using System.Collections;

public class FriendenergyData : PropertyReader {
	
	//==合体技id==//
	public int id{get;set;}
	//==第x回合可以放好友合体技==//
	public int number{get;set;}

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static int getNumber(int id)
	{
		FriendenergyData fd=(FriendenergyData)data[id];
		if(fd!=null)
		{
			return fd.number;
		}
		return 0;
	}
}
