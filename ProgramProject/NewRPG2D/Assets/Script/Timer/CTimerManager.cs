using Assets.Script.Base;
using Assets.Script.EventMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Timer
{
    public class CTimerManager : TSingleton<CTimerManager>, IDisposable
    {
        private List<CTimer> mCTimerList;
        private const int m_deltaTime = 100;
        private int intervalTime = 0;
        private int m_sequenceTime = 0;

        public override void Init()
        {
            base.Init();
            mCTimerList = new List<CTimer>();
        }

        public void Update(int deltaTime)
        {
            intervalTime += deltaTime;
            if (intervalTime > m_deltaTime)
            {
                intervalTime = 0;
                int i = 0;
                while (i < mCTimerList.Count)
                {
                    CTimer mCTimer = mCTimerList[i];
                    if (mCTimer.isFinish)
                    {
                        mCTimer.Dispose();
                        mCTimerList.Remove(mCTimer);
                    }
                    else
                    {
                        mCTimerList[i].Update(m_deltaTime);
                        i++;
                    }
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            mCTimerList = null;
        }

        public int AddListener(int durationTime, int loopTime, TDelegate<int> mCTimeEventHandle)
        {
            CTimer mCTimer = new CTimer(++m_sequenceTime, durationTime, loopTime);
            mCTimer.AddListener(mCTimeEventHandle);
            mCTimerList.Add(mCTimer);
            return m_sequenceTime;
        }

        public void RemoveLister(int sequenceTime)
        {
            CTimer mCTimer = GetCTimer(sequenceTime);
            RemoveCTimer(mCTimer);
        }

        public void RemoveLister(int sequenceTime, TDelegate<int> eventHandle)
        {
            CTimer mCTimer = GetCTimer(sequenceTime);
            if (mCTimer!=null && mCTimer.EqualByDelegate(eventHandle))
            {
                RemoveCTimer(mCTimer);
            }
        }

        public void RemoveAll()
        {
            for (int i = 0; i < mCTimerList.Count; i++)
            {
                mCTimerList[i].Dispose();
            }
            mCTimerList.Clear();
        }

        public void PauseTimer(int sequenceTime)
        {
            CTimer mCTimer = GetCTimer(sequenceTime);
            if (mCTimer != null)
            {
                mCTimer.SetRunState(false);
            }
        }

        public void ResetTimer(int sequenceTime)
        {
            CTimer mCTimer = GetCTimer(sequenceTime);
            if (mCTimer != null)
            {
                mCTimer.SetRunState(true);
                mCTimer.ResetTime();
            }
        }

        public void ResetDurationTime(int sequenceTime,int duration)
        {
            CTimer mCTimer = GetCTimer(sequenceTime);
            if (mCTimer != null)
            {
                mCTimer.SetRunState(true);
                mCTimer.ResetDurationTime(duration);
            }
        }

        private CTimer GetCTimer(int sequenceTime)
        {
            for (int i = 0; i < mCTimerList.Count; i++)
            {
                CTimer mCTimer = mCTimerList[i];
                if (mCTimer.EqualBySequenceTime(sequenceTime))
                {
                    return mCTimer;
                }
            }
            return null;
        }

        private void RemoveCTimer(CTimer mCTimer)
        {
            mCTimer.Finish();
            mCTimer.Dispose();
            mCTimerList.Remove(mCTimer);
        }
    }
}
