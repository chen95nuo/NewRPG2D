using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class TowerSceneTest : MonoBehaviour
{
	TowerPlayerRole playerRole;
	void Start()
	{
		// Initialize game modules
		SysModuleManager.Instance.Initialize(this.gameObject);
		SysModuleManager.Instance.AddSysModule<ResourceManager>(true);
		SysModuleManager.Instance.AddSysModule<ResourceCache>(true);
		SysModuleManager.Instance.AddSysModule<AudioManager>(true);
		SysUIEnv uiEnv = SysModuleManager.Instance.AddSysModule<SysUIEnv>(true);
		foreach (var ui in GameDefines.GetAllUIModuleDatas())
			uiEnv.RegisterUIModule(ui.type, ui.prefabName, ui.moduleType, ui.linkedTypes, ui.hideOtherModules, ui.ignoreMutexTypes);

		SysModuleManager.Instance.AddSysModule<SysFx>(true);
		SysModuleManager.Instance.AddSysModule<SysPrefs>(true);

		// Load config
		ConfigDatabase.Initialize(new MathParserFactory(), false, false);
		ConfigDatabase.AddLogger(new ClientServerCommon.UnityLogger());
		ConfigDatabase.DelayLoadFileDel = ConfigDelayLoader.DelayLoadConfig;

		// Load manifest.
		string filePath = ResourceManager.Instance.GetLocalFilePath(PlayerPrefs.GetString("BuildProduct.GameConfigName", ""));
		AssetBundle ab = AssetBundle.CreateFromFile(filePath);
		if (ab == null)
		{
			Debug.LogError("Load Game Config failed : " + filePath);
			return;
		}

		IFileLoader fileLoader = new FileLoaderFromAssetBundle(ab);
		ConfigSetting cfgSetting = GameDefines.SetupConfigSetting(new ConfigSetting(Configuration._FileFormat.Xml));
		ConfigDatabase.DefaultCfg.LoadConfig<ClientManifest>(fileLoader, cfgSetting);

		ab.Unload(true);

		//机关无双
		//playerRole = TowerPlayerRole.Create(0x02320001, true, false, TowerSceneData.Instance.initPathNode, TowerSceneData.Instance.initDoorNode);
		//西蜀石兰
		//playerRole = TowerPlayerRole.Create(0x2120014, true, false, TowerSceneData.Instance.initPathNode, TowerSceneData.Instance.initDoorNode);
		//盖聂
		//playerRole = TowerPlayerRole.Create(0x02220003, true, false, TowerSceneData.Instance.initPathNode, TowerSceneData.Instance.initDoorNode);
		//班大师
		//playerRole = TowerPlayerRole.Create(0x02420002, true, false, TowerSceneData.Instance.initPathNode, TowerSceneData.Instance.initDoorNode);
		//秦始皇
		playerRole = TowerPlayerRole.Create(0x02220024, true, false, TowerSceneData.Instance.initPathNode, TowerSceneData.Instance.initDoorNode);

		TowerSceneData.Instance.melaCamera.towerPlayerRole = playerRole;

		//TowerSceneData.Instance.ReFillFloorData();
	}

	void OnGUI()
	{
		if (GUILayout.Button("MoveToNextDoor"))
		{
			playerRole.MoveToNextDoorNode(1);
		}


		if (GUILayout.Button("MoveToNextDoor4"))
		{
			playerRole.MoveToNextDoorNode(4);
		}


		if (GUILayout.Button("MoveToNextDoor8"))
		{
			playerRole.MoveToNextDoorNode(8);
		}

		if (GUILayout.Button("Set1Floor"))
		{
			TowerSceneData.Instance.melaCamera.SetRotateLimit(1, 1);
		}

		if (GUILayout.Button("Set4Floor"))
		{
			TowerSceneData.Instance.melaCamera.SetRotateLimit(4, 0);
		}

		if (GUILayout.Button("Set8Floor"))
		{
			TowerSceneData.Instance.melaCamera.SetRotateLimit(8, 0);
		}
	}
}

