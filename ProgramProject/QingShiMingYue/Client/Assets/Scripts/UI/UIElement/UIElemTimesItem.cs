using UnityEngine;
using System.Collections;
using ClientServerCommon;

using DateTime = System.DateTime;

public class UIElemTimesItem : MonoBehaviour
{
	public SpriteText itemTxt;
	public SpriteText timeOut;
	public SpriteText onOffer;

	private int timeDurationType;
	private System.DateTime startTime;
	private System.DateTime endTime;

	public void SetData(long start, long end, int timeType)
	{
		this.timeDurationType = timeType;
		this.startTime = KodGames.TimeEx.ToLocalDataTime(start);
		this.endTime = KodGames.TimeEx.ToLocalDataTime(end);

		itemTxt.Text = GameUtility.FormatUIString("UIActivityInnTab_OpenningData", startTime, endTime);
	}

	public void RefreshData(DateTime nowTime, DateTime lastGetTime)
	{
		string txtKey = string.Empty;
		Color txtColor = Color.white;
		string ctrlStr = GameUtility.FormatUIString("UIActivityInnTab_OpenningData", startTime, endTime);

		if (KodGames.TimeEx.IsInTimeSpan(nowTime, startTime, endTime, timeDurationType))
		{
			if (KodGames.TimeEx.IsInSameTimeSpan(nowTime, lastGetTime, startTime, endTime, timeDurationType))
				txtKey = "UIPnlActivityInn_Served";
			else
				txtKey = "UIPnlActivityInn_OnServ";

			txtColor = GameDefines.txColorGreen;
		}
		else
		{
			txtKey = "UIPnlActivityInn_NoServ";
			txtColor = GameDefines.txColorRed;
		}

		itemTxt.Text = GameUtility.FormatUIString("UIActivityInnTab_Openning", ctrlStr, txtColor, GameUtility.GetUIString(txtKey));
	}
}
