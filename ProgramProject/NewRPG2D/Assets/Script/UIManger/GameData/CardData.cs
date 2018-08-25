using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    [SerializeField]
    private int id;//物品id

    [SerializeField]
    private string name;//名字

    [SerializeField]
    private string spriteName;//图片ID

    [SerializeField]
    private string iconSpriteName;//IconID

    [SerializeField]
    private string animationName;//动画ID

    [SerializeField]
    private int stars;//星级

    [SerializeField]
    private int attribute;//元素

    [SerializeField]
    private float health; //生命值

    [SerializeField]
    private string helthMaxGrow;//生命值成长上限

    [SerializeField]
    private float attack;//攻击

    [SerializeField]
    private string attackMaxGrow;//攻击成长上限

    [SerializeField]
    private float defense;//防御

    [SerializeField]
    private string defenseMaxGrow;//防御成长

    [SerializeField]
    private float agile;//敏捷

    [SerializeField]
    private string agileMaxGrow;//敏捷成长上限

    [SerializeField]
    private string skill_1;//技能1

    [SerializeField]
    private string skill_2;//技能2

    [SerializeField]
    private TeamType teamType; //状态信息

    [SerializeField]
    private int teamPos; //小队位置编号

    [SerializeField]
    private bool fighting; //战斗中

    [SerializeField]
    private int level;//等级

    [SerializeField]
    private float exp;//经验

    [SerializeField]
    private int quality;//品质

    [SerializeField]
    private float useAddExp;//使用获得经验

    [SerializeField]
    private EquipData[] equipdata; //装备

    [SerializeField]
    private float healthGrow; //生命值成长

    [SerializeField]
    private float attackGrow;//攻击成长

    [SerializeField]
    private float defenseGrow;//防御成长

    [SerializeField]
    private float agileGrow;//敏捷成长

    [SerializeField]
    private int goodFeeling;//好感度

    [SerializeField]
    private int maxFeeling;//好感度上限

    [SerializeField]
    private ItemType itemType;//物品类型

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
            if (level == 0)
            {
                return 1;
            }
            return level;
        }
    }

    public float Exp
    {
        get
        {
            return exp;
        }
    }
    public float maxExp
    {
        get
        {
            float index = GameCardExpData.Instance.GetItem(Level).NeedExp;
            return index;
        }

    }
    public float AddExp
    {
        get
        {
            return exp;
        }
        set
        {
            Debug.Log(exp);
            exp += value;
            Debug.Log(value);
            while (exp >= maxExp)
            {
                exp -= maxExp;
                level++;
            }
        }
    }

    public EquipData[] Equipdata
    {
        get
        {
            if (equipdata == null)
            {
                equipdata = new EquipData[4];
            }
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

    public int Attribute
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
            return ItemType.Role;
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

            return FormatGrow(helthMaxGrow, 0);
        }
    }

    public float HealthMaxGrow
    {
        get
        {
            return FormatGrow(helthMaxGrow, 1);
        }
    }

    public float AttackMinGrow
    {
        get
        {
            return FormatGrow(attackMaxGrow, 0);
        }
    }

    public float AttackMaxGrow
    {
        get
        {
            return FormatGrow(attackMaxGrow, 1);
        }
    }

    public float AgileMinGrow
    {
        get
        {
            return FormatGrow(agileMaxGrow, 0);
        }
    }

    public float AgileMaxGrow
    {
        get
        {
            return FormatGrow(agileMaxGrow, 1);
        }
    }

    public float DefenseMinGrow
    {
        get
        {
            return FormatGrow(defenseMaxGrow, 0);
        }
    }

    public float DefenseMaxGrow
    {
        get
        {
            return FormatGrow(defenseMaxGrow, 1);
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

    public float UseAddExp
    {
        get
        {
            useAddExp *= Level;
            return useAddExp;
        }
    }

    public string IconSpriteName
    {
        get
        {
            return iconSpriteName;
        }
    }

    public string AnimationName
    {
        get
        {
            return animationName;
        }
    }

    public string BattleIconSpriteName;

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
        this.spriteName = data.spriteName;
        this.attack = data.attack;
        this.attackGrow = data.attackGrow;
        this.attackMaxGrow = data.attackMaxGrow;
        this.attribute = data.attribute;
        this.defense = data.defense;
        this.defenseGrow = data.defenseGrow;
        this.defenseMaxGrow = data.defenseMaxGrow;
        this.equipdata = data.equipdata;
        this.exp = data.exp;
        this.fighting = data.fighting;
        this.goodFeeling = data.goodFeeling;
        this.maxFeeling = data.maxFeeling;
        this.health = data.health;
        this.healthGrow = data.healthGrow;
        this.helthMaxGrow = data.helthMaxGrow;
        this.id = data.id;
        this.itemType = data.itemType;
        this.level = data.level;
        this.name = data.name;
        this.quality = data.quality;
        this.stars = data.stars;
        this.teamType = data.teamType;
        this.teamPos = data.teamPos;
        this.useAddExp = data.useAddExp;
        this.iconSpriteName = data.iconSpriteName;
        this.animationName = data.animationName;
    }
    public CardData(CardData data, float attack, float health, float defense, float agile, int quality)
    {
        this.agile = data.agile;
        this.agileGrow = agile;
        this.agileMaxGrow = data.agileMaxGrow;
        this.spriteName = data.spriteName;
        this.attack = data.attack;
        this.attackGrow = attack;
        this.attackMaxGrow = data.attackMaxGrow;
        this.attribute = data.attribute;
        this.defense = data.defense;
        this.defenseGrow = defense;
        this.defenseMaxGrow = data.defenseMaxGrow;
        this.equipdata = data.equipdata;
        this.exp = data.exp;
        this.fighting = data.fighting;
        this.goodFeeling = data.goodFeeling;
        this.maxFeeling = data.maxFeeling;
        this.health = data.health;
        this.healthGrow = health;
        this.helthMaxGrow = data.helthMaxGrow;
        this.id = data.id;
        this.itemType = data.itemType;
        this.level = data.level;
        this.name = data.name;
        this.quality = quality;
        this.stars = data.stars;
        this.teamType = data.teamType;
        this.teamPos = data.teamPos;
        this.useAddExp = data.useAddExp;
        this.iconSpriteName = data.iconSpriteName;
        this.animationName = data.animationName;
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

    private float FormatGrow(string grow, int type)
    {
        if (grow != "")
        {
            string[] str;
            str = grow.Split(',');
            float value = System.Convert.ToSingle(str[type]);
            return value;
        }
        else
        {
            Debug.LogError("角色属性参数不正确" + grow + " " + type);
            return 0;
        }
    }
}

[System.Serializable]
public class CardSkill
{
    [SerializeField]
    private int skillId;

    [SerializeField]
    private int skillSpriteName;

    [SerializeField]
    private int skillLevel;

    public int SkillId
    {
        get
        {
            return skillId;
        }
    }

    public int SkillSpriteName
    {
        get
        {
            return skillSpriteName;
        }
    }

    public int SkillLevel
    {
        get
        {
            return skillLevel;
        }
    }
}
