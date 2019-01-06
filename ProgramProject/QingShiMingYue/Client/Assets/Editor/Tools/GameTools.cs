using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class GameTools
{
	private static string sModelInputPath = @"Assets/WorkAssets/Objects/Character";
	private static string sModelOutputPath = @"Assets/Resources/Objects/Character/";

	[MenuItem("Product/Tools/Create Character Prefabs")]
	public static void LoadModels()
	{
		DoLoadModels(sModelInputPath, sModelOutputPath);
	}

	private static void DoClearModelPrefabs(string prefabPath)
	{
		DirectoryInfo dir = new DirectoryInfo(prefabPath);

		// If the source directory does not exist, throw an exception.
		if (!dir.Exists)
			return;

		// Get the file contents of the directory to copy.
		foreach (FileInfo file in dir.GetFiles())
		{
			AssetDatabase.DeleteAsset(Path.Combine(prefabPath, file.Name));
			Debug.Log(file.FullName);
		}

		AssetDatabase.Refresh();
	}

	public static void DoLoadModels(string inputPath, string outputPath)
	{
		DirectoryInfo dir = new DirectoryInfo(inputPath);

		// If the source directory does not exist, throw an exception.
		if (!dir.Exists)
			return;

		var sb = new System.Text.StringBuilder();
		sb.AppendLine("Create Character Prefab :");
		
		
		// Get the file contents of the directory to copy.
		foreach (FileInfo file in dir.GetFiles())
		{
			// Copy the file.
			GameObject prefab = AssetDatabase.LoadAssetAtPath(Path.Combine(inputPath, file.Name), typeof(GameObject)) as GameObject;
			if (prefab == null)
				continue;

			if (PrefabUtility.GetPrefabType(prefab) == PrefabType.None)
				continue;


			string outputPrefabPath = Path.Combine(outputPath, file.Name);
			outputPrefabPath = Path.ChangeExtension(outputPrefabPath, ".prefab");
			
			
			GameObject newPrefeb = (GameObject)GameObject.Instantiate(prefab);
			SkinnedMeshRenderer[] childSkinnedRenders = newPrefeb.GetComponentsInChildren<SkinnedMeshRenderer>();
			MeshRenderer[] childMeshRenders = newPrefeb.GetComponentsInChildren<MeshRenderer>();
			
			
			
			foreach (var childRender in childSkinnedRenders)
			{
				if (childRender.sharedMaterials.Length <= 0)
				{
					Debug.LogError(childRender.name + ": Material Missing");
				}
				else
				{
					for(int index = 0; index < childRender.sharedMaterials.Length; index++)
					{
						var meterial = childRender.sharedMaterials[index];
						if(meterial == null)
							Debug.LogError(childRender.name + ": Material " + index  + " Missing");
					}
				}
				
				if(childRender.sharedMesh == null)
					Debug.LogError(childRender.name + ": Mesh Missing");
				
				if(childRender.rootBone == null)
					Debug.LogError(childRender.name + ": RootBone Missing");
			}
			
			foreach(var childMeshRender in childMeshRenders)
			{
				
				if(childMeshRender.sharedMaterials.Length <= 0)
				{
					Debug.LogError(childMeshRender.name + ": Material Missing");
				}
				else
				{
					for(int index = 0; index < childMeshRender.sharedMaterials.Length; index++)
					{
						var meterial = childMeshRender.sharedMaterials[index];
						if(meterial == null)
							Debug.LogError(childMeshRender.name + ": Material " + index  + " Missing");
					}
				}
				
				MeshFilter meshFilter = childMeshRender.gameObject.GetComponent<MeshFilter>();
				if(meshFilter == null)
				{
					Debug.LogError(childMeshRender.name + ": MeshFilter Missing");
				}
				else
				{
					if(meshFilter.sharedMesh == null)
						Debug.LogError(childMeshRender.name + ":MeshFilter Mesh Missing");
				}
			}
			
			GameObject.DestroyImmediate(newPrefeb);

			//// Skip existing
			//GameObject targetGo = AssetDatabase.LoadAssetAtPath(outputPrefabPath, typeof(GameObject)) as GameObject;
			//if (targetGo != null)
			//    continue;

			sb.AppendLine(outputPrefabPath);


			PrefabUtility.CreatePrefab(outputPrefabPath, prefab);
			
		}

		Debug.Log(sb);
		AssetDatabase.SaveAssets();
	}
}
