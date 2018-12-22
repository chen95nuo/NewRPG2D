using UnityEngine;
using System.Collections;

public class MapResultJson : ErrorJson {

	public int m1;
	public string star1;
	public string b1;//领取过bonus为1,没有bonus为0---（0101）//
	public string t1;
	public int m2;
	public string star2;
	public string b2;//领取过bonus为1,没有bonus为0---（0101）//
	public string t2;
	
	public int c;//水晶//
	public int d;//金币//
	public int kopoint;//ko积分//
	public int koType;//0没有新的ko兑换 1有新的ko兑换//
	public int fType;//阵容界面提醒(1为由提醒，0为没有，2为有提升);
}
