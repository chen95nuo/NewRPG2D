using UnityEngine;
using System.Collections;

public class UIPanelSwich : MonoBehaviour {

    public UIPanel myPanel;
	// Use this for initialization
	void Start () {
        if (myPanel == null)
        {
            myPanel = GetComponent<UIPanel>();
        }
	}

    void OnEnable()
    {
        if (myPanel != null)
        {
            myPanel.enabled = true;
        }
    }

    void OnDisable()
    {
        if (myPanel != null)
        {
            myPanel.enabled = false;
        }
    }
	
}
