using System;
using System.Collections.Generic;

public class EquipScroll : KodGames.ClientClass.Equipment
{
	private int amount;
	public int Amount
	{
		get { return amount; }
		set { amount = value; }
	}

	public EquipScroll()
	{
		this.Guid = "";
	}
}