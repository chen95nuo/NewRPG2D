using UnityEngine;
using System.Collections;

public class UIElemGuildPointScheduleItem : MonoBehaviour 
{
	public SpriteText rankLabel;
	public SpriteText nameLabel;	
	public SpriteText scheduleLabel;

	public void SetData(string rank, string name, string schedule)
	{
		rankLabel.Text = rank;
		nameLabel.Text = name;
		scheduleLabel.Text = schedule;
	}
}
