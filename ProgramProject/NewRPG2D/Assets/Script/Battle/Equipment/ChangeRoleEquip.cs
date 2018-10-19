using System.Collections.Generic;
using Assets.Script.Utility;
using Assets.Script.Utility.Tools;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using UnityEngine;

namespace Assets.Script.Battle
{

    public class ChangeRoleEquip : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation skeletonAnimation;
        [SerializeField]
        private Material sourceMaterial;

        private Skin customSkin;
        private Skeleton skeleton;

        private Dictionary<EquipTypeEnum, List<string>> equipSlot = new Dictionary<EquipTypeEnum, List<string>>
        {
            {EquipTypeEnum.Armor, new List<string> { "body", "left_foot", "left_leg1", "left_leg2", "left_shoulder1", "left_shoulder2", "right_foot", "right_leg1", "right_leg2", "right_shoulder1", "right_shoulder2" } },
        };
        private Dictionary<BodyTypeEnum, string> BodySlot = new Dictionary<BodyTypeEnum, string>
        {
            {BodyTypeEnum.Beard, "face"},
            {BodyTypeEnum.Hair, "hair1"},
            {BodyTypeEnum.Body, "body1"},
        };

        private void Start()
        {
            skeleton = skeletonAnimation.Skeleton;
            customSkin = customSkin ?? new Skin("custom skin");
            skeleton.SetSkin(customSkin);
        }

        public void ChangeEquip(EquipTypeEnum equipType, string equipName)
        {
            for (int i = 0; i < equipSlot[equipType].Count; i++)
            {
                ChangeEquip(equipSlot[equipType][i],
                      ResourcesLoadMgr.instance.LoadResource<Texture2D>(string.Format("Equipment/{0}/{0}{1}", equipName, i)));
            }
        }

        public void ChangeBody(BodyTypeEnum bodyType, string bodyName)
        {
            ChangeEquip(BodySlot[bodyType], ResourcesLoadMgr.instance.LoadResource<Texture2D>("Body/" + bodyName));
        }

        private void ChangeEquip(string targetSlotName, Texture2D newTexture)
        {
            Slot targetSlot = skeleton.FindSlot(targetSlotName);

            if (targetSlot == null)
            {
                Debug.Log("未找到Slot");
            }
            else
            {
                Debug.Log("找到Slot" + targetSlot);

                int visorSlotIndex = targetSlot.Data.Index;

                Attachment templateAttachment = targetSlot.Attachment;

                Sprite newSpr = SpriteHelper.CreateSprite(newTexture);

                Attachment newAttachment = templateAttachment.GetRemappedClone(newSpr, sourceMaterial);

                customSkin.SetAttachment(visorSlotIndex, templateAttachment.Name, newAttachment);

                skeleton.SetAttachment(targetSlotName, templateAttachment.Name);

            }
        }
    }
}
