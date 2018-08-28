using System;
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
        public BattleUIEnemyInfo EnemyInfo;
        public BattleUIMyRoleInfo RoleInfo;
        public BattleUISceneInfo SceneInfo;
        public BattleUIRoleHpInfo HpInfo;
        public Image StartImage;
        private Color tempColor;

        private bool startGame;
        private float addTime = 0;

        public void Awake()
        {
            EventManager.instance.AddListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
            EventManager.instance.AddListener<bool>(EventDefineEnum.GameOver, IsGameOver);
            tempColor = StartImage.color;
        }

        public void OnDestroy()
        {
            EventManager.instance.RemoveListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
            EventManager.instance.RemoveListener<bool>(EventDefineEnum.GameOver, IsGameOver);
        }

       
        private void Update()
        {
            addTime += Time.deltaTime;
            if (startGame && addTime > 1)
            {
                if (tempColor.a > 0)
                {
                    tempColor.a -= Time.deltaTime;
                    StartImage.color = tempColor;
                }
                else
                {
                    StartImage.gameObject.CustomSetActive(false);
                }
            }
          
        }

        private void LoadLevelUpdate(LoadLevelParam param)
        {
            startGame = true;
            StartImage.gameObject.SetActive(true);
            StartImage.color = Color.white;
            SceneInfo.StartGame();
        }

        public void IsGameOver(bool win)
        {
            gameObject.CustomSetActive(false);
        }
    }
}
