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
        if (type != RoleAtrType.Nothing)
        {
            TTUIPage.ShowPage<UILittleTipPage>();
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, type);
        }
        else
        {
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.gameObject);
        }
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("enter");
            if (type != RoleAtrType.Nothing)
            {
                TTUIPage.ShowPage<UILittleTipPage>();
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, type);

            }
            else
            {
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.gameObject);
            }
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);
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
