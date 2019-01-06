using UnityEngine;
using System.Collections;

public class UIPnlTip : UIModule 
{
	//Tip content.
	public SpriteText tipContentLabel;
	
	//Tip box and invisible button.
	public UIButton tipScreenBtn;
	public UIBox tipBoxBtn;
	
	//Tip content pro.
	public float TipMaxHeight = 340f;
	public float TipMinHeight = 75f;
	public float TipMaxWidth = 280f;
	public float TipMinWidth = 220f;
	public float TipBoxPaddingTop = 20f;
	public float TipBoxPaddingBot = 20f;
	public float TipBoxPaddingLeft = 20f;
	public float TipBoxPaddingRight = 20f;
	
	//Local data.
	private ShowData showData;
	
	//Show tip according to the show data.
	public bool ShowTip(ShowData showData)
	{
		tipScreenBtn.scriptWithMethodToInvoke = this;
		
		if(showData.isScreenBtnHide)
		{
			tipScreenBtn.methodToInvoke = "OnHideClick";
		}
		else
		{
			tipScreenBtn.methodToInvoke = "";
		}
		
		tipContentLabel.Text = showData.tipText;
		this.showData = showData;

		float totalWidth = tipContentLabel.GetWidth(showData.tipText);
		float lineHeight = tipContentLabel.GetLineHeight();
		float charWidth = tipContentLabel.characterSize * tipContentLabel.characterSpacing;
		
		int minNeedLines = 0;
		int maxNeedLines = 0;
		
		for(int index = 0, begin = 0, length = 1; index < tipContentLabel.Text.Length; index++)
		{
			float result = TipMaxWidth - TipBoxPaddingLeft - TipBoxPaddingRight 
				- tipContentLabel.GetWidth(tipContentLabel.Text.Substring(begin, length++));
			
			if(result < 0)
			{
				maxNeedLines++;
				begin = index--;
				length = 1;
			}
			else
			{
				if(index == tipContentLabel.text.Length - 1)
				{
					maxNeedLines++;
				}
			}
			
			
		}
		
		for(int index = 0, begin = 0, length = 1; index < tipContentLabel.Text.Length; index++)
		{
			float result = TipMinWidth - TipBoxPaddingLeft - TipBoxPaddingRight 
				- tipContentLabel.GetWidth(tipContentLabel.Text.Substring(begin, length++));
			
			if(result < 0)
			{
				minNeedLines++;
				begin = index--;
				length = 1;
			}
			else
			{
				if(index == tipContentLabel.text.Length - 1)
				{
					minNeedLines++;
				}
			}
		}
		
		
		int minContainLines = Mathf.FloorToInt((TipMinHeight - TipBoxPaddingTop - TipBoxPaddingBot)/lineHeight);
		int maxContainLines = Mathf.FloorToInt((TipMaxHeight - TipBoxPaddingTop - TipBoxPaddingBot)/lineHeight);

        int newLineCount = showData.tipText.Split('\n').Length;
        minNeedLines = Mathf.Max(minNeedLines, newLineCount);
        maxNeedLines = Mathf.Max(maxNeedLines, newLineCount);

		if(maxNeedLines > minContainLines && maxNeedLines < maxContainLines)
		{
			tipContentLabel.maxWidth = TipMaxWidth - TipBoxPaddingLeft - TipBoxPaddingRight;
			tipContentLabel.Text = showData.tipText;
			
			tipBoxBtn.SetSize(TipMaxWidth, maxNeedLines*lineHeight + TipBoxPaddingTop + TipBoxPaddingBot);
		}
		else if(minNeedLines <= minContainLines)
		{
			tipContentLabel.maxWidth = TipMinWidth - TipBoxPaddingLeft - TipBoxPaddingRight;
			tipContentLabel.Text = showData.tipText;
			
			tipBoxBtn.SetSize(TipMinWidth, TipMinHeight);
		}
		else if(maxNeedLines <= minContainLines &&  minNeedLines > minContainLines)
		{	
			for(float width = TipMinWidth; width < TipMaxWidth; width += charWidth)
			{
				if(Mathf.CeilToInt(totalWidth/(width - TipBoxPaddingLeft - TipBoxPaddingRight)) <= minContainLines)
				{
					tipContentLabel.maxWidth = width - TipBoxPaddingLeft - TipBoxPaddingRight;
					tipContentLabel.Text = showData.tipText;

					tipBoxBtn.SetSize(width, TipMinHeight);
					break;
				}
			}
		}
		else if(maxNeedLines >= maxContainLines)
		{
			tipContentLabel.maxWidth = TipMaxWidth - TipBoxPaddingLeft - TipBoxPaddingRight;
			tipContentLabel.Text = showData.tipText;
			
			tipBoxBtn.SetSize(TipMaxWidth, TipMaxHeight);
		}
		return ShowSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnHideClick(UIButton btn)
	{
		HideSelf();
		
		if(showData.OnHideCallback != null)
		{
			showData.OnHideCallback(showData.onHideCallbackObj);
		}
	}
	
	public class ShowData
	{
		public string tipText = "";
		public bool isScreenBtnHide = false;
		public bool tapClose = false;
		
		public delegate bool Callback(object data);
		public Callback OnHideCallback = null;
		public object onHideCallbackObj = null;
		
		public void SetData(string tipText, bool isScreenBtnHide, bool tapClose)
		{
			this.tipText = tipText;
			this.isScreenBtnHide = isScreenBtnHide;
			this.tapClose = tapClose;
		}
		
		public void SetData(string tipText, bool isScreenBtnHide, bool tapClose,Callback onHideCallback,object onHideCallbackObj)
		{
			this.tipText = tipText;
			this.isScreenBtnHide = isScreenBtnHide;
			this.tapClose = tapClose;
			this.OnHideCallback = onHideCallback;
			this.onHideCallbackObj = onHideCallbackObj;
		}
	}
}
