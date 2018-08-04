
using Assets.Script.Timer;
using Assets.Script.Utility;
using UnityEngine;
using System;
using System.Collections.Generic;
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

        private static ushort currentInstanceId = 0;

        private int sceneId = 1001;
        private Queue<CreateEnemyData> enemyDatas;
        private CreateEnemyData currentEnemyData;
        private List<BornEnemyInfo> currentEnemyInfoList;

        private bool isCreateEnemy;
        private float addTime;

        private void Awake()
        {
            ReadXmlNewMgr.instance.LoadSpecialXML(XmlName.MapSceneLevel, sceneName);
            enemyDatas = new Queue<CreateEnemyData>();
            currentEnemyInfoList = new List<BornEnemyInfo>();
            currentInstanceId = 0;
            isCreateEnemy = false;
        }

        private void Start()
        {
            CTimerManager.instance.AddListener(0.1f, 1, AddRole);
        }

        private void Update()
        {
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
            if (enemyDatas.Count > 0)
            {
                SetEnemyInfo();
            }
            for (int i = 0; i < heroPoint.Length; i++)
            {
                BornHero(heroPoint[i].Point.position, currentInstanceId++, 1001, 0);
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
            currentEnemyInfoList.Clear();
            currentEnemyData = enemyDatas.Dequeue();
            InitEnemyInfoDic(BornPositionTypeEnum.Point01);
            InitEnemyInfoDic(BornPositionTypeEnum.Point02);
            InitEnemyInfoDic(BornPositionTypeEnum.Point03);
            InitEnemyInfoDic(BornPositionTypeEnum.Point04);
            InitEnemyInfoDic(BornPositionTypeEnum.Point05);
        }

        private void BornHero(Vector3 mPosition, ushort instanceId, int roleId, float angle)
        {
            GameRoleMgr.instance.AddHeroRole("", "Hero", transform, mPosition, instanceId, roleId, angle);
        }

        private void BornEnemy(Vector3 mPosition, ushort instanceId, int roleId, float angle)
        {
            GameRoleMgr.instance.AddMonsterRole("", "Monster", transform, mPosition, instanceId, roleId, angle);
        }

        private void GetEnemyData(int enemyDataId)
        {
            CreateEnemyData enemyData = CreateEnemyMgr.instance.GetXmlDataByItemId<CreateEnemyData>(enemyDataId);
            if (enemyData != null) enemyDatas.Enqueue(enemyData);
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
            Vector3 bornPosition = GetEnemyBornPoisition(bornPositionType);
            BornEnemy(bornPosition, currentInstanceId++, enemyRoleId, 0);
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

        private Vector3 GetEnemyBornPoisition(BornPositionTypeEnum bornPositionType)
        {
            for (int i = 0; i < monsterPoint.Length; i++)
            {
                if (monsterPoint[i].BornPositionType == bornPositionType)
                {
                    return monsterPoint[i].Point.position;
                }
            }
            return Vector3.zero;
        }
    }
}
