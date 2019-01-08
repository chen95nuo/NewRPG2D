using UnityEngine;
using System.Collections;

public class CreateBtnControl : MonoBehaviour {
    
    public UIToggle checkbox2V2;
    public UIToggle checkbox4V4;

    public GameObject btnCreate;

    void OnEnable()
    {
        ShowCreateButton();
    }

    /// <summary>
    /// ��ʾ������ť
    /// </summary>
    void ShowCreateButton() 
    {
        if (checkbox2V2.isChecked && string.IsNullOrEmpty(BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText.Trim()))
        {
            btnCreate.GetComponent<UIButtonMessage>().functionName = "CreateCorps";
            //btnCreate.SetActiveRecursively(true);
        }
        else if (checkbox4V4.isChecked && string.IsNullOrEmpty(BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText.Trim()))
        {
            btnCreate.GetComponent<UIButtonMessage>().functionName = "CreatePVP4";
            //btnCreate.SetActiveRecursively(true);
        }
        //else
        //{
        //    btnCreate.SetActiveRecursively(false); 
        //}
    }
}
