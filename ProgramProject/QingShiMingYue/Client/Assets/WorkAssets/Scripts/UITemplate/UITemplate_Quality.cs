#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

public class UITemplate_Quality: UITemplate
{
	public UITemplate_AutoSpriteControlBase emptyBase;
	public UITemplate_AutoSpriteControlBase filedBase;

	public override System.Type GetTargetType()
	{
		return emptyBase.GetTargetType();
	}

	public override bool Apply(Component component)
	{
		AutoSpriteControlBase btnCtrl = component as AutoSpriteControlBase;
		if (btnCtrl == null)
			return false;

		if (btnCtrl.templateName == null)
			return false;

		if (btnCtrl.templateName.Equals(this.gameObject.name, System.StringComparison.OrdinalIgnoreCase) == false)
			return false;

		emptyBase.templateData.Format(btnCtrl, emptyBase.template);

		// Set Filed Base.
		UIElemBreakThroughBtn breakBtn = component.GetComponent<UIElemBreakThroughBtn>();
		if (breakBtn == null)
			breakBtn = component.gameObject.AddComponent<UIElemBreakThroughBtn>();

		if (breakBtn.emptyBase == null)
		{
			breakBtn.emptyBase = component.GetComponent<UIBox>();

			if (breakBtn.emptyBase == null)
			{
				breakBtn.emptyBase = component.gameObject.AddComponent<UIBox>();
				breakBtn.emptyBase.name = "emptyBase";
			}
		}

		emptyBase.templateData.Format(breakBtn.emptyBase, emptyBase.template);

		if (breakBtn.fieldBase == null)
		{
			breakBtn.fieldBase = ObjectUtility.FindComponentInChildren<UIBox>(component.gameObject, "FieldBase", 1);

			if (breakBtn.fieldBase == null)
				breakBtn.fieldBase = ObjectUtility.CreateChildGameObject(component.gameObject, "FieldBase").AddComponent<UIBox>();
		}

		filedBase.templateData.Format(breakBtn.fieldBase, filedBase.template);

		return true;
	}
}
#endif