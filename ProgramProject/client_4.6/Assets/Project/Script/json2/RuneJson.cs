using UnityEngine;
using System.Collections;

public class RuneJson : BasicJson {
	
	/**0表示打开符文界面,其他值代表点亮符文操作**/
	public int page{get;set;}
	
	public RuneJson(int page)
	{
		this.page=page;
	}
}
