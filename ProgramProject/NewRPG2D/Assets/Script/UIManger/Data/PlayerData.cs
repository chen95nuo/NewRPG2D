using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string name;//玩家昵称
    public int level;//城堡等级
    public int gold;//金币数量
    public int diamonds;//钻石数量
    public int food;//食物
    public int mana;//法力值
    public int wood;//木材
    public int iron;//铁矿
    public int mainHallLevel;//大殿等级
    public int goldSpace;//金币空间
    public int foodSpace;//食物空间
    public int manaSpace;//魔法空间
    public int woodSpace;//木材空间
    public int ironSpace;//铁矿空间

    public PlayerData()
    {
        name = "Baymax";
        level = 1;
        gold = 100;
        diamonds = 100;
        food = 0;
        mana = 0;
        wood = 0;
        iron = 0;
        mainHallLevel = 1;
        goldSpace = 5000;
        foodSpace = 5000;
        manaSpace = 5000;
        woodSpace = 5000;
        ironSpace = 5000;
    }
}
