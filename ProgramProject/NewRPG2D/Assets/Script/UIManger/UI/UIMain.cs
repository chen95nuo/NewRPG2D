using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public Text text_GoldCoin;
    public Text text_Diamonds;
    public Text text_Fatigue;
    public Slider slider_Exp;
    public Text text_Exp;

    private void Awake()
    {
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdatePlayerGoldCoin, UpdateGoldCoin);
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdatePlayerDiamonds, UpdateDiamonds);
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdatePlayerFatigue, UpdateFatigue);
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdatePlayerExp, UpdateExp);
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdatePlayerGoldCoin, UpdateGoldCoin);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdatePlayerDiamonds, UpdateDiamonds);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdatePlayerFatigue, UpdateFatigue);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdatePlayerExp, UpdateExp);
    }

    private void Start()
    {
        Invoke("UpdateUI", 0.5f);
    }

    private void UpdateUI()
    {

        text_GoldCoin.text = GetPlayData.Instance.player[0].GoldCoin.ToString();
        text_Diamonds.text = GetPlayData.Instance.player[0].Diamonds.ToString();
        text_Fatigue.text = GetPlayData.Instance.player[0].Fatigue.ToString();
        PlayerData data = GetPlayData.Instance.player[0];
        text_Exp.text = data.Level.ToString();
        float maxExp = GetPlayerExpData.Instance.GetItem(data.Level).NeedExp;
        slider_Exp.maxValue = maxExp;
        text_Exp.text = "Lv." + data.Level + " / Exp: " + data.Exp;
    }

    private void UpdateGoldCoin()
    {
        text_GoldCoin.text = GetPlayData.Instance.player[0].GoldCoin.ToString();
    }

    private void UpdateDiamonds()
    {
        text_Diamonds.text = GetPlayData.Instance.player[0].Diamonds.ToString();
    }

    private void UpdateFatigue()
    {
        text_Fatigue.text = GetPlayData.Instance.player[0].Fatigue.ToString();
    }

    private void UpdateExp()
    {
        PlayerData data = GetPlayData.Instance.player[0];
        float maxExp = GetPlayerExpData.Instance.GetItem(data.Level).NeedExp;
        slider_Exp.maxValue = maxExp;
        text_Exp.text = "Lv." + data.Level + " / Exp: " + data.Exp;
    }

}
