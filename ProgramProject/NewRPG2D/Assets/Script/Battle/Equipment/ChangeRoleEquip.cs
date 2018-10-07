using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Utility.Tools;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using UnityEngine;

namespace Assets.Script.Battle
{




    public class ChangeRoleEquip : MonoBehaviour
    {
       [SerializeField] private SkeletonAnimation skeletonAnimation;
       [SerializeField] private Material sourceMaterial;

        private Skin customSkin;
        private Skeleton skeleton;

        private Dictionary<EquipTypeEnum, string> equipSlot = new Dictionary<EquipTypeEnum, string>
        {
            {EquipTypeEnum.Arrow, ""},
        };


        private void Start()
        {
            skeleton = skeletonAnimation.Skeleton;
            customSkin = customSkin ?? new Skin("custom skin");
            skeleton.SetSkin(customSkin);
        }

        public void ChangeEquip(EquipTypeEnum equipType, string equipName)
        {
            ChangeEquip(equipSlot[equipType], Texture2D.blackTexture);
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

                Attachment newAttachment = templateAttachment.GetRemappedClone(newSpr, sourceMaterial, false);

                customSkin.SetAttachment(visorSlotIndex, templateAttachment.Name, newAttachment);

                skeleton.SetAttachment(targetSlotName, templateAttachment.Name);

            }
        }
    }
}
