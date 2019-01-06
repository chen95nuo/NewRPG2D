using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDlgMessage : UIModule
{
	//Title bar buttons.
	public SpriteText titleLabel;
	public UIButton closeBtn;

	//Message to show.
	public SpriteText msgContentLabel;

	//Operation buttons.
	public UIButton okBtn;
	public UIButton cancelBtn;
	public UIButton thirdBtn;

	public UIChildLayoutControl btnContainer;

	//Bg border.
	public UIBox messageBoder;
	public UIBox border;

	public Vector2 paddingBorder;
	public float maxBorderWidth;
	public float msgBtnMargin;

	//Local data.
	private ShowData showData;
	private const float VerticalBorderWidth = 266f;
	private const float VerticalBorderHeight = 160f;
	private const float VerticalButtonWidth = 160f;
	private const float VerticalButtonHeight = 32f;
	private Vector2 origionalBorderSize = Vector2.zero;
	private Vector2 origionalMsgBorderSize = Vector2.zero;
	private Vector2 origionalBtnSize = Vector2.zero;
	private Vector3 origionalBtnContainerPos = Vector3.zero;
	private Vector3 origionalTitlePos;
	private Vector3 origionalClosePos;

	private float characterSize = 17.0f;
	private float lineSpacing = 0.9f;

	private bool resetColor = false;

#if UNITY_ANDROID
	public bool IgnoreEscape { get { return showData.IgnoreEscape; } }
#endif

	public class ShowData
	{
		public ShowData()
		{
			verticalLayout = false;
		}

		public string title;
		public string message;

		private MainMenuItem okBehaviour;
		public MainMenuItem OKCallback
		{
			get { return this.okBehaviour; }
		}
		public bool UseOkBtn
		{
			get { return okBehaviour != null; }
		}

		private MainMenuItem cancelBehaviour;
		public MainMenuItem CancelCallback
		{
			get { return this.cancelBehaviour; }
		}
		public bool UseCancelBtn
		{
			get { return cancelBehaviour != null; }
		}


		private MainMenuItem thirdBehaviour;
		public MainMenuItem ThirdCallback
		{
			get { return this.thirdBehaviour; }
		}

		public bool UseThirdBtn
		{
			get { return thirdBehaviour != null; }
		}

		private bool useCloseBtn = true;
		public bool UseCloseBtn
		{
			get { return useCloseBtn; }
		}

		private bool verticalLayout;
		public bool VerticalLayout
		{
			get { return this.verticalLayout; }
		}

#if UNITY_ANDROID
		private bool ignoreEscape = false;
		public bool IgnoreEscape { get { return ignoreEscape; } }

		public void SetData(bool ignoreEscape, string title, string message, params MainMenuItem[] menuCallbacks)
		{
			SetData(title, message, menuCallbacks);
			this.ignoreEscape = ignoreEscape;
		}

		public void SetData(bool ignoreEscape, string title, string message, bool useCloseBtn, params MainMenuItem[] menuCallbacks)
		{
			SetData(title, message, useCloseBtn, menuCallbacks);
			this.ignoreEscape = ignoreEscape;
		}
#endif

		public void SetData(string title, string message, params MainMenuItem[] menuCallbacks)
		{
			this.title = title;
			this.message = message;

			if (menuCallbacks == null)
				return;

			this.okBehaviour = menuCallbacks.Length > 0 ? menuCallbacks[0] : null;
			this.cancelBehaviour = menuCallbacks.Length > 1 ? menuCallbacks[1] : null;
			this.thirdBehaviour = menuCallbacks.Length > 2 ? menuCallbacks[2] : null;
		}

		public void SetData(string title, string message, bool useCloseBtn, params MainMenuItem[] menuCallbacks)
		{
			this.useCloseBtn = useCloseBtn;

			if (menuCallbacks == null)
				return;

			if (menuCallbacks.Length > 1)
			{
				SetData(title, message, menuCallbacks);
			}
			else
			{
				SetData(title, message);
			}

		}

		public void SetData(string title, bool useCloseBtn, bool verticalLayout, params MainMenuItem[] menuCallbacks)
		{
			this.verticalLayout = verticalLayout;
			SetData(title, "", useCloseBtn, menuCallbacks);
		}
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		origionalBorderSize.Set(border.width, border.height);
		origionalMsgBorderSize.Set(messageBoder.width, messageBoder.height);
		origionalBtnSize.Set(okBtn.width, okBtn.height);
		origionalBtnContainerPos = btnContainer.transform.localPosition;
		origionalTitlePos = titleLabel.transform.localPosition;
		origionalClosePos = closeBtn.transform.localPosition;
		return true;
	}

	public bool ShowDlg(ShowData showData)
	{
		return ShowSelf(showData);
	}

	public bool ShowDlg(ShowData showData, bool resetColor)
	{
		this.resetColor = resetColor;
		return ShowSelf(showData);
	}

	public bool ShowDlg(string title, string message)
	{
		MainMenuItem okMenu = new MainMenuItem();
		okMenu.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Title");
		ShowData tempShowData = new ShowData();
		tempShowData.SetData(title, message, okMenu);

		return ShowDlg(tempShowData);
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		this.showData = userDatas[0] as ShowData;

		if (resetColor)
			msgContentLabel.SetColor(Color.white);
		else
			msgContentLabel.SetColor(GameDefines.textColorBtnYellow);
		
		resetColor = false;
		titleLabel.Text = showData.title;

		msgContentLabel.SetAlignment(userDatas.Length >= 2 ? (SpriteText.Alignment_Type)userDatas[1] : SpriteText.Alignment_Type.Left);
		msgContentLabel.Text = showData.message;
		msgContentLabel.SetCharacterSize(userDatas.Length >= 3 ? (float)userDatas[2] : characterSize);
		msgContentLabel.SetLineSpacing(userDatas.Length >= 4 ? (float)userDatas[3] : lineSpacing);

		UpdateOperateBtnLayout();
		UpdateBtnStatus();
		LayoutControls();

		return true;
	}

	private void LayoutControls()
	{
		Vector2 borderSize = new Vector2(origionalBorderSize.x, origionalBorderSize.y);
		float btnHeight = okBtn.height;
		if (showData.VerticalLayout)
		{
			borderSize.x = VerticalBorderWidth;
			borderSize.y = VerticalBorderHeight;

			btnContainer.transform.localPosition = new Vector3(btnContainer.transform.localPosition.x, 0, btnContainer.transform.localPosition.z);
		}
		else
		{
			btnContainer.transform.localPosition = origionalBtnContainerPos;

			if (showData.message == null || showData.message.Equals(""))
			{
				border.SetSize(borderSize.x, borderSize.y);
			}
			else
			{
				float maxLabelWidth = maxBorderWidth - paddingBorder.x * 2;
				float minLabelWidth = origionalBorderSize.x - paddingBorder.x * 2;
				float minLabelHeight = origionalBorderSize.y - 2 * paddingBorder.y - btnHeight - msgBtnMargin;
				float messageWidth = msgContentLabel.GetWidth(showData.message);
				float lineHeight = msgContentLabel.GetLineHeight();

				if (messageWidth <= minLabelWidth)
				{
					msgContentLabel.maxWidth = minLabelWidth;
				}
				else if (messageWidth > minLabelWidth)
				{
					float actualHeight = Mathf.Ceil(messageWidth / minLabelWidth) * lineHeight;
					if (actualHeight < minLabelHeight)
					{
						msgContentLabel.maxWidth = minLabelWidth;
					}
					else
					{
						float labelMaxWidth = messageWidth / Mathf.Floor(minLabelHeight / lineHeight);
						if (labelMaxWidth <= maxLabelWidth)
						{
							msgContentLabel.maxWidth = labelMaxWidth;
						}
						else
						{
							msgContentLabel.maxWidth = maxLabelWidth;
						}
					}
				}

				msgContentLabel.Text = showData.message;
				borderSize.x = Mathf.Max(origionalBorderSize.x, msgContentLabel.BottomRight.x - msgContentLabel.TopLeft.x + 2 * paddingBorder.x);
				borderSize.y = Mathf.Max(origionalBorderSize.y, msgContentLabel.TopLeft.y - msgContentLabel.BottomRight.y + msgBtnMargin + btnHeight + 2 * paddingBorder.y);

			}

			border.SetSize(borderSize.x, borderSize.y);
			messageBoder.SetSize(borderSize.x / origionalBorderSize.x * origionalMsgBorderSize.x, borderSize.y / origionalBorderSize.y * origionalMsgBorderSize.y);

			Vector3 childLayoutCtrlPos = new Vector3(0, paddingBorder.y / 4 - borderSize.y / 2f, btnContainer.transform.localPosition.z);
			btnContainer.transform.localPosition = childLayoutCtrlPos;

			float heightDelta = borderSize.y - origionalBorderSize.y;
			closeBtn.transform.localPosition = new Vector3(borderSize.x / 2f - 10f, origionalClosePos.y + heightDelta / 2f, closeBtn.transform.localPosition.z);
			titleLabel.transform.localPosition = new Vector3(0, origionalTitlePos.y + heightDelta / 2f, closeBtn.transform.localPosition.z);
		}
	}

	private void UpdateOperateBtnLayout()
	{
		if (showData.VerticalLayout)
		{
			okBtn.SetSize(VerticalButtonWidth, VerticalButtonHeight);
			cancelBtn.SetSize(VerticalButtonWidth, VerticalButtonHeight);
			thirdBtn.SetSize(VerticalButtonWidth, VerticalButtonHeight);
			okBtn.transform.localPosition = new Vector3(0f, okBtn.transform.localPosition.y, okBtn.transform.localPosition.z);
			cancelBtn.transform.localPosition = new Vector3(0f, cancelBtn.transform.localPosition.y, cancelBtn.transform.localPosition.z);
			thirdBtn.transform.localPosition = new Vector3(0f, thirdBtn.transform.localPosition.y, thirdBtn.transform.localPosition.z);
		}
		else
		{
			okBtn.SetSize(origionalBtnSize.x, origionalBtnSize.y);
			cancelBtn.SetSize(origionalBtnSize.x, origionalBtnSize.y);
			thirdBtn.SetSize(origionalBtnSize.x, origionalBtnSize.y);
			okBtn.transform.localPosition = new Vector3(okBtn.transform.localPosition.x, 0f, okBtn.transform.localPosition.z);
			cancelBtn.transform.localPosition = new Vector3(cancelBtn.transform.localPosition.x, 0f, cancelBtn.transform.localPosition.z);
			thirdBtn.transform.localPosition = new Vector3(thirdBtn.transform.localPosition.x, 0f, thirdBtn.transform.localPosition.z);
		}

		btnContainer.orientation = showData.VerticalLayout ? UIChildLayoutControl.ORIENTATION.VERTICAL_CENTER : UIChildLayoutControl.ORIENTATION.HORIZONTAL_CENTER;
		btnContainer.HideChildObj(okBtn.gameObject, !showData.UseOkBtn);
		btnContainer.HideChildObj(cancelBtn.gameObject, !showData.UseCancelBtn);
		btnContainer.HideChildObj(thirdBtn.gameObject, !showData.UseThirdBtn);
	}

	private void UpdateBtnStatus()
	{
		//Show or hide close button.
		closeBtn.Hide(!showData.UseCloseBtn);

		//Set operate btns datas.
		if (showData.UseOkBtn)
			okBtn.Text = showData.OKCallback.ControlText;

		if (showData.UseCancelBtn)
			cancelBtn.Text = showData.CancelCallback.ControlText;

		if (showData.UseThirdBtn)
			thirdBtn.Text = showData.ThirdCallback.ControlText;

		okBtn.controlIsEnabled = true;
		cancelBtn.controlIsEnabled = true;
		thirdBtn.controlIsEnabled = true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOk(UIButton btn)
	{
		//先Hide，因为在执行按钮的回调中可能还要再显示新的Dialogue
		HideSelf();
		
		if (showData.OKCallback != null && showData.OKCallback.Callback != null)
			if (showData.OKCallback.Callback(showData.OKCallback.CallbackData) == false)
				return;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCancel(UIButton btn)
	{
		//先Hide，因为在执行按钮的回调中可能还要再显示新的Dialogue
		HideSelf();
		
		if (showData.CancelCallback != null && showData.CancelCallback.Callback != null)
			if (showData.CancelCallback.Callback(showData.CancelCallback.CallbackData) == false)
				return;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnThird(UIButton btn)
	{
		//先Hide，因为在执行按钮的回调中可能还要再显示新的Dialogue
		HideSelf();
		
		if (showData.ThirdCallback != null && showData.ThirdCallback.Callback != null)
			if (showData.ThirdCallback.Callback(showData.ThirdCallback.CallbackData) == false)
				return;
	}
}