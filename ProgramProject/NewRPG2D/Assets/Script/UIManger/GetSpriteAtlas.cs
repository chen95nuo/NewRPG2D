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

    /// <summary>
    /// 获取房间内角色对应ICon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite ChickRoomIcon(BuildRoomName name)
    {
        string st = "";
        switch (name)
        {
            case BuildRoomName.Nothing:
                break;
            case BuildRoomName.Gold:
                st = "Mint";
                break;
            case BuildRoomName.Food:
                st = "Kitchen";
                break;
            case BuildRoomName.Mana:
                st = "Laboratory";
                break;
            case BuildRoomName.Wood:
                st = "Crafting";
                break;
            case BuildRoomName.Iron:
                st = "Foundry";
                break;
            case BuildRoomName.FighterRoom:
                st = "Fight";
                break;
            case BuildRoomName.Mint:
                st = "Mint";
                break;
            case BuildRoomName.Kitchen:
                st = "Kitchen";
                break;
            case BuildRoomName.Laboratory:
                st = "Laboratory";
                break;
            case BuildRoomName.Crafting:
                st = "Crafting";
                break;
            case BuildRoomName.Foundry:
                st = "Foundry";
                break;
            case BuildRoomName.Barracks:
                st = "Fight";
                break;
            default:
                break;
        }
        return GetIcon(st);
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
