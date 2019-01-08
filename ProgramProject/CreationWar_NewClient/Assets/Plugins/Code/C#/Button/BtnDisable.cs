using UnityEngine;
using System.Collections;

public class BtnDisable : MonoBehaviour {

    public UISprite picDisabled;
    public UISprite picEnabled;
	public UISprite picQualit;
	public UILabel lblText;
	public UILabel lblNum;
	public UIButtonMessage myMessage;
	public UIButton Mybutton;
	public ParticleSystem TheStart;
    public bool disable = false;

	public string ItemID;
    /// <summary>
    /// 获取或设置控件是否可用
    /// </summary>

	void Awake(){
		if(Mybutton){
		Mybutton = gameObject.GetComponent<UIButton>();
		}
	}
    public bool Disable
    {
        get { return disable; }
        set
        {
            disable = value;
            SetDisable(disable);
        }
    }

    private void SetDisable(bool mDisable)
    {
        if (mDisable)
        {
            picEnabled.gameObject.SetActive(false);
			if(picQualit){
			picEnabled.gameObject.SetActive(true);
			picQualit.gameObject.SetActive(false);
			lblText.gameObject.SetActive(true);
			Mybutton.enabled = false;
				if(TheStart){
				if (!TheStart.isPlaying)
			{
					TheStart.Stop();
			}
				}

			}
        }
        else
        {
            if (this.gameObject.active)
            {
				picEnabled.gameObject.SetActive(true);
				if(picQualit){
				picQualit.gameObject.SetActive(true);
				lblText.gameObject.SetActive(false);
				Mybutton.enabled = true;
					if(TheStart){
				if (!TheStart.isPlaying)
				{
					TheStart.Play();
				}
					}

				}
            }
        }
    }

    public virtual void OnEnable()
    {
        Invoke("InvokDisable", 0.2f);
    }

    private void InvokDisable()
    {
        SetDisable(this.disable);
    }

	void OnClick()
	{
		//		if (!canClickIcon) return;
		
		if(ItemID!="")
		{
			PanelStatic.StaticIteminfo.SetActiveRecursively(true);
			PanelStatic.StaticIteminfo.transform.position = new Vector3(transform.position.x,100,transform.position.z);
			PanelStatic.StaticIteminfo.SendMessage("SetItemID", ItemID, SendMessageOptions.DontRequireReceiver);
		}
	}

}
