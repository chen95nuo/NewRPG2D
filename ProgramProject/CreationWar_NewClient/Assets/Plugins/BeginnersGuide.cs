using UnityEngine;
using System.Collections;

public enum InstructionsBoxStyle
{ 
    LeftTop,
    LeftBottom,
    RightTop,
    RightBottom
}

public class BeginnersGuide : MonoBehaviour {
	public static BeginnersGuide beginnersGuide;
    public Transform beginnersGuidePanel;
	public UISprite spriteScreenMask;
    public Transform lblInstruction;
    public Transform spriteArrow;
    public Transform spriteTalkbox;
    public Transform spriteGirl;
    private UILabel lblShowTxt;
    private GameObject target;//UIButtonMessage的target变量
    private string functionName;//所调用的方法
	private yuan.YuanPhoton.GameScheduleType gameScheduleType = yuan.YuanPhoton.GameScheduleType.FirstClickGO;//新手引导进度

	void Awake ()
	{
		beginnersGuide = this;
	}

	public yuan.YuanPhoton.GameScheduleType GameScheduleType
	{
		set { gameScheduleType = value; }
	}

	/// <summary>
	/// 切换新手引导的样式
	/// </summary>
	/// <param name="style">样式</param>
	/// <param name="pos">位置</param>
	/// <param name="showTxt">显示文字</param>
	/// <param name="targetObj">UIButtonMessage的target变量</param>
	/// <param name="funcName">所调用的方法</param>
	public void SwitchInstructionsBoxStyle(InstructionsBoxStyle style, Transform pos, string showTxt, GameObject targetObj, string funcName)
	{
		if (null == beginnersGuidePanel || null == lblInstruction || null == spriteArrow || null == spriteTalkbox || null == spriteGirl)
		{
			Debug.LogWarning("新手引导指示条某个变量没有赋值！！！！");
			return;
		}
		//TweenPosition tp = beginnersGuidePanel.GetComponentInChildren<TweenPosition>();
		//tp.enabled = false;
		switch (style)
		{ 
		case InstructionsBoxStyle.LeftTop:
			lblInstruction.localPosition = new Vector3(-260, -35, 0);
			spriteArrow.localPosition = new Vector3(-407, -3, 0);
			spriteArrow.localScale = new Vector3(-1, 1, 1);
			spriteTalkbox.localPosition = new Vector3(-392, -96, 0);
			spriteGirl.localPosition = new Vector3(-116, -17, 0);
			break;
		case InstructionsBoxStyle.LeftBottom:
			lblInstruction.localPosition = new Vector3(-297, -35, 0);
			spriteArrow.localPosition = new Vector3(-407, -68, 0);
			spriteArrow.localScale = new Vector3(-1, 1, 1);
			spriteTalkbox.localPosition = new Vector3(-392, -96, 0);
			spriteGirl.localPosition = new Vector3(-116, -17, 0);
			break;
		case InstructionsBoxStyle.RightTop:
			lblInstruction.localPosition = new Vector3(-198, -35, 0);
			spriteArrow.localPosition = new Vector3(-28, -3, 0);
			spriteArrow.localScale = Vector3.one;
			spriteTalkbox.localPosition = new Vector3(-392, -96, 0);
			spriteGirl.localPosition = new Vector3(-368, -17, 0);
			break;
		case InstructionsBoxStyle.RightBottom:
			lblInstruction.localPosition = new Vector3(-198, -35, 0);
			spriteArrow.localPosition = new Vector3(-28, -68, 0);
			spriteArrow.localScale = Vector3.one;
			spriteTalkbox.localPosition = new Vector3(-392, -96, 0);
			spriteGirl.localPosition = new Vector3(-368, -17, 0);
			break;
		}
		lblInstruction.GetComponent<UILabel>().text = showTxt;
		//tp.enabled = true;
		beginnersGuidePanel.localPosition = pos.localPosition;

		target = targetObj;
		functionName = funcName;

		ShowAndHideBeginnersGuide(true);
		isSaveInfoToServer = false;
	}

    /// <summary>
    /// 切换新手引导的样式
    /// </summary>
    /// <param name="style">样式</param>
    /// <param name="pos">位置</param>
    /// <param name="showTxt">显示文字</param>
    /// <param name="targetObj">UIButtonMessage的target变量</param>
    /// <param name="funcName">所调用的方法</param>
	/// /// <param name="type">服务器记录玩家行为的协议</param>
	public void SwitchInstructionsBoxStyle(InstructionsBoxStyle style, Transform pos, string showTxt, GameObject targetObj, string funcName, yuan.YuanPhoton.GameScheduleType type)
    {
		this.SwitchInstructionsBoxStyle(style, pos, showTxt, targetObj, funcName);
		gameScheduleType = type;
		isSaveInfoToServer = true;
    }

    /// <summary>
    /// 显示和隐藏新手引导
    /// </summary>
    /// <param name="isShow">是否显示</param>
    void ShowAndHideBeginnersGuide(bool isShow)
    {
//        if (beginnersGuidePanel.parent.gameObject && beginnersGuidePanel.parent.gameObject.activeSelf != isShow)
//        {
//            beginnersGuidePanel.parent.gameObject.SetActive(isShow);
//        } 

		if (this.gameObject.activeSelf != isShow)
		{
			this.gameObject.SetActive(isShow);
		} 
    }

	private bool isSaveInfoToServer = false;//是否将引导步骤信息保存到服务器
    /// <summary>
    /// 新手引导板被点击
    /// </summary>
    void GuidPanelClick()
    {
		if(isSaveInfoToServer)
		{
            if(!string.IsNullOrEmpty(tdStr))
            {
//                Debug.Log("wei++++++++++++++++++++++++++++++++++++TDstr:" + tdStr);
               // TD_info.guidePlayer(tdStr);
            }
            
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , ((int)(gameScheduleType)).ToString());
		}
//		Debug.Log("++++++++++++++++++++++++++++++++++++" + gameScheduleType.ToString());
		ShowAndHideBeginnersGuide(false);
        target.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);  
    }

    private string tdStr = "";
    private static int count = 0;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    public void SetTDString(string str)
    {
        count++;
        tdStr = string.Format("{0}.{1}",count,str);
    }

	/// <summary>
	/// Sets the sprite alpha.
	/// </summary>
	/// <param name="alpha">Alpha.</param>
	public void SetSpriteAlpha(float alpha)
	{
		if(null != spriteScreenMask)
		{
			spriteScreenMask.alpha = alpha;
		}
	}

	/// <summary>
	/// 当新手引导做到点NPC对话那一步时，引导板被点击一次后，引导板需要销毁
	/// </summary>
	void OnDisable()
	{
//		if(functionName.Equals("OneNPCTalk"))
//		{
//			Destroy(this.gameObject);
//		}
	}
}
