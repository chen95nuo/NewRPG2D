using System;
using System.Collections.Generic;

namespace KodGames
{
	public class Command_ToggleAutoScrollConsoloMessage : IConsoleCommand
	{
		public string Name { get { return "AutoScroll"; } }
		public string Description { get { return "Auto scroll console message panel to bottom"; } }
		public bool Execute(string[] parameters)
		{
			DebugConsole.AutoScroll = !DebugConsole.AutoScroll;
			DebugConsole.AddOutputString(Name + " : " + DebugConsole.AutoScroll);
			return true;
		}
	}

	public class Command_ToggleConsoleListenLogOutput : IConsoleCommand
	{
		public string Name { get { return "ListenLogOutput"; } }
		public string Description { get { return "Attach console to log output redirector"; } }
		public bool Execute(string[] parameters)
		{
			DebugConsole.ListenLogOutput = !DebugConsole.ListenLogOutput;
			DebugConsole.AddOutputString(Name + " : " + DebugConsole.ListenLogOutput);
			return true;
		}
	}
}
