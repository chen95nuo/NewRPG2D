using UnityEngine;
using System;
using System.Collections;

// mark - xuyan//
public class BWUIPanel : MonoBehaviour 
{
	bool mIsVisible;
	protected GameObject _MyObj;
	public virtual void init()
	{
		gameObject.transform.localPosition = new Vector3(0,0,0);
	}
	
	public virtual void show()
	{
		gameObject.SetActive(true);
		mIsVisible = true;
	}

	public virtual void hide()
	{
		gameObject.SetActive(false);
		mIsVisible = false;
	}
	
	public void close()
	{
		gameObject.SetActive(false);
		mIsVisible = false;
		
	}
	
	public bool isVisible()
	{
		return mIsVisible;
	}

    public virtual void tweenScale(Vector3 v1, Vector3 v2, EventDelegate.Callback callback = null)
    {
        TweenScale tween = UITweener.Begin<TweenScale>(_MyObj, PANEL_SCALE_TIME);
        tween.from = v1;
        tween.to = v2;
        if (callback != null)
            EventDelegate.Add(tween.onFinished, callback);
    }

    public virtual void tweenAlpha(float a1, float a2, EventDelegate.Callback callback = null)
    {
        TweenAlpha tween = UITweener.Begin<TweenAlpha>(_MyObj, PANEL_ALPHA_TIME);
        tween.from = a1;
        tween.to = a2;
        if (callback != null)
            EventDelegate.Add(tween.onFinished, callback);
    }

    public const float PANEL_SCALE_TIME = 0.1f;
    public const float PANEL_SCALE_SIZE = 0.6f;

    public const float PANEL_ALPHA_TIME = 0.1f;
    public const float PANEL_ALPHA_SIZE = 0.5f;
    
}
