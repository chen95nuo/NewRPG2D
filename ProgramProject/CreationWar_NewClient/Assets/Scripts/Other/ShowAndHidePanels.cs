using UnityEngine;
using System.Collections;

public class ShowAndHidePanels : MonoBehaviour
{
    public GameObject[] targetPanels;
    public bool isEnableHide = false;
	public UIToggle[] checkboxs;
	public CkbToPanel[] ckbs;
    
    void OnEnable()
    {
        if (isEnableHide)
        {
            HidePanel();
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    void HidePanel()
    {
        foreach(GameObject targetPanel in targetPanels)
        {
            if (targetPanel.active)
            {
                targetPanel.SetActiveRecursively(false);
            }
        } 
    }

    /// <summary>
    /// ��ʾ����
    /// </summary>
    void ShowPanel()
    {
        foreach (GameObject targetPanel in targetPanels)
        {
            if (!targetPanel.active)
            {
                targetPanel.SetActiveRecursively(true);
            }
        }
    }
	
	void CheckboxChecked()
	{
        foreach (UIToggle checkbox in checkboxs)
        {
            checkbox.isChecked = true;
        }

        foreach (CkbToPanel ckb in ckbs)
        {
            ckb.CbkClick();
        }
	}
}
