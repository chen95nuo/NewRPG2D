//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <remarks>
/// Button class that allows you to toggle sequentially
/// through an arbitrary number of states.
/// </remarks>
[AddComponentMenu("EZ GUI/Controls/Box")]
public class UIBox : AutoSpriteControlBase
{
	// The zero-based index of the current state
	protected int curStateIndex;

	// Tracks whether the state was changed while
	// the control was deactivated
	protected bool stateChangeWhileDeactivated = false;

	/// <summary>
	/// Returns the zero-based number/index
	/// of the current state.
	/// </summary>
	public int StateNum
	{
		get { return curStateIndex; }
	}

	/// <summary>
	/// Returns the name of the current state.
	/// </summary>
	public string StateName
	{
		get { return states[curStateIndex].name; }
	}

	/// <summary>
	/// Zero-based index of the state that 
	/// should be the default, initial state.
	/// </summary>
	public int defaultState;

	/// Array of states that this button can have.
	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
		{
			new TextureAnim("Default"),
		};

	public override TextureAnim[] States
	{
		get { return states; }
		set { states = value; }
	}

	// Strings to display for each state.
	[HideInInspector]
	public string[] stateLabels = new string[] { DittoString };

	public override string[] StateLabels
	{
		get { return stateLabels; }
		set { stateLabels = value; }
	}

	public override string GetStateLabel(int index)
	{
		return stateLabels[index];
	}

	public override void SetStateLabel(int index, string label)
	{
		stateLabels[index] = label;

		if (index == curStateIndex)
			UseStateLabel(index);
	}

	// Transitions - one set for each state
	public override EZTransitionList GetTransitions(int index)
	{
		return null;
	}

	public override EZTransitionList[] Transitions
	{
		get { return null; }
		set { }
	}

	public override CSpriteFrame DefaultFrame
	{
		get
		{
			if (States[defaultState].spriteFrames.Length != 0)
				return States[defaultState].spriteFrames[0];
			else
				return null;
		}
	}

	public override TextureAnim DefaultState
	{
		get
		{
			return States[defaultState];
		}
	}

	//---------------------------------------------------
	// Misc
	//---------------------------------------------------
	protected override void Awake()
	{
		base.Awake();

		curStateIndex = defaultState;
	}

	public override void Start()
	{
		if (m_started)
			return;

		base.Start();

		// Runtime init stuff:
		if (Application.isPlaying)
		{
			SetToggleState(curStateIndex);
		}

		// Since hiding while managed depends on
		// setting our mesh extents to 0, and the
		// foregoing code causes us to not be set
		// to 0, re-hide ourselves:
		if (managed && m_hidden)
			Hide(true);
	}

	public override void Copy(SpriteRoot s)
	{
		Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);

		if (!(s is UIBox))
			return;

		UIBox b = (UIBox)s;

		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			defaultState = b.defaultState;
		}

		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			if (Application.isPlaying)
				SetToggleState(b.StateNum);
		}
	}

	/// <summary>
	/// Toggles the button's state to the next in the
	/// sequence and returns the resulting state number.
	/// </summary>
	public int ToggleState()
	{
		SetToggleState(curStateIndex + 1);

		return curStateIndex;
	}

	/// <summary>
	/// Sets the button's toggle state to the specified state.
	/// </summary>
	/// <param name="s">The zero-based state number/index.</param>
	/// <param name="suppressTransition">Whether or not to suppress transitions when changing states.</param>
	public virtual void SetToggleState(int s)
	{
		curStateIndex = s % states.Length;

		// First see if we need to postpone this state
		// change for when we are active:
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
		if (!gameObject.activeInHierarchy)
#else
		if (!gameObject.active)
#endif
		{
			stateChangeWhileDeactivated = true;

			// Call our changed delegate:
			if (changeDelegate != null)
				changeDelegate(this);

			return;
		}

		this.SetState(curStateIndex);

		this.UseStateLabel(curStateIndex);

		// Call our changed delegate:
		if (changeDelegate != null && !stateChangeWhileDeactivated)
			changeDelegate(this);
	}

	/// <summary>
	/// Sets the button's toggle state to the specified state.
	/// Does nothing if the specified state is not found.
	/// </summary>
	/// <param name="s">The name of the desired state.</param>
	/// <param name="suppressTransition">Whether or not to suppress transitions when changing states.</param>
	public virtual void SetToggleState(string stateName)
	{
		for (int i = 0; i < states.Length; ++i)
		{
			if (states[i].name == stateName)
			{
				SetToggleState(i);
				return;
			}
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();

		if (stateChangeWhileDeactivated)
		{
			SetToggleState(curStateIndex);
			stateChangeWhileDeactivated = false;
		}
	}

	public override int GetStateIndex(string stateName)
	{
		int index = base.GetStateIndex(stateName);
		return index < 0 ? 0 : index;
	}

	public void RemoveState(int stateIndex)
	{
		// Remove the selected state:
		List<TextureAnim> tempList = new List<TextureAnim>();
		tempList.AddRange(states);
		tempList.RemoveAt(stateIndex);
		states = tempList.ToArray();

		// Remove the associated label:
		List<string> tempLabels = new List<string>();
		tempLabels.AddRange(stateLabels);
		tempLabels.RemoveAt(stateIndex);
		stateLabels = tempLabels.ToArray();
	}

	// Sets the default UVs:
	public override void InitUVs()
	{
		if (states != null)
			if (defaultState <= states.Length - 1)
				if (states[defaultState].spriteFrames.Length != 0)
					frameInfo.Copy(states[defaultState].spriteFrames[0]);

		base.InitUVs();
	}

	// Draw our state creation/deletion controls in the GUI:
	public override int DrawPreStateSelectGUI(int selState, bool inspector)
	{
		GUILayout.BeginHorizontal(GUILayout.MaxWidth(50f));

		// Add a new state
		if (GUILayout.Button(inspector ? "+" : "Add State", inspector ? "ToolbarButton" : "Button"))
		{
			// Insert the new state before the "disabled" state:
			List<TextureAnim> tempList = new List<TextureAnim>();
			tempList.AddRange(states);
			tempList.Add(new TextureAnim("State " + (states.Length - 1)));
			states = tempList.ToArray();

			// Add a state label to match:
			List<string> tempLabels = new List<string>();
			tempLabels.AddRange(stateLabels);
			tempLabels.Add(DittoString);
			stateLabels = tempLabels.ToArray();
		}

		// Only allow removing a state if it isn't
		// our last one or our "disabled" state
		// which is always our last state:
		if (states.Length > 1 && selState != 0)
		{
			// Delete a state
			if (GUILayout.Button(inspector ? "-" : "Delete State", inspector ? "ToolbarButton" : "Button"))
			{
				// Remove the selected state:
				List<TextureAnim> tempList = new List<TextureAnim>();
				tempList.AddRange(states);
				tempList.RemoveAt(selState);
				states = tempList.ToArray();

				// Remove the associated label:
				List<string> tempLabels = new List<string>();
				tempLabels.AddRange(stateLabels);
				tempLabels.RemoveAt(selState);
				stateLabels = tempLabels.ToArray();
			}

			// Make sure the default state is
			// within a valid range:
			defaultState = defaultState % states.Length;
		}

		if (inspector)
		{
			GUILayout.FlexibleSpace();
		}


		GUILayout.EndHorizontal();

		return 14;
	}

	// Draw our state naming controls in the GUI:
	public override int DrawPostStateSelectGUI(int selState)
	{
		GUILayout.BeginHorizontal(GUILayout.MaxWidth(50f));

		GUILayout.Space(20f);
		GUILayout.Label("State Name:");

		states[selState].name = GUILayout.TextField(states[selState].name);

		GUILayout.EndHorizontal();

		return 28;
	}

	/// <summary>
	/// Creates a GameObject and attaches this
	/// component type to it.
	/// </summary>
	/// <param name="name">Name to give to the new GameObject.</param>
	/// <param name="pos">Position, in world space, where the new object should be created.</param>
	/// <returns>Returns a reference to the component.</returns>
	static public UIStateToggleBtn Create(string name, Vector3 pos)
	{
		GameObject go = new GameObject(name);
		go.transform.position = pos;
		return (UIStateToggleBtn)go.AddComponent(typeof(UIStateToggleBtn));
	}

	/// <summary>
	/// Creates a GameObject and attaches this
	/// component type to it.
	/// </summary>
	/// <param name="name">Name to give to the new GameObject.</param>
	/// <param name="pos">Position, in world space, where the new object should be created.</param>
	/// <param name="rotation">Rotation of the object.</param>
	/// <returns>Returns a reference to the component.</returns>
	static public UIStateToggleBtn Create(string name, Vector3 pos, Quaternion rotation)
	{
		GameObject go = new GameObject(name);
		go.transform.position = pos;
		go.transform.rotation = rotation;
		return (UIStateToggleBtn)go.AddComponent(typeof(UIStateToggleBtn));
	}
}
