using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class GameMovieSceneData : MonoBehaviour
{
	public delegate void OnMovieFinishDelegate();

	public OnMovieFinishDelegate movieFinishDel;

	public KodGames.MoviePlayer movePlayer;	
	public string moviePath;
	public FullScreenMovieControlMode moveControlMode = FullScreenMovieControlMode.Hidden;
	public bool TestModel = false;

	private static GameMovieSceneData instance = null;
	public static GameMovieSceneData Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(GameMovieSceneData)) as GameMovieSceneData;

			return instance;
		}
	}

	void Awake()
	{
		if(TestModel)
			PlayMovie();
	}

	public void PlayMovie()
	{
		Platform.Instance.ShowToolBar(false);
		movePlayer.PlayMovie(moviePath, OnMoveFinished, Color.black, FullScreenMovieControlMode.Hidden);
	}

	private void OnMoveFinished()
	{
		Platform.Instance.ShowToolBar(true);
		if (movieFinishDel != null)
			movieFinishDel();
	}
}
