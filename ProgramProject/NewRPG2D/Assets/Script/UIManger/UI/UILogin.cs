using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using UnityEngine.SceneManagement;
using System;

public class UILogin : MonoBehaviour
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

    private void ChickLogin()
    {
        SceneManager.LoadScene("EasonMainScene");
    }
}
