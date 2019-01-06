using System;
using System.Collections.Generic;

public class SkillComposite : KodGames.ClientClass.Skill
{
	private int avatarResourceId;
	public int AvatarResourceId
	{
		set { avatarResourceId = value; }
		get { return avatarResourceId; }
	}

	private int avatarBreakLevel;
	public int AvatarBreakLevel
	{
		set { avatarBreakLevel = value; }
		get { return avatarBreakLevel; }
	}
}