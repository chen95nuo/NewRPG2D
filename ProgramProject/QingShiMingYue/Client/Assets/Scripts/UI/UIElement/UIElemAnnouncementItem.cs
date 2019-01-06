using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemAnnouncementItem : MonoBehaviour
{
	//Announcement detail.
	public SpriteText contentLabel;

	public void SetData(com.kodgames.corgi.protocol.Notice announcement)
	{
		contentLabel.Text = announcement.content;
	}
}
