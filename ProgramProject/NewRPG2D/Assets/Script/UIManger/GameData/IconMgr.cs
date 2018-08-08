using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class IconMgr : MonoBehaviour
{
    public static IconMgr Instance = null;
    //Dictionary<string, Sprite> icon = new Dictionary<string, Sprite>();
    SpriteAtlas iconImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        iconImage = Resources.Load<SpriteAtlas>("UISpriteAtlas/IconImage");

        Sprite[] sprites = new Sprite[iconImage.spriteCount];
        iconImage.GetSprites(sprites);

        //for (int i = 0; i < sprites.Length; i++)
        //{
        //    icon.Add(sprites[i].name, sprites[i]);
        //}
    }

    public Sprite GetIcon(string name)
    {
        var sprite = iconImage.GetSprite(name);
        //if (icon[name + "(Clone)"] == null)
        //{
        //    return null;
        //}
        //return icon[name + "(Clone)"];
        if (sprite != null)
        {
            return sprite;
        }
        else return null;
    }


}
