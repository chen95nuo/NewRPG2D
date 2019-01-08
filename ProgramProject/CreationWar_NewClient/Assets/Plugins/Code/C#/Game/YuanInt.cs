using UnityEngine;
using System.Collections;

public static class YuanInt  {

	public static int Parse(this string mStr,int num)
	{
		if(!string.IsNullOrEmpty(mStr))
		{
			int tempNum=0;
			if(int.TryParse (mStr,out tempNum))
			{
				num=tempNum;
			}
			
		}
		return num;
	}
	
	public static string To16String(this int mInt)
	{
			return System.Convert.ToString(mInt,16);
	}
}
