using UnityEngine;
using System.Collections;

public class ShowInfoOnFirst : MonoBehaviour {
    private RefreshList refreshListScript;

	// Use this for initialization
	void Start () {
        refreshListScript = GetComponent<RefreshList>();
	}
	
	// Update is called once per frame
	void Update () {
     //   ShowInfo();
	}

    void ShowInfo()
    {
        if (transform.childCount > 0)
        { 
            foreach(UIToggle checkbox in gameObject.GetComponentsInChildren<UIToggle>())
            {
                if (checkbox.isChecked && refreshListScript != null)
                {
                    refreshListScript.SetPlayerInfo(checkbox.gameObject);
                }
            }
        }
    }
}
