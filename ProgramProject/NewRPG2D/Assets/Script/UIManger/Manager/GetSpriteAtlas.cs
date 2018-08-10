using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class GetSpriteAtlas : MonoBehaviour
{

    public static GetSpriteAtlas insatnce = null;

    private SpriteAtlas currencyImage;

    private void Awake()
    {
        insatnce = this;
        currencyImage = Resources.Load<SpriteAtlas>("UISpriteAtlas/CurrencyImage");
    }

    public Sprite GetIcon(string name)
    {
        Sprite sp = currencyImage.GetSprite(name);
        if (sp != null)
        {
            return sp;
        }
        return null;
    }
    public void SetImage(Image[] images)
    {
        StartCoroutine(SetSprite(images, currencyImage));
    }

    public void SetImage(Image[] images, SpriteAtlas sprites)
    {
        StartCoroutine(SetSprite(images, sprites, currencyImage));
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
