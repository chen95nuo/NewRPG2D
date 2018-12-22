using UnityEngine;
using System.Collections.Generic;

public class BattleLogJson:BasicJson
{
	public List<string> bs;//==放技能前双方血量(&号分割12个血量)-放技能后双方血量(&号分割12个血量)-释放技能者index-伤害(&号分割若干个伤害)-当前回合数-合体技ids(&分割)==//
	public int r;//result:1胜利,2失败,3 DemoBattle//
	public string gm;//goldMul金币倍数//
	public int bNum;//场次信息
	public int sNum;		//星级：1,2,3//
	public int bonus;		//是否出现bouns(0未出现， 1出现)//
	public int round;//==经历回合数==//
}
