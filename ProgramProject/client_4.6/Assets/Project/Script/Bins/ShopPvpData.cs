using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopPvpData : PropertyReader
{
	///编号id//
    public int id { get; set; }
	//物品类型//
    public int goodstype { get; set; }
	//物品id//
    public int itemId { get; set; }
	//物品名称//
    public string name { get; set; }
	//vip限制//
    public int vipconfine { get; set; }
	//货币类型//
    public int costtype { get; set; }
	//需求金额//
    public int cost { get; set; }
	//刷新个数//
	public int number{ get; set;}
	//需求等级//
	public int level{ get; set;}
	//是否显示//
	public int showup{ get; set;}
	//几率1//
	public int probability1{ get; set;}
	//几率2//
	public int probability2{ get; set;}
	//几率3//
	public int probability3{ get; set;}

    private static Dictionary<int, ShopPvpData> data = new Dictionary<int, ShopPvpData>();

    public void addData()
    {
        data.Add(id, this);
    }

    public void resetData()
    {
        data.Clear();
    }

    public void parse(string[] ss) { }

    public static ShopPvpData getData(int id)
    {
        return data[id];
    }
}
