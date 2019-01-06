using UnityEngine;
using System.Collections;

namespace KodGames
{
	public class MoviePlayer : MonoBehaviour
	{
		public delegate void MovieFinishDelegate();

		private MovieFinishDelegate finishDel;

		public void PlayMovie(string filePath)
		{
			PlayMovie(filePath, null);
		}

		public void PlayMovie(string filePath, MovieFinishDelegate del)
		{
			PlayMovie(filePath, del, Color.black);
		}

		public void PlayMovie(string filePath, MovieFinishDelegate del, Color bgColor)
		{
			PlayMovie(filePath, del, bgColor, FullScreenMovieControlMode.Full);
		}

		public void PlayMovie(string filePath, MovieFinishDelegate del, Color bgColor, FullScreenMovieControlMode controlMode)
		{
			PlayMovie(filePath, del, bgColor, controlMode, FullScreenMovieScalingMode.AspectFit);
		}

		public void PlayMovie(string filePath, MovieFinishDelegate del, Color bgColor, FullScreenMovieControlMode controlMode, FullScreenMovieScalingMode scalingMode)
		{
			Debug.Log(string.Format("Play movie : {0}", filePath));
			finishDel = del;

#if UNITY_EDITOR
			OnMoiveFinish();
#else
			Handheld.PlayFullScreenMovie(filePath, bgColor, controlMode, scalingMode);			
#endif
		}

		public void AddFinishDelegate(MovieFinishDelegate del)
		{
			finishDel += del;
		}

		public void RemoveFinishDelegate(MovieFinishDelegate del)
		{
			finishDel -= del;
		}

		void OnMoiveFinish()
		{
			if (finishDel != null)
				finishDel();

			finishDel = null;
		}

		void OnApplicationPause(bool pause)
		{
			if (pause == false)
				OnMoiveFinish();
		}
	}
}
