using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Net;

public class UILevelUpTipGrid : MonoBehaviour
{
    public Text txt_time;
    [System.NonSerialized]
    private Canvas canvas;
    public Image slider;
    private Vector3 point;
    public Image icon;
    public Text txt_Tip;

    private LocalBuildingData buildingData;

    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.CameraMove, UpdatePos);
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CameraMove, UpdatePos);
    }

    private void Update()
    {
        if (buildingData != null && buildingData.leftTime > 0)
        {
            buildingData.leftTime -= Time.deltaTime;
            float time = buildingData.leftTime;
            txt_time.text = (time <= 0 ? 0 : time).ToString();
            if (buildingData.leftTime <= -1)
            {
                string roomid = (int)buildingData.buildingData.RoomName + "_" + buildingData.id;
                WebSocketManger.instance.Send(NetSendMsg.Q_RoomState, roomid);

            }
        }
    }

    public void GetInfo(LocalBuildingData buildingData, Canvas canvas)
    {
        this.buildingData = buildingData;
        Transform ts = buildingData.currentRoom.disTip.transform;
        point = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        this.canvas = canvas;
        UpdatePos();
    }

    public void UpdatePos()
    {
        if (transform.position.z >= 100 || MapControl.instance.type == CastleType.edit)
        {
            return;
        }
        Vector2 pos;
        RectTransform rt = canvas.transform as RectTransform;
        Vector3 v3 = Camera.main.WorldToScreenPoint(point);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, v3, canvas.worldCamera, out pos);
        RectTransform rect = transform.transform as RectTransform;
        rect.anchoredPosition = pos;
    }
}
