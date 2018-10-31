using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoxGrid : MonoBehaviour
{

    public Image icon;
    public Image iconBg;
    public Text txt_Num;
    public GameObject numBG;

    public void UpdateInfo(PropData data, int num = 0)
    {
        icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.SpriteName);
        if (num == 0)
        {
            numBG.SetActive(false);
        }
        else
        {
            numBG.SetActive(true);
            txt_Num.text = num.ToString();
        }
    }

    public void UpdateInfo(EquipmentRealProperty equipData)
    {
        icon.sprite = GetSpriteAtlas.insatnce.GetIcon(equipData.SpriteName);
        numBG.SetActive(false);
    }
}
