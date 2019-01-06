using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class SVNHelper
{
	/// <summary>
	/// Get the last error message when GetFileRevision
	/// </summary>
	[DllImport("SVNPlugin")]
	private static extern IntPtr UnityCall_SVN_GetLastErrorMsg();

	/// <summary>
	/// The extern function to get the revision of a file
	/// </summary>
	/// <param name="fileName">File name</param>
	/// <returns>Return the revision of the file. If the revision is 0, use GetLastErrorMsg the check the reason</returns>
	[DllImport("SVNPlugin")]
	private static extern int UnityCall_SVN_GetFileRevision(string fileName);

	public static string GetLastErrorMsg()
	{
		return Marshal.PtrToStringAnsi(UnityCall_SVN_GetLastErrorMsg());
	}

	public static int GetFileRevision(string fileName)
	{
		return UnityCall_SVN_GetFileRevision(fileName);
	}
	
	/// <summary>
	/// Get the highest version number among the files
	/// </summary>
	/// <param name="fileList">File name list</param>
	/// <returns>Return the highest version</returns>
	public static int GetHighestVersionNumberOfFiles(List<string> fileList)
	{
		int highestVersion = 0;

		foreach (string name in fileList)
		{
			int version = UnityCall_SVN_GetFileRevision(name);

			if (version == 0)
				continue;

			highestVersion = Math.Max(highestVersion, version);
		}
		
		return highestVersion;
	}	

	/// <summary>
	/// Testing function
	/// </summary>
	[MenuItem("Asset Test/TestSVN")]
	public static void Test()
	{
		if (Selection.activeObject == null)
		{
			Debug.LogError("Please select a object in project view.");
			return;
		}

		if (AssetDatabase.IsMainAsset(Selection.activeObject) == false)
		{
			Debug.LogError("Please a main asset");
			return;
		}

		string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		Debug.Log("assetPath : " + assetPath);

		int version = GetFileRevision(assetPath);
		Debug.Log(version);
		if (version == 0)
		{
			Debug.LogError("Get File revision failed : " + GetLastErrorMsg());
		}
		else
		{
			Debug.Log("Version : " + version);
		}
	}
}