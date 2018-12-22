using UnityEngine;
using System.Collections.Generic;

public class MainResultJson : ErrorJson
{
		
	public List<string> cs;			//获取出战卡牌id//
	public List<string> s;     		//模块是否有更新格式 id(模块id-mark(是否有更新0没有，1有, 2 提升))//

    public int onlineT { get; set; }
    public int giftId { get; set; }
	public int cost{get;set;}		//是否充值过？0表示没有,>0表示之前充值过//
    public int type { get; set; }   //聚宝盆是否开启表示，0未开启，1开启//
	
	public List<int> getList()
	{
		List<int> list=new List<int>();
		for(int i=0;i<cs.Count;i++)
		{
			list.Add(StringUtil.getInt(cs[i]));
		}
		return list;
	}
	
}
