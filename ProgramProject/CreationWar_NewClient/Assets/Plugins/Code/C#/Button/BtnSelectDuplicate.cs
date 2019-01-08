using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnSelectDuplicate : MonoBehaviour
{
    public UILabel lblName;
    public UIToggle mCheck;
    public UIGrid grid;
    public string strLevel = string.Empty;
    public List<GameObject> listLevelBtn;





    void OnClick()
    {
        foreach (GameObject item in listLevelBtn)
        {
            item.SetActiveRecursively(false);
        }
        if (strLevel != string.Empty)
        {
            for (int i = 0; i < int.Parse(strLevel); i++)
            {
                listLevelBtn[i].SetActiveRecursively(true);
            }

        }
        grid.repositionNow = true;
    }
    



}
