using UnityEngine;
using System.Collections;

public class BtnClick : MonoBehaviour {
	
	public BtnManager btnManager;
	public string invokMethodName;
    public bool isOnMouseDown = false;

	public UISprite Me ; 
	public UISprite otherOne ; 
	public UISprite otherTwo ; 
	// Use this for initialization
	void Start () {
	
	}
	
	void OnClick()
	{
		if(btnManager!=null)
		{
			btnManager.Invoke (invokMethodName,0);
		}
	}

    public void OnMouseDown()
    {
        if (btnManager != null&&isOnMouseDown)
        {
            btnManager.Invoke(invokMethodName, 0);
			ShowMyBtn();
        }
    }

	public void ShowMyBtn()
	{
		if(Me&&otherOne&&otherTwo){
		Me.enabled = true;
		otherOne.enabled = false;
		otherTwo.enabled = false;
	}
	}
}
