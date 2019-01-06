using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class UIObjectEx
{
	// [Add by kingper] Invoke method to pass current object to callback function.
	// All controls will use use method for invoking callback.
	public static bool InvokeMethod( object script, string methodName, object ctrl )
	{
		if ( script == null || methodName == "" )
			return false;
		
		// Find the method
		Type []types = { ctrl.GetType() };
		MethodInfo method = script.GetType().GetMethod( methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, types, null );
		if (method == null)
			return false;

#if UNITY_EDITOR
		// This method name can not be obfuscated
		KodGames.ObfuscateUtility.CheckExcludeMethodName(method);
#endif

		// Call the method
		object[] obj = new object[1];
		obj[0] = ( object )ctrl;
		
		try
		{
			method.Invoke( script, obj );
		}
		catch ( System.Exception e )
		{
			Debug.LogError( e.ToString() );
		}

		return true;
	}
}
