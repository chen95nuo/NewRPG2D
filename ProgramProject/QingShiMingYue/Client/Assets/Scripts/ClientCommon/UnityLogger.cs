using System;

namespace ClientServerCommon
{
	public class UnityLogger : ILogger
	{
		public void Debug(object msg)
		{
			global::Debug.Log(msg);
		}

		public void Debug(string format, params object[] args)
		{
			global::Debug.Log(string.Format(format, args));
		}		

		public void Info(object msg)
		{
			global::Debug.LogTrace(msg);
		}

		public void Info(string format, params object[] args)
		{
			global::Debug.LogTrace(string.Format(format, args));
		}

		public void Warn(object msg)
		{
			global::Debug.LogWarning(msg);
		}

		public void Warn(string format, params object[] args)
		{
			global::Debug.LogWarning(string.Format(format, args));
		}

		public void Error(object msg)
		{
			global::Debug.LogError(msg);
		}

		public void Error(string format, params object[] args)
		{
			global::Debug.LogError(string.Format(format, args));
		}
	}
}
