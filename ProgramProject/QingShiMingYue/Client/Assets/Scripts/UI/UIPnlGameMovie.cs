using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;
using System.Collections;

public class UIPnlGameMovie : UIModule
{
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		object screenMask = GameObject.FindObjectOfType(typeof(UIScreenMask));
		if (screenMask != null)
			(screenMask as UIScreenMask).EnableBorderV = false;

		StartCoroutine("PlayMovie");
		return true;
	}

	void OnDestroy()
	{
		object screenMask = GameObject.FindObjectOfType(typeof(UIScreenMask));
		if (screenMask != null)
			(screenMask as UIScreenMask).EnableBorderV = true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator PlayMovie()
	{
		// 播放视频会进入后台, 导致游戏断线, 延迟两帧使能新手消息能够发送出去
		yield return null;
		yield return null;
		GameMovieSceneData.Instance.movieFinishDel = OnMovieFinished;
		GameMovieSceneData.Instance.PlayMovie();
	}

	protected void OnMovieFinished()
	{
		HideSelf();
	}
}
