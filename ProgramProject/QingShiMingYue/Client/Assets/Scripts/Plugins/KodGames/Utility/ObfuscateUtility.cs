// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Reflection;

namespace KodGames
{
#if UNITY_EDITOR
	public static class ObfuscateUtility
	{
		public static void CheckExcludeMethodName(MethodInfo method)
		{
			CheckExcludeMethodName(method, true);
		}

		public static void CheckExcludeMethodName(MethodInfo method, bool skipPublic)
		{
			if (method == null)
				return;

			// Skip public method
			if (skipPublic && method.IsPublic)
				return;

			// Check attribute
			foreach (var attribute in method.GetCustomAttributes(true))
			{
				if (attribute is ObfuscationAttribute)
				{
					var attri = attribute as ObfuscationAttribute;
					if (attri.Exclude == true && attri.Feature.Contains("renaming"))
						return;
				}
			}

			Debug.LogError(string.Format("Method({0}.{1}) is called by name cannot be obfuscated, add [System.Reflection.Obfuscation(Exclude=false, Feature=\"ExcludeMethodName\")]", method.ReflectedType.Name, method.Name));
		}

		public static void CheckExcludeMethodName(Type type, string methodName, params object[] parameters)
		{
			if (type == null)
				return;

			if (string.IsNullOrEmpty(methodName))
				return;
			
			// Find the method
			Type[] types = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; ++i)
				types[i] = parameters[i].GetType();

			MethodInfo method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, types, null);
			if (method == null)
			{
				Debug.LogWarning(string.Format("Missing method {0} in {1}", methodName, type));
				return;
			}

			CheckExcludeMethodName(method);
		}
	}
#endif
}

