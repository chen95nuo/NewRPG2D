using UnityEngine;
using System.Collections;

public class ShowLblState : MonoBehaviour {
    public static ShowLblState showLblState;

    public UILabel LblState;

    void Awake()
    {
        showLblState = this;
    }

	// Use this for initialization
	void Start () 
    {
        if (LblState.enabled)
        {
            LblState.enabled = false;
        }
	}

    public void SetLabelTxt(string txt, bool isShow)
    {
        if (null != LblState)
        {
            LblState.enabled = isShow;
            LblState.text = txt;
        }
    }
}
