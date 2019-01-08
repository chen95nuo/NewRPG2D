using UnityEngine;
using System.Collections;

public class BtnAim : MonoBehaviour {

    private string aimID;
    public BtnDisable btnDisable;
    public BtnEvent btnEvent;
    public UILabel lblName;
    public UILabel lblNum;
    public UILabel lblAddActiveValue;
    public UISprite pic;
    public UIButtonMessage goAheadBtn;

    public string AimID
    {
        get { return aimID; }
        set { aimID = value; }
    }
}
