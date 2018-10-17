using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicGrid : MonoBehaviour
{
    public Text txt_Tip_1;
    public Text txt_MagicLevel;
    public Button btn_LevelUp;

    private void Awake()
    {
        btn_LevelUp = GetComponent<Button>();
    }
    public void UpdateInfo(MagicData data, int roomLevel)
    {
        if (roomLevel == 0)
        {
            txt_MagicLevel.gameObject.SetActive(false);
            btn_LevelUp.interactable = false;
            txt_Tip_1.gameObject.SetActive(false);
        }
        txt_MagicLevel.gameObject.SetActive(true);
        txt_MagicLevel.text = (data.level - 1).ToString();
        int needLevel = data.needLevel;
        if (roomLevel >= needLevel)
        {
            btn_LevelUp.interactable = true;
        }
        else
        {
            btn_LevelUp.interactable = false;
        }
        if (needLevel > 0)
        {
            txt_Tip_1.gameObject.SetActive(true);
            string tip = string.Format("需要{0}级房间", needLevel);
            txt_Tip_1.text = tip;
            return;
        }
        txt_Tip_1.gameObject.SetActive(false);
    }
}
