using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

// 战斗前摄像机动画round，纯客户端逻辑round，与服务器无关
// 只在竞技场战斗前播放摄像机动画时使用
public class CameraAnimationBattleRound : BattleRound
{
	private Animation currAnim = null;
	private AnimationState currState = null;
	private Camera currCamera = null;
	private int currAnimIndex = 0;

	public CameraAnimationBattleRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	public override BattleRound Initialize()
	{
		base.Initialize();

		return this;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		PlayNextCameraAnimation();
		
		battleRecordPlayer.CanSkipCameraAnimation = true;

		return true;
	}

	private void PlayNextCameraAnimation()
	{
		if(currAnimIndex >= battleRecordPlayer.BattleScene.GetCameraAnimationCount())
		{
			return;
		}
		
		currAnim = battleRecordPlayer.BattleScene.cameraAnimaObj[currAnimIndex].animation;
		currCamera = battleRecordPlayer.BattleScene.cameraAnima[currAnimIndex++];

		battleRecordPlayer.BattleScene.SetAnimCameraEnable(currCamera);
		foreach(AnimationState state in currAnim)
		{
			currState = state;
			break;
		}
		currState.wrapMode = WrapMode.ClampForever;
		currAnim.Play(currState.name);
	}

	private bool IsCurrAnimationFinished()
	{
		return currState.time >= currState.length;
	}

	public override bool CanStartAfterRound()
	{
		if(battleRecordPlayer.IsSkipCameraAnimation)
		{
			return true;
		}
		
		return currAnimIndex == battleRecordPlayer.BattleScene.GetCameraAnimationCount() && IsCurrAnimationFinished();
	}

    public override void Finish()
	{
		base.Finish();
		battleRecordPlayer.BattleScene.SetAnimCameraEnable(null);
	}

	private void OnLoadingShowComplete()
	{
		//SysUIEnv.Instance.HideUIModule(typeof(UIPnlLoading));
		PlayNextCameraAnimation();
	}

	public override void Update()
	{
		if (RoundState != _RoundState.Running)
		{
			return;
		}

		base.Update();
		
		if(battleRecordPlayer.IsSkipCameraAnimation)
		{
			Finish();
			return;
		}
		
		currCamera.transform.LookAt(Vector3.zero);

		if (IsCurrAnimationFinished())
		{
			OnLoadingShowComplete();

			//SysUIEnv.Instance.GetUIModule<UIPnlLoading>().SetShowAnimCompletionDelegate(
			//() =>
			//{
			//    OnLoadingShowComplete();
			//});
			//SysUIEnv.Instance.ShowUIModule(typeof(UIPnlLoading),true);
		}
	}
}
