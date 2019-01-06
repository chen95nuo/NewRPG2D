using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAvatarTrainningTab : UIModule
{
	// Avatar properties
	public UIElemAssetIcon avatarIconBtn;
	public SpriteText avatarNameLabel;
	public SpriteText avatarQualityLabel;
	public SpriteText avatarWorthLabel;
	public SpriteText avatarEquipLabel;
	public SpriteText healthLabel;
	public SpriteText attackLabel;
	public SpriteText defenceLabel;

	// Cost desc
	public SpriteText trainningItem;
	public SpriteText attributePoint;

	// Training settings
	public List<UIRadioBtn> trainBtns;
	public UIRadioBtn trainBtn;
	public SpriteText normalOneCostLabel;
	public UIRadioBtn tenTrainBtn;
	public SpriteText normalTenCostLabel;
	public UIRadioBtn ingotBtn;
	public SpriteText ingotOneCostLabel;
	public UIRadioBtn tenIngoBtn;
	public SpriteText ingotTenCostLabel;

	// Training button
	public GameObject trainRoot;
	public SpriteText trainFuncMessage;
	public UIButton trainningBtn;

	// Training result
	public GameObject resultRoot;
	public SpriteText originalAvatarHealth;
	public SpriteText originalAvatarAttack;
	public SpriteText originalAvatarDefence;
	public SpriteText powerUpAvatarHealth;
	public SpriteText powerUpAvatarAttack;
	public SpriteText powerUpAvatarDefence;
	public SpriteText attrPointCostLabel;

	// Local data.	
	private KodGames.ClientClass.Avatar avatarData;

	//public override bool Initialize()
	//{
	//    if (!base.Initialize())
	//        return false;

	//    // Save training setting Type in button
	//    trainBtn.Data = _TrainType.CommonTrain;
	//    tenTrainBtn.Data = _TrainType.TenCommonTrain;
	//    ingotBtn.Data = _TrainType.MoneyTrain;
	//    tenIngoBtn.Data = _TrainType.TenMoneyTrain;

	//    return true;
	//}

	//public override bool OnShow(_UILayer layer, params object[] userDatas)
	//{
	//    if (base.OnShow(layer, userDatas) == false)
	//        return false;

	//    avatarData = userDatas[0] as KodGames.ClientClass.Avatar;

	//    SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().UpdateTabStatus(_UIType.UIPnlAvatarTrainningTab, avatarData);

	//    // Set AvatarTrain Way Labels.
	//    SetTrainningLabels();

	//    // If last training has not been saved, show training result UI
	//    if (avatarData.HasUnsavedTrainingAttribute())
	//        ShowResultUI();
	//    else
	//        ShowTrainingUI(GetDefaultTrainingId());

	//    return true;
	//}

	//private void SetTrainningLabels()
	//{
	//    var normalTrainnings = new List<AvatarConfig.TrainingSetting>();
	//    var ingotTrainnings = new List<AvatarConfig.TrainingSetting>();

	//    foreach (var trainningSetting in ConfigDatabase.DefaultCfg.AvatarConfig.trainingSettings)
	//    {
	//        normalTrainnings.Add(trainningSetting);
	//        foreach (var cost in trainningSetting.costs)
	//        {
	//            if (IDSeg._SpecialId.RealMoney.Equals(cost.id))
	//            {
	//                normalTrainnings.Remove(trainningSetting);
	//                ingotTrainnings.Add(trainningSetting);
	//                break;
	//            }
	//        }
	//    }

	//    normalTrainnings.Sort(SortByCost);
	//    ingotTrainnings.Sort(SortByCost);

	//    // Set cost.
	//    AvatarConfig avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig;

	//    normalOneCostLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTrainningTab_Label_Cost"),
	//        avatarConfig.GetTrainningSetting((int)trainBtn.data, avatarData.BreakthoughtLevel).costs[0].count);

	//    normalTenCostLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTrainningTab_Label_Cost"),
	//        avatarConfig.GetTrainningSetting((int)tenTrainBtn.data, avatarData.BreakthoughtLevel).costs[0].count);

	//    ingotOneCostLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTrainningTab_Label_IngotCost"),
	//        avatarConfig.GetTrainningSetting((int)ingotBtn.data, avatarData.BreakthoughtLevel).costs[0].count,
	//        avatarConfig.GetTrainningSetting((int)ingotBtn.data, avatarData.BreakthoughtLevel).costs[1].count);

	//    ingotTenCostLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTrainningTab_Label_IngotCost"),
	//        avatarConfig.GetTrainningSetting((int)tenIngoBtn.data, avatarData.BreakthoughtLevel).costs[0].count,
	//        avatarConfig.GetTrainningSetting((int)tenIngoBtn.data, avatarData.BreakthoughtLevel).costs[1].count);

	//    // Set default checked Train Tab.
	//    trainningBtn.data = trainBtn.data;
	//    foreach (UIRadioBtn trainWayBtn in trainBtns)
	//    {
	//        if (trainWayBtn.data.Equals(trainBtn.data))
	//            trainWayBtn.Value = true;
	//        else
	//            trainWayBtn.Value = false;
	//    }

	//    // Set the Train Func Message.
	//    trainFuncMessage.Text = GameUtility.FormatUIString(
	//        "UIPnlAvatarTrainningTab_Ctrl_TrainAttrExchange",
	//        ConfigDatabase.DefaultCfg.AvatarConfig.trainingProportion.AP,
	//        ConfigDatabase.DefaultCfg.AvatarConfig.trainingProportion.DP,
	//        ConfigDatabase.DefaultCfg.AvatarConfig.trainingProportion.MaxHP);
	//}

	//private void SetAvatarControls()
	//{
	//    // Icon
	//    avatarIconBtn.SetData(avatarData);

	//    // Name
	//    avatarNameLabel.Text = ItemInfoUtility.GetAssetName(avatarData.ResourceId);

	//    //Set avatar quality.
	//    avatarQualityLabel.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Label_Quality", ItemInfoUtility.GetLevelCN(ItemInfoUtility.GetAssetQualityLevel(avatarData.ResourceId)));

	//    //Set avatar worth.
	//    avatarWorthLabel.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Label_Score", MathFactory.ExpressionCalculate.GetValue_AvatarEvaluation(avatarData));

	//    // Set avatar equipped information.
	//    avatarEquipLabel.Text = "";
	//    if (ItemInfoUtility.IsAvatarEquipped(avatarData))
	//        avatarEquipLabel.Text = GameUtility.FormatUIString("UIPnlAvatar_State_LineUp", GameDefines.txColorWhite);

	//    if (ItemInfoUtility.IsAvatarCheered(avatarData))
	//        avatarEquipLabel.Text = GameUtility.FormatUIString("UIPnlAvatar_State_Encourage", GameDefines.txColorGreen);

	//    // Set avatar attribute.
	//    foreach (var attribute in PlayerDataUtility.GetAvatarAttributesForTraining(avatarData, SysLocalDataBase.Inst.LocalPlayer))
	//    {
	//        switch (attribute.type)
	//        {
	//            case _AvatarAttributeType.MaxHP:
	//                healthLabel.Text = ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value);
	//                break;

	//            case _AvatarAttributeType.AP:
	//                attackLabel.Text = ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value);
	//                break;

	//            case _AvatarAttributeType.DP:
	//                defenceLabel.Text = ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value);
	//                break;
	//        }
	//    }

	//    //Set avatar training point.
	//    attributePoint.Text = avatarData.AttributePoint.ToString();

	//    //Set training item.		
	//    var trainingConsumble = ItemInfoUtility.FindConsumableById(SysLocalDataBase.Inst.LocalPlayer.Consumables, ConfigDatabase.DefaultCfg.ItemConfig.avatarTrainItemId);
	//    int itemCount = trainingConsumble != null ? trainingConsumble.Amount : 0;
	//    trainningItem.Text = itemCount.ToString();
	//}

	//private void SetSelectedTrainingId(int trainingId)
	//{
	//    foreach (UIRadioBtn trainningBtn in trainBtns)
	//    {
	//        if (trainingId == (int)trainningBtn.Data)
	//        {
	//            trainningBtn.Value = true;
	//            break;
	//        }
	//    }
	//}

	//private int GetSelectedTrainingId()
	//{
	//    foreach (UIRadioBtn trainningBtn in trainBtns)
	//        if (trainningBtn.Value)
	//            return (int)trainningBtn.Data;

	//    return IDSeg.InvalidId;
	//}

	//private void EnalbeTrainingSelectButton(bool enable)
	//{
	//    int layer = enable ? GameDefines.UIRaycastLayer : GameDefines.UIIgnoreRaycastLayer;

	//    trainBtn.gameObject.layer = layer;
	//    tenTrainBtn.gameObject.layer = layer;
	//    ingotBtn.gameObject.layer = layer;
	//    tenIngoBtn.gameObject.layer = layer;
	//}

	//private int GetDefaultTrainingId()
	//{
	//    return (int)trainBtn.Data;
	//}

	//private static int SortByCost(AvatarConfig.TrainingSetting t1, AvatarConfig.TrainingSetting t2)
	//{
	//    return t1.costs[0].count.CompareTo(t2.costs[0].count);
	//}

	//[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	//private void OnClose(UIButton btn)
	//{
	//    SysUIEnv.Instance.HideUIModule(_UIType.UIPnlAvatarPowerUpTab);
	//}

	//#region OnShow training UI
	//private void ShowTrainingUI(int trainingId)
	//{
	//    SetAvatarControls();
	//    EnalbeTrainingSelectButton(true);
	//    SetSelectedTrainingId(trainingId);

	//    trainRoot.SetActive(true);
	//    resultRoot.SetActive(false);
	//}

	//[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	//private void OnTrainningClick(UIButton btn)
	//{
	//    int trainingId = GetSelectedTrainingId();
	//    RequestMgr.Inst.Request(new TrainAvatarReq(avatarData.Guid, trainingId));
	//}

	//public void OnTrainingSuccess(string avatarGUID)
	//{
	//    if (avatarData.Guid.Equals(avatarGUID) == false)
	//        return;

	//    ShowResultUI();
	//}
	//#endregion

	//#region OnShow training result UI
	//private void ShowResultUI()
	//{
	//    SetAvatarControls();
	//    EnalbeTrainingSelectButton(false);
	//    SetSelectedTrainingId(avatarData.LastTrainId);
	//    SetResultUI();

	//    trainRoot.SetActive(false);
	//    resultRoot.SetActive(true);
	//}

	//private void SetResultUI()
	//{
	//    // Attribute delta value.
	//    double healthDelta = 0;
	//    double attackDelta = 0;
	//    double defenceDelta = 0;

	//    // Set current avatar attribute
	//    foreach (var attribute in PlayerDataUtility.GetAvatarAttributesForTraining(avatarData, SysLocalDataBase.Inst.LocalPlayer))
	//    {
	//        switch (attribute.type)
	//        {
	//            case _AvatarAttributeType.MaxHP:
	//                healthDelta = attribute.value;
	//                originalAvatarHealth.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Label_HealthBefore", ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value));
	//                break;

	//            case _AvatarAttributeType.AP:
	//                attackDelta = attribute.value;
	//                originalAvatarAttack.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Label_AttackBefore", ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value));
	//                break;

	//            case _AvatarAttributeType.DP:
	//                defenceDelta = attribute.value;
	//                originalAvatarDefence.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Label_DefenseBefore", ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value));
	//                break;
	//        }
	//    }

	//    // Set trained attribute
	//    KodGames.ClientClass.Avatar trainedAvatar = new KodGames.ClientClass.Avatar();
	//    trainedAvatar.ResourceId = avatarData.ResourceId;
	//    trainedAvatar.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
	//    trainedAvatar.LevelAttrib.Level = avatarData.LevelAttrib.Level;
	//    trainedAvatar.LevelAttrib.Experience = avatarData.LevelAttrib.Experience;
	//    trainedAvatar.BreakthoughtLevel = avatarData.BreakthoughtLevel;
	//    trainedAvatar.TrainingAttribute = new List<KodGames.ClientClass.Attribute>();

	//    foreach (KodGames.ClientClass.Attribute attr in avatarData.TrainingAttribute)
	//    {
	//        KodGames.ClientClass.Attribute trainedAttr = new KodGames.ClientClass.Attribute();
	//        trainedAttr.Type = attr.Type;
	//        trainedAttr.Value = attr.Value;

	//        foreach (KodGames.ClientClass.Attribute unsavedAttr in avatarData.UnsavedTrainingAttribute)
	//        {
	//            if (unsavedAttr.Type == attr.Type)
	//            {
	//                trainedAttr.Value += unsavedAttr.Value;
	//                break;
	//            }
	//        }

	//        trainedAvatar.TrainingAttribute.Add(trainedAttr);
	//    }

	//    foreach (var attribute in PlayerDataUtility.GetAvatarAttributesForTraining(trainedAvatar, SysLocalDataBase.Inst.LocalPlayer))
	//    {
	//        switch (attribute.type)
	//        {
	//            case _AvatarAttributeType.MaxHP:
	//                healthDelta = KodGames.Math.RoundToInt(attribute.value - healthDelta) / ConfigDatabase.DefaultCfg.AvatarConfig.trainingProportion.MaxHP;

	//                if (healthDelta != 0)
	//                    SetAttrStrFormat(healthDelta, powerUpAvatarHealth, attribute);
	//                else
	//                    powerUpAvatarHealth.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Label_HealthBefore", ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value));
	//                break;

	//            case _AvatarAttributeType.AP:
	//                attackDelta = KodGames.Math.RoundToInt(attribute.value - attackDelta) / ConfigDatabase.DefaultCfg.AvatarConfig.trainingProportion.AP;

	//                // Set the attack value.
	//                if (attackDelta != 0)
	//                    SetAttrStrFormat(attackDelta, powerUpAvatarAttack, attribute);
	//                else
	//                    powerUpAvatarAttack.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Label_AttackBefore", ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value));


	//                break;

	//            case _AvatarAttributeType.DP:
	//                defenceDelta = KodGames.Math.RoundToInt(attribute.value - defenceDelta) / ConfigDatabase.DefaultCfg.AvatarConfig.trainingProportion.DP;

	//                //Set the defense value.
	//                if (defenceDelta != 0)
	//                    SetAttrStrFormat(defenceDelta, powerUpAvatarDefence, attribute);
	//                else
	//                    powerUpAvatarDefence.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Label_DefenseBefore", ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value));
	//                break;
	//        }
	//    }

	//    attrPointCostLabel.Text = GameUtility.FormatUIString("UIPnlAvatarTrainningTab_Ctrl_TrainAttrCost", KodGames.Math.RoundToInt(healthDelta + attackDelta + defenceDelta));
	//}


	//private void SetAttrStrFormat(double delta, SpriteText txtCtrl, AttributeCalculator.Attribute attribute)
	//{
	//    int factor = ConfigDatabase.DefaultCfg.AvatarConfig.GetAttrFactorByType(attribute.type);

	//    string attributeAddKey = "";
	//    string attributeAddNoFactorKey = "";
	//    Color attributeDeltaColor;

	//    if (delta > 0)
	//    {
	//        attributeAddKey = "UIPnlTrainningResultTab_Label_HealthAdd";
	//        attributeAddNoFactorKey = "UIPnlTrainningResultTab_Label_HealthAddNoFactor";
	//        attributeDeltaColor = GameDefines.txColorGreen;
	//    }
	//    else
	//    {
	//        attributeAddKey = "UIPnlTrainningResultTab_Label_HealthReduce";
	//        attributeAddNoFactorKey = "UIPnlTrainningResultTab_Label_HealthReduceNoFactor";
	//        attributeDeltaColor = GameDefines.txColorRed;
	//    }

	//    if (factor != 1)
	//        txtCtrl.Text = GameUtility.FormatUIString(attributeAddKey,
	//                _AvatarAttributeType.GetDisplayNameByType(attribute.type, ConfigDatabase.DefaultCfg),
	//                ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value),
	//                attributeDeltaColor,
	//                ItemInfoUtility.GetAttribDisplayString(attribute.type, delta),
	//                ConfigDatabase.DefaultCfg.AvatarConfig.GetAttrFactorByType(attribute.type),
	//                GameDefines.txColorWhite);
	//    else
	//        txtCtrl.Text = GameUtility.FormatUIString(attributeAddNoFactorKey,
	//                _AvatarAttributeType.GetDisplayNameByType(attribute.type, ConfigDatabase.DefaultCfg),
	//                ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value),
	//                attributeDeltaColor,
	//                ItemInfoUtility.GetAttribDisplayString(attribute.type, delta),
	//                GameDefines.txColorWhite);
	//}

	//[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	//private void OnSaveTrainingResultClick(UIButton btn)
	//{
	//    RequestMgr.Inst.Request(new SaveTrainedAvatarReq(avatarData.Guid));
	//}

	//public void OnSaveTrainingResultSuccess(string avatarGUID, int lastTrainId)
	//{
	//    ShowTrainingUI(lastTrainId != IDSeg.InvalidId ? lastTrainId : GetDefaultTrainingId());
	//}

	//[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	//private void OnCancelTrainningResultClick(UIButton btn)
	//{
	//    RequestMgr.Inst.Request(new UnsaveTrainedAvatarReq(avatarData.Guid));
	//}

	//public void OnCancelTrainingResultSuccess(string avatarGUID, int lastTrainId)
	//{
	//    ShowTrainingUI(lastTrainId != IDSeg.InvalidId ? lastTrainId : GetDefaultTrainingId());
	//}
	//#endregion
}
