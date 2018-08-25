using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LessonData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string name;
    [SerializeField]
    private string spriteName;
    [SerializeField]
    private string fightSceneId;

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

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public float[] FightSceneId
    {
        get
        {
            float[] id = UIFormatString.Instance.FormatDataString(fightSceneId);
            return id;
        }
    }
}
