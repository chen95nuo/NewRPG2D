using UnityEngine;
using System.Collections;

public class UIJson :BasicJson
{
	/**
	 * ui编号
	 * 1背包
	 * 2出售
	 * 3主界面
	 * 4卡组
	 * 9背包物品详细信息
	 * 12 compose
	 * 13抽卡
	 * 34 头像区域设置(获取界面信息)
	 * 35 头像修改
	 * 36 军团名称修改
	 * 46背包全选设定
	 * 5 打开替换列表
	 **/
	public int ui;
	/**背包类型:1角色卡,2主动技能和被动技能,3装备,4材料**/
	/**卡组:表示卡组索引0--4**/
	/**ui=7:1角色卡,2主动技能,3被动技能,4装备,5材料**/
	/**ui=12:1装备,2角色卡,3材料**/
	/**ui = 25type表示副本中关卡在表中的id **/
	/**ui = 27type表示要重置的迷宫的id **/
	// 38 card info Equips And Passiveskill tip mark
    /**ui = 45 type=1商城 2黑市 3黑市刷新*/
	/* ui = 5  :1 是卡牌  2 主动技能 3 被动技能 4是装备 */
	public int type;
	
	/**第几组**/
	/**ui=8:吞噬者索引  
	 * ui = 19，20  表示进入迷宫的编号*
	 * ui = 31 : break target card index
	 * ui = 38 : card pos in card group
	 */
	public int i;
	
	/*0显示全部，1显示同种族  
	 * ui=20 表示在迷宫中选择的位置id 0-37, 0表示起始位置，37表示boss位置
	 */
	public int show;
	
	//ui = 35 表示头像icon的名字， ui= 36 军团的名字,ui=46 表示全选里面选择的类别id,用&连接起来//
	public string str;

	public UIJson()
	{
		
	}
	
	public UIJson(int ui)
	{
		this.ui=ui;
	}

    public void UIJsonForGift(int ui, int i)
    {
        this.ui = ui;
        this.i = i;
    }
	/**
	 * 吞噬
	 * ui:ui号
	 * type:吞噬类型,1角色卡,2主动技能,3被动技能,4装备
	 **/
	public UIJson(int ui,int type)
	{
		this.ui=ui;
		this.type=type;
	}
	
	/**
	 * 设置卡组战斗或休息
	 * ui:ui号
	 * cgIndex:要设置的卡组索引
	 * type:0休息,1战斗
	 **/
	public UIJson(int ui,int cgIndex,int type)
	{
		this.ui=ui;
		this.i=cgIndex;
		this.type=type;
	}
	/**
	 * 吞噬
	 * ui:ui号
	 * cgIndex:当前吞噬者id
	 * type:吞噬类型,1角色卡,2主动技能,3被动技能,4装备
	 * showType:0显示全部，1显示同种族
	 **/
	public UIJson(int ui,int cgIndex,int type,int show)
	{
		this.ui=ui;
		this.i=cgIndex;
		this.type=type;
		this.show = show;
	}
	
	/**
	 * compose
	 * ui:ui号
	 * formID : target id in form
	 **/
	public void UIJsonForCompose(int ui,int formID,int type)
	{
		this.ui = ui;
		this.i = formID;
		this.type = type;
	}
	
	/**
	 * break
	 * ui:ui号
	 * formID : target card index
	 **/
	public void UIJsonForBreak(int ui,int index)
	{
		this.ui = ui;
		this.i = index;
	}
	
	public void UIJsonForHeadSet(int ui, string s)
	{
		this.ui = ui;
		this.str = s;
	}
	
	/**
	 * card info
	 * ui : 38
	 * index : card pos in cardgroup
	 * */
	public void UIJsonForCardInfo(int ui, int index)
	{
		this.ui = ui;
		this.i = index;
	}
	
	/**
	 * card info
	 * ui : 46
	 * ids : 背包里面全选设定的id.用&连接起来
	 * */
	public void UIJsonForPackSelectAll(int ui, string ids)
	{
		this.ui = ui;
		this.str = ids;
	}

    /// <summary>
    /// 七天奖励封装json
    /// </summary>
    /// <param name="ui">UI_SEVEN_BUT=48</param>
    /// <param name="type">点击领取所在天数</param>
    /// <param name="activityId">前天奖励的活动id</param>
    public void UIJsonForSevenPrize(int ui, int type, int activityId)
    {
        this.ui = ui;
        this.type = type;
        this.i = activityId;
    }
	/// <summary>
	/// 迷宫boss奖励
	/// </summary
	public void UIJsonForMazeBossReward(int ui,int type,int show)
	{
		this.ui = ui;
		this.type = type;
		this.show = show;
	}
	
	/// <summary>
	///大风车档位请求
	/// </summary>
	/// <param name='ui'>UI_ACT_GEAR_WHEEL</param>
	/// <param name='id'>档位id</param>
	/// <param name='type'>请求类型，1请求界面，2转动转盘抽奖</param>
	public void UIJsonForActWheel(int ui,int id,int type)
	{
		this.ui = ui;
		this.i = id;
		this.type = type;
	}
	
	//大风车奖励请求//
	public void UIJsonForWheelReward(int ui,int id)
	{
		this.ui = ui;
		this.i = id;
	}
}
