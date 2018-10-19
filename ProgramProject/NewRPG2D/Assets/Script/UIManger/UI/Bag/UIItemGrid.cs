using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemGrid : MonoBehaviour
{
    private Button btn_Click;
    public Image image_Item;

    private void Awake()
    {
        btn_Click.onClick.AddListener(ChickClick);
    }

    public void UpdateInfo(EquipmentRealProperty EquipData)
    {

    }

    public void UpdateInfo() { }

    public void ChickClick()
    {
        Debug.Log("show装备简介");
    }
}
