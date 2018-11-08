using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorkShopTypeGrid : MonoBehaviour
{
    public Button btn_Type;
    public Text txt_Name;
    public Image icon;

    public void UpdateInfo(BuildRoomName name, int index)
    {
        icon.sprite = GetSpriteAtlas.insatnce.GetIcon(name.ToString() + index);
        txt_Name.text = LanguageDataMgr.instance.GetString(name.ToString() + index);
    }
}
