using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string name;
    [SerializeField]
    private string spriteName;
    [SerializeField]
    private string lessonsId;

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

    public float[] LessonsId
    {
        get
        {
            float[] id = UIFormatString.Instance.FormatDataString(lessonsId);

            return id;
        }
    }
}
