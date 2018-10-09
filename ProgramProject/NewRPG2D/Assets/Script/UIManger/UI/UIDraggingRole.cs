/*
 * 控制抓住角色
 * 若被抓住且不放手则跟着手走
 * 若放手则检查 若指向房间则进入房间 
 * 若指向其他位置则回到原位
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIDraggingRole : TTUIPage
{
    private Canvas canvas;

    public Image hand;

    private HallRole role;
    private Vector3 originPoint;

    private void Awake()
    {
        canvas = TTUIRoot.Instance.gameObject.GetComponent<Canvas>();
    }

    private void OnDisable()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int layerMask = 1 << 8;
        int layerName = LayerMask.NameToLayer("Room");
        int layer = LayerMask.GetMask("Room");
        //layerMask = ~layerMask;
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        { Debug.Log(hit.collider.name); }
        if (hit.collider == null)
        {
            //放回原处
            role.transform.position = originPoint;
        }
        else if (hit.collider.tag == "Room")
        {
            RoomMgr room = hit.collider.GetComponent<RoomMgr>();
            room.AddRole(role);
        }
        CameraControl.instance.isHoldRole = false;
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        role = mData as HallRole;
        originPoint = role.transform.position;
    }

    private void Update()
    {
        Vector2 _pos = Vector2.one;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                    Input.mousePosition, canvas.worldCamera, out _pos);
        hand.rectTransform.anchoredPosition = _pos;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = role.transform.position.z;
        role.transform.position = worldPos;
    }
}
