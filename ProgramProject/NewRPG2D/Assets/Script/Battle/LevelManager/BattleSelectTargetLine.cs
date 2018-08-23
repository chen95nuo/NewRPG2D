using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class BattleSelectTargetLine : MonoBehaviour
    {
        public Transform lineImg, targetImg;
        private Transform selectTarget;
        private Vector3 originalVector;
        private float defaultZ;

        private Vector3 lineScale, worldPosition;

        public void Awake()
        {
            EventManager.instance.AddListener<RoleRender>(EventDefineEnum.DragStart, OnDragStart);
            EventManager.instance.AddListener<SelectTargetParam>(EventDefineEnum.Draging, OnDraging);
            EventManager.instance.AddListener<RoleRender>(EventDefineEnum.DragEnd, OnDragEnd);
            SetSelectLineState(false);
            defaultZ = transform.position.z;
            lineScale = lineImg.localScale;
        }

        public void OnDestroy()
        {
            EventManager.instance.RemoveListener<RoleRender>(EventDefineEnum.DragStart, OnDragStart);
            EventManager.instance.RemoveListener<SelectTargetParam>(EventDefineEnum.Draging, OnDraging);
            EventManager.instance.RemoveListener<RoleRender>(EventDefineEnum.DragEnd, OnDragEnd);
        }



        private void OnDragStart(RoleRender role)
        {
            selectTarget = role.transform;
            SetSelectLineState(true);
            worldPosition = ScreenToWorldPoint(Input.mousePosition);
            targetImg.position = worldPosition;
        }

        private void OnDraging(SelectTargetParam data)
        {
            lineImg.position = data.OriginalTransform.position;
            worldPosition = ScreenToWorldPoint(Input.mousePosition);
            lineImg.up = (data.OriginalTransform.position - worldPosition).normalized;
            lineScale.y = Vector3.Distance(data.OriginalTransform.position, worldPosition) * 3f;

            targetImg.position = worldPosition;

            if (data.TargetTransform != null)
            {
                lineScale.x = 1.2f;
                targetImg.localScale = Vector3.one * 1.5f;
            }
            else
            {
                lineScale.x = 0.4f;
                targetImg.localScale = Vector3.one;
            }
            lineImg.localScale = lineScale;
        }

        private void OnDragEnd(RoleRender role)
        {
            SetSelectLineState(false);
        }

        private Vector3 ScreenToWorldPoint(Vector2 pos)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(pos);
            mousePos.z = defaultZ;
            return mousePos;
        }

        private Vector3 WorldToScreenPoint(Vector2 pos)
        {
            Vector3 mousePos = Camera.main.WorldToScreenPoint(pos);
            return mousePos;
        }

        private void SetSelectLineState(bool active)
        {

            lineImg.gameObject.CustomSetActive(active);
            targetImg.gameObject.CustomSetActive(active);
        }
    }
}
