using UnityEngine;
using System.Collections;

public class RedemptionCode : MonoBehaviour {
	
	public UIInput txtCode;
	
	void Awake()
	{
		PanelStatic.StaticBtnGameManager.redemptionCode=this;
	}
	
	void OnGetCode()
	{
		if(txtCode.text.Trim ()!="")
		{
			InRoom.GetInRoomInstantiate ().RedemptionCode (txtCode.text.Trim ());
		}
		else
		{
			PanelStatic.StaticWarnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info500"));
		}
	}
	
	public void GetPage(string mPageId)
	{
		yuan.YuanMemoryDB.YuanRow row=YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ytGameItem.SelectRowEqual ("id",mPageId);
		if(row!=null)
		{
			if(row["ItemID"].YuanColumnText.Substring (0,2)=="88")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewDaojuItemAsID", row["ItemID"].YuanColumnText+",01", SendMessageOptions.DontRequireReceiver);
			}
			else if(row["ItemID"].YuanColumnText.Substring (0,2)=="72")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewRideItemAsID",  row["ItemID"].YuanColumnText, SendMessageOptions.DontRequireReceiver);
			}
			else if(row["ItemID"].YuanColumnText.Substring (0,2)=="70")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagDigestItemAsID",  row["ItemID"].YuanColumnText, SendMessageOptions.DontRequireReceiver);
			}
			else if(row["ItemID"].YuanColumnText.Substring (0,2)=="71")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagSoulItemAsID",  row["ItemID"].YuanColumnText, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagItemAsID",  row["ItemID"].YuanColumnText+",01", SendMessageOptions.DontRequireReceiver);
			}			
			 //PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagItemAsID", row["ItemID"].YuanColumnText+",01", SendMessageOptions.DontRequireReceiver);
			PanelStatic.StaticWarnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info501"));
		}
		else
		{
			PanelStatic.StaticWarnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info502"));
		}
	}
	
}
