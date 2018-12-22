using UnityEngine;
using System.Collections.Generic;

public class BreakResultJson : ErrorJson 
{
	public List<PackElement> pes;


    public PackElement pe;
    public int pd; //玩家拥有的金罡心数量//
    public int d;//突破需要的金罡心数量//
    public int cn;//突破需要的卡牌数量//
    public int pcn;//玩家拥有此卡牌的数量//
    public int pmc;//同星级的万能突破卡//

    public int sell; //新手标识//


}
