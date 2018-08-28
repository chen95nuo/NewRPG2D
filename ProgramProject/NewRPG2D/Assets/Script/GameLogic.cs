
//功能： 游戏总控制
//创建者: 胡海辉
//创建时间：


using Assets.Script.Utility;
using Assets.Script.Tools;
using Assets.Script.Timer;
using UnityEngine;
using Assets.Script.Battle;
using Assets.Script.Battle.LevelManager;

namespace Assets.Script
{
    public class GameLogic : MonoBehaviour
    {

        [SerializeField]private LayerMask inputLayerMask;
       // public float audioTime = 0.5f;
        public static GameLogic Instance = null;

        private bool isGameOver;

        public void Awake()
        {
            DebugHelper.bEnableDebug = true;
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                Init();
            }
            else
            {
                Destroy(this);
            }
        
          //  Debug.Log("-----------GameHallScene-----------" + ((1 << 6) | 3).ToString());
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            //AudioControl.GetInstance().Init();
            InitComponent();
            InitListener();
            InitData();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        public void InitComponent()
        {
            StringHelper.CreateInstance();
            EventManager.CreateInstance();
            ReadXmlNewMgr.CreateInstance();
            CTimerManager.CreateInstance();
            ResourcesLoadMgr.GetInstance();
            // MapColliderMgr.CreateInstance();
            InputContorlMgr.CreateInstance();
        }

        /// <summary>
        /// 初始化监听
        /// </summary>
        public void InitListener()
        {
            EventManager.instance.AddListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
        }

        public void InitData()
        {
            isGameOver = true;
            InputContorlMgr.instance.SetMainCamera(Camera.main);
            InputContorlMgr.instance.SetLayMask(inputLayerMask);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public void RemoveListener()
        {
            EventManager.instance.RemoveListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
        }

        #region Mono Update

        //private float addTime = 0;
        public void Update()
        {
            if (isGameOver)
            {
                return;
            }
            //addTime += Time.deltaTime;
            //if (addTime > StaticAndConstParamter.DeltaTime)
            {
                //addTime = 0;
                using (var mRole = GameRoleMgr.instance.RolesList.GetEnumerator())
                {
                    while (mRole.MoveNext())
                    {
                        if (mRole.Current != null) mRole.Current.UpdateLogic(Time.smoothDeltaTime);
                    }
                }
                //AudioControl.GetInstance().Update(audioTime);
                if (CTimerManager.HasInstance())
                {
                    CTimerManager.instance.Update(Time.smoothDeltaTime);
                }
                if (ResourcesLoadMgr.HasInstance())
                {
                    ResourcesLoadMgr.instance.Update(Time.smoothDeltaTime);
                }
                if (InputContorlMgr.HasInstance())
                {
                    InputContorlMgr.instance.Update(Time.deltaTime);
                }
            }
        }

        public void FixedUpdate()
        {
            using (var mRole = GameRoleMgr.instance.RolesList.GetEnumerator())
            {
                while (mRole.MoveNext())
                {
                    if (mRole.Current != null) mRole.Current.FixedUpdateLogic(Time.fixedDeltaTime);
                }
            }
        }

        public void IsGameOver()
        {
            isGameOver = true;
            GameRoleMgr.instance.ClearAllRole();
        }

        public void OnDestroy()
        {
            //AudioControl.Destory();
            RemoveListener();
            StringHelper.DestroyInstance();
            EventManager.DestroyInstance();
            ReadXmlNewMgr.DestroyInstance();
            ResourcesLoadMgr.DestroyInstance();
            //     MapColliderMgr.DestroyInstance();
            InputContorlMgr.DestroyInstance();
        }
        #endregion

        #region Event

        private void LoadLevelUpdate(LoadLevelParam param)
        {
            isGameOver = false;
        }

        #endregion
    }
}
