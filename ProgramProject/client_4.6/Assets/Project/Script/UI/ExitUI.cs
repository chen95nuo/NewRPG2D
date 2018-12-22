using UnityEngine;
using System.Collections;

public class ExitUI : BWUIPanel
{
	public GameObject child;

    void Awake()
    {
        _MyObj = gameObject;
    }

	// Use this for initialization
	void Start () {
		gameObject.transform.localPosition=Vector3.zero;
		child.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(SDKManager.getInstance().isSDKGCUsing())
			{
				if(Application.platform==RuntimePlatform.Android)
				{
					SDK_GCStubManager.sdk_openExitPopup();
				}
			}
			else
			{
                if (!child.activeSelf)
                {
                    child.SetActive(true);
                    tweenScale(new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1), new Vector3(1, 1, 1));
                }
			}
		}
	}
	
	public void onClickSure()
	{
		if(SDKManager.getInstance().isSDKBDDKUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				SDK_BDDK_Manager.sdk_exit();
			}
		}
		else
		{
			Application.Quit();
		}
	}
	
	public void onClickCancel()
	{
        tweenAlpha(1, PANEL_ALPHA_SIZE, childHide);
	}

    void childHide()
    {
        gameObject.transform.localScale = new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1);
        gameObject.GetComponent<UIPanel>().alpha = 1;
        child.SetActive(false);
    }
	
}
