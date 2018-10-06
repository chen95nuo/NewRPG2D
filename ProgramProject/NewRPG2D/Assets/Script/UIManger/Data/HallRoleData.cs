using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallRoleData
{
    private string name;//名字
    private int star;//星级
    public int fightLevel;//战斗等级
    public int goldLevel;//财务等级
    public int foodLevel;//烹饪等级
    public int manaLevel;//炼金等级
    public int woodLevel;//木工等级
    public int ironLevel;//矿工等级
    public Dictionary<RoleAttribute, float> attribute;//属性
    private int[] equip;//装备 1武器2防具3戒指4项链 5神器

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
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
            temp += HP;
            return (int)temp;
        }
    }
    public int Attack
    {
        get
        {
            float temp = 10 + 1 * fightLevel * 1.1f;
            temp += DPS;
            return (int)temp;
        }
    }

    #region 装备所增加的各属性值
    public float Gold
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.Gold))
            {
                float temp = attribute[RoleAttribute.Gold];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.Gold))
            {
                attribute[RoleAttribute.Gold] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.Gold, value);
            }
        }
    }
    public float Food
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.Food))
            {
                float temp = attribute[RoleAttribute.Food];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.Food))
            {
                attribute[RoleAttribute.Food] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.Food, value);
            }
        }
    }
    public float Mana
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.Mana))
            {
                float temp = attribute[RoleAttribute.Mana];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.Mana))
            {
                attribute[RoleAttribute.Mana] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.Mana, value);
            }
        }
    }
    public float ManaSpeed
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.ManaSpeed))
            {
                float temp = attribute[RoleAttribute.ManaSpeed];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.ManaSpeed))
            {
                attribute[RoleAttribute.ManaSpeed] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.ManaSpeed, value);
            }
        }
    }
    public float Wood
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.Wood))
            {
                float temp = attribute[RoleAttribute.Wood];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.Wood))
            {
                attribute[RoleAttribute.Wood] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.Wood, value);
            }
        }
    }
    public float Iron
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.Iron))
            {
                float temp = attribute[RoleAttribute.Iron];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.Iron))
            {
                attribute[RoleAttribute.Iron] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.Iron, value);
            }
        }
    }
    public float HurtType
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.HurtType))
            {
                float temp = attribute[RoleAttribute.HurtType];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.HurtType))
            {
                attribute[RoleAttribute.HurtType] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.HurtType, value);
            }
        }
    }
    public float DPS
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.DPS))
            {
                float temp = attribute[RoleAttribute.DPS];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.DPS))
            {
                attribute[RoleAttribute.DPS] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.DPS, value);
            }
        }
    }
    public float Crt
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.Crt))
            {
                float temp = attribute[RoleAttribute.Crt];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.Crt))
            {
                attribute[RoleAttribute.Crt] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.Crt, value);
            }
        }
    }
    public float PArmor
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.PArmor))
            {
                float temp = attribute[RoleAttribute.PArmor];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.PArmor))
            {
                attribute[RoleAttribute.PArmor] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.PArmor, value);
            }
        }
    }
    public float MArmor
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.MArmor))
            {
                float temp = attribute[RoleAttribute.MArmor];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.MArmor))
            {
                attribute[RoleAttribute.MArmor] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.MArmor, value);
            }
        }
    }
    public float Dodge
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.Dodge))
            {
                float temp = attribute[RoleAttribute.Dodge];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.Dodge))
            {
                attribute[RoleAttribute.Dodge] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.Dodge, value);
            }
        }
    }
    public float HIT
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.HIT))
            {
                float temp = attribute[RoleAttribute.HIT];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.HIT))
            {
                attribute[RoleAttribute.HIT] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.HIT, value);
            }
        }
    }
    public float INT
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.INT))
            {
                float temp = attribute[RoleAttribute.INT];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.INT))
            {
                attribute[RoleAttribute.INT] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.INT, value);
            }
        }
    }
    public float HP
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.HP))
            {
                float temp = attribute[RoleAttribute.HP];
                return temp;
            }
            return 0;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.HP))
            {
                attribute[RoleAttribute.HP] = value;
            }
            else
            {
                attribute.Add(RoleAttribute.HP, value);
            }
        }
    }
    #endregion

    #region 各属性产量
    public int GoldProduce
    {
        get
        {
            float temp = goldLevel * 4;
            temp += Gold;
            return (int)temp;
        }
    }
    public int FoodProduce
    {
        get
        {
            float temp = foodLevel * 9;
            temp += Food;
            return (int)temp;
        }
    }
    public int ManaProduce
    {
        get
        {
            float temp = manaLevel * 4;
            temp += Mana;
            return (int)temp;
        }
    }
    public float ManaSpeedUp
    {
        get
        {
            float temp = (manaLevel * 0.05f) + 1.25f;
            temp += ManaSpeed;
            return temp;
        }
    }
    public int WoodProduce
    {
        get
        {
            float temp = woodLevel * 3;
            temp += Wood;
            return (int)temp;
        }
    }
    public int IronProduce
    {
        get
        {
            float temp = ironLevel * 3;
            temp += Iron;
            return (int)temp;
        }
    }
    #endregion


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

        attribute = new Dictionary<RoleAttribute, float>();
    }
}
