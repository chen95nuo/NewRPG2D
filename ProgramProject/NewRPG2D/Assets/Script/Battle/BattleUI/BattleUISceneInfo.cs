using System.Collections.Generic;
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
        public Text PlayerValue;
        public Slider EnemySlider;
        public Text EnemyValue;

        private int totleTime = 300;
        private int remainEnenyCount = 0;
        private bool startGame = false;
        private int timeId;
        private float maxPlayerHp, currentPlayerHp, maxEnemyHp, currentEnemyHp;

        public void Awake()
        {
            
        }

        public void OnDestroy()
        {
            CTimerManager.instance.RemoveLister(timeId);
            EventManager.instance.AddListener<HpChangeParam>(EventDefineEnum.HpChange, HpChange);
        }

        public void StartGame()
        {
            Debug.LogError("start game");
            startGame = true;
            timeId = CTimerManager.instance.AddListener(1, -1, BattleTimeChange);
            List<RoleBase> heroList = GameRoleMgr.instance.RolesHeroList;
            List<RoleBase> enemyList = GameRoleMgr.instance.RolesEnemyList;
            for (int i = 0; i < heroList.Count; i++)
            {
                maxPlayerHp += heroList[i].RolePropertyValue.RoleHp;
            }
            currentPlayerHp = maxPlayerHp;
            SetHpSilderInfo(PlayerSlider, PlayerValue, maxPlayerHp, currentPlayerHp);

            for (int i = 0; i < enemyList.Count; i++)
            {
                maxEnemyHp += enemyList[i].RolePropertyValue.RoleHp;
            }
            currentEnemyHp = maxEnemyHp;
            SetHpSilderInfo(EnemySlider, EnemyValue, maxEnemyHp, currentEnemyHp);
            EventManager.instance.AddListener<HpChangeParam>(EventDefineEnum.HpChange, HpChange);
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

        private void HpChange(HpChangeParam param)
        {
            if(param.role.TeamId == TeamTypeEnum.Hero)
            {
                currentPlayerHp -= param.changeValue;
                SetHpSilderInfo(PlayerSlider, PlayerValue, maxPlayerHp, currentPlayerHp);
            }
            else if(param.role.TeamId == TeamTypeEnum.Monster)
            {
                currentEnemyHp -= param.changeValue;
                SetHpSilderInfo(EnemySlider, EnemyValue, maxEnemyHp, currentEnemyHp);
            }
            
        }

        private void SetHpSilderInfo(Slider slider, Text text, float maxValue, float currentValue)
        {
            text.text = currentValue + "/" + maxValue;
            slider.value = currentValue/maxValue;
        }
    }
}
