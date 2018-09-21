using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public abstract class UIRoomInfo : TTUIPage
{
    public Text txt_Name;
    public Text txt_Level;

    protected RoomMgr roomData;
    public override void Show(object mData)
    {
        base.Show(mData);
        roomData = mData as RoomMgr;
    }

    protected virtual void UpdateName(RoomMgr data)
    {
        txt_Name.text = data.buildingData.RoomName.ToString();
        txt_Level.text = data.buildingData.Level.ToString();
    }

    protected abstract void UpdateInfo();
}
