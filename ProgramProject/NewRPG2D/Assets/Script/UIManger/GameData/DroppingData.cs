using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingData
{

    private int id;
    private string spriteName;//道具的图片名称
    private int quality;//道具品质


    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public int Quality
    {
        get
        {
            return quality;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }
    }

    public DroppingData() { }
    public DroppingData(int id, string spriteName, int quality)
    {
        this.id = id;
        this.spriteName = spriteName;
        this.quality = quality;
    }
}
