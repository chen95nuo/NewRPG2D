using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Script.UIManger;

public class UIMapLockGrid : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        object str = "该关卡暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(str);
    }
}
