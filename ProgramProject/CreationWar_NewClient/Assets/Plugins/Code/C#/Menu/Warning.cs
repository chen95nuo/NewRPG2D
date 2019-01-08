using UnityEngine;
using System.Collections;

public class Warning : MonoBehaviour {

    public BtnClick btnYes;
    public BtnClick btnNo;
    public UILabel lblTitle;
    public UILabel lblText;

    public TweenScale tweenScale;


	

    /// <summary>
    /// µ¯³öÌáÊ¾¿ò
    /// </summary>
    public void Out()
    {
        this.gameObject.SetActive(true);
        tweenScale.Play(true);
    }


}
