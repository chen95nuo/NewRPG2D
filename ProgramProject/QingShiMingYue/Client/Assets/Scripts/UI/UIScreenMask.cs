using UnityEngine;
using System.Collections;

public class UIScreenMask : MonoBehaviour
{
	public float standardScreenWidth = 320;
	public float standardScreenHeight = 480;
	public bool enableBorderH = true;
	public bool enableBorderV = true;
	public UIButton leftBorder;
	public UIButton rightBorder;
	public UIButton topBorder;
	public UIButton bottomBorder;

	private UIScreenMask instance;
	public UIScreenMask Instance { get { return instance; } }

	public bool EnableBorderH
	{
		get { return enableBorderH; }
		set
		{
			if (enableBorderH == value)
				return;

			enableBorderH = value;
			SetBorderSize();
		}
	}

	public bool EnableBorderV
	{
		get { return enableBorderV; }
		set
		{
			if (enableBorderV == value)
				return;

			enableBorderV = value;
			SetBorderSize();
		}
	}

	void Awake()
	{
		instance = this;

		Object.DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		SetBorderSize();
	}

	private void SetBorderSize()
	{
		float standardScreenProportion = standardScreenWidth / standardScreenHeight;
		float currentScreenProportion = Screen.width / (float)Screen.height;
		if (Mathf.Approximately(currentScreenProportion, standardScreenProportion))
		{
			leftBorder.gameObject.SetActive(false);
			rightBorder.gameObject.SetActive(false);
			topBorder.gameObject.SetActive(false);
			bottomBorder.gameObject.SetActive(false);
		}
		else if (currentScreenProportion < standardScreenProportion)
		{
			// V-Scale
			leftBorder.gameObject.SetActive(false);
			rightBorder.gameObject.SetActive(false);

			if (enableBorderV)
			{
				topBorder.gameObject.SetActive(true);
				bottomBorder.gameObject.SetActive(true);

				// Update top bottom border
				float borderSize = SysUIEnv.Instance.UICam.orthographicSize - standardScreenHeight / 2;
				topBorder.SetSize(topBorder.width, borderSize);
				bottomBorder.SetSize(bottomBorder.width, borderSize);
			}
			else
			{
				topBorder.gameObject.SetActive(false);
				bottomBorder.gameObject.SetActive(false);
			}
		}
		else
		{
			// H-Scale
			topBorder.gameObject.SetActive(false);
			bottomBorder.gameObject.SetActive(false);

			if (enableBorderH)
			{
				leftBorder.gameObject.SetActive(true);
				rightBorder.gameObject.SetActive(true);

				// Update top bottom border
				float borderSize = (Screen.width / (float)Screen.height * SysUIEnv.Instance.UICam.orthographicSize * 2 - standardScreenWidth) / 2;
				leftBorder.SetSize(borderSize, leftBorder.height);
				rightBorder.SetSize(borderSize, rightBorder.height);
			}
			else
			{
				leftBorder.gameObject.SetActive(false);
				rightBorder.gameObject.SetActive(false);
			}
		}
	}

#if UNITY_EDITOR
	public void Update()
	{
		SetBorderSize();
	}
#endif
}