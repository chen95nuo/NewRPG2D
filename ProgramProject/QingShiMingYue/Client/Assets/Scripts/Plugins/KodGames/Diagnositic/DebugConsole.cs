using UnityEngine;
using System;
using System.Collections.Generic;

namespace KodGames
{
	/// <summary>
	/// Console for debugging
	/// </summary>
	public class DebugConsole
	{
		/// <summary>
		/// Whether debug console is active
		/// </summary>
		public static bool Active { get { return mActive; } set { mActive = value; } }

		/// <summary>
		/// Auto scroll message panel to the bottom
		/// </summary>
		public static bool AutoScroll { get { return mAutoScroll; } set { mAutoScroll = value; } }

		/// <summary>
		/// Field to toggle log listener
		/// </summary>
		public static bool ListenLogOutput
		{
			get { return mListenLogOutput; }
			set
			{
				mListenLogOutput = value;
				if (mListenLogOutput)
				{
					Debug.RegisterLogOutputListener(mLogOutputListener);
				}
				else
				{
					Debug.UnregisterLogOutputListener(mLogOutputListener);
				}
			}
		}

		/// <summary>
		/// Initialize debug console
		/// </summary>
		public static void Initialize()
		{
			// Return if has be initialized
			if (mIsInitialized)
				return;

			// Initialize fields
			Active = false;
			ListenLogOutput = false;

			// Register default command
			RegisterCommand(new Command_ToggleAutoScrollConsoloMessage());
			RegisterCommand(new Command_ToggleConsoleListenLogOutput());

			mIsInitialized = true;
		}

		/// <summary>
		/// Register a command
		/// </summary>
		/// <param name="command">Command to register</param>
		/// <returns>Return true if the command is succeed registering, otherwise return false</returns>
		public static bool RegisterCommand(IConsoleCommand command)
		{
			string nameLower = command.Name.ToLower();

			// Can not register two command with the same name
			if (mCommands.ContainsKey(nameLower))
			{
				Debug.Log(string.Format("Command with name{0} already exists.", nameLower));
				return false;
			}

			mCommands[nameLower] = command;
			return true;
		}

		/// <summary>
		/// Add a string to console panel
		/// </summary>
		/// <param name="message">string to display</param>
		public static void AddOutputString(object message)
		{
			// If has reached max count, remove the oldest
			if (mOutputStrings.Count == MaxMessageCount)
			{
				mOutputStrings.RemoveFirst();
			}

			mOutputStrings.AddLast(message == null ? "null" : message.ToString());
		}

		public static void Update()
		{
			if (Input.GetKeyDown(KeyCode.BackQuote))
			{
				Active = !Active;
			}

			if (Active == false)
				return;

			//////////////////////////////////////////////////////////////////////////
			// Process command.
			if (Input.GetKeyDown(KeyCode.Return))
			{
				ProcessCommand(mStringInTextField);
				mStringInTextField = "";
			}

			//////////////////////////////////////////////////////////////////////////
			// Show last command in last command list
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (mLastCommandIter == null)
				{
					mLastCommandIter = mLastCommands.First;
				}
				else if (mLastCommandIter.Next != null)
				{
					mLastCommandIter = mLastCommandIter.Next;
				}

				if (mLastCommandIter != null)
				{
					mStringInTextField = mLastCommandIter.Value;
				}
			}

			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				if (mLastCommandIter == null)
				{
					mLastCommandIter = mLastCommands.First;
				}
				else if (mLastCommandIter.Previous != null)
				{
					mLastCommandIter = mLastCommandIter.Previous;
				}

				if (mLastCommandIter != null)
				{
					mStringInTextField = mLastCommandIter.Value;
				}
			}

			//////////////////////////////////////////////////////////////////////////
			// Scroll controller
			if (AutoScroll)
			{
				mOutScrPos.y = Mathf.Infinity;
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.PageDown)) mOutScrPos.y += ScrollStep;
				if (Input.GetKeyDown(KeyCode.PageUp)) mOutScrPos.y -= ScrollStep;
				if (Input.GetKeyDown(KeyCode.End)) mOutScrPos.y = Mathf.Infinity;
				if (Input.GetKeyDown(KeyCode.Home)) mOutScrPos.y = Mathf.NegativeInfinity;
			}

			//foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
			//{
			//    if (Input.GetKeyDown(code))
			//    {
			//        Debug.Log(code);
			//    }
			//}
		}

		public static void OnGUI()
		{
			if (Active == false)
				return;

			int panelHeight = Screen.height / ScreenPanelRate;

			// Draw panel
			GUI.Box(new Rect(0, 0, Screen.width, panelHeight), "");

			// Draw input control
			mStringInTextField = GUI.TextField(new Rect(0, panelHeight - LineHeight, Screen.width, LineHeight), mStringInTextField, MaxInputContent);

			// Draw message list
			mOutScrPos = GUILayout.BeginScrollView(mOutScrPos, GUILayout.Width(Screen.width), GUILayout.Height(panelHeight - LineHeight));

			LinkedList<string>.Enumerator iter = mOutputStrings.GetEnumerator();
			while (iter.MoveNext())
			{
				GUILayout.Label(iter.Current, GUILayout.Width(Screen.width - LineWidthOffet));
			}

			GUILayout.EndScrollView();
		}

		/// <summary>
		/// Prevent instantiating
		/// </summary>
		private DebugConsole() { }

		/// <summary>
		/// Parse and process a command
		/// </summary>
		/// <param name="commandWithParam">command line</param>
		private static void ProcessCommand(string commandWithParam)
		{
			string[] parameters = commandWithParam.Split(new char[] { ' ' });

			if (parameters.Length == 0)
				return;

			string commandName = parameters[0].ToLower();
			if (mCommands.ContainsKey(commandName))
			{
				if (mCommands[commandName].Execute(parameters))
				{
					// Add to last processed command list
					AddToLastCommandList(commandWithParam);
				}
			}
		}

		protected static void AddToLastCommandList(string command)
		{
			if (mLastCommands.Count == MaxLastCommandCount)
			{
				mLastCommands.RemoveLast();
			}

			mLastCommandIter = null;
			mLastCommands.AddFirst(command);
		}

		/// <summary>
		/// Listener to receive log.
		/// </summary>
		class LogOutputListener : ILogOutputListener
		{
			public void OnLog(string condition, string stackTrace, LogType type)
			{
				AddOutputString(condition);
			}
		}

		private static int MaxInputContent { get { return 256; } }
		private static int LineHeight { get { return 20; } }
		private static int LineWidthOffet { get { return 25; } }
		private static int ScreenPanelRate { get { return 1; } }
		private static float ScrollStep { get { return 10.0f; } }
		private static int MaxMessageCount { get { return 3; } }
		private static int MaxLastCommandCount { get { return 10; } }

		private static bool mIsInitialized = false;
		private static bool mActive = false;
		private static bool mAutoScroll = true;

		private static LinkedList<string> mOutputStrings = new LinkedList<string>();
		private static string mStringInTextField = "";
		private static Vector2 mOutScrPos = new Vector2(0, Mathf.Infinity);

		private static bool mListenLogOutput = false;
		private static LogOutputListener mLogOutputListener = new LogOutputListener();

		private static Dictionary<string, IConsoleCommand> mCommands = new Dictionary<string, IConsoleCommand>();

		private static LinkedList<string> mLastCommands = new LinkedList<string>();
		private static LinkedListNode<string> mLastCommandIter;
	}
}

