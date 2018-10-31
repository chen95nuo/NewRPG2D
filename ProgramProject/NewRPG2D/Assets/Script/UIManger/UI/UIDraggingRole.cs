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
using DG.Tweening;

public class UIDraggingRole : TTUIPage
{
    private Canvas canvas;

    public RectTransform handTs;
    public Image hand;
    public Sprite[] roleSp;
    public Text txt_Tip;
    public Image Icon;

    private HallRole role;
    private RoomMgr currentRoom;

    private void Awake()
    {
        canvas = TTUIRoot.Instance.gameObject.GetComponent<Canvas>();
    }

    private void OnDisable()
    {
        if (currentRoom != null)
        {
            currentRoom.ShowRoomLockUI(false);
        }

        hand.rectTransform.anchoredPosition = Vector2.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int layerMask = 1 << 8;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Room")
            {
                RoomMgr room = hit.collider.GetComponent<RoomMgr>();
                if (role.RoleData.currentRoom == null || room.Id != role.RoleData.currentRoom.Id)
                {
                    if (room.RoomName == BuildRoomName.Stairs)
                    {
                        object st = "该处为楼梯无法进入";
                        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
                    }
                    else
                    {
                        room.AddRole(role);
                    }
                }
            }
            else
            {
                object st = "请将角色放置正确位置";
                UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
            }
        }
        CameraControl.instance.isHoldRole = false;
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        role = mData as HallRole;
        MouseMove();
        hand.rectTransform.DOAnchorPos(new Vector2(0, -50), 0.5f).From();
        txt_Tip.text = "";
        Icon.gameObject.SetActive(false);
    }

    private void Update()
    {
        MouseMove();
        ChickRay();
    }

    private void MouseMove()
    {
        Vector2 _pos = Vector2.one;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                    Input.mousePosition, canvas.worldCamera, out _pos);
        handTs.anchoredPosition = _pos;
    }

    private void ChickRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            if (hit.collider.tag == "Room")
            {
                RoomMgr room = hit.collider.GetComponent<RoomMgr>();
                if (currentRoom != null && room.Id == currentRoom.Id)
                {
                    return;
                }
                Debug.Log(hit.collider.name);
                bool isTrue = ChickPlayerInfo.instance.ChickProduction(room.currentBuildData);
                if (isTrue)
                {
                    RoleAttribute roleAtr = room.NeedAttribute;
                    int index = room.currentBuildData.ScreenAllYeild(roleAtr, false);
                    float p_1 = role.RoleData.GetArtProduce(room.RoomName);

                    string st = ChicttxtColorOrIcon(room.RoomName);
                    string stColor = "";
                    switch (room.RoomName)
                    {
                        case BuildRoomName.Gold:
                            stColor = "<color=#f5d835>";
                            break;
                        case BuildRoomName.Food:
                            stColor = "<color=#eda160>";
                            break;
                        case BuildRoomName.Mana:
                            stColor = "<color=#c415d0>";
                            break;
                        case BuildRoomName.Wood:
                            stColor = "<color=#be7f27>";
                            break;
                        case BuildRoomName.Iron:
                            stColor = "<color=#9fbdd7>";
                            break;
                        default:
                            break;
                    }

                    Icon.gameObject.SetActive(true);
                    if (index >= 0)
                    {

                        float p_2 = room.currentBuildData.roleData[index].GetArtProduce(room.RoomName);
                        float p_3 = p_1 - p_2;
                        if (p_3 >= 0)
                        {
                            txt_Tip.text = st + stColor + "+" + (p_3).ToString() + "</color>";
                        }
                        else
                        {
                            txt_Tip.text = p_3.ToString();
                        }
                    }
                    else
                    {
                        txt_Tip.text = st + stColor + "+" + p_1.ToString() + "</color>";
                    }
                }
                else
                {
                    Icon.gameObject.SetActive(false);
                    txt_Tip.text = "";
                }
                if (currentRoom != null)
                {
                    currentRoom.ShowRoomLockUI(false);
                }
                currentRoom = room;
                if (role.RoleData.currentRoom == room)
                {
                    room.ShowRoomLockUI(true, true);
                }
                else
                {
                    room.ShowRoomLockUI(true);
                }
            }
        }
        else
        {
            Icon.gameObject.SetActive(false);
            txt_Tip.text = "";
        }

    }

    private string ChicttxtColorOrIcon(BuildRoomName name)
    {
        switch (name)
        {
            case BuildRoomName.Gold:
                Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(name.ToString());
                return "";
            case BuildRoomName.Food:
                Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(name.ToString());
                return "";
            case BuildRoomName.Mana:
                Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(name.ToString());
                return "";
            case BuildRoomName.Wood:
                Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(name.ToString());
                return "";
            case BuildRoomName.Iron:
                Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(name.ToString());
                return "";
            default:
                break;
        }
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(name.ToString());
        return "";

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
