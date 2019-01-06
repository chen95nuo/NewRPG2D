using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UITipHelp : UIModule
{
	public GameObject messageRoot;
	public SpriteText messageLabel;

	public GameObject arrowPoint;
	public UIButton screenButton;
	public UIButton messageClostBtn;
	public GameObject piXiuRoot;
	public EZAnimation.EASING_TYPE animationEasingType;
	public GameObject leftPosition;
	public GameObject rightPosition;

	private ClientServerCommon.TutorialConfig.Action action;
	private GameObject attachObj;

	public delegate void OnModuleHideDel();
	private OnModuleHideDel uiHideDel;

	public float animationDuration = 150f;
	public float arrowMoveMaxTime = 1.2f;
	public float arrowMoveMinTime = 0.4f;

	private bool IsSoundPlaying = false;
	private bool ArrowShown = false;
	private bool IsAnimatePlaying = false;
	private AnimatePosition position;
	private Vector3 arrowAttachPosition = Vector3.zero;


	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		ClearData();

		arrowPoint.SetActive(false);
		messageRoot.SetActive(false);
		messageLabel.Text = "";

		ArrowShown = false;
		IsSoundPlaying = false;
		IsAnimatePlaying = false;

		return true;
	}

	public override void OnHide()
	{
		if (position != null)
		{
			position.Clear();
			EZAnimator.instance.StopAnimation(position, true);
		}

		ObjectUtility.DestroyChildObjects(arrowPoint);

		base.OnHide();

		//记录界面关闭时手指的位置
		SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("ArrowPositon_x", arrowPoint.transform.position.x);
		SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("ArrowPositon_y", arrowPoint.transform.position.y);

		StopVoices();
		ClearData();

		if (uiHideDel != null)
			uiHideDel();
	}

	private void ClearData()
	{
		this.attachObj = null;
		this.uiHideDel = null;

		ArrowShown = false;
		IsSoundPlaying = false;
		IsAnimatePlaying = false;
	}

	public void ShowHelp(ClientServerCommon.TutorialConfig.Action action, GameObject attachObj, OnModuleHideDel uiHideDel)
	{
		if (ShowSelf() == false)
			return;

		this.action = action;
		this.attachObj = attachObj;
		this.uiHideDel = uiHideDel;

		if (action.showNpc == false && action.attachComponentName.Equals(""))
			Debug.LogError("Both ShowNpc and AttachComponent not Data!!");

		// If show NPC (it means show message ) set the UI visible.
		if (action.showNpc)
		{
			messageRoot.SetActive(true);

			//设置貔貅基于对话框的位置（左上，右上）
			if (action.arrowDirection == TutorialConfig._NPCDirection.Left)
				piXiuRoot.transform.position = new Vector3(leftPosition.transform.position.x, leftPosition.transform.position.y, piXiuRoot.transform.localPosition.z);
			else
				piXiuRoot.transform.position = new Vector3(rightPosition.transform.position.x, rightPosition.transform.position.y, rightPosition.transform.localPosition.z);
		}
		else
			messageRoot.SetActive(false);

		// If dialogues message should auto scroll , set the screen Button 's invoke method and show it.

		if (string.IsNullOrEmpty(action.strValue))
			messageLabel.Text = "";
		else
			messageLabel.Text = action.strValue;

		SetAchorButton();

	}

	public void ShowHelp(ClientServerCommon.TutorialConfig.Action action)
	{
		ShowHelp(action, null, null);
	}

	public void ShowHelp(ClientServerCommon.TutorialConfig.Action action, GameObject attachObj)
	{
		ShowHelp(action, attachObj, null);
	}

	private void SetAchorButton()
	{
		// If has old FX, Destroy it.

		//获取上一次引导手指的位置
		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("ArrowPositon_x") &&
			SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("ArrowPositon_y"))
		{
			float clientValue_x = (float)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("ArrowPositon_x");
			float clientValue_y = (float)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("ArrowPositon_y");

			arrowPoint.transform.position = new Vector3(clientValue_x, clientValue_y, arrowPoint.transform.position.z);
		}

		arrowPoint.transform.localScale = new Vector3(action.arrowScale.x, action.arrowScale.y, action.arrowScale.z);


		//查找需要指向的object
		if (attachObj == null)
		{
			if (action.componentType || action.buttonData > 0)
			{
				if (action.isSkillOrEquip)
					attachObj = GetListIndexByData(action);
				else
					SysUIEnv.Instance.FindControlInListByTag(action.attachComponentName, action.intValue - 1, action.buttonData, ref attachObj);
			}
			else
				attachObj = GameObject.FindWithTag(action.attachComponentName);
		}

		if (attachObj != null)
			arrowPoint.transform.position = new Vector3(
				arrowPoint.transform.position.x,
				arrowPoint.transform.position.y,
				attachObj.transform.localPosition.z);

		//引导没有貔貅对话，直接显示手指
		if (!action.showNpc)
			ArrowMove();

		PlayerVoices();
	}

	private void ArrowMove()
	{
		//手指缩放 vector3.zero为手指不显示
		if (new Vector3(action.arrowScale.x, action.arrowScale.y, action.arrowScale.z) == Vector3.zero)
		{
			arrowPoint.SetActive(false);
			return;
		}
		arrowPoint.SetActive(true);

		GameObject fxControll = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.tutorialHandStatic));

		ObjectUtility.AttachToParentAndResetLocalTrans(arrowPoint, fxControll);

		SetArrowPosition();

		position = AnimatePosition.Do(arrowPoint, EZAnimation.ANIM_MODE.FromTo,
			arrowPoint.transform.position,
			arrowAttachPosition,
			EZAnimation.GetInterpolator(animationEasingType),
			SetMoveSpeed(),
			0f, null,
			(data) =>
			{
				SetAnimation();
			});

	}

	private float SetMoveSpeed()
	{
		float time = Math.Abs(Vector3.Distance(arrowPoint.transform.position, arrowAttachPosition) / animationDuration);

		return Mathf.Clamp(time, arrowMoveMinTime, arrowMoveMaxTime);
	}

	private void SetAnimation()
	{
		if (IsAnimatePlaying)
			return;

		if (arrowPoint == null)
			return;

		if (!arrowPoint.activeSelf)
			return;

		SetArrowPosition(false);

		arrowPoint.transform.position = new Vector3(arrowAttachPosition.x, arrowAttachPosition.y, arrowPoint.transform.position.z);

		ObjectUtility.DestroyChildObjects(arrowPoint);

		FXController fxControll = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.uiFx_Assistant)).GetComponent<FXController>();

		ObjectUtility.AttachToParentAndResetLocalTrans(arrowPoint, fxControll.gameObject);

		IsAnimatePlaying = true;
	}

	private void SetArrowPosition()
	{
		if (attachObj == null)
		{
			string errorMsg = string.Format("Tutorial ObjectTag {0} is not found.", action.attachComponentName);
			Debug.LogError(errorMsg);
			GameAnalyticsUtility.OnEventErrorMessage("UITipHelp", errorMsg);
			return;
		}

		//UIObject or SceneObject
		if (SysUIEnv.Instance.IsUIObject(attachObj))
			arrowAttachPosition = new Vector3(attachObj.transform.position.x, attachObj.transform.position.y, arrowAttachPosition.z);
		else
		{
			arrowAttachPosition = KodGames.Camera.main.WorldToScreenPoint(attachObj.transform.position);
			arrowAttachPosition = SysUIEnv.Instance.UICam.ScreenToWorldPoint(arrowAttachPosition) + new UnityEngine.Vector3(0, 0, -1);
		}
		arrowAttachPosition.z = attachObj.transform.localPosition.z;

		if (new Vector3(action.arrowPosOffset.x, action.arrowPosOffset.y, action.arrowPosOffset.z) == Vector3.zero)
			arrowAttachPosition = new Vector3(arrowAttachPosition.x + 20f, arrowAttachPosition.y - 10f, arrowAttachPosition.z);

	}

	private void SetArrowPosition(bool IsStatic)
	{
		if (IsStatic)
			return;

		if (attachObj == null)
		{
			if (action.componentType || action.buttonData > 0)
			{
				if (action.isSkillOrEquip)
					attachObj = GetListIndexByData(action);
				else
					SysUIEnv.Instance.FindControlInListByTag(action.attachComponentName, action.intValue - 1, action.buttonData, ref attachObj);
			}
			else
				attachObj = GameObject.FindWithTag(action.attachComponentName);
		}

		if (attachObj == null)
			return;

		if (SysUIEnv.Instance.IsUIObject(attachObj))
			arrowAttachPosition = new Vector3(attachObj.transform.position.x, attachObj.transform.position.y, arrowAttachPosition.z);
		else
		{
			arrowAttachPosition = KodGames.Camera.main.WorldToScreenPoint(attachObj.transform.position);
			arrowAttachPosition = SysUIEnv.Instance.UICam.ScreenToWorldPoint(arrowAttachPosition) + new UnityEngine.Vector3(0, 0, -1);
		}

		arrowAttachPosition.z = attachObj.transform.localPosition.z;

		if (new Vector3(action.arrowPosOffset.x, action.arrowPosOffset.y, action.arrowPosOffset.z) != Vector3.zero)
			arrowAttachPosition = new Vector3(arrowAttachPosition.x + action.arrowPosOffset.x, arrowAttachPosition.y + action.arrowPosOffset.y, arrowAttachPosition.z);
	}

	private void Update()
	{
		if (!AudioManager.Instance.IsSoundPlaying(action.voice) && IsSoundPlaying)
			IsSoundPlaying = false;

		//SetAnimtion();

		//音效播放完毕，本身引导带有貔貅对话
		if (IsSoundPlaying == false && action.showNpc && !ArrowShown && !string.IsNullOrEmpty(action.voice))
		{
			ArrowMove();
			messageRoot.SetActive(false);
			ArrowShown = true;
		}

	}

	private GameObject GetListIndexByData(TutorialConfig.Action action)
	{
		GameObject scrollObj = GameObject.FindGameObjectWithTag(action.attachComponentName);

		UIScrollList scrollList = scrollObj.GetComponent<UIScrollList>();

		if (scrollList.IsScrolling)
			return null;

		if (scrollObj == null)
		{
			Debug.Log("Could Not Found ScrollList with tag " + action.attachComponentName);
		}
		else
		{

			for (int i = 0; i < scrollList.Count; i++)
			{
				UIListItemContainer container = scrollList.GetItem(i) as UIListItemContainer;
				if (container == null || container.Data == null)
					continue;

				UIElemEquipSelectItem item = container.Data as UIElemEquipSelectItem;
				if (item != null)
				{
					int id = (int)item.selectBtn.indexData;

					if (id == action.buttonData)
					{
						return item.selectBtn.gameObject;
					}
				}
				else
				{
					UIElemSkillItem skillItem = container.Data as UIElemSkillItem;
					int id = (int)skillItem.selectBtn.indexData;

					if (id == action.buttonData)
					{
						return skillItem.selectBtn.gameObject;
					}
				}
			}
		}
		return null;
	}

	#region Voice

	private void PlayerVoices()
	{
		if (!string.IsNullOrEmpty(action.voice))
		{
			if (AudioManager.Instance.IsSoundPlaying(action.voice))
				AudioManager.Instance.StopSound(action.voice);

			AudioManager.Instance.PlayStreamSound(action.voice, 0.1f);
		}

		if (!IsSoundPlaying) IsSoundPlaying = true;
	}

	private void StopVoices()
	{
		if (AudioManager.Instance.IsSoundPlaying(action.voice))
			AudioManager.Instance.StopSound(action.voice);

		if (IsSoundPlaying) IsSoundPlaying = false;
	}

	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		if (action.tapClose && !messageRoot.activeSelf)
		{
			HideSelf();

			if (action.tapCloseUIType != ClientServerCommon._UIType.UnKonw)
				SysUIEnv.Instance.ShowUIModule(action.tapCloseUIType);
		}
		else if (action.showNpc)
		{
			messageRoot.SetActive(false);
			StopVoices();
		}
	}
}
