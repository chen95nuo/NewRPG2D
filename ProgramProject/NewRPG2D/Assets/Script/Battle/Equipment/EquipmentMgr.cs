using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Assets.Script.Battle.BattleData;
using UnityEngine;

public class EquipmentRealProperty
{
    public int EquipId;
    public EquipTypeEnum EquipType;
    public QualityTypeEnum QualityType;
    public int NeedLevel;
    public int Level;
    public Dictionary<RoleAttribute, PropertyValueRange> RoleProperty;
    public List<int> SpecialProperty;
}

public struct PropertyValueRange
{
    public float MinValue;
    public float MaxValue;

    public PropertyValueRange(float minValue, float maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }
}

public class EquipmentMgr : TSingleton<EquipmentMgr>
{
    private Dictionary<int, EquipmentRealProperty> AllEquipmentData = new Dictionary<int, EquipmentRealProperty>();
    private static int equipInstanceId = 1000;

    public override void Init()
    {
        base.Init();
        AllEquipmentData.Clear();
    }

    public EquipmentRealProperty CreateNewEquipment(int itemId)
    {
        EquipmentData data = EquipmentDataMgr.instance.GetXmlDataByItemId<EquipmentData>(itemId);

        if (data == null)
        {
            return null;
        }

        Dictionary<RoleAttribute, PropertyValueRange> roleProperty = new Dictionary<RoleAttribute, PropertyValueRange>();
        List<int> specialProperty = new List<int>();
        int equipId = equipInstanceId++;
        EquipmentRealProperty realProperty = new EquipmentRealProperty();
        realProperty.EquipId = equipId;
        realProperty.EquipType = data.EquipType;
        realProperty.Level = Random.Range(data.LevelMin, data.LevelMax);
        realProperty.NeedLevel = data.NeedLevel;
        GetRoleProperty(roleProperty, data, realProperty.Level);
        realProperty.RoleProperty = roleProperty;
        GetSpecialProperty(specialProperty, data);
        realProperty.SpecialProperty = specialProperty;
        AllEquipmentData[equipId] = realProperty;
        return realProperty;
    }

    public EquipmentRealProperty GetEquipmentByEquipId(int equipId)
    {
        return AllEquipmentData[equipId];
    }

    public float GetEquipmentValueByEquipIdAndType(int equipId, RoleAttribute type)
    {
        float minValue = AllEquipmentData[equipId].RoleProperty[type].MinValue;
        float maxValue = AllEquipmentData[equipId].RoleProperty[type].MaxValue;

        return Random.Range(minValue, maxValue);
    }

    private void GetRoleProperty(Dictionary<RoleAttribute, PropertyValueRange> roleProperty, EquipmentData data, int currentLevel)
    {
        float minValueTimes = data.DamageMinRange*(1 + 0.05f*(currentLevel - data.LevelMin));
        float maxValueTimes = data.DamageMinRange * (1 + 0.05f * (currentLevel - data.LevelMin));

        CalculateRoleProperty(roleProperty, data.NormalPropertyId1, minValueTimes, maxValueTimes);
        CalculateRoleProperty(roleProperty, data.NormalPropertyId2, minValueTimes, maxValueTimes);
        CalculateRoleProperty(roleProperty, data.NormalPropertyId3, minValueTimes, maxValueTimes);
        int[] RandomPropertyId = new int[]
        {
            data.RandomPropertyId1,
            data.RandomPropertyId2,
            data.RandomPropertyId3,
        };

        int loopCount = data.RandomCount;
        while (loopCount > 0)
        {
            int selectId = Random.Range(0, RandomPropertyId.Length);
            if (RandomPropertyId[selectId] != 0)
            {
                loopCount --;
                CalculateRoleProperty(roleProperty, RandomPropertyId[selectId], minValueTimes, maxValueTimes);
                RandomPropertyId[selectId] = 0;
            }
        }
    }

    private void GetSpecialProperty(List<int> roleProperty, EquipmentData data)
    {
        if (data.SpecialPropertyId1 > 0)
        {
            roleProperty.Add(data.SpecialPropertyId1);
        }

        if (data.SpecialPropertyId2 > 0)
        {
            roleProperty.Add(data.SpecialPropertyId2);
        }
    }

    private void CalculateRoleProperty(Dictionary<RoleAttribute, PropertyValueRange> roleProperty, int propertyId, float minValueTimes, float maxValueTimes)
    {
        if (propertyId <= 0)
        {
            return;
        }

        EquipBasePropertyData baseData =
            EquipBasePropertyDataMgr.instance.GetXmlDataByItemId<EquipBasePropertyData>(propertyId);
        if (baseData == null)
        {
            return;
        }
        roleProperty[baseData.RolePropertyType] = new PropertyValueRange(baseData.BaseValue * minValueTimes, baseData.BaseValue * maxValueTimes);
    }
}
