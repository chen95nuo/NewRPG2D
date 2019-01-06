using System;
using System.Collections.Generic;
using KodGames;

public class Debug
{
	public enum LogLevel
	{
		Trace,
		Log,
		Warning,
		Error,
		Fatal
	}

	/// <summary>
	/// Initialize debug system
	/// </summary>
	public static void Initialize()
	{
		if (mInitialized)
			return;

		m_logLevelEnabled[LogLevel.Trace] = true;
		m_logLevelEnabled[LogLevel.Log] = true;
		m_logLevelEnabled[LogLevel.Warning] = true;
		m_logLevelEnabled[LogLevel.Error] = true;
		m_logLevelEnabled[LogLevel.Fatal] = true;

		mInitialized = true;
	}

	public static void Release()
	{
		UnregisterAllLogOutputListener();
	}
	
	public static bool isDebugBuild
	{
		get { return UnityEngine.Debug.isDebugBuild; }
	}
	
	/// <summary>
	/// Enable/Disable the specific log level
	/// </summary>
	/// <param name="logLevel">Log level to be set</param>
	/// <param name="enable">Enable/Disable</param>
	public static void EnableLogLevel(LogLevel logLevel, bool enable)
	{
		Initialize();
		m_logLevelEnabled[logLevel] = enable;
	}

	/// <summary>
	/// Check if the specific log level enabled.
	/// </summary>
	/// <param name="logLevel">Log level to check</param>
	/// <returns>Enable/Disable</returns>
	public static bool IsLogLevelEnabled(LogLevel logLevel)
	{
		Initialize();
		return m_logLevelEnabled[logLevel];
	}

	/// <summary>
	/// Log a message at trace level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void LogTrace(object msg)
	{
		Log(LogLevel.Trace, msg);
	}

	/// <summary>
	/// Log a message at log level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void Log(object msg)
	{
		Log(LogLevel.Log, msg);
	}

	/// <summary>
	/// Log a message at warning level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void LogWarning(object msg)
	{
		Log(LogLevel.Warning, msg);
	}

	/// <summary>
	/// Log a message at error level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void LogError(object msg)
	{
		Log(LogLevel.Error, msg);
	}

	/// <summary>
	/// Log a message at fatal level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void LogFatal(object msg)
	{
		Log(LogLevel.Error, msg);
	}
	
	/// <summary>
	/// Assert with default message.
	/// </summary>
	public static void Assert(bool condition)
	{
		Assert(condition, "Internal Error");
	}
	
	/// <summary>
	/// Asset if condition is false
	/// </summary>
	/// <param name="condition">Condition</param>
	/// <param name="msg">Message to display</param>
	public static void Assert(bool condition, string msg)
	{
		if (!condition)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine(msg);

			// Output stack
			sb.AppendLine("Stack:");
			System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
			System.Diagnostics.StackFrame[] stackFrames = stackTrace.GetFrames();
			foreach (var stackFrame in stackFrames)
			{
				sb.AppendFormat("{0} : {1} : {2}", stackFrame.GetFileName(), stackFrame.GetMethod().Name, stackFrame.GetFileLineNumber());
				sb.AppendLine();
			}
			
			Debug.LogError(sb);
			
//			throw new checkNoEntryException(msg);
		}
	}

	/// <summary>
	/// Register a listener to receive log
	/// </summary>
	/// <param name="listener">listener</param>
	public static void RegisterLogOutputListener(ILogOutputListener listener)
	{
		if (mLogOutputListeners.Contains(listener) == false)
		{
			mLogOutputListeners.Add(listener);
		}

		if (mLogOutputListeners.Count != 0)
		{
			UnityEngine.Application.RegisterLogCallback(LogCallback);
		}
	}

	/// <summary>
	/// Unregister a log listener
	/// </summary>
	/// <param name="listener">listener</param>
	public static void UnregisterLogOutputListener(ILogOutputListener listener)
	{
		mLogOutputListeners.Remove(listener);

		if (mLogOutputListeners.Count == 0)
		{
			UnityEngine.Application.RegisterLogCallback(null);
		}
	}

	/// <summary>
	/// Unregister all log listeners
	/// </summary>
	/// <param name="listener">listener</param>
	public static void UnregisterAllLogOutputListener()
	{
		mLogOutputListeners.Clear();
		UnityEngine.Application.RegisterLogCallback(null);
	}

	/// <summary>
	/// Prevent instantiating
	/// </summary>
	private Debug() { }

	private static void Log(LogLevel logLevel, object msg)
	{		
		if (IsLogLevelEnabled(logLevel))
		{
			string logMessage = "";
			logMessage += string.Format("[{1}][{0}] ", logLevel, UnityEngine.Time.realtimeSinceStartup);
			logMessage += msg == null ? "null" : msg.ToString();

			switch (logLevel)
			{
			case LogLevel.Trace:
			case LogLevel.Log:
				UnityEngine.Debug.Log(logMessage);
				break;
			case LogLevel.Warning:
				UnityEngine.Debug.LogWarning(logMessage);
				break;
			case LogLevel.Error:
				UnityEngine.Debug.LogError(logMessage);
				break;
			case LogLevel.Fatal:
				UnityEngine.Debug.LogError(logMessage);
				UnityEngine.Debug.Break();
				break;
			}
		}
	}

	private static void LogCallback(string condition, string stackTrace, UnityEngine.LogType type)
	{
		foreach (ILogOutputListener linstener in mLogOutputListeners)
		{
			linstener.OnLog(condition, stackTrace, type);
		}
	}

	private static bool mInitialized = false;
	private static Dictionary<LogLevel, bool> m_logLevelEnabled = new Dictionary<LogLevel, bool>();
	private static List<ILogOutputListener> mLogOutputListeners = new List<ILogOutputListener>();
}
