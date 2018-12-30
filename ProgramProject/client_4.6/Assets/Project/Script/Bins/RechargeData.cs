using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RechargeData : PropertyReader{

	//==编号==//
	public int id{get;set;}
	//==类别==//
	public int type{get;set;}
	//==名称==//
	public string name{get;set;}
	//==描述==//
	public string description{get;set;}
	//==获得水晶==//
	public int crystal{get;set;}
	//==icon==//
	public string icon{get;set;}
	//==金额==//
	public int cost{get;set;}
	//==是否首冲双倍==//
	public int double1{get;set;}

    //==金额==//
    public int RealCost {
        get
        {
            switch (cost)
            {
                case 6:
                    return 1;
                case 30:
                    return 4;
                case 98:
                    return 14;
                case 198:
                    return 28;
                case 328:
                    return 47;
                case 648:
                    return 93;
                case 25:
                    return 4;
            }
            return 0;
        }
    }


    public static Hashtable data=new Hashtable();
	
	public static List<RechargeData> dataList = new List<RechargeData>();
	
	public void addData()
	{
		data.Add(id,this);
		dataList.Add(this);
	}
	public void resetData()
	{}
	public void parse(string[] ss)
	{}
	
	public static RechargeData getData(int id)
	{
		return (RechargeData)data[id];
	}
}
