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

    public void SetImage(Image[] images)
    {
        StartCoroutine(SetSprite(images, currencyImage));
    }

    public void SetImage(Image[] images, SpriteAtlas sprites)
    {
        StartCoroutine(SetSprite(images, sprites));
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
}
