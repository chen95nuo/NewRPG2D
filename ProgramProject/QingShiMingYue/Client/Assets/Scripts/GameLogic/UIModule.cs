using UnityEngine;
using System.Collections;

public class UIModule : MonoBehaviour
{
	public bool persistent; // Persistent flag to specify this instance can't be destroyed during changing state. 
	public bool model; // Panel show mode.
	public _UILayer uiLayer = _UILayer.Normal; // Panel show layer.
	public bool overlay; // This panel will overlay other behind UI
	public bool canNotOverlay;
	public bool ignoreMutex;

	private bool init; // Initialize flag.	

	public GameObject PnlObj
	{
		get { return pnl.Obj; }
	}

	private UIPanelEx pnl; // UIPanelEx.
	public UIPanelEx PnlEx
	{
		get { return pnl; }
	}

	private _UILayer showLayer;
	public _UILayer ShowLayer
	{
		get { return showLayer; }
	}

	public bool IsShown
	{
		get { return PnlEx.IsShown; }
	}

	private bool overlayed = false;
	public bool IsOverlayed
	{
		get { return overlayed; }
	}

	public virtual bool Initialize()
	{
		if (init)
			return false;

		SysUIEnv uiEvn = SysModuleManager.Instance.GetSysModule<SysUIEnv>();

		Transform objTrans = gameObject.transform;
		ObjectUtility.AttachToParentAndResetLocalTrans(uiEvn.UIRoot, objTrans);
		objTrans.localScale = new Vector3(uiEvn.UIModuleScaleRate, uiEvn.UIModuleScaleRate, uiEvn.UIModuleScaleRate);

		UIPanelBase pnlBase = gameObject.GetComponent<UIPanelBase>();
		if (pnlBase == null)
			return false;

		// Create panel ex.
		pnl = new UIPanelEx(pnlBase, model);

		// Add this panel.
		uiEvn.UIPnlMgr.AddPanel(pnl);

		// Don't destroy this panel when changing scene.
		GameObject.DontDestroyOnLoad(pnl.Obj);

		// Get UI sounds.
		//		if (GameDefines.uiSndOnClick != "" || GameDefines.uiSndOnOver != "")
		//		{
		//			AudioSource sndBtnClick = AudioManager.Instance.GetPersistentSound(GameDefines.uiSndOnClick);
		//			AudioSource sndBtnOver = AudioManager.Instance.GetPersistentSound(GameDefines.uiSndOnOver);
		//			
		//			// Set UI sounds.
		//			UIButton[] btn = PnlObj.GetComponentsInChildren<UIButton>(true);
		//			foreach (UIButton b in btn)
		//			{
		//				if (b != PnlEx.MaskBtn)
		//				{
		//					b.soundOnOver = sndBtnOver;
		//					b.soundOnClick = sndBtnClick;
		//				}
		//			}
		//			
		//			UIRadioBtn[] rBtn = PnlObj.GetComponentsInChildren<UIRadioBtn>(true);
		//			foreach (UIRadioBtn b in rBtn)
		//			{
		//				b.soundToPlay = sndBtnClick;
		//			}
		//
		//			//UIButton3D[] dBtn = GameObject.FindObjectsOfType(typeof(UIButton3D)) as UIButton3D[];
		//			//foreach (UIButton3D b in dBtn)
		//			//{
		//			//    b.soundOnClick = sndBtnClick;
		//			//}
		//		}

		init = true;

		return true;
	}

	public virtual void Dispose()
	{
		// Destroy this panel.
		if (pnl != null)
		{
			// Delete this panel.
			SysModuleManager.Instance.GetSysModule<SysUIEnv>().UIPnlMgr.DelPanel(pnl);
		}

		init = false;
	}

	protected bool ShowSelf(params object[] userDatas)
	{
		if (IsShown)
			return false;

		SysUIEnv.Instance.ShowUIModule(this.GetType(), userDatas);

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void HideSelf()
	{
		SysUIEnv.Instance.HideUIModule(this.GetType());
	}

	public virtual bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (pnl.IsShown)
			return false;

		ShowInternal(layer);

		return true;
	}

	public virtual void OnHide()
	{
		if (!pnl.IsShown)
			return;

		HideInternal();
	}

	public void AfterFreeText()
	{
		foreach (SpriteText st in this.PnlObj.GetComponentsInChildren<SpriteText>(true))
		{
			st.UpdateFontMesh(true);
			if (this.IsShown && st.IsHidden() == false)
				st.Text = st.text;
		}
	}

	private void ShowInternal(_UILayer layer)
	{
		this.showLayer = layer != _UILayer.Invalid ? layer : uiLayer;

		SysUIEnv.Instance.UIPnlMgr.Show(pnl, layer != _UILayer.Invalid ? layer : uiLayer);

		// Overlay flag
		overlayed = false;

		// Process after show.
		if (pnl.IsTrans)
			pnl.SetTrnsFnsCb(PnlTrnsFnsCb, true);
		else
			AfterShow();

		//// Test if panel is very clear to UI container, here we don't sort the panels, just simply move far away from camera.
		//Camera uiCam = uiEvn.UICam;
		//Transform rtTrans = uiEvn.UIRoot.transform;

		//if (rtTrans.localPosition.z + pnl.OffsetZ < uiCam.nearClipPlane + 5)
		//    rtTrans.localPosition += new Vector3(0, 0, uiCam.nearClipPlane + 5 - pnl.OffsetZ);
	}

	private void HideInternal()
	{
		// Hide UI
		SysUIEnv.Instance.UIPnlMgr.Hide(pnl, uiLayer);

		// Notice UI hidden
		SysUIEnv.Instance.OnUIModuleHidden(this);

		// Process after hide.
		if (pnl.IsTrans)
			pnl.SetTrnsFnsCb(PnlTrnsFnsCb, false);
		else
			AfterHide();
	}

	public virtual void AfterShow()
	{
		SysUIEnv.Instance.OnUIModuleShown(this);
	}

	public virtual void AfterHide()
	{
	}

	public virtual void Overlay()
	{
		if (IsShown == false)
			return;

		if (overlayed == false)
		{
			PnlObj.SetActive(false);
			overlayed = true;
		}
	}

	public virtual void RemoveOverlay()
	{
		if (IsShown == false)
			return;

		if (overlayed)
		{
			PnlObj.SetActive(true);
			overlayed = false;
		}
	}

	private void PnlTrnsFnsCb(object userData)
	{
		bool isShow = (bool)userData;

		if (isShow)
			AfterShow();
		else
			AfterHide();
	}
}
