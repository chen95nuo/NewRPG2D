using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUp_1 : TTUIPage
{
    public Text txt_Gold;
    public Text txt_Mana;
    public Text txt_Wood;
    public Text txt_Iron;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_DiaNum;
    public Button btn_enter;
    public Button btn_back;

    public bool isStockFull = false;//仓库满了
    private Dictionary<MaterialName, int> needStock;

    private int needDiaNumber = 0;

    private void Awake()
    {
        ChickAllTxt(false);

        txt_Tip_1.text = LanguageDataMgr.instance.GetUIString("goumaisuoxuwuzi");
        txt_Tip_2.text = LanguageDataMgr.instance.GetUIString("goumai");
        txt_Tip_3.text = LanguageDataMgr.instance.GetUIString("quxiao");

        btn_enter.onClick.AddListener(ChickEnter);
        btn_back.onClick.AddListener(ClosePage);
    }

    private void OnDisable()
    {
        ChickAllTxt(false);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        Dictionary<MaterialName, int> needStock = mData as Dictionary<MaterialName, int>;
        UpdateInfo(needStock);
    }

    public void UpdateInfo(Dictionary<MaterialName, int> needStock)
    {
        this.needStock = needStock;
        float diamonds = needStock[MaterialName.Diamonds];

        PlayerData player = GetPlayerData.Instance.GetData();
        needDiaNumber = (int)(player.Diamonds - diamonds);
        if (needDiaNumber > 0)
        {
            txt_DiaNum.text = diamonds.ToString();
        }
        else
        {
            txt_DiaNum.text = "<color=#ee5151>" + diamonds.ToString() + "</color>";
        }
    }

    private void ChickAllTxt(bool isTrue)
    {
        txt_Gold.transform.parent.gameObject.SetActive(isTrue);
        txt_Mana.transform.parent.gameObject.SetActive(isTrue);
        txt_Wood.transform.parent.gameObject.SetActive(isTrue);
        txt_Iron.transform.parent.gameObject.SetActive(isTrue);
    }

    private void ChickEnter()
    {
        if (needDiaNumber < 0)
        {
            isStockFull = false;
            needDiaNumber = -needDiaNumber;
            UIPanelManager.instance.ShowPage<UIPopUP_5>(needDiaNumber);
        }
        else
        {
            //向服务器发送 消耗钻石购买建材
            Debug.Log("向服务器发送消耗钻石购买材料"); 
        }
        ClosePage();
    }


    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }
}
