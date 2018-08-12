using Assets.Script.Timer;
using Assets.Script.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUISceneInfo : MonoBehaviour
    {
        public Text RemainTimeText;
        public Text RemainEnemyCountText;
        public Scrollbar MpScrollbar;

        private int totleTime = 300;
        private int remainEnenyCount = 0;
        private bool startGame = false;
        private int timeId;
        private int maxMp, currentMp;

        public void Awake()
        {
            GameRoleMgr.instance.RemianEnemyCount.AddListener(OnRemianEnemyCount);
            GameRoleMgr.instance.CurrentPlayerMp.AddListener(OnCurrentPlayerMp);
        }

        public void OnDestroy()
        {
            GameRoleMgr.instance.RemianEnemyCount.RemoveListener(OnRemianEnemyCount);
            GameRoleMgr.instance.CurrentPlayerMp.RemoveListener(OnCurrentPlayerMp);
            CTimerManager.instance.RemoveLister(timeId);
        }

        public void StartGame(int roleMp)
        {
            startGame = true;
            timeId = CTimerManager.instance.AddListener(1, -1, BattleTimeChange);
        }

        private void OnRemianEnemyCount(int lastCount, int currentCount)
        {
            RemainTimeText.text = StringHelper.instance.IntToString(currentCount);
        }

        private void OnCurrentPlayerMp(int lastCount, int currentCount)
        {
            if (maxMp == 0)
            {
                maxMp = currentCount;
                currentMp = maxMp;
            }
            MpScrollbar.value = (currentMp*1.0f)/maxMp;
        }


        private void BattleTimeChange(int timeId)
        {
            totleTime--;
            RemainTimeText.text = GetCurrentRemainTime(totleTime);
        }

        private string GetCurrentRemainTime(int time)
        {
            string mintue = (time/60).ToString("D2");
            string second = (time % 60).ToString("D2");
            return (mintue + " : " + second);
        }
    }
}
