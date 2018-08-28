using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Timer;
using Assets.Script.Utility;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Script.Battle
{
    public class FxManger : TSingleton<FxManger>
    {
        private Dictionary<int, Transform> FxList;

        public override void Init()
        {
            base.Init();
            FxList = new Dictionary<int, Transform>();
        }

        public void Clean()
        {
            FxList.Clear();
        }

        public void PlayFx(string FxName, Transform parent)
        {
            Transform hitObject = GameRoleMgr.instance.SetRoleTransform<Transform>("BattleFX/" + FxName, parent.name + FxName, 1,
            parent, parent.position + parent.up, 0);
            SortingGroup fxSortingGroup = hitObject.GetComponent<SortingGroup>();
            SortingGroup parentGroup = parent.GetComponent<SortingGroup>();
            fxSortingGroup.sortingOrder = parentGroup.sortingOrder;
            fxSortingGroup.sortingLayerID = parentGroup.sortingLayerID;

            int timeId = CTimerManager.instance.AddListener(1, 1, DelayHide);
            FxList[timeId] = hitObject;
        }

        private void DelayHide(int timeId)
        {
            CTimerManager.instance.RemoveLister(timeId);
            ResourcesLoadMgr.instance.PushObjIntoPool(FxList[timeId].name, FxList[timeId].gameObject);
            FxList.Remove(timeId);
        }
    }
}
