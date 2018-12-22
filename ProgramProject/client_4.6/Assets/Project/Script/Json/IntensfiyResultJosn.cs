using UnityEngine;
using System.Collections.Generic;

public class IntensifyResultJosn : ErrorJson 
{
	/**强化是否成功 默认0成功，1暴击，-1失败**/
	public int state;
	/**强化对象**/
	public PackElement pe;
}
