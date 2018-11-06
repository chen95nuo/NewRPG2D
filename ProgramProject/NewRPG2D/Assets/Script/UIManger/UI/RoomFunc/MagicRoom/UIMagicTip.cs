using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicTip : MonoBehaviour
{
    public Button btn_Message;
    public Button btn_Add;
    public Button btn_Remove;
    public Button btn_Change;
    public Button btn_Cancel;
    public Button btn_SpeedUp;

    private RectTransform rt;

    public Text txt_SpeedUpTip;

    private RealMagic currentGrid;

    private void Awake()
    {
        rt = transform.transform as RectTransform;
        btn_Message.onClick.AddListener(ChickMessage);
        btn_Add.onClick.AddListener(ChickAdd);
        btn_Remove.onClick.AddListener(ChickRemove);
        btn_Change.onClick.AddListener(ChickChange);
        btn_Cancel.onClick.AddListener(ChickCancel);
        btn_SpeedUp.onClick.AddListener(ChickSpeedUp);
    }

    public void ShowPage(int type, RealMagic grid)
    {
        currentGrid = grid;
        btn_Message.gameObject.SetActive(true);
        switch (type)
        {
            case 0:
                btn_Add.gameObject.SetActive(true);
                break;
            case 1:
                btn_Remove.gameObject.SetActive(true);
                break;
            case 2:
                btn_Change.gameObject.SetActive(true);
                break;
            case 3:
                btn_Cancel.gameObject.SetActive(true);
                txt_SpeedUpTip.text = "";
                btn_SpeedUp.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void RemovePage()
    {
        btn_Message.gameObject.SetActive(false);
        btn_Add.gameObject.SetActive(false);
        btn_Remove.gameObject.SetActive(false);
        btn_Change.gameObject.SetActive(false);
        btn_Cancel.gameObject.SetActive(false);
        btn_SpeedUp.gameObject.SetActive(false);
    }

    private void ChickSpeedUp()
    {

    }

    private void ChickCancel()
    {

    }

    private void ChickChange()
    {

    }

    private void ChickRemove()
    {
        MagicDataMgr.instance.UnloadMagic(currentGrid.magicID);
    }

    private void ChickAdd()
    {

    }

    private void ChickMessage()
    {

    }



}
