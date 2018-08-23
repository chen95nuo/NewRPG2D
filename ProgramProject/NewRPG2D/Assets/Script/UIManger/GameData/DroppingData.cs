using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingData
{

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

    public DroppingData() { }
    public DroppingData(string spriteName, int quality)
    {
        this.spriteName = spriteName;
        this.quality = quality;
    }
}
