using System;
using System.Collections.Generic;

public class AvatarScroll : KodGames.ClientClass.Avatar
{
	private int amount;
	public int Amount
	{
		get { return amount; }
		set { amount = value; }
	}

	public AvatarScroll()
	{
		this.Guid = "";
	}
}
