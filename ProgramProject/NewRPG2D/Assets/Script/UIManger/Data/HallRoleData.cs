using Assets.Script.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallRoleData
{
    public readonly SexTypeEnum sexType;
    private string name;//名字
    private int star;//星级
    private HallRoleLevel[] roleLevel = new HallRoleLevel[7];
    //private int fightLevel;//战斗等级
    //public int goldLevel;//财务等级
    //public int foodLevel;//烹饪等级
    //public int manaLevel;//炼金等级
    //public int woodLevel;//木工等级
    //public int ironLevel;//矿工等级
    //public int MaxLevel;//最大等级
    public Dictionary<RoleAttribute, float> attribute = new Dictionary<RoleAttribute, float>();//属性
    private int[] equip;//装备 1武器2防具3戒指4项链 5神器
    private RoleTrainType trainType;//训练类型
    public int trainIndex;//训练编号
    public RoleLoveType LoveType;//爱情状态
    private float nowHp;//当前血量
    public bool isBaby;//是不是小孩
    public RoleBabyData babyData;//宝宝数据
    public RoomMgr currentRoom;

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

    public int Health
    {
        get
        {
            float temp = ((roleLevel[0].Level - 1) * 40) + 200;
            return (int)(temp + HP);
        }
    }
    public int Attack
    {
        get
        {
            float temp = 10 + 1 * roleLevel[0].Level * 1.1f;
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
    public HurtTypeEnum HurtType
    {
        get
        {
            if (attribute.ContainsKey(RoleAttribute.HurtType))
            {
                float temp = attribute[RoleAttribute.HurtType];
                return (HurtTypeEnum)temp;
            }
            return HurtTypeEnum.Physic;
        }
        set
        {
            if (attribute.ContainsKey(RoleAttribute.HurtType))
            {
                attribute[RoleAttribute.HurtType] = (int)value;
            }
            else
            {
                attribute.Add(RoleAttribute.HurtType, (int)value);
            }
        }
    }
    private float DPS
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
            float temp = roleLevel[1].Level * 4;
            temp += Gold;
            return (int)temp;
        }
    }
    public int FoodProduce
    {
        get
        {
            float temp = roleLevel[2].Level * 9;
            temp += Food;
            return (int)temp;
        }
    }
    public int ManaProduce
    {
        get
        {
            float temp = roleLevel[3].Level * 4;
            temp += Mana;
            return (int)temp;
        }
    }
    public float ManaSpeedUp
    {
        get
        {
            float temp = (roleLevel[3].Level * 0.05f) + 1.25f;
            temp += ManaSpeed;
            return temp;
        }
    }
    public int WoodProduce
    {
        get
        {
            float temp = roleLevel[4].Level * 3;
            temp += Wood;
            return (int)temp;
        }
    }
    public int IronProduce
    {
        get
        {
            float temp = roleLevel[5].Level * 3;
            temp += Iron;
            return (int)temp;
        }
    }

    public int FightLevel
    {
        get
        {
            return roleLevel[0].Level;
        }
    }
    public int GoldLevel
    {
        get { return roleLevel[1].Level; }
    }
    public int FoodLevel
    {
        get { return roleLevel[2].Level; }
    }
    public int ManaLevel
    {
        get { return roleLevel[3].Level; }
    }
    public int WoodLevel
    {
        get { return roleLevel[4].Level; }
    }
    public int IronLevel
    {
        get { return roleLevel[5].Level; }
    }

    public HallRoleLevel[] RoleLevel
    {
        get
        {
            return roleLevel;
        }
    } //0战斗 1金币 2食物 3魔法 4木材 5铁

    public RoleTrainType TrainType
    {
        get
        {
            return trainType;
        }

        set
        {
            RoleTrainType temp = value;
            if (temp != trainType)
            {
                trainType = value;
                HallEventManager.instance.SendEvent<HallRoleData>(HallEventDefineEnum.ChickRoleTrain, this);
            }

        }
    }

    public float NowHp
    {
        get
        {
            if (nowHp == -1)
            {
                nowHp = Health;
            }
            return nowHp;
        }

        set
        {
            nowHp = value;
        }
    }

    public int[] Equip
    {
        get
        {
            if (equip == null)
            {
                equip = new int[5];
            }
            return equip;
        }
    }
    #endregion

    public float Attribute(RoleAttribute attribute)
    {
        switch (attribute)
        {
            case RoleAttribute.Fight:
                return FightLevel;
            case RoleAttribute.Gold:
                return GoldProduce;
            case RoleAttribute.Food:
                return FoodProduce;
            case RoleAttribute.Mana:
                return ManaProduce;
            case RoleAttribute.ManaSpeed:
                return ManaSpeedUp;
            case RoleAttribute.Wood:
                return WoodProduce;
            case RoleAttribute.Iron:
                return IronProduce;
            case RoleAttribute.HurtType:
                return (int)HurtType;
            case RoleAttribute.DPS:
                return DPS;
            case RoleAttribute.Crt:
                return Crt;
            case RoleAttribute.PArmor:
                return PArmor;
            case RoleAttribute.MArmor:
                return MArmor;
            case RoleAttribute.Dodge:
                return Dodge;
            case RoleAttribute.HIT:
                return HIT;
            case RoleAttribute.INT:
                return INT;
            case RoleAttribute.HP:
                return HP;
            default:
                break;
        }
        return 0;
    }

    public int GetAtrLevel(RoleAttribute atr)
    {
        switch (atr)
        {
            case RoleAttribute.Fight:
                return roleLevel[0].Level;
            case RoleAttribute.Gold:
                return roleLevel[1].Level;
            case RoleAttribute.Food:
                return roleLevel[2].Level;
            case RoleAttribute.Mana:
                return roleLevel[3].Level;
            case RoleAttribute.Wood:
                return roleLevel[4].Level;
            case RoleAttribute.Iron:
                return roleLevel[5].Level;
            case RoleAttribute.Max:
                return roleLevel[6].Level;
            default:
                break;
        }
        return 0;
    }
    public int GetAtrLevel(TrainType type)
    {
        int index = 0;
        switch (type)
        {
            case global::TrainType.Nothing:
                break;
            case global::TrainType.Fight:
                index = roleLevel[0].Level;
                break;
            case global::TrainType.Gold:
                index = roleLevel[1].Level;
                break;
            case global::TrainType.Food:
                index = roleLevel[2].Level;
                break;
            case global::TrainType.Mana:
                index = roleLevel[3].Level;
                break;
            case global::TrainType.Wood:
                index = roleLevel[4].Level;
                break;
            case global::TrainType.Iron:
                index = roleLevel[5].Level;
                break;
            case global::TrainType.Max:
                break;
            default:
                break;
        }
        return index;
    }
    public int GetAtrLevel(BuildRoomName name)
    {
        int index = 0;
        switch (name)
        {
            case BuildRoomName.FighterRoom:
                index = roleLevel[0].Level;
                break;
            case BuildRoomName.Mint:
                index = roleLevel[1].Level;
                break;
            case BuildRoomName.Kitchen:
                index = roleLevel[2].Level;
                break;
            case BuildRoomName.Laboratory:
                index = roleLevel[3].Level;
                break;
            case BuildRoomName.Crafting:
                index = roleLevel[4].Level;
                break;
            case BuildRoomName.Foundry:
                index = roleLevel[5].Level;
                break;
            default:
                break;
        }
        return index;
    }
    public float GetAtrProduce(RoleAttribute atr)
    {
        switch (atr)
        {
            case RoleAttribute.Fight:
                return FightLevel;
            case RoleAttribute.Gold:
                return GoldProduce;
            case RoleAttribute.Food:
                return FoodProduce;
            case RoleAttribute.Mana:
                return ManaProduce;
            case RoleAttribute.ManaSpeed:
                return ManaSpeed;
            case RoleAttribute.Wood:
                return WoodProduce;
            case RoleAttribute.Iron:
                return IronProduce;
            case RoleAttribute.Max:
                return RoleLevel[6].Level;
            default:
                break;
        }
        return 0;
    }
    public float GetArtProduce(BuildRoomName atr)
    {
        switch (atr)
        {
            case BuildRoomName.Nothing:
                return 0;
            case BuildRoomName.Gold:
                return GoldProduce;
            case BuildRoomName.Food:
                return FoodProduce;
            case BuildRoomName.Mana:
                return ManaProduce;
            case BuildRoomName.Wood:
                return WoodProduce;
            case BuildRoomName.Iron:
                return IronProduce;
            case BuildRoomName.FighterRoom:
                return FightLevel;
            case BuildRoomName.Kitchen:
                return roleLevel[2].Level++;
            case BuildRoomName.Mint:
                return roleLevel[1].Level++;
            case BuildRoomName.Laboratory:
                return roleLevel[3].Level++;
            case BuildRoomName.Crafting:
                return roleLevel[4].Level++;
            case BuildRoomName.Foundry:
                return roleLevel[5].Level++;
            case BuildRoomName.Hospital:
                return HP;
            case BuildRoomName.Barracks:
                return FightLevel;
            default:
                break;
        }
        return ManaSpeedUp;
    }
    public void LevelUp(RoleAttribute atr)
    {
        switch (atr)
        {
            case RoleAttribute.Fight:
                roleLevel[0].Level++;
                break;
            case RoleAttribute.Gold:
                roleLevel[1].Level++;
                break;
            case RoleAttribute.Food:
                roleLevel[2].Level++;
                break;
            case RoleAttribute.Mana:
                roleLevel[3].Level++;
                break;
            case RoleAttribute.Wood:
                roleLevel[4].Level++;
                break;
            case RoleAttribute.Iron:
                roleLevel[5].Level++;
                break;
            default:
                break;
        }
    }
    public void LevelUp(BuildRoomName atr)
    {
        switch (atr)
        {
            case BuildRoomName.FighterRoom:
                roleLevel[0].Level++;
                break;
            case BuildRoomName.Mint:
                roleLevel[1].Level++;
                break;
            case BuildRoomName.Kitchen:
                roleLevel[2].Level++;
                break;
            case BuildRoomName.Laboratory:
                roleLevel[3].Level++;
                break;
            case BuildRoomName.Crafting:
                roleLevel[4].Level++;
                break;
            case BuildRoomName.Foundry:
                roleLevel[5].Level++;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 检查最高等级
    /// </summary>
    public void ChickMaxLevel()
    {
        int temp = 0;
        int index = 0;
        for (int i = 0; i < roleLevel.Length - 1; i++)
        {
            if (roleLevel[i].Level > temp)
            {
                temp = roleLevel[i].Level;
                index = i;
            }
        }
        roleLevel[6] = roleLevel[index];
    }

    public void AddEquip(EquipmentRealProperty equipData)
    {

        if (Equip[(int)equipData.EquipType] == 0)
        {
            UseEquip(equipData);
        }
        else
        {
            ChangeEquip(equipData);
        }
    }
    private void UseEquip(EquipmentRealProperty equipData)
    {
        EquipmentMgr.instance.RemoveEquipmentData(equipData);
        Equip[(int)equipData.EquipType] = equipData.EquipId;
        foreach (var item in equipData.RoleProperty)
        {
            if (item.Value > 0 && item.Key <= RoleAttribute.DPS)
            {
                if (attribute.ContainsKey(item.Key))
                {
                    attribute[item.Key] += item.Value;
                }
                else
                {
                    attribute.Add(item.Key, item.Value);
                }
            }
        }

        if (equipData.EquipType == EquipTypeEnum.Sword)
        {
            HurtType = equipData.HurtType;
        }

        nowHp = HP;

        //换皮肤
        HallRole role = HallRoleMgr.instance.GetRole(this);
        if ((int)equipData.EquipType <= 1)
        {
            role.ChangeSkil(equipData);
        }
    }
    private void ChangeEquip(EquipmentRealProperty equipData)
    {
        EquipmentRealProperty data = EquipmentMgr.instance.GetEquipmentByEquipId(Equip[(int)equipData.EquipType]);
        EquipmentMgr.instance.AddEquipmentData(data);
        UseEquip(equipData);

        foreach (var item in equipData.RoleProperty)
        {
            if (item.Value > 0 && item.Key <= RoleAttribute.DPS)
            {
                attribute[item.Key] -= item.Value;
            }
        }

    }
    public void UnloadEquip(EquipmentRealProperty equipData)
    {
        if (Equip[(int)equipData.EquipType] == 0)
        {
            Debug.LogError("脱装备出错 该装备不存在");
        }
        Equip[(int)equipData.EquipType] = 0;
        EquipmentMgr.instance.AddEquipmentData(equipData);
        foreach (var item in equipData.RoleProperty)
        {
            if (item.Value > 0 && item.Key <= RoleAttribute.DPS)
            {
                attribute[item.Key] -= item.Value;
            }
        }

        if (equipData.EquipType == EquipTypeEnum.Sword)
        {
            HurtType = HurtTypeEnum.Physic;
        }

        nowHp = HP;

        //皮肤换回来
        HallRole role = HallRoleMgr.instance.GetRole(this);
        //if ((int)equipData.EquipType <= 1)
        //{
        //    role.ChangeSkil(equipData);
        //}
    }

    public HallRoleData() { }
    public HallRoleData(int sex, int star, int[] level)
    {
        this.sexType = (SexTypeEnum)sex;
        this.star = star;
        this.name = "Json";
        roleLevel[0] = new HallRoleLevel(RoleAttribute.Fight, level[0]);
        roleLevel[1] = new HallRoleLevel(RoleAttribute.Gold, level[1]);
        roleLevel[2] = new HallRoleLevel(RoleAttribute.Food, level[2]);
        roleLevel[3] = new HallRoleLevel(RoleAttribute.Mana, level[3]);
        roleLevel[4] = new HallRoleLevel(RoleAttribute.Wood, level[4]);
        roleLevel[5] = new HallRoleLevel(RoleAttribute.Iron, level[5]);
        nowHp = -1;
        ChickMaxLevel();
    }
}

public class HallRoleLevel
{
    public RoleAttribute atr;
    public int Level;

    public HallRoleLevel(RoleAttribute atr, int level)
    {
        this.atr = atr;
        this.Level = level;
    }
}
