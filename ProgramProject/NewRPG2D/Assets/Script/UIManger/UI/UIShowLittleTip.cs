using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TinyTeam.UI;

public class UIShowLittleTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public RoleAtrType type;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        if (type != RoleAtrType.Nothing)
        {
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, type);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (Input.GetMouseButton(0))
        {
            Debug.Log("enter");
            if (type != RoleAtrType.Nothing)
            {
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, type);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, false);
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Exit");
            TTUIPage.ClosePage<UILittleTipPage>();
        }
    }

}
