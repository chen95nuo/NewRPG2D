using UnityEngine;
using System.Collections;

public class RuneResultJson : ErrorJson {

	/**当前符文信息:x-xx-xx-xx-xx-xx-xx,遍数-第1图点亮个数-第2图点亮个数-第3图点亮个数-第4图点亮个数-第5图点亮个数-第6图点亮个数**/
	public string id{get;set;}
	/**当前符文值**/
	public int num{get;set;}
	/**当前符文成功率增加值**/
	public string pro{get;set;}
}
