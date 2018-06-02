
//功能： 摄像机控制
//创建者: 胡海辉
//创建时间：


using UnityEngine;
using System;
using Assets.Script.Base;

namespace Assets.Script
{
    public class CameraContorlMgr : BaseMonoBehaviour
    {
        public Camera mainCamera;
        private Transform playerTrans;
        private Vector3 cameraOffest;
        #region Init

        public override void Init()
        {
            base.Init();
        }

        public override void InitComponent() 
        {
            base.InitComponent();
            //mEffectController = GetComponent<GlitchEffectController>();
            //mEffectController.apply = false;
        }

        /// <summary>
        /// 初始化监听
        /// </summary>
        public override void InitListener()
        {
            base.InitListener();
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public override void RemoveListener()
        {
            base.RemoveListener();
        }

        #endregion

        public override void BaseUpdate(float time)
        {
            base.BaseUpdate(time);
        }

        public void OnTiggerType(object obj, EventArgs e)
        {
            //TiggerTypeParam param = (TiggerTypeParam)obj;

        }

        #region fuc
       
        #endregion
    }
}
