using UnityEngine;
using System.Collections;

public class BtnExpression : MonoBehaviour {

    public YuanInput yInput;
	public TalkLogin talkLogin;

    void OnClick()
    {

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            #if UNITY_IPHONE||UNITY_ANDROID
            yInput.keyboard.active = false;

			if(talkLogin.btnExpr==null)
			{
				talkLogin.btnExpr.yInput=TalkLoginStatic.my.yuanInput;
			}

			if(talkLogin.loadExpr.input==null)
			{
				talkLogin.loadExpr.input=TalkLoginStatic.my.yuanInput.gameObject;
			}
            #endif
        }
    }
}
