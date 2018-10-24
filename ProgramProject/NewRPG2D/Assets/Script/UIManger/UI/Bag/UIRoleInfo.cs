using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    public Text txt_Dps;
    public Text txt_DpsTip;
    public Text txt_CrtType;
    public Text txt_CrtTip;
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

    public Button btn_back;
    #region Bag
    public Button[] btn_AllType;
    public ScrollControl sc;
    #endregion

    private int currentbtnNumb = 0;

    private void Awake()
    {
        btn_back.onClick.AddListener(ClosePage);
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            btn_AllType[i].onClick.AddListener(ChickBagType);
        }
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

        txt_Dps.text = ((RoleHurtType)data.HurtType).ToString();
        txt_DpsTip.text = string.Format("{0}伤害/秒", data.HurtType);
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

    private void ChickBagType()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            if (btn_AllType[i].gameObject == go)
            {
                btn_AllType[currentbtnNumb].interactable = true;
                btn_AllType[i].interactable = false;
                sc.UpdateInfo((BagType)i);
                currentbtnNumb = i;
                return;
            }
        }
        Debug.LogError("没有找到对应按钮");
    }
}
