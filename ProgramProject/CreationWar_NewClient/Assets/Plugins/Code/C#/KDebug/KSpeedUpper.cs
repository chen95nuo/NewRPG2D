using UnityEngine;
using System.Collections;

public	class KSpeedUpper : MonoBehaviour
{

	// Use this for initialization
	void	Start()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void	Update()
	{
#if UNITY_EDITOR
		if(	Input.GetKeyDown(	KeyCode.UpArrow		)	)
		{
			Time.timeScale	+=	0.1f;
		}
		if(	Input.GetKeyDown(	KeyCode.DownArrow	)	)
		{
			Time.timeScale	-=	0.1f;
			if(	Time.timeScale	<	0.2f	)
			{
				Time.timeScale	=	0.2f;
			}
		}
#endif
	}
}
