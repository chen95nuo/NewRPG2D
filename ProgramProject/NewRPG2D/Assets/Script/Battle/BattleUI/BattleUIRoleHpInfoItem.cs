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
            EventManager.instance.AddListener<RoleBase>(EventDefineEnum.HpChange, OnHpChange);
            useImage = redImg;
        }

        private void LateUpdate()
        {
            if (HpTarget == null) return;
            transform.position = WorldToScreenPoint(HpTarget.position);
        }

        private void OnDestroy()
        {
            EventManager.instance.RemoveListener<RoleBase>(EventDefineEnum.HpChange, OnHpChange);
        }

        public void SetHpItemInfo(bool isHero, float Hp, int instanceId, Transform target)
        {
            HpTarget = target;
            this.instanceId = instanceId;
            maxHp = currentHp = Hp;
            if (isHero)
            {
                greenImg.gameObject.SetActive(true);
                redImg.gameObject.SetActive(false);
                useImage = greenImg;
            }
            else
            {
                greenImg.gameObject.SetActive(false);
                redImg.gameObject.SetActive(true);
                useImage = redImg;
            }
        }

        private void OnHpChange(RoleBase role)
        {
            if (role.InstanceId == instanceId)
            {
                currentHp = role.RolePropertyValue.RoleHp;
                useImage.fillAmount = currentHp / maxHp;
                if (currentHp <= 0)
                {
                    useImage.gameObject.SetActive(false);
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
