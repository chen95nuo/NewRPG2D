using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIRoleHpInfoItem : MonoBehaviour
    {
        public Image greenImg, redImg, BgImage;

        private Image useImage;
        private float maxHp, currentHp;
        private int instanceId;
        private Transform HpTarget;

        private void Awake()
        {
            EventManager.instance.AddListener<HpChangeParam>(EventDefineEnum.HpChange, OnHpChange);
            useImage = redImg;
        }

        private void LateUpdate()
        {
            if (HpTarget == null) return;
            transform.position = WorldToScreenPoint(HpTarget.position);
        }

        private void OnDestroy()
        {
            EventManager.instance.RemoveListener<HpChangeParam>(EventDefineEnum.HpChange, OnHpChange);
        }

        public void SetHpItemInfo(bool isHero, float Hp, int instanceId, Transform target)
        {
           
            HpTarget = target;
            this.instanceId = instanceId;
            maxHp = currentHp = Hp;
            if (isHero)
            {
                greenImg.gameObject.CustomSetActive(false);
                redImg.gameObject.CustomSetActive(false);
                BgImage.gameObject.CustomSetActive(false);
                useImage = greenImg;
            }
            else
            {
                greenImg.gameObject.CustomSetActive(false);
                redImg.gameObject.CustomSetActive(false);
                BgImage.gameObject.CustomSetActive(false);
                useImage = redImg;
            }
        }

        private void OnHpChange(HpChangeParam param)
        {
            if (param.role.InstanceId == instanceId)
            {
                currentHp = param.role.RolePropertyValue.RoleHp;
                if (currentHp <= 0)
                {
                    useImage.gameObject.CustomSetActive(false);
                    BgImage.gameObject.CustomSetActive(false);
                }
                else
                {
                    useImage.gameObject.CustomSetActive(true);
                    BgImage.gameObject.CustomSetActive(true);
                }
                useImage.fillAmount = currentHp / maxHp;
            }
        }

        private Vector3 WorldToScreenPoint(Vector2 pos)
        {
            pos.y += 5;
            Vector3 mousePos = Camera.main.WorldToScreenPoint(pos);
            return mousePos;
        }
    }
}
