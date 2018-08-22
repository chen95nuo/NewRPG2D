using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardSkillData
{

    [SerializeField]
    private int id;//技能Id

    [SerializeField]
    private string name;//技能名称

    [SerializeField]
    private int level;//技能等级

    [SerializeField]
    private string spriteName;//图片名称

    [SerializeField]
    private string skillDescribe;//技能描述

    public int Id
    {
        get
        {
            return id;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public string SkillDescribe
    {
        get
        {
            return skillDescribe;
        }
    }
}
