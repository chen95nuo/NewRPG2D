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
    private RoomMgr nowRoom;
    private Collider2D[] col = new Collider2D[10];

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

        if (nowRoom != null)
        {
            if (role.RoleData.currentRoom == null || nowRoom.Id != role.RoleData.currentRoom.Id)
            {
                if (nowRoom.RoomName == BuildRoomName.Stairs)
                {
                    object st = "该处为楼梯无法进入";
                    UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
                }
                else
                {
                    nowRoom.AddRole(role);
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
        int colNum = Physics2D.OverlapPointNonAlloc(Camera.main.ScreenToWorldPoint(Input.mousePosition), col);
        if (colNum > 0)
        {
            bool isRoom = false;
            RoomMgr room = null;
            for (int i = 0; i < colNum; i++)
            {
                if (col[i].tag == "Room")
                {
                    isRoom = true;
                    room = col[i].GetComponent<RoomMgr>();
                    nowRoom = room;
                    break;
                }
            }
            if (isRoom)
            {
                if (currentRoom != null && room.Id == currentRoom.Id)
                {
                    return;
                }
                bool isTrue = ChickPlayerInfo.instance.ChickProduction(room.currentBuildData);
                if (isTrue)
                {
                    RoleAttribute roleAtr = room.NeedAttribute;
                    int temp = room.currentBuildData.ScreenAllYeild(roleAtr, false);
                    Debug.Log(temp);
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
                    if (temp >= 0)//包括+0
                    {
                        float p_2 = room.currentBuildData.roleData[temp].GetArtProduce(room.RoomName);
                        float p_3 = p_1 - p_2;
                        if (p_3 >= 0)//包括+0
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
            else
            {
                nowRoom = null;
            }
        }
        else
        {
            nowRoom = null;
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
