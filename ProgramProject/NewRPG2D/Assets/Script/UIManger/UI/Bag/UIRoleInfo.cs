using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleInfo : TTUIPage
{
    public GameObject Train;
    public GameObject LoveTip;

    public Text txt_TrainTime;
    public Text txt_HateLoveTime;

    public Text txt_fight;
    public Text txt_food;
    public Text txt_gold;
    public Text txt_mana;
    public Text txt_wood;
    public Text txt_iron;

    public Text txt_hurtType;
    public Text txt_Dps;
    public Text txt_PArmor;
    public Text txt_MArmor;
    public Text txt_Dodge;
    public Text txt_Hit;
    public Text txt_INT;
    public Text txt_Hp;

    public override void Show(object mData)
    {
        base.Show(mData);
        HallRoleData data = mData as HallRoleData;
        UpdateInfo(data);
    }

    public void UpdateInfo(HallRoleData data)
    {
        txt_fight.text = data.FightLevel.ToString();
        txt_food.text = data.FoodLevel.ToString();
        txt_gold.text = data.GoldLevel.ToString();
        txt_mana.text = data.ManaLevel.ToString();
        txt_wood.text = data.WoodLevel.ToString();
        txt_iron.text = data.IronLevel.ToString();
        ChickLevelUI(true);

        txt_hurtType.text = ((RoleHurtType)data.HurtType).ToString();
        if (data.DPS > 0)
        {
            txt_Dps.text = data.DPS.ToString();
        }
        if (data.PArmor > 0)
        {
            txt_PArmor.text = data.PArmor.ToString();
        }
        if (data.MArmor > 0)
        {
            txt_MArmor.text = data.MArmor.ToString();
        }
        if (data.Dodge > 0)
        {
            txt_Dodge.text = data.Dodge.ToString();
        }
        if (data.HIT > 0)
        {
            txt_Hit.text = data.Dodge.ToString();
        }
        if (data.INT > 0)
        {
            txt_INT.text = data.INT.ToString();
        }
        txt_Hp.text = data.nowHp + "/" + data.HP;
    }

    public void ChickLevelUI(bool isTrue)
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        if (playerData.MainHallLevel > 4 || isTrue == false)
        {
            txt_mana.transform.parent.gameObject.SetActive(isTrue);
        }
        if (playerData.MainHallLevel >= 6 || isTrue == false)
        {
            txt_wood.transform.parent.gameObject.SetActive(isTrue);
        }
        if (playerData.MainHallLevel >= 9 || isTrue == false)
        {
            txt_iron.transform.parent.gameObject.SetActive(isTrue);
        }
    }
}
