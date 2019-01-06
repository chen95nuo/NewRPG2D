#if UNITY_EDITOR
﻿using UnityEngine;

public class UITemplate_Button : UITemplate_SpriteRoot
{
	public UIButton template;
	public UITemplateData_UIButton templateData;

	public override SpriteRoot Template { get { return template; } }
	public override UITemplateData_SpriteRoot TemplateData { get { return templateData; } }
}

#endif
