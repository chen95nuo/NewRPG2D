using UnityEngine;
using System.Collections;

public class MailResultJson : ErrorJson {
	public string title;
	public string content;
	public string reward1;//格式:type&id&number
	public string reward2;
	public string reward3;
	public string reward4;
	public string reward5;
	public string reward6;
	public int gold;
	public int crystal;
	public int runeNum;
	public int power;
	public int friendNum;
	public int honor;//荣誉点//
	public int diamond;//金刚心//
	public int mark;//==1领取按钮,0关闭按钮==//
}
