using UnityEngine;
using System.Collections.Generic;

// intensify and compose json
public class IntensifyJson : BasicJson 
{
	/**intesify:1角色卡,2主动技能卡,3 passive skill, 4equip**/
	/**compose:1角色卡,2 equip,3 item**/
	public int type;
	
	/**强化者索引**/
	/*compose: index = formID*/
	public int index;
	
	/**被吞噬者索引**/
	/*compose: send null*/
	public List<int> list;



    public int numType; //强化次数标识,1表单次强化,多次强化为强化次数//
	// intensify
	public IntensifyJson(int type,int index,List<int> list)
	{
		this.type=type;
		this.index=index;
		this.list=list;
	}

    public IntensifyJson(int type, int index, List<int> list,int numType)
    {
        this.type = type;
        this.index = index;
        this.list = list;
        this.numType = numType;
    }

	// compose
	public IntensifyJson(int type,int formID)
	{
		this.type = type;
		this.index = formID;
	}
}
