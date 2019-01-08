using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TVMessageBody
{
	public string message;
	public bool isSystem;

	public TVMessageBody(string mMessage,bool mIsSystem)
	{
		this.message=mMessage;
		this.isSystem=mIsSystem;
	}
}

public class TVMessage : MonoBehaviour {

    public int playLoopNum = 1;
    public float playSpeed=1;
    public UILabel lblText;
	public GameObject yuanInput;
    public GameObject panel;
    public EventCollider colliderStart;
    public EventCollider colliderEnd;
    public EventCollider colliderLableEnd;
	public static List<TVMessageBody> listText = new List<TVMessageBody>();

    private int tempNum = 0;
    private bool canPlayer = true;
    public bool CanPlayer
    {
        get { return canPlayer; }
        set 
        {
            canPlayer = value;
            if (canPlayer)
            {
				this.panel.SetActive(false);
                //this.panel.SetActiveRecursively(false);
            }
            else
            {
				this.panel.SetActive(true);
                //this.panel.SetActiveRecursively(true);
            }
        }
    }



    void Awake()
    {
		
        colliderStart.eventCollider += this.OnStartTriggerEnter;
        colliderEnd.eventCollider += this.OnEndTriggerEnter;
        colliderLableEnd.eventCollider += this.OnLableEndTriggerEnter;
		yuanInput.SendMessage ("SetMyLeble",lblText,SendMessageOptions.DontRequireReceiver);
		this.panel.SetActive(false);
		//this.panel.SetActiveRecursively(false);
        ResetPos();
        ResetColliderPos();
		
    }
	

	private bool isPlaying=false;
    void FixedUpdate()
    {
        if (CanPlayer && listText.Count > 0)
        {
            if (listText.Count > 10)
            {
                playLoopNum = 1;
            }
            else
            {
                playLoopNum = 1;
            }
            StartPlay(listText[0].message);
			if(listText[0].isSystem)
			{
				PanelStatic.StaticSendManager.AddMessage (listText[0].message,"","",yuan.YuanPhoton.MessageType.System);
			}

            listText.RemoveAt(0);
        }
		if(isPlaying)
		{
			StartTran ();
		}
		
    }

    /// <summary>
    /// 添加文本到队列
    /// </summary>
    /// <param name="mText">内容</param>
    public static void Add(string mText,bool isSystem)
    {
       // listText.Add(mText);
		listText.Add(new TVMessageBody(mText,isSystem));
    }

    /// <summary>
    /// 重置字幕位置
    /// </summary>
    private void ResetPos()
    {
		
        yuanInput.transform.position = colliderStart.transform.position;
    }

    /// <summary>
    /// 重置字幕碰撞位置
    /// </summary>
    private void ResetColliderPos()
    {
        colliderLableEnd.transform.localPosition = new Vector3( lblText.width, colliderLableEnd.transform.localPosition.y, colliderLableEnd.transform.localPosition.z);
    }

    /// <summary>
    /// 开始播放
    /// </summary>
    private void StartPlay()
    {
        if (CanPlayer)
        {
            CanPlayer = false;
            ResetPos();
            ResetColliderPos();
            //InvokeRepeating("StartTran", 0.05f, 0.05f);
			isPlaying=true;
        }
    }

    /// <summary>
    /// 开始播放
    /// </summary>
    /// <param name="mText">播放内容</param>
    private void StartPlay(string mText)
    {
        if (CanPlayer)
        {
            CanPlayer = false;
            //lblText.text = mText;
            yuanInput.SendMessage("AddTextObj", string.Format(mText, StaticLoc.Loc.Get("info1056")), SendMessageOptions.DontRequireReceiver);
            ResetPos();
            ResetColliderPos();
            //InvokeRepeating("StartTran", 0.05f, 0.05f);
			isPlaying=true;
        }
    }

    private void StartTran()
    {
            yuanInput.transform.Translate(yuanInput.transform.right * -0.1f * playSpeed * Time.deltaTime);
    }

    private void OnStartTriggerEnter(Collider mCollider)
    {
 
    }

    private void OnEndTriggerEnter(Collider mCollider)
    {
        if (mCollider.gameObject == this.colliderLableEnd.gameObject)
        {
            tempNum++;
            ResetPos();
            ResetColliderPos();
            if (tempNum >= playLoopNum)
            {
                tempNum = 0;
                CanPlayer = true;
                //CancelInvoke("StartTran");
				isPlaying=false;
            }
        }
    }

    private void OnLableEndTriggerEnter(Collider mCollider)
    {
        
    }
}
