using Assets.Script.Tools;
using Assets.Script.Utility.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIEnemyInfo : MonoBehaviour
    {
        public Image roleIcon;
        public Image HpImage;
        public Text HpValue;
        public Text maxHpValue;
        public Text roleName;
        public Animation appearAnimation;

        private float maxHp, currentHp;
        private int currentInstanceId;
        private string animationName = "EnemyInfoBoardAppear";
        private bool isAppear;

        private void Awake()
        {
            isAppear = false;
            EventManager.instance.AddListener<RoleBase>(EventDefineEnum.ClickEnemyRole, OnClickEnemyRole);
            EventManager.instance.AddListener<RoleBase>(EventDefineEnum.HpChange, OnHpChange);
        }

        private void OnDestroy()
        {
            EventManager.instance.RemoveListener<RoleBase>(EventDefineEnum.ClickEnemyRole, OnClickEnemyRole);
        }

        private void OnClickEnemyRole(RoleBase roleInfo)
        {
            if (roleInfo == null)
            {
                if (isAppear)
                {
                    isAppear = false;
                    appearAnimation[animationName].time = appearAnimation[animationName].length;
                    appearAnimation[animationName].speed = -1;
                    appearAnimation.Play(animationName);
                }
            }
            else
            {
                SetRoleInfo(roleInfo.RoleDetailInfo, roleInfo.InstanceId);
                if (isAppear == false)
                {
                    isAppear = true;
                    appearAnimation[animationName].time = 0;
                    appearAnimation[animationName].speed = 1;
                    appearAnimation.Play(animationName);
                }
            }
        }

        private void SetRoleInfo(RoleDetailData info, int instanceId)
        {
            currentInstanceId = instanceId;
            roleIcon.sprite = SpriteHelper.instance.GetIcon(SpriteAtlasTypeEnum.Icon, info.BattleIconSpriteName);
            currentHp = maxHp = info.Health;
            HpImage.fillAmount = 1;
            maxHpValue.text = " /" + StringHelper.instance.IntToString((int)maxHp);
            HpValue.text = StringHelper.instance.IntToString((int)currentHp);
            roleName.text = info.Name;
        }

        private void SetHpValue(float Hp)
        {
            currentHp = Hp;
            HpImage.fillAmount = currentHp / maxHp;
            HpValue.text = StringHelper.instance.IntToString((int)currentHp);
        }

        private void OnHpChange(RoleBase role)
        {
            if (currentInstanceId == role.InstanceId)
            {
               SetHpValue(role.RolePropertyValue.RoleHp);
            }
        }

    }
}
