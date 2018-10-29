using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIRoleHpInfoItem : MonoBehaviour
    {
        public Image greenImg, redImg;

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
                greenImg.gameObject.SetActive(false);
                redImg.gameObject.SetActive(false);
                useImage = greenImg;
            }
            else
            {
                greenImg.gameObject.SetActive(false);
                redImg.gameObject.SetActive(false);
                useImage = redImg;
            }
        }

        private void OnHpChange(HpChangeParam param)
        {
            if (param.role.InstanceId == instanceId)
            {
                currentHp = param.role.RolePropertyValue.RoleHp;
                useImage.fillAmount = currentHp / maxHp;
                if (currentHp <= 0)
                {
                    useImage.gameObject.SetActive(false);
                }
                else
                {
                    useImage.gameObject.CustomSetActive(true);

                }
            }
        }

        private Vector3 WorldToScreenPoint(Vector2 pos)
        {
            pos.y += 3;
            Vector3 mousePos = Camera.main.WorldToScreenPoint(pos);
            return mousePos;
        }
    }
}
