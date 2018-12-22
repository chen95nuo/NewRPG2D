using UnityEngine;
using System.Collections;

public class ImaginationResultJson : ErrorJson {
	/**
	 * 服务器->客户端
	 * 进入冥想界面返回数据
	 */

	//背包的空余格数//
	public int i;
	//当前激活的npc的id, 默认为1//
	public int id;
	//玩家当前金币//
	public int g;

    //当前玩家激活的领奖id//

    public int mid;

    //当前玩家冥想次数//
    public int mnum;
}
