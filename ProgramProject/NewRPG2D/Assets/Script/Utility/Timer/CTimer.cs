//功能： 单个计时器
//创建者: 胡海辉
//创建时间：
using System;

namespace Assets.Script.Timer
{
    public class CTimer : IDisposable
    {
        public event TDelegate<int> CTimerEvent;
        public event TDelegate<int> CTimerTEvent;
        public float m_durationTime
        {
            get;
            private set;
        }
        public int m_loopTime
        {
            get;
            private set;
        }
        public int m_sequenceTime
        {
            get;
            private set;
        }
        public bool isFinish = false;
        private bool running = false;
        private float intervalTime = 0;

        public CTimer(int sequenceTime, float durationTime, int loopTime)
        {
            AddData(sequenceTime, durationTime, loopTime);
        }

        public void Init()
        {
            m_durationTime = 0;
            m_loopTime = 0;
            isFinish = false;
        }

        public void Dispose()
        {
            CTimerEvent = null;
            m_durationTime = 0;
            m_loopTime = 0;
            m_sequenceTime = 0;
            intervalTime = 0;
            isFinish = false;
        }

        public void AddListener(TDelegate<int> mCTimeEventHandle)
        {
            CTimerEvent += mCTimeEventHandle;
        }

        //public void AddListener<T>(TDelegate<int, T> mCTimeEventHandle)
        //{
        //    CTimerEvent += mCTimeEventHandle;
        //}

        public void RemoveListener(int sequenceTime, TDelegate<int> mCTimeEventHandle)
        {
            if (sequenceTime == m_sequenceTime)
                CTimerEvent -= mCTimeEventHandle;
        }

        public void Update(float deltaTime)
        {
            if (m_loopTime == 0)
                isFinish = true;

            if (isFinish || !running) return;

            intervalTime += deltaTime;
            if (intervalTime > m_durationTime)
            {
                intervalTime -= m_durationTime;
                m_loopTime--;
                Finish();
            }
        }

        public void Finish(int sequenceTime)
        {
            if (sequenceTime == m_sequenceTime)
                Finish();
        }

        public void Finish()
        {
            if (null == CTimerEvent) return;
            CTimerEvent(m_sequenceTime);
        }

        public void SetRunState(bool state)
        {
            running = state;
        }

        public void ResetTime()
        {
            intervalTime = 0;
        }

        public void ResetDurationTime(float duration)
        {
            if (m_durationTime == duration) return;
            m_durationTime = duration;
            intervalTime = 0;
        }

        public bool EqualBySequenceTime(int sequenceTime)
        {
            return m_sequenceTime == sequenceTime;
        }

        public bool EqualByDelegate(TDelegate<int> m_eventHandle)
        {
            return CTimerEvent == m_eventHandle;
        }

        private void AddData(int sequenceTime, float durationTime, int loopTime)
        {
            running = true;
            m_sequenceTime = sequenceTime;
            m_durationTime = durationTime;
            m_loopTime = loopTime;
            if (loopTime == 0)
            {
                m_loopTime = -1;
            }
        }
    }
}
