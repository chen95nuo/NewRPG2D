using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class GetSpriteAtlas : MonoBehaviour
{

    public static GetSpriteAtlas insatnce = null;

    public SpriteAtlas Icons;

    private void Awake()
    {
        insatnce = this;
    }

    public Sprite GetIcon(string name)
    {
        Sprite sp = Icons.GetSprite(name);
        if (sp != null)
        {
            return sp;
        }
        return null;
    }

    public void SetImage(Image[] images)
    {
        StartCoroutine(SetSprite(images, Icons));
    }

    public void SetImage(Image[] images, SpriteAtlas sprites)
    {
        StartCoroutine(SetSprite(images, sprites, Icons));
    }

    IEnumerator SetSprite(Image[] images, SpriteAtlas currencyImage)
    {
        yield return images;

        foreach (var item in images)
        {
            var sprite = currencyImage.GetSprite(item.name);
            if (sprite != null)
            {
                item.sprite = sprite;
            }
        }
    }

    IEnumerator SetSprite(Image[] images, SpriteAtlas currentImages, SpriteAtlas currencyImage)
    {
        yield return images;

        foreach (var item in images)
        {
            var sprite = currencyImage.GetSprite(item.name);
            var sprite_2 = currentImages.GetSprite(item.name);
            if (sprite != null)
            {
                item.sprite = sprite;
                Debug.Log("1");
            }
            else if (sprite_2 != null)
            {
                item.sprite = sprite_2;
                Debug.Log("2");
            }
        }
    }
}
