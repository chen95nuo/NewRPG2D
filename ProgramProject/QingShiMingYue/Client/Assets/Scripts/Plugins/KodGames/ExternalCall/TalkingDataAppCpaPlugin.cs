using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall
{
	public class TalkingDataAppCpaPlugin
	{
#if UNITY_IPHONE
		//[DllImport ("__Internal")]
		//private static extern void UnityCall_AppCpa_Converted();
#endif

		public static void Converted()
		{
#if UNITY_IPHONE		
			//if (Application.platform == RuntimePlatform.IPhonePlayer)
			//    UnityCall_AppCpa_Converted();
#elif UNITY_ANDROID
#endif
		}
	}
}
