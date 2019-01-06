using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

class UIPnlFriendCampaginShop : UIModule
{
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		return true;
	}
}