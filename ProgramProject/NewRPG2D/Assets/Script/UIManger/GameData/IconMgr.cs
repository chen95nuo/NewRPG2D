using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconMgr : MonoBehaviour
{
    public static IconMgr Instance = null;
    Dictionary<string, Sprite> icon = new Dictionary<string, Sprite>();

    public Sprite[] sprites;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        sprites = Resources.LoadAll<Sprite>("UITexture/Icon");

        for (int i = 0; i < sprites.Length; i++)
        {
            icon.Add(sprites[i].name, sprites[i]);
        }
    }

    public Sprite GetIcon(string name)
    {
        if (icon[name] == null)
        {
            return null;
        }
        return icon[name];
    }


}
