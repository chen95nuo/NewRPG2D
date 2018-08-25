using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LessonFightData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private int name;
    [SerializeField]
    private string spriteName;
    [SerializeField]
    private string description;//描述
    [SerializeField]
    private int needFatigue;//需要体力
    [SerializeField]
    private string dropID;//显示的掉落信息
    [SerializeField]
    private int dropBoxID;//掉落包ID

    public int Id
    {
        get
        {
            return id;
        }
    }

    public int Name
    {
        get
        {
            return name;
        }
    }

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public int NeedFatigue
    {
        get
        {
            return needFatigue;
        }
    }

    public string DropID
    {
        get
        {
            return dropID;
        }
    }

    public int DropBoxID
    {
        get
        {
            return dropBoxID;
        }
    }
}
