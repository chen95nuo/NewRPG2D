using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMarket : MonoBehaviour
{
    public Button btn_back;

    public UIMain main;

    private void Awake()
    {
        btn_back.onClick.AddListener(ClosePage);
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void ClosePage()
    {
        main.CloseSomeUI(true);
        this.gameObject.SetActive(false);
    }
}
