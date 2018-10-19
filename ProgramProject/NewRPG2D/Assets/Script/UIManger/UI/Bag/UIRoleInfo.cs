using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleInfo : TTUIPage
{

    public GameObject Train;
    public GameObject LoveTip;

    #region GetText
    public Text txt_TrainTime;
    public Text txt_HateLoveTime;

    public Text txt_fight;
    public Text txt_food;
    public Text txt_gold;
    public Text txt_mana;
    public Text txt_wood;
    public Text txt_iron;

    public Text txt_hurtType;
    public Text txt_hurtTip;
    public Text txt_Dps;
    public Text txt_DpsTip;
    public Text txt_PArmor;
    public Text txt_PArmorTip;
    public Text txt_MArmor;
    public Text txt_MArmorTip;
    public Text txt_Dodge;
    public Text txt_DodgeTip;
    public Text txt_Hit;
    public Text txt_HitTip;
    public Text txt_INT;
    public Text txt_INTTip;
    public Text txt_Hp;
    #endregion

    #region GetButton
    public Button btn_back;
    public Button btn_AllItem;
    public Button btn_Weapon;
    public Button btn_Armor;
    public Button btn_Jewelry;
    public Button btn_Box;
    public Button btn_Prop;
    public Button btn_Star;
    #endregion



    private void Awake()
    {
        btn_back.onClick.AddListener(ClosePage);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        HallRoleData data = mData as HallRoleData;
        UpdateInfo(data);
        ChickLevelUI(false);
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

        txt_hurtTip.text = ((RoleHurtType)data.HurtType).ToString();
        txt_hurtType.text = data.Attack.ToString();
        ChickAtr(data);//检查属性

        if (data.LoveType == RoleLoveType.boredom)
        {
            LoveTip.SetActive(true);
        }
        else
        {
            LoveTip.SetActive(false);
        }

        if (data.TrainType == RoleTrainType.LevelUp)
        {
            Train.SetActive(true);
        }
        else
        {
            Train.SetActive(false);
        }

        txt_Hp.text = data.NowHp + "/" + data.Health;
    }

    /// <summary>
    /// 检查等级UI显示数量
    /// </summary>
    /// <param name="isTrue"></param>
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


    public void ChickAtr(HallRoleData data)
    {
        if (data.DPS > 0)
        {
            txt_Dps.text = data.DPS.ToString();
        }
        else
        {
            txt_Dps.transform.parent.gameObject.SetActive(false);
        }
        if (data.PArmor > 0)
        {
            txt_PArmor.text = data.PArmor.ToString();
        }
        else
        {
            txt_PArmor.transform.parent.gameObject.SetActive(false);
        }
        if (data.MArmor > 0)
        {
            txt_MArmor.text = data.MArmor.ToString();
        }
        else
        {
            txt_MArmor.transform.parent.gameObject.SetActive(false);
        }
        if (data.Dodge > 0)
        {
            txt_Dodge.text = data.Dodge.ToString();
        }
        else
        {
            txt_Dodge.transform.parent.gameObject.SetActive(false);
        }
        if (data.HIT > 0)
        {
            txt_Hit.text = data.Dodge.ToString();
        }
        else
        {
            txt_Hit.transform.parent.gameObject.SetActive(false);
        }
        if (data.INT > 0)
        {
            txt_INT.text = data.INT.ToString();
        }
        else
        {
            txt_INT.transform.parent.gameObject.SetActive(false);
        }
    }
}
