using UnityEngine;
using System.Collections;
//using System.Convert;

public class ToInt : MonoBehaviour {
	public static int StrToInt(string str){
		return System.Convert.ToInt32(str , 16);
	}
	public static string IntToStr(int num){
		return System.Convert.ToString(num , 16);
	}
}
