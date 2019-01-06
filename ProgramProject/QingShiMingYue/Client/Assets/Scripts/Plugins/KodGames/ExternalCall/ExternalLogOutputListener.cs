using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall
{
	public class ExternalLogOutputListener : ILogOutputListener
	{
#if UNITY_EDITOR || UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern void UnityCall_OutputLog(string log, string stackTrace, int type);
#endif

		public virtual void OnLog(string condition, string stackTrace, LogType type)
		{
#if UNITY_EDITOR || UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				UnityCall_OutputLog(condition, stackTrace, (int)type);
			}
#endif
		}
	}
}
