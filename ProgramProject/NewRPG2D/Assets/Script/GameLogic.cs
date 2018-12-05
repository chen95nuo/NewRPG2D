
//功能： 游戏总控制
//创建者: 胡海辉
//创建时间：


using Assets.Script.Utility;
using Assets.Script.Tools;
using Assets.Script.Timer;
using UnityEngine;
using Assets.Script.Battle;
using Assets.Script.UIManger;
using Assets.Script.Net;

namespace Assets.Script
{
    public class GameLogic : MonoBehaviour
    {

        public static GameLogic Instance = null;

        private bool isInit = false;

        public void Awake()
        {
            Application.targetFrameRate = 60;

            DebugHelper.bEnableDebug = true;
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                Init();
            }
            else
            {
                Destroy(this.gameObject);
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
            //InitData();


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
            WebSocketManger.GetInstance();
            ReadTextAssetMgr.CreateInstance();
        }

        /// <summary>
        /// 初始化监听
        /// </summary>
        public void InitListener()
        {
        }

        public void InitData(string serverIp, int Port, string token)
        {
            WebSocketManger.instance.GameStartMessaget(string.Format("ws://{0}:{1}/websocket", serverIp, Port), token);
            //WebSocketManger.instance.Connect();

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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // WebSocketManger.instance.Send(NetSendMsg.RQ_StartGame, 1, "2");
                HTTPManager.instance.Register("abc", "123456");
            }

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
            UIPanelManager.instance.allPages.Clear();
            UIPanelManager.instance.currentPageNodes.Clear();
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
