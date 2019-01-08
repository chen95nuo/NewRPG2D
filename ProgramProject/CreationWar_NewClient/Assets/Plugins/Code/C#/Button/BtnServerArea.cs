using UnityEngine;
using System.Collections;

public class BtnServerArea : MonoBehaviour {
    public UILabel serverArea;
    private string areaName;
    private UIToggle toggle;

    public string AreaName
    {
        get { return areaName; }
        set { areaName = value; serverArea.text = areaName; }
    }

	// Use this for initialization
	void Awake () {
        toggle = this.GetComponent<UIToggle>();
	}

    public void ItemClick()
    {
        if(toggle.value)
        {
            MainMenuManage.my.gameObject.SendMessage("ShowServer", areaName, SendMessageOptions.DontRequireReceiver);
        }   
    }
}
