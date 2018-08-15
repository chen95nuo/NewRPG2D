using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIManager : MonoBehaviour
    {
        public BattleUIEnemyInfo EnemyInfo;
        public BattleUIMyRoleInfo RoleInfo;
        public BattleUISceneInfo SceneInfo;
        public BattleUIRoleHpInfo HpInfo;
        public Image UiSelectImg, lineImg, targetImg;

        private Transform selectTarget;
        private Vector3 originalVector;

        public void Awake()
        {
            EventManager.instance.AddListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
            EventManager.instance.AddListener<RoleRender>(EventDefineEnum.DragStart, OnDragStart);
            EventManager.instance.AddListener<RoleRender>(EventDefineEnum.Draging, OnDraging);
            EventManager.instance.AddListener<RoleRender>(EventDefineEnum.DragEnd, OnDragEnd);
            SetSelectLineState(false);
        }

        public void OnDestroy()
        {
            EventManager.instance.RemoveListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
            EventManager.instance.RemoveListener<RoleRender>(EventDefineEnum.DragStart, OnDragStart);
            EventManager.instance.RemoveListener<RoleRender>(EventDefineEnum.Draging, OnDraging);
            EventManager.instance.RemoveListener<RoleRender>(EventDefineEnum.DragEnd, OnDragEnd);
        }

        private void LoadLevelUpdate(LoadLevelParam param)
        {
            SceneInfo.StartGame();
        }

        private void OnDragStart(RoleRender role)
        {
            selectTarget = role.transform;
            SetSelectLineState(true);
            originalVector = Input.mousePosition;
            Vector2 screenPos = WorldToScreenPoint(selectTarget.position);
            UiSelectImg.rectTransform.position = Input.mousePosition;
            lineImg.rectTransform.position = Input.mousePosition;
        }

        private Vector2 tempsizeDelta;
        private void OnDraging(RoleRender role)
        {
            tempsizeDelta = lineImg.rectTransform.sizeDelta;
            lineImg.rectTransform.up = (originalVector - Input.mousePosition).normalized;
            tempsizeDelta.y = Vector3.Distance(originalVector, Input.mousePosition) * 4.5f;
            lineImg.rectTransform.sizeDelta = tempsizeDelta;
            targetImg.rectTransform.position = Input.mousePosition;
        }

        private void OnDragEnd(RoleRender role)
        {
            tempsizeDelta = lineImg.rectTransform.sizeDelta;          
            tempsizeDelta.y = 20;
            lineImg.rectTransform.sizeDelta = tempsizeDelta;
            SetSelectLineState(false);
        }

        private Vector3 ScreenToWorldPoint(Vector2 pos)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(pos);
            return mousePos;
        }

        private Vector3 WorldToScreenPoint(Vector2 pos)
        {
            Vector3 mousePos = Camera.main.WorldToScreenPoint(pos);
            return mousePos;
        }

        private void SetSelectLineState(bool active)
        {
            UiSelectImg.gameObject.CustomSetActive(active);
            lineImg.gameObject.CustomSetActive(active);
            targetImg.gameObject.CustomSetActive(active);
        }
    }
}
