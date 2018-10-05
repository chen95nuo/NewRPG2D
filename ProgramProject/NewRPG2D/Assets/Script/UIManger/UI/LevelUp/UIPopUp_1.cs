using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUp_1 : TTUIPage
{
    public static UIPopUp_1 instance;

    public Text txt_Gold;
    public Text txt_Food;
    public Text txt_Mana;
    public Text txt_Wood;
    public Text txt_Iron;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;

    private void Awake()
    {
        instance = this;
    }

    private void UpdateInfo()
    {

    }
}
