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

    private float currentNum;

    // Update is called once per frame
    void Update()
    {
        if (data != null && data.leftTime >= 0)
        {
            currentNum += Time.deltaTime;
            if (currentNum >= 1)
            {
                data.leftTime -= currentNum;
                if (data.leftTime == -1)
                {
                    WebSocketManger.instance.Send(NetSendMsg.Q_RoomState, data.id);
                }
            }
            txt_Time.text = SystemTime.instance.TimeNormalizedOf(data.leftTime);
            currentNum = 0;
        }
    }

    public void UpdateInfo(LocalBuildingData data, bool isEdit = false, bool isWork = false)
    {
        this.data = data;
        Icon.transform.parent.gameObject.SetActive(!isEdit);
        Icon.SetActive(!isWork);
    }
}
