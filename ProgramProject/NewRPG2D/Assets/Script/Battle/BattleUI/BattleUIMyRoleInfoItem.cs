using Assets.Script.Tools;
using Assets.Script.Utility.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleUIMyRoleInfoItem : MonoBehaviour
    {
        public Image roleIcon;
        public Image HpImage;
        public Text HpValue;
        public Text maxHpValue;
        public Image SelectImage;
        

        private float maxHp, currentHp;
        public int CurrentInstanceId { get; private set; }

        private void Awake()
        {
            EventTriggerListener.Get(roleIcon.gameObject).onClick += OnClick;
            EventManager.instance.AddListener<int>(EventDefineEnum.ClickMyRole, OnClickMyRole);
        }

        public void SetRoleInfo(CardData info, int instanceId)
        {
            CurrentInstanceId = instanceId;
            roleIcon.sprite = SpriteHelper.instance.GetIcon(SpriteAtlasTypeEnum.Icon, info.BattleIconSpriteName);
            currentHp = maxHp = info.Health;
            HpImage.fillAmount = 1;
            maxHpValue.text = " /"+StringHelper.instance.IntToString((int)maxHp);
            HpValue.text = StringHelper.instance.IntToString((int)currentHp);
        }

        public void SetHpValue(float Hp)
        {
            currentHp = Hp;
            HpImage.fillAmount = currentHp/maxHp;
            HpValue.text = StringHelper.instance.IntToString((int) currentHp);
        }

        private void OnClickMyRole(int instanceId)
        {
            if (CurrentInstanceId == instanceId)
            {
                SelectImage.gameObject.SetActive(true);
            }
            else
            {
                SelectImage.gameObject.SetActive(false);
            }
        }

        private void OnClick(GameObject go)
        {
            EventManager.instance.SendEvent(EventDefineEnum.ClickMyRole, CurrentInstanceId);
        }
    }
}
