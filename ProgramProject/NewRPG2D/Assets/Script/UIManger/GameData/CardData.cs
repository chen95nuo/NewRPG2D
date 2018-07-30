using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    [SerializeField]
    private string name;//名字

    [SerializeField]
    private int id;//物品id

    [SerializeField]
    private string spriteName;//图片ID

    [SerializeField]
    private TeamType teamType; //状态信息

    [SerializeField]
    private int teamPos;

    [SerializeField]
    private bool fighting; //战斗中

    [SerializeField]
    private int level;//等级

    [SerializeField]
    private int quality;//稀有度

    [SerializeField]
    private int exp;//经验

    [SerializeField]
    private EquipData[] equipdata; //装备

    [SerializeField]
    private float health; //生命值

    [SerializeField]
    private float healthGrow; //生命值成长

    [SerializeField]
    private float healthMinGrow;//生命值成长下限

    [SerializeField]
    private float healthMaxGrow;//生命值成长上限

    [SerializeField]
    private float attack;//攻击

    [SerializeField]
    private float attackGrow;//攻击成长

    [SerializeField]
    private float attackMinGrow;//攻击成长下限

    [SerializeField]
    private float attackMaxGrow;//攻击成长上限

    [SerializeField]
    private float agile;//敏捷

    [SerializeField]
    private float agileGrow;//敏捷成长

    [SerializeField]
    private float agileMinGrow;//敏捷成长下限

    [SerializeField]
    private float agileMaxGrow;//敏捷成长上限

    [SerializeField]
    private float defense;//防御

    [SerializeField]
    private float defenseGrow;//防御成长

    [SerializeField]
    private float defenseMinGrow;//防御成长

    [SerializeField]
    private float defenseMaxGrow;//防御成长

    [SerializeField]
    private int goodFeeling;//好感度

    [SerializeField]
    private int maxFeeling;//好感度上限

    [SerializeField]
    private int stars;//星级

    [SerializeField]
    private ItemType itemType;//物品类型

    [SerializeField]
    private string attribute;//属性

    public string Name
    {
        get
        {
            return name;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }
    }

    public TeamType TeamType
    {
        get
        {
            return teamType;
        }
        set
        {
            teamType = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public int Exp
    {
        get
        {
            return exp;
        }
    }

    public EquipData[] Equipdata
    {
        get
        {
            return equipdata;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
    }

    public float HealthGrow
    {
        get
        {
            return healthGrow;
        }
    }

    public float Attack
    {
        get
        {
            return attack;
        }
    }

    public float AttackGrow
    {
        get
        {
            return attackGrow;
        }
    }

    public float Agile
    {
        get
        {
            return agile;
        }
    }

    public float AgileGrow
    {
        get
        {
            return agileGrow;
        }
    }

    public float Defense
    {
        get
        {
            return defense;
        }
    }

    public float DefenseGrow
    {
        get
        {
            return defenseGrow;
        }
    }

    public int GoodFeeling
    {
        get
        {
            return goodFeeling;
        }
        set
        {
            goodFeeling = value;
        }
    }

    public int Stars
    {
        get
        {
            return stars;
        }
    }

    public int Quality
    {
        get
        {
            return quality;
        }
    }

    public string Attribute
    {
        get
        {
            return attribute;
        }
    }

    public ItemType ItemType
    {
        get
        {
            return itemType;
        }
    }

    public bool Fighting
    {
        get
        {
            return fighting;
        }
        set
        {
            bool index = fighting;
            if (index != value)
            {
                fighting = value;
            }
        }
    }

    public int TeamPos
    {
        get
        {
            return teamPos;
        }

        set
        {
            teamPos = value;
        }
    }

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public float HealthMinGrow
    {
        get
        {
            return healthMinGrow;
        }
    }

    public float HealthMaxGrow
    {
        get
        {
            return healthMaxGrow;
        }
    }

    public float AttackMinGrow
    {
        get
        {
            return attackMinGrow;
        }
    }

    public float AttackMaxGrow
    {
        get
        {
            return attackMaxGrow;
        }
    }

    public float AgileMinGrow
    {
        get
        {
            return agileMinGrow;
        }
    }

    public float AgileMaxGrow
    {
        get
        {
            return agileMaxGrow;
        }
    }

    public float DefenseMinGrow
    {
        get
        {
            return defenseMinGrow;
        }
    }

    public float DefenseMaxGrow
    {
        get
        {
            return defenseMaxGrow;
        }
    }

    public int MaxFeeling
    {
        get
        {
            return maxFeeling;
        }
        set
        {
            maxFeeling = value;
        }
    }

    public CardData() { }

    public CardData(int id)
    {
        this.id = id;
    }
    public CardData(CardData data)
    {
        this.agile = data.agile;
        this.agileGrow = data.agileGrow;
        this.agileMaxGrow = data.agileMaxGrow;
        this.agileMinGrow = data.agileMinGrow;
        this.spriteName = data.spriteName;
        this.attack = data.attack;
        this.attackGrow = data.attackGrow;
        this.attackMaxGrow = data.attackMaxGrow;
        this.attackMinGrow = data.attackMinGrow;
        this.attribute = data.attribute;
        this.defense = data.defense;
        this.defenseGrow = data.defenseGrow;
        this.defenseMaxGrow = data.defenseMaxGrow;
        this.defenseMinGrow = data.defenseMinGrow;
        this.equipdata = data.equipdata;
        this.exp = data.exp;
        this.fighting = data.fighting;
        this.goodFeeling = data.goodFeeling;
        this.maxFeeling = data.maxFeeling;
        this.health = data.health;
        this.healthGrow = data.healthGrow;
        this.healthMaxGrow = data.healthMaxGrow;
        this.healthMinGrow = data.healthMinGrow;
        this.id = data.id;
        this.itemType = data.itemType;
        this.level = data.level;
        this.name = data.name;
        this.quality = data.quality;
        this.stars = data.stars;
        this.teamType = data.teamType;
        this.teamPos = data.teamPos;
    }
    public CardData(CardData data, float attack, float health, float defense, float agile,int quality)
    {
        this.agile = data.agile;
        this.agileGrow = agile;
        this.agileMaxGrow = data.agileMaxGrow;
        this.agileMinGrow = data.agileMinGrow;
        this.spriteName = data.spriteName;
        this.attack = data.attack;
        this.attackGrow = attack;
        this.attackMaxGrow = data.attackMaxGrow;
        this.attackMinGrow = data.attackMinGrow;
        this.attribute = data.attribute;
        this.defense = data.defense;
        this.defenseGrow = defense;
        this.defenseMaxGrow = data.defenseMaxGrow;
        this.defenseMinGrow = data.defenseMinGrow;
        this.equipdata = data.equipdata;
        this.exp = data.exp;
        this.fighting = data.fighting;
        this.goodFeeling = data.goodFeeling;
        this.maxFeeling = data.maxFeeling;
        this.health = data.health;
        this.healthGrow = health;
        this.healthMaxGrow = data.healthMaxGrow;
        this.healthMinGrow = data.healthMinGrow;
        this.id = data.id;
        this.itemType = data.itemType;
        this.level = data.level;
        this.name = data.name;
        this.quality = quality;
        this.stars = data.stars;
        this.teamType = data.teamType;
        this.teamPos = data.teamPos;
    }
    public CardData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
    public CardData(int id, string name, ItemType type)
    {
        this.id = id;
        this.name = name;
        this.itemType = type;
    }


}
