#if UNITY_EDITOR || AVATAR_VIEWER
using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.IO;

public class AvatarViewer : MonoBehaviour
{
	static class ConfigDelayLoader
	{
		public static IFileLoader DelayLoadConfig(System.Type configType, out string fileName, out int fileFormat)
		{
			fileName = "";
			fileFormat = Configuration._FileFormat.Xml;

			if (configType == typeof(AvatarConfig))
			{
				return new FileLoaderFromTextAsset(AvatarViewer.Inst.avatarCfgFile);
			}
			else if (configType == typeof(AssetDescConfig))
			{
				return new FileLoaderFromTextAsset(AvatarViewer.Inst.assetdescCfgFile);

			}
			else if (configType == typeof(AnimationConfig))
			{
				return new FileLoaderFromTextAsset(AvatarViewer.Inst.animationCfgFile);

			}
			else if (configType == typeof(AvatarAssetConfig))
			{
				return new FileLoaderFromTextAsset(AvatarViewer.Inst.avatarAssetCfgFile);
			}
			else
				return null;
		}
	}

	private BattleRole role = null;
	private GameObject pfxObj = null;

	private string[] avatarNameStrs = null;
	private int[] avatarIds = null;
	private int preAvatarSelectIndex = -1;
	private int avatarSelectIndex = 0;
	private Vector2 avatarScrollPosition = Vector2.zero;

	private string[] animationNameStrs = null;
	private int preAnimationSelectIndex = -1;
	private int animationSelectIndex = 0;
	private Vector2 animationScrollPosition = Vector2.zero;

	private string[] particleNameStrs = null;
	private int preParticleSelectIndex = 0;
	private int particleSelectIndex = 0;
	private Vector2 particleScrollPosition = Vector2.zero;

	private string[] weaponNameStrs = null;
	private int[] weaponIds = null;
	private int preWeaponSelectIndex = 0;
	private int weaponSelectIndex = 0;
	private AvatarAssetConfig.Part mountPart = null;
	private Vector2 weaponScrollPosition = Vector2.zero;
	private bool leftHandUseWpn = false;
	private bool doubleWp = false;

	//private string[] equipmentStrs = null;
	//private int[] equipmentIds = null;
	//private int preEquipmentSelectIndex = 0;
	//private int equipmentSelectIndex = 0;
	//private Vector2 equipmentScrollPosition = Vector2.zero;

	private GUIStyle listGridStyle;

	private bool toggleLoopAnimation = true;

	private bool showAllUI = true;

	static AvatarViewer inst;
	public static AvatarViewer Inst
	{
		get
		{
			if (inst == null)
				//Unity4.1.2不支持该泛型函数，美工取项目时无法编译
				//inst = GameObject.FindObjectOfType<AvatarViewer>();
				inst = GameObject.FindObjectOfType(typeof(AvatarViewer)) as AvatarViewer;


			return inst;
		}
	}

	public TextAsset avatarCfgFile;
	public TextAsset assetdescCfgFile;
	public TextAsset animationCfgFile;
	public TextAsset avatarAssetCfgFile;

	public Vector3 orbitSensitive = new Vector3(0, 0, 0.3f);

	public float orbitDistance = 4;

	public Camera orbitCamera;

	public float angleV = 0f;
	public float angleHDefault = 0f;

	void Start()
	{
		Initialize();
	}

	private void InitializeLoadConfig()
	{
		// Load config
		ConfigDatabase.Initialize(new MathParserFactory(), false, false);
		ConfigDatabase.DelayLoadFileDel = AvatarViewer.ConfigDelayLoader.DelayLoadConfig;
	}

	private void InitializeAvatarNameStrs()
	{
		avatarNameStrs = new string[ConfigDatabase.DefaultCfg.AvatarConfig.avatars.Count];
		avatarIds = new int[ConfigDatabase.DefaultCfg.AvatarConfig.avatars.Count];
		for (int i = 0; i < ConfigDatabase.DefaultCfg.AvatarConfig.avatars.Count; i++)
		{
			AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.avatars[i];

			string name = ItemInfoUtility.GetAssetName(avatarCfg.id);
			avatarIds[i] = avatarCfg.id;
			avatarNameStrs[i] = name;
		}
	}

	private void InitializeWeaponAssetStrs()
	{
		mountPart = ConfigDatabase.DefaultCfg.AvatarAssetConfig.commonParts.Find(n => n.assembleType == AvatarAssetConfig._AssembleType.Mount);
		if (mountPart == null)
			return;

		weaponNameStrs = new string[mountPart.components.Count];
		weaponIds = new int[mountPart.components.Count];

		for (int i = 0; i < mountPart.components.Count; i++)
		{
			var weaponCfg = mountPart.components[i];

			string name = weaponCfg.asset;
			weaponIds[i] = weaponCfg.id;
			weaponNameStrs[i] = name;
		}
	}

	private void InitializeEquipmentNameStrs()
	{
		//int weaponCount = 0;
		//for (int i = 0; i < ConfigDatabase.DefaultCfg.EquipmentConfig.equipments.Count; i++)
		//{
		//    EquipmentConfig.Equipment equipmentCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.equipments[i];
		//    if (equipmentCfg.type == EquipmentConfig._Type.Weapon)
		//    {
		//        weaponCount++;
		//    }
		//}
		//equipmentStrs = new string[weaponCount];
		//equipmentIds = new int[weaponCount];
		//for (int i = 0; i < ConfigDatabase.DefaultCfg.EquipmentConfig.equipments.Count; i++)
		//{
		//    EquipmentConfig.Equipment equipmentCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.equipments[i];
		//    if (equipmentCfg.type == EquipmentConfig._Type.Weapon)
		//    {
		//        string name = ItemInfoUtility.GetAssetName(equipmentCfg.id);
		//        equipmentIds[i] = equipmentCfg.id;
		//        equipmentStrs[i] = name;
		//    }
		//}
	}

	private void InitializeParticleNameStrs()
	{
		string[] filenames = Directory.GetFiles(Application.dataPath + "/resources/" + GameDefines.pfxPath);
		int count = 0;
		foreach (string fname in filenames)
		{
			string extension = Path.GetExtension(fname);
			if (extension == ".prefab")
			{
				count++;
			}
		}
		particleNameStrs = new string[count];
		int index = 0;
		foreach (string fname in filenames)
		{
			string extension = Path.GetExtension(fname);
			string fileName = Path.GetFileNameWithoutExtension(fname);

			if (extension == ".prefab")
			{
				particleNameStrs[index++] = fileName;
			}
		}
	}

	private void InitializeAnimationNameStrs()
	{
		animationNameStrs = new string[ConfigDatabase.DefaultCfg.AnimationConfig.animations.Count];

		int index = 0;
		foreach (AnimationConfig.Animation animConfig in ConfigDatabase.DefaultCfg.AnimationConfig.animations)
		{
			animationNameStrs[index++] = animConfig.name;
		}
	}

	void Initialize()
	{
		// Initialize game modules
		SysModuleManager.Instance.Initialize(this.gameObject);
		SysModuleManager.Instance.AddSysModule<ResourceManager>(true);

		InitializeLoadConfig();
		InitializeAvatarNameStrs();
		InitializeWeaponAssetStrs();
		InitializeEquipmentNameStrs();
#if !AVATAR_VIEWER
		InitializeParticleNameStrs();
#endif
		InitializeAnimationNameStrs();
	}

	void LoadAvatar(int avatarResourceId)
	{
		if (role != null)
			MonoBehaviour.Destroy(role.gameObject);

		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarResourceId);
		if (avatarCfg == null)
			Debug.LogError(string.Format("Miss Avatar Setting : Id({0:X})", avatarResourceId));

		Avatar avatar = Avatar.CreateAvatar(0x00000000);

		role = avatar.gameObject.AddComponent<BattleRole>();
		KodGames.ClientClass.CombatAvatarData avatarData = new KodGames.ClientClass.CombatAvatarData();
		avatarData.ResourceId = avatarResourceId;

		role.AvatarData = avatarData;
		role.Awake();
		role.AvatarAssetId = avatarCfg.breakThroughs[0].assetId;
		role.Avatar.Load(avatarCfg.breakThroughs[0].assetId, true, false);

		role.UseDefaultWeapons();
	}

	private void OnAvatarGUI()
	{
		avatarScrollPosition = GUI.BeginScrollView(new Rect(0, 205, 90, 200), avatarScrollPosition, new Rect(0, 0, 90, 3000));
		avatarSelectIndex = GUI.SelectionGrid(new Rect(0, 0, 70, 3000), avatarSelectIndex, avatarNameStrs, 1, listGridStyle);
		GUI.EndScrollView();

		if (preAvatarSelectIndex != avatarSelectIndex)
		{
			preAvatarSelectIndex = avatarSelectIndex;
			LoadAvatar(avatarIds[avatarSelectIndex]);
		}
	}

	private void OnAnimationGUI()
	{
		animationScrollPosition = GUI.BeginScrollView(new Rect(0, 0, 210, 200), animationScrollPosition, new Rect(0, 0, 210, 600));
		animationSelectIndex = GUI.SelectionGrid(new Rect(0, 0, 190, 600), animationSelectIndex, animationNameStrs, 1, listGridStyle);
		GUI.EndScrollView();

		if (preAnimationSelectIndex != animationSelectIndex)
		{
			preAnimationSelectIndex = animationSelectIndex;
			role.Avatar.PlayAnim(animationNameStrs[animationSelectIndex]);
		}
	}

	void OnWeaponsGUI()
	{
		weaponScrollPosition = GUI.BeginScrollView(new Rect(Screen.width - 220, 205, 220, 200), weaponScrollPosition, new Rect(0, 0, 220, mountPart.components.Count * 30));
		weaponSelectIndex = GUI.SelectionGrid(new Rect(0, 0, 190, mountPart.components.Count * 30), weaponSelectIndex, weaponNameStrs, 1, listGridStyle);
		GUI.EndScrollView();
		bool needUpdate = false;
		if (GUI.Button(new Rect(Screen.width - 220, 420, 160, 20), "toogleLeftRight"))
		{
			needUpdate = true;
			leftHandUseWpn = !leftHandUseWpn;
		}

		if (GUI.Button(new Rect(Screen.width - 60, 420, 40, 20), "双手"))
		{
			needUpdate = true;
			doubleWp = !doubleWp;
		}

		if (preWeaponSelectIndex != weaponSelectIndex || needUpdate)
		{
			role.Avatar.DeleteComponent(weaponIds[weaponSelectIndex]);
			role.Avatar.DeleteComponent(weaponIds[weaponSelectIndex]);
			preWeaponSelectIndex = weaponSelectIndex;

			if (doubleWp)
			{
				role.Avatar.UseComponent(weaponIds[weaponSelectIndex], "marker_weaponL");
				role.Avatar.UseComponent(weaponIds[weaponSelectIndex], "marker_weaponR");
			}
			else
			{
				if (leftHandUseWpn)
					role.Avatar.UseComponent(weaponIds[weaponSelectIndex], "marker_weaponL");
				else
					role.Avatar.UseComponent(weaponIds[weaponSelectIndex], "marker_weaponR");
			}
		}
	}

	private void OnEquipmentGUI()
	{
		//equipmentScrollPosition = GUI.BeginScrollView(new Rect(Screen.width - 95, 205, 90, 200), equipmentScrollPosition, new Rect(0, 0, 90, 800));
		//equipmentSelectIndex = GUI.SelectionGrid(new Rect(0, 0, 70, 800), equipmentSelectIndex, equipmentStrs, 1);
		//GUI.EndScrollView();

		//if (preEquipmentSelectIndex != equipmentSelectIndex)
		//{
		//    preEquipmentSelectIndex = equipmentSelectIndex;
		//    int equipmentId = equipmentIds[equipmentSelectIndex];

		//    EquipmentConfig.Equipment equipmentCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipmentId);
		//    if (equipmentCfg == null)
		//    {
		//        Debug.LogWarning("Missing equipment setting : " + equipmentId.ToString("X8"));
		//        return;
		//    }

		//    if (equipmentCfg.equipmentAssets.Count == 0)
		//        return;

		//    foreach (var asset in equipmentCfg.equipmentAssets)
		//    {
		//        // Equipment has no asset
		//        if (asset.avatarAssetId == 0)
		//        {
		//            continue;
		//        }
		//        avatar.UseComponent(asset.avatarAssetId, asset.mountBone);
		//    }
		//}
	}

	private void OnParticleGUI()
	{
		particleScrollPosition = GUI.BeginScrollView(new Rect(Screen.width - 240, 0, 240, 200), particleScrollPosition, new Rect(0, 0, 240, 3000));
		particleSelectIndex = GUI.SelectionGrid(new Rect(0, 0, 220, 3000), particleSelectIndex, particleNameStrs, 1, listGridStyle);
		GUI.EndScrollView();

		if (preParticleSelectIndex != particleSelectIndex)
		{
			preParticleSelectIndex = particleSelectIndex;

			if (pfxObj != null)
			{
				role.Avatar.StopPfxByUsd(int.MaxValue);
			}

			pfxObj = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.pfxPath, particleNameStrs[particleSelectIndex]));
			role.Avatar.PlayPfx(pfxObj.GetComponent<FXController>(), AvatarAction.Effect._DestroyType.Buff, int.MaxValue, "", "", true, Vector3.zero, Vector3.zero, false, Vector3.zero);
		}
	}

	private void OnOrbitCtrlGUI()
	{
		orbitSensitive.z = GUI.VerticalSlider(new Rect(Screen.width - 20, Screen.height - 400, 18, 200), orbitSensitive.z, 1, 0);

		GUILayout.Space(Screen.height * 5 / 6);
		GUILayout.BeginHorizontal();
		GUI.skin.button.fixedHeight = 30;

		//垂直旋转

		GUIUtility.RotateAroundPivot(-90, new Vector2(35, Screen.height - 210));
		if (GUI.RepeatButton(new Rect(0, Screen.height - 210, 70, 30), "————>"))
			angleV += orbitSensitive.y;
		GUIUtility.RotateAroundPivot(90, new Vector2(35, Screen.height - 210));

		GUIUtility.RotateAroundPivot(-90, new Vector2(35, Screen.height - 110));
		if (GUI.RepeatButton(new Rect(0, Screen.height - 110, 70, 30), "<————"))
			angleV -= orbitSensitive.y;
		GUIUtility.RotateAroundPivot(90, new Vector2(35, Screen.height - 110));


		//水品旋转
		if (GUI.RepeatButton(new Rect(0, Screen.height - 174, 70, 30), "<————"))
			angleHDefault -= orbitSensitive.x;

		if (GUI.RepeatButton(new Rect(210, Screen.height - 174, 70, 30), "————>"))
			angleHDefault += orbitSensitive.x;

		if (GUI.RepeatButton(new Rect(70, Screen.height - 174, 70, 30), "+Distance"))
			orbitDistance += orbitSensitive.z;

		if (GUI.RepeatButton(new Rect(140, Screen.height - 174, 70, 30), "-Distance"))
			orbitDistance -= orbitSensitive.z;

		GUI.skin.button.fixedHeight = 0;
		GUILayout.EndHorizontal();
	}

	private void OnResetButtonGUI()
	{
		if (GUI.Button(new Rect(0, 410, 90, 30), "重置"))
		{
			if (pfxObj != null)
			{
				role.Avatar.StopPfxByUsd(int.MaxValue);
			}

			role.Avatar.gameObject.transform.rotation = Quaternion.identity;
		}
	}

	private void OnToggleLoopAnimationGUI()
	{
		toggleLoopAnimation = GUI.Toggle(new Rect(0, 445, 90, 30), toggleLoopAnimation, "动画循环");

		if (toggleLoopAnimation)
		{
			role.Avatar.AvatarAnimation.PlayingAnimation.animationState.wrapMode = WrapMode.Loop;
			//avatarAnimation.clip.wrapMode = WrapMode.Loop;
			role.Avatar.PlayAnim(animationNameStrs[animationSelectIndex]);
		}
		else
		{
			role.Avatar.AvatarAnimation.PlayingAnimation.animationState.wrapMode = WrapMode.Default;
		}
	}

	void OnGUI()
	{
		if (listGridStyle == null)
		{
			listGridStyle = new GUIStyle(GUI.skin.button);
			listGridStyle.fixedHeight = 25;
		}

		if (showAllUI)
		{
			if (GUI.Button(new Rect(0, Screen.height - 30, 100, 30), "HideAllUI"))
				showAllUI = false;
		}
		else
		{
			if (GUI.Button(new Rect(0, Screen.height - 30, 100, 30), "ShowAllUI"))
				showAllUI = true;
		}

		if (!showAllUI)
			return;


		// Keep invoke sequence
		OnAvatarGUI();
		OnWeaponsGUI();
		OnAnimationGUI();
		OnEquipmentGUI();
#if !AVATAR_VIEWER
		OnParticleGUI();
#endif
		OnOrbitCtrlGUI();
		OnResetButtonGUI();
		OnToggleLoopAnimationGUI();

	}

	void Update()
	{
		if (role == null)
			return;

		if (orbitDistance < 0.5)
			return;

		if (orbitDistance > 100)
			orbitDistance = 100;
		if (orbitDistance < 0.5f)
			orbitDistance = 0.5f;

		float temp = orbitDistance * Mathf.Cos(angleV * Mathf.Deg2Rad);

		Vector3 RelaVector = Vector3.zero;

		RelaVector.y = orbitDistance * Mathf.Sin(angleV * Mathf.Deg2Rad);
		RelaVector.x = temp * Mathf.Cos(angleHDefault * Mathf.Deg2Rad);
		RelaVector.z = temp * Mathf.Sin(angleHDefault * Mathf.Deg2Rad);

		Vector3 SourcePosi = role.CachedTransform.position;
		SourcePosi.y += 1;

		orbitCamera.transform.position = SourcePosi + RelaVector;
		orbitCamera.transform.LookAt(SourcePosi);
		//CachedTransform.position += fixedOffset;
	}
}
#endif