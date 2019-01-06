#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class UITemplate_UIToggleButton : UITemplate_SpriteRoot 
{
	public UIStateToggleBtn template;
	public UITemplateData_UIStateToggleBtn templateData;

	public override SpriteRoot Template { get { return template; } }
	public override UITemplateData_SpriteRoot TemplateData { get { return templateData; } }
}
#endif

