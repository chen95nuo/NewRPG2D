namespace KodGames
{
	public static class Camera
	{
		private static UnityEngine.Camera _main = null;
		public static UnityEngine.Camera main
		{
			get
			{
				if (_main != null)
					return _main;

				UnityEngine.GameObject go = UnityEngine.GameObject.FindGameObjectWithTag("MainCamera");
				if (go == null)
				{
					Debug.LogError("No MainCamera GameoObject");
					return null;
				}

				_main = go.GetComponent(typeof(UnityEngine.Camera)) as UnityEngine.Camera;
				return _main;
			}
		}
	}
}
