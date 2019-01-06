using System;
using System.Collections.Generic;

namespace KodGames
{
	public interface IConsoleCommand
	{
		string Name { get; }
		string Description { get; }
		bool Execute(string[] parameters);
	}
}
