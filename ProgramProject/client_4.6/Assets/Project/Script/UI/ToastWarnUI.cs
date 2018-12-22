using UnityEngine;
using System.Collections;

public class ToastWarnUI : BWUIPanel
{
    public static ToastWarnUI mInstance;

    public UILabel label;
    public UILabel sure;
    public UILabel cancel;

    bool showAfterOk;
    BWWarnUI beWarned;

    void Awake()
    {
        mInstance = this;
        _MyObj = gameObject;
        _MyObj.transform.localPosition = new Vector3(0, 0, -720);
		sure.text = TextsData.getData(9).chinese;
        cancel.text = TextsData.getData(97).chinese;
        base.hide();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void hide()
    {
        sure.text = TextsData.getData(9).chinese;
        cancel.text = TextsData.getData(97).chinese;
        tweenAlpha(1, PANEL_ALPHA_SIZE, baseHide);  //base.hide is wrong
        //base.hide();
    }

    void baseHide()
    {
        _MyObj.transform.localScale = new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1);
        _MyObj.GetComponent<UIPanel>().alpha = 1;
        base.hide();
    }

    public void showWarn(string msg, BWWarnUI beWarned, bool showAfterOk = false)
    {
        label.text = msg;
        this.beWarned = beWarned;
        this.showAfterOk = showAfterOk;
        show();

        tweenScale(new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1), new Vector3(1, 1, 1));
    }

    public void showWarn(string msg, BWWarnUI beWarned, string cancelText, string sureText)
    {
        sure.text = sureText;
        cancel.text = cancelText;
        showWarn(msg, beWarned);
    }

    public void onClickBtn(int param)
    {
        switch (param)
        {
            case 0:
                MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
                beWarned.warnningCancel();
                beWarned = null;
                hide();
                break;
            case 1:
                MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
                if (!showAfterOk)
                {
                    hide();
                }
                beWarned.warnningSure();
                break;
        }
    }
}
