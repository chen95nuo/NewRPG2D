using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrainRoleGrid : MonoBehaviour
{

    public GameObject[] LevelUpType;

    public Text txt_name;
    public Text txt_level;
    public Text txt_Damionds;
    public Text txt_Time;
    public Text txt_Tip;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Button btn_LevelUp;
    public Button btn_SpeedUp;

    public GameObject info;

    private void Awake()
    {
        btn_LevelUp.onClick.AddListener(ChickLevelUp);
        btn_SpeedUp.onClick.AddListener(ChickSpeedUp);
    }

    public void UpdateInfo()
    {
        info.SetActive(false);
    }
    public void UpdateInfo(HallRoleData data)
    {
        info.SetActive(true);
        txt_name.text = data.Name;
    }

    public void UpdateTime()
    {

    }


    public void ChickLevelUp() { }
    public void ChickSpeedUp() { }
}
