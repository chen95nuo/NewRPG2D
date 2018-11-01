using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAtrGrid : MonoBehaviour
{
    public Image icon;
    public Text txt_Tip;

    public void UpdateInfo(RoleAttribute Icon, string tip)
    {
        icon.sprite = GetSpriteAtlas.insatnce.GetIcon(Icon.ToString());
        txt_Tip.text = tip;
    }
}
