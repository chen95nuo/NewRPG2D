#if UNITY_EDITOR
#define ENABLE_AVATAR_COMPONENT_LOG
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class AvatarComponent
{
	// Used usedComponent data.
	public class UsedComponent
	{
		public int componentId;	// Style id.
		public string mountBone;
		public GameObject rootObject;	// Component game object.

		public AvatarAssetConfig.Component CommponentCfg
		{
			get { return ConfigDatabase.DefaultCfg.AvatarAssetConfig.GetComponentById(componentId); }
		}

		public UsedComponent(int componentId, string mountBone)
		{
			this.componentId = componentId;
			this.mountBone = mountBone;
		}

		public void Release()
		{
			// Destroy old game object.
			if (rootObject != null)
				GameObject.Destroy(rootObject);
		}
	}

	private Avatar avatar; // The avatar which this usedComponent module belongs to.	
	private Texture sknTex;	// Skin texture.
	private string sknMshName;	// Skin mesh name.
	private List<UsedComponent> usedComponents = new List<UsedComponent>();	// Current used usedComponents list.
	private List<UsedComponent> dlyUsedList = new List<UsedComponent>();	// Delay used usedComponents list.

	public AvatarComponent(Avatar avatar)
	{
		this.avatar = avatar;
	}

	public void Update()
	{
	}

	public void Release()
	{
		sknTex = null;
		sknMshName = "";
		dlyUsedList.Clear();

		foreach (UsedComponent cmp in usedComponents)
			cmp.Release();

		usedComponents.Clear();
	}

	public bool UseComponent(int componentId, string mountBone)
	{
		// Check if the style is for the avatar 
		if (AvatarAssetConfig.IsCommomComponent(componentId) == false && AvatarAssetConfig.ComponentIdToAvatarTypeId(componentId) != avatar.AvatarTypeId)
		{
#if ENABLE_AVATAR_COMPONENT_LOG
			Debug.LogWarning(System.String.Format("This usedComponent {0:X} is not for this avatar {1:X}", componentId, avatar.AvatarTypeId));
#endif
			return false;
		}

		// Create used usedComponent.
		UsedComponent usdCmp = new UsedComponent(componentId, mountBone);

//		// If avatar object not created, delay use.
//		if (avatar.AvatarObject == null)
//		{
//			dlyUsedList.Add(usdCmp);
//			return true;
//		}

		// Process old usedComponent.
		int oldComponentIdx = FindComponentWithSamePartTypeId(usdCmp.componentId, mountBone);

		if (oldComponentIdx >= 0)
		{
			UsedComponent oldCmp = usedComponents[oldComponentIdx];

			// The same usedComponent and style, do nothing.
			if (oldCmp.componentId == usdCmp.componentId)
				return true;

			// Need not add the usedCmp here, it will replace the oldComponent after download.
		}
		else
		{
			// Add this usedComponent.
			usedComponents.Add(usdCmp);
		}

		// Download usedComponent and style.
		OnUseComponentProcess(oldComponentIdx, usdCmp);

		return true;
	}

	public void DeleteCompoent(int componentId)
	{
		int componentIndex = FindComponent(componentId);

		if (componentIndex >= 0)
		{
			usedComponents[componentIndex].Release();
			usedComponents.RemoveAt(componentIndex);
		}
	}

	public GameObject ExtractComponent(int componentId)
	{
		int componentIndex = FindComponent(componentId);

		if (componentIndex >= 0)
		{
			UsedComponent usedComponent = usedComponents[componentIndex];

			if (usedComponent.rootObject != null)
			{
				GameObject obj = usedComponent.rootObject;
				obj.transform.parent = null;

				usedComponent.rootObject = null;

				return obj;
			}
		}

		return null;
	}

	public void SetActiveComponent(int componentId,bool active)
	{
		int componentIndex = FindComponent(componentId);
		if (componentIndex >= 0)
		{
			UsedComponent usedComponent = usedComponents[componentIndex];

			if (usedComponent.rootObject != null)
			{
				usedComponent.rootObject.SetActive(active);
			}
		}
	}

	public string GetComponentAssetPath(int componentId)
	{
		int componentIndex = FindComponent(componentId);
		if (componentIndex >= 0)
		{
			UsedComponent usedComponent = usedComponents[componentIndex];
			if (usedComponent.CommponentCfg != null)
			{
				return PathUtility.Combine(ConfigDatabase.DefaultCfg.AvatarAssetConfig.assetPath, usedComponent.CommponentCfg.asset);
			}
		}

		return "";
	}

	public GameObject CloneComponent(int componentId)
	{
		int componentIndex = FindComponent(componentId);

		if (componentIndex >= 0)
		{
			UsedComponent usedComponent = usedComponents[componentIndex];

			if (usedComponent.rootObject != null)
			{
				GameObject obj = (GameObject)GameObject.Instantiate(usedComponent.rootObject);
				return obj;
			}
		}

		return null;
	}

	public UsedComponent GetUsedComponent(int componentId)
	{
		int componentIndex = FindComponent(componentId);

		if (componentIndex >= 0)
		{
			UsedComponent usedComponent = usedComponents[componentIndex];
			return usedComponent;
		}

		return null;
	}

	public void ReuseDelayStyles()
	{
		foreach (UsedComponent dlyCmp in dlyUsedList)
			UseComponent(dlyCmp.componentId, dlyCmp.mountBone);
	}

	private void OnUseComponentProcess(int oldComponentIndex, UsedComponent usedComponent)
	{
		if (oldComponentIndex >= 0)
		{
			UsedComponent oldComponent = usedComponents[oldComponentIndex];
			oldComponent.Release();
			usedComponents[oldComponentIndex] = usedComponent;
		}

		// Mount mesh.
		if (usedComponent.CommponentCfg == null)
			return;

		if (usedComponent.CommponentCfg.Part.assembleType == AvatarAssetConfig._AssembleType.Mount)
		{
			// Use and save this new usedComponent.
			usedComponent.rootObject = UseMountComponent(usedComponent.CommponentCfg.asset, usedComponent.mountBone, usedComponent.CommponentCfg.Part.mountMarker);

			//// Set usedComponent color with avt color
			//usedComponent.mat.color = avatar.materialColor;
		}
		// Attach mesh.
		else if (usedComponent.CommponentCfg.Part.assembleType == AvatarAssetConfig._AssembleType.Skin)
		{
			// Use and save this new usedComponent.
			usedComponent.rootObject = UseBaseComponent(usedComponent.CommponentCfg.asset);
//			usedComponent.rootObject = UseSkinComponent(usedComponent.CommponentCfg.asset);
		}
	}

	private GameObject CreateComponent(string assetName)
	{
		AvatarAssetConfig avatarAssetCfg = ConfigDatabase.DefaultCfg.AvatarAssetConfig;

		// Create new usedComponent.
		GameObject newComponent = ResourceManager.Instance.InstantiateAsset<GameObject>(PathUtility.Combine(avatarAssetCfg.assetPath, assetName));
		if (newComponent == null)
		{
#if ENABLE_AVATAR_COMPONENT_LOG
			Debug.LogWarning("Failed to create model: " + assetName);
#endif
			return null;
		}

		newComponent.name = assetName;
		return newComponent;
	}

	// Mount style mesh.
	private GameObject MountComponent(Transform mountingObject, string mountBoneName, string mountingMakerName)
	{
		// Find mount mountBone.
		var mountBone = ObjectUtility.FindChildObject(avatar.AvatarObject, mountBoneName);
		if (mountBone == null)
		{
#if ENABLE_AVATAR_COMPONENT_LOG
			Debug.LogWarning("Not found the mount bone on base object : " + mountBoneName);
#endif
			return null;
		}

		// Find mount mountBone in first depth children.
		var mountingBone = ObjectUtility.FindChildObject(mountingObject, mountingMakerName, true);
		if (mountingBone == null)
		{
#if ENABLE_AVATAR_COMPONENT_LOG
			Debug.LogWarning("Not found the mount mountBone in usedComponent model: " + mountingObject.name + " " + mountingMakerName);
#endif
			return null;
		}

		// Create one new object for this usedComponent.
		Transform mountingRoot = new GameObject(mountingObject.gameObject.name).transform;
		ObjectUtility.UnifyWorldTrans(mountingBone, mountingRoot);

		// Attach to mounting root
		ObjectUtility.AttachToParentAndKeepWorldTrans(mountingRoot, mountingObject);

		// Set to layer.
		ObjectUtility.SetObjectLayer(mountingRoot.gameObject, avatar.gameObject.layer);

		// Attach to mount bone.
		Transform avatarTrans = avatar.gameObject.transform;
		Vector3 oldScale = avatarTrans.localScale;
		avatarTrans.localScale = Vector3.one;
		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(mountBone, mountingRoot);
		avatarTrans.localScale = oldScale;

		return mountingRoot.gameObject;
	}

	// Attach style mesh, return the new game object.
	private GameObject AttachSkinComponent(GameObject attachingObject)
	{
		// Create one new object for this mesh renderer.
		GameObject attachingRoot = new GameObject(attachingObject.name);

		// Attach to avatar object.
		Vector3 oldScale = avatar.gameObject.transform.localScale;
		avatar.gameObject.transform.localScale = new Vector3(1, 1, 1);
		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(avatar.AvatarObject, attachingRoot);

		// Copy SkinnedMeshRenderer.
		foreach (SkinnedMeshRenderer renderer in attachingObject.GetComponentsInChildren<SkinnedMeshRenderer>(true))
		{
			// Create one new object for this mesh renderer.
			GameObject newSubMsh = new GameObject(renderer.sharedMesh.name);
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(attachingRoot, newSubMsh);

			// Copy renderer.
			SkinnedMeshRenderer newRenderer = newSubMsh.AddComponent<SkinnedMeshRenderer>();
			newRenderer.sharedMesh = renderer.sharedMesh;
			newRenderer.sharedMaterials = renderer.sharedMaterials;
			newRenderer.quality = renderer.quality;
			newRenderer.updateWhenOffscreen = renderer.updateWhenOffscreen;

			Transform[] bones = new Transform[renderer.bones.Length];
			for (int i = 0; i < renderer.bones.Length; i++)
			{
				var bone = ObjectUtility.FindChildObject(avatar.AvatarObject, renderer.bones[i].name);
				if (bone == null)
				{
					Debug.LogError(string.Format("Bone {0} is missing", renderer.bones[i].name));
					continue;
				}

				bones[i] = bone;
			}

			newRenderer.bones = bones;
		}

		// Reset local scale.
		avatar.gameObject.transform.localScale = oldScale;

		// Set to current layer.
		ObjectUtility.SetObjectLayer(attachingRoot, avatar.gameObject.layer);

		return attachingRoot;
	}

	// Use mounted style.
	private GameObject UseMountComponent(string assetName, string mountBoneName, string mountingMakerName)
	{
		GameObject newComponent = CreateComponent(assetName);
		if (newComponent == null)
			return null;

		GameObject mountingRoot = MountComponent(newComponent.transform, mountBoneName, mountingMakerName);

		if (mountingRoot == null)
			GameObject.Destroy(newComponent);

		return mountingRoot;
	}

	// Use attached style mesh.
	private GameObject UseSkinComponent(string assetName)
	{
		GameObject resCmp = CreateComponent(assetName);
		if (resCmp == null)
			return null;

		// Attach new usedComponent.
		GameObject attachingRoot = AttachSkinComponent(resCmp);

		// Destroy resource usedComponent.
		GameObject.Destroy(resCmp);

		return attachingRoot;
	}

	private GameObject UseBaseComponent(string assetName)
	{
		GameObject resCmp = CreateComponent(assetName);
		if (resCmp == null)
			return null;

		// Create avatar object.
		avatar.AvatarObject = resCmp;

		return resCmp;
	}

	// Try to set the renderer material to skin material.
	private void TryUseSknMat(Renderer mshRnd, Material mat)
	{
		if (mshRnd is ParticleRenderer)
			return;

		mshRnd.material = mat;

		if (mshRnd.name == sknMshName)
		{
			if (mat.HasProperty("_BlendTex"))
				mshRnd.material.SetTexture("_BlendTex", mat.mainTexture);

			mshRnd.material.SetTexture("_MainTex", sknTex);
		}
	}

	private int FindComponentWithSamePartTypeId(int componentId, string mountBone)
	{
		return FindCmpByType(AvatarAssetConfig.ComponentIdToPartTypeId(componentId), mountBone);
	}

	private int FindComponent(int componentId)
	{
		for (int i = 0; i < usedComponents.Count; i++)
		{
			if (usedComponents[i].CommponentCfg.id == componentId)
				return i;
		}

		return -1;
	}

	private int FindCmpByType(int partTypeId, string mountBone)
	{
		for (int i = 0; i < usedComponents.Count; i++)
		{
			if (usedComponents[i].CommponentCfg == null)
			{
				Debug.LogError("CommponentCfg == null partTypeId = " + partTypeId.ToString());
				continue;
			}

			if (usedComponents[i].mountBone.Equals(mountBone) == false)
				continue;

			if (partTypeId == usedComponents[i].CommponentCfg.Part.typeId)
				return i;
		}

		return -1;
	}
}
