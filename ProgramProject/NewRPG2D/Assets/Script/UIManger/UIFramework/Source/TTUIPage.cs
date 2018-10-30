using System;
using Assets.Script.UIManger;
using System.Collections;
using DG.Tweening;

namespace Assets.Script.UIManger
{
    using UnityEngine;

    public abstract class TTUIPage : MonoBehaviour
    {
        //this page's id
        public int id = -1;

        //this page's type
        public UIType type = UIType.Normal;

        //how to show this page.
        public UIMode mode = UIMode.DoNothing;

        //the background collider mode
        public UICollider collider = UICollider.None;

        //path to load ui
        public string uiPath = string.Empty;

        //record this ui load mode.async or sync.
        public bool isAsyncUI = false;

        public Animation UIAnimation;

        protected virtual string ShowAnimationName
        {
            get
            {
                return "";
            }
        }

        protected virtual string HideAnimationName
        {
            get
            {
                return "";
            }
        }

        //this page active flag
        protected bool isActived = false;

        //refresh page 's data.
        private object m_data = null;
        protected object data { get { return m_data; } }


        #region virtual api


        public virtual void Show(object mData)
        {
            m_data = mData;
            Active();
        }

        public virtual void Show(object mData, Action callback)
        {
            m_data = mData;
            Active();
            if (callback != null)
            {
                callback();
            }

            Sequence mSequence = DOTween.Sequence();
            mSequence.Append(transform.DOScale(0.1f, 0.5f));
            mSequence.Append(transform.DOScale(1.5f, 0.5f));
        }

        /// <summary>
        /// Only Deactive UI wont clear Data.
        /// </summary>
        public virtual void Hide(bool needAnim = true)
        {
            if (needAnim)
            {
                if (UIAnimation != null)
                {
                    float delayTime = UIAnimation[HideAnimationName].time;
                    StartCoroutine(DelayHidePage(delayTime));
                }
                else
                {
                    Sequence mSequence = DOTween.Sequence();
                    mSequence.Append(transform.DOScale(1.08f, 0.1f));
                    mSequence.Append(transform.DOScale(0.98f, 0.1f));
                    mSequence.Append(transform.DOScale(1.0f, 0f));
                    mSequence.OnComplete(() => gameObject.SetActive(false));
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
            isActived = false;
            //set this page's data null when hide.
            this.m_data = null;
        }

        ///Active this UI
        public virtual void Active(bool needAnim = true)
        {
            if (needAnim)
            {
                if (UIAnimation != null)
                {
                    float delayTime = UIAnimation[ShowAnimationName].time;
                    StartCoroutine(DelayShowPage(delayTime));
                }
                else
                {
                    this.gameObject.SetActive(true);
                    Sequence mSequence = DOTween.Sequence();
                    mSequence.Append(transform.DOScale(0.98f, 0.1f));
                    mSequence.Append(transform.DOScale(1.08f, 0.1f));
                    mSequence.OnComplete(() => transform.localScale = Vector3.one);
                }
            }
            else
            {
                this.gameObject.SetActive(true);
            }
            isActived = true;
        }
        private IEnumerator DelayShowPage(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.CustomSetActive(true);
        }

        private IEnumerator DelayHidePage(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.CustomSetActive(false);
        }

        #endregion

        #region internal api

        public bool CheckIfNeedBack()
        {
            if (type == UIType.Fixed || type == UIType.PopUp || type == UIType.None) return false;
            else if (mode == UIMode.NoNeedBack || mode == UIMode.DoNothing) return false;
            return true;
        }

        public override string ToString()
        {
            return ">Name:" + name + ",ID:" + id + ",Type:" + type.ToString() + ",ShowMode:" + mode.ToString() + ",Collider:" + collider.ToString();
        }

        public bool isActive()
        {
            //fix,if this page is not only one gameObject
            //so,should check isActived too.
            bool ret = gameObject != null && gameObject.activeSelf;
            return ret || isActived;
        }

        #endregion

        /// <summary>
        /// 关闭面板根据类名
        /// </summary>
        public virtual void ClosePage()
        {
            System.Type t = this.GetType();
            UIPanelManager.instance.ClosePage(t);
        }
    }//TTUIPage
}//namespace