using System;
using System.Collections.Generic;

public class UIPnlTowerScene : UIModule
{
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		TowerSceneData.Instance.MainCamera.enabled = true;

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		TowerSceneData.Instance.MainCamera.enabled = false;
	}

	public override void Overlay()
	{
		base.Overlay();

		TowerSceneData.Instance.MainCamera.enabled = false;
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		TowerSceneData.Instance.MainCamera.enabled = true;
	}
}
