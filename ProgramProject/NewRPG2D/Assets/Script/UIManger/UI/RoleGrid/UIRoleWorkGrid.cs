using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleWorkGrid : UIRoleGridMgr
{
    public Text txt_Tip;

    protected override void ShowLevelUp(bool isShow, HallRoleData role = null)
    {
        if (isShow)
        {
            txt_Level.gameObject.SetActive(true);
            txt_Level.text = role.GetAtrLevel(RoleAttribute.Mana).ToString();
            txt_Tip.gameObject.SetActive(true);
            txt_Tip.text = role.GetAtrProduce(RoleAttribute.ManaSpeed).ToString();
        }
        else
        {
            txt_Level.gameObject.SetActive(false);
            txt_Tip.gameObject.SetActive(false);
        }
    }

}
