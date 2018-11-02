using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIManager : MonoBehaviour
    {
        public BattleUISceneInfo SceneInfo;
        public BattleUIRoleHpInfo HpInfo;
        public GameObject BattleUI;
        public BattleEndAward EndAward;
        //  public Image StartImage;
        public Text AddSpeedTxt;
        private Color tempColor;

        private bool startGame;
        private float addTime = 0;

        public void Awake()
        {
            EventManager.instance.AddListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
            EventManager.instance.AddListener<bool>(EventDefineEnum.GameOver, IsGameOver);
            // tempColor = StartImage.color;
        }

        public void OnDestroy()
        {
            EventManager.instance.RemoveListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
            EventManager.instance.RemoveListener<bool>(EventDefineEnum.GameOver, IsGameOver);
        }

        private void LoadLevelUpdate(LoadLevelParam param)
        {
            startGame = true;
            StartCoroutine(SceneInfo.StartGame());
        }

        public void IsGameOver(bool win)
        {
            Time.timeScale = 1;
            StartCoroutine(DelayWin(win));
        }

        private IEnumerator DelayWin(bool win)
        {
            yield return new WaitForSeconds(1.5f);

            BattleUI.CustomSetActive(false);
            EndAward.gameObject.CustomSetActive(true);
            EndAward.ShowPanel(win);
        }

        float currentSpeed = 1;
        public void AddSpeedClick()
        {
            if (currentSpeed >= 2)
            {
                currentSpeed = 0;
            }
            currentSpeed += 0.5f;
            AddSpeedTxt.text = (int)currentSpeed + "X";
            Time.timeScale = currentSpeed;
        }

    }
}
