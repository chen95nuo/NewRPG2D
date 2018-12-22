using UnityEngine;
using System.Collections;

public class NoticeUIManager : BWUIPanel {
	
	public static NoticeUIManager mInstance;
	public UILabel showLabel;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
	}

	public void SetData(string str)
	{
		show ();
		showLabel.text = str;

        tweenScale(new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1), new Vector3(1, 1, 1));
	}
	
	public void OnClickBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		//hide();
        tweenAlpha(1, PANEL_ALPHA_SIZE, baseHide);
	}

    void baseHide()
    {
        _MyObj.transform.localScale = new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1);
        _MyObj.GetComponent<UIPanel>().alpha = 1;
        hide();
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
	
}
