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
        if (data != null && data.leftTime > -1)
        {
            currentNum += Time.deltaTime;
            if (currentNum >= 1)
            {
                data.leftTime -= currentNum;
                UpdateSlider();
                currentNum = 0;
                if (data.leftTime <= -1)
                {
                    Debug.Log("发送升级完毕");
                    string id = (int)data.buildingData.RoomName + "_" + data.id;
                    WebSocketManger.instance.Send(NetSendMsg.Q_RoomState, id);
                }
                txt_Time.text = SystemTime.instance.TimeNormalizedOf(data.leftTime, false);
            }
        }
    }

    public void UpdateInfo(LocalBuildingData data, bool isEdit = false, bool isWork = false)
    {
        this.data = data;
        Icon.transform.parent.gameObject.SetActive(!isEdit);
        Icon.SetActive(!isWork);
        txt_Time.text = SystemTime.instance.TimeNormalizedOf(data.leftTime, false);
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        Vector2 size = sr.size;
        size.x = maxLeft - ((data.leftTime / (data.buildingData.NeedTime * 60)) * maxLeft);
        sr.size = size;
    }
}
