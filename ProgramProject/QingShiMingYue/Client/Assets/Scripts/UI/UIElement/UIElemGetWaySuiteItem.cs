using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

//public class UIElemGetWaySuiteItem : MonoBehaviour
public class UIElemGetWaySuiteItem
{
	//public UIElemAssetIcon itemIcon;
	//public SpriteText SuiteNameLabel;
	//public SpriteText itemGetLabel;
	//public UIButton gotoBtn;

	//public AutoSpriteControlBase backBg;
	//private const float originalWidth = 248;
	//private const float originalHeight = 60;

	//private readonly Color AVATAR_TRAITDESC_TXCOLOR_GREEN = GameDefines.txColorGreen;

	//public void SetData(AvatarConfig.Avatar avatar, int index, SuiteConfig.AssembleSetting assemble)
	//{
	//    itemIcon.SetData(avatar.id);

	//    List<string> formatParams = new List<string>();

	//    for (int i = 0; i < assemble.Parts.Count; i++)
	//    {
	//        var part = assemble.Parts[i];
	//        for (int j = 0; j < part.Requiremets.Count; j++)
	//        {
	//            var require = part.Requiremets[j];
	//            string assetName = ItemInfoUtility.GetAssetName(require.Value);
	//            if (avatar.id == require.Value)
	//                assetName = AVATAR_TRAITDESC_TXCOLOR_GREEN + assetName + GameDefines.txColorWhite;
	//            formatParams.Add(assetName);
	//        }
	//    }

	//    string suiteDesc = GameUtility.FormatStringOnlyWithParams(assemble.Assembles[0].AssembleEffectDesc, formatParams.ToArray()) +
	//        "\n" + string.Format(GameUtility.GetUIString("UIDlgItemGetWay_GetWay_Label"), avatar.getways[index].desc);

	//    itemGetLabel.Text = suiteDesc;
	//    SuiteNameLabel.Text = assemble.Name;
	//    gotoBtn.Data = avatar.getways[index];
	//    backBg.SetSize(originalWidth, originalHeight + 55);
	//}

	//public void SetData(EquipmentConfig.Equipment equip, int index, SuiteConfig.AssembleSetting assemble)
	//{
	//    itemIcon.SetData(equip.id);

	//    List<string> formatParams = new List<string>();

	//    for (int i = 0; i < assemble.Parts.Count; i++)
	//    {
	//        var part = assemble.Parts[i];
	//        for (int j = 0; j < part.Requiremets.Count; j++)
	//        {
	//            var require = part.Requiremets[j];
	//            string assetName = ItemInfoUtility.GetAssetName(require.Value);
	//            if (equip.id == require.Value)
	//                assetName = AVATAR_TRAITDESC_TXCOLOR_GREEN + assetName + GameDefines.txColorWhite;
	//            formatParams.Add(assetName);
	//        }
	//    }
	//    string suiteDesc = GameUtility.FormatStringOnlyWithParams(assemble.Assembles[0].AssembleEffectDesc, formatParams.ToArray()) +
	//        "\n" + string.Format(GameUtility.GetUIString("UIDlgItemGetWay_GetWay_Label"), equip.getways[index].desc);
	//    if(assemble.Id == 0)
	//    {
	//        suiteDesc = string.Empty;
	//        for(int i = 0; i < formatParams.Count; i++)
	//        {
	//            suiteDesc += formatParams[i] + "\n";
	//        }
	//        suiteDesc += "\n" + string.Format(GameUtility.GetUIString("UIDlgItemGetWay_GetWay_Label"), equip.getways[index].desc);
	//    }
	//    itemGetLabel.Text = suiteDesc;
	//    SuiteNameLabel.Text = assemble.Name;
	//    gotoBtn.Data = equip.getways[index];
	//    if (assemble.Id == 0)
	//        UpDataBgHeight();
	//    else
	//        backBg.SetSize(originalWidth, originalHeight + 55);
	//}

	//public void SetData(SkillConfig.Skill skill, int index, SuiteConfig.AssembleSetting assemble)
	//{
	//    itemIcon.SetData(skill.id);

	//    List<string> formatParams = new List<string>();

	//    for (int i = 0; i < assemble.Parts.Count; i++)
	//    {
	//        var part = assemble.Parts[i];
	//        for (int j = 0; j < part.Requiremets.Count; j++)
	//        {
	//            var require = part.Requiremets[j];
	//            string assetName = ItemInfoUtility.GetAssetName(require.Value);
	//            if (skill.id == require.Value)
	//                assetName = AVATAR_TRAITDESC_TXCOLOR_GREEN + assetName + GameDefines.txColorWhite;
	//            formatParams.Add(assetName);
	//        }
	//    }
	//    string suiteDesc = GameUtility.FormatStringOnlyWithParams(assemble.Assembles[0].AssembleEffectDesc, formatParams.ToArray()) +
	//        "\n" + string.Format(GameUtility.GetUIString("UIDlgItemGetWay_GetWay_Label"), skill.getways[index].desc);
	//    if (assemble.Id == 0)
	//    {
	//        suiteDesc = string.Empty;

	//        for (int i = 0; i < formatParams.Count; i++)
	//        {
	//            suiteDesc += formatParams[i] + "\n";
	//        }
	//        suiteDesc += "\n" + string.Format(GameUtility.GetUIString("UIDlgItemGetWay_GetWay_Label"), skill.getways[index].desc);
	//    }
	//    itemGetLabel.Text = suiteDesc;
	//    SuiteNameLabel.Text = assemble.Name;
	//    gotoBtn.Data = skill.getways[index];
	//    if (assemble.Id == 0)
	//        UpDataBgHeight();
	//    else
	//        backBg.SetSize(originalWidth, originalHeight + 55);
	//}

	//public void UpDataBgHeight()
	//{
	//    float lineHeight = itemGetLabel.GetLineHeight();
	//    int lineCount = itemGetLabel.Text.Split('\n').Length;
	//    int lineNeed = lineCount ;
	//    if (lineNeed > 0)
	//    {
	//        float heightNeed = lineNeed * lineHeight;
	//        backBg.SetSize(originalWidth, originalHeight + heightNeed);
	//    }
	//}
}
