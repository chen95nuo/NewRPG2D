#if UNITY_EDITOR
using UnityEngine;

public class UITemplate_TextField : UITemplate_SpriteRoot
{
	public UITextField template;
	public UITemplateData_UITextField templateData;

	public override SpriteRoot Template { get { return template; } }
	public override UITemplateData_SpriteRoot TemplateData { get { return templateData; } }
}
#endif
