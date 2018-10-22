using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoxWordBar : MonoBehaviour
{
    public Image image_Icon;
    public Text txt_Tip;

    public void UpdateInfo(string IconName, string tip)
    {
        //Sprite sp = GetSpriteAtlas.insatnce.GetIcon(IconName);
        //image_Icon.sprite = sp;
        txt_Tip.text = tip;
    }
}
