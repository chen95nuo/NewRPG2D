#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

public class UITemplate_Icon : UITemplate
{
	public UITemplate_Button borderTemplate;

	public UITemplate_AutoSpriteControlBase breakBorderTemplate;
	public UITemplate_AutoSpriteControlBase breakFieldBorderTemplate;
	public UITemplate_SpriteText leftLableTemplate;
	public UITemplate_SpriteText rightLableTemplate;
	public UITemplate_SpriteText emptyLabelTemplate;
	public bool useSamllIcon = false;

	public override System.Type GetTargetType()
	{
		return borderTemplate.GetTargetType();
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

		borderTemplate.templateData.Format(btnCtrl, borderTemplate.template);

		// Set Asset Icon
		UIElemAssetIcon icon = component.GetComponent<UIElemAssetIcon>();
		if (icon == null)
			icon = component.gameObject.AddComponent<UIElemAssetIcon>();

		// Set borderTemplate
		icon.border = btnCtrl;
		icon.useSamllIcon = useSamllIcon;

		// 删除之前的没用的控件
		var oldLeftBorder = ObjectUtility.FindChildObject(icon.gameObject, "LeftBorder");
		if (oldLeftBorder != null)
			Object.DestroyImmediate(oldLeftBorder.gameObject);
		var oldRightBorder = ObjectUtility.FindChildObject(icon.gameObject, "RightBorder");
		if (oldRightBorder != null)
			Object.DestroyImmediate(oldRightBorder.gameObject);
		var emptyBaseBorder = ObjectUtility.FindChildObject(icon.gameObject, "emptyBase");
		if (emptyBaseBorder != null)
			Object.DestroyImmediate(emptyBaseBorder.gameObject);
		var fieldBgBorder = ObjectUtility.FindChildObject(icon.gameObject, "fieldBg");
		if (fieldBgBorder != null)
			Object.DestroyImmediate(fieldBgBorder.gameObject);

		var oldBreak = ObjectUtility.FindComponentInChildren<UIElemProgressItem>(icon.gameObject, "BreakBorder", 1);
		if(oldBreak != null)
			Object.DestroyImmediate(oldBreak.gameObject);

		if (breakBorderTemplate != null)
		{
			if (icon.breakBorder == null)
			{
				icon.breakBorder = ObjectUtility.FindComponentInChildren<UIElemAssetIconBreakThroughBtn>(icon.gameObject, "BreakBorder", 1);

				if (icon.breakBorder == null)
				{
					icon.breakBorder = ObjectUtility.CreateChildGameObject(icon.border.gameObject, "BreakBorder").AddComponent<UIElemAssetIconBreakThroughBtn>();
				}				
			}

			if (icon.breakBorder.emptyBase != null && icon.breakBorder.emptyBase.GetType() != breakBorderTemplate.template.GetType())
			{
				Object.DestroyImmediate(icon.breakBorder.gameObject);
				icon.breakBorder = null;
			}

			if (icon.breakBorder.emptyBase == null)
			{
				icon.breakBorder.emptyBase = icon.breakBorder.GetComponent<UIBox>();

				if (icon.breakBorder.emptyBase == null)
				{
					icon.breakBorder.emptyBase = icon.breakBorder.gameObject.AddComponent<UIBox>();
				}
			}

			if (icon.breakBorder.fieldBase == null)
			{
				icon.breakBorder.fieldBase = ObjectUtility.FindComponentInChildren<UIBox>(icon.breakBorder.gameObject, "FiledBorder", 1);

				if (icon.breakBorder.fieldBase == null)
				{
					icon.breakBorder.fieldBase = ObjectUtility.CreateChildGameObject(icon.breakBorder.gameObject,"FiledBorder").AddComponent<UIBox>();
				}
			}

			ApplyChildBorderTemplate(borderTemplate, breakBorderTemplate, null, icon.border, icon.breakBorder.emptyBase);
			ApplyChildBorderTemplate(borderTemplate, breakFieldBorderTemplate, null, icon.border, icon.breakBorder.fieldBase);
		}
		else if (icon.breakBorder != null)
		{
			Object.DestroyImmediate(icon.breakBorder.gameObject);
			icon.breakBorder = null;
		}

		// Set soul border template
		if (leftLableTemplate != null)
		{
			if (icon.leftLable == null)
			{
				// Get existing left borderTemplate
				icon.leftLable = ObjectUtility.FindComponentInChildren<SpriteText>(icon.gameObject, "LeftLable", 1);

				if (icon.leftLable == null)
				{
					// Create left borderTemplate
					icon.leftLable = ObjectUtility.CreateChildGameObject(icon.border.gameObject, "LeftLable").AddComponent<SpriteText>();
					icon.leftLable.Text = "";
				}
			}

			ApplyChildTextTemplate(borderTemplate, icon.border, leftLableTemplate, icon.leftLable);
		}
		else if (icon.leftLable != null)
		{
			Object.DestroyImmediate(icon.leftLable.gameObject);
			icon.leftLable = null;
		}

		// Set right border template
		if (rightLableTemplate != null)
		{
			if (icon.rightLable == null)
			{
				// Get existing left borderTemplate
				icon.rightLable = ObjectUtility.FindComponentInChildren<SpriteText>(icon.gameObject, "RightLable", 1);

				if (icon.rightLable == null)
				{
					// Create left borderTemplate
					icon.rightLable = ObjectUtility.CreateChildGameObject(icon.border.gameObject, "RightLable").AddComponent<SpriteText>();
					icon.rightLable.Text = "";
				}
			}

			ApplyChildTextTemplate(borderTemplate, icon.border, rightLableTemplate, icon.rightLable);
		}
		else if (icon.rightLable != null)
		{
			Object.DestroyImmediate(icon.rightLable.gameObject);
			icon.rightLable = null;
		}

		// Set emptyLabel Template
		if (emptyLabelTemplate != null)
		{
			if (icon.emptyLabel == null)
			{
				// Create SpriteText component:
				icon.emptyLabel = ObjectUtility.FindComponentInChildren<SpriteText>(icon.gameObject, "emptyLabel", 1);

				if (icon.emptyLabel == null)
				{
					icon.emptyLabel = ObjectUtility.CreateChildGameObject(icon.border.gameObject, "emptyLabel").AddComponent<SpriteText>();
					icon.emptyLabel.Text = "";
				}
			}

			ApplyChildTextTemplate(borderTemplate, icon.border, emptyLabelTemplate, icon.emptyLabel);
		}
		else if (icon.emptyLabel != null)
		{
			Object.DestroyImmediate(icon.emptyLabel.gameObject);
			icon.emptyLabel = null;
		}

		// Set border layers
		SetBorderLayers(icon);

		return true;
	}

	private Vector2 GetOffset(SpriteRoot sprite)
	{
		Vector2 offset = new Vector2(0f, 0f);

		switch (sprite.anchor)
		{
			case SpriteRoot.ANCHOR_METHOD.UPPER_LEFT:
				offset.Set(sprite.width / 2, -sprite.height / 2);
				break;
			case SpriteRoot.ANCHOR_METHOD.UPPER_CENTER:
				offset.Set(0f, -sprite.height / 2);
				break;
			case SpriteRoot.ANCHOR_METHOD.UPPER_RIGHT:
				offset.Set(-sprite.width / 2, -sprite.height / 2);
				break;
			case SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT:
				offset.Set(sprite.width / 2, 0f);
				break;
			case SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER:
				break;
			case SpriteRoot.ANCHOR_METHOD.MIDDLE_RIGHT:
				offset.Set(-sprite.width / 2, 0f);
				break;
			case SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT:
				offset.Set(sprite.width / 2, sprite.height / 2);
				break;
			case SpriteRoot.ANCHOR_METHOD.BOTTOM_CENTER:
				offset.Set(0f, sprite.height / 2);
				break;
			case SpriteRoot.ANCHOR_METHOD.BOTTOM_RIGHT:
				offset.Set(-sprite.width / 2, sprite.height / 2);
				break;
		}

		return offset;
	}

	private void ApplyQualityBorderTemplate(UITemplate_AutoSpriteControlBase borderTemplate, UITemplate_AutoSpriteControlBase childBorderTemplate, AutoSpriteControlBase border, AutoSpriteControlBase childBorder)
	{
		if (childBorder == null)
			return;

		childBorder.templateName = string.Empty;

		// Get scale rate

		float xScale = border.width / borderTemplate.template.width;
		float yScale = border.height / borderTemplate.template.height;

		// Apply border template
		childBorderTemplate.templateData.Format(childBorder, childBorderTemplate.template);

		// Scale size
		childBorder.width = childBorderTemplate.template.width * xScale;
		childBorder.height = childBorderTemplate.template.height * yScale;

		// Set border position
		SetPosition(childBorderTemplate.template, childBorder, xScale, yScale);
	}

	private void ApplyChildBorderTemplate(UITemplate_Button borderTemplate, UITemplate_AutoSpriteControlBase childBorderTemplate, UITemplate_SpriteText childBorderTextTemplate, AutoSpriteControlBase border, AutoSpriteControlBase childBorder)
	{
		if (childBorder == null)
			return;

		childBorder.templateName = "";

		// Get scale rate
		float xScale = border.width / borderTemplate.template.width;
		float yScale = border.height / borderTemplate.template.height;

		// Apply border template
		childBorderTemplate.templateData.Format(childBorder, childBorderTemplate.template);

		// Scale size
		childBorder.width = childBorderTemplate.template.width * xScale;
		childBorder.height = childBorderTemplate.template.height * yScale;

		// Set border position
		SetPosition(childBorderTemplate.template, childBorder, xScale, yScale, GetOffset(border));

		// Apply border text template
		if (childBorderTextTemplate != null)
		{
			childBorder.spriteText.templateName = "";

			childBorderTextTemplate.templateData.Format(childBorder.spriteText, childBorderTextTemplate.template, Mathf.Min(xScale, yScale));

			// Scale size
			SetPosition(childBorderTextTemplate.template, childBorder.spriteText, xScale, yScale, GetOffset(childBorderTemplate.template) - GetOffset(childBorder));
		}
	}

	private void ApplyRootQualityTemplate(UITemplate_Button borderTemplate, UITemplate_AutoSpriteControlBase childBorderTemplate, AutoSpriteControlBase border, AutoSpriteControlBase childBorder)
	{
		if (childBorder == null)
			return;

		// Get scale rate
		float xScale = border.width / borderTemplate.template.width;
		float yScale = border.height / borderTemplate.template.height;

		// Apply border template
		childBorderTemplate.templateData.Format(childBorder, childBorderTemplate.template);
		// Scale size
		childBorder.width = childBorderTemplate.template.width * xScale;
		childBorder.height = childBorderTemplate.template.height * yScale;

		// Set border position
		SetPosition(childBorderTemplate.template, childBorder, xScale, yScale, GetOffset(border));
	}

	private void ApplyChildQualityTemplate(UITemplate_AutoSpriteControlBase borderTemplate, UITemplate_AutoSpriteControlBase childBorderTemplate, AutoSpriteControlBase border, AutoSpriteControlBase childBorder)
	{
		if (childBorder == null)
			return;

		// Get scale rate
		float xScale = border.width / borderTemplate.template.width;
		float yScale = border.height / borderTemplate.template.height;

		// Apply border template
		childBorderTemplate.templateData.Format(childBorder, childBorderTemplate.template);
		// Scale size
		childBorder.width = childBorderTemplate.template.width * xScale;
		childBorder.height = childBorderTemplate.template.height * yScale;

		// Set border position
		SetPosition(childBorderTemplate.template, childBorder, xScale, yScale, GetOffset(border));
	}

	private void ApplyChildTextTemplate(UITemplate_Button borderTemplate, AutoSpriteControlBase border, UITemplate_SpriteText childBorderTextTemplate, SpriteText spriteText)
	{
		if (spriteText == null)
			return;

		spriteText.templateName = "";

		// Get scale rate
		float xScale = border.width / borderTemplate.template.width;
		float yScale = border.height / borderTemplate.template.height;

		// Apply border text template
		if (childBorderTextTemplate != null)
		{
			childBorderTextTemplate.templateData.Format(spriteText, childBorderTextTemplate.template, Mathf.Min(xScale, yScale));

			// Scale size
			SetPosition(childBorderTextTemplate.template, spriteText, xScale, yScale, GetOffset(border));
		}
	}

	private void SetPosition(Component template, Component go, float xScale, float yScale, Vector2 offset)
	{
		Vector2 position = new Vector2(template.transform.localPosition.x * xScale, template.transform.localPosition.y * yScale);
		position += offset;
		go.transform.localPosition = new Vector3(position.x, position.y, -0.001f);
	}

	private void SetPosition(Component template, Component go, float xScale, float yScale)
	{
		Vector2 position = new Vector2(template.transform.localPosition.x * xScale, template.transform.localPosition.y * yScale);
		go.transform.localPosition = new Vector3(position.x, position.y, -0.001f);
	}

	private void SetBorderLayers(UIElemAssetIcon icon)
	{
		UIButton borderIcon = icon.border as UIButton;
		if (borderIcon == null)
			return;

		List<SpriteRoot> layers = new List<SpriteRoot>();
		List<SpriteText> textLayers = new List<SpriteText>();

		// Set child border layer
		if (icon.breakBorder != null)
		{
			if (icon.breakBorder.emptyBase != null)
				layers.Add(icon.breakBorder.emptyBase);
			if (icon.breakBorder.fieldBase != null)
				layers.Add(icon.breakBorder.fieldBase);
			if (icon.breakBorder.progressLabel != null)
				textLayers.Add(icon.breakBorder.progressLabel);
		}

		if (icon.leftLable != null)
			textLayers.Add(icon.leftLable);

		if (icon.rightLable != null)
			textLayers.Add(icon.rightLable);

		if (icon.emptyLabel != null)
			textLayers.Add(icon.emptyLabel);

		borderIcon.layers = layers.ToArray();
		borderIcon.textLayers = textLayers.ToArray();
	}
}
#endif