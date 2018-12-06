using Assets.Script.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildLevelUp : MonoBehaviour
{
    public TextMesh txt_Time;
    public TextMesh txt_Tip;
    public SpriteRenderer sr;
    public GameObject Icon;

    public float maxLeft = 1.77f;

    private LocalBuildingData data;

    // Update is called once per frame
    void Update()
    {
        if (data != null && data.leftTime >= 0)
        {
            data.leftTime -= Time.deltaTime;
            if (data.leftTime == -1)
            {
                WebSocketManger.instance.Send(NetSendMsg.Q_RoomState, data.id);
            }
            txt_Time.text = data.leftTime.ToString();
        }
    }

    public void UpdateInfo(LocalBuildingData data)
    {
        this.data = data;
    }
}
