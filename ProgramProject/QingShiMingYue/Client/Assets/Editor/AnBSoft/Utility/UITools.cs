using UnityEngine;
using ClientServerCommon;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR && !UNITY_WEBPLAYER
using UnityEditor;

public static class UITools
{
	//[MenuItem("Tools/UI/UI control counts.")]
	//static void UICtrlCounts()
	//{
	//    GameObject root = GameObject.Find("UIContainer");

	//    if (root == null)
	//    {
	//        Debug.LogError("Not found UIContainer in this scene.");
	//        return;
	//    }

	//    Transform[] uis = root.GetComponentsInChildren<Transform>();

	//    Debug.Log("Current scene ui controls count: " + uis.Length);
	//}

	[MenuItem("Tools/UI/Select DisEnable Objs")]
	public static void SelectDisableObjects()
	{
		GameObject cameraGO = GameObject.Find("UIContainer");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectUIButton", "No UIContainer Game Object in the scene", "Ok");
			return;
		}

		List<GameObject> selects = new List<GameObject>();

		foreach (var obj in cameraGO.GetComponentsInChildren<Transform>(true))
		{
			if (obj.gameObject == cameraGO)
				continue;

			var mono = obj.GetComponent<MonoBehaviour>();
			if (mono == null)
				continue;

			if (!mono.enabled)
				selects.Add(obj.gameObject);
		}

		Selection.objects = selects.ToArray();
	}

	[MenuItem("Tools/UI/Select Layer Default")]
	public static void SelectDefaultLayer()
	{
		GameObject cameraGO = GameObject.Find("UIContainer");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectUIButton", "No UIContainer Game Object in the scene", "Ok");
			return;
		}

		List<GameObject> selects = new List<GameObject>();

		foreach (var obj in cameraGO.GetComponentsInChildren<Transform>(true))
		{
			if (obj.gameObject == cameraGO)
				continue;

			if (obj.gameObject.layer == GameDefines.DefaultLayer)
				selects.Add(obj.gameObject);
		}

		Selection.objects = selects.ToArray();
	}

	[MenuItem("Tools/UI/Select Tag")]
	public static void SelectTagedObjects()
	{
		GameObject cameraGO = GameObject.Find("UIContainer");

		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectUIButton", "No UIContainer Game Object in the scene", "Ok");
			return;
		}

		List<GameObject> selects = new List<GameObject>();

		foreach (var obj in cameraGO.GetComponentsInChildren<Transform>(true))
		{
			if (obj.gameObject == cameraGO)
				continue;

			if (!obj.tag.Equals("Untagged"))
				selects.Add(obj.gameObject);
		}

		Selection.objects = selects.ToArray();
	}


	[MenuItem("Tools/UI/Reset UI Offset Z")]
	public static void SetUIOffestZ()
	{
		if (Selection.gameObjects.Length == 0)
			EditorUtility.DisplayDialog("Opps!", "No UI Selected", "OK");

		foreach (var go in Selection.gameObjects)
		{
			foreach (var child in go.GetComponentsInChildren<Transform>(true))
			{
				if (child.gameObject == go)
					continue;

				Vector3 pos = child.localPosition;
				if (child.parent != null && Mathf.Abs(child.parent.eulerAngles.y) > 90)
					pos.z = 0.001f;
				else
					pos.z = -0.001f;
				child.localPosition = pos;
			}

			// Internal offset z should always be zero, offset will be adjust by gameo object z
			foreach (SpriteText st in go.GetComponentsInChildren<SpriteText>(true))
				st.offsetZ = 0;
		}
	}

	[MenuItem("Tools/UI/Reset UI Offset Z", true)]
	static bool ValidateSetUIOffestZ()
	{
		return Selection.activeObject is GameObject && AssetDatabase.Contains(Selection.activeObject) == false;
	}

	static void FormatButton(UIButton button)
	{
		// Set animation
		foreach (UIButton.CONTROL_STATE state in System.Enum.GetValues(typeof(UIButton.CONTROL_STATE)))
		{
			ASCSEInfo stateInfo = button.GetStateElementInfo((int)state);

			while (stateInfo.transitions.list[0].animationTypes.Length < 2)
				stateInfo.transitions.list[0].Add();

			stateInfo.transitions.list[0].animationTypes[0] = EZAnimation.ANIM_TYPE.FadeSprite;
			stateInfo.transitions.list[0].animationTypes[1] = EZAnimation.ANIM_TYPE.FadeText;

			switch (state)
			{
				// There is a bug on ios device, the animation of normal can not be active sometimes.
				case UIButton.CONTROL_STATE.ACTIVE:
					stateInfo.transitions.list[0].animParams[0].color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
					stateInfo.transitions.list[0].animParams[1].color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
					break;
				default:
					stateInfo.transitions.list[0].animParams[0].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					stateInfo.transitions.list[0].animParams[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					break;
			}

			stateInfo.transitions.CloneAll(0);
		}
	}

	static void FormatRadioButton(UIRadioBtn button)
	{
		// Set label
		button.Text = button.Text.ToUpper();
	}

	//[MenuItem("Tools/UI/Set UI Inorder")]
	//static void SetUIInOrder()
	//{
	//    /*
	//     * Test Code
	//     *     */
	//    //if (Selection.activeGameObject == null)
	//    //     return;
	//    //SetPnlUIInOrder(Selection.activeGameObject.transform);

	//    /*
	//     * Product Code
	//     */
	//    GameObject root = GameObject.Find("UIContainer");

	//    if (root == null)
	//        return;

	//    foreach (UIPanel pnl in root.GetComponentsInChildren<UIPanel>())
	//    {
	//        SetPnlUIInOrder(pnl.transform);
	//    }
	//}

	////UI Tree
	//private class UITransNodesContainer
	//{
	//    //UI Tree Node
	//    private class UITransNode
	//    {
	//        public int level;
	//        public Transform baseTransform;
	//        public Transform parentTransform;
	//        public System.Type type = null;

	//        public UITransNode parentNode = null;
	//        public List<UITransNode> childNodeList = new List<UITransNode>();

	//        public UITransNode(int level, Transform baseTransform, Transform parentTransform)
	//        {
	//            this.level = level;
	//            this.baseTransform = baseTransform;
	//            this.parentTransform = parentTransform;
	//        }

	//        //Set node z value
	//        public void SetZValue(Transform root)
	//        {
	//            baseTransform.position = new Vector3(baseTransform.position.x, baseTransform.position.y, root.position.z - 0.001f * level);
	//        }

	//        //Set children nodes level value 
	//        public void ResetLevel(int newLevel)
	//        {
	//            int offsetLv = newLevel - level;
	//            level = newLevel;

	//            Queue<UITransNode> tempNodeList = new Queue<UITransNode>();

	//            foreach (UITransNode node in childNodeList)
	//            {
	//                tempNodeList.Enqueue(node);
	//            }

	//            while (tempNodeList.Count != 0)
	//            {
	//                UITransNode nodeChild = tempNodeList.Dequeue();
	//                nodeChild.level += offsetLv;

	//                foreach (UITransNode node in nodeChild.childNodeList)
	//                {
	//                    tempNodeList.Enqueue(node);
	//                }
	//            }
	//        }

	//        //add child node to nodeList
	//        public void SearchAndSetNodeData(List<UITransNode> nodeList)
	//        {
	//            //Set parentNode
	//            parentNode = GetNodeFromListByTransform(parentTransform, nodeList);

	//            //Add baseNode to  parentNode's childNode List
	//            if (parentNode != null)
	//                parentNode.childNodeList.Add(this);
	//        }

	//        //Get child node  by child transform
	//        private UITransNode GetNodeFromListByTransform(Transform transform, List<UITransNode> nodeList)
	//        {
	//            foreach (UITransNode node in nodeList)
	//            {
	//                if (node.baseTransform == transform)
	//                    return node;
	//            }

	//            return null;
	//        }
	//    }

	//    private List<UITransNode> transNodeList = new List<UITransNode>();
	//    private Dictionary<System.Type, List<UITransNode>> transNodeDic = new Dictionary<System.Type, List<UITransNode>>();
	//    private Dictionary<System.Type, int> speicialTypeHighestZ = new Dictionary<System.Type, int>();
	//    System.Type[] specialTypes = { typeof(UIElemAvatarIcon), typeof(UIElemSkillIcon), typeof(UIElemGameIcon), typeof(UIElemWeaponIcon) };
	//    private int maxLevel = 0;

	//    public UITransNodesContainer()
	//    {
	//        foreach (System.Type type in specialTypes)
	//        {
	//            transNodeDic.Add(type, new List<UITransNode>());
	//        }
	//    }

	//    private void AddNodeSpecialTypesList(UITransNode node)
	//    {
	//        if (node.type == null)
	//            return;

	//        if (!speicialTypeHighestZ.ContainsKey(node.type))
	//            speicialTypeHighestZ.Add(node.type, 0);

	//        if (speicialTypeHighestZ[node.type] < node.level)
	//            speicialTypeHighestZ[node.type] = node.level;

	//        transNodeDic[node.type].Add(node);
	//    }


	//    private bool ContainsSpecialType(UITransNode node)
	//    {
	//        foreach (System.Type type in specialTypes)
	//        {
	//            if (node.baseTransform.GetComponent(type) != null)
	//            {
	//                node.type = type;
	//                return true;
	//            }
	//        }
	//        return false;
	//    }

	//    public bool AddNodeToList(int level, Transform baseTransform, Transform parentTransform)
	//    {
	//        UITransNode node = new UITransNode(level, baseTransform, parentTransform);

	//        if (ContainsSpecialType(node))
	//            AddNodeSpecialTypesList(node);

	//        //set max level
	//        if (node.level > maxLevel)
	//            maxLevel = node.level;

	//        transNodeList.Add(node);

	//        node.SearchAndSetNodeData(transNodeList);

	//        return true;
	//    }

	//    //set and get max node Level
	//    private int GetMaxLevel()
	//    {
	//        int maxLevel = 0;
	//        foreach (var node in transNodeList)
	//        {
	//            if (node.level > maxLevel)
	//                maxLevel = node.level;
	//        }
	//        return maxLevel;
	//    }

	//    //Reset Nodes Z value
	//    public void ResetNodesLoacalPosition(Transform root)
	//    {
	//        //Initial Z value
	//        foreach (var node in transNodeList)
	//        {
	//            node.SetZValue(root);
	//        }

	//        maxLevel = GetMaxLevel();

	//        //Set Special Type Z value
	//        foreach (System.Type type in speicialTypeHighestZ.Keys)
	//        {
	//            foreach (var node in transNodeDic[type])
	//            {
	//                node.ResetLevel(maxLevel + 1);
	//                node.SetZValue(root);
	//                break;
	//            }

	//            maxLevel = GetMaxLevel();
	//        }

	//        //Reset Sprite Text
	//        float highestZ = 10000f;
	//        foreach (Transform z in root.GetComponentsInChildren<Transform>())
	//        {
	//            if (z.position.z < highestZ)
	//                highestZ = z.position.z;
	//        }

	//        ResetUIZValue(typeof(SpriteText), highestZ - 0.001f, root.gameObject);
	//    }

	//}

	///// <summary>
	///// Set Z Value of Single Pnl's children transform 
	///// </summary>
	///// <param name="parent"></param>
	//private static void SetPnlUIInOrder(Transform parent)
	//{
	//    if (parent == null)
	//        return;

	//    Queue<Transform> taskQueue = new Queue<Transform>();

	//    //The Tree Container
	//    UITransNodesContainer nodesContainer = new UITransNodesContainer();

	//    for (int idx = 0; idx < parent.GetChildCount(); idx++)
	//    {
	//        taskQueue.Enqueue(parent.GetChild(idx));
	//    }

	//    while (taskQueue.Count != 0)
	//    {
	//        Transform childTransform = taskQueue.Dequeue();

	//        if (childTransform != null)
	//        {
	//            //Initial tree node to  add it to nodelist
	//            nodesContainer.AddNodeToList(CalculateLevelFromRoot(parent, childTransform), childTransform, childTransform.parent);

	//            for (int idx = 0; idx < childTransform.GetChildCount(); idx++)
	//            {
	//                taskQueue.Enqueue(childTransform.GetChild(idx));
	//            }
	//        }
	//    }

	//    //To reset the tree Z value
	//    nodesContainer.ResetNodesLoacalPosition(parent);
	//}

	//private static int CalculateLevelFromRoot(Transform root, Transform baseTransform)
	//{
	//    int level = 1;
	//    Transform transform = baseTransform;

	//    while (transform.parent != root)
	//    {
	//        level++;
	//        transform = transform.parent;
	//    }

	//    return level;
	//}

	//private static void ResetUIZValue(System.Type type, float zValue, GameObject parentObj)
	//{
	//    foreach (var item in parentObj.GetComponentsInChildren(type))
	//    {
	//        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, zValue);
	//    }
	//}

	[MenuItem("Tools/ScanFileProjectFolder")]
	static void ScanProjectFolder()
	{
		string[] files;
		List<string> strings = new List<string>();
		List<Object> objects = new List<Object>();

		// Stack of folders:
		Stack stack = new Stack();

		// Add root directory:
		stack.Push(Application.dataPath);
		
		// Continue while there are folders to process
		while (stack.Count > 0)
		{
			// Get top folder:
			string dir = (string)stack.Pop();
			
			try
			{
				// Get a list of all prefabs in this folder:
				files = Directory.GetFiles(dir, "*.mat");
				
				// Process all prefabs:
				for (int i = 0; i < files.Length; ++i)
				{
					// Make the file path relative to the assets folder:
					files[i] = files[i].Substring(Application.dataPath.Length - 6);
					
					var obj = AssetDatabase.LoadAssetAtPath(files[i], typeof(Material)) as Material;
					
					if (obj != null)
					{
						if (obj.shader != null && obj.shader.name.StartsWith("Kod") == false && objects.Contains(obj) == false)
						{
							if (strings.Contains(obj.shader.name) == false)
								strings.Add(obj.shader.name);

							objects.Add(obj);
						}
					}
				}
				
				// Add all subfolders in this folder:
				foreach (string dn in Directory.GetDirectories(dir))
				{
					stack.Push(dn);
				}
			}
			catch
			{
				// Error
				Debug.LogError("Could not access folder: \"" + dir + "\"");
			}
		}

		Selection.objects = objects.ToArray();

		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach (var s in strings)
			sb.AppendLine(s);
		Debug.Log(sb.ToString());
	}

	[MenuItem("Tools/SelectObjectWithMaterialProjectFolder")]
	static void SelectObjectWithMaterialProjectFolder()
	{
		var mat = Selection.activeObject as Material;
		if (mat == null)
		{
			EditorUtility.DisplayDialog("", "No material selected", "OK");
			return;
		}

		List<Object> objects = new List<Object>();
		
		// Stack of folders:
		Stack stack = new Stack();
		
		// Add root directory:
		stack.Push(Application.dataPath);
		
		// Continue while there are folders to process
		while (stack.Count > 0)
		{
			// Get top folder:
			string dir = (string)stack.Pop();
			
			try
			{
				// Get a list of all prefabs in this folder:
				foreach (var f in Directory.GetFiles(dir, "*.prefab"))
				{
					var fileName = f.Substring(Application.dataPath.Length - 6);

					var obj = AssetDatabase.LoadAssetAtPath(fileName, typeof(GameObject)) as GameObject;
					if (obj == null)
						continue;

					foreach (var r in obj.GetComponentsInChildren<Component>(true))
					{
						if (!(r is Renderer))
							continue;

						if ((r as Renderer).sharedMaterial == mat && objects.Contains(obj) == false)
						{
							objects.Add(obj);
							break;
						}
					}
				}

				// Get a list of all prefabs in this folder:
				foreach (var f in Directory.GetFiles(dir, "*.fbx"))
				{
					var fileName = f.Substring(Application.dataPath.Length - 6);
					
					var obj = AssetDatabase.LoadAssetAtPath(fileName, typeof(GameObject)) as GameObject;
					if (obj == null)
						continue;
					
					foreach (var r in obj.GetComponentsInChildren<Component>(true))
					{
						if (!(r is Renderer))
							continue;
						
						if ((r as Renderer).sharedMaterial == mat && objects.Contains(obj) == false)
						{
							objects.Add(obj);
							break;
						}
					}
				}

				// Add all subfolders in this folder:
				foreach (string dn in Directory.GetDirectories(dir))
				{
					stack.Push(dn);
				}
			}
			catch
			{
				// Error
				Debug.LogError("Could not access folder: \"" + dir + "\"");
			}
		}

		if (objects.Count == 0)
			EditorUtility.DisplayDialog("", "Not found", "OK");
		else
			Selection.objects = objects.ToArray();
	}

	[MenuItem("Tools/UI/Format button text color")]
	static void FormatButtonTextColor()
	{
		if (Selection.activeGameObject == null)
			return;

		foreach (UIButton btn in Selection.activeGameObject.GetComponentsInChildren<UIButton>())
		{
			for (int i = 0; i < btn.transitions.Length; i++)
			{
				for (int z = 0; z < btn.GetTransitions(i).list.Length; z++)
				{
					bool hasTextFade = false;
					AnimParams animPar = null;
					for (int j = 0; j < btn.GetTransitions(i).list[z].animationTypes.Length; j++)
					{
						if (btn.GetTransitions(i).list[z].animationTypes[j] == EZAnimation.ANIM_TYPE.FadeText)
						{
							hasTextFade = true;
							animPar = btn.GetTransitions(i).list[z].animParams[j];
							break;
						}
					}

					if (!hasTextFade)
						animPar = btn.GetTransitions(i).list[z].AddElement(EZAnimation.ANIM_TYPE.FadeText);

					if (i == 2)
						animPar.color = Color.gray;
					else if (btn.spriteText != null)
						animPar.color = btn.spriteText.Color;
					else
						animPar.color = Color.white;
				}
			}
		}
	}

	[MenuItem("Tools/UI/Format button recursively")]
	static void FormatButtonInChildren()
	{
		if (Selection.activeGameObject == null)
			return;

		//        string[] btnTextureNams = { "button01.tga", "button02.tga", "close.tga", "tab01.tga", "tab02.tga", "back.tga" };

		// Format button
		foreach (UIButton button in Selection.activeGameObject.GetComponentsInChildren<UIButton>())
		{
			FormatButton(button);
			//            ASCSEInfo stateInfo = button.GetStateElementInfo((int)UIButton.CONTROL_STATE.NORMAL);
			//            string texName = AssetDatabase.GUIDToAssetPath(stateInfo.stateObj.frameGUIDs[0]).ToLower();
			//            foreach (string textureName in btnTextureNams)
			//            {
			//                if (texName.EndsWith(textureName))
			//                {
			//                    FormatButton(button);
			//                    break;
			//                }
			//            }
		}

		// Set radio button animation
		foreach (UIRadioBtn button in Selection.activeGameObject.GetComponentsInChildren<UIRadioBtn>())
		{
			FormatRadioButton(button);
			//            ASCSEInfo stateInfo = button.GetStateElementInfo((int)UIRadioBtn.CONTROL_STATE.True);
			//
			//            //Directory.
			//            string texName = AssetDatabase.GetAssetPath(stateInfo.tex).ToLower();
			//            foreach (string textureName in btnTextureNams)
			//            {
			//                if (texName.EndsWith(textureName))
			//                {
			//                    FormatRadioButton(button);
			//                    break;
			//                }
			//            }
		}
	}

	[MenuItem("Tools/UI/Find Used Texture ")]
	static void FindReadableTxture()
	{
		GameObject root = GameObject.Find("UIContainer");
		bool texUsed = false;

		Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + Selection.activeObject.name + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
		foreach (AutoSpriteControlBase item in root.GetComponentsInChildren<AutoSpriteControlBase>())
		{
			item.Aggregate(AssetDatabase.GUIDToAssetPath, AssetDatabase.LoadAssetAtPath, AssetDatabase.AssetPathToGUID);

			foreach (Texture2D tex in item.SourceTextures)
			{
				if (tex != null && tex.name.Equals(Selection.activeObject.name))
				{
					Debug.Log(item.gameObject.name + " : " + item.transform.parent.name);
					texUsed = true;
				}
			}
		}
		if (!texUsed)
			Debug.Log("The texture has not been used in this scene.");

		Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
	}	

	[MenuItem("Tools/UI/Replace UI has empty texture with alpha texture")]
	public static void ReplaseUIWithAlphaTexture()
	{
		GameObject root = GameObject.Find("UIContainer");
		if (root == null)
		{
			EditorUtility.DisplayDialog("", "No UIContainer in scene", "OK");
			return;
		}

		string alphaTexName = @"Assets\WorkAssets\Textures\Interface\alpha.tga";
		if (AssetDatabase.LoadAssetAtPath(alphaTexName, typeof(Texture)) == null)
		{
			EditorUtility.DisplayDialog("", "No alpha texture", "OK");
			return;
		}

		string alphaGUID = AssetDatabase.AssetPathToGUID(alphaTexName);

		List<Object> controlObjects = new List<Object>();
		foreach (var sprite in root.GetComponentsInChildren<AutoSpriteBase>())
			foreach (var state in sprite.States)
				for (int i = 0; i < state.frameGUIDs.Length; ++i)
					if (state.frameGUIDs[i] == null || state.frameGUIDs[i] == "" || AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(state.frameGUIDs[i]), typeof(Texture)) == null)
						state.frameGUIDs[i] = alphaGUID;

		if (controlObjects.Count != 0)
			Selection.objects = controlObjects.ToArray();
	}

	[MenuItem("Tools/UI/Set Missing Material")]
	public static void SetMissingMaterial()
	{
		Material selectMat = Selection.activeObject as Material;
		if (selectMat == null)
		{
			EditorUtility.DisplayDialog("", "No material selected", "OK");
			return;
		}

		GameObject root = GameObject.Find("UIContainer");
		if (root == null)
		{
			EditorUtility.DisplayDialog("", "No UIContainer in scene", "OK");
			return;
		}

		foreach (var sprite in root.GetComponentsInChildren<AutoSpriteBase>())
			if (sprite.gameObject.renderer != null)
				if (sprite.gameObject.renderer.sharedMaterial == null)
					sprite.gameObject.renderer.sharedMaterial = selectMat;
	}

	[MenuItem("Tools/UI/Used UI Texture")]
	public static void GetUsedUITextureAsset()
	{
		GameObject uiRoot = GameObject.Find("UIContainer");
		HashSet<string> usedTexture = new HashSet<string>();

		foreach (var sprite in uiRoot.GetComponentsInChildren<AutoSpriteBase>())
			foreach (var state in sprite.States)
				foreach (var name in state.frameGUIDs)
					if (usedTexture.Contains(name) == false)
						usedTexture.Add(name);

		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach (string item in usedTexture)
			sb.AppendLine(AssetDatabase.GUIDToAssetPath(item));

		Debug.Log(sb);
	}

	[MenuItem("Tools/UI/Format control")]
	static void FormatControl()
	{
		if (Selection.activeGameObject == null)
			return;

		if (Selection.activeGameObject.GetComponent<UIButton>() != null)
		{
			FormatButton(Selection.activeGameObject.GetComponent<UIButton>());
		}
		else if (Selection.activeGameObject.GetComponent<UIRadioBtn>() != null)
		{
			FormatRadioButton(Selection.activeGameObject.GetComponent<UIRadioBtn>());
		}
	}

	[MenuItem("Tools/UI/Format control", true)]
	static bool ValidateFormatControl()
	{
		return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<UIButton>() != null;
	}

	[MenuItem("Tools/UI/Find control")]
	static void FindAssetInScene()
	{
		if (Selection.activeGameObject != null)
		{
			System.Collections.Generic.List<GameObject> selectsGOs = new System.Collections.Generic.List<GameObject>();

			Renderer[] renderers = Selection.activeGameObject.GetComponentsInChildren<Renderer>();
			foreach (var renderer in renderers)
			{
				if (renderer.transform.localScale != Vector3.one)
				//				if (renderer.sharedMaterial != null && renderer.sharedMaterial.name.Contains("UICommon"))
				{
					selectsGOs.Add(renderer.gameObject);
				}
			}

			Selection.objects = selectsGOs.ToArray();
		}
	}

	[MenuItem("Tools/Localization/Select unlocalized game object")]
	static void SelectUnlocalizedGameObject()
	{
		GameObject uiRoot = GameObject.Find("UIContainer");
		if (uiRoot == null)
			return;

		List<GameObject> go = new List<GameObject>();
		SpriteText[] textCtrls = uiRoot.GetComponentsInChildren<SpriteText>();
		foreach (var textCtrl in textCtrls)
		{
			if (textCtrl.localizingKey == "" && textCtrl.Text != "")
				go.Add(textCtrl.gameObject);
		}

		Selection.objects = go.ToArray();
	}

	[MenuItem("Tools/UI/Remove collider in UI object")]
	static void RemoveObjectWithCollider()
	{
		GameObject uiRoot = GameObject.Find("UIContainer");
		if (uiRoot == null)
			return;

		foreach (var bc in uiRoot.GetComponentsInChildren<Collider>())
			Object.DestroyImmediate(bc);
	}

	[UnityEditor.MenuItem("Tools/UI/Set ScreenPlacement Camera")]
	public static void SetBindingCamera()
	{
		GameObject cameraGO = GameObject.Find("UICamera");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("No UICamera found", "No UICamera Game Object in the scene", "Ok");
			return;
		}

		Camera uiCamera = cameraGO.GetComponent<Camera>();
		if (uiCamera == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("No UICamera found", "No UICamera Game Object in the scene", "Ok");
			return;
		}

		if (UnityEditor.Selection.activeGameObject == null)
			return;

		foreach (var go in UnityEditor.Selection.gameObjects)
		{
			foreach (var sp in go.GetComponents<EZScreenPlacement>())
				sp.renderCamera = uiCamera;
			foreach (var sp in go.GetComponentsInChildren<EZScreenPlacement>())
				sp.renderCamera = uiCamera;
		}
	}

	[UnityEditor.MenuItem("Tools/UI/Remove ScreenPlacement Camera")]
	public static void RemoveBindingCamera()
	{
		GameObject cameraGO = GameObject.Find("UICamera");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("No UICamera found", "No UICamera Game Object in the scene", "Ok");
			return;
		}

		Camera uiCamera = cameraGO.GetComponent<Camera>();
		if (uiCamera == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("No UICamera found", "No UICamera Game Object in the scene", "Ok");
			return;
		}

		if (UnityEditor.Selection.activeGameObject == null)
			return;

		foreach (var go in UnityEditor.Selection.gameObjects)
		{
			foreach (var sp in go.GetComponents<SpriteRoot>())
				sp.renderCamera = null;
			foreach (var sp in go.GetComponentsInChildren<SpriteRoot>())
				sp.renderCamera = null;
			foreach (var sp in go.GetComponents<EZScreenPlacement>())
				sp.renderCamera = null;
			foreach (var sp in go.GetComponentsInChildren<EZScreenPlacement>())
				sp.renderCamera = null;
		}
	}

	[UnityEditor.MenuItem("Tools/UI/Remove EZScreenPlacement Component")]
	public static void RemoveEZScreenPlacementComponent()
	{
		if (UnityEditor.Selection.activeGameObject == null)
			return;

		foreach (var sp in UnityEditor.Selection.activeGameObject.GetComponentsInChildren<EZScreenPlacement>(true))
			Object.DestroyImmediate(sp);
	}

	//[MenuItem("Tools/Localization/Find invalid text key")]
	//static void FindInvalidStringKey()
	//{
	//    // Load string config
	//    StringsConfig stringCfg = LoadStringsConfig(@"Assets/Resources/Text/Configs/Text.xml");
	//    if (stringCfg == null)
	//        return;

	//    // Remove ServerError block
	//    stringCfg.StrBlocks.Remove("ServerError");

	//    // Check in all game level 
	//    //		if (false)
	//    {
	//        string[] levelNames = { 
	//            @"Assets/Works/Res/Level/UIBattle.unity",
	//            @"Assets/Works/Res/Level/UICommon.unity",
	//            @"Assets/Works/Res/Level/UIMainMenu.unity",
	//            @"Assets/Works/Res/Level/UISliderShow.unity"
	//        };

	//        foreach (var levelName in levelNames)
	//            if (EditorApplication.OpenScene(levelName))
	//                CheckStringConfigInCurrentScene(stringCfg);
	//    }

	//    // Check in text files (XML,CS)
	//    //		if (false)
	//    {
	//        // Print invalid key
	//        System.Text.StringBuilder sb = new System.Text.StringBuilder();

	//        string dataPath = Directory.GetCurrentDirectory().ToLower();

	//        List<string> textFileList = new List<string>();
	//        GetTextAssetInDirectory("Assets", ref textFileList);
	//        foreach (var textFile in textFileList)
	//        {
	//            Debug.Assert(textFile.IndexOf(dataPath) == 0, "Internal Error");
	//            string fileInDataPath = textFile.Substring(dataPath.Length).TrimStart('/', '\\');

	//            TextAsset file = AssetDatabase.LoadAssetAtPath(fileInDataPath, typeof(TextAsset)) as TextAsset;

	//            List<KeyValuePair<string, string>> validKeys = new List<KeyValuePair<string, string>>();
	//            foreach (var block in stringCfg.StrBlocks)
	//            {
	//                foreach (var kvp in block.Value)
	//                {
	//                    if (file.text.Contains("\"" + kvp.Key + "\""))
	//                    {
	//                        sb.AppendLine(block.Key + " " + kvp.Key + " in " + fileInDataPath);
	//                        validKeys.Add(new KeyValuePair<string, string>(block.Key, kvp.Key));
	//                    }
	//                }
	//            }

	//            // Remove invalid key from string config
	//            foreach (var key in validKeys)
	//                stringCfg.StrBlocks[key.Key].Remove(key.Value);
	//        }

	//        Debug.Log(sb);
	//    }

	//    // Print invalid key
	//    {
	//        System.Text.StringBuilder sb = new System.Text.StringBuilder();
	//        foreach (var block in stringCfg.StrBlocks)
	//        {
	//            if (block.Value.Count == 0)
	//                continue;

	//            sb.AppendLine("Invalid Key in block : " + block.Key);
	//            foreach (var kvp in block.Value)
	//                sb.AppendLine("\t" + kvp.Key);
	//        }

	//        Debug.Log(sb);
	//    }

	//    System.GC.Collect();
	//}

	//static void CheckStringConfigInCurrentScene(StringsConfig stringCfg)
	//{
	//    // Get UI root
	//    GameObject root = GameObject.Find("UIContainer");
	//    if (root == null)
	//    {
	//        Debug.LogError("Not found UIContainer in this scene.");
	//        return;
	//    }

	//    SpriteText[] spriteTexts = root.GetComponentsInChildren<SpriteText>();
	//    foreach (var spriteText in spriteTexts)
	//    {
	//        if (spriteText.localizingKey == "")
	//            continue;

	//        List<KeyValuePair<string, string>> validKeys = new List<KeyValuePair<string, string>>();
	//        foreach (var block in stringCfg.StrBlocks)
	//        {
	//            foreach (var kvp in block.Value)
	//            {
	//                if (kvp.Key == spriteText.localizingKey)
	//                    validKeys.Add(new KeyValuePair<string, string>(block.Key, kvp.Key));
	//            }
	//        }

	//        // Remove invalid key from string config
	//        foreach (var key in validKeys)
	//        {
	//            //				Debug.Log("Find valid string key : " + key.Value);
	//            stringCfg.StrBlocks[key.Key].Remove(key.Value);
	//        }
	//    }
	//}

	//static StringsConfig LoadStringsConfig(string textFile)
	//{
	//    // Load language config from target folder
	//    TextAsset langTxt = AssetDatabase.LoadAssetAtPath(textFile, typeof(TextAsset)) as TextAsset;
	//    if (langTxt == null)
	//    {
	//        Debug.LogError(System.String.Format("Language file not found: {0}", textFile));
	//        return null;
	//    }

	//    // Parse xml
	//    Mono.Xml.SecurityParser xmlParser = new Mono.Xml.SecurityParser();

	//    bool parseSuccess = true;
	//    try
	//    {
	//        xmlParser.LoadXml(langTxt.text);
	//    }
	//    catch (System.Exception ex)
	//    {
	//        Debug.LogError("Parse string config failed : " + textFile + " " + ex.Message);
	//        parseSuccess = false;
	//    }

	//    if (parseSuccess == false)
	//        return null;

	//    // Load config
	//    StringsConfig cfg = new StringsConfig();
	//    cfg.Load(xmlParser.ToXml());

	//    return cfg;
	//}

	//public static void GetTextAssetInDirectory(string root, ref List<string> assets)
	//{
	//    // Skip hidden folder
	//    DirectoryInfo dirInfo = new DirectoryInfo(root);
	//    if ((dirInfo.Attributes & FileAttributes.Hidden) != 0)
	//    {
	//        return;
	//    }

	//    // Go over all file in the directory
	//    FileInfo[] fileInfos = dirInfo.GetFiles();
	//    foreach (FileInfo fileInfo in fileInfos)
	//    {
	//        string ext = fileInfo.Extension;
	//        if (string.Compare(ext, ".cs", true) == 0 || string.Compare(ext, ".xml", true) == 0)
	//        {
	//            if (string.Compare(ext, ".xml", true) == 0 &&
	//                (string.Compare(fileInfo.Name, "text.xml", true) == 0 ||
	//                string.Compare(fileInfo.Name, "text_cn.xml", true) == 0 ||
	//                string.Compare(fileInfo.Name, "text_en.xml", true) == 0))
	//            {
	//                //					Debug.Log("Skip text file : " + fileInfo.FullName);
	//                continue;
	//            }

	//            assets.Add(fileInfo.FullName.ToLower());
	//        }
	//    }

	//    // Go over all sub folder
	//    DirectoryInfo[] subDirInfos = dirInfo.GetDirectories();
	//    foreach (DirectoryInfo subDirInfo in subDirInfos)
	//    {
	//        GetTextAssetInDirectory(Path.Combine(root, subDirInfo.Name), ref assets);
	//    }
	//}

	//[MenuItem("Tools/Get dependencies")]
	//public static void GetDependencies()
	//{
	//    List<string> selPathes = new List<string>();
	//    foreach (var obj in Selection.objects)
	//    {
	//        selPathes.Add(AssetDatabase.GetAssetPath(obj));
	//    }

	//    System.Text.StringBuilder sb = new System.Text.StringBuilder();
	//    foreach (var path in AssetDatabase.GetDependencies(selPathes.ToArray()))
	//    {
	//        if (selPathes.Contains(path) == false && string.Compare(Path.GetExtension(path), ".cs", true) != 0)
	//            sb.AppendLine(path);
	//    }

	//    Debug.Log(sb.ToString());
	//}

	[MenuItem("Tools/UI/Select Text Without Template")]
	public static void SelectTextWithoutTemplate()
	{
		GameObject cameraGO = GameObject.Find("UIContainer");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("Select Text Without Template", "No UICamera Game Object in the scene", "Ok");
			return;
		}

		List<GameObject> selections = new List<GameObject>();
		var templateList = UITemplateFormatter.GetTemplateNameList(typeof(SpriteText), false);
		foreach (var st in cameraGO.GetComponentsInChildren<SpriteText>())
			if (templateList.Contains(st.templateName) == false)
				if (selections.Contains(st.gameObject) == false)
					selections.Add(st.gameObject);

		if (selections.Count != 0)
			Selection.objects = selections.ToArray();
		else
			UnityEditor.EditorUtility.DisplayDialog("Select Text Without Template", "Not Found", "Ok");
	}

	[MenuItem("Tools/UI/Select Disabled UI")]
	public static void SelectDisabledUI()
	{
		GameObject cameraGO = GameObject.Find("UICamera");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectDisabledUI", "No UICamera Game Object in the scene", "Ok");
			return;
		}

		List<GameObject> selections = new List<GameObject>();
		foreach (var st in cameraGO.GetComponentsInChildren<SpriteText>(true))
			if (st.enabled == false)
				selections.Add(st.gameObject);

		foreach (var st in cameraGO.GetComponentsInChildren<SpriteRoot>(true))
			if (st.enabled == false)
				selections.Add(st.gameObject);

		if (selections.Count != 0)
			Selection.objects = selections.ToArray();
		else
			UnityEditor.EditorUtility.DisplayDialog("SelectDisabledUI", "Not Found", "Ok");
	}

	[MenuItem("Tools/UI/Enable UI")]
	public static void EnableUI()
	{
		if (Selection.activeGameObject == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectDisabledUI", "No selection", "Ok");
			return;
		}

		foreach (var st in Selection.activeGameObject.GetComponentsInChildren<SpriteText>(true))
			st.enabled = true;

		foreach (var st in Selection.activeGameObject.GetComponentsInChildren<SpriteRoot>(true))
			st.enabled = true;
	}

	private static bool IsListObject(Transform obj)
	{
		if (obj.GetComponent(typeof(IUIListObject)) != null)
			return true;

		if (obj.parent == null)
			return false;

		return IsListObject(obj.parent);
	}

	[MenuItem("Tools/UI/Set UI Invoke Event")]
	public static void SetUIInvokeEvent()
	{
		foreach (UIActionBtn st in GameObject.FindObjectsOfType(typeof(UIActionBtn)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIBtnChangePanel st in GameObject.FindObjectsOfType(typeof(UIBtnChangePanel)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIBtnLoadScene st in GameObject.FindObjectsOfType(typeof(UIBtnLoadScene)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIBtnWWW st in GameObject.FindObjectsOfType(typeof(UIBtnWWW)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIButton st in GameObject.FindObjectsOfType(typeof(UIButton)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIListButton st in GameObject.FindObjectsOfType(typeof(UIListButton)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIListButton3D st in GameObject.FindObjectsOfType(typeof(UIListButton3D)))
			st.whenToInvoke = POINTER_INFO.INPUT_EVENT.TAP;

		foreach (UIListItem st in GameObject.FindObjectsOfType(typeof(UIListItem)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIPanelTab st in GameObject.FindObjectsOfType(typeof(UIPanelTab)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIRadioBtn st in GameObject.FindObjectsOfType(typeof(UIRadioBtn)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIRadioBtn3D st in GameObject.FindObjectsOfType(typeof(UIRadioBtn3D)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIStateToggleBtn st in GameObject.FindObjectsOfType(typeof(UIStateToggleBtn)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;

		foreach (UIStateToggleBtn3D st in GameObject.FindObjectsOfType(typeof(UIStateToggleBtn3D)))
			st.whenToInvoke = IsListObject(st.transform) ? POINTER_INFO.INPUT_EVENT.TAP : POINTER_INFO.INPUT_EVENT.RELEASE;
	}

	[MenuItem("Tools/UI/Set RemoveUnsupportedCharacters False")]
	public static void SetRemoveUnsupportedCharactersFalse()
	{
		if (Selection.activeGameObject == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectDisabledUI", "No selection", "Ok");
			return;
		}

		foreach (var st in Selection.activeGameObject.GetComponentsInChildren<SpriteText>(true))
			st.removeUnsupportedCharacters = false;


		foreach (var st in Selection.activeGameObject.GetComponentsInChildren<UITextField>(true))
			st.spriteText.removeUnsupportedCharacters = true;
	}

	[MenuItem("Tools/UI/Select RemoveUnsupportedCharacters True SpriteText")]
	public static void SelectRemoveUnsupportedCharactersTrueSpriteText()
	{
		if (Selection.activeGameObject == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectDisabledUI", "No selection", "Ok");
			return;
		}

		List<GameObject> selections = new List<GameObject>();
		foreach (var st in Selection.activeGameObject.GetComponentsInChildren<SpriteText>(true))
		{
			if (st.removeUnsupportedCharacters == true && st.transform.parent.gameObject.GetComponent<UITextField>() == null)
			{
				selections.Add(st.gameObject);
			}
		}

		if (selections.Count != 0)
			Selection.objects = selections.ToArray();
		else
			UnityEditor.EditorUtility.DisplayDialog("SelectCollider", "Not Found", "Ok");
	}

	[MenuItem("Tools/UI/Convert UIButton To UIBox")]
	public static void ConvertUIButtonToUIBox()
	{
		if (Selection.activeGameObject == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectDisabledUI", "No selection", "Ok");
			return;
		}

		foreach (GameObject go in Selection.gameObjects)
		{
			foreach (var st in go.GetComponentsInChildren<AutoSpriteControlBase>(true))
			{
				if (st.gameObject.layer != GameDefines.UIIgnoreRaycastLayer)
					continue;

				// Only convert UIButton
				if (!(st is UIButton))
					continue;

				var uiBox = st.gameObject.AddComponent(typeof(UIBox)) as UIBox;
				uiBox.spriteText = st.spriteText;
				uiBox.Copy(st);

				// Keep "Normal" state
				while (uiBox.states.Length > 1)
					uiBox.RemoveState(uiBox.states.Length - 1);

				GameObject.DestroyImmediate(st);
			}
		}

		
	}

	[MenuItem("Tools/UI/SetScrollListLateUpdate")]
	public static void SetScrollListLateUpdate()
	{
		GameObject cameraGO = GameObject.Find("UICamera");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectDisabledUI", "No UICamera Game Object in the scene", "Ok");
			return;
		}

		List<GameObject> objects = new List<GameObject>();
		foreach (var st in cameraGO.GetComponentsInChildren<UIScrollList>(true))
		{
			st.positionItemsImmediately = false;
			objects.Add(st.gameObject);
		}

		Selection.objects = objects.ToArray();
	}

	[MenuItem("Tools/UI/SelectUIButton")]
	public static void SelectUIButton()
	{
		GameObject cameraGO = GameObject.Find("UIContainer");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectUIButton", "No UIContainer Game Object in the scene", "Ok");
			return;
		}

		List<GameObject> selections = new List<GameObject>();
		foreach (var st in cameraGO.GetComponentsInChildren<UIButton>())
			selections.Add(st.gameObject);
		foreach (var st in cameraGO.GetComponentsInChildren<UIStateToggleBtn>())
			selections.Add(st.gameObject);
		foreach (var st in cameraGO.GetComponentsInChildren<UIRadioBtn>())
			selections.Add(st.gameObject);

		if (selections.Count != 0)
			Selection.objects = selections.ToArray();
		else
			UnityEditor.EditorUtility.DisplayDialog("SelectUIButton", "Not Found", "Ok");
	}

	[MenuItem("Tools/UI/SelectIconWithoutTemplate")]
	public static void SelectIconWithoutTemplate()
	{
		GameObject cameraGO = GameObject.Find("UIContainer");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectUIButton", "No UIContainer Game Object in the scene", "Ok");
			return;
		}
		
		List<GameObject> selections = new List<GameObject>();
		foreach (var icon in cameraGO.GetComponentsInChildren<UIElemAssetIcon>())
		{
			var st = icon.GetComponent<SpriteRoot>();
			if (st == null || UITemplateFormatter.GetTemplateByName(typeof(SpriteRoot), st.templateName) == null)
				selections.Add(icon.gameObject);
		}

		if (selections.Count != 0)
			Selection.objects = selections.ToArray();
		else
			UnityEditor.EditorUtility.DisplayDialog("SelectIconWithoutTemplate", "Not Found", "Ok");
	}

	[MenuItem("Tools/UI/SelectUIBox")]
	public static void SelectUIBox()
	{
		GameObject cameraGO = GameObject.Find("UIContainer");
		if (cameraGO == null)
		{
			UnityEditor.EditorUtility.DisplayDialog("SelectUIButton", "No UIContainer Game Object in the scene", "Ok");
			return;
		}
		
		List<GameObject> selections = new List<GameObject>();
		foreach (var st in cameraGO.GetComponentsInChildren<UIBox>())
			if (st.states.Length > 1)
				selections.Add(st.gameObject);

		if (selections.Count != 0)
			Selection.objects = selections.ToArray();
		else
			UnityEditor.EditorUtility.DisplayDialog("SelectUIButton", "Not Found", "Ok");
	}

	[MenuItem("Tools/UI/SelectCollider")]
	public static void SelectCollider()
	{
		List<GameObject> selections = new List<GameObject>();
		foreach (Collider st in GameObject.FindObjectsOfType(typeof(Collider)))
			selections.Add(st.gameObject);

		if (selections.Count != 0)
			Selection.objects = selections.ToArray();
		else
			UnityEditor.EditorUtility.DisplayDialog("SelectCollider", "Not Found", "Ok");
	}

	[MenuItem("Tools/UI/Change Sprite Size")]
	public static void ChangeSize()
	{
		SpriteRoot[] spriteRoots = Selection.gameObjects[0].GetComponentsInChildren<SpriteRoot>();
		foreach (var spriteRoot in spriteRoots)
		{
			Vector2 spriteSize = new Vector2(spriteRoot.width, spriteRoot.height);
			spriteRoot.width = 1.4f * spriteSize.x;
			spriteRoot.height = 1.4f * spriteSize.y;

			Debug.Log(spriteRoot.width + "/" + spriteRoot.height);
			spriteRoot.UpdateCamera();
		}
	}

	[MenuItem("Tools/UI/Select ScrollList Pos Item Immed")]
	public static void SelectScrollListPosItemImmed()
	{
		List<GameObject> selections = new List<GameObject>();
		foreach (UIScrollList list in GameObject.FindObjectsOfType(typeof(UIScrollList)))
		{
			if (list.positionItemsImmediately == true)
				selections.Add(list.gameObject);
		}

		if (selections.Count != 0)
			Selection.objects = selections.ToArray();
		else
			UnityEditor.EditorUtility.DisplayDialog("SelectScrollList", "Not Found", "Ok");
	}
}

#endif