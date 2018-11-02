using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleEndAward : MonoBehaviour
    {
        public GameObject failedObj, victoryObj;
        public BattleEndPlayerInfo playerInfo;
        public BattleEndPlayerInfo enemyInfo;
        public BattleEndAwardList endAwardList;

        public void ShowPanel(bool isWin)
        {
            failedObj.CustomSetActive(!isWin);
            victoryObj.CustomSetActive(isWin);
            SetRoleInfo(isWin);
        }

        private void SetRoleInfo(bool isWin)
        {
            List<RoleBase> rolesHeroList = new List<RoleBase>(9);
            List<RoleBase> rolesEnemyList = new List<RoleBase>(9);
            foreach (var role in GameRoleMgr.instance.RoleDic)
            {
                if (role.Value.TeamId == TeamTypeEnum.Hero)
                {
                    rolesHeroList.Add(role.Value);
                }
                else if (role.Value.TeamId == TeamTypeEnum.Monster)
                {
                    rolesEnemyList.Add(role.Value);
                }
            }
            playerInfo.SetPlayerInfo(rolesHeroList);
            enemyInfo.SetPlayerInfo(rolesEnemyList);
            endAwardList.SetAwardInfo(LocalStartFight.instance.MapData.AwardItem,
                LocalStartFight.instance.MapData.TreasureBoxIds, isWin);

            int nextSceneId = LocalStartFight.instance.MapData.NextLessonID;
            if (nextSceneId != 0 && isWin)
            {
                GetPlayerData.Instance.GetData().CurrentLessonID = nextSceneId;
            }

        }

        public void BackHall()
        {
            GameRoleMgr.instance.ClearAllRole();
            Time.timeScale = 1;
            SceneManager.LoadScene("EasonMainScene");
        }
    }
}
