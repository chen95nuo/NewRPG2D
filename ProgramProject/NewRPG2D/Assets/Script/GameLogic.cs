
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

        public static GameLogic Instance = null;

        private bool isInit = false;

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
            isInit = true;
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
        }

        /// <summary>
        /// 初始化监听
        /// </summary>
        public void InitListener()
        {
        }

        public void InitData()
        {
            //isGameOver = true;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public void RemoveListener()
        {
        }

        #region Mono Update

        //private float addTime = 0;
        public void Update()
        {

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

        public void FixedUpdate()
        {
        }

       

        public void OnDestroy()
        {
            //AudioControl.Destory();
            if (isInit == false)
            {
                return;
            }
            RemoveListener();
            StringHelper.DestroyInstance();
            EventManager.DestroyInstance();
            ReadXmlNewMgr.DestroyInstance();
            ResourcesLoadMgr.DestroyInstance();
        }
        #endregion

        #region Event

        #endregion
    }
}
