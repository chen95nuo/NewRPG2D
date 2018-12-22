using UnityEngine;
using System.Collections;

public class SweepUiJson : BasicJson 
{

	/**
	 *请求扫荡界面信息的json 
	 */
	//关卡id//
	public int md;
	
	public SweepUiJson(int md)
	{
		this.md = md;
	}
}
