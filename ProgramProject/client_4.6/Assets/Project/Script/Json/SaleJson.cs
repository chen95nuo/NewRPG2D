using UnityEngine;
using System.Collections.Generic;

public class SaleJson:BasicJson
{
	/**1card,2skill,3equip,4item,8passiveSkill**/
	public int type;
	/**元素格式:位置**/
	public List<int> list;
	
	public SaleJson(int type,List<int> list)
	{
		this.type=type;
		this.list=list;
	}
}
