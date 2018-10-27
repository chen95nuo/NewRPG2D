using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle;
using UnityEngine;

public class RoleDetailData
{
    public int Id;
    public string Name;
    public int Level;
    public string IconName;
    public ProfessionNeedEnum Profession;
    public List<int> EquipIdList;
    public List<int> BodyIdList;
    public BornPositionTypeEnum BornPositionType;

    public void InitData()
    {
    }
}
