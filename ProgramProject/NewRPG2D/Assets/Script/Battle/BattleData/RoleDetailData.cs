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
    public WeaponProfessionEnum Profession;
    public SexTypeEnum sexType;
    public int[] EquipIdList;
    public List<int> BodyIdList;
    public BornPositionTypeEnum BornPositionType;

    public RoleDetailData()
    {
        EquipIdList = new int[5];
        BodyIdList = new List<int>();
    }

    public void InitData()
    {
    }
}
