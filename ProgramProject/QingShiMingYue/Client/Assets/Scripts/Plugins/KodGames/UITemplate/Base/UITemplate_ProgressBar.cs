#if UNITY_EDITOR
using UnityEngine;

public class UITemplate_ProgressBar : UITemplate_SpriteRoot
{
	public UIProgressBar template;
	public UITemplateData_UIProgressBar templateData;

	public override SpriteRoot Template { get { return template; } }
	public override UITemplateData_SpriteRoot TemplateData { get { return templateData; } }
}

#endif
