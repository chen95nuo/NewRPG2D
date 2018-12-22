using UnityEngine;
using System.Collections;

public class BagCostData : PropertyReader {

	/**购买次数**/
	public int number{get;set;}
	/**扩充格数**/
	public int number1{get;set;}
	/**花费类型:1钻石,2金币**/
	public int type{get;set;}
	/**花费**/
	public int cost{get;set;}

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(number, this);
	}

	public void parse(string[] ss)
	{

	}

	public void resetData()
	{

	}

	public static BagCostData getData(int number)
	{
		return (BagCostData)data[number];
	}
	
}
