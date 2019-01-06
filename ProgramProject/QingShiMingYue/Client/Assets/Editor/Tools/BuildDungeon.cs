using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using System.IO;
using ClientServerCommon;
using System.Text;
using System.Xml;
using KodGames;
using System;
using System.Globalization;

namespace KodGames.WorkFlow
{
	public class BuildDungeon : ScriptableWizard
	{
		private static string DungeonConfig = "Assets/WorkAssets/Texts/GameConfig/CampaignConfig.xml";

		private int locationNameIndex;  // LocationKey index
		private int lastLocationNameIndex = -1;

		private List<string> locationNames = new List<string>(); //List of LocationKey

		private List<MyVector3> dungeonLocations = new List<MyVector3>();
		private List<MyVector3> dungeonShopLocations = new List<MyVector3>();
		private List<int> dungeonIds = new List<int>();
		private List<int> dungeonShopIds = new List<int>();
		private List<UIElemDungeonMapIcon> dungeonMapIcons = new List<UIElemDungeonMapIcon>();
		private List<UIElemDungeonShopIcon> dungeonShopIcons = new List<UIElemDungeonShopIcon>();

		private bool init = false;

		private UIElementDungeonItem _dungeonItem;
		private UIElementDungeonItem DungeonItem
		{
			get
			{
				if (_dungeonItem == null)
				{
					GameObject container = GameObject.Find("UIContainer");
					if (container == null)
						Debug.LogError("UIContainer is not found!");
					else
						_dungeonItem = container.GetComponentInChildren<UIElementDungeonItem>();
				}

				return _dungeonItem;
			}
		}

		private CampaignConfig campaignConfig;

		[MenuItem("Tools/Dungeon Location Setting")]
		public static void LocalizeMenuItem()
		{
			BuildDungeon.ShowPanel();
		}

		public static void ShowPanel()
		{
			BuildDungeon buildDungeon = ScriptableWizard.DisplayWizard<BuildDungeon>("Dungeon Location Setting");
			buildDungeon.LoadLocationConfig();
		}

		public void OnDestroy()
		{
			init = false;
			dungeonLocations.Clear();
			locationNames.Clear();

			lastLocationNameIndex = -1;
			locationNameIndex = 0;

			for (int index = dungeonMapIcons.Count - 1; index >= 0; index--)
			{
				var mapIcon = dungeonMapIcons[index];
				dungeonMapIcons.RemoveAt(index);

				if (mapIcon != null)
					GameObject.DestroyImmediate(mapIcon.gameObject);
			}

			for (int index = dungeonShopIcons.Count - 1; index >= 0; index--)
			{
				var shopIcon = dungeonShopIcons[index];
				dungeonShopIcons.RemoveAt(index);

				if (shopIcon != null)
					GameObject.DestroyImmediate(shopIcon.gameObject);
			}

			DungeonItem.mapIconPool.gameObject.SetActive(true);
			DungeonItem.mapShopPool.gameObject.SetActive(true);

			EditorApplication.SaveScene(EditorApplication.currentScene);
		}

		public void OnGUI()
		{
			if (!init)
				return;

			string errorMsg = "";
			if (Application.isPlaying)
				errorMsg = "Can not use this tool when playing";
			else
			{
				locationNameIndex = EditorGUILayout.Popup("Position Name", locationNameIndex, locationNames.ToArray());

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Positions:");

				if (lastLocationNameIndex != locationNameIndex)
				{
					lastLocationNameIndex = locationNameIndex;
					GetDungeonLocation(locationNameIndex);
				}

				for (int index = 0; index < dungeonLocations.Count; index++)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("小副本" + (index + 1));
					try
					{
						EditorGUILayout.LabelField(dungeonLocations[index].x.ToString());
						EditorGUILayout.LabelField(dungeonLocations[index].y.ToString());
						EditorGUILayout.LabelField(dungeonLocations[index].z.ToString());
					}
					catch (System.Exception e)
					{
						Debug.Log(e.ToString());
					}
					finally
					{
						EditorGUILayout.EndHorizontal();
					}
				}

				for (int index = 0; index < dungeonShopLocations.Count; index++)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(string.Format("小副本{0}云游", dungeonIds.IndexOf(dungeonShopIds[index]) + 1));
					try
					{
						EditorGUILayout.LabelField(dungeonShopLocations[index].x.ToString());
						EditorGUILayout.LabelField(dungeonShopLocations[index].y.ToString());
						EditorGUILayout.LabelField(dungeonShopLocations[index].z.ToString());
					}
					catch (System.Exception e)
					{
						Debug.Log(e.ToString());
					}
					finally
					{
						EditorGUILayout.EndHorizontal();
					}
				}

				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();

				if (GUILayout.Button("LoadLocation"))
					LoadLocations();

				if (GUILayout.Button("SetLocation"))
					SetLocations();

				if (GUILayout.Button("SaveLocation"))
					SaveLocations();

				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("", errorMsg);
		}

		private void GetDungeonLocation(int dungeonIndex)
		{
			var dataStr = locationNames[dungeonIndex].Split('_');
			int zoneId = 0;

			if (Int32.TryParse(dataStr[dataStr.Length - 2], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out zoneId) == false)
			{
				Debug.Log("Invalid PositionName : " + locationNames[dungeonIndex] + " ZoneId InValid : " + dataStr[dataStr.Length - 2]);
				return;
			}

			int zoneDiffcult = _DungeonDifficulity.Parse(dataStr[dataStr.Length - 1], _DungeonDifficulity.Unknow);
			if (zoneDiffcult == _DungeonDifficulity.Unknow)
			{
				Debug.Log("Invalid PositionName : " + locationNames[dungeonIndex] + " Zone Difficult InValid : " + dataStr[dataStr.Length - 1]);
				return;
			}

			var zoneCfg = campaignConfig.GetZoneById(zoneId);
			if (zoneCfg == null)
			{
				Debug.Log("Invalid PositionName : " + locationNames[dungeonIndex]);
				return;
			}

			var dungeonDifficult = zoneCfg.GetDungeonDifficultyByDifficulty(zoneDiffcult);
			if (dungeonDifficult == null)
			{
				Debug.Log("Invalid PositionName : " + locationNames[dungeonIndex] + " Not this Difficult in Zone " + zoneId.ToString("X8"));
				return;
			}

			// Get ShopInfo.
			List<CampaignConfig.TravelTrader> travelTraders = new List<CampaignConfig.TravelTrader>();
			foreach (var dungeon in dungeonDifficult.dungeons)
			{
				var traverTrader = campaignConfig.GetTravelTradeByDungeonId(dungeon.dungeonId);

				if (traverTrader == null)
					continue;

				travelTraders.Add(traverTrader);
			}

			// Init Data.
			dungeonIds.Clear();
			foreach (var dungeon in dungeonDifficult.dungeons)
				dungeonIds.Add(dungeon.dungeonId);

			dungeonShopIds.Clear();
			foreach (var shop in travelTraders)
				dungeonShopIds.Add(shop.dungeonId);

			// Get Serialize Data.
			AssetDatabase.Refresh();
			SerializeDungeonData dungeonData = SerializeDungoenTools.GetSerializeDataFromFile(locationNames[dungeonIndex]);

			if (dungeonData == null)
				dungeonData = new SerializeDungeonData(locationNames[dungeonIndex]);

			// Set MapIcon Data.
			dungeonLocations.Clear();
			foreach (var location in dungeonData.locations)
				dungeonLocations.Add(new MyVector3(location.ConvertToVector3()));

			if (dungeonLocations.Count > dungeonDifficult.dungeons.Count)
				for (int index = dungeonLocations.Count - 1; index >= dungeonDifficult.dungeons.Count; index--)
					dungeonLocations.RemoveAt(index);
			else if (dungeonLocations.Count < dungeonDifficult.dungeons.Count)
				for (int index = dungeonLocations.Count; index < dungeonDifficult.dungeons.Count; index++)
					dungeonLocations.Add(new MyVector3());

			if (dungeonMapIcons.Count > dungeonDifficult.dungeons.Count)
			{
				for (int index = dungeonMapIcons.Count - 1; index >= dungeonDifficult.dungeons.Count; index--)
				{
					var mapIcon = dungeonMapIcons[index];
					dungeonMapIcons.RemoveAt(index);

					if (mapIcon != null)
						GameObject.DestroyImmediate(mapIcon.gameObject);
				}
			}
			else
			{
				if (DungeonItem == null)
					return;

				for (int index = dungeonMapIcons.Count; index < dungeonDifficult.dungeons.Count; index++)
				{
					var mapItem = DungeonItem.mapIconPool.AllocateItem().GetComponent<UIElemDungeonMapIcon>();
					mapItem.gameObject.SetActive(true);
					mapItem.CachedTransform.parent = DungeonItem.bgButton.CachedTransform;
					mapItem.CachedTransform.localPosition = new Vector3(0f, 0f, -0.001f);
					dungeonMapIcons.Add(mapItem);
				}
			}

			// Set MapShopIcon Data.
			dungeonShopLocations.Clear();

			foreach (var location in dungeonData.shopLocations)
				dungeonShopLocations.Add(new MyVector3(location.ConvertToVector3()));

			if (dungeonShopLocations.Count > travelTraders.Count)
				for (int index = dungeonShopLocations.Count - 1; index >= travelTraders.Count; index--)
					dungeonShopLocations.RemoveAt(index);
			else if (dungeonShopLocations.Count < travelTraders.Count)
				for (int index = dungeonShopLocations.Count; index < travelTraders.Count; index++)
					dungeonShopLocations.Add(new MyVector3());

			if (dungeonShopIcons.Count > travelTraders.Count)
			{
				for (int index = dungeonShopIcons.Count - 1; index >= travelTraders.Count; index--)
				{
					var shopIcon = dungeonShopIcons[index];
					dungeonShopIcons.RemoveAt(index);

					if (shopIcon != null)
						GameObject.DestroyImmediate(shopIcon.gameObject);
				}
			}
			else
			{
				if (DungeonItem == null)
					return;

				for (int index = dungeonShopIcons.Count; index < travelTraders.Count; index++)
				{
					var mapItem = DungeonItem.mapShopPool.AllocateItem().GetComponent<UIElemDungeonShopIcon>();
					mapItem.gameObject.SetActive(true);
					mapItem.CachedTransform.parent = DungeonItem.bgButton.CachedTransform;
					mapItem.CachedTransform.localPosition = new Vector3(0f, 0f, -0.001f);
					dungeonShopIcons.Add(mapItem);
				}
			}

			dungeonData = null;
		}

		private void LoadLocations()
		{
			// Load MapIcon Locations.
			for (int index = 0; index < dungeonLocations.Count; index++)
				dungeonLocations[index] = new MyVector3(dungeonMapIcons[index].CachedTransform.localPosition);

			// Load MapShopIcon Locations.
			for (int index = 0; index < dungeonShopLocations.Count; index++)
				dungeonShopLocations[index] = new MyVector3(dungeonShopIcons[index].CachedTransform.localPosition);
		}

		private void SetLocations()
		{
			// Set MapIcon Locations.
			for (int index = 0; index < dungeonLocations.Count; index++)
				dungeonMapIcons[index].CachedTransform.localPosition = dungeonLocations[index].ConvertToVector3();

			// Set MapShopIcon Locations.
			for (int index = 0; index < dungeonShopLocations.Count; index++)
				dungeonShopIcons[index].CachedTransform.localPosition = dungeonShopLocations[index].ConvertToVector3();
		}

		private void SaveLocations()
		{
			// Get Serialize Dungeon Data.
			List<SerializeDungeonData> dungeonDatas = SerializeDungoenTools.DeserializeFromFile();

			SerializeDungeonData locationdata = null;
			foreach (SerializeDungeonData data in dungeonDatas)
			{
				if (data.dungeonkey == locationNames[locationNameIndex])
				{
					locationdata = data;
					break;
				}
			}

			// Create If Null.
			if (locationdata == null)
			{
				locationdata = new SerializeDungeonData(locationNames[locationNameIndex]);
				dungeonDatas.Add(locationdata);
			}

			// Clear MapIcon Locations, Set new location data.
			locationdata.locations.Clear();
			foreach (var location in dungeonLocations)
				locationdata.locations.Add(new MyVector3(location));

			// Clear MapShopIcon Locations , Set new location data.
			locationdata.shopLocations.Clear();
			foreach (var location in dungeonShopLocations)
				locationdata.shopLocations.Add(new MyVector3(location));

			SerializeDungoenTools.SerializeDataToFile(dungeonDatas);

			GC.Collect();
		}

		private void LoadLocationConfig()
		{
			// Load ConfigDatabase.
			ConfigDatabase.Initialize(new MathParserFactory(), false, false);
			IFileLoader fileLoader = new FileLoaderFromWorkspace();
			campaignConfig = new CampaignConfig();
			campaignConfig.LoadFromXml(fileLoader.LoadAsXML(DungeonConfig));
			campaignConfig.ConstructLogicData(null, 0);

			LoadZoneLocationConfig(campaignConfig.zones);
			LoadZoneLocationConfig(campaignConfig.secretZones);

			foreach (var zone in campaignConfig.zones)
			{
				if (zone == null)
					continue;

				foreach (var diffcult in zone.dungeonDifficulties)
				{
					if (diffcult == null)
						continue;
				}
			}

			SerializeDungoenTools.InitSerializeFile(locationNames);
			DungeonItem.mapIconPool.gameObject.SetActive(false);
			DungeonItem.mapShopPool.gameObject.SetActive(false);
			init = true;
		}

		private void LoadZoneLocationConfig(List<CampaignConfig.Zone> zones)
		{
			if (zones == null)
				return;

			foreach (var zone in zones)
			{
				if (zone == null)
					continue;

				foreach (var diffcult in zone.dungeonDifficulties)
				{
					if (diffcult == null || diffcult.dungeons.Count <= 0)
						continue;

					this.locationNames.Add(diffcult.positionName);
				}
			}
		}
	}
}