using UnityEngine;
using System.Collections;

public class WarningAll : MonoBehaviour {
    public UILabel lblTitle;
    public UILabel lblText;
    public UIButtonMessage btnEnter;
    public UIButtonMessage btnExit;
    public BtnEvent btnEnterEvent;
    public BtnEvent btnExitEvent;
    public int showTime = 0;
    public ParticleEmitter myParticle;
    public AudioSource myAudio;

    void Start()
    {
        //lblTitle = transform.FindChild("lblTitle").GetComponent<UILabel>();
        //lblText = transform.FindChild("lblText").GetComponent<UILabel>();
        //btnEnter = transform.FindChild("btnEnter").GetComponent<UIButtonMessage>();
        //btnExit = transform.FindChild("btnExit").GetComponent<UIButtonMessage>();
        if (btnExit != null)
        {
            btnExit.target = this.gameObject;
            btnExit.functionName = "Close";
        }
    }
	
	public void ShowLucky(int nLucky){
		string nowLuckey = string.Format("{0}:{1}:{2}",StaticLoc.Loc.Get("info796"),nLucky.ToString(),StaticLoc.Loc.Get("info297"));
		this.lblTitle.text = StaticLoc.Loc.Get("info358");
		this.lblText.text = nowLuckey;
		this.gameObject.SetActiveRecursively(true);
		 if (showTime != 0)
        {
            this.CancelInvoke("Close");
            Invoke("Close", showTime);
        }
	}

    /// <summary>
    /// µ¯³öŸ¯žæÃæ°å
    /// </summary>
    /// <param name="mTitle">±êÌâ</param>
    /// <param name="mText">ÄÚÈÝ</param>
    public void Show(string mTitle,string mText)
    {
        this.lblTitle.text = mTitle;
        this.lblText.text = mText;
        this.gameObject.SetActiveRecursively(true);
//        if (myParticle != null)
//        {
//           myParticle.Emit();
//        }
//        if (myAudio != null)
//        {
         
//            myAudio.Play();
//        }
        if (showTime != 0)
        {
            this.CancelInvoke("Close");
            Invoke("Close", showTime);
        }

    }
	
	public void ShowTimeOut(string mTitle,string mText,BtnEvent.ActionTimeOut mActionTimeOut)
	{
		this.lblTitle.text = mTitle;
        this.lblText.text = mText;
		btnEnterEvent.SetTimeOut (mActionTimeOut);
        this.gameObject.SetActiveRecursively(true);
		
	}

    /// <summary>
    /// ¹Ø±ÕŸ¯žæÃæ°å
    /// </summary>
    public void Close()
    {
		this.gameObject.SetActive(false);
    }
	

}
