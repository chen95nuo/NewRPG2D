using UnityEngine;
using System.Collections.Generic;

public class PackResultJson : ErrorJson
{
	public List<PackElement> list; // remove passive skill from this list
	
	public List<PackElement> list2; // passive skill list

    public List<PackElementJson> pejs;
	
	public int buyTimes;
	
	public List<string> allSelect;
}
