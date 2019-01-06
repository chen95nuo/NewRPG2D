using UnityEngine;
using UnityEditor;

public class AShimSetGameSizeWindow : EditorWindow
{

	private Vector2 _size = new Vector2(640, 1136);

	[MenuItem("Window/Set Game Size...")]
	public static void Init()
	{
		EditorWindow.GetWindow(typeof(AShimSetGameSizeWindow));
	} // Init()

	public static EditorWindow GetMainGameView()
	{
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetMainGameView.Invoke(null, null);
		return (EditorWindow)Res;
	} // GetMainGameView()

	void OnGUI()
	{
		_size.x = EditorGUILayout.IntField("X", (int)_size.x);
		_size.y = EditorGUILayout.IntField("Y", (int)_size.y);
		if (GUILayout.Button("Set"))
		{
			EditorWindow gameView = GetMainGameView();
			Rect pos = gameView.position;
			pos.y = pos.y - 0;
			pos.width = _size.x;
			pos.height = _size.y + 17;
			gameView.position = pos;
		}
	} // OnGUI()

} // class AShimSetGameSizeWindow