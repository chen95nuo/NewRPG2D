using UnityEngine;
using System.Collections;

public class MailInfoWirte : MonoBehaviour {

    public UIButtonMessage btnCancel;
    public UIButtonMessage btnSend;
    public UIToggle cbxIsPaymentPickup;
    public UIInput txtBloodStone;
	public UISprite picBloodStone;
    public UIInput txtGold;
    public UIInput txtAddressee;
    public UIInput txtText;
    public UIInput txtTitle;
    public string mailTool;



	public GameObject[] listMail;
	public void Back()
	{
		yuan.YuanClass.SwitchListOnlyOne (listMail,0,true,true);
	}
	
	public void OnEnable()
	{
		if(InRoom.GetInRoomInstantiate ().GetBloodTran()!=1)
		{
			picBloodStone.gameObject.SetActiveRecursively (false);
			txtGold.gameObject.SetActiveRecursively (false);
		}
	}



}
