
//功能：值变化监听
//创建者: 胡海辉
//创建时间：


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Assets.Script.EventMgr
{
    public class TSimpleNotifier<T> where T : IComparable
    {
        private T m_curValue;
        private T m_lastValue;
        private UnityAction<T, T> m_Listeners;

        public TSimpleNotifier()
        {
            this.m_Listeners = null;
            this.Default();
        }

        public TSimpleNotifier(T defaultValue)
            : this()
        {
            this.m_curValue = defaultValue;
        }

        private void _NotifyChanged()
        {
            if (this.m_Listeners != null)
            {
                Delegate[] invocationList = this.m_Listeners.GetInvocationList();
                if (invocationList != null)
                {
                    for (int i = 0; i < invocationList.Length; i++)
                    {
                        ((UnityAction<T, T>)invocationList[i])(this.m_curValue, this.m_lastValue);
                    }
                }
            }
        }

        public void AddListener(UnityAction<T, T> fun)
        {
            if (this.m_Listeners == null)
            {
                this.m_Listeners = fun;
            }
            else
            {
                this.m_Listeners = (UnityAction<T, T>)Delegate.Combine(this.m_Listeners, fun);
            }
            fun(this.m_curValue, this.m_lastValue);
        }

        public void Clear()
        {
            this.RemoveAllListener();
            this.Default();
        }

        protected void Default()
        {
            this.m_curValue = default(T);
            this.m_lastValue = default(T);
        }

        public void RemoveAllListener()
        {
            this.m_Listeners = null;
        }

        public void RemoveListener(UnityAction<T, T> fun)
        {
            this.m_Listeners = (UnityAction<T, T>)Delegate.Remove(this.m_Listeners, fun);
        }

        public void setDefault(T defaultValue)
        {
            this.m_curValue = defaultValue;
        }

        // Properties
        public T LastValue
        {
            get
            {
                return this.m_lastValue;
            }
        }

        public T Value
        {
            get
            {
                return this.m_curValue;
            }
            set
            {
                if (((this.m_curValue != null) || (value != null)) && ((this.m_curValue == null) || (this.m_curValue.CompareTo(value) != 0)))
                {
                    this.m_lastValue = this.m_curValue;
                    this.m_curValue = value;
                    this._NotifyChanged();
                }
            }
        }

    }
}
