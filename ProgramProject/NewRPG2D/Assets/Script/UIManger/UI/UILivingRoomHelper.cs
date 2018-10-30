using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILivingRoomHelper : MonoBehaviour
{
    public GameObject type_1;
    public GameObject type_2;
    public Text txt_Tip;
    public Text txt_time;

    public Image slider;

    public void UpdateInfo()
    {
        type_1.SetActive(false);
        type_2.SetActive(false);
        txt_Tip.text = "";
    }
    public void UpdateInfo(float index, string st)
    {
        type_1.SetActive(true);

        txt_time.text = st;
        slider.fillAmount = index;
    }
    public void UpdateDontNothing()
    {
        type_2.SetActive(true);
        txt_Tip.text = "什么也不会发生";
    }
}
