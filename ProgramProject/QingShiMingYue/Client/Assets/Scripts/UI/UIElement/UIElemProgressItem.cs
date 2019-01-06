using System;
using UnityEngine;
using System.Collections.Generic;

public class UIElemProgressItem : MonoBehaviour
{
	public AutoSpriteControlBase emptyBase; // Can Not Be Null.
	public AutoSpriteControlBase fieldBase;
	public SpriteText progressLabel;

	public bool horizontal = true;
	public bool fieldCurrent = false;

	private Vector2 emptyWH = Vector2.zero;
	private Vector3 fieldPos = Vector3.zero;

	private int maxValue;
	public int MaxValue { get { return maxValue; } }

	public int Value
	{
		set
		{
			if (emptyBase == null)
			{
				Debug.LogError("Missing EmtyBase in " + this.gameObject.name);
				return;
			}

			// Set Process Label.
			SetProgressLabel(value);

			// Set Icon.
			var icons = GetIconBorders();
			if (icons != null)
			{
				if (emptyBase != null)
					UIUtility.CopyIcon(emptyBase, value <= MaxValue ? icons[0] : icons[1]);

				if (fieldBase != null)
					UIUtility.CopyIcon(fieldBase, value <= MaxValue ? icons[1] : icons[2]);
			}

			if (fieldBase != null)
				fieldBase.gameObject.SetActive(value > 0);

			if (value < 0 || maxValue <= 0)
				return;

			// Reset Value.
			value = value <= maxValue ? value : value - maxValue;

			if (fieldBase != null)
			{
				if (fieldCurrent)
					fieldBase.SetSize(emptyWH.x, emptyWH.y);
				else if (horizontal)
					fieldBase.SetSize(emptyWH.x * value, emptyWH.y);
				else
					fieldBase.SetSize(emptyWH.x, emptyWH.y * value);

				int halfMax = maxValue / 2;
				int halfMaxRe = maxValue % 2;

				Vector3 posOffset = Vector3.zero;
				if (horizontal)
				{
					switch (emptyBase.anchor)
					{
						case SpriteRoot.ANCHOR_METHOD.UPPER_LEFT:
						case SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT:
						case SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT:

							if (fieldCurrent)
								posOffset.x = (value - 1) * emptyWH.x;

							break;

						case SpriteRoot.ANCHOR_METHOD.UPPER_CENTER:
						case SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER:
						case SpriteRoot.ANCHOR_METHOD.BOTTOM_CENTER:
						case SpriteRoot.ANCHOR_METHOD.TEXTURE_OFFSET:

							if (fieldCurrent)
								posOffset.x = (value - (halfMax + 1)) * emptyWH.x + emptyBase.Center.x + (halfMaxRe == 0 ? emptyWH.x / 2 : 0);
							else
								posOffset.x = Math.Abs(fieldBase.width / 2) - Math.Abs(emptyBase.width / 2);
							break;

						case SpriteRoot.ANCHOR_METHOD.UPPER_RIGHT:
						case SpriteRoot.ANCHOR_METHOD.MIDDLE_RIGHT:
						case SpriteRoot.ANCHOR_METHOD.BOTTOM_RIGHT:

							posOffset.x = (value - maxValue) * emptyWH.x;
							break;
					}
				}
				else
				{
					switch (emptyBase.anchor)
					{
						case SpriteRoot.ANCHOR_METHOD.UPPER_LEFT:
						case SpriteRoot.ANCHOR_METHOD.UPPER_CENTER:
						case SpriteRoot.ANCHOR_METHOD.UPPER_RIGHT:

							if (fieldCurrent)
								posOffset.y = (1 - value) * emptyWH.y;

							break;

						case SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT:
						case SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER:
						case SpriteRoot.ANCHOR_METHOD.MIDDLE_RIGHT:
						case SpriteRoot.ANCHOR_METHOD.TEXTURE_OFFSET:

							if (fieldCurrent)
								posOffset.y = (value - (halfMax + 1)) * emptyWH.y + emptyBase.Center.y + (halfMaxRe == 0 ? emptyWH.y / 2 : 0);
							else
								posOffset.y = Math.Abs(fieldBase.height / 2) - Math.Abs(emptyBase.height / 2);
							break;

						case SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT:
						case SpriteRoot.ANCHOR_METHOD.BOTTOM_CENTER:
						case SpriteRoot.ANCHOR_METHOD.BOTTOM_RIGHT:
							posOffset.y = (maxValue - value) * emptyWH.y;
							break;
					}
				}

				fieldBase.CachedTransform.localPosition = fieldPos + posOffset;
			}
			else
			{
				if (horizontal)
					emptyBase.SetSize(emptyWH.x * value, emptyWH.y);
				else
					emptyBase.SetSize(emptyWH.x, emptyWH.y * value);
			}
		}
	}

	public virtual void Awake()
	{
		if (emptyBase != null)
			emptyWH = new Vector2(emptyBase.width, emptyBase.height);

		if (fieldBase != null)
			fieldPos = new Vector3(fieldBase.CachedTransform.localPosition.x, fieldBase.CachedTransform.localPosition.y, fieldBase.CachedTransform.localPosition.z);
	}

	public virtual AutoSpriteControlBase[] GetIconBorders()
	{
		return null;
	}

	public void SetMax(int maxValue)
	{
		this.maxValue = maxValue;

		Hide(maxValue <= 0);

		if (emptyBase != null)
		{
			emptyBase.Start();

			emptyBase.keepBorderAspectRate = true;
			emptyBase.xTextureTile = horizontal;
			emptyBase.yTextureTile = !horizontal;
		}

		if (fieldBase != null)
		{
			fieldBase.Start();

			if (!fieldCurrent)
			{
				fieldBase.xTextureTile = horizontal;
				fieldBase.yTextureTile = !horizontal;
			}

			if (horizontal)
				emptyBase.SetSize(emptyWH.x * maxValue, emptyWH.y);
			else
				emptyBase.SetSize(emptyWH.x, emptyWH.y * maxValue);
		}

		//Value = 0;
	}

	private void SetProgressLabel(int value)
	{
		if (progressLabel == null)
			return;

		if (value <= 0)
			progressLabel.Text = string.Empty;
		else
			progressLabel.Text = string.Format("{0}/{1}", value, maxValue);
	}

	public void Hide(bool hf)
	{
		this.gameObject.SetActive(!hf);
	}
}