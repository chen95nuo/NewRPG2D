using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILogin : TTUIPage
{

    public Button btn_Login;
    public GameObject Load;
    public Slider slider;
    public Text txt_LoadNum;

    private int index;
    private bool isRun = false;

    private void Awake()
    {
        slider.value = 0;
        btn_Login.onClick.AddListener(ChickLogin);
    }

    private void OnEnable()
    {
        Load.SetActive(false);
    }

    private void Update()
    {
        if (isRun)
        {
            if (slider.value < 97)
            {
                slider.value += Time.deltaTime * 80;
            }
            else
            {
                slider.value += Time.deltaTime * 5;
            }
            txt_LoadNum.text = slider.value.ToString("#0") + "%";
            if (slider.value == slider.maxValue)
            {
                isRun = false;
                ClosePage();
                UIPanelManager.instance.ShowPage<UIMain>();
            }
        }
    }

    private void ChickLogin()
    {
        Load.SetActive(true);
        isRun = true;
    }
}
