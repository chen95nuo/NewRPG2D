
using Assets.Script.Timer;
using UnityEngine;

namespace Assets.Script.Battle.LevelManager
{
    public class LevelManager : MonoBehaviour
    {
       [SerializeField] private Transform heroPoint;
       [SerializeField] private Transform monsterPoint;

        public void Start()
        {
            CTimerManager.instance.AddListener(0.1f, 1, AddRole);
        }

        public void OnDestroy()
        {
           
        }

        private void AddRole(int timeId)
        {
            CTimerManager.instance.RemoveLister(timeId);
            GameRoleMgr.instance.AddHeroRole("", "Hero", transform, heroPoint.position,0,1001,0);
            GameRoleMgr.instance.AddMonsterRole("", "Monster", transform, monsterPoint.position, 0, 1001, 0);
        }
    }
}
