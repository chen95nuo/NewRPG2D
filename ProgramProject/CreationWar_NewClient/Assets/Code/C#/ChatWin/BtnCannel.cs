using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnCannel : MonoBehaviour {


    public List<UIToggle> listCheckBox;
    public List<GameObject> listCharBar;

    void Start()
    {
        foreach (GameObject cb in listCharBar)
        {
            cb.SetActive(false);
        }
    }

    void OnClick()
    {
        int num = 0;
        foreach (GameObject cb in listCharBar)
        {
            cb.SetActive(false);
        }
        foreach (UIToggle cbx in listCheckBox)
        {
            if (cbx.isChecked)
            {
                listCharBar[num].SetActive(true);
                break;
            }
            num++;
        }
    }
}
