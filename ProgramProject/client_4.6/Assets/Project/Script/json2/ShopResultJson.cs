using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopResultJson : ErrorJson
{
    public List<ShopElement> ses1;
    public List<ShopElement> ses2;
    public int refreshTime;
    public int refresh;
	public int pvpHonor;//pvp荣誉点//
}

public class ShopElement
{
    public int id;  //shop&blackMarket id   //day of seven-days prize
    public int num; //buy num               //flag of seven-days 0未领取 1已领取//
}

