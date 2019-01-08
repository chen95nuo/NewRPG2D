using UnityEngine;
using System.Collections;

public class BtnActivity : MonoBehaviour {

    public UIButtonMessage MyMessage;
    //public UIToggle MyCheckbox;
    public BtnDisable btnDisable;
    public UILabel lblTime;
    public UILabel lblText;
    public UISprite pic;
    public yuan.YuanMemoryDB.YuanRow yr;
    public string activityID;
    public bool canGetReward = false;
    public bool isCancelQueue = false;// 是否可以取消排队
}
