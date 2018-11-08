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
        photo.sprite = GetSpriteAtlas.insatnce.GetIcon(roleData.sexType.ToString());
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
        Vector2 v2 = TTUIRoot.Instance.uiCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(v2, Vector2.zero);
        if (hit.collider != null)
        {
            UIRoleGridMgr roleGrid = hit.collider.GetComponent<UIRoleGridMgr>();
            if (roleGrid != null)
            {
                HallRole role = HallRoleMgr.instance.GetRole(roleData);
                roleGrid.UIAddRole(role);
            }
            else
            {
                UITrainRoleGrid trainRoleGrid = hit.collider.GetComponent<UITrainRoleGrid>();
                if (trainRoleGrid != null)
                {
                    HallRole trainRole = HallRoleMgr.instance.GetRole(roleData);
                    trainRoleGrid.UIAddRole(trainRole);
                }
            }
        }

        ClosePage();
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }
}
