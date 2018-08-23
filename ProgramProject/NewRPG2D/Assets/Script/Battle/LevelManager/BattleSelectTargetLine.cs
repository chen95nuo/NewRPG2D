using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class BattleSelectTargetLine : MonoBehaviour
    {
        public SpriteRenderer lineImg, targetImg;
        public Sprite redLine, redCicle, greenLine, greenCicle;

        private Vector3 originalVector;
        private float defaultZ;

        private Vector3 lineScale, worldPosition;
        private bool haveTarget;

        public void Awake()
        {
            EventManager.instance.AddListener<RoleRender>(EventDefineEnum.DragStart, OnDragStart);
            EventManager.instance.AddListener<SelectTargetParam>(EventDefineEnum.Draging, OnDraging);
            EventManager.instance.AddListener<RoleRender>(EventDefineEnum.DragEnd, OnDragEnd);
            SetSelectLineState(false);
            defaultZ = transform.position.z;
            lineScale = lineImg.transform.localScale;
        }

        public void OnDestroy()
        {
            EventManager.instance.RemoveListener<RoleRender>(EventDefineEnum.DragStart, OnDragStart);
            EventManager.instance.RemoveListener<SelectTargetParam>(EventDefineEnum.Draging, OnDraging);
            EventManager.instance.RemoveListener<RoleRender>(EventDefineEnum.DragEnd, OnDragEnd);
        }



        private void OnDragStart(RoleRender role)
        {
            SetSelectLineState(true);
            worldPosition = ScreenToWorldPoint(Input.mousePosition);
            targetImg.transform.position = worldPosition;
        }

        private void OnDraging(SelectTargetParam data)
        {
            lineImg.transform.position = data.OriginalTransform.position;
            worldPosition = ScreenToWorldPoint(Input.mousePosition);
            lineImg.transform.up = (data.OriginalTransform.position - worldPosition).normalized;
            lineScale.y = Vector3.Distance(data.OriginalTransform.position, worldPosition) * 17f;

            targetImg.transform.position = worldPosition;

            if (data.TargetTransform != null)
            {
                if (haveTarget == false)
                {
                    haveTarget = true;
                    lineScale.x = 1.2f;
                    lineImg.sprite = redLine;
                    targetImg.sprite = redCicle;
                }
            }
            else if (haveTarget)
            {
                haveTarget = false;
                lineImg.sprite = greenLine;
                targetImg.sprite = greenCicle;
                lineScale.x = 1f;
            }
            lineImg.transform.localScale = lineScale;
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
