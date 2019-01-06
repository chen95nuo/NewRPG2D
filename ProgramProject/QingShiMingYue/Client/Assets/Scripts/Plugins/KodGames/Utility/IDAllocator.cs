using System;
using System.Collections;
using System.Collections.Generic;

public class IDAllocator
{
	public int NewID()
	{
		do
		{
			idCounter++;

			if (idCounter >= Int32.MaxValue)
			{
				idCounter = idStart;
				idLoops++;
			}

		} while (idLoops > 0 && usedIDs.Contains(idCounter));

		usedIDs.Add(idCounter);

		return idCounter;
	}

	public void ReleaseID(int id)
	{
		if (id == InvalidID)
			return;

		usedIDs.Remove(id);
	}

	public const int InvalidID = 0;
	protected const int idStart = InvalidID + 1;
	protected int idCounter = idStart;
	protected int idLoops = 0;
	protected List<int> usedIDs = new List<int>();
}
