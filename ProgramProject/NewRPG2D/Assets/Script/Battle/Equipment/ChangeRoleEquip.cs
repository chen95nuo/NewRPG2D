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
        private SkeletonGraphic UIskeletonAnimation;
        [SerializeField]
        private Material sourceMaterial;

        private Skin customSkin;
        private Skeleton skeleton;
        private Dictionary<string, Attachment> originalAttachments = new Dictionary<string, Attachment>();

        private Dictionary<EquipTypeEnum, List<string>> equipSlot = new Dictionary<EquipTypeEnum, List<string>>
        {
            {EquipTypeEnum.Armor, new List<string> { "body", "center", "Hats1", "left_hand", "left_foot", "left_leg1", "left_leg2", "left_shoulder1", "left_shoulder2",
                "right_hand", "right_foot", "right_leg1", "right_leg2", "right_shoulder1", "right_shoulder2" } },
            {EquipTypeEnum.Sword, new List<string> { "weapon", "arrow_line1", "arrow_line2" } },
        };
        private Dictionary<BodyTypeEnum, string> BodySlot = new Dictionary<BodyTypeEnum, string>
        {
            {BodyTypeEnum.Beard, "Beard"},
            {BodyTypeEnum.Hair_1, "hair1"},
            {BodyTypeEnum.Hair_2, "hair2"},
            //{BodyTypeEnum.Face, "face"},
            //{BodyTypeEnum.Beard, "Beard"},
        };

        private void Awake()
        {
            if (UIskeletonAnimation != null)
                skeleton = UIskeletonAnimation.Skeleton;
            else
                skeleton = skeletonAnimation.Skeleton;
            customSkin = customSkin ?? new Skin("custom skin");
            skeleton.SetSkin(customSkin);
        }

        public void ChangeEquip(EquipTypeEnum equipType, string equipName, SexTypeEnum sexType = SexTypeEnum.Man)
        {
            List<string> equipment = null;
            if (equipSlot.TryGetValue(equipType, out equipment) == false)
            {
                return;
            }

            for (int i = 0; i < equipment.Count; i++)
            {
                string path = equipType == EquipTypeEnum.Armor ? string.Format("Equipment/{0}/{1}/{2}", sexType, equipName, equipment[i])
                                                               : string.Format("Equipment/Weapon/{0}/{1}", equipName, equipment[i]);
                Texture2D texture2D = ResourcesLoadMgr.instance.LoadResource<Texture2D>(path);
                ChangeEquip(equipment[i], texture2D);
            }
        }

        public void ChangeBody(BodyTypeEnum bodyType, string bodyName)
        {
            Texture2D texture2D = ResourcesLoadMgr.instance.LoadResource<Texture2D>("Body/" + bodyName);
            ChangeEquip(BodySlot[bodyType], texture2D);
        }

        public void ChangeOriginalEquip(EquipTypeEnum equipType)
        {
            for (int i = 0; i < equipSlot[equipType].Count; i++)
            {
                ChangeEquip(equipSlot[equipType][i], null);
            }
        }

        public void ChangeOriginalBody(BodyTypeEnum bodyType)
        {
            ChangeEquip(BodySlot[bodyType], null);
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
                if (originalAttachments.ContainsKey(targetSlotName) == false)
                {
                    originalAttachments[targetSlotName] = templateAttachment;
                }
                Sprite newSpr = newTexture == null ? null : SpriteHelper.CreateSprite(newTexture);

                Attachment newAttachment = newSpr == null ? originalAttachments[targetSlotName] : templateAttachment.GetRemappedClone(newSpr, sourceMaterial);

                customSkin.SetAttachment(visorSlotIndex, templateAttachment.Name, newAttachment);

                skeleton.SetAttachment(targetSlotName, templateAttachment.Name);

            }
        }
    }
}
