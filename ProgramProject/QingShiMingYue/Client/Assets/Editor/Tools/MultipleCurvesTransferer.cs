using UnityEditor;
using UnityEngine;

using System.IO;
using System.Collections;

public class MultipleCurvesTransferer
{
	const string duplicatePostfix = "";
	const string animationFolder = "Assets/WorkAssets/Animations";

	static AnimationClip CreateCopiedClip(string importedPath, string copyPath)
	{
		AnimationClip src = AssetDatabase.LoadAssetAtPath(importedPath, typeof(AnimationClip)) as AnimationClip;
		AnimationClip newClip = new AnimationClip();
		newClip.name = src.name + duplicatePostfix;
		AssetDatabase.CreateAsset(newClip, copyPath);
		AssetDatabase.Refresh();

		return newClip;
	}

	static bool HasSameTypeCurve(AnimationClipCurveData curveData, AnimationClipCurveData[] inCurveDatas)
	{
		foreach (var inCurveData in inCurveDatas)
		{
			if (curveData.path.Equals(inCurveData.path) && curveData.type == inCurveData.type && curveData.propertyName.Equals(inCurveData.propertyName))
			{
				return true;
			}
		}

		return false;
	}

	[MenuItem("Tools/Animation/Transfer Multiple Clips Curves to Copy")]
	static void CopyCurvesToDuplicate()
	{
		// Get selected AnimationClip
		Object[] imported = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Unfiltered);
		if (imported.Length == 0)
		{
			Debug.LogWarning("Either no objects were selected or the objects selected were not AnimationClips.");
			return;
		}

		// If necessary, create the animations folder.
		if (Directory.Exists(animationFolder) == false)
		{
			AssetDatabase.CreateFolder("/", animationFolder);
		}

		foreach (AnimationClip clip in imported)
		{
			string importedPath = AssetDatabase.GetAssetPath(clip);

			// If the animation came from an FBX, then use the FBX name as a sub-folder to contain the animations.
			string copyPath;
			if (importedPath.Contains(".fbx"))
			{
				// With sub-folder.
				string folder = importedPath.Substring(importedPath.LastIndexOf("/") + 1, importedPath.LastIndexOf(".") - importedPath.LastIndexOf("/") - 1);
				if (!Directory.Exists(animationFolder + "/" + folder))
				{
					AssetDatabase.CreateFolder(animationFolder, folder);
				}
				copyPath = animationFolder + "/" + folder + "/" + clip.name + duplicatePostfix + ".anim";
			}
			else
			{
				// No sub-folder
				copyPath = animationFolder + "/" + clip.name + duplicatePostfix + ".anim";
			}

			//Debug.Log("CopyPath: " + copyPath);

			// If copyPath exists, get curve data in old animation clip
			AnimationClip copy = AssetDatabase.LoadAssetAtPath(copyPath, typeof(AnimationClip)) as AnimationClip;
			if (copy == null)
			{
				copy = CreateCopiedClip(importedPath, copyPath);

				if (copy == null)
				{
					Debug.Log("Create animation clip failed : " + copyPath);
					return;
				}
			}

			// Copy curves from imported to copy
			AnimationClipCurveData[] curveDatas = AnimationUtility.GetAllCurves(clip, true);
			foreach (var curveData in curveDatas)
			{
				//Debug.Log(string.Format("Add curve : {0} {1} {2}", oldCurveData.path, oldCurveData.type, oldCurveData.propertyName));
				AnimationUtility.SetEditorCurve(copy, curveData.path, curveData.type, curveData.propertyName, curveData.curve);
			}

			Debug.Log("Copying curves into " + copy.name + " is done");
		}
	}
}