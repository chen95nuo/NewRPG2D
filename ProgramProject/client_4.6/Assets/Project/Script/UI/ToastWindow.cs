using UnityEngine;
using System.Collections;

public class ToastWindow : BWUIPanel
{

    public static ToastWindow mInstance;
    public UILabel textLabel;
    public UILabel titleLabel;

    BWWarnUI toastSrc;
	
	static public int closeType;
	static public bool needShow = false;
	
	public GameObject childObj;
	
	public bool isClosing = false;

    void Awake()
    {
        mInstance = this;
        _MyObj = mInstance.gameObject;
        _MyObj.transform.localPosition = new Vector3(0, 0, -720);
		childObj.SetActive(false);
        //base.hide();
    }


    // Update is called once per frame
    void Update()
    {
		if(needShow)
		{
			if(closeType == -3)
			{
				showText(TextsData.getData(520).chinese);
			}
			needShow = false;
		}
    }

    public override void show()
    {
     	childObj.SetActive(true);
		isClosing = false;
    }

    public override void hide()
    {
        tweenAlpha(1, PANEL_ALPHA_SIZE, baseHide);  //base.hide is wrong
        //base.hide();
    }

    void baseHide()
    {
        _MyObj.transform.localScale = new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1);
        _MyObj.GetComponent<UIPanel>().alpha = 1;
        childObj.SetActive(false);
    }

    public void CleanData()
    {
        gc();
    }

    private void gc()
    {
        mInstance = null;
        GameObject.Destroy(_MyObj);
        _MyObj = null;
        Resources.UnloadUnusedAssets();
    }

    public void showText(string title, string text)
    {
        show();
        titleLabel.text = title;
        textLabel.text = text;

        tweenScale(new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1), new Vector3(1, 1, 1));
    }

    public void showText(string text)
    {
        showText("", text);
    }

    public void showText(string text, BWWarnUI src)
    {
        showText("", text);
        toastSrc = src;
    }

    public void closeToastWindow(int param)
    {
		if(isClosing)
			return;
		isClosing = true;
		if(closeType == -3)
		{
			closeType = 0;
			if(SwitchAccountManager.mInstance!= null)
			{
				SwitchAccountManager.mInstance.logout();
			}
		}
		
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (toastSrc != null)
        {
            toastSrc.warnningCancel();
            toastSrc = null;
        }
       hide ();
    }
}