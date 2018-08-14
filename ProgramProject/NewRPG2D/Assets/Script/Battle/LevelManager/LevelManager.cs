
using Assets.Script.Timer;
using Assets.Script.Utility;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Battle.LevelManager
{

    public class LevelManager : MonoBehaviour
    {
        private class BornEnemyInfo
        {
            public bool IsBornFirst;
            public int BornCount;
            public float CurrentTime;
            public CreateEnemyInfo enemyInfo;
        }

        [SerializeField]
        private BornPoint[] heroPoint;
        [SerializeField]
        private BornPoint[] monsterPoint;
        [SerializeField]
        private string sceneName;

        public static ushort currentInstanceId = 100;

        private int sceneId = 100001;
        private Queue<CreateEnemyData> enemyDatas;
        private CreateEnemyData currentEnemyData;
        private List<BornEnemyInfo> currentEnemyInfoList;

        private bool isCreateEnemy;
        private float addTime;
        private bool isGameOver;
        private CardData[] roleInfoArray;

        private void Awake()
        {

            ReadXmlNewMgr.instance.LoadSpecialXML(XmlName.MapSceneLevel, sceneName);
            enemyDatas = new Queue<CreateEnemyData>();
            currentEnemyInfoList = new List<BornEnemyInfo>();
            roleInfoArray = GoFightMgr.instance.cardData;
            Debug.LogError(" roleInfoArray " + roleInfoArray.Length);
            LoadLevelParam temp = new LoadLevelParam();
            EventManager.instance.SendEvent(EventDefineEnum.LoadLevel, temp);
            GameRoleMgr.instance.CurrentPlayerMp.Value = GoFightMgr.instance.PlayerLevel * 50;
            currentInstanceId = 100;
            isCreateEnemy = false;
            isGameOver = false;
        }

        private void Start()
        {
            CTimerManager.instance.AddListener(0.1f, 1, AddRole);
        }

        private void Update()
        {
            if (isGameOver)
            {
                return;
            }
            addTime += Time.deltaTime;
            StartBornEnemy();
            CheckEnemyCount();
        }

        private void OnDestroy()
        {

        }

        private void AddRole(int timeId)
        {
            CTimerManager.instance.RemoveLister(timeId);
            InitEnemyData();
            SetEnemyInfo();
            float heroPointLength = heroPoint.Length;
            for (int i = 0; i < heroPointLength; i++)
            {
                for (int j = 0; j < roleInfoArray.Length; j++)
                {
                    if ((int)heroPoint[i].BornPositionType == roleInfoArray[j].TeamPos)
                    {
                        BornHero(heroPoint[i].Point, currentInstanceId++, roleInfoArray[j], 0);
                        break;
                    }
                }

            }
        }

        private void InitEnemyData()
        {
            MapSceneLevelData sceneLevelData = MapSceneLevelMgr.instance.GetXmlDataByItemId<MapSceneLevelData>(sceneId);
            GetEnemyData(sceneLevelData.CreateEnemy01);
            GetEnemyData(sceneLevelData.CreateEnemy02);
            GetEnemyData(sceneLevelData.CreateEnemy03);
        }

        private void SetEnemyInfo()
        {
            DebugHelper.Log(" SetEnemyInfo " + enemyDatas.Count);
            if (enemyDatas.Count <= 0)
            {
                isGameOver = true;
                DebugHelper.LogError("  -----------------Win----- ");
                UIEventManager.instance.SendEvent(UIEventDefineEnum.MissionComplete);
                return;
            }
            currentEnemyInfoList.Clear();
            currentEnemyData = enemyDatas.Dequeue();
            InitEnemyInfoDic(BornPositionTypeEnum.Point01);
            InitEnemyInfoDic(BornPositionTypeEnum.Point02);
            InitEnemyInfoDic(BornPositionTypeEnum.Point03);
            InitEnemyInfoDic(BornPositionTypeEnum.Point04);
            InitEnemyInfoDic(BornPositionTypeEnum.Point05);
        }

        private void BornHero(Transform mPoint, ushort instanceId, CardData roleData, float angle)
        {
            GameRoleMgr.instance.AddHeroRole("Hero", mPoint, mPoint.position, instanceId, roleData, angle);
        }

        private void BornEnemy(Transform mPoint, ushort instanceId, int roleId, float angle)
        {
            Debug.Log(" monster id " + roleId);
            GameRoleMgr.instance.AddMonsterRole("Monster", mPoint, mPoint.position, instanceId, roleId, angle);
        }

        private void GetEnemyData(int enemyDataId)
        {
            CreateEnemyData enemyData = CreateEnemyMgr.instance.GetXmlDataByItemId<CreateEnemyData>(enemyDataId);
            if (enemyData != null)
            {
                enemyDatas.Enqueue(enemyData);
                for (int i = 0; i < enemyData.CreateEnemyInfoList.Count; i++)
                {
                    if (enemyData.CreateEnemyInfoList[i].EnemyPointRoleId > 0)
                    {
                        //Debug.LogError(" RemianEnemyCount  " + enemyData.CreateEnemyInfoList[i].EnemyPointRoleId);
                        GameRoleMgr.instance.RemianEnemyCount.Value += enemyData.CreateEnemyInfoList[i].EnemyCount;
                    }
                }
            }
        }

        private void InitEnemyInfoDic(BornPositionTypeEnum bornPositionType)
        {
            CreateEnemyInfo enemyInfo = default(CreateEnemyInfo);
            GetEnemyInfo(bornPositionType, ref enemyInfo);
            BornEnemyInfo info = new BornEnemyInfo();
            info.enemyInfo = enemyInfo;
            currentEnemyInfoList.Add(info);
        }

        private void CheckEnemyCount()
        {
            if (isCreateEnemy && GameRoleMgr.instance.RolesEnemyList.Count <= 0)
            {
                SetEnemyInfo();
            }

            if (isCreateEnemy && GameRoleMgr.instance.RolesHeroList.Count <= 0)
            {
                DebugHelper.LogError("  -----------------Lose----- ");
                isGameOver = true;
                GameLogic.Instance.IsGameOver();
            }
        }

        private void StartBornEnemy()
        {
            for (int i = 0; i < currentEnemyInfoList.Count; i++)
            {
                BornEnemyInfo info = currentEnemyInfoList[i];
                if (info.IsBornFirst == false)
                {
                    if (addTime > info.enemyInfo.FirstEnemyDelayTime)
                    {
                        DebugHelper.Log(" IsBornFirst ");
                        isCreateEnemy = true;
                        info.IsBornFirst = true;
                        info.BornCount++;
                        info.CurrentTime = addTime;
                        BornEnemy(info.enemyInfo.PositionType, info.enemyInfo.EnemyPointRoleId);
                    }
                }
                else
                {
                    if (addTime - info.CurrentTime > info.CurrentTime && info.BornCount < info.enemyInfo.EnemyCount)
                    {
                        DebugHelper.Log(" EnemyCount " + info.enemyInfo.EnemyCount + " BornCount == " + info.BornCount);
                        info.BornCount++;
                        info.CurrentTime = addTime;
                        BornEnemy(info.enemyInfo.PositionType, info.enemyInfo.EnemyPointRoleId);
                    }
                }
            }
        }

        private void BornEnemy(BornPositionTypeEnum bornPositionType, int enemyRoleId)
        {
            Transform bornPoint = GetEnemyBornPoisition(bornPositionType);
            BornEnemy(bornPoint, currentInstanceId++, enemyRoleId, 0);
        }

        private void GetEnemyInfo(BornPositionTypeEnum bornPositionType, ref CreateEnemyInfo enemyInfo)
        {
            for (int i = 0; i < currentEnemyData.CreateEnemyInfoList.Count; i++)
            {
                if (currentEnemyData.CreateEnemyInfoList[i].PositionType == bornPositionType)
                {
                    enemyInfo = currentEnemyData.CreateEnemyInfoList[i];
                    break;
                }
            }
        }

        private Transform GetEnemyBornPoisition(BornPositionTypeEnum bornPositionType)
        {
            for (int i = 0; i < monsterPoint.Length; i++)
            {
                if (monsterPoint[i].BornPositionType == bornPositionType)
                {
                    return monsterPoint[i].Point;
                }
            }
            return transform;
        }
    }
}
