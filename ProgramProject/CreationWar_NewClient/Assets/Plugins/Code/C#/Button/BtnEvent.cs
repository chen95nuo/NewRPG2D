using UnityEngine;
using System.Collections;

public class BtnEvent : MonoBehaviour {

    public delegate void DelegateBtn(object sender,object parm);
    public event DelegateBtn BtnClickEvent;
    public UIButton myButton;
	public WarningAll myWarningAll;
	public delegate void ActionTimeOut();
	public ActionTimeOut actionTimeOut;

    public void SetEvent(DelegateBtn mDele)
    {
        BtnClickEvent = mDele;
    }
	
	public void SetTimeOut(ActionTimeOut mActionTImeOut)
	{
		actionTimeOut=mActionTImeOut;
	}

    void OnClick()
    {
        if (BtnClickEvent != null)
        {
            BtnClickEvent(this.gameObject, null);
        }
		if(null!=actionTimeOut)
		{
			
			//StartCoroutine (actionTimeOut());
			actionTimeOut();
		}
		if(null!=myWarningAll)
		{
			myWarningAll.Close ();
		}
    }

}
