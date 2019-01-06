#if UNITY_EDITOR
using UnityEngine;

public abstract class UITemplate_SpriteRoot : UITemplate
{
	public abstract SpriteRoot Template { get; }
	public abstract UITemplateData_SpriteRoot TemplateData { get; }

	public override System.Type GetTargetType()
	{
		return typeof(SpriteRoot);
	}

	public override bool Apply(Component component)
	{
		SpriteRoot s = component as SpriteRoot;
		if (s == null)
			return false;

		if (s.templateName == null || this.TemplateData == null)
			return false;

		if (s.templateName.Equals(this.gameObject.name, System.StringComparison.OrdinalIgnoreCase) == false)
			return false;

		TemplateData.Format(s, Template);
		return true;
	}

	public virtual void Copy(UITemplate_SpriteRoot t)
	{
		this.TemplateData.Copy(t.TemplateData);
	}

	[ContextMenu("Copy From other template")]
	public void CopyFromOtherTemplate()
	{
		foreach (UITemplate_SpriteRoot control in this.GetComponents(typeof(UITemplate_SpriteRoot)))
			if (control != this)
			{
				this.Copy(control);
				break;
			}
	}
}
#endif
