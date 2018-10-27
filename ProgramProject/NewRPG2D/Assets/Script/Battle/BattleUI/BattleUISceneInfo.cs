using Assets.Script.Timer;
using Assets.Script.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUISceneInfo : MonoBehaviour
    {
        public Text RemainTimeText;
        public Slider PlayerSlider;
        public string playerValue;
        public Slider EnemySlider;
        public string enemyValue;

        private int totleTime = 300;
        private int remainEnenyCount = 0;
        private bool startGame = false;
        private int timeId;
        private int maxMp, currentMp;

        public void Awake()
        {
        }

        public void OnDestroy()
        {
            CTimerManager.instance.RemoveLister(timeId);
        }

        public void StartGame()
        {
            Debug.LogError("start game");
            startGame = true;
            timeId = CTimerManager.instance.AddListener(1, -1, BattleTimeChange);
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
