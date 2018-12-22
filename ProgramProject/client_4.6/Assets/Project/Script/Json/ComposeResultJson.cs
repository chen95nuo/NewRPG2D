using UnityEngine;
using System.Collections.Generic;

public class ComposeResultJson : ErrorJson
{
	public List<string> cs;
	public List<PackElement> pes;
	/**合成成功为0，失败为-1**/
	//public string s;// new unlock items id  : id,id,id ...

    public List<string> mark; //种类-Type   0为没有新提醒，1为有新提醒

	public int t;

}
