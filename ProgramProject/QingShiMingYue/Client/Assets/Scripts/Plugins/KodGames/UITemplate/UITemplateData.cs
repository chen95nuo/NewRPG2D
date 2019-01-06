#if UNITY_EDITOR
using System;
using UnityEngine;

[Serializable]
public class UITemplateData_Align
{
	public enum HORIZONTAL_ALIGN
	{
		NONE,
		OBJECT,
		SPRITE_LEFT,
		SPRITE_RIGHT,
		SPRITE_CENTER,
	}

	public enum VERTICAL_ALIGN
	{
		NONE,
		OBJECT,
		SPRITE_TOP,
		SPRITE_BOTTOM,
		SPRITE_CENTER,
	}

	public bool enable;
	public GameObject parent;
	public HORIZONTAL_ALIGN horizontalAlign = HORIZONTAL_ALIGN.NONE;
	public VERTICAL_ALIGN verticalAlign = VERTICAL_ALIGN.NONE;

	public bool Aligh(GameObject target, GameObject template)
	{
		if (target == null || template == null)
			return false;

		return true;
	}
}

[Serializable]
public class UITemplateData_EZScreenPlacement
{
	public bool enable;
	public bool createWhenNotExist;

	//public bool screenPos;
	//public bool ignoreZ;
	//public bool relativeSize;
	//public bool orignalSpriteSize;
	//public bool relativeTo;
	//public bool relativeObject;
	//public bool alwaysRecursive;
	//public bool allowTransformDrag;

	public bool FormatEZScreenPlacement(GameObject targetGO, GameObject templateGO)
	{
		if (targetGO == null || templateGO == null)
			return false;

		if (enable == false)
			return false;

		EZScreenPlacement template = templateGO.GetComponent<EZScreenPlacement>();
		if (template == null)
			return false;

		EZScreenPlacement target = targetGO.GetComponent<EZScreenPlacement>();
		if (target == null && createWhenNotExist)
			target = targetGO.AddComponent<EZScreenPlacement>();

		if (target == null)
			return false;

		target.screenPos = template.screenPos;
		target.ignoreZ = template.ignoreZ;
		target.relativeSize = template.relativeSize;
		target.orignalSpriteSize = template.orignalSpriteSize;

		if (target.relativeTo == null)
			target.relativeTo = new EZScreenPlacement.RelativeTo(target);
		target.relativeTo.Copy(template.relativeTo);
		target.alwaysRecursive = template.alwaysRecursive;
		target.allowTransformDrag = template.allowTransformDrag;

		return true;
	}
}

[Serializable]
public class UITemplateData_SpriteText
{
	public bool enable;

	public bool gameobjectLayer;
	public bool localPositionX;
	public bool localPositionY;
	public bool localPositionZ;
	public bool text;
	public bool localizingKey;
	public bool offsetZ;
	public bool characterSize;
	public bool characterSpacing;
	public bool lineSpacing;
	public bool anchor;
	public bool alignment;
	public bool tabSize;
	public bool fontName;
	public bool color;
	public bool pixelPerfect;
	public bool maxWidth;
	public bool maxWidthInPixels;
	public bool multiline;
	public bool dynamicLength;
	public bool removeUnsupportedCharacters;
	public bool parseColorTags;
	public bool password;
	public bool maskingCharacter;
	public bool renderCamera;
	public bool hideAtStart;
	public bool persistent;
	public bool ignoreClipping;

	public virtual bool Format(SpriteText target, SpriteText template, float scale)
	{
		if (target == null || template == null)
			return false;

		if (enable == false)
			return false;

		if (gameobjectLayer)
			target.gameObject.layer = template.gameObject.layer;
		{
			// Position
			// Set position but z value
			Transform taretTransform = target.transform;
			Vector3 pos = template.transform.localPosition * scale;
			if (localPositionX == false)
				pos.x = taretTransform.localPosition.x;
			if (localPositionY == false)
				pos.y = taretTransform.localPosition.y;
			if (localPositionZ == false)
				pos.z = taretTransform.localPosition.z;
			target.transform.localPosition = pos;
		}
		if (text)
			target.text = template.text;
		if (localizingKey)
			target.localizingKey = template.localizingKey;
		if (offsetZ)
			target.offsetZ = template.offsetZ;
		if (characterSize)
			target.characterSize = template.characterSize * scale;
		if (characterSpacing)
			target.characterSpacing = template.characterSpacing * scale;
		if (lineSpacing)
			target.lineSpacing = template.lineSpacing * scale;
		if (anchor)
			target.anchor = template.anchor;
		if (alignment)
			target.alignment = template.alignment;
		if (tabSize)
			target.tabSize = template.tabSize;
		if (fontName)
			target.fontName = template.fontName;
		if (color)
			target.color = template.color;
		if (pixelPerfect)
			target.pixelPerfect = template.pixelPerfect;
		if (maxWidth)
			target.maxWidth = template.maxWidth * scale;
		if (maxWidthInPixels)
			target.maxWidthInPixels = template.maxWidthInPixels;
		if (multiline)
			target.multiline = template.multiline;
		if (dynamicLength)
			target.dynamicLength = template.dynamicLength;
		if (removeUnsupportedCharacters)
			target.removeUnsupportedCharacters = template.removeUnsupportedCharacters;
		if (parseColorTags)
			target.parseColorTags = template.parseColorTags;
		if (password)
			target.password = template.password;
		if (maskingCharacter)
			target.maskingCharacter = template.maskingCharacter;
		if (renderCamera)
			target.renderCamera = template.renderCamera;
		if (hideAtStart)
			target.hideAtStart = template.hideAtStart;
		if (persistent)
			target.persistent = template.persistent;
		if (ignoreClipping)
			target.ignoreClipping = template.ignoreClipping;

		return true;
	}
}

[Serializable]
public class UITemplateData_SpriteRoot
{
	public bool enable;

	public bool forceSameType;
	public bool gameObjectLayer;
	public bool localPositionX;
	public bool localPositionY;
	public bool localPositionZ;
	public bool material;
	public bool managed;
	public bool manager;
	public bool drawLayer;
	public bool persistent;
	public bool plane;
	public bool width;
	public bool height;
	public bool textureStyle;
	public bool bleedCompensation;
	public bool anchor;
	public bool pixelPerfect;
	public bool autoResize;
	public bool offset;
	public bool color;
	public bool renderCamera;
	public bool hideAtStart;
	public bool ignoreClipping;
	public UITemplateData_EZScreenPlacement screenPlacement;

	public virtual bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (target == null || template == null)
			return false;

		if (enable == false)
			return false;

		if (forceSameType && target.GetType() != template.GetType())
		{
			SpriteRoot newTarget = target.gameObject.AddComponent(template.GetType()) as SpriteRoot;
			newTarget.Copy(target);
			UnityEngine.Object.DestroyImmediate(target);
			target = newTarget;
		}

		if (gameObjectLayer)
			target.gameObject.layer = template.gameObject.layer;
		{
			// Position
			// Set position but z value
			Transform taretTransform = target.transform;
			Vector3 pos = template.transform.localPosition;
			if (localPositionX == false)
				pos.x = taretTransform.localPosition.x;
			if (localPositionY == false)
				pos.y = taretTransform.localPosition.y;
			if (localPositionZ == false)
				pos.z = taretTransform.localPosition.z;
			target.transform.localPosition = pos;
		}
		if (material || target.renderer.sharedMaterial == null)
			target.renderer.sharedMaterial = template.renderer.sharedMaterial;
		if (managed)
			target.managed = template.managed;
		if (manager)
			target.manager = template.manager;
		if (drawLayer)
			target.drawLayer = template.drawLayer;
		if (persistent)
			target.persistent = template.persistent;
		if (plane)
			target.plane = template.plane;
		if (width)
			target.width = template.width;
		if (height)
			target.height = template.height;
		if (textureStyle)
		{
			target.xTextureTile = template.xTextureTile;
			target.yTextureTile = template.yTextureTile;
			target.xMirror = template.xMirror;
			target.yMirror = template.yMirror;
			target.leftBorder = template.leftBorder;
			target.rightBorder = template.rightBorder;
			target.topBorder = template.topBorder;
			target.bottomBorder = template.bottomBorder;
			target.keepBorderAspectRate = template.keepBorderAspectRate;
		}
		if (bleedCompensation)
			target.bleedCompensation = template.bleedCompensation;
		if (anchor)
			target.anchor = template.anchor;
		if (pixelPerfect)
			target.pixelPerfect = template.pixelPerfect;
		if (autoResize)
			target.autoResize = template.autoResize;
		if (offset)
			target.offset = template.offset;
		if (color)
			target.color = template.color;
		if (renderCamera)
			target.renderCamera = template.renderCamera;
		if (hideAtStart)
			target.hideAtStart = template.hideAtStart;
		if (ignoreClipping)
			target.ignoreClipping = template.ignoreClipping;
		screenPlacement.FormatEZScreenPlacement(target.gameObject, template.gameObject);

		return true;
	}

	public virtual void Copy(UITemplateData_SpriteRoot t)
	{
		this.enable = t.enable;

		this.gameObjectLayer = t.gameObjectLayer;
		this.localPositionX = t.localPositionX;
		this.localPositionY = t.localPositionY;
		this.localPositionZ = t.localPositionZ;
		this.material = t.material;
		this.managed = t.managed;
		this.manager = t.manager;
		this.drawLayer = t.drawLayer;
		this.persistent = t.persistent;
		this.plane = t.plane;
		this.width = t.width;
		this.height = t.height;
		this.textureStyle = t.textureStyle;
		this.bleedCompensation = t.bleedCompensation;
		this.anchor = t.anchor;
		this.pixelPerfect = t.pixelPerfect;
		this.autoResize = t.autoResize;
		this.offset = t.offset;
		this.color = t.color;
		this.renderCamera = t.renderCamera;
		this.hideAtStart = t.hideAtStart;
		this.ignoreClipping = t.ignoreClipping;
	}

	public static TextureAnim[] Clone(TextureAnim[] source, TextureAnim[] target, bool keepTargetSize)
	{
		if (keepTargetSize)
		{
			if (source == null || source.Length == 0 || target == null)
				return target;

			for (int i = 0; i < target.Length; ++i)
			{
				int index = i < source.Length ? i : 0;
				if (source[index] != null)
				{
					target[i] = new TextureAnim();
					target[i].Copy(source[index]);
				}
			}
		}
		else
		{
			target = null;
			if (source == null)
				return target;

			target = new TextureAnim[source.Length];
			for (int i = 0; i < source.Length; ++i)
			{
				if (source[i] != null)
				{
					target[i] = new TextureAnim();
					target[i].Copy(source[i]);
				}
			}
		}

		return target;
	}

	public static UVAnimation[] Clone(UVAnimation[] source, UVAnimation[] target, bool keepTargetSize)
	{
		if (keepTargetSize)
		{
			if (source == null || source.Length == 0 || target == null)
				return target;

			for (int i = 0; i < target.Length; ++i)
			{
				int index = i < source.Length ? i : 0;
				if (source[index] != null)
					target[i] = source[index].Clone();
			}
		}
		else
		{
			target = null;
			if (source == null)
				return target;

			target = new UVAnimation[source.Length];
			for (int i = 0; i < source.Length; ++i)
				if (source[i] != null)
					target[i] = source[i].Clone();
		}

		return target;
	}

	public static string[] Clone(string[] source, string[] target, bool keepTargetSize)
	{
		if (keepTargetSize)
		{
			if (source == null || source.Length == 0 || target == null)
				return target;

			for (int i = 0; i < target.Length; ++i)
			{
				int index = i < source.Length ? i : 0;
				if (source[index] != null)
					target[i] = source[index];
			}
		}
		else
		{
			target = null;
			if (source == null)
				return target;

			target = new string[source.Length];
			for (int i = 0; i < source.Length; ++i)
				target[i] = source[i];
		}

		return target;
	}

	public static EZTransitionList[] Clone(EZTransitionList[] source, EZTransitionList[] target, bool keepTargetSize)
	{
		if (keepTargetSize)
		{
			if (source == null || source.Length == 0 || target == null)
				return target;

			for (int i = 0; i < target.Length; ++i)
			{
				int index = i < source.Length ? i : 0;
				if (source[index] != null)
				{
					target[i] = new EZTransitionList();
					source[index].CopyToNew(target[i]);
				}
			}
		}
		else
		{
			target = null;
			if (source == null)
				return target;

			target = new EZTransitionList[source.Length];
			for (int i = 0; i < source.Length; ++i)
			{
				if (source[i] != null)
				{
					target[i] = new EZTransitionList();
					source[i].CopyToNew(target[i], true);
				}
			}
		}

		return target;
	}
}

[Serializable]
public class UITemplateData_SpriteBase : UITemplateData_SpriteRoot
{
	public bool playAnimOnStart;
	//public bool crossfadeFrames;
	public bool defaultAnim;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is SpriteBase && template is SpriteBase)
		{
			var _target = target as SpriteBase;
			var _template = template as SpriteBase;

			if (playAnimOnStart)
				_target.playAnimOnStart = _template.playAnimOnStart;
			//if (crossfadeFrames)
			//    target.crossfadeFrames = template.crossfadeFrames;
			if (defaultAnim)
				_target.defaultAnim = _template.defaultAnim;
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		var s = t as UITemplateData_SpriteBase;
		if (s != null)
		{
			this.playAnimOnStart = s.playAnimOnStart;
			this.defaultAnim = s.defaultAnim;
		}
	}
}

[Serializable]
public class UITemplateData_AutoSpriteBase : UITemplateData_SpriteBase
{
	public bool doNotTrimImages;
	public bool animations;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is AutoSpriteBase && template is AutoSpriteBase)
		{
			var _target = target as AutoSpriteBase;
			var _template = template as AutoSpriteBase;

			if (doNotTrimImages)
				_target.doNotTrimImages = _template.doNotTrimImages;
			if (animations)
				_target.animations = Clone(_template.animations, _target.animations, target is UIBox);
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		var s = t as UITemplateData_AutoSpriteBase;
		if (s != null)
		{
			this.doNotTrimImages = s.doNotTrimImages;
			this.animations = s.animations;
		}
	}
}

[Serializable]
public class UITemplateData_AutoSprite : UITemplateData_AutoSpriteBase
{
	public bool textureAnimations;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is AutoSprite && template is AutoSprite)
		{
			var _target = target as AutoSprite;
			var _template = template as AutoSprite;

			if (textureAnimations)
				_target.textureAnimations = Clone(_template.textureAnimations, _target.textureAnimations, target is UIBox);
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		if (t is UITemplateData_AutoSprite)
		{
			var s = t as UITemplateData_AutoSprite;
			this.textureAnimations = s.textureAnimations;
		}
	}
}

[Serializable]
public class UITemplateData_AutoSpriteControlBase : UITemplateData_AutoSpriteBase
{
	public bool text;
	public UITemplate_SpriteText textTemplate;
	public bool textOffsetZ;
	public bool includeTextInAutoCollider;
	public bool colliderOffset;

	public bool keepSourceSize;
	public bool states;
	public bool transitions;
	public bool stateLabels;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is AutoSpriteControlBase && template is AutoSpriteControlBase)
		{
			var _target = target as AutoSpriteControlBase;
			var _template = template as AutoSpriteControlBase;

			if (text)
				_target.text = _template.text;
			if (textTemplate != null)
			{
				if (_target.spriteText == null)
					_target.text = "";
				textTemplate.templateData.Format(_target.spriteText, _template.spriteText, 1.0f);
			}
			if (textOffsetZ)
				_target.textOffsetZ = _template.textOffsetZ;
			if (includeTextInAutoCollider)
				_target.includeTextInAutoCollider = _template.includeTextInAutoCollider;
			if (colliderOffset)
			{
				_target.colliderLeftOffset = _template.colliderLeftOffset;
				_target.colliderRightOffset = _template.colliderRightOffset;
				_target.colliderTopOffset = _template.colliderTopOffset;
				_target.colliderBottomOffset = _template.colliderBottomOffset;
			}
			if (states)
				_target.States = Clone(_template.States, _target.States, keepSourceSize ? false : target is UIBox);
			if (transitions)
				_target.Transitions = Clone(_template.Transitions, _target.Transitions, keepSourceSize ? false : target is UIBox);
			if (stateLabels)
				_target.StateLabels = Clone(_template.StateLabels, _target.StateLabels, keepSourceSize ? false : target is UIBox);
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		var s = t as UITemplateData_AutoSpriteControlBase;
		if (s != null)
		{
			this.text = s.text;
			this.textOffsetZ = s.textOffsetZ;
			this.includeTextInAutoCollider = s.includeTextInAutoCollider;
			this.colliderOffset = s.colliderOffset;

			this.states = s.states;
			this.transitions = s.transitions;
			this.stateLabels = s.stateLabels;
		}
	}
}

[Serializable]
public class UITemplateData_UIProgressBar : UITemplateData_AutoSpriteControlBase
{
	//public bool states;
	public UITemplateData_AutoSpriteControlBase[] filledLayers;
	public UITemplateData_AutoSpriteControlBase[] emptyLayers;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is UIProgressBar && template is UIProgressBar)
		{
			var _target = target as UIProgressBar;
			var _template = template as UIProgressBar;

			//if (states)
			//    _target.states = Clone(_template.states);
			for (int i = 0; i < filledLayers.Length && i < _target.filledLayers.Length && i < _template.filledLayers.Length; ++i)
				filledLayers[i].Format(_target.filledLayers[i], _template.filledLayers[i]);
			for (int i = 0; i < emptyLayers.Length && i < _target.emptyLayers.Length && i < _template.emptyLayers.Length; ++i)
				emptyLayers[i].Format(_target.emptyLayers[i], _template.emptyLayers[i]);
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		if (t is UITemplateData_UIProgressBar)
		{
			var s = t as UITemplateData_UIProgressBar;

			//this.states = s.states;

			this.filledLayers = null;
			if (s.filledLayers != null)
			{
				this.filledLayers = new UITemplateData_AutoSpriteControlBase[s.filledLayers.Length];
				for (int i = 0; i < filledLayers.Length && i < s.filledLayers.Length; ++i)
					filledLayers[i].Copy(s.filledLayers[i]);
			}

			this.emptyLayers = null;
			if (s.emptyLayers != null)
			{
				this.emptyLayers = new UITemplateData_AutoSpriteControlBase[s.emptyLayers.Length];
				for (int i = 0; i < emptyLayers.Length && i < s.emptyLayers.Length; ++i)
					emptyLayers[i].Copy(s.emptyLayers[i]);
			}
		}
	}
}

[Serializable]
public class UITemplateData_UIRadioBtn : UITemplateData_AutoSpriteControlBase
{
	public bool useParentForGrouping;
	public bool radioGroup;
	public bool defaultValue;
	//public bool states;
	//public bool stateLabels;
	//public bool transitions;
	public UITemplateData_AutoSpriteControlBase[] layers;
	public bool scriptWithMethodToInvoke;
	public bool methodToInvoke;
	public bool whenToInvoke;
	public bool delay;
	public bool soundToPlay;
	public bool disableHoverEffect;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is UIRadioBtn && template is UIRadioBtn)
		{
			var _target = target as UIRadioBtn;
			var _template = template as UIRadioBtn;

			if (useParentForGrouping)
				_target.useParentForGrouping = _template.useParentForGrouping;
			if (radioGroup)
				_target.radioGroup = _template.radioGroup;
			if (defaultValue)
				_target.defaultValue = _template.defaultValue;
			//if (states)
			//    _target.states = Clone(_template.states);
			//if (stateLabels)
			//    _target.stateLabels = Clone(_template.stateLabels);
			//if (transitions)
			//_target.transitions = Clone(_template.transitions);
			for (int i = 0; i < layers.Length && i < _target.layers.Length && i < _template.layers.Length; ++i)
				layers[i].Format(_target.layers[i], _template.layers[i]);
			if (scriptWithMethodToInvoke)
				_target.scriptWithMethodToInvoke = _template.scriptWithMethodToInvoke;
			if (methodToInvoke)
				_target.methodToInvoke = _template.methodToInvoke;
			if (whenToInvoke)
				_target.whenToInvoke = _template.whenToInvoke;
			if (delay)
				_target.delay = _template.delay;
			if (soundToPlay)
				_target.soundToPlay = _template.soundToPlay;
			if (disableHoverEffect)
				_target.disableHoverEffect = _template.disableHoverEffect;
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		if (t is UITemplateData_UIRadioBtn)
		{
			var s = t as UITemplateData_UIRadioBtn;

			this.useParentForGrouping = s.useParentForGrouping;
			this.radioGroup = s.radioGroup;
			this.defaultValue = s.defaultValue;
			//this.states = s.states;
			//this.transitions = s.transitions;
			//this.stateLabels = s.stateLabels;
			this.layers = null;
			if (s.layers != null)
			{
				this.layers = new UITemplateData_AutoSpriteControlBase[s.layers.Length];
				for (int i = 0; i < layers.Length && i < s.layers.Length; ++i)
					layers[i].Copy(s.layers[i]);
			}
			this.scriptWithMethodToInvoke = s.scriptWithMethodToInvoke;
			this.methodToInvoke = s.methodToInvoke;
			this.whenToInvoke = s.whenToInvoke;
			this.delay = s.delay;
			this.soundToPlay = s.soundToPlay;
			this.disableHoverEffect = s.disableHoverEffect;
		}
	}
}

[Serializable]
public class UITemplateData_UISlider : UITemplateData_AutoSpriteControlBase
{
	public bool scriptWithMethodToInvoke;
	public bool methodToInvoke;
	public bool defaultValue;
	public bool stopKnobFromEdge;
	public bool knobOffset;
	public bool knobSize;
	public bool knobColliderSizeFactor;
	//public bool states;
	//public bool transitions;
	public UITemplateData_AutoSpriteControlBase[] filledLayers;
	public UITemplateData_AutoSpriteControlBase[] emptyLayers;
	public UITemplateData_AutoSpriteControlBase[] knobLayers;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is UISlider && template is UISlider)
		{
			var _target = target as UISlider;
			var _template = template as UISlider;

			if (scriptWithMethodToInvoke)
				_target.scriptWithMethodToInvoke = _template.scriptWithMethodToInvoke;
			if (methodToInvoke)
				_target.methodToInvoke = _template.methodToInvoke;
			if (defaultValue)
				_target.defaultValue = _template.defaultValue;
			if (stopKnobFromEdge)
				_target.stopKnobFromEdge = _template.stopKnobFromEdge;
			if (knobOffset)
				_target.knobOffset = _template.knobOffset;
			if (knobSize)
				_target.knobSize = _template.knobSize;
			if (knobSize)
				_target.knobColliderSizeFactor = _template.knobColliderSizeFactor;
			//if (states)
			//    _target.states = Clone(_template.states);
			//if (transitions)
			//    _target.transitions = Clone(_template.transitions);
			for (int i = 0; i < filledLayers.Length && i < _target.filledLayers.Length && i < _template.filledLayers.Length; ++i)
				filledLayers[i].Format(_target.filledLayers[i], _template.filledLayers[i]);
			for (int i = 0; i < emptyLayers.Length && i < _target.emptyLayers.Length && i < _template.emptyLayers.Length; ++i)
				emptyLayers[i].Format(_target.emptyLayers[i], _template.emptyLayers[i]);
			for (int i = 0; i < knobLayers.Length && i < _target.knobLayers.Length && i < _template.knobLayers.Length; ++i)
				knobLayers[i].Format(_target.knobLayers[i], _template.knobLayers[i]);
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		if (t is UITemplateData_UISlider)
		{
			var s = t as UITemplateData_UISlider;

			this.scriptWithMethodToInvoke = s.scriptWithMethodToInvoke;
			this.methodToInvoke = s.methodToInvoke;
			this.defaultValue = s.defaultValue;
			this.stopKnobFromEdge = s.stopKnobFromEdge;
			this.knobOffset = s.knobOffset;
			this.knobSize = s.knobSize;
			this.knobColliderSizeFactor = s.knobColliderSizeFactor;
			//this.states = s.states;
			//this.transitions = s.transitions;

			this.filledLayers = null;
			if (s.filledLayers != null)
			{
				this.filledLayers = new UITemplateData_AutoSpriteControlBase[s.filledLayers.Length];
				for (int i = 0; i < filledLayers.Length && i < s.filledLayers.Length; ++i)
					filledLayers[i].Copy(s.filledLayers[i]);
			}

			this.emptyLayers = null;
			if (s.emptyLayers != null)
			{
				this.emptyLayers = new UITemplateData_AutoSpriteControlBase[s.emptyLayers.Length];
				for (int i = 0; i < emptyLayers.Length && i < s.emptyLayers.Length; ++i)
					emptyLayers[i].Copy(s.emptyLayers[i]);
			}

			this.knobLayers = null;
			if (s.knobLayers != null)
			{
				this.knobLayers = new UITemplateData_AutoSpriteControlBase[s.knobLayers.Length];
				for (int i = 0; i < knobLayers.Length && i < s.knobLayers.Length; ++i)
					knobLayers[i].Copy(s.knobLayers[i]);
			}
		}
	}
}

[Serializable]
public class UITemplateData_UIStateToggleBtn : UITemplateData_AutoSpriteControlBase
{
	public bool defaultState;
	//public bool states;
	//public bool stateLabels;
	public UITemplateData_AutoSpriteControlBase[] layers;
	public bool scriptWithMethodToInvoke;
	public bool methodToInvoke;
	public bool whenToInvoke;
	public bool delay;
	public bool soundToPlay;
	public bool disableHoverEffect;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is UIStateToggleBtn && template is UIStateToggleBtn)
		{
			var _target = target as UIStateToggleBtn;
			var _template = template as UIStateToggleBtn;

			if (defaultState)
				_target.defaultState = _template.defaultState;
			//if (states)
			//    _target.states = Clone(_template.states);
			//if (stateLabels)
			//    _target.stateLabels = Clone(_template.stateLabels);
			for (int i = 0; i < layers.Length && i < _target.layers.Length && i < _template.layers.Length; ++i)
				layers[i].Format(_target.layers[i], _template.layers[i]);
			if (scriptWithMethodToInvoke)
				_target.scriptWithMethodToInvoke = _template.scriptWithMethodToInvoke;
			if (methodToInvoke)
				_target.methodToInvoke = _template.methodToInvoke;
			if (delay)
				_target.delay = _template.delay;
			if (soundToPlay)
				_target.soundToPlay = _template.soundToPlay;
			if (disableHoverEffect)
				_target.disableHoverEffect = _template.disableHoverEffect;
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		if (t is UITemplateData_UIStateToggleBtn)
		{
			var s = t as UITemplateData_UIStateToggleBtn;

			this.defaultState = s.defaultState;
			//this.states = s.states;
			//this.stateLabels = s.stateLabels;

			this.layers = null;
			if (s.layers != null)
			{
				this.layers = new UITemplateData_AutoSpriteControlBase[s.layers.Length];
				for (int i = 0; i < layers.Length && i < s.layers.Length; ++i)
					layers[i].Copy(s.layers[i]);
			}

			this.scriptWithMethodToInvoke = s.scriptWithMethodToInvoke;
			this.methodToInvoke = s.methodToInvoke;
			this.delay = s.delay;
			this.soundToPlay = s.soundToPlay;
			this.disableHoverEffect = s.disableHoverEffect;
		}
	}
}

[Serializable]
public class UITemplateData_UITextField : UITemplateData_AutoSpriteControlBase
{
	//public bool states;
	//public bool transitions;
	public bool margins;
	public bool placeHolderColorTag;
	public bool placeHolder;
	public bool localizingKey;
	public bool maxLength;
	public bool multiline;
	public bool password;
	public bool maskingCharacter;
	public bool caretSize;
	public bool caretAnchor;
	public bool caretOffset;
	public bool showCaretOnMobile;
	public bool allowClickCaretPlacement;
	public bool keyboardType;
	public bool autoCorrect;
	public bool alert;
	public bool hideInput;
	public bool scriptWithMethodToInvoke;
	public bool methodToInvoke;
	public bool typingSoundEffect;
	public bool fieldFullSound;
	public bool customKeyboard;
	public bool commitOnLostFocus;
	public bool customFocusEvent;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is UITextField && template is UITextField)
		{
			var _target = target as UITextField;
			var _template = template as UITextField;

			//if (states)
			//    _target.states = Clone(_template.states);
			//if (transitions)
			//    _target.transitions = Clone(_template.transitions);
			if (margins)
				_target.margins = _template.margins;
			if (placeHolderColorTag)
				_target.placeHolderColorTag = _template.placeHolderColorTag;
			if (placeHolder)
				_target.placeHolder = _template.placeHolder;
			if (localizingKey)
				_target.localizingKey = _template.localizingKey;
			if (maxLength)
				_target.maxLength = _template.maxLength;
			if (multiline)
				_target.multiline = _template.multiline;
			if (password)
				_target.password = _template.password;
			if (maskingCharacter)
				_target.maskingCharacter = _template.maskingCharacter;
			if (caretSize)
				_target.caretSize = _template.caretSize;
			if (caretAnchor)
				_target.caretAnchor = _template.caretAnchor;
			if (caretOffset)
				_target.caretOffset = _template.caretOffset;
			if (showCaretOnMobile)
				_target.showCaretOnMobile = _template.showCaretOnMobile;
			if (allowClickCaretPlacement)
				_target.allowClickCaretPlacement = _template.allowClickCaretPlacement;
#if UNITY_IPHONE || UNITY_ANDROID
			if (keyboardType)
				_target.type = _template.type;
			if (autoCorrect)
				_target.autoCorrect = _template.autoCorrect;
			if (alert)
				_target.alert = _template.alert;
			if (hideInput)
				_target.hideInput = _template.hideInput;
#endif
			if (scriptWithMethodToInvoke)
				_target.scriptWithMethodToInvoke = _template.scriptWithMethodToInvoke; ;
			if (methodToInvoke)
				_target.methodToInvoke = _template.methodToInvoke; ;
			if (typingSoundEffect)
				_target.typingSoundEffect = _template.typingSoundEffect; ;
			if (fieldFullSound)
				_target.fieldFullSound = _template.fieldFullSound;
			if (customKeyboard)
				_target.customKeyboard = _template.customKeyboard;
			if (commitOnLostFocus)
				_target.commitOnLostFocus = _template.commitOnLostFocus;
			if (customFocusEvent)
				_target.customFocusEvent = _template.customFocusEvent;
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		if (t is UITemplateData_UITextField)
		{
			var s = t as UITemplateData_UITextField;

			//this.states = s.states;
			//this.transitions = s.transitions;
			this.margins = s.margins;
			this.placeHolderColorTag = s.placeHolderColorTag;
			this.placeHolder = s.placeHolder;
			this.localizingKey = s.localizingKey;
			this.maxLength = s.maxLength;
			this.multiline = s.multiline;
			this.password = s.password;
			this.maskingCharacter = s.maskingCharacter;
			this.caretSize = s.caretSize;
			this.caretAnchor = s.caretAnchor;
			this.caretOffset = s.caretOffset;
			this.showCaretOnMobile = s.showCaretOnMobile;
			this.allowClickCaretPlacement = s.allowClickCaretPlacement;
			this.keyboardType = s.keyboardType;
			this.autoCorrect = s.autoCorrect;
			this.alert = s.alert;
			this.hideInput = s.hideInput;
			this.scriptWithMethodToInvoke = s.scriptWithMethodToInvoke;
			this.methodToInvoke = s.methodToInvoke;
			this.typingSoundEffect = s.typingSoundEffect;
			this.fieldFullSound = s.fieldFullSound;
			this.customKeyboard = s.customKeyboard;
			this.commitOnLostFocus = s.commitOnLostFocus;
			this.customFocusEvent = s.customFocusEvent;
		}
	}
}

[Serializable]
public class UITemplateData_UIButton : UITemplateData_AutoSpriteControlBase
{
	//public bool states;
	//public bool transitions;
	//public bool stateLabels;
	public UITemplateData_AutoSpriteControlBase[] layers;
	public bool scriptWithMethodToInvoke;
	public bool methodToInvoke;
	public bool whenToInvoke;
	public bool delay;
	public bool soundOnOver;
	public bool soundOnClick;
	public bool repeat;
	public bool alwaysFinishActiveTransition;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is UIButton && template is UIButton)
		{
			var _target = target as UIButton;
			var _template = template as UIButton;

			//if (states)
			//    _target.states = Clone(_template.states);
			//if (transitions)
			//    _target.transitions = Clone(_template.transitions);
			//if (stateLabels)
			//    _target.stateLabels = Clone(_template.stateLabels);
			for (int i = 0; i < layers.Length && i < _target.layers.Length && i < _template.layers.Length; ++i)
				layers[i].Format(_target.layers[i], _template.layers[i]);
			if (scriptWithMethodToInvoke)
				_target.scriptWithMethodToInvoke = _template.scriptWithMethodToInvoke;
			if (methodToInvoke)
				_target.methodToInvoke = _template.methodToInvoke;
			if (whenToInvoke)
				_target.whenToInvoke = _template.whenToInvoke;
			if (delay)
				_target.delay = _template.delay;
			if (soundOnOver)
				_target.soundOnOver = _template.soundOnOver;
			if (soundOnClick)
				_target.soundOnClick = _template.soundOnClick;
			if (repeat)
				_target.repeat = _template.repeat;
			if (alwaysFinishActiveTransition)
				_target.alwaysFinishActiveTransition = _template.alwaysFinishActiveTransition;
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		if (t is UITemplateData_UIButton)
		{
			var s = t as UITemplateData_UIButton;

			//this.states = s.states;
			//this.transitions = s.transitions;
			//this.stateLabels = s.stateLabels;
			if (s.layers != null)
			{
				this.layers = new UITemplateData_AutoSpriteControlBase[s.layers.Length];
				for (int i = 0; i < layers.Length && i < s.layers.Length; ++i)
					layers[i].Copy(s.layers[i]);
			}
			this.scriptWithMethodToInvoke = s.scriptWithMethodToInvoke;
			this.methodToInvoke = s.methodToInvoke;
			this.whenToInvoke = s.whenToInvoke;
			this.delay = s.delay;
			this.soundOnOver = s.soundOnOver;
			this.soundOnClick = s.soundOnClick;
			this.repeat = s.repeat;
			this.alwaysFinishActiveTransition = s.alwaysFinishActiveTransition;
		}
	}
}

[Serializable]
public class UITemplateData_UIListItem : UITemplateData_UIButton
{
	public bool activeOnlyWhenSelected;
	public bool gameObjectPool;

	public override bool Format(SpriteRoot target, SpriteRoot template)
	{
		if (base.Format(target, template) == false)
			return false;

		if (target is UIListItem && template is UIListItem)
		{
			var _target = target as UIListItem;
			var _template = template as UIListItem;

			if (activeOnlyWhenSelected)
				_target.activeOnlyWhenSelected = _template.activeOnlyWhenSelected;
			if (gameObjectPool)
				_target.gameObjectPool = _template.gameObjectPool;
		}

		return true;
	}

	public override void Copy(UITemplateData_SpriteRoot t)
	{
		base.Copy(t);

		if (t is UITemplateData_UIListItem)
		{
			var s = t as UITemplateData_UIListItem;

			this.activeOnlyWhenSelected = s.activeOnlyWhenSelected;
			this.gameObjectPool = s.gameObjectPool;
		}
	}
}
#endif
