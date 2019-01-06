using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemGuildPointTalentLayerItem : MonoBehaviour
{
	public UIBox selectedLight;
	public UIButton alphy;
	public SpriteText layerLabel;

	private GuildStageConfig.ValueRange valueRangeData;
	public GuildStageConfig.ValueRange ValueRangeData
	{
		get { return valueRangeData; }
	}

	public void SetData(GuildStageConfig.ValueRange valueRange)
	{
		this.valueRangeData = valueRange;
		alphy.Data = valueRange;
		if (valueRange.Min == valueRange.Max)
			layerLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointTalent_layer", valueRange.Min);
		else
			layerLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointTalent_layerRange", valueRange.Min, valueRange.Max);
	}

	public void SetSelectedStat(bool selected)
	{
		selectedLight.Hide(!selected);
	}
}
