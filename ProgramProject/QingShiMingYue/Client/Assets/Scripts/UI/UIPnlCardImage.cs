using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlCardImage : UIModule
{
	public UIElemAvatarCard avatarCard;

    public override bool OnShow(_UILayer layer, params object[] userDatas)
    {
        if (base.OnShow(layer, userDatas) == false)
            return false;

		avatarCard.SetData((int)userDatas[0], (bool)userDatas[1], (bool)userDatas[2], (Material)userDatas[3]);

        return true;
    }

    [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
    private void OnCloseClick(UIButton btn)
    {
		avatarCard.StopVoice();
        HideSelf();
    }
}
