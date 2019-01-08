using UnityEngine;
using System.Collections;

public enum IssueOptions
{
    PlayerLocked = 1,
    BugReport,
    SomethingLose,
    AccountSecurity,
    ReportPlayer,
    CommitAdvise,
}

public class ContactGM : MonoBehaviour {
    public UILabel mailContent;
    public GameObject radioButtonGroup;
    private Warnings warnings;

    
	void Start () {
        warnings = PanelStatic.StaticWarnings;
	}
	
    public void SendGM()
    {
        string issueOption = "";

        if (null != radioButtonGroup)
        {
            UIToggle[] checkboxes = radioButtonGroup.GetComponentsInChildren<UIToggle>();
            foreach(UIToggle checkbox in checkboxes)
            {
                if(checkbox.isChecked)
                {
                    //issueOption = checkbox.transform.GetComponentInChildren<UILabel>().text;
                    issueOption = checkbox.gameObject.name.Replace("Checkbox_","");
                }
            }
        }
        else
        {
            Debug.LogWarning("ContactGM :: SendGM() - radioButtonGroup is null.");
        }

        if (null != mailContent && mailContent.text != "" && issueOption != "")
        {
            //InRoom.GetInRoomInstantiate().MailSend(issueOption, "", mailContent.text, "", "0", "0", "0", true, 0, 0);
			PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate().MailSend(issueOption, "", mailContent.text, "", "0", "0", "0", true));
            //Debug.Log("Issue Option : " + issueOption + " , Mail Content :��" + mailContent.text);
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info488"));
        }
    }
}
