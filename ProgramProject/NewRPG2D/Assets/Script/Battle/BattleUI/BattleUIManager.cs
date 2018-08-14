using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIManager : MonoBehaviour
    {
        public BattleUIEnemyInfo EnemyInfo;
        public BattleUIMyRoleInfo RoleInfo;
        public BattleUISceneInfo SceneInfo;
        public BattleUIRoleHpInfo HpInfo;

        public void Awake()
        {
            EventManager.instance.AddListener<LoadLevelParam>(EventDefineEnum.LoadLevel, LoadLevelUpdate);
        }

        private void LoadLevelUpdate(LoadLevelParam param)
        {
            SceneInfo.StartGame();
        }
    }
}
