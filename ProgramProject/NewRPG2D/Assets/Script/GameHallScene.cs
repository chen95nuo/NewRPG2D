
//功能： 游戏总控制
//创建者: 胡海辉
//创建时间：


using Assets.Script.AudioMgr;
using Assets.Script.Base;
using Assets.Script.EventMgr;
using Assets.Script.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Script
{
    public class GameHallScene : BaseMonoBehaviour
    {
        public float audioTime = 0.5f;
        public static GameHallScene Instance = null;
        public UnityEvent<string> strAA; 
        public override void Awake()
        {
            DebugHelper.bEnableDebug = true;
            base.Awake();
            if (Instance == null) 
            {
                Instance = this;
            }
           
            Debug.Log("-----------GameHallScene-----------");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            //AudioControl.GetInstance().Init();
            CharActorHelper.CreateInstance();
            ControlManager.CreateInstance();
            PlayerRootMgr.CreateInstance();
            base.Init();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        public override void  InitComponent()
        {
            base.InitComponent();
        }

        /// <summary>
        /// 初始化监听
        /// </summary>
        public override void InitListener()
        {
            base.InitListener();
        }

        public override void InitData()
        {
            base.InitData();
            //AudioControl.GetInstance().PlayBGMAudio(1);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public override void RemoveListener()
        {
            base.RemoveListener();
        }


        public override void Update()
        {
            base.Update();
            Debug.Log("----------------------");
            //AudioControl.GetInstance().Update(audioTime);
            ControlManager.GetInstance().Update(Time.deltaTime);
            PlayerRootMgr.GetInstance().Update(Time.deltaTime);
        }

        public override void OnDestroy()
        {
            ControlManager.DestroyInstance();
            CharActorHelper.DestroyInstance();
            //AudioControl.Destory();
            PlayerRootMgr.DestroyInstance();
            base.OnDestroy();
        }

    }
}
