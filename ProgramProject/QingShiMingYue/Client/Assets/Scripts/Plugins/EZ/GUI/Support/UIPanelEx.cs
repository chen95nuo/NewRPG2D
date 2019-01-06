using UnityEngine;
using System.Collections;

// Panel shown mode.
public class SW_MD
{
	// Model mode.
	public const int Model = 1 << 0; // Do model mode.

	// Transition.
	public const int Transition = 1 << 1; // Has transition.
}

public class UIPanelEx
{
	// Panel callback.
	public delegate void TransFinishedCb(object userData); // Panel transition finished callback.
	public delegate void CloseCb(object userData); // Panel close callback.
		
	private UIPanelBase uiPnl; // UI panel base.
	private UIPanelTab uiPnlTb; // UI panel tab binding to this panel.
	
	private bool model; // Showing mode.
	private bool shown; // Showing flag.
	
	private float offsetZ = 0; // Offset z.
	
	private TransFinishedCb trnFnsCb; // Transition finished callback.
	private object trnFnsUsrDt; // Transition finished callback user data.
	
	private CloseCb clsCb; // Close callback.
	private object clsCbUsrDt; // Close callback user data.
	
	private UIButton btMask; // Mask button.
	private Color clrMask = new Color(0.0f, 0.0f, 0.0f, 0.5f); // mask color.

	public Transform Trans
	{
		get { return uiPnl.CachedTransform; }
	}

	public GameObject Obj
	{
		get { return uiPnl.gameObject; }
	}

	public string Name
	{
		get { return Obj.name; }
	}

	public bool IsShown
	{
		get { return shown; }
	}

	public bool IsTrans
	{
		//		get { return ( swMd & SW_MD.Transition ) != 0; }
		get { return false; }
	}

	public bool IsModel
	{
		//		get { return ( swMd & SW_MD.Model ) != 0; }
		get { return model; }
	}

	public float OffsetZ
	{
		get { return offsetZ; }
	}

	public UIPanelEx(UIPanelBase pnl, bool model)
	{
		uiPnl = pnl;
		this.model = model;
		
		// Detractive when dismissed.
		pnl.deactivateAllOnDismiss = true;
		
		// Create mask object.
		if (IsModel)
			CreateMask(Screen.width, Screen.height, clrMask);
		
		// Detractive panel at first.
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
		Obj.SetActive(false);
#else
		Obj.SetActiveRecursively( false );
#endif
	}

	public void Show(float offsetZ)
	{
		if (IsShown)
			return;

		// Save flag.
		shown = true;

		// Refresh panel tab.
		if (uiPnlTb != null)
			uiPnlTb.Value = true;

		// Show panel.
		if (IsTrans)
		{
			uiPnl.StartTransition(UIPanelManager.SHOW_MODE.BringInForward);

			// Add transition callback.
			uiPnl.AddTempTransitionDelegate(TrnsCmpCb);
		}
		else
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
			Obj.SetActive(true);
#else
			Obj.SetActiveRecursively( true );
#endif

		// Set offset z.
		this.offsetZ = offsetZ;
		SetOffsetZInternal(offsetZ);
	}

	public void Hide()
	{
		if (!IsShown)
			return;
		
		// Refresh panel tab.
		if (uiPnlTb != null)
			uiPnlTb.Value = false;
		
		// Hide panel.
		if (IsTrans)
		{
			uiPnl.StartTransition(UIPanelManager.SHOW_MODE.DismissForward);
			
			// Add transition callback.
			uiPnl.AddTempTransitionDelegate(TrnsCmpCb);
		}
		else
		{
			// Reset offset z.
			SetOffsetZInternal(0);
			offsetZ = 0;
			
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
			Obj.SetActive(false);
#else
			Obj.SetActiveRecursively( false );
#endif
		}
		
		// Callback.
		if (clsCb != null)
		{
			clsCb(clsCbUsrDt);
			clsCb = null;
			clsCbUsrDt = null;
		}
		
		// Reset flag.
		shown = false;
	}

	// Pause current transition.
	public void PauseTrans()
	{
		if (!IsTrans)
			return;

		EZTransition trans = uiPnl.GetTransition(IsShown ? UIPanelManager.SHOW_MODE.BringInForward : UIPanelManager.SHOW_MODE.DismissForward);
		trans.Pause();
	}

	// Unpause current transition.
	public void UnpauseTrans()
	{
		if (!IsTrans)
			return;

		EZTransition trans = uiPnl.GetTransition(IsShown ? UIPanelManager.SHOW_MODE.BringInForward : UIPanelManager.SHOW_MODE.DismissForward);
		trans.Unpause();
	}

	public void BindToTab(UIPanelTab pnlTb)
	{
		uiPnlTb = pnlTb;
	}

	// Set transition finished callback.
	public void SetTrnsFnsCb(TransFinishedCb cb, object usrDt)
	{
		if (!IsTrans)
			return;

		trnFnsCb = cb;
		trnFnsUsrDt = usrDt;
	}

	public void SetCloseCb(CloseCb cb, object usrDt)
	{
		clsCb = cb;
		clsCbUsrDt = usrDt;
	}

	public UIButton MaskBtn
	{
		get { return btMask; }
	}

	public void ResetOffsetZ(float offset)
	{
		if (Mathf.Approximately(offsetZ, offset))
			return;

		// Set offset z.
		offsetZ = offset;
		SetOffsetZInternal(offsetZ);
	}

	// Set panel offset z.
	private void SetOffsetZInternal(float offset)
	{
		Vector3 pos = Trans.localPosition;
		pos.z = offset;
		Trans.localPosition = pos;
	}

	// Transition callback.
	private void TrnsCmpCb(UIPanelBase pnl, EZTransition trns)
	{
		// If hide transition, set back offset.
		if (trns == uiPnl.GetTransition(UIPanelManager.SHOW_MODE.DismissForward))
		{
			// Reset offset z.
			SetOffsetZInternal(0);
			offsetZ = 0;
		}

		if (trnFnsCb != null)
		{
			trnFnsCb(trnFnsUsrDt);
			trnFnsCb = null;
			trnFnsUsrDt = null;
		}
	}

	// Create panel mask.
	private void CreateMask(float width, float height, Color color)
	{
		// Create mask object.
		GameObject mskObj;

		btMask = UIButton.Create("UIMask", Vector3.zero);
		btMask.Setup(width, height, new Material(Shader.Find("Kod/UI/Transparent Color")));

		btMask.SetUVs(new Rect(0, 0, 1, 1));
		btMask.SetColor(color);

		mskObj = btMask.gameObject;
		mskObj.gameObject.tag = "UI_MASK_PNL";
		ObjectUtility.SetObjectLayer(mskObj, Obj.layer);
		ObjectUtility.AttachToParentAndResetLocalPosAndRotation(Obj, mskObj);

		// Update max local z.
		mskObj.transform.localPosition = new Vector3(0, 0, 0.001f);

#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
		mskObj.SetActive(true);
#else
		mskObj.SetActiveRecursively( false );
#endif
	}
}
