
using UnityEngine;
using System.Collections;
using System;

public enum BoxNumber
{
    None = -1,
    btnChest0 = 0,
    btnChest1 = 1,
    btnChest2 = 2,
    btnChest3 = 3,
}

public class OpenChestsBox : MonoBehaviour {
    public UILabel lblTime;
	public UISprite PicSprite;
    public ParticleSystem xingxing;
	public Color b;
	public Color c;

    /// <summary>
    /// 宝箱编号
    /// </summary>
    private BoxNumber boxNum = BoxNumber.None;

    private BoxNumber BoxNum
    {
        get { return boxNum; }
        set { boxNum = value; }
    }

    /*
    /// <summary>
    /// 5分钟开启第一个宝箱，这里用秒表示
    /// </summary>
    public const int FIRST_BOX_TIME = 300;
    /// <summary>
    /// 15分钟开启第二个宝箱，这里用秒表示
    /// </summary>
    //public const int SECOND_BOX_TIME = 900;
    public const int SECOND_BOX_TIME = 300;
    /// <summary>
    /// 30分钟开启第三个宝箱，这里用秒表示
    /// </summary>
    //public const int THIRD_BOX_TIME = 1800;
    public const int THIRD_BOX_TIME = 300;
    /// <summary>
    /// 60分钟开启第四个宝箱，这里用秒表示
    /// </summary>
    //public const int FOURTH_BOX_TIME = 3600;
    public const int FOURTH_BOX_TIME = 300;
    */

    /// <summary>
    /// 是否是第一次打开宝箱面板,这个是为了防止切换界面时倒计时重置
    /// </summary>
    private bool isFirst = false;

	void Awake () 
    {
        isFirst = true;
        boxNum = (BoxNumber)Enum.Parse(typeof(BoxNumber), gameObject.name);
	}

    
    void OnEnable()
    {
        if (isFirst)
        {
            //Debug.Log("1在线宝箱--------------------------" + BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText);
            //int onlineChestTime = int.Parse(BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText);
            //onlineChestTime += ((int)(InRoom.GetInRoomInstantiate().serverTime - DateTime.Parse(BtnGameManager.yt[0]["LogonTime"].YuanColumnText)).TotalSeconds);
            //BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText = onlineChestTime.ToString();
            //Debug.Log("2在线宝箱--------------------------" + BtnGameManager.yt[0]["OnlineChestsTime"].YuanColumnText);

            OpenChest.my.ReceiveOpenBox();
            isFirst = false;
        }
        else
        {
            // 这里是控制宝箱上的粒子系统的逻辑
            string openBoxInfo = BtnGameManager.yt[0]["OpenedChests"].YuanColumnText;
            string[] openBoxes = openBoxInfo.Split(',');

            SetBoxState(openBoxes);
        }
    }

    private float startTime;
    private int hasTotalTime;// 已经倒计时的时间
    private int totalTime;
    private int currentTime;
    private int lastTime;
    private int hours;
    private int minutes;
    private int seconds;
    private bool isCountDown = false;

    /// <summary>
    /// 设置宝箱状态
    /// </summary>
    private void SetBoxState(string[] boxesInfo)
    {
        if ((int)boxNum < 0)
        {
            return;
        }

        if (string.Equals(boxesInfo[(int)boxNum], "-1"))
        {
            // 不可开启的状态
			PicSprite.spriteName = "zaixianbaoxiang";
			PicSprite.color = c;
            if (xingxing.isPlaying)
            {
                xingxing.Stop();
            }
        }
        else if (string.Equals(boxesInfo[(int)boxNum], "0"))
        {
            // 时间未到，不能开启
			PicSprite.spriteName = "zaixianbaoxiang";
			PicSprite.color = b;
            if (!xingxing.isPlaying)
            {
                xingxing.Play();
            }
        }
        else
        { 
            // 可开启状态
			PicSprite.spriteName = "zaixianbaoxiang";
			PicSprite.color = b;
            if (!xingxing.isPlaying)
            {
                xingxing.Play();
            }
        }
    }

    /// <summary>
    /// 刷新时间
    /// </summary>
    /// <param name="time"></param>
    public void RefreshInfo(int time, string[] boxesInfo)
    {
        hasTotalTime = time;
        startTime = Time.time;

        if (boxNum > BoxNumber.None && string.Equals(boxesInfo[(int)boxNum], "0")) // 只有当宝箱为时间未到状态时，才将即时标志位设为true
        {
            isCountDown = true;        
        }

        SetBoxState(boxesInfo);
    }

    void Update()
    {
        if (isCountDown)
        {
            if (!lblTime.enabled)
            {
                lblTime.enabled = true;
            }

            if (null == OpenChest.my || null == OpenChest.my.OpenBoxesTime)
            {
                return;
            }

            switch (boxNum)
            {
                case BoxNumber.btnChest0:
                    totalTime = OpenChest.my.OpenBoxesTime[0] - hasTotalTime;
                    break;
                case BoxNumber.btnChest1:
                    totalTime = OpenChest.my.OpenBoxesTime[1] - hasTotalTime;
                    break;
                case BoxNumber.btnChest2:
                    totalTime = OpenChest.my.OpenBoxesTime[2] - hasTotalTime;
                    break;
                case BoxNumber.btnChest3:
                    totalTime = OpenChest.my.OpenBoxesTime[3] - hasTotalTime;
                    break;
            }

            if (totalTime < 0)
            {
                totalTime = 0;
            }

            currentTime = (int)(Time.time - startTime);
            lastTime = (int)(totalTime - currentTime);
            hours = (int)(lastTime / (60 * 60));
            minutes = lastTime % (60 * 60) / 60;
            seconds = lastTime % (60 * 60) % 60;

            if (null != lblTime && lblTime.enabled)
            {
                lblTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            }

            if (lastTime <= 0)
            {
                isCountDown = false;
                lblTime.text = "";
                lblTime.enabled = false;
            }
        }
        else
        {
            if (lblTime.enabled)
            {
                lblTime.enabled = false;
            }
        }
    }
}
