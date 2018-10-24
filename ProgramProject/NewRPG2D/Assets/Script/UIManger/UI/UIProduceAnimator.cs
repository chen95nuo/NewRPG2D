using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using DG.Tweening;

public class UIProduceAnimator : TTUIPage
{
    public int Number = 5;//每次弹出图标的数量

    public GameObject produceIcon;

    private List<UIProduceAnimHelper> Icons = new List<UIProduceAnimHelper>();

    public Sprite[] sp;

    public override void Show(object mData)
    {
        Number = 5;
        transform.SetSiblingIndex(-1);
        base.Show(mData);
        RoomMgr room = mData as RoomMgr;
        UpdateInfo(room);
    }

    private void UpdateInfo(RoomMgr data)
    {
        Vector3 point = GetPoint(data);
        Vector3 startPoint = GetStartPoint(data);
        if (Icons.Count <= 0)
        {
            GameObject go = Instantiate(produceIcon, this.transform) as GameObject;
            Image im = go.GetComponent<Image>();
            UIProduceAnimHelper IconData = new UIProduceAnimHelper(im);
            Icons.Add(IconData);
        }
        int iconNum = 0;
        switch (data.RoomName)
        {
            case BuildRoomName.Gold:
                iconNum = 0;
                break;
            case BuildRoomName.Food:
                iconNum = 1;
                break;
            case BuildRoomName.Mana:
                iconNum = 2;
                break;
            case BuildRoomName.Wood:
                iconNum = 3;
                break;
            case BuildRoomName.Iron:
                iconNum = 4;
                break;
            case BuildRoomName.MaxRoom:
                iconNum = 5;
                break;
            default:
                break;
        }
        StartCoroutine(IconMove(startPoint, point, iconNum, data.RoomName));
    }
    private Vector3 GetStartPoint(RoomMgr data)
    {
        Canvas canvas = transform.parent.parent.GetComponent<Canvas>();
        Vector2 pos;
        RectTransform rt = canvas.transform as RectTransform;
        Vector3 v3 = Camera.main.WorldToScreenPoint(data.roomProp.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, v3, canvas.worldCamera, out pos);
        return pos;
    }
    private Vector3 GetPoint(RoomMgr data)
    {
        switch (data.RoomName)
        {
            case BuildRoomName.Gold:
                return UIMain.instance.Icons[0].transform.position;
            case BuildRoomName.Food:
                return UIMain.instance.Icons[1].transform.position;
            case BuildRoomName.Mana:
                return UIMain.instance.Icons[2].transform.position;
            case BuildRoomName.Wood:
                return UIMain.instance.Icons[3].transform.position;
            case BuildRoomName.Iron:
                return UIMain.instance.Icons[4].transform.position;
            default:
                break;
        }
        return Vector3.zero;
    }

    private void IconMove(UIProduceAnimHelper Icons, Vector3 point)
    {
        Icons.icon.transform.DOMove(point, 1f).OnComplete(() => IconMoveFinsh(Icons));
    }

    private void IconMoveFinsh(UIProduceAnimHelper icons)
    {
        icons.IsUse = false;
        HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, icons.name);
    }

    private IEnumerator IconMove(Vector3 startPoint, Vector3 point, int iconNum, BuildRoomName name)
    {
        for (int i = 0; i < Icons.Count; i++)
        {
            if (Icons[i].IsUse == false)
            {
                Number--;
                Icons[i].name = name;
                Icons[i].IsUse = true;
                Icons[i].icon.sprite = sp[iconNum];
                int x = Random.Range(-30, 30);
                int y = Random.Range(-30, 30);
                Vector3 s_point = new Vector3(startPoint.x + x, startPoint.y + y, startPoint.z);
                Icons[i].icon.rectTransform.anchoredPosition3D = s_point;
                IconMove(Icons[i], point);
                yield return new WaitForSeconds(0.1f);
            }
            if (i == Icons.Count - 1 && Number >= 1)
            {
                GameObject go = Instantiate(produceIcon, this.transform) as GameObject;
                Image image = go.GetComponent<Image>();
                UIProduceAnimHelper IconData = new UIProduceAnimHelper(image);
                IconData.name = name;
                Icons.Add(IconData);
            }
        }
    }
}

[System.Serializable]
public class UIProduceAnimHelper
{
    public Image icon;
    public BuildRoomName name;
    private bool isUse;
    public UIProduceAnimHelper(Image icon)
    {
        this.icon = icon;
        IsUse = false;
    }

    public bool IsUse
    {
        get
        {
            return isUse;
        }

        set
        {
            bool temp = value;
            if (isUse != temp)
            {
                isUse = value;
                if (IsUse == false)
                {
                    icon.transform.position = Vector3.back * 1000;
                }
            }
        }
    }
}
