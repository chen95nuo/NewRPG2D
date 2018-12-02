using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIBarracksInfo : UIRoomInfo
{
    public Text txt_AllFight;
    public Text txt_Tip_1;

    private List<UIBarrcksGrid> roleGrids = new List<UIBarrcksGrid>();
    private int UnlockIndex = 0;

    protected override void ChickShowScreenRole()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < UnlockIndex; i++)
        {
            if (btn_ScreenRole[i].gameObject == go)
            {
                btn_ScreenRole[currentBtn].interactable = true;
                btn_ScreenRole[i].interactable = false;
                currentBtn = i;
                if (!IsShow)
                {
                    IsShow = true;
                }
                return;
            }
        }
    }

    protected override void UpdateInfo(LocalBuildingData roomData)
    {
        txt_AllFight.text = "19999";
        txt_Tip_1.text = "战斗力";

        ChickRoleNumber(roleGrids);
        UnlockIndex = (int)roomData.buildingData.Param2;
        for (int i = UnlockIndex; i < roleGrids.Count; i++)
        {
            roleGrids[i].UpdateLockInfo(this, i);
        }
    }
}
