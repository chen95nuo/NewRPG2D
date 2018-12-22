using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PkBattleLogJson : BasicJson {
	//场次信息，用于校验//
	public int bNum;
	//==放技能前双方血量(&号分割12个血量)-放技能后双方血量(&号分割12个血量)-释放技能者index-伤害(&号分割若干个伤害)-当前回合数-合体技ids(&分割)==//
	public List<string> bs;
	//战斗结果，1胜利，2失败//
	public int r;
	
}
