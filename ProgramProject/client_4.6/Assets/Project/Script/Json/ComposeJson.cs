using UnityEngine;
using System.Collections.Generic;

public class ComposeJson : BasicJson 
{
	/**compose:1角色卡,2 equip,3 item**/
	public int type;
	
	/*compose: index = formID*/
	public int index;
	
	// compose
	public ComposeJson(int type,int formID)
	{
		this.type = type;
		this.index = formID;
	}
}
