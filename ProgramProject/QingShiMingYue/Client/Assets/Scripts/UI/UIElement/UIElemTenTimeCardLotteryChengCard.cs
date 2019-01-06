using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class UIElemTenTimeCardLotteryChengCard : MonoBehaviour
{
	public UIElemAvatarCard avatarCard;
	public FXController fxController;
	public UIBox avatarCountryBox;
	public UIBox avatarTraitBox;
	public AnimationEventHandler animEvtHandler;

	public void SetData(int avatarAssetId)
	{
		avatarCard.SetData(avatarAssetId, false, false, null);
		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarAssetId);

		//设置国家
		UIElemTemplate.Inst.SetAvatarCountryIcon(avatarCountryBox, avatarCfg.countryType);
		//设置属性
		UIElemTemplate.Inst.SetAvatarTraitIcon(avatarTraitBox, avatarCfg.traitType);
	}
}
