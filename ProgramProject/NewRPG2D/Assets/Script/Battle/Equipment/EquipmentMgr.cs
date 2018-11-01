using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Assets.Script.Battle;
using Assets.Script.Battle.BattleData;
using UnityEngine;

public class EquipmentRealProperty
{
    public int EquipId;
    public EquipTypeEnum EquipType;
    public QualityTypeEnum QualityType;
    public HurtTypeEnum HurtType;
    public WeaponTypeEnum WeaponType;
    public WeaponProfessionEnum WeaponProfession;
    public ProfessionNeedEnum ProfessionNeed;
    public string SpriteName;
    public string EquipName;
    public string Name;
    public int Level;
    public int AttackRange;
    public float AttackSpeed;
    public Dictionary<RoleAttribute, float> RoleProperty;
    public List<SpecialPropertyData> SpecialProperty;
}

public class EquipmentMgr : TSingleton<EquipmentMgr>
{
    private Dictionary<int, EquipmentRealProperty> AllEquipmentData = new Dictionary<int, EquipmentRealProperty>();
    private List<EquipmentRealProperty> AllEquipmentDataList = new List<EquipmentRealProperty>();
    private static int equipInstanceId = 1000;
    private float attackSpeed = StaticAndConstParamter.AttackSpeed;

    public override void Init()
    {
        base.Init();
        AllEquipmentData.Clear();
    }

    public EquipmentRealProperty CreateNewEquipment(int itemId, int dependLevel = -1)
    {
        EquipmentData data = EquipmentDataMgr.instance.GetXmlDataByItemId<EquipmentData>(itemId);

        if (data == null)
        {
            return null;
        }

        Dictionary<RoleAttribute, float> roleProperty = new Dictionary<RoleAttribute, float>();
        List<SpecialPropertyData> specialProperty = new List<SpecialPropertyData>();
        int equipId = equipInstanceId++;
        EquipmentRealProperty realProperty = new EquipmentRealProperty();
        realProperty.EquipId = equipId;
        realProperty.EquipType = data.EquipType;
        realProperty.Level = dependLevel < 0 ? (int)Random.Range(data.LevelRange.Min, data.LevelRange.Max) : dependLevel;
        attackSpeed = data.AttackSpeed;
        if (attackSpeed < 0.0001f)
        {
            attackSpeed = 1;
        }

        GetRoleProperty(roleProperty, data, realProperty.Level);
        realProperty.RoleProperty = roleProperty;
        realProperty.AttackRange = data.AttackRange;
        realProperty.AttackSpeed = attackSpeed;
        realProperty.QualityType = data.QualityType;
        GetSpecialProperty(specialProperty, data);
        realProperty.SpecialProperty = specialProperty;
        realProperty.HurtType = data.HurtType;
        realProperty.WeaponType = data.WeaponType;
        realProperty.WeaponProfession = data.WeaponProfession;
        realProperty.ProfessionNeed = data.ProfessionNeed;
        realProperty.EquipName = data.EquipName;
        realProperty.SpriteName = data.SpriteName;
        realProperty.Name = data.ItemName;
        AllEquipmentData[equipId] = realProperty;
        AllEquipmentDataList.Add(realProperty);
        return realProperty;
    }

    public EquipmentRealProperty GetEquipmentByEquipId(int equipId)
    {
        EquipmentRealProperty data = null;
        if (AllEquipmentData.TryGetValue(equipId, out data))
        {
            return data;
        }
        else
        {

            return null;
        }
    }

    public float GetEquipmentValueByEquipIdAndType(int equipId, RoleAttribute type)
    {
        return AllEquipmentData[equipId].RoleProperty[type];
    }
    #region HallEquipType

    public List<EquipmentRealProperty> GetAllEquipmentData()
    {
        return AllEquipmentDataList;
    }
    public void RemoveEquipmentData(EquipmentRealProperty equipData)
    {
        bool isTrue = AllEquipmentDataList.Remove(equipData);
        if (isTrue == false)
        {
            Debug.LogError("没有找到要使用的装备");
        }
    }
    public void AddEquipmentData(EquipmentRealProperty equipData)
    {
        AllEquipmentDataList.Add(equipData);
    }

    #endregion

    private void GetRoleProperty(Dictionary<RoleAttribute, float> roleProperty, EquipmentData data, int currentLevel)
    {
        float times = (1 + 0.05f * (currentLevel - data.LevelRange.Min));

        CalculateRoleProperty(roleProperty, RoleAttribute.Dodge, data.AvoidHurtRange, times);
        CalculateRoleProperty(roleProperty, RoleAttribute.HP, data.HPRange, times);
        CalculateRoleProperty(roleProperty, RoleAttribute.PArmor, data.PhysicArmorRange, times);
        CalculateRoleProperty(roleProperty, RoleAttribute.MArmor, data.MagicArmorRange, times);
        CalculateRoleProperty(roleProperty, RoleAttribute.HIT, data.HitEnemyRange, times);
        CalculateRoleProperty(roleProperty, RoleAttribute.INT, data.MagicDamageRange, times);
        CalculateRoleProperty(roleProperty, RoleAttribute.Crt, data.CritialRange, times);
        CalculateRoleProperty(roleProperty, RoleAttribute.MinDamage, data.DamageMinRange, times);
        CalculateRoleProperty(roleProperty, RoleAttribute.MaxDamage, data.DamageMaxRange, times);

        if (roleProperty.ContainsKey(RoleAttribute.DPS) == false)
        {
            roleProperty[RoleAttribute.DPS] = 0;
        }

        roleProperty[RoleAttribute.DPS] +=
            ((Random.Range(data.DamageMinRange.Min * times, data.DamageMinRange.Max * times) +
             Random.Range(data.DamageMaxRange.Min * times, data.DamageMaxRange.Max * times)) / attackSpeed);

        RandomPropertyData[] tempDatas = new RandomPropertyData[3];
        for (int i = 0; i < tempDatas.Length; i++)
        {
            tempDatas[i] = data.RandomPropertyDatas[i];
        }

        int loopCount = data.RandomCount;
        while (loopCount > 0)
        {
            int selectId = Random.Range(0, tempDatas.Length);
            if (tempDatas[selectId].AttributeType >= 0)
            {
                loopCount--;
                CalculateRoleProperty(roleProperty, tempDatas[selectId].AttributeType, tempDatas[selectId].ValueRange, times);
                tempDatas[selectId].AttributeType = RoleAttribute.Nothing;
            }
        }
    }

    private void GetSpecialProperty(List<SpecialPropertyData> roleProperty, EquipmentData data)
    {
        for (int i = 0; i < data.SpecialPropertyDatas.Length; i++)
        {
            if (data.SpecialPropertyDatas[i].SpecialPropertyType != SpecialPropertyEnum.None)
            {
                roleProperty.Add(data.SpecialPropertyDatas[i]);
            }
        }
    }

    private void CalculateRoleProperty(Dictionary<RoleAttribute, float> roleProperty, RoleAttribute rolePropertyType, RangeData value, float times)
    {
        if (roleProperty.ContainsKey(rolePropertyType) == false)
        {
            roleProperty[rolePropertyType] = 0;
        }

        roleProperty[rolePropertyType] += Random.Range(value.Min * times, value.Max * times);
    }
}
