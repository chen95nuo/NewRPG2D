using UnityEngine;
using System.Collections;

public class ImaginationClickResultJson : ErrorJson {

	/**
	 * 服务器->客户端
	 * 点击冥想按钮返回数据
	 */
	//当前激活的npc在表中的id//
	public int id;
	//获得的物品：物品类型-物品id//
	public string s;
	//玩家当前金币//
	public int g;


    //当前玩家激活的领奖id//

    public int mid;

    //当前玩家冥想次数//
    public int mnum;
}
