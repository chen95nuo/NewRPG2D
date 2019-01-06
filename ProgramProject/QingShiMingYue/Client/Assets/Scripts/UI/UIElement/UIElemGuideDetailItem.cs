using UnityEngine;
using System.Collections;

public class UIElemGuideDetailItem : MonoBehaviour
{
	public UIBox titleBg;
	public SpriteText titleLabel;
	public SpriteText contentLabel;
	public UIButton gotoBtn;

	private const float TITLE_H = 18f;
	private const float TITLE_EXCEED_W = 20f;

	public void SetData(ClientServerCommon.GuideConfig.SubType guideSubType)
	{
		if (gotoBtn != null)
		{
			//两种状态都要设置，因为将Item返回池中后会保留CopyIcon之后的状态。
			if (!GameUtility.CheckUIAccess(guideSubType.gotoUI, false))
				UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetIcon(gotoBtn, true);
			else
				UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetIcon(gotoBtn, false);

			gotoBtn.Hide(guideSubType.gotoUI == ClientServerCommon._UIType.UnKonw);
			gotoBtn.Data = guideSubType;
		}

		titleLabel.Text = guideSubType.name;

		contentLabel.Text = guideSubType.desc;

		//设置标题框的大小
		//titleBg.SetSize(TITLE_EXCEED_W + titleLabel.BottomRight.x - titleLabel.TopLeft.x, TITLE_H);
	}
}
