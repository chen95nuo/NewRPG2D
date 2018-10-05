using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallRoleData
{
    private int id;
    private int star;//星级
    public int fightLevel;//战斗等级
    public int goldLevel;//财务等级
    public int foodLevel;//烹饪等级
    public int manaLevel;//炼金等级
    public int woodLevel;//木工等级
    public int ironLevel;//矿工等级
    private int[] equip;//装备 1武器2防具3戒指4项链 5神器

    public int Id
    {
        get
        {
            return id;
        }
    }

    public int Star
    {
        get
        {
            return star;
        }
        set
        {
            int temp = value;
            if (temp != star)
            {
                star = value;
                Debug.LogError("角色的星级被改变了");
            }
        }
    }



    public int[] Equip
    {
        get
        {
            return equip;
        }
    }

    public int Health
    {
        get
        {
            float temp = (fightLevel * 40) + 200;
            return (int)temp;
        }
    }

    public int Attack
    {
        get
        {
            float temp = 10 + 1 * fightLevel * 1.1f;
            return (int)temp;
        }
    }

    public int FoodProduce
    {
        get
        {
            float temp = foodLevel * 9;
            return (int)temp;
        }
    }

    public int ManaProduce
    {
        get
        {
            float temp = manaLevel * 4;
            return (int)temp;
        }
    }

    public float ManaSpeedUp
    {
        get
        {
            float temp = (manaLevel * 0.05f) + 1.25f;
            return temp;
        }
    }

    public int WoodProduce
    {
        get
        {
            float temp = woodLevel * 3;
            return (int)temp;
        }
    }

    public int IronProduce
    {
        get
        {
            float temp = ironLevel * 3;
            return (int)temp;
        }
    }

    public int GoldProduce
    {
        get
        {
            float temp = goldLevel * 4;
            return (int)temp;
        }
    }


    public HallRoleData() { }
    public HallRoleData(int star, int[] level)
    {
        this.star = star;
        this.fightLevel = level[0];
        this.goldLevel = level[1];
        this.foodLevel = level[2];
        this.manaLevel = level[3];
        this.woodLevel = level[4];
        this.ironLevel = level[5];
    }
}
