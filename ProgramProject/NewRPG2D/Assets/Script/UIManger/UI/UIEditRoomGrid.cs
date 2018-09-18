using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEditRoomGrid : MonoBehaviour
{
    public Button btn_enter;
    public Text txt_name;
    public RoomMgr roomMgr;

    private void Awake()
    {
        btn_enter.onClick.AddListener(ChickEndter);
    }

    private void ChickEndter()
    {
        HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.AddBuild, roomMgr);
        gameObject.SetActive(false);
    }
}
