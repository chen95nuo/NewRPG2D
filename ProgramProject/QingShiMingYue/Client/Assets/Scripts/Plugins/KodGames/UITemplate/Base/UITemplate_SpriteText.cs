#if UNITY_EDITOR
using UnityEngine;

public class UITemplate_SpriteText : UITemplate
{
	public SpriteText template;
	public UITemplateData_SpriteText templateData;

	public override System.Type GetTargetType()
	{
		return typeof(SpriteText);
	}

	public override bool Apply(Component component)
	{
		SpriteText btnCtrl = component as SpriteText;
		if (btnCtrl == null)
			return false;

		if (btnCtrl.templateName == null || this.templateData == null)
			return false;

		if (btnCtrl.templateName.Equals(this.gameObject.name, System.StringComparison.OrdinalIgnoreCase) == false)
			return false;

		templateData.Format(btnCtrl, template, 1);
		return true;
	}
}

#endif
