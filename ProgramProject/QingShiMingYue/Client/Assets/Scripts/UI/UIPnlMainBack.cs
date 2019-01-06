using UnityEngine;
using ClientServerCommon;

public class UIPnlMainBack : UIModule
{
	public UIButton backBtn;

	public class ShowData
	{
		private MainMenuItem backBehaviour;
		public MainMenuItem backCallback
		{
			get { return this.backBehaviour; }
		}
		public bool UseOkBtn
		{
			get { return backBehaviour != null; }
		}

		public void SetData(MainMenuItem backBtn)
		{
			this.backBehaviour = backBtn;
		}
	}

	private ShowData showData;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!(base.OnShow(layer, userDatas)))
			return false;

		this.showData = userDatas[0] as ShowData;

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOk(UIButton btn)
	{
		//先Hide，因为在执行按钮的回调中可能还要再显示
		HideSelf();

		if (showData.backCallback != null && showData.backCallback.Callback != null)
			if (showData.backCallback.Callback(showData.backCallback.Callback) == false)
				return;
	}
}
