using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChickItem : MonoBehaviour
{
    public Text txt_Name;
    public Text txt_Quality;
    public Text txt_MainAtr;
    public Text txt_MainAtrTip;
    public Text txt_Hurt;
    public Text txt_HurtTip;
    public Text txt_AtkSpeed;
    public Text txt_NeedType;
    public Text txt_NeedLevel;

    public Button btn_UnLoad;
    public Button btn_Salv;
    public Button btn_Equip;

    public GameObject TipInfo;
    public Transform TipInfoPoint;

    public GameObject UpInfoGroup; //道具没有装备属性
    public GameObject ButtonGroup; //道具和双装备查看没有按钮

    private void Awake()
    {
        btn_UnLoad.onClick.AddListener(ChickUnload);
        btn_Salv.onClick.AddListener(ChickSalv);
        btn_Equip.onClick.AddListener(ChickEquip);
    }

    private void ChickUnload() { }
    private void ChickSalv() { }
    private void ChickEquip() { }
}
