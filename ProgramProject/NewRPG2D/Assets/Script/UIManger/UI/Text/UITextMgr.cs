using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UITextMgr : MonoBehaviour
{
    public Button btn_Button;

    public string str;


    private void Awake()
    {
        btn_Button = GetComponent<Button>();
        btn_Button.onClick.AddListener(ChickBtn);
    }

    public virtual void UpdateInfo()
    {

    }

    public virtual void ChickBtn()
    {

    }

}
