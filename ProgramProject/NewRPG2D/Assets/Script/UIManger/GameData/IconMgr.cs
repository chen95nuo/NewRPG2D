using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class IconMgr : MonoBehaviour
{
    public static IconMgr Instance = null;
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

    }

    public Sprite GetIcon(string name)
    {
        var sprite = iconImage.GetSprite(name);

        if (sprite != null)
        {
            return sprite;
        }
        else return null;
    }


}
