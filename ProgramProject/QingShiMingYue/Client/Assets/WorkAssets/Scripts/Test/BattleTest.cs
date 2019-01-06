using UnityEngine;
using System.Collections;
using ClientServerCommon;
using KodGames;
using System.IO;
using System.Collections.Generic;

public class BattleTest : MonoBehaviour
{
#if UNITY_EDITOR
	public enum BattleType
	{
		ActivityCampaign,
		Arena,
		Campaign,
		CombatFriend,
		Tower,
		WolfSmoke,
		WorldBoss
	}

	public class BattleTestBattleResultData : UIPnlBattleResultBase.BattleResultData
	{
		int battleTestCombatType = _CombatType.Unknown;
		public int BattleTestCombatType
		{
			get { return battleTestCombatType; }
		}
		public BattleTestBattleResultData(BattleType battleType)
			: base(_CombatType.Unknown)
		{
			switch (battleType)
			{
				case BattleType.ActivityCampaign:
					battleTestCombatType = _CombatType.ActivityCampaign;
					break;

				case BattleType.Arena:
					battleTestCombatType = _CombatType.Arena;
					break;

				case BattleType.Campaign:
					battleTestCombatType = _CombatType.Campaign;
					break;

				case BattleType.CombatFriend:
					battleTestCombatType = _CombatType.CombatFriend;
					break;

				case BattleType.Tower:
					battleTestCombatType = _CombatType.Tower;
					break;

				case BattleType.WolfSmoke:
					battleTestCombatType = _CombatType.WolfSmoke;
					break;

				case BattleType.WorldBoss:
					battleTestCombatType = _CombatType.WolfSmoke;
					break;
			}
		}
	}

	public BattleType battleType = BattleType.Campaign;
	public TextAsset battleRecordFile;
	public int recordCount = 3;
	public bool playAll = false;
	public bool useXmlData = false;
	public bool useXmlByteData = false;

	private static string BattleRecordFilePath = "./Assets/WorkAssets/Texts/LocalData";
	private static int SceneId = 0x01000003;
	void Start()
	{
		BattleRound.EnableLog = false;
		CharacterController.LogAction = false;
		Avatar.LogAnim = false;

		Initialize();
	}

	void Update()
	{
		SysModuleManager.Instance.OnUpdate();
	}

	public void OnDrawGizmos()
	{
		//recordPlayer.OnDrawGizmos();
	}

	void Initialize()
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

		SysGameStateMachine stateMachine = SysModuleManager.Instance.AddSysModule<SysGameStateMachine>(true);
		List<object> paramsters = new List<object>();
		KodGames.ClientClass.CombatResultAndReward combatResultAndReward = new KodGames.ClientClass.CombatResultAndReward();
		combatResultAndReward.BattleRecords = new List<KodGames.ClientClass.BattleRecord>();

		if (playAll)
		{
			string[] fileNames = Directory.GetFiles(BattleRecordFilePath);
			foreach (string file in fileNames)
			{
				if (string.Compare(Path.GetExtension(file), ".bytes", true) != 0)
				{
					continue;
				}

				Stream oStream = File.OpenRead(file);
				byte[] arrBytes = new byte[oStream.Length];
				int offset = 0;
				while (offset < arrBytes.LongLength)
				{
					offset += oStream.Read(arrBytes, offset, arrBytes.Length - offset);
				}

				KodGames.ClientClass.BattleRecord battleRecord = new KodGames.ClientClass.BattleRecord().FromProtoBuffData(arrBytes);
				//LogBattle(battleRecord);
				battleRecord.SceneId = SceneId;
				combatResultAndReward.BattleRecords.Add(battleRecord);
			}
		}
		else
		{
			if (useXmlByteData)
			{
				combatResultAndReward.FromProtobufData(battleRecordFile.bytes);
			}
			else if (useXmlData)
			{
				CombatDataConfig combatDataConfig = new CombatDataConfig();
				combatDataConfig.LoadFromXml(System.Security.SecurityElement.FromString(battleRecordFile.text));
				combatResultAndReward = combatDataConfig.combatResultAndReward;
				combatResultAndReward.IsPlotBattle = true;
			}
			else
			{
				KodGames.ClientClass.BattleRecord battleRecord = new KodGames.ClientClass.BattleRecord().FromProtoBuffData(battleRecordFile.bytes);

				battleRecord.SceneId = SceneId;
				//LogBattle(battleRecord);
				for (int i = 0; i < recordCount; i++)
				{
					combatResultAndReward.BattleRecords.Add(battleRecord);
				}
			}

		}

		paramsters.Add(combatResultAndReward);
		paramsters.Add(new BattleTestBattleResultData(battleType));

		//var battleResultData = new UIPnlWorldBossBattleChallengeResult.WorldBossBattleChanllegeResult(0, 0, 0, null);
		//paramsters.Add(battleResultData);

		stateMachine.EnterState<GameState_Battle>(paramsters, true);
	}

	void LogBattle(KodGames.ClientClass.BattleRecord battleRecords)
	{
		//for (int roundIdx = 0; roundIdx < battleRecords.CombatRecord.RoundRecords.Count; roundIdx++)
		//{
		//    var roundRecord = battleRecords.CombatRecord.RoundRecords[roundIdx];
		//    Debug.Log(string.Format("Round : {0} {1}++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++", GetCombatRonndType.getDisplayNameByType(roundRecord.RoundType), roundIdx));

		//    System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//    for (int i = 0; i < roundRecord.TurnRecords.Count; i++)
		//    {
		//        var turn = roundRecord.TurnRecords[i];
		//        sb.Append("Avatar: " + turn.AvatarIndex);
		//        for (int j = 0; j < turn.ActionRecords.Count; j++)
		//        {
		//            var actRec = turn.ActionRecords[j];

		//            var actCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionById(actRec.ActionId);

		//            CombatTurn turnCfg = null;
		//            if (actCfg != null)
		//                turnCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetCombatTurnByID(actCfg.sourceTurnId);

		//            if (turnCfg != null)
		//                sb.Append(string.Format("Turn:{0}    ", turnCfg.id.ToString("X")));
		//            if (actCfg != null)
		//                sb.Append("Action:" + AvatarAction._Type.GetNameByType(actCfg.actionType));

		//            sb.Append('\n');
		//        }
		//    }
		//    Debug.Log(sb.ToString());
		//}
	}

	void OnGUI()
	{
		SysModuleManager.Instance.UpdateGUI();

		//if (recordPlayer.PlayerState != BattleRecordPlayer._PlayerState.Playering)
		//{
		//    if (GUI.Button(new Rect(0, 0, 100, 100), "Play"))
		//    {
		//        //GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		//        //battleState.PlayNextBattle();
		//        //recordPlayer.Start();
		//    }
		//}
		//		else if (pause == false && GUI.Button(new Rect(0, 0, 100, 100), "Pause"))
		//		{
		//			foreach (var avatar in recordPlayer.BattleRoles)
		//				avatar.Pause = true;
		//
		//			pause = true;
		//		}
		//		else if (pause && GUI.Button(new Rect(0, 0, 100, 100), "Resume"))
		//		{
		//			foreach (var avatar in recordPlayer.BattleRoles)
		//				avatar.Pause = false;
		//
		//			pause = false;
		//		}
	}

	[UnityEditor.MenuItem("Tools/BattleTest/Scale Time 3 %#i")]
	static void ScaleTime3()
	{
		Time.timeScale = 3;
	}

	[UnityEditor.MenuItem("Tools/BattleTest/Scale Time 1 %#o")]
	static void ScaleTime1()
	{
		Time.timeScale = 1;
	}

	[UnityEditor.MenuItem("Tools/BattleTest/Scale Time 6 %#j")]
	static void ScaleTime6()
	{
		Time.timeScale = 6;
	}
#endif
}