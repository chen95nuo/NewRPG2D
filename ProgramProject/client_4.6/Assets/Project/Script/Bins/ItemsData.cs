using UnityEngine;
using System.Collections;

public class ItemsData : PropertyReader {

	public int id{get;set;}
	public int number{get;set;}
	public int type{get;set;}
	public string name{get;set;}
	public int star{get;set;}
	public string discription{get;set;}
	public int pile{get;set;}
	public int sell{get;set;}
	public string icon{get;set;}
	public int sound{get;set;}
			
	public int fragment{get;set;}
	public int goodztype{get;set;}
	public int goodsid{get;set;}
	public int use{get;set;}
	public int usetimes{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss){}
	
	public static ItemsData getData(int id)
	{
		return (ItemsData)data[id];
	}
	
}

