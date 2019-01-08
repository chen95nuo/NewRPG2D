using UnityEngine;
using System.Collections;

public class BtnItem : MonoBehaviour {

    public yuan.YuanMemoryDB.YuanRow yr;
    public UISprite pic;
    public UILabel lblName;
    public UILabel lblInfo;
    public UILabel lblFavorable;
    public UISprite spriteFavorable;
    public UIButtonMessage myMessage;
    public UIToggle myCheck;
	public UISprite spriteBackground;
    private System.DateTime dtEnd;
    /// <summary>
    /// ½áÊøÊ±¼ä
    /// </summary>
    public System.DateTime DtEnd
    {
        get { return dtEnd; }
        set { dtEnd = value; SetTime(dtEnd);}
    }
    [HideInInspector]
    public int ItemNeedBlood;
    [HideInInspector]
    public int ItemNeedCash;
    public string storeItemID;
    public string iconName;
    public string ItemID;
    private bool isFavorable = true;
    /// <summary>
    /// ÊÇ·ñ´òÕÛ
    /// </summary>
    public bool IsFavorable
    {
        get { return isFavorable; }
        set {
            isFavorable = value;
            SetFavorable(isFavorable);
        }
    }

    public bool isStart;
	
	void Awake()
	{
		infoBar=PanelStatic.StaticIteminfo.gameObject;
	}

    private void SetFavorable(bool mIsFavorable)
    {
        lblFavorable.gameObject.SetActive(mIsFavorable);
        spriteFavorable.gameObject.SetActive(mIsFavorable);
    }

    void OnEnable()
    {
        Invoke("InvokSetFavorable", 0.3f);
    }

    public GameObject infoBar;
    void OnClick()
    {

   }

	void OnPress (bool isDown)
	{
		if(infoBar){
			if(isDown)
			{
				infoBar.SetActiveRecursively(true);
				//infoBar.transform.Translate (infoBar.transform.up);
				//Debug.Log("-------------------" + ItemID);
				infoBar.transform.localPosition=new Vector3(-0.2875011f,100.1449f,-5.680656f);
				infoBar.SendMessage("SetItemID",ItemID,SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				infoBar.transform.position=new Vector3(0,-999999,0);
				
				infoBar.gameObject.SetActiveRecursively (false);
			}
			
		}
	}

	void OnDrag (Vector2 delta)
	{
		if(infoBar&&infoBar.gameObject.activeSelf&&Mathf.Abs ( delta.y)>10)
		{
			infoBar.transform.position=new Vector3(0,-999999,0);
			
			infoBar.gameObject.SetActiveRecursively (false);
		}
	}

    private void InvokSetFavorable()
    {
        SetFavorable(this.isFavorable);
    }

    private void SetTime(System.DateTime dt)
    {
       
        //time = dt.TimeOfDay - System.DateTime.Now.TimeOfDay;
        time = dt.TimeOfDay - InRoom.GetInRoomInstantiate().serverTime.TimeOfDay;
        isStart = true;
        InvokeRepeating("GetTime", 1, 1);
    }

    public string tttttt;
    private System.TimeSpan time;
    private System.TimeSpan timeSpan = new System.TimeSpan(0,0,1);
    private void GetTime()
    {
       
        if (time.Hours <= 0 && time.Minutes <= 0 && time.Seconds <= 0)
        {
            CancelInvoke("GetTime");
            lblInfo.text = StaticLoc.Loc.Get("buttons644");
            SetFavorable(false);
            isStart = false;
        }
        else
        {
            time = time.Subtract(timeSpan);
            //lblInfo.text = time.Hours + ":" + time.Minutes + StaticLoc.Loc.Get("info988");
            lblInfo.text = time.Hours + StaticLoc.Loc.Get("info988") + time.Minutes + StaticLoc.Loc.Get("messages045");
            tttttt = time.ToString();
        }
     
    }

    
}
