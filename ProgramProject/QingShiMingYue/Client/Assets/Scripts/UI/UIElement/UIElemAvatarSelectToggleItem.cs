using UnityEngine;
using System.Collections;
using ClientServerCommon;
using KodGames;
using KodGames.ClientClass;

public class UIElemAvatarSelectToggleItem : MonoBehaviour
{
	//List item avatar icon button.
	public UIBox avatarSelectBtn;
	public UIElemAssetIcon  avatarIconBorderBtn;
	public UIButton avatarQualityBtn;
	
	private KodGames.ClientClass.Avatar avatarData;
	public KodGames.ClientClass.Avatar AvatarData
	{
		get { return this.avatarData; }
	}
	
	private bool isSelected = false;
	public bool IsSelected
	{
		get { return this.isSelected; }
	}
	
	/// <summary>
	/// Set the avatar icon item data.
	/// </summary>
	/// <param name='avatar'>
	/// Avatar.
	/// </param>
	/// <param name='scriptMethodToInvoke'>
	/// Script method to invoke.
	/// </param>
	/// <param name='method'>
	/// Method.
	/// </param>
	public void SetData(KodGames.ClientClass.Avatar avatar)
	{
		avatarData = avatar;
		if(avatar == null)
		{
			avatarIconBorderBtn.gameObject.SetActive(false);
			return;
		}
		else
		{
			avatarIconBorderBtn.gameObject.SetActive(true);
		}
		
		if(avatarQualityBtn != null)
		{
			//Avatar quality.
//			int quality = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).qualityLevel;
			
			//Set quality button icon.
			UIElemTemplate uiElemTemplate = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIElemTemplate>();
		 	UIElemAvatarQualityTemplate avatarQualityTemplate = uiElemTemplate.avatarQualityTemplate;
			uiElemTemplate.gameObject.SetActive(false);
			
			//If quality is right, set the star icon.
			UIUtility.CopyIcon(avatarQualityBtn, avatarQualityTemplate.avatarQualityBtn);
		}
		
		avatarIconBorderBtn.Data = this;
		
		//Set the avatar icon by resource Id.
		avatarIconBorderBtn.SetData(avatar);
	}
	
	/// <summary>
	/// Get the icon control.
	/// </summary>
	/// <returns>
	/// The control.
	/// </returns>
	
	public void ToggleState()
	{
		isSelected = !isSelected;
		SelectBtnToggle(isSelected);
	}
	
	public void ResetToggleState(bool selected)
	{
		isSelected = selected;
		SelectBtnToggle(selected);
	}
	
	private void SelectBtnToggle(bool selected)
	{
		avatarSelectBtn.Start();
		if(selected)
		{
			avatarSelectBtn.SetToggleState(0);
			UIUtility.CopyIconTrans(avatarIconBorderBtn.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);
		}
		else
		{
			avatarSelectBtn.SetToggleState(1);
			UIUtility.CopyIconTrans(avatarIconBorderBtn.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardGray);
		}
	}
}
