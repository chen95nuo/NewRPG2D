
using Assets.Script.Timer;
using Assets.Script.Utility;
using UnityEngine;
using System;
using System.Collections;
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
        private LayerMask inputLayerMask;
        [SerializeField]
        private BornPoint[] heroPoint;
        [SerializeField]
        private Transform[] heroBornPoint;

        [SerializeField]
        private BornPoint[] monsterPoint;
        [SerializeField]
        private Transform[] monsterBornPoint;

        [SerializeField]
        private string sceneName;

        public static ushort currentInstanceId = 100;

        private int sceneId = 10001;
        //  private Queue<CreateEnemyData> enemyDatas;
        private CreateEnemyData currentEnemyData;
        private List<CreateEnemyInfo> currentEnemyInfoList;

        private bool isCreateEnemy;
        private float addTime;
        private bool isGameOver;
        private RoleDetailData[] roleInfoArray;

        private void Awake()
        {
            InputContorlMgr.CreateInstance();
            ReadXmlNewMgr.instance.ReadXmlByType(XmlName.RoleData, XmlName.Battle, XmlTypeEnum.Battle);
            //  ReadXmlNewMgr.instance.LoadSpecialXML(XmlName.MapSceneLevel, sceneName, XmlTypeEnum.Battle);
            //  enemyDatas = new Queue<CreateEnemyData>();
            currentEnemyInfoList = new List<CreateEnemyInfo>();
            roleInfoArray = BattleDetailDataMgr.instance.RoleDatas;

            LoadLevelParam temp = new LoadLevelParam();
            EventManager.instance.SendEvent(EventDefineEnum.LoadLevel, temp);
            GameRoleMgr.instance.CurrentPlayerMp.Value = 1 * 500;
            currentInstanceId = 100;
            isCreateEnemy = false;
            isGameOver = false;
            InputContorlMgr.instance.SetMainCamera(Camera.main);
            InputContorlMgr.instance.SetLayMask(inputLayerMask);
        }

        private void Start()
        {
            CTimerManager.instance.AddListener(1.5f, 1, AddRole);
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
            using (var mRole = GameRoleMgr.instance.RolesList.GetEnumerator())
            {
                while (mRole.MoveNext())
                {
                    if (mRole.Current != null) mRole.Current.UpdateLogic(Time.smoothDeltaTime);
                }
            }
        }

        public void FixedUpdate()
        {
            if (isGameOver)
            {
                return;
            }

            using (var mRole = GameRoleMgr.instance.RolesList.GetEnumerator())
            {
                while (mRole.MoveNext())
                {
                    if (mRole.Current != null) mRole.Current.FixedUpdateLogic(Time.fixedDeltaTime);
                }
            }
        }

        private void OnDestroy()
        {
            InputContorlMgr.DestroyInstance();
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
                    if (roleInfoArray[j] == null)
                    {
                        continue;
                    }
                    if (heroPoint[i].BornPositionType == roleInfoArray[j].BornPositionType)
                    {
                        BornHero(heroPoint[i], currentInstanceId++, roleInfoArray[j], 0);
                        break;
                    }
                }
            }
        }

        private void InitEnemyData()
        {
            currentEnemyData = CreateEnemyMgr.instance.GetXmlDataByItemId<CreateEnemyData>(sceneId);
            GetEnemyData(sceneId);
        }

        private void SetEnemyInfo()
        {
            InitEnemyInfoDic(BornPositionTypeEnum.Point01);
            InitEnemyInfoDic(BornPositionTypeEnum.Point02);
            InitEnemyInfoDic(BornPositionTypeEnum.Point03);
            InitEnemyInfoDic(BornPositionTypeEnum.Point04);
            InitEnemyInfoDic(BornPositionTypeEnum.Point05);
            InitEnemyInfoDic(BornPositionTypeEnum.Point06);
            InitEnemyInfoDic(BornPositionTypeEnum.Point07);
            InitEnemyInfoDic(BornPositionTypeEnum.Point08);
            InitEnemyInfoDic(BornPositionTypeEnum.Point09);
        }

        private void BornHero(BornPoint mPoint, ushort instanceId, RoleDetailData roleData, float angle)
        {
            Vector3 startPoint = mPoint.Point.position;
            int pointIndex = (int)mPoint.BornPositionType % 3;
            startPoint = heroBornPoint[pointIndex].position;
            bool success = GameRoleMgr.instance.AddHeroRole("Hero", mPoint.Point, startPoint, instanceId, roleData, angle);
            if (success) StartCoroutine(DelayMoveRole(instanceId, mPoint.Point.position));
        }

        private IEnumerator DelayMoveRole(ushort instanceId, Vector3 position)
        {
            yield return null;
            RoleBase role = GameRoleMgr.instance.GetRole(instanceId);
            role.SetRoleActionState(ActorStateEnum.Run);
            role.RoleMoveMoment.SetTargetPosition(position);
        }

        private void BornEnemy(BornPoint mPoint, ushort instanceId, int roleId, float angle)
        {
            Vector3 startPoint = mPoint.Point.position;
            int pointIndex = (int)mPoint.BornPositionType % 3;
            startPoint = monsterBornPoint[pointIndex].position;
            bool success = GameRoleMgr.instance.AddMonsterRole("Monster", mPoint.Point, startPoint, instanceId, roleId, angle);
            if (success) StartCoroutine(DelayMoveRole(instanceId, mPoint.Point.position));
        }

        private void GetEnemyData(int enemyDataId)
        {
            CreateEnemyData enemyData = CreateEnemyMgr.instance.GetXmlDataByItemId<CreateEnemyData>(enemyDataId);
            if (enemyData != null)
            {
                //enemyDatas.Enqueue(enemyData);
                for (int i = 0; i < enemyData.CreateEnemyIds.Length; i++)
                {
                    if (enemyData.CreateEnemyIds[i] > 0)
                    {
                        //Debug.LogError(" RemianEnemyCount  " + enemyData.CreateEnemyInfoList[i].EnemyPointRoleId);
                        GameRoleMgr.instance.RemianEnemyCount.Value += enemyData.CreateEnemyIds[i];
                    }
                }
            }
        }

        private void InitEnemyInfoDic(BornPositionTypeEnum bornPositionType)
        {
            CreateEnemyInfo enemyInfo = default(CreateEnemyInfo);

            GetEnemyInfo(bornPositionType, ref enemyInfo);
            currentEnemyInfoList.Add(enemyInfo);
        }

        private void CheckEnemyCount()
        {
            if (isCreateEnemy && GameRoleMgr.instance.RolesEnemyList.Count <= 0)
            {
                isGameOver = true;
                DebugHelper.LogError("  -----------------Win----- ");
                EventManager.instance.SendEvent(EventDefineEnum.GameOver, true);
                //UIEventManager.instance.SendEvent(UIEventDefineEnum.MissionComplete);
                //GoFightMgr.instance.MissionComplete();
                return;
            }

            if (isCreateEnemy && GameRoleMgr.instance.RolesHeroList.Count <= 0)
            {
                DebugHelper.LogError("  -----------------Lose----- ");
                //UIEventManager.instance.SendEvent(UIEventDefineEnum.MissionComplete);
                //GoFightMgr.instance.MissionComplete();
                isGameOver = true;
                EventManager.instance.SendEvent(EventDefineEnum.GameOver, false);
            }
        }

        private void StartBornEnemy()
        {
            for (int i = 0; i < currentEnemyInfoList.Count; i++)
            {
                CreateEnemyInfo info = currentEnemyInfoList[i];
                DebugHelper.Log(" IsBornFirst ");
                isCreateEnemy = true;
                BornEnemy(info.PositionType, info.EnemyPointRoleId);
            }
            currentEnemyInfoList.Clear();
        }

        private void BornEnemy(BornPositionTypeEnum bornPositionType, int enemyRoleId)
        {
            BornPoint bornPoint = GetEnemyBornPoisition(bornPositionType);
            BornEnemy(bornPoint, currentInstanceId++, enemyRoleId, 0);
        }

        private void GetEnemyInfo(BornPositionTypeEnum bornPositionType, ref CreateEnemyInfo enemyInfo)
        {
            for (int i = 0; i < currentEnemyData.CreateEnemyIds.Length; i++)
            {
                if (currentEnemyData.CreateEnemyIds[i] == 0)
                {
                    continue;
                }

                if ((BornPositionTypeEnum)i == bornPositionType)
                {
                    enemyInfo.EnemyPointRoleId = currentEnemyData.CreateEnemyIds[i];
                    enemyInfo.PositionType = bornPositionType;
                    break;
                }
            }
        }

        private BornPoint GetEnemyBornPoisition(BornPositionTypeEnum bornPositionType)
        {
            for (int i = 0; i < monsterPoint.Length; i++)
            {
                if (monsterPoint[i].BornPositionType == bornPositionType)
                {
                    return monsterPoint[i];
                }
            }
            return null;
        }
    }
}
