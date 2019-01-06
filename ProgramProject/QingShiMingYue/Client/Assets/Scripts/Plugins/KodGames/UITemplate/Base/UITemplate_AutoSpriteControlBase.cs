#if UNITY_EDITOR
using UnityEngine;

public class UITemplate_AutoSpriteControlBase : UITemplate_SpriteRoot
{
	public AutoSpriteControlBase template;
	public UITemplateData_AutoSpriteControlBase templateData;

	public override SpriteRoot Template { get { return template; } }
	public override UITemplateData_SpriteRoot TemplateData { get { return templateData; } }
}

#endif
