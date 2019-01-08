using UnityEngine;
using System.Collections;
using System;

public class DeletePlayerPanel : MonoBehaviour {
	public UIInput inputContent;
	public BtnManager btnManager;
	public GameObject deletePlayerPanel;
	public Warnings warnings;
	private string initTxt = "";
	// Use this for initialization
	void Start () {
		initTxt = inputContent.text;
	}
	
	void OnClick ()
	{
        string identifiableInfo = StaticLoc.Loc.Get("info740");
        if (identifiableInfo.Equals("info740"))
        {
            identifiableInfo = "DEL";
        }

        if (string.Equals(inputContent.text, identifiableInfo, StringComparison.InvariantCultureIgnoreCase))	//输入del不区分大小写
		{
			btnManager.DeletePlayerBtnClick();
			deletePlayerPanel.SendMessage("MoveTo", SendMessageOptions.RequireReceiver);
			inputContent.text = initTxt;
		}
		else
		{
			warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info741"));
		}
	}
}
