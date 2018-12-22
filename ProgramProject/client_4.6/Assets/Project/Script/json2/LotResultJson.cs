using UnityEngine;
using System.Collections.Generic;

public class LotResultJson : ErrorJson {
	public int n;//==水晶抽卡点==//
	public int d;//==屌丝券==//
	public int f;//==友情值==//
	public int c;//==水晶值==//
    public int t;//免费抽卡倒计时（秒）//
	public List<PackElement> list;
}
