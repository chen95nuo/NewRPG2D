using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemAnnouncementTitleItem : MonoBehaviour
{
	//Announcement detail.
	public SpriteText titleLabel;
	public UIElemAssetIcon announcementInfoBtn;

	public void SetData(com.kodgames.corgi.protocol.Notice announcement)
	{

		titleLabel.Text = announcement.title;
		announcementInfoBtn.Data = announcement;
		announcementInfoBtn.SetData(announcement.iconId);
		//announcementInfoBtn.SetData(352322050);
		//announcementInfoBtn.SetData(352322052);
		//announcementInfoBtn.SetData(15000204);
	}
}
