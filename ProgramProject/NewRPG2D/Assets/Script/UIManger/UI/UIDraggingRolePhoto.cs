using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using UnityEngine.EventSystems;

public class UIDraggingRolePhoto : TTUIPage
{
    private Canvas canvas;

    public RectTransform handTs;
    public Image photo;

    private HallRoleData roleData;
    private RoomMgr currentRoom;

    private void Awake()
    {
        canvas = TTUIRoot.Instance.gameObject.GetComponent<Canvas>();
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        roleData = mData as HallRoleData;
        MouseMove();
    }

    private void Update()
    {
        MouseMove();

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            ChickBtnUp();
        }
    }

    private void MouseMove()
    {
        Vector2 _pos = Vector2.one;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                    Input.mousePosition, canvas.worldCamera, out _pos);
        handTs.anchoredPosition = _pos;
    }

    private void ChickBtnUp()
    {
        UIScreenRole.instance.sr.horizontal = true;
        Ray2D ray = new Ray2D(handTs.transform.position, handTs.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            UIRoleGrid roleGrid = hit.collider.GetComponent<UIRoleGrid>();
            if (roleGrid != null)
            {
                HallRole role = HallRoleMgr.instance.GetRole(roleData);
                roleGrid.UIAddRole(role);
            }
        }

        ClosePage();
    }


}
