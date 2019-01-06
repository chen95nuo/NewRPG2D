using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <remarks>
/// A phase progress bar class. 
/// 
/// The progress bar will increase from 0 to phase[last].
/// The current value will dynamically increase to target value.
/// If the phase is not setted, the progress bar will not work.
/// 
/// E.g. phase = { 100, 200, 300 }, value = 50, tovalue = 150
/// 
/// The value will increasing like follow:
/// 
/// 0   |        50------->| 100
/// 101 |<-----------------| 200
/// 201 |-------->150      | 300
/// 
/// </remarks>
[AddComponentMenu("EZ GUI/Controls/Phase Progress Bar")]
public class UIPhaseProgressBar : UIProgressBar 
{
	// Update callback.
	public delegate void UpdateCb( UIPhaseProgressBar bar, object usd );
	
	// Phase changed callback.
	public delegate void PhaseChangedCb( UIPhaseProgressBar bar, object usd );
	
	public delegate void UpdateFinishCb();

	/// <summary>
	/// Get the scales of progress.
	/// </summary>
	public List<int> GetScales()
	{
		return new List<int>( scales );
	}
	
	/// <summary>
	/// Get the phases of progress.
	/// </summary>
	public List<int> GetPhases()
	{
		List<int> phases = new List<int>();
		
		if ( !ValidScales )
			return phases;
		
		for ( int i = 1; i < scales.Count; i ++ )
			phases.Add( scales[i] - scales[ i - 1 ] );
		
		return phases;
	}
	
	/// <summary>
	/// Test whether scales is valid.
	/// </summary>
	public bool ValidScales
	{
		get { return scales.Count > 1; }
	}
	
	/// <summary>
	/// Add one scale, if this scale exists, it will be discarded.
	/// </summary>
	/// <param name="scale">
	/// The scale value to be added.
	/// </param>
	public void AddScale( int scale )
	{
		if ( scales.Contains( scale ) )
			return;
		
		UpdateScales();
	}
	
	/// <summary>
	/// Delete one scale.
	/// </summary>
	/// <param name="scale">
	/// The scale value to be deleted.
	/// </param>
	public void DelScale( int scale )
	{
		if ( !scales.Contains( scale ) )
			return;
		
		scales.Remove( scale );
		
		UpdateScales();
	}
	
	/// <summary>
	/// Clear all scales.
	/// </summary>
	public void ClearScale()
	{
		scales.Clear();
	}
	
	/// <summary>
	/// Add one phase length. This will enlarge the maximum scale.
	/// </summary>
	/// <param name="length">
	/// The phase length. It must be bigger than zero.
	/// </param>
	public void AddPhase( int length )
	{
		// Length must be valid.
		if ( length <= 0 )
			return;
		
		// If still has no scales, set default scale zero.
		if ( scales.Count == 0 )
			scales.Add( 0 );
		
		// Enlage the maximum scale.
		scales.Add( scales[ scales.Count - 1 ] + length );
		
		UpdateScales();
	}
	
	/// <summary>
	/// The bar increasing speed during one phase( 0-1 ).
	/// </summary>
	public double BarSpeed
	{
		get { return barSpd; }
		set { barSpd = Mathf.Clamp01( (float)value ); }
	}
	
	/// <summary>
	/// Set bar update callback.
	/// </summary>
	/// <param name="cb">
	/// The callback function.
	/// </param>
	/// <param name="cbDt">
	/// The callback data.
	/// </param>
	public void SetUpdateCb( UpdateCb cb, object cbDt )
	{
		updateCb = cb;
		updateCbDt = cbDt;
	}

	public void SetUpdateFinishedCb(UpdateFinishCb cb )
	{
		updataFinishCb = cb;
	}
	
	/// <summary>
	/// Set phase changed callback.
	/// </summary>
	/// <param name="cb">
	/// The callback function.
	/// </param>
	/// <param name="cbDt">
	/// The callback data.
	/// </param>
	public void SetPhaseChangedCb( PhaseChangedCb cb, object cbDt )
	{
		phsChgCb = cb;
		phsChgDbDt = cbDt;
	}
	
	/// <summary>
	/// Set or get the bar current value.
	/// </summary>
	public new int Value
	{
		get { return curVal; }
		set
		{
			// Set value needs valid scales.
			if ( !ValidScales )
				return;
			
			// Clamp value.
			curVal = Mathf.Clamp(value, scales[0], scales[ scales.Count - 1 ] );
			
			// Calculate phase data.
			CalCurPhase( false );
			
			// Calculate speed.
			CalSpeed();
			
			// Update bar.
			UpdateBar();
			
			// Stop bar increasing or decreasing.
			running = false;
		}
	}
	
	/// <summary>
	/// Set or get the bar destination value.
	/// </summary>
	public int ToValue
	{
		get { return toVal; }
		set
		{
			// Set to value needs valid scales.
			if ( !ValidScales )
				return;
			
			// Clamp value.
			toVal = Mathf.Clamp(value, scales[0], scales[ scales.Count - 1 ]);
		
			// Calculate speed.
			CalSpeed();
			
			// Stop bar increasing or decreasing.
			running = false;
		}
	}
	
	/// <summary>
	/// Convert phase value to total value.
	/// </summary>
	/// <param name="phase">
	/// The phase of the phase value.
	/// </param>
	/// <param name="phaseValue">
	/// The phase value.
	/// </param>
	/// <param name="val">
	/// Receive the total value.
	/// </param>
	/// <returns>
	/// The successful flag.
	/// </returns>
	public bool PhsVal2Val( int phase, int phaseValue, out int val )
	{
		// Default valule.
		val = 0;
		
		// Set value needs valid scales.
		if ( !ValidScales )
			return false;
		
		// Check whether this is valid phase index.
		if ( phase < 0 || phase > scales.Count - 2 )
			return false;
		
		// Check phase vlaue whether is valid in this phase.
		if ( phaseValue < 0 || phaseValue > scales[ phase + 1 ] - scales[ phase ] )
			return false;
		
		// Calculate new value.
		val = scales[ phase ] + phaseValue;
		
		return true;
	}
	
	//public bool Val2PhsVal( float val, out int phase, out float phaseValue )
	//{
		// Todo:
	//}
	
	/// <summary>
	/// Get current phase value.
	/// </summary>
	public int CurPhaseValue
	{
		get { return Value - curPhsMin; }
	}
	
	/// <summary>
	/// Current phase index.
	/// </summary>
	public int CurPhase
	{
		get { return curPhs; }
	}
	
	/// <summary>
	/// Get specified phase length.
	/// </summary>
	/// <param name="phase">
	/// The phase index.
	/// </param>
	/// <returns>
	/// The phase length. If invalid phase index, will return zero.
	/// </returns>
	public int GetPhaseLength( int phase )
	{
		if ( !ValidScales )
			return 0;
			
		if ( phase < 0 || phase > scales.Count - 2 )
			return 0;
			
			return scales[ phase + 1 ] - scales[ phase ];
	}
	
	/// <summary>
	/// Get current phase length.
	/// Or return zero if current phase is invalid.
	/// </summary>
	public int CurPhaseLength
	{
		get{ return curPhsMax - curPhsMin; }
	}
	
	/// <summary>
	/// Enable or disable increasing or decreasing.
	/// </summary>
	public bool Running
	{
		get { return running; }
		set 
		{ 
			running = value; 
			
			if (running)
				valIncrease = 0;
		}
	}
	
	protected void Update () 
	{
		if ( !running)
			return;

		if (valSpd == 0)
		{
			curVal = toVal;
			running = false;
			updataFinishCb();
			CalCurPhase(true);
			return;
		}

		// Use a float value to calculate increase to avoid loss of significance
		valIncrease += (valSpd * Time.deltaTime);
		if (valIncrease > 0)
		{
			float floor = Mathf.Floor(valIncrease);
			curVal += (int)floor;
			valIncrease -= floor;
		}
		else if (valIncrease < 0)
		{
			float ceil = Mathf.Ceil(valIncrease);
			curVal += (int)ceil;
			valIncrease -= ceil;
		}

		// Increase
		if ( barSpdDir > 0 )
		{
			// Check to value.
			if ( curVal >= toVal )
			{
				curVal = toVal;
				running = false;
				updataFinishCb();
			}
			
			// Check phase.
			if (curVal >= curPhsMax)
			{
				CalCurPhase(true);
			}
		}
		// Decrease
		else
		{
			// Check to value.
			if ( curVal <= toVal )
			{
				curVal = toVal;
				running = false;
				updataFinishCb();
			}

			if (curVal < curPhsMin)
			{
				CalCurPhase(true);
			}
		}
		
		UpdateBar();
		
		if ( updateCb != null )
			updateCb( this, updateCbDt );
	}
	
	protected void UpdateScales()
	{
		if ( scales.Count == 0 )
			return;
		
		scales.Sort();
		
		// Remove same scale.
		for ( int i = scales.Count - 1; i > 0; i -- )
		{
			if ( scales[i] == scales[ i - 1 ] )
				scales.RemoveAt( i );
		}
	}
	
	protected void CalCurPhase( bool phsCb )
	{
		lastPhs = curPhs;
		
		// Find current phase.
		for ( curPhs = 0; curPhs < scales.Count; curPhs ++ )
		{
			
			if ( curVal < scales[ curPhs ] )
				break;
		}
		
		// Last scale.
		if ( curPhs == scales.Count )
			curPhs = scales.Count - 1;
		
		// Phase index is begin from 0.
		curPhs -= 1;
		
		// If current value is 0, current phase is the first.
		if ( curPhs < 0 )
			curPhs = 0;

		curPhsMin = scales[ curPhs ];
		curPhsMax = scales[ curPhs + 1 ];

		// Phase changed callback.
		if (phsCb && lastPhs != curPhs && phsChgCb != null)
		{
			phsChgCb(this, phsChgDbDt);
		}
	}
	
	protected void CalSpeed()
	{	
		if ( toVal == curVal )
		{
			valSpd = 0;
			return;
		}
		
		// Update increase flag.
		barSpdDir = toVal > curVal ? 1 : -1;
		
		valSpd = (float)(barSpd * barSpdDir * ( curPhsMax - curPhsMin ));
	}
	
	protected void UpdateBar()
	{
		base.Value = (float)( curVal - curPhsMin ) / ( curPhsMax - curPhsMin );
	}
	
	protected bool running;
	protected float barSpd = 0.2f;
	protected float barSpdDir = 1f;
	
	protected int curVal = 0;
	protected int toVal;
	protected float valSpd;
	protected float valIncrease;
	protected List<int> scales = new List<int>(); // Scales.
	protected int curPhs;
	protected int lastPhs;
	protected int curPhsMin;
	protected int curPhsMax;
	
	protected UpdateCb updateCb;
	protected object updateCbDt;

	protected UpdateFinishCb updataFinishCb;
	
	protected PhaseChangedCb phsChgCb;
	protected object phsChgDbDt;
}
