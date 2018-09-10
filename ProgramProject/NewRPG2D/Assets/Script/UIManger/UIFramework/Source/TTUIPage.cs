using System;
using Assets.Script.UIManger;

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

        //this page active flag
        protected bool isActived = false;

        //refresh page 's data.
        private object m_data = null;
        protected object data { get { return m_data; } }


        #region virtual api

        ///Active this UI
        private void Active()
        {
            this.gameObject.SetActive(true);
            isActived = true;
        }

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
        }

        /// <summary>
        /// Only Deactive UI wont clear Data.
        /// </summary>
        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
            isActived = false;
            //set this page's data null when hide.
            this.m_data = null;
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

    }//TTUIPage
}//namespace