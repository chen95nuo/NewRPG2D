using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIAvatarBattleBar : MonoBehaviour
{
	public LifeBar hpBar;
	public SpriteText nameCtrl;
	public UIBox bossFlag;
	private BattleRole battleRole;
	private BattleScene battleScene;
	private bool boss = false;

	private Vector3 viewPos;
	public Vector3 ViewPos { get { return viewPos; } }
	private Vector3 worldPos;
	public Vector3 WorldPos { get { return worldPos; } }

	//现在界面不会有拉伸，始终按2:3比例显示，不足部分会有背景补充，血条不必再计算缩放，否则反而会出现显示问题
	//private static float scaleY = ((float)Screen.height / (float)Screen.width) / (GameDefines.uiDefaultScreenSize.y / GameDefines.uiDefaultScreenSize.x);
	private static float scaleY = 1;

	private bool hide = false;
	public bool IsHide { get { return hide; } }

	private void Start()
	{
		bossFlag.Hide(true);

		// adjust local position
		if (scaleY.CompareTo(1f) != 0)
		{
			hpBar.CachedTransform.localPosition = new Vector3(hpBar.CachedTransform.localPosition.x, hpBar.CachedTransform.localPosition.y * scaleY, hpBar.CachedTransform.localPosition.z);
			nameCtrl.CachedTransform.localPosition = new Vector3(nameCtrl.CachedTransform.localPosition.x, nameCtrl.CachedTransform.localPosition.y * scaleY, nameCtrl.CachedTransform.localPosition.z);
			bossFlag.CachedTransform.localPosition = new Vector3(bossFlag.CachedTransform.localPosition.x, bossFlag.CachedTransform.localPosition.y * scaleY, bossFlag.CachedTransform.localPosition.z);
		}
	}

	public void Hide(bool tf)
	{
		hide = tf;
		hpBar.Hide(tf);
		nameCtrl.Hide(tf);

		if (tf == false)
			UpdateHP();

		if (boss)
		{
			bossFlag.Hide(tf);
		}
	}

	public void DestroySelf()
	{
		var fx = GetComponent<FXController>();
		if (fx != null)
			fx.StopFX();
		else
			Object.Destroy(this.gameObject);
	}

	public void SetBattleRole(BattleRole role)
	{
		battleScene = BattleScene.GetBattleScene();

		battleRole = role;

		nameCtrl.Text = battleRole.AvatarData.DisplayName;
		boss = battleRole.AvatarData.NpcType == _NpcType.Boss;

		UpdateHP();
		UpdatePosition(true);
	}

	public void UpdateHP()
	{
		hpBar.Value = (float)System.Math.Max(0, System.Math.Min(1, battleRole.AvatarHP / battleRole.AvatarData.GetAttributeByType(_AvatarAttributeType.MaxHP).Value));
	}

	private void UpdatePosition(bool updateZ)
	{
		// Attach to battle role
		if (battleRole != null && Camera.main != null && Camera.main.enabled)
		{
			SysUIEnv uiEvn = SysModuleManager.Instance.GetSysModule<SysUIEnv>();

			// It's in world space, convert to UI space.
			float heightScale = 0;
			if (battleRole.TeamIndex != 0 && string.IsNullOrEmpty(battleScene.sponsorLiftBar) == false)
			{
				// Boss special
				heightScale = 1 + (battleRole.CachedTransform.localScale.x - 1);
			}
			else
			{
				heightScale = 1 + (battleRole.CachedTransform.localScale.x - 1) / 2;
			}

			viewPos = Camera.main.WorldToViewportPoint(battleRole.CachedTransform.position + battleScene.GetAvatarHeight() * heightScale + battleScene.GetAvatarBattleBarOffset());
			worldPos = uiEvn.UICam.ViewportToWorldPoint(viewPos);
			if (updateZ)
				worldPos.z = uiEvn.UIFxRoot.position.z + uiEvn.DynamicLocalZ;
			else
				worldPos.z = CachedTransform.position.z;

			CachedTransform.position = worldPos;
		}
	}

	public void Update()
	{
		UpdatePosition(false);
	}
}
